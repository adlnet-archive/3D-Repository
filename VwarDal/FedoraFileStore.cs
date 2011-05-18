using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Net;

namespace vwarDAL
{
    class FedoraFileStore : vwarDAL.IFileStore
    {
        private readonly System.Net.NetworkCredential _Credantials;
        private const string DUBLINCOREID = "Dublin Core Record for this object";
        private readonly string _BaseUrl;
        private readonly string _AccessUrl;
        private readonly string _ManagementUrl;
        private static readonly string BASECONTENTURL = "{0}objects/{1}/datastreams/{2}/";
        private static readonly string DOWNLOADURL = BASECONTENTURL + "content";
        private static readonly string REVIEWNAMESPACE = "review";
        private const string DATEFORMAT = "yyyy'-'MM'-'dd'Z'";
        public FedoraFileStore(string url, string userName, string password, string access, string management)
        {
            _BaseUrl = url;
            _AccessUrl = access;
            _ManagementUrl = management;
            _Credantials = new System.Net.NetworkCredential(userName, password);
        }
        private FedoraAPIA.FedoraAPIAService GetAccessService()
        {
            FedoraAPIA.FedoraAPIAService svc = new FedoraAPIA.FedoraAPIAService();
            svc.Url = _AccessUrl;
            svc.Credentials = _Credantials;
            return svc;
        }

        private FedoraAPIM.FedoraAPIMService GetManagementService()
        {
            FedoraAPIM.FedoraAPIMService svc = new FedoraAPIM.FedoraAPIMService();
            svc.Url = _ManagementUrl;
            svc.Credentials = _Credantials;
            return svc;
        }
        public void InsertContentObject(ContentObject co)
        {
            using (var srv = GetManagementService())
            {
                var pid = string.IsNullOrEmpty(co.PID) ? srv.getNextPID("1", "adl")[0] : co.PID;
                co.PID = pid;
                var dataObject = CreateDigitalObject(co);
                var data = SerializeObject(dataObject);
                srv.ingest(data, "info:fedora/fedora-system:FOXML-1.1", "add file");
            }
        }
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
                    srv.modifyDatastreamByReference(pid,
                        GetDSId(pid, fileName),
                        new string[0],
            newFileName,
            mimeType,
            "",
            GetContentUrl(pid, "Dublin Core Record for this object"),
                    "Disabled",
                    "none",
            "Add Review",
            true
            );
                }
            }

            var requestURL = GetContentUrl(pid, fileName);
            requestURL = requestURL.Substring(0, requestURL.LastIndexOf('/'));
            using (WebClient client = new WebClient())
            {

                client.Credentials = _Credantials;
                client.Headers.Add("Content-Type", mimeType);
                client.UploadData(requestURL, "POST", data);

            }

            return GetDSId(pid, fileName);
        }
        public void RemoveFile(string pid, string fileName)
        {
            string dsid = GetDSId(pid, fileName);
            using (var srv = GetManagementService())
            {
                srv.purgeDatastream(pid, dsid, CurrentDate, CurrentDate, "", false);
            }
        }
        public Stream GetContentFile(string pid, string file)
        {
            return new MemoryStream(GetContentFileData(pid, file));
        }
        public byte[] GetContentFileData(string pid, string dsid)
        {
            var url = GetContentUrl(pid, dsid);
            using (var client = new WebClient())
            {
                client.Credentials = _Credantials;
                if (url != "")
                    try
                    {
                        return client.DownloadData(url);
                    }
                    catch (Exception)
                    {
                    }

                return new byte[0];
            }
        }
        public ContentObject GetNewContentObject()
        {
            ContentObject co = new ContentObject();

            return co;
        } 
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
                        GetContentUrl(pid, "Dublin Core Record for this object"),
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
        private string UploadFile(Stream data, string pid, string fileName)
        {
            data.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[data.Length];
            data.Read(buffer, 0, (int)data.Length);
            return UploadFile(buffer, pid, fileName);
        }
        private string UploadFile(string data, string pid, string fileName)
        {
            System.Text.UTF8Encoding encoding = new UTF8Encoding();
            return UploadFile(encoding.GetBytes(data), pid, fileName);
        }
        private String CurrentDate
        {
            get { return ""; }// DateTime.Now.ToString(DATEFORMAT); 
        }
        private static readonly Dictionary<string, IEnumerable<FedoraAPIM.Datastream>> DATASTREAMCACHE = new Dictionary<string, IEnumerable<FedoraAPIM.Datastream>>();
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
                        DATASTREAMCACHE.Add(pid, streams);
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
        private string GetContentUrl(string pid, string fileName)
        {
            if (String.IsNullOrEmpty(pid) || String.IsNullOrEmpty(fileName)) return "";
            string dsid = fileName.Equals(DUBLINCOREID) ? "DC" : GetDSId(pid, fileName);

            return string.Format(DOWNLOADURL, _BaseUrl, pid, dsid);
        }
        private string FormatContentUrl(string pid, string dsid)
        {
            return string.Format(DOWNLOADURL, _BaseUrl, pid, dsid);
        }
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
        public void DeleteContentObject(ContentObject co)
        {
            //remove the files from fedora
            using (var srv = GetManagementService())
            {
                srv.modifyObject(co.PID, "D", co.Label, "", "");
            }
        }
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
        public string UpdateFile(Stream data, string pid, string fileName, string newfileName = null)
        {
            data.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[data.Length];
            data.Read(buffer, 0, (int)data.Length);
            return UpdateFile(buffer, pid, fileName, newfileName);
        }
    }
}
