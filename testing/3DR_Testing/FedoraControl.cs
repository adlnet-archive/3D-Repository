using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using System.Threading;
using NUnit.Framework;
using vwarDAL;
using Selenium;
using System.Configuration;

namespace _3DR_Testing
{
    

    public class FedoraControl
    {
        static string contentPath = ConfigurationManager.AppSettings["ContentPath"] + "\\Preconverted\\";
        public static ContentObject Default3drContentObject
        {
            get
            {
                ContentObject co = new ContentObject();
                co.ArtistName = FormDefaults.ArtistName;
                co.AssetType = "MODEL";
                co.CreativeCommonsLicenseURL = "http://creativecommons.org/licenses/by-sa/3.0/legalcode";
                co.Description = FormDefaults.Description;
                co.DeveloperName = FormDefaults.DeveloperName;
                co.Enabled = true;
                co.Format = ".DAE";
                co.Keywords = FormDefaults.Tags;
                co.Ready = true;
                co.RequireResubmit = false;
                co.SponsorName = FormDefaults.SponsorName;
                co.SubmitterEmail = ConfigurationManager.AppSettings["_3DRUserName"];
                co.UpAxis = "Y";
                co.UnitScale = "1.0";
                co.Title = "test";

                co.ScreenShot = "screenshot.png";
                co.DeveloperLogoImageFileName = "devlogo.jpg";
                co.SponsorLogoImageFileName = "sponsorlogo.jpg";
                co.Location = "test.zip";
                co.OriginalFileName = "original_test.zip";
                co.DisplayFile = "test.o3d";
                return co;
            }
        }


        public static ContentObject AddDefaultObject()
        {
            
            IDataRepository dal = new DataAccessFactory().CreateDataRepositorProxy();
            ContentObject dco = Default3drContentObject;
            dal.InsertContentObject(dco);
            dco.ScreenShotId = dal.SetContentFile(new FileStream(contentPath + "screenshot.png", FileMode.Open), dco.PID, dco.ScreenShot);
            dco.DeveloperLogoImageFileNameId = dal.SetContentFile(new FileStream(contentPath + "devlogo.jpg", FileMode.Open), dco.PID, dco.DeveloperLogoImageFileName);
            dco.SponsorLogoImageFileNameId = dal.SetContentFile(new FileStream(contentPath + "sponsorlogo.jpg", FileMode.Open), dco.PID, dco.SponsorLogoImageFileName);
            dco.OriginalFileId = dal.SetContentFile(new FileStream(contentPath + "original_test.zip", FileMode.Open), dco.PID, dco.OriginalFileName);
            dco.DisplayFileId = dal.SetContentFile(new FileStream(contentPath + "test.o3d", FileMode.Open), dco.PID, dco.DisplayFile);
            dal.SetContentFile(new FileStream(contentPath + "test.zip", FileMode.Open), dco.PID, dco.Location);
            dal.UpdateContentObject(dco);

            return dco;
        }
        
       
        public static void PurgeAll()
        {
            var creds = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["FedoraAdminName"], ConfigurationManager.AppSettings["FedoraAdminPassword"]);
            var svcm = new fedoraM.FedoraAPIMService();
            svcm.Credentials = creds;
            foreach (var result in GetAllContentObjects(creds, ConfigurationManager.AppSettings["FedoraAccessURL"]))
            {
                Console.WriteLine(result.pid);
                svcm.purgeObject(result.pid, "remove", false);
            }

        }

        private static FedoraA.ObjectFields[] GetAllContentObjects(NetworkCredential creds, string url)
        {
            FedoraA.FedoraAPIAService svc = new FedoraA.FedoraAPIAService();
            if (!String.IsNullOrEmpty(url))
            {
                svc.Url = url;
            }
            svc.Credentials = creds;

            FedoraA.FieldSearchQuery query = new FedoraA.FieldSearchQuery();
            query.Item = "pid~adl?";

            FedoraA.FieldSearchQuery fsq = new FedoraA.FieldSearchQuery();


            FedoraA.FieldSearchQueryConditions fsqConditions = new FedoraA.FieldSearchQueryConditions();

            FedoraA.Condition c = new FedoraA.Condition();

            FedoraA.Condition c2 = new FedoraA.Condition();
            c2.property = "pid";
            c2.@operator = FedoraA.ComparisonOperator.has;
            c2.value = "adl:*";

            fsqConditions.condition = new FedoraA.Condition[] { c2 };

            fsq.Item = fsqConditions;
            var results = svc.findObjects(new String[] { "pid", "label" }, "10000", fsq);

            return (results.resultList);
        }

    }
}
