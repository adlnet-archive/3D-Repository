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
            ServerManager serverManager = new ServerManager();

            if (args[0] == "/install" || args[0] == "/i")
            {
                try
                {
                    Site mySite = serverManager.Sites.Add("3DR", args[args.Length-1], 80);
                    mySite.Applications.Add("/3DR", args[args.Length - 1] + "\\VwarWeb\\");
                    mySite.Applications.Add("/API", args[args.Length - 1] + "\\3d.service.host\\");

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
                    Console.WriteLine(e.Message);
                }
            }
            else if(args[0] == "/uninstall" || args[0] == "/u")
            {
                foreach (Site s in serverManager.Sites)
                {
                    if (s.Name == "3DR")
                        serverManager.Sites.Remove(s);
                }
                foreach (ApplicationPool p in serverManager.ApplicationPools)
                {
                    if (p.Name == "3DRAPPPOOL")
                        serverManager.ApplicationPools.Remove(p);
                }
                serverManager.CommitChanges();
            }
            else if (args[0] == "/stop")
            {
                foreach (Site s in serverManager.Sites)
                {
                    if (s.Name == "3DR")
                        s.Stop();
                    
                }
                foreach (ApplicationPool p in serverManager.ApplicationPools)
                {
                    if (p.Name == "3DRAPPPOOL")
                        p.Stop();
                }
                serverManager.CommitChanges();

            }
            else if (args[0] == "/start")
            {
                foreach (Site s in serverManager.Sites)
                {
                    if (s.Name == "3DR")
                        s.Start();

                }
                foreach (ApplicationPool p in serverManager.ApplicationPools)
                {
                    if (p.Name == "3DRAPPPOOL")
                        p.Start();
                }
                serverManager.CommitChanges();

            }
        }
    }
}