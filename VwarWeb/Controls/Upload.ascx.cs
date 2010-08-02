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
using Telerik.Web.UI;

public partial class Controls_Upload : System.Web.UI.UserControl
{
    //TODO: remove - testing only
   
    private bool containsValidTextureFile = true;
    private Utility_3D.ConvertedModel mModel;
    public void SetModel(Utility_3D.ConvertedModel inModel)
    {
        mModel = inModel;
        Context.Session["Model"] = inModel;
    }
    public Utility_3D.ConvertedModel GetModel()
    {
        mModel = (Utility_3D.ConvertedModel)Context.Session["Model"];
        return mModel;
    }
    protected bool IsNew
    {
        get
        {
            bool rv = string.IsNullOrEmpty(this.ContentObjectID);

            return rv;
        }

    }

    private bool IsModelUpload
    {
        get
        {
            return ddlAssetType.SelectedValue.Equals("Model", StringComparison.InvariantCultureIgnoreCase);
        }
    }

    protected string ContentObjectID
    {
        get
        {
            string rv = "";
            if (Request.QueryString["ContentObjectID"] != null)
            {
                rv = Request.QueryString["ContentObjectID"].Trim();
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
       
        //RadAjaxManager manager = RadAjaxManager.GetCurrent(this.Page);

        //manager.AjaxSettings.AddAjaxSetting(manager, this.ThumbnailFileImage);
        //manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate(manager_AjaxRequest);

      
       
        if (this.Page.Master.FindControl("SearchPanel") != null)
        {
            //hide the search panel
            this.Page.Master.FindControl("SearchPanel").Visible = false;
        }

        //redirect if user is not authenticated
        if (!Context.User.Identity.IsAuthenticated)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Page.IsPostBack)
        {
           
        
            this.MultiView1.ActiveViewIndex = 0;
            this.BindCCLHyperLink();
            this.BindContentObject();



        }



    }

    //protected void manager_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
    //{
    //    switch (e.Argument)
    //    {
    //        case "BindImage":
    //            this.BindImage();

    //            break;
    //        case "RemoveImage":
    //            this.RemoveImage();
    //            break;
    //    }


    //}

    
    //protected void BindImage()
    //{
    //    if (this.ThumbnailFileUpload.UploadedFiles.Count > 0)
    //    {   
    //        ThumbnailFileImage.Width = Unit.Pixel(200);
    //        ThumbnailFileImage.Height = Unit.Pixel(200);
    //        int length = (int)this.ThumbnailFileUpload.UploadedFiles[0].InputStream.Length;
    //        byte[] imageData = new byte[length];

    //        using (Stream stream = this.ThumbnailFileUpload.UploadedFiles[0].InputStream)
    //        {
    //            stream.Read(imageData, 0, length);
    //        }

    //        ThumbnailFileImage.DataValue = imageData;
    //        ThumbnailFileImage.Visible = true;
    //    }

    //}

    //protected void RemoveImage()
    //{
    //    byte[] imageData = new byte[0];
    //    this.ThumbnailFileImage.DataValue = imageData;
    //    this.ThumbnailFileImage.Visible = false;
    //}

    //protected void ThumbnailFileUpload_FileUploaded(object sender, Telerik.Web.UI.FileUploadedEventArgs e)
    //{
    //   // UploadedFile f = e.File;

        


    //}


    private void BindContentObject()
    {

        if (!this.IsNew)
        {
            //update

            //current
            var factory = new vwarDAL.DataAccessFactory();
            vwarDAL.IDataRepository vd = factory.CreateDataRepositorProxy();
            var co = vd.GetContentObjectById(this.ContentObjectID, false);

            if (co != null)
            {
                //TODO: Uncomment
                //remove the required field validators for model and thumbnail = false
               // this.ContentFileUploadRequiredFieldValidator.Enabled = false;

                //TODO: Need to add required field validator back in
                //this.ThumbnailFileUploadRequiredFieldValidator.Enabled = false;


                //redirect if the user is not the owner
                if (!co.SubmitterEmail.Equals(Context.User.Identity.Name, StringComparison.InvariantCultureIgnoreCase) & !Website.Security.IsAdministrator())
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }

                //Asset Type
                if (!string.IsNullOrEmpty(co.AssetType))
                {
                    //set selected
                    if (this.ddlAssetType.SelectedItem != null)
                    {
                        this.ddlAssetType.ClearSelection();
                    }

                    this.ddlAssetType.Items.FindItemByValue(co.AssetType.Trim()).Selected = true;

                }


                //Title
                if (!string.IsNullOrEmpty(co.Title))
                {
                    this.TitleTextBox.Text = co.Title.Trim();
                }

                //Developer name
                if (!string.IsNullOrEmpty(co.DeveloperName))
                {
                    this.DeveloperNameTextBox.Text = co.DeveloperName.Trim();
                }


                //Bind Developer Logo / Sponsor Logo
                UserProfile p = null;

                if (Context.User.Identity.IsAuthenticated)
                {
                    try
                    {
                        p = UserProfileDB.GetUserProfileByUserName(Context.User.Identity.Name);
                    }
                    catch
                    {


                    }
                }

                //Developer Logo
                this.BindDeveloperLogo(co, p);

                //sponsor logo
                this.BindSponsorLogo(co, p);


                //Sponsor Name
                if (!string.IsNullOrEmpty(co.SponsorName))
                {
                    this.SponsorNameTextBox.Text = co.SponsorName.Trim();
                }

                //Artist Name
                if (!string.IsNullOrEmpty(co.ArtistName))
                {
                    this.ArtistNameTextBox.Text = co.ArtistName.Trim();
                }


                //CC License
                if (!string.IsNullOrEmpty(co.CreativeCommonsLicenseURL))
                {


                    //set selected
                    if (this.CCLicenseDropDownList.Items.FindItemByValue(co.CreativeCommonsLicenseURL) != null)
                    {
                        //clear selection
                        if (this.CCLicenseDropDownList.SelectedItem != null)
                        {
                            this.CCLicenseDropDownList.ClearSelection();
                        }

                        this.CCLicenseDropDownList.Items.FindItemByValue(co.CreativeCommonsLicenseURL).Selected = true;
                    }

                    //set the hyperlink
                    this.CCLHyperLink.NavigateUrl = co.CreativeCommonsLicenseURL.Trim();
                }
                else
                {
                    //set none selected
                    if (this.CCLicenseDropDownList.SelectedItem != null)
                    {
                        this.CCLicenseDropDownList.ClearSelection();
                    }

                    this.CCLicenseDropDownList.Items.FindItemByValue("None").Selected = true;


                }



                //Description
                if (!string.IsNullOrEmpty(co.Description))
                {
                    this.DescriptionTextBox.Text = co.Description.Trim();

                }

                //More Information
                if (!string.IsNullOrEmpty(co.MoreInformationURL))
                {
                    this.MoreInformationURLTextBox.Text = co.MoreInformationURL.Trim();

                }

                //Keywords
                if (!string.IsNullOrEmpty(co.Keywords))
                {
                    this.KeywordsTextBox.Text = co.Keywords.Trim();
                }


                //Unit Scale
                if (!string.IsNullOrEmpty(co.UnitScale))
                {
                    this.UnitScaleTextBox.Text = co.UnitScale.Trim();
                }

                //Up Axis
                if (!string.IsNullOrEmpty(co.UpAxis))
                {
                    this.UpAxisRadioButtonList.ClearSelection();

                    if (this.UpAxisRadioButtonList.Items.FindByText(co.UpAxis) != null)
                    {
                        this.UpAxisRadioButtonList.Items.FindByText(co.UpAxis).Selected = true;
                    }


                }

                //NumPolygons
                if (!string.IsNullOrEmpty(co.NumPolygons.ToString()))
                {
                    this.NumPolygonsTextBox.Text = co.NumPolygons.ToString();
                }

                //NumTextures
                if (!string.IsNullOrEmpty(co.NumTextures.ToString()))
                {
                    this.NumTexturesTextBox.Text = co.NumTextures.ToString();
                }

                //UV Coordinate Channel
                if (!string.IsNullOrEmpty(co.UVCoordinateChannel))
                {
                    this.UVCoordinateChannelTextBox.Text = co.UVCoordinateChannel.Trim();
                }

                //Intention of Texture
                if (!string.IsNullOrEmpty(co.IntentionOfTexture))
                {
                    this.IntentionofTextureTextBox.Text = co.IntentionOfTexture.Trim();
                }


            }
            else
            {



                //Show error message
                this.errorMessage.Text = "Model not found.";

            }




        }
        else
        {

            //new
            //TODO: Uncomment
            //this.ContentFileUploadRequiredFieldValidator.Enabled = true;
            //this.ThumbnailFileUploadRequiredFieldValidator.Enabled = true;



            UserProfile p = null;

            if (Context.User.Identity.IsAuthenticated)
            {
                try
                {
                    p = UserProfileDB.GetUserProfileByUserName(Context.User.Identity.Name);
                }
                catch
                {


                }
            }



            //Developer Logo
            this.BindDeveloperLogo(null, p);

            //sponsor logo
            this.BindSponsorLogo(null, p);

        }





    }

