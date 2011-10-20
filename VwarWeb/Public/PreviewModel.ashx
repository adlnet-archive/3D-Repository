<%@ WebHandler Language="C#" Class="PreviewModel" %>

using System;
using System.Web;
using vwarDAL;
using vwar;
using System.IO;
public class PreviewModel : IHttpHandler {

    //write the json file to the response;
    public void WriteJSONtoResponse(Stream stream, HttpResponse _response, HttpContext context)
    {
        byte[] buffer = new byte[stream.Length];
        stream.Read(buffer, 0, (int)stream.Length);
        Utility_3D _3d = new Utility_3D();
        _3d.Initialize(Website.Config.ConversionLibarayLocation);
        Utility_3D.Model_Packager pack = new Utility_3D.Model_Packager();
        Utility_3D.ConvertedModel model = pack.Convert(new MemoryStream(buffer), "ThisShouldAlwaysBeZip.zip", "json");
        Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(model.data);
        _response.ContentType = "application/octet-stream";
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
    
    public void ProcessRequest (HttpContext context) {

        
        //Get the PID for the request
        string Pid = context.Request.QueryString["pid"];
        if (Pid == null)
            Pid = context.Request.QueryString["ContentObjectID"];

        Pid = Pid.Replace('_', ':');
        //The viewer that will be viewing the content
        string ViewerType = context.Request.QueryString["ViewerType"];
        
        //Get the content object
        vwarDAL.DataAccessFactory factory = new vwarDAL.DataAccessFactory();
        vwarDAL.IDataRepository vd = factory.CreateDataRepositorProxy();
        ContentObject co = vd.GetContentObjectById(Pid, false, false);

        //If the viewer requesting the model is webgl
        if (ViewerType.Contains("WebGL"))
        {
            Stream data = vd.GetContentFile(Pid, co.Location);
            WriteJSONtoResponse(data, context.Response, context);

        }
        //If the viewer requesting the model is o3d
        else if (ViewerType.Contains("o3d"))
        {
            
           Stream data =null;//= vd.GetContentFile(Pid, co.DisplayFileId);
           if( data == null) data = vd.GetContentFile(Pid, co.DisplayFile);
           context.Response.ContentType = "application/octet-stream";

           byte[] buffer = new byte[data.Length];
           data.Seek(0, SeekOrigin.Begin);
           data.Read(buffer, 0, (int)data.Length);
           context.Response.BinaryWrite(buffer);

        }
        //If the viewer requesting the model is flash
        else if (ViewerType.Contains("Away3D"))
        {
            Stream data = vd.GetContentFile(Pid, co.Location);
            context.Response.ContentType = "application/octet-stream";
            byte[] buffer = new byte[data.Length];
            data.Seek(0, SeekOrigin.Begin);
            data.Read(buffer, 0, (int)data.Length);
            context.Response.BinaryWrite(buffer);

        }
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}