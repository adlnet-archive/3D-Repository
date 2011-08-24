<%--
Copyright 2011 U.S. Department of Defense

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
--%>



<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" EnableSessionState="True"
    CodeFile="Edit.aspx.cs" Inherits="Users_Edit" Title="Edit Model" MaintainScrollPositionOnPostback="true" %>
<%@ Register src="../Controls/Edit.ascx" tagname="EditModelControl" tagprefix="VwarWeb" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../scripts/o3djs/base.js"></script>
    <script type="text/javascript" src="../scripts/o3djs/simpleviewer.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.mousewheel.min.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/webgl-utils.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/osg.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/osgUtil.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/osgAnimation.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/osgGA.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/osgViewer.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/examples/viewer/webglviewer.js"></script>
    <script type="text/javascript" src="../Scripts/_lib/jquery.cookie.js" ></script>
    <script type="text/javascript" src="../Scripts/ViewerLoad.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
         <VwarWeb:EditModelControl ID="EditControl" runat="server" />
</asp:Content>
