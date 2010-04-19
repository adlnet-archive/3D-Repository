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

public partial class Public_Model : Website.Pages.PageBase
{
    protected int ContentObjectID
    {
        get
        {
            int rv = 0;
            if (Request.QueryString["ContentObjectID"] != null)
            {
                try
                {
                    rv = int.Parse(Request.QueryString["ContentObjectID"].Trim());
                }
                catch (Exception ex)
                {

                }
            }
            else if (ViewState["ContentObjectID"] != null)
            {
                rv = (int)ViewState["ContentObjectID"];
            }

            return rv;
        }
        set { ViewState["ContentObjectID"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {


        if (!(ContentObjectID > 0))
        {
            Response.Redirect("~/Default.aspx");
        }
        
        vwarDAL.vwarDAL vd = new vwarDAL.vwarDAL(Website.Config.EntityConnectionString);
        vwarDAL.ContentObject co = vd.GetContentObjectById(ContentObjectID, !IsPostBack);
        var rootPath = Path.Combine(Path.Combine(Path.Combine(Request.PhysicalApplicationPath, "Content"), ContentObjectID.ToString()), co.Location.Replace(".zip", ""));
        if (Directory.Exists(rootPath))
        {
            var rawPath = Directory.GetFiles(rootPath, "*.o3d", SearchOption.AllDirectories);
            if (rawPath != null && rawPath.Count() > 0)
            {
                var path = rawPath.First();
                path = path.Replace(Request.PhysicalApplicationPath, "").Replace("\\", "/");
                BodyTag.Attributes["onload"] = string.Format("init('../{0}');", path);
                BodyTag.Attributes["onunload"] = "uninit();";
            }
        }
        if (co != null)
        {
            var keywordsList = co.Keywords.Split(new char[] { ',' });

            HtmlControl links = (HtmlControl)ContentObjectFormView.Row.FindControl("keywords");
            HtmlControl keywordLabel = (HtmlControl)ContentObjectFormView.Row.FindControl("keywordLabel");
            Button downloadButton = (Button)ContentObjectFormView.Row.FindControl("DownloadButton");
            downloadButton.Enabled = Request.IsAuthenticated;
            HyperLink descLink = (HyperLink)ContentObjectFormView.Row.FindControl("DescriptionWebsiteURLHyperLink");
            keywordLabel.Visible = !String.IsNullOrEmpty(co.Keywords);
            descLink.Visible = !String.IsNullOrEmpty(co.DescriptionWebsiteURL);
            foreach (var keyword in keywordsList)
            {
                HyperLink link = new HyperLink()
                {
                    Text = keyword,
                    NavigateUrl = "~/Public/Results.aspx?Search=" + Server.UrlEncode(keyword.Trim()),
                    CssClass = "Hyperlink"
                };
                links.Controls.Add(link);
                links.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            }
            if (Context.User.Identity.IsAuthenticated)
            {
                if (co.SubmitterEmail.Equals(Context.User.Identity.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    var editLink = (HyperLink)ContentObjectFormView.FindControl("editLink");
                    editLink.Visible = true;

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
    protected void Rating_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(ratingText.Text))
        {
            vwarDAL.vwarDAL dal = new vwarDAL.vwarDAL(Website.Config.EntityConnectionString);
            dal.InsertReview(rating.CurrentRating, ratingText.Text.Length > 255 ? ratingText.Text.Substring(0, 255)
                : ratingText.Text, Context.User.Identity.Name, ContentObjectID);
            Response.Redirect(Request.RawUrl);
        }
    }
    protected void ContentObjectFormView_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "DownloadZip":
                Label IDLabel = (Label)ContentObjectFormView.Row.FindControl("IDLabel");
                Label LocationLabel = (Label)ContentObjectFormView.Row.FindControl("LocationLabel");
                vwarDAL.vwarDAL vd = new vwarDAL.vwarDAL(Website.Config.EntityConnectionString);
                vd.IncrementDownloads(ContentObjectID);
                string filePath = Website.Common.FormatZipFilePath(IDLabel.Text.Trim(), LocationLabel.Text.Trim());
                string clientFileName = System.IO.Path.GetFileName(filePath);
                Website.Documents.ServeDocument(filePath, clientFileName);
                break;
        }
    }
}
