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

namespace vwarDAL
{
    /// <summary>
    /// Location - Model Zip File
    /// 
    /// </summary>
    ///     
    public class ContentObject
    {
        /// <summary>
        /// 
        /// </summary>
        private ContentObjectMetadata _Metadata = new ContentObjectMetadata();
        /// <summary>
        /// 
        /// </summary>
        public string PID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get { return _Metadata.Description; } set { _Metadata.Description = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string Title { get { return _Metadata.Title; } set { _Metadata.Title = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string Label { get { return _Metadata.Label; } set { _Metadata.Label = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string Location { get { return _Metadata.Location; } set { _Metadata.Location = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string SubmitterEmail { get { return _Metadata.SubmitterEmail; } set { _Metadata.SubmitterEmail = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string SponsorLogoImageFileName { get { return _Metadata.SponsorLogoImageFileName; } set { _Metadata.SponsorLogoImageFileName = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string SponsorLogoImageFileNameId { get { return _Metadata.SponsorLogoImageFileNameId; } set { _Metadata.SponsorLogoImageFileNameId = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string DeveloperLogoImageFileName { get { return _Metadata.DeveloperLogoImageFileName; } set { _Metadata.DeveloperLogoImageFileName = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string DeveloperLogoImageFileNameId { get { return _Metadata.DeveloperLogoImageFileNameId; } set { _Metadata.DeveloperLogoImageFileNameId = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string AssetType { get { return _Metadata.AssetType; } set { _Metadata.AssetType = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string ScreenShot { get { return _Metadata.ScreenShot; } set { _Metadata.ScreenShot = value; } }
        public string ScreenShotId { get { return _Metadata.ScreenShotId; } set { _Metadata.ScreenShotId = value; } }
        public string Thumbnail { get { return _Metadata.Thumbnail; } set { _Metadata.Thumbnail = value; } }
        public string ThumbnailId { get { return _Metadata.ThumbnailId; } set { _Metadata.ThumbnailId = value; } }      
        public string DisplayFile { get { return _Metadata.DisplayFile; } set { _Metadata.DisplayFile = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string DisplayFileId { get { return _Metadata.DisplayFileId; } set { _Metadata.DisplayFileId = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string OriginalFileName { get { return _Metadata.OriginalFileName; } set { _Metadata.OriginalFileName = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string OriginalFileId { get { return _Metadata.OriginalFileId; } set { _Metadata.OriginalFileId = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string Keywords { get { return _Metadata.Keywords; } set { _Metadata.Keywords = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string MoreInformationURL { get { return _Metadata.MoreInformationURL; } set { _Metadata.MoreInformationURL = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string DeveloperName { get { return _Metadata.DeveloperName; } set { _Metadata.DeveloperName = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string SponsorName { get { return _Metadata.SponsorName; } set { _Metadata.SponsorName = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string ArtistName { get { return _Metadata.ArtistName; } set { _Metadata.ArtistName = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string CreativeCommonsLicenseURL { get { return _Metadata.CreativeCommonsLicenseURL; } set { _Metadata.CreativeCommonsLicenseURL = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string UnitScale { get { return _Metadata.UnitScale; } set { _Metadata.UnitScale = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string UpAxis { get { return _Metadata.UpAxis; } set { _Metadata.UpAxis = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string UVCoordinateChannel { get { return _Metadata.UVCoordinateChannel; } set { _Metadata.UVCoordinateChannel = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string IntentionOfTexture { get { return _Metadata.IntentionOfTexture; } set { _Metadata.IntentionOfTexture = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string Format { get { return _Metadata.Format; } set { _Metadata.Format = value; } }
        /// <summary>
        /// 
        /// </summary>
        public int Revision { get { return _Metadata.Revision; } set { _Metadata.Revision = value; } }
        /// <summary>
        /// 
        /// </summary>
        public int Views { get { return _Metadata.Views; } set { _Metadata.Views = value; } }
        /// <summary>
        /// 
        /// </summary>
        public int Downloads { get { return _Metadata.Downloads; } set { _Metadata.Downloads = value; } }
        /// <summary>
        /// 
        /// </summary>
        public int NumPolygons { get { return _Metadata.NumPolygons; } set { _Metadata.NumPolygons = value; } }
        /// <summary>
        /// 
        /// </summary>
        public int NumTextures { get { return _Metadata.NumTextures; } set { _Metadata.NumTextures = value; } }
        /// <summary>
        /// 
        /// </summary>
        public int NumberOfRevisions { get { return _Metadata.NumberOfRevisions; } set { _Metadata.NumberOfRevisions = value; } }
        /// <summary>
        /// 
        /// </summary>
        public DateTime UploadedDate { get { return _Metadata.UploadedDate; } set { _Metadata.UploadedDate = value; } }
        /// <summary>
        /// 
        /// </summary>
        public DateTime LastModified { get { return _Metadata.LastModified; } set { _Metadata.LastModified = value; } }
        /// <summary>
        /// 
        /// </summary>
        public DateTime LastViewed { get { return _Metadata.LastViewed; } set { _Metadata.LastViewed = value; } }
        /// <summary>
        /// 
        /// </summary>
        private List<Review> _Reviews = new List<Review>();
        /// <summary>
        /// 
        /// </summary>
        public List<Review> Reviews { get { return _Reviews; } set { _Reviews = value; } }
        /// <summary>
        /// 
        /// </summary>
        public List<SupportingFile> SupportingFiles;
        /// <summary>
        /// 
        /// </summary>
        public List<Texture> MissingTextures;
        /// <summary>
        /// 
        /// </summary>
        public List<Texture> TextureReferences;
        /// <summary>
        /// 
        /// </summary>
        IDataRepository mParentRepo;
        /// <summary>
        /// 
        /// </summary>
        public ContentObject()
        {
            MissingTextures = new List<Texture>();
            TextureReferences = new List<Texture>();
            SupportingFiles = new List<SupportingFile>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inRepo"></param>
        public ContentObject(IDataRepository inRepo)
            : this()
        {
            mParentRepo = inRepo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inRepo"></param>
        public void SetParentRepo(IDataRepository inRepo) { mParentRepo = inRepo; }
        /// <summary>
        /// 
        /// </summary>
        private bool _RequireResubmit = false;
        /// <summary>
        /// 
        /// </summary>
        public bool RequireResubmit { get { return _RequireResubmit; } set { _RequireResubmit = value; } }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public System.IO.Stream GetDisplayFile() { return mParentRepo.GetContentFile(this.PID, this.DisplayFile); }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public System.IO.Stream GetContentFile() { return mParentRepo.GetContentFile(this.PID, this.Location); }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public System.IO.Stream GetDeveloperLogoFile() { return mParentRepo.GetContentFile(this.PID, this.DeveloperLogoImageFileName); }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public System.IO.Stream GetSponsorLogoFile() { return mParentRepo.GetContentFile(this.PID, this.SponsorLogoImageFileName); }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public System.IO.Stream GetScreenShotFile() { return mParentRepo.GetContentFile(this.PID, this.ScreenShot); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public System.IO.Stream GetSupportingFile(string file) { return mParentRepo.GetSupportingFile(this, file); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public System.IO.Stream GetOriginalUploadFile() { return mParentRepo.GetContentFile(this.PID, this.OriginalFileId); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool SetDisplayFile(System.IO.Stream data, string file)
        {
            var ret = mParentRepo.SetContentFile(data, this, file);
            this.DisplayFile = file;
            this.DisplayFileId = ret;
            if (!string.IsNullOrEmpty(ret))
                CommitChanges();
            return !string.IsNullOrEmpty(ret);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool SetContentFile(System.IO.Stream data, string file)
        {
            var ret = mParentRepo.SetContentFile(data, this, file);
            this.Location = file;
            if (!string.IsNullOrEmpty(ret))
                CommitChanges();
            return !string.IsNullOrEmpty(ret);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public bool AddReview(vwarDAL.Review r)
        {
            mParentRepo.InsertReview(r.Rating, r.Text, r.SubmittedBy, this.PID);
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool SetDeveloperLogoFile(System.IO.Stream data, string file)
        {
            var ret = mParentRepo.SetContentFile(data, this, file);
            this.DeveloperLogoImageFileName = file;
            this.DeveloperLogoImageFileNameId = ret;
            if (!string.IsNullOrEmpty(ret))
                CommitChanges();
            return !string.IsNullOrEmpty(ret);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool SetSponsorLogoFile(System.IO.Stream data, string file)
        {
            var ret = mParentRepo.SetContentFile(data, this, file);
            this.SponsorLogoImageFileName = file;
            this.SponsorLogoImageFileNameId = ret;
            if (!string.IsNullOrEmpty(ret))
                CommitChanges();
            return !string.IsNullOrEmpty(ret);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool SetScreenShotFile(System.IO.Stream data, string file)
        {
            var ret = mParentRepo.SetContentFile(data, this, file);
            this.ScreenShot = file;
            this.ScreenShotId = ret;
            if (!string.IsNullOrEmpty(ret))
                CommitChanges();
            return !string.IsNullOrEmpty(ret);
        }

        public bool SetThumbnailFile(System.IO.Stream data, string file)
        {
            var ret = mParentRepo.SetContentFile(data, this, file);
            this.Thumbnail = file;
            this.ThumbnailId = ret;
            if (!string.IsNullOrEmpty(ret))
                CommitChanges();
            return !string.IsNullOrEmpty(ret);
        }

        public bool AddTextureReference(string file, string Type, int set)
        {
            bool ret = mParentRepo.AddTextureReference(this, file, Type, set);
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="Type"></param>
        /// <param name="set"></param>
        /// <returns></returns>
        public bool AddMissingTexture(string file, string Type, int set)
        {
            bool ret = mParentRepo.AddMissingTexture(this, file, Type, set);
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="indata"></param>
        /// <param name="file"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public string AddSupportingFile(System.IO.Stream indata, string file, string description)
        {
            string ret = mParentRepo.AddSupportingFile(indata, this, file, description);
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool RemoveSupportingFile(string file)
        {
            bool ret = mParentRepo.RemoveSupportingFile(this, file);
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool RemoveTextureReference(string file)
        {
            bool ret = mParentRepo.RemoveTextureReference(this, file);
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool RemoveMissingTexture(string file)
        {
            bool ret = mParentRepo.RemoveMissingTexture(this, file);
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        public void CommitChanges()
        {
            mParentRepo.UpdateContentObject(this);
        }
        /// <summary>
        /// 
        /// </summary>
        public void RemoveFromRepo()
        {
            mParentRepo.DeleteContentObject(this);
            this.PID = "";
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Ready { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Enabled { get; set; }
    }
}
