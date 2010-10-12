<%@ Page language="VB" Inherits="DMGForums.Topics.SelectForum" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="Select Forum" runat="server" />
	</HEAD>
	<BODY>
		<form runat="server">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" runat="server" />

	<asp:Panel id="PagePanel" runat="server">

	<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
	<tr class="HeaderCell">
	<td align="left">
		<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b>Post New Topic</b></font>
	</td>
	</tr>
	<tr class="TableRow1">
	<td style="border-top:1px solid <%=Settings.TableBorderColor%>;">
		<font size="2" color="<%=Settings.TopicsFontColor%>"> 
			<center>
			Select a forum to post to.
			<br /><br />
			<asp:DropDownList id="ForumList" AutoPostBack="true" OnSelectedIndexChanged="SelectForum" runat="server" />
			</center>
		</font>
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