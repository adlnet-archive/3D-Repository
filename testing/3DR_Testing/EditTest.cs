using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace _3DR_Testing
{
    using vwarDAL;
    using NUnit.Framework;

    public class EditDefaults
    {
        public const string Title = "Edited Test";
        public const string FileName = "new_test.zip";
        public const string DevlogoName = "new_devlogo.jpg";
        public const string SponsorlogoName = "new_sponsorlogo.jpg";
        public const string DeveloperName = "Edited Developer Name";
        public const string ArtistName = "Edited Artist Name";
        public const string SponsorName = "Edited Sponsor Name";
        public const string Description = "This is a test description that has been edited";
        public const string DevUrl = "http://example.com/edited";
        public const string Tags = "NewTag1, NewTag2, NewTag3, NewTag4";

    }

    [TestFixture]
    public class EditTest : SeleniumTest
    {
        private string editedContentPath = _3DR_Testing.Properties.Settings.Default.ContentPath + "\\EditTest\\";

        [Test]
        public void TestChangeAllFields()
        {
            if (!UserLoggedIn)
            {
                Login();
            }
            ContentObject testCO = FedoraControl.AddDefaultObject();
            
            selenium.Open("/Users/Edit.aspx?ContentObjectID=" + testCO.PID);
            selenium.WaitForPageToLoad("30000");

            driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_EditControl_TitleTextBox")).SendKeys(EditDefaults.Title);
            driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_EditControl_ContentFileUpload")).SendKeys(editedContentPath + EditDefaults.FileName);
            driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_EditControl_RequireResubmitCheckbox")).Click();
            driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_EditControl_DeveloperLogoRadioButtonList_1")).Click();
            System.Threading.Thread.Sleep(5000);
            driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_EditControl_DeveloperLogoFileUpload")).SendKeys(editedContentPath + EditDefaults.DevlogoName);
            driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_EditControl_DeveloperNameTextBox")).SendKeys(EditDefaults.DeveloperName);
            driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_EditControl_ArtistNameTextBox")).SendKeys(EditDefaults.ArtistName);
            driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_EditControl_SponsorLogoRadioButtonList_1")).Click();
            System.Threading.Thread.Sleep(5000);
            driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_EditControl_SponsorLogoFileUpload")).SendKeys(editedContentPath + EditDefaults.SponsorlogoName);
            driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_EditControl_SponsorNameTextBox")).SendKeys(EditDefaults.SponsorName);
            driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_EditControl_DescriptionTextBox")).SendKeys(EditDefaults.Description);
            driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_EditControl_MoreInformationURLTextBox")).SendKeys(EditDefaults.DevUrl);

            //Delete the text in the textbox and replace it with the new tags
            selenium.GetEval("window.jQuery('#ctl00_ContentPlaceHolder1_EditControl_KeywordsTextBox').val('')");
            driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_EditControl_KeywordsTextBox")).SendKeys(EditDefaults.Tags);
           
            driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_EditControl_Step1NextButton")).Click();
            selenium.WaitForPageToLoad("1200000");


            driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_EditControl_ValidationViewSubmitButton")).Click();
            selenium.WaitForPageToLoad("1200000");

            IDataRepository dal = new DataAccessFactory().CreateDataRepositorProxy();
            ContentObject newCO = dal.GetContentObjectById(testCO.PID, false);
            Exception rethrow = null;
            try
            {
                Assert.True(newCO.Title == EditDefaults.Title);
                Assert.True(newCO.Keywords == EditDefaults.Tags.Replace(" ", ""));
                Assert.True(newCO.Description == EditDefaults.Description);
                Assert.True(newCO.ArtistName == EditDefaults.ArtistName);
                Assert.True(newCO.DeveloperName == EditDefaults.DeveloperName);
                Assert.True(newCO.MoreInformationURL == EditDefaults.DevUrl);

                //TODO: Determine if this is desired or non-desired behavior
                //string newfilename_base = EditDefaults.Title.ToLower().Replace(' ', '_') + ".zip";
                //Assert.True(newCO.OriginalFileName == "original_" + newfilename_base);
                //Assert.True(newCO.Location == newfilename_base);
                //Assert.True(newCO.DisplayFile == newfilename_base.Replace(".zip", ".o3d"));
            }
            catch (Exception e) {
                rethrow = e;
            }
            finally
            {

                dal.DeleteContentObject(newCO);
                //We have to rethrow to let NUnit know the test failed
                if (rethrow != null)
                {
                    throw rethrow;
                }
            }
        }


    }
}
