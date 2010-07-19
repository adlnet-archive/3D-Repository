<%@ control language="C#" autoeventwireup="true" inherits="Administrators_ManageUsers, App_Web_eepm41e1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<div class="ListTitle">
    Approve Users</div>
<br />
<asp:GridView ID="NotApprovedUsersGridView" SkinID="GridView" runat="server" AllowPaging="True" onrowcommand="NotApprovedUsersGridView_RowCommand">
    <Columns>
        <asp:TemplateField HeaderText="Name">
            <ItemTemplate>
                <asp:Label ID="NameLabel" runat="server" Text='<%# FormatName(Eval("Comment")) %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Username">
            <ItemTemplate>
                <asp:Label ID="UsernameLabel" runat="server" Text='<%# Eval("Username") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Email">
            <ItemTemplate>
                <asp:HyperLink ID="EmailHyperLink" runat="server" Text='<%# Eval("Email") %>' NavigateUrl='<%# Website.Pages.Types.FormatEmail(Eval("Email")) %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Date Created">
            <ItemTemplate>
                <asp:Label ID="DateCreatedLabel" runat="server" Text='<%# String.Format("{0:d}" , Eval("CreationDate")) %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
                <asp:Button ID="ApproveButton" runat="server" CausesValidation="false" CommandName="ApproveUser" CommandArgument='<%# Eval("Username") %>' Text="Approve" />
            </ItemTemplate>
        </asp:TemplateField>
        
       
         <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
                <asp:Button ID="Delete" runat="server" CausesValidation="false" CommandName="DeleteUser"  CommandArgument='<%# Eval("Username") %>' Text="Delete"  OnClientClick='<%# Eval("Username", "return confirm(\"Are you sure you want to delete user {0}? This will delete all user and profile information. Click OK to continue.\");") %>'  />
            </ItemTemplate>
        </asp:TemplateField>

    </Columns>
    <EmptyDataTemplate>
        There are no users that need approval.
    </EmptyDataTemplate>
</asp:GridView>
<br />
<br />
<div class="ListTitle">
    Locked Out Users</div>
<br />
<asp:GridView ID="LockedOutUsersGridView" SkinID="GridView" runat="server" AllowPaging="True">
    <Columns>
        <asp:TemplateField HeaderText="Name">
            <ItemTemplate>
                <asp:Label ID="NameLabel" runat="server" Text='<%# FormatName(Eval("Comment")) %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Username">
            <ItemTemplate>
                <asp:Label ID="UsernameLabel" runat="server" Text='<%# Eval("Username") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Email">
            <ItemTemplate>
                <asp:HyperLink ID="EmailHyperLink" runat="server" Text='<%# Eval("Email") %>' NavigateUrl='<%# Website.Pages.Types.FormatEmail(Eval("Email")) %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Date Created">
            <ItemTemplate>
                <asp:Label ID="DateCreatedLabel" runat="server" Text='<%# String.Format("{0:d}" , Eval("CreationDate")) %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
                <asp:Button ID="UnlockButton" runat="server" CausesValidation="false" CommandName="UnlockUser" CommandArgument='<%# Eval("Username") %>' Text="Unlock"  />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        There are no locked out users.
    </EmptyDataTemplate>
</asp:GridView>
