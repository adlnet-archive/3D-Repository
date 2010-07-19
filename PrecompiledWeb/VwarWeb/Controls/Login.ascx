<%@ control language="C#" autoeventwireup="true" inherits="Controls_Login, App_Web_eepm41e1" %>
<asp:Login ID="Login1" runat="server" FailureText="Invalid credentials. Try again." BorderStyle="None" HelpPageText="Help" onloggingin="Login_LoggingIn" onloggedin="Login1_LoggedIn">
    <LayoutTemplate>
        <div class="ListTitle" style="width: 180px; text-align: center;">
            Member Login</div>
        E-mail:<br />
        <asp:TextBox runat="server" Width="180px" ID="UserName" AccessKey="u" ToolTip="Email" />
        <asp:RequiredFieldValidator runat="server" ID="UserNameRequired" ControlToValidate="UserName" ValidationGroup="Login1" ErrorMessage="'Email' is required." ToolTip="User Name&#9;is required.">*</asp:RequiredFieldValidator>
        <br />
        Password:<br />
        <asp:TextBox runat="server" Width="180px" ID="Password" TextMode="Password" AccessKey="p" ToolTip="Password" />
        <asp:RequiredFieldValidator runat="server" ID="PasswordRequired" ControlToValidate="Password" ValidationGroup="Login1" ErrorMessage="'Password' is required." ToolTip="'Password' is required.">*</asp:RequiredFieldValidator>
        <br />
        <div style="padding-top: 4px;">
            <asp:Button runat="server" ToolTip="Login" Text="Login" ValidationGroup="Login1" ID="LoginButton" CommandName="Login" Style="float: left; margin-right: 8px;" />
            <asp:HyperLink NavigateUrl="~/Public/ForgotPassword.aspx" CssClass="LoginHyperlink" title="Forgot Password?" runat="server" ID="ForgotPasswordHyperLink">Forgot Password?</asp:HyperLink>
            <br />
            <asp:HyperLink NavigateUrl="~/Public/Register.aspx" CssClass="LoginHyperlink" title="Create an Account" runat="server" ID="RegisterHyperLink">Create an Account</asp:HyperLink>
        </div>
        <div class="LoginFailureTextStyle">
            <asp:Image ID="ErrorIconImage" runat="server" Visible="false" ImageUrl="~/Images/Icons/delete2.gif" />
            <asp:Literal runat="server" ID="FailureText" EnableViewState="False"></asp:Literal>
        </div>
    </LayoutTemplate>
</asp:Login>
