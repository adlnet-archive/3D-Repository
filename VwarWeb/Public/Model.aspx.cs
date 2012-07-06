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
using System.Net;
using AjaxControlToolkit;
using vwarDAL;
using System.Web.Script.Serialization;
using LR;
using System.Web.Hosting;
public partial class Public_Model : Website.Pages.PageBase
{
    private const string VIOLATION_REPORT_SUCCESS = "A message has been sent to the site administator concerning this content.";
    private const string VIOLATION_REPORT_UNAUTHENTICATED = "You must be logged into 3DR to report an offensive content/license violation.";
    private const string VIOLATION_REPORT_EMAIL_ERROR = "An error occurred when trying to notify the administrator. Please try again later.";

    protected string ContentObjectID
    {
        get
        {
            string rv = "";
            if (Request.QueryString["ContentObjectID"] != null)
            {
                rv = Server.UrlDecode(Request.QueryString["ContentObjectID"].Trim());

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
        if (!Page.IsPostBack)
        {
            // CreateViewOptionTabs();
            this.BindModelDetails();
            if (!Context.User.Identity.IsAuthenticated)
            {
                Control prnt = PermissionsManagementControl.Parent;
                prnt.Controls.Remove(PermissionsManagementControl);
                prnt = IDRow.Parent;
                prnt.Controls.Remove(IDRow);
            }
        }
    }

    protected void Page_PreRender()
    {
        if (Context.User.Identity.IsAuthenticated)
        {
            AuthenticatedReviewSubmission.Style["display"] = "block";
        }
        else
        {
            UnauthenticatedReviewSubmission.Style["display"] = "block";
        }
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string ReportViolation(string pid, string title, string description)
    {
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            try
            {
                Website.Mail.SendReportViolationEmail(pid, title, description, HttpContext.Current.User.Identity.Name);
                return VIOLATION_REPORT_SUCCESS;
            }
            catch
            {
                return VIOLATION_REPORT_EMAIL_ERROR;
            }

        }
        else
        {
            return VIOLATION_REPORT_UNAUTHENTICATED;
        }
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string DeleteModel(string pid)
    {
        pid = HttpContext.Current.Server.UrlDecode(pid);
        string response = "0";
        var factory = new DataAccessFactory();
        IDataRepository dal = factory.CreateDataRepositorProxy();
        ContentObject co = dal.GetContentObjectById(pid, false);
        if (co != null &&
             HttpContext.Current.User.Identity.IsAuthenticated &&
             (co.SubmitterEmail.Equals(HttpContext.Current.User.Identity.Name, StringComparison.InvariantCultureIgnoreCase) ||
                 Website.Security.IsAdministrator()))
        {
            try
            {
                dal.DeleteContentObject(co);
                response = "1";
            }
            catch { }
        }
        else if (!HttpContext.Current.User.Identity.IsAuthenticated)
        {
            HttpContext.Current.Response.StatusCode = 403;
        }
        dal.Dispose();
        return response;
    }

    protected void AddHeaderTag(string type, string name, string description)
    {
        if (type == "meta")
        {
            System.Web.UI.HtmlControls.HtmlMeta newTag = new HtmlMeta();
            newTag.Name = name;
            newTag.Content = description;
            Page.Header.Controls.Add(newTag);
        }
        else if (type == "link")
        {
            System.Web.UI.HtmlControls.HtmlLink newLink = new HtmlLink();
            newLink.Attributes.Add("rel", name);
            newLink.Attributes.Add("href", description);
            Page.Header.Controls.Add(newLink);
        }
    }

    private void BindModelDetails()
    {

       

        if (String.IsNullOrEmpty(ContentObjectID))
        {
            Response.Redirect("~/Default.aspx");
        }       
        PermissionsManager prm = new PermissionsManager();



        ModelPermissionLevel Permission = prm.GetPermissionLevel(Context.User.Identity.Name, ContentObjectID);
        prm.Dispose();
        prm = null;
        if (Permission < ModelPermissionLevel.Searchable)
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
       
       
        var uri = Request.Url;
        //string proxyTemplate = "Model.ashx?pid={0}&file={1}&fileid={2}";

        vwarDAL.IDataRepository vd = (new vwarDAL.DataAccessFactory()).CreateDataRepositorProxy();
        vwarDAL.ContentObject co = vd.GetContentObjectById(ContentObjectID, !IsPostBack, true);
        vd.Dispose();
        vd = null;
        //model screenshot
        if (co != null)
        {
            if (LR_3DR_Bridge.LR_Integration_Enabled())
                LR_3DR_Bridge.ModelViewed(co);
            DownloadButton.Enabled = Permission >= ModelPermissionLevel.Fetchable;
            
            DownloadButton.Visible = Permission >= ModelPermissionLevel.Fetchable;
            if ("Model".Equals(co.AssetType, StringComparison.InvariantCultureIgnoreCase) || true)
            {
                //if the content object file is null, dont' try to display
                if (co.DisplayFile != string.Empty && co.Location != string.Empty && Permission > ModelPermissionLevel.Searchable)
                {
                    Page.ClientScript.RegisterClientScriptBlock(GetType(), "vload", string.Format("vLoader = new ViewerLoader('{0}', '{1}', '{2}', '{3}', {4});", Page.ResolveClientUrl("~/Public/Serve.ashx?mode=PreviewModel"),
                                                                                                           (co.UpAxis != null) ? co.UpAxis : "",
                                                                                                           (co.UnitScale != null) ? co.UnitScale : "", co.NumPolygons, "\"" + co.PID.Replace(':','_') + "\""), true);

                    BodyTag.Attributes["onunload"] += "vLoader.DestroyViewer();";

                }
                if (String.IsNullOrWhiteSpace(co.ScreenShot) && String.IsNullOrWhiteSpace(co.ScreenShotId))
                {
                    ScreenshotImage.ImageUrl = Page.ResolveUrl("~/styles/images/nopreview_icon.png");
                }
                else
                {
                    ScreenshotImage.ImageUrl = String.Format("Serve.ashx?pid={0}&mode=GetScreenshot", co.PID);
                }
                AddHeaderTag("link", "og:image", ScreenshotImage.ImageUrl);
            }
            else if ("Texture".Equals(co.AssetType, StringComparison.InvariantCultureIgnoreCase))
            {
                ScreenshotImage.ImageUrl = String.Format("Serve.ashx?pid={0}&mode=GetScreenshot", co.PID, co.Location);
            }

            IDLabel.Text = co.PID;
            TitleLabel.Text = co.Title;
            AddHeaderTag("meta", "og:title", co.Title);
            //show hide edit link
            if (Permission >= ModelPermissionLevel.Fetchable)
            {

                if (Website.Security.IsAdministrator() || Permission >= ModelPermissionLevel.Editable)
                {
                    editLink.Visible = true;
                    PermissionsLink.Visible = true;
                    DeleteLink.Visible = true;
                    //editLink.NavigateUrl = "~/Users/Edit.aspx?ContentObjectID=" + co.PID;
                }
                
                //show and hide requires resubmit checkbox
                if (co.RequireResubmit)
                {
                    RequiresResubmitCheckbox.Visible = true;
                    RequiresResubmitCheckbox.Enabled = true;
                    RequiresResubmitLabel.Visible = true;
                }
                submitRating.Visible = true;
            }
            else
            {
                string returnUrlParam = "?ReturnUrl=" + Page.ResolveUrl("~/Public/Model.aspx?ContentObjectID=" + co.PID);
                LoginLink.NavigateUrl += returnUrlParam;
                ReveiwLoginHyperLink.NavigateUrl += returnUrlParam;
                LoginToDlLabel.Visible = true;
                submitRating.Visible = false;
            }

            //rating
            int rating = Website.Common.CalculateAverageRating(co.Reviews);
            ir.CurrentRating = rating;
            this.NotRatedLabel.Visible = (rating == 0);

            //description
            DescriptionLabel.Text = String.IsNullOrEmpty(co.Description) ? "No description available." : co.Description;
            AddHeaderTag("meta", "og:description", co.Description);
            upAxis.Value = co.UpAxis;
            unitScale.Value = co.UnitScale;
            //keywords
            var keywordsList = string.IsNullOrEmpty(co.Keywords) ? new String[0] : co.Keywords.Split(new char[] { ',' });
            foreach (var keyword in keywordsList)
            {
                HyperLink link = new HyperLink()
                {
                    Text = keyword,
                    NavigateUrl = "~/Public/Results.aspx?ContentObjectID=" + ContentObjectID + "&Keywords=" + Server.UrlEncode(keyword.Trim()),
                    CssClass = "Hyperlink"
                };
                keywords.Controls.Add(link);
                keywords.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            }
            

            //more details
            this.MoreDetailsHyperLink.NavigateUrl = co.MoreInformationURL;
            this.MoreDetailsHyperLink.Text = co.MoreInformationURL;
            

            string submitterFullName = Website.Common.GetFullUserName(co.SubmitterEmail);
            if (co.UploadedDate != null)
            {
                UploadedDateLabel.Text = "Uploaded by: " + submitterFullName + " on " + co.UploadedDate.ToString();
            }

            
                //sponsor logo
                if (!string.IsNullOrEmpty(co.SponsorLogoImageFileName))
                {
                    this.SponsorLogoImage.ImageUrl = String.Format("Serve.ashx?pid={0}&mode=GetSponsorLogo", co.PID);
                }

                
                this.SponsorNameLabel.Text = co.SponsorName;
               
            

           
                //developr logo
                if (!string.IsNullOrEmpty(co.DeveloperLogoImageFileName))
                {
                    this.DeveloperLogoImage.ImageUrl = String.Format("Serve.ashx?pid={0}&mode=GetDeveloperLogo", co.PID);
                }


                this.DeveloperLogoRow.Visible = !string.IsNullOrEmpty(co.DeveloperLogoImageFileName);

                //developer name
                this.DeveloperNameHyperLink.NavigateUrl = "~/Public/Results.aspx?ContentObjectID=" + ContentObjectID + "&DeveloperName=" + Server.UrlEncode(co.DeveloperName);
                this.DeveloperNameHyperLink.Text = co.DeveloperName;

                if (String.IsNullOrEmpty(co.ArtistName))
                {
                    
                }
                else
                {
                    this.ArtistNameHyperLink.NavigateUrl = "~/Public/Results.aspx?ContentObjectID=" + ContentObjectID + "&ArtistName=" + Server.UrlEncode(co.ArtistName);
                    this.ArtistNameHyperLink.Text = co.ArtistName;
                }

                //this.DeveloperRow.Visible = !string.IsNullOrEmpty(co.DeveloperName);
           
            this.FormatLabel.Text = ((string.IsNullOrEmpty(co.Format)) ? "Unknown" : co.Format);

            //num polygons   
            this.NumPolygonsLabel.Text = co.NumPolygons.ToString();
           

            //num textures
            this.NumTexturesLabel.Text = co.NumTextures.ToString();
            

            //cclrow
            
            this.CCLHyperLink.NavigateUrl = co.CreativeCommonsLicenseURL;


            if (!string.IsNullOrEmpty(co.CreativeCommonsLicenseURL))
            {
                switch (co.CreativeCommonsLicenseURL.ToLower().Trim())
                {
                    case "http://creativecommons.org/licenses/by-nc-sa/3.0/legalcode":
                        this.CCLHyperLink.ImageUrl = "http://i.creativecommons.org/l/by-nc-sa/3.0/88x31.png";
                        this.CCLHyperLink.ToolTip = "by-nc-sa";
                        break;

                    case "http://creativecommons.org/licenses/by-nc-nd/3.0/legalcode":
                        this.CCLHyperLink.ImageUrl = "http://i.creativecommons.org/l/by-nc-nd/3.0/88x31.png";
                        this.CCLHyperLink.ToolTip = "by-nc-nd";
                        break;

                    case "http://creativecommons.org/licenses/by-nc/3.0/legalcode":
                        this.CCLHyperLink.ImageUrl = "http://i.creativecommons.org/l/by-nc/3.0/88x31.png";
                        this.CCLHyperLink.ToolTip = "by-nc";
                        break;

                    case "http://creativecommons.org/licenses/by-nd/3.0/legalcode":
                        this.CCLHyperLink.ImageUrl = "http://i.creativecommons.org/l/by-nd/3.0/88x31.png";
                        this.CCLHyperLink.ToolTip = "by-nd";
                        break;

                    case "http://creativecommons.org/licenses/by-sa/3.0/legalcode":
                        this.CCLHyperLink.ImageUrl = "http://i.creativecommons.org/l/by-sa/3.0/88x31.png";
                        this.CCLHyperLink.ToolTip = "by-sa";
                        break;
                    case "http://creativecommons.org/publicdomain/mark/1.0/":
                        this.CCLHyperLink.ImageUrl = "http://i.creativecommons.org/l/publicdomain/88x31.png";
                        this.CCLHyperLink.ToolTip = "Public Domain";
                        break;
                    case "http://creativecommons.org/licenses/by/3.0/legalcode":
                        this.CCLHyperLink.ImageUrl = "http://i.creativecommons.org/l/by/3.0/88x31.png";
                        this.CCLHyperLink.ToolTip = "by";
                        break;
                }

            }

            //downloads
            DownloadsLabel.Text = co.Downloads.ToString();
            this.DownloadsRow.Visible = !string.IsNullOrEmpty(co.Downloads.ToString());

            //views
            ViewsLabel.Text = co.Views.ToString();
            this.ViewsRow.Visible = !string.IsNullOrEmpty(co.Views.ToString());

            //download buton
            //this.DownloadButton.Visible = Context.User.Identity.IsAuthenticated;

            
            this.CommentsGridView.DataSource = co.Reviews;
            this.CommentsGridView.DataBind();
            
            //SupportingFileGrid.DataSource = co.SupportingFiles;
            //if(Permission < ModelPermissionLevel.Fetchable)
            //    ((ButtonField)SupportingFileGrid.Columns[2]).ImageUrl = "../styles/images/icons/expand_disabled.jpg";
            //SupportingFileGrid.DataBind();

            //SupportingFileGrid.Enabled = Permission >= ModelPermissionLevel.Fetchable;
            EditKeywords.Text = co.Keywords;
        }
    }

    private const string RATINGKEY = "rating";

    protected void Rating_Set(object sender, RatingEventArgs args)
    {
        ViewState[RATINGKEY] = args.Value;
    }
 
    protected void Rating_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(ratingText.Text))
        {
            vwarDAL.IDataRepository vd = (new vwarDAL.DataAccessFactory()).CreateDataRepositorProxy(); ;

            var ratingValue = rating.CurrentRating;
            vd.InsertReview(ratingValue, ratingText.Text.Length > 255 ? ratingText.Text.Substring(0, 255)
                : ratingText.Text, Context.User.Identity.Name, ContentObjectID);
            ViewState[RATINGKEY] = null;
            Response.Redirect(Request.RawUrl);

            ContentObject co = vd.GetContentObjectById(ContentObjectID,false);

            if (LR_3DR_Bridge.LR_Integration_Enabled())
                LR_3DR_Bridge.ModelRated(co);
            vd.Dispose();
            vd = null;
        }
    }

    protected void ContentObjectFormView_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "DownloadZip":
                Label IDLabel = (Label)FindControl("IDLabel");
                Label LocationLabel = (Label)FindControl("LocationLabel");
                vwarDAL.IDataRepository vd = (new vwarDAL.DataAccessFactory()).CreateDataRepositorProxy();;
                vd.IncrementDownloads(ContentObjectID);
                string filePath = Website.Common.FormatZipFilePath(IDLabel.Text.Trim(), LocationLabel.Text.Trim());
                string clientFileName = System.IO.Path.GetFileName(filePath);
                Website.Documents.ServeDocument(vd.GetContentFile(ContentObjectID, clientFileName), clientFileName);
                vd.Dispose();
                vd = null;
                break;
        }
    }


    //protected void SupportingFileGrid_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName == "Download")
    //    {
    //        int index = Convert.ToInt32(e.CommandArgument);
            

    //        PermissionsManager prm = new PermissionsManager();
    //        ModelPermissionLevel Permission = prm.GetPermissionLevel(Context.User.Identity.Name, ContentObjectID);
    //        prm.Dispose();
    //        prm = null;
    //        if (Permission < ModelPermissionLevel.Editable)
    //        {
    //            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
    //            BindModelDetails();
    //            return;
    //        }

    //        vwarDAL.IDataRepository vd = (new vwarDAL.DataAccessFactory()).CreateDataRepositorProxy();
    //        vwarDAL.ContentObject co = vd.GetContentObjectById(ContentObjectID, !IsPostBack, true);
           
    //        vd.Dispose();

    //        HttpContext.Current.Response.Redirect("./Serve.ashx?pid=" + ContentObjectID + "&mode=GetSupportingFile&SupportingFileName=" + co.SupportingFiles[index].Filename);

    //    }
    //}
    [System.Web.Services.WebMethod()]
    public static void DownloadButton_Click_Impl(string format, string ContentObjectID)
    {
        var factory = new DataAccessFactory();
        IDataRepository vd = factory.CreateDataRepositorProxy();
        var co = vd.GetContentObjectById(ContentObjectID, false);

        if (LR_3DR_Bridge.LR_Integration_Enabled())
            LR_3DR_Bridge.ModelDownloaded(co);

        vd.IncrementDownloads(ContentObjectID);
        try
        {
            if (String.IsNullOrEmpty(format))
            {
                string clientFileName = (!String.IsNullOrEmpty(co.OriginalFileName)) ? co.OriginalFileName : co.Location;
                var data = vd.GetContentFile(co.PID, clientFileName);

                Website.Documents.ServeDocument(data, clientFileName);
            }
            else
            {
                var data = vd.GetContentFile(co.PID, co.Location);
                if (format == ".dae")
                {
                    Website.Documents.ServeDocument(data, co.Location);
                }
                else if (format != ".O3Dtgz")
                {
                    Website.Documents.ServeDocument(data, co.Location, null, format);
                }
                else
                {
                    var displayFile = vd.GetContentFile(co.PID, co.DisplayFile);
                    Website.Documents.ServeDocument(displayFile, co.DisplayFile);
                }


            }
        }
        catch (System.Threading.ThreadAbortException tabexc) { }
        catch (Exception f)
        {
        }
        vd.Dispose();
    }
    public class UpdateAssetDataResponse
    {
        public bool Success;
        public string[] Keywords;
        public string Description;
        public string Title;
        public string LicenseTitle;
        public string LicenseURL;
        public string LicenseImage;
        public UpdateAssetDataResponse(bool suc)
        {
            Success = suc;
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static UpdateAssetDataResponse UpdateAssetData(string Title, string Description, string Keywords, string License, string pid)
    {

        PermissionsManager prm = new PermissionsManager();

        MembershipUser user = Membership.GetUser();
        if (user ==null || !user.IsApproved)
        {
            return new UpdateAssetDataResponse(false);
        }
        ModelPermissionLevel Permission = prm.GetPermissionLevel(user.UserName, pid);
        prm.Dispose();
        prm = null;
        if (Permission < ModelPermissionLevel.Editable)
        {
            return new UpdateAssetDataResponse(false);
        }

        vwarDAL.IDataRepository vd = (new vwarDAL.DataAccessFactory()).CreateDataRepositorProxy();
        vwarDAL.ContentObject co = vd.GetContentObjectById(pid, false, true);
        

        if (co != null)
        {
            co.Title = Title;
            co.Description = Description;
            co.Keywords = Keywords;
            co.CreativeCommonsLicenseURL = License;
           
            UpdateAssetDataResponse response = new UpdateAssetDataResponse(true);
            response.Title = co.Title;
            response.Description = co.Description;
            response.Keywords = co.Keywords.Split(new char[] { ',', '|' });
            for (int i = 0; i < response.Keywords.Length; i++)
                response.Keywords[i] = response.Keywords[i].ToLower().Trim();

            switch (License.ToLower().Trim())
            {
                case "by-nc-sa":
                    response.LicenseURL = "http://creativecommons.org/licenses/by-nc-sa/3.0/legalcode";
                    response.LicenseImage = "http://i.creativecommons.org/l/by-nc-sa/3.0/88x31.png";
                    response.LicenseTitle = "by-nc-sa";
                    break;

                case "by-nc-nd":
                response.LicenseURL =  "http://creativecommons.org/licenses/by-nc-nd/3.0/legalcode";
                    response.LicenseImage = "http://i.creativecommons.org/l/by-nc-nd/3.0/88x31.png";
                    response.LicenseTitle = "by-nc-nd";
                    break;

                case "by-nc":
                response.LicenseURL =  "http://creativecommons.org/licenses/by-nc/3.0/legalcode";
                    response.LicenseImage = "http://i.creativecommons.org/l/by-nc/3.0/88x31.png";
                    response.LicenseTitle = "by-nc";
                    break;

                case "by-nd":
                response.LicenseURL =  "http://creativecommons.org/licenses/by-nd/3.0/legalcode";
                    response.LicenseImage = "http://i.creativecommons.org/l/by-nd/3.0/88x31.png";
                    response.LicenseTitle = "by-nd";
                    break;

                case "by-sa":
                response.LicenseURL =  "http://creativecommons.org/licenses/by-sa/3.0/legalcode";
                    response.LicenseImage = "http://i.creativecommons.org/l/by-sa/3.0/88x31.png";
                    response.LicenseTitle = "by-sa";
                    break;

                case "publicdomain":
                response.LicenseURL =  "http://creativecommons.org/publicdomain/mark/1.0/";
                    response.LicenseImage = "http://i.creativecommons.org/l/publicdomain/88x31.png";
                    response.LicenseTitle = "Public Domain";
                    break;

                case "by":
                    response.LicenseURL = "http://creativecommons.org/licenses/by/3.0/legalcode";
                    response.LicenseImage = "http://i.creativecommons.org/l/by/3.0/88x31.png";
                    response.LicenseTitle = "by";
                    break;
            }
            co.CreativeCommonsLicenseURL = response.LicenseURL;
            vd.UpdateContentObject(co);
            vd.Dispose();
            return response;
        }
        
        vd.Dispose();
        vd = null;
        return new UpdateAssetDataResponse(false);
    }
    public class UpdateDetailsResponse
    {
        public bool Success;
        public string format;
        public string polys;
        public string textures;
        public UpdateDetailsResponse(bool suc)
        {
            Success = suc;
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static UpdateDetailsResponse UpdateDetails(string polys,string textures,string format, string pid)
    {

        PermissionsManager prm = new PermissionsManager();

        MembershipUser user = Membership.GetUser();
        if (user == null || !user.IsApproved)
        {
            return new UpdateDetailsResponse(false);
        }
        ModelPermissionLevel Permission = prm.GetPermissionLevel(user.UserName, pid);
        prm.Dispose();
        prm = null;
        if (Permission < ModelPermissionLevel.Editable)
        {
            return new UpdateDetailsResponse(false);
        }

        vwarDAL.IDataRepository vd = (new vwarDAL.DataAccessFactory()).CreateDataRepositorProxy();
        vwarDAL.ContentObject co = vd.GetContentObjectById(pid, false, true);


        if (co != null)
        {


            UpdateDetailsResponse response = new UpdateDetailsResponse(true);
            co.NumPolygons = System.Convert.ToInt32(polys);
            co.NumTextures = System.Convert.ToInt32(textures);
            co.Format = format;
            response.format = format;
            response.polys = co.NumPolygons.ToString();
            response.textures = co.NumTextures.ToString();
            vd.UpdateContentObject(co);
            vd.Dispose();
            return response;
        }

        vd.Dispose();
        vd = null;
        return new UpdateDetailsResponse(false);
    }
    public class UpdateDeveloperInfoResponse
    {
        public bool Success;
        public string DeveloperName;
        public string ArtistName;
        public string MoreInfoURL;
        public UpdateDeveloperInfoResponse(bool suc)
        {
            Success = suc;
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static UpdateDeveloperInfoResponse UpdateDeveloperInfo(string DeveloperName, string ArtistName, string MoreInfoURL, string pid, string newfilename)
    {

        PermissionsManager prm = new PermissionsManager();

        MembershipUser user = Membership.GetUser();
        if (user == null || !user.IsApproved)
        {
            return new UpdateDeveloperInfoResponse(false);
        }
        ModelPermissionLevel Permission = prm.GetPermissionLevel(user.UserName, pid);
        prm.Dispose();
        prm = null;
        if (Permission < ModelPermissionLevel.Editable)
        {
            return new UpdateDeveloperInfoResponse(false);
        }

        vwarDAL.IDataRepository vd = (new vwarDAL.DataAccessFactory()).CreateDataRepositorProxy();
        vwarDAL.ContentObject co = vd.GetContentObjectById(pid, false, true);


        if (co != null)
        {

            try
            {
                using (FileStream stream = new FileStream(HostingEnvironment.MapPath(String.Format("~/App_Data/imageTemp/{0}", newfilename)), FileMode.Open))
                {
                    co.SetDeveloperLogoFile(stream, newfilename);
                }
                File.Delete(HostingEnvironment.MapPath(String.Format("~/App_Data/imageTemp/{0}", newfilename)));
            }
            catch (Exception e)
            {

            }

            UpdateDeveloperInfoResponse response = new UpdateDeveloperInfoResponse(true);

            co.DeveloperName = DeveloperName;
            co.ArtistName = ArtistName;
            co.MoreInformationURL = MoreInfoURL;

            response.MoreInfoURL = co.MoreInformationURL;
            response.ArtistName = co.ArtistName;
            response.DeveloperName = co.DeveloperName;
            vd.UpdateContentObject(co);
            vd.Dispose();
            return response;
        }

        vd.Dispose();
        vd = null;
        return new UpdateDeveloperInfoResponse(false);
    }
    public class UpdateSponsorInfoResponse
    {
       
        public string SponsorName;
        public bool Success;
        public UpdateSponsorInfoResponse(bool suc)
        {
            Success = suc;
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static UpdateSponsorInfoResponse UpdateSponsorInfo(string SponsorName, string pid, string newfilename)
    {

        PermissionsManager prm = new PermissionsManager();

        MembershipUser user = Membership.GetUser();
        if (user == null || !user.IsApproved)
        {
            return new UpdateSponsorInfoResponse(false);
        }
        ModelPermissionLevel Permission = prm.GetPermissionLevel(user.UserName, pid);
        prm.Dispose();
        prm = null;
        if (Permission < ModelPermissionLevel.Editable)
        {
            return new UpdateSponsorInfoResponse(false);
        }

        vwarDAL.IDataRepository vd = (new vwarDAL.DataAccessFactory()).CreateDataRepositorProxy();
        vwarDAL.ContentObject co = vd.GetContentObjectById(pid, false, true);


        if (co != null)
        {


            UpdateSponsorInfoResponse response = new UpdateSponsorInfoResponse(true);

            co.SponsorName = SponsorName;


            response.SponsorName = co.SponsorName;

            try
            {
                using (FileStream stream = new FileStream(HostingEnvironment.MapPath(String.Format("~/App_Data/imageTemp/{0}", newfilename)), FileMode.Open))
                {
                    co.SetSponsorLogoFile(stream,newfilename);
                }
                File.Delete(HostingEnvironment.MapPath(String.Format("~/App_Data/imageTemp/{0}", newfilename)));
            }
            catch (Exception e)
            {

            }
            vd.UpdateContentObject(co);
            vd.Dispose();
            return response;
        }

        vd.Dispose();
        vd = null;
        return new UpdateSponsorInfoResponse(false);
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static bool UpdateScreenshot(string pid, string newfilename)
    {

        PermissionsManager prm = new PermissionsManager();

        MembershipUser user = Membership.GetUser();
        if (user == null || !user.IsApproved)
        {
            return (false);
        }
        ModelPermissionLevel Permission = prm.GetPermissionLevel(user.UserName, pid);
        prm.Dispose();
        prm = null;
        if (Permission < ModelPermissionLevel.Editable)
        {
            return (false);
        }

        vwarDAL.IDataRepository vd = (new vwarDAL.DataAccessFactory()).CreateDataRepositorProxy();
        vwarDAL.ContentObject co = vd.GetContentObjectById(pid, false, true);


        if (co != null)
        {


            
            try
            {
                using (FileStream stream = new FileStream(HostingEnvironment.MapPath(String.Format("~/App_Data/imageTemp/{0}", newfilename)), FileMode.Open))
                {
                    co.SetScreenShotFile(stream, newfilename);
                }
                File.Delete(HostingEnvironment.MapPath(String.Format("~/App_Data/imageTemp/{0}", newfilename)));
            }
            catch (Exception e)
            {

            }
            vd.UpdateContentObject(co);
            vd.Dispose();
            return true;
        }

        vd.Dispose();
        vd = null;
        return (false);
    }
    public class UploadSupportingFileResponse
    {

        public string Filename;
        public string Description;
        public bool Success;
        public UploadSupportingFileResponse(bool suc)
        {
            Success = suc;
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static UploadSupportingFileResponse UploadSupportingFileHandler(string filename, string description, string pid, string newfilename)
    {

        PermissionsManager prm = new PermissionsManager();

        MembershipUser user = Membership.GetUser();
        if (user == null || !user.IsApproved)
        {
            return new UploadSupportingFileResponse(false);
        }
        ModelPermissionLevel Permission = prm.GetPermissionLevel(user.UserName, pid);
        prm.Dispose();
        prm = null;
        if (Permission < ModelPermissionLevel.Editable)
        {
            return new UploadSupportingFileResponse(false);
        }

        vwarDAL.IDataRepository vd = (new vwarDAL.DataAccessFactory()).CreateDataRepositorProxy();
        vwarDAL.ContentObject co = vd.GetContentObjectById(pid, false, true);

        UploadSupportingFileResponse response = new UploadSupportingFileResponse(true);
        if (co != null)
        {

            try
            {
                using (FileStream stream = new FileStream(HostingEnvironment.MapPath(String.Format("~/App_Data/imageTemp/{0}", newfilename)), FileMode.Open))
                {
                    co.AddSupportingFile(stream, filename, description);
                }
                File.Delete(HostingEnvironment.MapPath(String.Format("~/App_Data/imageTemp/{0}", newfilename)));
                response = new UploadSupportingFileResponse(true);
                response.Filename = filename;
                response.Description = description;
            }
            catch (Exception e)
            {
                response = new UploadSupportingFileResponse(false);
            }
            vd.UpdateContentObject(co);
            vd.Dispose();
            return response;
        }

        vd.Dispose();
        vd = null;
        return new UploadSupportingFileResponse(false);
    }
    public class GetSupportingFilesResponse
    {

        public bool EditAllowed;
        public bool DownloadAllowed;
        public bool Success;
        public SupportingFile[] files;
        public GetSupportingFilesResponse(bool suc)
        {
            Success = suc;
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static GetSupportingFilesResponse GetSupportingFiles(string pid)
    {

        PermissionsManager prm = new PermissionsManager();

        MembershipUser user = Membership.GetUser();
       
        ModelPermissionLevel Permission = prm.GetPermissionLevel(user != null?user.UserName:DefaultUsers.Anonymous[0], pid);
        prm.Dispose();
        prm = null;
        if (Permission < ModelPermissionLevel.Searchable)
        {
            return new GetSupportingFilesResponse(false);
        }

        vwarDAL.IDataRepository vd = (new vwarDAL.DataAccessFactory()).CreateDataRepositorProxy();
        vwarDAL.ContentObject co = vd.GetContentObjectById(pid, false, true);

        GetSupportingFilesResponse response = new GetSupportingFilesResponse(true);
        if (co != null)
        {

            try
            {
                response.files = new SupportingFile[co.SupportingFiles.Count];
                for (int i = 0; i < co.SupportingFiles.Count; i++)
                    response.files[i] = co.SupportingFiles[i];

                response.EditAllowed = Permission >= ModelPermissionLevel.Editable;
                response.DownloadAllowed = Permission >= ModelPermissionLevel.Fetchable;
            }
            catch (Exception e)
            {
                response = new GetSupportingFilesResponse(false);
            }
            vd.UpdateContentObject(co);
            vd.Dispose();
            return response;
        }

        vd.Dispose();
        vd = null;
        return new GetSupportingFilesResponse(false);
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static bool DeleteSupportingFile(string Filename, string pid)
    {

        PermissionsManager prm = new PermissionsManager();

        MembershipUser user = Membership.GetUser();

        ModelPermissionLevel Permission = prm.GetPermissionLevel(user != null ? user.UserName : DefaultUsers.Anonymous[0], pid);
        prm.Dispose();
        prm = null;
        if (Permission < ModelPermissionLevel.Editable)
        {
            return (false);
        }

        vwarDAL.IDataRepository vd = (new vwarDAL.DataAccessFactory()).CreateDataRepositorProxy();
        vwarDAL.ContentObject co = vd.GetContentObjectById(pid, false, true);

        if (co != null)
        {

            try
            {

                bool ret = co.RemoveSupportingFile(Filename);
                vd.Dispose();
                return ret;

            }
            catch (Exception e)
            {
                vd.Dispose();
                return false;
            }
            vd.UpdateContentObject(co);
            vd.Dispose();
            return true;
        }

        vd.Dispose();
        vd = null;
        return (false);
    }
}
