using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vwar.uploader
{
    public class TempMetadata
    {
        public String Title { get; set; }
        public String Description{ get; set; }
        public String ModelLocation { get; set; }
        public String ScreenshotLocation { get; set; }
        public String License { get; set; }
        public override string ToString()
        {
           return Title;
        }
    }
}
