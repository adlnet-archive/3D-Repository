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
    public class _3DRAPI : _3DRAPI_Imp, I3DRAPI
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<SearchResult> Search2(string terms, string key) { return Search(terms, key); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<Review> GetReviews2(string pid, string key) { return GetReviews(pid, key); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public Metadata GetMetadata2(string pid, string key) { return GetMetadata(pid, key); }

        public List<SearchResult> AdvancedSearch2(string searchmethod, string searchstring, string key) { return AdvancedSearch(searchmethod, searchstring, key); }

        public string GetGroupPermission2(string pid, string username, string key)
        {
            return GetGroupPermission(pid, username, key);
        }
        public string GetUserPermission2(string pid, string username, string key)
        {
            return GetUserPermission(pid, username, key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="indata"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
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
            Metadata md = GetMetadata(pid, key);
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
            List<Review> md = GetReviews(pid, key);
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
            List<SearchResult> md = Search(terms, key);
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
            List<SearchResult> md = AdvancedSearch(searchmethod, searchstring, key);
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
