<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Contact.aspx.cs" Inherits="Public_Contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="ListTitle">Contact Us</div>

<table cellpadding="0" cellspacing="0">
    <tr>
        <td align="left" style="width: 525px">
            <asp:Panel ID="DefaultPanel" runat="server" Visible="true">
                <table width="100%">
                   
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                   
                                    <td>
                                        First Name<span class="Red">*</span><asp:RequiredFieldValidator Font-Bold="True"
                                            ID="RequiredFieldValidator2" runat="server" ControlToValidate="FirstNameTextBox"
                                            ErrorMessage="'First Name' is required" ForeColor="White" Text="*"></asp:RequiredFieldValidator><br />
                                        <asp:TextBox ID="FirstNameTextBox" runat="server" Width="200px"></asp:TextBox></td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        Last Name<span class="Red">*</span><asp:RequiredFieldValidator Font-Bold="True" ID="RequiredFieldValidator3"
                                            runat="server" ControlToValidate="LastNameTextBox" ErrorMessage="'Last Name' is required"
                                            ForeColor="White" Text="*"></asp:RequiredFieldValidator><br />
                                        <asp:TextBox ID="LastNameTextBox" runat="server" Width="200px"></asp:TextBox></td>
                                    
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            E-Mail Address<span class="Red">*</span><asp:RequiredFieldValidator Font-Bold="True"
                                ID="RequiredFieldValidator4" runat="server" ControlToValidate="EmailTextBox"
                                ErrorMessage="'Email Address' is required" ForeColor="White" Text="*"></asp:RequiredFieldValidator><br />
                            <asp:TextBox ID="EmailTextBox" runat="server" Width="200px"></asp:TextBox></td>
                        <td>
                            Phone Number<span class="Red">*</span><asp:RequiredFieldValidator Font-Bold="True"
                                ID="RequiredFieldValidator5" runat="server" ControlToValidate="PhoneNumberTextBox"
                                ErrorMessage="'Phone Number' is required" ForeColor="White" Text="*"></asp:RequiredFieldValidator><br />
                            <asp:TextBox ID="PhoneNumberTextBox" Width="200px" runat="server"></asp:TextBox></td>
                    </tr>
                   
                   
                 
                    <tr>
                        <td>
                            My question relates to<span class="Red">*</span><asp:RequiredFieldValidator Font-Bold="True"
                                ID="RequiredFieldValidator7" runat="server" ControlToValidate="MyQuestionRelatesToDropDownList"
                                ErrorMessage="'My question relates to' is required" ForeColor="White" Text="*"></asp:RequiredFieldValidator><br />
                            <asp:DropDownList ID="MyQuestionRelatesToDropDownList" runat="server">
                                <asp:ListItem Selected="True"></asp:ListItem>
                                <asp:ListItem>General</asp:ListItem>
                                <asp:ListItem>Web Site</asp:ListItem>
                                <asp:ListItem>Support</asp:ListItem>
                                <asp:ListItem>Other</asp:ListItem>
                            </asp:DropDownList></td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            Questions/Comments<span class="Red">*</span><asp:RequiredFieldValidator Font-Bold="True"
                                ID="RequiredFieldValidator8" runat="server" ControlToValidate="QuestionsTextBox"
                                ErrorMessage="'Questions/Comments' are required" ForeColor="White" Text="*"></asp:RequiredFieldValidator><br />
                            <asp:TextBox ID="QuestionsTextBox" Width="400px" runat="server" TextMode="MultiLine"
                                Rows="5"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="ErrorMessageLabel" runat="server" CssClass="Red"></asp:Label>
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                                ShowSummary="False" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="ConfirmationPanel" Visible="false" runat="server">
                <asp:Label ID="Label2" runat="server" CssClass="AnnouncementTitle" Text="Confirmation"></asp:Label>
                <br />
                <br />
                <b>Your question has been submitted.</b>
                <br />
                <br />
                Details:
                <br />
                <br />
                <asp:Label ID="EmailBodyLabel" runat="server" Text=""></asp:Label>
                <br />
                <br />
                A representative will contact you soon.
                <br />
                <br />
                <asp:Button ID="ContinueButton" runat="server" Text="Continue" onclick="CancelButton_Click" /></asp:Panel>
            <br />
        </td>
    </tr>
    <tr>
        <td colspan="2" align="left">
            <br />
            <asp:Button ID="SubmitButton" runat="server" Text="Submit" onclick="SubmitButton_Click" />&nbsp;
            <asp:Button ID="Button1" runat="server" onclick="CancelButton_Click" Text="Cancel" CausesValidation="false" />
            <br />
            <br />
            <br />
            <br />
            <br />
        </td>
    </tr>
</table>


</asp:Content>

