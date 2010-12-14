<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewUpload.ascx.cs" Inherits="Controls_NewUpload" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="VwarWeb" TagName="Viewer3D" Src="~/Controls/Viewer3D.ascx" %>
<script src="../Scripts/jquery-1.3.2.min.js" type="text/javascript"></script>
<script src="../Scripts/jquery-ui-1.7.3.custom.min.js" type="text/javascript"></script>
<script src="../Scripts/swfupload/vendor/swfupload/swfupload.js" type="text/javascript"></script>
<script src="../Scripts/swfupload/src/jquery.swfupload.js" type="text/javascript"></script>
<link href="../App_Themes/Default/jquery-ui-1.7.3.custom.css" rel="Stylesheet" runat="server" />
<script type="text/javascript">

    var iconBase = "../Images/Icons/";
    var cancelled = false;

    var loadingLocation = iconBase + "loading.gif";
    var checkLocation = iconBase + "checkmark.gif";
    var failLocation = iconBase + "xmark.png";
    var warningLocation = iconBase + "warning.gif";

    var MODE = "";

    $(document).ready(function () {
        $('#modelUploadProgress').progressbar();
    });


    /* Advances to the next step in the uploading process.
     *
     * Step refers to the top-level steps required for
     * the entire process, rather than the substeps
     * found in places like Step 1 (Model Upload) 
     */
    function nextStep() {
        $.ajax({
            type: "POST",
            url: "Upload.aspx/Next",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        });
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
            data:  '{ "filename" : "' + filename + '"}',
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
            }
        });
    }

    $(function () {


        $('#ChooseModelContainer').swfupload({
            upload_url: "http://localhost:2974/Public/Upload.ashx",
            file_size_limit: "10240",
            file_types: "*.zip; *.skp",
            file_types_description: "Recognized 3DR format",
            file_upload_limit: "0",
            flash_url: "../Scripts/swfupload/vendor/swfupload/swfupload.swf",
            button_image_url: "../Images/3DR-Upload-Icon.png",
            button_width: 100,
            button_height: 100,
            button_placeholder_id: "modelUploadPlaceholderButton"
        })
        .bind('fileDialogComplete', function (event, numSelected, numQueued, totalQueued) {
            cancelled = false;
            
            changeCurrentModelUploadStep('#modelUploadStatus', '#modelUploadIcon');
            if (numSelected > 0) {
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
                $('#modelUploadProgress').progressbar("option", "value", 100);
                $('#modelUploadProgress').slideUp(400, function () { $('#modelUploadStatus').html("Upload Complete"); });
                $('#modelUploadIcon').attr("src", checkLocation);
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
            resetUpload(); //Reset it either way
            $('#ChooseModelContainer').swfupload('setButtonDisabled', false);
        });
    });
	

</script>
<style type="text/css">
    .UploadControlItem
    {
        /*padding: 5px;*/
    }
    
    .PanelLayoutContainer
    {
        background-color: white;
        border: 1px solid;
        margin: 20px auto;
        position: relative;
        width: 855px;
    }
    
    .LRPanelLayout
    {
        height: 98%;
        position: absolute;
        z-index: 1;
        text-align: center;
        display: inline;
    }
    
    .Right
    {
        left: 50%;
    }
    
    .ui-progressbar
    {
        width: 150px;
        height: 10px;
        font-size: 1px;
    }
    
    #UploadStatusArea
    {
        margin-left: 100px;
        width: 200px;
        text-align: left;
        line-height: 15px;
    }
    
    #DetailsAndStatusPanel
    {
        display: none;
    }
    
    
    .rpTemplate 
    {
        overflow: hidden;
    }
</style>
<div style="width: 900px; margin: 0 auto">
<telerik:RadAjaxManagerProxy runat="server" />
    <telerik:RadAjaxPanel ID="UploadControlAjaxPanel" runat="server" EnableAJAX="true" >
    <h1>
        Upload a 3D Model</h1>
    <telerik:RadPanelBar ID="UploadControl" ExpandMode="SingleExpandedItem" Width="900" runat="server" Style="position: relative;
        z-index: 1">
        <Items>
            <telerik:RadPanelItem ID="UploadPanel"  Text="1. Upload" Expanded="true" runat="server">
                <ContentTemplate>
                    <div class="UploadControlItem">
                        <div class="PanelLayoutContainer" style="height: 200px">
                            <div class="LRPanelLayout" style="width: 30%;">
                            <span style="font-size:large; font-weight: bold; text-align: left; margin: 0 0">Choose Your Format:</span>
                                <div id="ChooseModelContainer" style="display: inline; width: 50%; height: 45%; position: relative;
                                    top: 20px; z-index: 2">
                                    <button id="modelUploadPlaceholderButton" value="Upload" />
                                </div>
                                <div style="margin:56px 30px 0 30px;text-align:left; display:none;">
                                    <a href="#" onclick="return false;">Learn more</a> about best practices for uploading
                                    a model to 3DR.
                                </div>
                            </div>
                            <div class="LRPanelLayout" style="text-align: center; padding-top: 5px; left: 30%; vertical-align: bottom;
                                width: 30%">
                                <asp:Image ImageUrl="~/Images/thumb-zip-folder.jpg"  runat="server" />
                                <ul style="text-align: left;">
                                    <li>A .zip file containing the following:</li>
                                    <ul>
                                        <li>A <a href="#" onclick="return false;">recognized model format</a></li>
                                        <li>Referenced texture files</li>
                                    </ul>
                                </ul>
                            </div>
                            <div class="LRPanelLayout" style="text-align: center; padding-top: 100px; font-weight: bold;
                                font-size: large; left: 60%; width: 10%;">
                                OR</div>
                            <div class="LRPanelLayout" style="padding-top: 2px; left: 70%; width: 25%;">
                            <img src="http://upload.wikimedia.org/wikipedia/en/b/bf/Sketchuplogo.png"  />
                            <ul style="text-align: left;">
                                <li>An .skp (Google SketchUp) file</li>
                            </ul>                           
                            </div>
                        </div>
                        <div id="DetailsAndStatusPanel" class="PanelLayoutContainer" style="height: 300px;">
                            <div class="LRPanelLayout" style="width: 30%">
                                <div id="UploadStatusArea">
                                    <div style="position: absolute; top: 50px; right: 5px">
                                        <a href='#' id="CancelButton" onclick='cancelModelUpload(); return false'>Cancel</a>
                                    </div>
                                    <table style="position: relative; right: 40px; top: 64px;">
                                        
                                        <tr id="modelUpload">
                                            <td width="16" valign="top">
                                                1.
                                            </td>
                                            <td width="150">
                                                <div id="modelUploadProgress" class="progress">
                                                </div>
                                                <div id="modelUploadStatus">
                                                </div>
                                            </td>
                                            <td width="16" valign="top">
                                                <img id="modelUploadIcon" />
                                            </td>
                                        </tr>
                                        <tr class="resettable upload">
                                            <td />
                                            <td>
                                                <div id="modelUploadMessage">
                                                </div>
                                            </td>
                                            <td />
                                        </tr>
                                        <tr id="formatDetect" style="display: none;" class="resettable upload">
                                            <td width="16">
                                                2.
                                            </td>
                                            <td width="150">
                                                <div id="formatDetectStatus">
                                                </div>
                                            </td>
                                            <td valign="top">
                                                <img id="formatDetectIcon" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td />
                                            <td>
                                                <div id="formatDetectMessage" class="resettable upload">
                                                </div>
                                            </td>
                                            <td />
                                        </tr>
                                        <tr id="conversionStep" style="display: none;" class="resettable upload">
                                            <td width="16" valign="top">
                                                3.
                                            </td>
                                            <td width="150">
                                                <div id="conversionStatus">
                                                </div>
                                            </td>
                                            <td width="16" valign="top">
                                                <img id="conversionIcon" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td />
                                            <td>
                                                <div id="conversionMessage" class="resettable upload">
                                                </div>
                                            </td>
                                            <td />
                                        </tr>
                                        <tr>
                                            <td />
                                            <td>
                                                <br />
                                                <br />
                                                <br />
                                                <asp:Button ID="NextButton_Step1" OnClick="ModelUpload_NextStep" Text="Next Step" runat="server" />
                                            </td>
                                            <td />
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="LRPanelLayout" style="width: 70%; left: 30%;">
                                <div style="margin-left: 78px">
                                    <h2 style="margin-bottom: 3px">
                                        While You're Waiting...</h2>
                                    Please fill out the following info:
                                    <br />
                                </div>
                                <table style="margin: 0 auto;">
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="TitleLabel" runat="server" Text="Title (Required)" />
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="TitleInput" runat="server" Columns="55"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator Text="*" ControlToValidate="TitleInput" style="color: Red; font-weight:bold; font-size: large" runat="server"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" valign="top">
                                            <asp:Label ID="DescriptionLabel" runat="server" Text="Description" />
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="DescriptionInput" runat="server" Style="width: 424px" TextMode="MultiLine"
                                                Rows="4"></asp:TextBox>
                                        </td>
                                        <td />
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="TagsLabel" runat="server" Text="Tags" />
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="TagsInput" runat="server" Columns="55"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <%--This is the privacy settings section. Since we haven't implemented privacy settings, it is purposefully commmented out. %>
<%--                                    <tr>
                                        <td align="right" valign="top">
                                            <asp:Label ID="PrivacySettingsLabel" runat="server" Text="Privacy Settings" />
                                        </td>
                                        <td align="left">
                                            <asp:RadioButtonList ID="PrivacyOptions" runat="server">
                                                <asp:ListItem Text="Public (anyone can search for and view your model)" Value="public"
                                                    Selected="True" />
                                                <asp:ListItem>Private (only users you specify can find your content) <a href="#" onclick="return false;">Add users</a><sup>+</sup></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>--%>
                                </table>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </telerik:RadPanelItem>
            <telerik:RadPanelItem ID="AxisScalePanel" Text="2. Set Axis and Scale" Enabled="true"
                runat="server">
                <ContentTemplate>
                    <div class="PanelLayoutContainer" style="height: 600px">
                        <VwarWeb:Viewer3D ID="ModelViewer" runat="server" />
                    </div>
                </ContentTemplate>
            </telerik:RadPanelItem>
            <telerik:RadPanelItem ID="ThumbnailPanel" Text="3. Choose Thumbnail" Enabled="false"
                runat="server">
            </telerik:RadPanelItem>
            <telerik:RadPanelItem ID="DetailsPanel" Text="4. Add Details" Enabled="false" runat="server">
            </telerik:RadPanelItem>
        </Items>
    </telerik:RadPanelBar>
    </telerik:RadAjaxPanel>
</div>
