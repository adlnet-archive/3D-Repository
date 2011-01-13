$.fn.preload = function () {
    this.each(function () {
        $('<img/>')[0].src = this;
    });
}

var iconBase = "../Images/Icons/";
var cancelled = false;
var modelUploadFinished = false;
var modelUploadRunning = false;

var currentPanel;
var CurrentHashname;

var imgBase = "../Images/";
var largeUploadButtonLocation = imgBase + "3DR-Upload-Icon.png";
var smallUploadButtonLocation = imgBase + "SmallUpload_Btn.png";

var loadingLocation = iconBase + "loading.gif";
var checkLocation = iconBase + "checkmark.gif";
var failLocation = iconBase + "xmark.png";
var warningLocation = iconBase + "warning.gif";
var thumbnailLoadingLocation = iconBase + "loadingThumbnail.gif";

var ScaleSlider;
var ViewableThumbnailUpload, RecognizedThumbnailUpload, DevLogoUpload, SponsorLogoUpload;
var MODE = "";


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
        $('#ChooseModelContainer').swfupload('setButtonDisabled', false);
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
        data: '{ "filename" : "' + filename + '"}',
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
        data: '{ "filename" : "' + filename + '"}',
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
                        $('#ChooseModelContainer').swfupload('setButtonDisabled', false);
                        break;

                    case "MULTIPLE_RECOGNIZED":
                        $('#formatDetectStatus').html("Multiple Models Detected");
                        $('#formatDetectIcon').attr("src", failLocation);
                        $('#formatDetectMessage').show();
                        $('#formatDetectMessage').html(fileStatus.msg);
                        $('#ChooseModelContainer').swfupload('setButtonDisabled', false);
                        break;

                    case "RECOGNIZED":
                        $('#formatDetectStatus').html("Format Detected: " + fileStatus.extension);
                        $('#formatDetectIcon').attr("src", warningLocation);
                        $('#formatDetectMessage').show();
                        $('#formatDetectMessage').html(fileStatus.msg);
                        $('#nextbutton_upload').show();
                        $('#ChooseModelContainer').swfupload('setButtonDisabled', false);

                        break;

                    case "VIEWABLE":
                        $('#formatDetectStatus').html("Format Detected: " + fileStatus.extension);
                        $('#formatDetectIcon').attr("src", checkLocation);
                        convertModel(fileStatus.filename);
                        break;

                    default:
                        $('#formatDetectMessage').html("Invalid response received from the server. Please try again later.");
                        $('#formatDetectIcon').attr("src", failLocation);
                        $('#ChooseModelContainer').swfupload('setButtonDisabled', false);
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
                if (object.d.converted == "true") {
                    $('#conversionStatus').html("Model Ready for Viewer");
                    $('#conversionIcon').attr("src", checkLocation);
                    $('#ChooseModelContainer').swfupload('setButtonDisabled', false);
                    $('#nextbutton_upload').show();
                } else {

                    $('#conversionStatus').html("Conversion Failed");
                    $('#conversionIcon').attr("src", failLocation);
                    $('#conversionMessage').show();
                    $('#conversionMessage').html(object.d.msg);
                    $('#ChooseModelContainer').swfupload('setButtonDisabled', false);
                }
            } else {
                resetUpload(filename);
            }
            modelUploadRunning = false;
        }
    });
}




