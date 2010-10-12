<%@ WebHandler Language="C#" Class="Screenshot" %>

using System;
using System.Web;
using System.Web.SessionState;
public class Screenshot : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {

        var session = context.Request.QueryString["Session"];
        var ContentObjectID = context.Request.QueryString["ContentObjectID"];
        var factory = new vwarDAL.DataAccessFactory();
        vwarDAL.IDataRepository dal = factory.CreateDataRepositorProxy();
        vwarDAL.ContentObject rv = dal.GetContentObjectById(ContentObjectID, false);
        
        if (session == "true")
        {
            
            context.Response.BinaryWrite(dal.GetContentFileData(rv.PID,rv.ScreenShot));
            return;
        } 

     
        var format = context.Request.QueryString["Format"];
        byte[] bytes = new byte[context.Request.InputStream.Length];

        System.IO.StringWriter w = new System.IO.StringWriter();
        
        context.Request.InputStream.Read(bytes, 0, bytes.Length);
        
        System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        String str = enc.GetString(bytes);
        byte[] decodedBytes = new byte[0];
        if (str.Length < 22 && format == "png")
            return;
        if (format == "png")
            decodedBytes = Convert.FromBase64CharArray(str.Substring(22).ToCharArray(), 0, (int)str.Length - 22);
        if (format == "jpg")
            decodedBytes = Convert.FromBase64CharArray(str.ToCharArray(), 0, (int)str.Length);

        if (decodedBytes.Length == 0)
            return;
       
      

       if (format == "png")
           rv.ScreenShot = "screenshot.png";
       if (format == "jpg")
           rv.ScreenShot = "screenshot.jpg";
       
        //try to get the file contents. If you could get them, that means it exists and
        //should be updated
        try{
            byte[] testdata = dal.GetContentFileData(ContentObjectID, rv.ScreenShot);
            dal.UpdateFile(decodedBytes, ContentObjectID, rv.ScreenShot);
        }
            //if that failed, it doest not exist and should be uploaded
        catch(Exception e)
        {
            dal.UploadFile(decodedBytes, ContentObjectID, rv.ScreenShot);
        }
      
           
       
        dal.UpdateContentObject(rv);  
       
       
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}