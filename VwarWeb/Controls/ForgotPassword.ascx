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



<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ForgotPassword.ascx.cs" Inherits="Controls_ForgotPassword" %>
<div class="ListTitle">Forgot Your Password?</div>
<br />Enter your <b>registered e-mail address</b> below
<br />
to have your password sent to the account.
<br />
<br />
<div style="text-align: center;">
    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" style="margin-left: 20px;">E-mail:</asp:Label>
    <asp:TextBox ID="UserName" runat="server" Width="200px"></asp:TextBox>
    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
        ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="PasswordRecovery1">*Required</asp:RequiredFieldValidator>
</div>

<div class="Red" style="margin-left: 20px; margin-bottom: 10px; top: 10px;"><asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal></div>
<asp:Button ID="SubmitButton" runat="server" CommandName="Submit" style="margin-left: 77px" Text="Submit" ValidationGroup="PasswordRecovery1" OnClick="SubmitButton_Click" />
<asp:Button ID="CancelButton" runat="server" Text="Return to Login" CausesValidation="false" onclick="CancelButton_Click" />
<br />
<asp:Label ID="Label1" runat="server"></asp:Label>
