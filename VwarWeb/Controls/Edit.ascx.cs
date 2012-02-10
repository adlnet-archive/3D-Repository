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
using vwarDAL;
using System.Collections.Generic;


public partial class Controls_Edit : Website.Pages.ControlBase
{
    private Utility_3D.ConvertedModel mModel;

    protected bool IsNew
    {
        get
        {

            return bool.Parse(ViewState["IsNew"].ToString());


        }
        set
        {
            ViewState["IsNew"] = value;
        }

    }

    private bool IsModelUpload
    {
        get { return true; }//Not sure what this would break if we took it away...
    }
    private const string FEDORACONTENTOBJECT = "FedoraContentObject";

    private ContentObject CachedFedoraContentObject
    {
        get
        {
            var co = Session[FEDORACONTENTOBJECT] as ContentObject;
            if (co == null || !co.PID.Equals(ContentObjectID, StringComparison.InvariantCultureIgnoreCase))
            {
                CachedFedoraContentObject = DAL.GetContentObjectById(ContentObjectID, false, false);
            }
            return Session[FEDORACONTENTOBJECT] as ContentObject;
        }
        set
        {
            Session[FEDORACONTENTOBJECT] = value;
        }

    }
    private ContentObject FedoraContentObject;
    protected string ContentObjectID
    {
        get
        {
            string rv = "";
            if (Request.QueryString["ContentObjectID"] != null)
            {
                rv = Request.QueryString["ContentObjectID"].Trim();
                IsNew = false;
            }
            else if (ViewState["ContentObjectID"] != null)
            {
                rv = ViewState["ContentObjectID"].ToString();
            }


            return rv;
        }
        set { ViewState["ContentObjectID"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        //maintain scroll position

        if (this.Page.Master.FindControl("SearchPanel") != null)
        {
            //hide the search panel
            this.Page.Master.FindControl("SearchPanel").Visible = false;
        }

        try { FedoraContentObject = CachedFedoraContentObject; }
        catch { if (!String.IsNullOrEmpty(ContentObjectID)) FedoraContentObject = DAL.GetContentObjectById(ContentObjectID, false); }
        //redirect if user is not authenticated
        if (!Context.User.Identity.IsAuthenticated)
        {
            Response.Redirect(Website.Pages.Types.Default);
        }

        if (!Page.IsPostBack)
        {
            IsNew = true;
            var id = ContentObjectID;

            this.MultiView1.ActiveViewIndex = 0;
            this.BindCCLHyperLink();
            this.BindContentObject();

            ContentFileUploadRequiredFieldValidator.Enabled = IsNew;

        }
    }

    private void BindContentObject()
    {


        if (!this.IsNew)
        {

            if (this.FedoraContentObject != null)
            {


                if (!string.IsNullOrEmpty(this.FedoraContentObject.DisplayFile))
                {
                    this.ContentFileUploadRequiredFieldValidator.Enabled = false;
                }

                this.BindThumbnail();

                //redirect if the user is not the owner
                if (!this.FedoraContentObject.SubmitterEmail.Equals(Context.User.Identity.Name, StringComparison.InvariantCultureIgnoreCase) & !Website.Security.IsAdministrator())
                {
                    Response.Redirect(Website.Pages.Types.Default);
                    return;
                }

                //Title
                if (!string.IsNullOrEmpty(this.FedoraContentObject.Title))
                {
                    this.TitleTextBox.Text = this.FedoraContentObject.Title.Trim();
                }


                //Format
                if (!string.IsNullOrEmpty(this.FedoraContentObject.Format))
                {
                    this.FormatTextBox.Text = this.FedoraContentObject.Format.Trim();
                }

                //Developer name
                if (!string.IsNullOrEmpty(this.FedoraContentObject.DeveloperName))
                {
                    this.DeveloperNameTextBox.Text = this.FedoraContentObject.DeveloperName.Trim();
                }



                //Sponsor Name
                if (!string.IsNullOrEmpty(this.FedoraContentObject.SponsorName))
                {
                    this.SponsorNameTextBox.Text = this.FedoraContentObject.SponsorName.Trim();
                }

                //Artist Name
                if (!string.IsNullOrEmpty(this.FedoraContentObject.ArtistName))
                {
                    this.ArtistNameTextBox.Text = this.FedoraContentObject.ArtistName.Trim();
                }


                //CC License
                if (!string.IsNullOrEmpty(this.FedoraContentObject.CreativeCommonsLicenseURL))
                {



                    if (this.CCLicenseDropDownList.Items.FindByValue(this.FedoraContentObject.CreativeCommonsLicenseURL) != null)
                    {
                        //clear selection
                        if (this.CCLicenseDropDownList.SelectedItem != null)
                        {
                            this.CCLicenseDropDownList.ClearSelection();
                        }


                        this.CCLicenseDropDownList.Items.FindByValue(this.FedoraContentObject.CreativeCommonsLicenseURL).Selected = true;
                    }

                    //set the hyperlink
                    this.CCLHyperLink.NavigateUrl = this.FedoraContentObject.CreativeCommonsLicenseURL.Trim();
                }
                else
                {
                    //set none selected
                    if (this.CCLicenseDropDownList.SelectedItem != null)
                    {
                        this.CCLicenseDropDownList.ClearSelection();
                    }

                    this.CCLicenseDropDownList.Items[this.CCLicenseDropDownList.Items.Count - 1].Selected = true;


                }

                this.RequireResubmitCheckbox.Checked = FedoraContentObject.RequireResubmit;



                //Description
                if (!string.IsNullOrEmpty(this.FedoraContentObject.Description))
                {
                    this.DescriptionTextBox.Text = this.FedoraContentObject.Description.Trim();

                }

                //More Information
                if (!string.IsNullOrEmpty(this.FedoraContentObject.MoreInformationURL))
                {
                    this.MoreInformationURLTextBox.Text = this.FedoraContentObject.MoreInformationURL.Trim();

                }

                //Keywords
                if (!string.IsNullOrEmpty(this.FedoraContentObject.Keywords))
                {
                    this.KeywordsTextBox.Text = this.FedoraContentObject.Keywords;
                }


                //Unit Scale
                if (!string.IsNullOrEmpty(this.FedoraContentObject.UnitScale))
                {


                    this.UnitScaleTextBox.Text = this.FedoraContentObject.UnitScale.Trim();
                }

                //Up Axis
                if (!string.IsNullOrEmpty(this.FedoraContentObject.UpAxis))
                {
                    this.UpAxisRadioButtonList.ClearSelection();
                    UpAxisRadioButtonList.SelectedValue = this.FedoraContentObject.UpAxis;
                }

                //NumPolygons
                if (!string.IsNullOrEmpty(this.FedoraContentObject.NumPolygons.ToString()))
                {
                    this.NumPolygonsTextBox.Text = this.FedoraContentObject.NumPolygons.ToString();
                }

                //NumTextures
                if (!string.IsNullOrEmpty(this.FedoraContentObject.NumTextures.ToString()))
                {
                    this.NumTexturesTextBox.Text = this.FedoraContentObject.NumTextures.ToString();
                }

                //UV Coordinate Channel
                if (!string.IsNullOrEmpty(this.FedoraContentObject.UVCoordinateChannel))
                {
                    this.UVCoordinateChannelTextBox.Text = this.FedoraContentObject.UVCoordinateChannel.Trim();
                }



            }
            else
            {
                //Show error message
                this.errorMessage.Text = "Model not found.";
            }


        }



        //bind developer logo
        this.BindDeveloperLogo();

        //bind sponsor logo
        this.BindSponsorLogo();


    }
    private void LogError(string message)
    {
        using (FileStream s = new FileStream("C:\\log.txt", FileMode.OpenOrCreate))
        {
            using (StreamWriter writer = new StreamWriter(s))
            {
                writer.WriteLine(message);
            }
        }
    }
    protected void Step1NextButton_Click(object sender, EventArgs e)
    {

        //update
        if (IsModelUpload)
        {

            vwarDAL.IDataRepository dal = DAL;


            if (this.IsNew)
            {
                //create new & add to session       
                ContentObject co = new ContentObject(dal);
                co.Title = this.TitleTextBox.Text.Trim();
                co.UploadedDate = DateTime.Now;
                co.LastModified = DateTime.Now;
                co.Views = 0;
                co.SubmitterEmail = Context.User.Identity.Name.Trim();
                dal.InsertContentObject(co);
                FedoraContentObject = co;
                this.ContentObjectID = co.PID;

            }
            else
            {
                FedoraContentObject.Title = TitleTextBox.Text.Trim();
            }


            if (this.FedoraContentObject != null)
            {

                //asset type                       
                this.FedoraContentObject.AssetType = "Model";
                string newFileName = TitleTextBox.Text.ToLower().Replace(' ', '_') + Path.GetExtension(this.ContentFileUpload.PostedFile.FileName);
                //model upload
                Utility_3D.ConvertedModel model = null;
                if (this.ContentFileUpload.HasFile)
                {

                    string newOriginalFileName = "original_" + newFileName;
                    
                    if (IsNew)
                    {
                        using (MemoryStream stream = new MemoryStream())
                        {
                            stream.Write(this.ContentFileUpload.FileBytes, 0, this.ContentFileUpload.FileBytes.Length);
                            FedoraContentObject.OriginalFileId = dal.SetContentFile(stream, FedoraContentObject.PID, newOriginalFileName);
                        }
                    }
                    else
                    {

                        //Update the original file
                        dal.UpdateFile(this.ContentFileUpload.FileBytes, FedoraContentObject.PID, FedoraContentObject.OriginalFileName, newOriginalFileName);

                    }
                    FedoraContentObject.OriginalFileName = newOriginalFileName;

                    Utility_3D.Model_Packager pack = new Utility_3D.Model_Packager();
                    Utility_3D _3d = new Utility_3D();
                    Utility_3D.ConverterOptions cOptions = new Utility_3D.ConverterOptions();
                    cOptions.EnableTextureConversion(Utility_3D.ConverterOptions.PNG);
                    cOptions.EnableScaleTextures(Website.Config.MaxTextureDimension);
                    _3d.Initialize(Website.Config.ConversionLibarayLocation);

                    string UploadedFilename = newFileName;
                    if (Path.GetExtension(UploadedFilename) != ".zip")
                        UploadedFilename = Path.ChangeExtension(newFileName, ".zip");

                    try
                    {
                        model = pack.Convert(this.ContentFileUpload.PostedFile.InputStream, this.ContentFileUpload.PostedFile.FileName, cOptions);
                    }
                    catch
                    {
                        Response.Redirect(Page.ResolveClientUrl("~/Public/Model.aspx?ContentObjectID=" + this.ContentObjectID));
                    }


                    var displayFilePath = "";
                    string convertedFileName = newFileName.Replace(Path.GetExtension(newFileName).ToLower(), ".zip");
                    if (this.ContentFileUpload.HasFile)
                    {
                        using (Stream stream = new MemoryStream())
                        {
                            stream.Write(model.data, 0, model.data.Length);
                            stream.Seek(0, SeekOrigin.Begin);
                            FedoraContentObject.Location = UploadedFilename;
                            //FedoraContentObject.DisplayFileId =
                            dal.SetContentFile(stream, FedoraContentObject, UploadedFilename);
                        }
                    }
                    else
                    {
                        using (MemoryStream stream = new MemoryStream())
                        {

                            stream.Write(model.data, 0, model.data.Length);
                            stream.Seek(0, SeekOrigin.Begin);
                            dal.SetContentFile(stream, FedoraContentObject, UploadedFilename);
                            FedoraContentObject.Location = UploadedFilename;
                        }
                    }
                    FedoraContentObject.Location = convertedFileName;

                    if (model.type != "UNKNOWN")
                    {
                        string optionalPath = (newFileName.LastIndexOf("o3d", StringComparison.CurrentCultureIgnoreCase) != -1) ? "viewerTemp/" : "converterTemp/";
                        string pathToTempFile = "~/App_Data/" + optionalPath;

                        string destPath = Path.Combine(Context.Server.MapPath(pathToTempFile), newFileName);

                        System.IO.FileStream savefile = new FileStream(destPath, FileMode.OpenOrCreate);
                        byte[] filedata = new Byte[model.data.Length];
                        model.data.CopyTo(filedata, 0);
                        savefile.Write(model.data, 0, (int)model.data.Length);
                        savefile.Close();

                        string outfilename = Context.Server.MapPath("~/App_Data/viewerTemp/") + newFileName.Replace(".zip", ".o3d");
                        string convertedtempfile = ConvertFileToO3D(destPath,outfilename);



                        displayFilePath = FedoraContentObject.DisplayFile;
                        string o3dFileName = newFileName.Replace(Path.GetExtension(newFileName).ToLower(), ".o3d");
                        if (this.ContentFileUpload.HasFile)
                        {
                            using (FileStream stream = new FileStream(convertedtempfile, FileMode.Open))
                            {
                                FedoraContentObject.DisplayFileId = dal.SetContentFile(stream, FedoraContentObject, o3dFileName);
                            }
                        }
                        else
                        {
                            using (MemoryStream stream = new MemoryStream())
                            {
                                stream.Write(filedata, 0, filedata.Length);
                                stream.Seek(0, SeekOrigin.Begin);
                                dal.SetContentFile(stream, FedoraContentObject, UploadedFilename);
                                FedoraContentObject.DisplayFile = Path.GetFileName(FedoraContentObject.DisplayFile);
                            }
                        }
                        FedoraContentObject.DisplayFile = o3dFileName;

                    }
                    else
                    {
                        FedoraContentObject.DisplayFile = string.Empty;
                    }


                    FedoraContentObject.UnitScale = model._ModelData.TransformProperties.UnitMeters.ToString();
                    FedoraContentObject.UpAxis = model._ModelData.TransformProperties.UpAxis;
                    FedoraContentObject.NumPolygons = model._ModelData.VertexCount.Polys;
                    FedoraContentObject.NumTextures = model._ModelData.ReferencedTextures.Length;

                    PopulateValidationViewMetadata(FedoraContentObject);

                }

                //upload thumbnail 
                if (this.ThumbnailFileUpload.HasFile)
                {
                    int length = (int)this.ThumbnailFileUpload.PostedFile.InputStream.Length;

                    if (IsNew || FedoraContentObject.ScreenShot == "")// order counts here have to set screenshot id after the update so we can find the correct dsid
                        this.FedoraContentObject.ScreenShot = "screenshot.png";
                        
                    FedoraContentObject.ScreenShotId = dal.SetContentFile(this.ThumbnailFileUpload.PostedFile.InputStream, this.FedoraContentObject, FedoraContentObject.ScreenShot);

                    string ext = new FileInfo(ThumbnailFileUpload.PostedFile.FileName).Extension.ToLower();
                    System.Drawing.Imaging.ImageFormat format;

                    if (ext == ".png")
                        format = System.Drawing.Imaging.ImageFormat.Png;
                    else if (ext == ".jpg")
                        format = System.Drawing.Imaging.ImageFormat.Jpeg;
                    else if (ext == ".gif")
                        format = System.Drawing.Imaging.ImageFormat.Gif;
                    else
                    {
                        ScreenshotValidator.Visible = true;
                        ScreenshotValidator.Text = "Nice job generating a POST request without the interface. Don't you feel special?";
                        return;
                    }
                    
                    //Avoid wasting space by removing old thumbnails
                    if (!String.IsNullOrEmpty(FedoraContentObject.ThumbnailId))
                        File.Delete("~/thumbnails/" + FedoraContentObject.ThumbnailId);

                    //Use the original file bytes to remain consistent with the new file upload ID creation for thumbnails
                    FedoraContentObject.ThumbnailId = Website.Common.GetFileSHA1AndSalt(ThumbnailFileUpload.PostedFile.InputStream) + ext;

                    using (FileStream outFile = new FileStream(HttpContext.Current.Server.MapPath("~/thumbnails/" + FedoraContentObject.ThumbnailId), FileMode.Create))
                        Website.Common.GenerateThumbnail(ThumbnailFileUpload.PostedFile.InputStream, outFile, format);

                }

                //creative commons license url
                if (this.CCLicenseDropDownList.SelectedItem != null && this.CCLicenseDropDownList.SelectedValue != "None")
                {
                    this.FedoraContentObject.CreativeCommonsLicenseURL = this.CCLicenseDropDownList.SelectedValue.Trim();
                }

                //Require Resubmit Checkbox
                FedoraContentObject.RequireResubmit = this.RequireResubmitCheckbox.Checked;

                //developer logo
                this.UploadDeveloperLogo(dal, this.FedoraContentObject);

                //developer name
                if (!string.IsNullOrEmpty(this.DeveloperNameTextBox.Text))
                {
                    this.FedoraContentObject.DeveloperName = this.DeveloperNameTextBox.Text.Trim();
                }

                //sponsor logo
                this.UploadSponsorLogo(dal, this.FedoraContentObject);


                //sponsor name
                if (!string.IsNullOrEmpty(this.SponsorNameTextBox.Text))
                {
                    this.FedoraContentObject.SponsorName = this.SponsorNameTextBox.Text.Trim();
                }

                //artist name
                if (!string.IsNullOrEmpty(this.ArtistNameTextBox.Text))
                {
                    this.FedoraContentObject.ArtistName = this.ArtistNameTextBox.Text.Trim();
                }

                //format
                if (!string.IsNullOrEmpty(this.FormatTextBox.Text))
                {
                    this.FedoraContentObject.Format = this.FormatTextBox.Text.Trim();
                }


                //description
                if (!string.IsNullOrEmpty(this.DescriptionTextBox.Text))
                {
                    this.FedoraContentObject.Description = this.DescriptionTextBox.Text.Trim();
                }

                //more information url
                if (!string.IsNullOrEmpty(this.MoreInformationURLTextBox.Text))
                {
                    this.FedoraContentObject.MoreInformationURL = this.MoreInformationURLTextBox.Text.Trim();

                }

                //keywords
                string cleanTags = "";
                foreach (string s in KeywordsTextBox.Text.Split(','))
                {
                    cleanTags += s.Trim() + ",";
                }
                cleanTags = HttpContext.Current.Server.HtmlEncode(cleanTags.Trim(','));
                this.FedoraContentObject.Keywords = cleanTags;
            }
            
            dal.UpdateContentObject(FedoraContentObject);
            if (IsNew)
            {
                PermissionsManager perm = new PermissionsManager();
                perm.SetModelToGroupLevel(System.Configuration.ConfigurationManager.AppSettings["DefaultAdminName"], FedoraContentObject.PID, vwarDAL.DefaultGroups.AllUsers, ModelPermissionLevel.Fetchable);
                perm.SetModelToGroupLevel(System.Configuration.ConfigurationManager.AppSettings["DefaultAdminName"], FedoraContentObject.PID, vwarDAL.DefaultGroups.AnonymousUsers, ModelPermissionLevel.Searchable);
            }
            SetModelDisplay();
            this.PopulateValidationViewMetadata(FedoraContentObject);
            this.MultiView1.SetActiveView(this.ValidationView);
            var admins = UserProfileDB.GetAllAdministrativeUsers();
            foreach (DataRow row in admins.Rows)
            {
                var url = Request.Url.OriginalString.Replace(Request.Url.PathAndQuery, this.ResolveUrl(Website.Pages.Types.FormatModel(this.ContentObjectID)));
                Website.Mail.SendModelUploaded(FedoraContentObject);
            }

        }

    }

    protected void PopulateValidationViewMetadata(ContentObject co)
    {


        UnitScaleTextBox.Text = co.UnitScale;
        try
        {
            UpAxisRadioButtonList.SelectedValue = co.UpAxis;
        }
        catch { }
        NumPolygonsTextBox.Text = co.NumPolygons.ToString();
        NumTexturesTextBox.Text = co.NumTextures.ToString();
        UVCoordinateChannelTextBox.Text = "1";

        if (!String.IsNullOrEmpty(co.ScreenShot))
        {
            ThumbnailImage.ImageUrl =
                Page.ResolveClientUrl("~/Public/Serve.ashx") + "?pid=" + co.PID + "&mode=GetThumbnail";
        }


    }




    protected void ValidationViewSubmitButton_Click(object sender, EventArgs e)
    {


        /* if (this.FedoraContentObject == null || String.IsNullOrEmpty(this.FedoraContentObject.PID))
         {
             FedoraContentObject = DAL.GetContentObjectById(ContentObjectID, false, false); ;
         }*/
        FedoraContentObject = DAL.GetContentObjectById(ContentObjectID, false, false);
        vwarDAL.IDataRepository dal = DAL;


        if (!string.IsNullOrEmpty(this.UnitScaleTextBox.Text))
        {
            this.FedoraContentObject.UnitScale = this.UnitScaleTextBox.Text.Trim();
        }

        this.FedoraContentObject.UpAxis = this.UpAxisRadioButtonList.SelectedValue.Trim();

        //polygons
        int numPolys = 0;
        if (int.TryParse(NumPolygonsTextBox.Text, out numPolys))
        {
            FedoraContentObject.NumPolygons = numPolys;
        }
        int numTextures = 0;
        if (int.TryParse(NumTexturesTextBox.Text, out numTextures))
        {
            FedoraContentObject.NumTextures = numTextures;
        }

        FedoraContentObject.Enabled = true;
        dal.UpdateContentObject(this.FedoraContentObject);

        //redirect
        Response.Redirect(Website.Pages.Types.FormatModel(this.ContentObjectID));

    }

    private string ConvertFileToO3D(string infilename, string outfilename)
    {
        var application = HttpContext.Current.Server.MapPath("~/processes/o3dConverter.exe");
        System.Diagnostics.ProcessStartInfo processInfo = new System.Diagnostics.ProcessStartInfo(application);
        processInfo.Arguments = String.Format("\"{0}\" \"{1}\"", infilename, outfilename);
        processInfo.WindowStyle = ProcessWindowStyle.Hidden;
        processInfo.RedirectStandardError = true;
        processInfo.CreateNoWindow = true;
        processInfo.UseShellExecute = false;
        var p = Process.Start(processInfo);
        var error = p.StandardError.ReadToEnd();
        return outfilename;
    }

    private void ConvertToVastpark(string path)
    {
        var application = Path.Combine(Path.Combine(Request.PhysicalApplicationPath, "bin"), "ModelPackager.exe");
        System.Diagnostics.ProcessStartInfo processInfo = new System.Diagnostics.ProcessStartInfo(application);
        processInfo.Arguments = path;
        processInfo.WindowStyle = ProcessWindowStyle.Hidden;
        processInfo.RedirectStandardError = true;
        processInfo.CreateNoWindow = true;
        processInfo.UseShellExecute = false;
        var p = Process.Start(processInfo);
        var error = p.StandardError.ReadToEnd();
    }

    private void SetModelDisplay()
    {

        
          //  string proxyTemplate = "Model.ashx?pid={0}&file=";
            HtmlGenericControl body = this.Page.Master.FindControl("bodyTag") as HtmlGenericControl;
            var uri = Request.Url;
            //var url = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;
           // var url = String.Format(proxyTemplate, this.FedoraContentObject.PID);//, this.FedoraContentObject.DisplayFileId);//.Replace("&", "_Amp_") + "&UpAxis=" + this.FedoraContentObject.UpAxis + "&UnitScale=" + this.FedoraContentObject.UnitScale.ToString() + "&ContentObjectID=" + this.FedoraContentObject.PID;
            //url += String.Format("VwarWeb/Public/Model.ashx?pid={0}&file={1}", FedoraContentObject.PID, FedoraContentObject.Location);
            ContentObject co = this.FedoraContentObject;

            string script;
         //   if (!this.ContentFileUpload.HasFile)
            {
                script = string.Format("vLoader = new EditViewerLoader('{0}', '{1}', '{2}', '{3}', {4});", Page.ResolveClientUrl("~/Public/Serve.ashx?mode=PreviewModel"),
                                                                                                              (co.UpAxis != null) ? co.UpAxis : "",
                                                                                                              (co.UnitScale != null) ? co.UnitScale : "", co.NumPolygons, "\"" + co.PID.Replace(':', '_') + "\"");
            }
         //   else
            {
         //       script = string.Format("vLoader = new TempViewerLoader('{0}', '{1}','{2}', '{3}','{4}');",Page.ResolveClientUrl("~/Public/PreviewTempModel.ashx"),co.Location,co.UpAxis,co.UnitScale,co.NumPolygons);


            }

            script += "vLoader.LoadViewer();";
            Page.ClientScript.RegisterStartupScript(GetType(), "loadViewer", script, true);
        
    }

    private string ExtractFile(byte[] data, string destination)
    {
        string destPath = Path.Combine(Path.GetTempPath(), destination);
        using (Ionic.Zip.ZipFile zipFile = Ionic.Zip.ZipFile.Read(data))
        {
            zipFile.ExtractAll(destPath, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
        }
        return destPath;
    }


    protected void MissingTextureViewBackButton_Click(object sender, EventArgs e)
    {
        this.MultiView1.SetActiveView(this.DefaultView);
    }

    private void GenerateMissingTextureUI(Utility_3D.ConvertedModel model)
    {
        foreach (var texture in model.missingTextures)
        {
            var textureDialog = new Controls_MissingTextures { OldFile = texture };
            MissingTextureArea.Controls.Add(textureDialog);
        }
    }

    protected void MissingTextureViewNextButton_Click(object sender, EventArgs e)
    {
        //get a reference to the model
        Utility_3D.ConvertedModel model = null;// GetModel();
        foreach (Control c in MissingTextureArea.Controls)
        {
            if (c is Controls_MissingTextures)
            {
                //loop over each dialog and find out if the user uploaded a file
                Utility_3D.Model_Packager pack = new Utility_3D.Model_Packager();
                var uploadfile = (Controls_MissingTextures)c;
                //if they uploaded a file, push the file and model into the dll which will add the file
                if (uploadfile.FileName != "")
                    pack.AddTextureToModel(ref model, uploadfile.FileContent, uploadfile.FileName);
            }
        }
        //save the changes to the model
        //SetModel(model);
        //Get the DAL

        vwarDAL.IDataRepository dal = DAL;

        //Get the content object for this model
        ContentObject contentObj = dal.GetContentObjectById(ContentObjectID, false);

        using (MemoryStream stream = new MemoryStream())
        {
            //upload the modified datastream to the dal
            stream.Write(model.data, 0, model.data.Length);
            stream.Seek(0, SeekOrigin.Begin);
            dal.SetContentFile(stream, contentObj, contentObj.Location);
        }
        this.MultiView1.SetActiveView(this.ValidationView);
    }

    protected void CancelButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(Website.Pages.Types.Default);
    }

    protected void MissingTextureViewCancelButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(Website.Pages.Types.Default);
    }

    protected void SkipStepButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(Website.Pages.Types.FormatModel(this.ContentObjectID));
    }

