<%@ Page language="VB" Inherits="DMGForums.Admin.CreateMember" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="Admin | Create Member" runat="server" />
	</HEAD>
	<BODY>
		<form runat="server">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" ShowLogin="1" runat="server" />

	<asp:PlaceHolder id="PagePanel" runat="server">

		<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
		<tr class="HeaderCell">
		<td align="left">
			<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b><a href="admin.aspx" style="color:<%=Settings.HeaderFontColor%>;">DMG Forums Admin Console</a> >> <a href="admin_createmember.aspx" style="color:<%=Settings.HeaderFontColor%>;">Create Member</a></b></font>
		</td>
		</tr>
		<tr class="TableRow1">
		<td style="border-top:1px solid <%=Settings.TableBorderColor%>;">
			<table border="2" cellpadding="10" align="center">
			<tr class="TableRow2">
			<td align="center" valign="middle" height="40">
				<font size="4" color="<%=Settings.TopicsFontColor%>"><b>Create Member</b></font>
			</td>
			</tr>
			<tr>
			<td valign="top">
				<table border="0" cellpadding="6" align="center">
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Username:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtUsername" size="30" maxlength="50" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Password:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtPassword" TextMode="password" size="30" maxlength="50" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Member Type:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtLevel" runat="server">
							<asp:ListItem Value="1" Text="Forum Member" Selected="True" />
							<asp:ListItem Value="2" Text="Moderator" />
							<asp:ListItem Value="3" Text="Administrator" />
							<asp:ListItem Value="0" Text="Banned Member" />
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Custom Title:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtTitle" size="30" maxlength="30" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Use Custom Title?</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtUseTitle" runat="server">
							<asp:ListItem Value="1" Text="Yes" />
							<asp:ListItem Value="0" Text="No" Selected="True" />
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Allow Title Editing?</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtAllowTitle" runat="server">
							<asp:ListItem Value="1" Text="Yes" />
							<asp:ListItem Value="0" Text="No" Selected="True" />
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Ranking:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtRanking" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>E-Mail Address:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtEmail" size="30" maxlength="50" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Show E-Mail?</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtShowEmail" runat="server">
							<asp:ListItem Value="1" Text="Yes" />
							<asp:ListItem Value="0" Text="No" Selected="True" />
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Allow Custom Avatars?</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtAllowCustomAvatar" runat="server">
							<asp:ListItem Value="1" Text="Yes" />
							<asp:ListItem Value="0" Text="No" Selected="True" />
						</asp:DropDownList>
					</td>
				</tr>
				</table>
			</td>
			</tr>
			<tr>
				<td align="center" valign="middle">
					<asp:Button type="submit" id="CreateMemberSubmit" onclick="SubmitNewMember" text="Submit New Member" CssClass="AdminButtons" runat="server" />
				</td>
			</tr>
			</table>
		</td>
		</tr>
		</table>
		<br />

	</asp:PlaceHolder>

	<div align="center" id="NoItemsDiv" class="MessageBox" runat="server" />

	<DMG:Footer id="DMGFooter" runat="server" />

		</form>
	</BODY>
</HTML>