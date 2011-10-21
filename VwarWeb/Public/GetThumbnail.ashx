<%@ WebHandler Language="C#" Class="GetThumbnail" %>


using System;
using System.Web;

public class GetThumbnail : IHttpHandler
{
    
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

        string NameOfFile = rv.ThumbnailId;
        
        if(String.IsNullOrEmpty(NameOfFile))
            NameOfFile = rv.Thumbnail;
        if (String.IsNullOrEmpty(NameOfFile))
            NameOfFile = rv.ScreenShotId;
        if (String.IsNullOrEmpty(NameOfFile))
            NameOfFile = rv.ScreenShot;
        using (System.IO.Stream s = dal.GetContentFile(rv.PID, NameOfFile))
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