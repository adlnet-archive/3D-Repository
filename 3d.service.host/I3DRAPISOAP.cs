using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;
using System.Xml;

namespace vwar.service.host
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
   
    public interface I3DRAPISOAP
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
      
        [OperationContract]
        List<SearchResult> Search(string terms, string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [OperationContract]
        List<SearchResult> AdvancedSearch(string searchmode, string searchstring, string key);
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="format"></param>
        /// <param name="options"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        
        [OperationContract]
        Stream GetModel(string pid, string format, string options, string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="format"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        
        [OperationContract]
        Stream GetModelSimple(string pid, string format, string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        
        [OperationContract]
        List<Review> GetReviews(string pid, string key);


     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
      
        [OperationContract]
        Stream GetScreenshot(string pid, string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
      
        [OperationContract]
        Stream GetThumbnail(string pid, string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        
        [OperationContract]
        Stream GetDeveloperLogo(string pid, string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
       
        [OperationContract]
        Stream GetSponsorLogo(string pid, string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
       
        [OperationContract]
        Metadata GetMetadata(string pid, string key);

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="filename"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        
        [OperationContract]
        Stream GetSupportingFile(string pid, string filename, string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="filename"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        
        [OperationContract]
        Stream GetTextureFile(string pid, string filename, string key);

       
        [OperationContract]
        Stream GetOriginalUploadFile(string pid, string key);
    }
}
