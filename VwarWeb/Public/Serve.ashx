 <%@ WebHandler Language="C#" Class="Serve" %>

using System;
using System.Web;
using vwar;
using vwarDAL;
using System.IO;
public class Serve : IHttpHandler
{
    /// <summary>
    /// Get the query string parameter. Some of our code generates messed up params, and this finds the correct value regardless
    /// Deals with "_Amp_" used to seperate some params
    /// </summary>
    /// <param name="context">the HTTP context which contains the parameters</param>
    /// <param name="value">The string paired with the input value</param>
    /// <returns>The value of the input url parameter name</returns>
    public string GetQueryStringParam(HttpContext context, string value)
    {
        System.Collections.Specialized.NameValueCollection col = new System.Collections.Specialized.NameValueCollection();
        //iterate over all params
        foreach(string key in context.Request.QueryString.AllKeys)
        {
            //store normal params normally
            col[key] = context.Request.QueryString[key];
            //break up the value by _Amp_
            string[] array = context.Request.QueryString[key].Split(new string[] { "_Amp_" }, StringSplitOptions.RemoveEmptyEntries);
            //for every string pair separated by _Amp_
            foreach(string s in array)
            {
                //split it based on the equals sign, and set the value in the collection
                string[] pair = s.Split(new char[]{'='},StringSplitOptions.RemoveEmptyEntries);
                //If there is no second parameter, then the first value is the normally formated parameter
                if (pair.Length > 1)
                    col[pair[0]] = pair[1];
                else
                    col[key] = pair[0];   
            }
        }
        return col[value];
    }
    /// <summary>
    /// Switches base on URL parameter MODE to do all the necessary service operations
    /// </summary>
    /// <param name="context">The Current HTTP Context</param>
    public void ProcessRequest(HttpContext context) {

       
        //Get the current user name
        string username = context.User.Identity.Name;
        
        //If the current user is not correctly logged in, treat them as anonymous
        if (!context.User.Identity.IsAuthenticated)
        {
            username = vwarDAL.DefaultUsers.Anonymous[0];
        }
        //create an API wrapper for this context and user to handle all services
        APIWrapper api = new APIWrapper(username,context);
        //Find the pid in the query string
        string Pid = GetQueryStringParam(context,"pid");
        if(Pid == null)
            Pid = GetQueryStringParam(context,"ContentObjectID");
        if(Pid!=null)
            Pid = Pid.Replace('_', ':');
        
        //Get the mode for this handler, decides what it will do
        string mode = context.Request.QueryString["mode"];
        
        //Fake API key for operations, could be something specific or a special case which skips the database
        string APIKEY = "00-00-00";

        
        switch(mode)
        {
            //Going to download the correct model format based on the type of viewer    
            case "PreviewModel":
                {
                    //The viewer that will be viewing the content
                    string ViewerType = GetQueryStringParam(context,"ViewerType");
        
                    //If the viewer requesting the model is webgl - note that we must get the uncompressed data!
                    if (ViewerType.Contains("WebGL"))
                    {
                       WriteStream(context,api.GetModel(Pid,"json","uncompressed",APIKEY));
                    }
                    //If the viewer requesting the model is o3d
                    else if (ViewerType.Contains("o3d"))
                    {
                        WriteStream(context,api.GetModel(Pid,"o3d","",APIKEY));
                    }
                    //If the viewer requesting the model is flash
                    else if (ViewerType.Contains("Away3D"))
                    {
                        WriteStream(context,api.GetModel(Pid,"dae","",APIKEY));
                    }    
                }
                break;
            //Going to get an uncompressed texture for the viewer    
            case "PreviewTexture":
                {
                     //Get the name of the texture for the request
                    string TextureName = context.Request.QueryString["TextureName"];
                    Stream data = api.GetTextureFile(Pid,TextureName,APIKEY);
                    if (data != null)
                        WriteStream(context, data);
                    else
                        context.Response.StatusCode = 404;
                        
                }
                break;
            //Downloading the model to the users machine. Format can be 'original' for original download    
            case "DownloadModel":
                { 
                    string Format = context.Request.QueryString["Format"];
                    if(Format !="original")
                        WriteStream(context, api.GetModel(Pid, Format, "", APIKEY));
                    else
                        WriteStream(context, api.GetOriginalUploadFile(Pid, APIKEY));
                    
                      
                }
                break;
            //view a texture from the temp storage used by the ajax uploader    
            case "PreviewTempTexture":
                {
                     //The name of hte texture
                    string TextureName = context.Request.QueryString["TextureName"];
                    //The name of the zip file archive
                    string TempArchiveName = context.Request.QueryString["TempArchiveName"];
        
                    string path_to_converted_file = context.Server.MapPath("~/App_Data/converterTemp/") + TempArchiveName;
                    Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(path_to_converted_file);
                    Website.Common.WriteTextureToResponseFromZip(zip, TextureName, context);
                    zip.Dispose();                   
                }
                break;
            //Preview a model from temp filesystem storage based on the type of viewer    
            case "PreviewTempModel":
                {
                   //The viewer that will be viewing the content
                    string ViewerType = GetQueryStringParam(context,"ViewerType");
                    string TempArchiveName = GetQueryStringParam(context, "TempArchiveName");
       
                    //If the viewer requesting the model is webgl
                    if (ViewerType.Contains("WebGL"))
                    {
                        Stream data = new FileStream(context.Server.MapPath("~/App_Data/converterTemp/") + TempArchiveName, FileMode.Open);
                        Website.Common.WriteJSONtoResponse(data, context);
                        data.Close();
                    }
                    //If the viewer requesting the model is o3d
                    else if (ViewerType.Contains("o3d"))
                    {
                       Website.Common.WriteLocalFileToResponse(context.Server.MapPath("~/App_Data/viewerTemp/") + TempArchiveName.Replace(".zip", ".o3d"), context);
                    }
                    //If the viewer requesting the model is flash
                    else if (ViewerType.Contains("Away3D"))
                    {
                        Website.Common.WriteLocalFileToResponse(context.Server.MapPath("~/App_Data/converterTemp/") + TempArchiveName, context);
                    }
                }        
                break;
            //Get the screenshot for the model    
            case "GetScreenshot":
                {
                    WriteStream(context, api.GetScreenshot(Pid, APIKEY));
                }
                break;
            //Get the thumbnail for them model    
            case "GetThumbnail":
                {
                    WriteStream(context, api.GetThumbnail(Pid, APIKEY)); 
                }
                break;
            //Get the developer logo    
            case "GetDeveloperLogo":
                {
                    WriteStream(context, api.GetDeveloperLogo(Pid, APIKEY));
                }
                break;
            //Get the sponsor logo    
            case "GetSponsorLogo":
                {
                    WriteStream(context, api.GetSponsorLogo(Pid, APIKEY));
                }
                break;
            case "GetSupportingFile":
                {
                    string SupportingFileName = GetQueryStringParam(context, "SupportingFileName");
                    WriteStream(context, api.GetSupportingFile(Pid, SupportingFileName , APIKEY));
                }
                break;    
            default :
                {
                    
                }
                break;  
        }
        api.Dispose();
        if(context.User.Identity.IsAuthenticated == false)
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
        else
        context.Response.Cache.SetCacheability(HttpCacheability.Private);
        context.Response.Cache.SetMaxAge(new TimeSpan(1,0,0,0,0));
    }
    /// <summary>
    /// Write a stream to the HTTP Context
    /// </summary>
    /// <param name="context">the HTTP context which contains the parameters</param>
    /// <param name="io">the stream to write to the response</param>
    private void WriteStream(HttpContext context, Stream io)
    {
        if (io == null)
            return;
        byte[] data = new byte[io.Length];
        io.Read(data, 0, (int)io.Length);
        context.Response.BinaryWrite(data);
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}