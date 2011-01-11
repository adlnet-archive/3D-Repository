function ImageUploadWidget(property, WidgetContainer) {

    this.FlashDiv = $(WidgetContainer).find('.flashContainer');
    this.ProgressBar = $(WidgetContainer).find('.progressbarContainer');
    this.StatusIcon = $(WidgetContainer).find('.statusIcon');
    this.StatusText = $(WidgetContainer).find('.statusText');
    this.ErrorMessage = $(WidgetContainer).find('.errorMessage');
    this.PreviewImage = $(WidgetContainer).find('.previewImage');
    this.LoadingImageContainer = $(WidgetContainer).find('.LoadingImageContainer');
    $(this.LoadingImageContainer).hide();

    this.CancelButton = $(WidgetContainer).find('.cancel');
    this.IsCancelled = false;
    this.Active = false;


    $(this.PreviewImage).load(jQuery.proxy(function () {
        $(this.LoadingImageContainer).hide();
        $(this.PreviewImage).show();
    }, this));

    $(this.CancelButton).click(function(){
        $(this.CancelButton).hide();
        this.IsCancelled = true;
        $(this.StatusText).html("Cancelled");
        $(this.StatusIcon).attr("src", failLocation);
    });

    this.Settings = {
        upload_url: "../Public/Upload.ashx?image=true&method=set&property="+property,
        file_size_limit: "2048",
        file_types: "*.png; *.jpg; *.gif",
        file_types_description: "Image files",
        file_upload_limit: "0",
        flash_url: "../Scripts/swfupload/vendor/swfupload/swfupload.swf",
        button_image_url: "../Images/SmallUpload_Btn.png",
        button_width: 63,
        button_height: 21,
        button_placeholder_id: property + "_Placeholder"
    }

    this.SuccessCallback = function (event, file, newfilename, success) {
        if (!this.IsCancelled) {
            $(this.CancelButton).hide();
            $(this.ProgressBar).progressbar("option", "value", 100);
            $(this.ProgressBar).slideUp(400);
            $(this.StatusText).html("Upload Complete");
            $(this.StatusIcon).attr("src", checkLocation);
            $(this.PreviewImage).attr("src", "../Public/Upload.ashx?image=true&method=get&hashname=" + newfilename + "&time=" + new Date().getTime());
            $(this.PreviewImage).css("width", $(this.PreviewImage).parent().css("width"));
            $(this.PreviewImage).css("height", $(this.PreviewImage).parent().css("min-height"));
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

    this.FileDialogCompleteCallback = function (event, numSelected, numQueued, totalQueued) {
        cancelled = false;
        if (numSelected > 0) {
            $(this.PreviewImage).hide();
            $(this.LoadingImageContainer).show();
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

     this.Activate = function (hashname) {
         $(this.PreviewImage).bind('load', jQuery.proxy(this.ToggleLoadingContainer, this));
        settings = this.Settings;
        settings.upload_url += "&hashname=" + hashname;
        $(this.FlashDiv).swfupload(settings)
        .bind("fileDialogComplete", jQuery.proxy(this.FileDialogCompleteCallback, this))
        .bind("uploadSuccess", jQuery.proxy(this.SuccessCallback, this))
        .bind("uploadError", jQuery.proxy(this.FailCallback, this))
        .bind("uploadProgress", jQuery.proxy(this.ProgressCallback, this));
        this.Active = true;
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