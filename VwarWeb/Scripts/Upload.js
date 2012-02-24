var browserVersion = -1;
var iconBase =  "../styles/images/Icons/";
var cancelled = false;
var ModelUploadFinished = false;
var ModelUploadResult;
var modelUploadRunning = false;
var currentPanel;
var CurrentHashname;
var imgBase = "../styles/images/";
var largeUploadButtonLocation = imgBase + "3DR-Upload-Icon.png";
var smallUploadButtonLocation = imgBase + "SmallUpload_Btn.png";
var loadingLocation = iconBase + "loading.gif";
var checkLocation = iconBase + "checkmark.gif";
var failLocation = iconBase + "xmark.png";
var warningLocation = iconBase + "warning.gif";
var thumbnailLoadingLocation = iconBase + "loadingThumbnail.gif";
var previewImageLocation = imgBase + "nopreview_icon.png";
var ScaleSlider;
var ViewableThumbnailUpload, RecognizedThumbnailUpload, DevLogoUpload, SponsorLogoUpload;
var ModelUploader;
var PermissionsManager;
var MODE = "";
var ModelConverted = false;
var SubmissionSuccess = false;


$(document).ready(function () {
    CreateWidgets();
    UpdateCss();
    BindEventHandlers();
});

function BindEventHandlers() {
    //Click handlers
    $('.FormatsLink').click(function () { $('#FormatsModal').dialog("open"); return false; });
    $('#SponsorInfoTab').click(function () { $("#Tab2Content").show(); });
    $('#CancelButton').click(function () { cancelModelUpload(); return false; });
    $('#nextbutton_upload').click(function () { step1_next(); return false; });
    $('#ViewableSnapshotButton').click(function () { TakeUploadSnapshot(); return false; });
    $('#Step2Panel').find('.BackButton').click(function () { step2_back(); return false; });
    $('#Step2Panel').find('.NextButton').click(function () { step2_next(); return false; });
    $('#Step3Panel').find('.BackButton').click(function () { step3_back(); return false; });
    $('#Step3Panel').find('.NextButton').click(function () { submitUpload(); return false; });
    $(".disabled").click(function () { return false; });
    $("#PermissionsLink").click(function () {
        if (!PermissionsManager)
            PermissionsManager = new PermissionsManager;
        PermissionsManager.open(); 
        return false; 
    });
   
    //Change Handlers
    $("#LicenseType").change(function (eventObject) {
        $(".license-selected").hide();
        var url, imgSrc;
        var newSelection = $(this).val();
        if (newSelection == ".publicdomain") {
            url = "http://creativecommons.org/publicdomain/mark/1.0/";
            imgSrc = "http://i.creativecommons.org/l/publicdomain/88x31.png";
        } else {
            var urlParam = newSelection.replace(/\./g, "");
            url = "http://creativecommons.org/licenses/" + urlParam + "/3.0/legalcode";
            imgSrc = "http://i.creativecommons.org/l/" + urlParam + "/3.0/88x31.png";
        }
        $("#LicenseImage").attr("src", imgSrc);
        $("#LicenseLink").attr("href", url);
        $(newSelection).addClass("license-selected");
        $(newSelection).show();
    });

    $('input[name="UpAxis"]').change(function (eventObj) {
        SetCurrentUpAxis($(this).val());
    });

    $(document).ajaxError(function (event, request, ajaxOptions, thrownError) {
        if (request.status == 401) 
            window.location.href = "../Public/Login.aspx?ReturnUrl=%2fUsers%2fUpload.aspx";
    });

    $(window).unload(function () { if (!SubmissionSuccess) { resetUpload(CurrentHashname); } });

}

