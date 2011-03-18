var isViolationReported = false;



String.prototype.format = function () {
    var s = this,
            i = arguments.length;

    while (i--) {
        s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i]);
    }
    return s;
};

//Taken from http://jquery-howto.blogspot.com/2009/09/get-url-parameters-values-with-jquery.html
$.extend({
    getUrlVars: function () {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    },
    getUrlVar: function (name) {
        return $.getUrlVars()[name];
    }
});

$(document).ready(function () {
    $('.viewerTab').click(function () {
        SetViewerMode("o3d");
        vLoader.LoadViewer();
    });

    $('.imageTab').click(function () {
        vLoader.ResetViewer();
    });


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
                        data: '{ "pid" : "{0}" }'.format($.getUrlVar("ContentObjectID")),
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
            window.location.href = "../Public/Login.aspx?ReturnUrl=%2fPublic%2fModel.aspx?ContentObjectID=" + $.getUrlVar("ContentObjectID");
        }
    });

    $("#NotificationDialog").dialog({
        modal: true,
        autoOpen: false,
        closeOnEscape: true,
        draggable: true,
        resizable: false,
        zindex: 9999,
        position: [961, 310],
        width: 327,
        buttons: {
            "Ok": function () { $(this).dialog("close"); }
        }
    });

    $("#NotificationDialog").parent().find(".ui-widget-content").css({ border: "none" });
    $("#NotificationDialog").parent().find(".ui-dialog-buttonpane .ui-dialog-buttonset").css({ float: "none", textAlign: "center" });
    $('#ReportViolationButton').click(function () {
        if (!isViolationReported) {
            $.ajax({
                type: "POST",
                url: "Model.aspx/ReportViolation",
                data: '{ "pid" : "{0}", "title" : "{1}" }'
                          .format($.getUrlVar("ContentObjectID"),
                                  $("#ctl00_ContentPlaceHolder1_TitleLabel").text()),

                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (object, status, request) {
                    $("#NotificationDialog").dialog("open");
                    $("#NotificationDialog").find('.statusText').html("<br/>" + object.d);
                    isViolationReported = true;
                },
                error: function (object, status, request) {
                    $("#NotificationDialog").dialog("open");
                    $("#NotificationDialog").find('.statusText').html("<br/>The server was unable to process your request. Please try again later.");
                }
            });
        } else {
            $("#NotificationDialog").dialog("open");
            $("#NotificationDialog").find('.statusText').html("<br/>You have already reported a violation for this object.");
        }
    });
});


function ValidateResubmitChecked() {
    var checkboxElement = $("#ctl00_ContentPlaceHolder1_RequiresResubmitCheckbox");
    if ( checkboxElement.length == 0 ) return true;
    var ResubmitChecked = $("#ctl00_ContentPlaceHolder1_RequiresResubmitCheckbox:checked").val();
    if ( ResubmitChecked !== undefined ) {
        return true;
    } else {
        $("#NotificationDialog").dialog("open");
        $("#NotificationDialog").find('.statusText').html("This work is protected under special provisions, and you must agree to resubmit any changes before downloading.");
        return false;
    }
}

function OnDeleteResponseReceived(data) {
    if (data.d == "1") {
        window.location.href= document.referrer;
    } else {
        $("#ConfirmationDialog").dialog("close");
        $("#NotificationDialog").dialog("open");
        $("#NotificationDialog").find('.statusText').html("Unable to delete model. Please try again later.");
    }
}