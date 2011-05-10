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

namespace _3DR_Testing
{
    

    public class FedoraControl
    {
        static string contentPath = _3DR_Testing.Properties.Settings.Default.ContentPath + "\\Preconverted\\";
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
                co.SubmitterEmail = _3DR_Testing.Properties.Settings.Default._3DRUserName;
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
            dco.ScreenShotId = dal.UploadFile(File.ReadAllBytes(contentPath + "screenshot.png"), dco.PID, dco.ScreenShot);
            dco.DeveloperLogoImageFileNameId = dal.UploadFile(File.ReadAllBytes(contentPath + "devlogo.jpg"), dco.PID, dco.DeveloperLogoImageFileName);
            dco.SponsorLogoImageFileNameId = dal.UploadFile(File.ReadAllBytes(contentPath + "sponsorlogo.jpg"), dco.PID, dco.SponsorLogoImageFileName);
            dco.OriginalFileId = dal.UploadFile(File.ReadAllBytes(contentPath + "original_test.zip"), dco.PID, dco.OriginalFileName);
            dco.DisplayFileId = dal.UploadFile(File.ReadAllBytes(contentPath + "test.o3d"), dco.PID, dco.DisplayFile);
            dal.UploadFile(File.ReadAllBytes(contentPath + "test.zip"), dco.PID, dco.Location);
            dal.UpdateContentObject(dco);

            return dco;
        }
        
        [Test]
        public static void PurgeAll()
        {
            var creds = new System.Net.NetworkCredential(Properties.Settings.Default.FedoraAdminName, Properties.Settings.Default.FedoraAdminPassword);
            var svcm = new fedoraM.FedoraAPIMService();
            svcm.Credentials = creds;
            foreach (var result in GetAllContentObjects(creds, Properties.Settings.Default.FedoraAccessURL))
            {
                Console.WriteLine(result.pid);
                svcm.purgeObject(result.pid, "remove", false);
            }

        }
        [Test]
        public static void ClearDatabase()
        {
           // string clearcommand = "C:\\Program Files\\MySQL\\MySQL Server 5.1\\bin\\mysql.exe "-h 10.100.10.83 -P 3306 --protocol=TCP -uroot -ppassword test < c:\\Development\\_3DR_Testing\\newTables.sql";
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
            info.Arguments = "-h 10.100.10.83 -P 3306 --protocol=TCP -uroot -ppassword test";
            info.FileName = "C:\\Program Files\\MySQL\\MySQL Server 5.1\\bin\\mysql.exe";
          //  info.RedirectStandardOutput = true;
            info.RedirectStandardInput = true;
          //  info.RedirectStandardError = true;
            info.UseShellExecute = false;

            System.Diagnostics.Process clear = new System.Diagnostics.Process();
            clear.StartInfo = info;
            clear.Start();

            StreamReader rw = new StreamReader("c:\\Development\\_3DR_Testing\\newTables.sql");
            string text = rw.ReadToEnd();
            clear.StandardInput.Write(text);

            System.Threading.Thread.Sleep(1000);
            //string output = clear.StandardOutput.ReadToEnd();
            //clear.WaitForExit();



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
