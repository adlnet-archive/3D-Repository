<%@ Page language="VB" Inherits="DMGForums.EditPages" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="Edit Pages" runat="server" />
	</HEAD>
	<BODY>
		<form runat="server">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" ShowLogin="1" runat="server" />

	<asp:Repeater id="CategoryList" runat="server">
		<HeaderTemplate>
			<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
			<tr class="HeaderCell">
			<td align="left">
				<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b><a href="admin.aspx" style="color:<%=Settings.HeaderFontColor%>;">DMG Forums Admin Console</a></b></font>
			</td>
			</tr>
			<tr class="TableRow1">
			<td style="border-top:1px solid <%=Settings.TableBorderColor%>;">
				<table border="2" cellpadding="10" align="center" width="700">
				<tr class="TableRow2">
				<td colspan="2" align="center" valign="middle" height="40" colspan="2">
					<font size="4" color="<%=Settings.TopicsFontColor%>"><b>Edit Pages</b></font>
				</td>
				</tr>
				<tr>
				<td align="center" width="100%">
					<font size="3" color="<%=Settings.TopicsFontColor%>">Click the edit buttons to edit a page.  Click the names to edit sub-categories.</font><br /><br />
					<table border="0" cellpadding="10" align="center">
		</HeaderTemplate>
		<ItemTemplate>

					<tr>
					<td>
						<font size="3" color="<%=Settings.TopicsFontColor%>">
						<a href="editpages.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "PAGE_ID") %>">
						<%# DataBinder.Eval(Container.DataItem, "PAGE_NAME") %>
						</a>
						</font>
					</td>
					<td>
						<asp:button id="EditCatButton" onclick="EditCategory" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PAGE_ID") %>' CssClass="dmgbuttons" runat="server" Text="EDIT" />&nbsp;&nbsp;&nbsp;
						<asp:button id="DeleteCatButton" onclick="DeleteCategory" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PAGE_ID") %>' CssClass="dmgbuttons" runat="server" Text="DELETE" visible='<%# IIF(DataBinder.Eval(Container.DataItem, "PAGE_ID") = 1, "False", "True") %>' />
						<%# IIF(DataBinder.Eval(Container.DataItem, "PAGE_STATUS") = 0, "<font size=""2"" color=""" & Settings.TopicsFontColor & """><b>&nbsp;&nbsp;&nbsp;(OFF)</b></font>", "") %>
					</td>
					</tr>
		</ItemTemplate>
		<FooterTemplate>
					</table>
				</td>
				</tr>
				</table>
				<br />
			</td></tr>
			</table>
			<br />
		</FooterTemplate>
	</asp:Repeater>

	<div align="center" id="NoItemsDiv" class="MessageBox" runat="server" />

	<DMG:Footer id="DMGFooter" runat="server" />

		</form>
	</BODY>
</HTML>