    protected void Step1NextButton_Click(object sender, EventArgs e)
    {



        Stream data = ContentFileUpload.FileContent;
        Utility_3D.Model_Packager pack = new Utility_3D.Model_Packager();
        Utility_3D _3d = new Utility_3D();
        _3d.Initialize(Website.Config.ConversionLibarayLocation);


        SetModel(pack.Convert(ContentFileUpload.FileContent, ContentFileUpload.FileName));


        //SetModelDisplay();
        var factory = new vwarDAL.DataAccessFactory();
        vwarDAL.IDataRepository dal = factory.CreateDataRepositorProxy();
        ContentObject contentObj = null;

        if (!this.IsNew)
        {
            contentObj = dal.GetContentObjectById(ContentObjectID, false);

            if (contentObj == null)
            {
                //show error message
                this.errorMessage.Text = "Model not found.";
                this.MultiView1.SetActiveView(this.DefaultView);
                return;
            }






        }
        else
        {
            contentObj = new vwarDAL.ContentObject();

        }

        contentObj.AssetType = this.ddlAssetType.SelectedValue;

        //required fields
        contentObj.Title = this.TitleTextBox.Text.Trim();

        if (!string.IsNullOrEmpty(this.ContentFileUpload.FileName))
        {
            contentObj.Location = Path.GetFileNameWithoutExtension(this.ContentFileUpload.FileName) + ".zip";
        }


        contentObj.ScreenShot = this.ThumbnailFileUpload.FileName.Trim();
        //contentObj.ScreenShot = this.ThumbnailFileUpload.UploadedFiles[0].FileName;
       



        //optional fields

        //developer name
        if (!string.IsNullOrEmpty(this.DeveloperNameTextBox.Text.Trim()))
        {
            contentObj.DeveloperName = this.DeveloperNameTextBox.Text.Trim();
        }

        //sponsor name
        if (!string.IsNullOrEmpty(this.SponsorNameTextBox.Text.Trim()))
        {
            contentObj.SponsorName = this.SponsorNameTextBox.Text.Trim();
        }

        //artist name
        if (!string.IsNullOrEmpty(this.ArtistNameTextBox.Text.Trim()))
        {
            contentObj.ArtistName = this.ArtistNameTextBox.Text.Trim();
        }

        //format
        if (!string.IsNullOrEmpty(this.FormatTextBox.Text.Trim()))
        {
            contentObj.Format = this.FormatTextBox.Text.Trim();
        }


        //creative commons license url
        if (this.CCLicenseDropDownList.SelectedItem != null && this.CCLicenseDropDownList.SelectedValue != "None")
        {
            contentObj.CreativeCommonsLicenseURL = this.CCLicenseDropDownList.SelectedValue.Trim();

        }

        //description
        if (!string.IsNullOrEmpty(this.DescriptionTextBox.Text.Trim()))
        {
            contentObj.Description = this.DescriptionTextBox.Text.Trim();
        }

        //more information url
        if (!string.IsNullOrEmpty(this.MoreInformationURLTextBox.Text.Trim()))
        {
            contentObj.MoreInformationURL = this.MoreInformationURLTextBox.Text.Trim();

        }

        //keywords
        if (!string.IsNullOrEmpty(this.KeywordsTextBox.Text.Trim()))
        {
            string words = "";
            foreach (ListItem li in this.KeywordsListBox.Items)
            {
                int count = 1;


                if (count > 1)
                {
                    words += "," + li.Text.Trim();
                }
                else
                {
                    words = li.Text;
                }


                count++;
            }


            contentObj.Keywords = words;

        }



        if (!this.IsNew)
        {
            //update
            contentObj.LastModified = DateTime.Now;
            contentObj.LastViewed = DateTime.Now;
            UpdateContentObject(dal, contentObj, GetModel());

        }
        else
        {
            //insert
            contentObj.UploadedDate = DateTime.Now;
            contentObj.LastModified = DateTime.Now;
            contentObj.Views = 0;
            contentObj.SubmitterEmail = Context.User.Identity.Name.Trim();            
            SaveNewContentObject(dal, contentObj, GetModel());
        }

        PopulateValidationViewMetadata(GetModel(), this.ValidationView);

        if (GetModel().missingTextures.Count() == 0)
        {
            this.MultiView1.SetActiveView(this.ValidationView);

        }
        else
        {
            GenerateMissingTextureUI(GetModel());
            this.MultiView1.SetActiveView(this.MissingTextureView);


        }

    }

