<%@ Page language="VB" Inherits="DMGForums.Forums.ForumsPage" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>
<%@ Import Namespace="DMGForums.Global.Functions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings id="DMGSettings" runat="server" />
		<%= IIF(Settings.RSSFeeds = 1, "<link rel=""alternate"" type=""application/rss+xml"" title=""" & Settings.PageTitle & " - " & ForumName & " [RSS]"" href=""rssfeed.aspx?ID=" & ForumID & """ />", "") %>
	</HEAD>
	<BODY>
		<form runat="server">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" runat="server" />

	<asp:Panel ID="PagePanel" runat="server">

		<asp:PlaceHolder id="ForumContentPanel" runat="server" >
			<table width="97%" align="center" cellpadding="0" cellspacing="0">
			<tr><td width="100%" align="left">
				<font size="<%=Settings.FontSize%>" color="<%=Settings.FontColor%>">
					<asp:Literal id="ForumContent" runat="server" />
				</font>
			</td></tr>
			</table>
			<br />
		</asp:PlaceHolder>

		<table border="0" width="97%" align="center" cellpadding="5" cellspacing="0">
		<tr>
		<td align="left" valign="bottom" nowrap>
			<font size="2" color="<%=Settings.FontColor%>">
			<a href="default.aspx"><%=Settings.PageTitle%></a> >> <a href="ForumHome.aspx?ID=<%=CategoryID%>"><%=CategoryName%></a> >> <%=ForumName%>
			</font>
		</td>
		<td width="100%" align="right" valign="bottom">
			<% if (ForumStatus = 1 and Session("UserLogged") = "1") or (Session("UserLevel") = "3") then %>
				<font size="3"><b>
				<a href="newtopic.aspx?ID=<%=ForumID%>">New Topic</a>
				</b></font>
			<% end if %>
		</td>
		</tr>
		</table>

	<asp:Repeater id="Topics" runat="server">
		<HeaderTemplate>
			<asp:Label id="theTopicID" visible="false" text="" runat="server" />
			<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
			<tr class="HeaderCell">
			<td colspan="5" align="left">
				<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b><%= ForumName %></b></font>
			</td>
			<td align="right">
				<%# IIF(Settings.RSSFeeds = 1, "<a target=""_blank"" href=""rssfeed.aspx?ID=" & ForumID & """><img src=""forumimages/rss.gif"" border=""0""></a>", "")%>
			</td>
			</tr>
			<tr class="SubHeaderCell">
			<td width="100%"><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Topic</b></font></td>
			<td width="180" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Author</b></font></td>
			<td width="65" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Replies</b></font></td>
			<td width="65" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Views</b></font></td>
			<td width="180" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Last Post</b></font></td>
			<td width="65" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>">&nbsp;</font></td>
			</tr>
			</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr class="TableRow1">
			<td width="100%" style="border-top:1px solid <%=Settings.TableBorderColor%>;">
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(DataBinder.Eval(Container.DataItem, "TOPIC_STATUS") = 2, "<img src=""forumimages/lock.gif"">", "")%>
					<%# IIF(DataBinder.Eval(Container.DataItem, "TOPIC_STATUS") = 0, "<b>[Hidden]</b>&nbsp;", "")%>
					<%# IIF(DataBinder.Eval(Container.DataItem, "TOPIC_STICKY") = 1, "<b>Sticky:</b>&nbsp;", "")%>
					<%# IIF(DataBinder.Eval(Container.DataItem, "TOPIC_CONFIRMED") = 0, "<b>[Not Confirmed]</b>&nbsp;", "")%>
					<%# IIF(((DataBinder.Eval(Container.DataItem, "TOPIC_UNCONFIRMED_REPLIES") > 0) and (AllowModeration)), "<b>[Unconfirmed Replies]</b>&nbsp;", "")%>
					<a href="topics.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "TOPIC_ID") %>">

					<b><%# CurseFilter(DataBinder.Eval(Container.DataItem, "TOPIC_SUBJECT")) %></b>
					</a>
					<%# QuickPaging(DataBinder.Eval(Container.DataItem, "TOPIC_ID"), DataBinder.Eval(Container.DataItem, "TOPIC_REPLIES"), Settings.ItemsPerPage) %>
				</font>
			</td>
			<td width="180" align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
				<font size="2">
					<nobr>
					<a href="profile.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "TOPIC_AUTHOR") %>">
					<%# DataBinder.Eval(Container.DataItem, "TOPIC_AUTHOR_NAME") %>
					</a>
					</nobr>
				</font>
			</td>
			<td width="65" align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					<%# DataBinder.Eval(Container.DataItem, "TOPIC_REPLIES") %>
				</font>
			</td>
			<td width="65" align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					<%# DataBinder.Eval(Container.DataItem, "TOPIC_VIEWS") %>
				</font>
			</td>
			<td width="180" align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
				<font size="1" color="<%=Settings.TopicsFontColor%>">
					<%# LastTopicBy(DataBinder.Eval(Container.DataItem, "TOPIC_LASTPOST_AUTHOR"), DataBinder.Eval(Container.DataItem, "TOPIC_LASTPOST_NAME"), DataBinder.Eval(Container.DataItem, "TOPIC_LASTPOST_DATE"))%>
				</font>
			</td>
			<td width="65" align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
				<table border="0" width="100%">
				<tr><td align="center" valign="middle" width="100%">
					<asp:button id="ConfirmTopicButton" onclick="ConfirmTopic" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TOPIC_ID") %>' CssClass="dmgbuttons" visible='<%# IIF(((AllowModeration) and (DataBinder.Eval(Container.DataItem, "TOPIC_CONFIRMED") = 0)), "True", "False") %>' runat="server" Text="CONFIRM" />
				</td></tr>
				<tr><td align="center" valign="middle" width="100%">
					<nobr>
					<asp:button id="EditTopicButton" onclick="EditTopic" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TOPIC_ID") %>' CssClass="dmgbuttons" visible='<%# IIF((((DataBinder.Eval(Container.DataItem, "TOPIC_AUTHOR") = Session("UserID")) and (DataBinder.Eval(Container.DataItem, "TOPIC_STATUS") <> 2) and (Settings.AllowEdits = 1)) or ((AllowModeration) and (Session("UserLevel") > DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL"))) or (Session("UserLevel") = "3")), "True", "False") %>' runat="server" Text="EDIT" /> 
					<asp:button id="DeleteTopicButton" onclick="DeleteTopic" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TOPIC_ID") %>' CssClass="dmgbuttons" visible='<%# IIF((((AllowModeration) and (Session("UserLevel") > DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL"))) or (Session("UserLevel") = "3")), "True", "False") %>' runat="server" Text="DELETE" />
					</nobr>
				</td></tr>
				<tr><td align="center" valign="middle" width="100%">
					<asp:button id="ReplyButton" onclick="NewReply" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TOPIC_ID") %>' CssClass="dmgbuttons" visible='<%# IIF(((Session("UserLogged") = "1") and (DataBinder.Eval(Container.DataItem, "TOPIC_STATUS") <> 2) and (ForumStatus <> 2)) or (AllowModeration), "True", "False") %>' runat="server" Text="REPLY" />
				</td></tr>
				</table>
			</td>
			</tr>
		</ItemTemplate>
		<FooterTemplate>
			</table>
			<br />
		</FooterTemplate>
	</asp:Repeater>

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

	<asp:Repeater id="ModeratorsList" runat="server">
		<HeaderTemplate>
			<table border="0" width="97%" align="center" cellpadding="5" cellspacing="0">
			<tr>
			<td align="left" valign="bottom">
				<font size="2" color="<%=Settings.FontColor%>"><b>Moderated By:</b> (
		</HeaderTemplate>
		<ItemTemplate>
			&nbsp;-&nbsp;<%# DataBinder.Eval(Container.DataItem, "MEMBER_USERNAME") %>
		</ItemTemplate>
		<FooterTemplate>
				&nbsp;-&nbsp;)</font>
			</td>
			</tr>
			</table>
			<br />
		</FooterTemplate>
	</asp:Repeater>

	<asp:Panel id="ConfirmTopicForm" visible="false" runat="server">
		<table border="0" width="97%" align="center" cellpadding="5" cellspacing="0">
		<tr>
		<td width="100%" align="center" valign="bottom">
			<br /><br />
			<font size="2" color="<%=Settings.FontColor%>">Are you sure you want to confirm this topic and make it visible to the public?</font><br /><br />
			<asp:DropDownList id="ConfirmTopicDropdown" runat="server">
				<asp:ListItem Selected="True" Value="1" Text="Yes" />
				<asp:ListItem Value="0" Text="No" />
			</asp:DropDownList>
			<br /><br />
			<asp:Button id="ConfirmButton" onclick="ApplyConfirmation" runat="server" Text="Submit" />
		</td>
		</tr>
		</table>
		<br />
	</asp:Panel>

	</asp:Panel>

	<asp:Panel id="PasswordPanel" visible="false" runat="server">
		<table border="0" width="97%" align="center" cellpadding="5" cellspacing="0">
		<tr>
		<td width="100%" align="center" valign="bottom">
			<font size="2" color="<%=Settings.FontColor%>">This Forum Requires A Password</font><br /><br />
			<asp:Textbox id="PasswordBox" size="30" maxlength="50" textmode="password" runat="server" /><br />
			<asp:Button id="PasswordButton" onclick="ApplyForumPassword" runat="server" Text="Submit" />
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