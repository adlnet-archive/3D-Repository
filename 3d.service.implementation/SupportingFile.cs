using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;

namespace _3d.service.host
{
    [DataContractFormat]
    public class SupportingFile
    {
        public string Filename;
        public string Description;
    }
}