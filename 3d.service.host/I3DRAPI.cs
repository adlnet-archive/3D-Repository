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
using System.Xml;

namespace vwar.service.host
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
    [System.Web.Script.Services.ScriptService] 
    public interface I3DRAPI
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/Search/{terms}/json?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<SearchResult> Search(string terms, string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/Search/{terms}/jsonp?ID={key}&callback={callback}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream SearchJSONP(string terms, string key, string callback);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/Search/{terms}/xml?ID={key}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        List<SearchResult> Search2(string terms, string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="format"></param>
        /// <param name="options"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/{pid}/Model/{format}/{options}?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream GetModel(string pid, string format, string options, string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="format"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/{pid}/Format/{format}?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream GetModelSimple(string pid, string format, string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/{pid}/Reviews/json?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<Review> GetReviews(string pid, string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/{pid}/Reviews/jsonp?ID={key}&callback={callback}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream GetReviewsJSONP(string pid, string key, string callback);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/{pid}/Reviews/xml?ID={key}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        List<Review> GetReviews2(string pid, string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/{pid}/Screenshot?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream GetScreenshot(string pid, string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/{pid}/Thumbnail?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream GetThumbnail(string pid, string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/{pid}/DeveloperLogo?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream GetDeveloperLogo(string pid, string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/{pid}/SponsorLogo?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream GetSponsorLogo(string pid, string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/{pid}/Metadata/json?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Metadata GetMetadata(string pid, string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        
        [WebGet(UriTemplate = "/{pid}/Metadata/jsonp?ID={key}&callback={callback}", BodyStyle= WebMessageBodyStyle.Bare ,ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream GetMetadataJSONP(string pid, string key, string callback);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/{pid}/Metadata/xml?ID={key}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Metadata GetMetadata2(string pid, string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="filename"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/{pid}/SupportingFiles/{filename}?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream GetSupportingFile(string pid, string filename, string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="filename"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/{pid}/Textures/{filename}?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream GetTextureFile(string pid, string filename, string key);

        [WebGet(UriTemplate = "/{pid}/OriginalUpload?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream GetOriginalUploadFile(string pid, string key);
    }
}
