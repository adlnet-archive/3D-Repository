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
using System.Web.Script.Services;
using System.Web.Services;
/// <summary>
/// Summary description for RebuildThumbnailCache
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class RebuildThumbnailCache : System.Web.Services.WebService {

    public RebuildThumbnailCache () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    static IEnumerable<vwarDAL.ContentObject> allpids = null;
    static vwarDAL.IDataRepository dal = null;
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ArrayList GetAllPIDS()
    {
        dal = (new vwarDAL.DataAccessFactory()).CreateDataRepositorProxy();
        allpids = dal.GetAllContentObjects();
        ArrayList list = new ArrayList();
        foreach (vwarDAL.ContentObject co in allpids)
        {
            list.Add(co.PID);
        }
        return list;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public string UpdateThumbnailCache(string pid)
    {
      
        foreach (vwarDAL.ContentObject co in allpids)
        {
            if (co.PID == pid)
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
                            return "No screenshot data";
                        }
                    }
                    else
                    {
                        return "No screenshot data";
                    }
                }
                catch (System.Exception ex)
                {
                    return ex.Message;
                }
        }
        return "OK";
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {

    }
    
    
}
