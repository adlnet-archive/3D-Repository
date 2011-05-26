<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Viewer3D.ascx.cs" Inherits="Controls_Viewer3D" %>


<div id="viewer">
    <div id="plugin_Wrapper">
        <div id="o3d" style="width: 100%; height: 100%; visibility: visible; position: absolute;">
        </div>
        <div style="color: red; margin-top: 50%; margin-left: 33%; position: absolute;" id="loading">
            Loading O3D, please wait...
        </div>
    </div>
    <div id="away3d_Wrapper" style="display: none; height: 100%; width: 100%;">
        <iframe id="flashFrame" class="ViewerItem" style="width:100%; height:100%; border: none;"></iframe>
    </div>
    <div id="canvas_Wrapper" style="text-align: left; display: none; height: 100%; width: 100%; position: absolute; clip: rect(0px, 500px, 500px, 0px);">
       <canvas id="WebGLCanvas" tabindex=0 style="height: 100%; width: 100%; "></canvas>
    </div>
</div>
