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



<%@ WebHandler Language="C#" Class="DownloadScreenshot" %>

using System;
using System.Web;
using System.Configuration;
/// <summary>
/// 
/// </summary>
public class DownloadScreenshot : IHttpHandler
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
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");

        context.Response.Cache.SetExpires(DateTime.Now.AddSeconds(600));
        context.Response.Cache.SetCacheability(HttpCacheability.Public);
        context.Response.Cache.VaryByParams["PID"] = true;


        var pid = context.Request.QueryString["PID"];

        var factory = new vwarDAL.DataAccessFactory();
        vwarDAL.IDataRepository vd = factory.CreateDataRepositorProxy();

        vwarDAL.ContentObject co = vd.GetContentObjectById(pid, false);
        string fileName = co.ScreenShot;

        var creds = new System.Net.NetworkCredential(FedoraUserName, FedoraPasswrod);
        context.Response.Clear();
        context.Response.AppendHeader("content-disposition", "attachment; filename=" + fileName);
        context.Response.ContentType = vwarDAL.DataUtils.GetMimeType(fileName);
        // string localPath = Path.GetTempFileName();
        using (System.IO.Stream s = vd.GetContentFile(pid, fileName))
        {
            try
            {
                byte[] data = new byte[s.Length];
                s.Read(data, 0, data.Length);
                context.Response.BinaryWrite(data);
            }
            catch { }

        }
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