function CreateWidgets() {

    ScaleSlider = new SliderWidget($("#scaleSlider"), $("#scaleText"), $('#unitType'), 1.0);

    $("#DetailsTabs").tabs();

    //Image upload widgets
    ViewableThumbnailUpload = new ImageUploadWidget("screenshot_viewable", $("#ThumbnailViewableWidget"));
    RecognizedThumbnailUpload = new ImageUploadWidget("screenshot_recognized", $("#ThumbnailRecognizedWidget"));
  
    DevLogoUpload = new ImageUploadWidget("devlogo", $("#DevLogoUploadWidget"));
    SponsorLogoUpload = new ImageUploadWidget("sponsorlogo", $("#SponsorLogoUploadWidget"));

    //Accordions
    $("#UploadControl").accordion({
        autoHeight: false,
        clearStyle: true,
        icons: false
    });
    $('#ViewerAdjustmentAccordion').accordion({
        autoHeight: false,
        clearStyle: true
    });

    //Modal dialog windows
    $('#SubmittingModalWindow').dialog({
        modal: true,
        autoOpen: false,
        open: function (event, ui) { $(this).parent().find('.ui-dialog-titlebar-close').hide(); },
        closeOnEscape: false,
        draggable: false,
        resizable: false,
        zindex: 3999
    });

    $('#FormatsModal').dialog({
        autoOpen: false,
        closeOnEscape: true,
        draggable: true,
        resizable: false,
        zindex: 3999,
        width: 300
    });

    $('#UnclassifiedWarningModal').dialog({
        modal: true,
        autoOpen: true,
        closeOnEscape: false,
        draggable: false,
        resizable: false,
        width: 640,
        zindex: 3999,
        open: function (event, ui) { $(this).parent().find('.ui-dialog-titlebar-close').hide(); },
        buttons: {
            "I agree to the above conditions": function () { $(this).dialog("close"); },
            "Get me outta here!": function () { window.location.href = "../Default.aspx"; }
        }
    });

    ModelUploader = new qq.FileUploaderBasic({
        button: document.getElementById("ModelUploadButton"),
        action: '../Public/Upload.ashx',
        allowedExtensions: ['zip', 'skp'],
        sizeLimit: 104857600, //110MB
        minSize: 512000,
        onSubmit: function (id, fileName) {
            cancelled = false;
            changeCurrentModelUploadStep('#modelUploadStatus', '#modelUploadIcon');
            if (ModelUploadFinished) { //delete the temporary data associated with the old model
                resetUpload(CurrentHashname);
            }
            ModelConverted = false;
            modelUploadRunning = true;
            $('#CancelButton').show();
            if (MODE != "") { //reset the progress bar and hide the steps since this has already attempted to be processed
                $('.resettable.upload').hide();
            } else { //Show the status panel for the first time
                $('#DetailsAndStatusPanel').slideDown("fast");
            }

            $('#modelUploadStatus').html("Uploading Model");
            $('#modelUploadIcon').attr("src", loadingLocation);

            if (browserVersion == -1) {
                $('#modelUploadProgress').show();
                $('#modelUploadProgress').progressbar();
                $('#modelUploadProgress').progressbar("option", "value", 0);
            }
            return true;
        },
        onProgress: function (id, file, bytesLoaded, totalBytes) {
            totalBytes *= 1.0; bytesLoaded *= 1.0;
            result = (bytesLoaded / totalBytes) * 100.0;
            $('#modelUploadProgress').progressbar("option", "value", result);
        },
        onComplete: function (id, fileName, responseJSON) {
            ModelUploadFinished = true;
            ModelUploadResult = responseJSON.success;
            if (responseJSON.success == "true") {
                if (!cancelled) {
                    CurrentHashname = responseJSON.newfilename;
                    if (browserVersion == -1) {
                        $('#modelUploadProgress').progressbar("option", "value", 100);
                        $('#modelUploadProgress').slideUp(400, function () { $('#modelUploadStatus').html("Upload Complete"); });
                    }
                    $('#modelUploadIcon').attr("src", checkLocation);

                    detectFormat(responseJSON.newfilename);

                } else {
                    resetUpload(responseJSON.newfilename); //Reset silently as user initiated cancel process
                }

            } else {
                $('#CancelButton').hide();
                if (!cancelled) {
                    $('#modelUploadProgress').slideUp(400, function () { $('#modelUploadStatus').html("Upload Failed"); });
                    $('#modelUploadIcon').attr("src", failLocation);
                    $('#modelUploadMessage').show();
                    $('#modelUploadMessage').html('An error occured while trying to upload your model. The server may be busy or down. Please try again.');
                }
            }

        }
    });
}

function UpdateCss() {
    browserVersion = getInternetExplorerVersion();
    if (browserVersion > -1 && browserVersion <= 7) {
        $("#Step3Panel .ImagePreviewArea").css('top', '7px');
        $("#LicenseDescriptionContainer").css('margin-top', '0');
        $("#ScreenshotUploadButton_Viewable").hide();
        $("#ViewableSnapshotButton").hide();
        $("#ChooseModelContainer").css('left', '0px');
        $("#nextbutton_upload").css('left', '0px');
        $("#BasicInfoHeader").css('margin-top', '20px');
    }

    $("#away3d_Wrapper").css('margin-top', '25px');

    $("#UnclassifiedWarningModal").parent().find(".ui-widget-content").css({ border: "none" });
    $("#UnclassifiedWarningModal").parent().find(".ui-dialog-buttonpane .ui-dialog-buttonset").css({ 'float': "none", textAlign: "center" });

    $([thumbnailLoadingLocation,
       loadingLocation,
       checkLocation,
       failLocation,
       warningLocation,
       smallUploadButtonLocation,
       largeUploadButtonLocation]).preload();

    $(".tabs-bottom .ui-tabs-nav, .tabs-bottom .ui-tabs-nav > *")
			.removeClass("ui-corner-all ui-corner-top")
			.addClass("ui-corner-bottom");

}




