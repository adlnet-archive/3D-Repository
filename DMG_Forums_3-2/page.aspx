<%@ Page language="VB" Inherits="DMGForums.PageContent" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings id="DMGSettings" runat="server" />
	</HEAD>
	<BODY>
		<form runat="server">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" runat="server" />

	<asp:Panel ID="PagePanel" runat="server">

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

	<asp:Panel id="PasswordPanel" visible="false" runat="server">
		<table border="0" width="97%" align="center" cellpadding="5" cellspacing="0">
		<tr>
		<td width="100%" align="center" valign="bottom">
			<font size="2" color="<%=Settings.FontColor%>">This Page Requires A Password</font><br /><br />
			<asp:Textbox id="PasswordBox" size="30" maxlength="50" textmode="password" runat="server" /><br />
			<asp:Button id="PasswordButton" onclick="ApplyPagePassword" runat="server" Text="Submit" />
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