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

var isViolationReported = false;
var downloadDialog, confirmationDialog, downloadButton;
var vLoader;

function DownloadModel(informat) {

    if (ValidateResubmitChecked()) {
        
        window.location.href = "../DownloadModel.ashx?PID=" + querySt('ContentObjectID') + "&Format=" + informat;
    } else {
        downloadDialog.dialog("close");
        createNotificationDialog("This work is protected under special provisions, and you must agree to resubmit any changes before downloading.");
    }
}

$(document).ready(function () {


    downloadButton = $('#ctl00_ContentPlaceHolder1_DownloadButton');
    confirmationDialog = $('#ConfirmationDialog');

    if (downloadButton.length > 0) {
        var top = downloadButton.offset().top + downloadButton.height();
        var left = $('#DownloadDiv').offset().left;
        var width = $('#3DAssetbar').width();
        var dialog = $('<div />')
		            .load('downloadoptions.html')
		            .dialog({
		                autoOpen: false,
		                title: 'Download Model',
		                show: "fold",
		                hide: "fold",
		                //modal: true,
		                resizable: false,
		                draggable: false,
		                position: [left, top],
		                width: 'auto',
		                height: 'auto'
		            });

        $(dialog).parent()
               .find('.ui-widget-content').css({ top: '-20px', zIndex: '-1' }).parent()
               .find('.ui-widget-header').css({
                   background: 'none',
                   border: 'none'
               }).parent()
               .find('.ui-dialog-title').html('');

        downloadDialog = dialog;
        downloadButton.click(function () {

            if (vLoader) {
                var top = downloadButton.offset().top + downloadButton.height() - $(document).scrollTop();
                var left = $('#DownloadDiv').offset().left;
                var width = $('#3DAssetbar').width();

                dialog.dialog("option", "width", 'auto');
                dialog.dialog("option", "position", [left, top]);
                dialog.dialog('open');

                // prevent the default action, e.g., following a link
                return false;
            }
            else {

                if (ValidateResubmitChecked()) {


                    DownloadModel("original");
                }
                return false;
            }
        });
    }
    if ($('#ctl00_ContentPlaceHolder1_editLink').length == 0) {
        $('#pipehack').remove();
    }
    $("#ViewOptionsMultiPage").tabs()
     .bind("tabsshow", function (event, ui) {
         if (ui.index == 1) {
             SetViewerMode("WebGL");
             vLoader.LoadViewer();
         } else {
             vLoader.ResetViewer();
         }
     }).find("li").last().append(
        $("<span />").addClass("ui-tabs-separator")
                     .addClass("blue")
                     .addClass("last")
     );

    if (!vLoader) {

        $("#ViewOptionsMultiPage").tabs("remove", 1);
    }

    $('#ctl00_ContentPlaceHolder1_DeleteLink').click(function () {
        confirmationDialog.dialog({
            modal: true,
            autoOpen: true,
            closeOnEscape: true,
            draggable: false,
            resizable: false,
            zindex: 9999,
            position: [961, 310],
            width: 327,
            buttons: {
                "Delete Model": function () {
                    $(this).dialog("close");
                    $.ajax({
                        type: "POST",
                        url: "Model.aspx/DeleteModel",
                        data: '{ "pid" : "{0}" }'.format(querySt("ContentObjectID")),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: OnDeleteResponseReceived
                    });
                },
                "Cancel": function () { $(this).dialog("close") }
            }
        });
        confirmationDialog.parent().find(".ui-dialog-buttonpane .ui-dialog-buttonset").css({ 'float': "none", 'textAlign': "center" });
        confirmationDialog.find(".statusText").html("Are you sure you want to delete this model? This action cannot be undone.");
    });

    $(document).ajaxError(function (event, request, ajaxOptions, thrownError) {
        if (request.status == 401) {
            window.location.href = "../Public/Login.aspx?ReturnUrl=%2fPublic%2fModel.aspx?ContentObjectID=" + querySt("ContentObjectID");
        }
    });



    $('#ReportViolationButton').click(function () {
        if (!isViolationReported) {
            var sendReport = function (sender) {
                $(sender)
                .dialog("option", "buttons", [])
                .text("Sending report...");
                $.ajax({
                    type: "POST",
                    url: "Model.aspx/ReportViolation",
                    data: JSON.stringify({
                        "pid": querySt("ContentObjectID"),
                        "title": $("#ctl00_ContentPlaceHolder1_TitleLabel").text(),
                        "description": $(sender).find("textarea").text()
                    }),

                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (object, status, request) {
                        $(sender).html(object.d).dialog("option", { buttons: { "Ok": closeDialog} });
                        isViolationReported = true;
                    },
                    error: function (object, status, request) {
                        $(sender)
                        .dialog("option", { buttons: { "Ok": closeDialog} })
                        .html("<br/>The server was unable to process your request. Please try again later.");
                    }
                })
            };

            var reportForm = "Violation Description:<br/>" +
                             "<textarea style='width: 100%; height: 150px; color: AAA'>" +
                             "</textarea>";


            $("<div>")
            .append(reportForm)
            .dialog({
                modal: true,
                autoOpen: true,
                closeOnEscape: true,
                draggable: true,
                resizable: false,
                zindex: 9999,
                width: 327,
                buttons: { "Submit report": function () { sendReport(this) }, "Cancel": closeDialog }
            });

        } else {
            createNotificationDialog("<br/>You have already reported a violation for this object.");
        }
    });
});


function ValidateResubmitChecked() {
    var checkboxElement = $("#ctl00_ContentPlaceHolder1_RequiresResubmitCheckbox");
    if (checkboxElement.length == 0) return true;
    var ResubmitChecked = $("#ctl00_ContentPlaceHolder1_RequiresResubmitCheckbox:checked").val();
    if (ResubmitChecked !== undefined) {
        return true;
    } else {
        return false;
    }
}

function OnDeleteResponseReceived(data) {
    if (data.d == "1") {
        window.location.href = "../Default.aspx";
    } else {
        confirmationDialog.dialog("close");
        createNotificationDialog("Unable to delete your model. Please try again later.");
    }
}

function createNotificationDialog(message) {
    var options = {
           modal: true,
           autoOpen: true,
           closeOnEscape: true,
           draggable: true,
           resizable: false,
           zindex: 9999,
           width: 327,
           buttons: { "Ok": closeDialog }
       }

       var dialog =  $("<div />").html(message)
                          .dialog(options)
                          .css({ border: "none", textAlign: "center" }).parent();
                          
        dialog.find(".ui-dialog-buttonpane").css({border: "none"})
                    .find(".ui-dialog-buttonset").css({ 'float': "none", textAlign: "center" });
        dialog.find(".ui-widget-header a").remove();
        return dialog;                    
}

function closeDialog() { $(this).dialog("close").parent().remove(); }