using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;
using vwarDAL;
using System.Collections.Generic;

public partial class Users_RebuildThumbnailCache : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
      

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (User.Identity.IsAuthenticated && Roles.IsUserInRole("Administrators"))
        {

            vwarDAL.IDataRepository dal = (new vwarDAL.DataAccessFactory()).CreateDataRepositorProxy();
            IEnumerable<vwarDAL.ContentObject> allpids = dal.GetAllContentObjects();

            foreach (vwarDAL.ContentObject co in allpids)
            {
                ListBox1.Items.Add("Processing " + co.PID);
                try
                {
                    System.IO.Stream screenshotdata = dal.GetContentFile(co.PID, co.ScreenShot);
                    if (screenshotdata != null)
                    {
                        int length = (int)screenshotdata.Length;

                        if (length != 0)
                        {

                            string ext = new FileInfo(co.ScreenShot).Extension.ToLower();
                            System.Drawing.Imaging.ImageFormat format;
                            format = System.Drawing.Imaging.ImageFormat.Png;
                            if (ext == ".png")
                                format = System.Drawing.Imaging.ImageFormat.Png;
                            else if (ext == ".jpg")
                                format = System.Drawing.Imaging.ImageFormat.Jpeg;
                            else if (ext == ".gif")
                                format = System.Drawing.Imaging.ImageFormat.Gif;



                            //Use the original file bytes to remain consistent with the new file upload ID creation for thumbnails
                            co.ThumbnailId = Website.Common.GetFileSHA1AndSalt(screenshotdata) + ext;
                            dal.UpdateContentObject(co);

                            try
                            {
                                File.Delete(HttpContext.Current.Server.MapPath("~/thumbnails/" + co.ThumbnailId));
                            }
                            catch (System.IO.FileNotFoundException t)
                            {

                            }

                            using (FileStream outFile = new FileStream(HttpContext.Current.Server.MapPath("~/thumbnails/" + co.ThumbnailId), FileMode.Create))
                                Website.Common.GenerateThumbnail(screenshotdata, outFile, format);
                        }
                        else
                        {
                            ListBox1.Items.Add("No screenshot data");
                        }
                    }
                    else
                    {
                        ListBox1.Items.Add("No screenshot data");
                    }
                }
                catch (System.Exception ex)
                {
                    ListBox1.Items.Add(ex.Message);
                }
            }
        }
    }
}