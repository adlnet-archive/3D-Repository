<%@ Page language="VB" Inherits="DMGForums.Members.Profile" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>
<%@ Import Namespace="DMGForums.Global.Functions" %>
<%@ Import Namespace="System.Data" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings id="DMGSettings" runat="server" />
	</HEAD>
	<BODY>
		<form runat="server">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" runat="server" />

<asp:Panel id="PagePanel" runat="server">

	<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">

	<asp:Repeater id="ProfileTop" runat="server">
		<ItemTemplate>
			<tr class="HeaderCell">
			<td colspan="2" align="left">
				<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b><%# DataBinder.Eval(Container.DataItem, "MEMBER_USERNAME") %></b></font>
			</td>
			</tr>
			<tr class="TableRow1">
			<td colspan="2" style="border-bottom:1px solid <%=Settings.TableBorderColor%>;">
				<font size="<%=Settings.TopicsFontSize%>" color="<%=Settings.TopicsFontColor%>">
					<%# PosterDetails(DataBinder.Eval(Container.DataItem, "MEMBER_ID"), DataBinder.Eval(Container.DataItem, "MEMBER_LOCATION"), DataBinder.Eval(Container.DataItem, "MEMBER_POSTS"), DataBinder.Eval(Container.DataItem, "MEMBER_DATE_JOINED"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_USECUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_ALLOWCUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_CUSTOMLOADED"), DataBinder.Eval(Container.DataItem, "MEMBER_TITLE_USECUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_TITLE_ALLOWCUSTOM"), DataBinder.Eval(Container.DataItem, "MEMBER_TITLE"), DataBinder.Eval(Container.DataItem, "AVATAR_IMAGE"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_CUSTOMTYPE"), DataBinder.Eval(Container.DataItem, "MEMBER_AVATAR_SHOW"), DataBinder.Eval(Container.DataItem, "MEMBER_PHOTO"), DataBinder.Eval(Container.DataItem, "MEMBER_LEVEL"), DataBinder.Eval(Container.DataItem, "MEMBER_RANKING"), 2) %>
				</font>
				<center><font size="2"><b>
				<asp:LinkButton runat="server" ID="SendPMLink" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "MEMBER_ID") %>' Text="Send Private Message" onClick="SendPM" visible='<%# IIF(Session("UserLogged") = "1", "True", "False") %>' />
				<asp:LinkButton runat="server" ID="SendMailLink" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "MEMBER_ID") %>' Text="<br /><br />Send E-Mail Message" onClick="SendEmail" visible='<%# IIF(((Session("UserLogged") = "1") and ((Settings.EmailAllowSend = 1) or (Session("UserLevel") = "3"))), "True", "False") %>' />
				</b></font></center>
				<br />
			</td>
			</tr>
		</ItemTemplate>
	</asp:Repeater>

	<asp:PlaceHolder id="ProfilePhotosPanel" runat="server">
		<tr class="TableRow1">
		<td colspan="2" align="center" style="border-bottom:1px solid <%=Settings.TableBorderColor%>;">
			<asp:DataList id="ProfilePhotos" runat="server" AutoGenerateColumns="False" border="0" CellPadding="10" RepeatColumns="5" RepeatDirection="Horizontal">
				<ItemTemplate>
					<center>
					<a href="javascript:openPhoto('showphoto.aspx?PHOTO=memberphotos/<%# DataBinder.Eval(Container.DataItem, "PHOTO_ID") %>.<%# DataBinder.Eval(Container.DataItem, "PHOTO_EXTENSION") %>')"><img border="0" src="memberphotos/<%# DataBinder.Eval(Container.DataItem, "PHOTO_ID") %>_s.<%# DataBinder.Eval(Container.DataItem, "PHOTO_EXTENSION") %>"><%# IIF(Databinder.Eval(Container.DataItem, "PHOTO_DESCRIPTION") = "", "", "<br />" & Databinder.Eval(Container.DataItem, "PHOTO_DESCRIPTION")) %></a>
					</center>
				</ItemTemplate>
			</asp:DataList>
		</td>
		</tr>
	</asp:PlaceHolder>

	<asp:Repeater id="ProfileBottom" runat="server">
		<ItemTemplate>
			<tr class="TableRow1">
			<td width="50%" align="left" valign="top" style="border-right:1px solid <%=Settings.TableBorderColor%>;">

				<table border="0" cellpadding="5" align="center">
				<tr>
				<td align="right" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# IIF((Databinder.Eval(Container.DataItem, "MEMBER_EMAIL") = "") or (Databinder.Eval(Container.DataItem, "MEMBER_EMAIL_SHOW") = 0),"", "<b>E-Mail Address:</b> ")%>
					</font>
				</td>
				<td align="left" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# IIF((Databinder.Eval(Container.DataItem, "MEMBER_EMAIL") = "") or (Databinder.Eval(Container.DataItem, "MEMBER_EMAIL_SHOW") = 0),"", "<a href=""mailto:" & Databinder.Eval(Container.DataItem, "MEMBER_EMAIL") & """>" & Databinder.Eval(Container.DataItem, "MEMBER_EMAIL") & "</a>")%>
					</font>
				</td>
				</tr>
				<tr>
				<td align="right" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(Databinder.Eval(Container.DataItem, "MEMBER_HOMEPAGE") = "","", "<b>Homepage:</b> ")%>
					</font>
				</td>
				<td align="left" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(Databinder.Eval(Container.DataItem, "MEMBER_HOMEPAGE") = "","", FormatURL(Databinder.Eval(Container.DataItem, "MEMBER_HOMEPAGE")))%>
					</font>
				</td>
				</tr>
				<tr>
				<td align="right" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<b>Join Date: </b>
					</font>
				</td>
				<td align="left" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# FormatDate(DataBinder.Eval(Container.DataItem, "MEMBER_DATE_JOINED"), 1) %>
					</font>
				</td>
				</tr>
				<tr>
				<td align="right" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<b>Last Visit: </b>
					</font>
				</td>
				<td align="left" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# FormatDate(DataBinder.Eval(Container.DataItem, "MEMBER_DATE_LASTVISIT"), 1) %>
					</font>
				</td>
				</tr>
				<tr>
				<td align="right" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<b>Posts: </b>
					</font>
				</td>
				<td align="left" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# DataBinder.Eval(Container.DataItem, "MEMBER_POSTS") %>
					</font>
				</td>
				</tr>
				<tr>
				<td align="right" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(Databinder.Eval(Container.DataItem, "MEMBER_IM_AOL") = "","", "<b>AIM:</b> ")%>
					</font>
				</td>
				<td align="left" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(Databinder.Eval(Container.DataItem, "MEMBER_IM_AOL") = "","", "<img src=""forumimages/im_aol.gif"">&nbsp;" & Databinder.Eval(Container.DataItem, "MEMBER_IM_AOL"))%>
					</font>
				</td>
				</tr>
				<tr>
				<td align="right" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(IsInteger(Databinder.Eval(Container.DataItem, "MEMBER_IM_ICQ")), "<b>ICQ:</b> ", "")%>
					</font>
				</td>
				<td align="left" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(IsInteger(Databinder.Eval(Container.DataItem, "MEMBER_IM_ICQ")), "<img src=""http://online.mirabilis.com/scripts/online.dll?icq=" & Databinder.Eval(Container.DataItem, "MEMBER_IM_ICQ") & "&img=5|18|18"">&nbsp;" & Databinder.Eval(Container.DataItem, "MEMBER_IM_ICQ"), "")%>
					</font>
				</td>
				</tr>
				<tr>
				<td align="right" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(Databinder.Eval(Container.DataItem, "MEMBER_IM_MSN") = "","", "<b>MSN:</b> ")%>
					</font>
				</td>
				<td align="left" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(Databinder.Eval(Container.DataItem, "MEMBER_IM_MSN") = "","", "<img src=""forumimages/im_msn.gif"">&nbsp;" & Databinder.Eval(Container.DataItem, "MEMBER_IM_MSN"))%>
					</font>
				</td>
				</tr>
				<tr>
				<td align="right" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(Databinder.Eval(Container.DataItem, "MEMBER_IM_YAHOO") = "","", "<b>Yahoo IM:</b> ")%>
					</font>
				</td>
				<td align="left" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(Databinder.Eval(Container.DataItem, "MEMBER_IM_YAHOO") = "","", "<a href=""http://edit.yahoo.com/config/send_webmesg?.target=" & Databinder.Eval(Container.DataItem, "MEMBER_IM_YAHOO") & "&.src=pg"" target=""_blank""><img border=""0"" src=""http://opi.yahoo.com/online?u=" & Databinder.Eval(Container.DataItem, "MEMBER_IM_YAHOO") & "&m=g&t=2|125|25""></a>")%>
					</font>
				</td>
				</tr>
				</table>
				
			</td>
			<td width="50%" align="left" valign="top">

				<table border="0" cellpadding="5" align="center">
				<tr>
				<td align="right" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(Databinder.Eval(Container.DataItem, "MEMBER_REALNAME") = "","", "<b>Real Name:</b> ")%>
					</font>
				</td>
				<td align="left" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(Databinder.Eval(Container.DataItem, "MEMBER_REALNAME") = "","", Databinder.Eval(Container.DataItem, "MEMBER_REALNAME"))%>
					</font>
				</td>
				</tr>
				<tr>
				<td align="right" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(Databinder.Eval(Container.DataItem, "MEMBER_LOCATION") = "","", "<b>Location:</b> ")%>
					</font>
				</td>
				<td align="left" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(Databinder.Eval(Container.DataItem, "MEMBER_LOCATION") = "","", Databinder.Eval(Container.DataItem, "MEMBER_LOCATION"))%>
					</font>
				</td>
				</tr>
				<tr>
				<td align="right" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(Databinder.Eval(Container.DataItem, "MEMBER_AGE") = "","", "<b>Age:</b> ")%>
					</font>
				</td>
				<td align="left" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(Databinder.Eval(Container.DataItem, "MEMBER_AGE") = "","", Databinder.Eval(Container.DataItem, "MEMBER_AGE"))%>
					</font>
				</td>
				</tr>
				<tr>
				<td align="right" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(Databinder.Eval(Container.DataItem, "MEMBER_BIRTHDAY") = "","", "<b>Birthday:</b> ")%>
					</font>
				</td>
				<td align="left" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(Databinder.Eval(Container.DataItem, "MEMBER_BIRTHDAY") = "","", Databinder.Eval(Container.DataItem, "MEMBER_BIRTHDAY"))%>
					</font>
				</td>
				</tr>
				<tr>
				<td align="right" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(Databinder.Eval(Container.DataItem, "MEMBER_SEX") = "","", "<b>Sex:</b> ")%>
					</font>
				</td>
				<td align="left" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(Databinder.Eval(Container.DataItem, "MEMBER_SEX") = "","", Databinder.Eval(Container.DataItem, "MEMBER_SEX"))%>
					</font>
				</td>
				</tr>
				<tr>
				<td align="right" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(Databinder.Eval(Container.DataItem, "MEMBER_OCCUPATION") = "","", "<b>Occupation:</b> ")%>
					</font>
				</td>
				<td align="left" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(Databinder.Eval(Container.DataItem, "MEMBER_OCCUPATION") = "","", Databinder.Eval(Container.DataItem, "MEMBER_OCCUPATION"))%>
					</font>
				</td>
				</tr>
				<tr>
				<td align="right" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(Databinder.Eval(Container.DataItem, "MEMBER_FAVORITESITE") = "","", "<b>Favorite Site:</b> ")%>
					</font>
				</td>
				<td align="left" valign="top">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<%# IIF(Databinder.Eval(Container.DataItem, "MEMBER_FAVORITESITE") = "","", FormatURL(Databinder.Eval(Container.DataItem, "MEMBER_FAVORITESITE")))%>
					</font>
				</td>
				</tr>
				</table>

		</ItemTemplate>
	</asp:Repeater>

				<asp:Panel id="TopicsPanel" runat="server">
					<font size="3" color="<%=Settings.TopicsFontColor%>">
					<br clear="all" /><br /><center><b>Latest Topics</b><br /></center>
					</font>
					<asp:DataGrid id="RecentPosts" runat="server" AutoGenerateColumns="False" CellPadding="5" align="center" width="90%">
						<Columns>
						<asp:TemplateColumn>
							<HeaderTemplate>
								<font Size="2" color="<%=Settings.TopicsFontColor%>"><b>Forum</b></font>
							</HeaderTemplate>
							<ItemTemplate>
								<font Size="2">
								<a href="forums.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "FORUM_ID") %>">
									<%#DataBinder.Eval(Container.DataItem, "FORUM_NAME")%>
								</a>
								</font>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn>
							<HeaderTemplate>
								<font Size="2" color="<%=Settings.TopicsFontColor%>"><b>Topic</b></font>
							</HeaderTemplate>
							<ItemTemplate>
								<font Size="2">
								<a href="topics.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "TOPIC_ID") %>">
									<%#CurseFilter(Left(DataBinder.Eval(Container.DataItem, "TOPIC_SUBJECT"), 50))%>
								</a>
								</font>
							</ItemTemplate>
						</asp:TemplateColumn>
						</Columns>
					</asp:DataGrid>
				</asp:Panel>

				<br />

		</td>
		</tr>

	<asp:PlaceHolder id="BlogsNotesPanel" runat="server">
		<tr class="TableRow1">
		<td colspan="2" style="border-top:1px solid <%=Settings.TableBorderColor%>;">

			<asp:Repeater id="BlogsListing" runat="server">
				<HeaderTemplate>
					<table style="border:2px solid <%=Settings.TableBorderColor%>;margin:5px;" width="425" align="left">
					<tr class="HeaderCell">
					<td colspan="3">
						<font size="3" color="<%=Settings.HeaderFontColor%>">
						Latest Blog Entries
						</font>
					</td>
					</tr>
				</HeaderTemplate>
				<ItemTemplate>
					<tr class="TableRow2">
					<td>
						<font size="2" color="<%=Settings.TopicsFontColor%>">
						&nbsp;&nbsp;<%# FormatDate(Databinder.Eval(Container.DataItem, "BLOG_DATE"), 1) %>
						</font>
					</td>
					<td>
						<font size="2">
						&nbsp;&nbsp;<a href="blogs.aspx?ID=<%# Databinder.Eval(Container.DataItem, "BLOG_ID") %>"><%# CurseFilter(Databinder.Eval(Container.DataItem, "BLOG_TITLE")) %></a>
						</font>
					</td>
					<td>
						<font size="2" color="<%=Settings.TopicsFontColor%>">
						(<%# Databinder.Eval(Container.DataItem, "BLOG_REPLIES") %> comments)
						</font>
					</td>
					</tr>
				</ItemTemplate>
				<FooterTemplate>
					<tr class="TableRow2">
					<td align="center" colspan="3">
						<font size="3">
						<a href="blogslist.aspx?ID=<%=Request.Querystring("ID")%>">View All Blogs</a>
						</font>
					</td>
					</tr>
					</table>
				</FooterTemplate>
			</asp:Repeater>

			<font size="<%=Settings.TopicsFontSize%>" color="<%=Settings.TopicsFontColor%>">
				<%= FormatString(MemberNotesText) %>
			</font>

			<br clear="all" /><br />
		</td>
		</tr>
	</asp:PlaceHolder>

	</table>
	<br />

</asp:Panel>

	<div align="center" id="NoItemsDiv" class="MessageBox" runat="server" />

	<DMG:Footer id="DMGFooter" runat="server" />

		</form>
	</BODY>
</HTML>