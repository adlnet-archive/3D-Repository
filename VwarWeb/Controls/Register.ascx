<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Register.ascx.cs" Inherits="Controls_Register" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:MultiView ID="MultiView1" runat="server">
    <asp:View runat="server" ID="DefaultView">
        Enter this code in the box below:
        <br />
        <asp:Image ID="CaptchaImage" runat="server" ImageUrl="~/Images/Captcha.aspx" ToolTip="Captcha Image" />
        <br />
        <asp:Panel ID="CaptchaPanel" runat="server" DefaultButton="CaptchaSubmitButton">
            <asp:TextBox ID="CaptchaCodeTextBox" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                ControlToValidate="CaptchaCodeTextBox" ValidationGroup="CaptchaControls"></asp:RequiredFieldValidator>
            <asp:Button ID="CaptchaSubmitButton" runat="server" ValidationGroup="CaptchaControls"
                Text="Submit" OnClick="CaptchaSubmitButton_Click" />
        </asp:Panel>
        <br />
        <br />
        Can't read this one?
        <asp:HyperLink ID="ReloadHyperLink" CssClass="Hyperlink" runat="server" ToolTip="Get a new code">Get a new code</asp:HyperLink>
    </asp:View>
    <asp:View runat="server" ID="RegisterView">
        <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" FinishDestinationPageUrl="~/Default.aspx"
            OnCreatingUser="CreateUserWizardStep1_CreatingUser" NavigationStyle-HorizontalAlign="Center"
            OnCreatedUser="CreateUserWizardStep1_CreatedUser" OnCreateUserError="CreateUserWizardStep1_CreateUserError">
            <WizardSteps>
                <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
                    <ContentTemplate>
                        <table border="0">
                            <tr>
                                <td colspan="2">
                                    <div class="ListTitle" style="width: 390px; text-align: center;">
                                        Create an Account</div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:LinkButton runat="server" ID="OpenIdSwitch" Text="Create Account With OpenID" OnClick="SwitchToOpenId" CssClass="Hyperlink"></asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="FirstNameLabel" runat="server" AssociatedControlID="Email">
                                        First Name<span class="Red">*</span>:
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="FirstName" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="FirstNameRequired" runat="server" ControlToValidate="FirstName"
                                        ErrorMessage="First Name is required." ToolTip="First Name is required." ValidationGroup="CreateUserWizard1"
                                        SetFocusOnError="true" Display="None" />
                                    <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" HighlightCssClass="ValidatorCallOutStyle"
                                        Width="150px" TargetControlID="FirstNameRequired" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="LastNameLabel" runat="server" AssociatedControlID="LastName">
                                        Last Name<span class="Red">*</span>:
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="LastName" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="LastNameRequired" runat="server" ControlToValidate="LastName"
                                        ErrorMessage="Last Name is required." ToolTip="Last Name is required." ValidationGroup="CreateUserWizard1"
                                        SetFocusOnError="true" Display="None" />
                                    <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" HighlightCssClass="ValidatorCallOutStyle"
                                        Width="150px" TargetControlID="LastNameRequired" />
                                </td>
                            </tr>
                            <tr runat="server" id="UserNameTableRow" visible="false">
                                <td align="right">
                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">
                                        User Name<span class="Red">*</span>:
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                        ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="CreateUserWizard1"
                                        SetFocusOnError="true" Display="None" />
                                    <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" HighlightCssClass="ValidatorCallOutStyle"
                                        Width="150px" TargetControlID="UserNameRequired" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">
                                        E-mail<span class="Red">*</span>:
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="Email" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email"
                                        ErrorMessage="E-mail is required." ToolTip="E-mail is required." ValidationGroup="CreateUserWizard1"
                                        SetFocusOnError="true" Display="None" />
                                    <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" HighlightCssClass="ValidatorCallOutStyle"
                                        Width="150px" TargetControlID="EmailRequired" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">
                                        Password<span class="Red">*</span>:
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="Password" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                        ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="CreateUserWizard1"
                                        SetFocusOnError="true" Display="None" />
                                    <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" runat="server" HighlightCssClass="ValidatorCallOutStyle"
                                        Width="150px" TargetControlID="PasswordRequired" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password"
                                        ControlToValidate="ConfirmPassword" ErrorMessage="The Password and Confirmation Password must match."
                                        ValidationGroup="CreateUserWizard1" SetFocusOnError="true" Display="None" />
                                    <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender6" runat="server" HighlightCssClass="ValidatorCallOutStyle"
                                        Width="150px" TargetControlID="PasswordCompare" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">
                                        Confirm Password<span class="Red">*</span>:
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPassword"
                                        ErrorMessage="Confirm Password is required." ToolTip="Confirm Password is required."
                                        ValidationGroup="CreateUserWizard1" SetFocusOnError="true" Display="None" />
                                    <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender7" runat="server" HighlightCssClass="ValidatorCallOutStyle"
                                        Width="150px" TargetControlID="ConfirmPasswordRequired" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="color: Red;">
                                    <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:CreateUserWizardStep>
                <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
                </asp:CompleteWizardStep>
            </WizardSteps>
            <StartNavigationTemplate>
                <asp:Button ID="StartNextButton" runat="server" CommandName="MoveNext" Text="Next" />
            </StartNavigationTemplate>
            <NavigationStyle HorizontalAlign="Center" />
        </asp:CreateUserWizard>
    </asp:View>
    <asp:View runat="server" ID="OpenIdCreationView">
        <asp:CreateUserWizard ID="CreateOpenIDWizard" RequireEmail="False" AutoGeneratePassword="True"
            runat="server" FinishDestinationPageUrl="~/Default.aspx" NavigationStyle-HorizontalAlign="Center"
            OnCreatedUser="CreateUserWizardStep1_CreatedUser" OnCreateUserError="CreateUserWizardStep1_CreateUserError"
            MembershipProvider="OpenIDMembershipProvider">
            <WizardSteps>
                <asp:CreateUserWizardStep ID="CreateUserWizardStep2" runat="server">
                    <ContentTemplate>
                        <table border="0" class="textImag">
                            <tr>
                                <td colspan="2">
                                    <div class="ListTitle" style="width: 390px; text-align: center;">
                                        Create an Account</div>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">OpenID:</asp:Label>
                                </td>
                                <td style="height: 26px">
                                    <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                                    <!--<asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                        ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>-->
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="color: red; height: 22px;">
                                    <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:CreateUserWizardStep>
                <asp:CompleteWizardStep ID="CompleteWizardStep2" runat="server">
                </asp:CompleteWizardStep>
            </WizardSteps>
            <StartNavigationTemplate>
                <asp:Button ID="StartNextButton" runat="server" CommandName="MoveNext" Text="Next" />
            </StartNavigationTemplate>
            <NavigationStyle HorizontalAlign="Center" />
        </asp:CreateUserWizard>
    </asp:View>
    <asp:View runat="server" ID="ConfirmationView">
        <asp:Label ID="ConfirmationLabel" runat="server">You have successfully created your membership to the ADL 3DR Repository. You will recieve an email once your account is approved. </asp:Label>
        <br />
        <br />
        <asp:Button ID="ConfirmationViewContinueButton" runat="server" OnClick="ConfirmationViewContinueButton_Click"
            Text="Continue" />
        <br />
        <br />
    </asp:View>
    <asp:View runat="server" ID="ErrorView">
        <asp:Label ID="ErrorLabel" runat="server">
            The code you provided was invalid.  Please try again.
        </asp:Label>
        <br />
        <br />
        <asp:Button ID="ErrorContinueButton" runat="server" Text="Continue" ToolTip="Continue"
            CausesValidation="false" OnClick="ErrorContinueButton_Click" />
    </asp:View>
</asp:MultiView>
