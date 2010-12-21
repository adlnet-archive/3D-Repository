function ImageUploadWidget(property, WidgetContainer) {

    this.FlashDiv = $(WidgetContainer).find('.flashContainer');
    this.ProgressBar = $(WidgetContainer).find('.progressbarContainer');
    this.StatusIcon = $(WidgetContainer).find('.statusIcon');
    this.StatusText = $(WidgetContainer).find('.statusText');
    this.ErrorMessage = $(WidgetContainer).find('.errorMessage');
    this.PreviewImage = $(WidgetContainer).find('.previewImage');
    this.CancelButton = $(WidgetContainer).find('.cancel');
    this.IsCancelled = false;


    $(this.CancelButton).click(function(){
        $(this.CancelButton).hide();
        this.IsCancelled = true;
        $(this.StatusText).html("Cancelled");
        $(this.StatusIcon).attr("src", failLocation);
    });

    this.Settings = {
        upload_url: "../Public/Upload.ashx?image=true&property=" + property,
        file_size_limit: "2048",
        file_types: "*.png; *.jpg; *.gif",
        file_types_description: "Image files",
        file_upload_limit: "0",
        flash_url: "../Scripts/swfupload/vendor/swfupload/swfupload.swf",
        button_image_url: "../Scripts/swfupload/vendor/swfupload/XPButtonUploadText_61x22.png",
        button_width: 61,
        button_height: 22,
        button_placeholder_id: property + "_Placeholder"
    }

    this.SuccessCallback = function (event, file, newfilename, success) {    
       if(!this.IsCancelled) {
            $(this.CancelButton).hide();
            $(this.ProgressBar).progressbar("option", "value", 100);
            $(this.ProgressBar).slideUp(400, $(this.StatusText).html("Upload Complete"));
            $(this.StatusIcon).attr("src", checkLocation);
            $(this.PreviewImage).attr("src", "Upload.aspx/GetImage?file=" + newfilename);
            $(this.FlashDiv).swfupload('setButtonDisabled', false);
       } else {
            this.DeleteTempImage(newfilename);
       }
    }

    this.FailCallback = function (event, file, message, code) {
       $(this.CancelButton).hide();
        if(!this.IsCancelled) {
            $(this.ProgressBar).slideUp(400, function () { $(this.StatusText).html("Upload Failed"); });
            $(this.StatusIcon).attr("src", failLocation);
            $(this.ErrorMessage).show();
            $(this.ErrorMessage).html('An error occured while trying to upload your image. The server may be busy or down. Please try again.');
        }
        $(this.FlashDiv).swfupload('setButtonDisabled', false);
    }

    this.FileDialogCompleteCallback = function(event, numSelected, numQueued, totalQueued) {
            cancelled = false;      
            if (numSelected > 0) {
                $(this.ErrorMessage).hide();
                $(this.CancelButton).show();
                $(this.ProgressBar).show();
                $(this.ProgressBar).progressbar();
                $(this.StatusText).html("Uploading image...");
                $(this.StatusIcon).attr("src", loadingLocation);
                $(this.ProgressBar).progressbar("option", "value", 0);
                $(this.FlashDiv).swfupload('setButtonDisabled', "true");
                $(this.FlashDiv).swfupload('startUpload');
            }
    }

    this.ProgressCallback = function (event, file, bytesLoaded, totalBytes) {
         totalBytes *= 1.0; bytesLoaded *= 1.0;
         result = (bytesLoaded / totalBytes) * 100.0;
         $(this.ProgressBar).progressbar("option", "value", result);
    }

    this.Activate = function () {
        $(this.FlashDiv).swfupload(this.Settings)
        .bind("fileDialogComplete", jQuery.proxy(this.FileDialogCompleteCallback, this))
        .bind("uploadSuccess", jQuery.proxy(this.SuccessCallback, this))
        .bind("uploadError", jQuery.proxy(this.FailCallback, this))
        .bind("uploadProgress", jQuery.proxy(this.ProgressCallback, this));
    }


    this.DeleteTempImage = function(filename) {
        $.ajax({
            type: "post",
            url: "Upload.aspx/DeleteTempImage",
            data: '{ "filename" : "' + filename + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        });
    }

    return true;
}