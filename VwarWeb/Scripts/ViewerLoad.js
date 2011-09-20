/**  
 * Copyright 2011 U.S. Department of Defense

 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at

 *     http://www.apache.org/licenses/LICENSE-2.0

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */


var currentLoader;
var currentMode = "";
var GotGL = false;
var MAX_POLYGONS = 26000;

//Hooks for the testing system to check the state of things.
function GetLoadingComplete() {
    var swfDiv = document.getElementById("flashFrame").contentWindow.document.getElementById('test3d'),
    	  loaded = false;
    
    if (currentLoader != null) {
        switch (currentLoader.viewerMode) {
            case "o3d":
                loaded = g_finished;
                break;
            case "away3d":
                loaded = swfDiv.GetLoadingComplete();
                break;
            case "WebGL":
                loaded = WebGL.LoadingComplete;
                break;
            default:
                break;
        }
    }
	
    return loaded;
}
function GetCurrentUpAxis() {

    swfDiv = document.getElementById("flashFrame").contentWindow.document.getElementById('test3d');
    if (currentLoader.viewerMode == "o3d")
        return g_upaxis;
    if (currentLoader.viewerMode == "away3d")
        return swfDiv.GetCurrentUpAxis();
    if (currentLoader.viewerMode == "WebGL")
        WebGlGetUpAxis();
}

function SetCurrentUpAxis(newAxis) {
    swfDiv = document.getElementById("flashFrame").contentWindow.document.getElementById('test3d');
    if (currentLoader.viewerMode == "o3d")
        SetAxis(newAxis);
    if (currentLoader.viewerMode == "away3d")
        swfDiv.SetUpVec(newAxis);
    if (currentLoader.viewerMode == "WebGL")
        WebGlSetUpVector(newAxis);
}
function TakeScreenShot() {

    swfDiv = document.getElementById("flashFrame").contentWindow.document.getElementById('test3d');
    if (currentLoader.viewerMode == "o3d")
        screenshot();
    if (currentLoader.viewerMode == "away3d")
        swfDiv.TakeScreenShot();
    if (currentLoader.viewerMode == "WebGL")
        WebGLScreenshot();
}

function SetViewerMode(mode) {
    currentMode = mode;
    if (currentLoader != null) {
        currentLoader.viewerMode = currentMode;
    }
}
function GetViewerMode() {
    return currentMode;
}

//This should probably be moved into ViewerLoad.js, and the ViewerLoader 
//should just become a wrapper for the viewer and its associated functionality
function ajaxImageSend(path, params, returnURL) {

    var xhr;
    try { xhr = new ActiveXObject('Msxml2.XMLHTTP'); }
    catch (e) {
        try { xhr = new ActiveXObject('Microsoft.XMLHTTP'); }
        catch (e2) {
            try { xhr = new XMLHttpRequest(); }
            catch (e3) { xhr = false; }
        }
    }

    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4) {

            if (xhr.status == 200) {

                var path2 = "../Public/ScreenShot.ashx" + returnURL;

                var image = document.getElementById("ThumbnailPreview_Viewable");
                //Legacy, remove with new upload stuff
                if (!image) image = document.getElementById("ctl00_ContentPlaceHolder1_EditControl_ThumbnailImage");
                preventcache += '1';

                if (currentLoader.isTemp)
                    path2 += "&temp=true";

                image.src = path2 + "&Session=true&keepfromcache=" + preventcache;
            }
            else
                alert("Error code " + xhr.status);
        }
    };

    if (currentLoader.isTemp)
        path += "&temp=true";

    xhr.open("POST", path, true);
    xhr.send(params);

}
function GetCurrentUnitScale() {


    swfDiv = document.getElementById("flashFrame").contentWindow.document.getElementById('test3d');
    if (currentLoader.viewerMode == "o3d")
        return g_unitscale;
    if (currentLoader.viewerMode == "away3d")
        return swfDiv.GetCurrentUnitScale();
    if (currentLoader.viewerMode == "WebGL")
        return WebGLGetUnitScale();
}
function SetUnitScale(s) {

    swfDiv = document.getElementById("flashFrame").contentWindow.document.getElementById('test3d');
    if (currentLoader.viewerMode == "o3d")
        SetScale(s);
    if (currentLoader.viewerMode == "away3d")
        swfDiv.SetUnitScale(s);
    if (currentLoader.viewerMode == "WebGL")
        WebGlSetUnitScale(s);
}

