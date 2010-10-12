<%@ Page language="VB" Inherits="DMGForums.DeletePage" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="Delete Page" runat="server" />
	</HEAD>
	<BODY>
	<form runat="server">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" ShowLogin="1" runat="server" />
	<br />

	<asp:Panel ID="DeleteForm" runat="server">
		<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
		<tr class="HeaderCell">
		<td align="left">
			<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b>Delete Page</b></font>
		</td>
		</tr>
		<tr class="TableRow1">
		<td style="border-top:1px solid <%=Settings.TableBorderColor%>;">
		
			<font size="3" color="<%=Settings.TopicsFontColor%>"><b>Are you sure you want to delete this page?</b>
			<br /><br />
			Page Name: <asp:Label id="PageName" runat="server" />
			<br /><br />
			<asp:Button type="submit" id="DeleteButton" onclick="DeletePage" text="Yes" runat="server" />&nbsp;&nbsp;&nbsp;
			<asp:Button type="submit" id="Cancel" onclick="CancelDelete" text="No" runat="server" />
			</font>

		</td>
		</tr>
		</table>
	</asp:Panel>

	<div align="center" id="NoItemsDiv" class="MessageBox" runat="server" />

	<br />

	<DMG:Footer id="DMGFooter" runat="server" />

		</form>
	</BODY>
</HTML>