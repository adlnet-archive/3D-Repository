using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;

namespace _3d.service.host
{
    [DataContractFormat]
    public class Texture
    {

        public string mFilename;
        public string mType;
        public int mUVSet;
    }
}