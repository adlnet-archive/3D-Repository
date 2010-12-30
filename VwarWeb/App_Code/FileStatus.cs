using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Utils
{
    public class ImagePrefix
    {
        public const string SCREENSHOT = "screenshot";
        public const string DEVELOPER_LOGO = "devlogo";
        public const string SPONSOR_LOGO = "sponsorlogo";
    }

    //An enumeration indicating a result of a format detection attempt
    public class FormatType
    {
        public const string UNRECOGNIZED = "UNRECOGNIZED";
        public const string RECOGNIZED = "RECOGNIZED";
        public const string VIEWABLE = "VIEWABLE";
        public const string MULTIPLE_RECOGNIZED = "MULTIPLE_RECOGNIZED";
    }

    public class FileStatus
    {
        public static string[] ViewableFormats = { ".fbx", ".dae", ".obj", ".3ds", ".skp", ".kmz" };
        public static string[] RecognizedFormats = { ".blend", ".max", ".mb", ".flt", ".c4d", ".cob", ".lwo", ".lws", ".xsi", ".ma", ".x3d" };

        public const string UnrecognizedMessage = "We're sorry, but no recognized model files were found in your archive. To ensure security and quality control, content "
                                                 + "uploaded to the 3D Repository must be on our list of <a href='#' onclick='return false;'>recognized formats</a>.";

        public const string WarningMessage = "While this format is shareable on our site, it cannot be displayed in our 3D Viewer. You may want to consider converting to "
                                            + "one of our supported <a href='#' onclick='return false;'>viewable formats</a> before uploading.";

        public const string MultipleRecognizedMessage = "More than one model file that is <a href='#' onclick='return false;'>recognized or viewable</a> was found inside "
                                                       + "your archive. Please ensure only one model file is placed in your .zip file and try again.";

        public const string ConversionFailedMessage = "An error occurred while trying to prepare your model for our viewers. This "
                                                     + "is usually an indication of a corrupt file or incorrect extension. Please check your files and try again.";

        public string extension { get; set; }
        public string type { get; set; }
        public string filename { get; set; }
        public string hashname { get; set; }
        public string msg { get; set; }
        public string converted { get; set; }


        public FileStatus(string name, string ty)
        {
            filename = name;
            type = ty;
            converted = "false";
        }

        public static string GetType(string extension)
        {
            if (ExtensionInArray(extension, ViewableFormats))
            {
                return FormatType.VIEWABLE;
            }
            else if (ExtensionInArray(extension, RecognizedFormats))
            {
                return FormatType.RECOGNIZED;
            }
            else
            {
                return FormatType.UNRECOGNIZED;
            }
        }



        private static bool ExtensionInArray(string extension, string[] formatList)
        {
            foreach (string s in formatList)
            {
                if (string.Compare(extension, s, true) == 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}