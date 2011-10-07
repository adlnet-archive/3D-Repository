<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ModelGroupPermissions.ascx.cs" Inherits="Controls_ModelGroupPermissions" %>
<div class="BodyContainer">
    <div id="addGroupDialog" class="ui-dialog ui-widget ui-widget-content ui-corner-all ui-draggable ui-resizable"
        style="display: none">
        <asp:Label runat="server" ID="lblAddGroup" Text="Add Group"></asp:Label>
        <asp:TextBox runat="server" ID="txtGroupName"></asp:TextBox>
        <asp:Label runat="server" ID="Label1" Text="Permission"></asp:Label>
        <asp:DropDownList runat="server" ID="ddlPermission"></asp:DropDownList>
        <asp:Button ID="btnAddUser" Text="Add" runat="server" OnClick="btnAddUser_Click" />
    </div>   
        <div style='text-align:left'>
            <asp:Label runat="server" Font-Bold="true">Group Permissions</asp:Label>
            <asp:GridView runat="server" ID="usersWithPermissionToModel" AutoGenerateColumns="false" OnRowDeleting="usersWithPermissionToModel_RowDeleting">
                <EmptyDataTemplate>
                    <asp:Label Text="No groups have permission to this object." runat="server" ></asp:Label>
                </EmptyDataTemplate>
                <Columns>
                    <asp:BoundField DataField="Name" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="lblPermission" runat="server" Text='<%# Eval("PermissionLevel").ToString() %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Wrap="False" />
                    </asp:TemplateField>
                    <asp:CommandField ShowDeleteButton="true" DeleteText="Remove Permission"/>
                </Columns>
            </asp:GridView>
            <input type="button" value="Add Group" id="addGroup" />
        </div>
        
</div>
<style type="text/css">
    .CenterContent
    {
        width: 900px;
        margin: auto;
        position: relative;
        z-index: 1;
    }
</style>
<script type="text/javascript">
        var load = function () {
            var $dialog = $('#addGroupDialog').dialog({
                autoOpen: false,
                title: 'Add Group'
            });
            $('#addGroup').click(function () {
                $dialog.dialog('open');
                // prevent the default action, e.g., following a link
                return false;
            });
            $dialog.parent().appendTo($("form:first"));
        };
        load();
</script>