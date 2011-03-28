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
            TestUpload(mFileToUpload);
        }


        [Test]
        public void RequireResubmitUpload([Values("box.zip")] string mFileToUpload)
        {
            this.DoResubmitCheckTest = true;
            TestUpload(mFileToUpload);
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
            TestUpload(mFileToUpload);
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
            TestUpload(mFileToUpload);
        }
    }
}
