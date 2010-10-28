<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Viewer3D.ascx.cs" Inherits="Controls_Viewer3D" %>

<div id="plugin_Wrapper" style="width: 550px; height: 550px">
    <div id="o3d" style="width: 90%; height: 90%; visibility: visible; position: absolute;
        margin: 25px">
    </div>
    <div style="color: red; margin-top: 50%; margin-left: 33%; position: absolute;" id="loading">
        Loading O3D, please wait...
    </div>
</div>
<div id="away3d_Wrapper" style="display: none;">
    <iframe id="flashFrame" class="ViewerItem"></iframe>
</div>
