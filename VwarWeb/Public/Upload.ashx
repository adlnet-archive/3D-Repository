
<%@ WebHandler Language="C#" Class="Upload" %>

using System;
using System.Web;
using vwarDAL;
using System.IO;
using Utils;


public class Upload : IHttpHandler
{
    public const int MAX_RANDOM_VALUE = 100;
    
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
                
                string filenameTemplate = "~/App_Data/{0}{1}"; 

                using (FileStream stream = new FileStream(context.Server.MapPath(String.Format(filenameTemplate, hash, tempExtension)), FileMode.Create))
                {
                    using (BinaryWriter outwriter = new BinaryWriter(stream))
                    {
                        outwriter.Write(input);
                    }
                }

                Response.Write("{'success' : 'true', 'newfilename' : '"+ hash + tempExtension + "'}");

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

    /* Saves an image to a temporary directory to bind to Fedora when the object is created.
     * Params: The current HttpContext
     * Return: The filename of the newly uploaded temporary image file
     */
    private void SaveTempImage(HttpContext context)
    {
        try
        {

            HttpRequest Request = context.Request;
            
            var property = context.Request.Params["property"].Replace("_recognized", "").Replace("_viewable", "") ;
            var hashname = context.Request.Params["hashname"];
            var filename = context.Request.Params["qqfile"];

            byte[] input;
            if (Request.Browser.Type.Contains("IE"))
            {
                input = new byte[Request.Files["Filedata"].ContentLength];
                Request.Files["Filedata"].InputStream.Read(input, 0, Request.Files["Filedata"].ContentLength);
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

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }

}