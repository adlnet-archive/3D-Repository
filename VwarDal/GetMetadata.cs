//  Copyright 2011 U.S. Department of Defense

//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at

//      http://www.apache.org/licenses/LICENSE-2.0

//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.



using System;
using System.Web;
using System.Configuration;
namespace vwarDAL
{
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
            context.Response.Write("results=");

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
}
