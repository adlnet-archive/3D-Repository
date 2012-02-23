using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;

namespace vwar.service.host
{
    [DataContractFormat]
    public class Review
    {
        public string Submitter {get;set;}
        public int Rating { get; set; }
        public string ReviewText { get; set; }
        public string DateTime { get; set; }
        public string PIDLink;
    }
}