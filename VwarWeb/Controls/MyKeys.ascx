<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MyKeys.ascx.cs" Inherits="Controls_MyKeys" %>
<script type="text/javascript">
    $(function () {

        function handleDialogError() { $(this).html("An unknown error occurred. Please try again later.") };
        $("#RequestKeyLink").click(function (event) {
            event.preventDefault();
            $("<div id='RequestKeyDialog' />").load("KeyRequestForm.htm")
                        .dialog({
                            modal: "true",
                            width: 400,
                            height: 250,
                            close: function () { $("#KeyRequestForm").die(); $("#RequestKeyDialog").remove() }
                        });



            $("#KeyRequestForm").live("submit", function (event) {
                event.preventDefault();
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
                                    "<td><a href='#' class='update-key-request'>Edit</a> " +
                                    "<a href='#' class='delete-key-request'>Delete</a>" +
                                    "</td>" +
                                    "</tr>"
                                );

                            if(! $(table).find("tr:last").prev().hasClass("blue") ) {
                                $(table).find("tr:last").addClass("blue");
                            } 
                        };

                        var params = obj.d;
                        $(this).html(params.Message).append(
                            $("<input type='submit' value='Ok' style='position: absolute; bottom: 10px; right: 45%'/>").click(function(){$("#RequestKeyDialog").dialog("close")})
                        );
                        $(this).parent().find("form").remove();
                        if (params.Key) {
                            $(this).append("<br /><br />")
                                   .append($("<span>" + params.Key + "</span>").css("font-weight", "bold"));

                            if ($("#ctl00_ContentPlaceHolder1_KeysControl_APIKeysListView_KeysTable").length > 0) {
                                generateRow($("#ctl00_ContentPlaceHolder1_KeysControl_APIKeysListView_KeysTable"), params);
                            } else {
                                generateRow($('.table-placeholder').html($(
                                    "<table id='ctl00_ContentPlaceHolder1_KeysControl_APIKeysListView_KeysTable'><tr><td>Key</td><td>Usage</td><td>Active</td><td>Actions</td>"
                                )).find('table'), params)
                            }
                        }
                    },
                    error: handleDialogError
                });
                return false;
            });

        });

        $(".delete-key-request").live("click", function (event) {
            event.preventDefault();
            var keyToDelete = $(this).parent().siblings('.key').text().trim();
            $("<div id='ConfirmDeleteDialog'>" +
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
		  	                dataType: "json",
		  	                data: JSON.stringify({ Key: keyToDelete }),
		  	                success: function (obj, status, xhr) {
		  	                    var buttons = $("#ConfirmDeleteDialog").siblings(".ui-dialog-buttonpane").find(".ui-dialog-buttonset span");
		  	                    buttons.first().remove();
		  	                    buttons.last().text("Ok");
		  	                    var keyRows = $("#ctl00_ContentPlaceHolder1_KeysControl_APIKeysListView_KeysTable tr");
		  	                    $("#ConfirmDeleteDialog").html(obj.d);
		  	                    keyRows.filter(function (i) { return $(this).find('.key').text().trim() == keyToDelete })
                                       .remove();
		  	                    if ($("#ctl00_ContentPlaceHolder1_KeysControl_APIKeysListView_KeysTable tr").length < 2) {
                                    $("#ctl00_ContentPlaceHolder1_KeysControl_APIKeysListView_KeysTable").remove();
                                    $("#RequestKeyLink").before("You don't have any API keys.<br/><br/>"); 
                                }
		  	                },
		  	                error: handleDialogError,
		  	                statusCode: { 401: function () { window.location.href = "../Public/Login.aspx"; } }
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
                            height: 250,
                            close: function () { $("#KeyRequestForm").die(); $("#UpdateKeyDialog").remove() },
                        });


            $("#KeyRequestForm").live("submit", function (event) {
                event.preventDefault();

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
                            $("<input type='submit' value='Ok' style='position: absolute; bottom: 10px; right: 45%'/>").click(function(){$("#UpdateKeyDialog").dialog("close")})
                        );
                        var keyRows = $("#ctl00_ContentPlaceHolder1_KeysControl_APIKeysListView_KeysTable tr");
                        keyRows.filter(function (i) { return $(this).find('.key').text().trim() == keyToUpdate })
                                      .find(".usage").text(params.Usage);
                    },
                    error: handleDialogError
                });
                return false;
            });

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
    .keys-table td
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
</style>
<asp:Panel runat="server" ID="APIKeysPanel">
    <div class="ListTitle">
        My API Keys</div>
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
                    <a href='#' class='update-key-request'>Edit</a> 
                    <a href='#' class='delete-key-request'>Delete</a>
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
                    <a href='#' class='update-key-request'>Edit</a> 
                    <a href='#' class='delete-key-request'>Delete</a>
                </td>
            </tr>
        </AlternatingItemTemplate>
        <EmptyDataTemplate>
            <div class='table-placeholder'>
                You don't have any API Keys.
            </div>
        </EmptyDataTemplate>
    </asp:ListView>
    <br />
    <a href="#" id="RequestKeyLink">Request a key</a>
</asp:Panel>
