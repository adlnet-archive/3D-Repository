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
using System.IO;
using System.Collections;
using Microsoft.VisualBasic;
using System.Web.Caching;

/// <summary>
/// Summary description for Documents
/// </summary>
/// 
namespace Website
{
    /// <summary>
    /// 
    /// </summary>
    public class Documents
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dPath"></param>
        /// <param name="includeSubFolders"></param>
        /// <returns></returns>
        public static double GetFolderSize(string dPath, bool includeSubFolders)
        {
            try
            {
                long size = 0;
                DirectoryInfo diBase = new DirectoryInfo(dPath);
                FileInfo[] files = null;
                if (includeSubFolders)
                {
                    files = diBase.GetFiles("*", SearchOption.AllDirectories);
                }
                else
                {
                    files = diBase.GetFiles("*", SearchOption.TopDirectoryOnly);
                }
                IEnumerator ie = files.GetEnumerator();
                while (ie.MoveNext())
                {
                    // And Not abort
                    size += ((FileInfo)ie.Current).Length;
                }
                return Math.Round((double)size / 1024 / 1024, 2);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileData"></param>
        /// <param name="clientFileName"></param>
        /// <param name="creds"></param>
        /// <param name="format"></param>
        public static void ServeDocument(Stream fileData, string clientFileName, System.Net.NetworkCredential creds = null, string format = "")
        {
            HttpResponse _response = HttpContext.Current.Response;

            var utility = new Utility_3D();
            utility.Initialize(Website.Config.ConversionLibarayLocation);
            //clear response
            _response.Clear();

            _response.AppendHeader("content-disposition", "attachment; filename=" + clientFileName);
            //serve file
            _response.ContentType = vwarDAL.DataUtils.GetMimeType(clientFileName);


            byte[] data = new byte[fileData.Length];
            fileData.Read(data, 0, data.Length);
            if (Path.GetExtension(clientFileName).Equals(".zip", StringComparison.InvariantCultureIgnoreCase) && !String.IsNullOrEmpty(format))
            {

                Utility_3D.Model_Packager packer = new Utility_3D.Model_Packager();
                fileData.Seek(0, SeekOrigin.Begin);
                var package = packer.Convert(fileData, clientFileName, format);
                data = package.data;
            }
            // _response.End();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualFolderPath"></param>
        /// <param name="uploadDocument"></param>
        /// <param name="newFileName"></param>
        /// <returns></returns>
        public static bool Upload(string virtualFolderPath, HttpPostedFile uploadDocument, string newFileName)
        {
            string fileName = "";
            string newPath = "";

            //extract filename from client path
            fileName = Path.GetFileName(uploadDocument.FileName);

            //create new path
            if (newFileName.Equals(string.Empty))
            {
                newPath = virtualFolderPath + fileName;
            }
            else
            {
                newPath = virtualFolderPath + newFileName;
            }

            //save new doc on server 
            try
            {
                uploadDocument.SaveAs(System.Web.HttpContext.Current.Server.MapPath(newPath));
                return true;
            }
            catch (Exception ex)
            {
                return false;

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool DeleteFile(string fileName)
        {
            try
            {
                System.IO.File.Delete(fileName);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="defaultIconPath"></param>
        /// <returns></returns>
        public static string GetDocumentCategoryImageUrl(string filePath, string defaultIconPath)
        {
            defaultIconPath = "~/styles/images/Icons/file.jpg";
            string rv = defaultIconPath;

            string fileExtension = Path.GetExtension(filePath).ToLower();

            switch (fileExtension)
            {
                case ".doc":
                case ".rtf":
                    rv = "~/styles/images/Icons/word.gif";
                    break;
                case ".txt":
                    rv = "~/styles/images/Icons/text.gif";
                    break;
                case ".csv":
                case ".xls":
                    rv = "~/styles/images/Icons/excel.gif";
                    break;
                case ".pdf":
                    rv = "~/styles/images/Icons/pdf.gif";
                    break;
                case ".htm":
                case ".html":
                    rv = "~/styles/images/Icons/html.gif";
                    break;
                case ".ppt":
                    rv = "~/styles/images/Icons/powerpoint.gif";
                    break;
                case ".mpp":
                    rv = "~/styles/images/Icons/project.gif";
                    break;
                case ".mdb":
                    rv = "~/styles/images/Icons/access.gif";
                    break;
                case ".flv":
                case ".swf":
                    rv = "~/styles/images/Icons/application_flash.gif";
                    break;
                case ".xml":
                    rv = "~/styles/images/Icons/xml.gif";
                    break;
                case ".wmv":
                case ".avi":
                case ".mpeg":
                case ".mpg":
                    rv = "~/styles/images/Icons/movie.gif";
                    break;
            }

            return rv;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string GetStringFromTextFile(string fullPath)
        {
            StreamReader sr = default(StreamReader);
            string rv = "";

            if (System.IO.File.Exists(fullPath))
            {
                sr = new StreamReader(fullPath);

                while (sr.Peek() > -1)
                {
                    try
                    {
                        rv += sr.ReadLine().Trim();
                    }
                    catch (Exception ex)
                    {


                    }
                }

                //close sr
                sr.Close();
            }

            return rv;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string[] GetThemes()
        {
            string[] rv = null;
            string cacheKey = "SiteThemesStringArray";

            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                rv = (string[])HttpContext.Current.Cache[cacheKey];
            }
            else
            {
                string themesDirPath = HttpContext.Current.Server.MapPath("~/App_Themes");
                string[] themes = System.IO.Directory.GetDirectories(themesDirPath);
                for (int i = 0; i <= themes.Length - 1; i++)
                {
                    themes[i] = System.IO.Path.GetFileName(themes[i]);
                }
                CacheDependency dep = new CacheDependency(themesDirPath);
                HttpContext.Current.Cache.Insert(cacheKey, themes, dep);
                rv = themes;
            }

            return rv;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logFilePath"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool WriteToLogFile(string logFilePath, string message)
        {
            bool rv = false;

            //file exists
            if (System.IO.File.Exists(logFilePath))
            {

                //create streamwriter
                StreamWriter sr = null;
                try
                {
                    sr = new StreamWriter(logFilePath, true);
                }
                catch (Exception ex)
                {
                    //Throw ex
                    sr = null;
                }

                //write message
                if (sr != null)
                {
                    try
                    {
                        sr.WriteLine(message);
                        sr.Close();
                        rv = true;
                    }
                    catch (Exception ex)
                    {
                        //Throw ex
                        rv = false;
                    }

                }
            }
            else
            {
            }
            //Throw New ApplicationException("Log file does not exist at " & logFilePath)

            return rv;
        }
    }
}
