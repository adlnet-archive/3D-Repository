using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;

using System.ServiceModel.Web;
using System.IO;
namespace FederatedAPI
{
    [ServiceContract]
    public interface I3DRFederation : vwar.service.host.I3DRAPI
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="filename"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/{pid}/Textures/NoRedirect/{filename}?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream GetTextureFileNoRedirect(string pid, string filename, string key);


        [WebGet(UriTemplate = "/{pid}/Model/NoRedirect/{format}/{options}?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream GetModelNoRedirect(string pid, string format, string options, string key);
    }
}