/* Changes the UI to show the process has been cancelled
*  and sets the cancelled flag to true.
*
*  Any AJAX success callbacks for step 1
*  should check this cancelled flag and initiate
*  another request to a web method that cleans up data that's
*  already been processed.
*/
function cancelModelUpload() {

    //Make sure it hasn't already been cancelled
    if (!cancelled) {
        cancelled = true; //set it immediately in case the user spams the cancel button
        $('.currentStepIcon').attr("src", failLocation);
        $('.currentStatus').html("Cancelled");

        //If a progressbar element exists, then it will  have the .progress class,
        //so we need to hide it
        $('.currentStatus').siblings('.progress').slideUp(400);
        $('#CancelButton').hide();
    }

}
/* Swaps the .currentXXX to the step being processed */
function changeCurrentModelUploadStep(newStepElement, newIconElement) {
    $('.currentStepIcon').removeClass('currentStepIcon');
    $('.currentStatus').removeClass('currentStatus');

    $(newIconElement).addClass('currentStepIcon');
    $(newStepElement).addClass('currentStatus');
}

/* A wrapper for an AJAX request that calls the 
* WebMethod to clean up step 1 temp data
*/
function resetUpload(filename) {
    $.ajax({
        type: "POST",
        url: "Upload.aspx/UploadReset",
        data: JSON.stringify({filename: filename}),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    });
}

