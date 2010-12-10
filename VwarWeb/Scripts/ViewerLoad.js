var currentLoader;
var currentMode = "";

//Hooks for the testing system to check the state of things.
function GetLoadingComplete() {
    swfDiv = document.getElementById("flashFrame").contentWindow.document.getElementById('test3d');
    if (currentLoader.viewerMode == "o3d")
        return g_finished;
    else
        return swfDiv.GetLoadingComplete();
}
function GetCurrentUpAxis() {

    swfDiv = document.getElementById("flashFrame").contentWindow.document.getElementById('test3d');
    if (currentLoader.viewerMode == "o3d")
        return g_upaxis;
    else
        return swfDiv.GetCurrentUpAxis();
}
function TakeScreenShot() {

    swfDiv = document.getElementById("flashFrame").contentWindow.document.getElementById('test3d');
    if (currentLoader.viewerMode == "o3d")
        screenshot();
    else
        swfDiv.TakeScreenShot();
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

function GetCurrentUnitScale() {


    swfDiv = document.getElementById("flashFrame").contentWindow.document.getElementById('test3d');
    if (currentLoader.viewerMode == "o3d")
        return g_unitscale;
    else
        swfDiv.GetCurrentUnitScale();
}
function SetUnitScale(s) {

    swfDiv = document.getElementById("flashFrame").contentWindow.document.getElementById('test3d');
    if (currentLoader.viewerMode == "o3d")
        SetScale(s);
    else
        swfDiv.SetUnitScale(s);
}
function ViewerLoader(basePath, baseContentURL, flashLoc, o3dLoc, axis, scale, showScreenshot) {
    this.viewerLoaded = false;
    //flag to switch the screenshot button on and off in both viewers
    this.ShowScreenshotButton = showScreenshot;
    this.upAxis = axis;
    this.unitScale = scale;
    this.flashContentUrl = "";
    var path = window.location.href;
    //var basePath = window.location.protocol + "//" + window.location.host + "/Public/";
    var index = path.lastIndexOf('/');
  //  o3dfilename = path.substring(path.lastIndexOf('='), path.length); 
    var params = (axis != '' && scale != '') ? "&UpAxis=" + axis + "&UnitScale=" + scale : "";
    //need to modify the flash params to include the screenshot flag

    params += "&AllowScreenshotButton=" + this.ShowScreenshotButton;

   // params.replace(/&amp;/g, "_Amp_");
    this.flashContentUrl = basePath + "Away3D/ViewerApplication_back.html?URL=" + "http://" + window.location.host + "/Public/" + baseContentURL.replace("&", "_Amp_") + flashLoc + params;
    this.o3dContentUrl = basePath + baseContentURL + o3dLoc;
    this.viewerMode = (currentMode != "") ? currentMode : "o3d";
    
    this.pluginNotificationHtml = "<a id='HideButton' style='float: right; font-size: small; margin-right: 10px' href='#'>Hide</a><br />" +
                                  "You are using the Flash version of the 3D Viewer, which may cause performance issues when viewing large models. This page is optimized for the O3D Plugin." +
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
            if (viewerMode == 'o3d') {
                $('#plugin_Wrapper').show();
                init(o3dContentUrl, this.ShowScreenshotButton, upAxis, unitScale, this.o3dFailureCallback);
            } else {
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


    
