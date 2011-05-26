var isViolationReported = false;

String.prototype.format = function () {
    var s = this,
            i = arguments.length;

    while (i--) {
        s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i]);
    }
    return s;
};

function querySt(ji) {
    hu = window.location.search.substring(1);
    gy = hu.split("&");
    for (i = 0; i < gy.length; i++) {
        ft = gy[i].split("=");
        if (ft[0] == ji) {
            return ft[1];
        }
    }
}

function DownloadModel(informat) {
    if (ValidateResubmitChecked()) {
        window.location.href = "../DownloadModel.ashx?PID=" + querySt('ContentObjectID') + "&Format=" + informat;
    } else {
        createNotificationDialog("This work is protected under special provisions, and you must agree to resubmit any changes before downloading.");
    }
}

$(document).ready(function () {

    if ($('#ctl00_ContentPlaceHolder1_DownloadButton').length > 0) {
        var top = $('#ctl00_ContentPlaceHolder1_DownloadButton').offset().top + $('#ctl00_ContentPlaceHolder1_DownloadButton').height();
        var left = $('#DownloadDiv').offset().left;
        var width = $('#3DAssetbar').width();
        var dialog = $('<div></div>')
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

        $('#ctl00_ContentPlaceHolder1_DownloadButton').click(function () {

            var top = $('#ctl00_ContentPlaceHolder1_DownloadButton').offset().top + $('#ctl00_ContentPlaceHolder1_DownloadButton').height() - $(document).scrollTop();
            var left = $('#DownloadDiv').offset().left;
            var width = $('#3DAssetbar').width();

            dialog.dialog("option", "width", 'auto');
            dialog.dialog("option", "position", [left, top]);
            dialog.dialog('open');

            // prevent the default action, e.g., following a link
            return false;
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
        $("#tabs").tabs("remove", 1);
    }


    $('#ctl00_ContentPlaceHolder1_DeleteLink').click(function () {
        $("#ConfirmationDialog").dialog({
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
        $("#ConfirmationDialog").parent().find(".ui-dialog-buttonpane .ui-dialog-buttonset").css({ float: "none", textAlign: "center" });
        $("#ConfirmationDialog").find(".statusText").html("Are you sure you want to delete this model? This action cannot be undone.");
    });

    $(document).ajaxError(function (event, request, ajaxOptions, thrownError) {
        if (request.status == 401) {
            window.location.href = "../Public/Login.aspx?ReturnUrl=%2fPublic%2fModel.aspx?ContentObjectID=" + querySt("ContentObjectID");
        }
    });



    $('#ReportViolationButton').click(function () {
        if (!isViolationReported) {
            $.ajax({
                type: "POST",
                url: "Model.aspx/ReportViolation",
                data: '{ "pid" : "{0}", "title" : "{1}" }'
                          .format(querySt("ContentObjectID"),
                                  $("#ctl00_ContentPlaceHolder1_TitleLabel").text()),

                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (object, status, request) {
                    createNotificationDialog(object.d);
                    isViolationReported = true;
                },
                error: function (object, status, request) {
                   createNotificationDialog("<br/>The server was unable to process your request. Please try again later.");
                }
            });
       } else {
           createNotificationDialog.html("<br/>You have already reported a violation for this object.");
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
        $("#ConfirmationDialog").dialog("close");
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
           buttons: {
               "Ok": function () { $(this).dialog("close").parent().remove(); }
           }
       }

       var dialog =  $("<div />").html(message)
                          .dialog(options)
                          .css({ border: "none", textAlign: "center" }).parent();
                          
        dialog.find(".ui-dialog-buttonpane").css({border: "none"})
                    .find(".ui-dialog-buttonset").css({ float: "none", textAlign: "center" });
        dialog.find(".ui-widget-header a").remove();
        return dialog;                    
}