using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Web;
using System.IO;
using System.Configuration;

namespace vwar.service.host
{
    public class _3DRAPI : _3DRAPI_Http_Imp, I3DRAPI
    {
        public List<SearchResult> Search2(string terms) {return Search(terms); }
        public List<Review> GetReviews2(string pid){return GetReviews(pid);}
        public Metadata GetMetadata2(string pid){return GetMetadata(pid);}
    }
}
