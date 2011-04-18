using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vwarDAL;
using System.Configuration;
using System.IO;
public partial class AdvancedDownload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    public void BuildOptions()
    {
        
        string NewTextureForamt = ConvertTexturesList.SelectedItem.Value;
        string NewModelFormat = ModelTypeDropDownList.SelectedItem.Value;
        int NewTextureSize = System.Convert.ToInt32(ResizeTexturesList.SelectedItem.Value);
        bool bConvertTextures = ConvertTextures.Checked;
        bool bResizeTextures = ResizeTextures.Checked;
        bool bReducePolys = ReducePolys.Checked;
        bool bConvert = ConvertFormat.Checked;
        float PolygonThresh = System.Convert.ToSingle(Threshold.Text);

        if (!bConvert)
            NewModelFormat = ".dae";

        Utility_3D.ConverterOptions opts = new Utility_3D.ConverterOptions();
        if (bConvertTextures)
            opts.EnableTextureConversion(NewTextureForamt);
        if (bResizeTextures)
            opts.EnableScaleTextures(NewTextureSize);
        if (bReducePolys)
            opts.EnablePolygonReduction(PolygonThresh);
        HttpContext.Current.Session["options"] = opts;
        HttpContext.Current.Session["format"] = NewModelFormat; 

    }
    public static Utility_3D.ConvertedModel GetAndConvertModel(String PID)
    {
        Utility_3D.ConverterOptions opts = (Utility_3D.ConverterOptions)HttpContext.Current.Session["options"];
        string NewModelFormat = (string)HttpContext.Current.Session["format"];

        DataAccessFactory factory = new vwarDAL.DataAccessFactory();
        vwarDAL.IDataRepository vd = factory.CreateDataRepositorProxy();

        ContentObject co = vd.GetContentObjectById(PID, false, false);

        byte[] filedata = vd.GetContentFileData(PID, co.Location);

        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + co.Location);
        HttpContext.Current.Response.ContentType = vwarDAL.DataUtils.GetMimeType(co.Location);

        Utility_3D _3d = new Utility_3D();
        _3d.Initialize(Website.Config.ConversionLibarayLocation);
        Utility_3D.Model_Packager pack = new Utility_3D.Model_Packager();
        Utility_3D.ConvertedModel model = pack.Convert(new System.IO.MemoryStream(filedata), "temp.zip", NewModelFormat, opts);
        return model;

    }
    public void DownloadButton_Click(object sender, EventArgs e)
    {
        string PID = HttpContext.Current.Request.QueryString["ContentObjectID"];
        BuildOptions();
        HttpContext.Current.Response.BinaryWrite(GetAndConvertModel(PID).data);
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static JsonWrappers.AdvancedDownloadPreviewSettings ViewButton_Click(string PID, bool bConvert, bool bConvertTextures, bool bResizeTextures, bool bReducePolys, string NewModelFormat, string NewTextureFormat, int NewTextureSize, float PolygonThresh,
                                                                                bool Smoothing, bool IgnoreBoundry, float MaxError, float MaxEdgeLength,string mode)
    {

        if (!bConvert)
            NewModelFormat = ".dae";

        Utility_3D.ConverterOptions opts = new Utility_3D.ConverterOptions();
        if (bConvertTextures)
            opts.EnableTextureConversion(NewTextureFormat);
        if (bResizeTextures)
            opts.EnableScaleTextures(NewTextureSize);
        if (bReducePolys)
            opts.EnablePolygonReduction(PolygonThresh);
       
            opts.SetPolygonReductionSmoothing(Smoothing);
            opts.SetPolygonReductionMaxLength(MaxEdgeLength);
            opts.SetPolygonReductionMaxError(MaxError);
            if (mode == "Simple")
                opts.SetPolygonReductionModeSimple();
            else
                opts.SetPolygonReductionModeComplex();

        HttpContext.Current.Session["options"] = opts;
        HttpContext.Current.Session["format"] = NewModelFormat;

       // string PID = HttpContext.Current.Request.QueryString["ContentObjectID"];
        DataAccessFactory factory = new vwarDAL.DataAccessFactory();
        vwarDAL.IDataRepository vd = factory.CreateDataRepositorProxy();

        ContentObject co = vd.GetContentObjectById(PID, false, false);


        JsonWrappers.AdvancedDownloadPreviewSettings jsReturnParams = new JsonWrappers.AdvancedDownloadPreviewSettings();
        jsReturnParams.FlashLocation = co.Location;

        Utils.FileStatus currentStatus = new Utils.FileStatus("", Utils.FormatType.UNRECOGNIZED);

            //tempFedoraCO.DisplayFile = currentStatus.filename.Replace("zip", "o3d").Replace("skp", "o3d");
        currentStatus.filename = co.Location;
        currentStatus.hashname = co.Location;

            jsReturnParams.IsViewable = true;
            jsReturnParams.BasePath = "../Public/";
            jsReturnParams.BaseContentUrl = "Model.ashx?temp=true&file=";
            jsReturnParams.O3DLocation = currentStatus.hashname.ToLower().Replace("zip", "o3d").Replace("skp", "o3d");
            jsReturnParams.FlashLocation = currentStatus.hashname;
            jsReturnParams.ShowScreenshot = true;
            jsReturnParams.UpAxis = co.UpAxis;
            jsReturnParams.UnitScale = co.UnitScale;

            string optionalPath = (co.Location.LastIndexOf("o3d", StringComparison.CurrentCultureIgnoreCase) != -1) ? "viewerTemp/" : "converterTemp/";
            string pathToTempFile = "~/App_Data/" + optionalPath + co.Location;
            using (FileStream stream = new FileStream(HttpContext.Current.Server.MapPath(pathToTempFile), FileMode.Create, FileAccess.Write))
            {
                Utility_3D.ConvertedModel model = GetAndConvertModel(PID);
                byte[] data = model.data;
                Utility_3D.ConvertedModel model2 = (new Utility_3D.Model_Packager()).Convert(new MemoryStream(model.data),"test.zip");
                jsReturnParams.Polygons = model2._ModelData.VertexCount.Polys;
                stream.Write(data,0,data.Length);
                stream.Close();
            }
        
        
        
        HttpContext.Current.Session["contentObject"] = co;
        return jsReturnParams;

       // HttpContext.Current.Response.BinaryWrite(GetAndConvertModel().data);
    }

    protected void ConvertFormat_CheckedChanged(object sender, EventArgs e)
    {
        BuildOptions();
    }
    protected void ConvertTextures_CheckedChanged(object sender, EventArgs e)
    {
        BuildOptions();
    }
    protected void ResizeTextures_CheckedChanged(object sender, EventArgs e)
    {
        BuildOptions();
    }
    protected void ReducePolys_CheckedChanged(object sender, EventArgs e)
    {
        BuildOptions();
    }
    protected void Threshold_TextChanged(object sender, EventArgs e)
    {
        BuildOptions();
    }
}