    protected void PopulateValidationViewMetadata(Utility_3D.ConvertedModel model, View view)
    {
        ((TextBox)(view.FindControl("UnitScaleTextBox"))).Text = model._ModelData.TransformProperties.UnitMeters.ToString();
        if (model._ModelData.TransformProperties.UpAxis == "Y")
            ((RadioButtonList)(view.FindControl("UpAxisRadioButtonList"))).SelectedIndex = 0;
        if (model._ModelData.TransformProperties.UpAxis == "Z")
            ((RadioButtonList)(view.FindControl("UpAxisRadioButtonList"))).SelectedIndex = 1;
        ((TextBox)(view.FindControl("NumPolygonsTextBox"))).Text = model._ModelData.VertexCount.Polys.ToString();
        ((TextBox)(view.FindControl("NumTexturesTextBox"))).Text = model._ModelData.ReferencedTextures.Length.ToString();
        ((TextBox)(view.FindControl("UVCoordinateChannelTextBox"))).Text = "1";


    }

    protected void ValidationViewSubmitButton_Click(object sender, EventArgs e)
    {
        var factory = new vwarDAL.DataAccessFactory();
        vwarDAL.IDataRepository dal = factory.CreateDataRepositorProxy();

        ContentObject contentObj = dal.GetContentObjectById(ContentObjectID, false);


        contentObj.UnitScale = this.UnitScaleTextBox.Text.Trim();


        contentObj.UpAxis = this.UpAxisRadioButtonList.SelectedValue.Trim();


        contentObj.IntentionOfTexture = this.IntentionofTextureTextBox.Text.Trim();



        //polygons
        if (!string.IsNullOrEmpty(this.NumPolygonsTextBox.Text.Trim()))
        {

            try
            {
                contentObj.NumPolygons = Int32.Parse(this.NumPolygonsTextBox.Text.Trim());
            }
            catch
            {


            }


        }

        //textures
        if (!string.IsNullOrEmpty(this.NumTexturesTextBox.Text.Trim()))
        {

            try
            {
                contentObj.NumTextures = Int32.Parse(this.NumTexturesTextBox.Text.Trim());
            }
            catch
            {


            }


        }



        //update
        dal.UpdateContentObject(contentObj);

        //redirect
        Response.Redirect(Website.Pages.Types.Default);



    }

