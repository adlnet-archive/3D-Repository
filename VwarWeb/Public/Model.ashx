<%@ WebHandler Language="C#" Class="Model" %>

using System;
using System.Web;
using System.Configuration;
using System.IO;
using System.Web.SessionState;
public class Model : IHttpHandler, IReadOnlySessionState
{
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
    public void ProcessRequest(HttpContext context)
    {
        HttpResponse _response = HttpContext.Current.Response;
        var session = context.Request.QueryString["Session"];
        if (session == "true")
        {
            Utility_3D.ConvertedModel model = (Utility_3D.ConvertedModel)context.Session["Model"];
            _response.BinaryWrite(model.data);
            return;
        }
        
        var pid = context.Request.QueryString["pid"];
        var fileName = context.Request.QueryString["file"];
        var factory = new vwarDAL.DataAccessFactory();
        vwarDAL.IDataRepository vd = factory.CreateDataRepositorProxy();

        var url = "";
        if (!String.IsNullOrEmpty(context.Request.QueryString["Cache"]))
        {
            url = vd.FormatContentUrl(pid, fileName);
        }
        else
        {
            url = vd.GetContentUrl(pid, fileName);
        }
        if (String.IsNullOrEmpty(url)) return;
       
        var creds = new System.Net.NetworkCredential(FedoraUserName, FedoraPasswrod);
        _response.Clear();
        _response.AppendHeader("content-disposition", "attachment; filename=" + fileName);
        _response.ContentType = vwarDAL.FedoraCommonsRepo.GetMimeType(fileName);
       // string localPath = Path.GetTempFileName();
        using (System.Net.WebClient client = new System.Net.WebClient())
        {
            try
            {
                client.Credentials = creds;
                //client.DownloadFile(url, localPath);
                _response.BinaryWrite(client.DownloadData(url));
            }
            catch { }

        }
       // if (File.Exists(localPath))
       // {
      //      _response.WriteFile(localPath);
       // }
        _response.End();
      //  File.Delete(localPath);

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}