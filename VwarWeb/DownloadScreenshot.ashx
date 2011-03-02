<%@ WebHandler Language="C#" Class="DownloadScreenshot" %>

using System;
using System.Web;
using System.Configuration;
public class DownloadScreenshot : IHttpHandler {

    private string FedoraUserName
    {
        get
        {
            return (ConfigurationManager.AppSettings["fedoraUserName"]);
        }
    }
    private string FedoraPasswrod
    {
        get
        {
            return (ConfigurationManager.AppSettings["fedoraPassword"]);
        }
    }
    
    public void ProcessRequest (HttpContext context) {
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");


        var pid = context.Request.QueryString["PID"];
       
            var factory = new vwarDAL.DataAccessFactory();
            vwarDAL.IDataRepository vd = factory.CreateDataRepositorProxy();

            vwarDAL.ContentObject co = vd.GetContentObjectById(pid, false);
            string fileName = co.ScreenShot;

            var creds = new System.Net.NetworkCredential(FedoraUserName, FedoraPasswrod);
            context.Response.Clear();
            context.Response.AppendHeader("content-disposition", "attachment; filename=" + fileName);
            context.Response.ContentType = vwarDAL.FedoraCommonsRepo.GetMimeType(fileName);
            // string localPath = Path.GetTempFileName();
            using (System.IO.Stream s = vd.GetContentFile(pid,fileName))
            {
                try
                {
                    byte[] data = new byte[s.Length];
                    s.Read(data,0,data.Length);
                    context.Response.BinaryWrite(data);
                }
                catch { }

            }

           
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}