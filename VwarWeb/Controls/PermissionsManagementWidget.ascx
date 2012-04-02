<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PermissionsManagementWidget.ascx.cs"
    Inherits="Controls_PermissionsManagementWidget" %>
<script type="text/javascript" src="../Scripts/jquery-ui-1.8.7.custom.min.js"></script>
<script type="text/javascript">

     

    function PermissionsWidget(isTemp) {
        //For internal context
        var self = this;

        /* Member variables */

        // Normal Members
        this.validUser = true;
        this.serviceURL = '<%= Page.ResolveClientUrl("~/Users/Permissions.asmx") %>';
        this.modelId = (querySt("pid")) ? querySt("pid") : querySt("ContentObjectID");
        this.removeUserHtml = "&nbsp;<a class='remove-user' href='#'>Remove User</a>";
        this.pollID = null;
        this.polling = false;
        this.isTemp = isTemp;
        this.selectedItems = [];

        // jQuery Members
        this.$AddUserTextbox = $("#AddUserTextbox");
        this.$AddUserSelect = $("#AddUserForm select");
        this.$PermissionsWidget = $("#PermissionsWidget");
        this.$PermissionsDialog = $("<div>")
                                        .dialog({
                                                    modal: true,
                                                    title: "Edit Permissions",
                                                    autoOpen: false,
                                                    width: 740
                                                });

        /* Internal functions 
           (context was easier to set here than inside prototype functions,
            mostly for use with jQuery. If you can find a better way to 
            structure this, it would be very much appreciated) */

        // Checks the table to see if the user already exists
        function isExistingUser() {
            return $("#ManageUsers .group-name")
                        .filter(function () { return $(this).text() == self.$AddUserTextbox.val() })
                        .length > 0;
        }

        // Polls the table and the server to see if the user
        // for which permissions are to be added is valid
        function pollUserValid() {
            if (isExistingUser()) {
                $('#UserValidationStatus').text("User already exists!");
                self.validUser = false;
            } else {
                $.ajax({
                    url: self.serviceURL + "/CheckUser",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({
                        username: self.$AddUserTextbox.val()
                    }),
                    success: function (res, status, jqXHR) {
                        if (!res.d) {
                            $('#UserValidationStatus').text("Not a valid username!");
                            self.validUser = false;
                        } else {
                            $('#UserValidationStatus').text("Valid username!");
                            self.validUser = true;
                        }
                    }
                });
            }
        }

        // Synchronizes added users with the user permission table, post-save
        function syncRows(res, status, jqXHR) {
            $("#UserValidationStatus").text('');
            if (!isExistingUser()
                && self.$AddUserTextbox.val() != self.DEFAULT_ADDUSER_VALUE
                && self.$AddUserTextbox.val().length > 0) {

                var tid = "#ctl00_ContentPlaceHolder1_PermissionsManagementControl_UserPermissionsDataList_PermissionsListView_PermissionsTable",
                $userTable = $(tid);

                if ($userTable.length < 1)
                    $userTable = $("<table>").attr("id", tid).append("<tbody>").appendTo("#ManageUsers");

                var currPos = $userTable.find("tr").length;
                $userTable.children("tbody").append(
                $("<tr>").append(
                    "<td class='group-name'>" + self.$AddUserTextbox.val() + "</td>",
                    $("<td id='userperm_" + currPos + "' class='current-permissions'>").append(
                            self.$AddUserSelect.clone(false).val(self.$AddUserSelect.val()).removeAttr("id").get(0),
                            "<span class='status-bar saved'>&nbsp&nbsp;</span>" + self.removeUserHtml
                        )
                    )
                );

                self.$AddUserTextbox.val('');
            }
        }

        // Starts the username valid poll if not already started
        function startPolling(e) {
            $(this).val('');
            if (!self.polling) {
                self.pollID = setInterval(pollUserValid, 1000);
                setTimeout(function () { clearInterval(self.pollID); }, 30000);
                self.polling = true;
            }
        }

        // Stops the username valid poll if running
        function stopPolling(e) {
            if (self.polling) {
                pollUserValid();
                clearInterval(self.pollID);
                polling = false;
            }
        }

        bindOriginalSelection = function() {
            self.selectedItems[$(this).attr("id")] = $(this).val(); 
        };
        
        function handleSelectionChanged() {
            if (self.selectedItems[$(this).attr("id")] === $(this).val())
                $(this).siblings("span").removeClass("unsaved");
            else {
                $(this).siblings("span")
                    .addClass("unsaved")
                    .removeClass("saved");
            }
        }

        /* Setup */

        //Bind member jquery object events
        this.$PermissionsDialog
            .append(this.$PermissionsWidget.css('display', 'block'))
            .find(".Close").click(function (e) { self.close(); } );

        this.$AddUserTextbox
                .focusin(startPolling)
                .focusout(stopPolling);

        this.DEFAULT_ADDUSER_VALUE = this.$AddUserTextbox.val();

        //Bind jquery events for one-time references
        $("#SaveGroupChanges").click(function(e) { self.saveChanges(e, "#ManageGroups", "group"); });
        $("#SaveUserChanges").click(function(e) { self.saveChanges(e, "#ManageUsers", "user", syncRows); });
        $(".remove-user").live("click", function () { self.removeUser(this); });

        $("button").button();

        $("#PermissionsTabs").tabs({ width: 500 });
        $(".permissions-table select")
            .after("<span class='status-bar'>&nbsp;&nbsp;</span>")
            .each(bindOriginalSelection)
            .live('change', handleSelectionChanged);

        $("#ManageUsers .permissions-table .status-bar")
            .after(this.removeUserHtml);
    }
    

    PermissionsWidget.prototype.close = function () {
        this.$PermissionsDialog.dialog("close");
    }

    PermissionsWidget.prototype.open = function () {
        this.$PermissionsDialog.dialog("open");
    }

    PermissionsWidget.prototype.saveChanges = function (e, dataParent, type, onSuccess) {
        var self = this;

        function resolveUnsaved() {
            $unsaved
                .removeClass("unsaved")
                .addClass("saved")
                .siblings("select")
                    .each(function () {
                        self.selectedItems[$(this).attr("id")] = $(this).val(); 
                     });
        }

        e.preventDefault();
        $container = $(dataParent);

        if (type == "user" &&
                 (!this.validUser && this.$AddUserTextbox.val().length > 0)) {
            return;
        }

        var permissions = [], groups = [];
        var $unsaved = $container.find(".unsaved").each(function () {
            groups.push($(this).parent().siblings(".group-name").text());
            permissions.push($(this).siblings("select").val());
        });


        if (type == "user"
                    && this.$AddUserTextbox.val() != this.DEFAULT_ADDUSER_VALUE
                    && this.$AddUserTextbox.val().length > 0) {
            groups.push(this.$AddUserTextbox.val());
            permissions.push(this.$AddUserSelect.val());
        }

        if (groups.length < 1) return;

        $.ajax({
            url: this.serviceURL + "/SavePermissions",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({
                "type": type,
                pid: this.modelId,
                targets: groups,
                permissions: permissions,
                temp: this.isTemp
            }),
            success: [onSuccess, resolveUnsaved]
        });
    }

    PermissionsWidget.prototype.removeUser = function (sender) {

            function removeFromPermissionsTable(res, status, jqXHR) {
                if (res.d === true)
                    $(sender).parents("tr").remove();
            }

            $.ajax({
                url: this.serviceURL + "/RemoveUserPermission",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    pid: this.modelId,
                    username: $(sender).parent().siblings(".group-name").text(),
                    temp: this.isTemp
                }),
                success: removeFromPermissionsTable
            });
   }

