<%@ Page language="VB" Inherits="DMGForums.Members.MembersPage" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>
<%@ Import Namespace="DMGForums.Global.Functions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="Members" runat="server" />
	</HEAD>
	<BODY>
		<form runat="server">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" runat="server" />

	<asp:Panel ID="PagePanel" runat="server">

		<table border="0" cellpadding="8" cellspacing="0" align="center">
		<tr>
		<td align="center">
			<font size="2" color="<%=Settings.FontColor%>">
			<b>Search Usernames:</b>
			&nbsp;<asp:LinkButton runat="server" ID="AllButton" CommandArgument="All" Text="All" onClick="ChangeSearchLetter" />
			&nbsp;<asp:LinkButton runat="server" ID="A" CommandArgument="A" Text="A" onClick="ChangeSearchLetter" />
			&nbsp;<asp:LinkButton runat="server" ID="B" CommandArgument="B" Text="B" onClick="ChangeSearchLetter" />
			&nbsp;<asp:LinkButton runat="server" ID="C" CommandArgument="C" Text="C" onClick="ChangeSearchLetter" />
			&nbsp;<asp:LinkButton runat="server" ID="D" CommandArgument="D" Text="D" onClick="ChangeSearchLetter" />
			&nbsp;<asp:LinkButton runat="server" ID="E" CommandArgument="E" Text="E" onClick="ChangeSearchLetter" />
			&nbsp;<asp:LinkButton runat="server" ID="F" CommandArgument="F" Text="F" onClick="ChangeSearchLetter" />
			&nbsp;<asp:LinkButton runat="server" ID="G" CommandArgument="G" Text="G" onClick="ChangeSearchLetter" />
			&nbsp;<asp:LinkButton runat="server" ID="H" CommandArgument="H" Text="H" onClick="ChangeSearchLetter" />
			&nbsp;<asp:LinkButton runat="server" ID="I" CommandArgument="I" Text="I" onClick="ChangeSearchLetter" />
			&nbsp;<asp:LinkButton runat="server" ID="J" CommandArgument="J" Text="J" onClick="ChangeSearchLetter" />
			&nbsp;<asp:LinkButton runat="server" ID="K" CommandArgument="K" Text="K" onClick="ChangeSearchLetter" />
			&nbsp;<asp:LinkButton runat="server" ID="L" CommandArgument="L" Text="L" onClick="ChangeSearchLetter" />
			&nbsp;<asp:LinkButton runat="server" ID="M" CommandArgument="M" Text="M" onClick="ChangeSearchLetter" />
			&nbsp;<asp:LinkButton runat="server" ID="N" CommandArgument="N" Text="N" onClick="ChangeSearchLetter" />
			&nbsp;<asp:LinkButton runat="server" ID="O" CommandArgument="O" Text="O" onClick="ChangeSearchLetter" />
			&nbsp;<asp:LinkButton runat="server" ID="P" CommandArgument="P" Text="P" onClick="ChangeSearchLetter" />
			&nbsp;<asp:LinkButton runat="server" ID="Q" CommandArgument="Q" Text="Q" onClick="ChangeSearchLetter" />
			&nbsp;<asp:LinkButton runat="server" ID="R" CommandArgument="R" Text="R" onClick="ChangeSearchLetter" />
			&nbsp;<asp:LinkButton runat="server" ID="S" CommandArgument="S" Text="S" onClick="ChangeSearchLetter" />
			&nbsp;<asp:LinkButton runat="server" ID="T" CommandArgument="T" Text="T" onClick="ChangeSearchLetter" />
			&nbsp;<asp:LinkButton runat="server" ID="U" CommandArgument="U" Text="U" onClick="ChangeSearchLetter" />
			&nbsp;<asp:LinkButton runat="server" ID="V" CommandArgument="V" Text="V" onClick="ChangeSearchLetter" />
			&nbsp;<asp:LinkButton runat="server" ID="W" CommandArgument="W" Text="W" onClick="ChangeSearchLetter" />
			&nbsp;<asp:LinkButton runat="server" ID="X" CommandArgument="X" Text="X" onClick="ChangeSearchLetter" />
			&nbsp;<asp:LinkButton runat="server" ID="Y" CommandArgument="Y" Text="Y" onClick="ChangeSearchLetter" />
			&nbsp;<asp:LinkButton runat="server" ID="Z" CommandArgument="Z" Text="Z" onClick="ChangeSearchLetter" />
			</font>
		</td>
		</tr>
		</table>

	<asp:Panel id="PagingPanel" runat="server">
		<table border="0" cellpadding="8" cellspacing="0">
		<tr>
		<td width="150" align="right">
			<font size="2"><b>
			<asp:LinkButton runat="server" ID="FirstLink" Text="&laquo; First" onClick="ChangePage" />
			&nbsp;&nbsp;
			<asp:LinkButton runat="server" ID="PreviousLink" Text="&laquo; Previous" onClick="ChangePage" />
			</b></font>
		</td>
		<td align="center">
			<font size="2" color="<%=Settings.FontColor%>">
			<b>Page <asp:DropDownList runat="server" ID="JumpPage" AutoPostBack="true" OnSelectedIndexChanged="ChangePage"  /> of <asp:Label runat="server" ID="PageCountLabel" EnableViewState="true" /></b>
			</font>
		</td>
		<td width="150" align="left">
			<font size="2"><b>
			<asp:LinkButton runat="server" ID="NextLink" Text="Next &raquo;" onClick="ChangePage" />
			&nbsp;&nbsp;
			<asp:LinkButton runat="server" ID="LastLink" Text="Last &raquo;" onClick="ChangePage" />
			</b></font>
		</td>
		</tr>
		</table>
	</asp:Panel>

	<asp:Label ID="SortLabel" visible="false" runat="server" />
	<asp:Label ID="SearchLetter" visible="false" runat="server" />

	<asp:Repeater id="MembersList" runat="server">
		<HeaderTemplate>
			<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
			<tr class="HeaderCell">
			<td colspan="7" align="left">
				<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b>
					Members
				</b></font>
			</td>
			</tr>
			<tr class="SubHeaderCell">
			<td width="100%"><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b><asp:LinkButton runat="server" ID="MemberNameLink" Text="Member Name" onClick="ChangeSort" /></b></font></td>
			<td width="150" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b><asp:Label runat="server" ID="TitleLink" Text="Title" /></b></font></td>
			<td width="65" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b><asp:LinkButton runat="server" ID="PostsLink" Text="Posts" onClick="ChangeSort" /></b></font></td>
			<td width="150" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b><asp:LinkButton runat="server" ID="LastPostLink" Text="Last Post" onClick="ChangeSort" /></b></font></td>
			<td width="120" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b><asp:LinkButton runat="server" ID="DateJoinedLink" Text="Date Joined" onClick="ChangeSort" /></b></font></td>
			<td width="120" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b><asp:LinkButton runat="server" ID="LastVisitLink" Text="Last Visit" onClick="ChangeSort" /></b></font></td>
			<td width="150" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b><asp:Label runat="server" ID="LocationLink" Text="Location" /></b></font></td>
			</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr class="TableRow1">
			<td width="100%" style="border-top:1px solid <%=Settings.TableBorderColor%>;">
				<font size="2">
					<% if Session("UserLevel") = "3" then %>
						<asp:button id="EditMemberButton" onclick="EditMember" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "MEMBER_ID") %>' CssClass="dmgbuttons" runat="server" Text="EDIT" />&nbsp;&nbsp;&nbsp;
					<% end if %>
					<a href="profile.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "MEMBER_ID") %>">
					<%# DataBinder.Eval(Container.DataItem, "MEMBER_USERNAME") %>
					</a>
					<%# IIF(Databinder.Eval(Container.DataItem, "MEMBER_IM_AOL") = "","", "&nbsp;&nbsp;&nbsp;<img src=""forumimages/im_aol.gif"">")%>
					<%# IIF(Databinder.Eval(Container.DataItem, "MEMBER_IM_ICQ") = "","", "&nbsp;&nbsp;&nbsp;<img src=""forumimages/im_icq.gif"">")%>
					<%# IIF(Databinder.Eval(Container.DataItem, "MEMBER_IM_MSN") = "","", "&nbsp;&nbsp;&nbsp;<img src=""forumimages/im_msn.gif"">")%>
					<%# IIF(Databinder.Eval(Container.DataItem, "MEMBER_IM_YAHOO") = "","", "&nbsp;&nbsp;&nbsp;<img src=""forumimages/im_yahoo.gif"">")%>
				</font>
			</td>
			<td align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					<%# GetTitle(DataBinder.Eval(Container.DataItem, "MEMBER_RANKING"), DataBinder.Eval(Container.DataItem, "MEMBER_POSTS"), DataBinder.Eval(Container.DataItem, "MEMBER_TITLE_USECUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_TITLE"), DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL")) %>
				</font>
			</td>
			<td align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					<%# DataBinder.Eval(Container.DataItem, "MEMBER_POSTS") %>
				</font>
			</td>
			<td align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(IsDBNull(DataBinder.Eval(Container.DataItem, "MEMBER_DATE_LASTPOST")), "&nbsp;", DataBinder.Eval(Container.DataItem, "MEMBER_DATE_LASTPOST")) %>
				</font>
			</td>
			<td align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					<%# FormatDate(DataBinder.Eval(Container.DataItem, "MEMBER_DATE_JOINED"), 1) %>
				</font>
			</td>
			<td align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					<%# FormatDate(DataBinder.Eval(Container.DataItem, "MEMBER_DATE_LASTVISIT"), 1) %>
				</font>
			</td>
			<td align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(DataBinder.Eval(Container.DataItem, "MEMBER_LOCATION") = "", "&nbsp;", DataBinder.Eval(Container.DataItem, "MEMBER_LOCATION")) %>
				</font>
			</td>
			</tr>
		</ItemTemplate>
		<FooterTemplate>
			</table>
			<br />
		</FooterTemplate>
	</asp:Repeater>

		<asp:PlaceHolder id="StatsPanel" visible="false" runat="server">
			<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
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

	<div align="center" id="NoItemsDiv" class="MessageBox" runat="server" />

	<DMG:Footer id="DMGFooter" runat="server" />

		</form>
	</BODY>
</HTML>