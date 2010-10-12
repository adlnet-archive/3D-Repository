<%@ Page language="VB" Inherits="DMGForums.Members.BlogsList" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>
<%@ Import Namespace="DMGForums.Global.Functions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings id="DMGSettings" runat="server" />
	</HEAD>
	<BODY>
		<form runat="server">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" runat="server" />

	<asp:PlaceHolder id="PagePanel" runat="server">

	<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
	<tr class="HeaderCell">
	<td align="left">
		<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b><asp:Label id="BlogUser" runat="server" /> >> <asp:Label id="BlogCategory" runat="server" /></b></font>
	</td>
	</tr>
	<tr class="TableRow1">
	<td style="border-top:1px solid <%=Settings.TableBorderColor%>;" align="left">

		<br />

		<asp:Repeater id="BlogTopics" runat="server">
			<HeaderTemplate>
				<table border="0" cellpadding="6" align="center">
			</HeaderTemplate>
			<ItemTemplate>
				<tr>
					<td><font color="<%=Settings.TopicsFontColor%>"><%# FormatDate(Databinder.Eval(Container.DataItem, "BLOG_DATE"), 1) %></td>
					<td>-</td>
					<td><a href="blogs.aspx?ID=<%# Databinder.Eval(Container.DataItem, "BLOG_ID") %>"><b><%# CurseFilter(Databinder.Eval(Container.DataItem, "BLOG_TITLE")) %></b></a></td>
					<td><font size="2" color="<%=Settings.TopicsFontColor%>">(<%# Databinder.Eval(Container.DataItem, "BLOG_REPLIES") %> comments)</font></td>
					<td>
						<asp:button id="EditButton" onclick="EditBlog" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "BLOG_ID") %>' visible='<%# IIF(((DataBinder.Eval(Container.DataItem, "BLOG_AUTHOR") = Session("UserID")) or (Session("UserLevel") = "3")), "True", "False") %>' CssClass="dmgbuttons" runat="server" Text="EDIT" />&nbsp;
						<asp:button id="DeleteButton" onclick="DeleteBlog" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "BLOG_ID") %>' visible='<%# IIF(((DataBinder.Eval(Container.DataItem, "BLOG_AUTHOR") = Session("UserID")) or (Session("UserLevel") = "3")), "True", "False") %>' CssClass="dmgbuttons" runat="server" Text="DELETE" />
					</td>
				</tr>
			</ItemTemplate>
			<FooterTemplate>
				</table>
			</FooterTemplate>
		</asp:Repeater>

		<asp:PlaceHolder id="EditBlogForm" runat="server" visible="false">
			<table border="0" cellpadding="6" align="center">
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Blog Subject:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtBlogTopic" size="50" maxlength="100" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Blog Text:</b><br /><br /><a href="javascript:openHelp('DMGCode.html')">Forum Code</a><br />Allowed</font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtBlogText" Columns="65" Rows="13" Textmode="multiline" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="top"></td>
				<td align="left" valign="middle">
					<font size="2"> 
						<asp:Button type="submit" id="BlogSubmitButton" onclick="SubmitBlog" text="Save Changes" runat="server" />
					</font>
				</td>
			</tr>
			</table>
		</asp:PlaceHolder>

		<asp:PlaceHolder id="DeleteBlogForm" runat="server" visible="false">
			<font size="3" color="<%=Settings.TopicsFontColor%>"> 
				<b>Warning:</b> This action can not be undone.  Pressing the delete button will completely remove the blog from the database along with all of its comments.
				<br /><br />
				<center>
				<asp:Button type="submit" id="FinalDeleteButton" onclick="ConfirmDeleteBlog" text="Delete Blog" runat="server" />
				</center>
			</font>
		</asp:PlaceHolder>


		<br clear="all" />


	</td>
	</tr>
	</table>
	<br />

	</asp:PlaceHolder>

	<div align="center" id="NoItemsDiv" class="MessageBox" runat="server" />

	<DMG:Footer id="DMGFooter" runat="server" />

		</form>
	</BODY>
</HTML>