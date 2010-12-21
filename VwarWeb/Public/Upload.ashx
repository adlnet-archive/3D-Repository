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
            SaveTempImage(context);
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
            FileStatus fileStatus = (FileStatus)context.Session["fileStatus"];
            var property = context.Request.Params["property"];


            // Get the data
            HttpPostedFile postedfile = context.Request.Files["Filedata"];
            byte[] input = new byte[postedfile.ContentLength];
            Stream filestream = postedfile.InputStream;
            filestream.Read(input, 0, postedfile.ContentLength);

            string tempFilename = property + "_" + fileStatus.hashname.Replace("zip", Path.GetExtension(postedfile.FileName));

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



    public bool IsReusable
    {
        get
        {
            return true;
        }
    }

}