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
using System.ServiceModel.Web;
using System.IO;
using System.Configuration;
using System.Web.Script.Serialization;
namespace vwar.service.host
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceDescription
    {
        public String SearchJSON;
        public String SearchJSONP;
        public String SearchXML;
        public String Upload;
        public String AccessJSONP;
        public String AccessJSON;
        public String AccessXML;
        public string Namespace;
        
        public string OrganizationName;
        public string OrganizationPOC;
        public string OrganizationPOCEmail;
        public string OrganizationURL;
    }
    public class _3DRAPI : _3DRAPI_Imp, I3DRAPI
    {
        public string GetBaseAddress()
        {
            string location = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri.AbsoluteUri;
           // location = location.Substring(location.LastIndexOf(".svc") + 4);
            return location;
        }
        public ServiceDescription Describe()
        {
            ServiceDescription result = new ServiceDescription();
            result.SearchJSON = GetBaseAddress() +"/Search/term/json?ID=00-00-00";
            result.SearchJSONP = GetBaseAddress() +"/Search/term/jsonp?ID=00-00-00&callback=callme";
            result.SearchXML = GetBaseAddress() +"/Search/term/xml?ID=00-00-00";
            result.Upload = GetBaseAddress() +"/UploadFile?ID=00-00-00";
            result.AccessJSON = GetBaseAddress() +"/pid:0/Metadata/json?ID=00-00-00";
            result.AccessJSONP = GetBaseAddress() +"/pid:0/Metadata/json?ID=00-00-00&callback=callme";
            result.AccessXML = GetBaseAddress() +"/pid:0/Metadata/xml?ID=00-00-00";

            result.Namespace = ConfigurationManager.AppSettings["fedoraNamespace"];
           
            result.OrganizationName = ConfigurationManager.AppSettings["OrganizationName"]; ;
            result.OrganizationPOC = ConfigurationManager.AppSettings["OrganizationPOC"]; ;
            result.OrganizationPOCEmail = ConfigurationManager.AppSettings["OrganizationPOCEmail"]; ;
            result.OrganizationURL = ConfigurationManager.AppSettings["OrganizationURL"]; ;
           

            return result;
        }
        public List<SearchResult> SearchXML(string terms, string key) 
        {
            List<SearchResult> results = Search(terms, key);
            foreach (SearchResult sr in results)
            {
                sr.DataLink = GetBaseAddress()+"/"+sr.PID+"/Metadata/xml?ID="+key;
            }
            return results;
        }
        public List<Review> GetReviewsXML(string pid, string key) 
        {
            List<Review> results = GetReviews(pid, key);
            foreach (Review rv in results)
            {
                rv.PIDLink = GetBaseAddress() + "/" + pid + "/Metadata/xml?ID=" + key;
            }
            return results;
            
        }
        public Metadata GetMetadataXML(string pid, string key) { 
            Metadata result = GetMetadataJSON(pid, key);
            result._ReviewsLink = GetBaseAddress() + "/" + pid + "/Reviews/XML?ID=" + key;
            return result;
        }
        public List<SearchResult> AdvancedSearchXML(string searchmethod, string searchstring, string key) {
            List<SearchResult> results = AdvancedSearch(searchmethod, searchstring, key);
            foreach (SearchResult sr in results)
            {
                sr.DataLink = GetBaseAddress() + "/" + sr.PID + "/Metadata/xml?ID=" + key;
            }
            return results;
        }
        public string GetGroupPermissionXML(string pid, string username, string key){return GetGroupPermission(pid, username, key);}
        public string GetUserPermissionXML(string pid, string username, string key){ return GetUserPermission(pid, username, key);}

        public List<SearchResult> SearchJSON(string terms, string key)
        {
            List<SearchResult> results = Search(terms, key);
            foreach (SearchResult sr in results)
            {
                sr.DataLink = GetBaseAddress() + "/" + sr.PID + "/Metadata/json?ID=" + key;
            }
            return results;
        }
        public List<Review> GetReviewsJSON(string pid, string key)
        {
            List<Review> results = GetReviews(pid, key);
            foreach (Review rv in results)
            {
                rv.PIDLink = GetBaseAddress() + "/" + pid + "/Metadata/xml?ID=" + key;
            }
            return results;

        }
        public Metadata GetMetadataJSON(string pid, string key) {
            Metadata result = GetMetadata(pid, key);
            if (result.ConversionAvailable)
            {
                result._3dsLink = GetBaseAddress() + "/" + pid + "/Format/3ds?ID=" + key;
                result._fbxLink = GetBaseAddress() + "/" + pid + "/Format/fbx?ID=" + key;
                result._jsonLink = GetBaseAddress() + "/" + pid + "/Format/json?ID=" + key;
                result._o3dLink = GetBaseAddress() + "/" + pid + "/Format/o3d?ID=" + key;
                result._objLink = GetBaseAddress() + "/" + pid + "/Format/obj?ID=" + key;
            }
            result._OriginalUploadLink = GetBaseAddress() + "/" + pid + "/OriginalUpload?ID=" + key;
            result._ScreenshotLink = GetBaseAddress() + "/" + pid + "/ScreenShot?ID=" + key;
            result._ThumbnailLink = GetBaseAddress() + "/" + pid + "/Thumbnail?ID=" + key;
            result._ReviewsLink = GetBaseAddress() + "/" + pid + "/Reviews/json?ID=" + key;
            foreach(Texture t in result.TextureReferences)
            {
                bool missing = false;
                foreach(Texture t2 in result.MissingTextures)
                {
                    if (t2.mFilename == t.mFilename)
                        missing = true;
                }
                if (!missing) 
                t._Link = GetBaseAddress() + "/" + pid + "/Textures/"+t.mFilename+"?ID=" + key;
            }
            foreach (SupportingFile t in result.SupportingFiles)
            {
                t._Link = GetBaseAddress() + "/" + pid + "/SupportingFiles/" + t.Filename + "?ID=" + key;
            }
            return result;
        }
        public List<SearchResult> AdvancedSearchJSON(string searchmethod, string searchstring, string key) {
            
            List < SearchResult > results = AdvancedSearch(searchmethod, searchstring, key);
            foreach (SearchResult sr in results)
            {
                sr.DataLink = GetBaseAddress() + "/" + sr.PID + "/Metadata/json?ID=" + key;
            }
            return results;
        }
        public string GetGroupPermissionJSON(string pid, string username, string key) { return GetGroupPermission(pid, username, key); }
        public string GetUserPermissionJSON(string pid, string username, string key) { return GetUserPermission(pid, username, key); }


        public string AddReviewJSON(Stream indata, string pid, string key)
        {
            //Read in the data as it streams in
            MemoryStream ms = new MemoryStream();
            int pos = 1;
            while (pos != -1)
            {
                pos = indata.ReadByte();
                if (pos > -1)
                    ms.WriteByte((byte)pos);
            }
            //We now have compleated the streamin in operation
            //Read the stream as a string
            ms.Seek(0, SeekOrigin.Begin);
            byte[] data = new byte[ms.Length];
            ms.Read(data, 0, (int)ms.Length);
            ms.Seek(0, SeekOrigin.Begin);
            string s = "";
            for (int i = 0; i < data.Length; i++)
                s += Convert.ToChar(data[i]);

            //deSerialize the data

            Review md = (new JavaScriptSerializer()).Deserialize<Review>(s);

            //Call the base class
            return base.AddReview(md, pid, key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="indata"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public string AddReviewXML(Stream indata, string pid, string key)
        {
            //Read in the data as it streams in
            MemoryStream ms = new MemoryStream();
            int pos = 1;
            while (pos != -1)
            {
                pos = indata.ReadByte();
                if (pos > -1)
                    ms.WriteByte((byte)pos);
            }
            //We now have compleated the streamin in operation
            //Read the stream as a string
            ms.Seek(0, SeekOrigin.Begin);
            byte[] data = new byte[ms.Length];
            ms.Read(data, 0, (int)ms.Length);
            ms.Seek(0, SeekOrigin.Begin);
            string s = "";
            for (int i = 0; i < data.Length; i++)
                s += Convert.ToChar(data[i]);

            //deSerialize the data
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(Review));
            System.IO.TextReader tx = new System.IO.StringReader(s);
            Review md = (Review)xs.Deserialize(tx);

            //Call the base class
            return base.AddReview(md, pid, key);
        }
        /// <summary>
        /// This must deserialize a metadata object from the 
        /// data in an HTTP post
        /// </summary>
        /// <param name="indata"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public string UpdateMetadataXML(Stream indata, string pid, string key)
        {
            //Read in the data as it streams in
            MemoryStream ms = new MemoryStream();
            int pos = 1;
            while (pos != -1)
            {
                pos = indata.ReadByte();
                if (pos > -1)
                    ms.WriteByte((byte)pos);
            }
            //We now have compleated the streamin in operation
            //Read the stream as a string
            ms.Seek(0, SeekOrigin.Begin);
            byte[] data = new byte[ms.Length];
            ms.Read(data, 0, (int)ms.Length);
            ms.Seek(0, SeekOrigin.Begin);
            string s = "";
            for (int i = 0; i < data.Length; i++)
                s += Convert.ToChar(data[i]);

            //deSerialize the data
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(Metadata));
            System.IO.TextReader tx = new System.IO.StringReader(s);
            Metadata md = (Metadata)xs.Deserialize(tx);

            //Call the base class
            return base.UpdateMetadata(md, pid, key);
        }

        /// <summary>
        /// This must deserialize a metadata object from the 
        /// data in an HTTP post
        /// </summary>
        /// <param name="indata"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public string UpdateMetadataJSON(Stream indata, string pid, string key)
        {
            
            //Read in the data as it streams in
            MemoryStream ms = new MemoryStream();
            int pos = 1;
            while (pos != -1)
            {
                pos = indata.ReadByte();
                if (pos > -1)
                    ms.WriteByte((byte)pos);
            }
            //We now have compleated the streamin in operation
            //Read the stream as a string
            ms.Seek(0, SeekOrigin.Begin);
            byte[] data = new byte[ms.Length];
            ms.Read(data, 0, (int)ms.Length);
            ms.Seek(0, SeekOrigin.Begin);
            string s = "";
            for (int i = 0; i < data.Length; i++)
                s += Convert.ToChar(data[i]);

            //deSerialize the data
            JavaScriptSerializer js = new JavaScriptSerializer();
            Metadata md = (Metadata)js.Deserialize<Metadata>(s);

            //Call the base class
            return base.UpdateMetadata(md, pid, key);
        }

        /// <summary>
        /// Convert a stream to data 
        /// </summary>
        /// <param name="indata"></param>
        /// <returns></returns>
        private byte[] StreamToData(Stream indata)
        {
            //Read in the streaming data
            MemoryStream ms = new MemoryStream();
            int pos = 1;
            while (pos != -1)
            {
                pos = indata.ReadByte();
                //no idea whats going on, sseems to be padded with 23 '45's
                
                ms.WriteByte((byte)pos);
                
            }
            //Now, the streaming is complete
            ms.Seek(0, SeekOrigin.Begin);
            byte[] data = new byte[ms.Length];
            ms.Read(data, 0, (int)ms.Length);
            return data;
        }

        /// <summary>
        /// Add a new content object 
        /// </summary>
        /// <param name="indata"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public string UploadFileStub(Stream indata, string key)
        {
            //Read the stream then call base class
            return base.UploadFile(StreamToData(indata), "", key);
        }

        /// <summary>
        /// Add a new content object 
        /// </summary>
        /// <param name="indata"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public string UploadFile(Stream indata, string pid, string key)
        {
            //Read the stream then call base class
            return base.UploadFile(StreamToData(indata), pid, key);
        }

        /// <summary>
        /// Add a new content object 
        /// </summary>
        /// <param name="indata"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public string UploadFileStub3(Stream indata, string key)
        {
            //Read the stream then call base class
            return base.UploadFile(StreamToData(indata), "", key);
        }

        /// <summary>
        /// Add a new content object 
        /// </summary>
        /// <param name="indata"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public string UploadFileStub2(Stream indata, string pid, string key)
        {
            //Read the stream then call base class
            return base.UploadFile(StreamToData(indata), pid, key);
        }

        /// <summary>
        /// Add a developer logo 
        /// </summary>
        /// <param name="indata"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public string UploadDeveloperLogo(Stream indata, string pid, string key)
        {
            //Get the name of the file that the client uploaded
            string content = WebOperationContext.Current.IncomingRequest.Headers["Content-disposition"];
            //Read the stream then call base class
            string filename = content.Substring(content.LastIndexOf("=") + 1);
            return base.UploadDeveloperLogo(StreamToData(indata), pid, filename, key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="indata"></param>
        /// <param name="pid"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public string UploadSupportingFile(Stream indata, string pid, string description, string key)
        {
            //Get the name of the file that the client uploaded
            string content = WebOperationContext.Current.IncomingRequest.Headers["Content-disposition"];
            //Read the stream then call base class
            string filename = content.Substring(content.LastIndexOf("=") + 1);
            return base.UploadSupportingFile(StreamToData(indata), pid, filename, description, key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="indata"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public string UploadSponsorLogo(Stream indata, string pid, string key)
        {
            //Get the name of the file that the client uploaded
            string content = WebOperationContext.Current.IncomingRequest.Headers["Content-disposition"];
            //Read the stream then call base class
            string filename = content.Substring(content.LastIndexOf("=") + 1);
            return base.UploadSponsorLogo(StreamToData(indata), pid, filename, key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="indata"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public string UploadScreenShot(Stream indata, string pid, string key)
        {
            //Get the name of the file that the client uploaded
            string content = WebOperationContext.Current.IncomingRequest.Headers["Content-disposition"];
            //Read the stream then call base class
            string filename = content.Substring(content.LastIndexOf("=") + 1);
            return base.UploadScreenShot(StreamToData(indata), pid, filename, key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="indata"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public string UploadMissingTexture(Stream indata, string pid, string key)
        {
            //Get the name of the file that the client uploaded
            string content = WebOperationContext.Current.IncomingRequest.Headers["Content-disposition"];
            //Read the stream then call base class
            string filename = content.Substring(content.LastIndexOf("=") + 1);
            return base.UploadMissingTexture(StreamToData(indata), pid, filename, key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="indata"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public Stream GetMetadataJSONP(string pid, string key, string callback)
        {
            Metadata md = GetMetadataJSON(pid, key);
            MemoryStream stream1 = new MemoryStream();
            System.Runtime.Serialization.Json.DataContractJsonSerializer ser = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(Metadata));
            ser.WriteObject(stream1, md);
            stream1.Seek(0, SeekOrigin.Begin);
            System.IO.StreamReader sr = new StreamReader(stream1);
            string data = sr.ReadToEnd();
            data = callback + "(" + data + ");";

            byte[] a = System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(data);

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            return new MemoryStream(a);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indata"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public Stream GetReviewsJSONP(string pid, string key, string callback)
        {
            List<Review> md = GetReviewsJSON(pid, key);
            MemoryStream stream1 = new MemoryStream();
            System.Runtime.Serialization.Json.DataContractJsonSerializer ser = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(List<Review>));
            ser.WriteObject(stream1, md);
            stream1.Seek(0, SeekOrigin.Begin);
            System.IO.StreamReader sr = new StreamReader(stream1);
            string data = sr.ReadToEnd();
            data = callback + "(" + data + ");";

            byte[] a = System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(data);

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            return new MemoryStream(a);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="indata"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public Stream SearchJSONP(string terms, string key, string callback)
        {
            List<SearchResult> md = SearchJSON(terms, key);
            MemoryStream stream1 = new MemoryStream();
            System.Runtime.Serialization.Json.DataContractJsonSerializer ser = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(List<SearchResult>));
            ser.WriteObject(stream1, md);
            stream1.Seek(0, SeekOrigin.Begin);
            System.IO.StreamReader sr = new StreamReader(stream1);
            string data = sr.ReadToEnd();
            data = callback + "(" + data + ");";

            byte[] a = System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(data);

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            return new MemoryStream(a);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="indata"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public Stream AdvancedSearchJSONP(string searchmethod, string searchstring, string key, string callback)
        {
            List<SearchResult> md = AdvancedSearchJSON(searchmethod, searchstring, key);
            MemoryStream stream1 = new MemoryStream();
            System.Runtime.Serialization.Json.DataContractJsonSerializer ser = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(List<SearchResult>));
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
