<%@ Page language="VB" Inherits="DMGForums.Topics.EditReply" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="Edit Reply" runat="server" />
	</HEAD>
	<BODY>
		<form runat="server">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" runat="server" />

	<asp:Panel id="PagePanel" runat="server">

	<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
	<tr class="HeaderCell">
	<td align="left">
		<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b>Edit Reply</b></font>
	</td>
	</tr>
	<tr class="TableRow1">
	<td style="border-top:1px solid <%=Settings.TableBorderColor%>;">

		<table border="0" cellpadding="6" align="center">
		<tr>
			<td align="right" valign="middle">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Topic:</b></font>
			</td>
			<td align="left" valign="middle">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><asp:Label id="txtTopicSubject" runat="server" /></font>
			</td>
		</tr>
		<tr>
			<td align="right" valign="top">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Reply Message:</b><br /><br /><br /><a href="javascript:openHelp('DMGcode.html')">Forum Code</a><br />Allowed</font>
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
					<br />
					<asp:Textbox ID="txtTopicID" visible="false" runat="server" />
					<asp:Button type="submit" id="SubmitButton" onclick="SubmitReply" text="Submit Reply" runat="server" />
				</font>
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