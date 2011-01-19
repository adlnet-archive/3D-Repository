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

using Selenium;

namespace _3DR_Testing
{
    public class FedoraControl
    {
        
      
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
