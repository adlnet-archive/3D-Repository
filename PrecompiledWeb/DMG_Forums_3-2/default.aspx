<%@ Page language="VB" Inherits="DMGForums.DefaultPage" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>
<%@ Import Namespace="DMGForums.Global.Functions" %>
<%@ Import Namespace="System.Data" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD runat="server">
		<DMG:Settings runat="server" />
	</HEAD>
	<BODY>
		<form runat="server">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" runat="server" />

	<asp:Panel ID="PagePanel" visible="false" runat="server">
		<table width="97%" align="center" cellpadding="0" cellspacing="0">
		<tr><td width="100%" align="left">

			<asp:Label id="PagePhoto" runat="server" />

			<font size="<%=Settings.HeaderSize%>" color="<%=Settings.FontColor%>"><b>
				<asp:Label id="PageTitle" runat="server" />
			</b></font>

			<font size="<%=Settings.FontSize%>" color="<%=Settings.FontColor%>">
				<asp:Label id="PageContent" runat="server" />
			</font>

			<asp:DataList id="SubCategories" runat="server" AutoGenerateColumns="False" RepeatDirection="Horizontal" Cellpadding="15" ItemStyle-VerticalAlign="Bottom">
				<HeaderTemplate>
					<font color="<%=Settings.FontColor%>">
						<%=SubTitleText%>
					</font>
				</HeaderTemplate>
				<ItemTemplate>
					<font size="3">
					<a href="page.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "PAGE_ID") %>"><%# IIF(DataBinder.Eval(Container.DataItem, "PAGE_THUMBNAIL") <> "", "<img border=""0"" src=""pageimages/" & DataBinder.Eval(Container.DataItem, "PAGE_THUMBNAIL") & """><br />", "") %><%# DataBinder.Eval(Container.DataItem, "PAGE_NAME") %></a>
					</font>
				</ItemTemplate>
			</asp:DataList>

		</td></tr>
		</table>
		<br />
	</asp:Panel>

	<asp:Panel ID="ForumPanel" visible="false" runat="server">
	
	<table border="0" width="97%" align="center" cellpadding="5" cellspacing="0">
	<tr>
	<td align="left" valign="bottom" nowrap>
		<% if CustomCategory = "Yes" then %>
			<font size="2" color="<%=Settings.FontColor%>">
			<a href="default.aspx"><%=Settings.PageTitle%></a> >> <%=CategoryName%>
			</font>
		<% end if %>
	</td>
	<td width="100%" align="right" valign="bottom">
		<% if (Session("UserLogged") = "1") and (Session("UserLevel") = "3") then %>
			<font size="2"><b>
			<a href="newcategory.aspx">New Category</a>
			</b></font>
		<% end if %>
	</td>
	</tr>
	</table>

	<asp:Repeater id="Category" runat="server">
		<ItemTemplate>
			<table width="97%" align="center" cellpadding="5" cellspacing="0" class="ContentBox">
			<tr class="HeaderCell">
			<td colspan="5" align="left">
				<table border="0" width="100%">
				<tr>
				<td align="left" valign="middle" nowrap>
				<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b>
					<a href="ForumHome.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "CATEGORY_ID") %>" style="color:<%=Settings.HeaderFontColor%>;">
						<%# DataBinder.Eval(Container.DataItem, "CATEGORY_NAME") %>
					</a>
				</b></font>
				</td>
				<% if Session("UserLevel") = "3" then %>
					<td width="100%" align="right">
						<asp:button id="EditCatButton" onclick="EditCategory" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CATEGORY_ID") %>' CssClass="dmgbuttons" runat="server" Text="EDIT" />&nbsp;
						<asp:button id="DeleteCatButton" onclick="DeleteCategory" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CATEGORY_ID") %>' CssClass="dmgbuttons" runat="server" Text="DELETE" />&nbsp;
						<asp:button id="AddForumButton" onclick="AddForum" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CATEGORY_ID") %>' CssClass="dmgbuttons" runat="server" Text="NEW FORUM" />
					</td>
				<% end if %>
				</tr>
				</table>
			</td>
			</tr>
			<tr class="SubHeaderCell">
			<td width="100%"><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Forum</b></font></td>
			<td width="180" align="center"><font size="1" color="<%=Settings.SubHeaderFontColor%>" nowrap><b>Last Post</b></font></td>
			<td width="65" align="center"><font size="1" color="<%=Settings.SubHeaderFontColor%>" nowrap><b>Topics</b></font></td>
			<td width="65" align="center"><font size="1" color="<%=Settings.SubHeaderFontColor%>" nowrap><b>Posts</b></font></td>
			<td width="65" align="center"><font size="1" color="<%=Settings.SubHeaderFontColor%>" nowrap>&nbsp;</font></td>
			</tr>
				<asp:Repeater id="Forum" DataSource='<%# (CType(Container.DataItem,DataRowView)).Row.GetChildRows("CategoryRelation")%>' runat="server">
					<ItemTemplate>
						<asp:PlaceHolder id="ShowForumPanel" visible='<%#IsPrivileged(DataBinder.Eval(Container.DataItem, "(""FORUM_ID"")"), DataBinder.Eval(Container.DataItem, "(""FORUM_TYPE"")"), HttpContext.Current.Session("UserID"), HttpContext.Current.Session("UserLevel"), HttpContext.Current.Session("UserLogged")).ToString()%>' runat="server">
						<tr class="TableRow1">
							<td width="100%" style="border-top:1px solid <%=Settings.TableBorderColor%>">
								<font size="2">
									<%# IIF(DataBinder.Eval(Container.DataItem, "(""FORUM_STATUS"")") = 2, "<img src=""forumimages/lock.gif"">", "")%>
									<a href="forums.aspx?id=<%# DataBinder.Eval(Container.DataItem, "(""FORUM_ID"")") %>">
									<%# DataBinder.Eval(Container.DataItem, "(""FORUM_NAME"")") %>
									</a><br />
								</font>
								<font size="1" color="<%=Settings.TopicsFontColor%>">
									<%# FormatString(DataBinder.Eval(Container.DataItem, "(""FORUM_DESCRIPTION"")")) %>
								</font>
							</td>
							<td width="180" align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
								<font size="1" color="<%=Settings.TopicsFontColor%>">
								<%# LastTopicBy(DataBinder.Eval(Container.DataItem, "(""MEMBER_ID"")"), DataBinder.Eval(Container.DataItem, "(""MEMBER_USERNAME"")"), DataBinder.Eval(Container.DataItem, "(""FORUM_LASTPOST_DATE"")"))%>
								</font>
							</td>
							<td width="65" align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
								<font size="2" color="<%=Settings.TopicsFontColor%>">
								<%# DataBinder.Eval(Container.DataItem, "(""FORUM_TOPICS"")") %>
								</font>
							</td>
							<td width="65" align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
								<font size="2" color="<%=Settings.TopicsFontColor%>">
								<%# DataBinder.Eval(Container.DataItem, "(""FORUM_POSTS"")") %>
								</font>
							</td>
							<td width="65" align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
							<table border="0" width="100%">
							<% if Session("UserLevel") = "3" then %>
							<tr><td align="center" valign="middle" width="100%">
								<nobr>
								<asp:button id="EditForumButton" onclick="EditForum" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "(""FORUM_ID"")") %>' CssClass="dmgbuttons" runat="server" Text="EDIT" />&nbsp;
								<asp:button id="DeleteForumButton" onclick="DeleteForum" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "(""FORUM_ID"")") %>' CssClass="dmgbuttons" runat="server" Text="DELETE" />
								</nobr>
							</td></tr>
							<% end if %>
							<tr><td align="center" valign="middle" width="100%">
							<asp:button id="NewTopicButton" onclick="NewTopic" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "(""FORUM_ID"")") %>' Visible='<%# IIF(((DataBinder.Eval(Container.DataItem, "(""FORUM_STATUS"")") = 1) and Session("UserLogged") = "1") or (Session("UserLevel") = "3"), "True", "False")%>' CssClass="dmgbuttons" runat="server" Text="NEW TOPIC" />
							</td></tr>
							</table>
							</td>
						</tr>
						</asp:PlaceHolder>
					</ItemTemplate>
				</asp:Repeater>
			</table>
			<br />
		</ItemTemplate>
	</asp:Repeater>

		<asp:PlaceHolder id="StatsPanel" visible="false" runat="server">
			<table width="97%" align="center" cellpadding="5" cellspacing="0" class="ContentBox">
			<tr class="HeaderCell">
			<td align="left">
				<table border="0" width="100%">
				<tr>
				<td align="left" valign="middle">
				<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b>
					Statistics
				</b></font>
				</td>
				</tr>
				</table>
			</td>
			</tr>
			<tr class="TableRow1">
				<td style="border-bottom:1px solid <%=Settings.TableBorderColor%>">
					<font size="2" color="<%=Settings.TopicsFontColor%>">
						<asp:Label id="MembersText" runat="server" />
					</font>
				</td>
			</tr>
			<tr class="TableRow1">
				<td style="border-bottom:1px solid <%=Settings.TableBorderColor%>">
					<font size="2" color="<%=Settings.TopicsFontColor%>">
						<asp:Label id="NewMemberText" runat="server" />
					</font>
				</td>
			</tr>
			<tr class="TableRow1">
				<td>
					<font size="2" color="<%=Settings.TopicsFontColor%>">
						<asp:Label id="TopicsText" runat="server" />
					</font>
				</td>
			</tr>
			</table>
			<br />
		</asp:PlaceHolder>

	</asp:Panel>

	<br />

	<div align="center" id="NoItemsDiv" class="MessageBox" runat="server" />

	<DMG:Footer id="DMGFooter" runat="server" />

		</form>
	</BODY>
</HTML>