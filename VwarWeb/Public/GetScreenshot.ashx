<%@ WebHandler Language="C#" Class="GetScreenshot" %>

using System;
using System.Web;

public class GetScreenshot : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

        var ContentObjectID = context.Request.QueryString["ContentObjectID"];
        if(ContentObjectID== null) 
            ContentObjectID = context.Request.QueryString["pid"];

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

      
            using (System.IO.Stream s = dal.GetContentFile(rv.PID, rv.ScreenShotId))
            {
                byte[] data = new byte[s.Length];
                s.Read(data, 0, data.Length);
                context.Response.BinaryWrite(data);
                return;
            }
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}