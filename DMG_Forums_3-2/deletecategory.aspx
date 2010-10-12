<%@ Page language="VB" Inherits="DMGForums.Forums.DeleteCategory" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="Delete Category" runat="server" />
	</HEAD>
	<BODY>
		<form runat="server">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" ShowLogin="1" runat="server" />

	<asp:Panel id="PagePanel" runat="server">

	<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
	<tr class="HeaderCell">
	<td align="left">
		<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b>Delete Category</b></font>
	</td>
	</tr>
	<tr class="SubHeaderCell">
	<td width="*" align="center"><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b><asp:Label id="CategoryName" runat="server" /></b></font></td>
	</tr>
	<tr class="TableRow1">
	<td style="border-top:1px solid <%=Settings.TableBorderColor%>;">
		<font size="2" color="<%=Settings.TopicsFontColor%>"> 
			<b>Warning:</b> This action can not be undone.  Pressing the delete button will completely remove the category from the database along with all of its forums and topics.  If you would just like to make the category invisible but would like to keep all of the data, you can merely edit it and set its status to "off."
			<br /><br />
			<center>
			<asp:Button type="submit" id="DeleteButton" onclick="DeleteCategory" text="Delete Category" runat="server" />
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