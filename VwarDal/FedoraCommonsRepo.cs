using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
namespace vwarDAL
{
    public class FedoraCommonsRepo : IDataRepository
    {
        private IMetadataStore _metadataStore;
        private IFileStore _fileStore;
        internal FedoraCommonsRepo(string url, string userName, string password, string access, string management, string connectionString, string fileNamespace)
        {
            _metadataStore = new MySqlMetadataStore(connectionString);
            _fileStore = new FedoraFileStore(url, userName, password, access, management, fileNamespace);
        }



        public IEnumerable<ContentObject> GetAllContentObjects()
        {
            return _metadataStore.GetAllContentObjects();
        }



        public IEnumerable<ContentObject> GetHighestRated(int count, int start = 0)
        {

            return _metadataStore.GetObjectsWithRange("{CALL GetHighestRated(?,?)}", count, start);

        }

        public IEnumerable<ContentObject> GetMostPopular(int count, int start = 0)
        {
            return _metadataStore.GetObjectsWithRange("{CALL GetMostPopular(?,?)}", count, start);
        }
        public IEnumerable<ContentObject> GetRecentlyUpdated(int count, int start = 0)
        {
            return _metadataStore.GetObjectsWithRange("{CALL GetMostRecentlyUpdated(?,?)}", count, start);
        }

        public void InsertReview(int rating, string text, string submitterEmail, string contentObjectId)
        {


            _metadataStore.InsertReview(rating, text, submitterEmail, contentObjectId);
        }

        public void UpdateContentObject(ContentObject co)
        {
            _metadataStore.RemoveAllKeywords(co);
            _metadataStore.UpdateContentObject(co);

        }

        public IEnumerable<ContentObject> GetRecentlyViewed(int count, int start = 0)
        {
            return _metadataStore.GetObjectsWithRange("{CALL GetMostRecentlyViewed(?,?)}", count, start);
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

        public ContentObject GetContentObjectById(string pid, bool updateViews, bool getReviews = true, int revision = -1)
        {
            ContentObject ret = _metadataStore.GetContentObjectById(pid, updateViews, getReviews, revision);
            ret.SetParentRepo(this);
            return ret;
        }
        public bool AddSupportingFile(Stream data, ContentObject co, string filename, string description)
        {
            _metadataStore.AddSupportingFile(co, filename, description);
            _fileStore.AddSupportingFile(data, co, filename);
            return true;
        }
        public bool AddTextureReference(ContentObject co, string filename, string type, int UVset)
        {
            return _metadataStore.AddTextureReference(co, filename, type, UVset);
        }
        public bool RemoveMissingTexture(ContentObject co, string filename)
        {
            return _metadataStore.RemoveMissingTexture(co, filename);

        }
        public bool RemoveTextureReference(ContentObject co, string filename)
        {
            return _metadataStore.RemoveTextureReference(co, filename);

        }
        public bool RemoveSupportingFile(ContentObject co, string filename)
        {
            return _metadataStore.RemoveSupportingFile(co, filename);
        }
        public bool AddMissingTexture(ContentObject co, string filename, string type, int UVset)
        {
            return _metadataStore.AddMissingTexture(co, filename, type, UVset);
        }
        public void DeleteContentObject(ContentObject co)
        {
            _fileStore.DeleteContentObject(co);
            _metadataStore.DeleteContentObject(co);
        }
        public void InsertContentRevision(ContentObject co)
        {
            _metadataStore.InsertContentRevision(co);
        }
        public void InsertContentObject(ContentObject co)
        {
            _fileStore.InsertContentObject(co);
            _metadataStore.InsertContentObject(co);
        }

        public string SetContentFile(Stream data, ContentObject co, string filename)
        {
            return SetContentFile(data, co.PID, filename);
        }
        public string SetContentFile(Stream data, string pid, string filename)
        {
            return _fileStore.SetContentFile(data, pid, filename);
        }
        public Stream GetCachedContentObjectTransform(ContentObject co, string extension)
        {
            if (extension.ToLower() == "o3d" || extension.ToLower() == "tgz")
                return new MemoryStream(GetContentFileData(co.PID, co.DisplayFile));
            else return null;
        }
        public Stream GetSupportingFile(ContentObject co, string filename)
        {
            return new MemoryStream(GetContentFileData(co.PID, filename));
        }
        public string UpdateFile(Stream data, string pid, string fileName, string newfileName = null)
        {
            return _fileStore.UpdateFile(data, pid, fileName, newfileName);
        }



        public void IncrementDownloads(string id)
        {
            _metadataStore.IncrementDownloads(id);
        }


        public string UpdateFile(byte[] data, string pid, string fileName, string newFileName = null)
        {
            return _fileStore.UpdateFile(data, pid, fileName, newFileName);
        }
        public void RemoveFile(string pid, string fileName)
        {
            _fileStore.RemoveFile(pid, fileName);
        }
        public Stream GetContentFile(string pid, string file)
        {
            return _fileStore.GetContentFile(pid, file);
        }
        public byte[] GetContentFileData(string pid, string dsid)
        {
            return _fileStore.GetContentFileData(pid, dsid);
        }
        public ContentObject GetNewContentObject()
        {
            ContentObject co = new ContentObject(this);

            return co;
        }


        
    }
}
