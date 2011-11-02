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
using System.IO;

namespace vwarDAL
{
    public interface IDataRepository
    {
        IEnumerable<ContentObject> GetAllContentObjects();
        //IEnumerable<ContentObject> GetAllContentObjects(String UserName);
        void InsertReview(int rating, string text, string submitterEmail, string contentObjectId);
        void UpdateContentObject(ContentObject co);
        
        ContentObject GetContentObjectById(string pid, bool updateViews, bool getReviews = false, int revision = -1);

        Stream GetContentFile(string pid, string file);
        Stream GetSupportingFile(ContentObject co, string filename);
        Stream GetCachedContentObjectTransform(ContentObject co, string extension);

        string SetContentFile(Stream data, ContentObject co, string filename);
        string SetContentFile(Stream data, string pid, string filename);
        string AddSupportingFile(Stream data, ContentObject co, string filename, string description);
        bool AddMissingTexture(ContentObject co, string filename, string type, int UVset);
        bool AddTextureReference(ContentObject co, string filename, string type, int UVset);

        bool RemoveTextureReference(ContentObject co, string filename);
        bool RemoveMissingTexture(ContentObject co, string filename);
        bool RemoveSupportingFile(ContentObject co, string filename);
        void DeleteContentObject(ContentObject id);
        byte[] GetContentFileData(string pid, string dsid);
        
        string UpdateFile(byte[] data, string pid, string fileName, string newFileName = null);
        void RemoveFile(string pid, string fileName);
        void InsertContentRevision(ContentObject co);
        void InsertContentObject(ContentObject co);

        void IncrementDownloads(string id);

        ContentObject GetNewContentObject();

        IEnumerable<ContentObject> SearchContentObjects(string searchTerm);
    }
}
