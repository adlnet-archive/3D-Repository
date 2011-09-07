using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace _3DR_Testing
{
    public class GeneralUploadTest : UploadTest
    {
        public GeneralUploadTest() : base() { }

        [Test]
        public void RegularUpload()
        {
            TestUpload("SU27.zip");
        }


        [Test]
        public void RequireResubmitUpload()
        {
            this.DoResubmitCheckTest = true;
            TestUpload("SU27.zip");
        }

        [Test]
        public void SketchupUpload([Values("SU27.zip")] string mFileToUpload)
        {
            //TestUpload(mFileToUpload);
        }

        [Test]
        public void LargeUpload()
        {
            //TestUpload("mwrap.zip");
        }


    }
}
