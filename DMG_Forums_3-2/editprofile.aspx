<%@ Page language="VB" Inherits="DMGForums.Members.EditProfile" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="Edit Profile" runat="server" />
	</HEAD>
	<BODY>
		<form runat="server" name="EditProfile" id="EditProfile">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" ShowLogin="1" runat="server" />

	<asp:Panel ID="PagePanel" runat="server">

	<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
	<tr class="HeaderCell">
	<td align="left">
			<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b><asp:Label id="UsernameLabel" runat="server" /></b></font>
	</td>
	</tr>
	<tr class="TableRow1">

	<td style="border-top:1px solid <%=Settings.TableBorderColor%>;border-right:1px solid <%=Settings.TableBorderColor%>;">

	<asp:Panel ID="EditProfilePanel" visible="false" runat="server">

		<table border="0" cellpadding="10" align="center"><tr>
		<td width="50%" align="left" valign="top">

		<font size="3" color="<%=Settings.TopicsFontColor%>"><center><b>User Data</b></center></font>

		<table align="center" border="0" cellpadding="5" align="center">

		<asp:PlaceHolder ID="MemberTypePanel" visible="false" runat="server">
			<tr>
			<td align="right" valign="top">
				<font size="2" color="<%=Settings.TopicsFontColor%>">
				Member Type:
				</font>
			</td>
			<td align="left" valign="top">
				<asp:DropDownList id="txtLevel" runat="server">
					<asp:ListItem Value="1" Text="Forum Member" />
					<asp:ListItem Value="2" Text="Moderator" />
					<asp:ListItem Value="3" Text="Administrator" />
					<asp:ListItem Value="0" Text="Banned Member" />
				</asp:DropDownList>
			</td>
			<td></td>
			</tr>
			<tr>
			<td align="right" valign="top">
				<font size="2" color="<%=Settings.TopicsFontColor%>">
				Post Count:
				</font>
			</td>
			<td align="left" valign="top">
				<asp:Textbox id="txtPostCount" size="5" runat="server" />
			</td>
			<td></td>
			</tr>
		</asp:PlaceHolder>

		<asp:PlaceHolder ID="MemberTitlePanel" visible="false" runat="server">
			<tr>
			<td align="right" valign="top">
				<font size="2" color="<%=Settings.TopicsFontColor%>">
				Custom Title:
				</font>
			</td>
			<td align="left" valign="top">
				<asp:Textbox id="txtTitle" size="30" maxlength="30" runat="server" />
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
					<asp:ListItem Value="0" Text="No" />
				</asp:DropDownList>
			</td>
			<td></td>
			</tr>
		</asp:PlaceHolder>

		<asp:PlaceHolder ID="MemberTitleEditPanel" visible="false" runat="server">
			<tr>
			<td align="right" valign="top">
			</td>
			<td align="left" valign="top">
				<font size="2" color="<%=Settings.TopicsFontColor%>">
				Allow Title Editing?
				</font>
				<asp:DropDownList id="txtAllowTitle" runat="server">
					<asp:ListItem Value="1" Text="Yes" />
					<asp:ListItem Value="0" Text="No" />
				</asp:DropDownList>
			</td>
			<td></td>
			</tr>
		</asp:PlaceHolder>

		<asp:PlaceHolder ID="MemberRankingPanel" visible="false" runat="server">
			<tr>
			<td align="right" valign="top">
				<font size="2" color="<%=Settings.TopicsFontColor%>">
				Ranking:
				</font>
			</td>
			<td align="left" valign="top">
				<asp:DropDownList id="txtRanking" runat="server" />
			</td>
			<td></td>
			</tr>
		</asp:PlaceHolder>

		<tr>
		<td align="right" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			E-Mail Address:
			</font>
		</td>
		<td align="left" valign="top">
			<asp:textbox id="txtEmail" size="30" maxlength="50" runat="server" />
		</td>
		<td>
			<asp:RequiredFieldValidator runat="server"
				id="reqEmail1"
				ControlToValidate="txtEmail"
				ErrorMessage="*"
				Display="Static" />
		</td>
		</tr>
		<tr>
		<td align="right" valign="top">
		</td>
		<td align="left" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			Show E-Mail?
			</font>
			<asp:DropDownList id="txtShowEmail" runat="server">
				<asp:ListItem Value="1" Text="Yes" />
				<asp:ListItem Value="0" Text="No" />
			</asp:DropDownList>
		</td>
		<td></td>
		</tr>
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

		<asp:PlaceHolder ID="MemberAvatarPanel" visible="false" runat="server">
		<tr>
		<td align="right" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			Avatar:
			</font>
		</td>
		<td align="left" valign="top">
			<asp:textbox id="txtAvatarName" enabled="false" size="30" runat="server" /><br />
			<font size="2">
			<center>
				<a href="javascript:openAvatars('avatars.aspx')">Choose New Avatar</a>
			</center>
			</font>
		</td>
		<td></td>
		</tr>
		<tr>
		<td align="right" valign="top"></td>
		<td align="left" valign="top">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
			<center>
				Show Avatar?
				<asp:DropDownList id="txtShowAvatar" runat="server">
					<asp:ListItem Value="1" Text="Yes" />
					<asp:ListItem Value="0" Text="No" />
				</asp:DropDownList>
			</center>
			</font>
		</td>
		<td></td>
		</tr>
		</asp:PlaceHolder>

		<input type="hidden" id="txtAvatar" name="txtAvatar" value="<%=mAvatar%>" />

		<asp:PlaceHolder ID="MemberCustomAvatarPanel" visible="false" runat="server">
			<tr>
			<td align="right" valign="top">
			</td>
			<td align="left" valign="top">
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					<center>
					<br />
					<a href="javascript:openUploader('upload.aspx?TYPE=customavatar&ID=<%=Request.Querystring("ID")%>')">Upload Custom Avatar</a><br /><br />
					Use Custom Avatar?
					</font>
					<asp:DropDownList id="txtUseCustomAvatar" runat="server">
						<asp:ListItem Value="1" Text="Yes" />
						<asp:ListItem Value="0" Text="No" />
					</asp:DropDownList>
					</center>
				</font>
			</td>
			<td></td>
			</tr>
		</asp:PlaceHolder>

		<asp:PlaceHolder ID="MemberCustomAvatarEditPanel" visible="false" runat="server">
			<tr>
			<td align="right" valign="top">
			</td>
			<td align="left" valign="top">
				<font size="2" color="<%=Settings.TopicsFontColor%>">
				Allow Custom Avatar Editing?
				</font>
				<asp:DropDownList id="txtAllowCustomAvatar" runat="server">
					<asp:ListItem Value="1" Text="Yes" />
					<asp:ListItem Value="0" Text="No" />
				</asp:DropDownList>
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
				<asp:ListItem Value="1" Text="Yes" />
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
				<asp:ListItem Value="" Text="" />
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

		<asp:PlaceHolder ID="MemberNotesEditPanel" visible="false" runat="server">
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

		<asp:RegularExpressionValidator runat="server"
			id="EmailValidator1"
			ControlToValidate="txtEmail"
			ValidationExpression= "^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$"
			ErrorMessage = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Not Valid E-Mail" />

		</font>

		<br /><br />
		<asp:Button type="submit" id="SubmitButton" onclick="SubmitChanges" text="Save Profile" runat="server" />
		</center>

	</asp:Panel>


	<asp:Panel id="ConfirmationPanel" visible="false" runat="server">
		<font size="2" color="<%=Settings.TopicsFontColor%>">
		Your profile has been updated.  Click the links below to continue editing or return to the forums and begin using your new details.
		<br /><br />
		<ul>
			<li><a href="editprofile.aspx?ID=<%=Request.Querystring("ID")%>">Continue Editing Your Profile</a></li>
			<li><a href="profile.aspx?ID=<%=Request.Querystring("ID")%>">View Your Profile</a></li>
			<li><a href="usercp.aspx?ID=<%=Request.Querystring("ID")%>">View Your Control Panel</a></li>
		</ul>
		</font>
	</asp:Panel>

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