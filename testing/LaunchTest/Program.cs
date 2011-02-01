using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _3DR_Testing;
//using _3DR_Testing.UploadAll;
namespace LaunchTest
{
    class Program
    {
        static void Main(string[] args)
        {

            //_3DR_Testing.FedoraControl.ClearDatabase();
            //SimpleUpload up = new SimpleUpload();
            //up.SetupTest();
            //up.Upload("AH1_Cobra.zip");
            //up.TeardownTest();
            _3DR_Testing.DamagedFilesUploadTest test = new DamagedFilesUploadTest();
            test.SetupTest();
            test.ZeroByteZip();

        }
    }
}