function ViewerLoader(basePath, displayFileName, displayFileId, axis, scale, showScreenshot, showScale, numPolygons, isTemp, pid) {

    if (isTemp == null) { isTemp = false; }
    this.isTemp = isTemp;
    this.viewerLoaded = false;
    //flag to switch the screenshot button on and off in both viewers
    this.ShowScreenshotButton = showScreenshot;
    this.ShowScale = showScale;
    this.upAxis = axis;
    this.unitScale = scale;
    this.flashContentUrl = "";

    var path = window.location.href;

    var index = path.lastIndexOf('/');
    var params = (axis != '' && scale != '') ? "&UpAxis=" + axis + "&UnitScale=" + scale : "";
    //need to modify the flash params to include the screenshot flag

    params += "&AllowScreenshotButton=" + this.ShowScreenshotButton.toString();


       
    var modelRequestParams = "?file=" + displayFileName;
    modelRequestParams += (isTemp) ? "&temp=true" : "&fileId=" + displayFileId;

    
    if (isTemp) {
        this.flashContentUrl = "../Public/Away3D/ViewerApplication_back.html?URL=" + "http://" + window.location.host + "/Public/Model.ashx" + "?"  + "temp=true" + "_Amp_file=" + displayFileName ;
    } else {
        this.flashContentUrl = "../Public/Away3D/ViewerApplication_back.html?URL=" + "http://" + window.location.host + "/Public/Model.ashx" + "?" + "pid=" + querySt("ContentObjectID") + "_Amp_file=" + displayFileName + params;
    }

    if (pid != null)
        modelRequestParams += "&pid=" + pid;
    else if (querySt("ContentObjectID") != null)
        modelRequestParams += "&pid=" + querySt("ContentObjectID");

    this.o3dContentUrl = basePath + modelRequestParams + "&format=o3d";
    this.webglContent = basePath + modelRequestParams + "&format=json";

    this.viewerMode = (currentMode != "") ? currentMode : "WebGL";

    this.pluginNotificationHtml = "<a id='HideButton' style='float: right; font-size: small; margin-right: 10px' href='#'>Hide</a><br />" +
                                  "You are using the Flash version of the 3D Viewer, which may cause performance issues when viewing some models." +
                                  "This page is optimized for the HTML5 canvas element in newer browsers like <a target='_blank' href='http://www.mozilla.com/en-US/firefox/fx/'>Firefox 4</a> " +
                                  "and <a href='http://www.google.com/chrome' target='_blank'>Chrome</a>, or alternatively the O3D Plugin for legacy browsers." +
                                  "<br /><br />" +
                                  "<span style='text-align: center;'>" +
                                  "<a href='http://tools.google.com/dlpage/o3d' target='_blank'>Click here</a> to download O3D.</a>" +
                                  "</span>";

    this.viewerStatusElement = "#ViewerStatus";

    this.ShowNotificationMessage = vShowNotification;
    this.HideNotificationMessage = vHideNotification;
    this.o3dFailureCallback = o3dFailCallback;

    this.LoadViewer = vLoad;
    this.ResetViewer = vReset;
    this.DestroyViewer = uninit;


    this.NumPolygons = numPolygons;

    currentLoader = this;

}



