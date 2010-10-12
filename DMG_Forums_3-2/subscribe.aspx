<%@ Page language="VB" Inherits="DMGForums.Topics.Subscribe" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="Subscribe" runat="server" />
	</HEAD>
	<BODY>
		<form runat="server">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" runat="server" />

	<asp:PlaceHolder id="PagePanel" runat="server">

	<asp:PlaceHolder id="SubForm" runat="server">
		<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
		<tr class="HeaderCell">
		<td align="left">
			<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b>Subscribe To Thread</b></font>
		</td>
		</tr>
		<tr class="TableRow1">
		<td style="border-top:1px solid <%=Settings.TableBorderColor%>;">

			<table border="0" cellpadding="6" align="center">
			<tr>
				<td colspan="2" align="center" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"> 
						Click the "Subscribe" button to subscribe to this thread.
					</font>
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Receive E-Mail Notices Of New Replies?</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:DropDownList id="txtSendEmail" runat="server">
						<asp:ListItem Value="1" Text="Yes" />
						<asp:ListItem Value="0" Text="No" Selected="True" />
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td colspan="2" align="center" valign="middle">
					<font size="2"> 
						<asp:Button type="submit" id="SubmitButton" onclick="SubmitSubscription" text="Subscribe" runat="server" />
					</font>
				</td>
			</tr>
			</table>
	
		</td>
		</tr>
		</table>
		<br />
	</asp:PlaceHolder>

	<asp:PlaceHolder id="UnSubForm" visible="false" runat="server">
		<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
		<tr class="HeaderCell">
		<td align="left">
			<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b>Unsubscribe From Thread</b></font>
		</td>
		</tr>
		<tr class="TableRow1">
		<td style="border-top:1px solid <%=Settings.TableBorderColor%>;" align="center">

			<br /><br />
			<font size="2" color="<%=Settings.TopicsFontColor%>"><b>You are currently subscribed to this thread.  Would you like to unsubscribe?</b></font>
			<br /><br />
			<asp:Button type="submit" id="SubmitButton2" onclick="SubmitUnSubscription" text="Yes" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
			<asp:Button type="submit" id="SubmitButton3" onclick="CancelUnSubscription" text="No" runat="server" />
			<br /><br />
	
		</td>
		</tr>
		</table>
		<br />
	</asp:PlaceHolder>

	</asp:PlaceHolder>

	<div align="center" id="NoItemsDiv" class="MessageBox" runat="server" />

	<DMG:Footer id="DMGFooter" runat="server" />

		</form>
	</BODY>
</HTML>