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



    public bool IsReusable
    {
        get
        {
            return true;
        }
    }

}