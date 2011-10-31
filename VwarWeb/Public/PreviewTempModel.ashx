<%@ WebHandler Language="C#" Class="PreviewModel" %>

using System;
using System.Web;
using vwarDAL;
using vwar;
using System.IO;
public class PreviewModel : IHttpHandler {

    
    
    public void ProcessRequest (HttpContext context) {


        //The viewer that will be viewing the content
        string ViewerType = context.Request.QueryString["ViewerType"];
        string TempArchiveName = context.Request.QueryString["TempArchiveName"];
       
        //If the viewer requesting the model is webgl
        if (ViewerType.Contains("WebGL"))
        {
            Stream data = new FileStream(context.Server.MapPath("~/App_Data/converterTemp/") + TempArchiveName, FileMode.Open);
            Website.Common.WriteJSONtoResponse(data, context);
            data.Close();
        }
        //If the viewer requesting the model is o3d
        else if (ViewerType.Contains("o3d"))
        {
           Website.Common.WriteLocalFileToResponse(context.Server.MapPath("~/App_Data/viewerTemp/") + TempArchiveName.Replace(".zip", ".o3d"), context);
        }
        //If the viewer requesting the model is flash
        else if (ViewerType.Contains("Away3D"))
        {
            Website.Common.WriteLocalFileToResponse(context.Server.MapPath("~/App_Data/converterTemp/") + TempArchiveName, context);
        }
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}