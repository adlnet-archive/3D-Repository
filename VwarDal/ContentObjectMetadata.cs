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
using System.Reflection;

namespace vwarDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class ContentObjectMetadata : MetaDataBase
    {
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }       
        /// <summary>
        /// 
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SubmitterEmail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SponsorLogoImageFileName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SponsorLogoImageFileNameId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DeveloperLogoImageFileName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DeveloperLogoImageFileNameId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        private string _AssetType = "Model";
        /// <summary>
        /// 
        /// </summary>
        public string AssetType { get { return _AssetType; } set { _AssetType = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string ScreenShot { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ScreenShotId { get; set; }
        public string Thumbnail { get; set; }
        public string ThumbnailId { get; set; }
        public string CollectionName { get; set; }        
        public string DisplayFile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DisplayFileId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OriginalFileName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OriginalFileId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Keywords { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MoreInformationURL { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DeveloperName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SponsorName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ArtistName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CreativeCommonsLicenseURL { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UnitScale { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UpAxis { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UVCoordinateChannel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IntentionOfTexture { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int NumberOfRevisions { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Views { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Downloads { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int NumPolygons { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int NumTextures { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime LastModified { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime LastViewed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime UploadedDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Revision { get; set; }
    }
}
