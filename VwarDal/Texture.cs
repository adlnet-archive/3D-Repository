using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vwarDAL
{
    public class Texture
    {
        public Texture(string filename, string type, int UVset)
        {
            mFilename = filename;
            mType = type;
            mUVSet = UVset;
        }
        public string mFilename { get; set; }
        public string mType { get; set; }
        public int mUVSet { get; set; }
    }
}
