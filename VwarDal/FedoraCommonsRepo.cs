using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
using vwarDAL.FedoraAPIA;
using System.Xml.Linq;

namespace vwarDAL
{
    public class FedoraCommonsRepo : IDataRepository
    {
        private readonly System.Net.NetworkCredential _Credantials;
        private const string DUBLINCOREID = "Dublin Core Record for this object";
        private readonly string _BaseUrl;
        private readonly string _AccessUrl;
        private readonly string _ManagementUrl;
        private static readonly string BASECONTENTURL = "{0}objects/{1}/datastreams/{2}/";
        private static readonly string DOWNLOADURL = BASECONTENTURL + "content";
        private static readonly string REVIEWNAMESPACE = "review";

        internal FedoraCommonsRepo(string url, string userName, string password, string access, string management)
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

        public IEnumerable<ContentObject> GetAllContentObjects()
        {
            return QueryContentObjects("pid", "adl:*", ComparisonOperator.has);
        }

        private IEnumerable<ContentObject> QueryContentObjects(string field, string value, ComparisonOperator op, string count = "100000")
        {

            string[] fieldsToSearch = new string[] { "pid" };

            using (var asrv = GetAccessService())
            {
                FieldSearchQuery fsq = new FieldSearchQuery();


                FieldSearchQueryConditions fsqConditions = new FieldSearchQueryConditions();

                Condition c = new Condition();

                Condition c2 = new Condition();
                c2.property = field;
                c2.@operator = op;
                c2.value = value;

                fsqConditions.condition = new Condition[] { c2 };

                fsq.Item = fsqConditions;
                FieldSearchResult results = null;
                try
                {
                    results = asrv.findObjects(fieldsToSearch, count, fsq);
                }
                catch (WebException ex)
                {

                }
                List<ContentObject> cos = new List<ContentObject>();
                if (results != null)
                {

                    foreach (var result in results.resultList)
                    {
                        cos.Add(GetContentObjectById(result.pid, false));
                    }
                }
                return cos;
            }
        }

        public IEnumerable<ContentObject> GetContentObjectsByCollectionName(string collectionName)
        {

            return QueryContentObjects("CollectionName", collectionName, ComparisonOperator.eq);
        }

        public IEnumerable<ContentObject> GetHighestRated(int count)
        {
            var cos = (from c in GetAllContentObjects()
                       orderby c.Reviews.Sum((Review r) => r.Rating) descending
                       select c);
            return cos.Take(count);
        }

        public IEnumerable<ContentObject> GetMostPopular(int count)
        {
            return (from c in GetAllContentObjects()
                    orderby c.Views descending
                    select c).Take(count);
        }

        public IEnumerable<ContentObject> GetRecentlyUpdated(int count)
        {
            return (from c in GetAllContentObjects()
                    orderby c.LastModified descending
                    select c).Take(count);
        }

