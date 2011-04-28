<%@ WebHandler Language="C#" Class="DownloadModel" %>

using System;
using System.Web;
using System.Configuration;
using System.Web.Caching;

public class DownloadModel : IHttpHandler
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
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World")

        context.Response.Cache.SetExpires(DateTime.Now.AddSeconds(600));
        context.Response.Cache.SetCacheability(HttpCacheability.Public);
        context.Response.Cache.VaryByParams["PID"] = true;
        context.Response.Cache.VaryByParams["Format"] = true;

        //Cache int the application memory if the query string is the same!
        byte[] cache_data = (byte[])HttpRuntime.Cache[context.Request.QueryString + "_data"];
        if (cache_data != null)
        {
            string cache_filename = (string)HttpRuntime.Cache[context.Request.QueryString + "_filename"];
            string cache_filetype = (string)HttpRuntime.Cache[context.Request.QueryString + "_filetype"];

            context.Response.AppendHeader("content-disposition", "attachment; filename=" + cache_filename);
            context.Response.ContentType = cache_filetype;
            context.Response.BinaryWrite(cache_data);
            return;
        }



        var pid = context.Request.QueryString["PID"];
        var format = context.Request.QueryString["Format"];

        var factory = new vwarDAL.DataAccessFactory();
        vwarDAL.IDataRepository vd = factory.CreateDataRepositorProxy();

        vwarDAL.ContentObject co = vd.GetContentObjectById(pid, false);

        string fileName = "";
        if (format == "o3dtgz" || format == "o3d")
        {
            fileName = co.DisplayFile;
        }


        if (format == "dae" || format == "collada")
        {
            fileName = co.Location;

        }


        if (format == "o3dtgz" || format == "o3d")
        {
            fileName = co.DisplayFile + "tgz";
        }

        byte[] data = null;
        var creds = new System.Net.NetworkCredential(FedoraUserName, FedoraPasswrod);
        context.Response.Clear();
        context.Response.AppendHeader("content-disposition", "attachment; filename=" + fileName);
        context.Response.ContentType = vwarDAL.DataUtils.GetMimeType(fileName);
        // string localPath = Path.GetTempFileName();
        using (System.IO.Stream stream = vd.GetContentFile(pid, fileName))
        {
            try
            {
                data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
            }
            catch
            {

            }
        }
        if (data == null || data.Length <= 0)
        {    
            context.Response.Clear();
            context.Response.ClearHeaders();
            context.Response.AppendHeader("content-disposition", "attachment; filename=" + co.Location);
            context.Response.ContentType = vwarDAL.DataUtils.GetMimeType(co.Location);

            using (var modelStream = vd.GetContentFile(pid, fileName))
            {
                data = new byte[modelStream.Length];
                modelStream.Read(data, 0, data.Length);
            }
            Utility_3D _3d = new Utility_3D();
            _3d.Initialize(Website.Config.ConversionLibarayLocation);
            Utility_3D.Model_Packager pack = new Utility_3D.Model_Packager();
            Utility_3D.ConvertedModel model = pack.Convert(new System.IO.MemoryStream(data), "temp.zip", format);
            data = model.data;

            HttpRuntime.Cache[context.Request.QueryString + "_data"] = model.data;
            HttpRuntime.Cache[context.Request.QueryString + "_filename"] = co.Location;
            HttpRuntime.Cache[context.Request.QueryString + "_filetype"] = context.Response.ContentType;

        }

        context.Response.BinaryWrite(data);
        context.Response.Close();
    }



    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}