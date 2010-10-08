<%@ control language="C#" autoeventwireup="true" inherits="Controls_ManageAdministrativeUsers, App_Web_5p0leyam" %>


<div class="ListTitle">
    Manage Administrators</div>
<br />
<table border="0" cellpadding="4" cellspacing="0">
    <tr>
        
        <td valign="top">
            <asp:GridView ID="AdminGridView" runat="server" AutoGenerateColumns="False" Width="314px" SkinID="GridView" onrowdatabound="AdminGridView_RowDataBound" onrowcommand="AdminGridView_RowCommand" >
                <Columns>
                    <asp:TemplateField HeaderText="Email">
                        <ItemTemplate>
                            <asp:HyperLink ID="EmailHyperLink" runat="server" Text='<%# Eval("Username") %>' NavigateUrl='<%# Website.Pages.Types.FormatEmail(Eval("Username")) %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:Button ID="DeleteButton" runat="server" CausesValidation="false" CommandName="DeleteUser" CommandArgument='<%# Eval("Username") %>' Text="Delete" OnClientClick='<%# Eval("Username", "return confirm(\"Are you sure you want to delete user {0}? This will delete all user information. Click OK to continue.\");") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    There were no administrators found.
                </EmptyDataTemplate>
            </asp:GridView>
        </td>
    </tr>
    <tr>
        <td valign="top">
            <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" oncreateduser="CreateUserWizard1_CreatedUser" oncreateusererror="CreateUserWizard1_CreateUserError" oncreatinguser="CreateUserWizard1_CreatingUser">
                <WizardSteps>
                    <asp:CreateUserWizardStep runat="server" ID="CreateUserWizardStep1">
                        <ContentTemplate>
                            <table border="0">
                                <tr>
                                    <td align="center" colspan="2" class="GridViewHeaderStyle">
                                        Create Site Administrators
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">E-mail:</asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="Email" runat="server"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ToolTip="A valid E-mail is required." ControlToValidate="Email" ValidationGroup="CreateUserWizard1" ErrorMessage="Invalid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email" ErrorMessage="E-mail is required." ToolTip="E-mail is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr runat="server" id="UserNameTableRow" visible="false">
                                    <td align="right">
                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">Confirm Password:</asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPassword" ErrorMessage="Confirm Password is required." ToolTip="Confirm Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword" Display="Dynamic" ErrorMessage="The Password and Confirmation Password must match." ValidationGroup="CreateUserWizard1"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2" style="color: red">
                                        <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:CreateUserWizardStep>
                    <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
                    </asp:CompleteWizardStep>
                </WizardSteps>
            </asp:CreateUserWizard>
            <asp:Label ID="ErrorLabel" runat="server" ForeColor="Red" Visible="False"></asp:Label>
        </td>
    </tr>
</table>
