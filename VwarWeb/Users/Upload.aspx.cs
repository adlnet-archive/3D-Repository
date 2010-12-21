using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;
using System.Threading;
using Ionic.Zip;
using vwarDAL;
using Utils;





public partial class Users_Upload : Website.Pages.PageBase
{

    public ContentObject tempFedoraObject
    {
        get
        {

            return (ContentObject)Session["contentObject"];
        }
        set
        {
            Session["contentObject"] = value;
        }
    }


    private FileStatus currentFileStatus
    {
        get
        {
            return (FileStatus)Session["fileStatus"];
        }
        set
        {
            currentFileStatus = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Page.Master.FindControl("SearchPanel") != null)
        {
            //hide the search panel
            this.Page.Master.FindControl("SearchPanel").Visible = false;
            
        }

        if (!Page.IsPostBack)
        {
            HttpContext.Current.Session["fileStatus"] = null; //Reset the FileStatus in case page was refreshed
        }
    }


    //AJAX-enabled web method to detect the format of the file
    //Params: the uploaded filename 
    //Returns: A JSON-encoded FileStatus object containing the extension and the type (Recognized, Unrecognized, or Viewable)
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static FileStatus DetectFormat(string filename)
    {

        FileStatus currentStatus = new FileStatus("", FormatType.UNRECOGNIZED);


        /*The temp filename is a sha1 hash
          We should generate a final filename from the title once we bind,
          since if we just left it as-is, people could name their stuff
          "sh!ttymodel.zip" or some other expletive that we may become
          responsible for. Plus a lot of people suck at naming files,
          and batched stuff could have "10933.zip" as the filename */
        currentStatus.hashname = filename; 
        currentStatus.msg = FileStatus.UnrecognizedMessage;

        //Check to see if it's an skp file
        if (string.Compare(currentStatus.hashname.Substring(currentStatus.hashname.LastIndexOf('.')), ".skp", true) == 0)
        {
            currentStatus.type = FormatType.VIEWABLE;
            currentStatus.extension = ".skp";
        }
        else
        {
            int recognizedCount = 0;
            using (ZipFile zip = ZipFile.Read(HttpContext.Current.Server.MapPath("~/App_Data/") + currentStatus.hashname))
            {
                foreach (string s in zip.EntryFileNames)
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(s);
                    if (FileStatus.GetType(f.Extension) == FormatType.VIEWABLE)
                    {
                        currentStatus.extension = f.Extension;
                        currentStatus.type = FormatType.VIEWABLE;
                        recognizedCount++;
                    }
                    else if (FileStatus.GetType(f.Extension) == FormatType.RECOGNIZED)
                    {
                        currentStatus.extension = f.Extension;
                        currentStatus.type = FormatType.RECOGNIZED;
                        currentStatus.msg = FileStatus.WarningMessage;
                        recognizedCount++;
                    }
                }
            }

            //Make sure there is only one recognized or viewable model format in the zip file
            //If multiple have been detected, set the format type and break
            if (recognizedCount > 1)
            {
                currentStatus.type = FormatType.MULTIPLE_RECOGNIZED;
                currentStatus.msg = FileStatus.MultipleRecognizedMessage;

            }
        }
        if (currentStatus.type == FormatType.UNRECOGNIZED ||
            currentStatus.type == FormatType.MULTIPLE_RECOGNIZED)
        {
            deleteTempFile(currentStatus.hashname);
        }
        else
        {
            HttpContext.Current.Session["fileStatus"] = currentStatus;
        }

