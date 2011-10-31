<%@ WebHandler Language="C#" Class="PreviewTexture" %>

using System;
using System.Web;
using vwar;
using vwarDAL;
using System.IO;
public class PreviewTexture : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

        //Get the PID for the request
        string Pid = Website.Common.GetPidFromURL(context);
        Pid = Pid.Replace('_', ':');
        string TextureName = context.Request.QueryString["TextureName"];

        //Get the content object
        vwarDAL.DataAccessFactory factory = new vwarDAL.DataAccessFactory();
        vwarDAL.IDataRepository vd = factory.CreateDataRepositorProxy();
        ContentObject co = vd.GetContentObjectById(Pid, false, false);

        byte[] DAEZIPFILE = vd.GetContentFileData(Pid, co.Location);

        Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(DAEZIPFILE);
        Website.Common.WriteTextureToResponseFromZip(zip, TextureName, context);      
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}