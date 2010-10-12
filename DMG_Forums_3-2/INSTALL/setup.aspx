<%@ Page language="VB" Inherits="DMGForums.Setup.SQLSetup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<title>DMG Forums Installer</title>
	</HEAD>
	<BODY>
		<form enctype="multipart/form-data" runat="server">

		<table border="0" width="100%" height="100%">
		<tr>
		<td width="100%" height="100%" align="center" valign="middle">

			<asp:Panel id="InformationPanel" visible="false" runat="server">
				<font size="4" face="arial,helvetica"><b>DMG Forums Installer</b></font><br /><br />
				<font face="arial,helvetica" size="2">
				Enter the details below and press "Continue" to set up the forums.<br />Be sure to make an administrator username/password that you will always remember and that can not be hacked easily.<br /><br />
				</font>

				<table border="0" cellpadding="6" align="center">
				<tr>
					<td align="right" valign="middle">
						<font face="arial,helvetica" size="2"><b>Administrator Username:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="AdminUsername" size="50" maxlength="50" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font face="arial,helvetica" size="2"><b>Administrator Password:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox TextMode="Password" id="AdminPassword" size="50" maxlength="100" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font face="arial,helvetica" size="2"><b>Administrator E-Mail:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="AdminEmail" size="50" maxlength="50" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font face="arial,helvetica" size="2"><b>Forum Title:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="ForumTitle" size="50" maxlength="100" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font face="arial,helvetica" size="2"><b>Main Web Site URL:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="WebSiteURL" size="50" maxlength="100" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="top"></td>
					<td align="left" valign="middle">
						<asp:Button type="submit" id="Submit" onclick="SubmitInformation" text="Continue" runat="server" />
					</td>
				</tr>
				</table>
			</asp:Panel>

			<asp:Panel id="TemplatePanel" visible="false" runat="server">
				<font size="4" face="arial,helvetica"><b>DMG Forums Installer</b></font><br /><br />
				<font face="arial,helvetica" size="2">
				Select a default template.  All colors, HTML, and layout can be modified after installation.<br /><br />
				</font>

				<table border="0" cellpadding="6" align="center">
				<tr>
					<td align="center" valign="top">
						<asp:LinkButton runat="server" ID="BlankTemplateButton" CommandArgument="BlankTemplate" Text="<img src='templateicons/blanktemplate.gif'><br />Blank Template" onClick="SubmitTemplate" />
					</td>
					<td align="center" valign="top">
						<asp:LinkButton runat="server" ID="WhiteSmokeButton" CommandArgument="WhiteSmoke" Text="<img src='templateicons/whitesmoke.gif'><br />White Smoke" onClick="SubmitTemplate" />
					</td>
					<td align="center" valign="top">
						<asp:LinkButton runat="server" ID="CenterScrollBlackButton" CommandArgument="CenterScrollBlack" Text="<img src='templateicons/centerscrollblack.gif'><br />Center Scroll Black" onClick="SubmitTemplate" />
					</td>
				</tr>
				<tr>
					<td align="center" valign="top">
						<asp:LinkButton runat="server" ID="BlueGradientsButton" CommandArgument="BlueGradients" Text="<img src='templateicons/bluegradients.gif'><br />Blue Gradients" onClick="SubmitTemplate" />
					</td>
					<td align="center" valign="top">
						<asp:LinkButton runat="server" ID="RedGradientsButton" CommandArgument="RedGradients" Text="<img src='templateicons/redgradients.gif'><br />Red Gradients" onClick="SubmitTemplate" />
					</td>
					<td align="center" valign="top">
						<asp:LinkButton runat="server" ID="GreenGradientsButton" CommandArgument="GreenGradients" Text="<img src='templateicons/greengradients.gif'><br />Green Gradients" onClick="SubmitTemplate" />
					</td>
				</tr>
				<tr>
					<td align="center" valign="top">
						<asp:LinkButton runat="server" ID="CenterScrollBlueButton" CommandArgument="CenterScrollBlue" Text="<img src='templateicons/centerscrollblue.gif'><br />Center Scroll Blue" onClick="SubmitTemplate" />
					</td>
					<td align="center" valign="top">
						<asp:LinkButton runat="server" ID="CenterScrollRedButton" CommandArgument="CenterScrollRed" Text="<img src='templateicons/centerscrollred.gif'><br />Center Scroll Red" onClick="SubmitTemplate" />
					</td>
					<td align="center" valign="top">
						<asp:LinkButton runat="server" ID="CenterScrollGreenButton" CommandArgument="CenterScrollGreen" Text="<img src='templateicons/centerscrollgreen.gif'><br />Center Scroll Green" onClick="SubmitTemplate" />
					</td>
				</tr>
				<tr>
					<td align="center" valign="top">
						<asp:LinkButton runat="server" ID="LeftButtonsBlueButton" CommandArgument="LeftButtonsBlue" Text="<img src='templateicons/leftbuttonsblue.gif'><br />Left Buttons Blue" onClick="SubmitTemplate" />
					</td>
					<td align="center" valign="top">
						<asp:LinkButton runat="server" ID="LeftButtonsRedButton" CommandArgument="LeftButtonsRed" Text="<img src='templateicons/leftbuttonsred.gif'><br />Left Buttons Red" onClick="SubmitTemplate" />
					</td>
					<td align="center" valign="top">
						<asp:LinkButton runat="server" ID="LeftButtonsGreenButton" CommandArgument="LeftButtonsGreen" Text="<img src='templateicons/leftbuttonsgreen.gif'><br />Left Buttons Green" onClick="SubmitTemplate" />
					</td>
				</tr>
				</table>
			</asp:Panel>

			<asp:Panel id="FinalOptionsPanel" visible="false" runat="server">
				<font size="4" face="arial,helvetica"><b>DMG Forums Installer</b></font><br /><br />
				<font face="arial,helvetica" size="2">
				Select the final options and press "Install DMG Forums" to finalize the installation.<br /><br />
				The installation type will build your default settings based on how you plan to use the application.<br />
				If you choose "Forums Only" and want to add content pages later, it will still be possible.<br /><br />
				"Install All Templates" will install the templates that you did not choose as default, but will leave them unselected.<br /><br />
				Unicode encoding will allow you to use foreign languages and special characters in your text forms, but will require<br />
				larger data types in your database.  If you plan to have an English-only forum, "Standard" is the preferred method.<br /><br />
				</font>

				<table border="0" cellpadding="6" align="center">
				<tr>
					<td align="right" valign="middle">
						<font face="arial,helvetica" size="2"><b>Installation Type:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="InstallationType" runat="server">
							<asp:ListItem Value="2" Selected="True" Text="Full Portal" />
							<asp:ListItem Value="1" Text="Forums Only" />
							<asp:ListItem Value="0" Text="Content Pages Only" />
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font face="arial,helvetica" size="2"><b>Install All Templates?</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="InstallAllTemplates" runat="server">
							<asp:ListItem Value="1" Text="Yes" />
							<asp:ListItem Value="0" Selected="True" Text="No" />
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font face="arial,helvetica" size="2"><b>Character Encoding:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="CharacterEncoding" runat="server">
							<asp:ListItem Value="1" Selected="True" Text="Standard" />
							<asp:ListItem Value="2" Text="Unicode" />
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right" valign="top"></td>
					<td align="left" valign="middle">
						<asp:Button type="submit" id="InstallSubmit" onclick="InstallDMGForums" text="Install DMG Forums" runat="server" />
					</td>
				</tr>
				</table>
			</asp:Panel>

			<asp:Panel id="MessagePanel" visible="false" runat="server">
				<font face="arial,helvetica" size="3">
					<asp:Label id="Message" runat="server" />
				</font>
			</asp:Panel>

		</td>
		</tr>
		</table>

		</form>
	</BODY>
</HTML>
