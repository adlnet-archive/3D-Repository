<%@ WebHandler Language="C#" Class="Vastpark" %>

using System;
using System.Web;
using System.Web.Security;
public class Vastpark : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        var creds = context.Request.QueryString["c"];
        var parts = creds.Split('|');
        if (Membership.ValidateUser(parts[0], parts[1]))
        {
            var factory = new vwarDAL.DataAccessFactory();
            vwarDAL.IDataRepository vd = factory.CreateDataRepositorProxy();
            var contentObjectId = (context.Request.QueryString["id"]);
            vd.IncrementDownloads(contentObjectId);
            var contentObject = vd.GetContentObjectById(contentObjectId, false);
            string filePath = Website.Common.FormatZipFilePath(contentObjectId.ToString(), contentObject.Location);
            context.Response.WriteFile(filePath);
        }
        else
        {
            context.Response.StatusCode = 401;
            context.Response.Write("Invalid Username and Password");
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}