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



<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Profile.ascx.cs" Inherits="Controls_Profile" %>
<%@ Register Src="ChangePassword.ascx" TagName="ChangePassword" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="uc1" TagName="GroupAdmin" Src="~/Controls/GroupAdmin.ascx" %>
<div class="ListTitle">
    My Account</div>
<asp:MultiView ID="MultiView1" runat="server">
    <asp:View runat="server" ID="DefaultView">
        <br />
        <asp:LinkButton ID="ChangePasswordLinkButton" runat="server" CssClass="Hyperlink" ToolTip="Change Password" OnClick="ChangePasswordLinkButton_Click">Change Password</asp:LinkButton>
        <br />
        <br />

        <asp:Panel runat="server" ID="EditProfilePanel">
            <asp:LinkButton ID="EditProfileLinkButton" runat="server" CssClass="Hyperlink" ToolTip="Edit Profile"
                OnClick="EditProfileLinkButton_Click">Edit Profile</asp:LinkButton>
            <br />
            <br />
        </asp:Panel>
        <asp:LinkButton runat="server" ID="EditGroupsButton" CssClass="Hyperlink" ToolTip="Edit Groups" OnClick="EditGroupsButton_Click">
            Edit Groups
        </asp:LinkButton>
        <br />
        <br />
        <asp:Button ID="CancelButton" runat="server" Text="Cancel" ToolTip="Cancel" CausesValidation="false"
            OnClick="CancelButton_Click" />
    </asp:View>
    <asp:View runat="server" ID="EditGroups">
        <uc1:GroupAdmin runat="server" ID="GroupAdmin" />
    </asp:View>
    <asp:View runat="server" ID="EditProfileView">
        <table cellpadding="4" cellspacing="0" border="0">
            <tr>
                <td align="right" valign="top">
                    &nbsp;
                </td>
                <td align="left" valign="top">
                    <asp:Label ID="EditProfileViewErrorMessageLabel" runat="server" CssClass="LoginFailureTextStyle"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top">
                    <asp:Label ID="FirstNameLabel" CssClass="Bold" AssociatedControlID="FirstNameTextBox" runat="server" ToolTip="First Name">First Name<span class="Red">*</span>:</asp:Label>
                    

                </td>
                <td align="left" valign="top">
                    <asp:TextBox ID="FirstNameTextBox" runat="server" Width="200px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="FirstNameRequired" runat="server" ControlToValidate="FirstNameTextBox" ErrorMessage="First Name is required." ToolTip="First Name is required." SetFocusOnError="true" Display="None" />
                    <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" HighlightCssClass="ValidatorCallOutStyle" Width="150px" TargetControlID="FirstNameRequired" />  
                </td>
            </tr>
            <tr>
                <td align="right" valign="top">
                    <asp:Label ID="LastNameLabel" CssClass="Bold" AssociatedControlID="LastNameTextBox" runat="server" ToolTip="Last Name"> Last Name<span class="Red">*</span>:</asp:Label>
                </td>
                <td align="left" valign="top">
                    <asp:TextBox ID="LastNameTextBox" runat="server" Width="200px"></asp:TextBox>
                   <asp:RequiredFieldValidator ID="LastNameRequired" runat="server" ControlToValidate="LastNameTextBox" ErrorMessage="Last Name is required." ToolTip="Last Name is required." SetFocusOnError="true" Display="None"  />
                     <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" HighlightCssClass="ValidatorCallOutStyle" Width="150px" TargetControlID="LastNameRequired" />  
                </td>
                
            </tr>
            <tr>
                <td align="right" valign="top">
                    <asp:Label ID="EmailLabel" CssClass="Bold" AssociatedControlID="EmailTextBox" runat="server" ToolTip="Email"> E-mail<span class="Red">*</span>:</asp:Label>
                </td>
                <td align="left" valign="top">
                    <asp:TextBox ID="EmailTextBox" runat="server" Width="200px"></asp:TextBox>
                    <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" HighlightCssClass="ValidatorCallOutStyle" Width="150px" TargetControlID="RegularExpressionValidator1" />                       
                    <asp:RegularExpressionValidator SetFocusOnError="true" ID="RegularExpressionValidator1"
                        runat="server" ControlToValidate="EmailTextBox" ValidationExpression="([a-zA-Z0-9_\-])([a-zA-Z0-9_\-\.]*)@(\[((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\.){3}|((([a-zA-Z0-9\-]+)\.)+))([a-zA-Z]{2,}|(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\])"
                        Display="None" ErrorMessage="Invalid Email Address" />
                 <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="EmailTextBox" ErrorMessage="E-mail is required." ToolTip="E-mail is required." SetFocusOnError="true" Display="None" />
                  <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" HighlightCssClass="ValidatorCallOutStyle" Width="150px" TargetControlID="EmailRequired" />  
                </td>
            </tr>
            <tr>
                <td align="right" valign="top">
                    <asp:Label ID="WebsiteURLLabel" CssClass="Bold" AssociatedControlID="WebsiteURLTextBox" runat="server" ToolTip="Website">Website:</asp:Label>
                </td>
                <td align="left" valign="top">
                    <asp:TextBox ID="WebsiteURLTextBox" runat="server" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top">
                    <asp:Label ID="SponsorNameLabel" CssClass="Bold" AssociatedControlID="SponsorNameTextBox" runat="server" ToolTip="Sponsor Name">Sponsor Name:</asp:Label>
                </td>
                <td align="left" valign="top">
                    <asp:TextBox runat="server" ID="SponsorNameTextBox" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top">
                    <asp:Label ID="SponsorLogoLabel" CssClass="Bold" AssociatedControlID="SponsorLogoFileUpload" runat="server" ToolTip="Sponsor Logo">Sponsor Logo:</asp:Label>
                </td>
                <td align="left" valign="top">
                    <asp:FileUpload ID="SponsorLogoFileUpload" runat="server" Width="200px" /><asp:CheckBox ID="RemoveSponsorLogoCheckBox" runat="server" Text="Remove Logo" Visible="False" /><br />     
                              
                    <asp:Image ID="SponsorImageThumbnail" runat="server" Visible="false" />
                </td>
            </tr>
            <tr>
                <td align="right" valign="top">
                    <asp:Label ID="DeveloperNameLabel" AssociatedControlID="DeveloperNameTextBox" CssClass="Bold" runat="server" ToolTip="Developer Name"> Developer Name:</asp:Label>
                </td>
                <td align="left" valign="top">
                    <asp:TextBox ID="DeveloperNameTextBox" runat="server" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top">
                    <asp:Label ID="Label3" CssClass="Bold" AssociatedControlID="DeveloperLogoFileUpload" runat="server" ToolTip="Developer Logo"> Developer Logo:</asp:Label>
                </td>
                <td align="left" valign="top">
                    <asp:FileUpload ID="DeveloperLogoFileUpload" runat="server" Width="200px" /><asp:CheckBox ID="RemoveDeveloperLogoCheckBox" runat="server" Text="Remove Logo" Visible="False" /><br />     
                    <asp:Image ID="DeveloperLogoImageThumbnail" runat="server" Visible="false" />
                </td>
            </tr>
            <tr>
                <td align="right" valign="top">
                    <asp:Label ID="ArtistNameLabel" CssClass="Bold" AssociatedControlID="ArtistNameTextBox" runat="server" ToolTip="Artist Name">Artist Name:</asp:Label>
                </td>
                <td align="left" valign="top">
                    <asp:TextBox runat="server" ID="ArtistNameTextBox" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top">
                    <asp:Label ID="PhoneLabel" CssClass="Bold" AssociatedControlID="PhoneTextBox" runat="server" ToolTip="Phone">Phone:</asp:Label>
                </td>
                <td align="left" valign="top">
                    <asp:TextBox runat="server" ID="PhoneTextBox" Width="200px"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="PhoneTextBox" FilterType="Custom" ValidChars="0123456789-() " />

                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td align="left" valign="top">
                    <asp:Button ID="SubmitButton" runat="server"  Text="Submit" ToolTip="Submit" OnClick="SubmitButton_Click" />
                    <asp:Button ID="CancelButton1" runat="server" Text="Cancel" ToolTip="Cancel" OnClick="CancelButton_Click" CausesValidation="false" />
                </td>
            </tr>
        </table>
    </asp:View>
    <asp:View runat="server" ID="ConfirmationView">
        <asp:Label ID="ConfirmationLabel" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Button ID="ConfirmationButton" runat="server" Text="Continue" ToolTip="Continue" OnClick="ConfirmationButton_Click" />
    </asp:View>
    <asp:View runat="server" ID="ErrorView">
        <asp:Label ID="ErrorLabel" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Button ID="ContinueButton" runat="server" Text="Continue" ToolTip="Continue" OnClick="ContinueButton_Click" />
    </asp:View>
</asp:MultiView>