        public void InsertReview(int rating, string text, string submitterEmail, string contentObjectId)
        {
            Review review = new Review()
            {
                Rating = rating,
                Text = text,
                SubmittedBy = submitterEmail,
                SubmittedDate = DateTime.Now
            };
            using (var srv = GetManagementService())
            {
                contentObjectId = contentObjectId.Replace("~", ":");
                var contentId = srv.getNextPID("1", REVIEWNAMESPACE)[0].Replace(":", "");
                srv.addDatastream(contentObjectId,
                    contentId,
                    new string[0],
                    contentId,
                    true,
                    "text/xml",
                    "",
                    GetContentUrl(contentObjectId, "Dublin Core Record for this object"),
                    "X",
                    "A",
                    "Disabled",
                    "none",
                    "Add Review"
                    );
                string requestURL = String.Format(BASECONTENTURL, _BaseUrl, contentObjectId, contentId);
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        client.Credentials = _Credantials;
                        client.Headers.Add("Content-Type", "text/xml");
                        client.UploadString(requestURL, review.Serialize());
                    }
                }
                catch (WebException exception)
                {

                    var rs = exception.Response.GetResponseStream();
                    using (StreamReader reader = new StreamReader(rs))
                    {
                        var error = reader.ReadToEnd();
                        Console.WriteLine(error);
                    }
                }
            }
        }

        public void UpdateContentObject(ContentObject co)
        {
            co.PID = co.PID.Replace("~", ":");
            if (_Memory.ContainsKey(co.PID))
            {
                _Memory[co.PID] = co;
            }
            var metadataUrl = GetContentUrl(co.PID, DUBLINCOREID);
            using (var srv = GetManagementService())
            {
                srv.modifyObject(co.PID, "A", co.Title, "", "update");
            }
            using (WebClient client = new WebClient())
            {
                client.Credentials = _Credantials;
                var dublicCoreData = client.DownloadString(metadataUrl);
                dublicCoreData = dublicCoreData.Replace("\r", "").Replace("\n", "");
                var match = System.Text.RegularExpressions.Regex.Replace(dublicCoreData, "<ContentObjectMetadata.*</ContentObjectMetadata>", co._Metadata.Serialize().Replace("<?xml version=\"1.0\"?>", "").Trim());
                client.UploadString(metadataUrl.Replace("content", ""), "PUT", match);
            }

        }

        public IEnumerable<ContentObject> GetRecentlyViewed(int count)
        {
            return (from c in GetAllContentObjects()
                    orderby c.LastViewed
                    select c).Take(count);
        }

        public IEnumerable<ContentObject> SearchContentObjects(string searchTerm)
        {
            var svc = GetAccessService();
            FedoraAPIA.FieldSearchQuery query = new FedoraAPIA.FieldSearchQuery();
            query.Item = searchTerm;
            var results = svc.findObjects(new String[] { "pid" }, "5000000", query);
            List<ContentObject> objects = new List<ContentObject>();
            foreach (var result in results.resultList)
            {
                objects.Add(GetContentObjectById(result.pid, false));
            }
            return objects;
        }

        public IEnumerable<ContentObject> GetContentObjectsBySubmitterEmail(string email)
        {
            var co = from c in GetAllContentObjects()
                     where !String.IsNullOrEmpty(c.SubmitterEmail) && c.SubmitterEmail.ToLower().Equals(email.ToLower().Trim())
                     select c;

            return co;

        }

        public IEnumerable<ContentObject> GetContentObjectsByDeveloperName(string developerName)
        {
            //TODO: change to use the generice search provider
            var co = from c in GetAllContentObjects()
                     where !String.IsNullOrEmpty(c.DeveloperName) && c.DeveloperName.ToLower().Equals(developerName.ToLower().Trim())
                     select c;

            return co;

        }

        public IEnumerable<ContentObject> GetContentObjectsBySponsorName(string sponsorName)
        {
            //TODO: change to use the generice search provider
            var co = from c in GetAllContentObjects()
                     where !String.IsNullOrEmpty(c.SponsorName) && c.SponsorName.ToLower().Contains(sponsorName.ToLower().Trim())
                     select c;

            return co;

        }

        public IEnumerable<ContentObject> GetContentObjectsByArtistName(string artistName)
        {
            //TODO: change to use the generice search provider
            var co = from c in GetAllContentObjects()
                     where !String.IsNullOrEmpty(c.ArtistName) && c.ArtistName.ToLower().Contains(artistName.ToLower().Trim())
                     select c;

            return co;


        }

        public IEnumerable<ContentObject> GetContentObjectsByKeyWords(string keyword)
        {
            //TODO: change to use the generice search provider
            var co = from c in GetAllContentObjects()
                     where !String.IsNullOrEmpty(c.Keywords) && c.Keywords.ToLower().Contains(keyword.ToLower().Trim())
                     select c;

            return co;


        }

        public IEnumerable<ContentObject> GetContentObjectsByDescription(string description)
        {
            //TODO: change to use the generice search provider
            var co = from c in GetAllContentObjects()
                     where !String.IsNullOrEmpty(c.Description) && c.Description.Equals(description, StringComparison.InvariantCultureIgnoreCase)
                     select c;

            return co;


        }

        public IEnumerable<ContentObject> GetContentObjectsByTitle(string title)
        {
            //TODO: change to use the generice search provider
            var co = from c in GetAllContentObjects()
                     where c.Title.ToLower().Contains(title.ToLower().Trim())
                     select c;

            return co;


        }

        private Dictionary<String, ContentObject> _Memory = new Dictionary<string, ContentObject>();

        public ContentObject GetContentObjectById(string pid, bool updateViews, bool getReviews = true)
        {
            var co = new ContentObject()
            {
                PID = pid.Replace(':', '~'),
                Reviews = new List<Review>()
            };
            if (_Memory.ContainsKey(co.PID))
            {
                co = _Memory[co.PID];
            }
            else
            {
                using (var svc = GetManagementService())
                {
                    pid = pid.Replace('~', ':');
                    var bytes = svc.getObjectXML(pid);
                    using (WebClient client = new WebClient())
                    {
                        client.Credentials = _Credantials;
                        if (getReviews)
                        {
                            string dateString = DateTime.Now.ToString(/*"yyyy'-'MM'-'dd'Z'"*/);
                            var dataStreams = svc.getDatastreams(pid, dateString, "A");
                            var reviews = from r in dataStreams
                                          where r.ID.StartsWith(REVIEWNAMESPACE, StringComparison.InvariantCultureIgnoreCase)
                                          select r;


                            foreach (var r in reviews)
                            {
                                var url = string.Format(DOWNLOADURL, _BaseUrl, pid, r.ID);
                                var data = client.DownloadString(url);
                                var review = new Review();
                                review.Deserialize(data);
                                co.Reviews.Add(review);
                            }
                        }
                        var finalUrl = GetContentUrl(co.PID, DUBLINCOREID);
                        var dublicCoreData = client.DownloadString(finalUrl);
                        var dublicCoreDocument = new XmlDocument();
                        dublicCoreDocument.LoadXml(dublicCoreData);
                        var coMetaData = ((XmlElement)dublicCoreDocument.FirstChild).GetElementsByTagName("ContentObjectMetadata")[0];
                        co._Metadata = new ContentObjectMetadata();
                        co._Metadata.Deserialize(coMetaData.OuterXml);
                        //bool changed = false;
                        //co.DisplayFile = Path.GetFileName(co.DisplayFile);
                        //if (String.IsNullOrEmpty(co.DisplayFileId) && !String.IsNullOrEmpty(co.Location))
                        //{
                        //    changed = true;
                        //    co.DisplayFileId = GetDSId(co.PID, co.Location);
                        //}
                        //if (String.IsNullOrEmpty(co.ScreenShotId) && !String.IsNullOrEmpty(co.ScreenShot))
                        //{
                        //    changed = true;
                        //    co.ScreenShotId = GetDSId(co.PID, co.ScreenShot);
                        //}
                        //if (changed)
                        //{
                        //    UpdateContentObject(co);
                        //}
                    }

                }
                if (!_Memory.ContainsKey(co.PID))
                {
                    _Memory.Add(co.PID, co);
                }
            }
            if (updateViews)
            {
                co.Views++;
                UpdateContentObject(co);
            }
            return co;
        }

        public void DeleteContentObject(string id)
        {
            using (var srv = GetManagementService())
            {
                var co = GetContentObjectById(id, false);
                srv.modifyObject(id, "D", co.Label, "", "");
            }
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
                var dsId = srv.getNextPID("1", "metadata")[0].Replace(":", "");
                var metadataUrl = GetContentUrl(pid, DUBLINCOREID);
                WebClient client = new WebClient();
                client.Credentials = _Credantials;
                var dublinCoreMetadata = client.DownloadString(metadataUrl);
                var dublinCoreXmlDoc = new XmlDataDocument();
                dublinCoreXmlDoc.LoadXml(dublinCoreMetadata);
                var root = dublinCoreXmlDoc.FirstChild;
                var objectMetadata = co._Metadata.Serialize();
                root.InnerXml += objectMetadata.Replace("<?xml version=\"1.0\"?>", "").Trim();
                metadataUrl = metadataUrl.Replace("/content", "");
                client.UploadString(metadataUrl, "PUT", dublinCoreXmlDoc.OuterXml);
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
        public string UploadFile(Stream data, string pid, string fileName)
        {
            data.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[data.Length];
            data.Read(buffer, 0, (int)data.Length);
            return UploadFile(buffer, pid, fileName);
        }
        public string UploadFile(byte[] data, string pid, string fileName)
        {
            var mimeType = GetMimeType(fileName);
            using (var srv = GetManagementService())
            {
                pid = pid.Replace("~", ":");
                string dsid = srv.getNextPID("1", "content")[0].Replace(":", "");
                var output = srv.addDatastream(pid,
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
                string requestURL = String.Format(BASECONTENTURL, _BaseUrl, pid, output);
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        client.Credentials = _Credantials;
                        client.Headers.Add("Content-Type", mimeType);
                        client.UploadData(requestURL, data);
                    }
                }
                catch (WebException exception)
                {

                    var rs = exception.Response.GetResponseStream();
                    using (StreamReader reader = new StreamReader(rs))
                    {
                        Console.WriteLine(reader.ReadToEnd());
                    }
                }
                return dsid;
            }            
        }
        public string UploadFile(string data, string pid, string fileName)
        {
            if (!File.Exists(data)) return "";
            var mimeType = GetMimeType(fileName);
            using (var srv = GetManagementService())
            {
                pid = pid.Replace("~", ":");
                string dsid = srv.getNextPID("1", "content")[0].Replace(":", "");
                var output = srv.addDatastream(pid,
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
                string requestURL = String.Format(BASECONTENTURL, _BaseUrl, pid, output);
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        client.Credentials = _Credantials;
                        client.Headers.Add("Content-Type", mimeType);
                        client.UploadFile(requestURL, data);
                    }
                }
                catch (WebException exception)
                {

                    var rs = exception.Response.GetResponseStream();
                    using (StreamReader reader = new StreamReader(rs))
                    {
                        Console.WriteLine(reader.ReadToEnd());
                    }
                }
                return dsid;
            }
        }

        public static string GetMimeType(string fileName)
        {
            if (String.IsNullOrEmpty(fileName)) return "";
            string mimeType = "text/plain";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        public void IncrementDownloads(string id)
        {
            ContentObject co = GetContentObjectById(id, false);
            co.Downloads++;
            UpdateContentObject(co);
        }
        private static readonly Dictionary<string, IEnumerable<FedoraAPIM.Datastream>> DATASTREAMCACHE = new Dictionary<string, IEnumerable<FedoraAPIM.Datastream>>();
        private string GetDSId(string pid, string fileName)
        {
            string dsid = "";
            pid = pid.Replace("~", ":");
            using (var srv = GetManagementService())
            {
                IEnumerable<FedoraAPIM.Datastream> streams;
                if (DATASTREAMCACHE.ContainsKey(pid))
                {
                    streams = DATASTREAMCACHE[pid];
                }
                else
                {
                    streams = srv.getDatastreams(pid, DateTime.Now.ToString(), "A"); ;
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
                    streams = srv.getDatastreams(pid, DateTime.Now.ToString(), "A"); ;
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
        public string GetContentUrl(string pid, string fileName)
        {
            if (String.IsNullOrEmpty(pid) || String.IsNullOrEmpty(fileName)) return "";
            pid = pid.Replace("~", ":");
            string dsid = fileName.Equals(DUBLINCOREID) ? "DC" : GetDSId(pid, fileName); ;
            return string.Format(DOWNLOADURL, _BaseUrl, pid, dsid);
        }
        public string FormatContentUrl(string pid, string dsid)
        {
            pid = pid.Replace("~", ":");
            return string.Format(DOWNLOADURL, _BaseUrl, pid, dsid);
        }
        public void UpdateFile(byte[] data, string pid, string fileName)
        {

            if (String.IsNullOrEmpty(pid) || String.IsNullOrEmpty(fileName)) return;
            pid = pid.Replace("~", ":");

            var requestURL = GetContentUrl(pid, fileName);
            requestURL = requestURL.Substring(0, requestURL.LastIndexOf('/'));
            using (WebClient client = new WebClient())
            {
                var mimeType = GetMimeType(fileName);
                client.Credentials = _Credantials;
                client.Headers.Add("Content-Type", mimeType);
                client.UploadData(requestURL, "PUT", data);

            }


        }
        public void RemoveFile(string pid, string fileName)
        {
            pid = pid.Replace("~", ":");
            string dsid = GetDSId(pid, fileName);
            using (var srv = GetManagementService())
            {
                srv.purgeDatastream(pid, dsid, DateTime.Now.ToString(), DateTime.Now.ToString(), "", false);
            }
        }
        public byte[] GetContentFileData(string pid, string fileName)
        {
            var url = GetContentUrl(pid, fileName);
            using (var client = new WebClient())
            {
                client.Credentials = _Credantials;
                if (url != "")
                    return client.DownloadData(url);
                else return new byte[0];
            }
        }
    }
}
