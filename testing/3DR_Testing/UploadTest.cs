using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _3DR_Testing;

namespace _3DR_Testing
{

    using System.Text.RegularExpressions;
    using System.Threading;
    using NUnit.Framework;
    using Selenium;
    using System.IO;
    using System.Diagnostics;

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
    public struct FormDefaults
    {
        public const string Description = "Sample Description";
        public const string Tags = "Tag1, Tag2, Tag 3";
        public const string DeveloperName = "test developer name";
        public const string ArtistName = "test artist name";
        public const string DeveloperUrl = "www.example.com";
        public const string SponsorName = "test sponsor name";
        public const string LicenseTypeUrl = "http://creativecommons.org/publicdomain/mark/1.0/";
        public const string ScreenshotFilename = "DefaultScreenshot.png";
        public const string DevLogoFilename = "DefaultDevLogo.jpg";
        public const string SponsorLogoFilename = "DefaultSponsorLogo.jpg";
        public const string Title = "test";
    }

        [TestFixture]
        public abstract class UploadTest : SeleniumTest
        {
            
           
            
            protected string scaleValue;
            protected string upAxisValue;
            
            private bool _DoResubmitCheckTest = false;
            protected bool DoResubmitCheckTest
            {
                get { return _DoResubmitCheckTest; }
                set { _DoResubmitCheckTest = value; }
            }

            virtual protected string GetImageFileName(string filename)
            {

                string[] allowedImageExtensions = { "jpg", "png", "gif" };
                foreach (string s in allowedImageExtensions)
                {
                    string newFilename = filename.Replace("zip", s);
                    if (File.Exists(path + newFilename))
                    {
                        return path + newFilename;
                    }
                }

                throw new Exception("No image file found.");
            }
            virtual protected bool UploadFile(string filename, string buttonType)
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
            virtual protected void NavigateToUploadPage()
            {
               
                selenium.Open("/Users/Upload.aspx");
                selenium.WaitForPageToLoad("30000");
            }
            virtual protected void TestUpload(string filename)
            {

                
                if (!UserLoggedIn)
                {
                    Login();
                }

                NavigateToUploadPage();

                
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
                selenium.Type("id=ctl00_ContentPlaceHolder1_Upload1_DescriptionInput", FormDefaults.Description);
                selenium.Type("id=ctl00_ContentPlaceHolder1_Upload1_TagsInput", FormDefaults.Tags);

                string nextButtonDisplay = selenium.GetEval(windowHandle+".jQuery('#nextbutton_upload').css('display')");
                Assert.AreEqual("block", nextButtonDisplay);

                selenium.Click("id=nextbutton_upload");
                selenium.WaitForCondition("window.g_init == true", "60000");
                Thread.Sleep(2000);
                string imageFileName = path + FormDefaults.ScreenshotFilename;//GetImageFileName(filename);
                if (currentFormat == "VIEWABLE")
                {
                    scaleValue = selenium.GetEval("window.g_unitscale");
                    upAxisValue = selenium.GetEval("window.g_upaxis");
                    selenium.GetEval("window.updateCamera()");
                    selenium.Click("id=SetThumbnailHeader");
                    Thread.Sleep(500);
                    selenium.Click("id=ViewableSnapshotButton");
                    selenium.WaitForCondition("var thumbnailSrc = " + windowHandle + ".jQuery('#ThumbnailPreview_Viewable').attr('src');" +
                                              "thumbnailSrc != window.thumbnailLoadingLocation && thumbnailSrc != window.previewImageLocation", "30000");

                }
                else
                { 
                    if (!UploadFile(imageFileName, UploadButtonIdentifier.SCREENSHOT_RECOGNIZED))
                    {
                        return;
                    } 
                }

                selenium.Click("id=nextbutton_step2");
                Thread.Sleep(3000);

                selenium.Type("id=DeveloperName", FormDefaults.DeveloperName);
                selenium.Type("id=ArtistName", FormDefaults.ArtistName);
                selenium.Type("id=DeveloperUrl", FormDefaults.DeveloperUrl);
                UploadFile(path + FormDefaults.DevLogoFilename, UploadButtonIdentifier.DEVLOGO);

                selenium.Click("id=SponsorInfoTab");

                selenium.Type("id=SponsorName", FormDefaults.SponsorName);
                UploadFile(path + FormDefaults.SponsorLogoFilename, UploadButtonIdentifier.SPONSORLOGO);

                if (_DoResubmitCheckTest)
                {
                    selenium.Check("id=RequireResubmitCheckbox");
                }

                DateTime startTime = DateTime.Now;
                selenium.Click("id=nextbutton_step3");
                selenium.WaitForPageToLoad("120000");
                TimeSpan duration = DateTime.Now - startTime;
                Console.WriteLine(String.Format("Submit took {0} seconds", duration.Seconds.ToString()));
                Assert.True(selenium.IsTextPresent(FormDefaults.ArtistName));
                Assert.True(selenium.IsTextPresent(FormDefaults.DeveloperName));
                Assert.True(selenium.IsTextPresent(FormDefaults.DeveloperUrl));
                Assert.True(selenium.IsTextPresent(FormDefaults.SponsorName));
                Assert.True(selenium.IsTextPresent(FormDefaults.Description));

                if (_DoResubmitCheckTest)
                {
                    string eval_script = windowHandle + ".document.getElementById('ctl00_ContentPlaceHolder1_RequiresResubmitCheckbox') !== undefined";
                    Assert.AreEqual(selenium.GetEval(eval_script), "true");
                    
                }

                int tagsCount = 0;
                string[] expectedTags = FormDefaults.Tags.Split(',');
                foreach (string s in expectedTags)
                {
                    if(selenium.IsTextPresent(s))
                    {
                        tagsCount++;
                    }
                }
                if (tagsCount < expectedTags.Length)
                {
                    throw new Exception("Not all tags were found on the details page.");
                }

                Assert.AreEqual(FormDefaults.LicenseTypeUrl, selenium.GetAttribute("ctl00_ContentPlaceHolder1_CCLHyperLink@href"));
                if (currentFormat == "VIEWABLE")
                {
                    Thread.Sleep(4000);
                    selenium.Click("xpath=//div[@id='ctl00_ContentPlaceHolder1_ViewOptionsTab']/div/ul/li[2]/a/span/span/span");
                    selenium.WaitForCondition("window.g_init == true", "60000");
                    Assert.AreEqual(scaleValue, selenium.GetEval("window.g_unitscale"));
                    Assert.AreEqual(upAxisValue.ToLower(), selenium.GetEval("window.g_upaxis").ToLower());
                }
                
            }

        }
    
}
