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



using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
namespace vwarDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class FedoraCommonsRepo : IDataRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private IMetadataStore _metadataStore;
        /// <summary>
        /// 
        /// </summary>
        private IFileStore _fileStore;

        private string _identity;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="access"></param>
        /// <param name="management"></param>
        /// <param name="connectionString"></param>
        /// <param name="fileNamespace"></param>
        internal FedoraCommonsRepo(string url, string userName, string password, string access, string management, string connectionString, string fileNamespace, string identity="")
        {
            _metadataStore = new MySqlMetadataStore(connectionString);
            _fileStore = new FedoraFileStore(url, userName, password, access, management, fileNamespace);
            _identity = identity;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContentObject> GetAllContentObjects()
        {
            return _metadataStore.GetAllContentObjects();
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
            _metadataStore.InsertReview(rating, text, submitterEmail, contentObjectId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        public void UpdateContentObject(ContentObject co)
        {
            _metadataStore.RemoveAllKeywords(co);
            _metadataStore.UpdateContentObject(co);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public IEnumerable<ContentObject> SearchContentObjects(string searchTerm)
        {
            var items = from co in GetAllContentObjects()
                        where SearchFunction(co, searchTerm)
                        select co;
            return items;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public IEnumerable<ContentObject> GetContentObjectsBySubmitterEmail(string email)
        {
            var co = from c in GetAllContentObjects()
                     where !String.IsNullOrEmpty(c.SubmitterEmail) && c.SubmitterEmail.ToLower().Equals(email.ToLower().Trim())
                     select c;

            return co;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="developerName"></param>
        /// <returns></returns>
        public IEnumerable<ContentObject> GetContentObjectsByDeveloperName(string developerName)
        {
            //TODO: change to use the generice search provider
            var co = from c in GetAllContentObjects()
                     where !String.IsNullOrEmpty(c.DeveloperName) && c.DeveloperName.ToLower().Equals(developerName.ToLower().Trim())
                     select c;

            return co;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sponsorName"></param>
        /// <returns></returns>
        public IEnumerable<ContentObject> GetContentObjectsBySponsorName(string sponsorName)
        {
            //TODO: change to use the generice search provider
            var co = from c in GetAllContentObjects()
                     where !String.IsNullOrEmpty(c.SponsorName) && c.SponsorName.ToLower().Contains(sponsorName.ToLower().Trim())
                     select c;

            return co;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="artistName"></param>
        /// <returns></returns>
        public IEnumerable<ContentObject> GetContentObjectsByArtistName(string artistName)
        {
            //TODO: change to use the generice search provider
            var co = from c in GetAllContentObjects()
                     where !String.IsNullOrEmpty(c.ArtistName) && c.ArtistName.ToLower().Contains(artistName.ToLower().Trim())
                     select c;

            return co;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public IEnumerable<ContentObject> GetContentObjectsByKeyWords(string keyword)
        {
            //TODO: change to use the generice search provider
            var co = from c in GetAllContentObjects()
                     where !String.IsNullOrEmpty(c.Keywords) && c.Keywords.ToLower().Contains(keyword.ToLower().Trim())
                     select c;

            return co;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public IEnumerable<ContentObject> GetContentObjectsByDescription(string description)
        {
            //TODO: change to use the generice search provider
            var co = from c in GetAllContentObjects()
                     where !String.IsNullOrEmpty(c.Description) && c.Description.Equals(description, StringComparison.InvariantCultureIgnoreCase)
                     select c;

            return co;


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public IEnumerable<ContentObject> GetContentObjectsByTitle(string title)
        {
            //TODO: change to use the generice search provider
            var co = from c in GetAllContentObjects()
                     where c.Title.ToLower().Contains(title.ToLower().Trim())
                     select c;

            return co;
        }        
        /// <summary>
        /// private Dictionary<String, ContentObject> _Memory = new Dictionary<string, ContentObject>(); 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="updateViews"></param>
        /// <param name="getReviews"></param>
        /// <param name="revision"></param>
        /// <returns></returns>
        public ContentObject GetContentObjectById(string pid, bool updateViews, bool getReviews = true, int revision = -1)
        {
            ContentObject ret = _metadataStore.GetContentObjectById(pid, updateViews, getReviews, revision);
            ret.SetParentRepo(this);
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="co"></param>
        /// <param name="filename"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public string AddSupportingFile(Stream data, ContentObject co, string filename, string description)
        {
            string dsid = _fileStore.AddSupportingFile(data, co, filename);
            _metadataStore.AddSupportingFile(co, filename, description, dsid);

            return dsid;
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
            return _metadataStore.AddTextureReference(co, filename, type, UVset);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool RemoveMissingTexture(ContentObject co, string filename)
        {
            return _metadataStore.RemoveMissingTexture(co, filename);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool RemoveTextureReference(ContentObject co, string filename)
        {
            return _metadataStore.RemoveTextureReference(co, filename);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool RemoveSupportingFile(ContentObject co, string filename)
        {
            return _metadataStore.RemoveSupportingFile(co, filename);
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
            return _metadataStore.AddMissingTexture(co, filename, type, UVset);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        public void DeleteContentObject(ContentObject co)
        {
            _fileStore.DeleteContentObject(co);
            _metadataStore.DeleteContentObject(co);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        public void InsertContentRevision(ContentObject co)
        {
            _metadataStore.InsertContentRevision(co);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        public void InsertContentObject(ContentObject co)
        {
            _fileStore.InsertContentObject(co);
            _metadataStore.InsertContentObject(co);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="co"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string SetContentFile(Stream data, ContentObject co, string filename)
        {
            return SetContentFile(data, co.PID, filename);
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
            return _fileStore.SetContentFile(data, pid, filename);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public Stream GetCachedContentObjectTransform(ContentObject co, string extension)
        {
            if (extension.ToLower() == "o3d" || extension.ToLower() == "tgz")
                return new MemoryStream(GetContentFileData(co.PID, co.DisplayFile));
            else return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public Stream GetSupportingFile(ContentObject co, string dsid)
        {
            return new MemoryStream(GetContentFileData(co.PID, dsid));
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
            return _fileStore.UpdateFile(data, pid, fileName, newfileName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void IncrementDownloads(string id)
        {
            _metadataStore.IncrementDownloads(id);
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
            return _fileStore.UpdateFile(data, pid, fileName, newFileName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="fileName"></param>
        public void RemoveFile(string pid, string fileName)
        {
            _fileStore.RemoveFile(pid, fileName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public Stream GetContentFile(string pid, string file)
        {
            return _fileStore.GetContentFile(pid, file);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="dsid"></param>
        /// <returns></returns>
        public byte[] GetContentFileData(string pid, string dsid)
        {
            return _fileStore.GetContentFileData(pid, dsid);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ContentObject GetNewContentObject()
        {
            ContentObject co = new ContentObject(this);

            return co;
        }
    }
}
