<%@ WebHandler Language="C#" Class="Model" %>

using System;
using System.Web;
using System.Configuration;
using System.IO;
using System.Web.SessionState;
using vwarDAL;



public class Model : IHttpHandler, IReadOnlySessionState
{
    //stream should be a zip file!
    public void WriteTexturetoResponse(Stream stream, HttpResponse _response, HttpContext context)
    {
        byte[] buffer = new byte[stream.Length];
        stream.Read(buffer, 0, (int)stream.Length);
        Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(buffer);
        foreach (Ionic.Zip.ZipEntry ze in zip)
        {
            if (ze.FileName == context.Request.Params["Texture"])
            {
                MemoryStream mem = new MemoryStream();
                ze.Extract(mem);
                byte[] jsonbuffer = new byte[mem.Length];
                mem.Seek(0, SeekOrigin.Begin);
                mem.Read(jsonbuffer, 0, (int)mem.Length);
                _response.BinaryWrite(jsonbuffer);
                return;

            }
        }
    }
    //stream should be a zip file!
    public void WriteJSONtoResponse(Stream stream, HttpResponse _response, HttpContext context, string filename)
    {
        byte[] buffer = new byte[stream.Length];
        stream.Read(buffer, 0, (int)stream.Length);
        Utility_3D _3d = new Utility_3D();
        _3d.Initialize(Website.Config.ConversionLibarayLocation);
        Utility_3D.Model_Packager pack = new Utility_3D.Model_Packager();
        Utility_3D.ConvertedModel model = pack.Convert(new MemoryStream(buffer), filename, "json");
        Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(model.data);
        foreach (Ionic.Zip.ZipEntry ze in zip)
        {
            if (Path.GetExtension(ze.FileName) == ".json")
            {
                MemoryStream mem = new MemoryStream();
                ze.Extract(mem);
                byte[] jsonbuffer = new byte[mem.Length];
                mem.Seek(0, SeekOrigin.Begin);
                mem.Read(jsonbuffer, 0, (int)mem.Length);
                _response.BinaryWrite(jsonbuffer);

            }
        }
    }
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
                _response.ContentType = vwarDAL.DataUtils.GetMimeType(fileName);
                string optionalPath = (fileName.LastIndexOf("o3d", StringComparison.CurrentCultureIgnoreCase) != -1) ? "viewerTemp/" : "converterTemp/";
                string pathToTempFile = "~/App_Data/" + optionalPath + fileName;

                if (context.Request.Params["Texture"] != null)
                {
                    using (FileStream stream = new FileStream(context.Server.MapPath(pathToTempFile), FileMode.Open, FileAccess.Read))
                    {
                        WriteTexturetoResponse(stream, _response, context);
                    }


                }
                if (context.Request.Params["Format"] == "json")
                {
                        using (FileStream stream = new FileStream(context.Server.MapPath(pathToTempFile), FileMode.Open, FileAccess.Read))
                        {
                            WriteJSONtoResponse(stream, _response, context, context.Server.MapPath(pathToTempFile));
                        }  
                }
                else
                {
                    using (FileStream stream = new FileStream(context.Server.MapPath(pathToTempFile), FileMode.Open, FileAccess.Read))
                    {
                        byte[] buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, (int)stream.Length);
                        _response.BinaryWrite(buffer);
                        stream.Close();
                    }
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
        //if (!String.IsNullOrEmpty(context.Request.QueryString["Cache"]))
        //{
        //    url = vd.FormatContentUrl(pid, fileName);
        //}
        //else
        //{
            try
            {
                url = vd.GetContentUrl(pid, fileName);
            }
            catch
            {
                context.Response.StatusCode = 404;
            }
       // }
        if (String.IsNullOrEmpty(url)) return;
        var creds = new System.Net.NetworkCredential(FedoraUserName, FedoraPasswrod);
        _response.Clear();
        _response.AppendHeader("content-disposition", "attachment; filename=" + fileName);
        _response.ContentType = vwarDAL.DataUtils.GetMimeType(fileName);
        using (System.Net.WebClient client = new System.Net.WebClient())
        {
              try
              {
                    client.Credentials = creds;
                    byte[] modeldata = client.DownloadData(url);
            
                    if (context.Request.Params["Texture"] != null)
                    {
                        using (MemoryStream stream = new MemoryStream(modeldata))
                        {
                            WriteTexturetoResponse(stream, _response, context);
                            _response.AppendHeader("content-disposition", "attachment; filename=" + context.Request.Params["Texture"]);
                            _response.ContentType = vwarDAL.DataUtils.GetMimeType(context.Request.Params["Texture"]);
                        }
                    }
                    else if (context.Request.Params["Format"] == "json")
                    {
                        using (MemoryStream stream = new MemoryStream(modeldata))
                        {
                            WriteJSONtoResponse(stream, _response, context,fileName);
                            _response.AppendHeader("content-disposition", "attachment; filename=" + context.Request.Params["Format"]);
                            _response.ContentType = vwarDAL.DataUtils.GetMimeType(context.Request.Params["Format"]);
                        }  
                    }
                    else
                    {
                        _response.BinaryWrite(modeldata);
                        
                        _response.AppendHeader("content-disposition", "attachment; filename=" + fileName);
                        _response.ContentType = vwarDAL.DataUtils.GetMimeType(fileName);
                        
                    }
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