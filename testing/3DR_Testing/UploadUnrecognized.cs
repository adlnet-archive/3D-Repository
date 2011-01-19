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

    namespace UploadUnrecognized
    {
        [TestFixture]
        public class FF_SimpleUpload
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
           public void Test(
                [Values(
                "desk.zip",
                "desk_and_chair.zip",
                "desk_chair.zip",
                "glowing_pointer.zip",
                "info_booth.zip",
                "exit_sign.zip",
                "jeep_full.zip",
                "jeep_windows_down.zip",
                "news_stand.zip",
                "street_fighter_arcade.zip",
                "q70_chair.zip"
                )] 
                string mFileToUpload)
                {
                if (mFileToUpload == "")
                    return;
                selenium.Open("/Default.aspx");

                path = Properties.Settings.Default.ContentPath;

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
                selenium.Type("ctl00_ContentPlaceHolder1_Upload1_TitleTextBox", "Automatic Test of " + mFileToUpload + " " + System.DateTime.Now.ToString());
                selenium.Type("ctl00_ContentPlaceHolder1_Upload1_ContentFileUpload", path + mFileToUpload);
                selenium.Click("ctl00_ContentPlaceHolder1_Upload1_Step1NextButton");
                selenium.WaitForPageToLoad("300000");
               

                //  ctl00_ContentPlaceHolder1_Upload1_ThumbnailImage
                selenium.Click("ctl00_ContentPlaceHolder1_Upload1_ValidationViewSubmitButton");
                selenium.WaitForPageToLoad("30000");
            }
        }
        //class IE_SimpleUpload : FF_SimpleUpload
        //{
        //    [SetUp]
        //    public override void SetupTest()
        //    {
        //        selenium = new DefaultSelenium("localhost", 4444, "*iehta", Properties.Settings.Default._3DRURL);
        //        selenium.Start();
        //        verificationErrors = new StringBuilder();
        //    }
        //}
        //class CH_SimpleUpload : FF_SimpleUpload
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
