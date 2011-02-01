using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _3DR_Testing;
namespace _3DR_Testing
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using NUnit.Framework;

    using Selenium;

    namespace UploadAll
    {
        [TestFixture]
        public class FF_SimpleUpload
        {
            protected ISelenium selenium;
            protected StringBuilder verificationErrors;


            private string path;
           
            [SetUp]
            virtual public void SetupTest()
            {
                selenium = new DefaultSelenium("localhost", 4444, "*chrome", _3DR_Testing.Properties.Settings.Default._3DRURL);
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
                    // Ignore errors if unable to close the browser
                }
                Assert.AreEqual("", verificationErrors.ToString());
            }





            [Test]
            public void RegularUpload(
                [Values("40ftContainer.zip",
                "AH1_Cobra.zip",
                "bellAugusta.zip",
                "blackhawk2.zip",
                "box.zip",
                "briefcase.zip",
                "Chain_linked_fence.zip",
                "Cruiseship.zip",
                "DeltaFlyer.zip",
                "duffle_bag.zip",
                "Huey.zip",
                "LCS-FreedomVar.zip",
                "M1_2.zip",
                "M939.zip",
                "paint_thinner.zip",
                "RadDetector.zip",
                "Rock.zip",
                "shelf.zip",
                "Sleeper.zip",
                "Sub_LAclass.zip",
                "toilet.zip",
                "tree.zip",
                "vp-collada-medieval-axe.zip",
                "vp-collada-medieval-shield.zip",
                "vp-collada-spacestation.zip",
                "4_drawer_file_cabinet.zip",
                "Ak47.zip",
                "bench.zip",
                "Bookshelf.zip",
                "bradley.zip",
                "Building.zip",
                "coffeehouse.zip",
                "Defender25.zip",
                "Dragunov.zip",
                "DumpTruck.zip",
                "generator.zip",
                "HMMWV.zip",
                "M249SAW.zip",
                "Mi24.zip",
                "office_desk.zip",
                "pallet_cement(2).zip",
                "planter.zip",
                "RGP7.zip",
                "rpg.zip",
                "sink.zip",
                "SU27.zip",
                "Table.zip",
                "travel_agency.zip",
                "Tug.zip",
                "vp-collada-medieval-helmet.zip",
                "vp-collada-spacestation-damaged.zip",
                "Wooden_bucket.zip",
                "mwrap.zip")] 
                string mFileToUpload)
                {
                    Upload(mFileToUpload);
                }

            

            [Test]
            public void SketchupUpload(
                [Values(
                "capilla.skp",
                "ChryslerBuilding.skp",
                "parochie1.skp",
                "Station.skp",
                "Untitled.skp",
                "US_Capitol_Building.skp",
                "UV.skp",
                "VISOKIDECANI.skp"
                )] 
                string mFileToUpload)
            {
                Upload(mFileToUpload);
            }

            [Test]
            public void _3DSUpload(
                [Values(
                "C13DS.3DS",    
                "C23DS.3DS",    
                "C33DS.3DS",    
                "S13DS.3DS",    
                "S23DS.3DS",   
                "S33DS.3DS",
                "S33DSb.3DS",   
                "S43DS.3DS"
                )] 
                string mFileToUpload)
            {
                Upload(mFileToUpload);
            }

            [Test]
            public void OBJUpload(
                [Values(
                "C1OBJ.obj",
                "C1OBJb.obj",
                "C2OBJ.obj",
                "C2OBJb.obj",
                "C3OBJ.obj",
                "C3OBJb.obj",
                "S1OBJ.obj",
                "S2OBJ.obj",
                "S3OBJ.obj",
                "S3OBJb.obj",
                "S3OBJc.obj",
                "S3OBJd.obj",
                "S4OBJ.obj"
                )] 
                string mFileToUpload)
            {
                Upload(mFileToUpload);
            }

            [Test]
            public void FBXUpload(
                [Values(
                "C1FBX.FBX",
                "C2FBX.FBX",
                "C2FBXb.FBX",
                "C3FBX.FBX",
                "S1FBX.FBX",
                "S2FBX.FBX",
                "S3FBX.FBX",
                "S4FBX.FBX"
                )] 
                string mFileToUpload)
            {
                Upload(mFileToUpload);
            }

            [Test]
            public void FurintureUpload(
                [Values("BathroomFixtures02.zip",
                    "Bidet02.zip",
                    "BlockTable02.zip",
                    "Chandleir01.zip",
                    "Chandleir03.zip",
                    "ClassicDresser02.zip",
                    "ClassicWoodCoffeeTable02.zip",
                    "CoffeeTAble02.zip",
                    "Console02.zip",
                    "Console03.zip",
                    "DeskLamp02.zip",
                    "DinnerChair with arms02.zip",
                    "DinnerChair01.zip",
                    "DinnerChair02.zip",
                    "DirectorsChair02.zip",
                    "Dresser02.zip",
                    "EasyChair02.zip",
                    "EasyChair03.zip",
                    "EasyChair04.zip",
                    "ElegantCouch02.zip",
                    "FloorLamp01.zip",
                    "FloorLamp03.zip",
                    "GreenChair02.zip",
                    "GreenCouch02.zip",
                    "Grill02.zip",
                    "Lamp01.zip",
                    "Lamp02.zip",
                    "Lamp03.zip",
                    "LargeChaise02.zip",
                    "MetalDiningChair02.zip",
                    "MetalDinnerChairArms02.zip",
                    "MetalExteriorChair02.zip",
                    "MetalPoolChair02.zip",
                    "Microwave02.zip",
                    "MissionTable01.zip",
                    "MissionTable02.zip",
                    "ModernCeilingLight02.zip",
                    "ModernConsoleTable02.zip",
                    "ModernEasyChair02.zip",
                    "ModernEndTable02.zip",
                    "ModernFloorLamp02.zip",
                    "ModernHangingLight02.zip",
                    "ModernSconce01.zip",
                    "ModernSconce02.zip",
                    "OfficeChair02.zip",
                    "OutdoorChair02.zip",
                    "OvenFront02.zip",
                    "PlushCouch02.zip",
                    "RectangularAutoman02.zip",
                    "RollingChaise02.zip",
                    "RoundAutoman02.zip",
                    "RoundEndTable01.zip",
                    "RoundEndTable02.zip",
                    "Sconce02.zip",
                    "ServingTrayTable02.zip",
                    "SquareLamp02.zip",
                    "Stool02.zip",
                    "TableLamp02.zip",
                    "Tent 02.zip",
                    "Tent02.zip",
                    "Toilet02.zip",
                    "Umbrella 01.zip",
                    "Umbrella01.zip",
                    "Umbrella03.zip",
                    "WickerAutoman02.zip",
                    "WickerChair02.zip",
                    "WickerCouch02.zip",
                    "WickerOutdoorLoveseat02.zip",
                    "WingBackChair02.zip",
                    "WoodAndMetalCoffeeTable02.zip",
                    "WoodAndMetalDinnerTable01.zip",
                    "WoodAndMetalDinnerTable02.zip",
                    "WoodenDresser02.zip",
                    "WoodEndTable02.zip")] 
                string mFileToUpload)
            {

                path = _3DR_Testing.Properties.Settings.Default.ContentPath;
                if (mFileToUpload == "")
                    return;
                selenium.Open("/Default.aspx");

                if (selenium.IsTextPresent("Sign In"))
                {
                    selenium.Click("ctl00_LoginStatus1");
                    selenium.WaitForPageToLoad("30000");
                    selenium.Type("ctl00_ContentPlaceHolder1_Login1_Login1_UserName", _3DR_Testing.Properties.Settings.Default._3DRUserName);
                    selenium.Type("ctl00_ContentPlaceHolder1_Login1_Login1_Password", _3DR_Testing.Properties.Settings.Default._3DRPassword);
                    selenium.Click("ctl00_ContentPlaceHolder1_Login1_Login1_LoginButton");
                }
                selenium.WaitForPageToLoad("30000");
                selenium.Open("/Users/Upload.aspx");
                selenium.WaitForPageToLoad("30000");
                
                selenium.Type("ctl00_ContentPlaceHolder1_Upload1_TitleTextBox", System.IO.Path.GetFileNameWithoutExtension(mFileToUpload));




                

                selenium.Click("ctl00_ContentPlaceHolder1_Upload1_ExpandCollapseImage");
                selenium.Type("ctl00_ContentPlaceHolder1_Upload1_ArtistNameTextBox", "Rob Chadwick");
                selenium.Type("ctl00_ContentPlaceHolder1_Upload1_SponsorNameTextBox", "ADL");
                selenium.Type("ctl00_ContentPlaceHolder1_Upload1_FormatTextBox", "Max, Collada");
                selenium.Click("ctl00_ContentPlaceHolder1_Upload1_KeywordsTextBox_Input");
                selenium.Type("ctl00_ContentPlaceHolder1_Upload1_KeywordsTextBox_Input", "Furniture");
                selenium.Click("ctl00_ContentPlaceHolder1_Upload1_AddKeywordButton");
                selenium.Type("ctl00_ContentPlaceHolder1_Upload1_SponsorLogoFileUpload", "C:\\Documents and Settings\\chadwickr\\My Documents\\Downloads\\logo.gif");
                selenium.Click("ctl00_ContentPlaceHolder1_Upload1_CCLicenseDropDownList_Arrow");
                selenium.Click("//div[@id='ctl00_ContentPlaceHolder1_Upload1_CCLicenseDropDownList_DropDown']/div/ul/li[5]");
                selenium.Type("ctl00_ContentPlaceHolder1_Upload1_CCLicenseDropDownList_Input", "Attribution Share Alike (by-sa)");

                SetUploadFile(path + mFileToUpload);
                selenium.Click("ctl00_ContentPlaceHolder1_Upload1_Step1NextButton");
                selenium.WaitForPageToLoad("300000");
                selenium.WaitForCondition("window.GetLoadingComplete != undefined", "20000");
                int count = 0;
                while ((selenium.GetEval("window.GetLoadingComplete() == true") != "true" && count < 30))
                {
                    count++;
                    System.Threading.Thread.Sleep(1000);

                }
                System.Threading.Thread.Sleep(5000);
                if (selenium.GetEval("window.GetLoadingComplete() == true") == "true")
                {
                    System.Threading.Thread.Sleep(300);
                    selenium.GetEval("window.updateCamera();");
                    System.Threading.Thread.Sleep(300);
                    selenium.GetEval("window.TakeScreenShot();");
                    System.Threading.Thread.Sleep(3000);
                    //count = 0;
                    //string Thumbstring = selenium.GetEval("document.getElementById('ctl00_ContentPlaceHolder1_Upload1_ThumbnailImage')");
                    //while (Thumbstring.IndexOf("ScreenShot") == -1 && count < 10)
                    //{
                    //    count++;
                    //    System.Threading.Thread.Sleep(1000);
                    //    Thumbstring = selenium.GetEval("document.getElementById('ctl00_ContentPlaceHolder1_Upload1_ThumbnailImage')");
                    //}
                }



                //  ctl00_ContentPlaceHolder1_Upload1_ThumbnailImage
                selenium.Click("ctl00_ContentPlaceHolder1_Upload1_ValidationViewSubmitButton");
                selenium.WaitForPageToLoad("30000");






            }

            public virtual void SetUploadFile(string infile)
            {
                selenium.Type("ctl00_ContentPlaceHolder1_Upload1_ContentFileUpload", infile);
            }

            public void Upload(string mFileToUpload)
            {
                path = _3DR_Testing.Properties.Settings.Default.ContentPath;
                if (mFileToUpload == "")
                    return;
                selenium.Open("/Default.aspx");

                if (selenium.IsTextPresent("Sign In"))
                {
                    selenium.Click("ctl00_LoginStatus1");
                    selenium.WaitForPageToLoad("30000");
                    selenium.Type("ctl00_ContentPlaceHolder1_Login1_Login1_UserName", _3DR_Testing.Properties.Settings.Default._3DRUserName);
                    selenium.Type("ctl00_ContentPlaceHolder1_Login1_Login1_Password", _3DR_Testing.Properties.Settings.Default._3DRPassword);
                    selenium.Click("ctl00_ContentPlaceHolder1_Login1_Login1_LoginButton");
                }
                selenium.WaitForPageToLoad("30000");
                selenium.Open("/Users/Upload.aspx");
                selenium.WaitForPageToLoad("30000");
                selenium.Type("ctl00_ContentPlaceHolder1_Upload1_TitleTextBox", "Automatic Test of " + mFileToUpload + " " + System.DateTime.Now.ToString());


               

                SetUploadFile(path + mFileToUpload);

               
                
                selenium.Click("ctl00_ContentPlaceHolder1_Upload1_Step1NextButton");
                selenium.WaitForPageToLoad("300000");
                selenium.WaitForCondition("window.GetLoadingComplete != undefined", "20000");
                int count = 0;
                while ((selenium.GetEval("window.GetLoadingComplete() == true") != "true" && count < 30))
                {
                    count++;
                    System.Threading.Thread.Sleep(1000);

                }

                if (selenium.GetEval("window.GetLoadingComplete() == true") == "true")
                {
                    System.Threading.Thread.Sleep(300);
                    selenium.GetEval("window.updateCamera();");
                    System.Threading.Thread.Sleep(300);
                    selenium.GetEval("window.TakeScreenShot();");
                    System.Threading.Thread.Sleep(3000);
                    //count = 0;
                    //string Thumbstring = selenium.GetEval("document.getElementById('ctl00_ContentPlaceHolder1_Upload1_ThumbnailImage')");
                    //while (Thumbstring.IndexOf("ScreenShot") == -1 && count < 10)
                    //{
                    //    count++;
                    //    System.Threading.Thread.Sleep(1000);
                    //    Thumbstring = selenium.GetEval("document.getElementById('ctl00_ContentPlaceHolder1_Upload1_ThumbnailImage')");
                    //}
                }



                //  ctl00_ContentPlaceHolder1_Upload1_ThumbnailImage
                selenium.Click("ctl00_ContentPlaceHolder1_Upload1_ValidationViewSubmitButton");
                selenium.WaitForPageToLoad("30000");

            }
        }
    //    class IE_SimpleUpload : FF_SimpleUpload
    //    {
    //        [SetUp]
    //         public override void SetupTest()
    //        {
    //            selenium = new DefaultSelenium("localhost", 4444, "*iehta", Properties.Settings.Default._3DRURL);
    //            selenium.Start();
    //            verificationErrors = new StringBuilder();
    //        }
    //        public override void SetUploadFile(string infile)
    //        {
    //            selenium.GetEval("window.document.getElementById('ctl00_ContentPlaceHolder1_Upload1_ContentFileUpload').focus();");
    //            for (int i = 0; i < infile.Length; i++)
    //            {
    //                int key = System.Convert.ToInt32(infile.ToUpper()[i]);
    //                if ((key >= 65 && key <= 90) || (key >= 48 && key <= 57))
    //                    selenium.KeyPressNative(key.ToString());
    //                else
    //                    if (infile[i] == ':')
    //                    {
    //                        selenium.KeyDownNative("16");
    //                        selenium.KeyPressNative("59");
    //                        selenium.KeyUpNative("16");
    //                    }
    //                    if (infile[i] == '\\')
    //                        selenium.KeyPressNative("92");
    //                    if (infile[i] == '\'')
    //                        selenium.KeyPressNative("222");
    //                    if (infile[i] == '`')
    //                        selenium.KeyPressNative("131");
                        
    //                    if (infile[i] == '(')
    //                    {
    //                        selenium.KeyDownNative("16");
    //                        selenium.KeyPressNative("48");
    //                        selenium.KeyUpNative("16");
    //                    }
    //                    if (infile[i] == ')')
    //                    {
    //                        selenium.KeyDownNative("16");
    //                        selenium.KeyPressNative("57");
    //                        selenium.KeyUpNative("16");
    //                    }
    //                    if (infile[i] == '.')
    //                        selenium.KeyPressNative("46");
    //                    if (infile[i] == ' ')
    //                        selenium.KeyPressNative("32");
    //                    if (infile[i] == '_')
    //                    {
    //                        selenium.KeyDownNative("16");
    //                        selenium.KeyPressNative("45");
    //                        selenium.KeyUpNative("16");
    //                    }

                       
    //            }
                
    //        }
    //        [TearDown]
    //        new public void TeardownTest()
    //        { 
    //           // try{ selenium.Stop();}
    //           // catch (Exception){}
    //           // Assert.AreEqual("", verificationErrors.ToString());
    //        }
    //    }
    //    //class CH_SimpleUpload : FF_SimpleUpload
    //    //{
    //    //    [SetUp]
    //    //    public override void SetupTest()
    //    //    {
    //    //        selenium = new DefaultSelenium("localhost", 4444, "*chrome", Properties.Settings.Default._3DRURL);
    //    //        selenium.Start();
    //    //        verificationErrors = new StringBuilder();
    //    //    }
    //    //}
    }
}


