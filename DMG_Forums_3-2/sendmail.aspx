<%@ Page language="VB" Inherits="DMGForums.Members.SendEmail" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="Send E-Mail" runat="server" />
	</HEAD>
	<BODY>
		<form runat="server">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" runat="server" />

	<asp:PlaceHolder id="PagePanel" runat="server">

	<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
	<tr class="HeaderCell">
	<td align="left">
		<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b>Send E-Mail Message</b></font>
	</td>
	</tr>
	<tr class="TableRow1">
	<td style="border-top:1px solid <%=Settings.TableBorderColor%>;">

		<table border="0" cellpadding="6" align="center">
		<tr>
			<td align="right" valign="middle">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Send To:</b></font>
			</td>
			<td align="left" valign="middle">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><asp:Label ID="txtTo" runat="server" /></font>
			</td>
		</tr>
		<tr>
			<td align="right" valign="middle">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Subject:</b></font>
			</td>
			<td align="left" valign="middle">
				<asp:Textbox id="txtSubject" size="50" maxlength="50" runat="server" />
			</td>
		</tr>
		<tr>
			<td align="right" valign="top">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Message:</b></font>
			</td>
			<td align="left" valign="middle">
				<asp:Textbox id="txtMessage" Textmode="Multiline" Columns="77" Rows="15" runat="server" />
			</td>
		</tr>
		<tr>
			<td align="right" valign="top"></td>
			<td align="left" valign="middle">
				<asp:Button type="submit" id="SubmitButton" onclick="SubmitEmail" text="Send E-Mail" runat="server" />
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