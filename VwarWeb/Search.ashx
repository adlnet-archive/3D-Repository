<%@ WebHandler Language="C#" Class="Search" %>

using System;
using System.Web;
using System.Configuration;
public class Search : IHttpHandler {

    private string FedoraUserName
    {
        get
        {
            return (ConfigurationManager.AppSettings["fedoraUserName"]);
        }
    }
    private string FedoraPasswrod
    {
        get
        {
            return (ConfigurationManager.AppSettings["fedoraPassword"]);
        }
    }
    
    public void ProcessRequest (HttpContext context) {


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
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}