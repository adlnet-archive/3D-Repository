//  Copyright 2011 U.S. Department of Defense

//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at

//      http://www.apache.org/licenses/LICENSE-2.0

//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.



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
using System.ComponentModel;
using System.Collections.Generic;


/// <summary>
/// 
/// </summary>
public class FedoraFileInfo
{
    /// <summary>
    /// 
    /// </summary>
    public string SourceFilepath;
    /// <summary>
    /// 
    /// </summary>
    public string DestinationFilename;
    /// <summary>
    /// 
    /// </summary>
    protected bool mNeedsId;
    /// <summary>
    /// 
    /// </summary>
    public bool NeedsId
    {
        get { return mNeedsId; }
    }
    /// <summary>
    /// 
    /// </summary>
    public FedoraFileInfo()
    {
        mNeedsId = false;
    }
}
/// <summary>
/// 
/// </summary>
public class FedoraReferencedFileInfo : FedoraFileInfo
{
    /// <summary>
    /// 
    /// </summary>
    public enum ReferencedIdType
    {
        DISPLAY_FILE,
        SCREENSHOT,
        DEV_LOGO,
        SPONSOR_LOGO
    }
    /// <summary>
    /// 
    /// </summary>
    public ReferencedIdType idType;
    /// <summary>
    /// 
    /// </summary>
    public FedoraReferencedFileInfo()
    {
        mNeedsId = true;
    }
}
/// <summary>
/// 
/// </summary>
public class FedoraFileUploadCollection
{
    /// <summary>
    /// 
    /// </summary>
    public string hash;
    public ContentObject currentFedoraObject;
    private List<FedoraFileInfo> mFileList;
    public List<FedoraFileInfo> FileList
    {
        get { return mFileList; }
    }

    public FedoraFileUploadCollection()
    {
        mFileList = new List<FedoraFileInfo>();
    }
}

