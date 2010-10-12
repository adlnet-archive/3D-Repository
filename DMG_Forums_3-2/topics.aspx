<%@ Page language="VB" Inherits="DMGForums.Topics.TopicsPage" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>
<%@ Import Namespace="DMGForums.Global.Functions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings id="DMGSettings" runat="server" />

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
	<DMG:Login id="DMGLogin" runat="server" />

	<asp:Panel ID="PagePanel" runat="server">

	<table border="0" width="97%" align="center" cellpadding="5" cellspacing="0">
	<tr>
	<td align="left" valign="bottom" width="100%">
		<font size="2" color="<%=Settings.FontColor%>">
		<a href="default.aspx"><%=Settings.PageTitle%></a> >> <a href="ForumHome.aspx?ID=<%=CategoryID%>"><%=CategoryName%></a> >> <a href="forums.aspx?ID=<%=ForumID%>"><%=ForumName%></a> >> <%=TopicSubject%>
		</font>
	</td>
	<asp:Panel id="PagingPanel2" runat="server">
		<td width="100%" align="center" valign="bottom" nowrap>
			<font size="2" color="<%=Settings.FontColor%>">
			<b>Page <asp:DropDownList runat="server" ID="JumpPage2" AutoPostBack="true" OnSelectedIndexChanged="ChangePage2"  /> of <asp:Label runat="server" ID="PageCountLabel2" EnableViewState="true" /></b>
			</font>
		</td>
	</asp:Panel>
	<asp:Panel id="ReplyButton" runat="server">
		<td width="100%" align="right" valign="bottom" nowrap>
			<font size="3"><b>
			<a href="newreply.aspx?ID=<%=TopicID%>">Reply To Topic</a>
			</b></font>
		</td>
	</asp:Panel>
	</tr>
	</table>


	<table width="97%" align="center" class="ContentBox" cellpadding="8" cellspacing="0">
	<tr class="HeaderCell">
	<td colspan="2" align="left">
		<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b><%= TopicSubject %></b></font>
	</td>
	</tr>
	<tr class="SubHeaderCell">
	<td width="205" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Author</b></font></td>
	<td width="100%" align="center"><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Topic</b></font></td>
	</tr>
	
	<asp:Repeater id="Topic" runat="server">
		<ItemTemplate>
			<tr class="TableRow1">
			<td width="205" align="center" valign="top" style="border-top:1px solid <%=Settings.TableBorderColor%>;" nowrap>
				<font size="3" color="<%=Settings.TopicsFontColor%>"><b>
					<center>
					<%# IIF(DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL") = -1, "Deleted Member", "<a href=""profile.aspx?ID=" & DataBinder.Eval(Container.DataItem, "TOPIC_AUTHOR") & """>" & DataBinder.Eval(Container.DataItem, "MEMBER_USERNAME") & "</a>") %>
					</center>
				</b></font>
				<table border="0"><tr><td>
					<font size="<%=Settings.TopicsFontSize%>" color="<%=Settings.TopicsFontColor%>">
						<%# PosterDetails(DataBinder.Eval(Container.DataItem, "MEMBER_ID"), DataBinder.Eval(Container.DataItem, "MEMBER_LOCATION"), DataBinder.Eval(Container.DataItem, "MEMBER_POSTS"), DataBinder.Eval(Container.DataItem, "MEMBER_DATE_JOINED"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_USECUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_ALLOWCUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_CUSTOMLOADED"), DataBinder.Eval(Container.DataItem, "MEMBER_TITLE_USECUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_TITLE_ALLOWCUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_TITLE"), DataBinder.Eval(Container.DataItem, "AVATAR_IMAGE"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_CUSTOMTYPE"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_SHOW"), DataBinder.Eval(Container.DataItem, "MEMBER_PHOTO"), DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL"), DataBinder.Eval(Container.DataItem, "MEMBER_RANKING"), 1) %>
					</font>
				</td></tr></table>
				<br />
				<center><asp:button id="PMButton" onclick="SendPM" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "MEMBER_ID") %>' CssClass="dmgbuttons" visible='<%# IIF(((Session("UserLogged") = "1") and (DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL") <> -1) and (DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL") <> 0)), "True", "False") %>' runat="server" Text="SEND PM" /></center>
			</td>
			<td width="100%" valign="top" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;">
				<table border="0" cellpadding="0" cellspacing="1" width="100%">
				<tr>
					<td align="left" valign="bottom" nowrap>
						<font size="1" color="<%=Settings.TopicsFontColor%>">
						Posted: <%# FormatDate(DataBinder.Eval(Container.DataItem, "TOPIC_DATE"), 3) %>
						</font>
					</td>
					<td width="100%" align="right" valign="middle">
						<nobr>
						<asp:button id="ConfirmTopicButton" onclick="ConfirmTopic" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TOPIC_ID") %>' CssClass="dmgbuttons" visible='<%# IIF(((AllowModeration) and (DataBinder.Eval(Container.DataItem, "TOPIC_CONFIRMED") = 0)), "True", "False") %>' runat="server" Text="CONFIRM" />
						<asp:button id="SubscribeButton" onclick="SubscribeTopic" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TOPIC_ID") %>' CssClass="dmgbuttons" visible='<%# IIF(((Session("UserLogged") = "1") and ((Settings.AllowSub = 1) or (Session("UserLevel") = "3"))), "True", "False") %>' runat="server" Text="SUBSCRIBE" />
						<asp:button id="ReportTopicButton" onclick="ReportTopic" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TOPIC_ID") %>' CssClass="dmgbuttons" visible='<%# IIF(((Session("UserLevel") = "1") and (Settings.AllowReporting = 1)), "True", "False") %>' runat="server" Text="REPORT ABUSE" />
						<asp:button id="QuoteButton" onclick="QuoteTopic" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TOPIC_ID") %>' CssClass="dmgbuttons" visible='<%# IIF(((Session("UserLogged") = "1") and (ShowQuoteButton = 1)), "True", "False") %>' runat="server" Text="QUOTE" /> 
						<asp:button id="EditTopicButton" onclick="EditTopic" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TOPIC_ID") %>' CssClass="dmgbuttons" visible='<%# IIF((((DataBinder.Eval(Container.DataItem, "TOPIC_AUTHOR") = Session("UserID")) and (TopicStatusSave <> 2) and (Settings.AllowEdits = 1)) or ((AllowModeration) and (Session("UserLevel") > DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL"))) or (Session("UserLevel") = "3")), "True", "False") %>' runat="server" Text="EDIT" /> 
						<asp:button id="DeleteTopicButton" onclick="DeleteTopic" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TOPIC_ID") %>' CssClass="dmgbuttons" visible='<%# IIF((((AllowModeration) and (Session("UserLevel") > DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL"))) or (Session("UserLevel") = "3")), "True", "False") %>' runat="server" Text="DELETE" />
						<asp:button id="BanMemberButton" onclick="BanMemberConfirm" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TOPIC_AUTHOR") %>' CssClass="dmgbuttons" visible='<%# IIF(AllowModeration and (Session("UserLevel") > DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL")) and (DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL") <> -1), "True", "False") %>' runat="server" Text="BAN AUTHOR" />
						</nobr>
					</td>
				</tr>
				<tr>
					<td colspan="2" width="100%"><hr clear="all" size="1"></td>
				</tr>
				<tr>
					<td colspan="2" align="left" valign="top" width="100%">
						<font size="<%=Settings.TopicsFontSize%>" color="<%=Settings.TopicsFontColor%>">
							<%# FormatString(DataBinder.Eval(Container.DataItem, "TOPIC_MESSAGE")) %>
							<%# IIF((DataBinder.Eval(Container.DataItem, "TOPIC_FILEUPLOAD") = ""), "", ShowTopicFileUpload(DataBinder.Eval(Container.DataItem, "TOPIC_FILEUPLOAD"))) %>
							<%# IIF((DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL") = -1), "", Signature(DataBinder.Eval(Container.DataItem, "TOPIC_SIGNATURE"), DataBinder.Eval(Container.DataItem, "MEMBER_SIGNATURE"))) %>
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
			<td width="205" align="center" valign="top" style="border-top:1px solid <%=Settings.TableBorderColor%>;" nowrap>
				<font size="3" color="<%=Settings.TopicsFontColor%>"><b>
					<center>
					<%# IIF(DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL") = -1, "Deleted Member", "<a href=""profile.aspx?ID=" & DataBinder.Eval(Container.DataItem, "REPLY_AUTHOR") & """>" & DataBinder.Eval(Container.DataItem, "MEMBER_USERNAME") & "</a>") %>
					</center>
				</b></font>
				<table border="0"><tr><td>
					<font size="<%=Settings.TopicsFontSize%>" color="<%=Settings.TopicsFontColor%>">
						<%# PosterDetails(DataBinder.Eval(Container.DataItem, "MEMBER_ID"), DataBinder.Eval(Container.DataItem, "MEMBER_LOCATION"), DataBinder.Eval(Container.DataItem, "MEMBER_POSTS"), DataBinder.Eval(Container.DataItem, "MEMBER_DATE_JOINED"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_USECUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_ALLOWCUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_CUSTOMLOADED"), DataBinder.Eval(Container.DataItem, "MEMBER_TITLE_USECUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_TITLE_ALLOWCUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_TITLE"), DataBinder.Eval(Container.DataItem, "AVATAR_IMAGE"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_CUSTOMTYPE"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_SHOW"), DataBinder.Eval(Container.DataItem, "MEMBER_PHOTO"), DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL"), DataBinder.Eval(Container.DataItem, "MEMBER_RANKING"), 1) %>
					</font>
				</td></tr></table>
				<br />
				<center><asp:button id="PMButton" onclick="SendPM" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "MEMBER_ID") %>' CssClass="dmgbuttons" visible='<%# IIF(((Session("UserLogged") = "1") and (DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL") <> -1) and (DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL") <> 0)), "True", "False") %>' runat="server" Text="SEND PM" /></center>
			</td>
			<td width="100%" valign="top" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;">
				<table border="0" cellpadding="0" cellspacing="1" width="100%">
				<tr>
					<%# IIF(((AllowModeration) and (DataBinder.Eval(Container.DataItem, "REPLY_CONFIRMED") = 0)), "<tr><td colspan=""2"" align=""center""><font size=""4""><b>Reply Not Confirmed</b></font></td></tr>", "") %>
					<td align="left" valign="bottom" nowrap>
						<font size="1" color="<%=Settings.TopicsFontColor%>">
						<a name="reply-<%# DataBinder.Eval(Container.DataItem, "REPLY_ID") %>">Posted: <%# FormatDate(DataBinder.Eval(Container.DataItem, "REPLY_DATE"), 3) %></a>
						</font>
					</td>
					<td width="100%" align="right" valign="middle">
						<nobr>
						<asp:button id="ConfirmReplyButton" onclick="ConfirmReply" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "REPLY_ID") %>' CssClass="dmgbuttons" visible='<%# IIF(((AllowModeration) and (DataBinder.Eval(Container.DataItem, "REPLY_CONFIRMED") = 0)), "True", "False") %>' runat="server" Text="CONFIRM" />
						<asp:button id="QuoteButton" onclick="QuoteReply" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "REPLY_ID") %>' CssClass="dmgbuttons" visible='<%# IIF(((Session("UserLogged") = "1") and (ShowQuoteButton = 1)), "True", "False") %>' runat="server" Text="QUOTE" /> 
						<asp:button id="EditReplyButton" onclick="EditReply" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "REPLY_ID") %>' CssClass="dmgbuttons" visible='<%# IIF((((DataBinder.Eval(Container.DataItem, "REPLY_AUTHOR") = Session("UserID")) and (TopicStatusSave <> 2) and (Settings.AllowEdits = 1)) or (AllowModeration) and (Session("UserLevel") > DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL")) or (Session("UserLevel") = "3")), "True", "False") %>' runat="server" Text="EDIT" />
						<asp:button id="DeleteReplyButton" onclick="DeleteReply" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "REPLY_ID") %>' CssClass="dmgbuttons" visible='<%# IIF(((AllowModeration) and (Session("UserLevel") > DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL")) or (Session("UserLevel") = "3")), "True", "False") %>' runat="server" Text="DELETE" />
						<asp:button id="BanMemberButton" onclick="BanMemberConfirm" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "REPLY_AUTHOR") %>' CssClass="dmgbuttons" visible='<%# IIF(AllowModeration and (Session("UserLevel") > DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL")) and (DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL") <> -1), "True", "False") %>' runat="server" Text="BAN AUTHOR" />
						</nobr>
					</td>
				</tr>
				<tr>
					<td colspan="2" width="100%"><hr clear="all" size="1"></td>
				</tr>
				<tr>
					<td colspan="2" align="left" valign="top" width="100%">
						<font size="<%=Settings.TopicsFontSize%>" color="<%=Settings.TopicsFontColor%>">
						<%# FormatString(DataBinder.Eval(Container.DataItem, "REPLY_MESSAGE")) %>
						<%# IIF((DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL") = -1), "", Signature(DataBinder.Eval(Container.DataItem, "REPLY_SIGNATURE"), DataBinder.Eval(Container.DataItem, "MEMBER_SIGNATURE"))) %>
						</font>
					</td>
				</tr>
				</table>
			</td>
			</tr>
		</ItemTemplate>
		<AlternatingItemTemplate>
			<tr class="TableRow1">
			<td width="205" align="center" valign="top" style="border-top:1px solid <%=Settings.TableBorderColor%>;" nowrap>
				<font size="3" color="<%=Settings.TopicsFontColor%>"><b>
					<center>
					<%# IIF(DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL") = -1, "Deleted Member", "<a href=""profile.aspx?ID=" & DataBinder.Eval(Container.DataItem, "REPLY_AUTHOR") & """>" & DataBinder.Eval(Container.DataItem, "MEMBER_USERNAME") & "</a>") %>
					</a>
					</center>
				</b></font>
				<table border="0"><tr><td>
					<font size="<%=Settings.TopicsFontSize%>" color="<%=Settings.TopicsFontColor%>">
						<%# PosterDetails(DataBinder.Eval(Container.DataItem, "MEMBER_ID"), DataBinder.Eval(Container.DataItem, "MEMBER_LOCATION"), DataBinder.Eval(Container.DataItem, "MEMBER_POSTS"), DataBinder.Eval(Container.DataItem, "MEMBER_DATE_JOINED"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_USECUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_ALLOWCUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_CUSTOMLOADED"), DataBinder.Eval(Container.DataItem, "MEMBER_TITLE_USECUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_TITLE_ALLOWCUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_TITLE"), DataBinder.Eval(Container.DataItem, "AVATAR_IMAGE"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_CUSTOMTYPE"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_SHOW"), DataBinder.Eval(Container.DataItem, "MEMBER_PHOTO"), DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL"), DataBinder.Eval(Container.DataItem, "MEMBER_RANKING"), 1) %>
					</font>
				</td></tr></table>
				<br />
				<center><asp:button id="PMButton" onclick="SendPM" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "MEMBER_ID") %>' CssClass="dmgbuttons" visible='<%# IIF(((Session("UserLogged") = "1") and (DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL") <> -1) and (DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL") <> 0)), "True", "False") %>' runat="server" Text="SEND PM" /></center>
			</td>
			<td width="100%" valign="top" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;">
				<table border="0" cellpadding="0" cellspacing="1" width="100%">
				<tr>
					<%# IIF(((AllowModeration) and (DataBinder.Eval(Container.DataItem, "REPLY_CONFIRMED") = 0)), "<tr><td colspan=""2"" align=""center""><font size=""4""><b>Reply Not Confirmed</b></font></td></tr>", "") %>
					<td align="left" valign="bottom" nowrap>
						<font size="1" color="<%=Settings.TopicsFontColor%>">
						<a name="reply-<%# DataBinder.Eval(Container.DataItem, "REPLY_ID") %>">Posted: <%# FormatDate(DataBinder.Eval(Container.DataItem, "REPLY_DATE"), 3) %></a>
						</font>
					</td>
					<td width="100%" align="right" valign="middle">
						<nobr>
						<asp:button id="ConfirmReplyButton" onclick="ConfirmReply" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "REPLY_ID") %>' CssClass="dmgbuttons" visible='<%# IIF(((AllowModeration) and (DataBinder.Eval(Container.DataItem, "REPLY_CONFIRMED") = 0)), "True", "False") %>' runat="server" Text="CONFIRM" />
						<asp:button id="QuoteButton" onclick="QuoteReply" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "REPLY_ID") %>' CssClass="dmgbuttons" visible='<%# IIF(((Session("UserLogged") = "1") and (ShowQuoteButton = 1)), "True", "False") %>' runat="server" Text="QUOTE" /> 
						<asp:button id="EditReplyButton" onclick="EditReply" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "REPLY_ID") %>' CssClass="dmgbuttons" visible='<%# IIF((((DataBinder.Eval(Container.DataItem, "REPLY_AUTHOR") = Session("UserID")) and (TopicStatusSave <> 2) and (Settings.AllowEdits = 1)) or (AllowModeration) and (Session("UserLevel") > DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL")) or (Session("UserLevel") = "3")), "True", "False") %>' runat="server" Text="EDIT" />
						<asp:button id="DeleteReplyButton" onclick="DeleteReply" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "REPLY_ID") %>' CssClass="dmgbuttons" visible='<%# IIF(((AllowModeration) and (Session("UserLevel") > DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL")) or (Session("UserLevel") = "3")), "True", "False") %>' runat="server" Text="DELETE" />
						<asp:button id="BanMemberButton" onclick="BanMemberConfirm" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "REPLY_AUTHOR") %>' CssClass="dmgbuttons" visible='<%# IIF(AllowModeration and (Session("UserLevel") > DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL")) and (DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL") <> -1), "True", "False") %>' runat="server" Text="BAN AUTHOR" />
						</nobr>
					</td>
				</tr>
				<tr>
					<td colspan="2" width="100%"><hr clear="all" size="1"></td>
				</tr>
				<tr>
					<td colspan="2" align="left" valign="top" width="100%">
						<font size="<%=Settings.TopicsFontSize%>" color="<%=Settings.TopicsFontColor%>">
						<%# FormatString(DataBinder.Eval(Container.DataItem, "REPLY_MESSAGE")) %>
						<%# IIF((DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL") = -1), "", Signature(DataBinder.Eval(Container.DataItem, "REPLY_SIGNATURE"), DataBinder.Eval(Container.DataItem, "MEMBER_SIGNATURE"))) %>
						</font>
					</td>
				</tr>
				</table>
			</td>
			</tr>
		</AlternatingItemTemplate>
	</asp:Repeater>

	</table>

		<asp:Panel id="PagingPanel" runat="server">
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
		</asp:Panel>

	<asp:Panel ID="QuickReply" runat="server">
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
				<asp:Textbox id="txtForumID" runat="server" visible="false" />
				<asp:Textbox id="txtTopicID" runat="server" visible="false" />
				<asp:Textbox id="txtAuthor" runat="server" visible="false" />

				<asp:Textbox id="txtPosts" runat="server" visible="false" />
				<asp:Textbox id="txtReplyMessage" Textmode="Multiline" Columns="65" Rows="10" runat="server" /><br/>
				<asp:CheckBox ID="txtSignature" Runat="server" text="Show Profile Signature?<br />" />
				<br />
				<center>
					<asp:Button type="submit" id="SubmitButton" onclick="SubmitReply" OnClientClick="javascript:HideButton()" text="Submit Reply" runat="server" />
					<asp:Label id="SubmitConfirm" text="Please Wait..." style="display: none;" runat="server" />
				</center>
				</font>
			</td>
		</tr>
		</table>
	</asp:Panel>

	</asp:Panel>

	<br />

	<asp:Panel id="ConfirmTopicForm" visible="false" runat="server">
		<table border="0" width="97%" align="center" cellpadding="5" cellspacing="0">
		<tr>
		<td width="100%" align="center" valign="bottom">
			<font size="2" color="<%=Settings.FontColor%>">Are you sure you want to confirm this topic and make it visible to the public?</font><br /><br />
			<asp:DropDownList id="ConfirmTopicDropdown" runat="server">
				<asp:ListItem Selected="True" Value="1" Text="Yes" />
				<asp:ListItem Value="0" Text="No" />
			</asp:DropDownList>
			<br /><br />
			<asp:Button id="ConfirmTopicSubmitButton" onclick="ApplyTopicConfirmation" runat="server" Text="Submit" />
		</td>
		</tr>
		</table>
		<br />
	</asp:Panel>

	<asp:Panel id="ConfirmReplyForm" visible="false" runat="server">
		<table border="0" width="97%" align="center" cellpadding="5" cellspacing="0">
		<tr>
		<td width="100%" align="center" valign="bottom">
			<font size="2" color="<%=Settings.FontColor%>">Are you sure you want to confirm this reply and make it visible to the public?</font><br /><br />
			<asp:DropDownList id="ConfirmReplyDropdown" runat="server">
				<asp:ListItem Selected="True" Value="1" Text="Yes" />
				<asp:ListItem Value="0" Text="No" />
			</asp:DropDownList>
			<br /><br />
			<asp:Button id="ConfirmReplySubmitButton" onclick="ApplyReplyConfirmation" runat="server" Text="Submit" />
		</td>
		</tr>
		</table>
		<br />
	</asp:Panel>

	<asp:Panel id="ReportTopicForm" visible="false" runat="server">
		<table border="0" width="97%" align="center" cellpadding="5" cellspacing="0">
		<tr>
		<td width="100%" align="center" valign="bottom">
			<font size="2" color="<%=Settings.FontColor%>">Enter a message that will be sent to the admins/moderators along with this alert.</font>
			<br /><br />
			<asp:Textbox id="txtReportTopicMessage" Columns="45" Rows="6" Textmode="multiline" runat="server" />
			<br /><br />
			<asp:Button id="ReportTopicSubmitButton" onclick="ReportTopicConfirmation" runat="server" Text="Submit" />
		</td>
		</tr>
		</table>
		<br />
	</asp:Panel>

	<div align="center" id="NoItemsDiv" class="MessageBox" runat="server" />

	<asp:Panel id="BanMemberPanel" visible="false" runat="server">
		<center>
		<asp:Button type="submit" id="YesButton" onclick="BanMember" text="Yes" runat="server" />
		&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
		<asp:Button type="submit" id="NoButton" onclick="CancelBanMember" text="No" runat="server" />
		</center>
		<br /><br />
	</asp:Panel>

	<DMG:Footer id="DMGFooter" runat="server" />

		</form>
	</BODY>
</HTML>