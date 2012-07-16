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
    
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ArrayList GetAllPIDS()
    {
        vwarDAL.IDataRepository dal = (new vwarDAL.DataAccessFactory()).CreateDataRepositorProxy();
        allpids = dal.GetAllContentObjects();
        ArrayList list = new ArrayList();
        foreach (vwarDAL.ContentObject co in allpids)
        {
            list.Add(co.PID);
        }
        dal.Dispose();
        return list;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public string UpdateThumbnailCache(string pid)
    {
       
        vwarDAL.IDataRepository dal = (new vwarDAL.DataAccessFactory()).CreateDataRepositorProxy();

        APIWrapper api = null;
        if (Membership.GetUser() != null && Membership.GetUser().IsApproved)
            api = new APIWrapper(Membership.GetUser().UserName, null);
        else
            api = new APIWrapper(vwarDAL.DefaultUsers.Anonymous[0], null);

        foreach (vwarDAL.ContentObject co in allpids)
        {
            if (co.PID == pid)
                try
                {
                    System.IO.Stream screenshotdata = api.GetScreenshot(co.PID, "00-00-00");
                    if (screenshotdata != null)
                    {
                        int length = (int)screenshotdata.Length;

                        if (length != 0)
                        {
                            byte[] data = new byte[screenshotdata.Length];
                            screenshotdata.Seek(0,SeekOrigin.Begin);
                            screenshotdata.Read(data,0,length);
                            api.UploadScreenShot(data, co.PID, co.ScreenShot, "00-00-00");
                        }
                        else
                        {
                            dal.Dispose();
                            return "No screenshot data";
                        }
                    }
                    else
                    {
                        dal.Dispose();
                        return "No screenshot data";
                    }
                }
                catch (System.Exception ex)
                {
                    return ex.Message;
                }
        }
        dal.Dispose();
        return "OK";
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {

    }
    
    
}
