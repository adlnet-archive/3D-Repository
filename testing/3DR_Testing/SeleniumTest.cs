using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;
using System.IO;
using System.Diagnostics;
namespace _3DR_Testing
{
 

    public abstract class SeleniumTest
    {
        protected ISelenium selenium;
        protected string path;
        protected HttpCommandProcessor proc;
        protected StringBuilder verificationErrors;
        protected Process seleniumServer;
        protected bool isSeleniumRunning = false;

        [SetUp]
        virtual public void SetupTest()
        {
            proc = new HttpCommandProcessor("localhost", 4444, "*chrome", _3DR_Testing.Properties.Settings.Default._3DRURL);
            path = _3DR_Testing.Properties.Settings.Default.ContentPath;
            selenium = new DefaultSelenium(proc);
            verificationErrors = new StringBuilder();
            selenium.Start();
            selenium.WindowMaximize();
            selenium.Open("/Default.aspx");
            selenium.WindowFocus();
            isSeleniumRunning = true;
        }

        [TearDown]
        virtual public void TeardownTest()
        {
            try
            {
                //StopSelenium();
                selenium.Stop();
                isSeleniumRunning = false;
            }
            catch (Exception)
            {

            }
            Assert.AreEqual(String.Empty, verificationErrors.ToString());
        }


        protected void StartSelenium()
        {
            ProcessStartInfo seleniumServerStart = new ProcessStartInfo("java");
            seleniumServerStart.Arguments = String.Format("-jar {0}", System.Configuration.ConfigurationSettings.AppSettings["SeleniumLocation"]);
            seleniumServerStart.WindowStyle = ProcessWindowStyle.Normal;

            seleniumServer = Process.Start(seleniumServerStart);
            seleniumServer.WaitForExit();
            isSeleniumRunning = true;
        }

        protected void StopSelenium()
        {
            seleniumServer.Close();
            isSeleniumRunning = false;
        }

        virtual protected bool UserLoggedIn
        {
            get { return !selenium.IsTextPresent("Sign In"); }
        }

        virtual protected void Login()
        {
            selenium.Click("ctl00_LoginStatus1");
            selenium.WaitForPageToLoad("30000");
            selenium.Type("ctl00_ContentPlaceHolder1_Login1_Login1_UserName", _3DR_Testing.Properties.Settings.Default._3DRUserName);
            selenium.Type("ctl00_ContentPlaceHolder1_Login1_Login1_Password", _3DR_Testing.Properties.Settings.Default._3DRPassword);
            selenium.Click("ctl00_ContentPlaceHolder1_Login1_Login1_LoginButton");
            selenium.WaitForPageToLoad("30000");
        }
    }
}
