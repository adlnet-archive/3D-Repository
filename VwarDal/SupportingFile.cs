using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vwarDAL
{
    public class SupportingFile
    {
        public SupportingFile(string filename, string description)
        {
            Filename = filename;
            Description = Description;

        }
        public string Filename { get; set; }
        public string Description { get; set; }

    }
}
