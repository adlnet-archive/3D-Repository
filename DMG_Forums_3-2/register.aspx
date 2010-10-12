<%@ Page language="VB" Inherits="DMGForums.Members.Register" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>
<%@ Import Namespace="DMGForums.Global.Functions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="Register" runat="server" />
	</HEAD>
	<BODY>
		<form runat="server" name="EditProfile" id="EditProfile">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" runat="server" />

	<asp:Panel ID="PagePanel" runat="server">

	<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
	<tr class="HeaderCell">
	<td align="left">
		<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b><asp:Label id="RegistrationTitle" runat="server" /></b></font>
	</td>
	</tr>
	<tr class="TableRow1">
	<td style="border-top:1px solid <%=Settings.TableBorderColor%>;">

	<input type="hidden" id="txtAvatar" name="txtAvatar" value="1" />

	<asp:PlaceHolder ID="PrivacyNoticePanel" visible="false" runat="server">
		<font size="2" color="<%=Settings.TopicsFontColor%>">
			<%=CustomMessage("MESSAGE_PRIVACYNOTICE")%>
		</font>
		<center>
		<br /><br />
		<asp:Button id="AgreeButton" onclick="UserRegistration" text="Agree" runat="server" />
		&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
		<asp:Button id="CancelButton" onclick="CancelRegistration" text="Cancel" runat="server" />
		</center>
	</asp:PlaceHolder>

	<asp:PlaceHolder ID="RegistrationPanel" visible="false" runat="server">
		<center><font size="2" color="<%=Settings.TopicsFontColor%>">A unique username and e-mail address are required to access the forums.  The form will not submit until all requirements are met.<br /><br /></font></center>

		<asp:Label id="RequireEmailLabel" runat="server" visible="false" />

		<table border="0" cellpadding="10" align="center"><tr>
		<td width="50%" align="left" valign="top">

		<font size="3" color="<%=Settings.TopicsFontColor%>"><center><b>User Data</b></center></font>

		<table align="center" border="0" cellpadding="5" align="center">
		<tr>
		<td align="right" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			Username:  <font color="red">*</font>
			</font>
		</td>
		<td align="left" valign="top">
			<asp:textbox id="txtUsername" size="30" maxlength="30" runat="server" />
		</td>
		<td>
			<asp:RequiredFieldValidator runat="server"
				id="reqUsername"
				ControlToValidate="txtUsername"
				ErrorMessage="Required"
				Display="Static" />
		</td>
		</tr>
		<tr>
		<td align="right" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			Password:  <font color="red">*</font>
			</font>
		</td>
		<td align="left" valign="top">
			<asp:textbox id="txtPassword1" TextMode="password" size="31" maxlength="50" runat="server" />
		</td>
		<td>
			<asp:RequiredFieldValidator runat="server"
				id="reqPassword1"
				ControlToValidate="txtPassword1"
				ErrorMessage="Required"
				Display="Static" />
		</td>
		</tr>
		<tr>
		<td align="right" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			Password Again:  <font color="red">*</font>
			</font>
		</td>
		<td align="left" valign="top">
			<asp:textbox id="txtPassword2" TextMode="password" size="31" maxlength="50" runat="server" />
		</td>
		<td>
			<asp:RequiredFieldValidator runat="server"
				id="reqPassword2"
				ControlToValidate="txtPassword2"
				ErrorMessage="Required"
				Display="Static" />
		</td>
		</tr>
		<tr>
		<td align="right" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			E-Mail Address:  <font color="red">*</font>
			</font>
		</td>
		<td align="left" valign="top">
			<asp:textbox id="txtEmail1" size="30" maxlength="50" runat="server" />
		</td>
		<td>
			<asp:RequiredFieldValidator runat="server"
				id="reqEmail1"
				ControlToValidate="txtEmail1"
				ErrorMessage="Required"
				Display="Static" />
		</td>
		</tr>
		<tr>
		<td align="right" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			E-Mail Address Again:  <font color="red">*</font>
			</font>
		</td>
		<td align="left" valign="top">
			<asp:textbox id="txtEmail2" size="30" maxlength="50" runat="server" />
		</td>
		<td>
			<asp:RequiredFieldValidator runat="server"
				id="reqEmail2"
				ControlToValidate="txtEmail2"
				ErrorMessage="Required"
				Display="Static" />
		</td>
		</tr>

