<%--
Copyright 2011 U.S. Department of Defense

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
--%>



<%@ WebHandler Language="C#" Class="Model" %>

using System;
using System.Web;
using System.Configuration;
using System.IO;
using System.Web.SessionState;
using vwarDAL;
/// <summary>
/// 
/// </summary>
public class Model : IHttpHandler, IReadOnlySessionState
{
    /// <summary>
    /// stream should be a zip file! 
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="_response"></param>
    /// <param name="context"></param>
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
    /// <summary>
    /// stream should be a zip file! 
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="_response"></param>
    /// <param name="context"></param>
    /// <param name="filename"></param>
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
                return;
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private string FedoraUserName
    {
        get
        {
            return (ConfigurationManager.AppSettings["fedoraUserName"]);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private string FedoraPasswrod
    {
        get
        {
            return (ConfigurationManager.AppSettings["fedoraPassword"]);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void ProcessRequest(HttpContext context)
    {
        HttpResponse _response = HttpContext.Current.Response;
        var session = context.Request.QueryString["Session"];
        var fileName = context.Request.QueryString["file"];
        var fileId = context.Request.QueryString["fileid"];
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
        using (Stream data = vd.GetContentFile(pid, fileId))
        {
            try
            {
                if (data == null)
                {
                    context.Response.StatusCode = 404;
                    return;
                }
                _response.Clear();
                _response.AppendHeader("content-disposition", "attachment; filename=" + fileName);
                _response.ContentType = vwarDAL.DataUtils.GetMimeType(fileName);
                if (context.Request.Params["Texture"] != null)
                {
                        WriteTexturetoResponse(data, _response, context);
                        _response.AppendHeader("content-disposition", "attachment; filename=" + context.Request.Params["Texture"]);
                        _response.ContentType = vwarDAL.DataUtils.GetMimeType(context.Request.Params["Texture"]);
                }
                else if ("json".Equals(context.Request.Params["format"], StringComparison.InvariantCultureIgnoreCase))
                {
                        WriteJSONtoResponse(data, _response, context, fileName);
                        _response.AppendHeader("content-disposition", "attachment; filename=" + context.Request.Params["Format"]);
                        _response.ContentType = vwarDAL.DataUtils.GetMimeType(context.Request.Params["Format"]);
                }
                else
                {
                    byte[] modeldata = new byte[data.Length];
                    data.Read(modeldata, 0, modeldata.Length);
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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="hash"></param>
    /// <param name="fileName"></param>
    /// <param name="context"></param>
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
    /// <summary>
    /// 
    /// </summary>
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
