var currentLoader;
var currentMode = "";

//Hooks for the testing system to check the state of things.
function GetLoadingComplete() {
    swfDiv = document.getElementById("flashFrame").contentWindow.document.getElementById('test3d');
    if (currentLoader.viewerMode == "o3d")
        return g_finished;
    if (currentLoader.viewerMode == "away3d")
        return swfDiv.GetLoadingComplete();
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
    if(currentLoader.viewerMode == "WebGL")
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
function ajaxImageSend(path, params,returnURL) {
 alert(path);
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

              image.src = path2 + "&Session=true&keepfromcache=" + preventcache;
              
              // $('#ThumbnailPreview_Viewable').attr({ width: '198', height: '198' });
          }
          else
              alert("Error code " + xhr.status);
      }
  };
  //alert(path);
  //alert(path);
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

function ViewerLoader(basePath, baseContentURL, flashLoc, o3dLoc, axis, scale, showScreenshot, showScale) {
    
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


    this.flashContentUrl = basePath + "Away3D/ViewerApplication_back.html?URL=" + "http://" + window.location.host + "/Public/" + baseContentURL.replace("&", "_Amp_") + flashLoc + params;
  
    this.o3dContentUrl = basePath + baseContentURL + o3dLoc;
    this.webglContent = basePath + baseContentURL + flashLoc + "&Format=json";
    this.viewerMode = (currentMode != "") ? currentMode : "WebGL";
    
    this.pluginNotificationHtml = "<a id='HideButton' style='float: right; font-size: small; margin-right: 10px' href='#'>Hide</a><br />" +
                                  "You are using the Flash version of the 3D Viewer, which may cause performance issues when viewing some models. This page is optimized for the O3D Plugin." +
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

    
    currentLoader = this;
   
}



function vLoad() {
    with (this) {
        if (!viewerLoaded) {
            //Try to load the o3d viewer
            if (viewerMode == "WebGL") {
                $('#canvas_Wrapper').show();
                $('#plugin_Wrapper').hide();
                var GotGL = initWebGL(this.webglContent, this.ShowScreenshotButton, this.upAxis, this.unitScale);
                if (!GotGL) {
                   // alert("WebGL not supported!");
                    viewerMode = 'o3d';
                }
        	//viewerMode = 'o3d';
            }
            if (viewerMode == 'o3d') {
                $('#plugin_Wrapper').show();
                init(o3dContentUrl, this.ShowScreenshotButton, this.upAxis, this.unitScale, this.o3dFailureCallback);
            }
            else if (viewerMode == 'away3d') {
                $('#away3d_Wrapper').show();
                $('#flashFrame').attr("src", this.flashContentUrl);
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
        
        $('#plugin_Wrapper').hide();

        uninit();
        currentLoader.viewerLoaded = false;

        currentLoader.ShowNotificationMessage();
        currentLoader.viewerMode = "away3d";
        currentLoader.LoadViewer();
    }

    
