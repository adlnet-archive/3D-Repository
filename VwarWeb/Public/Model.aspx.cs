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
        if (Permission < ModelPermissionLevel.Searchable)
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }

        var uri = Request.Url;
        string proxyTemplate = "Model.ashx?pid={0}&file={1}&fileid={2}";

        vwarDAL.IDataRepository vd = DAL;
        vwarDAL.ContentObject co = vd.GetContentObjectById(ContentObjectID, !IsPostBack, true);

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
                if (co.DisplayFile != string.Empty && co.Location != string.Empty && Permission >= ModelPermissionLevel.Searchable)
                {
                    Page.ClientScript.RegisterClientScriptBlock(GetType(), "vload", string.Format("vLoader = new ViewerLoader('{0}', '{1}', '{2}', '{3}', {4});", Page.ResolveClientUrl("~/Public/PreviewModel.ashx"),
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
                    ScreenshotImage.ImageUrl = String.Format("Screenshot.ashx?ContentObjectID={0}&Session=true", co.PID);
                }
                AddHeaderTag("link", "og:image", ScreenshotImage.ImageUrl);
            }
            else if ("Texture".Equals(co.AssetType, StringComparison.InvariantCultureIgnoreCase))
            {
                ScreenshotImage.ImageUrl = String.Format(proxyTemplate, co.PID, co.Location);
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
                    DeleteLink.Visible = true;
                    editLink.NavigateUrl = "~/Users/Edit.aspx?ContentObjectID=" + co.PID;
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
            this.keywordLabel.Visible = !string.IsNullOrEmpty(co.Keywords);

            //more details
            this.MoreDetailsHyperLink.NavigateUrl = co.MoreInformationURL;
            this.MoreDetailsHyperLink.Text = co.MoreInformationURL;
            this.MoreDetailsRow.Visible = !string.IsNullOrEmpty(co.MoreInformationURL);

            string submitterFullName = Website.Common.GetFullUserName(co.SubmitterEmail);
            if (co.UploadedDate != null)
            {
                UploadedDateLabel.Text = "Uploaded by: " + submitterFullName + " on " + co.UploadedDate.ToString();
            }

            if (!String.IsNullOrEmpty(co.SponsorName) || !String.IsNullOrEmpty(co.SponsorLogoImageFileName)
               || !String.IsNullOrEmpty(co.SponsorLogoImageFileNameId))
            {
                //sponsor logo
                if (!string.IsNullOrEmpty(co.SponsorLogoImageFileName))
                {
                    this.SponsorLogoImage.ImageUrl = String.Format(proxyTemplate, co.PID, co.SponsorLogoImageFileName, co.SponsorLogoImageFileNameId);
                }

                this.SponsorLogoRow.Visible = !string.IsNullOrEmpty(co.SponsorLogoImageFileName);
                this.SponsorNameLabel.Text = co.SponsorName;
                this.SponsorNameRow.Visible = !string.IsNullOrEmpty(co.SponsorName);
            }
            else
            {
                this.SponsorInfoSection.Visible = false;
            }

            if (!String.IsNullOrEmpty(co.DeveloperName) || !String.IsNullOrEmpty(co.ArtistName)
                || !String.IsNullOrEmpty(co.DeveloperLogoImageFileName) || !String.IsNullOrEmpty(co.DeveloperLogoImageFileNameId))
            {
                //developr logo
                if (!string.IsNullOrEmpty(co.DeveloperLogoImageFileName))
                {
                    this.DeveloperLogoImage.ImageUrl = String.Format(proxyTemplate, co.PID, co.DeveloperLogoImageFileName, co.DeveloperLogoImageFileNameId);
                }


                this.DeveloperLogoRow.Visible = !string.IsNullOrEmpty(co.DeveloperLogoImageFileName);

                //developer name
                this.DeveloperNameHyperLink.NavigateUrl = "~/Public/Results.aspx?ContentObjectID=" + ContentObjectID + "&DeveloperName=" + Server.UrlEncode(co.DeveloperName);
                this.DeveloperNameHyperLink.Text = co.DeveloperName;

                if (String.IsNullOrEmpty(co.ArtistName))
                {
                    this.ArtistRow.Visible = false;
                }
                else
                {
                    this.ArtistNameHyperLink.NavigateUrl = "~/Public/Results.aspx?ContentObjectID=" + ContentObjectID + "&ArtistName=" + Server.UrlEncode(co.ArtistName);
                    this.ArtistNameHyperLink.Text = co.ArtistName;
                }

                this.DeveloperRow.Visible = !string.IsNullOrEmpty(co.DeveloperName);
            }
            else
            {
                this.DeveloperInfoSection.Visible = false;
            }
            this.FormatLabel.Text = "Native format: " + ((string.IsNullOrEmpty(co.Format)) ? "Unknown" : co.Format);

            //num polygons   
            this.NumPolygonsLabel.Text = co.NumPolygons.ToString();
            this.NumPolygonsRow.Visible = !string.IsNullOrEmpty(co.NumPolygons.ToString());

            //num textures
            this.NumTexturesLabel.Text = co.NumTextures.ToString();
            this.NumTexturesRow.Visible = !string.IsNullOrEmpty(co.NumTextures.ToString());

            //cclrow
            this.CCLHyperLink.Visible = !string.IsNullOrEmpty(co.CreativeCommonsLicenseURL);
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
            vwarDAL.IDataRepository vd = DAL;

            var ratingValue = rating.CurrentRating;
            vd.InsertReview(ratingValue, ratingText.Text.Length > 255 ? ratingText.Text.Substring(0, 255)
                : ratingText.Text, Context.User.Identity.Name, ContentObjectID);
            ViewState[RATINGKEY] = null;
            Response.Redirect(Request.RawUrl);

            ContentObject co = vd.GetContentObjectById(ContentObjectID,false);

            if (LR_3DR_Bridge.LR_Integration_Enabled())
                LR_3DR_Bridge.ModelRated(co);
        }
    }

    protected void ContentObjectFormView_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "DownloadZip":
                Label IDLabel = (Label)FindControl("IDLabel");
                Label LocationLabel = (Label)FindControl("LocationLabel");
                vwarDAL.IDataRepository vd = DAL;
                vd.IncrementDownloads(ContentObjectID);
                string filePath = Website.Common.FormatZipFilePath(IDLabel.Text.Trim(), LocationLabel.Text.Trim());
                string clientFileName = System.IO.Path.GetFileName(filePath);
                Website.Documents.ServeDocument(vd.GetContentFile(ContentObjectID, clientFileName), clientFileName);
                break;
        }
    }

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
    }
}
