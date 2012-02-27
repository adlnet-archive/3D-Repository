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

namespace Utils
{
    /// <summary>
    /// 
    /// </summary>
     [Serializable]
    public class ImagePrefix
    {
        /// <summary>
        /// 
        /// </summary>
        public const string SCREENSHOT = "screenshot";
        /// <summary>
        /// 
        /// </summary>
        public const string DEVELOPER_LOGO = "devlogo";
        /// <summary>
        /// 
        /// </summary>
        public const string SPONSOR_LOGO = "sponsorlogo";
    }

    //An enumeration indicating a result of a format detection attempt
     [Serializable]
    public class FormatType
    {
        /// <summary>
        /// 
        /// </summary>
        public const string UNRECOGNIZED = "UNRECOGNIZED";
        /// <summary>
        /// 
        /// </summary>
        public const string RECOGNIZED = "RECOGNIZED";
        /// <summary>
        /// 
        /// </summary>
        public const string VIEWABLE = "VIEWABLE";
        /// <summary>
        /// 
        /// </summary>
        public const string MULTIPLE_RECOGNIZED = "MULTIPLE_RECOGNIZED";
    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class FileStatus
    {
        /// <summary>
        /// 
        /// </summary>
        public static string[] ViewableFormats = { ".fbx", ".dae", ".obj", ".3ds", ".skp", ".kmz" };
        /// <summary>
        /// 
        /// </summary>
        public static string[] RecognizedFormats = { ".blend", ".max", ".mb", ".flt", ".c4d", ".cob", ".lwo", ".lws", ".xsi", ".ma", ".x3d" };
        /// <summary>
        /// 
        /// </summary>
        public const string UnrecognizedMessage = "We're sorry, but no recognized model files were found in your archive. To ensure security and quality control, content "
                                                 + "uploaded to the 3D Repository must be on our list of <a href='#' onclick='return false;'>recognized formats</a>.";
        /// <summary>
        /// 
        /// </summary>
        public const string WarningMessage = "While this format is shareable on our site, it cannot be displayed in our 3D Viewer. You may want to consider converting to "
                                            + "one of our supported <a href='#' class='FormatsLink' onclick='return false;'>viewable formats</a> before uploading.";
        /// <summary>
        /// 
        /// </summary>
        public const string MultipleRecognizedMessage = "More than one model file that is <a href='#' onclick='return false;'>recognized or viewable</a> was found inside "
                                                       + "your archive. Please ensure only one model file is placed in your .zip file and try again.";
        /// <summary>
        /// 
        /// </summary>
        public const string ConversionFailedMessage = "An error occurred while trying to prepare your model for our viewers. This "
                                                     + "is usually an indication of a corrupt file or incorrect extension. Please check your files and try again.";
        /// <summary>
        /// 
        /// </summary>
        public const string InvalidZipMessage = "The zip file you uploaded appears to be damaged. Please verify the integrity of your archive and try again.";
        /// <summary>
        /// 
        /// </summary>
        public const string ModelFileEmptyMessage = "The model file you uploaded is zero bytes in size. Please upload a non-empty model file and try again.";
        /// <summary>
        /// 
        /// </summary>
        public string extension { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string filename { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string hashname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string converted { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ty"></param>
        public FileStatus(string name, string ty)
        {
            filename = name;
            type = ty;
            converted = "false";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="extension"></param>
        /// <param name="formatList"></param>
        /// <returns></returns>
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
