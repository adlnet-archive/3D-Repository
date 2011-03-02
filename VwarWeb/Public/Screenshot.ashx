<%@ WebHandler Language="C#" Class="Screenshot" %>

using System.IO;
using System;
using System.Web;
using System.Web.SessionState;
using vwarDAL;
public class Screenshot : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {

        var session = context.Request.QueryString["Session"];
        var ContentObjectID = context.Request.QueryString["ContentObjectID"];
        var Session = context.Session;
        var format = context.Request.QueryString["Format"];
        var tempFilename = context.Request.QueryString["file"];

        if (context.Request.QueryString["temp"] == "true")
        {
            if (session == "true")
            {
                using (FileStream stream = new FileStream(context.Server.MapPath("~/App_Data/imageTemp/screenshot_" + tempFilename.Replace(".o3d", ".png")), FileMode.Open))
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, (int)stream.Length);
                    context.Response.BinaryWrite(buffer);
                }

                context.Response.End();
            }
            else
            {
                try
                {
                    using (FileStream fs = new FileStream(context.Server.MapPath("~/App_Data/imageTemp/screenshot_" + tempFilename.Replace(".o3d", ".png")), FileMode.Create))
                    {
                        using (BinaryWriter outwriter = new BinaryWriter(fs))
                        {
                            outwriter.Write(GetDecodedImageBytes(context.Request.InputStream, format));
                        }
                    }
                    return;
                }
                catch
                {
                    return;//the original logic did not have any error handling and worked, so we replicate this here
                }
                
            }
        }
        else
        {


            if (Session["DAL"] == null)
            {
                var factory = new DataAccessFactory();
                Session["DAL"] = factory.CreateDataRepositorProxy();
            }
            vwarDAL.IDataRepository dal = Session["DAL"] as IDataRepository;
            vwarDAL.ContentObject rv = dal.GetContentObjectById(ContentObjectID, false);


            if (session == "true")
            {
                using (Stream s = dal.GetContentFile(rv.PID, rv.ScreenShot))
                {
                    byte[] data = new byte[s.Length];
                    s.Read(data, 0, data.Length);
                    context.Response.BinaryWrite(data);
                    return;
                }
            }
            else
            {
                if (String.IsNullOrEmpty(rv.ScreenShot))
                {
                    if (format == "png")
                        rv.ScreenShot = "screenshot.png";
                    if (format == "jpg")
                        rv.ScreenShot = "screenshot.jpg";
                }

                byte[] decodedBytes;
                try
                {
                   decodedBytes  = GetDecodedImageBytes(context.Request.InputStream, format);
                   using (MemoryStream stream = new MemoryStream())
                   {
                       stream.Write(decodedBytes, 0, decodedBytes.Length);
                       
                       rv.ScreenShotId = dal.SetContentFile(stream, ContentObjectID, rv.ScreenShot);
                   }
                }
                catch
                {
                    return;
                }
                
                //try to get the file contents. If you could get them, that means it exists and
                //should be updated
                


                dal.UpdateContentObject(rv);
            }
        }
    }

    private byte[] GetDecodedImageBytes(System.IO.Stream inputStream, string format)
    {
        byte[] bytes = new byte[inputStream.Length];

        inputStream.Read(bytes, 0, bytes.Length);

        System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        String str = enc.GetString(bytes);
        byte[] decodedBytes = new byte[0];
        if (str.Length < 22 && format == "png")
            throw new Exception("Invalid data for PNG");
        if (format == "png")
            decodedBytes = Convert.FromBase64CharArray(str.Substring(22).ToCharArray(), 0, (int)str.Length - 22);
        if (format == "jpg")
            decodedBytes = Convert.FromBase64CharArray(str.ToCharArray(), 0, (int)str.Length);
        if (decodedBytes.Length == 0)
            throw new Exception("Image request contains no data");

        return decodedBytes;

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}