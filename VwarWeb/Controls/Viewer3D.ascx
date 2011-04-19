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
    <div id="canvas_Wrapper" style="display: none; height: 100%; width: 100%; margin-top: 25px; position: absolute; clip: rect(0px, 500px, 500px, 25px);">
       <canvas id="WebGLCanvas" tabindex=0 style="height: 90%; width: 90%; "></canvas>
    </div>
</div>
