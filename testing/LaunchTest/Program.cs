using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _3DR_Testing;

namespace LaunchTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //_3DR_Testing.GeneralUploadTest gut = new GeneralUploadTest();
            //gut.SetupTest();
            //gut.RegularUpload("40ftContainer.zip");
            //gut.TeardownTest();

            //gut.SetupTest();
            //gut.RequireResubmitUpload("40ftContainer.zip");
            //gut.TeardownTest();

            //gut.SetupTest();
            //gut.SketchupUpload("capilla.skp");
            //gut.TeardownTest();

            _3DR_Testing.EditTest edit = new EditTest();
            edit.SetupTest();
            edit.TestChangeAllFields();
            edit.TeardownTest();

            _3DR_Testing.DamagedFilesUploadTest dam = new DamagedFilesUploadTest();
            dam.SetupTest();
            dam.Damaged3DS();
            dam.TeardownTest();

            dam.SetupTest();
            dam.DamagedDAE();
            dam.TeardownTest();

            dam.SetupTest();
            dam.DamagedFBX();
            dam.TeardownTest();

            dam.SetupTest();
            dam.DamagedOBJ();
            dam.TeardownTest();

            dam.SetupTest();
            dam.DamagedSKP();
            dam.TeardownTest();

            dam.SetupTest();
            dam.DamagedZip();
            dam.TeardownTest();

            dam.SetupTest();
            dam.ZeroByte3DS();
            dam.TeardownTest();

            dam.SetupTest();
            dam.ZeroByteDAE();
            dam.TeardownTest();

            dam.SetupTest();
            dam.ZeroByteFBX();
            dam.TeardownTest();

            dam.SetupTest();
            dam.ZeroByteOBJ();
            dam.TeardownTest();

            dam.SetupTest();
            dam.ZeroByteSKP();
            dam.TeardownTest();


            _3DR_Testing.MyKeysTest mkt = new MyKeysTest();
            mkt.SetupTest();
            mkt.TestAll();
            mkt.TeardownTest();

        }
    }
}