    private void UpdateContentObject(vwarDAL.IDataRepository dal, ContentObject co, Utility_3D.ConvertedModel model)
    {

        HandleFileUploads(dal, co, model);
    }

    private void SaveNewContentObject(vwarDAL.IDataRepository dal, ContentObject co, Utility_3D.ConvertedModel model)
    {

        HandleFileUploads(dal, co, model);

    }
    private void HandleFileUploads(vwarDAL.IDataRepository dal, ContentObject co, Utility_3D.ConvertedModel model)
    {
        try
        {

            //upload main content file

            if (IsModelUpload)
            {

                var path = ExtractFile(GetModel().data, Path.GetFileNameWithoutExtension(ContentFileUpload.FileName));
                foreach (var file in Directory.GetFiles(path, "*.dae"))
                {
                    co.DisplayFile = ConvertFileToO3D(file);
                    break;
                }
            }




            //insert model
            if (IsNew)
            {
                dal.InsertContentObject(co);
            }
            if (IsModelUpload)
            {
                var displayFilePath = co.DisplayFile;
                co.DisplayFile = Path.GetFileName(co.DisplayFile);
                dal.UploadFile(displayFilePath, co.PID, co.DisplayFile);

            }
            //upload images - required fields

            //The converter will have made whatever the uploaded file was a Zip file
            dal.UploadFile(model.data, co.PID, Path.GetFileNameWithoutExtension(this.ContentFileUpload.FileName) + ".zip");
            if (IsModelUpload)
            {
                //TODO: Uncomment
                dal.UploadFile(this.ThumbnailFileUpload.FileContent, co.PID, this.ThumbnailFileUpload.FileName);
                
                                

            }
            const string proxyTemplate = "~/Public/Model.ashx?pid={0}&file={1}";

            //TODO: Uncomment
           dal.UploadFile(this.ThumbnailFileUpload.FileContent, co.PID, this.ThumbnailFileUpload.FileName);

            //upload developer logo - optional
            if (this.DeveloperLogoRadioButtonList.SelectedItem != null)
            {
                switch (this.DeveloperLogoRadioButtonList.SelectedValue.Trim())
                {
                    case "0": //use profile logo
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
                                    dal.UploadFile(s, co.PID, co.DeveloperLogoImageFileName);
                                }
                            }
                        }



                        break;

                    case "1": //Upload logo

                        if (this.DeveloperLogoFileUpload.FileContent.Length > 0 && !string.IsNullOrEmpty(this.DeveloperLogoFileUpload.FileName))
                        {
                            co.DeveloperLogoImageFileName = this.DeveloperLogoFileUpload.FileName;
                            dal.UploadFile(this.DeveloperLogoFileUpload.FileContent, co.PID, this.DeveloperLogoFileUpload.FileName);
                        }


                        break;

                    case "2": //none                       

                        break;
                }

            }


            //upload sponsor logo

            if (this.SponsorLogoRadioButtonList.SelectedItem != null)
            {
                switch (this.SponsorLogoRadioButtonList.SelectedValue.Trim())
                {
                    case "0": //use profile logo
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

                                    dal.UploadFile(s, co.PID, co.SponsorLogoImageFileName);
                                }
                            }


                        }


                        break;

                    case "1": //Upload logo
                        if (this.SponsorLogoFileUpload.FileContent.Length > 0 && !string.IsNullOrEmpty(this.SponsorLogoFileUpload.FileName))
                        {
                            co.SponsorLogoImageFileName = this.SponsorLogoFileUpload.FileName;
                            dal.UploadFile(this.SponsorLogoFileUpload.FileContent, co.PID, this.SponsorLogoFileUpload.FileName);
                        }

                        break;

                    case "2": //none
                        break;
                }

            }


            //save ID to redirect to model view after confirmation
            ContentObjectID = co.PID;

            //update object
            dal.UpdateContentObject(co);



        }
        catch (ArgumentException ex)
        {
            errorMessage.Text = ex.Message;
        }
    }
    private string SaveFile(Stream stream, string fileName)
    {
        string savePath = Path.Combine(Path.GetTempPath(), fileName);
        if (Directory.Exists(savePath)) return savePath;
        byte[] data = new byte[stream.Length];
        stream.Seek(0, SeekOrigin.Begin);
        stream.Read(data, 0, data.Length);
        using (FileStream fstream = new FileStream(savePath, FileMode.Create))
        {
            fstream.Write(data, 0, data.Length);
        }
        return savePath;
    }

    private string ConvertFileToO3D(string path)
    {
        var application = Path.Combine(Path.Combine(Request.PhysicalApplicationPath, "bin"), "o3dConverter.exe");
        System.Diagnostics.ProcessStartInfo processInfo = new System.Diagnostics.ProcessStartInfo(application);
        processInfo.Arguments = String.Format("\"{0}\" \"{1}\"", path, path.ToLower().Replace("dae", "o3d"));
        processInfo.WindowStyle = ProcessWindowStyle.Hidden;
        processInfo.RedirectStandardError = true;
        processInfo.CreateNoWindow = true;
        processInfo.UseShellExecute = false;
        var p = Process.Start(processInfo);
        var error = p.StandardError.ReadToEnd();
        return path.ToLower().Replace("dae", "o3d");
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
        HtmlGenericControl body = this.Page.Master.FindControl("bodyTag") as HtmlGenericControl;
        var uri = Request.Url;
        var url = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;
        url += "/Public/Model.ashx?Session=true";
        body.Attributes.Add("onLoad", "DoLoadURL('" + url + "');");
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

    protected void ddlAssetType_Changed(object sender, EventArgs e)
    {
        thumbNailArea.Visible = IsModelUpload;
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
        Utility_3D.ConvertedModel model = GetModel();
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
        SetModel(model);
        SetModelDisplay();
        //Get the DAL
        var factory = new vwarDAL.DataAccessFactory();
        vwarDAL.IDataRepository dal = factory.CreateDataRepositorProxy();

        //Get the content object for this model
        ContentObject contentObj = dal.GetContentObjectById(ContentObjectID, false);

        //upload the modified datastream to the dal
        dal.UpdateFile(model.data, contentObj.PID, contentObj.Location);

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
        Response.Redirect(Website.Pages.Types.Default);
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

    private void BindSponsorLogo(ContentObject co = null, UserProfile p = null)
    {
        string logoImageURL = "";


        //check fedora
        if (co != null && !string.IsNullOrEmpty(co.SponsorLogoImageFileName))
        {


            try
            {
                var factory = new vwarDAL.DataAccessFactory();
                vwarDAL.IDataRepository vd = factory.CreateDataRepositorProxy();
                logoImageURL = vd.GetContentUrl(co.PID, co.SponsorLogoImageFileName).Trim();
            }
            catch
            {


            }

            if (!string.IsNullOrEmpty(logoImageURL))
            {
                this.SponsorLogoImage.ImageUrl = logoImageURL.Trim();
                return;

            }

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

    private void BindDeveloperLogo(ContentObject co = null, UserProfile p = null)
    {

        string logoImageURL = "";


        //check fedora
        if (co != null && !string.IsNullOrEmpty(co.DeveloperLogoImageFileName))
        {


            try
            {
                var factory = new vwarDAL.DataAccessFactory();
                vwarDAL.IDataRepository vd = factory.CreateDataRepositorProxy();
                logoImageURL = vd.GetContentUrl(co.PID, co.DeveloperLogoImageFileName).Trim();
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
                    //use current logo
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
    protected void UnitScaleTextBox_TextChanged(object sender, EventArgs e)
    {

    }

    protected void AddKeywordButton_Click(object sender, EventArgs e)
    {
        //NOTE: actually a combobox
        if (!string.IsNullOrEmpty(this.KeywordsTextBox.Text))
        {

            if (this.KeywordsListBox.Items.FindByText(this.KeywordsTextBox.Text.Trim()) == null)
            {
                this.KeywordsListBox.Items.Add(this.KeywordsTextBox.Text.Trim());

            }


        }
    }

    protected void RemoveKeywordsButton_Click(object sender, EventArgs e)
    {
        List<ListItem> selectedItems = new List<ListItem>();

        //get all selected items add to collection
        foreach (ListItem li in this.KeywordsListBox.Items)
        {
            //add to new collection
            if (li.Selected)
            {
                selectedItems.Add(li);
            }
        }

        //remove selected items
        foreach (ListItem li2 in selectedItems)
        {
            this.KeywordsListBox.Items.Remove(li2);
        }

    }


    //protected void ThumbnailFileUploadCustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
    //{
    //    args.IsValid = this.ThumbnailFileUpload.UploadedFiles.Count > 0;
    //} 

}