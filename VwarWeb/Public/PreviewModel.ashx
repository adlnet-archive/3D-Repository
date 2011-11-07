<%@ WebHandler Language="C#" Class="PreviewModel" %>

using System;
    
using vwarDAL;
using vwar;
using System.IO;
public class PreviewModel : IHttpHandler {

  
    
    public void ProcessRequest (HttpContext context) {


        string Pid = Website.Common.GetPidFromURL(context);

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
            Website.Common.WriteJSONtoResponse(data, context);

        }
        //If the viewer requesting the model is o3d
        else if (ViewerType.Contains("o3d"))
        {
            Website.Common.WriteContentFileToResponse(Pid, co.DisplayFile,context, vd);
        }
        //If the viewer requesting the model is flash
        else if (ViewerType.Contains("Away3D"))
        {
            Website.Common.WriteContentFileToResponse(Pid, co.Location, context, vd);
        }
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}