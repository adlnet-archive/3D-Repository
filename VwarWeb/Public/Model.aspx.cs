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

public partial class Public_Model : Website.Pages.PageBase
{
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

            this.BindModelDetails();

        }


    }

    protected void ReportViolationButton_Click(object sender, EventArgs e)
    {

        Website.Mail.SendReportViolationEmail(this.ContentObjectID, this.TitleLabel.Text.Trim());
        Website.Javascript.Confirm(this.ReportViolationButton, "A message has been sent to the site administator. Click OK to continue");

    }

    private void BindModelDetails()
    {
        if (String.IsNullOrEmpty(ContentObjectID))
        {
            Response.Redirect("~/Default.aspx");
        }

        var uri = Request.Url;
        string proxyTemplate = "Model.ashx?pid={0}&file={1}";
        var factory = new vwarDAL.DataAccessFactory();
        vwarDAL.IDataRepository vd = factory.CreateDataRepositorProxy();
        vwarDAL.ContentObject co = vd.GetContentObjectById(ContentObjectID, !IsPostBack);


        //model screenshot
        if (co != null)
        {
            if ("Model".Equals(co.AssetType, StringComparison.InvariantCultureIgnoreCase))
            {
                BodyTag.Attributes["onload"] = string.Format("LoadAway3D('{2}');", String.Format(proxyTemplate, co.PID, co.Location));
                //BodyTag.Attributes["onunload"] = "uninit();";
                ScreenshotImage.ImageUrl = String.Format(proxyTemplate, co.PID, co.ScreenShot);
            }
            else if ("Texture".Equals(co.AssetType, StringComparison.InvariantCultureIgnoreCase))
            {
                ScreenshotImage.ImageUrl = String.Format(proxyTemplate, co.PID, co.Location);
                tabHeaders.Visible = false;
            }
            else if ("Script".Equals(co.AssetType, StringComparison.InvariantCultureIgnoreCase))
            {
                var bytes = vd.GetContentFileData(co.PID, co.Location);
                using (MemoryStream stream = new MemoryStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Position = 0;
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        scriptDisplay.InnerText = reader.ReadToEnd();
                    }
                }
                tabHeaders.Visible = false;
            }


            IDLabel.Text = co.PID;
            TitleLabel.Text = co.Title;

            //show hide edit link
            if (Context.User.Identity.IsAuthenticated)
            {
                //Show edit link if the users owns the model or if user is Administrator
                if (co.SubmitterEmail.Equals(Context.User.Identity.Name, StringComparison.InvariantCultureIgnoreCase) | Website.Security.IsAdministrator())
                {
                    editLink.Visible = true;
                    editLink.NavigateUrl = "~/Users/Upload.aspx?ContentObjectID=" + co.PID;

                }
                submitRating.Visible = true;

            }
            else
            {
                submitRating.Visible = false;

            }

            //rating
            ir.CurrentRating = Website.Common.CalculateAverageRating(co.PID);

            //description
            DescriptionLabel.Text = co.Description;
            this.DescriptionRow.Visible = string.IsNullOrEmpty(co.Description) ? false : true;
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
            this.KeywordsRow.Visible = !string.IsNullOrEmpty(co.Keywords);

            //more details
            this.MoreDetailsHyperLink.NavigateUrl = co.MoreInformationURL;
            this.MoreDetailsHyperLink.Text = co.MoreInformationURL;
            this.MoreDetailsRow.Visible = !string.IsNullOrEmpty(co.MoreInformationURL);

            //submitter email & uploaded date
            SubmitterEmailHyperLink.NavigateUrl = "mailto:" + co.SubmitterEmail;
            SubmitterEmailHyperLink.Text = co.SubmitterEmail;
            if (co.UploadedDate != null)
            {
                UploadedDateLabel.Text = "Uploaded by: " + co.SubmitterEmail + " on " + co.UploadedDate.ToString();
            }



            //sponsor logo
            if (!string.IsNullOrEmpty(co.SponsorLogoImageFileName))
            {
                //this.SponsorLogoImage.ImageUrl = String.Format(proxyTemplate, co.PID, co.SponsorLogoImageFileName);
            }

            //TODO:Uncomment
            //this.SponsorLogoRow.Visible = !string.IsNullOrEmpty(co.SponsorLogoImageFileName);

            //sponsor name -changed hyperlink to label
            //this.SponsorNameHyperLink.NavigateUrl = "~/Public/Results.aspx?ContentObjectID=" + ContentObjectID + "&SponsorName=" + Server.UrlEncode(co.SponsorName);
            //this.SponsorNameHyperLink.Text = co.SponsorName;

            this.SponsorNameLabel.Text = co.SponsorName;

            //TODO:Uncomment
            //this.SponsorNameRow.Visible = !string.IsNullOrEmpty(co.SponsorName);

            //developr logo
            if (!string.IsNullOrEmpty(co.DeveloperLogoImageFileName))
            {
                //this.DeveloperLogoImage.ImageUrl = String.Format(proxyTemplate, co.PID, co.DeveloperLogoImageFileName);
            }

            //TODO:Uncomment
            // this.DeveloperLogoRow.Visible = !string.IsNullOrEmpty(co.DeveloperLogoImageFileName);

            //developer name
            this.DeveloperNameHyperLink.NavigateUrl = "~/Public/Results.aspx?ContentObjectID=" + ContentObjectID + "&DeveloperName=" + Server.UrlEncode(co.DeveloperName);
            this.DeveloperNameHyperLink.Text = co.DeveloperName;
            this.DeveloperRow.Visible = !string.IsNullOrEmpty(co.DeveloperName);


            this.FormatLabel.Text = "Format: " + co.Format;

            //artist
            //this.ArtistNameHyperLink.NavigateUrl = "~/Public/Results.aspx?ContentObjectID=" + ContentObjectID + "&Artist=" + Server.UrlEncode(co.ArtistName);
            //this.ArtistNameHyperLink.Text = co.ArtistName;
            //this.ArtistRow.Visible = !string.IsNullOrEmpty(co.ArtistName);

            //num polygons   
            this.NumPolygonsLabel.Text = co.NumPolygons.ToString();
            this.NumPolygonsRow.Visible = !string.IsNullOrEmpty(co.NumPolygons.ToString());

            //num textures
            this.NumTexturesLabel.Text = co.NumTextures.ToString();
            this.NumTexturesRow.Visible = !string.IsNullOrEmpty(co.NumTextures.ToString());

            //cclrow
            this.CCLHyperLink.Visible = !string.IsNullOrEmpty(co.CreativeCommonsLicenseURL);
            this.CCLHyperLink.NavigateUrl = co.CreativeCommonsLicenseURL;

            //this.CCLRow.Visible = !string.IsNullOrEmpty(co.CreativeCommonsLicenseURL);

            if (!string.IsNullOrEmpty(co.CreativeCommonsLicenseURL))
            {


                switch (co.CreativeCommonsLicenseURL.ToLower().Trim())
                {
                    case "http://creativecommons.org/licenses/by-nc-sa/3.0/legalcode":

                        this.CCLHyperLink.ImageUrl = "~/Images/Attribution Non-Commercial Share Alike.png";
                        this.CCLHyperLink.ToolTip = "by-nc-sa";

                        break;

                    case "http://creativecommons.org/licenses/by-nc-nd/3.0/legalcode":
                        this.CCLHyperLink.ImageUrl = "~/Images/Attribution Non-Commercial No Derivatives.png";
                        this.CCLHyperLink.ToolTip = "by-nc-nd";
                        break;

                    case "http://creativecommons.org/licenses/by-nc/3.0/legalcode":
                        this.CCLHyperLink.ImageUrl = "~/Images/Attribution Non-Commercial.png";
                        this.CCLHyperLink.ToolTip = "by-nc";
                        break;

                    case "http://creativecommons.org/licenses/by-nd/3.0/legalcode":
                        this.CCLHyperLink.ImageUrl = "~/Images/Attribution No Derivatives.png";
                        this.CCLHyperLink.ToolTip = "by-nd";
                        break;

                    case "http://creativecommons.org/licenses/by-sa/3.0/legalcode":
                        this.CCLHyperLink.ImageUrl = "~/Images/Attribution Share Alike.png";
                        this.CCLHyperLink.ToolTip = "by-sa";
                        break;

                }

            }





            //downloads
            DownloadsLabel.Text = co.Downloads.ToString();
            this.DownloadsRow.Visible = !string.IsNullOrEmpty(co.Downloads.ToString());

            //views
            ViewsLabel.Text = co.Views.ToString();
            this.ViewsRow.Visible = !string.IsNullOrEmpty(co.Views.ToString());

            //last modified
            //if (co.LastModified != null)
            //{
            //    LastModifiedLabel.Text = co.LastModified.ToString();
            //    this.LastModifiedRow.Visible = true;
            //}
            //else
            //{
            //    this.LastModifiedRow.Visible = false;
            //}


            ////location - don't show
            //this.LocationLabel.Text = co.Location;
            //this.LocationRow.Visible = false;

            //download buton
            this.DownloadButton.Visible = Context.User.Identity.IsAuthenticated;


            this.CommentsGridView.DataSource = co.Reviews;
            this.CommentsGridView.DataBind();
        }




    }

    private const string RATINGKEY = "rating";

    protected void Rating_Set(object sender, RatingEventArgs args)
    {
        Session[RATINGKEY] = args.Value;
    }

    protected void Rating_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(ratingText.Text))
        {
            var factory = new vwarDAL.DataAccessFactory();
            vwarDAL.IDataRepository vd = factory.CreateDataRepositorProxy();

            var ratingValue = rating.CurrentRating;
            var tempRating = Session[RATINGKEY];
            if (tempRating != null)
            {
                ratingValue = Convert.ToInt32(tempRating.ToString());
            }
            vd.InsertReview(ratingValue, ratingText.Text.Length > 255 ? ratingText.Text.Substring(0, 255)
                : ratingText.Text, Context.User.Identity.Name, ContentObjectID);
            Session[RATINGKEY] = null;
            Response.Redirect(Request.RawUrl);
        }
    }

    protected void ContentObjectFormView_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "DownloadZip":
                Label IDLabel = (Label)FindControl("IDLabel");
                Label LocationLabel = (Label)FindControl("LocationLabel");
                var factory = new vwarDAL.DataAccessFactory();
                vwarDAL.IDataRepository vd = factory.CreateDataRepositorProxy();
                vd.IncrementDownloads(ContentObjectID);
                string filePath = Website.Common.FormatZipFilePath(IDLabel.Text.Trim(), LocationLabel.Text.Trim());
                string clientFileName = System.IO.Path.GetFileName(filePath);
                Website.Documents.ServeDocument(filePath, clientFileName);
                break;
        }
    }

    protected void DownloadButton_Click(object sender, EventArgs e)
    {
        var factory = new vwarDAL.DataAccessFactory();
        vwarDAL.IDataRepository vd = factory.CreateDataRepositorProxy();
        var co = vd.GetContentObjectById(ContentObjectID, false);
        var url = vd.GetContentUrl(co.PID, co.Location);
        vd.IncrementDownloads(ContentObjectID);
        if (String.IsNullOrEmpty(ModelTypeDropDownList.SelectedValue))
        {
            Website.Documents.ServeDocument(url, co.Location);
        }
        else
        {
            Website.Documents.ServeDocument(url, co.Location, null, ModelTypeDropDownList.SelectedValue);
        }
    }
}
