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
namespace vwarDAL
{
    /// <summary>
    /// 
    /// </summary>
    interface IMetadataStore
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="filename"></param>
        /// <param name="type"></param>
        /// <param name="UVset"></param>
        /// <returns></returns>
        bool AddMissingTexture(ContentObject co, string filename, string type, int UVset);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="filename"></param>
        /// <param name="description"></param>
        void AddSupportingFile(ContentObject co, string filename, string description, string dsid);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="filename"></param>
        /// <param name="type"></param>
        /// <param name="UVset"></param>
        /// <returns></returns>
        bool AddTextureReference(ContentObject co, string filename, string type, int UVset);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        void DeleteContentObject(ContentObject co);
        /// <summary>
        /// Retrieves all content objects with all fields inside the repository. Use only when performance is not an issue, or 
        /// no other option is available without heavy DAL modification. This does NOT do a permissions check.
        /// </summary>
        /// <returns>An IEnumerable containing all content objects</returns>
        System.Collections.Generic.IEnumerable<ContentObject> GetAllContentObjects();
        /// <summary>
        /// Retrieves all content objects with all fields inside the repository. Use only when performance is not an issue, or 
        /// no other option is available without heavy DAL modification. This DOES do a permissions check.
        /// </summary>
        /// <returns>An IEnumerable containing all content objects</returns>
        System.Collections.Generic.IEnumerable<ContentObject> GetAllContentObjects(string UserName);
        /// <summary>
        /// Searches the content object metadata by individual field.
        /// </summary>
        /// <param name="field">The field name you would like to search in</param>
        /// <param name="identity">The value you would like to search for</param>
        /// <returns></returns>
        System.Collections.Generic.IEnumerable<ContentObject> GetContentObjectsByField(string field, string val, string identity);
        /// <summary>
        /// Searches the keywords of each content object and returns a list of ContentObjects tagged with any of the input terms
        /// </summary>
        /// <param name="keywords">A comma-delimited list of keywords or phrases to search for</param>
        /// <param name="identity">The current identity that authenticates via PermissionsManager (usually Context.User.Identity.Name)</param>
        /// <returns>An IEnumerable containing all content objects that match at least one of keywords</returns>
        System.Collections.Generic.IEnumerable<ContentObject> GetContentObjectsByKeywords(string keywords, string identity);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="updateViews"></param>
        /// <param name="getReviews"></param>
        /// <param name="revision"></param>
        /// <returns></returns>
        ContentObject GetContentObjectById(string pid, bool updateViews, bool getReviews = true, int revision = -1);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="count"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        System.Collections.Generic.IEnumerable<ContentObject> GetObjectsWithRange(string query, int count, int start, SortOrder order, string identity);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        void IncrementDownloads(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        void InsertContentObject(ContentObject co);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        void InsertContentRevision(ContentObject co);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rating"></param>
        /// <param name="text"></param>
        /// <param name="submitterEmail"></param>
        /// <param name="contentObjectId"></param>
        void InsertReview(int rating, string text, string submitterEmail, string contentObjectId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        bool RemoveMissingTexture(ContentObject co, string filename);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        bool RemoveKeyword(ContentObject co, string keyword);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <returns></returns>
        bool RemoveAllKeywords(ContentObject co);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        bool RemoveSupportingFile(ContentObject co, string filename);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        bool RemoveTextureReference(ContentObject co, string filename);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        void UpdateContentObject(ContentObject co);

        /// <summary>
        /// Gets the total number of content objects in the repository.
        /// </summary>
        /// <returns>The total count of content objects that have metadata available.</returns>
        int GetContentObjectCount(string identity);
    }
}
