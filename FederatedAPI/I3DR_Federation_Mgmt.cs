using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;

using System.ServiceModel.Web;
namespace FederatedAPI
{


//    {

//    ActivationState: 0,
//    AllowFederatedDownload: true,
//    AllowFederatedSearch: true,
//    JSONAPI: http://localhost/3DRAPI/_3DRAPI_Json.svc,
//    OrganizationPOC: "Rob Chadwick",
//    OrganizationPOCEmail: "Rob.Chadwick.Ctr@adlnet.gov",
//    OrganizationPOCPassword: "none",
//    OrganizationURL: http://www.adl.gov,
//    OrginizationName: "ADL",
//    SOAPAPI: http://localhost/3DRAPI/_3DRAPI_Soap.svc,
//    XMLAPI: http://localhost/3DRAPI/_3DRAPI_Xml.svc,
//    namespacePrefix: "adl"

//}


    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "I3DR_Federation_Mgnt" in both code and config file together.
    [ServiceContract]
    public interface I3DR_Federation_Mgmt
    {
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/RequestFederation")]
        //[WebGet(ResponseFormat=WebMessageFormat.Json, UriTemplate = "/RequestFederation")]
        [OperationContract]
        FederatedAPI.implementation.RequestFederationResponse RequestFederation(FederatedAPI.implementation.FederateRecord request);
    }
}
