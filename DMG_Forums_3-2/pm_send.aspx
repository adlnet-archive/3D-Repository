<%@ Page language="VB" Inherits="DMGForums.Topics.PMSend" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="Send Private Message" runat="server" />

		<script language="JavaScript">
		<!--
		function openMemberList(url)
		{
			popupWin = window.open(url,'new_page','width=300,height=350,scrollbars=yes,toolbar=no,menubar=no,resizable=no')
		}
		function update(elemValue)
		{
			document.getElementById('txtTo').innerText=elemValue;
		}
		function HideButton()   
		{
			var theButton = document.getElementById("<%=SubmitButton.ClientID%>");
			theButton.style.display = "none";
			var theText = document.getElementById("<%=SubmitConfirm.ClientID%>");
			theText.style.display = "block";
			theText.style.fontWeight = "bold";
			theText.style.fontSize = "16px";
		}
		//-->
		</script>

	</HEAD>
	<BODY>
		<form runat="server" name="PMSender" id="PMSender">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" ShowLogin="1" runat="server" />

	<asp:PlaceHolder id="PagePanel" runat="server">

		<table border="0" width="97%" align="center" cellpadding="5" cellspacing="0">
		<tr>
		<td width="100%" align="left">
			&nbsp;
		</td>
		<td width="65" align="center" valign="middle" nowrap>
			<a href="pm_inbox.aspx"><img border="0" src="forumimages/pm_inbox.gif"></a>
		</td>
		<td width="65" align="center" valign="middle" nowrap>
			<a href="pm_send.aspx"><img border="0" src="forumimages/pm_new.gif"></a>
		</td>
		</tr>
		</table>

		<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
		<tr class="HeaderCell">
		<td colspan="4" align="left">
			<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b>Send New Private Message</b></font>
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
					<font size="2"><asp:Textbox id="txtTo" size="50" maxlength="50" runat="server" /> (<a href="javascript:openMemberList('pm_selectuser.aspx')">Select User</a>)</font>
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
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Message:</b><br /><br /><br /><a href="javascript:openHelp('DMGcode.html')">Forum Code</a><br />Allowed</font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtMessage" Textmode="Multiline" Columns="77" Rows="15" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="top"></td>
				<td align="left" valign="middle">
					<font size="2">
						<asp:CheckBox ID="txtSaveCopy" Runat="server" Checked="true" text="Save A Copy In Your Inbox?<br /><br />" />
						<asp:Button type="submit" id="SubmitButton" onclick="SubmitTopic" OnClientClick="javascript:HideButton()" text="Send Message" runat="server" />
						<asp:Label id="SubmitConfirm" text="Please Wait..." style="display: none;" runat="server" />
					</font>
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