<asp:PlaceHolder ID="FullRegPanel" visible="true" runat="server">

		<tr>
		<td align="right" valign="top">
		</td>
		<td align="left" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			Show E-Mail?
			</font>
			<asp:DropDownList id="txtShowEmail" runat="server">
				<asp:ListItem Value="1" Selected="True" Text="Yes" />
				<asp:ListItem Value="0" Text="No" />
			</asp:DropDownList>
		</td>
		<td></td>
		</tr>

		<asp:PlaceHolder ID="CustomTitlePanel" visible="false" runat="server">
			<tr>
			<td align="right" valign="top">
				<font size="2" color="<%=Settings.TopicsFontColor%>">
				Custom Title:
				</font>
			</td>
			<td align="left" valign="top">
				<asp:textbox id="txtTitle" size="30" maxlength="30" runat="server" />
			</td>
			<td></td>
			</tr>
			<tr>
			<td align="right" valign="top">
			</td>
			<td align="left" valign="top">
				<font size="2" color="<%=Settings.TopicsFontColor%>">
				Use Custom Title?
				</font>
				<asp:DropDownList id="txtUseTitle" runat="server">
					<asp:ListItem Value="1" Text="Yes" />
					<asp:ListItem Value="0" Selected="True" Text="No" />
				</asp:DropDownList>
			</td>
			<td></td>
			</tr>
		</asp:PlaceHolder>

		<tr>
		<td align="right" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			AOL IM:
			</font>
		</td>
		<td align="left" valign="top">
			<asp:textbox id="txtAOL" size="30" maxlength="30" runat="server" />
		</td>
		<td></td>
		</tr>
		<tr>
		<td align="right" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			ICQ:
			</font>
		</td>
		<td align="left" valign="top">
			<asp:textbox id="txtICQ" size="30" maxlength="30" runat="server" />
		</td>
		<td></td>
		</tr>
		<tr>
		<td align="right" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			Yahoo IM:
			</font>
		</td>
		<td align="left" valign="top">
			<asp:textbox id="txtYahoo" size="30" maxlength="30" runat="server" />
		</td>
		<td></td>
		</tr>
		<tr>
		<td align="right" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			MSN Messenger:
			</font>
		</td>
		<td align="left" valign="top">
			<asp:textbox id="txtMSN" size="30" maxlength="30" runat="server" />
		</td>
		<td></td>
		</tr>

		<asp:PlaceHolder ID="AvatarPanel" visible="false" runat="server">
		<tr>
		<td align="right" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			Avatar:
			</font>
		</td>
		<td align="left" valign="top">
			<asp:textbox id="txtAvatarName" enabled="false" size="30" text="Default Avatar" runat="server" /><br />
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			<center>
				<a href="javascript:openAvatars('avatars.aspx')">Choose New Avatar</a><br />
				Show Avatar?
				<asp:DropDownList id="txtShowAvatar" runat="server">
					<asp:ListItem Value="1" Text="Yes" />
					<asp:ListItem Value="0" Selected="True" Text="No" />
				</asp:DropDownList>
			</center>
			</font>
		</td>
		<td></td>
		</tr>
		</asp:PlaceHolder>

		<tr>
		<td align="right" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			Signature:
			</font>
		</td>
		<td align="left" valign="top">
			<asp:textbox id="txtSignature" Columns="25" Rows="5" Textmode="multiline" runat="server" />
		</td>
		<td></td>
		</tr>
		<tr>
		<td align="right" valign="top">
		</td>
		<td align="left" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			Show Signature By Default?
			</font>
			<asp:DropDownList id="txtShowSignature" runat="server">
				<asp:ListItem Value="1" Selected="True" Text="Yes" />
				<asp:ListItem Value="0" Text="No" />
			</asp:DropDownList>
		</td>
		<td></td>
		</tr>
		</table>

		</td>
		<td align="left" valign="top" width="50%">

		<font size="3" color="<%=Settings.TopicsFontColor%>"><center><b>Profile Data</b></center></font>

		<table align="center" border="0" cellpadding="5" align="center">
		<tr>
		<td align="right" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			Real Name:
			</font>
		</td>
		<td align="left" valign="top">
			<asp:textbox id="txtRealName" size="30" maxlength="50" runat="server" />
		</td>
		<td></td>
		</tr>
		<tr>
		<td align="right" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			Location:
			</font>
		</td>
		<td align="left" valign="top">
			<asp:textbox id="txtLocation" size="30" maxlength="50" runat="server" />
		</td>
		<td></td>
		</tr>
		<tr>
		<td align="right" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			Homepage:
			</font>
		</td>
		<td align="left" valign="top">
			<asp:textbox id="txtHomePage" size="30" maxlength="100" runat="server" />
		</td>
		<td></td>
		</tr>
		<tr>
		<td align="right" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			Favorite Web Site:
			</font>
		</td>
		<td align="left" valign="top">
			<asp:textbox id="txtFavoriteSite" size="30" maxlength="100" runat="server" />
		</td>
		<td></td>
		</tr>
		<tr>
		<td align="right" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			Occupation:
			</font>
		</td>
		<td align="left" valign="top">
			<asp:textbox id="txtOccupation" size="30" maxlength="50" runat="server" />
		</td>
		<td></td>
		</tr>
		<tr>
		<td align="right" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			Sex:
			</font>
		</td>
		<td align="left" valign="top">
			<asp:DropDownList id="txtSex" runat="server">
				<asp:ListItem Selected="True" Value="" Text="" />
				<asp:ListItem Value="Male" Text="Male" />
				<asp:ListItem Value="Female" Text="Female" />
			</asp:DropDownList>
		</td>
		<td></td>
		</tr>
		<tr>
		<td align="right" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			Age:
			</font>
		</td>
		<td align="left" valign="top">
			<asp:textbox id="txtAge" size="5" maxlength="3" runat="server" />
		</td>
		<td></td>
		</tr>
		<tr>
		<td align="right" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			Birthday:
			</font>
		</td>
		<td align="left" valign="top">
			<asp:textbox id="txtBirthday" size="30" maxlength="50" runat="server" />
		</td>
		<td></td>
		</tr>
		<tr>
		<td align="right" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			Picture URL:
			</font>
		</td>
		<td align="left" valign="top">
			<asp:textbox id="txtPhoto" size="35" maxlength="100" runat="server" />
		</td>
		<td></td>
		</tr>
		<tr>
		<td align="right" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			Profile Text:
			</font>
		</td>
		<td align="left" valign="top">
			<asp:textbox id="txtNotes" Columns="30" Rows="6" Textmode="multiline" runat="server" />
		</td>
		<td></td>
		</tr>

