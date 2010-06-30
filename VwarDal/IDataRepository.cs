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
        IEnumerable<ContentObject> GetContentObjectsByCollectionName(string collectionName);
        IEnumerable<ContentObject> GetHighestRated(int count);
        IEnumerable<ContentObject> GetMostPopular(int count);
        IEnumerable<ContentObject> GetRecentlyUpdated(int count);
        void InsertReview(int rating, string text, string submitterEmail, string contentObjectId);
        void UpdateContentObject(ContentObject co);
        IEnumerable<ContentObject> GetRecentlyViewed(int count);
        IEnumerable<ContentObject> SearchContentObjects(string searchTerm);
        IEnumerable<ContentObject> GetContentObjectsBySubmitterEmail(string email);
        ContentObject GetContentObjectById(string pid, bool updateViews);
        void UploadFile(byte[] data, string pid, string fileName);
        void UpdateFile(byte[] data, string pid, string fileName);
        void UploadFile(Stream data, string pid, string fileName);
        void DeleteContentObject(string id);
        void InsertContentObject(ContentObject co);
        void IncrementDownloads(string id);
        string GetContentUrl(string pid, string fileName);
        byte[] GetContentFileData(string pid, string fileName);
    }
}
