<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Profile.ascx.cs" Inherits="Controls_Profile" %>
<%@ Register Src="ChangePassword.ascx" TagName="ChangePassword" TagPrefix="uc1" %>
<div class="ListTitle">My Account</div>
<asp:MultiView ID="MultiView1" runat="server">
    <asp:View runat="server" ID="DefaultView">
        <br />
        <asp:LinkButton ID="ChangePasswordLinkButton" runat="server" CssClass="Hyperlink"
            ToolTip="Change Password" OnClick="ChangePasswordLinkButton_Click">Change Password</asp:LinkButton>
        <br />
        <br />
        <asp:LinkButton ID="EditProfileLinkButton" runat="server" CssClass="Hyperlink" ToolTip="Edit Profile"
            OnClick="EditProfileLinkButton_Click">Edit Profile</asp:LinkButton>
        <br />
        <br />
        <asp:Button ID="CancelButton" runat="server" Text="Cancel" ToolTip="Cancel" CausesValidation="false"
            OnClick="CancelButton_Click" />
    </asp:View>
    <asp:View runat="server" ID="EditProfileView">
        <br />
        <br />
        <div>
            <asp:Label ID="FirstNameLabel" runat="server" AssociatedControlID="FirstNameTextBox"
                CssClass="FormLabel">
                First Name<span class="Red">*</span>:
            </asp:Label>
            <asp:TextBox ID="FirstNameTextBox" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*Required"
                ControlToValidate="FirstNameTextBox"></asp:RequiredFieldValidator>
        </div>
        <div>
            <asp:Label ID="LastNameLabel" runat="server" AssociatedControlID="LastNameTextBox"
                CssClass="FormLabel">
                Last Name<span class="Red">*</span>:
            </asp:Label>
            <asp:TextBox ID="LastNameTextBox" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*Required"
                ControlToValidate="LastNameTextBox"></asp:RequiredFieldValidator>
        </div>
        <br />
        <asp:Button ID="SubmitButton" runat="server" CssClass="FormLabelButton" 
            Text="Submit" ToolTip="Submit" onclick="SubmitButton_Click" />
        <asp:Button ID="CancelButton1" runat="server" Text="Cancel" ToolTip="Cancel" OnClick="CancelButton_Click" CausesValidation="false" />
        

    </asp:View>
    <asp:View runat="server" ID="ConfirmationView">
        <asp:Label ID="ConfirmationLabel" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Button ID="ConfirmationButton" runat="server" Text="Continue" 
            ToolTip="Continue" onclick="ConfirmationButton_Click" />
    </asp:View>
    <asp:View runat="server" ID="ErrorView">
        <asp:Label ID="ErrorLabel" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Button ID="ContinueButton" runat="server" Text="Continue" 
            ToolTip="Continue" onclick="ContinueButton_Click" />
    </asp:View>
</asp:MultiView>