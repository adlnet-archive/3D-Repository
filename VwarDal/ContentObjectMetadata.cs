using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace vwarDAL
{
    
    public class ContentObjectMetadata : MetaDataBase
    {
        
        public string Title { get; set; }
        public string Description { get; set; }       
        public string Label { get; set; }
        public string Location { get; set; }
        public string SubmitterEmail { get; set; }
        public string SponsorLogoImageFileName { get; set; }
        public string SponsorLogoImageFileNameId { get; set; }
        public string DeveloperLogoImageFileName { get; set; }
        public string DeveloperLogoImageFileNameId { get; set; }
        private string _AssetType = "Model";
        public string AssetType { get { return _AssetType; } set { _AssetType = value; } }
        public string ScreenShot { get; set; }
        public string ScreenShotId { get; set; }
        public string CollectionName { get; set; }        
        public string DisplayFile { get; set; }
        public string DisplayFileId { get; set; }
        public string OriginalFileName { get; set; }
        public string OriginalFileId { get; set; }
        public string Keywords { get; set; }
        public string MoreInformationURL { get; set; }
        public string DeveloperName { get; set; }
        public string SponsorName { get; set; }
        public string ArtistName { get; set; }
        public string CreativeCommonsLicenseURL { get; set; }
        public string UnitScale { get; set; }
        public string UpAxis { get; set; }
        public string UVCoordinateChannel { get; set; }
        public string IntentionOfTexture { get; set; }
        public string Format { get; set; }
        public int NumberOfRevisions { get; set; }

        public int Views { get; set; }
        public int Downloads { get; set; }
        public int NumPolygons { get; set; }
        public int NumTextures { get; set; }
        
        
     

        public DateTime LastModified { get; set; }
        public DateTime LastViewed { get; set; }
        public DateTime UploadedDate { get; set; }
        public int Revision { get; set; }
    }
}
