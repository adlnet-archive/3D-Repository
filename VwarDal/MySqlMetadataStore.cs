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
using System.Data.Odbc;
using System.IO;

namespace vwarDAL
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    class MySqlMetadataStore : vwarDAL.IMetadataStore
    {
        /// <summary>
        /// 
        /// </summary>
        private string ConnectionString;
        private System.Data.Odbc.OdbcConnection mConnection;
        private int _TotalObjects = -1; 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public MySqlMetadataStore(string connectionString)
        {
            ConnectionString = connectionString;
            mConnection = new OdbcConnection(ConnectionString);
        }
         ~MySqlMetadataStore()
        {
            KillODBCConnection(mConnection);
        }
         public void Dispose()
         {
             KillODBCConnection(mConnection);
             mConnection = null;
         }
        private System.Data.Odbc.OdbcConnection GetConnection()
        {
            if (mConnection == null)
                mConnection = new OdbcConnection(ConnectionString);
            if (mConnection.State == System.Data.ConnectionState.Closed)
                mConnection.Open();
            return mConnection;
        }
        static public bool KillODBCConnection(System.Data.Odbc.OdbcConnection myConn)
        {
            if (myConn != null)
            {
                if (myConn.State == System.Data.ConnectionState.Closed)
                    return false;

                try
                {
                    string strSQL = "kill connection_id()";
                    System.Data.Odbc.OdbcCommand myCmd = new System.Data.Odbc.OdbcCommand(strSQL, myConn);
                    myCmd.CommandText = strSQL;
                  
                   myCmd.ExecuteNonQuery();
                   

                }catch(Exception ex)
                {
                }
              
            }

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContentObject> GetAllContentObjects()
        {
            System.Data.Odbc.OdbcConnection conn = GetConnection();
            {
                List<ContentObject> objects = new List<ContentObject>();
                
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
                
                return objects;
            }
        }
        public IEnumerable<ContentObject> GetAllContentObjects(string UserName)
        {
            System.Data.Odbc.OdbcConnection conn = GetConnection();
            {
                List<ContentObject> objects = new List<ContentObject>();
                
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = "{CALL GetAllContentObjectsVisibleToUser(?)}";
                    command.Parameters.AddWithValue("uname", UserName);
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
                
                return objects;
            }
        }

        public string[] GetMostPopularTags()
        {
            string[] result = new string[25];
            for (int i = 0; i < 25; i++)
                result[i] = "Null";
            //SELECT *, count(*) as ct FROM `3dr`.`associatedkeywords`  inner join (select * from 3dr.keywords) as r on associatedkeywords.keywordid = r.id group by keyword order by ct desc limit 25
            System.Data.Odbc.OdbcConnection conn = GetConnection();
            {
                List<ContentObject> objects = new List<ContentObject>();

                using (var command = conn.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "{SELECT *, count(*) as ct FROM `3dr`.`associatedkeywords`  inner join (select * from 3dr.keywords) as r on associatedkeywords.keywordid = r.id group by keyword order by ct desc limit 25}";
                   
                    using (var resultSet = command.ExecuteReader())
                    {
                        int i = 0;
                        while (resultSet.Read())
                        {
                            result[i] = resultSet["keyword"].ToString() + "." + resultSet["ct"].ToString();
                            i++;
                        }
                    }
                }

                return result;
            }
        }
        public string[] GetMostPopularDevelopers()
        {
            string[] result = new string[15];
            for (int i = 0; i < 15; i++)
                result[i] = "Null";
            System.Data.Odbc.OdbcConnection conn = GetConnection();
            {
                List<ContentObject> objects = new List<ContentObject>();

                using (var command = conn.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "{Select sponsorname, count(sponsorname) as ct from contentobjects where sponsorname != ''   group by sponsorname union Select developername, count(developername) as ct from contentobjects where developername != '' group by developername union Select artistname, count(artistname) as ct from contentobjects where artistname != ''  group by artistname order by ct desc limit 15}";

                    using (var resultSet = command.ExecuteReader())
                    {
                        int i = 0;
                        while (resultSet.Read())
                        {
                            result[i] = resultSet["sponsorname"].ToString();
                            result[i] += " (" + resultSet["ct"].ToString() + ")";
                            i++;
                        }
                    }
                }

                return result;
            }
        }
        public IEnumerable<ContentObject> GetContentObjectsByField(string field, string value, string identity)
        {
            System.Data.Odbc.OdbcConnection conn = GetConnection();
            {
                List<ContentObject> objects = new List<ContentObject>();
                
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "{CALL GetContentObjectsByField(?,?,?)}";
                    command.Parameters.AddWithValue("field", field);
                    command.Parameters.AddWithValue("val", value);
                    command.Parameters.AddWithValue("uname", identity);

                    using (var resultSet = command.ExecuteReader())
                    {
                        while (resultSet.Read())
                        {
                            var co = new ContentObject();
                            FillContentObjectLightLoad(co, resultSet);
                            objects.Add(co);
                        }
                    }
                }
                
                return objects;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        public void UpdateContentObject(ContentObject co)
        {
            System.Data.Odbc.OdbcConnection conn = GetConnection();
            {
                
                int id = 0;
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = "{CALL UpdateContentObject(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?); }";
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="updateViews"></param>
        /// <param name="getReviews"></param>
        /// <param name="revision"></param>
        /// <returns></returns>
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
                System.Data.Odbc.OdbcConnection conn = GetConnection();
                {
                    
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
                            if (results.Count > 0)
                            {
                                if (revision == -1)
                                {
                                    highest = (from r in results
                                               orderby r.Revision descending
                                               select r).First();
                                }
                                else
                                {

                                    highest = (from r in results
                                               where r.Revision == revision
                                               select r).First();

                                }
                                resultCO = highest;
                            }
                            else
                                return null;
                        }
                        
                    }
                }
            }
            if (updateViews)
            {
                System.Data.Odbc.OdbcConnection secondConnection = GetConnection();
                {
                    
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rating"></param>
        /// <param name="text"></param>
        /// <param name="submitterEmail"></param>
        /// <param name="contentObjectId"></param>
        public void InsertReview(int rating, string text, string submitterEmail, string contentObjectId)
        {
            System.Data.Odbc.OdbcConnection connection = GetConnection();
            {
                
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="count"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public IEnumerable<ContentObject> GetObjectsWithRange(string query, int count, int start, SortOrder order, string username)
        {
            List<ContentObject> objects = new List<ContentObject>();
            System.Data.Odbc.OdbcConnection conn = GetConnection();
            {
                
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("s", start);
                    command.Parameters.AddWithValue("length", count);
                    command.Parameters.AddWithValue("sortOrder", (order == SortOrder.Descending) ? "DESC" : "ASC");
                    command.Parameters.AddWithValue("uname", username);
                    
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
                if(_TotalObjects < 0)
                    setContentObjectCount(conn, username);

                
            }

            return objects;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="filename"></param>
        /// <param name="description"></param>
        public void AddSupportingFile(ContentObject co, string filename, string description, string dsid)
        {
            System.Data.Odbc.OdbcConnection connection = GetConnection();
            {
                
                using (var command = connection.CreateCommand())
                {
                    //AddMissingTexture(pid,filename,texturetype,uvset)
                    command.CommandText = "{CALL AddSupportingFile(?,?,?,?)}";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("newfilename", filename);
                    command.Parameters.AddWithValue("newdescription", description);
                    command.Parameters.AddWithValue("newcontentobjectid", co.PID);
                    command.Parameters.AddWithValue("newdsid", dsid);

                    var result = command.ExecuteReader();
                    //while (result.Read())
                    //{
                    co.SupportingFiles.Add(new SupportingFile(filename, description,dsid));
                    // }
                    
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="filename"></param>
        /// <param name="type"></param>
        /// <param name="UVset"></param>
        /// <returns></returns>
        public bool AddTextureReference(ContentObject co, string filename, string type, int UVset)
        {
            System.Data.Odbc.OdbcConnection connection = GetConnection();
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool RemoveMissingTexture(ContentObject co, string filename)
        {
            System.Data.Odbc.OdbcConnection connection = GetConnection();
            {
               
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public bool RemoveKeyword(ContentObject co, string keyword)
        {
            bool result;
            System.Data.Odbc.OdbcConnection connection = GetConnection();
            {
               
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "{CALL RemoveKeyword(?,?)}";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("pid", co.PID);
                    command.Parameters.AddWithValue("keyword", keyword);
                    Boolean.TryParse(command.ExecuteReader().ToString(), out result);
                    if (result == null)
                    {
                        result = false;
                    }
                }
                
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <returns></returns>
        public bool RemoveAllKeywords(ContentObject co)
        {
            bool result;
            System.Data.Odbc.OdbcConnection connection = GetConnection();
            {
                
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "{CALL RemoveAllKeywords(?)}";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("pid", co.PID);
                    Boolean.TryParse(command.ExecuteReader().ToString(), out result);
                    if (result == null)
                    {
                        result = false;
                    }
                }
                
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool RemoveTextureReference(ContentObject co, string filename)
        {
            System.Data.Odbc.OdbcConnection connection = GetConnection();
            {
                
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool RemoveSupportingFile(ContentObject co, string filename)
        {
            System.Data.Odbc.OdbcConnection connection = GetConnection();
            {
                
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="filename"></param>
        /// <param name="type"></param>
        /// <param name="UVset"></param>
        /// <returns></returns>
        public bool AddMissingTexture(ContentObject co, string filename, string type, int UVset)
        {
            System.Data.Odbc.OdbcConnection connection = GetConnection();
            {
                
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        public void InsertContentRevision(ContentObject co)
        {
            System.Data.Odbc.OdbcConnection conn = GetConnection();
            {
                int id = 0;

                
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = "{CALL InsertContentObject(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?); }";
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        public void InsertContentObject(ContentObject co)
        {
            int id = 0;
            System.Data.Odbc.OdbcConnection conn = GetConnection();
            {
                
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = "{CALL InsertContentObject(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?); }";
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void IncrementDownloads(string id)
        {
            System.Data.Odbc.OdbcConnection connection = GetConnection();
            {
                
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "{CALL IncrementDownloads(?)}";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("targetpid", id);
                    command.ExecuteNonQuery();
                }
              
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        public void DeleteContentObject(ContentObject co)
        {
            System.Data.Odbc.OdbcConnection conn = GetConnection();
            {
                
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = "{CALL DeleteContentObject(?)}";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("targetpid", System.Data.Odbc.OdbcType.VarChar, 45).Value = co.PID;
                    command.ExecuteNonQuery();
                }
                
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="resultSet"></param>
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
                co.Thumbnail = resultSet["ThumbnailFileName"].ToString();
                co.ThumbnailId = resultSet["ThumbnailFileId"].ToString();
                co.UnitScale = resultSet["UnitScale"].ToString();
                co.UpAxis = resultSet["UpAxis"].ToString();
                if (DateTime.TryParse(resultSet["UploadedDate"].ToString(), out temp))
                {
                    co.UploadedDate = temp;
                }
                co.UVCoordinateChannel = resultSet["UVCoordinateChannel"].ToString();
                co.Views = int.Parse(resultSet["Views"].ToString());
                co.Revision = Convert.ToInt32(resultSet["Revision"].ToString());
                var RequiresResubmit = resultSet["requiressubmit"].ToString();
                var RequiresResubmitValue = int.Parse(RequiresResubmit);
                co.RequireResubmit = RequiresResubmitValue != 0;
                co.OriginalFileName = resultSet["OriginalFileName"].ToString();
                co.OriginalFileId = resultSet["OriginalFileId"].ToString();


                co.Distribution_Grade = (DistributionGrade)Enum.Parse(typeof(DistributionGrade), resultSet["Distribution_Grade"].ToString());
                co.Distribution_Regulation = resultSet["Distribution_Regulation"].ToString();
                co.Distribution_Determination_Date = DateTime.Parse(resultSet["Distribution_Determination_Date"].ToString());
                co.Distribution_Contolling_Office = resultSet["Distribution_Contolling_Office"].ToString();
                co.Distribution_Reason = resultSet["Distribution_Reason"].ToString();

            }
            catch
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="resultSet"></param>
        private void FillContentObjectLightLoad(ContentObject co, OdbcDataReader resultSet)
        {
            try
            {
                co.PID = resultSet["PID"].ToString();
                co.Description = resultSet["Description"].ToString();
                co.Title = resultSet["Title"].ToString();
                //co.ScreenShot = resultSet["ScreenShotFileName"].ToString();
                //co.ScreenShotId = resultSet["ScreenShotFileId"].ToString();
                co.Views = int.Parse(resultSet["Views"].ToString());
                //co.ThumbnailId = resultSet["ThumbnailFileId"].ToString();
                //co.Thumbnail = resultSet["ThumbnailFileName"].ToString();
            }
            catch
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="connection"></param>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="connection"></param>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="connection"></param>
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
                        SubmittedDate = DateTime.Parse(result["SubmittedDate"].ToString())
                    });
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="command"></param>
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
            command.Parameters.AddWithValue("newscreenshotfileid", co.ScreenShotId);
            command.Parameters.AddWithValue("newthumbnailfilename", co.Thumbnail);
            command.Parameters.AddWithValue("newthumbnailfileid", co.ThumbnailId);
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
            command.Parameters.AddWithValue("newenabled", co.Enabled);
            command.Parameters.AddWithValue("newready", co.Ready);
            command.Parameters.AddWithValue("newOriginalFileName", co.OriginalFileName);
            command.Parameters.AddWithValue("newOriginalFileId", co.OriginalFileId);
            command.Parameters.AddWithValue("newDistribution_Grade", co.Distribution_Grade);
            command.Parameters.AddWithValue("newDistribution_Regulation", co.Distribution_Regulation);
            command.Parameters.AddWithValue("newDistribution_Determination_Date", co.Distribution_Determination_Date);
            command.Parameters.AddWithValue("newDistribution_Contolling_Office", co.Distribution_Contolling_Office);
            command.Parameters.AddWithValue("newDistribution_Reason", co.Distribution_Reason);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="co"></param>
        /// <param name="id"></param>
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
                    wordsToSave.Add(word.Trim().ToLower());
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="PID"></param>
        /// <returns></returns>
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
                    keywords.Add(reader["Keyword"].ToString().Trim().ToLower());
                }
            }
            return String.Join(",", keywords.ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
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
                    co.SupportingFiles.Add(new SupportingFile(result["Filename"].ToString(), result["Description"].ToString(), result["dsid"].ToString()));
                }
            }
            return true;
        }


        public IEnumerable<ContentObject> GetContentObjectsByKeywords(string keywords, string identity)
        {
            //We must transform the list into something MySQL finds acceptible in its syntax
            char[] delimiters = new char[] { ',' };
            string[] list = keywords.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            string escapeTemplate = "'{0}'";

            //Add quotes around each of the list items, while also escaping any existing quotes
            for (int i = 0; i < list.Length; i++)
                list[i] = String.Format(escapeTemplate, list[i].Replace("'", "\'")); 

            keywords = String.Join(",", list);

            System.Data.Odbc.OdbcConnection conn = GetConnection();
            {
                List<ContentObject> objects = new List<ContentObject>();
                
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "{CALL GetContentObjectsByKeywords(?,?)}";
                    cmd.Parameters.AddWithValue("keylist", keywords);
                    cmd.Parameters.AddWithValue("uname", identity);
                    using (var results = cmd.ExecuteReader())
                    {
                        while (results.HasRows && results.Read())
                        {
                            ContentObject co = new ContentObject();
                            FillContentObjectLightLoad(co, results);
                            objects.Add(co);
                        }
                    }
                }
               
                return objects;
            }
        }

        public int GetContentObjectCount(string identity)
        {
            //Other methods calculate this already, so check to make sure we don't already have the count
            if(_TotalObjects < 0)
            {
                System.Data.Odbc.OdbcConnection conn = GetConnection();
                {
                    
                    setContentObjectCount(conn, identity);
                    
                }
            }
            return _TotalObjects;
        }

        private void setContentObjectCount(OdbcConnection conn, string identity)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "CALL GetContentObjectCount(?)";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("uname", identity);
                _TotalObjects = System.Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}
