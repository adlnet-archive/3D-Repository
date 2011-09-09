using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using vwar.service.host;
using System.Threading;

namespace _3DR_Testing
{
    [TestFixture]
    public class MyKeysTest : SeleniumTest
    {

        private bool isTestAll = false;
        private APIKey mTestKey;
        private APIKeyManager mKeyMananger;

        [SetUp]
        public override void SetupTest()
        {
            base.SetupTest();
            Login(false);
            mTestKey = new APIKey();
            mTestKey.Email = Properties.Settings.Default._3DR_UserName;
            mTestKey.Usage = "This is a test for the key request";
            mKeyMananger = new APIKeyManager();
        }

        [Test]
        public void TestAdd()
        {
            if (!isTestAll)
            {
                selenium.Open("/Users/Profile.aspx#");
            }
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
            if (!isTestAll)
            {
                selenium.Open("/Users/Profile.aspx");
            }

            string keyToCheck = selenium.GetEval("window.jQuery('#ctl00_ContentPlaceHolder1_KeysControl_APIKeysListView_KeysTable tr:last').find('.key').text().trim()");

            if (String.IsNullOrEmpty(keyToCheck))
            {
                throw new InconclusiveException("There were no keys to edit.");
            }

            mTestKey.Usage = "This has been edited, part of key UI test";
            mTestKey.Key = keyToCheck;
            mTestKey.State = APIKeyState.ACTIVE;

            selenium.GetEval("window.jQuery('#ctl00_ContentPlaceHolder1_KeysControl_APIKeysListView_KeysTable tr:last').find('.update-key-request').click()");
            Thread.Sleep(500);
            selenium.Type("#UsageTextArea", "This has been edited, part of key UI test");
            driver.FindElement(By.Id("KeyRequestSubmit")).Click();
            Thread.Sleep(2000);
            selenium.Click("//input[@value='Ok']");

            Assert.True(mTestKey.Equals(mKeyMananger.GetKeyByKey(keyToCheck)));
        }

        [Test]
        public void TestDelete()
        {
            if (!isTestAll)
            {
                selenium.Open("/Users/Profile.aspx#");
            }

            
            string keyToDelete = selenium.GetEval("window.jQuery('#ctl00_ContentPlaceHolder1_KeysControl_APIKeysListView_KeysTable tr:last').find('.key').text().trim()");

            if (String.IsNullOrEmpty(keyToDelete))
            {
                throw new InconclusiveException("There were no keys to delete.");
            }

            selenium.GetEval("window.jQuery('#ctl00_ContentPlaceHolder1_KeysControl_APIKeysListView_KeysTable tr:last').find('.delete-key-request').click()");
            selenium.Click("//button[@type='button']");
            Thread.Sleep(500);
            selenium.GetEval("window.jQuery('.ui-button .ui-widget .ui-state-default .ui-corner-all .ui-button-text-only').click()");


            
            //Make sure it is gone from the UI
            Thread.Sleep(2000);
            Assert.AreEqual(System.Convert.ToInt32(
                                selenium.GetEval("window.jQuery('#ctl00_ContentPlaceHolder1_KeysControl_APIKeysListView_KeysTable tr:last').find('.key').length")
                            ), 0);


            //Make sure it's not in the database anymore
            Assert.IsNull(mKeyMananger.GetKeyByKey(keyToDelete));
        }


        [Test]
        public void TestAll()
        {
            isTestAll = true;
            selenium.Open("/Users/Profile.aspx");
            TestAdd();
            TestEdit();
            TestDelete();
        }

    }
}
