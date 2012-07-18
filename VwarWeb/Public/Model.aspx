<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Model.aspx.cs" Inherits="Public_Model" Title="Model Details" SmartNavigation="True"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="VwarWeb" TagName="Viewer3D" Src="~/Controls/Viewer3D.ascx" %>
<%@ Register TagPrefix="VwarWeb" TagName="PermissionsManagementWidget" Src="~/Controls/PermissionsManagementWidget.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../styles/ModelDetails.css" rel="Stylesheet" type="text/css" />
    <link href="../styles/tabs-custom.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript" src="../scripts/o3djs/base.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.8.7.custom.min.js"></script>
    <script type="text/javascript" src="../scripts/o3djs/simpleviewer.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/webgl-utils.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/osg.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/osgUtil.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/osgAnimation.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/osgGA.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/osgViewer.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/examples/viewer/webglviewer.js"></script>
    <script type="text/javascript" src="../Scripts/_lib/jquery.cookie.js"></script>
    <script type="text/javascript" src="../Scripts/_lib/jquery.hotkeys.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.jstree.js"></script>
    <script type="text/javascript" src="../Scripts/ModelDetails.js"></script>
    <script type="text/javascript" src="../Scripts/ViewerLoad.js"></script>
    <script type="text/javascript" src="../Scripts/ImageUploadWidget.js"></script>
    <script type="text/javascript" src="../Scripts/fileuploader.js"></script>
    	<style>
	.ui-combobox {
		position: relative;
		display: inline-block;
	}
	.ui-combobox-toggle {
		position: absolute;
		top: 0;
		bottom: 0;
		margin-left: -1px;
		padding: 0;
		/* adjust styles for IE 6/7 */
		*height: 1.7em;
		*top: 0.1em;
	}
	.ui-combobox-input {
		margin: 0;
		padding: 0.3em;
	}
	</style>
	<script>
	    (function ($) {
	        $.widget("ui.combobox", {
	            _create: function () {
	                var input,
					self = this,
					select = this.element.hide(),
					selected = select.children(":selected"),
					value = selected.val() ? selected.text() : "",
					wrapper = this.wrapper = $("<span>")
						.addClass("ui-combobox").css('width','100%')
						.insertAfter(select);

	                input = $("<input>")
					.appendTo(wrapper)
					.val(value)
					.addClass("ui-state-default ui-combobox-input")
                    .css('width','85%')
					.autocomplete({
					    delay: 0,
					    minLength: 0,
					    source: function (request, response) {
					        var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
					        response(select.children("option").map(function () {
					            var text = $(this).text();
					            if (this.value && (!request.term || matcher.test(text)))
					                return {
					                    label: text.replace(
											new RegExp(
												"(?![^&;]+;)(?!<[^<>]*)(" +
												$.ui.autocomplete.escapeRegex(request.term) +
												")(?![^<>]*>)(?![^&;]+;)", "gi"
											), "<strong>$1</strong>"),
					                    value: text,
					                    option: this
					                };
					        }));
					    },
					        select: function (event, ui) {
					            ui.item.option.selected = true;
					            self._trigger("selected", event, {
					                item: ui.item.option
					            });
					            select.trigger("change");
					        },

					    change: function (event, ui) {
					        if (!ui.item) {
					            var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex($(this).val()) + "$", "i"),
									valid = false;
					            select.children("option").each(function () {
					                if ($(this).text().match(matcher)) {
					                    this.selected = valid = true;
					                    return false;
					                }
					            });
					            if (!valid) {
					                // remove invalid value, as it didn't match anything
					                $(this).val("");
					                select.val("");
					                input.data("autocomplete").term = "";
					                return false;
					            }
					        }
					    }
					})
					.addClass("ui-widget ui-widget-content ui-corner-left");

	                input.data("autocomplete")._renderItem = function (ul, item) {
	                    return $("<li></li>")
						.data("item.autocomplete", item)
						.append("<a>" + item.label + "</a>")
						.appendTo(ul);
	                };

	                $("<a>")
					.attr("tabIndex", -1)
					.attr("title", "Show All Items")
					.appendTo(wrapper)
					.button({
					    icons: {
					        primary: "ui-icon-triangle-1-s"
					    },
					    text: false
					})
					.removeClass("ui-corner-all")
					.addClass("ui-corner-right ui-combobox-toggle")
					.click(function () {
					    // close if already visible
					    if (input.autocomplete("widget").is(":visible")) {
					        input.autocomplete("close");
					        return;
					    }

					    // work around a bug (likely same cause as #5265)
					    $(this).blur();

					    // pass empty string as value to search for, displaying all results
					    input.autocomplete("search", "");
					    input.focus();
					});
	            },

	            destroy: function () {
	                this.wrapper.remove();
	                this.element.show();
	                $.Widget.prototype.destroy.call(this);
	            },
	            autocomplete: function (value) {
	                this.element.val(value);
	                $(this.wrapper[0].firstChild).val(value);
	                //this.input.val(value);
	            }
	        });
	    })(jQuery);

	    $(function () {
	        $("#combobox").combobox();
	        $("#toggle").click(function () {
	            $("#combobox").toggle();
	        });
	    });
	</script>
    <script type="text/javascript" >


        var urlParams = {};
        (function () {
            var match,
        pl = /\+/g,  // Regex for replacing addition symbol with a space
        search = /([^&=]+)=?([^&]*)/g,
        decode = function (s) { return decodeURIComponent(s.replace(pl, " ")); },
        query = window.location.search.substring(1);

            while (match = search.exec(query))
                urlParams[decode(match[1])] = decode(match[2]);
        })();

        var DeveloperLogoUploadWidget;
        var Backup_DeveloperLogoImageFileName;
        var Backup_DeveloperLogoImageFileNameId;
        var Backup_SponsorLogoImageFileName;
        var Backup_SponsorLogoImageFileNameId;
        var ThumbnailUploadWidget;
        function FireDeleteSupportingFile(filename) {

            

            $.ajax({
                type: "POST",
                url: "Model.aspx/DeleteSupportingFile",
                data: JSON.stringify({Filename:filename, pid: urlParams['ContentObjectID'] }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (e) {
                         GetSupportingFiles();
                    

                },
                error: function (e, xhr) {
                    alert(e);
                }
            });

        }

        function deleteSupportingFile(filename) {

            $('#deleteSupportingFileDialog').html('Are you sure you want to delete the supporting document called ' + filename +' ?');
            $('#deleteSupportingFileDialog').dialog({ modal:true,title: "Delete " + filename, buttons: { Cancel: function () { $(this).dialog('close'); }, Delete: function () { $(this).dialog('close'); FireDeleteSupportingFile(filename) } } }).dialog('open');

        }
        function SendRequest() {

            $.ajax({
                type: "POST",
                url: "Model.aspx/SendAccessRequest",
                data: JSON.stringify({ pid: urlParams['ContentObjectID'], message: $('#RequestAccessMessage').val() }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (e) {

                    $('#RequestAccessForm').dialog('close');

                },
                error: function (e, xhr) {
                    alert(e);
                }
            });


        }
        function GetSupportingFiles() {

            //UpdateAssetData(string Title, string Description, string Keywords, string License)
            $.ajax({
                type: "POST",
                url: "Model.aspx/GetSupportingFiles",
                data: JSON.stringify({ pid: urlParams['ContentObjectID'] }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (e) {

                    if (e.d.Success == true) {

                        var html = "";
                        $('#SupportingFilesList').html("");
                        for (var i = 0; i < e.d.files.length; i++) {

                            var onclick = "onclick = \"deleteSupportingFile('" + e.d.files[i].Filename + "');\"";


                            html += '<li style="border:1px ridge lightgray;border-radius:0px">' +
                                            (!e.d.DownloadAllowed ? '<div style="display:inline-block;width:33%;padding:5px 0px 5px 5px;vertical-align:top;overflow:hidden">' + e.d.files[i].Filename + '</div>' :
                                            '<a href="' + "./Serve.ashx?pid=" + urlParams['ContentObjectID'] + "&mode=GetSupportingFile&SupportingFileName=" + e.d.files[i].Filename + '" style=";overflow:hidden;display:inline-block;width:33%;padding:5px 0px 5px 5px;vertical-align:top">' + e.d.files[i].Filename + '</a>') +
                                            '<div style="display:inline-block;width:56%;border-left:1px solid lightgray;border-right:1px solid lightgray;padding:5px 0px 5px 5px;vertical-align:top;min-height:1.4em">' + e.d.files[i].Description + '</div>' +
                                            '<div style="display:inline-block;width:8%;margin-top:5px">' +
                                            (e.d.EditAllowed ? '<img class="deletesupportingfilebutton" style="float:right;cursor:pointer" src="../styles/images/icons/delete.gif" ' + onclick + ' />' : '') +
                                            (e.d.DownloadAllowed ? '<a style="float:right;" href="' + "./Serve.ashx?pid=" + urlParams['ContentObjectID'] + "&mode=GetSupportingFile&SupportingFileName=" + e.d.files[i].Filename + '"><img  src="../styles/images/icons/expand.jpg" /></a>'
                                            : '<img style="float:right;padding-right:0px" src="../styles/images/icons/expand_disabled.jpg" />') +

                                            '</div>'
                            '</li>'
                        }
                        $('#SupportingFilesList').html(html);
                        if($('#editLink').html() == 'Edit')
                            $('.deletesupportingfilebutton').hide();
                    }

                },
                error: function (e, xhr) {
                    alert(e);
                }
            });

        }
        function DisableAllSections() {
        $('#editLink').attr('disabled', 'disabled');
        $('#DeveloperInfoSection').attr('disabled', 'disabled');

        $('#DistributionStatementSection').attr('disabled', 'disabled');

        $('#SponsorInfoSection').attr('disabled', 'disabled');
        $('#AssetDetailsSection').attr('disabled', 'disabled');
        $('#SupportingFilesSection').attr('disabled', 'disabled');
        $('#_3DAssetSection').attr('disabled', 'disabled');
        $('#EditDeveloperInfo').attr('disabled', 'disabled');
        $('#EditSponsorInfo').attr('disabled', 'disabled');
        $('#EditAssetInfo').attr('disabled', 'disabled');
        $('#EditDetails').attr('disabled', 'disabled');
        $('#UploadSupportingFile').attr('disabled', 'disabled');
        $('#EditorButtons').attr('disabled', 'disabled');
        $('#editLink').attr('disabled', 'disabled');
        $('#PermissionsLink').attr('disabled', 'disabled');
        $('#DeleteLink').attr('disabled', 'disabled');

        $('#EditDeveloperInfo').css('color', 'lightgray');
        $('#EditSponsorInfo').css('color', 'lightgray');
        $('#EditAssetInfo').css('color', 'lightgray');
        $('#EditDetails').css('color', 'lightgray');
        $('#UploadSupportingFile').css('color', 'lightgray');
        $('#editLink').css('color', 'gray');

        $('#EditDeveloperInfo').css('cursor', 'default');
        $('#EditSponsorInfo').css('cursor', 'default');
        $('#EditAssetInfo').css('cursor', 'default');
        $('#EditDetails').css('cursor', 'default');
        $('#UploadSupportingFile').css('cursor', 'default');
        $('#editLink').css('cursor', 'default');

        $('#EditDistributionStatement').css('color', 'lightgray');
        $('#EditDistributionStatement').css('cursor', 'default');
        

        $('#EditThumbnail').css('color', 'lightgray');
        $('#EditThumbnail').css('cursor', 'default');
        $('#EditThumbnail').attr('disabled', 'disabled');
       
        }
        function Enable(id) {
            $('#' + id).css('cursor', 'pointer');
            $('#' + id).css('color', '#0E4F9C');
            $('#' + id).removeAttr('disabled');
        }
        function EnableAllSections()
        {

            $('#EditDeveloperInfo').css('cursor', 'pointer');
            $('#EditSponsorInfo').css('cursor', 'pointer');
            $('#EditAssetInfo').css('cursor', 'pointer');
            $('#EditDetails').css('cursor', 'pointer');
            $('#UploadSupportingFile').css('cursor', 'pointer');
            $('#editLink').css('cursor', 'pointer');
            $('#editLink').css('cursor', 'pointer');
            $('#EditDistributionStatement').css('cursor', 'pointer');

            
            $('#EditDistributionStatement').css('color', '#0E4F9C');
            $('#EditDeveloperInfo').css('color', '#0E4F9C');
            $('#EditSponsorInfo').css('color', '#0E4F9C');
            $('#EditAssetInfo').css('color', '#0E4F9C');
            $('#EditDetails').css('color', '#0E4F9C');
            $('#UploadSupportingFile').css('color', '#0E4F9C');
            $('#editLink').css('color', '#0E4F9C');
            
            $('#EditThumbnail').css('color', '#0E4F9C');
            $('#EditThumbnail').css('cursor', 'pointer');
            $('#EditThumbnail').removeAttr('disabled');

            $('#EditDistributionStatement').removeAttr('disabled');
            $('#DistributionStatementSection').removeAttr('disabled');
            $('#editLink').removeAttr('disabled');
            $('#DeveloperInfoSection').removeAttr('disabled');
            $('#SponsorInfoSection').removeAttr('disabled');
            $('#AssetDetailsSection').removeAttr('disabled');
            $('#SupportingFilesSection').removeAttr('disabled');
            $('#_3DAssetSection').removeAttr('disabled');
            $('#EditDeveloperInfo').removeAttr('disabled');
            $('#EditSponsorInfo').removeAttr('disabled');
            $('#EditAssetInfo').removeAttr('disabled');
            $('#EditDetails').removeAttr('disabled');
            $('#UploadSupportingFile').removeAttr('disabled');
            $('#EditorButtons').removeAttr('disabled');
            $('#editLink').removeAttr('disabled');
            $('#PermissionsLink').removeAttr('disabled');
            $('#DeleteLink').removeAttr('disabled');

        }

        function GetDistributionText() {
            var val = $("#EditDistributionStatementType :radio:checked").val();
            switch (val)
            {
                case "Distribution_A":
                    return "Approved for public release; distribution is unlimited";
                    break;
                case "Distribution_B":
                    return "Distribution authorized to U.S. Government agencies only. " + $('#EditDistributionReason').val() + " " + $('#EditDistributionDeterminationDate').val() + ". Other requests for this document shall be referred to " + $('#EditDistributionOffice').val();
                    break;
                case "Distribution_C":
                    return "Distribution authorized to U.S. Government Agencies and their contractors " + $('#EditDistributionReason').val() + " " + $('#EditDistributionDeterminationDate').val() + ". Other requests for this document shall be referred to " + $('#EditDistributionOffice').val();
                    break;
                case "Distribution_D":
                    return "Distribution authorized to the Department of Defense and U.S. DoD contractors only. " + $('#EditDistributionReason').val() + " " + $('#EditDistributionDeterminationDate').val() + ". Other requests shall be referred to " + $('#EditDistributionOffice').val();
                    break;
                case "Distribution_E":
                    return "Distribution authorized to DoD Components only  " + $('#EditDistributionReason').val() + " " + $('#EditDistributionDeterminationDate').val() + ". Other requests shall be referred to " + $('#EditDistributionOffice').val();
                    break;
                case "Distribution_F":
                    return "Further dissemination only as directed by " + $('#EditDistributionDeterminationOffice').val() + " " + $('#EditDistributionDeterminationDate').val() + " or higher DoD authority.";
                    break;
                case "Distribution_X":
                    return "Distribution authorized to U.S. Government Agencies and private individuals or enterprises eligible to obtain export-controlled technical data in accordance with " + $('#EditDistributionRegulation').val() + "; " + $('#EditDistributionDeterminationDate').val() + ". DoD Controlling Office is " + $('#EditDistributionOffice').val();
                    break;
                case "NA":
                    return "";
                    break;

            }
            return "";
        }
        function PreviewDistributionStatement() {

            $('#DistributionStatementText').html(GetDistributionText());
            $('#DistributionStatementText').css('color', 'blue');
        }
        function InitialHideShow() {

            $('#EditDetails').hide();
            $('#EditAssetInfo').hide();
            $('#EditSponsorInfo').hide();
            $('#EditDeveloperInfo').hide();
            $('#UploadSupportingFile').hide();
            $('#EditThumbnailCancel').hide();


            $('#EditDistributionStatement').hide();
            $('#EditDistributionStatementCancel').hide();
            $('#DistributionStatementEditSection').hide();
            $('#EditDeveloperNameHyperLink').hide();
            $('#EditAssetInfoCancel').hide();
            $('#EditAssetInfo').hide();
            $('#EditDeveloperNameHyperLink').hide();
            $('#EditArtistNameHyperLink').hide();
            $('#EditSponsorNameLabel').hide();
            $('#EditFormatLabel').hide();
            $('#EditNumPolygonsLabel').hide();
            $('#EditNumTexturesLabel').hide();
            $('#UploadSupportingFileSection').hide();
            $('#EditDeveloperInfoCancel').hide();
            $('#SelectLicenseArea').hide();
            $('#EditSponsorInfoCancel').hide();
            $('#UploadDeveloperLogoRow').hide();
            $('#EditDetailsCancel').hide();
            $('#UploadSponsorLogoRow').hide();
            $('#UploadSupportingFileCancel').hide();
            $('#EditMoreInformationURL').hide();

            $('#EditThumbnail').hide();
            $('#EditThumbnailSection').hide();

            $('#DistributionStatementSection').css('max-width', $('#_3DAssetSection').width() + 'px');

            $('#EditTitle').hide();
            $('#EditDescription').hide();
            $('#EditKeywords').hide();

            


            if ($('#DistributionStatementText').html() == "" || !$('#DistributionStatementText').html())
                $('#DistributionStatementSection').hide();

            if ($('#keywordLabel').html() == "")
                $('#keywordLabel').hide();

            if ($('#MoreDetailsHyperLink').attr('href') == "" || $('#MoreDetailsHyperLink').attr('href') == null)
                $('#MoreDetailsRow').hide();

            if ($('#DeveloperLogoImage').attr('src') == "" || $('#DeveloperLogoImage').attr('src') == null || $('#DeveloperLogoImage').attr('src') == window.location.href)
                $('#DeveloperLogoRow').hide();


            if ($('#SponsorLogoImage').attr('src') == "" || $('#SponsorLogoImage').attr('src') == null || $('#SponsorLogoImage').attr('src') == window.location.href)
                $('#SponsorLogoRow').hide();

            if ($('#SponsorNameLabel').html() == "")
                $('#SponsorNameRow').hide();

            if ($('#ArtistNameHyperLink').html() == "")
                $('#ArtistRow').hide();

            if ($('#DeveloperNameHyperLink').html() == "")
                $('#DeveloperRow').hide();


            if (!$('#SponsorLogoRow').is(":visible") && !$('#SponsorNameRow').is(":visible"))
                $('#SponsorInfoSection').hide();


            if (!$('#DeveloperRow').is(":visible") && !$('#ArtistRow').is(":visible") && !$('#DeveloperLogoRow').is(":visible"))
                $('#DeveloperInfoSection').hide();

            if ($('#NumPolygonsLabel').html() == "")
                $('#NumPolygonsRow').hide();

            if ($('#NumTexturesLabel').html() == "")
                $('#NumTexturesRow').hide();

            if ($('#CCLHyperLink').attr('href') == "" || $('#CCLHyperLink').attr('href') == null)
                $('#CCLHyperLink').hide();


            if ($('#SubmitterEmailHyperLink').html() == "[SubmitterEmailHyperLink]" || $('#SubmitterEmailHyperLink').html() == "")
                $('#SubmitterEmailHyperLink').hide();

            if ($('#ArtistNameHyperLink').html() == "[ArtistNameHyperLink]" || $('#ArtistNameHyperLink').html() == "")
                $('#ArtistRow').hide();

        }



        $(document).ready(function () {
            //DeveloperLogoUploadWidget = new ImageUploadWidget("screenshot_viewable", $("#DeveloperLogoUploadWidgetDiv"));


            ThumbnailUploadWidget = new qq.FileUploader({
                // pass the dom node (ex. $(selector)[0] for jQuery users)
                element: document.getElementById('EditThumbnailSection'),
                // path to server-side upload script
                action: '../Public/Upload.ashx?image=true&method=set&property=' + "TempScreenshot" + "&hashname=" + urlParams['ContentObjectID'].replace(":", "_"),
                allowedExtensions: ['jpg', 'jpeg', 'png', 'gif'],
                messages: {
                    // error messages, see qq.FileUploaderBasic for content            
                },


                onComplete: function (id, filename, responseJSON) {
                    $('#ctl00_ContentPlaceHolder1_ScreenshotImage')[0].src = '../Public/Upload.ashx?image=true&method=get&property=' + "TempScreenshot" + "&hashname=" + responseJSON.newfilename;
                    $('#ctl00_ContentPlaceHolder1_ScreenshotImage').attr('newfilename', responseJSON.newfilename);
                }
            });

            var DeveloperLogoUploadWidget = new qq.FileUploader({
                // pass the dom node (ex. $(selector)[0] for jQuery users)
                element: document.getElementById('DeveloperLogoUploadWidgetBase'),
                // path to server-side upload script
                action: '../Public/Upload.ashx?image=true&method=set&property=' + "TempDeveloperLogo" + "&hashname=" + urlParams['ContentObjectID'].replace(":", "_"),
                allowedExtensions: ['jpg', 'jpeg', 'png', 'gif'],
                messages: {
                    // error messages, see qq.FileUploaderBasic for content            
                },
                fileTemplate: '<li>' +
                    '<span class="qq-upload-file">&nbsp</span>' +
                    '<span class="qq-upload-spinner">&nbsp</span>' +
                    '<span class="qq-upload-size">&nbsp</span>' +
                    '<a class="qq-upload-cancel" href="#">Cancel</a>' +
                    '<span class="qq-upload-failed-text"></span>' +
                '</li>',
                onComplete: function (id, filename, responseJSON) {
                    $('#DeveloperLogoImage')[0].src = '../Public/Upload.ashx?image=true&method=get&property=' + "TempDeveloperLogo" + "&hashname=" + responseJSON.newfilename;
                    $('#DeveloperLogoImage').attr('newfilename', responseJSON.newfilename);
                    $('#DeveloperLogoRow').show();
                }
            });

            var SponsorLogoUploadWidget = new qq.FileUploader({
                // pass the dom node (ex. $(selector)[0] for jQuery users)
                element: document.getElementById('SponsorLogoUploadWidgetBase'),
                // path to server-side upload script
                action: '../Public/Upload.ashx?image=true&method=set&property=' + "TempSponsorLogo" + "&hashname=" + urlParams['ContentObjectID'].replace(":", "_"),
                allowedExtensions: ['jpg', 'jpeg', 'png', 'gif'],
                messages: {
                    // error messages, see qq.FileUploaderBasic for content            
                },
                fileTemplate: '<li>' +
                    '<span class="qq-upload-file">&nbsp</span>' +
                    '<span class="qq-upload-spinner">&nbsp</span>' +
                    '<span class="qq-upload-size">&nbsp</span>' +
                    '<a class="qq-upload-cancel" href="#">Cancel</a>' +
                    '<span class="qq-upload-failed-text"></span>' +
                '</li>',
                onComplete: function (id, filename, responseJSON) {
                    $('#SponsorLogoImage')[0].src = '../Public/Upload.ashx?image=true&method=get&property=' + "TempSponsorLogo" + "&hashname=" + responseJSON.newfilename;
                    $('#SponsorLogoImage').attr('newfilename', responseJSON.newfilename);
                    $('#SponsorLogoRow').show();
                }
            });

            var SupportingFileUploadWidget = new qq.FileUploader({
                // pass the dom node (ex. $(selector)[0] for jQuery users)
                element: document.getElementById('SupportingFileUploadWidgetBase'),
                // path to server-side upload script
                action: '../Public/Upload.ashx?image=true&method=set&property=' + "TempSupportingFile" + "&hashname=" + urlParams['ContentObjectID'].replace(":", "_"),
                allowedExtensions: [],
                messages: {
                    // error messages, see qq.FileUploaderBasic for content            
                },
                fileTemplate: '<li>' +
                    '<span class="qq-upload-file">&nbsp</span>' +
                    '<span class="qq-upload-spinner">&nbsp</span>' +
                    '<span class="qq-upload-size">&nbsp</span>' +
                    '<a class="qq-upload-cancel" href="#">Cancel</a>' +
                    '<span class="qq-upload-failed-text"></span>' +
                '</li>',
                onComplete: function (id, filename, responseJSON) {
                    $('#SupportingFileUploadWidgetBase').attr('newfilename', responseJSON.newfilename);
                    $('#SupportingFileUploadWidgetBase').attr('filename', filename);
                    Enable('UploadSupportingFile');
                }
            });

            $("#EditDistributionStatementType").buttonset();

            InitialHideShow();
            $('#EditDistributionStatementType').change(function () {
                PreviewDistributionStatement();
            });

            $('#EditDistributionOffice').keyup(function () {
                PreviewDistributionStatement();
            });


            $('#EditDistributionReason').combobox();
            $('#EditDistributionReason').change(function () {
                PreviewDistributionStatement();
            });

            $('#EditDistributionRegulation').keyup(function () {
                PreviewDistributionStatement();
            });
            $('#EditDistributionDeterminationDate').keyup(function () {
                PreviewDistributionStatement();
            });

            $('#EditDistributionDeterminationDate').datepicker({
                onSelect: function (date) {
                    PreviewDistributionStatement();
                },
                selectWeek: true,
                inline: true,
                startDate: '01/01/1900',
                firstDay: 1
            });

            $('#RequestAccessForm').dialog({ title: 'Request Access', autoOpen: false, buttons: { Send: function () { SendRequest() } } });
            $('#RequestAccess').click(function () {

                $('#RequestAccessForm').dialog('open');

            });
            $('#editLink').click(function () {

                if ($('#editLink').attr('disabled') == 'disabled') return;

                if ($('#editLink').html() == "Edit") {
                    //BindModelDetails();
                    $('#editLink').html("Stop Editing");
                    $('#EditDetails').show();
                    $('#EditAssetInfo').show();
                    $('#EditSponsorInfo').show();
                    $('#EditDeveloperInfo').show();
                    $('#UploadSupportingFile').show();
                    $('#DeveloperInfoSection').show();
                    $('#SponsorInfoSection').show();
                    $('.deletesupportingfilebutton').show();
                    $('#EditThumbnail').show();

                    $('#EditDistributionStatement').show();


                    $('#DistributionStatementSection').show();
                }
                else {
                    $('#editLink').html("Edit");
                    EnableAllSections();
                    //BindModelDetails();
                    InitialHideShow();
                    $('#EditDetails').hide();
                    $('#EditAssetInfo').hide();
                    $('#EditSponsorInfo').hide();
                    $('#EditDeveloperInfo').hide();
                    $('#UploadSupportingFile').hide();
                    $('.deletesupportingfilebutton').hide();
                    $('#EditThumbnail').hide();
                    $('#EditDistributionStatement').hide();

                }


            });

            $('#EditThumbnail').click(function () {

                if ($('#EditThumbnail').attr('disabled') == 'disabled') return;
                if ($('#EditThumbnail').html() == "Upload Screenshot") {
                    $('#EditThumbnailSection').show();
                    $('#EditThumbnail').html('Save');
                    $('#EditThumbnailCancel').show();
                    DisableAllSections();
                    Enable('EditThumbnail');

                    $('#ctl00_ContentPlaceHolder1_ScreenshotImage').attr('backupsrc', $('#ctl00_ContentPlaceHolder1_ScreenshotImage')[0].src);



                } else {
                    $('#EditThumbnail').html('Upload Screenshot');
                    $('#EditThumbnailSection').hide();
                    $('#EditThumbnailCancel').hide();
                    EnableAllSections();

                    //UpdateAssetData(string Title, string Description, string Keywords, string License)
                    $.ajax({
                        type: "POST",
                        url: "Model.aspx/UpdateScreenshot",
                        data: JSON.stringify({ pid: urlParams['ContentObjectID'], newfilename: $('#ctl00_ContentPlaceHolder1_ScreenshotImage').attr('newfilename') }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (e) {
                            console.log(e);
                            if (e.d.Success == true) {
                                $('#ctl00_ContentPlaceHolder1_ScreenshotImage')[0].src = $('#SponsorLogoImage').attr('backupSrc');
                            }

                        },
                        error: function (e, xhr) {
                            alert(e);
                        }
                    });
                }
            });
            $('#EditThumbnailCancel').click(function () {
                $('#EditThumbnail').html('Upload Screenshot');
                $('#EditThumbnailSection').hide();
                $('#EditThumbnailCancel').hide();

                $('#ctl00_ContentPlaceHolder1_ScreenshotImage')[0].src = $('#ctl00_ContentPlaceHolder1_ScreenshotImage').attr('backupsrc');
                $('#ctl00_ContentPlaceHolder1_ScreenshotImage').attr('backupsrc', "");

                EnableAllSections();
            });



            $('#EditDistributionStatement').click(function () {
                if ($('#EditDistributionStatement').attr('disabled') == 'disabled') return;

                if ($('#EditDistributionStatement').html() == "Save") {

                    //UpdateAssetData(string Title, string Description, string Keywords, string License)
                    $.ajax({
                        type: "POST",
                        url: "Model.aspx/UpdateDistributionInfo",
                        data: JSON.stringify({ Class: $("#EditDistributionStatementType :radio:checked").val(), DeterminationDate: $('#EditDistributionDeterminationDate').val(), Office: $('#EditDistributionOffice').val(), Regulation: $('#EditDistributionRegulation').val(), Reason: $('#EditDistributionReason').val(), pid: urlParams['ContentObjectID'] }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (e) {
                            console.log(e);
                            if (e.d.Success == true) {
                                $('#DistributionLabel').html(e.d.Class);
                                $('#DistributionStatementText').html(e.d.FullText);
                                $('#DistributionStatementText').attr('backupStatement', $('#DistributionStatementText').html());
                                $('#DistributionStatementText').css('color', 'black');
                                $('#EditDistributionReasonLabel').html(e.d.Reason);
                                $('#EditDistributionReason').combobox('autocomplete', e.d.Reason);
                            }

                        },
                        error: function (e, xhr) {
                            alert(e);
                        }
                    });

                }

                if ($('#EditDistributionStatement').html() == "Edit") {
                    DisableAllSections();
                    Enable('EditDistributionStatement');
                    Enable('DistributionStatementSection');
                    $('#EditDistributionStatement').html("Save");
                    $('#EditDistributionStatementCancel').show();
                    $('#DistributionStatementEditSection').show();
                    $('#EditDistributionReason').combobox('autocomplete', $('#EditDistributionReasonLabel').val());
                    $("#EditDistributionStatementType :radio").next().attr("aria-pressed", false);

                    $("#EditDistributionStatementType :radio[value='" + $('#DistributionLabel').html() + "']").next().attr("aria-pressed", true);
                    $("#EditDistributionStatementType :radio[value='" + $('#DistributionLabel').html() + "']").attr('checked', true);
                    $('#DistributionStatementText').attr('backupStatement', $('#DistributionStatementText').html());


                } else {
                    EnableAllSections();
                    $('#EditDistributionStatement').html("Edit");
                    $('#EditDistributionStatementCancel').hide();
                    $('#DistributionStatementEditSection').hide();

                }
            });

            $('#EditDistributionStatementCancel').click(function () {
                EnableAllSections();
                $('#DistributionStatementEditSection').hide();
                $('#EditDistributionStatementCancel').hide();
                $('#EditDistributionStatement').html("Edit");
                $('#DistributionStatementText').html($('#DistributionStatementText').attr('backupStatement'));
                $('#DistributionStatementText').css('color', 'black');
            });


            $('#EditDetails').click(function () {

                if ($('#EditDetails').attr('disabled') == 'disabled') return;

                if ($('#EditDetails').html() == "Save") {

                    //UpdateAssetData(string Title, string Description, string Keywords, string License)
                    $.ajax({
                        type: "POST",
                        url: "Model.aspx/UpdateDetails",
                        data: JSON.stringify({ polys: $('#EditNumPolygonsLabel').val(), textures: $('#EditNumTexturesLabel').val(), format: $('#EditFormatLabel').val(), pid: urlParams['ContentObjectID'] }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (e) {
                            console.log(e);
                            if (e.d.Success == true) {
                                $('#FormatLabel').html(e.d.format);
                                $('#NumPolygonsLabel').html(e.d.polys);
                                $('#NumTexturesLabel').html(e.d.textures);
                            }

                        },
                        error: function (e, xhr) {
                            alert(e);
                        }
                    });

                }
                $('#DeveloperInfoSection').show();
                $('#SponsorInfoSection').show();

                if ($('#EditDetails').html() == "Edit") {
                    DisableAllSections();

                    $('#AssetDetailsSection').removeAttr('disabled');
                    Enable('EditDetails');

                    $('#EditDetails').html('Save');
                    $('#EditDetailsCancel').show();
                    $('#FormatLabel').hide();
                    $('#NumPolygonsLabel').hide();
                    $('#NumTexturesLabel').hide();

                    $('#EditFormatLabel').show();
                    $('#EditNumPolygonsLabel').show();
                    $('#EditNumTexturesLabel').show();

                    $('#EditFormatLabel').val($('#FormatLabel').html());
                    $('#EditNumPolygonsLabel').val($('#NumPolygonsLabel').html());
                    $('#EditNumTexturesLabel').val($('#NumTexturesLabel').html());
                }
                else {
                    EnableAllSections();
                    $('#EditDetails').html('Edit');
                    $('#EditDetailsCancel').hide();

                    $('#FormatLabel').show();
                    $('#NumPolygonsLabel').show();
                    $('#NumTexturesLabel').show();

                    $('#EditFormatLabel').hide();
                    $('#EditNumPolygonsLabel').hide();
                    $('#EditNumTexturesLabel').hide();
                }
            });

            $('#EditDetailsCancel').click(function () {

                EnableAllSections();
                $('#EditDetails').html('Edit');
                $('#EditDetailsCancel').hide();

                $('#FormatLabel').show();
                $('#NumPolygonsLabel').show();
                $('#NumTexturesLabel').show();

                $('#EditFormatLabel').hide();
                $('#EditNumPolygonsLabel').hide();
                $('#EditNumTexturesLabel').hide();
            });

            $('#EditAssetInfo').click(function () {

                if ($('#EditAssetInfo').attr('disabled') == 'disabled') return;

                if ($('#EditAssetInfo').html() == "Save") {

                    //UpdateAssetData(string Title, string Description, string Keywords, string License)
                    $.ajax({
                        type: "POST",
                        url: "Model.aspx/UpdateAssetData",
                        data: JSON.stringify({ Title: $('#EditTitle').val(), Description: $('#EditDescription').val(), Keywords: $('#EditKeywords').val(), License: $('#LicenseType').val(), pid: urlParams['ContentObjectID'] }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (e) {
                            console.log(e);
                            if (e.d.Success == true) {


                                $('#DescriptionLabel').html(e.d.Description);
                                $('#TitleLabel').html(e.d.Title);

                                var keywords = "";
                                for (var i = 0; i < e.d.Keywords.length; i++) {
                                    keywords += "<a class=\"Hyperlink\" href=\"Results.aspx?ContentObjectID=Devtest2:539&amp;Keywords=" + e.d.Keywords[i] + "\">" + e.d.Keywords[i] + "</a>&nbsp&nbsp  ";
                                }
                                $('#keywords').html(keywords);

                                $('#CCLHyperLink').children()[0].src = e.d.LicenseImage;
                                $('#CCLHyperLink').attr('href', e.d.LicenseURL);
                                $('#CCLHyperLink').children()[0].title = $('#LicenseType').val();
                            }

                        },
                        error: function (e, xhr) {
                            alert(e);
                        }
                    });


                }
                if ($('#EditAssetInfo').html() == "Edit") {
                    DisableAllSections();

                    $('#_3DAssetSection').css('width', $('#_3DAssetSection').width() + 'px');

                    $('#_3DAssetSection').removeAttr('disabled');

                    Enable('EditAssetInfo');
                    $('#EditAssetInfo').html("Save");
                    $('#EditKeywords').show();
                    $('#EditDescription').show();
                    $('#EditTitle').show();
                    $('#DescriptionLabel').hide();
                    $('#keywords').hide();
                    $('#keywordLabel').show();
                    $('#TitleLabel').hide();
                    $('#DownloadButton').hide();
                    $('#SelectLicenseArea').show();
                    //$('#EditKeywords.Text = co.Keywords;
                    //$('#EditTitle.Text = co.Title;
                    $('#EditAssetInfoCancel').show();
                    $('#CCLHyperLink').hide();
                    $('#ReportViolationButton').hide();
                    $('#ir').hide();



                    $('#EditDescription').val($('#DescriptionLabel').html());
                    $('#EditTitle').val($('#TitleLabel').html());

                    if ($('#CCLHyperLink').attr('Title') == 'Public Domain') {
                        $('#LicenseType').val('publicdomain');
                    }
                    if ($('#CCLHyperLink').attr('Title') == 'Attribution') {
                        $('#LicenseType').val('by');
                    }
                    if ($('#CCLHyperLink').attr('Title') == 'Attribution-ShareAlike') {
                        $('#LicenseType').val('by-sa');
                    }
                    if ($('#CCLHyperLink').attr('Title') == 'Attribution-NoDerivatives') {
                        $('#LicenseType').val('by-nd');
                    }
                    if ($('#CCLHyperLink').attr('Title') == 'Attribution-NonCommercial') {
                        $('#LicenseType').val('by-nc');
                    }
                    if ($('#CCLHyperLink').attr('Title') == 'Attribution-NonCommercial-ShareAlike') {
                        $('#LicenseType').val('by-nc-sa');
                    }
                    if ($('#CCLHyperLink').attr('Title') == 'Attribution-NonCommercial-NoDerivatives') {
                        $('#LicenseType').val('by-nc-nd');
                    }


                }
                else {
                    EnableAllSections();
                    $('#EditAssetInfo').html("Edit");
                    $('#EditKeywords').hide();
                    $('#EditDescription').hide();
                    $('#EditTitle').hide();
                    $('#DescriptionLabel').show();
                    $('#keywords').show();
                    $('#TitleLabel').show();
                    $('#EditAssetInfoCancel').hide();
                    $('#DownloadButton').show();
                    $('#SelectLicenseArea').hide();
                    $('#CCLHyperLink').show();
                    $('#ReportViolationButton').show();
                    $('#ir').show();
                }
            });

            $('#EditAssetInfoCancel').click(function () {
                $('#EditAssetInfo').html("Edit");
                $('#EditKeywords').hide();
                $('#EditDescription').hide();
                $('#EditTitle').hide();
                $('#DescriptionLabel').show();
                $('#keywords').show();
                $('#TitleLabel').show();
                $('#EditAssetInfoCancel').hide();
                $('#DownloadButton').show();
                $('#CCLHyperLink').show();
                $('#ReportViolationButton').show();
                $('#keywords').show();
                $('#ir').show();
                $('#SelectLicenseArea').hide();
                EnableAllSections();
            });

            $('#EditDeveloperInfo').click(function () {

                if ($('#EditDeveloperInfo').attr('disabled') == 'disabled') return;
                if ($('#EditDeveloperInfo').html() == "Save") {

                    //UpdateAssetData(string Title, string Description, string Keywords, string License)
                    $.ajax({
                        type: "POST",
                        url: "Model.aspx/UpdateDeveloperInfo",
                        data: JSON.stringify({ DeveloperName: $('#EditDeveloperNameHyperLink').val(), ArtistName: $('#EditArtistNameHyperLink').val(), MoreInfoURL: $('#EditMoreInformationURL').val(), pid: urlParams['ContentObjectID'], newfilename: $('#DeveloperLogoImage').attr('newfilename') }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (e) {
                            console.log(e);
                            if (e.d.Success == true) {
                                $('#DeveloperNameHyperLink').html(e.d.DeveloperName);
                                $('#ArtistNameHyperLink').html(e.d.ArtistName);
                                $('#MoreDetailsHyperLink').html(e.d.MoreInfoURL);

                                $('#DeveloperNameHyperLink').attr('href', "http://localhost/VwarWeb/Public/Results.aspx?DeveloperName=" + e.d.DeveloperName);
                                $('#ArtistNameHyperLink').attr('href', "http://localhost/VwarWeb/Public/Results.aspx?ArtistName=" + e.d.ArtistName);
                                $('#ArtistNameHyperLink').attr('href', e.d.MoreInfoURL);
                                $('#DeveloperLogoImage')[0].src = "Serve.ashx?pid=" + urlParams['ContentObjectID'] + "&mode=GetDeveloperLogo"

                            }

                        },
                        error: function (e, xhr) {
                            alert(e);
                        }
                    });

                }

                $('#DeveloperInfoSection').show();
                $('#SponsorInfoSection').show();

                if ($('#EditDeveloperInfo').html() == "Edit") {

                    $('#DeveloperLogoImage').attr('newfilename', "");
                    $('#DeveloperLogoImage').attr('backupSrc', $('#DeveloperLogoImage')[0].src);
                    DisableAllSections();

                    $('#DeveloperInfoSection').removeAttr('disabled');
                    Enable('EditDeveloperInfo');

                    $('#EditDeveloperInfo').html("Save");
                    $('#EditDeveloperInfoCancel').show();

                    $('#DeveloperNameHyperLink').show();
                    $('#ArtistNameHyperLink').show();

                    $('#EditDeveloperNameHyperLink').show();
                    $('#EditArtistNameHyperLink').show();

                    $('#EditDeveloperNameHyperLink').val($('#DeveloperNameHyperLink').html());
                    $('#EditArtistNameHyperLink').val($('#ArtistNameHyperLink').html());

                    $('#EditMoreInformationURL').val($('#MoreDetailsHyperLink').html());

                    $('#UploadDeveloperLogoRow').show();
                    $('#DeveloperRow').show();
                    $('#ArtistRow').show();
                    $('#MoreDetailsRow').show();
                    $('#EditMoreInformationURL').show();
                    $('#DeveloperNameHyperLink').hide();
                    $('#ArtistNameHyperLink').hide();
                    Backup_DeveloperLogoImageFileName = "";
                    Backup_DeveloperLogoImageFileNameId = "";
                }
                else {
                    EnableAllSections();
                    $('#EditDeveloperInfo').html("Edit");
                    $('#EditDeveloperInfoCancel').hide();

                    Backup_DeveloperLogoImageFileName = "";
                    Backup_DeveloperLogoImageFileNameId = "";

                    $('#DeveloperNameHyperLink').show();
                    $('#ArtistNameHyperLink').show();
                    $('#EditDeveloperNameHyperLink').hide();
                    $('#EditArtistNameHyperLink').hide();
                    $('#UploadDeveloperLogoRow').hide();
                    $('#EditMoreInformationURL').hide();
                }

            });

            $('#EditDeveloperInfoCancel').click(function () {

                $('#DeveloperLogoImage')[0].src = $('#DeveloperLogoImage').attr('backupSrc');
                $('#DeveloperLogoImage').attr('newfilename', "");
                $('#EditDeveloperInfo').html("Edit");
                $('#EditDeveloperInfoCancel').hide();

                $('#DeveloperNameHyperLink').show();
                $('#ArtistNameHyperLink').show();
                $('#EditDeveloperNameHyperLink').hide();
                $('#EditArtistNameHyperLink').hide();
                $('#UploadDeveloperLogoRow').hide();
                $('#EditMoreInformationURL').hide();

                if (Backup_DeveloperLogoImageFileName != "") {
                    Backup_DeveloperLogoImageFileName = "";
                    Backup_DeveloperLogoImageFileNameId = "";
                }

                EnableAllSections();
            });


            $('#EditSponsorInfo').click(function () {

                if ($('#EditSponsorInfo').attr('disabled') == 'disabled') return;
                if ($('#EditSponsorInfo').html() == "Save") {

                    //UpdateAssetData(string Title, string Description, string Keywords, string License)
                    $.ajax({
                        type: "POST",
                        url: "Model.aspx/UpdateSponsorInfo",
                        data: JSON.stringify({ SponsorName: $('#EditSponsorNameLabel').val(), pid: urlParams['ContentObjectID'], newfilename: $('#SponsorLogoImage').attr('newfilename') }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (e) {
                            console.log(e);
                            if (e.d.Success == true) {
                                $('#SponsorNameLabel').html(e.d.SponsorName);
                                $('#SponsorLogoImage')[0].src = "Serve.ashx?pid=" + urlParams['ContentObjectID'] + "&mode=GetSponsorLogo"

                            }

                        },
                        error: function (e, xhr) {
                            alert(e);
                        }
                    });

                }

                $('#DeveloperInfoSection').show();
                $('#SponsorInfoSection').show();

                if ($('#EditSponsorInfo').html() == "Edit") {
                    DisableAllSections();

                    $('#SponsorLogoImage').attr('newfilename', "");
                    $('#SponsorLogoImage').attr('backupSrc', $('#SponsorLogoImage')[0].src);

                    $('#SponsorInfoSection').removeAttr('disabled');
                    Enable('EditSponsorInfo');

                    $('#EditSponsorInfo').html('Save');
                    $('#EditSponsorInfoCancel').show();

                    $('#EditSponsorNameLabel').show();
                    $('#EditSponsorNameLabel').val($('#SponsorNameLabel').html());
                    $('#SponsorNameLabel').hide();
                    $('#UploadSponsorLogoRow').show();
                    $('#SponsorNameRow').show();

                    Backup_SponsorLogoImageFileName = "";
                    Backup_SponsorLogoImageFileNameId = "";
                }
                else {
                    EnableAllSections();
                    $('#EditSponsorInfo').html('Edit');
                    $('#EditSponsorInfoCancel').hide();
                    $('#EditSponsorNameLabel').hide();
                    $('#SponsorNameLabel').show();
                    $('#UploadSponsorLogoRow').hide();
                    Backup_SponsorLogoImageFileName = "";
                    Backup_SponsorLogoImageFileNameId = "";


                }
            });
            $('#EditSponsorInfoCancel').click(function () {

                $('#SponsorLogoImage')[0].src = $('#SponsorLogoImage').attr('backupSrc');
                $('#SponsorLogoImage').attr('newfilename', "");
                EnableAllSections();
                $('#EditSponsorInfo').html('Edit');
                $('#EditSponsorInfoCancel').hide();
                $('#EditSponsorNameLabel').hide();
                $('#SponsorNameLabel').show();
                $('#UploadSponsorLogoRow').hide();
                Backup_SponsorLogoImageFileName = "";
                Backup_SponsorLogoImageFileNameId = "";

                if (Backup_SponsorLogoImageFileName != "") {
                    Backup_SponsorLogoImageFileName = "";
                    Backup_SponsorLogoImageFileNameId = "";
                }

            });

            $('#UploadSupportingFileCancel').click(function () {

                EnableAllSections();
                $('#UploadSupportingFile').html('Add');
                $('#UploadSupportingFileCancel').hide();
                $('#UploadSupportingFileSection').hide();
            });

            $('#UploadSupportingFile').click(function () {

                if ($('#UploadSupportingFile').attr('disabled') == 'disabled') return;
                if ($('#UploadSupportingFile').html() == 'Save') {

                    //UpdateAssetData(string Title, string Description, string Keywords, string License)
                    $.ajax({
                        type: "POST",
                        url: "Model.aspx/UploadSupportingFileHandler",
                        data: JSON.stringify({ filename: $('#SupportingFileUploadWidgetBase').attr('filename'), description: $('#SupportingFileUploadDescription').val(), pid: urlParams['ContentObjectID'], newfilename: $('#SupportingFileUploadWidgetBase').attr('newfilename') }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (e) {
                            console.log(e);
                            if (e.d.Success == true) {
                                $('#SupportingFileUploadWidgetBase').attr('newfilename', "");
                                $('#SupportingFileUploadWidgetBase').attr('filename', "");
                                GetSupportingFiles();
                            }

                        },
                        error: function (e, xhr) {
                            alert(e);
                        }
                    });

                }

                $('#DeveloperInfoSection').show();
                $('#SponsorInfoSection').show();



                if ($('#UploadSupportingFile').html() == 'Add') {
                    DisableAllSections();

                    $('#SupportingFilesSection').removeAttr('disabled');
                    Enable('UploadSupportingFile');

                    $('#UploadSupportingFile').html('Save');
                    $('#UploadSupportingFileCancel').show();
                    $('#UploadSupportingFileSection').show();

                    $('#UploadSupportingFile').css('cursor', 'default');
                    $('#UploadSupportingFile').attr('disabled', 'disabled');
                    $('#UploadSupportingFile').css('color', 'lightgray');

                }
                else {
                    EnableAllSections();
                    $('#UploadSupportingFile').html('Add');
                    $('#UploadSupportingFileCancel').hide();
                    $('#UploadSupportingFileSection').hide();


                }

            });

        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="NotificationDialog" style="text-align: center">
        <div class="statusText">
        </div>
    </div>
    <div id="ConfirmationDialog" style="text-align: center">
        <div class="statusText">
        </div>
    </div>
    <div id="ModelDetails">
        <input type="hidden" runat="server" id="upAxis" />
        <input type="hidden" runat="server" id="unitScale" />
        <input type="hidden" runat="server" id="modelURL" />
        <table class="CenteredTable" cellpadding="4" border="0">
            <tr>
                <td height="600" width="564" class="ViewerWrapper">
                    <div id="ViewOptionsMultiPage" class="ViewerPageContainer">
                        <ul class="tabContainer">
                            <li class="first"><span class="ui-tabs-separator"></span><a href="#ImageView">Screenshot</a></li>
                            <li><span class="ui-tabs-separator"></span><a href="#SceneView">3D View</a></li>
                        </ul>
                        <div id="ImageView">
                            <asp:Image Height="500px" Width="500px" ID="ScreenshotImage" runat="server" ToolTip='<%# Eval("Title") %>' AlternateText="Screenshot" />
                            <br />
                        </div>
                        <div id="SceneView">
                            <VwarWeb:Viewer3D ID="Viewer" runat="server" />
                        </div>
                    </div>
                    <div style="float:right;Margin-right:10px"><a class="Hyperlink" style="margin-right:1em" id="EditThumbnailCancel">Cancel</a><a class="Hyperlink" id="EditThumbnail">Upload Screenshot</a></div>
                    <div class="addthis_toolbox addthis_default_style" style="margin-top: 3px">
                        <a href="http://www.addthis.com/bookmark.php?v=250&amp;username=xa-4cd9c9466b809f73"
                            class="addthis_button_compact">Share</a> <span class="addthis_separator">|</span>
                        <a class="addthis_button_facebook" addthis:title="Test title"></a><a class="addthis_button_twitter">
                        </a><a class="addthis_button_linkedin"></a><a class="addthis_button_digg"></a>
                    </div>
                    <div id="EditThumbnailSection" style="border: solid 1px lightGrey;border-radius: 5px;min-height:1em">
                       
                    </div>
                    <script type="text/javascript" src="http://s7.addthis.com/js/250/addthis_widget.js#username=xa-4cd9c9466b809f73"></script>
                    <!-- AddThis Button END -->
                    <div id='ViewerStatus' style='display: none;'>
                    </div>
                </td>
                <td rowspan="2">
                    &nbsp;
                </td>
                <td rowspan="2">
                    <table border="0" cellpadding="4" cellspacing="0" width="100%">
                        <tr runat="server" id="IDRow" visible="true">
                            <td id="EditorButtons" runat="server" ClientIDMode="Static">
                                <a ID="editLink" class="Hyperlink" Visible="false" runat="server" ClientIDMode="Static"
                                    >Edit</a>
                                <span id="pipehack2">&nbsp;|&nbsp;</span> 
                                <a id="PermissionsLink" runat="server" visible="false" class="Hyperlink">Permissions</a>
                                <span id="Span1">&nbsp;|&nbsp;</span>
                                <a id="DeleteLink" runat="server" visible="false"
                                    class="Hyperlink">Delete</a>
                                <asp:Label ID="IDLabel" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td id="_3DAssetSection" runat="server" ClientIDMode="Static">
                                <div class="ListTitle">
                                    <div>
                                        3D Asset <a runat="server" ClientIDMode="Static" ID="EditAssetInfo" style="cursor:pointer;float:right"   >Edit</a>
                                                 <a runat="server" ClientIDMode="Static" ID="EditAssetInfoCancel" style="cursor:pointer;float:right;margin-right:10px"    >Cancel</a>
                                                 <asp:HyperLink runat="server" ClientIDMode="Static" ID="APILink" Target="_blank" ImageUrl="/styles/images/icons/json_file.png" style="cursor:pointer;float:right;margin-right:-5px;margin-top:-5px"/>
                                        </div>
                                </div>
                                <br />
                                <table border="0" style="margin-left: 5px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="TitleLabel" runat="server" ClientIDMode="Static" CssClass="ModelTitle"></asp:Label><asp:TextBox CssClass="ModelTitle" ID="EditTitle"  style="width:100%;border-radius:5px" runat="server" ClientIDMode="Static"></asp:TextBox>
                                            <asp:HyperLink ID="SubmitterEmailHyperLink" runat="server" ClientIDMode="Static" CssClass="Hyperlink">[SubmitterEmailHyperLink]</asp:HyperLink>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:HyperLink ID="CCLHyperLink" runat="server" ClientIDMode="Static" Target="_blank" CssClass="Hyperlink">
                                            </asp:HyperLink>
                                        </td>
                                    </tr>
                                    <tr runat="server" ClientIDMode="Static" id="DescriptionRow">
                                        <td>
                                            <asp:Label ID="DescriptionLabel" style='width: 350px; display: block' runat="server" ClientIDMode="Static" /><asp:TextBox TextMode="MultiLine" ID="EditDescription" ClientIDMode="Static" runat="server" style="width:100%;border-radius:5px;height:110px"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center;">
                                            <a id="ReportViolationButton" ClientIDMode="Static" runat="server" class="Hyperlink">Report a Violation</a>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="KeywordsRow">
                                        <td>
                                            <br />
                                            <span runat="server" id="keywordLabel">Keywords:</span> <span ClientIDMode="Static" id="keywords" runat="server">
                                            </span><asp:TextBox style="color:darkblue;font-size:small;border-radius:5px;width:100%;" ID="EditKeywords" runat="server" ClientIDMode="Static"></asp:TextBox>
                                        </td>
                                        <td>
                                            <table border="0" class="CenteredTable">
                                                <tr>
                                                    <td>
                                                        <ajax:Rating ID="ir" ClientIDMode="Static" runat="server" CurrentRating='<%# Website.Common.CalculateAverageRating(Eval("Reviews")) %>'
                                                            MaxRating="5" StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar"
                                                            FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar" ReadOnly="true">
                                                        </ajax:Rating>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                     
                                    <tr>
                                        <td colspan="2" width="400">
                                            <div id="DownloadDiv">
                                                <asp:ImageButton Style="vertical-align: bottom;" ID="DownloadButton" runat="server" ClientIDMode="Static"
                                                    Text="Download" ToolTip="Download" CommandName="DownloadZip" ImageUrl="~/styles/images/Download_BTN.png" AlternateText="Download Buton" />
                                                <asp:Label ID="LoginToDlLabel" Visible="false" runat="server">
                                                    <asp:HyperLink ID="LoginLink" NavigateUrl="~/Public/Login.aspx" runat="server">Log in</asp:HyperLink>
                                                    to download </asp:Label>
                                                <div ID="RequestAccessLabel" Visible="false" runat="server">
                                                    This content is protected.
                                                    <div id="RequestAccess" class="Hyperlink" style="cursor:pointer;color:darkblue">Request Access</div></div>
                                                <br />
                                                <br />
                                                <div id="RequiresResubmitWrapper">
                                                    <asp:CheckBox ID="RequiresResubmitCheckbox" Checked="true" runat="server" Visible="false"
                                                        Enabled="false" />
                                                    <asp:Label ID="RequiresResubmitLabel" runat="server" Style="display: inline-block;
                                                        width: 260px; vertical-align: top;" Visible="false">
                                                        I agree to re-submit any modifications back to the 3D Repository.
                                                    </asp:Label>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="SelectLicenseArea" runat="server" ClientIDMode="Static" >
                                    <td colspan="2" width="400">
                                        <asp:Label runat="server">
                                        License Type</asp:Label>
                                        <select id="LicenseType"  runat="server" ClientIDMode="Static" style="width:50%;float:right;border-radius:5px">
                                            <option value="publicdomain">Public Domain</option>
                                            <option value="by" >Attribution</option>
                                            <option value="by-sa" selected="selected">Attribution-ShareAlike</option>
                                            <option value="by-nd">Attribution-NoDerivatives</option>
                                            <option value="by-nc">Attribution-NonCommercial</option>
                                            <option value="by-nc-sa">Attribution-NonCommercial-ShareAlike</option>
                                            <option value="by-nc-nd">Attribution-NonCommercial-NoDerivatives</option>
                                        </select>
                                        </td>
                                    </tr> 
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td id="DistributionStatementSection" runat="server" ClientIDMode="Static">
                                <div class="ListTitle">
                                    <div>
                                        Distribution Statement <a runat="server" ClientIDMode="Static" ID="EditDistributionStatement"  style="float:right;cursor:pointer"  >Edit</a>
                                                              <a runat="server" ClientIDMode="Static" ID="EditDistributionStatementCancel"  style="float:right;margin-right:10px;cursor:pointer"  >Cancel</a>
                                    </div>
                                    
                                </div>
                                <div id="DistributionStatementText" style="color:black;width: 30em; padding: .5em;font-size: small;" runat="server" clientidmode="Static"></div>
                                <div id="DistributionStatementEditSection">
                                
                                
	                                <div id="EditDistributionStatementType" style="text-align:center">
                                        <input type="radio" id="EditDistributionStatementTypeNA" name="EditDistributionStatementType" value="NA"/><label for="EditDistributionStatementTypeNA">None</label>
		                                <input type="radio" id="EditDistributionStatementTypeA" name="EditDistributionStatementType" value="Distribution_A"/><label for="EditDistributionStatementTypeA">A</label>
		                                <input type="radio" id="EditDistributionStatementTypeB" name="EditDistributionStatementType" value="Distribution_B"/><label for="EditDistributionStatementTypeB">B</label>
		                                <input type="radio" id="EditDistributionStatementTypeC" name="EditDistributionStatementType" value="Distribution_C"/><label for="EditDistributionStatementTypeC">C</label>
                                        <input type="radio" id="EditDistributionStatementTypeD" name="EditDistributionStatementType" value="Distribution_D"/><label for="EditDistributionStatementTypeD">D</label>
                                        <input type="radio" id="EditDistributionStatementTypeE" name="EditDistributionStatementType" value="Distribution_E"/><label for="EditDistributionStatementTypeE">E</label>
                                        <input type="radio" id="EditDistributionStatementTypeF" name="EditDistributionStatementType" value="Distribution_F" /><label for="EditDistributionStatementTypeF">F</label>
                                        <input type="radio" id="EditDistributionStatementTypeX" name="EditDistributionStatementType" value="Distribution_X"/><label for="EditDistributionStatementTypeX">X</label>
	                                </div>
                                    <div style="text-align:center;font-size:small">Distribution Type</div>
                                    <table style="width:100%;">
                                    <tr style="">
                                        <td style="width:25%;font-size:small;vertical-align:middle">DoD Office</td><td><asp:Textbox type="text" id="EditDistributionOffice" style="border-radius:5px;width:100%;float:right" runat="server" ClientIDMode="Static"/></td>
                                    </tr>
                                    <tr >
                                        <td style="width:25%;font-size:small;vertical-align:middle">DoD Regulation</td><td><asp:Textbox type="text" id="EditDistributionRegulation" style="border-radius:5px;width:100%;float:right" runat="server" ClientIDMode="Static"/></td>
                                    </tr>
                                    <tr >
                                        <td style="width:25%;font-size:small;vertical-align:middle">Determination Date</td><td><asp:Textbox type="text" id="EditDistributionDeterminationDate" style="border-radius:5px;width:100%;float:right" runat="server" ClientIDMode="Static"/></td>
                                    </tr>
                                    <tr >
                                        <td style="width:25%;font-size:small;vertical-align:middle">Reason</td><td>
                                        <asp:Textbox TextMode="MultiLine" type="text" id="EditDistributionReasonLabel" style="display:none;border-radius:5px;width:100%;float:right" runat="server" ClientIDMode="Static"/>
                                        <select id="EditDistributionReason" style="width:100%">
		                                    <option value="">Select one...</option>
		                                    <option value="Foreign Government Information">Foreign Government Information</option>
		                                    <option value="Proprietary Information">Proprietary Information</option>
		                                    <option value="Critical Technology">Critical Technology</option>
		                                    <option value="Test and Evaluation">Test and Evaluation</option>
		                                    <option value="Contractor Performance Evaluation">Contractor Performance Evaluation</option>
		                                    <option value="Premature Dissemination">Premature Dissemination</option>
		                                    <option value="Administrative or Operational Use">Administrative or Operational Use</option>
		                                    <option value="Software Documentation">Software Documentation</option>
		                                    <option value="Specific Authority (identification of valid documented authority)">Specific Authority (identification of valid documented authority)</option>
		                                    <option value="Direct Military Support">Direct Military Support</option>
		                                    
	                                    </select>
                                        </td>
                                    </tr>
                                    </table>
                                    <asp:label id="DistributionLabel" style="display:none" runat="server" ClientIDMode="Static"></asp:label>
                                    
                                
                                
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td id="DeveloperInfoSection" runat="server" ClientIDMode="Static">
                                <div class="ListTitle">
                                    <div>
                                        Developer Information <a runat="server" ClientIDMode="Static" ID="EditDeveloperInfo"  style="float:right;cursor:pointer"  >Edit</a>
                                                              <a runat="server" ClientIDMode="Static" ID="EditDeveloperInfoCancel"  style="float:right;margin-right:10px;cursor:pointer"  >Cancel</a>
                                    </div>
                                </div>
                                <table border="0" style="margin-left: 5px;width:100%">
                                    <tr runat="server" ClientIDMode="Static" id="DeveloperLogoRow">
                                        <td>
                                            <asp:Image style="max-width: 400px" ID="DeveloperLogoImage" runat="server" ClientIDMode="Static" AlternateText="Developer Logo" />
                                        </td>
                                    </tr>
                                    <tr id="UploadDeveloperLogoRow"  runat="server" ClientIDMode="Static">
                                        <td>
                                            <div id="DeveloperLogoUploadWidgetBase" style="font-size: smaller;border: solid 1px lightGrey;border-radius: 5px;vertical-align: middle;"/><div id="DeveloperLogoUploadWidgetDiv" style="font-size: smaller;border: solid 1px lightGrey;border-radius: 5px;vertical-align: middle;"><asp:Button style="font-size:smaller;height:100%" runat="server" ClientIDMode="Static" ID="DeleteDeveloperLogo" text="Delete Logo" /><asp:FileUpload style="font-size:smaller;"  runat="server" ClientIDMode="Static" ID="UploadDeveloperLogo" /></div>
                                            
                                        </td>
                                    </tr>
                                    <tr runat="server" ClientIDMode="Static" id="SubmitterEmailRow">
                                        <td>
                                            <asp:Label ID="UploadedDateLabel" runat="server" ClientIDMode="Static" Enabled="false" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr runat="server" ClientIDMode="Static" id="DeveloperRow">
                                        <td style="vertical-align:middle">
                                            Developer Name:
                                            <asp:HyperLink ID="DeveloperNameHyperLink" runat="server" ClientIDMode="Static" NavigateUrl="#" CssClass="Hyperlink">[DeveloperNameHyperLink]</asp:HyperLink><asp:TextBox ID="EditDeveloperNameHyperLink" runat="server" ClientIDMode="Static" style="border-radius:5px;width:50%;float:right" ></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr runat="server" ClientIDMode="Static" id="ArtistRow">
                                        <td style="vertical-align:middle">
                                            Artist Name:
                                            <asp:HyperLink ID="ArtistNameHyperLink" runat="server" ClientIDMode="Static" NavigateUrl="#" CssClass="Hyperlink">[ArtistNameHyperLink]</asp:HyperLink><asp:TextBox ID="EditArtistNameHyperLink" runat="server" ClientIDMode="Static" style="border-radius:5px;width:50%;float:right" ></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr runat="server" ClientIDMode="Static" id="MoreDetailsRow">
                                        <td>
                                            <br />
                                            <asp:HyperLink ID="MoreDetailsHyperLink" runat="server" ClientIDMode="Static" Target="_blank" CssClass="Hyperlink" />&nbsp;<asp:Image
                                                ID="ExternalLinkIcon" runat="server" ImageUrl="~/styles/images/externalLink.gif" Width="15px"
                                                Height="15px" ImageAlign="Bottom" AlternateText="External Link" /><asp:TextBox ID="EditMoreInformationURL" runat="server" ClientIDMode="Static" style="border-radius:5px;width:50%;float:right" ></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td id="SponsorInfoSection" runat="server" ClientIDMode="Static">
                                <div class="ListTitle">
                                    <div>
                                        Sponsor Information <a runat="server" ClientIDMode="Static" ID="EditSponsorInfo"  style="float:right;cursor:pointer" >Edit</a>
                                                            <a runat="server" ClientIDMode="Static" ID="EditSponsorInfoCancel"  style="float:right;margin-right:10px;cursor:pointer" >Cancel</a>
                                        
                                        </div>
                                </div>
                                <table border="0" style="margin-left: 5px;width:100%">
                                    <tr runat="server" ClientIDMode="Static" id="SponsorLogoRow">
                                        <td>
                                            <asp:Image  style="max-width: 400px" ClientIDMode="Static" ID="SponsorLogoImage" runat="server" AlternateText="Sponsor Logo" />
                                        </td>
                                    </tr>
                                    <tr id="UploadSponsorLogoRow"  runat="server" ClientIDMode="Static">
                                        <td>
                                            <div id="SponsorLogoUploadWidgetBase" style="font-size: smaller;border: solid 1px lightGrey;border-radius: 5px;vertical-align: middle;"/><div id="Div1" style="font-size: smaller;border: solid 1px lightGrey;border-radius: 5px;vertical-align: middle;"><asp:Button style="font-size:smaller;height:100%" runat="server" ClientIDMode="Static" ID="DeleteSponsorLogo" text="Delete Logo"  /><asp:FileUpload style="font-size:smaller;"  runat="server" ClientIDMode="Static" ID="UploadSponsorLogo" /></div>
                                        </td>
                                    </tr>
                                    <tr runat="server" ClientIDMode="Static" id="SponsorNameRow">
                                        <td style="vertical-align:middle">
                                            Sponsor Name:
                                            <asp:Label ID="SponsorNameLabel" runat="server" ClientIDMode="Static" /><asp:TextBox ID="EditSponsorNameLabel" runat="server" ClientIDMode="Static" style="border-radius:5px;width:50%;float:right" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td id="AssetDetailsSection" runat="server" ClientIDMode="Static">
                                <div class="ListTitle">
                                    <div>
                                        Asset Details <a runat="server" ClientIDMode="Static" ID="EditDetails" style="float:right;cursor:pointer" >Edit</a>
                                                      <a runat="server" ClientIDMode="Static" ID="EditDetailsCancel" style="float:right;margin-right:10px;cursor:pointer" >Cancel</a>
                                    </div>
                                </div>
                                <table border="0" style="margin-left: 5px;">
                                    <tr>
                                        <td style="vertical-align:middle">
                                            <asp:Label ID="FormatLabelHead" text="Native format: " runat="server" ClientIDMode="Static" /> <asp:Label ID="FormatLabel" runat="server" ClientIDMode="Static" /><asp:TextBox ID="EditFormatLabel" style="border-radius:5px" runat="server" ClientIDMode="Static" />
                                        </td>
                                    </tr>
                                    <tr runat="server" ClientIDMode="Static" id="NumPolygonsRow">
                                        <td style="vertical-align:middle">
                                            <br />
                                            Number of Polygons:
                                            <asp:Label ID="NumPolygonsLabel" runat="server" ClientIDMode="Static"></asp:Label><asp:TextBox ID="EditNumPolygonsLabel" style="border-radius:5px" runat="server" ClientIDMode="Static" />
                                        </td>
                                    </tr>
                                    <tr runat="server" ClientIDMode="Static" id="NumTexturesRow">
                                        <td style="vertical-align:middle">
                                            Number of Textures:
                                            <asp:Label ID="NumTexturesLabel" runat="server" ClientIDMode="Static"></asp:Label><asp:TextBox ID="EditNumTexturesLabel" style="border-radius:5px" runat="server" ClientIDMode="Static" />
                                        </td>
                                    </tr>
                                    <tr runat="server" ClientIDMode="Static" id="DownloadsRow">
                                        <td>
                                            <br />
                                            Downloads:
                                            <asp:Label ID="DownloadsLabel" runat="server" ClientIDMode="Static" ></asp:Label>
                                        </td>
                                    </tr>
                                    <tr runat="server" ClientIDMode="Static"  id="ViewsRow">
                                        <td>
                                            Views:
                                            <asp:Label ID="ViewsLabel" runat="server"  ClientIDMode="Static"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td  id="SupportingFilesSection" runat="server"  ClientIDMode="Static">
                                <div class="ListTitle">
                                    <div>
                                        Supporting Documents <a runat="server"  ClientIDMode="Static"  ID="UploadSupportingFile" style="float:right;cursor:pointer"  >Add</a>
                                                             <a runat="server"  ClientIDMode="Static" ID="UploadSupportingFileCancel" style="float:right;margin-right:10px;cursor:pointer">Cancel</a>
                                        </div>
                                </div>
                               
                                <div id="SupportingFilesListDiv">
                                    <ul id="SupportingFilesList" style="list-style-type:none;padding-left:0px;margin-top:1px;font-size:.75em">
                                        <li style="border:1px ridge lightgray;border-radius:0px">
                                            <div style="display:inline-block;width:33%;padding:5px 0px 5px 5px;vertical-align:top">Filename</div>
                                            <div style="display:inline-block;width:56%;border-left:1px solid lightgray;border-right:1px solid lightgray;padding:5px 0px 5px 5px;vertical-align:top">Description test to see if it will wrap nicely</div>
                                            <div style="display:inline-block;width:6%;margin-top:5px"><a style="float:right;" href="download.aspx"><img  src="../styles/images/icons/expand.jpg" /></a><a style="float:right;padding-right:0px" href="download.aspx"><img  src="../styles/images/icons/expand_disabled.jpg" /></a></div>
                                        </li>
                                        <li style="border:1px ridge lightgray;border-radius:0px">
                                            <div style="display:inline-block;width:33%;padding:5px 0px 5px 5px;vertical-align:top">Filename</div>
                                            <div style="display:inline-block;width:56%;border-left:1px solid lightgray;border-right:1px solid lightgray;padding:5px 0px 5px 5px;vertical-align:top">Description test to see if it will wrap nicely</div>
                                            <div style="display:inline-block;width:6%;margin-top:5px"><a style="float:right;" href="download.aspx"><img  src="../styles/images/icons/expand.jpg" /></a><a style="float:right;padding-right:0px" href="download.aspx"><img  src="../styles/images/icons/expand_disabled.jpg" /></a></div>
                                        </li>
                                         <li style="border:1px ridge lightgray;border-radius:0px">
                                            <div style="display:inline-block;width:33%;padding:5px 0px 5px 5px;vertical-align:top">Filename</div>
                                            <div style="display:inline-block;width:56%;border-left:1px solid lightgray;border-right:1px solid lightgray;padding:5px 0px 5px 5px;vertical-align:top">Description test to see if it will wrap nicely</div>
                                            <div style="display:inline-block;width:6%;margin-top:5px"><a style="float:right;" href="download.aspx"><img  src="../styles/images/icons/expand.jpg" /></a><a style="float:right;padding-right:0px" href="download.aspx"><img  src="../styles/images/icons/expand_disabled.jpg" /></a></div>
                                        </li>
                                         <li style="border:1px ridge lightgray;border-radius:0px">
                                            <div style="display:inline-block;width:33%;padding:5px 0px 5px 5px;vertical-align:top">Filename</div>
                                            <div style="display:inline-block;width:56%;border-left:1px solid lightgray;border-right:1px solid lightgray;padding:5px 0px 5px 5px;vertical-align:top">Description test to see if it will wrap nicely</div>
                                            <div style="display:inline-block;width:6%;margin-top:5px"><a style="float:right;" href="download.aspx"><img  src="../styles/images/icons/expand.jpg" /></a><a style="float:right;padding-right:0px" href="download.aspx"><img  src="../styles/images/icons/expand_disabled.jpg" /></a></div>
                                        </li>
                                    </ul>
                                </div>
                                 <div id="UploadSupportingFileSection"  runat="server" ClientIDMode="Static">
                                       
                                            <asp:TextBox runat="server" ClientIDMode="Static" TextMode="MultiLine" id="SupportingFileUploadDescription" style="border-radius:5px;width:100%"></asp:TextBox><div id="SupportingFileUploadWidgetBase" style="font-size: smaller;border: solid 1px lightGrey;border-radius: 5px;vertical-align: middle;"/><div id="Div2" style="font-size: smaller;border: solid 1px lightGrey;border-radius: 5px;vertical-align: middle;"><asp:FileUpload style="font-size:smaller;vertical-align:top"  runat="server" ClientIDMode="Static" ID="SupportingFileUpload" /></div>
                                                
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="updatePanel" runat="server" ClientIDMode="Static" EnableViewState="true" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="ListTitle" style="width: 550px; margin-left: 6px;">
                                <div>
                                    Comments and Reviews</div>
                            </div>
                            <br />
                            <asp:Label ID="NotRatedLabel" runat="server" ClientIDMode="Static" Style="margin-left: 10px" Font-Bold="true"
                                Text="Not yet rated.  Be the first to rate!<br /><br />" Visible="false"></asp:Label>
                            <div style="margin-left: 5px">
                                <asp:GridView ID="CommentsGridView" runat="server" AutoGenerateColumns="false" BorderStyle="None"
                                    GridLines="None" ShowHeader="false">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <ajax:Rating ID="ir" runat="server" CurrentRating='<%# Eval("Rating") %>' MaxRating="5"
                                                    StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar" FilledStarCssClass="filledRatingStar"
                                                    EmptyStarCssClass="emptyRatingStar" ReadOnly="false">
                                                </ajax:Rating>
                                                <br />
                                                <asp:Label ID="Label2" Text='<%# Eval("Text") %>' runat="server"></asp:Label>
                                                <br />
                                                Submitted By:
                                                <asp:Label Text='<%#Website.Common.GetFullUserName( Eval("SubmittedBy")) %>' runat="server"></asp:Label>
                                                On
                                                <asp:Label ID="Label1" Text='<%# Eval("SubmittedDate","{0:d}") %>' runat="server"></asp:Label>
                                                <hr />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div id="UnauthenticatedReviewSubmission" style="display: none;" runat="server">
                                    <asp:HyperLink ID="ReveiwLoginHyperLink" NavigateUrl="~/Public/Login.aspx" Text="Log in"
                                        runat="server" />
                                    to submit a review
                                </div>
                                <div id="AuthenticatedReviewSubmission" style="display: none; margin-left: 5px" runat="server">
                                    <asp:Label Text="Write your own review:" runat="server" />
                                    <br />
                                    <ajax:Rating ID="rating" runat="server" CurrentRating="3" MaxRating="5" StarCssClass="ratingStar"
                                        OnChanged="Rating_Set" WaitingStarCssClass="savedRatingStar" FilledStarCssClass="filledRatingStar"
                                        EmptyStarCssClass="emptyRatingStar">
                                    </ajax:Rating>
                                    <br />
                                    <asp:TextBox ID="ratingText" runat="server" TextMode="MultiLine" Columns="44" SkinID="TextBox"
                                        Rows="4"></asp:TextBox>
                                    <br />
                                    <asp:RegularExpressionValidator ID="regexTextBox1" ControlToValidate="ratingText"
                                        runat="server" ValidationExpression="^[\s\S]{0,99}$" Text="99 characters max" />
                                    <br />
                                    <asp:ImageButton ID="submitRating" Text="Add Rating" runat="server" OnClick="Rating_Click"
                                        ImageUrl="~/styles/images/Add_Rating_BTN.png" AlternateText="Submit Rating" />
                                    <br />
                                    <br />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <div id='deleteSupportingFileDialog'></div>
        <div id='RequestAccessForm'>
            <div style="font-size:small">An access request will be sent to the owner of this model. Enter a message below to include in your request.</div>
            <textarea id="RequestAccessMessage" style="width:260px;height:100px"></textarea>
        </div>
        
    </div>
     <script type="text/javascript" >
         InitialHideShow();
         GetSupportingFiles();
     </script>
    <VwarWeb:PermissionsManagementWidget ID="PermissionsManagementControl" runat="server" />
</asp:Content>
