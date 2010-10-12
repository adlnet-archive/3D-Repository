<%@ Page language="VB" Inherits="DMGForums.Forums.EditForum" maintainScrollPositionOnPostBack="true" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="Edit Forum" runat="server" />
	</HEAD>
	<BODY>
		<form runat="server">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" ShowLogin="1" runat="server" />

	<asp:Panel id="PagePanel" runat="server">

	<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
	<tr class="HeaderCell">
	<td align="left">
		<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b>Edit Forum</b></font>
	</td>
	<tr class="TableRow1">
	<td style="border-top:1px solid <%=Settings.TableBorderColor%>;">

		<table border="0" cellpadding="6" align="center">
		<tr>
			<td align="right" valign="middle">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Category:</b></font>
			</td>
			<td align="left" valign="middle">
				<asp:DropDownList id="txtCategoryID" DataValueField="CATEGORY_ID" DataTextField="CATEGORY_NAME" runat="server" />
			</td>
		</tr>
		<tr>
			<td align="right" valign="middle">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Forum Name:</b></font>
			</td>
			<td align="left" valign="middle">
				<asp:Textbox id="txtName" size="60" maxlength="100" runat="server" />
			</td>
		</tr>
		<tr>
			<td align="right" valign="middle">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Forum Page Content:</b></font>
			</td>
			<td align="left" valign="middle"><font size="2" color="<%=Settings.TopicsFontColor%>"><a href="javascript:openHelp('DMGAdminCode.html')">DMG Admin Code</a> Allowed</font></td>
		</tr>
		<tr>
			<td align="right" valign="middle" colspan="2">
				<asp:Textbox id="txtContent" Columns="85" Rows="15" Textmode="multiline" runat="server" />
			</td>
		</tr>
		<tr>
			<td align="right" valign="top">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Status:</b></font>
			</td>
			<td align="left" valign="middle">
				<asp:DropDownList id="txtStatus" runat="server">
					<asp:ListItem Value="1" Text="On" runat="server" />
					<asp:ListItem Value="0" Text="Off" runat="server" />
					<asp:ListItem Value="2" Text="Locked" runat="server" />
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td align="right" valign="top">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Require Moderator Confirmation of New Posts?</b></font>
			</td>
			<td align="left" valign="middle">
				<asp:DropDownList id="txtForceConfirm" AutoPostBack="true" OnSelectedIndexChanged="ConfirmationChange" runat="server">
					<asp:ListItem Value="1" Text="Yes" runat="server" />
					<asp:ListItem Value="0" Text="No" runat="server" />
				</asp:DropDownList>
			</td>
		</tr>
		<asp:PlaceHolder id="EmailModeratorsPanel" visible="false" runat="server">
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Send E-Mail Updates To Moderators?</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:DropDownList id="txtEmailModerators" runat="server">
						<asp:ListItem Value="1" Text="Yes" runat="server" />
						<asp:ListItem Value="0" Text="No" runat="server" />
					</asp:DropDownList>
				</td>
			</tr>
		</asp:PlaceHolder>
		<tr>
			<td align="right" valign="top">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Show Posts In Latest Topics Box?</b></font>
			</td>
			<td align="left" valign="middle">
				<asp:DropDownList id="txtShowLatest" runat="server">
					<asp:ListItem Value="1" Text="Yes" runat="server" />
					<asp:ListItem Value="0" Text="No" runat="server" />
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td align="right" valign="top">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Show HTML Header/Footer?</b></font>
			</td>
			<td align="left" valign="middle">
				<asp:DropDownList id="txtShowHeaders" runat="server">
					<asp:ListItem Value="1" Text="Yes" runat="server" />
					<asp:ListItem Value="0" Text="No" runat="server" />
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td align="right" valign="top">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Show Login Box?</b></font>
			</td>
			<td align="left" valign="middle">
				<asp:DropDownList id="txtShowLogin" runat="server">
					<asp:ListItem Value="1" Text="Yes" runat="server" />
					<asp:ListItem Value="0" Text="No" runat="server" />
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td align="right" valign="top">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Sort By:</b></font>
			</td>
			<td align="left" valign="middle">
				<asp:Textbox id="txtSortBy" size="10" maxlength="10" runat="server" />
				<font size="2" color="<%=Settings.TopicsFontColor%>"><br />(Enter An Integer, Default Is 1)</font>
			</td>
		</tr>
		<tr>
			<td align="right" valign="top">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Description:</b></font>
			</td>
			<td align="left" valign="middle">
				<asp:Textbox id="txtDescription" Textmode="Multiline" Columns="50" Rows="7" runat="server" />
			</td>
		</tr>
		<tr>
			<td align="right" valign="top">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Security:</b></font>
			</td>
			<td align="left" valign="middle">
				<asp:DropDownList id="txtType" AutoPostBack="true" OnSelectedIndexChanged="SecurityChange" runat="server">
					<asp:ListItem Value="0" Text="All Visitors" runat="server" />
					<asp:ListItem Value="1" Text="Allowed Members List" runat="server" />
					<asp:ListItem Value="2" Text="Password Protected" runat="server" />
					<asp:ListItem Value="3" Text="Moderators Only" runat="server" />
					<asp:ListItem Value="4" Text="Members Only" runat="server" />
				</asp:DropDownList>
			</td>
		</tr>
		<asp:PlaceHolder id="PasswordPanel" visible="false" runat="server">
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Password:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtPassword" TextMode="password" size="30" maxlength="50" runat="server" />
					<font size="2" color="red"><br />(Must Be Re-Entered)</font>
				</td>
			</tr>
		</asp:PlaceHolder>
		<asp:PlaceHolder id="AllowedUsersPanel" visible="false" runat="server">
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Member List:</b></font>
				</td>
				<td align="left" valign="top">
					<asp:Textbox id="SelectedMemberList" visible="false" runat="server" />
					<table border="0" cellpadding="6">
					<tr>
					<td align="left" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>">Available</font><br />
						<asp:ListBox DataValueField="MEMBER_ID" DataTextField="MEMBER_USERNAME" id="txtMembers" runat="server" />
					</td>
					<td align="center" valign="middle">
						<asp:button id="FArrow" onclick="AddMember" CssClass="dmgbuttons" runat="server" Text=">>" /><br /><br />
						<asp:button id="BArrow" onclick="RemoveMember" CssClass="dmgbuttons" runat="server" Text="<<" />
					</td>
					<td align="left" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>">Selected</font><br />
						<asp:ListBox id="txtAllowedMembers" DataValueField="MEMBER_ID" DataTextField="MEMBER_USERNAME" runat="server" />
					</td>
					</tr>
					</table>
				</td>
			</tr>
		</asp:PlaceHolder>
		<tr>
			<td align="right" valign="top">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Moderators:</b></font>
			</td>
			<td align="left" valign="top">
				<asp:Textbox id="SelectedModeratorList" visible="false" runat="server" />
				<table border="0" cellpadding="6">
				<tr>
				<td align="left" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>">Available</font><br />
					<asp:ListBox DataValueField="MEMBER_ID" DataTextField="MEMBER_USERNAME" id="txtModerators" runat="server" />
				</td>
				<td align="center" valign="middle">
					<asp:button id="FmArrow" onclick="AddModerator" CssClass="dmgbuttons" runat="server" Text=">>" /><br /><br />
					<asp:button id="BmArrow" onclick="RemoveModerator" CssClass="dmgbuttons" runat="server" Text="<<" />
				</td>
				<td align="left" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>">Selected</font><br />
					<asp:ListBox DataValueField="MEMBER_ID" DataTextField="MEMBER_USERNAME" id="txtAllowedModerators" runat="server" />
				</td>
				</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td align="right" valign="top"></td>
			<td align="left" valign="middle">
				<asp:Button type="submit" id="SubmitButton" onclick="EditForum" text="Save Changes" runat="server" />
			</td>
		</tr>
		</table>
	</td>
	</tr>
	</table>
	<br />

	</asp:Panel>

	<div align="center" id="NoItemsDiv" class="MessageBox" runat="server" />

	<DMG:Footer id="DMGFooter" runat="server" />

		</form>
	</BODY>
</HTML>