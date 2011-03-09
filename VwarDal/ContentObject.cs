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
    public class ContentObject
    {

        public ContentObjectMetadata _Metadata = new ContentObjectMetadata();

        public string PID { get; set; }
        public string Description { get { return _Metadata.Description; } set { _Metadata.Description = value; } }
        public string Title { get { return _Metadata.Title; } set { _Metadata.Title = value; } }
        public string Label { get { return _Metadata.Label; } set { _Metadata.Label = value; } }
        public string Location { get { return _Metadata.Location; } set { _Metadata.Location = value; } }
        public string SubmitterEmail { get { return _Metadata.SubmitterEmail; } set { _Metadata.SubmitterEmail = value; } }
        public string SponsorLogoImageFileName { get { return _Metadata.SponsorLogoImageFileName; } set { _Metadata.SponsorLogoImageFileName = value; } }
        public string SponsorLogoImageFileNameId { get { return _Metadata.SponsorLogoImageFileNameId; } set { _Metadata.SponsorLogoImageFileNameId = value; } }
        public string DeveloperLogoImageFileName { get { return _Metadata.DeveloperLogoImageFileName; } set { _Metadata.DeveloperLogoImageFileName = value; } }
        public string DeveloperLogoImageFileNameId { get { return _Metadata.DeveloperLogoImageFileNameId; } set { _Metadata.DeveloperLogoImageFileNameId = value; } }
        public string AssetType { get { return _Metadata.AssetType; } set { _Metadata.AssetType = value; } }
        public string ScreenShot { get { return _Metadata.ScreenShot; } set { _Metadata.ScreenShot = value; } }
        public string ScreenShotId { get { return _Metadata.ScreenShotId; } set { _Metadata.ScreenShotId = value; } }      
        public string DisplayFile { get { return _Metadata.DisplayFile; } set { _Metadata.DisplayFile = value; } }
        public string DisplayFileId { get { return _Metadata.DisplayFileId; } set { _Metadata.DisplayFileId = value; } }
        public string OriginalFileName { get { return _Metadata.OriginalFileName; } set { _Metadata.OriginalFileName = value; } }
        public string OriginalFileId { get { return _Metadata.OriginalFileId; } set { _Metadata.OriginalFileId = value; } }
        public string Keywords { get { return _Metadata.Keywords; } set { _Metadata.Keywords = value; } }
        public string MoreInformationURL { get { return _Metadata.MoreInformationURL; } set { _Metadata.MoreInformationURL = value; } }
        public string DeveloperName { get { return _Metadata.DeveloperName; } set { _Metadata.DeveloperName = value; } }
        public string SponsorName { get { return _Metadata.SponsorName; } set { _Metadata.SponsorName = value; } }
        public string ArtistName { get { return _Metadata.ArtistName; } set { _Metadata.ArtistName = value; } }
        public string CreativeCommonsLicenseURL { get { return _Metadata.CreativeCommonsLicenseURL; } set { _Metadata.CreativeCommonsLicenseURL = value; } }
        public string UnitScale { get { return _Metadata.UnitScale; } set { _Metadata.UnitScale = value; } }
        public string UpAxis { get { return _Metadata.UpAxis; } set { _Metadata.UpAxis = value; } }
        public string UVCoordinateChannel { get { return _Metadata.UVCoordinateChannel; } set { _Metadata.UVCoordinateChannel = value; } }
        public string IntentionOfTexture { get { return _Metadata.IntentionOfTexture; } set { _Metadata.IntentionOfTexture = value; } }
        public string Format { get { return _Metadata.Format; } set { _Metadata.Format = value; } }

        public int Views { get { return _Metadata.Views; } set { _Metadata.Views = value; } }
        public int Downloads { get { return _Metadata.Downloads; } set { _Metadata.Downloads = value; } }
        public int NumPolygons { get { return _Metadata.NumPolygons; } set { _Metadata.NumPolygons = value; } }
        public int NumTextures { get { return _Metadata.NumTextures; } set { _Metadata.NumTextures = value; } }


        public DateTime UploadedDate { get { return _Metadata.UploadedDate; } set { _Metadata.UploadedDate = value; } }
        public DateTime LastModified { get { return _Metadata.LastModified; } set { _Metadata.LastModified = value; } }
        public DateTime LastViewed { get { return _Metadata.LastViewed; } set { _Metadata.LastViewed = value; } }

        private List<Review> _Reviews = new List<Review>();
        public List<Review> Reviews { get { return _Reviews; } set { _Reviews = value; } }
        private bool _Enabled = false;
        public bool Enabled { get { return _Enabled; } set { _Enabled = value; } }
        private bool _Ready = false;
        public bool Ready { get { return _Ready; } set { _Ready = value; } }
        private bool _RequireResubmit = false;
        public bool RequireResubmit { get { return _RequireResubmit; } set { _RequireResubmit = value; } }
    }
}
