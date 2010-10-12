<%@ Page language="VB" Inherits="DMGForums.Members.UserCP" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>
<%@ Import Namespace="DMGForums.Global.Functions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="User Control Panel" runat="server" />
	</HEAD>
	<BODY>
		<form runat="server">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" ShowLogin="1" runat="server" />

	<asp:PlaceHolder id="PagePanel" runat="server">

	<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
	<tr class="HeaderCell">
	<td align="left">
		<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b><asp:Label id="CPTitle" runat="server" /></b></font>
	</td>
	</tr>
	<tr class="TableRow1">
	<td style="border-top:1px solid <%=Settings.TableBorderColor%>;" align="center">

		<asp:PlaceHolder id="UserCPLinks" visible="false" runat="server">
			<br />
			<table border="0" align="center" cellpadding="7">
			<tr>
			<td align="left" valign="top">
				<font size="3" color="<%=Settings.TopicsFontColor%>"><b>User Profile</b></font><br /><br />
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					<li /><asp:LinkButton runat="server" ID="ViewProfileLink" Text="View Profile" onClick="ViewProfile" /><br /><br />
					<li /><asp:LinkButton runat="server" ID="EditProfileLink" Text="Edit Profile" onClick="EditProfile" /><br /><br />
					<li /><asp:LinkButton runat="server" ID="ResetPasswordLink" Text="Reset Password" onClick="ResetPasswordConfirm" /><br /><br />
				</font>
			</td>
			<td width="20">&nbsp;</td>
			<td align="left" valign="top">
				<font size="3" color="<%=Settings.TopicsFontColor%>"><b>Text & Photos</b></font><br /><br />
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					<asp:PlaceHolder id="PhotosPanel" runat="server">
						<li /><a href="javascript:openUploader('upload.aspx?TYPE=memberphoto&ID=<%=Request.Querystring("ID")%>')">Upload Photos</a><br /><br />
						<li /><asp:LinkButton runat="server" ID="EditPhotosLink" Text="View/Delete Photos" onClick="EditMemberPhotos" /><br /><br />
					</asp:PlaceHolder>
					<li /><asp:LinkButton runat="server" ID="EditNotesLink" Text="Edit Profile Text" onClick="EditNotes" /><br /><br />
				</font>
			</td>
			<td width="20">&nbsp;</td>
			<td align="left" valign="top">
				<font size="3" color="<%=Settings.TopicsFontColor%>"><b>Blogs</b></font><br /><br />
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					<li /><asp:LinkButton runat="server" ID="AddBlogLink" Text="Create New Blog Entry" onClick="AddBlog" /><br /><br />
					<li /><asp:LinkButton runat="server" ID="EditBlogsLink" Text="View Your Blog Entries" onClick="EditBlogs" /><br /><br />
				</font>
			</td>
			<asp:PlaceHolder id="PMPanel" runat="server">
				<td width="20">&nbsp;</td>
				<td align="left" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>"><b>Private Messages</b></font><br /><br />
					<font size="2" color="<%=Settings.TopicsFontColor%>">
						<li /><asp:LinkButton runat="server" ID="PMLink" Text="Private Messages Inbox" onClick="ViewPM" /><br /><br />
						<li /><asp:LinkButton runat="server" ID="PMSendLink" Text="Create Private Message" onClick="CreatePM" /><br /><br />
					</font>
				</td>
			</asp:PlaceHolder>
			<asp:PlaceHolder id="AdminPanel" visible="false" runat="server">
				<td width="20">&nbsp;</td>
				<td align="left" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>"><b>Administration</b></font><br /><br />
					<font size="2" color="<%=Settings.TopicsFontColor%>">
						<li /><asp:LinkButton runat="server" ID="CalculatePostsLink" Text="Re-Calculate Post Count" onClick="CalculatePosts" /><br /><br />
						<li /><asp:LinkButton runat="server" ID="BanMemberLink" Text="Ban Member" onClick="BanMemberConfirm" /><br /><br />
						<li /><asp:LinkButton runat="server" ID="DeleteMemberLink" Text="Delete Member" onClick="DeleteMemberConfirm" /><br /><br />
					</font>
				</td>
			</asp:PlaceHolder>
			</tr>
			</table>
		</asp:PlaceHolder>

		<asp:PlaceHolder id="EditNotesForm" runat="server" visible="false">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
				<br />This text is the main content that will appear on your user page.  Enter any text that you want other users to see when viewing the main page of your profile.  Forum Code is allowed in this box.<br /><br />
			</font>
			<table border="0" cellpadding="6" align="center">
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Profile Text:</b><br /><br /><a href="javascript:openHelp('DMGCode.html')">Forum Code</a><br />Allowed</font>
				</td>
				<td align="left" valign="middle">
					<asp:TextBox id="txtNotes" Columns="65" Rows="13" Textmode="multiline" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="top"></td>
				<td align="left" valign="middle">
					<asp:Button type="submit" id="NotesSubmit" onclick="SubmitNotes" text="Save Changes" runat="server" />
				</td>
			</tr>
			</table>
		</asp:PlaceHolder>

		<asp:PlaceHolder id="EditPhotosForm" runat="server" visible="false">
			<font size="2" color="<%=Settings.TopicsFontColor%>">
				<br />The current size limit for member photos is <%=Settings.MemberPhotoSize%>kb.  If you reach this size you must delete photos before you will be able to add new ones.  Click a delete button below to remove a photo from your profile.<br /><br />
			</font>
			<asp:DataList id="ProfilePhotos" runat="server" AutoGenerateColumns="False" border="0" CellPadding="10" RepeatColumns="5" RepeatDirection="Horizontal">
				<ItemTemplate>
					<center>
					<a href="javascript:openPhoto('showphoto.aspx?PHOTO=memberphotos/<%# DataBinder.Eval(Container.DataItem, "PHOTO_ID") %>.<%# DataBinder.Eval(Container.DataItem, "PHOTO_EXTENSION") %>')"><img border="0" src="memberphotos/<%# DataBinder.Eval(Container.DataItem, "PHOTO_ID") %>_s.<%# DataBinder.Eval(Container.DataItem, "PHOTO_EXTENSION") %>"></a><br />
					<asp:button id="DeletePhotoButton" onclick="DeleteMemberPhoto" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PHOTO_ID") %>' CssClass="dmgbuttons" runat="server" Text="DELETE" />
					</center>
				</ItemTemplate>
			</asp:DataList>
		</asp:PlaceHolder>

		<asp:PlaceHolder id="NewBlogForm" runat="server" visible="false">
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
					<font size="2" color="<%=Settings.TopicsFontColor%>"> 
						<asp:Button type="submit" id="BlogSubmit" onclick="SubmitBlog" text="Submit" runat="server" />
					</font>
				</td>
			</tr>
			</table>
		</asp:PlaceHolder>

		<asp:PlaceHolder id="ResetPasswordPanel" visible="false" runat="server">
			<br />
			<center>
			<font size="3" color="<%=Settings.TopicsFontColor%>">
				<b>Enter A New Password And Press Submit</b><br /><br />
			</font>
			<asp:textbox id="txtNewPassword" TextMode="password" size="30" maxlength="50" runat="server" />
			&nbsp;
			<asp:Button type="submit" id="PasswordSubmitButton" onclick="ResetPassword" text="Submit" runat="server" />
			</center>
			<br /><br />
		</asp:PlaceHolder>

		<asp:PlaceHolder id="BanMemberPanel" visible="false" runat="server">
			<br />
			<center>
			<font size="3" color="<%=Settings.TopicsFontColor%>">
				<b>Ban Member Confirmation</b><br /><br />
			</font>
			</center>

			<table border="0" cellpadding="6" align="center">
			<tr>
				<td align="right" valign="middle" width="50%">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Ban This User's IP Address Also?</b></font>
				</td>
				<td align="left" valign="middle" width="50%">
					<asp:DropDownList id="txtBanIP" runat="server">
						<asp:ListItem Value="1" Text="Yes" />
						<asp:ListItem Value="0" Text="No" Selected="True" />
					</asp:DropDownList>
			</td>
			</tr>
			<tr>
				<td align="center" valign="middle" colspan="2">
					<font size="2"> 
						<asp:Button type="submit" id="BanSubmitButton" onclick="BanMember" text="Ban Member" runat="server" />
					</font>
				</td>
			</tr>
			</table>
			<br />
		</asp:PlaceHolder>

		<asp:PlaceHolder id="DeleteMemberPanel" visible="false" runat="server">
			<br />
			<center>
			<font size="3" color="<%=Settings.TopicsFontColor%>">
				<b>Delete Member Confirmation</b><br /><br />
			</font>
			<font size="2" color="<%=Settings.TopicsFontColor%>">
				<b>Warning:</b> This action can not be undone.  If you decide to free this username for future registration, you must select "yes" to delete all current posts.  If you remove a user but do not delete the posts, they will show up on their respective threads as being posted by a "Deleted Member."<br /><br />
			</font>
			</center>

			<table border="0" cellpadding="6" align="center">
			<tr>
				<td align="right" valign="middle" width="50%">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Delete All Topics/Replies By This User?</b></font>
				</td>
				<td align="left" valign="middle" width="50%">
					<asp:DropDownList id="txtDeletePosts" runat="server">
						<asp:ListItem Value="1" Text="Yes" Selected="True" />
						<asp:ListItem Value="0" Text="No" />
					</asp:DropDownList>
			</td>
			</tr>
			<tr>
				<td align="right" valign="middle" width="50%">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Free This Username For Future Registration?</b></font>
				</td>
				<td align="left" valign="middle" width="50%">
					<asp:DropDownList id="txtFreeUsername" runat="server">
						<asp:ListItem Value="1" Text="Yes" />
						<asp:ListItem Value="0" Text="No" Selected="True" />
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td align="center" valign="middle" colspan="2">
					<asp:Button type="submit" id="DeleteSubmitButton" onclick="DeleteMember" text="Delete Member" runat="server" />
				</td>
			</tr>
			</table>
			<br />
		</asp:PlaceHolder>

	</td>
	</tr>
	</table>
	<br />

	<asp:Repeater id="SubscribedThreads" runat="server">
		<HeaderTemplate>
			<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
			<tr class="HeaderCell">
			<td colspan="5" align="left">
				<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b>Subscribed Threads</b></font>
			</td>
			</tr>
			<tr class="SubHeaderCell">
			<td width="100%"><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Topic</b></font></td>
			<td width="150" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Author</b></font></td>
			<td width="65" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Replies</b></font></td>
			<td width="65" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Views</b></font></td>
			<td width="150" align="center" nowrap><font size="1" color="<%=Settings.SubHeaderFontColor%>"><b>Last Post</b></font></td>
			</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr class="TableRow1">
			<td width="100%" style="border-top:1px solid <%=Settings.TableBorderColor%>;">
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(DataBinder.Eval(Container.DataItem, "TOPIC_STATUS") = 2, "<img src=""forumimages/lock.gif"">", "")%>
					<%# IIF(DataBinder.Eval(Container.DataItem, "TOPIC_STATUS") = 0, "<b>[Hidden]</b>&nbsp;", "")%>
					<%# IIF(DataBinder.Eval(Container.DataItem, "TOPIC_STICKY") = 1, "<b>Sticky:</b>&nbsp;", "")%>
					<a href="topics.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "TOPIC_ID") %>">
					<b><%# DataBinder.Eval(Container.DataItem, "TOPIC_SUBJECT") %></b>
					</a>
					<%# QuickPaging(DataBinder.Eval(Container.DataItem, "TOPIC_ID"), DataBinder.Eval(Container.DataItem, "TOPIC_REPLIES"), Settings.ItemsPerPage) %>
				</font>
			</td>
			<td align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
				<font size="2">
					<nobr>
					<a href="profile.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "TOPIC_AUTHOR") %>">
					<%# DataBinder.Eval(Container.DataItem, "TOPIC_AUTHOR_NAME") %>
					</a>
					</nobr>
				</font>
			</td>
			<td align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					<%# DataBinder.Eval(Container.DataItem, "TOPIC_REPLIES") %>
				</font>
			</td>
			<td align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					<%# DataBinder.Eval(Container.DataItem, "TOPIC_VIEWS") %>
				</font>
			</td>
			<td align="center" style="border-top:1px solid <%=Settings.TableBorderColor%>;border-left:1px solid <%=Settings.TableBorderColor%>;" nowrap>
				<font size="1" color="<%=Settings.TopicsFontColor%>">
					<%# LastTopicBy(DataBinder.Eval(Container.DataItem, "TOPIC_LASTPOST_AUTHOR"), DataBinder.Eval(Container.DataItem, "TOPIC_LASTPOST_NAME"), DataBinder.Eval(Container.DataItem, "TOPIC_LASTPOST_DATE"))%>
				</font>
			</td>
			</tr>
		</ItemTemplate>
		<FooterTemplate>
			</table>
			<br />
		</FooterTemplate>
	</asp:Repeater>

	</asp:PlaceHolder>

	<div align="center" id="NoItemsDiv" class="MessageBox" runat="server" />

	<DMG:Footer id="DMGFooter" runat="server" />

		</form>
	</BODY>
</HTML>