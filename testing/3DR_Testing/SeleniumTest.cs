using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Diagnostics;
using System.Configuration;


namespace _3DR_Testing
{
 

    public abstract class SeleniumTest
    {
        protected WebDriverBackedSelenium selenium;
        protected IWebDriver driver;

        protected string path;
        protected StringBuilder verificationErrors;
        protected Process seleniumServer;
        protected bool isSeleniumRunning = false;

        [SetUp]
        virtual public void SetupTest()
        {
            path = ConfigurationManager.AppSettings["ContentPath"].ToString();
            driver = new FirefoxDriver();
            
            selenium = new WebDriverBackedSelenium(driver, ConfigurationManager.AppSettings["_3DRURL"]);
            verificationErrors = new StringBuilder();
            selenium.Start();
            selenium.Open("/Default.aspx");
            selenium.WindowFocus();
            isSeleniumRunning = true;
        }

        [TearDown]
        virtual public void TeardownTest()
        {
            try
            {
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
            string username = (logInAsAdmin) ? ConfigurationManager.AppSettings["_3DR_AdminUserName"] : ConfigurationManager.AppSettings["_3DR_UserName"];
            string password = (logInAsAdmin) ? ConfigurationManager.AppSettings["_3DR_AdminPassword"] : ConfigurationManager.AppSettings["_3DR_Password"];
            driver.FindElement(By.Id("ctl00_LoginStatus1")).Click();
            selenium.WaitForPageToLoad("30000");
            driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_Login1_Login1_UserName")).SendKeys(username);
            driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_Login1_Login1_Password")).SendKeys(password);
            driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_Login1_Login1_LoginButton")).Click();
            selenium.WaitForPageToLoad("30000");
        }

        protected object GetJsGlobalVar(string name)
        {
            return ((IJavaScriptExecutor)this.driver).ExecuteScript("return window." + name);
        }

        protected void WaitForElementVisible(By by, int secondsToTimeout = 30)
        {
            
            new WebDriverWait(driver, TimeSpan.FromSeconds(secondsToTimeout)).Until(
                        d => d.FindElement(by).Displayed);
        }
    }
}
