<%@ Page language="VB" Inherits="DMGForums.Topics.DeleteTopic" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="Delete Topic" runat="server" />
	</HEAD>
	<BODY>
		<form runat="server">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" runat="server" />

	<asp:Panel id="PagePanel" runat="server">

	<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
	<tr class="HeaderCell">
	<td align="left">
		<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b>Delete Topic</b></font>
	</td>
	</tr>
	<tr class="SubHeaderCell">
	<td width="100%" align="center"><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b><asp:Label id="TopicSubject" runat="server" /></b></font></td>
	</tr>
	<tr class="TableRow1">
	<td style="border-top:1px solid <%=Settings.TableBorderColor%>;">
		<font size="2" color="<%=Settings.TopicsFontColor%>"> 
			<b>Warning:</b> This action can not be undone.  Pressing the delete button will completely remove the topic from the database along with all of its replies.  If you would just like to make the topic invisible but would like to keep all of the data, you can merely edit it and set its status to "hidden."
			<br /><br />
			<center>
			<asp:Textbox id="ForumID" visible="false" runat="server" />
			<asp:Button type="submit" id="DeleteButton" onclick="DeleteTopic" text="Delete Topic" runat="server" />
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