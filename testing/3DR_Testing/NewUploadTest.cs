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

    namespace NewUploadAll
    {
        [TestFixture]
        public class NewUploadTest
        {
            protected ISelenium selenium;
            protected StringBuilder verificationErrors;
            private string path;
            private HttpCommandProcessor proc;

            /// <summary>
            /// Javascript for the extension to simulate swfUpload
            /// </summary>
            #region doSWFUploadExtension
            private string doSWFUploadExtension = @"
// Custom method for uploading a file, simulating SWFUpload
Selenium.prototype.doSwfUpload = function(selector, filename) {
 
	// Grab the SWFUpload element from the DOM
	var doc = this.browserbot.getCurrentWindow().document;
	var swf_uploader = Element.select($(doc.body), selector)[0];
 
	// Slurp the list of params from SWFUload, and parse them into a Hash
	var flashvars_s = swf_uploader.down('param[name=flashvars]').value;
	var flashvars = {};
	$A(flashvars_s.split(/&/)).each(function(kv){
		var key = unescape(kv.split(/=/)[0]);
		var value = unescape(kv.split(/=/)[1]);
		flashvars[key] = value;
	});
	var params_array = $A(decodeURI(flashvars.params).split(/&amp;/));
	var params = new Hash({});
	params_array.each(function(kv){
		var key = decodeURI(kv.split(/=/)[0]);
		var value = decodeURI(kv.split(/=/)[1]);
		params.set(key, value);
	});
	params.unset('format'); // Remove the format param, since we don't want to request as json
 
	// Grab the SWFUpload form from the hidden IFrame,
	// and insert the key/value params into the form
	var faker_form = Element.select($$('#selenium_fileupload_iframe')[0].contentDocument.body, '#swfupload_faker_form')[0];
	params.each(function(kv) {
		Element.insert(faker_form, { bottom: '<input type=\'hidden\' name=\''+kv.key+'\' value=\''+kv.value+'\' />' });
	});
 
	// Assign the selected file to the file field
	netscape.security.PrivilegeManager.enablePrivilege('UniversalFileRead');
	this.browserbot.replaceText(faker_form.down('#swfupload_faker_file'), filename);
 
	// Assign the URL and submit the Form
	faker_form.action = flashvars.uploadURL;
	faker_form.submit();
 
	// Clean up the params
	Element.select(faker_form, 'input[type=hidden]').each(function(e){
		e.remove();
	});
 
	// Retarget the IFrame back to the fileupload frame
	$$('#selenium_fileupload_iframe')[0].contentWindow.location = 'TestRunner-fileupload.html';
};";
#endregion


            [SetUp]
            virtual public void SetupTest()
            {
                proc = new HttpCommandProcessor("localhost", 1234, "*chrome", _3DR_Testing.Properties.Settings.Default._3DRURL);
                selenium = new DefaultSelenium(proc);
          //      selenium.SetExtensionJs(doSWFUploadExtension);
                    
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

                }
                Assert.AreEqual(String.Empty, verificationErrors.ToString());
            }


            [Test]
            public void TestUpload([Values("SU27.zip")] string filename)
            {
                selenium.Open("/Default.aspx");

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

                string[] args = { "#SWFUpload_0", path+filename };
                proc.DoCommand("doSwfUpload", args);
                
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
        }
    }
}
