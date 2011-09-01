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
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;

namespace vwar.service.host
{
    [ServiceContract]
    public interface I3DRAPI_Http
    {
        [WebGet(UriTemplate = "/Search/{terms}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        List<SearchResult> Search(string terms);

        [WebGet(UriTemplate = "/Model/{pid}/{format}/{options}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Stream GetModel(string pid, string format, string options);

        [WebGet(UriTemplate = "/{pid}/{format}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Stream GetModelSimple(string pid, string format);

        [WebGet(UriTemplate = "/Reviews/{pid}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        List<Review> GetReviews(string pid);

        [WebGet(UriTemplate = "/Screenshot/{pid}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Stream GetScreenshot(string pid);

        [WebGet(UriTemplate = "/DeveloperLogo/{pid}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Stream GetDeveloperLogo(string pid);

        [WebGet(UriTemplate = "/SponsorLogo/{pid}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Stream GetSponsorLogo(string pid);

        [WebGet(UriTemplate = "/Metadata/{pid}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Metadata GetMetadata(string pid);

    }
}
