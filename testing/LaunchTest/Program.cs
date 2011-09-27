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
            RestAPITest rapit = new RestAPITest();
            rapit.SetupTest();
            try { rapit.TestSupportingFile(); }
            finally { rapit.TeardownTest(); }
        }
    }
}