        return currentStatus;
    }



    //Params: none
    //Returns: A JSON-encoded FileStatus object containing the extension and the type (Recognized, Unrecognized, or Viewable) PLUS conversion status
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static FileStatus Convert()
    {

        Utility_3D.Model_Packager pack = new Utility_3D.Model_Packager();
        Utility_3D _3d = new Utility_3D();
        _3d.Initialize(Website.Config.ConversionLibarayLocation);

        Utility_3D.ConvertedModel model = null;
        
        FileStatus status = (FileStatus)HttpContext.Current.Session["fileStatus"];
        using (FileStream stream = new FileStream(HttpContext.Current.Server.MapPath("~/App_data/" + status.hashname), FileMode.Open))
        {

            ContentObject tempFedoraObject = new ContentObject();
            try //convert the model
            {

                model = pack.Convert(stream, status.hashname);


                status.converted = "true";
                HttpContext.Current.Session["fileStatus"] = status;

                Utility_3D.Parser.ModelData mdata = model._ModelData;
                tempFedoraObject.NumPolygons = mdata.VertexCount.Polys;
                tempFedoraObject.NumTextures = mdata.ReferencedTextures.Length;
                tempFedoraObject.UpAxis = mdata.TransformProperties.UpAxis;
                tempFedoraObject.UnitScale = System.Convert.ToString(mdata.TransformProperties.UnitMeters);

                HttpContext.Current.Session["contentObject"] = tempFedoraObject;


                //Save the O3D file for the viewer into a temporary directory
                var tempfile = HttpContext.Current.Server.MapPath("~/App_data/viewerTemp/" + status.hashname);
                using (System.IO.FileStream savefile = new FileStream(tempfile, FileMode.CreateNew))
                {
                    byte[] filedata = new Byte[model.data.Length];
                    model.data.CopyTo(filedata, 0);
                    savefile.Write(model.data, 0, (int)model.data.Length);
                }
                ConvertFileToO3D(HttpContext.Current, tempfile);
                File.Delete(tempfile);
            }
            catch (Exception e) //Error while converting
            {
                stream.Close();
                //FileStatus.converted is set to false by default, no need to set
                status.msg = FileStatus.ConversionFailedMessage; //Add the conversion failed message
                deleteTempFile(status.hashname);
                HttpContext.Current.Session["fileStatus"] = null; //Reset the FileStatus for another upload attempt
                
            }
            

        }
        return status;

    }

    //Params: none
    //Returns: none
    //Description: Clears the Session variables and stored temp files from the server
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static void UploadReset(string filename)
    {    
        //Delete the temp file if it exists
        if (File.Exists(HttpContext.Current.Server.MapPath("~/App_Data/" + filename)))
        {
            deleteTempFile(filename);
        }

        //Delete the FileStatus from session if it exists
        if(HttpContext.Current.Session["fileStatus"] != null)
        {
            HttpContext.Current.Session["fileStatus"] = null;
        }

         //Delete the model from session if it exists
        if(HttpContext.Current.Session["model"] != null)
        {
            HttpContext.Current.Session["model"] = null;
        }
        
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static ViewerLoadParams Step1_Submit(string TitleInput, string DescriptionInput, string TagsInput)
    {
        FileStatus currentStatus = (FileStatus)HttpContext.Current.Session["fileStatus"];
        currentStatus.filename = TitleInput.Trim().Replace(' ', '_') + ".zip";

        ContentObject tempFedoraCO = (ContentObject)HttpContext.Current.Session["contentObject"];
        tempFedoraCO.Title = TitleInput.Trim();
        tempFedoraCO.Description = DescriptionInput.Trim();
        tempFedoraCO.Location = currentStatus.hashname;


        //Add the keywords
        if (TagsInput.LastIndexOf(',') == -1) //They used whitespace as delimiter
        {
            tempFedoraCO.Keywords = String.Join(",", TagsInput.Split(' '));
        }
        else
        {
            tempFedoraCO.Keywords = TagsInput;
        }

        ViewerLoadParams jsReturnParams = new ViewerLoadParams();

        /* If viewable, we go to the set axis and scale step
         * If it's recognized, then we skip to the thumbnail step.
         */
        if (currentStatus.type == FormatType.VIEWABLE)
        {
            tempFedoraCO.DisplayFile = currentStatus.hashname.Replace("zip", "o3d");
            jsReturnParams.IsViewable = true;
            jsReturnParams.BasePath = "../Public/";
            jsReturnParams.BaseContentUrl = "Model.ashx?temp=true&file=";
            jsReturnParams.FlashLocation = tempFedoraCO.Location;
            jsReturnParams.O3DLocation = tempFedoraCO.DisplayFile;
            jsReturnParams.ShowScale = true;
            jsReturnParams.ShowScreenshot = true;
            jsReturnParams.UpAxis = tempFedoraCO.UpAxis;
            jsReturnParams.UnitScale = tempFedoraCO.UnitScale;
        }
        else if (currentStatus.type == FormatType.RECOGNIZED)
        {
            tempFedoraCO.DisplayFile = "N/A";
        }

        HttpContext.Current.Session["contentObject"] = tempFedoraCO;
        return jsReturnParams;

        
    }

    [System.Web.Services.WebMethod()]
    public static void GetImage()
    {
        HttpContext context = HttpContext.Current;
        var filename = context.Request.Params["file"];
        string filepath = context.Server.MapPath("~/App_Data/imageTemp/" + filename);
        
        if (!String.IsNullOrEmpty(filename) && File.Exists(filepath))
        {
            context.Response.WriteFile(filepath);
        } 
        else
        {
            context.Response.StatusCode = 404;
            context.Response.Status = "Temporary resource was not found";
        }

        context.Response.End();
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static void DeleteImage(string filename)
    {
        string path = HttpContext.Current.Server.MapPath("~/App_Data/imageTemp/" + filename);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static void deleteTempFile(string filename)
    {
        File.Delete(HttpContext.Current.Server.MapPath("~/App_Data/" + filename));
    }

    private static string ConvertFileToO3D(HttpContext context, string path)
    {
        HttpRequest request = context.Request;

        var application = context.Server.MapPath("~/processes/o3dConverter.exe");//Path.Combine(Path.Combine(request.PhysicalApplicationPath, "bin"), "o3dConverter.exe");
        System.Diagnostics.ProcessStartInfo processInfo = new System.Diagnostics.ProcessStartInfo(application);
        processInfo.Arguments = String.Format("\"{0}\" \"{1}\"", path, path.ToLower().Replace("zip", "o3d"));
        processInfo.WindowStyle = ProcessWindowStyle.Hidden;
        processInfo.RedirectStandardError = true;
        processInfo.CreateNoWindow = true;
        processInfo.UseShellExecute = false;
        var p = Process.Start(processInfo);
        var error = p.StandardError.ReadToEnd();
        return path.ToLower().Replace("zip", "o3d");
    }
    

}
