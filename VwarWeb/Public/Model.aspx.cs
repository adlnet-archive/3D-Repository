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
        if (!Page.IsPostBack)
        {
            this.BindModelDetails();

        }


    }
    protected void ReportViolationButton_Click(object sender, EventArgs e)
    {

        Website.Mail.SendReportViolationEmail(this.ContentObjectID, this.TitleLabel.Text.Trim());


    }

    private void BindModelDetails()
    {
        if (String.IsNullOrEmpty(ContentObjectID))
        {
            Response.Redirect("~/Default.aspx");
        }
        const string proxyTemplate = "Model.ashx?pid={0}&file={1}";
        var factory = new vwarDAL.DataAccessFactory();
        vwarDAL.IDataRepository vd = factory.CreateDataRepositorProxy();
        vwarDAL.ContentObject co = vd.GetContentObjectById(ContentObjectID, !IsPostBack);
        if (co != null)
        {
            if ("Model".Equals(co.AssetType, StringComparison.InvariantCultureIgnoreCase))
            {
                BodyTag.Attributes["onload"] = string.Format("init('{0}','{1}');", String.Format(proxyTemplate, co.PID, co.DisplayFile), "");
                BodyTag.Attributes["onunload"] = "uninit();";
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
            ir.CurrentRating = Website.Common.CalculateAverageRating(co.PID);
            var keywordsList = String.IsNullOrEmpty(co.Keywords) ? new String[0] : co.Keywords.Split(new char[] { ',' });


            IDLabel.Text = co.PID;
            TitleLabel.Text = co.Title;
            DescriptionLabel.Text = co.Description;
            DownloadButton.Enabled = Request.IsAuthenticated;
            keywordLabel.Visible = !String.IsNullOrEmpty(co.Keywords);
            DescriptionWebsiteURLHyperLink.NavigateUrl = co.MoreInformationURL;
            SubmitterEmailHyperLink.NavigateUrl = "mailto:" + co.SubmitterEmail;
            SubmitterEmailHyperLink.Text = co.SubmitterEmail;
            Label13.Text = co.Views.ToString();
            Label12.Text = co.Downloads.ToString();
            LastModifiedLabel.Text = co.LastModified.ToString();
            UploadedDateLabel.Text = co.UploadedDate.ToString();

            //TODO: populate sponsor logo
            //SubmitterLogoImageFilePathImage.ImageUrl = String.Format(proxyTemplate, co.PID, co.SubmitterLogoImageFilePath);





            foreach (var keyword in keywordsList)
            {
                HyperLink link = new HyperLink()
                {
                    Text = keyword,
                    NavigateUrl = "~/Public/Results.aspx?Search=" + Server.UrlEncode(keyword.Trim()),
                    CssClass = "Hyperlink"
                };
                keywords.Controls.Add(link);
                keywords.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            }
            if (Context.User.Identity.IsAuthenticated)
            {
                //Show edit link if the users owns the model or if user is Administrator
                if (co.SubmitterEmail.Equals(Context.User.Identity.Name, StringComparison.InvariantCultureIgnoreCase) | Website.Security.IsAdministrator())
                {
                    editLink.Visible = true;
                    editLink.NavigateUrl = "~/Users/Upload.aspx?ContentObjectID=" + co.PID;

                }
                submitRating.Enabled = true;
            }
            else
            {
                submitRating.Enabled = false;
            }

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
        Website.Documents.ServeDocument(url, co.Location);
    }
}
