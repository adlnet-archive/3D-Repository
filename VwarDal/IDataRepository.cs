using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace vwarDAL
{
    public interface IDataRepository
    {
        IEnumerable<ContentObject> GetAllContentObjects();
        //IEnumerable<ContentObject> GetContentObjectsByCollectionName(string collectionName);
        IEnumerable<ContentObject> GetHighestRated(int count, int start = 0);
        IEnumerable<ContentObject> GetMostPopular(int count, int start = 0);
        IEnumerable<ContentObject> GetRecentlyUpdated(int count, int start = 0);
        IEnumerable<ContentObject> GetContentObjectsByDeveloperName(string developerName);
        IEnumerable<ContentObject> GetContentObjectsBySponsorName(string sponsorName);
        IEnumerable<ContentObject> GetContentObjectsByArtistName(string artistName);
        IEnumerable<ContentObject> GetContentObjectsByKeyWords(string keyword);
        IEnumerable<ContentObject> GetContentObjectsByDescription(string description);
        IEnumerable<ContentObject> GetContentObjectsByTitle(string title);

        void InsertReview(int rating, string text, string submitterEmail, string contentObjectId);
        void UpdateContentObject(ContentObject co);
        IEnumerable<ContentObject> GetRecentlyViewed(int count, int start = 0);
        IEnumerable<ContentObject> SearchContentObjects(string searchTerm);
        IEnumerable<ContentObject> GetContentObjectsBySubmitterEmail(string email);

        ContentObject GetContentObjectById(string pid, bool updateViews, bool getReviews = false, int revision = -1);

        Stream GetContentFile(string pid, string file);
        Stream GetSupportingFile(ContentObject co, string filename);
        Stream GetCachedContentObjectTransform(ContentObject co, string extension);

        string SetContentFile(Stream data, ContentObject co, string filename);
        string SetContentFile(Stream data, string pid, string filename);
        bool AddSupportingFile(Stream data, ContentObject co, string filename, string description);
        bool AddMissingTexture(ContentObject co, string filename, string type, int UVset);
        bool AddTextureReference(ContentObject co, string filename, string type, int UVset);

        bool RemoveTextureReference(ContentObject co, string filename);
        bool RemoveMissingTexture(ContentObject co, string filename);
        bool RemoveSupportingFile(ContentObject co, string filename);
        void DeleteContentObject(ContentObject id);

        byte[] GetContentFileData(string pid, string fileName);
        
        string UpdateFile(byte[] data, string pid, string fileName, string newFileName = null);
        void RemoveFile(string pid, string fileName);
        void InsertContentRevision(ContentObject co);
        void InsertContentObject(ContentObject co);

        void IncrementDownloads(string id);

        ContentObject GetNewContentObject();
   
    }
}
