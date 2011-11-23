<%--
Copyright 2011 U.S. Department of Defense

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
--%>



<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MyKeys.ascx.cs" Inherits="Controls_MyKeys" %>
<script type="text/javascript">
    $(function () {
        var MAX_USAGE_CHARS = 250;

        function validateMaxLength(jqo, max, errorFunc) {
            if ($(jqo).val().length > max) {
                errorFunc(jqo);
                return false;
            }
            return true;
        }

        function handleLengthValidationError(jqo) {
            $(jqo).after(
                $("<span />").html("<br/>Your description cannot be longer than 250 characters.")
                             .css("color", "red")
            );
            return jqo;
        }

        //Handlers for the statusCode callback on AJAX error
        var statusHandlers = {
            401: function () { window.location.href = "../Public/Login.aspx"; },
            500: function () { $(this).html("An unknown error occurred. Please try again later.") }
        }

        $("#RequestKeyLink").click(function (event) {
            event.preventDefault();
            $("<div id='RequestKeyDialog' />").load("KeyRequestForm.htm")
                .dialog({
                    modal: "true",
                    width: 400,
                    height: 300,
                    close: function () { $("#KeyRequestForm").die(); $("#RequestKeyDialog").remove() }
                });

            $("#KeyRequestForm").live("submit", function (event) {
                event.preventDefault();
                if (!validateMaxLength($(this).find("textarea"), MAX_USAGE_CHARS, handleLengthValidationError)) return false;
                $.ajax({
                    type: "POST",
                    url: "Profile.aspx/RequestKey",
                    contentType: "application/json; charset=utf-8",
                    context: $("#KeyRequestStatus"),
                    dataType: "json",
                    data: JSON.stringify({ Description: $(this).find("textarea").val() }),
                    success: function (obj, status, xhr) {
                        function generateRow(table, data) {
                            $(table).find("tr:last").after(
                                    "<tr>" +
                                    "<td class='key'>" + data.Key + "</td>" +
                                    "<td class='usage'>" + data.Usage + "</td>" +
                                    "<td class='state'>" + data.Active + "</td>" +
                                    "<td><a href='#' class='update-key-request Hyperlink'>Edit</a> | " +
                                    "<a href='#' class='delete-key-request Hyperlink'>Delete</a>" +
                                    "</td>" +
                                    "</tr>"
                                );

                            if (!$(table).find("tr:last").prev().hasClass("blue")) {
                                $(table).find("tr:last").addClass("blue");
                            }
                        };

                        var params = obj.d;
                        $(this).html(params.Message).append(
                            $("<input type='submit' value='Ok' style='position: absolute; bottom: 10px; right: 45%'/>").click(function () { $("#RequestKeyDialog").dialog("close") })
                        );
                        $(this).parent().find("form").remove();
                        if (params.Key) {
                            $(this).append("<br /><br />")
                                   .append($("<span>" + params.Key + "</span>").css("font-weight", "bold"));

                            if ($("#ctl00_ContentPlaceHolder1_KeysControl_APIKeysListView_KeysTable").length > 0) {
                                generateRow($("#ctl00_ContentPlaceHolder1_KeysControl_APIKeysListView_KeysTable"), params);
                            } else {
                                generateRow($('.table-placeholder').html($(
                                    "<table id='ctl00_ContentPlaceHolder1_KeysControl_APIKeysListView_KeysTable' class='keys-table' cellspacing=0>" +
                                    "<tr><th>Key</th><th>Usage</th><th>Active</th><th>Actions</th></tr>" +
                                    "</table>"
                                )).find('table'), params)
                            }
                        }
                    },
                    statusCode: statusHandlers
                });
                return false;
            }).find("textarea").live("keyup", function () {
                $('.chars-remaining').html(Math.max(0, MAX_USAGE_CHARS - $(this).val().length));
            });

        });

        $(".delete-key-request").live("click", function (event) {
            event.preventDefault();
            var keyToDelete = $(this).parent().siblings('.key').text().trim();
            var ajaxCompleteButtons = { buttons: { "Ok": function () { $("#ConfirmDeleteDialog").remove() } } };
            $("<div id='ConfirmDeleteDialog' style='text-align: center'>" +
    		  "Are you sure you want to delete the key? This action cannot be undone." +
    		  "</div>"
		  	).dialog({
		  	    close: function () { $("#ConfirmDeleteDialog").remove() },
		  	    buttons: {
		  	        "Yes": function () {
		  	            $.ajax({
		  	                type: "POST",
		  	                url: "Profile.aspx/DeleteKey",
		  	                contentType: "application/json; charset=utf-8",
		  	                context: $("#ConfirmDeleteDialog"),
		  	                dataType: "json",
		  	                data: JSON.stringify({ Key: keyToDelete }),
		  	                success: function (obj, status, xhr) {
		  	                    $("#ConfirmDeleteDialog").dialog('option', ajaxCompleteButtons);
		  	                    var keyRows = $("#ctl00_ContentPlaceHolder1_KeysControl_APIKeysListView_KeysTable tr");
		  	                    $("#ConfirmDeleteDialog").html(obj.d);
		  	                    keyRows.filter(function (i) { return $(this).find('.key').text().trim() == keyToDelete })
                                       .remove();
		  	                    if ($("#ctl00_ContentPlaceHolder1_KeysControl_APIKeysListView_KeysTable tr").length < 2) {
		  	                        $("#ctl00_ContentPlaceHolder1_KeysControl_APIKeysListView_KeysTable").remove();
		  	                        $(".table-placeholder").html("You don't have any API keys.<br/><br/>");
		  	                    }
		  	                },
		  	                statusCode: {
		  	                    401: statusHandlers["401"],
		  	                    500: function () {
		  	                        statusHandlers["500"].call(this);
		  	                        $("#ConfirmDeleteDialog").dialog('option', ajaxCompleteButtons);
		  	                    }
		  	                }
		  	            })
		  	        },

		  	        "No": function () {
		  	            $("#ConfirmDeleteDialog").remove()
		  	        }
		  	    }
		  	})
        });

        $(".update-key-request").live("click", function (event) {
            event.preventDefault();
            var keyToUpdate = $(this).parent().siblings('.key').text().trim();
            $("<div id='UpdateKeyDialog' />").load("KeyRequestForm.htm")
            .dialog({
                modal: "true",
                width: 400,
                height: 300,
                close: function () { $("#KeyRequestForm").die(); $("#UpdateKeyDialog").remove() }
            });
            $("#KeyRequestForm").live("submit", function (event) {
                event.preventDefault();
                if (!validateMaxLength($("#KeyRequestForm").find("textarea"), MAX_USAGE_CHARS, handleLengthValidationError)) return false;
                $.ajax({
                    type: "POST",
                    url: "Profile.aspx/UpdateKey",
                    contentType: "application/json; charset=utf-8",
                    context: $("#KeyRequestStatus"),
                    dataType: "json",
                    data: JSON.stringify({ Key: keyToUpdate, Description: $(this).find("textarea").val() }),
                    success: function (obj, status, xhr) {
                        var params = obj.d;
                        $(this).parent().find("form").remove();
                        $(this).html(params.Message).append(
                            $("<input type='submit' value='Ok' style='position: absolute; bottom: 10px; right: 45%'/>").click(function () { $("#UpdateKeyDialog").dialog("close") })
                        );
                        var keyRows = $("#ctl00_ContentPlaceHolder1_KeysControl_APIKeysListView_KeysTable tr");
                        keyRows.filter(function (i) { return $(this).find('.key').text().trim() == keyToUpdate })
                                      .find(".usage").text(params.Usage);
                    },
                    statusCode: statusHandlers
                });
                return false;
            }).find("textarea").live("keyup", function () {
                $('.chars-remaining').text(Math.max(0, MAX_USAGE_CHARS - $(this).val().length));
            });

            //TODO: put original text in the textarea, figure out why this doesn't work
            //$("textarea#UsageTextArea").html($(this).parent().siblings('.usage').text().trim());

        });
    });
