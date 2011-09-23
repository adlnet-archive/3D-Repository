//  Copyright 2011 U.S. Department of Defense

//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at

//      http://www.apache.org/licenses/LICENSE-2.0

//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.



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
    //    OrganizationPOC: "Admin",
    //    OrganizationPOCEmail: "Admin@somecompany.com",
    //    OrganizationPOCPassword: "none",
    //    OrganizationURL: http://www.someorg.com,
    //    OrginizationName: "Some Company",
    //    SOAPAPI: http://localhost/3DRAPI/_3DRAPI_Soap.svc,
    //    XMLAPI: http://localhost/3DRAPI/_3DRAPI_Xml.svc,
    //    namespacePrefix: "adl"

    //}

    /// <summary>
    /// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "I3DR_Federation_Mgnt" in both code and config file together. 
    /// </summary>
    [ServiceContract]
    public interface I3DR_Federation_Mgmt
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/RequestFederation")]
        //[WebGet(ResponseFormat=WebMessageFormat.Json, UriTemplate = "/RequestFederation")]        
        [OperationContract]
        FederatedAPI.implementation.RequestFederationResponse RequestFederation(FederatedAPI.implementation.FederateRecord request);

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/ModifyFederate/{state}")]
        //[WebGet(ResponseFormat=WebMessageFormat.Json, UriTemplate = "/RequestFederation")]        
        [OperationContract]
        FederatedAPI.implementation.ModifyFederationResponse ModifyFederate(FederatedAPI.implementation.ModifyFederationRequest request, string state);

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetAllFederates")]
        //[WebGet(ResponseFormat=WebMessageFormat.Json, UriTemplate = "/RequestFederation")]        
        [OperationContract]
        FederatedAPI.implementation.GetAllFederatesResponse GetAllFederates();

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/ApproveFederate")]
        //[WebGet(ResponseFormat=WebMessageFormat.Json, UriTemplate = "/RequestFederation")]        
        [OperationContract]
        FederatedAPI.implementation.ApproveFederateResponse ApproveFederate(FederatedAPI.implementation.ApproveFederateRequest request);
    }
}