function BeginLoadingScreen() {

    var viewerdiv = $('#viewer');
    var div = document.createElement("DIV");
    div.style.position = 'absolute';
    div.style.zorder = 1000;
    div.id = "loadinggraphic";
    var gif = document.createElement("IMG");
    gif.src = "../styles/images/icons/loading2.gif";
    div.style.width = "100%";
    div.style.height = "100%";
    gif.style.width = "100%";
    gif.style.height = "100%";
    
    document.getElementById('viewer').appendChild(div);
    div.appendChild(gif);

}
function EndLoadingScreen() {

    $('#loadinggraphic').animate(
    { opacity: 0.0 },
     1000,
     function () {
         document.getElementById('viewer').removeChild(document.getElementById('loadinggraphic'));
     });
 }
 function ErrorLoadingScreen() {

    // EndLoadingScreen();

     var viewerdiv = $('#viewer');
     var div = document.createElement("DIV");
     div.style.position = 'absolute';
     div.style.zorder = 1000;
     div.id = "errorloadinggraphic";
     var gif = document.createElement("IMG");
     gif.src = "../styles/images/icons/loadingerror.gif";
     div.style.width = "100%";
     div.style.height = "100%";
     gif.style.width = "100%";
     gif.style.height = "100%";
    // gif.style.opacity = "0.01";
   
     document.getElementById('viewer').appendChild(div);
     div.appendChild(gif);
  $('#errorloadinggraphic').css("opacity", 0.0);
     $('#errorloadinggraphic').animate(
    { opacity: 1.0 },
     1000);
 }
function vLoad() {
    with (this) {



        if (!viewerLoaded) {


            
            //Try to load the o3d viewer
            if (viewerMode == "WebGL") {
               
                $('#canvas_Wrapper').show();
                $('#plugin_Wrapper').hide();
                GotGL = initWebGL(this.webglContent, this.ShowScreenshotButton, this.upAxis, this.unitScale);
                if (!GotGL) {
                    // alert("WebGL not supported!");
                    viewerMode = 'o3d';
                    $('#canvas_Wrapper').hide();
                } else {
                    BeginLoadingScreen();
                }
            }
            if (viewerMode == 'o3d') {

                $('#plugin_Wrapper').show();
                BeginLoadingScreen();
                init(o3dContentUrl, this.ShowScreenshotButton, this.upAxis, this.unitScale, this.o3dFailureCallback);
            }
            else if (viewerMode == 'away3d') {
                
            $('#away3d_Wrapper').show();

                if (currentLoader.NumPolygons == undefined) {
                    var mdURL = "http://" + window.location.host + '/rest/_3DRAPI.svc/' + querySt('ContentObjectID') + '/metadata/json?ID=' + jQuery.cookie('APIKey');
                    $.ajax({
                        url: mdURL,
                        type: 'GET',
                        async: false,
                        contentType: 'application/json; charset=utf-8',
                        success: function (data) { currentLoader.NumPolygons = parseInt(data.NumPolygons) }
                    });
                }
                if (currentLoader.NumPolygons > MAX_POLYGONS) {
                    $('#flashFrame').remove();
                    $('#away3d_Wrapper').html("<br/><br/>Sorry, but this model is too large to be loaded with our Flash viewer. "
                                              + "Try using the latest versions of <a target='_blank' href='http://www.mozilla.com/en-US/firefox/fx/'>Firefox</a> "
                                              + "or <a href='http://www.google.com/chrome' target='_blank'>Chrome</a>.")
                                        .css({ textAlign: 'center' });
                    return;
                } else {
                    $('#flashFrame').attr("src", currentLoader.flashContentUrl);
                    currentLoader.ShowNotificationMessage();
                }
            }
            viewerLoaded = true;
            $('body').attr('onunload', 'uninit();');
        }
    }
}

function vReset() {
    with (this) {
        reset();
        $('#o3d').html('');
        viewerLoaded = false;
    }
}

function vShowNotification() {
    with (this) {
        $(viewerStatusElement).html(this.pluginNotificationHtml);
        $('#HideButton').live('click', vHideNotification);
        $(viewerStatusElement).fadeIn(500);
    }
}

function vHideNotification() {
    $(currentLoader.viewerStatusElement).hide();
    return false;
}

function o3dFailCallback(status, error, id, tag) {

    EndLoadingScreen();
    $('#plugin_Wrapper').hide();

    uninit();
    currentLoader.viewerLoaded = false;


    currentLoader.viewerMode = "away3d";
    currentLoader.LoadViewer();
}

    
