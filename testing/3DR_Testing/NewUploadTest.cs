using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _3DR_Testing;

namespace _3DR_Uploading
{
    using System.Text.RegularExpressions;
    using System.Threading;
    using NUnit.Framework;
    using Selenium;
    using System.IO;
    using System.Diagnostics;

    public enum AutoItResult { NO_ERROR, NOT_ENOUGH_ARGS, DIALOG_NEVER_APPEARS, EDIT_FILENAME_ERROR, OPEN_CLICK_ERROR }
    public struct UploadButtonIdentifier
    {
        public const string MODEL_UPLOAD = "ModelUploadButton";
        public const string SCREENSHOT_VIEWABLE = "ScreenshotUploadButton_Viewable";
        public const string SCREENSHOT_RECOGNIZED = "ScreenshotUploadButton_Recognized";
        public const string DEVLOGO = "DevLogoUploadButton";
        public const string SPONSORLOGO = "SponsorLogoUploadButton";
    }
    public struct BrowserMode
    {
        public const string IE = "exploder"; //you know it's true...
        public const string FIREFOX = "firefox";
    }

    

    namespace NewUploadAll
    {
        [TestFixture]
        public class NewUploadTest
        {
            protected ISelenium selenium;
            protected StringBuilder verificationErrors;
            private string path;
            private HttpCommandProcessor proc;

            [SetUp]
            virtual public void SetupTest()
            {
                proc = new HttpCommandProcessor("localhost", 4444, "*chrome", _3DR_Testing.Properties.Settings.Default._3DRURL);
                selenium = new DefaultSelenium(proc);
                verificationErrors = new StringBuilder();    
                selenium.Start();
                
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

                }
                Assert.AreEqual(String.Empty, verificationErrors.ToString());
            }


            [Test]
            public void TestUpload([Values("SU27.zip")] string filename)
            {
                selenium.WindowMaximize();
                selenium.Open("/Default.aspx");
                selenium.WindowFocus();
                
                if (!UserLoggedIn)
                {
                    Login();
                }

                selenium.WaitForPageToLoad("30000");
                selenium.Open("/Users/Upload.aspx");
                selenium.WaitForPageToLoad("30000");

                path = _3DR_Testing.Properties.Settings.Default.ContentPath;
                if (String.IsNullOrEmpty(filename))
                {
                    verificationErrors.Append("Error, filename to be upload cannot be blank.");
                    return;
                }
                else if (!File.Exists(path + filename))
                {
                    verificationErrors.Append("Error, " + path + filename + " could not be found");
                    return;
                }

               
                bool uploadResult = UploadFile(path + filename, UploadButtonIdentifier.MODEL_UPLOAD);
                if (!uploadResult)
                {
                    return;
                }

                string windowHandle = "selenium.browserbot.getCurrentWindow()";
                string currentFormat = "";
                try
                {
                    selenium.WaitForCondition(windowHandle + ".MODE != ''", "20000");
                    currentFormat = selenium.GetEval(windowHandle+".MODE");
                    string formatDetectStatus = selenium.GetText("id=formatDetectStatus");
                    int substringIndex = formatDetectStatus.LastIndexOf("Format Detected:");
                    switch(currentFormat)
                    {
                        case "VIEWABLE":  
                            Assert.GreaterOrEqual(substringIndex, 0);
                            selenium.WaitForCondition(windowHandle + ".ModelConverted == true", "120000");
                            string conversionStatus = selenium.GetText("id=conversionStatus");
                            if (conversionStatus != "Conversion Failed")
                            {
                                Assert.AreEqual("Model Ready for Viewer", conversionStatus);
                                break;
                            }
                            else return; //Conversion failed, test has ended
                        case "RECOGNIZED":
                            Assert.GreaterOrEqual(substringIndex, 0);
                            break;
                        case "MULTIPLE_RECOGNIZED":
                            Assert.AreEqual("Multiple Models Detected", formatDetectStatus);
                            return; //We have multiple recognized models, so the test has ended
                        case "UNRECOGNIZED":
                            Assert.AreEqual("Unrecognized Format", formatDetectStatus);
                            return; //Unrecognized format, test has ended
                        default:
                            Assert.AreEqual("Server Error", formatDetectStatus );
                            return; //Invalid server response, test has ended
                    } 

                }
                catch (SeleniumException e)
                {
                    throw new NUnit.Framework.InconclusiveException(e.Message);
                }

                string title = String.Format("Automatic Upload of {0} at {1}",
                                                filename,
                                                DateTime.Now.ToString());

                selenium.Type("id=ctl00_ContentPlaceHolder1_Upload1_TitleInput", title);
                selenium.Type("id=ctl00_ContentPlaceHolder1_Upload1_DescriptionInput", "Sample Description");
                selenium.Type("id=ctl00_ContentPlaceHolder1_Upload1_TagsInput", "Tag1, Tag2, Tag 3");

                string nextButtonDisplay = selenium.GetEval(windowHandle+".jQuery('#nextbutton_upload').css('display')");
                Assert.AreEqual("block", nextButtonDisplay);
                
            }

            protected bool UserLoggedIn
            {
                get { return !selenium.IsTextPresent("Sign In");}
            }

            protected void Login()
            {
                
                {
                    selenium.Click("ctl00_LoginStatus1");
                    selenium.WaitForPageToLoad("30000");
                    selenium.Type("ctl00_ContentPlaceHolder1_Login1_Login1_UserName", _3DR_Testing.Properties.Settings.Default._3DRUserName);
                    selenium.Type("ctl00_ContentPlaceHolder1_Login1_Login1_Password", _3DR_Testing.Properties.Settings.Default._3DRPassword);
                    selenium.Click("ctl00_ContentPlaceHolder1_Login1_Login1_LoginButton");
                }
               
            }

            protected bool UploadFile(string filename, string buttonType )
            {
                string locator = String.Format("//div[@id='{0}']/input[@type='file']", buttonType);
                
                string varToWaitFor = "";
                switch (buttonType)
                {
                    case UploadButtonIdentifier.MODEL_UPLOAD:
                        varToWaitFor = "ModelUploadFinished";
                        break;
                    case UploadButtonIdentifier.SCREENSHOT_VIEWABLE:
                        varToWaitFor = "ViewableThumbnailUpload.Finished";
                        break;
                    case UploadButtonIdentifier.SCREENSHOT_RECOGNIZED:
                        varToWaitFor = "RecognizedThumbnailUpload.Finished";
                        break;
                    case UploadButtonIdentifier.DEVLOGO:
                        varToWaitFor = "DevLogoUpload.Finished";
                        break;
                    case UploadButtonIdentifier.SPONSORLOGO:
                        varToWaitFor = "SponsorLogoUpload.Finished";
                        break;
                    default:
                        verificationErrors.Append("UploadButtonIdentifier is not recognized. ");
                        return false;
                }

                string varHandle = String.Format("selenium.browserbot.getCurrentWindow().{0}", varToWaitFor);
                selenium.AddScript(varHandle + " = false;", "UploadResetter_" + buttonType);
                selenium.Type(locator, filename);
                try
                {
                    selenium.WaitForCondition(varHandle + " == true", "120000");
                    return true;
                }
                catch (SeleniumException e)
                {
                    verificationErrors.Append(e.Message);
                    return false;
                }
            }
        }
    }
}
