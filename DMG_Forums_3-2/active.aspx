<%@ Page language="VB" Inherits="DMGForums.Topics.Active" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>
<%@ Import Namespace="DMGForums.Global.Functions" %>
<%@ Import Namespace="System.Data" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="Active Topics" runat="server" />
	</HEAD>
	<BODY>
		<form runat="server">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" runat="server" />

	<asp:Panel ID="PagePanel" runat="server">

	<table border="0" width="97%" align="center" cellpadding="5" cellspacing="0"><tr><td>
		<font size="2" color="<%=Settings.FontColor%>">
		Active Topics Since <%=Session("ActiveTime")%>
		</font>
	</td></tr></table>

	<asp:Repeater id="Forum" runat="server">
		<ItemTemplate>
			<table width="97%" align="center" cellpadding="5" cellspacing="0" class="ContentBox">
			<tr class="HeaderCell">
			<td colspan="5" align="left">
				<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b>
					<a href="default.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "CATEGORY_ID") %>" style="color:<%=Settings.HeaderFontColor%>;"><%# DataBinder.Eval(Container.DataItem, "CATEGORY_NAME") %></a> >> <a href="forums.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "FORUM_ID") %>" style="color:<%=Settings.HeaderFontColor%>;"><%# DataBinder.Eval(Container.DataItem, "FORUM_NAME") %></a>
				</b></font>
			</td>
			</tr>
			<tr class="SubHeaderCell">
			<td width="100%"><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Topic</b></font></td>
			<td width="180" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Author</b></font></td>
			<td width="65" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Replies</b></font></td>
			<td width="65" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Views</b></font></td>
			<td width="180" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Last Post</b></font></td>
			</tr>
				<asp:Repeater id="Topics" DataSource='<%# (CType(Container.DataItem,DataRowView)).Row.GetChildRows("TopicRelation")%>' runat="server">
					<ItemTemplate>
						<tr class="TableRow1">
						<td width="100%" style="border-top:1px solid <%=Settings.TableBorderColor%>;">
							<font size="2" color="<%=Settings.TopicsFontColor%>">
								<%# IIF(DataBinder.Eval(Container.DataItem, "(""TOPIC_STATUS"")") = 2, "<img src=""forumimages/lock.gif"">", "")%>
								<%# IIF(DataBinder.Eval(Container.DataItem, "(""TOPIC_STICKY"")") = 1, "<b>Sticky:</b>&nbsp;", "")%>
								<a href="topics.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "(""TOPIC_ID"")") %>">
								<b><%# DataBinder.Eval(Container.DataItem, "(""TOPIC_SUBJECT"")") %></b>
								</a>
								<%# QuickPaging(DataBinder.Eval(Container.DataItem, "(""TOPIC_ID"")"), DataBinder.Eval(Container.DataItem, "(""TOPIC_REPLIES"")"), Settings.ItemsPerPage) %>
							</font>
						</td>
						<td width="180" align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
							<font size="2">
								<nobr>
								<a href="profile.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "(""TOPIC_AUTHOR"")") %>">
								<%# DataBinder.Eval(Container.DataItem, "(""TOPIC_AUTHOR_NAME"")") %>
								</a>
								</nobr>
							</font>
						</td>
						<td width="65" align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
							<font size="2" color="<%=Settings.TopicsFontColor%>">
								<%# DataBinder.Eval(Container.DataItem, "(""TOPIC_REPLIES"")") %>
							</font>
						</td>
						<td width="65" align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
							<font size="2" color="<%=Settings.TopicsFontColor%>">
								<%# DataBinder.Eval(Container.DataItem, "(""TOPIC_VIEWS"")") %>
							</font>
						</td>
						<td width="180" align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
							<font size="1" color="<%=Settings.TopicsFontColor%>">
								<%# LastTopicBy(DataBinder.Eval(Container.DataItem, "(""TOPIC_LASTPOST_AUTHOR"")"), DataBinder.Eval(Container.DataItem, "(""TOPIC_LASTPOST_NAME"")"), DataBinder.Eval(Container.DataItem, "(""TOPIC_LASTPOST_DATE"")"))%>
							</font>
						</td>
						</tr>
					</ItemTemplate>
				</asp:Repeater>
			</table>
			<br />
		</ItemTemplate>
	</asp:Repeater>

	</asp:Panel>

	<div align="center" id="NoItemsDiv" class="MessageBox" runat="server" />

	<DMG:Footer id="DMGFooter" runat="server" />

		</form>
	</BODY>
</HTML>