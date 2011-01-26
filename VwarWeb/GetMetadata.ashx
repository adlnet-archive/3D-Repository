<%@ WebHandler Language="C#" Class="GetMetadata" %>

using System;
using System.Web;
using System.Configuration;

    public class GetMetadata : IHttpHandler
    {
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

        public void ProcessRequest(HttpContext context)
        {


            context.Response.ContentType = "text/plain";

            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            

            string searchterms = context.Request.QueryString["Field"];
            string pid = context.Request.QueryString["PID"];

            var factory = new vwarDAL.DataAccessFactory();
            vwarDAL.IDataRepository vd = factory.CreateDataRepositorProxy();

            vwarDAL.ContentObject co = vd.GetContentObjectById(pid,false);

            if (searchterms.Contains("UpAxis"))
            {
                context.Response.Write("UpAxis=" + co.UpAxis + ";");
            }
            if (searchterms.Contains("UnitScale"))
            {
                context.Response.Write("UnitScale=" + co.UnitScale + ";");
            }
            if (searchterms.Contains("NumPolygons"))
            {
                context.Response.Write("NumPolygons=" + co.NumPolygons + ";");
            }
            if (searchterms.Contains("NumTextures"))
            {
                context.Response.Write("NumTextures=" + co.NumTextures + ";");
            }
            if (searchterms.Contains("Label"))
            {
                context.Response.Write("Label=" + co.Label + ";");
            }
            if (searchterms.Contains("Keywords"))
            {
                context.Response.Write("Keywords=" + co.Keywords + ";");
            }
            if (searchterms.Contains("Title"))
            {
                context.Response.Write("Title=" + co.Title + ";");
            }
            if (searchterms.Contains("Format"))
            {
                context.Response.Write("Format=" + co.Format + ";");
            }

            context.Response.End();
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

