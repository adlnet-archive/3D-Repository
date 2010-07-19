<%@ WebHandler Language="C#" Class="Model" %>

using System;
using System.Web;
using System.Configuration;
using System.IO;
public class Model : IHttpHandler
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
        var pid = context.Request.QueryString["pid"];
        var fileName = context.Request.QueryString["file"];
        var factory = new vwarDAL.DataAccessFactory();
        vwarDAL.IDataRepository vd = factory.CreateDataRepositorProxy();
        var url = vd.GetContentUrl(pid, fileName);
        if (String.IsNullOrEmpty(url)) return;
        HttpResponse _response = HttpContext.Current.Response;
        var creds = new System.Net.NetworkCredential(FedoraUserName, FedoraPasswrod);
        _response.Clear();
        _response.AppendHeader("content-disposition", "attachment; filename=" + fileName);
        _response.ContentType = vwarDAL.FedoraCommonsRepo.GetMimeType(fileName);
        string localPath = Path.GetTempFileName();
        using (System.Net.WebClient client = new System.Net.WebClient())
        {
            try
            {
                client.Credentials = creds;
                client.DownloadFile(url, localPath);
            }
            catch { }

        }
        if (File.Exists(localPath))
        {
            _response.WriteFile(localPath);
        }
        _response.End();
        File.Delete(localPath);

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}