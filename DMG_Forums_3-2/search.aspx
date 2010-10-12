<%@ Page language="VB" Inherits="DMGForums.Search" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>
<%@ Import Namespace="DMGForums.Global.Functions" %>
<%@ Import Namespace="System.Data" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="Search" runat="server" />
	</HEAD>
	<BODY>
		<form runat="server">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" runat="server" />

	<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
	<tr class="HeaderCell">
	<td align="left">
		<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b>Search</b></font>
	</td>
	</tr>
	<tr class="TableRow1">
	<td style="border-top:1px solid <%=Settings.TableBorderColor%>;">

		<asp:Panel id="SearchLinks" visible="false" runat="server">
			<br />
			<table border="0" align="center" cellpadding="8">
			<tr>
			<td align="left" valign="top">
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					<asp:PlaceHolder id="TopicSearchPanel" visible="false" runat="server">
						<li /><asp:LinkButton runat="server" ID="TopicSearchLink" CommandArgument="1" Text="Search Topics" onClick="OpenConfig" /><br /><br />
					</asp:PlaceHolder>
					<asp:PlaceHolder id="MemberSearchPanel" visible="false" runat="server">
						<li /><asp:LinkButton runat="server" ID="MemberSearchLink" CommandArgument="2" Text="Search Members" onClick="OpenConfig" /><br /><br />
					</asp:PlaceHolder>
					<asp:PlaceHolder id="BlogSearchPanel" visible="false" runat="server">
						<li /><asp:LinkButton runat="server" ID="BlogSearchLink" CommandArgument="3" Text="Search Blogs" onClick="OpenConfig" /><br /><br />
					</asp:PlaceHolder>
					<asp:PlaceHolder id="PageSearchPanel" visible="false" runat="server">
						<li /><asp:LinkButton runat="server" ID="PageSearchLink" CommandArgument="4" Text="Search Pages" onClick="OpenConfig" /><br /><br />
					</asp:PlaceHolder>
				</font>
			</td>
			</tr>
			</table>
		</asp:Panel>


		<asp:Panel id="TopicSearch" runat="server" visible="false">
			<table border="0" cellpadding="6" align="center">
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Search For:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtTopicString" size="30" maxlength="50" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Forum:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:DropDownList id="ForumDropList" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Search In:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:DropDownList id="SearchInList" runat="server">
						<asp:ListItem Value="0" Text="Entire Message" Selected="True" runat="server" />
						<asp:ListItem Value="1" Text="Subject Only" runat="server" />
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Topic Posted:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:DropDownList id="DateList" runat="server">
						<asp:ListItem Value="999" Text="Any Date" runat="server" />
						<asp:ListItem Value="1" Text="In The Last 1 Day" runat="server" />
						<asp:ListItem Value="7" Text="In The Last 7 Days" runat="server" />
						<asp:ListItem Value="14" Text="In The Last 14 Days" runat="server" />
						<asp:ListItem Value="30" Text="In The Last 30 Days" Selected="True" runat="server" />
						<asp:ListItem Value="60" Text="In The Last 60 Days" runat="server" />
						<asp:ListItem Value="120" Text="In The Last 120 Days" runat="server" />
						<asp:ListItem Value="365" Text="In The Last Year" runat="server" />
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td></td>
				<td align="left" valign="middle">
					<asp:Button type="submit" id="Submit1" onclick="SubmitTopicSearch" text="Search" runat="server" />
				</td>
			</tr>
			</table>
		</asp:Panel>


		<asp:Panel id="BlogSearch" runat="server" visible="false">
			<table border="0" cellpadding="6" align="center">
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Search For:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtBlogSearchString" size="30" maxlength="50" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Search In:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:DropDownList id="BlogSearchInList" runat="server">
						<asp:ListItem Value="0" Text="Entire Blog" Selected="True" runat="server" />
						<asp:ListItem Value="1" Text="Title Only" runat="server" />
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Blog Posted:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:DropDownList id="BlogDateList" runat="server">
						<asp:ListItem Value="999" Text="Any Date" runat="server" />
						<asp:ListItem Value="1" Text="In The Last 1 Day" runat="server" />
						<asp:ListItem Value="7" Text="In The Last 7 Days" runat="server" />
						<asp:ListItem Value="14" Text="In The Last 14 Days" runat="server" />
						<asp:ListItem Value="30" Text="In The Last 30 Days" Selected="True" runat="server" />
						<asp:ListItem Value="60" Text="In The Last 60 Days" runat="server" />
						<asp:ListItem Value="120" Text="In The Last 120 Days" runat="server" />
						<asp:ListItem Value="365" Text="In The Last Year" runat="server" />
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td></td>
				<td align="left" valign="middle">
					<asp:Button type="submit" id="BlogSubmit" onclick="SubmitBlogSearch" text="Search" runat="server" />
				</td>
			</tr>
			</table>
		</asp:Panel>

		<asp:Repeater id="BlogTopics" visible="false" runat="server">
			<HeaderTemplate>
				<hr clear="all" />
				<table border="0" cellpadding="6" align="center">
			</HeaderTemplate>
			<ItemTemplate>
				<tr>
					<td><font color="<%=Settings.TopicsFontColor%>"><%# FormatDate(Databinder.Eval(Container.DataItem, "BLOG_DATE"), 1) %></font></td>
					<td><font color="<%=Settings.TopicsFontColor%>">-</font></td>
					<td><a href="blogs.aspx?ID=<%# Databinder.Eval(Container.DataItem, "BLOG_ID") %>"><b><%# CurseFilter(Databinder.Eval(Container.DataItem, "BLOG_TITLE")) %></b></a></td>
					<td><font size="2" color="<%=Settings.TopicsFontColor%>">(<%# Databinder.Eval(Container.DataItem, "BLOG_REPLIES") %> comments)</font></td>
					<td><font color="<%=Settings.TopicsFontColor%>">-</font></td>
					<td><font color="<%=Settings.TopicsFontColor%>">By <a href="profile.aspx?ID=<%# Databinder.Eval(Container.DataItem, "BLOG_AUTHOR") %>"><%# CurseFilter(Databinder.Eval(Container.DataItem, "MEMBER_USERNAME")) %></a></font></td>
				</tr>
			</ItemTemplate>
			<FooterTemplate>
				</table>
			</FooterTemplate>
		</asp:Repeater>


		<asp:Label id="SearchResults" style="font-size:20px;" runat="server" />


		<asp:Panel id="MemberSearch" runat="server" visible="false">
			<table border="0" cellpadding="6" align="center">
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Search For A Member:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="MemberSearchString" size="30" maxlength="50" runat="server" />
				</td>
				<td align="left" valign="middle">
					<asp:Button type="submit" id="Submit2" onclick="SubmitMemberSearch" text="Search" runat="server" />
				</td>
			</tr>
			</table>
			<asp:Repeater id="MemberList" runat="server">
				<HeaderTemplate>
					<table align="center" border="0"><tr><td>
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Search Results:</b></font>
					<br /><br />
				</HeaderTemplate>
				<ItemTemplate>
					<font size="2">
						<li /><a href="profile.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "MEMBER_ID") %>"><%# DataBinder.Eval(Container.DataItem, "MEMBER_USERNAME") %></a><br /><br />
					</font>
				</ItemTemplate>
				<FooterTemplate>
					</td></tr></table>
				</FooterTemplate>
			</asp:Repeater>
		</asp:Panel>


		<asp:Panel id="PageSearch" runat="server" visible="false">
			<table border="0" cellpadding="6" align="center">
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Search For:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtPageSearchString" size="30" maxlength="50" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Search In:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:DropDownList id="PageSearchInList" runat="server">
						<asp:ListItem Value="0" Text="Entire Page" Selected="True" runat="server" />
						<asp:ListItem Value="1" Text="Title Only" runat="server" />
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td></td>
				<td align="left" valign="middle">
					<asp:Button type="submit" id="PageSubmit" onclick="SubmitPageSearch" text="Search" runat="server" />
				</td>
			</tr>
			</table>
		</asp:Panel>

		<asp:Repeater id="PageSearchResults" visible="false" runat="server">
			<HeaderTemplate>
				<hr clear="all" />
			</HeaderTemplate>
			<ItemTemplate>
				<br />
				<font color="<%=Settings.TopicsFontColor%>">
				<a href="page.aspx?ID=<%# Databinder.Eval(Container.DataItem, "PAGE_ID") %>"><b><%# Databinder.Eval(Container.DataItem, "PAGE_NAME") %></b></a><br />
				<%# LeftText(RepairString(Databinder.Eval(Container.DataItem, "PAGE_CONTENT")), 200) %>
				<br /><br />
				</font>
			</ItemTemplate>
		</asp:Repeater>


	</td>
	</tr>
	</table>
	<br />


	<asp:Repeater id="Forum" visible="false" runat="server">
		<ItemTemplate>
			<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
			<tr class="HeaderCell">
			<td colspan="5" align="left">
				<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b>
					<a href="ForumHome.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "CATEGORY_ID") %>" style="color:<%=Settings.HeaderFontColor%>;"><%# DataBinder.Eval(Container.DataItem, "CATEGORY_NAME") %></a> >> <a href="forums.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "FORUM_ID") %>" style="color:<%=Settings.HeaderFontColor%>;"><%# DataBinder.Eval(Container.DataItem, "FORUM_NAME") %></a>
				</b></font>
			</td>
			</tr>
			<tr class="SubHeaderCell">
			<td width="100%"><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Topic</b></font></td>
			<td width="150" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Author</b></font></td>
			<td width="65" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Replies</b></font></td>
			<td width="65" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Views</b></font></td>
			<td width="150" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Last Post</b></font></td>
			</tr>
				<asp:Repeater id="Topics" DataSource='<%# (CType(Container.DataItem,DataRowView)).Row.GetChildRows("TopicRelation")%>' runat="server">
					<ItemTemplate>
						<tr class="TableRow1">
						<td width="100%" style="border-top:1px solid <%=Settings.TableBorderColor%>;">
							<font size="2" color="<%=Settings.TopicsFontColor%>">
								<%# IIF(DataBinder.Eval(Container.DataItem, "(""TOPIC_STATUS"")") = 2, "<img src=""forumimages/lock.gif"">", "")%>
								<%# IIF(DataBinder.Eval(Container.DataItem, "(""TOPIC_STICKY"")") = 1, "<b>Sticky:</b>&nbsp;", "")%>
								<a href="topics.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "(""TOPIC_ID"")") %>">
								<%# CurseFilter(DataBinder.Eval(Container.DataItem, "(""TOPIC_SUBJECT"")")) %>
								</a>
							</font>
						</td>
						<td align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
							<font size="2">
								<nobr>
								<a href="profile.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "(""TOPIC_AUTHOR"")") %>">
								<%# DataBinder.Eval(Container.DataItem, "(""TOPIC_AUTHOR_NAME"")") %>
								</a>
								</nobr>
							</font>
						</td>
						<td align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
							<font size="2" color="<%=Settings.TopicsFontColor%>">
								<%# DataBinder.Eval(Container.DataItem, "(""TOPIC_REPLIES"")") %>
							</font>
						</td>
						<td align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
							<font size="2" color="<%=Settings.TopicsFontColor%>">
								<%# DataBinder.Eval(Container.DataItem, "(""TOPIC_VIEWS"")") %>
							</font>
						</td>
						<td align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
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

	<div align="center" id="NoItemsDiv" class="MessageBox" runat="server" />

	<DMG:Footer id="DMGFooter" runat="server" />

		</form>
	</BODY>
</HTML>