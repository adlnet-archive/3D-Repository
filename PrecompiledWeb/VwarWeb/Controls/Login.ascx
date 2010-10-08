<%@ control language="C#" autoeventwireup="true" inherits="Controls_Login, App_Web_5p0leyam" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Login ID="Login1" runat="server" FailureText="Invalid credentials. Try again." BorderStyle="None" HelpPageText="Help" onloggingin="Login_LoggingIn" onloggedin="Login1_LoggedIn">
    <LayoutTemplate>
        <div class="ListTitle" style="width: 400px; text-align: center;">
            Member Login</div>
        E-mail:<br />
        <asp:TextBox runat="server" Width="400px" ID="UserName" AccessKey="u" ToolTip="Email" />
        <asp:RequiredFieldValidator runat="server" ID="UserNameRequired" ControlToValidate="UserName" ValidationGroup="Login1" ErrorMessage="'Email' is required." SetFocusOnError="true" Display="None"  ToolTip="User Name&#9;is required.">*</asp:RequiredFieldValidator>
        <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" HighlightCssClass="ValidatorCallOutStyle" Width="150px" TargetControlID="UserNameRequired" />  
       
        <br />
        Password:<br />
        <asp:TextBox runat="server" Width="400px" ID="Password" TextMode="Password" AccessKey="p" ToolTip="Password" />
        <asp:RequiredFieldValidator runat="server" ID="PasswordRequired" ControlToValidate="Password" ValidationGroup="Login1" ErrorMessage="'Password' is required." ToolTip="'Password' is required." SetFocusOnError="true" Display="None" />
               <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" HighlightCssClass="ValidatorCallOutStyle" Width="150px" TargetControlID="PasswordRequired" />  

       
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