/// <summary>
/// Web Page that allows for the uploading of 3D model content and associated metadata
/// </summary>
public partial class Users_Upload : Website.Pages.PageBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
            try
            {
                using (ZipFile zip = ZipFile.Read(HttpContext.Current.Server.MapPath("~/App_Data/") + currentStatus.hashname))
                {
                    int i = 0;
                    foreach (string s in zip.EntryFileNames)
                    {

                        System.IO.FileInfo f = new System.IO.FileInfo(s);
                        if (FileStatus.GetType(f.Extension) == FormatType.VIEWABLE)
                        {

                            if (zip.Entries[i].UncompressedSize == 0)
                            {
                                currentStatus.msg = FileStatus.ModelFileEmptyMessage;
                                return currentStatus;
                            }

                            currentStatus.extension = f.Extension;
                            currentStatus.type = FormatType.VIEWABLE;

                            recognizedCount++;
                        }
                        else if (FileStatus.GetType(f.Extension) == FormatType.RECOGNIZED)
                        {
                            if (currentStatus.type != FormatType.VIEWABLE)
                            {
                                if (zip.Entries[i].UncompressedSize == 0)
                                {
                                    currentStatus.msg = FileStatus.ModelFileEmptyMessage;
                                    return currentStatus;
                                }
                                currentStatus.extension = f.Extension;
                                currentStatus.type = FormatType.RECOGNIZED;
                            }
                            currentStatus.msg = FileStatus.WarningMessage;
                            viewableCount++;
                        }
                        i++;
                    }
                }
            }
            catch (ZipException e)
            {
                currentStatus.msg = FileStatus.InvalidZipMessage;
                return currentStatus;
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
        tempFedoraObject.Format = currentStatus.extension;
        HttpContext.Current.Session["contentObject"] = tempFedoraObject;

        return currentStatus;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="indata"></param>
    /// <returns></returns>
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static FileStatus SaveChanges(string indata)
    {
        string data = indata;
        StreamWriter fs = new StreamWriter("c:\\tempjson.json");
        fs.Write(data);
        fs.Close();
        return new FileStatus("asdf", "asdf");
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
        Utility_3D.ConverterOptions cOptions = new Utility_3D.ConverterOptions();
        cOptions.EnableTextureConversion(Utility_3D.ConverterOptions.PNG);
        cOptions.EnableScaleTextures(Website.Config.MaxTextureDimension);

        FileStatus status = (FileStatus)HttpContext.Current.Session["fileStatus"];

        if (status == null)
        {
            HttpContext.Current.Response.StatusCode = 500;
            return new FileStatus("error", "error");
        }
        using (FileStream stream = new FileStream(HttpContext.Current.Server.MapPath("~/App_data/" + status.hashname), FileMode.Open))
        {

            ContentObject tempFedoraObject = (ContentObject)HttpContext.Current.Session["contentObject"];
            try //convert the model
            {
                model = pack.Convert(stream, status.hashname, cOptions);

                if (model._ModelData.VertexCount.Polys == 0 && model._ModelData.VertexCount.Verts == 0)
                {
                    //don't say it's ok!
                }

                HttpContext.Current.Session["contentTextures"] = model.textureFiles;
                HttpContext.Current.Session["contentMissingTextures"] = model.missingTextures;


                status.converted = "true";
                HttpContext.Current.Session["fileStatus"] = status;

                Utility_3D.Parser.ModelData mdata = model._ModelData;
                tempFedoraObject.NumPolygons = mdata.VertexCount.Polys;
                tempFedoraObject.NumTextures = mdata.ReferencedTextures.Length;
                tempFedoraObject.UpAxis = mdata.TransformProperties.UpAxis;
                if (mdata.TransformProperties.UnitMeters != 0)
                {
                    tempFedoraObject.UnitScale = System.Convert.ToString(mdata.TransformProperties.UnitMeters);
                }
                else
                {
                    tempFedoraObject.UnitScale = "1.0";
                }

                HttpContext.Current.Session["contentObject"] = tempFedoraObject;


                //Save the O3D file for the viewer into a temporary directory
                var tempfile = HttpContext.Current.Server.MapPath("~/App_Data/viewerTemp/" + status.hashname).Replace(".skp", ".zip");
                using (System.IO.FileStream savefile = new FileStream(tempfile, FileMode.Create))
                {
                    byte[] filedata = new Byte[model.data.Length];
                    model.data.CopyTo(filedata, 0);
                    savefile.Write(model.data, 0, (int)model.data.Length);
                }
                ConvertFileToO3D(HttpContext.Current, tempfile);
                var rootDir = HttpContext.Current.Server.MapPath("~/App_Data/converterTemp/");
                var fileName = Path.Combine(rootDir, status.hashname.Replace(".skp", ".zip"));
                if (!Directory.Exists(rootDir))
                {
                    Directory.CreateDirectory(rootDir);
                }
                if (File.Exists(fileName))
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
    /// 
    /// </summary>
    /// <param name="data"></param>
    static void UploadToFedora(object data)
    {
        var factory = new DataAccessFactory();
        IDataRepository dal = factory.CreateDataRepositorProxy();
        FedoraFileUploadCollection modelsCol = data as FedoraFileUploadCollection;
        if (modelsCol == null) return;
        string pid = modelsCol.currentFedoraObject.PID;

        foreach (FedoraFileInfo f in modelsCol.FileList)
        {
            string newId;
            using (FileStream fstream = File.OpenRead(f.SourceFilepath))
            {
                newId = dal.SetContentFile(fstream, pid, f.DestinationFilename);
            }

            if (f.NeedsId)
            {
                switch (((FedoraReferencedFileInfo)f).idType)
                {
                    case FedoraReferencedFileInfo.ReferencedIdType.DISPLAY_FILE:
                        modelsCol.currentFedoraObject.DisplayFileId = newId;
                        break;
                    case FedoraReferencedFileInfo.ReferencedIdType.SCREENSHOT:
                        modelsCol.currentFedoraObject.ScreenShotId = newId;
                        break;
                    case FedoraReferencedFileInfo.ReferencedIdType.DEV_LOGO:
                        modelsCol.currentFedoraObject.DeveloperLogoImageFileNameId = newId;
                        break;
                    case FedoraReferencedFileInfo.ReferencedIdType.SPONSOR_LOGO:
                        modelsCol.currentFedoraObject.SponsorLogoImageFileNameId = newId;
                        break;
                    default: break;
                }
            }
        }
        modelsCol.currentFedoraObject.Ready = true;
        dal.UpdateContentObject(modelsCol.currentFedoraObject);
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
                }
                catch { }
            }

            string viewerTempPath = basePath + "viewerTemp/" + filename.Replace("zip", "o3d").Replace("skp", "o3d").ToLower();
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
        if (HttpContext.Current.Session["fileStatus"] != null)
        {
            HttpContext.Current.Session["fileStatus"] = null;
        }

        //Delete the model from session if it exists
        if (HttpContext.Current.Session["contentObject"] != null)
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
        var fileName = TitleInput.Trim().Replace(' ', '_').ToLower();

        if (currentStatus.hashname.LastIndexOf(".skp") != -1)
        {
            fileName += ".skp";
        }
        else
        {
            fileName += ".zip";
        }
        if (currentStatus != null)
        {

            currentStatus.filename = fileName;
        }

        ContentObject tempFedoraCO = (ContentObject)HttpContext.Current.Session["contentObject"];
        tempFedoraCO.PID = "";

        HttpServerUtility serverUtil = HttpContext.Current.Server;

        tempFedoraCO.Title = serverUtil.HtmlEncode(TitleInput.Trim());
        tempFedoraCO.Description = serverUtil.HtmlEncode(DescriptionInput.Trim());
        
        if(currentStatus.type == FormatType.VIEWABLE)
            tempFedoraCO.Location = fileName.Replace(".skp", ".zip");

        string cleanTags = "";
        foreach (string s in TagsInput.Split(','))
        {
            cleanTags += s.Trim() + ",";
        }
        cleanTags = serverUtil.HtmlEncode(cleanTags.Trim(','));
        tempFedoraCO.Keywords = cleanTags;

        JsonWrappers.ViewerLoadParams jsReturnParams = new JsonWrappers.ViewerLoadParams();
        jsReturnParams.FlashLocation = tempFedoraCO.Location;

        if (currentStatus.type == FormatType.VIEWABLE)
        {
            tempFedoraCO.DisplayFile = currentStatus.filename.Replace("zip", "o3d").Replace("skp", "o3d");
            jsReturnParams.isTemp = true;
            jsReturnParams.IsViewable = true;
            jsReturnParams.BasePath = "../Public/Model.ashx";
            jsReturnParams.O3DLocation = currentStatus.hashname.ToLower().Replace("zip", "o3d").Replace("skp", "o3d");
            jsReturnParams.FlashLocation = currentStatus.hashname.Replace("skp", "zip");
            jsReturnParams.ShowScreenshot = true;
            jsReturnParams.UpAxis = tempFedoraCO.UpAxis;
            jsReturnParams.UnitScale = tempFedoraCO.UnitScale;
            jsReturnParams.NumPolygons = tempFedoraCO.NumPolygons;
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
        HttpServerUtility server = context.Server;
        FileStatus currentStatus = (FileStatus)context.Session["fileStatus"];

        var factory = new DataAccessFactory();
        IDataRepository dal = factory.CreateDataRepositorProxy();
        ContentObject tempCO = (ContentObject)context.Session["contentObject"];
        tempCO.UpAxis = server.HtmlEncode(UpAxis);
        tempCO.UnitScale = server.HtmlEncode(ScaleValue);
        //dal.UpdateContentObject(tempCO);
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
    /// <param name="RequireResubmit">A string representing a boolean indicator to whether additional policy should be enforced.</param>
    /// <returns>A string containing the ContentObjectID for the newly inserted Content Object</returns>
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string SubmitUpload(string DeveloperName, string ArtistName, string DeveloperUrl,
                                       string SponsorName, string SponsorUrl, string LicenseType,
                                       bool RequireResubmit)
    {
        HttpServerUtility server = HttpContext.Current.Server;

        try
        {
            FileStatus status = (FileStatus)HttpContext.Current.Session["fileStatus"];
            ContentObject tempCO = (ContentObject)HttpContext.Current.Session["contentObject"];
            var factory = new DataAccessFactory();
            IDataRepository dal = factory.CreateDataRepositorProxy();
            dal.InsertContentObject(tempCO);
            tempCO.DeveloperName = server.HtmlEncode(DeveloperName);
            tempCO.ArtistName = server.HtmlEncode(ArtistName);
            tempCO.MoreInformationURL = server.HtmlEncode(DeveloperUrl);
            tempCO.RequireResubmit = RequireResubmit;
            tempCO.SponsorName = server.HtmlEncode(SponsorName);

            string pid = tempCO.PID;
            //tempCO.SponsorURL = SponsorUrl; !missing SponsorUrl metadata in ContentObject

            if (LicenseType == "publicdomain")
            {
                tempCO.CreativeCommonsLicenseURL = "http://creativecommons.org/publicdomain/mark/1.0/";
            }
            else
            {
                tempCO.CreativeCommonsLicenseURL = String.Format(ConfigurationManager.AppSettings["CCBaseUrl"], LicenseType);
            }


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
                            tempCO.DeveloperLogoImageFileNameId = dal.SetContentFile(fstream, tempCO.PID, tempCO.DeveloperLogoImageFileName);
                            break;

                        case ImagePrefix.SPONSOR_LOGO:
                            tempCO.SponsorLogoImageFileName = "sponsor_logo" + f.Extension;
                            tempCO.SponsorLogoImageFileNameId = dal.SetContentFile(fstream, tempCO.PID, tempCO.SponsorLogoImageFileName);
                            break;

                        case ImagePrefix.SCREENSHOT:
                            tempCO.ScreenShot = "screenshot" + f.Extension;
                            tempCO.ScreenShotId = dal.SetContentFile(fstream, tempCO.PID, tempCO.ScreenShot);          
                            tempCO.ThumbnailId = dal.SetContentFile(Website.Common.GenerateThumbnail(fstream), tempCO.PID, "thumbnail.png");
                            break;

                        default:
                            break;
                    }
                }
            }
            string dataPath = HttpContext.Current.Server.MapPath("~/App_Data/");
            if (status.type == FormatType.VIEWABLE)
            {
                //Upload the original file
                using (FileStream s = new FileStream(dataPath + status.hashname, FileMode.Open))
                {
                    tempCO.OriginalFileId = dal.SetContentFile(s, pid, "original_" + status.filename);
                    tempCO.OriginalFileName = "original_" + status.filename;
                }
                using (FileStream s = new FileStream(Path.Combine(dataPath, "converterTemp/" + status.hashname.ToLower().Replace("skp", "zip")), FileMode.Open))
                {
                    tempCO.DisplayFileId = dal.SetContentFile(s, pid, status.filename.ToLower().Replace("skp", "zip"));
                }
                using (FileStream s = new FileStream(Path.Combine(dataPath, "viewerTemp/" + status.hashname.ToLower().Replace("skp", "o3d").Replace("zip", "o3d")), FileMode.Open))
                {
                    dal.SetContentFile(s, pid, status.filename.ToLower().Replace("skp", "o3d").Replace("zip", "o3d"));
                }
            }
            else if (status.type == FormatType.RECOGNIZED)
            {
                using (FileStream s = new FileStream(dataPath + status.hashname, FileMode.Open))
                {
                    dal.SetContentFile(s, pid, "original_" + status.filename);
                }
            }
            tempCO.Enabled = true;
            tempCO.UploadedDate = DateTime.Now;

            dal.UpdateContentObject(tempCO);
            UploadReset(status.hashname);

            List<string> textureReferences = HttpContext.Current.Session["contentTextures"] as List<string>;

            List<string> textureReferenceMissing = HttpContext.Current.Session["contentMissingTextures"] as List<string>;

            if (textureReferences != null)
            {
                foreach (string tex in textureReferences)
                {
                    tempCO.SetParentRepo(dal);
                    tempCO.AddTextureReference(tex, "unknown", 0);
                }
            }
            if (textureReferenceMissing != null)
            {
                foreach (string tex in textureReferenceMissing)
                {
                    tempCO.SetParentRepo(dal);
                    tempCO.AddMissingTexture(tex, "unknown", 0);
                }
            }

            return tempCO.PID;
        }
        catch (Exception e)
        {
            #if DEBUG
                return String.Format("fedoraError|" + e.Message + "<br /><br />" + e.StackTrace);
            #else
                return "fedoraError|" + ConfigurationManager.AppSettings["UploadPage_FedoraError"];
            #endif
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

        var application = context.Server.MapPath("~/processes/o3dConverter.exe");
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
