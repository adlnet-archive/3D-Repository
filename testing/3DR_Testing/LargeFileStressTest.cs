
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

        [TestFixture]
        public class LargeFileStressTest
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
            public void LargeUpload([Range(0,100)] int i)
            {
                Upload("mwrap.zip");
            }

            public virtual void SetUploadFile(string infile)
            {
                selenium.Type("ctl00_ContentPlaceHolder1_Upload1_ContentFileUpload", infile);
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
                selenium.Type("ctl00_ContentPlaceHolder1_Upload1_TitleTextBox", "Automatic Test of " + mFileToUpload + " " + System.DateTime.Now.ToString());




                SetUploadFile(path + mFileToUpload);



                selenium.Click("ctl00_ContentPlaceHolder1_Upload1_Step1NextButton");
                selenium.WaitForPageToLoad("1200000");
                selenium.WaitForCondition("window.GetLoadingComplete != undefined", "300000");
                int count = 0;
                while ((selenium.GetEval("window.GetLoadingComplete() == true") != "true" && count < 300))
                {
                    count++;
                    System.Threading.Thread.Sleep(1000);

                }

                if (selenium.GetEval("window.GetLoadingComplete() == true") == "true")
                {
                    selenium.GetEval("window.TakeScreenShot();");
                    System.Threading.Thread.Sleep(3000);
                    //count = 0;
                    //string Thumbstring = selenium.GetEval("document.getElementById('ctl00_ContentPlaceHolder1_Upload1_ThumbnailImage')");
                    //while (Thumbstring.IndexOf("ScreenShot") == -1 && count < 10)
                    //{
                    //    count++;
                    //    System.Threading.Thread.Sleep(1000);
                    //    Thumbstring = selenium.GetEval("document.getElementById('ctl00_ContentPlaceHolder1_Upload1_ThumbnailImage')");
                    //}
                }



                //  ctl00_ContentPlaceHolder1_Upload1_ThumbnailImage
                selenium.Click("ctl00_ContentPlaceHolder1_Upload1_ValidationViewSubmitButton");
                selenium.WaitForPageToLoad("1200000");

            }
        }
        //    class IE_SimpleUpload : FF_SimpleUpload
        //    {
        //        [SetUp]
        //         public override void SetupTest()
        //        {
        //            selenium = new DefaultSelenium("localhost", 4444, "*iehta", Properties.Settings.Default._3DRURL);
        //            selenium.Start();
        //            verificationErrors = new StringBuilder();
        //        }
        //        public override void SetUploadFile(string infile)
        //        {
        //            selenium.GetEval("window.document.getElementById('ctl00_ContentPlaceHolder1_Upload1_ContentFileUpload').focus();");
        //            for (int i = 0; i < infile.Length; i++)
        //            {
        //                int key = System.Convert.ToInt32(infile.ToUpper()[i]);
        //                if ((key >= 65 && key <= 90) || (key >= 48 && key <= 57))
        //                    selenium.KeyPressNative(key.ToString());
        //                else
        //                    if (infile[i] == ':')
        //                    {
        //                        selenium.KeyDownNative("16");
        //                        selenium.KeyPressNative("59");
        //                        selenium.KeyUpNative("16");
        //                    }
        //                    if (infile[i] == '\\')
        //                        selenium.KeyPressNative("92");
        //                    if (infile[i] == '\'')
        //                        selenium.KeyPressNative("222");
        //                    if (infile[i] == '`')
        //                        selenium.KeyPressNative("131");

        //                    if (infile[i] == '(')
        //                    {
        //                        selenium.KeyDownNative("16");
        //                        selenium.KeyPressNative("48");
        //                        selenium.KeyUpNative("16");
        //                    }
        //                    if (infile[i] == ')')
        //                    {
        //                        selenium.KeyDownNative("16");
        //                        selenium.KeyPressNative("57");
        //                        selenium.KeyUpNative("16");
        //                    }
        //                    if (infile[i] == '.')
        //                        selenium.KeyPressNative("46");
        //                    if (infile[i] == ' ')
        //                        selenium.KeyPressNative("32");
        //                    if (infile[i] == '_')
        //                    {
        //                        selenium.KeyDownNative("16");
        //                        selenium.KeyPressNative("45");
        //                        selenium.KeyUpNative("16");
        //                    }


        //            }

        //        }
        //        [TearDown]
        //        new public void TeardownTest()
        //        { 
        //           // try{ selenium.Stop();}
        //           // catch (Exception){}
        //           // Assert.AreEqual("", verificationErrors.ToString());
        //        }
        //    }
        //    //class CH_SimpleUpload : FF_SimpleUpload
        //    //{
        //    //    [SetUp]
        //    //    public override void SetupTest()
        //    //    {
        //    //        selenium = new DefaultSelenium("localhost", 4444, "*chrome", Properties.Settings.Default._3DRURL);
        //    //        selenium.Start();
        //    //        verificationErrors = new StringBuilder();
        //    //    }
        //    //}
    
}
