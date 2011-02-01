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
    public class DamagedFilesUploadTest : UploadTest
    {
        [Test]
        public void DamagedZip()
        {
            TestUpload("DamagedZip.zip");
        }
        [Test]
        public void ZeroByteZip()
        {
         /*   if(!UserLoggedIn)
            {
                Login();
            }
            NavigateToUploadPage();

            string locator = String.Format("//div[@id='{0}']/input[@type='file']", UploadButtonIdentifier.MODEL_UPLOAD);
            selenium.Type(locator, path + "ZeroByteZip.zip");
            //selenium.WaitForPopUp("The page at http://localhost:1996 says:", "3000");*/
            TestUpload("ZeroByteZip");
        }
        [Test]
        public void DamagedDAE()
        {
            TestUpload("DamagedDAE.zip");
        }
        [Test]
        public void ZeroByteDAE()
        {
            TestUpload("ZeroByteDAE.zip");
        }
        [Test]
        public void DamagedFBX()
        {
            TestUpload("DamagedFBX.zip");
        }
        [Test]
        public void ZeroByteFBX()
        {
            TestUpload("ZeroByteFBX.zip");
        }
        [Test]
        public void Damaged3DS()
        {
            TestUpload("Damaged3DS.zip");
        }
        [Test]
        public void ZeroByte3DS()
        {
            TestUpload("ZeroByte3DS.zip");
        }
        [Test]
        public void DamagedOBJ()
        {
            TestUpload("DamagedOBJ.zip");
        }
        [Test]
        public void ZeroByteOBJ()
        {
            TestUpload("ZeroByteOBJ.zip");
        }
        [Test]
        public void DamagedSKP()
        {
            TestUpload("DamagedSKP.zip");
        }
        [Test]
        public void ZeroByteSKP()
        {
            TestUpload("ZeroByteSKP.zip");
        }


    }
}
