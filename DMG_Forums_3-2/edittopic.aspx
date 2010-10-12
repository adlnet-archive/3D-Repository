<%@ Page language="VB" Inherits="DMGForums.Topics.EditTopic" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="Edit Topic" runat="server" />
	</HEAD>
	<BODY>
		<form runat="server">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" runat="server" />

	<asp:Panel id="PagePanel" runat="server">

	<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
	<tr class="HeaderCell">
	<td align="left">
		<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b>Edit Topic</b></font>
	</td>
	</tr>
	<tr class="TableRow1">
	<td style="border-top:1px solid <%=Settings.TableBorderColor%>;">

		<table border="0" cellpadding="6" align="center">
		<asp:Panel id="ForumPanel" visible="false" runat="server">
		<tr>
			<td align="right" valign="middle">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Forum:</b></font>
			</td>
			<td align="left" valign="middle">
				<asp:DropDownList id="txtForumID" DataValueField="FORUM_ID" DataTextField="FORUM_NAME" runat="server" />
				<asp:Textbox id="OldForumID" visible="false" runat="server" />
			</td>
		</tr>
		</asp:Panel>
		<tr>
			<td align="right" valign="middle">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Subject:</b></font>
			</td>
			<td align="left" valign="middle">
				<asp:Textbox id="txtSubject" size="100" maxlength="100" runat="server" />
			</td>
		</tr>
		<tr>
			<td align="right" valign="top">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Message:</b><br /><br /><br /><a href="javascript:openHelp('DMGcode.html')">Forum Code</a><br />Allowed</font>
			</td>
			<td align="left" valign="middle">
				<asp:Textbox id="txtMessage" Textmode="Multiline" Columns="77" Rows="15" runat="server" />
			</td>
		</tr>
		<tr>
			<td align="right" valign="top"></td>
			<td align="left" valign="middle">
				<font size="2" color="<%=Settings.TopicsFontColor%>"> 
					<asp:CheckBox ID="txtSignature" Runat="server" text="Show Profile Signature?<br />" />
					<asp:CheckBox ID="txtSticky" visible="false" Runat="server" text="Make Topic Sticky?<br />" />
					<asp:CheckBox ID="txtNews" visible="false" Runat="server" text="Show Topic In Featured Topics Box?<br />" />
				</font>
			</td>
		</tr>
		<asp:Panel id="StatusPanel" visible="false" runat="server">
		<tr>
			<td align="right" valign="middle">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Status:</b></font>
			</td>
			<td align="left" valign="middle">
				<asp:DropDownList id="txtStatus" runat="server">
					<asp:ListItem Value="1" Text="Normal" />
					<asp:ListItem Value="2" Text="Locked" />
					<asp:ListItem Value="0" Text="Hidden" />
				</asp:DropDownList>
			</td>
		</tr>
		</asp:Panel>
		<tr>
			<td></td>
			<td align="left" valign="middle">
				<asp:Button type="submit" id="SubmitButton" onclick="SubmitTopic" text="Submit Topic" runat="server" />
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