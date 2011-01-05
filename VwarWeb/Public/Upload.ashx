<%@ WebHandler Language="C#" Class="Upload" %>

using System;
using System.Web;
using vwarDAL;
using System.IO;
using Utils;


public class Upload : IHttpHandler
{
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
                HttpPostedFile postedfile = Request.Files["Filedata"];

                //Create a SHA-1 hash of the file contents to avoid collisions in the temp directory
                byte[] input = new byte[postedfile.ContentLength];
                Stream filestream = postedfile.InputStream;
                filestream.Read(input, 0, postedfile.ContentLength);
                System.Security.Cryptography.SHA1CryptoServiceProvider cryptoTransform = new System.Security.Cryptography.SHA1CryptoServiceProvider();

                //Write the binary data to the newly-named file
                string hash = BitConverter.ToString(cryptoTransform.ComputeHash(input)).Replace("-", "");

                using (FileStream stream = new FileStream(context.Server.MapPath(String.Format("~/App_Data/{0}.zip", hash)), FileMode.Create))
                {
                    using (BinaryWriter outwriter = new BinaryWriter(stream))
                    {
                        outwriter.Write(input);
                    }
                }

                Response.Write(hash + ".zip");

            }
            catch (Exception e)
            {
                // If any kind of error occurs return a 500 Internal Server error
                Response.StatusCode = 500;
                Response.Write("An error occured");
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

            var property = context.Request.Params["property"].Replace("_recognized", "").Replace("_viewable", "") ;
            var hashname = context.Request.Params["hashname"];
            

            // Get the data
            HttpPostedFile postedfile = context.Request.Files["Filedata"];
            byte[] input = new byte[postedfile.ContentLength];
            using (Stream filestream = postedfile.InputStream)
            {
                filestream.Read(input, 0, postedfile.ContentLength);
            }
            string tempFilename = property + "_" + hashname.Replace(".zip", Path.GetExtension(postedfile.FileName));

            using (FileStream stream = new FileStream(context.Server.MapPath(String.Format("~/App_Data/imageTemp/{0}", tempFilename)), FileMode.Create))
            {
                using (BinaryWriter outwriter = new BinaryWriter(stream))
                {
                    outwriter.Write(input);
                }
            }

            context.Response.Write(tempFilename);

        }
        catch (Exception e)
        {
            context.Response.StatusCode = 500;
            context.Response.Write("An error occured");
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