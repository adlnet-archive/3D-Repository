using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Web.Administration;
namespace MSWebAdmin_Application
{      
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ServerManager serverManager = new ServerManager();

                Site mySite = serverManager.Sites.Add("3DR", args[0], 80);
                mySite.Applications.Add("/3DR", args[0] + "\\VwarWeb\\");
                mySite.Applications.Add("/API", args[0] + "\\3d.service.host\\");

                try
                {
                    serverManager.ApplicationPools.Add("3DRAPPPOOL");
                }
                catch (Exception e) { }
                serverManager.ApplicationPools["3DRAPPPOOL"].Enable32BitAppOnWin64 = true;
                serverManager.ApplicationPools["3DRAPPPOOL"].ManagedRuntimeVersion = "v4.0";
                mySite.ApplicationDefaults.ApplicationPoolName = "3DRAPPPOOL";
                mySite.ServerAutoStart = true;
                serverManager.CommitChanges();
            }
            catch (Exception e)
            {

            }
        }
    }
}