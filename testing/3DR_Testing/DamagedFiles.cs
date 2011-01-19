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

    namespace DamagedFiles
    {
        [TestFixture]
        public class FF_DamagedFiles
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
            public void DamagedZip(){
                Upload("DamagedZip.zip");
            }
            [Test]
            public void ZeroByteZip()
            {
                Upload("ZeroByteZip.zip");
            }
            [Test]
            public void DamagedDAE()
            {
                Upload("DamagedDAE.zip");
            }
            [Test]
            public void ZeroByteDAE()
            {
                Upload("ZeroByteDAE.zip");
            }
            [Test]
            public void DamagedFBX()
            {
                Upload("DamagedFBX.zip");
            }
            [Test]
            public void ZeroByteFBX()
            {
                Upload("ZeroByteFBX.zip");
            }
            [Test]
            public void Damaged3DS()
            {
                Upload("Damaged3DS.zip");
            }
            [Test]
            public void ZeroByte3DS()
            {
                Upload("ZeroByte3DS.zip");
            }
            [Test]
            public void DamagedOBJ()
            {
                Upload("DamagedOBJ.zip");
            }
            [Test]
            public void ZeroByteOBJ()
            {
                Upload("ZeroByteOBJ.zip");
            }
            [Test]
            public void DamagedSKP()
            {
                Upload("DamagedSKP.zip");
            }
            [Test]
            public void ZeroByteSKP()
            {
                Upload("ZeroByteSKP.zip");
            }
           



            public void Upload(string mFileToUpload)
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
                selenium.Type("ctl00_ContentPlaceHolder1_Upload1_TitleTextBox", "Invalid File: " + mFileToUpload + " " + System.DateTime.Now.ToString());
                selenium.Type("ctl00_ContentPlaceHolder1_Upload1_ContentFileUpload", path + mFileToUpload);
                selenium.Click("ctl00_ContentPlaceHolder1_Upload1_Step1NextButton");
                selenium.WaitForPageToLoad("300000");

                selenium.Click("ctl00_ContentPlaceHolder1_Upload1_ValidationViewSubmitButton");
                selenium.WaitForPageToLoad("30000");

            }
        }
        //class IE_DamagedFiles : FF_DamagedFiles
        //{
        //    [SetUp]
        //    public override void SetupTest()
        //    {
        //        selenium = new DefaultSelenium("localhost", 4444, "*iehta", Properties.Settings.Default._3DRURL);
        //        selenium.Start();
        //        verificationErrors = new StringBuilder();
        //    }
        //}
        //class CH_DamagedFiles : FF_DamagedFiles
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
