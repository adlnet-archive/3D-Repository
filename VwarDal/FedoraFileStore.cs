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
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Net;
using System.Configuration;
namespace vwarDAL
{
    /// <summary>
    /// 
    /// </summary>
    class FedoraFileStore : vwarDAL.IFileStore
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly System.Net.NetworkCredential _Credantials;
        /// <summary>
        /// 
        /// </summary>
        private const string DUBLINCOREID = "Dublin Core Record for this object";
        /// <summary>
        /// 
        /// </summary>
        private readonly string _BaseUrl;
        /// <summary>
        /// 
        /// </summary>
        private readonly string _AccessUrl;
        /// <summary>
        /// 
        /// </summary>
        private readonly string _ManagementUrl;
        /// <summary>
        /// 
        /// </summary>
        private static readonly string BASECONTENTURL = "{0}objects/{1}/datastreams/{2}/";
        /// <summary>
        /// 
        /// </summary>
        private static readonly string DOWNLOADURL = BASECONTENTURL + "content";
        /// <summary>
        /// 
        /// </summary>
        private static readonly string REVIEWNAMESPACE = "review";
        /// <summary>
        /// 
        /// </summary>
        private const string DATEFORMAT = "yyyy'-'MM'-'dd'Z'";
        /// <summary>
        /// 
        /// </summary>
        private string _FileNamespace;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="access"></param>
        /// <param name="management"></param>
        /// <param name="fileNamespace"></param>
        public FedoraFileStore(string url, string userName, string password, string access, string management, string fileNamespace)
        {
            _BaseUrl = url;
            _AccessUrl = access;
            _ManagementUrl = management;
            _Credantials = new System.Net.NetworkCredential(userName, password);
            _FileNamespace = fileNamespace;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private FedoraAPIA.FedoraAPIAService GetAccessService()
        {
            FedoraAPIA.FedoraAPIAService svc = new FedoraAPIA.FedoraAPIAService();
            svc.Url = _AccessUrl;
            svc.Credentials = _Credantials;
            return svc;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private FedoraAPIM.FedoraAPIMService GetManagementService()
        {
            FedoraAPIM.FedoraAPIMService svc = new FedoraAPIM.FedoraAPIMService();
            svc.Url = _ManagementUrl;
            svc.Credentials = _Credantials;
            return svc;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        public void InsertContentObject(ContentObject co)
        {
            using (var srv = GetManagementService())
            {
                var pid = string.IsNullOrEmpty(co.PID) ? srv.getNextPID("1", _FileNamespace)[0] : co.PID;
                co.PID = pid;
                var dataObject = CreateDigitalObject(co);
                var data = SerializeObject(dataObject);
                srv.ingest(data, "info:fedora/fedora-system:FOXML-1.1", "add file");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        private static byte[] SerializeObject(object dataObject)
        {
            XmlSerializer s = new XmlSerializer(dataObject.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                s.Serialize(stream, dataObject);
                stream.Position = 0;
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
                return data;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pid"></param>
        /// <param name="fileName"></param>
        /// <param name="newFileName"></param>
        /// <returns></returns>
        public string UpdateFile(byte[] data, string pid, string fileName, string newFileName = null)
        {

            var mimeType = "";
            //string destinationFileName = fileName.Replace(Path.GetExtension(fileName), Path.GetExtension(newFileName));
            if (!String.IsNullOrEmpty(newFileName))
            {
                mimeType = DataUtils.GetMimeType(newFileName);
            }
            else
            {
                mimeType = DataUtils.GetMimeType(fileName);
            }
            if (String.IsNullOrEmpty(pid) || String.IsNullOrEmpty(fileName)) return String.Empty;
            if (!String.IsNullOrEmpty(newFileName))
            {
                using (var srv = GetManagementService())
                {
                        //srv.modifyDatastreamByReference(pid,
                        //GetDSId(pid, fileName),
                        //new string[0],
                        //newFileName,
                        //mimeType,
                        //"",
                        //GetContentUrl(pid, "Dublin Core Record for this object"),
                        //"Disabled",
                        //"none",
                        //"Add Review",
                        //true
                        //);
                }
            }

            var requestURL = GetContentUrl(pid, GetDSId(pid,fileName));
            requestURL = requestURL.Substring(0, requestURL.LastIndexOf('/'));
            using (WebClient client = new WebClient())
            {

                client.Credentials = _Credantials;
                client.Headers.Add("Content-Type", mimeType);
                client.UploadData(requestURL, "POST", data);

            }

            return GetDSId(pid, fileName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="fileName"></param>
        public void RemoveFile(string pid, string fileName)
        {
            string dsid = GetDSId(pid, fileName);
            using (var srv = GetManagementService())
            {
                srv.purgeDatastream(pid, dsid, CurrentDate, CurrentDate, "", false);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public Stream GetContentFile(string pid, string file)
        {
            return new MemoryStream(GetContentFileData(pid, file));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="dsid"></param>
        /// <returns></returns>
        public byte[] GetContentFileData(string pid, string dsid)
        {
            var url = GetContentUrl(pid, dsid);
            byte[] data = new byte[0];
            var client = new WebClient();
            client.Credentials = _Credantials;

            if (url != "")
                try
                {
                    data = client.DownloadData(url);
                    if (data.Length != 0)
                        return data;
                }
                catch (Exception)
                {

                }

            url = GetContentUrl(pid, GetDSId(pid, dsid));
            try
            {
                data = client.DownloadData(url);
                if (data.Length != 0)
                    return data;
            }
            catch (Exception)
            {

            }


            return data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ContentObject GetNewContentObject()
        {
            ContentObject co = new ContentObject();

            return co;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <returns></returns>
        private static digitalObject CreateDigitalObject(ContentObject co)
        {
            var dObj = new digitalObject();
            dObj.PID = co.PID;
            dObj.objectProperties = new objectPropertiesType();
            dObj.objectProperties.property = new propertyType[1];
            var label = new propertyType();
            label.NAME = propertyTypeNAME.infofedorafedorasystemdefmodellabel;
            label.VALUE = co.Title;
            dObj.objectProperties.property[0] = label;
            return dObj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pid"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string UploadFile(byte[] data, string pid, string fileName)
        {
            var mimeType = DataUtils.GetMimeType(fileName);
            if (pid.Contains("~"))
            {
                return "";
            }
            string dsid = "", output = "";
            using (var srv = GetManagementService())
            {
                int maxNumberOfTries = 10;
                int numberOfTries = 0;
                while (String.IsNullOrEmpty(output) && numberOfTries <= maxNumberOfTries)
                {
                    try
                    {
                        dsid = srv.getNextPID("1", "content")[0].Replace(":", "");

                        output = srv.addDatastream(pid,
                        dsid,
                        new string[] { },
                        fileName,
                        true,
                        mimeType,
                        "",
                        "http://www.google.com",
                        "M",
                        "A",
                        "Disabled",
                        "none",
                        "add");
                    }
                    catch
                    {
                        numberOfTries++;
                    }
                }
                bool uploadComplete = false;

                while (!uploadComplete && numberOfTries <= maxNumberOfTries)
                {
                    try
                    {

                        string requestURL = String.Format(BASECONTENTURL, _BaseUrl, pid, output);

                        using (WebClient client = new WebClient())
                        {

                            client.Proxy = null;
                            client.Credentials = _Credantials;
                            client.Headers.Add("Content-Type", mimeType);
                            client.UploadData(requestURL, data);
                            uploadComplete = true;
                        }
                    }
                    catch (Exception e)
                    {
                        numberOfTries++;
                    }

                }

            }
            return dsid;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pid"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string UploadFile(Stream data, string pid, string fileName)
        {
            data.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[data.Length];
            data.Read(buffer, 0, (int)data.Length);
            return UploadFile(buffer, pid, fileName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pid"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string UploadFile(string data, string pid, string fileName)
        {
            System.Text.UTF8Encoding encoding = new UTF8Encoding();
            return UploadFile(encoding.GetBytes(data), pid, fileName);
        }
        /// <summary>
        /// 
        /// </summary>
        private String CurrentDate
        {
            get { return ""; }// DateTime.Now.ToString(DATEFORMAT); 
        }
        /// <summary>
        /// 
        /// </summary>
        private static readonly Dictionary<string, IEnumerable<FedoraAPIM.Datastream>> DATASTREAMCACHE = new Dictionary<string, IEnumerable<FedoraAPIM.Datastream>>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetDSId(string pid, string fileName)
        {
            string dsid = "";
            using (var srv = GetManagementService())
            {
                IEnumerable<FedoraAPIM.Datastream> streams;
                if (DATASTREAMCACHE.ContainsKey(pid))
                {
                    streams = DATASTREAMCACHE[pid];
                }
                else
                {
                    streams = srv.getDatastreams(pid, CurrentDate, "A"); ;
                    if (!DATASTREAMCACHE.ContainsKey(pid))
                    {
                        try
                        {
                            DATASTREAMCACHE.Add(pid, streams);
                        }
                        catch //TODO: Understand why we need this catch block for this to work!
                        {
                        }
                    }
                }

                var dss = (from s in streams
                           where s.label.Equals(fileName, StringComparison.InvariantCultureIgnoreCase)
                           select s);
                if (dss != null && dss.Count() > 0)
                {
                    var ds = dss.First();
                    dsid = ds.ID;

                }
                //if the content ID is cached, but we did not find the datastream we were looking for, 
                //update the value in the cache from fedora
                else
                {
                    //get the streams and overwrite the cached value
                    streams = srv.getDatastreams(pid, CurrentDate, "A"); ;
                    DATASTREAMCACHE[pid] = streams;

                    //check the new streams for the datastream with the label we wanted
                    dss = (from s in streams
                           where s.label.Equals(fileName, StringComparison.InvariantCultureIgnoreCase)
                           select s);
                    //set it and return
                    if (dss != null && dss.Count() > 0)
                    {
                        var ds = dss.First();
                        dsid = ds.ID;

                    }
                }
            }
            return dsid;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="dsid"></param>
        /// <returns></returns>
        private string GetContentUrl(string pid, string dsid)
        {
            if (String.IsNullOrEmpty(pid) || String.IsNullOrEmpty(dsid)) return "";

            return string.Format(DOWNLOADURL, _BaseUrl, pid, dsid);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="dsid"></param>
        /// <returns></returns>
        private string FormatContentUrl(string pid, string dsid)
        {
            return string.Format(DOWNLOADURL, _BaseUrl, pid, dsid);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="co"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool AddSupportingFile(Stream data, ContentObject co, string filename)
        {

            string url = GetDSId(co.PID, filename);

            if (url == "")
            {
                UploadFile(data, co.PID, filename);
            }
            else
            {
                //file already existed
                UpdateFile(data, co.PID, filename, filename);
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        public void DeleteContentObject(ContentObject co)
        {
            //remove the files from fedora
            using (var srv = GetManagementService())
            {
                srv.modifyObject(co.PID, "D", co.Label, "", "");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pid"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string SetContentFile(Stream data, string pid, string filename)
        {
            string url = GetDSId(pid, filename);

            if (url == "")
            {
                return UploadFile(data, pid, filename);
            }
            else
            {
                //file already existed
                return UpdateFile(data, pid, filename, filename);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pid"></param>
        /// <param name="fileName"></param>
        /// <param name="newfileName"></param>
        /// <returns></returns>
        public string UpdateFile(Stream data, string pid, string fileName, string newfileName = null)
        {
            data.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[data.Length];
            data.Read(buffer, 0, (int)data.Length);
            return UpdateFile(buffer, pid, fileName, newfileName);
        }
    }
}
