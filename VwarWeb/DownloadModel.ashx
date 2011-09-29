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



<%@ WebHandler Language="C#" Class="DownloadModel" %>

using System;
using System.Web;
using System.Configuration;
using System.Web.Caching;
/// <summary>
/// 
/// </summary>
public class DownloadModel : IHttpHandler
{
    /// <summary>
    /// 
    /// </summary>
    private string FedoraUserName
    {
        get
        {
            return (ConfigurationManager.AppSettings["fedoraUserName"]);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private string FedoraPasswrod
    {
        get
        {
            return (ConfigurationManager.AppSettings["fedoraPassword"]);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void ProcessRequest(HttpContext context)
    {
        bool needsConversion = false;
        var pid = context.Request.QueryString["PID"];

        vwarDAL.PermissionsManager prm = new vwarDAL.PermissionsManager();

        vwarDAL.ModelPermissionLevel Permission = prm.GetPermissionLevel(context.User.Identity.Name, pid);
        if (Permission < vwarDAL.ModelPermissionLevel.Fetchable)
        {
            context.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
            return;
        }
        
        
        var format = context.Request.QueryString["Format"];

        var factory = new vwarDAL.DataAccessFactory();
        vwarDAL.IDataRepository vd = factory.CreateDataRepositorProxy();

        vwarDAL.ContentObject co = vd.GetContentObjectById(pid, false);
        vd.IncrementDownloads(co.PID);

        string fileName = "";
        if (format == "o3dtgz" || format == "o3d")
        {
            fileName = co.DisplayFile;
            //fileId = co
        }
        else if (format == "original")
        {
            fileName = co.OriginalFileName;
        }
        else
        {
            fileName = co.Location;
            if (format != "dae")
            {
                needsConversion = true;
            }
        }


        byte[] data = null;
        var creds = new System.Net.NetworkCredential(FedoraUserName, FedoraPasswrod);
        context.Response.Clear();
        context.Response.AppendHeader("content-disposition", "attachment; filename=" + fileName);
        context.Response.ContentType = vwarDAL.DataUtils.GetMimeType(fileName);
        using (System.IO.Stream stream = vd.GetContentFile(pid, fileName))
        {
            try
            {
                data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
                if (needsConversion)
                {
                    Utility_3D _3d = new Utility_3D();
                    _3d.Initialize(Website.Config.ConversionLibarayLocation);
                    Utility_3D.Model_Packager pack = new Utility_3D.Model_Packager();
                    Utility_3D.ConvertedModel model = pack.Convert(stream, "temp.zip", format);
                    data = model.data;
                }

            }
            catch
            {

            }
        }

        context.Response.BinaryWrite(data);
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