</script>
<style type="text/css">
    .table-placeholder
    {
        margin-top: 10px;
    }
    .table-placeholder th
    {
        font-weight: bold;
        text-align: left;
        width: 900px;
    }
    .blue td
    {
        background-color: #C2EBFF;
    }
    .keys-table td, .keys-table th
    {
        padding: 5px;
        border: none;
        vertical-align: text-top;
    }
    .key, .state, .actions
    {
        width: 100px;
    }
    .usage
    {
        width: 300px;
    }
    .ui-dialog .ui-dialog-buttonpane .ui-dialog-buttonset
    {
    	float: none;
    	text-align: center;
    }
</style>
<asp:Panel runat="server" ID="APIKeysPanel">
    <div class="ListTitle">
        My API Keys</div>
    <div class='table-placeholder'>
    <asp:ListView ID="APIKeysListView" RepeatDirection="Vertical" runat="server">
        <LayoutTemplate>
            <table id="KeysTable" class='keys-table' runat="server" width="600" cellspacing="0">
                <tr id="Tr1" >
                    <th align="left" id="th1" runat="server">
                        Key
                    </th>
                    <th id="th2" align="left" runat="server">
                        Usage
                    </th>
                    <th id="th3" align="left" runat="server">
                        Active
                    </th>
                    <th id="th4" align="left" runat="server">
                        Actions
                    </th>
                </tr>
                <tr id="ItemPlaceholder" runat="server" />
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr class='blue' runat="server">
                <td class='key'>
                    <%# ((vwar.service.host.APIKey)Container.DataItem).Key %>
                </td>
                <td class='usage'>
                    <%# ((vwar.service.host.APIKey)Container.DataItem).Usage %>
                </td>
                <td class='state'>
                    <%# ((((vwar.service.host.APIKey)Container.DataItem).State) == vwar.service.host.APIKeyState.ACTIVE) ? "Yes" : "No" %>
                </td>
                <td class='actions'>
                    <a href='#' class='update-key-request Hyperlink'>Edit</a> |
                    <a href='#' class='delete-key-request Hyperlink'>Delete</a>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
         <tr runat="server">
                <td class='key'>
                    <%# ((vwar.service.host.APIKey)Container.DataItem).Key %>
                </td>
                <td class='usage'>
                    <%# ((vwar.service.host.APIKey)Container.DataItem).Usage %>
                </td>
                <td class='state'>
                    <%# ((((vwar.service.host.APIKey)Container.DataItem).State) == vwar.service.host.APIKeyState.ACTIVE) ? "Yes" : "No" %>
                </td>
                <td class='actions'>
                    <a href='#' class='update-key-request Hyperlink'>Edit</a> |
                    <a href='#' class='delete-key-request Hyperlink'>Delete</a>
                </td>
            </tr>
        </AlternatingItemTemplate>
        <EmptyDataTemplate>            
                You don't have any API Keys.
        </EmptyDataTemplate>
    </asp:ListView>
    </div>
    <br />
    <a href="#" id="RequestKeyLink" class='Hyperlink' style="margin-left: 5px" >Request a key</a>
</asp:Panel>
