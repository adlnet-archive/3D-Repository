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
    public class _3DR_Federation : FederatedAPI.implementation._3DR_Federation_Impl, FederatedAPI.I3DRFederation
    {

        

        public vwar.service.host.ServiceDescription Describe()
        {
            if (HandleHttpOptionsRequest()) return null;

            vwar.service.host.ServiceDescription des = new vwar.service.host.ServiceDescription();
            des.Upload = "Upload is disabled for the federation. Upload to a specific server";
            des.Namespace = "The Federation accepts requests for all registered namespaces.";
            des.OrganizationName = "ADL";
            des.OrganizationPOC = "Rob Chadwick";
            des.OrganizationPOCEmail = "robert.chadwick.ctr@adlnet.gov";
            des.OrganizationURL = "www.adlnet.gov";
            des.SearchJSON = "http://3dr.adlnet.gov/Federation/3dr_Federation.svc/term/json?ID=00-00-00";
            des.SearchJSONP = "http://3dr.adlnet.gov/Federation/3dr_Federation.svc/term/jsonp?ID=00-00-00&callback=callback";
            des.SearchXML = "http://3dr.adlnet.gov/Federation/3dr_Federation.svc/term/xml?ID=00-00-00";
            des.AccessJSON = "http://3dr.adlnet.gov/Federation/3dr_Federation.svc/adl:512/metadata/json?ID=00-00-00";
            des.AccessXML = "http://3dr.adlnet.gov/Federation/3dr_Federation.svc/adl:512/metadata/xml?ID=00-00-00";
            des.AccessJSONP = "http://3dr.adlnet.gov/Federation/3dr_Federation.svc/adl:512/metadata/jsonp?ID=00-00-00&callback=callback";
            WebOperationContext.Current.OutgoingResponse.Headers["Access-Control-Allow-Origin"] = WebOperationContext.Current.IncomingRequest.Headers["Origin"];
            return des;
        }
        /// <summary>
        /// A simpler url for retrieving a model 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public Stream GetModelSimple(string pid, string format, string key)
        {
            if (HandleHttpOptionsRequest()) return null;

            if (WebOperationContext.Current.IncomingRequest.Headers["Proxy"] != null)
                if (WebOperationContext.Current.IncomingRequest.Headers["Proxy"].ToString() == "true")
            {
                return GetModelNoRedirect(pid, format, "", key);
            }
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
            if (HandleHttpOptionsRequest()) return null;

                if( WebOperationContext.Current.IncomingRequest.Headers["Proxy"] != null)
                if (WebOperationContext.Current.IncomingRequest.Headers["Proxy"].ToString() == "true")
                {
                    return GetModelNoRedirect(pid, format, "", key);
                }
                string address = GetRedirectAddressModelAdvanced(implementation.APIType.REST, pid, format, options);
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
                WebOperationContext.Current.OutgoingResponse.Location = address + "?ID=" + "00-00-00";
                return null;
        }

        private bool HandleHttpOptionsRequest()
        {
           
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", WebOperationContext.Current.IncomingRequest.Headers["Origin"]);

            if (WebOperationContext.Current.IncomingRequest.Method == "OPTIONS")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get the content for a model 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="format"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public Stream GetModelNoRedirect(string pid, string format, string options, string key)
        {
            if (HandleHttpOptionsRequest()) return null;

            try
            {
                string address = GetRedirectAddressModelAdvanced(implementation.APIType.REST, pid, format, options);
                address = address + "?ID=" + "00-00-00";
                System.Net.WebClient wc = GetWebClient();
                
                wc.Headers["Authorization"] = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                
                return new MemoryStream(wc.DownloadData(address));
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("(401) Unauthorized"))
                {
                    WebOperationContext.Current.OutgoingResponse.Headers[System.Net.HttpResponseHeader.WwwAuthenticate] = "BASIC realm=\"3DR API\"";

                    
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    return null;
                }
                else throw;
            }
        }

        /// <summary>
        /// Get the screenshot for a content object 
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public Stream GetScreenshot(string pid, string key)
        {
            if (HandleHttpOptionsRequest()) return null;

            string address = GetRedirectAddress("Screenshot", implementation.APIType.REST, pid);
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            WebOperationContext.Current.OutgoingResponse.Location = address + "?ID=" + "00-00-00";
            return null;
        }
        public Stream GetThumbnail(string pid, string key)
        {
            if (HandleHttpOptionsRequest()) return null;

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
            if (HandleHttpOptionsRequest()) return null;

            string address = GetRedirectAddress("OriginalUpload", implementation.APIType.REST, pid);
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
            if (HandleHttpOptionsRequest()) return null;

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
            if (HandleHttpOptionsRequest()) return null;

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
        public vwar.service.host.Metadata GetMetadataJSON(string pid, string key)
        {
            if (HandleHttpOptionsRequest()) return null;

            string address = GetRedirectAddress("Metadata", implementation.APIType.REST, pid);
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            WebOperationContext.Current.OutgoingResponse.Location = address + "/json?ID=" + "00-00-00";
            return null;
        }
        public vwar.service.host.Metadata GetMetadataXML(string pid, string key)
        {
            if (HandleHttpOptionsRequest()) return null;

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
        public List<vwar.service.host.Review> GetReviewsJSON(string pid, string key)
        {
            if (HandleHttpOptionsRequest()) return null;

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

            if (HandleHttpOptionsRequest()) return null;

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
            if (HandleHttpOptionsRequest()) return null;

            string address = GetRedirectAddress("Textures", implementation.APIType.REST, pid) + "/" + filename;
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            string format = "xml";
            if (WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri.AbsoluteUri.Contains("json"))
                format = "json";
            WebOperationContext.Current.OutgoingResponse.Location = address + "?ID=" + "00-00-00";
            return null;
        }
        public Stream GetTextureFileNoRedirect(string pid, string filename, string key)
        {
            if (HandleHttpOptionsRequest()) return null;
            
            try
            {
                string address = GetRedirectAddress("Textures", implementation.APIType.REST, pid) + "/" + filename;

                address = address + "?ID=" + "00-00-00";
                System.Net.WebClient wc = GetWebClient();
                //When serving images over cors with basic auth, a * is not allowed in the CORS header. Must add the specific origin
                wc.Headers["Authorization"] = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
               
                return new MemoryStream(wc.DownloadData(address));
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("(401) Unauthorized"))
                {
                    WebOperationContext.Current.OutgoingResponse.Headers[System.Net.HttpResponseHeader.WwwAuthenticate] = "BASIC realm=\"3DR API\"";
                    
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    return null;
                }
                else throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="terms"></param>
        /// <returns></returns>
        public List<vwar.service.host.SearchResult> SearchXML(string terms, string key) { if (HandleHttpOptionsRequest()) return null; return Search(terms, key); }
        public List<vwar.service.host.SearchResult> SearchJSON(string terms, string key) { if (HandleHttpOptionsRequest()) return null; return Search(terms, key); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public List<vwar.service.host.Review> GetReviewsXML(string pid, string key) {
            if (HandleHttpOptionsRequest()) return null;
            string address = GetRedirectAddress("Reviews", implementation.APIType.REST, pid);
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
            if (HandleHttpOptionsRequest()) return null;

            try
            {
                string address = GetRedirectAddress("Metadata", implementation.APIType.REST, pid);
                address = address + "/jsonp?ID=00-00-00&callback=" + callback;
                System.Net.WebClient wc = GetWebClient();
                wc.Headers["Authorization"] = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                WebOperationContext.Current.OutgoingResponse.Headers["Access-Control-Allow-Origin"] = WebOperationContext.Current.IncomingRequest.Headers["Origin"];
                return new MemoryStream(wc.DownloadData(address));
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("(401) Unauthorized"))
                {
                    WebOperationContext.Current.OutgoingResponse.Headers[System.Net.HttpResponseHeader.WwwAuthenticate] = "BASIC realm=\"3DR API\"";
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    WebOperationContext.Current.OutgoingResponse.Headers["Access-Control-Allow-Origin"] = WebOperationContext.Current.IncomingRequest.Headers["Origin"];
                    return null;
                }
                else throw;
            }
           
        }
        public Stream GetReviewsJSONP(string pid, string key, string callback)
        {
            if (HandleHttpOptionsRequest()) return null;

            try
            {
                string address = GetRedirectAddress("Reviews", implementation.APIType.REST, pid);
                address += address + "/jsonp?ID=00-00-00&callback=" + callback;
                System.Net.WebClient wc = GetWebClient();
                wc.Headers["Authorization"] = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                WebOperationContext.Current.OutgoingResponse.Headers["Access-Control-Allow-Origin"] = WebOperationContext.Current.IncomingRequest.Headers["Origin"];
                return new MemoryStream(wc.DownloadData(address));
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("(401) Unauthorized"))
                {
                    WebOperationContext.Current.OutgoingResponse.Headers[System.Net.HttpResponseHeader.WwwAuthenticate] = "BASIC realm=\"3DR API\"";
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    WebOperationContext.Current.OutgoingResponse.Headers["Access-Control-Allow-Origin"] = WebOperationContext.Current.IncomingRequest.Headers["Origin"];
                    return null;
                }
                else throw;
            }
           
        }
        public Stream SearchJSONP(string terms, string key, string callback)
        {
            if (HandleHttpOptionsRequest()) return null;

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
            WebOperationContext.Current.OutgoingResponse.Headers["Access-Control-Allow-Origin"] = WebOperationContext.Current.IncomingRequest.Headers["Origin"];
            return new MemoryStream(a);
        }
        public List<vwar.service.host.SearchResult> AdvancedSearchJSON(string mode, string terms, string key){return AdvancedSearch( mode,  terms,  key);}
        public List<vwar.service.host.SearchResult> AdvancedSearchXML(string mode, string terms, string key) { return AdvancedSearch(mode, terms, key); }
        public Stream AdvancedSearchJSONP(string mode, string terms, string key, string callback)
        {
            if (HandleHttpOptionsRequest()) return null;

            List<vwar.service.host.SearchResult> md = AdvancedSearch(mode, terms, key);
            MemoryStream stream1 = new MemoryStream();
            System.Runtime.Serialization.Json.DataContractJsonSerializer ser = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(List<vwar.service.host.SearchResult>));
            ser.WriteObject(stream1, md);
            stream1.Seek(0, SeekOrigin.Begin);
            System.IO.StreamReader sr = new StreamReader(stream1);
            string data = sr.ReadToEnd();
            data = callback + "(" + data + ");";

            byte[] a = System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(data);

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            WebOperationContext.Current.OutgoingResponse.Headers["Access-Control-Allow-Origin"] = WebOperationContext.Current.IncomingRequest.Headers["Origin"];
            return new MemoryStream(a);
        }
        ///////////////////////////////////////////////////////
        ///
        ///Not implemented by the federation
        ////

        public string UploadScreenShot(Stream md, string pid, string key) { return "Not Implemented"; }

        public string UploadMissingTexture(Stream md, string pid, string key) { return "Not Implemented"; }

        public string UploadSupportingFile(Stream md, string pid, string description, string key) { return "Not Implemented"; }

        public string UploadDeveloperLogo(Stream md, string pid, string key) { return "Not Implemented"; }

        public string UploadSponsorLogo(Stream md, string pid, string key) { return "Not Implemented"; }

        public string AddReviewXML(Stream md, string pid, string key) { return "Not Implemented"; }

        public string AddReviewJSON(Stream md, string pid, string key) { return "Not Implemented"; }

        public string UpdateMetadataXML(Stream md, string pid, string key) { return "Not Implemented"; }

        public string UpdateMetadataJSON(Stream md, string pid, string key) { return "Not Implemented"; }

        public string UploadFile(Stream indata, string pid, string key) { return "Not Implemented"; }

        public string UploadFileStub(Stream indata, string key) { return "Not Implemented"; }

        public string GetGroupPermissionJSON(string n, string n1, string n2){return "Not Implemented";}
        public string GetGroupPermissionXML(string n, string n1, string n2){return "Not Implemented";}

        public string GetUserPermissionJSON(string n, string n1, string n2){return "Not Implemented";}
        public string GetUserPermissionXML(string n, string n1, string n2){return "Not Implemented";}

        public string SetUserPermission(string n, string n1, string n2, string n3) { return "Not Implemented"; }
        public string SetGroupPermission(string n, string n1, string n2, string n3) { return "Not Implemented"; }

        public string UploadFileStub2(System.IO.Stream o,string n, string n1){ return "Not Implemented"; }
        public string UploadFileStub3(System.IO.Stream o,string n){ return "Not Implemented"; }

        //////////////////////////////////////////////////////////////////////////////////////////////////
        //CORS handling
        private void SetCorsHeaders()
        {
            if (WebOperationContext.Current != null)
                WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", WebOperationContext.Current.IncomingRequest.Headers["Origin"]);
        }
        public Stream CORSGetDeveloperLogo(string i, string h) { SetCorsHeaders(); return null; }
        public Stream CORSGetModel(string f, string s, string ss, string g) { SetCorsHeaders(); return null; }
        public Stream CORSGetModelSimple(string s, string sa, string a) { SetCorsHeaders(); return null; }
        public Stream CORSGetOriginalUploadFile(string f, string d) { SetCorsHeaders(); return null; }
        public Stream CORSGetScreenshot(string f, string fs) { SetCorsHeaders(); return null; }
        public Stream CORSGetSponsorLogo(string f, string d) { SetCorsHeaders(); return null; }
        public Stream CORSGetSupportingFile(string f, string d, string fd) { SetCorsHeaders(); return null; }
        public Stream CORSGetTextureFile(string f, string d, string df) { SetCorsHeaders(); return null; }
        public Stream CORSGetThumbnail(string d, string da) { SetCorsHeaders(); return null; }
        public Stream CORSGetTextureFileNoRedirect(string pid, string filename, string key) { SetCorsHeaders(); return null; }
        public Stream CORSGetModelNoRedirect(string pid, string format, string options, string key) { SetCorsHeaders(); return null; }
    }
}