</asp:PlaceHolder>

		</table>

		</td>
		</tr></table>

		<center>
		<font size="2" color="<%=Settings.TopicsFontColor%>">
		<asp:CompareValidator runat="server"
			id="PasswordCompare"
			ControlToValidate="txtPassword1"
			ErrorMessage="Passwords Do Not Match"
			ControlToCompare="txtPassword2" />

		<asp:RegularExpressionValidator runat="server"
			id="EmailValidator1"
			ControlToValidate="txtEmail1"
			ValidationExpression= "^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$"
			ErrorMessage = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Not Valid E-Mail" />

		<asp:CompareValidator runat="server"
			id="EmailCompare"
			ControlToValidate="txtEmail1"
			ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;E-Mails Do Not Match"
			ControlToCompare="txtEmail2" />
		</font>

		<br /><br />
		<asp:Button type="submit" id="SubmitButton" onclick="SubmitRegistration" text="Submit" runat="server" />
		</center>
	</asp:PlaceHolder>

	<asp:PlaceHolder ID="ConfirmationPanel" visible="false" runat="server">
			<font size="3" color="<%=Settings.TopicsFontColor%>"><b>
			<br />
			<asp:Label id="Complete" runat="server" />
			<br /><br />
			</b></font>
	</asp:PlaceHolder>

	</td>
	</tr>
	</table>

	</asp:Panel>

	<br />

	<div align="center" id="NoItemsDiv" class="MessageBox" runat="server" />

	<DMG:Footer id="DMGFooter" runat="server" />

		</form>
	</BODY>
</HTML>