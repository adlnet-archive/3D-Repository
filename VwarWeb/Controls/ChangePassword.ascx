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



<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ChangePassword.ascx.cs" Inherits="Controls_ChangePassword" %>
                

                <asp:Panel ID="errorLink" style="text-align: center;font-size:12px;margin-bottom:15px;color:Red;" runat="server">

                    <asp:Literal ID="corruptedText" runat="server">Sorry, either your link has expired or has been corrupted. 
                    Please try again or contact us for help.</asp:Literal>
                </asp:Panel>

                <asp:Panel ID="initialEmail" runat="server">
                    <div class="ListTitle">Change Your Password</div>

                    <div style="margin: 20px auto; text-align: center;">
                        User name: <asp:TextBox ID="UserName" runat="server" /><br /><br />

                        <div class="Red" style="margin-left: 20px; margin-bottom: 10px; top: 10px;"><asp:Literal ID="EmailFailure" runat="server" EnableViewState="False"></asp:Literal></div>
                        <asp:Button ID="SubmitButton" runat="server" CommandName="Submit" style="margin-left: 77px" Text="Submit" ValidationGroup="PasswordRecovery1" OnClick="SubmitButton_Click" />
                    </div>

                    <p style="font-size:12px;">You will be emailed a link with instructions to the corresponding email address on file for your username.
                    If you are having difficulty with this form, please follow the "Contact Us" link above.</p>
                </asp:Panel>

                <asp:Panel ID="changeForm" runat="server">
                <table border="0" cellpadding="1" cellspacing="0" 
                    style="border-collapse:collapse;">
                    <tr>
                        <td>
                            <table border="0" cellpadding="0" style="width: 400px;">
                                <tr runat="server" id="TitleTableRow" visible="true">
                                    <td align="center" colspan="2">
                                        <div class="ListTitle">Change Your Password</div>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <br /><br />
                                        <asp:Label ID="NewPasswordLabel" runat="server" 
                                            AssociatedControlID="NewPassword">New Password:</asp:Label>
                                    </td>
                                    <td>
                                        <br /><br />
                                        <asp:TextBox ID="NewPassword" runat="server" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" 
                                            ControlToValidate="NewPassword" ErrorMessage="New Password is required." 
                                            ToolTip="New Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="ConfirmNewPasswordLabel" runat="server" 
                                            AssociatedControlID="ConfirmNewPassword">Confirm New Password:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" 
                                            ControlToValidate="ConfirmNewPassword" 
                                            ErrorMessage="Confirm New Password is required." 
                                            ToolTip="Confirm New Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:CompareValidator ID="NewPasswordCompare" runat="server" 
                                            ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword" 
                                            Display="Dynamic" 
                                            ErrorMessage="The Confirm New Password must match the New Password entry." 
                                            ValidationGroup="ChangePassword1"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2" style="color:Red;">
                                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <br />
                                        <asp:Button ID="ChangePasswordPushButton" runat="server" 
                                            CommandName="ChangePassword" Text="Change Password" 
                                            ValidationGroup="ChangePassword1" OnClick="ChangePasswordPushButton_Click" />
                                    </td>
                                    <td>
                                        <br />
                                        <asp:Button ID="CancelPushButton" runat="server" CausesValidation="False" 
                                            CommandName="Cancel" Text="Cancel" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                </asp:Panel>
