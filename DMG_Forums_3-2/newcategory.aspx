<%@ Page language="VB" Inherits="DMGForums.Forums.NewCategory" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="New Category" runat="server" />
	</HEAD>
	<BODY>
		<form runat="server">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" ShowLogin="1" runat="server" />

	<asp:Panel id="PagePanel" runat="server">

	<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
	<tr class="HeaderCell">
	<td align="left">
		<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b>Post New Category</b></font>
	</td>
	</tr>
	<tr class="TableRow1">
	<td style="border-top:1px solid <%=Settings.TableBorderColor%>;">

		<table border="0" cellpadding="6" align="center">
		<tr>
			<td align="right" valign="middle">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Category Name:</b></font>
			</td>
			<td align="left" valign="middle">
				<asp:Textbox id="txtName" size="60" maxlength="100" runat="server" />
			</td>
		</tr>
		<tr>
			<td align="right" valign="middle">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Category Page Content:</b></font>
			</td>
			<td align="left" valign="middle"><font size="2" color="<%=Settings.TopicsFontColor%>"><a href="javascript:openHelp('DMGAdminCode.html')">DMG Admin Code</a> Allowed</font></td>
		</tr>
		<tr>
			<td align="left" valign="middle" colspan="2">
				<asp:Textbox id="txtContent" Columns="85" Rows="15" Textmode="multiline" runat="server" />
			</td>
		</tr>
		<tr>
			<td align="right" valign="top">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Show HTML Header/Footer?</b></font>
			</td>
			<td align="left" valign="middle">
				<asp:DropDownList id="txtShowHeaders" runat="server">
					<asp:ListItem Value="1" Text="Yes" runat="server" />
					<asp:ListItem Value="0" Text="No" runat="server" />
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td align="right" valign="top">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Show Login Box?</b></font>
			</td>
			<td align="left" valign="middle">
				<asp:DropDownList id="txtShowLogin" runat="server">
					<asp:ListItem Value="1" Text="Yes" runat="server" />
					<asp:ListItem Value="0" Text="No" runat="server" />
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td align="right" valign="top">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Status:</b></font>
			</td>
			<td align="left" valign="middle">
				<asp:DropDownList id="txtStatus" runat="server">
					<asp:ListItem Selected="True" Value="1" Text="On" runat="server" />
					<asp:ListItem Value="0" Text="Off" runat="server" />
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td align="right" valign="top">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Sort By:</b></font>
			</td>
			<td align="left" valign="top">
				<asp:Textbox id="txtSortBy" size="10" maxlength="10" runat="server" />
				<font size="2" color="<%=Settings.TopicsFontColor%>"><br />(Enter An Integer, Default Is 1)</font>
			</td>
		</tr>
		<tr>
			<td align="right" valign="top"></td>
			<td align="left" valign="middle">
				<asp:Button type="submit" id="SubmitButton" onclick="SubmitCategory" text="Submit Category" runat="server" />
			</td>
		</tr>
		</table>
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