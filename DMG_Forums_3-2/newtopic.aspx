<%@ Page language="VB" Inherits="DMGForums.Topics.NewTopic" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="New Topic" runat="server" />

		<script type="text/javascript">
		<!--
			function HideButton()   
			{
				var theButton = document.getElementById("<%=SubmitButton.ClientID%>");
				theButton.style.display = "none";
				var theButton = document.getElementById("<%=SubmitPreview.ClientID%>");
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

		<table border="0" cellpadding="6" align="center">
		<tr>
			<td align="right" valign="middle">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Forum:</b></font>
			</td>
			<td align="left" valign="middle">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><asp:Label id="ForumName" runat="server" /></font>
			</td>
		</tr>
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
				<font size="2" color="<%=Settings.TopicsFontColor%>">
				<b>Message:</b><br /><br /><br /><br /><a href="javascript:openHelp('DMGcode.html')">Forum Code</a><br />Allowed
				</font>
			</td>
			<td align="left" valign="middle">
				<asp:Textbox id="txtMessage" Textmode="Multiline" Columns="77" Rows="15" runat="server" />
			</td>
		</tr>
		<asp:PlaceHolder id="FileUploadPanel" visible="false" runat="server">
		<tr>
			<td align="right" valign="middle">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>File Upload:</b></font>
			</td>
			<td align="left" valign="middle">
				<input type="file" id="file" size="40" runat="server" />
			</td>
		</tr>
		</asp:PlaceHolder>
		<tr>
			<td align="right" valign="top"></td>
			<td align="left" valign="middle">
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					<asp:CheckBox ID="txtSignature" Runat="server" text="Show Profile Signature?<br />" />
					<asp:CheckBox ID="txtSubscribe" visible="false" Runat="server" text="Subscribe To New Topic?<br />" />
					<asp:CheckBox ID="txtSticky" visible="false" Runat="server" text="Make Topic Sticky?<br />" />
					<asp:CheckBox ID="txtNews" visible="false" Runat="server" text="Show Topic In Featured Topics Box?<br />" />
					<br />
					<asp:Textbox ID="txtForumID" visible="false" runat="server" />
					<asp:Textbox ID="txtCategoryID" visible="false" runat="server" />
					<asp:Textbox ID="txtAuthor" visible="false" runat="server" />
					<asp:Button type="submit" id="SubmitButton" onclick="SubmitTopic" OnClientClick="javascript:HideButton()" text="Submit Topic" runat="server" />&nbsp;
					<asp:Button type="submit" id="SubmitPreview" onclick="PreviewTopic" text="Preview Topic" runat="server" />
					<asp:Label id="SubmitConfirm" text="Please Wait..." style="display: none;" runat="server" />
				</font>
			</td>
		</tr>
		</table>
	</td>
	</tr>
	</table>
	<br />

		<asp:PlaceHolder id="TopicPreview" visible="false" runat="server">
			<table width="97%" align="center" class="ContentBox" cellpadding="8" cellspacing="0">
			<tr class="HeaderCell">
			<td colspan="2" align="left">
				<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b><%= PrevSubject %></b></font>
			</td>
			</tr>
			<tr class="TableRow1">
			<td width="100%" valign="top" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;">
				<table border="0" cellpadding="0" cellspacing="1" width="100%">
				<tr>
					<td align="left" valign="bottom" nowrap>
						<font size="1" color="<%=Settings.TopicsFontColor%>">
						Posted: <%= PrevDate %>
						</font>
					</td>
				</tr>
				<tr>
					<td width="100%"><hr clear="all" size="1"></td>
				</tr>
				<tr>
					<td align="left" valign="top" width="100%">
						<font size="<%=Settings.TopicsFontSize%>" color="<%=Settings.TopicsFontColor%>">
							<%= PrevMessage %>
							<%= PrevSignature %>
						</font>
					</td>
				</tr>
				</table>
			</td>
			</tr>
			</table>
			<br />
		</asp:PlaceHolder>

	</asp:Panel>

	<div align="center" id="NoItemsDiv" class="MessageBox" runat="server" />

	<DMG:Footer id="DMGFooter" runat="server" />

		</form>
	</BODY>
</HTML>