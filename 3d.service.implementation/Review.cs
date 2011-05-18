using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;

namespace vwar.service.implementation
{
    [DataContractFormat]
    public class Review
    {
        public string Submitter;
        public int Rating;
        public string ReviewText;
        public string DateTime;
    }
}