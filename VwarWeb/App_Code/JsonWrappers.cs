using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JsonWrappers
{
    /// <summary>
    /// A container for parameters to be passed to the ViewerLoader javascript class.
    /// </summary>
    public class ViewerLoadParams
    {
        /// <summary>
        /// Determines whether the ViewerLoader can load the data.
        /// </summary>
        public bool IsViewable;

        /// <summary>
        /// The path to the "Public" folder where the Model IHttpHandler is located.
        /// </summary>
        public string BasePath;

        /// <summary>
        /// The filename and parameter template for the ModelIHttpHandler URL.
        /// </summary>
        public string BaseContentUrl;

        /// <summary>
        /// The filename for loading via the Flash viewer.
        /// </summary>
        public string FlashLocation;

        /// <summary>
        /// The filename for loading via the O3D viewer.
        /// </summary>
        public string O3DLocation;

        /// <summary>
        /// The Up Axis to be set for the viewer (Flash and O3D).
        /// </summary>
        public string UpAxis;

        /// <summary>
        /// The Unit Scale (in meters) to be set for the viewer (Flash and O3D).
        /// </summary>
        public string UnitScale;

        /// <summary>
        /// Determines whether to show the screenshot button in the viewer.
        /// </summary>
        public bool ShowScreenshot;


        public ViewerLoadParams()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
    /// <summary>
    /// A container for parameters to be passed to the ViewerLoader javascript class.
    /// </summary>
    public class AdvancedDownloadPreviewSettings
    {
        /// <summary>
        /// Determines whether the ViewerLoader can load the data.
        /// </summary>
        public bool IsViewable;

        /// <summary>
        /// Determines whether the ViewerLoader can load the data.
        /// </summary>
        public int Polygons;
        /// <summary>
        /// The path to the "Public" folder where the Model IHttpHandler is located.
        /// </summary>
        public string BasePath;

        /// <summary>
        /// The filename and parameter template for the ModelIHttpHandler URL.
        /// </summary>
        public string BaseContentUrl;

        /// <summary>
        /// The filename for loading via the Flash viewer.
        /// </summary>
        public string FlashLocation;

        /// <summary>
        /// The filename for loading via the O3D viewer.
        /// </summary>
        public string O3DLocation;

        /// <summary>
        /// The Up Axis to be set for the viewer (Flash and O3D).
        /// </summary>
        public string UpAxis;

        /// <summary>
        /// The Unit Scale (in meters) to be set for the viewer (Flash and O3D).
        /// </summary>
        public string UnitScale;

        /// <summary>
        /// Determines whether to show the screenshot button in the viewer.
        /// </summary>
        public bool ShowScreenshot;


        public AdvancedDownloadPreviewSettings()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }

    /// <summary>
    /// A container for parameters to be passed as defaults for Step 3 Upload form inputs
    /// </summary>
    public class UploadDetailDefaults
    {
        public bool HasDefaults;

        public string DeveloperName;
        public string ArtistName;
        public string DeveloperUrl;
        public string DeveloperLogoFilename;
        public string SponsorName;
        public string SponsorLogoFilename;

        public UploadDetailDefaults()
        {
            //Set to empty strings to avoid null references being returned
            HasDefaults = false;
            DeveloperName = String.Empty;
            ArtistName = String.Empty;
            DeveloperUrl = String.Empty;
            DeveloperLogoFilename = String.Empty;
            SponsorName = String.Empty;
            SponsorLogoFilename = String.Empty;
        }
    }
}