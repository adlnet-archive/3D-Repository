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



<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Login.ascx.cs" Inherits="Controls_Login" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="OrbitOne.OpenId.Controls" Namespace="OrbitOne.OpenId.Controls"
    TagPrefix="cc1" %>
<style type="text/css">
    .style2
    {
        width: 190px;
    }
    .style3
    {
        width: 184px;
    }
    .style4
    {
        width: 259px;
    }
    
</style>

<asp:Login ID="Login1" runat="server" 
    FailureText="Invalid credentials. Try again." BorderStyle="None" 
    HelpPageText="Help" onloggingin="Login_LoggingIn" onloggedin="Login1_LoggedIn" 
    style="margin-bottom: 2px" Width="606px" >
    <LayoutTemplate>
        <div class="ListTitle" style="width: 400px; text-align: left; margin-bottom: 5px;">
            OpenID Login</div>
        <cc1:OpenIdLogin runat="server" CssClass="OpenIdLogin" ID="openIdLogin" DestinationPageUrl="~/default.aspx" MembershipProvider="OpenIDMembershipProvider"></cc1:OpenIdLogin>
        <br />
        <div class="ListTitle" style="width: 400px; text-align: center;">
            Member Login</div>
        <div class="LoginFailureTextStyle" style="margin-left: 30px; margin-right: auto; width: 300px">
            <asp:Image ID="ErrorIconImage" runat="server" Visible="false" ImageUrl="~/styles/images/Icons/delete2.gif" />
            <asp:Literal runat="server" ID="FailureText" EnableViewState="False"></asp:Literal>
        </div>
        <table id="LoginTable" class="NormalLogin">
        </tr>
        
            <td align="right">
                E-mail:</td>
            <td class="style2">
                <asp:TextBox ID="UserName" runat="server" AccessKey="u" ToolTip="Email"  
                    Width="205px" />
            </td>
            <td class="style4">
                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                    ControlToValidate="UserName" ErrorMessage="'Email' is required." 
                    SetFocusOnError="true" ToolTip="User Name	is required." 
                    ValidationGroup="Login1"></asp:RequiredFieldValidator>
                <%--  <ajax:ValidatorCalloutExtender HighlightCssClass=" ID="ValidatorCalloutExtender2" runat="server" HighlightCssClass="ValidatorCallOutStyle" Width="150px" TargetControlID="UserNameRequired" />--%>
            </td>
        
        <tr>
        <td align="right">
        Password:
        </td>
        <td class="style2">
        <asp:TextBox runat="server" Width="205px" ID="Password" TextMode="Password" CssClass="NormalLogin"
                AccessKey="p" ToolTip="Password" />
        </td>
        <td class="style4">
        <asp:RequiredFieldValidator runat="server" ID="PasswordRequired" ControlToValidate="Password" ValidationGroup="Login1" ErrorMessage="'Password' is required." ToolTip="'Password' is required." SetFocusOnError="true" />
               <%--<ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" HighlightCssClass="ValidatorCallOutStyle" Width="150px" TargetControlID="PasswordRequired" />--%>  
        </td>
        </tr>
        <tr>
            <%-- <caption>--%>
                <%--<caption> --%>
                    <caption>
                        <br />
                        <tr>
                            <td />
                            <td class="style2">
                                <asp:Button ID="LoginButton" runat="server" CommandName="Login" Style="float: left;
                                    margin-right: 8px; margin-bottom: 1px;" Text="Login" ToolTip="Login" ValidationGroup="Login1"  />
                                <span style="float: right; width: 120px;">
                                    <asp:HyperLink ID="ForgotPasswordHyperLink" runat="server" CssClass="LoginHyperlink"
                                        NavigateUrl="~/Public/ForgotPassword.aspx" title="Forgot Password?">Forgot Password?</asp:HyperLink>
                                    <asp:HyperLink ID="RegisterHyperLink" runat="server" CssClass="LoginHyperlink" NavigateUrl="~/Public/Register.aspx"
                                        title="Create an Account">Create an Account</asp:HyperLink>
                                </span>
                            </td>
                        </tr>
                        <%--  </caption> --%>
            </caption>
            </tr>
        </table>
    </LayoutTemplate>
</asp:Login>
