using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;
using System.Threading;
using Ionic.Zip;
using vwarDAL;
using Utils;




/// <summary>
/// Web Page that allows for the uploading of 3D model content and associated metadata
/// </summary>
public partial class Users_Upload : Website.Pages.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Page.Master.FindControl("SearchPanel") != null)
        {
            //hide the search panel
            this.Page.Master.FindControl("SearchPanel").Visible = false;
            
        }

        if (!Page.IsPostBack)
        {
            HttpContext.Current.Session["fileStatus"] = null; //Reset the FileStatus in case page was refreshed
        }
    }



    /// <summary>
    /// AJAX-enabled web method to detect the format of the file
    /// </summary>
    /// <param name="filename">The uploaded filename </param>
    /// <returns>A JSON-encoded FileStatus object containing the extension and the type (Recognized, Unrecognized, or Viewable)</returns>
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static FileStatus DetectFormat(string filename)
    {

        FileStatus currentStatus = new FileStatus("", FormatType.UNRECOGNIZED);


        //The temp filename (hashname) is a sha1 hash plus a random number   
        currentStatus.hashname = filename; 
        currentStatus.msg = FileStatus.UnrecognizedMessage;

        //Check to see if it's an skp file
        if (string.Compare(currentStatus.hashname.Substring(currentStatus.hashname.LastIndexOf('.')), ".skp", true) == 0)
        {
            currentStatus.type = FormatType.VIEWABLE;
            currentStatus.extension = ".skp";
        }
        else
        {
            int recognizedCount = 0;
            int viewableCount = 0;
            using (ZipFile zip = ZipFile.Read(HttpContext.Current.Server.MapPath("~/App_Data/") + currentStatus.hashname))
            {
                foreach (string s in zip.EntryFileNames)
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(s);
                    if (FileStatus.GetType(f.Extension) == FormatType.VIEWABLE)
                    {
                        currentStatus.extension = f.Extension;
                        currentStatus.type = FormatType.VIEWABLE;
                        recognizedCount++;
                    }
                    else if (FileStatus.GetType(f.Extension) == FormatType.RECOGNIZED)
                    {
                        if (currentStatus.type != FormatType.VIEWABLE)
                        {
                            currentStatus.extension = f.Extension;
                            currentStatus.type = FormatType.RECOGNIZED;
                        }
                        currentStatus.msg = FileStatus.WarningMessage;
                        viewableCount++;
                    }
                }
            }

            //Make sure there is only one recognized or viewable model format in the zip file
            //If multiple have been detected, set the format type and break
            if (viewableCount > 1)
            {
                currentStatus.type = FormatType.MULTIPLE_RECOGNIZED;
                currentStatus.msg = FileStatus.MultipleRecognizedMessage;

            }
        }
        if (currentStatus.type == FormatType.UNRECOGNIZED ||
            currentStatus.type == FormatType.MULTIPLE_RECOGNIZED)
        {
            deleteTempFile(currentStatus.hashname);
        }
        else
        {
            HttpContext.Current.Session["fileStatus"] = currentStatus;
        }

        ContentObject tempFedoraObject = new ContentObject();
        tempFedoraObject.UploadedDate = DateTime.Now;
        tempFedoraObject.LastModified = DateTime.Now;
        tempFedoraObject.Views = 0;
        tempFedoraObject.SubmitterEmail = HttpContext.Current.User.Identity.Name.Trim();
        HttpContext.Current.Session["contentObject"] = tempFedoraObject;

        return currentStatus;
    }



    /// <summary>
    /// Sends the uploaded file through the conversion process and stores the temporary files in the App_Data folder. 
    /// Also updates the temporary content object and FileStatus for the session.
    /// </summary>
    /// <returns>A JSON-encoded FileStatus object containing the extension, the type (Recognized, Unrecognized, or Viewable), and conversion status</returns>
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static FileStatus Convert()
    {

        Utility_3D.Model_Packager pack = new Utility_3D.Model_Packager();
        Utility_3D _3d = new Utility_3D();
        _3d.Initialize(Website.Config.ConversionLibarayLocation);

        Utility_3D.ConvertedModel model = null;
        
        FileStatus status = (FileStatus)HttpContext.Current.Session["fileStatus"];
        using (FileStream stream = new FileStream(HttpContext.Current.Server.MapPath("~/App_data/" + status.hashname), FileMode.Open))
        {

            ContentObject tempFedoraObject = (ContentObject)HttpContext.Current.Session["contentObject"];
            try //convert the model
            {

                model = pack.Convert(stream, status.hashname);


                status.converted = "true";
                HttpContext.Current.Session["fileStatus"] = status;

                Utility_3D.Parser.ModelData mdata = model._ModelData;
                tempFedoraObject.NumPolygons = mdata.VertexCount.Polys;
                tempFedoraObject.NumTextures = mdata.ReferencedTextures.Length;
                tempFedoraObject.UpAxis = mdata.TransformProperties.UpAxis;
                tempFedoraObject.UnitScale = System.Convert.ToString(mdata.TransformProperties.UnitMeters);

                HttpContext.Current.Session["contentObject"] = tempFedoraObject;


                //Save the O3D file for the viewer into a temporary directory
                var tempfile = HttpContext.Current.Server.MapPath("~/App_Data/viewerTemp/" + status.hashname);
                using (System.IO.FileStream savefile = new FileStream(tempfile, FileMode.Create))
                {
                    byte[] filedata = new Byte[model.data.Length];
                    model.data.CopyTo(filedata, 0);
                    savefile.Write(model.data, 0, (int)model.data.Length);
                }
                ConvertFileToO3D(HttpContext.Current, tempfile);
                var rootDir = HttpContext.Current.Server.MapPath("~/App_Data/converterTemp/");
                var fileName = Path.Combine(rootDir,status.hashname);
                if (!Directory.Exists(rootDir))
                {
                    Directory.CreateDirectory(rootDir);
                }
                if (File.Exists( fileName))
                {
                    File.Delete(fileName);
                }
                File.Move(tempfile, fileName);
            }
            catch (Exception e) //Error while converting
            {
                stream.Close();
                //FileStatus.converted is set to false by default, no need to set
                status.msg = FileStatus.ConversionFailedMessage; //Add the conversion failed message
                deleteTempFile(status.hashname);
                HttpContext.Current.Session["fileStatus"] = null; //Reset the FileStatus for another upload attempt
                
            }
            

        }
        return status;

    }


    /// <summary>
    /// Clears the Session variables and stored temp files from the server
    /// </summary>
    /// <param name="filename">The name of the temporary file (possibly "undefined") to clean up, if necessary</param>
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static void UploadReset(string filename)
    {    
        string basePath = HttpContext.Current.Server.MapPath("~/App_Data/");
        //Delete the temp file if it exists
        if (File.Exists(HttpContext.Current.Server.MapPath("~/App_Data/" + filename)))
        {
            deleteTempFile(filename);          

            string converterTempPath = basePath + "converterTemp/" + filename;
            if (File.Exists(converterTempPath))
            {
                try
                {
                    File.Delete(converterTempPath);
                } catch {}
            }

            string viewerTempPath = basePath + "viewerTemp/" + filename.Replace("zip", "o3d").Replace("skp", "o3d").ToLower() ;
            if (File.Exists(viewerTempPath))
            {
                try
                {
                    File.Delete(viewerTempPath);
                }
                catch { }
            }
        }

        

        //Delete the FileStatus from session if it exists
        if(HttpContext.Current.Session["fileStatus"] != null)
        {
            HttpContext.Current.Session["fileStatus"] = null;
        }

         //Delete the model from session if it exists
        if(HttpContext.Current.Session["contentObject"] != null)
        {
            HttpContext.Current.Session["contentObject"] = null;
        }

        if (filename != "" && filename != "undefined")
        {
            string imagePath = HttpContext.Current.Server.MapPath("~/App_Data/imageTemp/");
            string basehash = filename.Substring(0, filename.LastIndexOf(".") - 1);
            foreach (string imgFileName in Directory.GetFiles(imagePath, "*" + basehash + "*"))
            {
                File.Delete(imgFileName);
            }
        }
        
    }


    /// <summary>
    /// Updates the content object with the metadata provided by the user in Step 1.
    /// </summary>
    /// <param name="TitleInput">The text from the "Title" text field (NewUpload.ascx)</param>
    /// <param name="DescriptionInput">The text from the "Description" textarea (NewUpload.ascx)</param>
    /// <param name="TagsInput">TagsInput - The comma or space-delimited list of tags from the tags text field (NewUpload.ascx)</param>
    /// <returns>A JSON object containing the parameters for the ViewerLoader javascript object constructor</returns>
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static JsonWrappers.ViewerLoadParams Step1_Submit(string TitleInput, string DescriptionInput, string TagsInput)
    {
        FileStatus currentStatus = (FileStatus)HttpContext.Current.Session["fileStatus"];
        var fileName = TitleInput.Trim().Replace(' ', '_') + ".zip";
        if (currentStatus != null)
        {
            currentStatus.filename = fileName;
        }
        ContentObject tempFedoraCO = (ContentObject)HttpContext.Current.Session["contentObject"];
        tempFedoraCO.Title = TitleInput.Trim();
        tempFedoraCO.Description = DescriptionInput.Trim();
        tempFedoraCO.Location = fileName;


        //Add the keywords
        if (TagsInput.LastIndexOf(',') == -1) //They used whitespace as delimiter
        {
            tempFedoraCO.Keywords = String.Join(",", TagsInput.Split(' '));
        }
        else
        {
            tempFedoraCO.Keywords = TagsInput;
        }

        JsonWrappers.ViewerLoadParams jsReturnParams = new JsonWrappers.ViewerLoadParams();
        jsReturnParams.FlashLocation = tempFedoraCO.Location;

        if (currentStatus.type == FormatType.VIEWABLE)
        {
            tempFedoraCO.DisplayFile = currentStatus.filename.Replace("zip", "o3d");
            jsReturnParams.IsViewable = true;
            jsReturnParams.BasePath = "../Public/";
            jsReturnParams.BaseContentUrl = "Model.ashx?temp=true&file=";
            jsReturnParams.O3DLocation = currentStatus.hashname.Replace("zip", "o3d");
            jsReturnParams.FlashLocation = currentStatus.hashname;
            jsReturnParams.ShowScreenshot = true;
            jsReturnParams.UpAxis = tempFedoraCO.UpAxis;
            jsReturnParams.UnitScale = tempFedoraCO.UnitScale;
        }
        else if (currentStatus.type == FormatType.RECOGNIZED)
        {
            tempFedoraCO.DisplayFile = "N/A";
            

        }

        HttpContext.Current.Session["contentObject"] = tempFedoraCO;
        return jsReturnParams;

        
    }


    /// <summary>
    /// Updates the temporary content object with the Up Axis and Unit Scale set in Step 2.
    /// </summary>
    /// <param name="ScaleValue">A float value representing the scale, expressed in meters (NewUpload.ascx)</param>
    /// <param name="UpAxis">A string, either "Y" or "Z", that specifies the selected Up Axis value (NewUpload.ascx)</param>
    /// <returns>A pipe-delimited string array that has the info to be filled in</returns>
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static JsonWrappers.UploadDetailDefaults Step2_Submit(string ScaleValue, string UpAxis)
    {
        HttpContext context = HttpContext.Current;
        FileStatus currentStatus = (FileStatus) context.Session["fileStatus"];
        ContentObject tempCO = (ContentObject) HttpContext.Current.Session["contentObject"];
        tempCO.UpAxis = UpAxis;
        tempCO.UnitScale = ScaleValue;
        context.Session["contentObject"] = tempCO;


        //Bind the 
        JsonWrappers.UploadDetailDefaults jsReturnParams = new JsonWrappers.UploadDetailDefaults();
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            UserProfile p = null;
            try
            {
                p = UserProfileDB.GetUserProfileByUserName(context.User.Identity.Name);
            }
            catch { }

            if (p != null)
            {
                jsReturnParams.HasDefaults = true;
                jsReturnParams.DeveloperName = p.DeveloperName;
                jsReturnParams.ArtistName = p.ArtistName;
                jsReturnParams.DeveloperUrl = p.WebsiteURL;
                jsReturnParams.SponsorName = p.SponsorName;

                string tempImagePath = context.Server.MapPath("~/App_Data/imageTemp/");
                if (p.DeveloperLogo != null)
                {

                    string extension = p.DeveloperLogoContentType.Substring(p.DeveloperLogoContentType.LastIndexOf("/") + 1);
                    string tempDevLogoFilename = "devlogo_" + currentStatus.hashname.Replace("zip", extension);
                    using (FileStream stream = new FileStream(tempImagePath + tempDevLogoFilename, FileMode.Create))
                    {
                        stream.Write(p.DeveloperLogo, 0, p.DeveloperLogo.Length);
                    }

                    jsReturnParams.DeveloperLogoFilename = tempDevLogoFilename;
                }

                if (p.SponsorLogo != null)
                {
                    string extension = p.SponsorLogoContentType.Substring(p.SponsorLogoContentType.LastIndexOf("/") + 1);
                    string tempSponsorLogoFilename = "sponsorlogo_" + currentStatus.hashname.Replace("zip", extension);
                    using (FileStream stream = new FileStream(tempImagePath + tempSponsorLogoFilename, FileMode.Create))
                    {
                        stream.Write(p.SponsorLogo, 0, p.SponsorLogo.Length);
                    }

                    jsReturnParams.SponsorLogoFilename = tempSponsorLogoFilename;
                }
            }
        }

        return jsReturnParams;
    }


    /// <summary>
    /// Binds the details from step 3 to the content object, sends it to Fedora, then adds the model and image datastreams.
    /// </summary>
    /// <param name="DeveloperName">The text from the "Developer Name" text field (NewUpload.ascx)</param>
    /// <param name="ArtistName">The text from the "Artist Name" text field (NewUpload.ascx)</param>
    /// <param name="DeveloperUrl">The url from the "Developer Url" text field (NewUpload.ascx)</param>
    /// <param name="SponsorName">The text from the "Sponsor Name" text field (NewUpload.ascx)</param>
    /// <param name="SponsorUrl">The url from the sponsor url text field (NewUpload.ascx)</param>
    /// <param name="LicenseType"> The shorthand notation for the Creative Commons License type</param>
    /// <returns>A string containing the ContentObjectID for the newly inserted Content Object</returns>
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string SubmitUpload(string DeveloperName, string ArtistName, string DeveloperUrl, string SponsorName, string SponsorUrl, string LicenseType)
    {

        try
        {
            ContentObject tempCO = (ContentObject)HttpContext.Current.Session["contentObject"];
            tempCO.DeveloperName = DeveloperName;
            tempCO.ArtistName = ArtistName;
            tempCO.MoreInformationURL = DeveloperUrl;
            //tempCO.SponsorURL = SponsorUrl; !missing SponsorUrl metadata in ContentObject
            if (LicenseType == "publicdomain")
            {
                tempCO.CreativeCommonsLicenseURL = "http://creativecommons.org/publicdomain/mark/1.0/";
            }
            else
            {
                tempCO.CreativeCommonsLicenseURL = String.Format(System.Configuration.ConfigurationManager.AppSettings["CCBaseUrl"], LicenseType);
            }
            tempCO.SponsorName = SponsorName;



            var factory = new DataAccessFactory();
            IDataRepository dal = factory.CreateDataRepositorProxy();
            dal.InsertContentObject(tempCO);


            FileStatus status = (FileStatus)HttpContext.Current.Session["fileStatus"];

            //Upload the thumbnail and logos
            string filename = status.hashname;
            string basehash = filename.Substring(0, filename.LastIndexOf(".") - 1);
            foreach (FileInfo f in new DirectoryInfo(HttpContext.Current.Server.MapPath("~/App_Data/imageTemp")).GetFiles("*" + basehash + "*"))
            {
                using (FileStream fstream = f.OpenRead())
                {
                    string type = f.Name.Substring(0, f.Name.IndexOf('_'));
                    switch (type)
                    {
                        case ImagePrefix.DEVELOPER_LOGO:
                            tempCO.DeveloperLogoImageFileName = "developer_logo" + f.Extension;
                            tempCO.DeveloperLogoImageFileNameId = dal.UploadFile(fstream, tempCO.PID, tempCO.DeveloperLogoImageFileName);
                            break;

                        case ImagePrefix.SPONSOR_LOGO:
                            tempCO.SponsorLogoImageFileName = "sponsor_logo" + f.Extension;
                            tempCO.SponsorLogoImageFileNameId = dal.UploadFile(fstream, tempCO.PID, tempCO.SponsorLogoImageFileName);
                            break;

                        case ImagePrefix.SCREENSHOT:
                            tempCO.ScreenShot = "screenshot" + f.Extension;
                            tempCO.ScreenShotId = dal.UploadFile(fstream, tempCO.PID, tempCO.ScreenShot);
                            break;

                        default:
                            break;
                    }
                }
            }



            //Upload the converted file and the O3D file
            if (status.type == FormatType.VIEWABLE)
            {
                //Upload the original model data
                using (FileStream stream = File.OpenRead(HttpContext.Current.Server.MapPath("~/App_Data/" + filename)))
                {
                    dal.UploadFile(stream, tempCO.PID, "original_" + status.filename);
                }
                using (FileStream stream = File.OpenRead(HttpContext.Current.Server.MapPath("~/App_Data/converterTemp/" + filename)))
                {
                    dal.UploadFile(stream, tempCO.PID, status.filename);
                }

                using (FileStream stream = File.OpenRead(HttpContext.Current.Server.MapPath("~/App_data/viewerTemp/" + filename.Replace("zip", "o3d").Replace("skp", "o3d"))))
                {
                    tempCO.DisplayFileId = dal.UploadFile(stream, tempCO.PID, tempCO.DisplayFile);
                }
            }
            else
            {
                //Upload the original model data
                using (FileStream stream = File.OpenRead(HttpContext.Current.Server.MapPath("~/App_Data/" + filename)))
                {
                    dal.UploadFile(stream, tempCO.PID, status.filename);
                }
            }


            dal.UpdateContentObject(tempCO);
            UploadReset(filename);
            return tempCO.PID;
        }
        catch(Exception e) {
            //add fail logic here
            return e.Message + "\n" + e.StackTrace;
        }
    }


    /// <summary>
    /// Deletes an image file from the imageTemp directory, resulting from a re-upload of an image file.
    /// </summary>
    /// <param name="filename">the name of the file (no path) in imageTemp that needs to be deleted</param>
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static void DeleteImage(string filename)
    {
        string path = HttpContext.Current.Server.MapPath("~/App_Data/imageTemp/" + filename);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }


    /// <summary>
    /// Deletes a file from the base temporary directory.
    /// </summary>
    /// <param name="filename">The name of the file to be deleted (no path)</param>
    public static void deleteTempFile(string filename)
    {
        File.Delete(HttpContext.Current.Server.MapPath("~/App_Data/" + filename));
    }

    /// <summary>
    /// Converts a file from its native format to the O3D format.
    /// </summary>
    /// <param name="context">The current web context.</param>
    /// <param name="path"></param>
    /// <returns></returns>
    private static string ConvertFileToO3D(HttpContext context, string path)
    {
        HttpRequest request = context.Request;

        var application = context.Server.MapPath("~/processes/o3dConverter.exe");//Path.Combine(Path.Combine(request.PhysicalApplicationPath, "bin"), "o3dConverter.exe");
        System.Diagnostics.ProcessStartInfo processInfo = new System.Diagnostics.ProcessStartInfo(application);
        processInfo.Arguments = String.Format("\"{0}\" \"{1}\"", path, path.ToLower().Replace("zip", "o3d").Replace("skp", "o3d"));
        processInfo.WindowStyle = ProcessWindowStyle.Hidden;
        processInfo.RedirectStandardError = true;
        processInfo.CreateNoWindow = true;
        processInfo.UseShellExecute = false;
        var p = Process.Start(processInfo);
        var error = p.StandardError.ReadToEnd();
        return path.ToLower().Replace("zip", "o3d");
    }
    

}
