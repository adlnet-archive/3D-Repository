<%@ WebHandler Language="C#" Class="PreviewTexture" %>

using System;
using System.Web;
using vwar;
using vwarDAL;
using System.IO;
public class PreviewTexture : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {


        //Get the PID for the request
        
        string TextureName = context.Request.QueryString["TextureName"];
        string TempArchiveName = context.Request.QueryString["TempArchiveName"];
        
        string path_to_converted_file = context.Server.MapPath("~/App_Data/converterTemp/") + TempArchiveName;
        Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(path_to_converted_file);
        Website.Common.WriteTextureToResponseFromZip(zip, TextureName, context);
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}