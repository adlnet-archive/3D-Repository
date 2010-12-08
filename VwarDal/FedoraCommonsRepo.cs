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
using System.Data.Odbc;
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
        private const string DATEFORMAT = "yyyy'-'MM'-'dd'Z'";
        private string ConnectionString;
        internal FedoraCommonsRepo(string url, string userName, string password, string access, string management, string connectionString)
        {
            ConnectionString = connectionString;
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
            using (System.Data.Odbc.OdbcConnection conn = new System.Data.Odbc.OdbcConnection(ConnectionString))
            {
                List<ContentObject> objects = new List<ContentObject>();
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = "{CALL GetAllContentObjects()}";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var resultSet = command.ExecuteReader())
                    {
                        while (resultSet.Read())
                        {
                            var co = new ContentObject();

                            FillContentObjectFromResultSet(co, resultSet);
                            LoadReviews(co, conn);
                            objects.Add(co);
                        }
                    }                   
                }
                conn.Close();
                return objects;
            }

        }
        private void FillContentObjectFromResultSet(ContentObject co, OdbcDataReader resultSet)
        {
            try
            {

                co.PID = resultSet["PID"].ToString();
                co.ArtistName = resultSet["ArtistName"].ToString();
                co.AssetType = resultSet["AssetType"].ToString();
                co.CreativeCommonsLicenseURL = resultSet["CreativeCommonsLicenseURL"].ToString();
                co.Description = resultSet["Description"].ToString();
                co.DeveloperLogoImageFileName = resultSet["DeveloperLogoFileName"].ToString();
                co.DeveloperLogoImageFileNameId = resultSet["DeveloperLogoFileId"].ToString();
                co.DeveloperName = resultSet["DeveloperName"].ToString();
                co.DisplayFile = resultSet["DisplayFileName"].ToString();
                co.DisplayFileId = resultSet["DisplayFileId"].ToString();
                co.Downloads = int.Parse(resultSet["Downloads"].ToString());
                co.Format = resultSet["Format"].ToString();
                co.IntentionOfTexture = resultSet["IntentionOfTexture"].ToString();
                DateTime temp;
                if (DateTime.TryParse(resultSet["LastModified"].ToString(), out temp))
                {
                    co.LastModified = temp;
                }
                if (DateTime.TryParse(resultSet["LastViewed"].ToString(), out temp))
                {
                    co.LastViewed = temp;
                }
                co.Location = resultSet["ContentFileName"].ToString();
                co.MoreInformationURL = resultSet["MoreInfoUrl"].ToString();
                co.NumPolygons = int.Parse(resultSet["NumPolygons"].ToString());
                co.NumTextures = int.Parse(resultSet["NumTextures"].ToString());

                co.ScreenShot = resultSet["ScreenShotFileName"].ToString();
                co.ScreenShotId = resultSet["ScreenShotFileId"].ToString();
                co.SponsorLogoImageFileName = resultSet["SponsorLogoFileName"].ToString();
                co.SponsorLogoImageFileNameId = resultSet["SponsorLogoFileId"].ToString();
                co.SponsorName = resultSet["SponsorName"].ToString();
                co.SubmitterEmail = resultSet["Submitter"].ToString();
                co.Title = resultSet["Title"].ToString();
                co.UnitScale = resultSet["UnitScale"].ToString();
                co.UpAxis = resultSet["UpAxis"].ToString();
                if (DateTime.TryParse(resultSet["UploadedDate"].ToString(), out temp))
                {
                    co.UploadedDate = temp;
                }
                co.UVCoordinateChannel = resultSet["UVCoordinateChannel"].ToString();
                co.Views = int.Parse(resultSet["Views"].ToString());
            }
            catch
            {
            }
        }
        private void FillContentObjectLightLoad(ContentObject co, OdbcDataReader resultSet)
        {
            try
            {

                co.PID = resultSet["PID"].ToString();
                co.Title = resultSet["Title"].ToString();
                co.ScreenShot = resultSet["ScreenShotFileName"].ToString();
                co.ScreenShotId = resultSet["ScreenShotFileId"].ToString();
            }
            catch
            {
            }
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

        public IEnumerable<ContentObject> GetHighestRated(int count, int start = 0)
        {

            return GetObjectsWithRange("{CALL GetHighestRated(?,?)}", count, start);

        }

        public IEnumerable<ContentObject> GetMostPopular(int count, int start = 0)
        {
            return GetObjectsWithRange("{CALL GetMostPopularContentObjects(?,?)}", count, start);
        }
        private IEnumerable<ContentObject> GetObjectsWithRange(string query, int count, int start)
        {
            using (System.Data.Odbc.OdbcConnection conn = new System.Data.Odbc.OdbcConnection(ConnectionString))
            {
                List<ContentObject> objects = new List<ContentObject>();
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("s", start);
                    command.Parameters.AddWithValue("length", count);
                    using (var resultSet = command.ExecuteReader())
                    {
                        while (resultSet.Read())
                        {
                            var co = new ContentObject();

                            FillContentObjectLightLoad(co, resultSet);
                            LoadReviews(co,conn);
                            objects.Add(co);
                        }
                    }
                }
                return objects;
            }
        }
        public IEnumerable<ContentObject> GetRecentlyUpdated(int count, int start = 0)
        {
            return GetObjectsWithRange("{CALL GetMostRecentlyUpdated(?,?)}", count, start);
        }

        public void InsertReview(int rating, string text, string submitterEmail, string contentObjectId)
        {


            using (var connection = new OdbcConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "{CALL InsertReview(?,?,?,?);}";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("newrating", rating);
                    command.Parameters.AddWithValue("newtext", text);
                    command.Parameters.AddWithValue("newsubmittedby", submitterEmail);
                    command.Parameters.AddWithValue("newcontentobjectid", contentObjectId);
                    var i = command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateContentObject(ContentObject co)
        {
            /*if (_Memory.ContainsKey(co.PID))
            {
                _Memory[co.PID] = co;
            }*/
            using (var conn = new OdbcConnection(ConnectionString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = "{CALL UpdateContentObject(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)}";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var properties = co.GetType().GetProperties();
                    foreach (var prop in properties)
                    {
                        if (prop.PropertyType == typeof(String) && prop.GetValue(co, null) == null)
                        {
                            prop.SetValue(co, String.Empty, null);
                        }
                    }
                    FillCommandFromContentObject(co, command);
                    var result = command.ExecuteNonQuery();

                }
            }
            /*var metadataUrl = GetContentUrl(co.PID, DUBLINCOREID);
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
            }*/

        }

        public IEnumerable<ContentObject> GetRecentlyViewed(int count, int start = 0)
        {
            return GetObjectsWithRange("{CALL GetMostRecentlyViewed(?,?)}", count, start);
        }

        private bool SearchFunction(ContentObject co, string searchTerm)
        {
            bool isGood = false;
            if (!String.IsNullOrEmpty(co.Title) && co.Title.ToLower().Contains(searchTerm.ToLower()))
            {
                isGood = true;
            }
            else if (!String.IsNullOrEmpty(co.Description) && co.Description.ToLower().Contains(searchTerm.ToLower()))
            {
                isGood = true;
            }
            else if (!String.IsNullOrEmpty(co.Keywords) && co.Keywords.ToLower().Contains(searchTerm.ToLower()))
            {
                isGood = true;
            }
            return isGood;
        }
        public IEnumerable<ContentObject> SearchContentObjects(string searchTerm)
        {
            var items = from co in GetAllContentObjects()
                        where SearchFunction(co, searchTerm)
                        select co;
            return items;
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

        //private Dictionary<String, ContentObject> _Memory = new Dictionary<string, ContentObject>();

        public ContentObject GetContentObjectById(string pid, bool updateViews, bool getReviews = true)
        {
            if (String.IsNullOrEmpty(pid))
            {
                return new ContentObject();
            }
            var co = new ContentObject()
            {
                PID = pid,
                Reviews = new List<Review>()
            };
            if (false)//(_Memory.ContainsKey(co.PID))
            {
                //co = _Memory[co.PID];
            }
            else
            {
                using (var conn = new OdbcConnection(ConnectionString))
                {
                    conn.Open();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandText = "{CALL GetContentObject(?);}";
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        var properties = co.GetType().GetProperties();
                        foreach (var prop in properties)
                        {
                            if (prop.PropertyType == typeof(String) && prop.GetValue(co, null) == null)
                            {
                                prop.SetValue(co, String.Empty, null);
                            }
                        }
                        command.Parameters.AddWithValue("targetpid", pid);
                        //command.Parameters.AddWithValue("pid", pid);
                        using (var result = command.ExecuteReader())
                        {
                            while (result.Read())
                            {
                                FillContentObjectFromResultSet(co, result);
                            }
                        }
                        LoadReviews(co,conn);

                    }

                }
                /*if (!_Memory.ContainsKey(co.PID))
                {
                    _Memory.Add(co.PID, co);
                }*/
            }
            if (updateViews)
            {
                using (var secondConnection = new OdbcConnection(ConnectionString))
                {
                    secondConnection.Open();
                    using (var command = secondConnection.CreateCommand())
                    {
                        command.CommandText = "{CALL IncrementViews(?)}";
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("targetpid", pid);
                        command.ExecuteNonQuery();
                    }
                }
            }
            return co;
        }
        private void LoadReviews(ContentObject co, OdbcConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "{CALL GetReviews(?)}";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("pid", co.PID);
                var result = command.ExecuteReader();
                while (result.Read())
                {
                    co.Reviews.Add(new Review()
                    {
                        Rating = int.Parse(result["Rating"].ToString()),
                        Text = result["Text"].ToString(),
                        SubmittedBy = result["SubmittedBy"].ToString(),
                    });
                }


            }
        }
        public void DeleteContentObject(string id)
        {
            using (var srv = GetManagementService())
            {
                var co = GetContentObjectById(id, false);
                srv.modifyObject(id, "D", co.Label, "", "");
            }
        }
        private void FillCommandFromContentObject(ContentObject co, OdbcCommand command)
        {
            command.Parameters.AddWithValue("newpid", co.PID);
            command.Parameters.AddWithValue("newtitle", co.Title);
            command.Parameters.AddWithValue("newcontentfilename", co.Location);
            command.Parameters.AddWithValue("newcontentfileid", co.DisplayFileId);
            command.Parameters.AddWithValue("newsubmitter", co.SubmitterEmail);
            command.Parameters.AddWithValue("newcreativecommonslicenseurl", co.CreativeCommonsLicenseURL);
            command.Parameters.AddWithValue("newdescription", co.Description);
            command.Parameters.AddWithValue("newscreenshotfilename", co.ScreenShot);
            command.Parameters.AddWithValue("screenshotfileid", co.ScreenShotId);
            command.Parameters.AddWithValue("newsponsorlogofilename", co.SponsorLogoImageFileName);
            command.Parameters.AddWithValue("newsponsorlogofileid", co.SponsorLogoImageFileNameId);
            command.Parameters.AddWithValue("newdeveloperlogofilename", co.DeveloperLogoImageFileName);
            command.Parameters.AddWithValue("newdeveloperlogofileid", co.DeveloperLogoImageFileNameId);
            command.Parameters.AddWithValue("newassettype", co.AssetType);
            command.Parameters.AddWithValue("newdisplayfilename", co.DisplayFile);
            command.Parameters.AddWithValue("newdisplayfileid", co.DisplayFileId);
            command.Parameters.AddWithValue("newmoreinfourl", co.MoreInformationURL);
            command.Parameters.AddWithValue("newdevelopername", co.DeveloperName);
            command.Parameters.AddWithValue("newsponsorname", co.SponsorName);
            command.Parameters.AddWithValue("newartistname", co.ArtistName);
            command.Parameters.AddWithValue("newunitscale", co.UnitScale);
            command.Parameters.AddWithValue("newupaxis", co.UpAxis);
            command.Parameters.AddWithValue("newuvcoordinatechannel", co.UVCoordinateChannel);
            command.Parameters.AddWithValue("newintentionoftexture", co.IntentionOfTexture);
            command.Parameters.AddWithValue("newformat", co.Format);
            command.Parameters.AddWithValue("newnumpolys", co.NumPolygons);
            command.Parameters.AddWithValue("newNumTextures", co.NumTextures);

        }
        public void InsertContentObject(ContentObject co)
        {
            int id = 0;
            using (var srv = GetManagementService())
            {
                var pid = string.IsNullOrEmpty(co.PID) ? srv.getNextPID("1", "adl")[0] : co.PID;
                co.PID = pid;
                var dataObject = CreateDigitalObject(co);
                var data = SerializeObject(dataObject);
                srv.ingest(data, "info:fedora/fedora-system:FOXML-1.1", "add file");
                using (var conn = new OdbcConnection(ConnectionString))
                {
                    conn.Open();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandText = "{CALL InsertContentObject(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)}";
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        var properties = co.GetType().GetProperties();
                        foreach (var prop in properties)
                        {
                            if (prop.PropertyType == typeof(String) && prop.GetValue(co, null) == null)
                            {
                                prop.SetValue(co, String.Empty, null);
                            }
                        }
                        FillCommandFromContentObject(co, command);
                        id = int.Parse(command.ExecuteScalar().ToString());

                    }
                }
                using (var conn = new OdbcConnection(ConnectionString))
                {
                    conn.Open();
                    char[] delimiters = new char[] { ',' };
                    string[] words = co.Keywords.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var keyword in words)
                    {
                        int keywordId = 0;
                        using (var command = conn.CreateCommand())
                        {
                            command.CommandText = "{CALL InsertKeyword(?)}";
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("newKeyword", keyword);
                            keywordId = int.Parse(command.ExecuteScalar().ToString());
                        }
                        using (var cm = conn.CreateCommand())
                        {
                            cm.CommandText = "{CALL AssociateKeyword(?,?)}";
                            cm.CommandType = System.Data.CommandType.StoredProcedure;
                            cm.Parameters.AddWithValue("coid", id);
                            cm.Parameters.AddWithValue("kid", keywordId);
                            cm.ExecuteNonQuery();
                        }
                    }
                }
                /*if (!_Memory.ContainsKey(co.PID))
                {
                    _Memory.Add(co.PID, co);
                }*/
                /*var dsId = srv.getNextPID("1", "metadata")[0].Replace(":", "");
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
                client.UploadString(metadataUrl, "PUT", dublinCoreXmlDoc.OuterXml);*/
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
            if (pid.Contains("~"))
            {
                return "";
            }
            using (var srv = GetManagementService())
            {
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
        public string GetContentUrl(string pid, string fileName)
        {
            if (String.IsNullOrEmpty(pid) || String.IsNullOrEmpty(fileName)) return "";
            string dsid = fileName.Equals(DUBLINCOREID) ? "DC" : GetDSId(pid, fileName); ;
            return string.Format(DOWNLOADURL, _BaseUrl, pid, dsid);
        }
        public string FormatContentUrl(string pid, string dsid)
        {
            return string.Format(DOWNLOADURL, _BaseUrl, pid, dsid);
        }
        public void UpdateFile(byte[] data, string pid, string fileName, string newFileName = null)
        {
            var mimeType = GetMimeType(newFileName);
            if (String.IsNullOrEmpty(pid) || String.IsNullOrEmpty(fileName)) return;
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


        }
        public void RemoveFile(string pid, string fileName)
        {
            string dsid = GetDSId(pid, fileName);
            using (var srv = GetManagementService())
            {
                srv.purgeDatastream(pid, dsid, CurrentDate, CurrentDate, "", false);
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
