using System;
namespace vwarDAL
{
    interface IMetadataStore
    {
        bool AddMissingTexture(ContentObject co, string filename, string type, int UVset);
        void AddSupportingFile(ContentObject co, string filename, string description);
        bool AddTextureReference(ContentObject co, string filename, string type, int UVset);
        void DeleteContentObject(ContentObject co);
        System.Collections.Generic.IEnumerable<ContentObject> GetAllContentObjects();
        ContentObject GetContentObjectById(string pid, bool updateViews, bool getReviews = true, int revision = -1);
        System.Collections.Generic.IEnumerable<ContentObject> GetObjectsWithRange(string query, int count, int start);
        void IncrementDownloads(string id);
        void InsertContentObject(ContentObject co);
        void InsertContentRevision(ContentObject co);
        void InsertReview(int rating, string text, string submitterEmail, string contentObjectId);
        bool RemoveMissingTexture(ContentObject co, string filename);
        bool RemoveSupportingFile(ContentObject co, string filename);
        bool RemoveTextureReference(ContentObject co, string filename);
        void UpdateContentObject(ContentObject co);
    }
}
