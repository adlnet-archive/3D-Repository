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
var permissionsWidget;

function DownloadModel(informat) {

        if(informat == "original")
            window.location.href =  "http://3dr.adlnet.gov/Federation/3DR_Federation.svc/" + querySt("ContentObjectID") + "/OriginalUpload?ID=00-00-00";
        else
            window.location.href = "http://3dr.adlnet.gov/Federation/3DR_Federation.svc/" + querySt("ContentObjectID") + "/Format/"+informat+"?ID=00-00-00";
   
    //    downloadDialog.dialog("close");
     //   createNotificationDialog("This work is protected under special provisions, and you must agree to resubmit any changes before downloading.");
    
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

          //  if (vLoader) {
                var top = downloadButton.offset().top + downloadButton.height() - $(document).scrollTop();
                var left = $('#DownloadDiv').offset().left;
                var width = $('#3DAssetbar').width();

                dialog.dialog("option", "width", 'auto');
                dialog.dialog("option", "position", [left, top]);
                dialog.dialog('open');

                // prevent the default action, e.g., following a link
                return false;
         //   }
         //   else {

               
         //       DownloadModel("original");
                
         //       return false;
         //   }
        });
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

    $(document).ajaxError(function (event, request, ajaxOptions, thrownError) {
        if (request.status == 401) {
            window.location.href = "../Public/Login.aspx?ReturnUrl=%2fPublic%2fModel.aspx?ContentObjectID=" + querySt("ContentObjectID");
        }
    });

});

function closeDialog() { $(this).dialog("close").parent().remove(); }