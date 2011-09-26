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
using System.IO;
using System.Web;
using System.ServiceModel.Web;

namespace FederatedAPI
{

    /// <summary>
    /// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "_3DR_Federation_JSON" in code, svc and config file together. 
    /// </summary>
    public class _3DR_Federation : FederatedAPI.implementation._3DR_Federation_Impl, vwar.service.host.I3DRAPI
    {
        /// <summary>
        /// A simpler url for retrieving a model 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public Stream GetModelSimple(string pid, string format, string key)
        {
            string address = GetRedirectAddressModel(implementation.APIType.REST, pid, format);
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            WebOperationContext.Current.OutgoingResponse.Location = address + "?ID=" + "00-00-00";
            return null;
        }

        /// <summary>
        /// Get the content for a model 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="format"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public Stream GetModel(string pid, string format, string options, string key)
        {
            string address = GetRedirectAddressModelAdvanced(implementation.APIType.REST, pid, format, options);
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            WebOperationContext.Current.OutgoingResponse.Location = address + "?ID=" + "00-00-00";
            return null;
        }

        /// <summary>
        /// Get the screenshot for a content object 
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public Stream GetScreenshot(string pid, string key)
        {
            string address = GetRedirectAddress("Screenshot", implementation.APIType.REST, pid);
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            WebOperationContext.Current.OutgoingResponse.Location = address + "?ID=" + "00-00-00";
            return null;
        }
        public Stream GetThumbnail(string pid, string key)
        {
            string address = GetRedirectAddress("Thumbnail", implementation.APIType.REST, pid);
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            WebOperationContext.Current.OutgoingResponse.Location = address + "?ID=" + "00-00-00";
            return null;
        }
        /// <summary>
        /// Get the original file
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public Stream GetOriginalUploadFile(string pid, string key)
        {
            string address = GetRedirectAddress("OriginalFile", implementation.APIType.REST, pid);
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            WebOperationContext.Current.OutgoingResponse.Location = address + "?ID=" + "00-00-00";
            return null;
        }
        /// <summary>
        /// Get the developer logo 
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public Stream GetDeveloperLogo(string pid, string key)
        {
            string address = GetRedirectAddress("DeveloperLogo", implementation.APIType.REST, pid);
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            WebOperationContext.Current.OutgoingResponse.Location = address + "?ID=" + "00-00-00";
            return null;
        }

        /// <summary>
        /// Get the developer logo 
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public Stream GetSponsorLogo(string pid, string key)
        {
            string address = GetRedirectAddress("SponsorLogo", implementation.APIType.REST, pid);
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            WebOperationContext.Current.OutgoingResponse.Location = address + "?ID=" + "00-00-00";
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public vwar.service.host.Metadata GetMetadata(string pid, string key)
        {
            string address = GetRedirectAddress("Metadata", implementation.APIType.REST, pid);
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            WebOperationContext.Current.OutgoingResponse.Location = address + "/json?ID=" + "00-00-00";
            return null;
        }

        /// <summary>
        /// Get all the reviews for the object. Uses query permissions 
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public List<vwar.service.host.Review> GetReviews(string pid, string key)
        {
            string address = GetRedirectAddress("Reviews", implementation.APIType.REST, pid);
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            WebOperationContext.Current.OutgoingResponse.Location = address + "/json?ID=" + "00-00-00";
            return null;
        }

        /// <summary>
        /// Get a supporting file from a content object 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public Stream GetSupportingFile(string pid, string filename, string key)
        {
            string address = GetRedirectAddress("SupportingFile", implementation.APIType.REST, pid) + "/" + filename;
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            string format = "xml";
            if(WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri.AbsoluteUri.Contains("json"))
                format="json";
            WebOperationContext.Current.OutgoingResponse.Location = address + "/" + format + "?ID=" + "00-00-00";
            return null;
        }

        /// <summary>
        /// Get a supporting file from a content object 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public Stream GetTextureFile(string pid, string filename, string key)
        {
            string address = GetRedirectAddress("Textures", implementation.APIType.REST, pid) + "/" + filename;
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            string format = "xml";
            if (WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri.AbsoluteUri.Contains("json"))
                format = "json";
            WebOperationContext.Current.OutgoingResponse.Location = address + "/" + format + "?ID=" + "00-00-00";
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="terms"></param>
        /// <returns></returns>
        public List<vwar.service.host.SearchResult> Search2(string terms, string key) { return Search(terms, key); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public List<vwar.service.host.Review> GetReviews2(string pid, string key) {

            string address = GetRedirectAddress("Reviews", implementation.APIType.REST, pid);
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            WebOperationContext.Current.OutgoingResponse.Location = address + "/xml?ID=" + "00-00-00";
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public vwar.service.host.Metadata GetMetadata2(string pid, string key) {
            string address = GetRedirectAddress("Metadata", implementation.APIType.REST, pid);
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            WebOperationContext.Current.OutgoingResponse.Location = address + "/xml?ID=" + "00-00-00";
            return null;
        }

        // <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public Stream GetMetadataJSONP(string pid, string key, string callback)
        {
            string address = GetRedirectAddress("Metadata", implementation.APIType.REST, pid);
            address = address + "/jsonp?ID=00-00-00&callback=" + callback;
            System.Net.WebClient wc = new System.Net.WebClient();
            return new MemoryStream(wc.DownloadData(address));
        }
        public Stream GetReviewsJSONP(string pid, string key, string callback)
        {
            string address = GetRedirectAddress("Reviews", implementation.APIType.REST, pid);
            address += address + "/jsonp?ID=00-00-00&callback=" + callback;
            System.Net.WebClient wc = new System.Net.WebClient();
            return new MemoryStream(wc.DownloadData(address));
        }
        public Stream SearchJSONP(string terms, string key, string callback)
        {
            List<vwar.service.host.SearchResult> md = Search(terms, key);
            MemoryStream stream1 = new MemoryStream();
            System.Runtime.Serialization.Json.DataContractJsonSerializer ser = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(List<vwar.service.host.SearchResult>));
            ser.WriteObject(stream1, md);
            stream1.Seek(0, SeekOrigin.Begin);
            System.IO.StreamReader sr = new StreamReader(stream1);
            string data = sr.ReadToEnd();
            data = callback + "(" + data + ");";

            byte[] a = System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(data);

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            return new MemoryStream(a);
        }
    }
}
