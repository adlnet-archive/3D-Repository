<%--
Copyright 2011 U.S. Department of Defense

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
--%>




<%@ WebHandler Language="C#" Class="Upload" %>

using System;
using System.Web;
using vwarDAL;
using System.IO;
using Utils;
using System.Drawing;


public class Upload : IHttpHandler
{
    /// <summary>
    /// 
    /// </summary>
    public const int MAX_RANDOM_VALUE = 100;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void ProcessRequest(HttpContext context)
    {
        HttpRequest Request = context.Request;
        HttpResponse Response = context.Response;

        if (Request.Params["image"] == "true")
        {
            if (Request.Params["method"] == "set")
            {
                SaveTempImage(context);
            }
            else if (Request.Params["method"] == "get")
            {
                WriteImage(context);
            }
        }
        else
        {
            try
            {
                // Get the data
                // HttpPostedFile postedfile = Request.Files["Filedata"];

                //Create a SHA-1 hash of the file contents to avoid collisions in the temp directory
                byte[] input;
                string tempExtension;
                if (Request.Browser.Type.Contains("IE"))
                {
                    input = new byte[Request.Files["qqfile"].ContentLength];
                    Request.Files["qqfile"].InputStream.Read(input, 0, Request.Files["qqfile"].ContentLength);
                    tempExtension = Path.GetExtension(Request.Files["qqfile"].FileName);
                }
                else
                {
                    input = Request.BinaryRead(Request.TotalBytes);
                    tempExtension = Path.GetExtension(Request.Params["qqfile"]);
                }
                //new byte[postedfile.ContentLength];
                // Stream filestream = postedfile.InputStream;
                // filestream.Read(input, 0, postedfile.ContentLength);
                System.Security.Cryptography.SHA1CryptoServiceProvider cryptoTransform = new System.Security.Cryptography.SHA1CryptoServiceProvider();

                //Write the binary data to the newly-named file
                //The filename also has a time-seeded random value attached to avoid I/O concurrency issues from cancel requests
                string hash = BitConverter.ToString(cryptoTransform.ComputeHash(input)).Replace("-", "") + new Random().Next(MAX_RANDOM_VALUE);

                string filenameTemplate = "~/App_Data/{0}";

                using (FileStream stream = new FileStream(context.Server.MapPath(String.Format(filenameTemplate, (hash + tempExtension).ToLower())), FileMode.Create))
                {
                    using (BinaryWriter outwriter = new BinaryWriter(stream))
                    {
                        outwriter.Write(input);
                    }
                }

                Response.Write("{'success' : 'true', 'newfilename' : '" + (hash + tempExtension).ToLower() + "'}");

            }
            catch (Exception e)
            {
                // If any kind of error occurs return the error json
                Response.Write("{'success' : 'false'}");
            }
            finally
            {
                Response.End();
            }
        }
    }
    /// <summary>
    /// Saves an image to a temporary directory to bind to Fedora when the object is created.
    /// Params: The current HttpContext
    /// Return: The filename of the newly uploaded temporary image file
    /// </summary>
    /// <param name="context"></param>
    private void SaveTempImage(HttpContext context)
    {
        try
        {

            HttpRequest Request = context.Request;

            var property = context.Request.Params["property"].Replace("_recognized", "").Replace("_viewable", "");
            var hashname = context.Request.Params["hashname"];
            var filename = context.Request.Params["qqfile"];

            byte[] input;
            if (Request.Browser.Type.Contains("IE"))
            {
                input = new byte[Request.Files["qqfile"].ContentLength];
                Request.Files["qqfile"].InputStream.Read(input, 0, Request.Files["qqfile"].ContentLength);
            }
            else
            {
                input = Request.BinaryRead(Request.TotalBytes);
            }

            string tempFilename = property + "_" + hashname.Replace(".zip", Path.GetExtension(context.Request.Params["qqfile"]));

            using (FileStream stream = new FileStream(context.Server.MapPath(String.Format("~/App_Data/imageTemp/{0}", tempFilename)), FileMode.Create))
            {
                stream.Write(input, 0, input.Length);
            }

            
            
            context.Response.Write("{'success' : 'true', 'newfilename' : '" + tempFilename + "'}");

        }
        catch (Exception e)
        {
            context.Response.Write("{'success' : 'false'}");
        }
        finally
        {
            context.Response.End();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    private void WriteImage(HttpContext context)
    {
        var filename = context.Request.Params["hashname"];
        string filepath = context.Server.MapPath("~/App_Data/imageTemp/" + filename);

        if (!String.IsNullOrEmpty(filename) && File.Exists(filepath))
        {
            context.Response.BinaryWrite(File.ReadAllBytes(filepath));
        }
        else
        {
            context.Response.StatusCode = 404;
            context.Response.Status = "Temporary resource was not found";
        }

        context.Response.End();
    }
    /// <summary>
    /// 
    /// </summary>
    public bool IsReusable
    {
        get
        {
            return true;
        }
    }
}
