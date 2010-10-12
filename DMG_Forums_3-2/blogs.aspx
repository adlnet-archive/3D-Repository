<%@ Page language="VB" Inherits="DMGForums.Members.Blogs" %>

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
		<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b><asp:Label id="BlogUser" runat="server" /> >> <asp:Label id="BlogCategory" runat="server" /> >> <asp:Label id="BlogTitle" runat="server" /></b></font>
	</td>
	</tr>
	<tr class="TableRow1">
	<td style="border-top:1px solid <%=Settings.TableBorderColor%>;" align="left">

		<br />

		<asp:Repeater id="BlogTopic" runat="server">
			<ItemTemplate>
				<font size="2" color="<%=Settings.TopicsFontColor%>">
				<i>Posted by <a href="profile.aspx?ID=<%# Databinder.Eval(Container.DataItem, "BLOG_AUTHOR") %>"><%# Databinder.Eval(Container.DataItem, "MEMBER_USERNAME") %></a> at <%# FormatDate(Databinder.Eval(Container.DataItem, "BLOG_DATE"), 3) %></i>
				&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				<asp:button id="EditButton" onclick="EditBlog" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "BLOG_ID") %>' visible='<%# IIF(((DataBinder.Eval(Container.DataItem, "BLOG_AUTHOR") = Session("UserID")) or (Session("UserLevel") = "3")), "True", "False") %>' CssClass="dmgbuttons" runat="server" Text="EDIT" />&nbsp;
				<asp:button id="DeleteButton" onclick="DeleteBlog" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "BLOG_ID") %>' visible='<%# IIF(((DataBinder.Eval(Container.DataItem, "BLOG_AUTHOR") = Session("UserID")) or (Session("UserLevel") = "3")), "True", "False") %>' CssClass="dmgbuttons" runat="server" Text="DELETE" />
				<br /><br />
				<ul>
				<%# FormatString(Databinder.Eval(Container.DataItem, "BLOG_TEXT")) %>
				</ul>
				</font>
			</ItemTemplate>
		</asp:Repeater>

		<asp:Repeater id="BlogReplies" runat="server">
			<HeaderTemplate>
				<table border="0" width="90%" align="center" cellpadding="0" cellspacing="0"><tr><td width="100%">
				<br />
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Reader Comments</b></font>
			</HeaderTemplate>
			<ItemTemplate>
				<br />
				<table width="100%" style="border-top:2px solid <%=Settings.TableBorderColor%>;border-bottom:2px solid <%=Settings.TableBorderColor%>;" cellpadding="5" cellspacing="0">
				<tr class="TableRow2">
				<td width="100%" align="left">
					<font size="2" color="<%=Settings.TopicsFontColor%>">
					<i>Posted by <a href="profile.aspx?ID=<%# Databinder.Eval(Container.DataItem, "BLOG_REPLY_AUTHOR") %>"><%# Databinder.Eval(Container.DataItem, "MEMBER_USERNAME") %></a> at <%# FormatDate(Databinder.Eval(Container.DataItem, "BLOG_REPLY_DATE"), 3) %></i>
					&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					<asp:button id="EditReplyButton" onclick="EditBlogReply" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "BLOG_REPLY_ID") %>' visible='<%# IIF(((DataBinder.Eval(Container.DataItem, "BLOG_REPLY_AUTHOR") = Session("UserID")) or (Session("UserLevel") = "3")), "True", "False") %>' CssClass="dmgbuttons" runat="server" Text="EDIT" />&nbsp;
					<asp:button id="DeleteReplyButton" onclick="DeleteBlogReply" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "BLOG_REPLY_ID") %>' visible='<%# IIF(((DataBinder.Eval(Container.DataItem, "BLOG_REPLY_AUTHOR") = Session("UserID")) or (Session("UserLevel") = "3")), "True", "False") %>' CssClass="dmgbuttons" runat="server" Text="DELETE" />
					<br /><br />
					<ul>
					<%# FormatString(Databinder.Eval(Container.DataItem, "BLOG_REPLY_TEXT")) %>
					</ul>
					</font>
				</td>
				</tr>
				</table>
			</ItemTemplate>
			<FooterTemplate>
				</td></tr></table>
			</FooterTemplate>
		</asp:Repeater>

		<asp:PlaceHolder id="CommentForm" visible="false" runat="server">
			<br />
			<table border="0" width="90%" align="center" cellpadding="0" cellspacing="0"><tr><td width="100%">
			<br />
			<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Post A Comment</b></font>
			<br />
			<table width="100%" style="border-top:2px solid <%=Settings.TableBorderColor%>;border-bottom:2px solid <%=Settings.TableBorderColor%>;" cellpadding="5" cellspacing="0">
			<tr class="TableRow2">
			<td width="100%" align="center">
				<table border="0" cellpadding="0" cellspacing="0"><tr><td>
				<font size="2" color="<%=Settings.TopicsFontColor%>">
				Type comments here.  <a href="javascript:openHelp('DMGCode.html')">Forum Code</a> is allowed.<br />
				</font>
				<asp:Textbox id="txtCommentText" Columns="65" Rows="8" Textmode="multiline" runat="server" />
				<br />
				<asp:Button type="submit" id="CommentSubmit" onclick="SubmitComments" text="Submit" runat="server" />
				</td></tr></table>
			</td>
			</tr>
			</table>
			</td></tr></table>
		</asp:PlaceHolder>

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

		<asp:PlaceHolder id="EditBlogReplyForm" runat="server" visible="false">
			<table border="0" cellpadding="6" align="center">
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Comment Text:</b><br /><br /><a href="javascript:openHelp('DMGCode.html')">Forum Code</a><br />Allowed</font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtBlogReplyText" Columns="65" Rows="13" Textmode="multiline" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="top"></td>
				<td align="left" valign="middle">
					<font size="2"> 
						<asp:Button type="submit" id="BlogReplySubmitButton" onclick="SubmitBlogReply" text="Save Changes" runat="server" />
					</font>
				</td>
			</tr>
			</table>
		</asp:PlaceHolder>

		<asp:PlaceHolder id="DeleteBlogReplyForm" runat="server" visible="false">
			<font size="3" color="<%=Settings.TopicsFontColor%>"> 
				<b>Warning:</b> This action can not be undone.  Pressing the delete button will completely remove the blog comment from the database.
				<br /><br />
				<center>
				<asp:Button type="submit" id="FinalDeleteReplyButton" onclick="ConfirmDeleteBlogReply" text="Delete Comment" runat="server" />
				</center>
			</font>
		</asp:PlaceHolder>

		<br /><br />

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