    protected void CCLicenseDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.BindCCLHyperLink();
    }

    private void BindCCLHyperLink()
    {

        if (this.CCLicenseDropDownList.SelectedItem != null)
        {
            this.CCLHyperLink.NavigateUrl = string.Empty;
            this.CCLHyperLink.Visible = false;

            if (!string.IsNullOrEmpty(this.CCLicenseDropDownList.SelectedValue))
            {
                this.CCLHyperLink.NavigateUrl = this.CCLicenseDropDownList.SelectedValue.Trim();
                this.CCLHyperLink.Visible = true;
            }
        }
    }

    protected void DeveloperLogoRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
    {
        //show/hide
        if (!string.IsNullOrEmpty(this.DeveloperLogoRadioButtonList.SelectedValue))
        {


            switch (this.DeveloperLogoRadioButtonList.SelectedValue.Trim())
            {
                case "0":
                    //use current logo
                    this.DeveloperLogoImage.Visible = true;
                    this.DeveloperLogoFileUploadPanel.Visible = false;
                    this.DeveloperLogoRadioButtonList.ClearSelection();
                    break;

                case "1":
                    //show upload control                   
                    this.DeveloperLogoImage.Visible = false;
                    this.DeveloperLogoFileUploadPanel.Visible = true;
                    break;

                case "2":
                    //none 

                    this.DeveloperLogoImage.Visible = false;
                    this.DeveloperLogoFileUploadPanel.Visible = false;
                    break;
            }
        }
    }

    protected void SponsorLogoRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(this.SponsorLogoRadioButtonList.SelectedValue))
        {
            switch (this.SponsorLogoRadioButtonList.SelectedValue.Trim())
            {
                case "0":

                    this.SponsorLogoImage.Visible = true;
                    this.SponsorLogoFileUploadPanel.Visible = false;
                    break;

                case "1":
                    //show upload control
                    this.SponsorLogoImage.Visible = false;
                    this.SponsorLogoFileUploadPanel.Visible = true;
                    break;

                case "2":
                    //none                  
                    this.SponsorLogoImage.Visible = false;
                    this.SponsorLogoFileUploadPanel.Visible = false;
                    break;
            }
        }
    }

    private void BindThumbnail()
    {
        this.ThumbnailFileImage.Visible = false;

        if (!this.IsNew && this.FedoraContentObject != null && !string.IsNullOrEmpty(this.FedoraContentObject.ScreenShot))
        {
            try
            {
                vwarDAL.IDataRepository vd = DAL;
                this.ThumbnailFileImage.ImageUrl = String.Format("http://{0}:{1}/json/Screenshot/{1}", Request.Url.Host, Request.Url.Port, this.FedoraContentObject.PID);
                this.ThumbnailFileImage.Visible = true;
                return;

            }
            catch
            {
            }
        }
    }

    private void BindSponsorLogo()
    {
        string logoImageURL = "";


        //check fedora
        if (!this.IsNew && this.FedoraContentObject != null && !string.IsNullOrEmpty(this.FedoraContentObject.SponsorLogoImageFileName))
        {
            this.SponsorLogoImage.ImageUrl = String.Format("~/Public/Serve.ashx?pid={0}&mode=GetSponsorLogo", this.FedoraContentObject.PID);
            return;
        }

        if (Context.User.Identity.IsAuthenticated)
        {
            UserProfile p = null;

            try
            {
                p = UserProfileDB.GetUserProfileByUserName(Context.User.Identity.Name);
            }
            catch
            {


            }



            //check profile if authenticated
            if (p != null && Context.User.Identity.IsAuthenticated)
            {
                if (p.SponsorLogo != null && !string.IsNullOrEmpty(p.SponsorLogoContentType))
                {

                    logoImageURL = Website.Pages.Types.FormatProfileImageHandler(p.UserID.ToString(), "Sponsor");

                    if (!string.IsNullOrEmpty(logoImageURL))
                    {
                        this.SponsorLogoImage.ImageUrl = logoImageURL.Trim();
                        return;

                    }

                }

            }



        }



        //Rremove use current logo from radiobuttonlist, show file upload
        if (string.IsNullOrEmpty(logoImageURL))
        {
            //clear selection
            if (this.SponsorLogoRadioButtonList.SelectedItem != null)
            {
                this.SponsorLogoRadioButtonList.ClearSelection();
            }

            //remove
            this.SponsorLogoRadioButtonList.Items.RemoveAt(0);

            //set upload new selected
            this.SponsorLogoRadioButtonList.Items.FindByValue("1").Selected = true;
            this.SponsorLogoImage.Visible = false;
            this.SponsorLogoFileUploadPanel.Visible = true;
        }

    }

    private void BindDeveloperLogo()
    {
        string logoImageURL = "";


        //check fedora
        if (!this.IsNew && this.FedoraContentObject != null && !string.IsNullOrEmpty(this.FedoraContentObject.DeveloperLogoImageFileName))
        {
            try
            {

                logoImageURL = String.Format("~/Public/Serve.ashx?pid={0}&mode=GetDeveloperLogo", this.FedoraContentObject.PID);
            }
            catch
            {
            }

            if (!string.IsNullOrEmpty(logoImageURL))
            {
                this.DeveloperLogoImage.ImageUrl = logoImageURL.Trim();
                return;
            }
        }
        //get from profile
        if (Context.User.Identity.IsAuthenticated)
        {
            UserProfile p = null;


            try
            {
                p = UserProfileDB.GetUserProfileByUserName(Context.User.Identity.Name);
            }
            catch
            {


            }

            //check profile if authenticated
            if (Context.User.Identity.IsAuthenticated && p != null)
            {
                if (p.DeveloperLogo != null && !string.IsNullOrEmpty(p.DeveloperLogoContentType))
                {

                    logoImageURL = Website.Pages.Types.FormatProfileImageHandler(p.UserID.ToString(), "Developer");

                    if (!string.IsNullOrEmpty(logoImageURL))
                    {
                        this.DeveloperLogoImage.ImageUrl = logoImageURL.Trim();
                        return;
                    }

                }

            }



        }


        //set selected
        //remove use current from radiobuttonlist, show file upload
        if (string.IsNullOrEmpty(logoImageURL))
        {
            //clear selection
            if (this.DeveloperLogoRadioButtonList.SelectedItem != null)
            {
                this.DeveloperLogoRadioButtonList.ClearSelection();
            }

            //remove
            this.DeveloperLogoRadioButtonList.Items.RemoveAt(0);

            //set upload new selected
            this.DeveloperLogoRadioButtonList.Items.FindByValue("1").Selected = true;
            this.DeveloperLogoImage.Visible = false;
            this.DeveloperLogoFileUploadPanel.Visible = true;
        }


    }

    private void UploadDeveloperLogo(vwarDAL.IDataRepository dal, ContentObject co)
    {
        if (this.DeveloperLogoRadioButtonList.SelectedItem != null)
        {
            switch (this.DeveloperLogoRadioButtonList.SelectedValue.Trim())
            {
                case "0": //use current

                    //if use current and there's an empty string then use the profile logo
                    if (string.IsNullOrEmpty(co.DeveloperLogoImageFileName))
                    {
                        //use the profile image
                        DataTable dt = UserProfileDB.GetUserProfileDeveloperLogoByUserName(Context.User.Identity.Name);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            DataRow dr = dt.Rows[0];

                            if (dr["Logo"] != System.DBNull.Value && dr["LogoContentType"] != System.DBNull.Value && !string.IsNullOrEmpty(dr["LogoContentType"].ToString()))
                            {
                                var data = (byte[])dr["Logo"];
                                using (MemoryStream s = new MemoryStream())
                                {
                                    s.Write(data, 0, data.Length);
                                    s.Position = 0;

                                    //filename
                                    co.DeveloperLogoImageFileName = "developer.jpg"; ;


                                    if (!string.IsNullOrEmpty(dr["FileName"].ToString()))
                                    {
                                        co.DeveloperLogoImageFileName = dr["FileName"].ToString();
                                    }

                                    //upload the file
                                    FedoraContentObject.DeveloperLogoImageFileNameId = dal.SetContentFile(s, co, co.DeveloperLogoImageFileName);
                                }
                            }
                        }



                    }
                    break;

                case "1": //Upload logo
                    if (this.DeveloperLogoFileUpload.FileContent.Length > 0 && !string.IsNullOrEmpty(this.DeveloperLogoFileUpload.FileName))
                    {
                        co.DeveloperLogoImageFileName = this.DeveloperLogoFileUpload.FileName;
                        co.DeveloperLogoImageFileNameId = dal.SetContentFile(this.DeveloperLogoFileUpload.FileContent, co, this.DeveloperLogoFileUpload.FileName);
                    }
                    break;

                case "2": //none                       
                    break;
            }

        }





    }

    private void UploadSponsorLogo(vwarDAL.IDataRepository dal, ContentObject co)
    {

        if (this.SponsorLogoRadioButtonList.SelectedItem != null)
        {
            switch (this.SponsorLogoRadioButtonList.SelectedValue.Trim())
            {
                case "0": //use profile logo

                    //use profile logo if use current and there's an empty file name otherwise don't change
                    if (string.IsNullOrEmpty(co.SponsorLogoImageFileName))
                    {

                        DataTable dt = UserProfileDB.GetUserProfileSponsorLogoByUserName(Context.User.Identity.Name);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            DataRow dr = dt.Rows[0];

                            if (dr["Logo"] != System.DBNull.Value && dr["LogoContentType"] != System.DBNull.Value && !string.IsNullOrEmpty(dr["LogoContentType"].ToString()))
                            {
                                var data = (byte[])dr["Logo"];
                                using (MemoryStream s = new MemoryStream())
                                {
                                    s.Write(data, 0, data.Length);
                                    s.Position = 0;

                                    //filename
                                    co.SponsorLogoImageFileName = "sponsor.jpg";


                                    if (!string.IsNullOrEmpty(dr["FileName"].ToString()))
                                    {
                                        co.SponsorLogoImageFileName = dr["FileName"].ToString();
                                    }
                                    FedoraContentObject.SponsorLogoImageFileNameId =
                                    dal.SetContentFile(s, co, co.SponsorLogoImageFileName);
                                }
                            }


                        }


                    }



                    break;

                case "1": //Upload logo
                    if (this.SponsorLogoFileUpload.FileContent.Length > 0 && !string.IsNullOrEmpty(this.SponsorLogoFileUpload.FileName))
                    {
                        co.SponsorLogoImageFileName = this.SponsorLogoFileUpload.FileName;
                        co.DeveloperLogoImageFileNameId = dal.SetContentFile(this.SponsorLogoFileUpload.FileContent, co, this.SponsorLogoFileUpload.FileName);
                    }

                    break;

                case "2": //none
                    break;
            }

        }



    }

}