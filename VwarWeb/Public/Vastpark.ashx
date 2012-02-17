<%--
Copyright 2011 U.S. Department of Defense

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
--%>



<%@ WebHandler Language="C#" Class="Vastpark" %>

using System;
using System.Web;
using System.Web.Security;
/// <summary>
/// 
/// </summary>
public class Vastpark : IHttpHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void ProcessRequest(HttpContext context)
    {
       /* var creds = context.Request.QueryString["c"];
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
        }*/

        vwarDAL.PermissionsManager m = new vwarDAL.PermissionsManager();
        m.DeleteGroup("psadmin@problemsolutions.net", "TestGroup");
        m.Dispose();
    }
    /// <summary>
    /// 
    /// </summary>
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
