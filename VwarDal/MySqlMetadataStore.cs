using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.IO;

namespace vwarDAL
{
    class MySqlMetadataStore : vwarDAL.IMetadataStore
    {
        private string ConnectionString;
        public MySqlMetadataStore(string connectionString)
        {
            ConnectionString = connectionString;
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
                            co.Keywords = LoadKeywords(conn, co.PID);
                            objects.Add(co);
                        }
                    }
                }
                conn.Close();
                return objects;
            }
        }
        public void UpdateContentObject(ContentObject co)
        {
            using (var conn = new OdbcConnection(ConnectionString))
            {
                conn.Open();
                int id = 0;
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = "{CALL UpdateContentObject(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)}";
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
                SaveKeywords(conn, co, id);
            }
        }
        public ContentObject GetContentObjectById(string pid, bool updateViews, bool getReviews = true, int revision = -1)
        {
            if (String.IsNullOrEmpty(pid))
            {
                return null;
            }
            List<ContentObject> results = new List<ContentObject>();
            ContentObject resultCO = null;
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

                        command.Parameters.AddWithValue("targetpid", pid);
                        //command.Parameters.AddWithValue("pid", pid);
                        using (var result = command.ExecuteReader())
                        {
                            int NumberOfRows = 0;
                            while (result.Read())
                            {
                                NumberOfRows++;
                                var co = new ContentObject()
                                {
                                    PID = pid,
                                    Reviews = new List<Review>()
                                };

                                var properties = co.GetType().GetProperties();
                                foreach (var prop in properties)
                                {
                                    if (prop.PropertyType == typeof(String) && prop.GetValue(co, null) == null)
                                    {
                                        prop.SetValue(co, String.Empty, null);
                                    }
                                }


                                FillContentObjectFromResultSet(co, result);
                                LoadTextureReferences(co, conn);
                                LoadMissingTextures(co, conn);
                                LoadSupportingFiles(co, conn);
                                LoadReviews(co, conn);
                                co.Keywords = LoadKeywords(conn, co.PID);

                                results.Add(co);
                            }


                            ContentObject highest = null;

                            foreach (ContentObject co in results)
                            {
                                co.NumberOfRevisions = NumberOfRows;
                                if (co.Revision == revision)
                                    resultCO = co;
                                else
                                {
                                    if (highest == null || co.Revision > highest.Revision)
                                        highest = co;
                                }
                            }
                            resultCO = highest;
                        }

                    }


                }
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
            return resultCO;
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
        public IEnumerable<ContentObject> GetObjectsWithRange(string query, int count, int start)
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
                            LoadReviews(co, conn);
                            objects.Add(co);
                        }
                    }
                }
                return objects;
            }
        }
        public void AddSupportingFile(ContentObject co, string filename, string description)
        {
            using (var connection = new OdbcConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    //AddMissingTexture(pid,filename,texturetype,uvset)
                    command.CommandText = "{CALL AddSupportingFile(?,?,?)}";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("filename", filename);
                    command.Parameters.AddWithValue("description", description);
                    command.Parameters.AddWithValue("pid", co.PID);

                    var result = command.ExecuteReader();
                    //while (result.Read())
                    //{
                    co.SupportingFiles.Add(new SupportingFile(filename, description));
                    // }
                }
            }
        }
        public bool AddTextureReference(ContentObject co, string filename, string type, int UVset)
        {
            var connection = new OdbcConnection(ConnectionString);
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                //AddMissingTexture(pid,filename,texturetype,uvset)
                command.CommandText = "{CALL AddTextureReference(?,?,?,?,?)}";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("filename", filename);
                command.Parameters.AddWithValue("texturetype", type);
                command.Parameters.AddWithValue("uvset", UVset);
                command.Parameters.AddWithValue("pid", co.PID);
                command.Parameters.AddWithValue("revision", co.Revision);
                var result = command.ExecuteReader();
                co.TextureReferences.Add(new Texture(filename, type, 0));
            }
            return true;
        }
        public bool RemoveMissingTexture(ContentObject co, string filename)
        {
            using (var connection = new OdbcConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "{CALL DeleteMissingTexture(?,?,?)}";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("pid", co.PID);
                    command.Parameters.AddWithValue("filename", filename);
                    command.Parameters.AddWithValue("revision", co.Revision);
                    var result = command.ExecuteReader();
                    List<Texture> remove = new List<Texture>();

                    foreach (Texture t in co.MissingTextures)
                    {
                        if (t.mFilename == filename)
                            remove.Add(t);
                    }
                    foreach (Texture t in remove)
                    {
                        if (t.mFilename == filename)
                            co.MissingTextures.Remove(t);
                    }
                }
            }
            return true;
        }
        public bool RemoveTextureReference(ContentObject co, string filename)
        {
            using (var connection = new OdbcConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "{CALL DeleteTextureReference(?,?,?)}";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("pid", co.PID);
                    command.Parameters.AddWithValue("filename", filename);
                    command.Parameters.AddWithValue("revision", co.Revision);
                    var result = command.ExecuteReader();


                    List<Texture> remove = new List<Texture>();

                    foreach (Texture t in co.TextureReferences)
                    {
                        if (t.mFilename == filename)
                            remove.Add(t);
                    }
                    foreach (Texture t in remove)
                    {
                        if (t.mFilename == filename)
                            co.TextureReferences.Remove(t);
                    }
                }
            }
            return true;
        }
        public bool RemoveSupportingFile(ContentObject co, string filename)
        {
            using (var connection = new OdbcConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "{CALL DeleteSupportingFile(?,?)}";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("pid", co.PID);
                    command.Parameters.AddWithValue("filename", filename);
                    var result = command.ExecuteReader();

                    List<SupportingFile> remove = new List<SupportingFile>();

                    foreach (SupportingFile t in co.SupportingFiles)
                    {
                        if (t.Filename == filename)
                            remove.Add(t);
                    }
                    foreach (SupportingFile t in remove)
                    {
                        if (t.Filename == filename)
                            co.SupportingFiles.Remove(t);
                    }
                }
            }
            return true;
        }
        public bool AddMissingTexture(ContentObject co, string filename, string type, int UVset)
        {
            using (var connection = new OdbcConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    //AddMissingTexture(pid,filename,texturetype,uvset)
                    command.CommandText = "{CALL AddMissingTexture(?,?,?,?,?)}";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("filename", filename);
                    command.Parameters.AddWithValue("texturetype", type);
                    command.Parameters.AddWithValue("uvset", UVset);
                    command.Parameters.AddWithValue("pid", co.PID);
                    command.Parameters.AddWithValue("revision", co.Revision);
                    var result = command.ExecuteReader();
                    co.MissingTextures.Add(new Texture(filename, type, 0));
                }
            }
            return true;
        }
        public void InsertContentRevision(ContentObject co)
        {
            using (var conn = new OdbcConnection(ConnectionString))
            {
                int id = 0;

                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = "{CALL InsertContentObject(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)}";
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
                SaveKeywords(conn, co, id);
            }
        }
        public void InsertContentObject(ContentObject co)
        {
            int id = 0;
            using (var conn = new OdbcConnection(ConnectionString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = "{CALL InsertContentObject(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)}";
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
                SaveKeywords(conn, co, id);
            }

        }
        public void IncrementDownloads(string id)
        {
            using (var secondConnection = new OdbcConnection(ConnectionString))
            {
                secondConnection.Open();
                using (var command = secondConnection.CreateCommand())
                {
                    command.CommandText = "{CALL IncrementDownloads(?)}";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("targetpid", id);
                    command.ExecuteNonQuery();
                }
            }
        }
        public void DeleteContentObject(ContentObject co)
        {
            using (var conn = new OdbcConnection(ConnectionString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = "{CALL DeleteContentObject(?)}";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("targetpid", System.Data.Odbc.OdbcType.VarChar, 45).Value = co.PID;
                    command.ExecuteNonQuery();
                }
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
                co.Revision = Convert.ToInt32(resultSet["Revision"].ToString());
                var RequiresResubmit = resultSet["requireresubmit"].ToString();
                var RequiresResubmitValue = int.Parse(RequiresResubmit);
                co.RequireResubmit = RequiresResubmitValue != 0;
                co.OriginalFileName = resultSet["OriginalFileName"].ToString();
                co.OriginalFileId = resultSet["OriginalFileId"].ToString();

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
                co.Description = resultSet["Description"].ToString();
                co.Title = resultSet["Title"].ToString();
                co.ScreenShot = resultSet["ScreenShotFileName"].ToString();
                co.ScreenShotId = resultSet["ScreenShotFileId"].ToString();
            }
            catch
            {
            }
        }
        private void LoadTextureReferences(ContentObject co, OdbcConnection connection)
        {

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "{CALL GetTextureReferences(?,?)}";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("pid", co.PID);
                command.Parameters.AddWithValue("revision", co.Revision);
                var result = command.ExecuteReader();
                while (result.Read())
                {
                    co.TextureReferences.Add(new Texture(result["Filename"].ToString(), result["Type"].ToString(), int.Parse(result["UVSet"].ToString())));
                }
            }

        }
        private void LoadMissingTextures(ContentObject co, OdbcConnection connection)
        {

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "{CALL GetMissingTextures(?,?)}";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("pid", co.PID);
                command.Parameters.AddWithValue("revision", co.Revision);
                var result = command.ExecuteReader();
                while (result.Read())
                {
                    co.MissingTextures.Add(new Texture(result["Filename"].ToString(), result["Type"].ToString(), int.Parse(result["UVSet"].ToString())));
                }
            }

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
            command.Parameters.AddWithValue("newRevisionNumber", co.Revision);


            command.Parameters.AddWithValue("newRequireResubmit", co.RequireResubmit);
            command.Parameters.AddWithValue("newOriginalFileName", co.OriginalFileName);
            command.Parameters.AddWithValue("newOriginalFileId", co.OriginalFileId);
        }
        private void SaveKeywords(OdbcConnection conn, ContentObject co, int id)
        {
            char[] delimiters = new char[] { ',' };
            string[] words = co.Keywords.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            string[] oldKeywords = LoadKeywords(conn, co.PID).Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            List<String> wordsToSave = new List<string>();
            foreach (var word in words)
            {
                bool shouldSave = true;
                foreach (var oldWord in oldKeywords)
                {
                    if (oldWord.Equals(word, StringComparison.InvariantCultureIgnoreCase))
                    {
                        shouldSave = false;
                        break;
                    }
                }
                if (shouldSave)
                {
                    wordsToSave.Add(word);
                }
            }
            words = wordsToSave.ToArray();
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
        private String LoadKeywords(OdbcConnection conn, string PID)
        {
            List<String> keywords = new List<string>();
            using (var command = conn.CreateCommand())
            {
                command.CommandText = "{CALL GetKeywords(?)}";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("targetPid", PID);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    keywords.Add(reader["Keyword"].ToString());
                }
            }
            return String.Join(",", keywords.ToArray());
        }
        private bool LoadSupportingFiles(ContentObject co, OdbcConnection connection)
        {

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "{CALL GetSupportingFiles(?)}";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("pid", co.PID);
                var result = command.ExecuteReader();
                while (result.Read())
                {
                    co.SupportingFiles.Add(new SupportingFile(result["Filename"].ToString(), result["Description"].ToString()));
                }
            }
            return true;
        }
        
        
    }
}
