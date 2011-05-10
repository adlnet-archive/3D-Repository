using System;
namespace vwarDAL
{
    interface IFileStore
    {
        bool AddSupportingFile(System.IO.Stream data, ContentObject co, string filename);
        void DeleteContentObject(ContentObject co);
        System.IO.Stream GetContentFile(string pid, string file);
        byte[] GetContentFileData(string pid, string dsid);
        ContentObject GetNewContentObject();
        void InsertContentObject(ContentObject co);
        void RemoveFile(string pid, string fileName);
        string SetContentFile(System.IO.Stream data, string pid, string filename);
        string UpdateFile(byte[] data, string pid, string fileName, string newFileName = null);
        string UpdateFile(System.IO.Stream data, string pid, string fileName, string newfileName = null);
    }
}
