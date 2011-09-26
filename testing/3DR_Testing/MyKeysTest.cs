using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using vwar.service.host;
using System.Threading;
using System.Configuration;

namespace _3DR_Testing
{
    [TestFixture]
    public class MyKeysTest : SeleniumTest
    {

        private APIKey mTestKey;
        private APIKeyManager mKeyMananger;

        [SetUp]
        public override void SetupTest()
        {
            base.SetupTest();
            Login(false);
            mTestKey = new APIKey();
            mTestKey.Email = ConfigurationManager.AppSettings["_3DR_UserName"];
            mTestKey.Usage = "This is a test for the key request";
            mKeyMananger = new APIKeyManager();
        }

        [TearDown]
        public override void TeardownTest()
        {
            base.TeardownTest();
            if (!String.IsNullOrEmpty(mTestKey.Key))
                mKeyMananger.DeleteKey(mTestKey.Key);
        }

        [Test]
        public void TestAdd()
        {

            selenium.Open("/Users/Profile.aspx");

            driver.FindElement(By.Id("RequestKeyLink")).Click();
            Thread.Sleep(1000); //Wait for ajax fetch of form
            selenium.Type("#UsageTextArea", "This is a test for the key request");
            driver.FindElement(By.Id("KeyRequestSubmit")).Click();

            Thread.Sleep(2000); //Wait for ajax key request
            selenium.Click("//input[@value='Ok']");
            

            string addedKeyVal = selenium.GetEval("window.jQuery('#ctl00_ContentPlaceHolder1_KeysControl_APIKeysListView_KeysTable tr:last').find('.key').text().trim()");

            Assert.IsNotNullOrEmpty(addedKeyVal);

            mTestKey.Key = addedKeyVal;
            mTestKey.State = APIKeyState.ACTIVE;

            APIKey serverKey = mKeyMananger.GetKeyByKey(addedKeyVal);
            Assert.True(mTestKey.Equals(serverKey));
        }

        [Test]
        public void TestEdit()
        {
            APIKey dummyInsertKey = mKeyMananger.CreateKey(mTestKey.Email, mTestKey.Usage);

            selenium.Open("/Users/Profile.aspx");

            mTestKey.Usage = "This has been edited, part of key UI test";
            mTestKey.Key = dummyInsertKey.Key;
            mTestKey.State = APIKeyState.ACTIVE;
           
            driver.FindElement(By.CssSelector("tr:last-child .update-key-request")).Click();
            WaitForElementVisible(By.Id("KeyRequestForm"));

            driver.FindElement(By.CssSelector("#KeyRequestForm textarea")).SendKeys(mTestKey.Usage);
            driver.FindElement(By.Id("KeyRequestSubmit")).Click();

            Thread.Sleep(2000);
            selenium.Click("//input[@value='Ok']");

            //Make sure the UI is updated
            Assert.AreEqual(driver.FindElement(By.CssSelector("tr:last-child .usage")).Text,
                            mTestKey.Usage);

            Assert.True(mTestKey.Equals(mKeyMananger.GetKeyByKey(dummyInsertKey.Key)));
        }

        [Test]
        public void TestDelete()
        {
            APIKey dummyInsertKey = mKeyMananger.CreateKey(mTestKey.Email, mTestKey.Usage);

            selenium.Open("/Users/Profile.aspx");

            string getNumRowsJs = "return window.jQuery('#ctl00_ContentPlaceHolder1_KeysControl_APIKeysListView_KeysTable tr').length";
            int numRowsOriginal = Int32.Parse(((IJavaScriptExecutor)driver).ExecuteScript(getNumRowsJs).ToString());

            driver.FindElement(By.CssSelector("tr:last-child .delete-key-request")).Click();
            driver.FindElement(By.CssSelector(".ui-dialog-buttonset > button:first-child")).Click();

            Thread.Sleep(500);
            driver.FindElement(By.CssSelector(".ui-dialog-buttonset > button:first-child")).Click();

            //Make sure it is gone from the UI
            Thread.Sleep(2000);
            int numRowsFinal = Int32.Parse(((IJavaScriptExecutor)driver).ExecuteScript(getNumRowsJs).ToString());

            try
            {
                Assert.AreEqual(numRowsOriginal - 1, numRowsFinal);
            } 
            catch
            {
                Assert.AreEqual(numRowsFinal, 0);
            }

            //Make sure it's not in the database anymore
            Assert.IsNull(mKeyMananger.GetKeyByKey(dummyInsertKey.Key));
        }
    }
}
