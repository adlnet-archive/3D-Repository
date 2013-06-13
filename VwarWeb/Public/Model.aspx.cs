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
using vwar.service.host;
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
            return;
        }


        APILink.NavigateUrl = "https://" + ConfigurationManager.AppSettings["LR_Integration_APIBaseURL"] +  "/" + ContentObjectID + "/Metadata/json?id=00-00-00";
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

            if (Permission >= ModelPermissionLevel.Editable)
            {
                editLink.Visible = true;
                PermissionsLink.Visible = true;
                DeleteLink.Visible = true;
                //editLink.NavigateUrl = "~/Users/Edit.aspx?ContentObjectID=" + co.PID;
            }
            else
                {
                    EditorButtons.Visible = false;
                }

            if (Permission >= ModelPermissionLevel.Fetchable)
            {  
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
                if (User.Identity.IsAuthenticated)
                {
                    RequestAccessLabel.Visible = true;
                }
                else
                {
                    LoginToDlLabel.Visible = true;
                }
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


                //this.DeveloperLogoRow.Visible = !string.IsNullOrEmpty(co.DeveloperLogoImageFileName);

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
                        this.CCLHyperLink.ImageUrl = "../styles/images/by-nc-sa.png";
                        this.CCLHyperLink.ToolTip = "by-nc-sa";
                        break;

                    case "http://creativecommons.org/licenses/by-nc-nd/3.0/legalcode":
                        this.CCLHyperLink.ImageUrl = "../styles/images/by-nc-nd.png";
                        this.CCLHyperLink.ToolTip = "by-nc-nd";
                        break;

                    case "http://creativecommons.org/licenses/by-nc/3.0/legalcode":
                        this.CCLHyperLink.ImageUrl = "../styles/images/by-nc.png";
                        this.CCLHyperLink.ToolTip = "by-nc";
                        break;

                    case "http://creativecommons.org/licenses/by-nd/3.0/legalcode":
                        this.CCLHyperLink.ImageUrl = "../styles/images/by-nd.png";
                        this.CCLHyperLink.ToolTip = "by-nd";
                        break;

                    case "http://creativecommons.org/licenses/by-sa/3.0/legalcode":
                        this.CCLHyperLink.ImageUrl = "../styles/images/by-sa.png";
                        this.CCLHyperLink.ToolTip = "by-sa";
                        break;
                    case "http://creativecommons.org/publicdomain/mark/1.0/":
                        this.CCLHyperLink.ImageUrl = "../styles/images/publicdomain.png";
                        this.CCLHyperLink.ToolTip = "Public Domain";
                        break;
                    case "http://creativecommons.org/licenses/by/3.0/legalcode":
                        this.CCLHyperLink.ImageUrl = "../styles/images/by.png";
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
            EditDistributionDeterminationDate.Text = co.Distribution_Determination_Date.ToString();
            EditDistributionOffice.Text = co.Distribution_Contolling_Office;
            EditDistributionReasonLabel.Text = co.Distribution_Reason;
            EditDistributionRegulation.Text = co.Distribution_Regulation;
            DistributionLabel.Text = Enum.GetName(typeof(DistributionGrade), co.Distribution_Grade);
            DistributionStatementText.InnerText = GetDistributionText(co);
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

    protected void DownloadButton_Click(object sender, EventArgs e)
    {

        HttpContext.Current.Response.Redirect("./Serve.ashx?pid=" + ContentObjectID + "&mode=DownloadModel&Format=original");
               
          
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

        APIWrapper api = null;
        if (Membership.GetUser() != null && Membership.GetUser().IsApproved)
            api = new APIWrapper(Membership.GetUser().UserName, null);
        else
            api = new APIWrapper(vwarDAL.DefaultUsers.Anonymous[0], null);


        vwar.service.host.Metadata md = api.GetMetadata(pid, "00-00-00");
       
        if(md == null)
            return new UpdateAssetDataResponse(false);

            UpdateAssetDataResponse response = new UpdateAssetDataResponse(true);
            response.Keywords = md.Keywords.Split(new char[] { ',', '|' });
            for (int i = 0; i < response.Keywords.Length; i++)
                response.Keywords[i] = response.Keywords[i].ToLower().Trim();

            switch (License.ToLower().Trim())
            {
                case "by-nc-sa":
                    response.LicenseURL = "http://creativecommons.org/licenses/by-nc-sa/3.0/legalcode";
                    response.LicenseImage = "../styles/images/by-nc-sa.png";
                    response.LicenseTitle = "by-nc-sa";
                    break;

                case "by-nc-nd":
                response.LicenseURL =  "http://creativecommons.org/licenses/by-nc-nd/3.0/legalcode";
                response.LicenseImage = "../styles/images/by-nc-nd.png";
                    response.LicenseTitle = "by-nc-nd";
                    break;

                case "by-nc":
                response.LicenseURL =  "http://creativecommons.org/licenses/by-nc/3.0/legalcode";
                response.LicenseImage = "../styles/images/by-nc.png";
                    response.LicenseTitle = "by-nc";
                    break;

                case "by-nd":
                response.LicenseURL =  "http://creativecommons.org/licenses/by-nd/3.0/legalcode";
                response.LicenseImage = "../styles/images/by-nd.png";
                    response.LicenseTitle = "by-nd";
                    break;

                case "by-sa":
                response.LicenseURL =  "http://creativecommons.org/licenses/by-sa/3.0/legalcode";
                response.LicenseImage = "../styles/images/by-sa.png";
                    response.LicenseTitle = "by-sa";
                    break;

                case "publicdomain":
                response.LicenseURL =  "http://creativecommons.org/publicdomain/mark/1.0/";
                response.LicenseImage = "../styles/images/publicdomain.png";
                    response.LicenseTitle = "Public Domain";
                    break;

                case "by":
                    response.LicenseURL = "http://creativecommons.org/licenses/by/3.0/legalcode";
                    response.LicenseImage = "../styles/images/by.png";
                    response.LicenseTitle = "by";
                    break;
            }

            md.Title = Title;
            md.Description = Description;
            md.Keywords = Keywords;
            md.License = response.LicenseURL;

            response.Title = md.Title;
            response.Description = md.Description;
            

            string result = api.UpdateMetadata(md, pid, "00-00-00");
            if(result != "Ok")
                return new UpdateAssetDataResponse(false);
            return response;
        

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

        APIWrapper api = null;
        if (Membership.GetUser() != null && Membership.GetUser().IsApproved)
            api = new APIWrapper(Membership.GetUser().UserName, null);
        else
            api = new APIWrapper(vwarDAL.DefaultUsers.Anonymous[0], null);


        vwar.service.host.Metadata md = api.GetMetadata(pid, "00-00-00");
         
        md.NumPolygons = polys;
        md.NumTextures = textures;
        md.Format = format;

        string result = api.UpdateMetadata(md, pid, "00-00-00");
        UpdateDetailsResponse response = new UpdateDetailsResponse(result == "Ok");
        response.polys = md.NumPolygons;
        response.textures = md.NumTextures;
        response.format = md.Format;

        return response;
       
       
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

        APIWrapper api = null;
        if (Membership.GetUser() != null && Membership.GetUser().IsApproved)
            api = new APIWrapper(Membership.GetUser().UserName, null);
        else
            api = new APIWrapper(vwarDAL.DefaultUsers.Anonymous[0], null);

        
        vwar.service.host.Metadata md = api.GetMetadata(pid, "00-00-00");
        UpdateDeveloperInfoResponse response = new UpdateDeveloperInfoResponse(false);

        if (md.ArtistName != ArtistName || md.DeveloperName != DeveloperName || md.MoreInformationURL != MoreInfoURL)
        {
            md.ArtistName = ArtistName;
            md.DeveloperName = DeveloperName;
            md.MoreInformationURL = MoreInfoURL;

            string result = api.UpdateMetadata(md, pid, "00-00-00");
            response = new UpdateDeveloperInfoResponse(result == "Ok");
            response.DeveloperName = md.DeveloperName;
            response.ArtistName = md.ArtistName;
            response.MoreInfoURL = md.MoreInformationURL;
        }


        try
        {
            using (FileStream stream = new FileStream(HostingEnvironment.MapPath(String.Format("~/App_Data/imageTemp/{0}", newfilename)), FileMode.Open))
            {
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                api.UploadDeveloperLogo(data, pid, newfilename, "00-00-00");
            }
            File.Delete(HostingEnvironment.MapPath(String.Format("~/App_Data/imageTemp/{0}", newfilename)));
        }
        catch (Exception e)
        {

        }
        

        return response;
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



        APIWrapper api = null;
        if (Membership.GetUser() != null && Membership.GetUser().IsApproved)
            api = new APIWrapper(Membership.GetUser().UserName, null);
        else
            api = new APIWrapper(vwarDAL.DefaultUsers.Anonymous[0], null);

        vwar.service.host.Metadata md = api.GetMetadata(pid, "00-00-00");
        UpdateSponsorInfoResponse response = new UpdateSponsorInfoResponse(false);

        if (md.SponsorName != SponsorName)
        {
            md.SponsorName = SponsorName;
            string result = api.UpdateMetadata(md, pid, "00-00-00");
            if (result == "Ok")
            {
                response = new UpdateSponsorInfoResponse(true);
                response.SponsorName = md.SponsorName;
            }
        }
        try
        {
            using (FileStream stream = new FileStream(HostingEnvironment.MapPath(String.Format("~/App_Data/imageTemp/{0}", newfilename)), FileMode.Open))
            {
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                api.UploadSponsorLogo(data,pid,newfilename,"00-00-00");
                    
            }
            File.Delete(HostingEnvironment.MapPath(String.Format("~/App_Data/imageTemp/{0}", newfilename)));
        }
        catch (Exception e)
        {

        }
          
        return response;
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static bool UpdateScreenshot(string pid, string newfilename)
    {

        APIWrapper api = null;
        if (Membership.GetUser() != null && Membership.GetUser().IsApproved)
            api = new APIWrapper(Membership.GetUser().UserName, null);
        else
            api = new APIWrapper(vwarDAL.DefaultUsers.Anonymous[0], null);

        bool response = false;
        try
        {
            using (FileStream stream = new FileStream(HostingEnvironment.MapPath(String.Format("~/App_Data/imageTemp/{0}", newfilename)), FileMode.Open))
            {
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                string result = api.UploadScreenShot(data, pid, newfilename, "00-00-00");
                if (result == "Ok")
                {
                    response = true;
                }
                
            }
            File.Delete(HostingEnvironment.MapPath(String.Format("~/App_Data/imageTemp/{0}", newfilename)));
        }
        catch (System.IO.FileNotFoundException t)
        {

        }
        return response;
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

        APIWrapper api = null;
        if (Membership.GetUser() != null && Membership.GetUser().IsApproved)
            api = new APIWrapper(Membership.GetUser().UserName, null);
        else
            api = new APIWrapper(vwarDAL.DefaultUsers.Anonymous[0], null);

        UploadSupportingFileResponse response = new UploadSupportingFileResponse(false);
        try
        {
            using (FileStream stream = new FileStream(HostingEnvironment.MapPath(String.Format("~/App_Data/imageTemp/{0}", newfilename)), FileMode.Open))
            {
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                string result = api.UploadSupportingFile(data, pid, filename, description, "00-00-00");
                if (result == "Ok")
                {
                    response = new UploadSupportingFileResponse(true);
                    response.Description = description;
                    response.Filename = filename;
                }

            }
            File.Delete(HostingEnvironment.MapPath(String.Format("~/App_Data/imageTemp/{0}", newfilename)));
        }
        catch (System.IO.FileNotFoundException t)
        {

        }
        return response;
    }
    public class GetSupportingFilesResponse
    {

        public bool EditAllowed;
        public bool DownloadAllowed;
        public bool Success;
        public vwarDAL.SupportingFile[] files;
        public GetSupportingFilesResponse(bool suc)
        {
            Success = suc;
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static GetSupportingFilesResponse GetSupportingFiles(string pid)
    {

        APIWrapper api = null;
        if (Membership.GetUser() != null && Membership.GetUser().IsApproved)
            api = new APIWrapper(Membership.GetUser().UserName, null);
        else
            api = new APIWrapper(vwarDAL.DefaultUsers.Anonymous[0], null);

        vwar.service.host.Metadata md = api.GetMetadata(pid, "00-00-00");
        if (md == null)
        {
            return new GetSupportingFilesResponse(false);
        }

        PermissionsManager prm = new PermissionsManager();

        MembershipUser user = Membership.GetUser();
        
        ModelPermissionLevel Permission = prm.GetPermissionLevel(user != null ? user.UserName:vwarDAL.DefaultUsers.Anonymous[0], pid);
        prm.Dispose();

        GetSupportingFilesResponse response = new GetSupportingFilesResponse(true);
        response.DownloadAllowed = Permission >= ModelPermissionLevel.Fetchable;
        response.EditAllowed = Permission >= ModelPermissionLevel.Editable;
        response.files = new vwarDAL.SupportingFile[md.SupportingFiles.Count];
        for(int i=0; i<md.SupportingFiles.Count; i++)
        {
            response.files[i] = new vwarDAL.SupportingFile(md.SupportingFiles[i].Filename, md.SupportingFiles[i].Description, "");
        }
        return response;
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static bool DeleteSupportingFile(string Filename, string pid)
    {

        APIWrapper api = null;
        if (Membership.GetUser() != null && Membership.GetUser().IsApproved)
            api = new APIWrapper(Membership.GetUser().UserName, null);
        else
            api = new APIWrapper(vwarDAL.DefaultUsers.Anonymous[0], null);

        bool result = api.DeleteSupportingFile(pid, Filename);
        return result;
    }

    public static string GetDistributionText(ContentObject co)
    {
        switch (co.Distribution_Grade)
        {
            case DistributionGrade.Distribution_A:
                return "Approved for public release; distribution is unlimited";
                break;
            case DistributionGrade.Distribution_B:
                return "Distribution authorized to U.S. Government agencies only. " + co.Distribution_Reason + " " + co.Distribution_Determination_Date + ". Other requests for this document shall be referred to " + co.Distribution_Contolling_Office;
                break;
            case DistributionGrade.Distribution_C:
                return "Distribution authorized to U.S. Government Agencies and their contractors " + co.Distribution_Reason + " " + co.Distribution_Determination_Date + ". Other requests for this document shall be referred to " + co.Distribution_Contolling_Office;
                break;
            case DistributionGrade.Distribution_D:
                return "Distribution authorized to the Department of Defense and U.S. DoD contractors only. " + co.Distribution_Reason + " " + co.Distribution_Determination_Date + ". Other requests shall be referred to " + co.Distribution_Contolling_Office;
                break;
            case DistributionGrade.Distribution_E:
                return "Distribution authorized to DoD Components only  " + co.Distribution_Reason + " " + co.Distribution_Determination_Date + ". Other requests shall be referred to " + co.Distribution_Contolling_Office;
                break;
            case DistributionGrade.Distribution_F:
                return "Further dissemination only as directed by " + co.Distribution_Contolling_Office + " " + co.Distribution_Determination_Date + " or higher DoD authority.";
                break;
            case DistributionGrade.Distribution_X:
                return "Distribution authorized to U.S. Government Agencies and private individuals or enterprises eligible to obtain export-controlled technical data in accordance with " + co.Distribution_Regulation + "; " + co.Distribution_Determination_Date + ". DoD Controlling Office is " + co.Distribution_Contolling_Office;
                break;
            case DistributionGrade.NA:
                return "";
                break;

        }
        return "";
    }

    public static string GetDistributionText(vwar.service.host.Metadata co)
    {
        switch ((vwarDAL.DistributionGrade)Enum.Parse(typeof(vwarDAL.DistributionGrade), co.Distribution_Grade))
        {
            case DistributionGrade.Distribution_A:
                return "Approved for public release; distribution is unlimited";
                break;
            case DistributionGrade.Distribution_B:
                return "Distribution authorized to U.S. Government agencies only. " + co.Distribution_Reason + " " + co.Distribution_Determination_Date + ". Other requests for this document shall be referred to " + co.Distribution_Contolling_Office;
                break;
            case DistributionGrade.Distribution_C:
                return "Distribution authorized to U.S. Government Agencies and their contractors " + co.Distribution_Reason + " " + co.Distribution_Determination_Date + ". Other requests for this document shall be referred to " + co.Distribution_Contolling_Office;
                break;
            case DistributionGrade.Distribution_D:
                return "Distribution authorized to the Department of Defense and U.S. DoD contractors only. " + co.Distribution_Reason + " " + co.Distribution_Determination_Date + ". Other requests shall be referred to " + co.Distribution_Contolling_Office;
                break;
            case DistributionGrade.Distribution_E:
                return "Distribution authorized to DoD Components only  " + co.Distribution_Reason + " " + co.Distribution_Determination_Date + ". Other requests shall be referred to " + co.Distribution_Contolling_Office;
                break;
            case DistributionGrade.Distribution_F:
                return "Further dissemination only as directed by " + co.Distribution_Contolling_Office + " " + co.Distribution_Determination_Date + " or higher DoD authority.";
                break;
            case DistributionGrade.Distribution_X:
                return "Distribution authorized to U.S. Government Agencies and private individuals or enterprises eligible to obtain export-controlled technical data in accordance with " + co.Distribution_Regulation + "; " + co.Distribution_Determination_Date + ". DoD Controlling Office is " + co.Distribution_Contolling_Office;
                break;
            case DistributionGrade.NA:
                return "";
                break;

        }
            return "";
    }

    public class UpdateDistributionInfoResponse
    {

        public string Class;
        public string DeterminationDate;
        public string Office;
        public string Regulation;
        public string Reason;
        public string FullText;

        public bool Success;
        public UpdateDistributionInfoResponse(bool suc)
        {
            Success = suc;
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static UpdateDistributionInfoResponse UpdateDistributionInfo( string Class,string DeterminationDate,string Office,string Regulation,string Reason, string pid)
    {

        APIWrapper api = null;
        if (Membership.GetUser() != null && Membership.GetUser().IsApproved)
            api = new APIWrapper(Membership.GetUser().UserName, null);
        else
            api = new APIWrapper(vwarDAL.DefaultUsers.Anonymous[0], null);


        vwar.service.host.Metadata md = api.GetMetadata(pid, "00-00-00");
        if (md == null)
            return new UpdateDistributionInfoResponse(false);

        UpdateDistributionInfoResponse response = new UpdateDistributionInfoResponse(false);

        md.Distribution_Grade = Class;
        md.Distribution_Reason = Reason;
        md.Distribution_Regulation = Regulation;
        md.Distribution_Determination_Date = DeterminationDate;
        md.Distribution_Contolling_Office = Office;

        string result = api.UpdateMetadata(md, pid, "00-00-00");
        if (result == "Ok")
        {
            response = new UpdateDistributionInfoResponse(true);
            response.Class = md.Distribution_Grade; ;
            response.DeterminationDate = DeterminationDate;
            response.Office = Office;
            response.Reason = Reason;
            response.Regulation = Regulation;
            response.FullText = GetDistributionText(md);
        }

        return response;
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static bool SendAccessRequest(string pid, string message)
    {

        
        if (Membership.GetUser() == null || !Membership.GetUser().IsApproved)
        {
            return false;
        }

         var factory = new DataAccessFactory();
        IDataRepository vd = factory.CreateDataRepositorProxy();
        var co = vd.GetContentObjectById(pid, false);

        MessageManager manager = new MessageManager();
        message = Westwind.Web.Utilities.HtmlSanitizer.SanitizeHtml(message,null);
        manager.SendMessage(Membership.GetUser().UserName, co.SubmitterEmail, "Requesting Access to Model: " + co.Title + " (" + pid + ")", "A request has been sent from " + Membership.GetUser().UserName + " for access to model " + co.Title + " (" + pid + "). Please review this request and decide on the proper action. Click here to access the model: <a href='/public/model.aspx?ContentObjectID=" + pid +"'>"+pid+"</a> The user sent this message in the request: \n\n" + message +
        "<br/><div class='accessbutton' onclick='GrantOrDenyRequest(\"" + Membership.GetUser().UserName + "\",\"Grant\",\"" + pid + "\");'>Grant Access</div><div class='accessbutton' onclick='GrantOrDenyRequest(\"" + Membership.GetUser().UserName + "\",\"Deny\",\"" + pid + "\");'>Deny Request</div>"
        
        , Membership.GetUser().UserName);
        return true;
    }
}
