<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PermissionsDataList.ascx.cs"
    Inherits="Controls_PermissionsDataList" %>

<asp:ListView ID="PermissionsListView" runat="server" OnItemDataBound="BindSelectedPermission" >
    <LayoutTemplate>
        <h3><asp:Label ID="PermissionsTableTitle" runat='server'></asp:Label></h3>
        <table id="PermissionsTable" class="permissions-table" runat="server" width="300" 
            cellspacing="0" cellpadding="0">
            <tr id="ItemPlaceholder" runat="server" />
        </table>
    </LayoutTemplate>
    <ItemTemplate>
        <tr class="blue" runat="server" id="DataRow" >
            <td class="group-name"><%# (string)Eval("Key") %></td>
            <td class="current-permissions" id="DropdownColumn" >
                <asp:DropDownList ID="PermissionsDropdownList" runat="server" >
                    <asp:ListItem Text="cannot see model" Value="0" />
                    <asp:ListItem Text="can see model's metadata only" Value="1" />
                    <asp:ListItem Text="can view/download model" Value="2" />
                    <asp:ListItem Text="can edit model" Value="3" />
                    <asp:ListItem Text="can edit/delete model" Value="4" />
                </asp:DropDownList>
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>            
        No permissions currently set.
    </EmptyDataTemplate>
</asp:ListView>
