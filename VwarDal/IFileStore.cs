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
    interface IFileStore
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="co"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        string AddSupportingFile(System.IO.Stream data, ContentObject co, string filename);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        void DeleteContentObject(ContentObject co);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        System.IO.Stream GetContentFile(string pid, string file);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="dsid"></param>
        /// <returns></returns>
        byte[] GetContentFileData(string pid, string dsid);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ContentObject GetNewContentObject();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        void InsertContentObject(ContentObject co);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="fileName"></param>
        void RemoveFile(string pid, string fileName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pid"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        string SetContentFile(System.IO.Stream data, string pid, string filename);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pid"></param>
        /// <param name="fileName"></param>
        /// <param name="newFileName"></param>
        /// <returns></returns>
        string UpdateFile(byte[] data, string pid, string fileName, string newFileName = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pid"></param>
        /// <param name="fileName"></param>
        /// <param name="newfileName"></param>
        /// <returns></returns>
        string UpdateFile(System.IO.Stream data, string pid, string fileName, string newfileName = null);
    }
}
