using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _3DR_Testing;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace _3DR_Testing
{

    using System.Text.RegularExpressions;
    using System.Threading;
    using NUnit.Framework;
    //using Selenium;
    using System.IO;
    using System.Diagnostics;
    using vwarDAL;


    public struct UploadButtonIdentifier
    {
        public const string MODEL_UPLOAD = "ModelUploadButton";
        public const string SCREENSHOT_VIEWABLE = "ScreenshotUploadButton_Viewable";
        public const string SCREENSHOT_RECOGNIZED = "ScreenshotUploadButton_Recognized";
        public const string DEVLOGO = "DevLogoUploadButton";
        public const string SPONSORLOGO = "SponsorLogoUploadButton";
    }
    public struct FormDefaults
    {
        public const string Description = "Sample Description";
        public const string Tags = "Tag1, Tag2, Tag 3";
        public const string DeveloperName = "test developer name";
        public const string ArtistName = "test artist name";
        public const string DeveloperUrl = "www.example.com";
        public const string SponsorName = "test sponsor name";
        public const string LicenseTypeUrl = "http://creativecommons.org/licenses/by-sa/3.0/legalcode";
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

        private bool _ModelInRepository = false;
        private string _currentFormat;

        protected IDataRepository DAL;


        public override void SetupTest()
        {
            base.SetupTest();
            DAL = new DataAccessFactory().CreateDataRepositorProxy();
        }

        public override void TeardownTest()
        {
            if (_ModelInRepository)
            {
                string pid = selenium.GetEval("window.querySt('ContentObjectID')");
                DAL.DeleteContentObject(DAL.GetContentObjectById(pid, false));
            }
            base.TeardownTest();
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

            selenium.AttachFile(locator, filename);
            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(30)).Until(
                    d => ((bool)GetJsGlobalVar(varToWaitFor)).Equals(true)
                );

                return true;
            }
            catch (Exception e)
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

            driver.FindElement(By.CssSelector(".ui-dialog-buttonset > button:first-child")).Click();

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


            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(30)).Until(
                    d => !String.IsNullOrEmpty(GetJsGlobalVar("MODE").ToString()));

                _currentFormat = GetJsGlobalVar("MODE").ToString();
                string formatDetectStatus = driver.FindElement(By.Id("formatDetectStatus")).Text;//selenium.GetText("id=formatDetectStatus");
                int substringIndex = formatDetectStatus.LastIndexOf("Format Detected:");
                switch (_currentFormat)
                {
                    case "VIEWABLE":
                        Assert.GreaterOrEqual(substringIndex, 0);
                        new WebDriverWait(driver, TimeSpan.FromSeconds(30)).Until(
                            d => ((bool)GetJsGlobalVar("ModelConverted")).Equals(true));//selenium.WaitForCondition("window.ModelConverted == true", "120000");
                        string conversionStatus = driver.FindElement(By.Id("conversionStatus")).Text;//selenium.GetText("id=conversionStatus");
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
                        Assert.AreEqual("Server Error", formatDetectStatus);
                        return; //Invalid server response, test has ended
                }

            }
            catch (Exception e)
            {
                throw new NUnit.Framework.InconclusiveException(e.Message);
            }

            string title = String.Format("Automatic Upload of {0} at {1}",
                                            filename,
                                            DateTime.Now.ToString());

            driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_Upload1_TitleInput")).SendKeys(title);
            driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_Upload1_DescriptionInput")).SendKeys(FormDefaults.Description);
            driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_Upload1_TagsInput")).SendKeys(FormDefaults.Tags);

            IWebElement nextButton = driver.FindElement(By.Id("nextbutton_upload"));

            Assert.AreEqual("block", nextButton.GetCssValue("display"));
            nextButton.Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(30)).Until(
                d => ((bool)GetJsGlobalVar("GotGL")).Equals(true));

            Thread.Sleep(5000);
            string imageFileName = path + FormDefaults.ScreenshotFilename;
            if (_currentFormat == "VIEWABLE")
            {
                scaleValue = GetJsGlobalVar("WebGL.gUnitScale").ToString();
                upAxisValue = GetJsGlobalVar("WebGL.gUpVector").ToString();

                driver.FindElement(By.Id("SetThumbnailHeader")).Click();

                WaitForElementVisible(By.Id("ViewableSnapshotButton"));

                driver.FindElement(By.Id("ViewableSnapshotButton")).Click();
                new WebDriverWait(driver, TimeSpan.FromSeconds(15)).Until(
                    d =>
                    {
                        var thmbSrc = d.FindElement(By.Id("ThumbnailPreview_Viewable")).GetAttribute("src");
                        return thmbSrc != (string)GetJsGlobalVar("thumbnailLoadingLocation")
                            && thmbSrc != (string)GetJsGlobalVar("previewImageLocation");
                    });
            }
            else if (!UploadFile(imageFileName, UploadButtonIdentifier.SCREENSHOT_RECOGNIZED))
                return;

            driver.FindElement(By.Id("nextbutton_step2")).Click();

            WaitForElementVisible(By.Id("Step3Panel"), 5);

            driver.FindElement(By.Id("DeveloperName")).SendKeys(FormDefaults.DeveloperName);
            driver.FindElement(By.Id("ArtistName")).SendKeys(FormDefaults.ArtistName);
            driver.FindElement(By.Id("DeveloperUrl")).SendKeys(FormDefaults.DeveloperUrl);

            UploadFile(path + FormDefaults.DevLogoFilename, UploadButtonIdentifier.DEVLOGO);

            driver.FindElement(By.Id("SponsorInfoTab")).Click();
            WaitForElementVisible(By.Id("tabs-2"), 3);

            driver.FindElement(By.Id("SponsorName")).SendKeys(FormDefaults.SponsorName);
            UploadFile(path + FormDefaults.SponsorLogoFilename, UploadButtonIdentifier.SPONSORLOGO);

            driver.FindElement(By.Id("LicenseTypeTab")).Click();
            WaitForElementVisible(By.Id("tabs-3"), 3);

            if (_DoResubmitCheckTest)
            {
                driver.FindElement(By.Id("RequireResubmitCheckbox")).Click();
            }

            DateTime startTime = DateTime.Now;
            driver.FindElement(By.Id("nextbutton_step3")).Click();
            selenium.WaitForPageToLoad("120000");
            TimeSpan duration = DateTime.Now - startTime;
            Console.WriteLine(String.Format("Submit took {0} seconds", duration.Seconds.ToString()));
            _ModelInRepository = true;

            AssertDetails();
        }
        private void AssertDetails()
        {
            Assert.True(selenium.IsTextPresent(FormDefaults.ArtistName));
            Assert.True(selenium.IsTextPresent(FormDefaults.DeveloperName));
            Assert.True(selenium.IsTextPresent(FormDefaults.DeveloperUrl));
            Assert.True(selenium.IsTextPresent(FormDefaults.SponsorName));
            Assert.True(selenium.IsTextPresent(FormDefaults.Description));

            if (_DoResubmitCheckTest)
            {
                try
                {
                    driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_RequiresResubmitCheckbox"));
                }
                catch (OpenQA.Selenium.NoSuchElementException)
                {
                    verificationErrors.AppendLine("No require resubmit checkbox found.");
                }
            }

            int tagsCount = 0;
            string[] expectedTags = FormDefaults.Tags.Split(',');
            foreach (string s in expectedTags)
            {
                if (selenium.IsTextPresent(s))
                {
                    tagsCount++;
                }
            }
            if (tagsCount < expectedTags.Length)
            {
                verificationErrors.Append("Not all tags were found on the details page.");
            }

            Assert.AreEqual(FormDefaults.LicenseTypeUrl, selenium.GetAttribute("ctl00_ContentPlaceHolder1_CCLHyperLink@href"));
            if (_currentFormat == "VIEWABLE")
            {
                Thread.Sleep(2000);
                selenium.Click("link=3D View");

                new WebDriverWait(driver, TimeSpan.FromSeconds(30)).Until(
                    d => ((bool)GetJsGlobalVar("GotGL")).Equals(true));

                Thread.Sleep(5000);
                Assert.AreEqual(scaleValue, GetJsGlobalVar("WebGL.gUnitScale").ToString());
                Assert.AreEqual(upAxisValue.ToLower(), GetJsGlobalVar("WebGL.gUpVector").ToString().ToLower());
            }
        }

    }

}
