<%@ Page language="VB" Inherits="DMGForums.Topics.PMInbox" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>
<%@ Import Namespace="DMGForums.Global.Functions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="Private Messages Inbox" runat="server" />
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

	<asp:Repeater id="PMList" runat="server">
		<HeaderTemplate>
			<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
			<tr class="HeaderCell">
			<td colspan="6" align="left">
				<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b><%= Session("UserName") %>'s Inbox</b></b></font>
			</td>
			</tr>
			<tr class="SubHeaderCell">
			<td width="100%"><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Topic</b></font></td>
			<td width="180" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>From</b></font></td>
			<td width="180" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>To</b></font></td>
			<td width="65" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Replies</b></font></td>
			<td width="180" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Last Post</b></font></td>
			<td width="65" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>">&nbsp;</font></td>
			</tr>
			</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr class="TableRow1">
			<td width="100%" style="border-top:1px solid <%=Settings.TableBorderColor%>;">
				<font size="2">
					<%# IIF((((DataBinder.Eval(Container.DataItem, "TOPIC_FROM") = Session("UserID")) and (DataBinder.Eval(Container.DataItem, "TOPIC_FROM_READ") = 0)) or ((DataBinder.Eval(Container.DataItem, "TOPIC_TO") = Session("UserID")) and (DataBinder.Eval(Container.DataItem, "TOPIC_TO_READ") = 0))), "<b>(New)</b> ", "")%>
					<a href="pm_topic.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "TOPIC_ID") %>">
					<b><%# DataBinder.Eval(Container.DataItem, "TOPIC_SUBJECT") %></b>
					</a>
				</font>
			</td>
			<td width="180" align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
				<font size="2">
					<nobr>
					<a href="profile.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "TOPIC_FROM") %>">
					<%# DataBinder.Eval(Container.DataItem, "MEMBER_USERNAME") %>
					</a>
					</nobr>
				</font>
			</td>
			<td width="180" align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
				<font size="2">
					<nobr>
					<a href="profile.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "TOPIC_TO") %>">
					<%# DataBinder.Eval(Container.DataItem, "TOPIC_TO_NAME") %>
					</a>
					</nobr>
				</font>
			</td>
			<td width="65" align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					<%# DataBinder.Eval(Container.DataItem, "TOPIC_REPLIES") %>
				</font>
			</td>
			<td width="180" align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
				<font size="1" color="<%=Settings.TopicsFontColor%>">
					<%# LastTopicBy(DataBinder.Eval(Container.DataItem, "TOPIC_LASTPOST_AUTHOR"), DataBinder.Eval(Container.DataItem, "TOPIC_LASTPOST_NAME"), DataBinder.Eval(Container.DataItem, "TOPIC_LASTPOST_DATE"))%>
				</font>
			</td>
			<td width="65" align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
				<asp:button id="DeletePMButton" onclick="DeletePMConfirm" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TOPIC_ID") %>' CssClass="dmgbuttons" runat="server" Text="DELETE" />
			</td>
			</tr>
		</ItemTemplate>
		<FooterTemplate>
			</table>
			<br />
		</FooterTemplate>
	</asp:Repeater>

	</asp:PlaceHolder>

	<asp:Panel id="ConfirmDeletePM" visible="false" runat="server">
		<table border="0" width="97%" align="center" cellpadding="5" cellspacing="0">
		<tr>
		<td width="100%" align="center" valign="bottom">
			<br /><br />
			<font size="2" color="<%=Settings.FontColor%>">Are you sure you want to delete this private message thread?</font><br /><br />
			<asp:DropDownList id="ConfirmDeletePMDropdown" runat="server">
				<asp:ListItem Selected="True" Value="1" Text="Yes" />
				<asp:ListItem Value="0" Text="No" />
			</asp:DropDownList>
			<br /><br />
			<asp:Button id="ConfirmDeletePMButton" onclick="DeletePM" runat="server" Text="Submit" />
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