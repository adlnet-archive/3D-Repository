<%@ WebHandler Language="C#" Class="PreviewTexture" %>

using System;
using System.Web;
using vwar;
using vwarDAL;
using System.IO;
public class PreviewTexture : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {


        //Get the PID for the request
        string Pid = context.Request.QueryString["pid"];
        if (Pid == null)
            Pid = context.Request.QueryString["ContentObjectID"];
        Pid = Pid.Replace('_', ':');
        string TextureName = context.Request.QueryString["TextureName"];

        //Get the content object
        vwarDAL.DataAccessFactory factory = new vwarDAL.DataAccessFactory();
        vwarDAL.IDataRepository vd = factory.CreateDataRepositorProxy();
        ContentObject co = vd.GetContentObjectById(Pid, false, false);


        byte[] DAEZIPFILE = vd.GetContentFileData(Pid, co.Location);
        Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(DAEZIPFILE);
        foreach (Ionic.Zip.ZipEntry ze in zip)
        {
            if (ze.FileName == TextureName)
            {
                MemoryStream mem = new MemoryStream();
                ze.Extract(mem);
                byte[] buffer = new byte[mem.Length];
                mem.Seek(0, SeekOrigin.Begin);
                mem.Read(buffer, 0, (int)mem.Length);
                context.Response.BinaryWrite(buffer);
                context.Response.ContentType = vwarDAL.DataUtils.GetMimeType(TextureName);
                return;
            }
        }
 
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}