function detectFormat(filename) {
    changeCurrentModelUploadStep('#formatDetectStatus', '#formatDetectIcon');
    $('#formatDetect').show();
    $('#formatDetectStatus').html("Detecting format...");
    $('#formatDetectIcon').attr("src", loadingLocation);

    $.ajax({
        type: "POST",
        url: "Upload.aspx/DetectFormat",
        data: JSON.stringify({ filename: filename }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (object, responseStatus, request) {

            if (!cancelled) {
                fileStatus = object.d;
                MODE = fileStatus.type;
                switch (fileStatus.type) {
                    case "UNRECOGNIZED":
                        $('#formatDetectStatus').html("Unrecognized Format");
                        $('#formatDetectIcon').attr("src", failLocation);
                        $('#formatDetectMessage').show();
                        $('#formatDetectMessage').html(fileStatus.msg);
                        break;

                    case "MULTIPLE_RECOGNIZED":
                        $('#formatDetectStatus').html("Multiple Models Detected");
                        $('#formatDetectIcon').attr("src", failLocation);
                        $('#formatDetectMessage').show();
                        $('#formatDetectMessage').html(fileStatus.msg);
                        break;

                    case "RECOGNIZED":
                        $('#formatDetectStatus').html("Format Detected: " + fileStatus.extension);
                        $('#formatDetectIcon').attr("src", warningLocation);
                        $('#formatDetectMessage').show();
                        $('#formatDetectMessage').html(fileStatus.msg);
                        $('#nextbutton_upload').show();
                        break;

                    case "VIEWABLE":
                        $('#formatDetectStatus').html("Format Detected: " + fileStatus.extension);
                        $('#formatDetectIcon').attr("src", checkLocation);
                        convertModel(fileStatus.filename);
                        break;

                    default:
                        $('#formatDetectStatus').html("Server Error");
                        $('#formatDetectMessage').html("Invalid response received from the server. Please try again later.");
                        $('#formatDetectIcon').attr("src", failLocation);
                }

                if (MODE != "VIEWABLE") {
                    $('#CancelButton').hide();
                    modelUploadRunning = false;
                }
            } else {
                resetUpload(filename);
            }

        }

    });
}

function convertModel(filename) {
    changeCurrentModelUploadStep('#conversionStatus', '#conversionIcon');
    $('#conversionStep').show();
    $('#conversionStatus').html("Preparing Model for Viewing");
    $('#conversionIcon').attr("src", loadingLocation);
    $.ajax({
        url: "Upload.aspx/Convert",
        type: "POST",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (object, textStatus, request) {
            if (!cancelled) {
                $('#CancelButton').hide(); //We should hide it either way b/c this is the last step
                MODE = object.d.type;
                if (object.d.converted == "true") {
                    $('#conversionStatus').html("Model Ready for Viewer");
                    $('#conversionIcon').attr("src", checkLocation);
                    $('#nextbutton_upload').show();
                } else {
                    if (object.d.type == "RECOGNIZED") {
                        $('#conversionStatus').html("Model Ready");
                        $('#conversionIcon').attr("src", warningLocation);
                        $('#conversionMessage').show();
                        $('#conversionMessage').html("We're sorry. We were unable to create a 3D view of this model. You may continue normally, but 3D viewing for this object will not be available.");
                        $('#nextbutton_upload').show();
                    }
                    else {
                        $('#conversionStatus').html("Conversion Failed");
                        $('#conversionIcon').attr("src", failLocation);
                        $('#conversionMessage').show();
                        $('#conversionMessage').html(object.d.msg);
                    }
                }
            } else {
                resetUpload(filename);
            }
            modelUploadRunning = false;
            ModelConverted = true;
        },
        error: function (request, textStatus, errorThrown) {
            $('#conversionStatus').html("Conversion Failed");
            $('#conversionIcon').attr("src", failLocation);
            $('#conversionMessage').show();
            $('#conversionMessage').html("An error occured while trying to convert your model. Please verify that it is not empty or damaged.");
        }
    });
}




function step1_next() {
    //Validate the title
    var titleText = document.getElementById('ctl00_ContentPlaceHolder1_Upload1_TitleInput').value;
    var reg = /^[a-zA-Z0-9 \-,!:.\/_*?]+$/;
    if (reg.test(titleText) == false) {
        $('#ctl00_ContentPlaceHolder1_Upload1_TitleInput').css("background-color", "#ffcccc");
        $('#TitleValidationMessage').show();
        return;
    } else {
        $('#ctl00_ContentPlaceHolder1_Upload1_TitleInput').css("background-color", "white");
        $('#TitleValidationMessage').hide();
    }
    $("#SubmittingModalWindow").dialog("open");
    //Send the other info
    $.ajax({
        type: "post",
        url: "upload.aspx/Step1_Submit",
        data: JSON.stringify({
            TitleInput: titleText,
            DescriptionInput: document.getElementById('ctl00_ContentPlaceHolder1_Upload1_DescriptionInput').value,
            TagsInput: document.getElementById('ctl00_ContentPlaceHolder1_Upload1_TagsInput').value
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (object, textStatus, request) {
            
            $("#SubmittingModalWindow").dialog("close");
            var panelBar = $find('ctl00_ContentPlaceHolder1_Upload1_UploadControl');
            var viewerLoadParams = object.d;
            if (viewerLoadParams.IsViewable) {


                $("#ViewableView").show();
                $("#RecognizedView").hide();


                var vLoader = new TempViewerLoader(viewerLoadParams.BasePath, viewerLoadParams.TempArchiveName,
                                               viewerLoadParams.UpAxis, viewerLoadParams.UnitScale,
                                               viewerLoadParams.NumPolygons);

                ScaleSlider.CurrentValue = viewerLoadParams.UnitScale;
                if (!ScaleSlider.Active) ScaleSlider.Activate();
                if (!ViewableThumbnailUpload.Active) ViewableThumbnailUpload.Activate(CurrentHashname); //the flash location is just <thehash>.zip
                RecognizedThumbnailUpload.Activate(CurrentHashname);


                $("#UploadControl").accordion("activate", 1);
                /*yet another ie7 css hack*/
                if (browserVersion > -1 && browserVersion <= 7) {
                    $("#ScreenshotUploadButton_Viewable").show();
                    $("#ViewableSnapshotButton").show();
                }
                setTimeout("currentLoader.LoadViewer()", 750); //The viewer will not work unless fully revealed
                if (viewerLoadParams.UpAxis != "") {
                    $('input[name="UpAxis"]').filter("[value='" + viewerLoadParams.UpAxis.toUpperCase() + "']").attr("checked", "checked");
                    SetCurrentUpAxis(viewerLoadParams.UpAxis.toUpperCase());
                }

            } else if (viewerLoadParams.IsViewable == false && MODE == "RECOGNIZED") {

               
                $("#ViewableView").hide();
                $("#RecognizedView").show();
                $("#UploadControl").accordion("activate", 1);
                //if (!RecognizedThumbnailUpload.Active) 
                RecognizedThumbnailUpload.Activate(CurrentHashname);

            }

        }
    });
}

function step2_next() {
    var params = "";
    if (MODE == "VIEWABLE") {
        params = '{"ScaleValue" : "' + ScaleSlider.CurrentValue + '",' +
                     ' "UnitType" : "' + $(ScaleSlider.UnitType).text() + '",' +
                     ' "UpAxis" : "' + $('input:radio[name="UpAxis"]:checked').val() + '"}';
    } else {
        params = '{"ScaleValue" : "", "UnitType" : "", "UpAxis" : ""}';
    }
    $.ajax({
        type: "POST",
        url: "Upload.aspx/Step2_Submit",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: params,
        success: function (object, status, request) {
            if (!DevLogoUpload.Active) DevLogoUpload.Activate(CurrentHashname);
            if (!SponsorLogoUpload.Active) SponsorLogoUpload.Activate(CurrentHashname);
            RecognizedThumbnailUpload.Activate(CurrentHashname);
            if (currentLoader != null && currentLoader.viewerMode == "o3d") {
                currentLoader.ResetViewer();
            }

            var formVals = object.d;
            if (formVals.HasDefaults) {
                $("#DeveloperName").val(formVals.DeveloperName);
                $("#ArtistName").val(formVals.ArtistName);
                $("#DeveloperUrl").val(formVals.DeveloperUrl);
                $("#SponsorName").val(formVals.SponsorName);
                if (formVals.DeveloperLogoFilename != "") {
                    $("#DevLogoImage").attr("src", "../Public/Upload.ashx?image=true&method=get&hashname=" + formVals.DeveloperLogoFilename);
                }
                if (formVals.SponsorLogoFilename != "") {
                    $("#SponsorLogoImage").attr("src", "../Public/Upload.ashx?image=true&method=get&hashname=" + formVals.SponsorLogoFilename);
                }
            }

            $("#UploadControl").accordion("activate", 2);
        }
    });
}

function step2_back() {
    RecognizedThumbnailUpload.Active = false;
    ViewableThumbnailUpload.Active = false;
    if (currentLoader != null && currentLoader.viewerMode == "o3d") {
        currentLoader.ResetViewer();
    }
    $("#UploadControl").accordion("activate", 0);
}

function submitUpload() {
    if ($("#CertificationError").is(":visible")) {
        $("#CertificationError").hide();
    }
    if ($("#SubmittalError").is(":visible")) {
        $("#SubmittalError").hide();
    }

    var params = JSON.stringify({
                        DeveloperName :  $("#DeveloperName").val(),
                        ArtistName : $("#ArtistName").val(),
                        DeveloperUrl :  $("#DeveloperUrl").val(),
                        SponsorName : $("#SponsorName").val(),
                        SponsorUrl : $("#SponsorUrl").val(),
                        LicenseType: $.trim($("#LicenseType").val().replace(/\./g, " ")),
                        RequireResubmit: $("#RequireResubmitCheckbox:checked").val() !== undefined
                    });

    $("#SubmittingModalWindow").dialog("open");

    $.ajax({
        type: "POST",
        url: "Upload.aspx/SubmitUpload",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: params,
        success: function (object, status, request) {
            var responseMessages = object.d.split("|");
            if (responseMessages[0] == "fedoraError") {
                $("#SubmittalError").html(responseMessages[1]);
                $("#SubmittingModalWindow").dialog("close");
                $("#SubmittalError").css('display', 'inline-block');
            } else {
                SubmissionSuccess = true;
                window.location.href = "../Public/Model.aspx?ContentObjectID=" + object.d;
            }
            
        },
        error: function(request, textStatus, errorThrown) {
            $("#SubmittingModalWindow").dialog("close");
            $("#SubmittalError").css('display', 'inline-block');
        } 
    });
}

function step3_back() {
    DevLogoUpload.Active = false; SponsorLogoUpload.Active = false;
    $("#UploadControl").accordion("option", "disabled", false).accordion("activate", 1);
    if (currentLoader != null) {
        setTimeout("currentLoader.LoadViewer();", 750);
    }
}

function TakeUploadSnapshot() {
    $('#ThumbnailPreviewContainer').find('#ThumbnailPreview_Viewable').hide();
    $('#ThumbnailPreviewContainer').find('.LoadingImageContainer').show();
    TakeScreenShot();
}

function getInternetExplorerVersion()
// Returns the version of Internet Explorer or a -1
// (indicating the use of another browser).
{
    var rv = -1; // Return value assumes failure.
    if (navigator.appName == 'Microsoft Internet Explorer') {
        var ua = navigator.userAgent;
        var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
        if (re.exec(ua) != null)
            rv = parseFloat(RegExp.$1);
    }
    return rv;
}