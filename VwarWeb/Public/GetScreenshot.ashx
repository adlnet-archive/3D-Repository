<%@ WebHandler Language="C#" Class="GetScreenshot" %>

using System;
using System.Web;

public class GetScreenshot : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

        var ContentObjectID = Website.Common.GetPidFromURL(context);
       
        var factory = new vwarDAL.DataAccessFactory();
        vwarDAL.IDataRepository dal =  factory.CreateDataRepositorProxy();
        vwarDAL.ContentObject rv = null;
        if (ContentObjectID != null)
            rv = dal.GetContentObjectById(ContentObjectID, false);

        if (ContentObjectID != null)
        {
            vwarDAL.PermissionsManager perm = new vwarDAL.PermissionsManager();
            vwarDAL.ModelPermissionLevel level = perm.GetPermissionLevel(HttpContext.Current.User.Identity.Name, ContentObjectID);

            if (level < vwarDAL.ModelPermissionLevel.Searchable)
                return;
        }
        
        string NameOfFile = rv.ScreenShotId;
            if(String.IsNullOrEmpty(NameOfFile))
            NameOfFile = rv.ScreenShot;

            Website.Common.WriteContentFileToResponse(rv.PID, NameOfFile, context, dal);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}