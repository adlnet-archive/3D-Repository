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
            catch { }
            Assert.AreEqual(String.Empty, verificationErrors.ToString());
        }

        virtual protected bool UserLoggedIn
        {
            get { return !selenium.IsTextPresent("Sign In"); }
        }

        virtual protected void Login(bool logInAsAdmin = true)
        {
            string username = (logInAsAdmin) ? Properties.Settings.Default._3DR_AdminUserName : Properties.Settings.Default._3DR_UserName;
            string password = (logInAsAdmin) ? Properties.Settings.Default._3DR_AdminPassword : Properties.Settings.Default._3DR_Password;
            selenium.Click("ctl00_LoginStatus1");
            selenium.WaitForPageToLoad("30000");
            selenium.Type("ctl00_ContentPlaceHolder1_Login1_Login1_UserName", username);
            selenium.Type("ctl00_ContentPlaceHolder1_Login1_Login1_Password", password);
            selenium.Click("ctl00_ContentPlaceHolder1_Login1_Login1_LoginButton");
            selenium.WaitForPageToLoad("30000");
        }
    }
}
