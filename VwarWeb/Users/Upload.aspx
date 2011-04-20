<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" EnableSessionState="True"
    CodeFile="Upload.aspx.cs" Inherits="Users_Upload" Title="Upload" MaintainScrollPositionOnPostback="true" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register src="../Controls/NewUpload.ascx" tagname="Upload" tagprefix="uc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="../scripts/o3djs/base.js"></script>
    <script type="text/javascript" src="../scripts/o3djs/simpleviewer.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.8.7.custom.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.mousewheel.min.js"></script>
    <script type="text/javascript" src="../Scripts/ViewerLoad.js"></script>
    <script type="text/javascript" src="../Scripts/sliderWidget.js"></script>
    <script type="text/javascript" src="../Scripts/ImageUploadWidget.js"></script>
    <script type="text/javascript" src="../../js/stats.js"></script>

<script type="text/javascript" src="../Scripts/OSGJS/js/webgl-utils.js"></script>
<script type="text/javascript" src="../Scripts/OSGJS/js/osg.js"></script>
<script type="text/javascript" src="../Scripts/OSGJS/js/osgUtil.js"></script>
<script type="text/javascript" src="../Scripts/OSGJS/js/osgAnimation.js"></script>
<script type="text/javascript" src="../Scripts/OSGJS/js/osgGA.js"></script>
<script type="text/javascript" src="../Scripts/OSGJS/js/osgViewer.js"></script>
<script type="text/javascript" src="../Scripts/OSGJS/examples/viewer/webglviewer.js"></script>

    <script type="text/javascript" src="../Scripts/swfupload/vendor/swfupload/swfupload.js" ></script>
    <%--<script type="text/javascript" src="../Scripts/swfupload/src/jquery.swfupload.js"></script>--%>
    <script type="text/javascript" src="../Scripts/fileuploader.js"></script>
    <script type="text/javascript" src="../Scripts/Upload.js"></script>
    <link type="text/css" rel="Stylesheet" href="../Stylesheets/UploadStyle.css" />
    <link type="text/css" rel="Stylesheet" href="../Stylesheets/fileuploader.css" />
    <script type="text/javascript">
        function SendThumbnailJpg(shot) {
            alert("ExternalInterface worked!");
            ajaxImageSend("../Public/ScreenShot.ashx" + gURL + '&Format=jpg', shot);
        }
    </script>
   <%-- <link href="../App_Themes/Default/jquery-ui-1.8.7.custom.css" type="text/css" rel="Stylesheet"
        runat="server" />
    <link href="../App_Themes/Default/UploadStyle.css" type="text/css" rel="Stylesheet" />--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
         <uc1:Upload ID="Upload1" runat="server" />
</asp:Content>
