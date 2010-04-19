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

public partial class Users_Upload : Website.Pages.PageBase
{
    readonly private String CONTENTOBJECTIDPARAM = "ContentObjectID";
    protected int ContentObjectID
    {
        get
        {
            int rv = 0;
            if (!String.IsNullOrEmpty(Request.QueryString[CONTENTOBJECTIDPARAM]))
            {
                rv = int.Parse(Request.QueryString[CONTENTOBJECTIDPARAM]);
            }
            else if (ViewState[CONTENTOBJECTIDPARAM] != null)
            {
                rv = (int)ViewState[CONTENTOBJECTIDPARAM];
            }

            return rv;
        }
        set { ViewState[CONTENTOBJECTIDPARAM] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Context.User.Identity.IsAuthenticated)
        {
            Response.Redirect("~/Default.aspx");
        }
        if (ContentObjectID > 0 && !IsPostBack)
        {
            vwarDAL.vwarDAL dal = new vwarDAL.vwarDAL(Website.Config.EntityConnectionString);
            var co = dal.GetContentObjectById(ContentObjectID, false);
            if (!co.SubmitterEmail.Equals(Context.User.Identity.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                Response.Redirect("~/Default.aspx");
            }
            RequiredFieldValidator1.Enabled = false;
            RequiredFieldValidator2.Enabled = false;
            title.Text = co.Title;
            description.Text = co.Description;
            keywords.Text = co.Keywords;
            infoLink.Text = co.DescriptionWebsiteURL;
        }
        SearchPanel.Visible = false;
    }

    public void btnSubmit_Click(object sender, EventArgs e)
    {
        vwarDAL.vwarDAL dal = new vwarDAL.vwarDAL(Website.Config.EntityConnectionString);
        if (ContentObjectID <= 0)
        {
            SaveNewContentObject(dal);
        }
        else
        {
            UpdateContentObject(dal);
        }
    }
    private void UpdateContentObject(vwarDAL.vwarDAL dal)
    {
        var co = CreateContentObjectFromUi();
        try
        {
            co.Id = ContentObjectID;
            if (SaveFiles(dal, co))
            {
                dal.UpdateContentObject(co);
                errorMessage.Text = string.Empty;
                //show confirmation
                ConfirmationLabel.Text = "The model '" + title.Text.Trim() + "' was successfully uploaded.";
                MultiView1.SetActiveView(ConfirmationView);
            }
        }
        catch (ArgumentException ex)
        {
            errorMessage.Text = ex.Message;
        }
    }
    private void SaveNewContentObject(vwarDAL.vwarDAL dal)
    {
        vwarDAL.ContentObject co = CreateContentObjectFromUi();
        try
        {
            dal.InsertContentObject(co);
            //save ID to redirect to model view after confirmation
            ContentObjectID = co.Id;
            if (SaveFiles(dal, co))
            {
                errorMessage.Text = string.Empty;
                //show confirmation
                ConfirmationLabel.Text = "The model '" + title.Text.Trim() + "' was successfully uploaded.";
                MultiView1.SetActiveView(ConfirmationView);
            }
        }
        catch (ArgumentException ex)
        {
            errorMessage.Text = ex.Message;
        }
    }

    private vwarDAL.ContentObject CreateContentObjectFromUi()
    {
        vwarDAL.ContentObject co = new vwarDAL.ContentObject()
        {
            Title = title.Text.Trim(),
            Location = contentFile.FileName.Trim(),
            ScreenShot = thumbnailFile.FileName.Trim(),
            CollectionName = collection.SelectedValue.Trim(),
            UploadedDate = DateTime.Now,
            LastModified = DateTime.Now,
            LastViewed = DateTime.Now,
            Description = description.Text.Trim().Length > 255 ? description.Text.Trim().Substring(0, 255) : description.Text.Trim(),
            SubmitterLogoImageFilePath = SubmitterLogoImageFilePath.FileName.Trim(),
            DescriptionWebsiteURL = infoLink.Text.Trim().Length > 255 ? infoLink.Text.Trim().Substring(0, 255) : infoLink.Text.Trim(),
            Keywords = keywords.Text.Trim().Length > 255 ? keywords.Text.Trim().Substring(0, 255) : keywords.Text.Trim(),
            Views = 0,
            SubmitterEmail = HttpContext.Current.User.Identity.Name.Trim()
        };
        return co;
    }

    private bool SaveFiles(vwarDAL.vwarDAL dal, vwarDAL.ContentObject co)
    {
        String path = Path.Combine(Path.Combine(Request.PhysicalApplicationPath, "Content"), co.Id.ToString());
        if (!Directory.Exists(path))
        {
            var dirPath = Directory.CreateDirectory(path).FullName;
        }
        if (!String.IsNullOrEmpty(contentFile.FileName))
        {
            WriteFile(contentFile.FileBytes, contentFile.FileName, path);
            var destPath = ExtractFile(Path.Combine(path, contentFile.FileName));
            if (!ProcessColladaModels(destPath))
            {
                dal.DeleteContentObject(co.Id);
                return false;
            }
        }
        if (!String.IsNullOrEmpty(thumbnailFile.FileName))
        {
            WriteFile(thumbnailFile.FileBytes, thumbnailFile.FileName, path);
        }

        path = Path.Combine(Path.Combine(Request.PhysicalApplicationPath, "Images"), "SubmitterLogos");
        if (!String.IsNullOrEmpty(SubmitterLogoImageFilePath.FileName) && SubmitterLogoImageFilePath.FileName.Contains('.'))
        {
            WriteFile(SubmitterLogoImageFilePath.FileBytes, co.Id + SubmitterLogoImageFilePath.FileName.Substring(SubmitterLogoImageFilePath.FileName.LastIndexOf(".")), path);
        }
        return true;

    }
    private bool ProcessColladaModels(string path)
    {
        var files = Directory.GetFiles(path, "*.dae", SearchOption.AllDirectories);
        if (files.Count() <= 0)
        {
            errorMessage.Text = "There were no valid collada files found in the package";
            return false;
        }
        foreach (var file in files)
        {
            ConvertFileToO3D(file);
        }
        return true;
    }
    private void ConvertFileToO3D(string path)
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
    }
    private string ExtractFile(string path)
    {
        var destPath = path.Replace(".zip", string.Empty);
        using (Ionic.Zip.ZipFile zipFile = new Ionic.Zip.ZipFile(path))
        {
            zipFile.ExtractAll(destPath, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
        }
        return destPath;
    }
    private void WriteFile(Byte[] content, string fileName, string path)
    {
        using (FileStream writer = new FileStream(Path.Combine(path, fileName), FileMode.Create))
        {
            writer.Write(content, 0, content.Length);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }
    protected void ContinueButton_Click(object sender, EventArgs e)
    {
        //redirect to model view with new ContentObjectID
        Response.Redirect("~/Public/Model.aspx?ContentObjectID=" + ContentObjectID.ToString());
    }
}
