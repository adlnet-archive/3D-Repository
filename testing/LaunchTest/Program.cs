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
            _3DR_Testing.EditTest etest = new EditTest();
            etest.SetupTest();
            etest.TestChangeAllFields();
            etest.TeardownTest();
        }
    }
}