</script>
<style type="text/css">
    .inactive
    {
        color: #AAAAAA;
        cursor: default;
    }
    .permissions-table
    {
        margin: 10px 0;
    }
    .group-name
    {
        min-width: 130px;
        text-align: right;
        padding-right: 5px;
    }
    .current-permissions
    {
        text-align: left;
        min-width: 400px;
    }
    .status-bar
    {
        min-width: 5px;
        min-height: 7px;
        margin-left: 5px;
    }
    .status-bar.unsaved
    {
        background-color: #FFDD00;
    }
    .status-bar.saved
    {
        background-color: #87D69B;
    }
    .button-group
    {
        margin-top: 20px;
    }
    #PermissionsWidget
    {
        display: none;
    }
    #PermissionsTabs 
    {
        border: none;
    }
</style>
<%@ Register Src="~/Controls/PermissionsDataList.ascx" TagName="PermissionsDataList"
    TagPrefix="vwarweb" %>
<div id="PermissionsWidget">
    <div id="PermissionsTabs" title="Manage Permissions">
        <ul class="tabContainer">
            <li class='first'><span class='ui-tabs-separator'></span><a href="#ManageGroups">Groups</a></li>
            <li><a href="#ManageUsers">Users</a><span class='ui-tabs-separator last'></li>
        </ul>
        <div id="ManageGroups">
            <vwarweb:PermissionsDataList ControlType="DefaultGroup" ID="DefaultGroupsPermissionDataList"
                runat="server" />
            <vwarweb:PermissionsDataList ControlType="Group" ID="MyGroupsPermissionsDataList"
                runat="server" />
            <div class="button-group">
                <button id="SaveGroupChanges">
                    Save</button>
                <button class="Close">
                    Close</button>
            </div>
        </div>
        <div id="ManageUsers">
            <vwarweb:PermissionsDataList ControlType="User" ID="UserPermissionsDataList" runat="server" />
            <h3>Add User</h3>
            <div ID="AddUserForm">
                Create a rule so that<br /> 
                <input type="text" id="AddUserTextbox" value="someone@example.com" style='width: 200px;' /> 
                <asp:DropDownList ID="PermissionsDropdownList" runat="server" >
                    <asp:ListItem Text="cannot see model" Value="0" />
                    <asp:ListItem Text="can see model's metadata only" Value="1" />
                    <asp:ListItem Text="can view/download model" Value="2" />
                    <asp:ListItem Text="can edit model" Value="3" />
                    <asp:ListItem Text="can edit/delete model" Value="4" />
                </asp:DropDownList>
                <span id="UserValidationStatus">
                </span>
            </div>

            <div class="button-group">
                <button id="SaveUserChanges">
                    Save</button>
                <button class="Close">
                    Close</button>
            </div>
        </div>
    </div>
</div>
