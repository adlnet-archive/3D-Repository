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
    //readonly private String CONTENTOBJECTIDPARAM = "ContentObjectID";
    //protected string ContentObjectID
    //{
    //    get
    //    {
    //        string rv = "";
    //        if (!String.IsNullOrEmpty(Request.QueryString[CONTENTOBJECTIDPARAM]))
    //        {
    //            rv = (Request.QueryString[CONTENTOBJECTIDPARAM]);
    //        }
    //        else if (ViewState[CONTENTOBJECTIDPARAM] != null)
    //        {
    //            rv = ViewState[CONTENTOBJECTIDPARAM].ToString();
    //        }

    //        return rv;
    //    }
    //    set { ViewState[CONTENTOBJECTIDPARAM] = value; }
    //}

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    if (!Context.User.Identity.IsAuthenticated)
    //    {
    //        Response.Redirect("~/Default.aspx");
    //    }
    //    if ((!String.IsNullOrEmpty(ContentObjectID)) && !IsPostBack)
    //    {
    //        var factory = new vwarDAL.DataAccessFactory();
    //        vwarDAL.IDataRepository dal = factory.CreateDataRepositorProxy();
    //        var co = dal.GetContentObjectById(ContentObjectID, false);
    //        if (!co.SubmitterEmail.Equals(Context.User.Identity.Name, StringComparison.InvariantCultureIgnoreCase))
    //        {
    //            Response.Redirect("~/Default.aspx");
    //        }

    //        //disable the required field validators
    //        ContentFileRequiredFieldValidator.Enabled = false;
    //        ThumbnailFileRequiredFieldValidator.Enabled = false;
           
    //        //bind controls to the cotent object
    //        title.Text = co.Title;
    //        description.Text = co.Description;
    //        keywords.Text = co.Keywords;
    //        infoLink.Text = co.MoreInformationURL;
    //    }
    //    SearchPanel.Visible = false;
    //}

    //public void btnSubmit_Click(object sender, EventArgs e)
    //{
    //    var factory = new vwarDAL.DataAccessFactory();
    //    vwarDAL.IDataRepository dal = factory.CreateDataRepositorProxy();
    //    if (String.IsNullOrEmpty(ContentObjectID))
    //    {
    //        SaveNewContentObject(dal);
    //    }
    //    else
    //    {
    //        UpdateContentObject(dal);
    //    }
    //}
    //private void UpdateContentObject(vwarDAL.IDataRepository dal)
    //{
    //    var co = CreateContentObjectFromUi();
    //    try
    //    {
    //        co.PID = ContentObjectID;
    //        dal.UpdateContentObject(co);
    //    }
    //    catch (ArgumentException ex)
    //    {
    //        errorMessage.Text = ex.Message;
    //    }
    //}
    //private void SaveNewContentObject(vwarDAL.IDataRepository dal)
    //{
    //    vwarDAL.ContentObject co = CreateContentObjectFromUi();
    //    try
    //    {
    //        var saveMainFilePath = SaveFile(contentFile.FileContent, contentFile.FileName);
    //        string ext = System.IO.Path.GetExtension(saveMainFilePath).ToLower();
    //        if (ext.Equals(".zip", StringComparison.InvariantCultureIgnoreCase))
    //        {
    //            var path = ExtractFile(saveMainFilePath);
    //            foreach (var file in Directory.GetFiles(path, "*.dae"))
    //            {
    //                var output = ConvertFileToO3D(file);
    //                co.DisplayFile = output;
    //                break;
    //            }
    //        }
    //        co.SponsorLogoImageFilePath = SubmitterLogoImageFilePath.FileName;
    //        var displayFilePath = co.DisplayFile;
    //        co.DisplayFile = Path.GetFileName(co.DisplayFile);
    //        dal.InsertContentObject(co);
    //        dal.UploadFile(displayFilePath, co.PID, (co.DisplayFile));

    //        dal.UploadFile(saveMainFilePath, co.PID, contentFile.FileName);
    //        dal.UploadFile(SaveFile(thumbnailFile.FileContent, thumbnailFile.FileName), co.PID, thumbnailFile.FileName);
    //        if (SubmitterLogoImageFilePath.FileContent.Length > 0 && !String.IsNullOrEmpty(SubmitterLogoImageFilePath.FileName))
    //        {
    //            dal.UploadFile(SaveFile(SubmitterLogoImageFilePath.FileContent, SubmitterLogoImageFilePath.FileName), co.PID, SubmitterLogoImageFilePath.FileName);
    //        }
    //        //save ID to redirect to model view after confirmation
    //        ContentObjectID = co.PID;
    //    }
    //    catch (ArgumentException ex)
    //    {
    //        errorMessage.Text = ex.Message;
    //    }
    //}
    //private string SaveFile(Stream stream, string fileName)
    //{
    //    string savePath = Path.Combine(Path.GetTempPath(), fileName);
    //    if (Directory.Exists(savePath)) return savePath;
    //    byte[] data = new byte[stream.Length];
    //    stream.Read(data, 0, data.Length);
    //    using (FileStream fstream = new FileStream(savePath, FileMode.Create))
    //    {
    //        fstream.Write(data, 0, data.Length);
    //    }
    //    return savePath;
    //}
    //private vwarDAL.ContentObject CreateContentObjectFromUi()
    //{
    //    vwarDAL.ContentObject co = null;
    //    if (String.IsNullOrEmpty(ContentObjectID))
    //    {
    //        co = new vwarDAL.ContentObject()
    //         {
    //             Title = title.Text.Trim(),
    //             Location = contentFile.FileName.Trim(),
    //             ScreenShot = thumbnailFile.FileName.Trim(),
    //             //CollectionName = collection.SelectedValue.Trim(),
    //             UploadedDate = DateTime.Now,
    //             LastModified = DateTime.Now,
    //             LastViewed = DateTime.Now,
    //             Description = description.Text.Trim().Length > 255 ? description.Text.Trim().Substring(0, 255) : description.Text.Trim(),
    //             SponsorLogoImageFilePath = SubmitterLogoImageFilePath.FileName.Trim(),
    //             MoreInformationURL = infoLink.Text.Trim().Length > 255 ? infoLink.Text.Trim().Substring(0, 255) : infoLink.Text.Trim(),
    //             Keywords = keywords.Text.Trim().Length > 255 ? keywords.Text.Trim().Substring(0, 255) : keywords.Text.Trim(),
    //             Views = 0,
    //             SubmitterEmail = HttpContext.Current.User.Identity.Name.Trim()
    //         };
    //    }
    //    else
    //    {
    //        var factory = new vwarDAL.DataAccessFactory();
    //        vwarDAL.IDataRepository dal = factory.CreateDataRepositorProxy();
    //        co = dal.GetContentObjectById(ContentObjectID, false);
    //        co.Title = title.Text.Trim();
    //        //co.CollectionName = collection.SelectedValue.Trim();
    //        co.LastModified = DateTime.Now;
    //        co.Description = description.Text.Trim().Length > 255 ? description.Text.Trim().Substring(0, 255) : description.Text.Trim();
    //        co.MoreInformationURL = infoLink.Text.Trim().Length > 255 ? infoLink.Text.Trim().Substring(0, 255) : infoLink.Text.Trim();
    //        co.Keywords = keywords.Text.Trim().Length > 255 ? keywords.Text.Trim().Substring(0, 255) : keywords.Text.Trim();

    //        co.Location = String.IsNullOrEmpty(contentFile.FileName.Trim()) ? co.Location : contentFile.FileName.Trim();
    //        co.ScreenShot = String.IsNullOrEmpty(thumbnailFile.FileName.Trim()) ? co.ScreenShot : thumbnailFile.FileName.Trim();
    //        co.SponsorLogoImageFilePath = String.IsNullOrEmpty(SubmitterLogoImageFilePath.FileName.Trim()) ? co.SponsorLogoImageFilePath : SubmitterLogoImageFilePath.FileName.Trim();
    //    }
    //    return co;
    //}
    //private string ConvertFileToO3D(string path)
    //{
    //    var application = Path.Combine(Path.Combine(Request.PhysicalApplicationPath, "bin"), "o3dConverter.exe");
    //    System.Diagnostics.ProcessStartInfo processInfo = new System.Diagnostics.ProcessStartInfo(application);
    //    processInfo.Arguments = String.Format("\"{0}\" \"{1}\"", path, path.ToLower().Replace("dae", "o3d"));
    //    processInfo.WindowStyle = ProcessWindowStyle.Hidden;
    //    processInfo.RedirectStandardError = true;
    //    processInfo.CreateNoWindow = true;
    //    processInfo.UseShellExecute = false;
    //    var p = Process.Start(processInfo);
    //    var error = p.StandardError.ReadToEnd();
    //    return path.ToLower().Replace("dae", "o3d");
    //}
    //private void ConvertToVastpark(string path)
    //{
    //    var application = Path.Combine(Path.Combine(Request.PhysicalApplicationPath, "bin"), "ModelPackager.exe");
    //    System.Diagnostics.ProcessStartInfo processInfo = new System.Diagnostics.ProcessStartInfo(application);
    //    processInfo.Arguments = path;
    //    processInfo.WindowStyle = ProcessWindowStyle.Hidden;
    //    processInfo.RedirectStandardError = true;
    //    processInfo.CreateNoWindow = true;
    //    processInfo.UseShellExecute = false;
    //    var p = Process.Start(processInfo);
    //    var error = p.StandardError.ReadToEnd();
    //}
    //private string ExtractFile(string path)
    //{
    //    var destPath = path.Replace(".zip", string.Empty);
    //    using (Ionic.Zip.ZipFile zipFile = new Ionic.Zip.ZipFile(path))
    //    {
    //        zipFile.ExtractAll(destPath, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
    //    }
    //    return destPath;
    //}
    //protected void btnCancel_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("~/Default.aspx");
    //}
    //protected void ContinueButton_Click(object sender, EventArgs e)
    //{
    //    //redirect to model view with new ContentObjectID
    //    Response.Redirect("~/Public/Model.aspx?ContentObjectID=" + ContentObjectID.ToString());
    //}
}
