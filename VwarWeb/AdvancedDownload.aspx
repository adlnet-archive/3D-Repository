<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdvancedDownload.aspx.cs"
    Inherits="AdvancedDownload" MasterPageFile="~/MasterPage.master" Title="Advanced Download | 3D Repository" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="VwarWeb" TagName="Viewer3D" Src="~/Controls/Viewer3D.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta property="og:title" content="Welcome to 3DR" />
    <meta property="og:description" content="A (free!) platform for sharing 3D content across a variety of formats." />
    <meta property="og:image" content="http://3dr.adlnet.gov/Images/meta_image.png" />
    <style type="text/css">
        .radRotatoritemTemplate
        {
            height: 200px;
            width: 170px;
            float: left;
            margin: 40px 0;
        }
        
        
        
        .rrClipRegion
        {
            border: 0;
            vertical-align: middle;
        }
        
        .rrItem
        {
            text-align: center;
        }
        
        .rtsSelected
        {
            border-top: 1px solid;
        }
        
        
        .rotatorView
        {
            background-color: White;
            border: 1px solid #7f7f7f;
            width: 864px;
            margin: 16px auto;
        }
        
        .rrItemsList
        {
            height: 250px;
            width: 864px;
        }
        
        #outerBoxWrapper
        {
            width: 900px;
            z-index: -1;
            margin-left: auto;
            margin-right: auto;
            position: relative;
            border: 1px solid;
            top: -1px;
            background: none repeat scroll 0 0 #FFFFFF;
        }
        
        #outerBoxBody
        {
            margin-left: auto;
            margin-right: auto;
            width: 866px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="../Scripts/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.8.7.custom.min.js"></script>
    <script type="text/javascript" src="../Scripts/ViewerLoad.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/webgl-utils.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/osg.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/osgUtil.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/osgAnimation.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/osgGA.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/osgViewer.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/examples/viewer/webglviewer.js"></script>
    <script type="text/javascript">
var viewerLoaded = false;
var viewerMode = "WebGL";





function querySt(ji) {
    hu = window.location.search.substring(1);
    gy = hu.split("&");
    for (i = 0; i < gy.length; i++) {
        ft = gy[i].split("=");
        if (ft[0] == ji) {
            return ft[1];
        }
    }
}

    function LoadModel() {
        

        //string PID, bool bConvert, bool bConvertTextures, bool bResizeTextures, bool bReducePolys, string NewModelFormat, string NewTextureFormat, int NewTextureSize, float PolygonThresh
        $.ajax({
            type: "post",
            url: "AdvancedDownload.aspx/ViewButton_Click",
            data: JSON.stringify({
                PID: querySt('ContentObjectID'),
                bConvert: document.getElementById('ctl00_ContentPlaceHolder1_ConvertFormat').checked,
                bConvertTextures: document.getElementById('ctl00_ContentPlaceHolder1_ConvertTextures').checked,
                bResizeTextures: document.getElementById('ctl00_ContentPlaceHolder1_ResizeTextures').checked,
                bReducePolys: document.getElementById('ctl00_ContentPlaceHolder1_ReducePolys').checked,
                NewModelFormat: document.getElementById('ctl00_ContentPlaceHolder1_ModelTypeDropDownList')[document.getElementById('ctl00_ContentPlaceHolder1_ModelTypeDropDownList').selectedIndex].value,
                NewTextureFormat: document.getElementById('ctl00_ContentPlaceHolder1_ConvertTexturesList')[document.getElementById('ctl00_ContentPlaceHolder1_ConvertTexturesList').selectedIndex].value,
                NewTextureSize: document.getElementById('ctl00_ContentPlaceHolder1_ResizeTexturesList')[document.getElementById('ctl00_ContentPlaceHolder1_ResizeTexturesList').selectedIndex].value,
                PolygonThresh: document.getElementById('ctl00_ContentPlaceHolder1_Threshold').value,
                
                Smoothing:document.getElementById('ctl00_ContentPlaceHolder1_Smoothing').checked ,
                IgnoreBoundry:document.getElementById('ctl00_ContentPlaceHolder1_IgnoreBoundry').checked ,
                MaxError:document.getElementById('ctl00_ContentPlaceHolder1_MaxError').value ,
                MaxEdgeLength:document.getElementById('ctl00_ContentPlaceHolder1_MaxEdgeLength').value ,
                mode:document.getElementById('ctl00_ContentPlaceHolder1_ReductionType')[document.getElementById('ctl00_ContentPlaceHolder1_ReductionType').selectedIndex].value 
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (object, textStatus, request) {


                var viewerLoadParams = object.d;



                var vLoader = new ViewerLoader(viewerLoadParams.BasePath, viewerLoadParams.BaseContentUrl, viewerLoadParams.FlashLocation,
                                                       viewerLoadParams.O3DLocation, viewerLoadParams.UpAxis, viewerLoadParams.UnitScale, viewerLoadParams.ShowScreenshot, viewerLoadParams.ShowScale);

                alert(viewerLoadParams.Polygons);
                vLoader.LoadViewer();

            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }


    </script>
    <table class="CenteredTable" cellpadding="4" border="0">
        <tr>
            <td height="600" width="564" class="ViewerWrapper">
                <VwarWeb:Viewer3D ID="viewer" align="center" runat="server" />
            </td>
        </tr>
    </table>
    <script type="text/javascript" src="../scripts/o3djs/base.js"></script>
    <script type="text/javascript" src="../scripts/o3djs/simpleviewer.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.8.7.custom.min.js"></script>
    <script type="text/javascript" src="../Scripts/ViewerLoad.js"></script>
    <script type="text/javascript" src="../Scripts/sliderWidget.js"></script>
    <script type="text/javascript" src="../Scripts/ImageUploadWidget.js"></script>
    <script type="text/javascript" src="../../js/stats.js"></script>
    <asp:CheckBox ID="ConvertFormat" runat="server" Text="Convert" />
    <asp:DropDownList ID="ModelTypeDropDownList" runat="server" EnableEmbeddedSkins="false">
        <Items>
            <asp:ListItem runat="server" Text="Collada" Value=".dae" Selected="true" />
            <asp:ListItem runat="server" Text="OBJ" Value=".obj" />
            <asp:ListItem runat="server" Text="3DS" Value=".3DS" />
            <asp:ListItem runat="server" Text="O3D" Value=".O3Dtgz" />
            <asp:ListItem runat="server" Text="FBX" Value=".fbx" />
            <asp:ListItem runat="server" Text="No Conversion" Value="" />
        </Items>
    </asp:DropDownList>
    <br />
    <asp:CheckBox ID="ConvertTextures" runat="server" Text="Convert Textures" />
    <asp:DropDownList ID="ConvertTexturesList" runat="server" EnableEmbeddedSkins="false">
        <Items>
            <asp:ListItem runat="server" Text="jpg" Value="jpg" Selected="true" />
            <asp:ListItem runat="server" Text="png" Value="png" />
            <asp:ListItem runat="server" Text="dds" Value="dds" />
            <asp:ListItem runat="server" Text="bmp" Value="bmp" />
        </Items>
    </asp:DropDownList>
    <br />
    <asp:CheckBox ID="ResizeTextures" runat="server" Text="Resize Textures" />
    <asp:DropDownList ID="ResizeTexturesList" runat="server" EnableEmbeddedSkins="false">
        <Items>
            <asp:ListItem runat="server" Text="1024" Value="1024" Selected="true" />
            <asp:ListItem runat="server" Text="512" Value="512" />
            <asp:ListItem runat="server" Text="256" Value="256" />
            <asp:ListItem runat="server" Text="128" Value="128" />
        </Items>
    </asp:DropDownList>
    <br />
    <asp:CheckBox ID="ReducePolys" runat="server" Text="ReducePolys" />
    <asp:TextBox ID="Threshold" Width="30" runat="server">1.0</asp:TextBox><br />
    <asp:CheckBox ID="IgnoreBoundry" runat="server" Text="Ignore Boundry" /><br />
    <asp:CheckBox ID="Smoothing" runat="server" Text="Smoothing" /><br />
    Max Error <asp:TextBox ID="MaxError" Width="30" runat="server">1.0</asp:TextBox><br />
    Max Edge Length <asp:TextBox ID="MaxEdgeLength" Width="30" runat="server">1.0</asp:TextBox><br />
    Mode <asp:DropDownList ID="ReductionType" runat="server" EnableEmbeddedSkins="false">
        <Items>
            <asp:ListItem runat="server" Text="Simple" Value="Simple" Selected="true" />
            <asp:ListItem runat="server" Text="Complex" Value="Complex" />
            </Items>
    </asp:DropDownList>

    <asp:ImageButton Style="vertical-align: bottom;" ID="DownloadButton" runat="server"
        Text="Download" ToolTip="Download" CommandName="DownloadZip" OnClick="DownloadButton_Click"
        ImageUrl="~/Images/Download_BTN.png" />
    <br />
    <br />
    <img style="vertical-align: bottom;" id="ViewButton" text="View" tooltip="View" src="/Images/Download_BTN.png"
        onclick="LoadModel();" />
</asp:Content>
