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



<%@ WebHandler Language="C#" Class="Search" %>

using System;
using System.Web;
using System.Configuration;
/// <summary>
/// 
/// </summary>
public class Search : IHttpHandler
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
        context.Response.ContentType = "text/plain";

        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        context.Response.Write("results=");

        var searchterms = context.Request.QueryString["SearchTerms"];

        var factory = new vwarDAL.DataAccessFactory();
        vwarDAL.IDataRepository vd = factory.CreateDataRepositorProxy();
        System.Collections.Generic.IEnumerable<vwarDAL.ContentObject> results = vd.SearchContentObjects(searchterms);
        foreach (vwarDAL.ContentObject co in results)
        {
            context.Response.Write(co.PID + ";");
        }


        context.Response.End();
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
