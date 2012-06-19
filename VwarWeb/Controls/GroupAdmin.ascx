<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GroupAdmin.ascx.cs" Inherits="Controls_GroupAdmin" %>
<style type="text/css">
    .CenterContent
    {
        width: 900px;
        margin: auto;
        position: relative;
        z-index: 1;
    }
</style>
<div class="BodyContainer">
    <script type="text/javascript">
        $(document).ready(function () {
            var $dialog = $('#addUserDialog').dialog({
                autoOpen: false,
                title: 'Add User'
            });
            var $addGroupDialog = $('#addGroupDialog').dialog({
                autoOpen: false,
                title: 'Create Group'
            });
            var $deleteGroupDialog = $('#deleteGroupDialog').dialog({
                autoOpen: false,
                title: 'Delete Group',
                buttons: { "Cancel": function () {  $('#deleteGroupDialog').dialog('close'); } },
            });
            $('#<%= addUser.ClientID %>').click(function () {
                $dialog.dialog('open');
                // prevent the default action, e.g., following a link
                return false;
            });
            $('#btnCreateGroup').click(function () {
                $addGroupDialog.dialog('open');
                // prevent the default action, e.g., following a link
                return false;
            });
            $dialog.parent().appendTo($("form:first"));
            $addGroupDialog.parent().appendTo($("form:first"));
            $deleteGroupDialog.parent().appendTo($("form:first"));
            document.ShowDeleteGroup = function (groupname) {
                $('#deleteGroupDialog').dialog('open');
                $('#deleteGroupDialog').groupName = groupname;
                $('#DeleteGroupMessage').html('Are you sure you want to delete the "' + groupname + '" group? This action cannot be undone.');
            }
            document.fireDeleteGroup = function(groupname)
            {
                
            }
        });
    </script>
    <div id="addUserDialog" class="ui-dialog ui-widget ui-widget-content ui-corner-all ui-draggable ui-resizable"
        style="display: none">
        <asp:Label runat="server" ID="lblAddUser" Text="Add User"></asp:Label>
        <asp:TextBox runat="server" ID="txtAddUser"></asp:TextBox>
        <asp:Button ID="btnAddUser" Text="Add" runat="server" OnClick="btnAddUser_Click" />
        
    </div>
    <div id="deleteGroupDialog" class="ui-dialog ui-widget ui-widget-content ui-corner-all ui-draggable ui-resizable"
        style="display: none">
        <p><div id="DeleteGroupMessage"></div></p>
        <asp:Button ID="bthDeleteGroup" Text="Add" runat="server" OnClick="bthDeleteGroup_Click" />
    </div>
    <div id="addGroupDialog" class="ui-dialog ui-widget ui-widget-content ui-corner-all ui-draggable ui-resizable"
        style="display: none">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblGroupName" runat="server" Text="Group Name"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtGroupName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblGroupDescription" runat="server" Text="Group Description"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtGroupDescription" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPermissionLevel" runat="server" Text="Permission Level"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlPermissionLevel" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button runat="server" ID="btnSubmit" OnClick="btnSubmit_Click" Text="Submit" />
                </td>
            </tr>
        </table>
    </div>
    <div class="CenterContent">
        <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
    </div>
    <div class="CenterContent">
        <input type="button" value="Create Group" id="btnCreateGroup" />
    </div>
    <div>
        <div>
            <asp:GridView runat="server" ID="CurrentUserGroups" OnSelectedIndexChanged="CurrentUserGroups_SelectedIndexChanged" OnRowCommand="CurrentUserGroups_rowCommand"
                SelectedIndex="1" CssClass="CenterContent">
                <EmptyDataTemplate>
                    <asp:Label Text="No Groups" runat="server"></asp:Label>
                </EmptyDataTemplate>
                <Columns>
                    <asp:CommandField ShowSelectButton="true" ButtonType="Link" SelectText="Select" />
                    <asp:ButtonField ButtonType="Link" Text="Delete" CommandName="DeleteGroup"/>
                </Columns>
            </asp:GridView>
        </div>
        <div class="CenterContent">
            <asp:Button Text="Add User To Group" ID="addUser" runat="server" Enabled="false" />
        </div>
        <div>
            <asp:GridView runat="server" ID="UsersPerGroup" CssClass="CenterContent" OnRowCommand="UsersPerGroup_RowCommand"
                OnRowDeleting="UsersPerGroup_RowDeleting">
                <EmptyDataTemplate>
                    <asp:Label ID="Label1" Text="No Users in this group" runat="server"></asp:Label>
                </EmptyDataTemplate>
                <Columns>
                    <asp:CommandField ShowDeleteButton="true" ButtonType="Link" DeleteText="Remove" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
