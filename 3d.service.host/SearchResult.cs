using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;

namespace _3d.service.host
{
    [DataContractFormat]
    public class SearchResult
    {
        public string PID;
        public string Title;
    }
}