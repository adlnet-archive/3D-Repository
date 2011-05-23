using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Web;
using System.IO;
using System.Configuration;
using System.ServiceModel.Activation;

namespace _3d.service.host
{
    [AspNetCompatibilityRequirements(RequirementsMode=AspNetCompatibilityRequirementsMode.Allowed)]
    public class _3DRAPI_Json : _3DRAPI_Http_Imp, vwar.service.host.I3DRAPI_Json
    {
      

    }
}
