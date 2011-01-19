using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DR_Testing
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using NUnit.Framework;

    using Selenium;

    namespace ScreenShot
    {
        [TestFixture]
        public class FF_ScreenShot
        {
            protected ISelenium selenium;
            protected StringBuilder verificationErrors;


            private string path;
           
            [SetUp]
            virtual public void SetupTest()
            {
                selenium = new DefaultSelenium("localhost", 4444, "*chrome", Properties.Settings.Default._3DRURL);
                selenium.Start();
                verificationErrors = new StringBuilder();
            }

            [TearDown]
            public void TeardownTest()
            {
                try
                {
                    selenium.Stop();
                }
                catch (Exception)
                {
                    // Ignore errors if unable to close the browser
                }
                Assert.AreEqual("", verificationErrors.ToString());
            }

            [Test]
            public void ScreenShotTest(
                 [Values(
                 "duffle_bag.zip"
                 )] 
                string mFileToUpload,
                 [Values(
                 "o3d",
                 "flash"
                 )] 
                string viewertype
                )
            {
                path = Properties.Settings.Default.ContentPath;
                if (mFileToUpload == "")
                    return;
                selenium.Open("/Default.aspx");

                if (selenium.IsTextPresent("Sign In"))
                {
                    selenium.Click("ctl00_LoginStatus1");
                    selenium.WaitForPageToLoad("30000");
                    selenium.Type("ctl00_ContentPlaceHolder1_Login1_Login1_UserName", Properties.Settings.Default._3DRUserName);
                    selenium.Type("ctl00_ContentPlaceHolder1_Login1_Login1_Password", Properties.Settings.Default._3DRPassword);
                    selenium.Click("ctl00_ContentPlaceHolder1_Login1_Login1_LoginButton");
                }
                selenium.WaitForPageToLoad("30000");
                selenium.Open("/Users/Upload.aspx");
                selenium.WaitForPageToLoad("30000");
                selenium.Type("ctl00_ContentPlaceHolder1_Upload1_TitleTextBox", "Screenshot Test" + System.DateTime.Now.ToString());
                selenium.Type("ctl00_ContentPlaceHolder1_Upload1_ContentFileUpload", path + mFileToUpload);
                selenium.Click("ctl00_ContentPlaceHolder1_Upload1_Step1NextButton");
                selenium.WaitForPageToLoad("300000");
                selenium.WaitForCondition("window.GetLoadingComplete != undefined", "20000");
                int count = 0;
                while ((selenium.GetEval("window.GetLoadingComplete() == true") != "true" && count < 300))
                {
                    count++;
                    System.Threading.Thread.Sleep(1000);

                }

                NUnit.Framework.Assert.AreEqual(selenium.GetEval("window.GetLoadingComplete() == true"), "true");

                selenium.GetEval("window.TakeScreenShot();");
                System.Threading.Thread.Sleep(10000);

                //document.getElementById('ctl00_ContentPlaceHolder1_Upload1_ThumbnailImage').src
                string Thumbstring = selenium.GetEval("window.document.getElementById('ctl00_ContentPlaceHolder1_Upload1_ThumbnailImage').src");

                NUnit.Framework.StringAssert.Contains("ScreenShot", Thumbstring);

                selenium.Click("ctl00_ContentPlaceHolder1_Upload1_ValidationViewSubmitButton");
                selenium.WaitForPageToLoad("30000");


                string BigThumbSTring = selenium.GetEval("window.document.getElementById('ctl00_ContentPlaceHolder1_ScreenshotImage').src");
                NUnit.Framework.StringAssert.Contains("screenshot", BigThumbSTring);
                

            }
        }
        //class IE_ScreenShot : FF_ScreenShot
        //{
        //    [SetUp]
        //    public override void SetupTest()
        //    {
        //        selenium = new DefaultSelenium("localhost", 4444, "*iehta", Properties.Settings.Default._3DRURL);
        //        selenium.Start();
        //        verificationErrors = new StringBuilder();
        //    }
        //}
        //class CH_ScreenShot : FF_ScreenShot
        //{
        //    [SetUp]
        //    public override void SetupTest()
        //    {
        //        selenium = new DefaultSelenium("localhost", 4444, "*chrome", Properties.Settings.Default._3DRURL);
        //        selenium.Start();
        //        verificationErrors = new StringBuilder();
        //    }
        //}
    }

}