function step1_next() {

    //Validate the title
    var titleText = document.getElementById('ctl00_ContentPlaceHolder1_Upload1_TitleInput').value;
    var reg = /^[a-zA-Z0-9 ]+$/;
    if (reg.test(titleText) == false) {
        $('#ctl00_ContentPlaceHolder1_Upload1_TitleInput').css("background-color", "#ffcccc");
        $('#TitleValidationMessage').show();
        return;
    } else {
        $('#ctl00_ContentPlaceHolder1_Upload1_TitleInput').css("background-color", "white");
        $('#TitleValidationMessage').hide();
    }

    //Send the other info
    $.ajax({
        type: "post",
        url: "upload.aspx/Step1_Submit",
        data: '{ "TitleInput" : "' + titleText + '",' +
                  '  "DescriptionInput" : "' + document.getElementById('ctl00_ContentPlaceHolder1_Upload1_DescriptionInput').value + '",' +
                  '  "TagsInput" : "' + document.getElementById('ctl00_ContentPlaceHolder1_Upload1_TagsInput').value + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (object, textStatus, request) {
            var panelBar = $find('ctl00_ContentPlaceHolder1_Upload1_UploadControl');
            var viewerLoadParams = object.d;
            if (viewerLoadParams.IsViewable) {


                $("#ViewableView").show();
                $("#RecognizedView").hide();
                

                var vLoader = new ViewerLoader(viewerLoadParams.BasePath, viewerLoadParams.BaseContentUrl, viewerLoadParams.FlashLocation,
                                                   viewerLoadParams.O3DLocation, viewerLoadParams.UpAxis, viewerLoadParams.UnitScale, viewerLoadParams.ShowScreenshot, viewerLoadParams.ShowScale);

                
                if (!ScaleSlider.Active) ScaleSlider.Activate();
                if (!ViewableThumbnailUpload.Active) ViewableThumbnailUpload.Activate(viewerLoadParams.FlashLocation); //the flash location is just <thehash>.zip



                $("#UploadControl").accordion("activate", 1);
                setTimeout("currentLoader.LoadViewer()", 750); //The viewer will not work unless fully revealed
                if (viewerLoadParams.UpAxis != "") {
                    $('input[name="UpAxis"]').filter("[value='" + viewerLoadParams.UpAxis.toUpperCase() + "']").attr("checked", "checked");
                    SetCurrentUpAxis(viewerLoadParams.UpAxis.toUpperCase());
                }

            } else if (viewerLoadParams.IsViewable == false && MODE == "RECOGNIZED") {

                $("#ViewableView").hide();
                $("#RecognizedView").show();
                $("#UploadControl").accordion("activate", 1);
                if (!RecognizedThumbnailUpload.Active) RecognizedThumbnailUpload.Activate(CurrentHashname);

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
            if (currentLoader != null && currentLoader.viewerMode == "o3d") {
                currentLoader.ResetViewer();
            }

            var formVals = object.d;
            if (formVals.HasDefaults) {
                $("#DeveloperName").val(formVals.DeveloperName);
                $("#ArtistName").val(formVals.ArtistName);
                $("#DeveloperUrl").val(formVals.DeveloperUrl);
                $("#SponsorName").val(formVals.SponsorName);
                $("#DevLogoImage").attr("src", "../Public/Upload.ashx?image=true&method=get&hashname=" + formVals.DeveloperLogoFilename);
                $("#SponsorLogoImage").attr("src", "../Public/Upload.ashx?image=true&method=get&hashname=" + formVals.SponsorLogoFilename);
            }

            $("#UploadControl").accordion("activate", 2);
        }
    });
}

function step2_back() {
    if (currentLoader != null && currentLoader.viewerMode == "o3d") {
        currentLoader.ResetViewer();
    }
    $("#UploadControl").accordion("activate", 0);
}

function submitUpload() {
    var params = '{' +
                        '"DeveloperName" : "' + $("#DeveloperName").val() + '",' +
                        '"ArtistName" : "' + $("#ArtistName").val() + '",' +
                        '"DeveloperUrl" : "' + $("#DeveloperUrl").val() + '",' +
                        '"SponsorName" : "' + $("#SponsorName").val() + '",' +
                        '"SponsorUrl" : "' + $("#SponsorUrl").val() + '",' +
                        '"LicenseType" : "' + $.trim($("#LicenseType").val().replace(/\./g, " ")) + '"' +
                     '}';

    $("#SubmittingModalWindow").dialog("open");

    $.ajax({
        type: "POST",
        url: "Upload.aspx/SubmitUpload",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: params,
        success: function (object, status, request) {
            window.location.href = "../Public/Model.aspx?ContentObjectID=" + object.d; 
        }
    });

}

function step3_back() {
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



$(function () {
    $(window).unload(function () { resetUpload(CurrentHashname); });
    $(".disabled").click(function () { return false; });

    $("#UploadControl").accordion({
        autoHeight: false,
        clearStyle: true,
        icons: false

    });

    $('#SubmittingModalWindow').dialog({
        modal: true,
        autoOpen: false,
        closeOnEscape: false,
        draggable: false,
        resizable: false,
        zindex: 3999
    });

    $('#modelUploadProgress').progressbar();
    
    $([thumbnailLoadingLocation, loadingLocation, checkLocation, failLocation, warningLocation, smallUploadButtonLocation, largeUploadButtonLocation]).preload();
    ViewableThumbnailUpload = new ImageUploadWidget("screenshot_viewable", $("#ThumbnailViewableWidget"));
    RecognizedThumbnailUpload = new ImageUploadWidget("screenshot_recognized", $("#ThumbnailRecognizedWidget"));
    DevLogoUpload = new ImageUploadWidget("devlogo", $("#DevLogoUploadWidget"));
    SponsorLogoUpload = new ImageUploadWidget("sponsorlogo", $("#SponsorLogoUploadWidget"));

    /* add the tabs for the details step */
    $("#DetailsTabs").tabs();
    $(".tabs-bottom .ui-tabs-nav, .tabs-bottom .ui-tabs-nav > *")
			.removeClass("ui-corner-all ui-corner-top")
			.addClass("ui-corner-bottom");


    /* add the callback for the license type change */
    $("#LicenseType").change(function (eventObject) {
        $(".license-selected").hide();
        var newSelection = $(this).val();
        $(newSelection).addClass("license-selected");
        $(newSelection).show();
    });

    ScaleSlider = new SliderWidget($("#scaleSlider"), $("#scaleText"), $('#unitType'), 1.0);
    $('#ViewerAdjustmentAccordion').accordion({
        autoHeight: false,
        clearStyle: true
    });

    $('input[name="UpAxis"]').change(function (eventObj) {
        SetCurrentUpAxis($(this).val());
    });

    $('#ChooseModelContainer').swfupload({
        upload_url: "../Public/Upload.ashx",
        file_size_limit: "102400",
        file_types: "*.zip; *.skp",
        file_types_description: "Recognized 3DR format",
        file_upload_limit: "0",
        flash_url: "../Scripts/swfupload/vendor/swfupload/swfupload.swf",
        button_image_url: largeUploadButtonLocation,
        button_width: 100,
        button_height: 100,
        button_placeholder_id: "modelUploadPlaceholderButton"
        
    })
        .bind('fileDialogComplete', function (event, numSelected, numQueued, totalQueued) {
            cancelled = false;

            changeCurrentModelUploadStep('#modelUploadStatus', '#modelUploadIcon');
            if (numSelected > 0) {
                if (modelUploadFinished) { //delete the temporary data associated with the old model
                    resetUpload(CurrentHashname);
                }
                modelUploadRunning = true;
                $('#CancelButton').show();
                if (MODE != "") { //reset the progress bar and hide the steps since this has already attempted to be processed
                    $('.resettable.upload').hide();
                } else { //Show the status panel for the first time
                    $('#DetailsAndStatusPanel').slideDown("fast");
                }
                $('#modelUploadProgress').show();
                $('#modelUploadProgress').progressbar();
                $('#modelUploadStatus').html("Uploading Model");
                $('#modelUploadIcon').attr("src", loadingLocation);
                $('#modelUploadProgress').progressbar("option", "value", 0);
                $('#ChooseModelContainer').swfupload('setButtonDisabled', "true");
                $('#ChooseModelContainer').swfupload('startUpload');

            }
        })
        .bind('uploadProgress', function (event, file, bytesLoaded, totalBytes) {
            totalBytes *= 1.0; bytesLoaded *= 1.0;
            result = (bytesLoaded / totalBytes) * 100.0;
            $('#modelUploadProgress').progressbar("option", "value", result);
        })
        .bind('uploadSuccess', function (event, file, newfilename, success) {
            if (!cancelled) {
                CurrentHashname = newfilename;
                $('#modelUploadProgress').progressbar("option", "value", 100);
                $('#modelUploadProgress').slideUp(400, function () { $('#modelUploadStatus').html("Upload Complete"); });
                $('#modelUploadIcon').attr("src", checkLocation);
                modelUploadFinished = true;
                detectFormat(newfilename);

            } else {
                resetUpload(newfilename); //Reset silently as user initiated cancel process
                $('#ChooseModelContainer').swfupload('setButtonDisabled', false);
            }
        }).bind('uploadError', function (event, file, message, code) {
            $('#CancelButton').hide();
            if (!cancelled) {
                $('#modelUploadProgress').slideUp(400, function () { $('#modelUploadStatus').html("Upload Failed"); });
                $('#modelUploadIcon').attr("src", failLocation);
                $('#modelUploadMessage').show();
                $('#modelUploadMessage').html('An error occured while trying to upload your model. The server may be busy or down. Please try again.');

            }
            //resetUpload(); //Reset it either way
            $('#ChooseModelContainer').swfupload('setButtonDisabled', false);
        });
});
