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





public partial class Users_Upload : Website.Pages.PageBase
{

    
    public ContentObject tempFedoraObject
    {
        get
        {

            return (ContentObject)Session["contentObject"];
        }
        set
        {
            Session["contentObject"] = value;
        }
    }


    private FileStatus currentFileStatus
    {
        get
        {
            return (FileStatus)Session["fileStatus"];
        }
        set
        {
            currentFileStatus = value;
        }
    }

    

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


    //AJAX-enabled web method to detect the format of the file
    //Params: the uploaded filename 
    //Returns: A JSON-encoded FileStatus object containing the extension and the type (Recognized, Unrecognized, or Viewable)
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static FileStatus DetectFormat(string filename)
    {

        FileStatus currentStatus = new FileStatus("", FormatType.UNRECOGNIZED);


        /*The temp filename is a sha1 hash
          We should generate a final filename from the title once we bind,
          since if we just left it as-is, people could name their stuff
          "sh!ttymodel.zip" or some other expletive that we may become
          responsible for. Plus a lot of people suck at naming files,
          and batched stuff could have "10933.zip" as the filename */
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
                        currentStatus.extension = f.Extension;
                        currentStatus.type = FormatType.RECOGNIZED;
                        currentStatus.msg = FileStatus.WarningMessage;
                        recognizedCount++;
                    }
                }
            }

            //Make sure there is only one recognized or viewable model format in the zip file
            //If multiple have been detected, set the format type and break
            if (recognizedCount > 1)
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



    //Params: none
    //Returns: A JSON-encoded FileStatus object containing the extension and the type (Recognized, Unrecognized, or Viewable) PLUS conversion status
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

                if (File.Exists(HttpContext.Current.Server.MapPath("~/App_Data/converterTemp/" + status.hashname)))
                {
                    File.Delete(HttpContext.Current.Server.MapPath("~/App_Data/converterTemp/" + status.hashname));
                }
                File.Move(tempfile, HttpContext.Current.Server.MapPath("~/App_Data/converterTemp/" + status.hashname));
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

    //Params: none
    //Returns: none
    //Description: Clears the Session variables and stored temp files from the server
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static void UploadReset(string filename)
    {    
        //Delete the temp file if it exists
        if (File.Exists(HttpContext.Current.Server.MapPath("~/App_Data/" + filename)))
        {
            deleteTempFile(filename);
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

    /* Updates the content object with the metadata provided by the user in Step 1
     *
     * Params: TitleInput - the text from the title textbox
     *         DescriptionInput - the text from the description textarea
     *         TagsInput - the comma or space-delimited list of tags from the tags textbox
     *
     * Returns: JSON object containing the parameters needed for LoadViewer.js to load the 3D viewer, and whether
     *          the viewer needs to be loaded 
     *        
     */
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static ViewerLoadParams Step1_Submit(string TitleInput, string DescriptionInput, string TagsInput)
    {
        FileStatus currentStatus = (FileStatus)HttpContext.Current.Session["fileStatus"];
        currentStatus.filename = TitleInput.Trim().Replace(' ', '_') + ".zip";

        ContentObject tempFedoraCO = (ContentObject)HttpContext.Current.Session["contentObject"];
        tempFedoraCO.Title = TitleInput.Trim();
        tempFedoraCO.Description = DescriptionInput.Trim();
        tempFedoraCO.Location = currentStatus.filename;


        //Add the keywords
        if (TagsInput.LastIndexOf(',') == -1) //They used whitespace as delimiter
        {
            tempFedoraCO.Keywords = String.Join(",", TagsInput.Split(' '));
        }
        else
        {
            tempFedoraCO.Keywords = TagsInput;
        }

        ViewerLoadParams jsReturnParams = new ViewerLoadParams();
        jsReturnParams.FlashLocation = tempFedoraCO.Location;

        if (currentStatus.type == FormatType.VIEWABLE)
        {
            tempFedoraCO.DisplayFile = currentStatus.filename.Replace("zip", "o3d");
            jsReturnParams.IsViewable = true;
            jsReturnParams.BasePath = "../Public/";
            jsReturnParams.BaseContentUrl = "Model.ashx?temp=true&file=";
            jsReturnParams.O3DLocation = currentStatus.hashname.Replace("zip", "o3d");
            jsReturnParams.FlashLocation = currentStatus.hashname;
            jsReturnParams.ShowScale = true;
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

    /* Updates the temporary content object with the Up Axis and Unit Scale
     *  set in Step 2.
     * 
     * Params: ScaleValue - A float value representing the scale, expressed in meters
     *         UpAxis - A string, either "Y" or "Z", that specifies the selected Up Axis value
     *         
     * Returns: none
     */
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static void Step2_Submit(string ScaleValue, string UpAxis)
    {
        ContentObject tempCO = (ContentObject) HttpContext.Current.Session["contentObject"];
        tempCO.UpAxis = UpAxis;
        tempCO.UnitScale = ScaleValue;
        HttpContext.Current.Session["contentObject"] = tempCO;
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string SubmitUpload(string DeveloperName, string ArtistName, string DeveloperUrl, string SponsorName, string SponsorUrl, string LicenseType)
    {

        
        ContentObject tempCO = (ContentObject)HttpContext.Current.Session["contentObject"];
        tempCO.DeveloperName = DeveloperName;
        tempCO.ArtistName = ArtistName;
        tempCO.MoreInformationURL = DeveloperUrl;
        //tempCO.SponsorURL = SponsorUrl; !missing SponsorUrl metadata in ContentObject
        tempCO.CreativeCommonsLicenseURL = String.Format(System.Configuration.ConfigurationManager.AppSettings["CCBaseUrl"], String.Join("-", LicenseType.Split(' ')));
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

    /* Deletes an image file from the imageTemp directory, resulting from a re-upload of an image file
     * 
     * Params: filename - the name of the file (no path) in imageTemp that needs to be deleted
     * Returns: none
     */
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

    public static void deleteTempFile(string filename)
    {
        File.Delete(HttpContext.Current.Server.MapPath("~/App_Data/" + filename));
    }

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
