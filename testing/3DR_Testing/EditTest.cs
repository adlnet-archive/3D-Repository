using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            selenium.Type("ctl00_ContentPlaceHolder1_EditControl_TitleTextBox", EditDefaults.Title);
            selenium.Type("ctl00_ContentPlaceHolder1_EditControl_ContentFileUpload", editedContentPath + EditDefaults.FileName);
            selenium.Check("ctl00_ContentPlaceHolder1_EditControl_RequireResubmitCheckbox");
            selenium.Click("ctl00_ContentPlaceHolder1_EditControl_DeveloperLogoRadioButtonList_1");
            System.Threading.Thread.Sleep(1000);
            selenium.Type("ctl00_ContentPlaceHolder1_EditControl_DeveloperLogoFileUpload", editedContentPath + EditDefaults.DevlogoName);
            selenium.Type("ctl00_ContentPlaceHolder1_EditControl_DeveloperNameTextBox", EditDefaults.DeveloperName);
            selenium.Type("ctl00_ContentPlaceHolder1_EditControl_ArtistNameTextBox", EditDefaults.ArtistName);
            selenium.Click("ctl00_ContentPlaceHolder1_EditControl_SponsorLogoRadioButtonList_1");
            System.Threading.Thread.Sleep(1000);
            selenium.Type("ctl00_ContentPlaceHolder1_EditControl_SponsorLogoFileUpload", editedContentPath + EditDefaults.SponsorlogoName);
            selenium.Type("ctl00_ContentPlaceHolder1_EditControl_SponsorNameTextBox", EditDefaults.SponsorName);
            selenium.Type("ctl00_ContentPlaceHolder1_EditControl_DescriptionTextBox", EditDefaults.Description);
            selenium.Type("ctl00_ContentPlaceHolder1_EditControl_MoreInformationURLTextBox", EditDefaults.DevUrl);

            //Remove the old keywords
            foreach(string t in FormDefaults.Tags.Split(','))
            {
                string formattedTag = t.Trim();
                selenium.AddSelection("ctl00_ContentPlaceHolder1_EditControl_KeywordsListBox", "label=" + formattedTag);
                selenium.Click("ctl00_ContentPlaceHolder1_EditControl_RemoveKeywordsButton");
                System.Threading.Thread.Sleep(1000);
            }

            ////Add the new keywords
            //foreach (string t in EditDefaults.Tags.Split(','))
            //{
            //    selenium.Type("ctl00_ContentPlaceHolder1_EditControl_KeywordsTextBox_Input", t);
            //    System.Threading.Thread.Sleep(1000);
            //    selenium.Click("ctl00_ContentPlaceHolder1_EditControl_AddKeywordButton");
            //    System.Threading.Thread.Sleep(1000);
            //}

            selenium.Click("ctl00_ContentPlaceHolder1_EditControl_Step1NextButton");
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
            }


            selenium.Click("ctl00_ContentPlaceHolder1_EditControl_ValidationViewSubmitButton");
            selenium.WaitForPageToLoad("1200000");

            IDataRepository dal = new DataAccessFactory().CreateDataRepositorProxy();
            ContentObject newCO = dal.GetContentObjectById(testCO.PID, false);
            try
            {
                Assert.True(newCO.Title == EditDefaults.Title);
                //Assert.True(newCO.Keywords == EditDefaults.Tags);
                Assert.True(newCO.Description == EditDefaults.Description);
                Assert.True(newCO.ArtistName == EditDefaults.ArtistName);
                Assert.True(newCO.DeveloperName == EditDefaults.DeveloperName);
                Assert.True(newCO.MoreInformationURL == EditDefaults.DevUrl);

                string newfilename_base = EditDefaults.Title.ToLower().Replace(' ', '_') + ".zip";
                Assert.True(newCO.OriginalFileName == "original_" + newfilename_base);
                Assert.True(newCO.Location == newfilename_base);
                Assert.True(newCO.DisplayFile == newfilename_base.Replace(".zip", ".o3d"));
            }
            catch (Exception e) { }
            finally
            {

                dal.DeleteContentObject(newCO.PID);
            }
        }


    }
}
