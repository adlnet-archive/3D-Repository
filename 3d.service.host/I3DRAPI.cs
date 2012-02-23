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
        /// Modify the permissions for a group on a model
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        ServiceDescription Describe();

        /// <summary>
        /// Modify the permissions for a group on a model
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "/{pid}/Permissions/Groups/{groupname}?ID={key}&Level={level}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string SetGroupPermission(string pid, string groupname, string level, string key);
        /// <summary>
        /// Modify the permissions for a group on a model
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/{pid}/Permissions/Groups/{groupname}/xml?ID={key}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        string GetGroupPermissionXML(string pid, string groupname, string key);
        /// <summary>
        /// Modify the permissions for a group on a model
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/{pid}/Permissions/Groups/{groupname}/json?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string GetGroupPermissionJSON(string pid, string groupname, string key);

        /// <summary>
        /// Modify the permissions for a group on a model
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "/{pid}/Permissions/Users/{username}?ID={key}&Level={level}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string SetUserPermission(string pid, string username, string level, string key);
        /// <summary>
        /// Modify the permissions for a group on a model
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/{pid}/Permissions/Users/{username}/json?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string GetUserPermissionJSON(string pid, string username, string key);
        /// <summary>
        /// Modify the permissions for a group on a model
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/{pid}/Permissions/Users/{username}/xml?ID={key}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        string GetUserPermissionXML(string pid, string username, string key);

        /// <summary>
        /// Set the screenshot - also creates thumbnail
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "/{pid}/Screenshot?ID={key}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        string UploadScreenShot(Stream md, string pid, string key);
        /// <summary>
        /// Resolve a missing texture reference
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "/{pid}/Textures?ID={key}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        string UploadMissingTexture(Stream md, string pid, string key);
        /// <summary>
        /// upload a supporting File
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "/{pid}/SupportingFiles?ID={key}&Description={description}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        string UploadSupportingFile(Stream md, string pid,string description, string key);
        /// <summary>
        /// upload the sponsors logo
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "/{pid}/DeveloperLogo?ID={key}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        string UploadDeveloperLogo(Stream md, string pid, string key);
        /// <summary>
        /// upload the sponsors logo
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "/{pid}/SponsorLogo?ID={key}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        string UploadSponsorLogo(Stream md, string pid, string key);
        /// <summary>
        /// add a review to the model
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "/{pid}/Reviews/xml?ID={key}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        string AddReviewXML(Stream md, string pid, string key);
        /// <summary>
        /// add a review to the model
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "/{pid}/Reviews/json?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string AddReviewJSON(Stream md, string pid, string key);
        /// <summary>
        /// overwrite metadata for a model
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "/{pid}/Metadata/xml?ID={key}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        string UpdateMetadataXML(Stream md, string pid, string key);
        /// <summary>
        /// overwrite metadata for a model
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "/{pid}/Metadata/json?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string UpdateMetadataJSON(Stream md, string pid, string key);
        /// <summary>
        /// overwrite a model
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "/{pid}?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string UploadFile(Stream indata, string pid, string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "/UploadModel?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string UploadFileStub(Stream indata, string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "/{pid}/OriginalUpload?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string UploadFileStub2(Stream indata, string pid, string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/Search/{terms}/json?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<SearchResult> SearchJSON(string terms, string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/AdvancedSearch/{searchmode}/{searchstring}/json?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<SearchResult> AdvancedSearchJSON(string searchmode,string searchstring, string key);
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
        [WebGet(UriTemplate = "/AdvancedSearch/{searchmode}/{searchstring}/jsonp?ID={key}&callback={callback}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Stream AdvancedSearchJSONP(string searchmode, string searchstring, string key, string callback);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/Search/{terms}/xml?ID={key}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        List<SearchResult> SearchXML(string terms, string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/AdvancedSearch/{searchmode}/{searchstring}/xml?ID={key}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        List<SearchResult> AdvancedSearchXML(string searchmode, string searchstring, string key);
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
        List<Review> GetReviewsJSON(string pid, string key);

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
        List<Review> GetReviewsXML(string pid, string key);
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
        Metadata GetMetadataJSON(string pid, string key);

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
        Metadata GetMetadataXML(string pid, string key);
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
