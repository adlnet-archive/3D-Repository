<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Viewer3D.ascx.cs" Inherits="Controls_Viewer3D" %>
<div id="viewer">
    <div id="plugin_Wrapper">
        <div id="o3d" style="width: 90%; height: 90%; visibility: visible; position: absolute;
            margin: 25px">
        </div>
        <div style="color: red; margin-top: 50%; margin-left: 33%; position: absolute;" id="loading">
            Loading O3D, please wait...
        </div>
    </div>
    <div id="away3d_Wrapper" style="display: none; height: 100%; width: 100%;">
        <iframe id="flashFrame" class="ViewerItem" style="width:90%; height:90%; border: none;"></iframe>
    </div>
    <div id="canvas_Wrapper" style="display: none; height: 100%; width: 100%;">
       <canvas id="WebGLCanvas" style="position: absolute; left:0px; top: 0px; height: 100%; width: 100%; "></canvas>
    </div>
</div>
