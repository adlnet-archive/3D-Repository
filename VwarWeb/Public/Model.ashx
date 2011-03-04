<%@ WebHandler Language="C#" Class="Model" %>

using System;
using System.Web;
using System.Configuration;
using System.IO;
using System.Web.SessionState;
using vwarDAL;

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
        var fileName = context.Request.QueryString["file"];
        if (session == "true")
        {
            Utility_3D.ConvertedModel model = (Utility_3D.ConvertedModel)context.Session["Model"];
            _response.BinaryWrite(model.data);
            return;
        }
        else if (context.Request.Params["temp"] == "true")
        {
            try
            {
                _response.AppendHeader("content-disposition", "attachment; filename=" + fileName);
                _response.ContentType = vwarDAL.FedoraCommonsRepo.GetMimeType(fileName);
                string optionalPath = (fileName.LastIndexOf("o3d", StringComparison.CurrentCultureIgnoreCase) != -1) ? "viewerTemp/" : "converterTemp/";
                string pathToTempFile = "~/App_Data/" + optionalPath + fileName;
                using (FileStream stream = new FileStream(context.Server.MapPath(pathToTempFile), FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, (int)stream.Length);
                    _response.BinaryWrite(buffer);
                }
            }
            catch
            {
                _response.StatusCode = 404;

            }
            finally
            {
                _response.End();
            }
        }

        var pid = context.Request.QueryString["pid"];

        var factory = new vwarDAL.DataAccessFactory();
        vwarDAL.IDataRepository vd = factory.CreateDataRepositorProxy();
        DataAccessFactory daf = new DataAccessFactory();
        /*ITempContentManager tcm = daf.CreateTempContentManager();
        string hash = tcm.GetTempLocation(pid);

        string extension = "";
        if (!String.IsNullOrEmpty(fileName))
        {
            int extensionLocation = fileName.LastIndexOf('.');
            extension = (extensionLocation != -1) ? fileName.Substring(extensionLocation) : "";
        }
        if (!String.IsNullOrEmpty(hash) && extension != ".jpg"
            && extension != ".png"
            && extension != ".gif")
        {
            downloadFromTemp(hash, fileName, context);
        }
        else*/
        //{
        var url = "";
        if (!String.IsNullOrEmpty(context.Request.QueryString["Cache"]))
        {
            url = vd.FormatContentUrl(pid, fileName);
        }
        else
        {
            try
            {
                url = vd.GetContentUrl(pid, fileName);
            }
            catch
            {
                context.Response.StatusCode = 404;
            }
        }
        if (String.IsNullOrEmpty(url)) return;
        var creds = new System.Net.NetworkCredential(FedoraUserName, FedoraPasswrod);
        _response.Clear();
        _response.AppendHeader("content-disposition", "attachment; filename=" + fileName);
        _response.ContentType = vwarDAL.FedoraCommonsRepo.GetMimeType(fileName);
        using (Stream s = vd.GetContentFile(pid,fileName))
        {
            try
            {
                byte[] data = new byte[s.Length];
                s.Read(data, 0, data.Length);
                _response.BinaryWrite(data);
            }
            catch
            {
                context.Response.StatusCode = 404;
            }

        }
        //}
        _response.End();

    }

    private void downloadFromTemp(string hash, string fileName, HttpContext context)
    {
        DataAccessFactory daf = new DataAccessFactory();
        ITempContentManager tcm = daf.CreateTempContentManager();
        //string hash = tcm.GetTempLocation(pid);
        string filePath = context.Server.MapPath("~/App_Data/");
        //The tests with the slashes in the filename will report a bad path from FileInfo
        string originalExtension = fileName.Substring(fileName.LastIndexOf('.'));
        if (fileName.IndexOf("original_") != -1)
        {
            filePath += hash + originalExtension;
        }
        else if (fileName.IndexOf(".o3d") != -1)
        {
            filePath += "viewerTemp/" + hash + ".o3d";
        }
        else if (fileName.IndexOf(".zip") != -1)
        {
            filePath += "converterTemp/" + hash + ".zip";
        }
        else
        {
            context.Response.StatusCode = 404;
            context.Response.End();
        }

        context.Response.AppendHeader("content-disposition", "attachment; filename=" + fileName);
        using (FileStream fstream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            byte[] buffer = new byte[fstream.Length];
            fstream.Read(buffer, 0, (int)fstream.Length);
            context.Response.BinaryWrite(buffer);
        }
        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}