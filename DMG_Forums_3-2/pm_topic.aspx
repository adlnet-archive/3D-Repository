<%@ Page language="VB" Inherits="DMGForums.Topics.PMTopic" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>
<%@ Import Namespace="DMGForums.Global.Functions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings runat="server" />

		<script type="text/javascript">
		<!--
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
		<form runat="server">

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

		<asp:PlaceHolder id="PagingPanel" runat="server">
			<table border="0" cellpadding="8" cellspacing="0">
			<tr>
			<td width="150" align="right" nowrap>
				<font size="2"><b>
				<asp:LinkButton runat="server" ID="FirstLink" Text="&laquo; First" onClick="ChangePage" />
				&nbsp;&nbsp;
				<asp:LinkButton runat="server" ID="PreviousLink" Text="&laquo; Previous" onClick="ChangePage" />
				</b></font>
			</td>
			<td align="center" nowrap>
					<font size="2" color="<%=Settings.FontColor%>">
					<b>Page <asp:DropDownList runat="server" ID="JumpPage" AutoPostBack="true" OnSelectedIndexChanged="ChangePage"  /> of <asp:Label runat="server" ID="PageCountLabel" EnableViewState="true" /></b>
					</font>
			</td>
			<td width="150" align="left" nowrap>
				<font size="2"><b>
				<asp:LinkButton runat="server" ID="NextLink" Text="Next &raquo;" onClick="ChangePage" />
				&nbsp;&nbsp;
				<asp:LinkButton runat="server" ID="LastLink" Text="Last &raquo;" onClick="ChangePage" />
				</b></font>
			</td>
			</tr>
			</table>
			<br />
		</asp:PlaceHolder>

		<table width="97%" align="center" style="border-top:2px solid <%=Settings.TableBorderColor%>;border-left:2px solid <%=Settings.TableBorderColor%>;border-right:2px solid <%=Settings.TableBorderColor%>;border-bottom:2px solid <%=Settings.TableBorderColor%>;" cellpadding="8" cellspacing="0">
		<tr class="HeaderCell">
		<td colspan="2" align="left">
			<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b><asp:Label id="TopicSubject" runat="server" /></b>&nbsp;&nbsp;&nbsp;-&nbsp;&nbsp;&nbsp;<asp:Label id="TopicFromTo" runat="server" /></font>
		</td>
		</tr>
		<tr class="SubHeaderCell">
		<td width="150" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Author</b></font></td>
		<td width="100%" align="center"><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Topic</b></font></td>
		</tr>
			<asp:Repeater id="Topic" runat="server">
				<ItemTemplate>
					<tr class="TableRow1">
					<td width="150" valign="top" style="border-top:1px solid <%=Settings.TableBorderColor%>;" nowrap>
						<font size="3"><b>
							<center>
							<a href="profile.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "TOPIC_FROM") %>">
							<%# DataBinder.Eval(Container.DataItem, "MEMBER_USERNAME") %>
							</a>
							</center>
						</b></font>
						<font size="<%=Settings.TopicsFontSize%>" color="<%=Settings.TopicsFontColor%>">
							<%# PosterDetails(DataBinder.Eval(Container.DataItem, "MEMBER_ID"), DataBinder.Eval(Container.DataItem, "MEMBER_LOCATION"), DataBinder.Eval(Container.DataItem, "MEMBER_POSTS"), DataBinder.Eval(Container.DataItem, "MEMBER_DATE_JOINED"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_USECUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_ALLOWCUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_CUSTOMLOADED"), DataBinder.Eval(Container.DataItem, "MEMBER_TITLE_USECUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_TITLE_ALLOWCUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_TITLE"), DataBinder.Eval(Container.DataItem, "AVATAR_IMAGE"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_CUSTOMTYPE"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_SHOW"), DataBinder.Eval(Container.DataItem, "MEMBER_PHOTO"), DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL"), DataBinder.Eval(Container.DataItem, "MEMBER_RANKING"), 1) %>
						</font>
					</td>
					<td width="100%" valign="top" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;">
						<table border="0" cellpadding="0" cellspacing="1" width="100%">
						<tr>
							<td align="left" valign="bottom">
								<font size="1" color="<%=Settings.TopicsFontColor%>">
								Posted: <%# FormatDate(DataBinder.Eval(Container.DataItem, "TOPIC_DATE"), 3) %>
								</font>
							</td>
						</tr>
						<tr>
							<td width="100%"><hr clear="all" size="1"></td>
						</tr>
						<tr>
							<td colspan="2" align="left" valign="top" width="100%">
								<font size="<%=Settings.TopicsFontSize%>" color="<%=Settings.TopicsFontColor%>">
									<%# FormatString(DataBinder.Eval(Container.DataItem, "TOPIC_MESSAGE")) %>
								</font>
							</td>
						</tr>
						</table>
					</td>
					</tr>
				</ItemTemplate>
			</asp:Repeater>

			<asp:Repeater id="Replies" runat="server">
				<ItemTemplate>
					<tr class="TableRow2">
					<td width="150" valign="top" style="border-top:1px solid <%=Settings.TableBorderColor%>;" nowrap>
						<font size="3"><b>
							<center>
							<a href="profile.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "REPLY_AUTHOR") %>">
							<%# DataBinder.Eval(Container.DataItem, "MEMBER_USERNAME") %>
							</a>
							</center>
						</b></font>
						<font size="<%=Settings.TopicsFontSize%>" color="<%=Settings.TopicsFontColor%>">
							<%# PosterDetails(DataBinder.Eval(Container.DataItem, "MEMBER_ID"), DataBinder.Eval(Container.DataItem, "MEMBER_LOCATION"), DataBinder.Eval(Container.DataItem, "MEMBER_POSTS"), DataBinder.Eval(Container.DataItem, "MEMBER_DATE_JOINED"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_USECUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_ALLOWCUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_CUSTOMLOADED"), DataBinder.Eval(Container.DataItem, "MEMBER_TITLE_USECUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_TITLE_ALLOWCUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_TITLE"), DataBinder.Eval(Container.DataItem, "AVATAR_IMAGE"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_CUSTOMTYPE"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_SHOW"), DataBinder.Eval(Container.DataItem, "MEMBER_PHOTO"), DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL"), DataBinder.Eval(Container.DataItem, "MEMBER_RANKING"), 1) %>
						</font>
					</td>
					<td width="100%" valign="top" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;">
						<table border="0" cellpadding="0" cellspacing="1" width="100%">
						<tr>
							<td align="left" valign="bottom">
								<font size="1" color="<%=Settings.TopicsFontColor%>">
								Posted: <%# FormatDate(DataBinder.Eval(Container.DataItem, "REPLY_DATE"), 3) %>
								</font>
							</td>
						</tr>
						<tr>
							<td width="100%"><hr clear="all" size="1"></td>
						</tr>
						<tr>
							<td colspan="2" align="left" valign="top" width="100%">
								<font size="<%=Settings.TopicsFontSize%>" color="<%=Settings.TopicsFontColor%>">
									<%# FormatString(DataBinder.Eval(Container.DataItem, "REPLY_MESSAGE")) %>
								</font>
							</td>
						</tr>
						</table>
					</td>
					</tr>
				</ItemTemplate>
				<AlternatingItemTemplate>
					<tr class="TableRow1">
					<td width="150" valign="top" style="border-top:1px solid <%=Settings.TableBorderColor%>;" nowrap>
						<font size="3"><b>
							<center>
							<a href="profile.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "REPLY_AUTHOR") %>">
							<%# DataBinder.Eval(Container.DataItem, "MEMBER_USERNAME") %>
							</a>
							</center>
						</b></font>
						<font size="<%=Settings.TopicsFontSize%>" color="<%=Settings.TopicsFontColor%>">
							<%# PosterDetails(DataBinder.Eval(Container.DataItem, "MEMBER_ID"), DataBinder.Eval(Container.DataItem, "MEMBER_LOCATION"), DataBinder.Eval(Container.DataItem, "MEMBER_POSTS"), DataBinder.Eval(Container.DataItem, "MEMBER_DATE_JOINED"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_USECUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_ALLOWCUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_CUSTOMLOADED"), DataBinder.Eval(Container.DataItem, "MEMBER_TITLE_USECUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_TITLE_ALLOWCUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_TITLE"), DataBinder.Eval(Container.DataItem, "AVATAR_IMAGE"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_CUSTOMTYPE"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_SHOW"), DataBinder.Eval(Container.DataItem, "MEMBER_PHOTO"), DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL"), DataBinder.Eval(Container.DataItem, "MEMBER_RANKING"), 1) %>
						</font>
					</td>
					<td width="100%" valign="top" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;">
						<table border="0" cellpadding="0" cellspacing="1" width="100%">
						<tr>
							<td align="left" valign="bottom">
								<font size="1" color="<%=Settings.TopicsFontColor%>">
								Posted: <%# FormatDate(DataBinder.Eval(Container.DataItem, "REPLY_DATE"), 3) %>
								</font>
							</td>
						</tr>
						<tr>
							<td width="100%"><hr clear="all" size="1"></td>
						</tr>
						<tr>
							<td colspan="2" align="left" valign="top" width="100%">
								<font size="<%=Settings.TopicsFontSize%>" color="<%=Settings.TopicsFontColor%>">
									<%# FormatString(DataBinder.Eval(Container.DataItem, "REPLY_MESSAGE")) %>
								</font>
							</td>
						</tr>
						</table>
					</td>
					</tr>
				</AlternatingItemTemplate>
			</asp:Repeater>
		</table>
		<br />

		<asp:PlaceHolder ID="QuickReply" runat="server">
			<br />
			<table align="center" border="0" cellpadding="8">
			<tr>
				<td align="center" valign="middle">
					<font size="2" color="<%=Settings.FontColor%>"><b>
					<a href="javascript:openHelp('DMGcode.html')">Forum Code</a><br />Allowed
					</b></font>
				</td>
				<td align="left" valign="top">
					<font size="2" color="<%=Settings.FontColor%>"><b>Reply Message:</b><br />
					<asp:Textbox id="txtReplyMessage" Textmode="Multiline" Columns="65" Rows="10" runat="server" /><br/>
					<br />
					<center>
						<asp:Button type="submit" id="SubmitButton" onclick="SubmitReply" OnClientClick="javascript:HideButton()" text="Submit Reply" runat="server" />
						<asp:Label id="SubmitConfirm" text="Please Wait..." style="display: none;" runat="server" />
					</center>
					</font>
				</td>
			</tr>
			</table>
		</asp:PlaceHolder>

	</asp:PlaceHolder>

	<div align="center" id="NoItemsDiv" class="MessageBox" runat="server" />

	<DMG:Footer id="DMGFooter" runat="server" />

		</form>
	</BODY>
</HTML>