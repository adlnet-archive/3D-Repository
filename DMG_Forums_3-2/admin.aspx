<%@ Page language="VB" Inherits="DMGForums.Admin.AdminPage" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>
<%@ Import Namespace="System.Data" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="Admin" runat="server" />
	</HEAD>
	<BODY>
		<form runat="server">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" ShowLogin="1" runat="server" />

	<asp:Panel id="PagePanel" runat="server">

	<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
	<tr class="HeaderCell">
	<td align="left">
		<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b><a href="admin.aspx" style="color:<%=Settings.HeaderFontColor%>;">DMG Forums Admin Console</a></b></font>
	</td>
	</tr>
	<tr class="TableRow1">
	<td style="border-top:1px solid <%=Settings.TableBorderColor%>;">

		<asp:Panel id="AdminLinks" visible="false" runat="server">
			<br />
			<table cellpadding="10" border="1" bordercolor="<%=Settings.TableBorderColor%>" align="center">
			<tr>
			<td align="center" valign="top">
				<table border="2" cellpadding="0" cellspacing="5" align="center" bgcolor="<%=Settings.TableBGColor2%>">
				<tr>
				<td align="center" valign="middle" height="40">
					<font size="4" color="<%=Settings.TopicsFontColor%>"><b>Site Setup</b></font>
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="SiteMainConfigButton" onclick="OpenConfig" CommandArgument="1" Text="Main Configuration" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="EmailConfigButton" onclick="OpenConfig" CommandArgument="12" Text="E-Mail Server Setup" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="CustomVariablesButton" onclick="OpenConfig" CommandArgument="10" Text="Custom Variables" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="SearchConfigButton" onclick="OpenConfig" CommandArgument="23" Text="Search Configuration" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				</table>
			</td>
			<td align="center" valign="top">
				<table border="2" cellpadding="0" cellspacing="5" align="center" bgcolor="<%=Settings.TableBGColor2%>">
				<tr>
				<td align="center" valign="middle" height="40">
					<font size="4" color="<%=Settings.TopicsFontColor%>"><b>Page Content</b></font>
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="CreatePageButton" onclick="CreateNewPage" Text="Create New Page" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="EditPageButton" onclick="EditPages" Text="Edit Pages" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="MainMenuConfigButton" onclick="EditMainMenu" Text="Main Menu Configuration" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="HTMLFormResultsButton" onclick="OpenConfig" CommandArgument="16" Text="HTML Form Results" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				</table>
			</td>
			<td align="center" valign="top">
				<table border="2" cellpadding="0" cellspacing="5" align="center" bgcolor="<%=Settings.TableBGColor2%>">
				<tr>
				<td align="center" valign="middle" height="40">
					<font size="4" color="<%=Settings.TopicsFontColor%>"><b>Web Page Design</b></font>
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="ColorsFontsButton" onclick="OpenConfig" CommandArgument="2" Text="Colors & Fonts" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="CustomHTMLButton" onclick="OpenConfig" CommandArgument="3" Text="HTML Design Layout" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="NewTemplateButton" onclick="CreateNewTemplate" Text="Create New Design Template" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="DeleteTemplateButton" onclick="DeleteTemplateMenu" Text="Delete Unused Templates" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				</table>
			</td>
			</tr>
			<tr>
			<td align="center" valign="top">
				<table border="2" cellpadding="0" cellspacing="5" align="center" bgcolor="<%=Settings.TableBGColor2%>">
				<tr>
				<td align="center" valign="middle" height="40">
					<font size="4" color="<%=Settings.TopicsFontColor%>"><b>DMG Forums</b></font>
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="ForumsMainConfigButton" onclick="OpenConfig" CommandArgument="21" Text="Forum Options" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="EditCategoriesButton" onclick="OpenConfig" CommandArgument="4" Text="Categories" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="EditForumsButton" onclick="OpenConfig" CommandArgument="5" Text="Forums" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="CreateMemberButton" onclick="CreateNewMember" Text="Create Member" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="EditMembersButton" onclick="OpenConfig" CommandArgument="6" Text="Edit Members" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="RankingButton" onclick="OpenConfig" CommandArgument="8" Text="Forum Rankings" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="VerifyMembersButton" onclick="OpenConfig" CommandArgument="13" Text="Members Pending Validation" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="BannedIPButton" onclick="OpenConfig" CommandArgument="17" Text="Banned IP Addresses" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="CurseFilterButton" onclick="OpenConfig" CommandArgument="14" Text="Bad Word Filter" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="CustomMessagesButton" onclick="OpenConfig" CommandArgument="19" Text="Customize E-Mails & Dialogs" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="CleanupPMButton" onclick="OpenConfig" CommandArgument="22" Text="Private Messages Cleanup" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="UpdateCountsButton" onclick="UpdateCounts" Text="Repair Forum Counts" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				</table>
			</td>
			<td align="center" valign="top">
				<table border="2" cellpadding="0" cellspacing="5" align="center" bgcolor="<%=Settings.TableBGColor2%>">
				<tr>
				<td align="center" valign="middle" height="40">
					<font size="4" color="<%=Settings.TopicsFontColor%>"><b>File Uploads</b></font>
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="FileManagerButton" onclick="GoToFileManager" Text="File Manager" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="AvatarButton" onclick="OpenConfig" CommandArgument="7" Text="Avatars" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="PhotoGalleryButton" onclick="OpenConfig" CommandArgument="18" Text="Photo Galleries" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="RotatorButton" onclick="OpenConfig" CommandArgument="15" Text="Image Rotators" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				</table>
			</td>
			<td align="center" valign="top">
				<table border="2" cellpadding="0" cellspacing="5" align="center" bgcolor="<%=Settings.TableBGColor2%>">
				<tr>
				<td align="center" valign="middle" height="40">
					<font size="4" color="<%=Settings.TopicsFontColor%>"><b>Help</b></font>
				</td>
				</tr>
				<tr>
				<td align="center">
					<input type="submit" id="DMGForumCodesButton" OnClick="openHelp('DMGCode.html')" value="DMG Forum Codes" class="AdminButtons" />
				</td>
				</tr>
				<tr>
				<td align="center">
					<input type="submit" id="DMGAdminCodesButton" OnClick="openHelp('DMGAdminCode.html')" value="DMG Admin Codes" class="AdminButtons" />
				</td>
				</tr>
				<tr>
				<td align="center">
					<asp:Button id="AboutDMGButton" onclick="OpenConfig" CommandArgument="20" Text="About DMG Forums" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				</table>
			</td>
			</tr>
			</table>

			<br />
			<center>
			<IFRAME src="http://www.dmgdevelopment.com/documents/dmgforumsupdate.asp?Ver=<%=Settings.DMGVersion%>" width="500" height="140" scrolling="no" frameborder="1" align="center" style="border: solid 1px;">
			[Your browser does not support frames.  To visit the page <A target="_blank" href="http://www.dmgdevelopment.com/documents/dmgforumsupdate.asp?Ver=<%=Settings.DMGVersion%>">click here</A>.]
			</IFRAME>
			</center>
			<br /><br />
		</asp:Panel>


		<asp:Panel id="AdminMainConfig" runat="server" visible="false">
			<table border="2" cellpadding="10" align="center">
			<tr class="TableRow2">
			<td align="center" valign="middle" height="40">
				<font size="4" color="<%=Settings.TopicsFontColor%>"><b>Main Configuration</b></font>
			</td>
			</tr>
			<tr>
			<td valign="top">
				<table border="0" cellpadding="6" align="center">
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Website Name:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGTitle" size="50" maxlength="100" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>HTML Page Title:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGHtmlTitle" size="50" maxlength="100" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Copyright Text:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGCopyright" size="50" maxlength="100" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Current Layout Template:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtDMGLayoutTemplate" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Main Page Options:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtDMGSetDefaultPage" runat="server">
							<asp:ListItem Value="1" Text="Forums Only" />
							<asp:ListItem Value="0" Text="Custom HTML (No Forums)" />
							<asp:ListItem Value="2" Text="Custom HTML Above Forums" />
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Website Logo URL:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGLogo" size="50" maxlength="100" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Website URL:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGURL" size="50" maxlength="100" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Maximum Thumbnail Width/Height:</b><br />(pixels)</font>
					</td>
					<td align="left" valign="top">
						<asp:Textbox id="txtDMGThumbnailSize" size="3" maxlength="5" runat="server" /></font>
					</td>
				</tr>
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>META Keywords:</b><br /><br /></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGCustomMeta" Columns="45" Rows="6" Textmode="multiline" runat="server" />
					</td>
				</tr>
				</table>
			</td>
			</tr>
			<tr>
				<td align="center" valign="middle">
					<asp:Button type="submit" id="MainConfigSubmit" onclick="SubmitMainConfig" text="Save Changes" CssClass="AdminButtons" runat="server" />
				</td>
			</tr>
			</table>
		</asp:Panel>


		<asp:Panel id="AdminForumMainConfig" runat="server" visible="false">
			<table border="2" cellpadding="10" align="center">
			<tr class="TableRow2">
			<td width="50%" align="center" valign="middle" height="40">
				<font size="4" color="<%=Settings.TopicsFontColor%>"><b>Forum Configuration</b></font>
			</td>
			<td width="50%" align="center" valign="middle" height="40">
				<font size="4" color="<%=Settings.TopicsFontColor%>"><b>Member Options</b></font>
			</td>
			</tr>
			<tr>
			<td width="50%" valign="top">
				<table border="0" cellpadding="6" align="center">
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>New Member Registration:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtDMGAllowRegistration" runat="server">
							<asp:ListItem Value="1" Text="On" />
							<asp:ListItem Value="0" Text="Off" />
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Registration Page Format:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtDMGQuickRegistration" runat="server">
							<asp:ListItem Value="1" Text="Quick Registration" />
							<asp:ListItem Value="0" Text="Full Registration" />
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Member Validation Options:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtDMGMemberValidation" runat="server">
							<asp:ListItem Value="0" Text="No Validation Required" />
							<asp:ListItem Value="1" Text="Send New Users A Validation E-Mail" />
							<asp:ListItem Value="2" Text="Admin Must Manually Validate New Users" />
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Send E-Mail Welcome Message?</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtDMGEnableEmailWelcomeMessage" runat="server">
							<asp:ListItem Value="1" Text="Yes" />
							<asp:ListItem Value="0" Text="No" />
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Forum Statistics Display:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtDMGShowStatistics" runat="server">
							<asp:ListItem Value="1" Text="On" />
							<asp:ListItem Value="0" Text="Off" />
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>RSS Feeds:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtDMGRSSFeeds" runat="server">
							<asp:ListItem Value="1" Text="On" />
							<asp:ListItem Value="0" Text="Off" />
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Curse Word Filter:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtDMGCurseFilter" runat="server">
							<asp:ListItem Value="1" Text="On" />
							<asp:ListItem Value="0" Text="Off" />
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Items Per Page:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGItemsPerPage" size="3" maxlength="2" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Spam Flood Control:</b><br />(Seconds)</font>
					</td>
					<td align="left" valign="top">
						<asp:Textbox id="txtDMGSpamFilter" size="3" maxlength="2" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Lock Member Pages For Non-Members?</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtDMGHideMembers" runat="server">
							<asp:ListItem Value="1" Text="Yes" />
							<asp:ListItem Value="0" Text="No" />
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Hide Login Box By Default?</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtDMGHideLogin" runat="server">
							<asp:ListItem Value="1" Text="Yes" />
							<asp:ListItem Value="0" Text="No" />
						</asp:DropDownList>
					</td>
				</tr>
				</table>
			</td>
			<td width="50%" valign="top">
				<table border="0" cellpadding="6" align="center">
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Allow Subscriptions?</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtDMGAllowSub" runat="server">
							<asp:ListItem Value="1" Text="Yes" />
							<asp:ListItem Value="0" Text="No" />
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Allow Multimedia In Posts?</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtDMGAllowMedia" runat="server">
							<asp:ListItem Value="1" Text="Yes" />
							<asp:ListItem Value="0" Text="No" />
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Allow Topic/Reply Editing?</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtDMGAllowEdits" runat="server">
							<asp:ListItem Value="1" Text="Yes" />
							<asp:ListItem Value="0" Text="No" />
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Allow Users To Report Abuse?</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtDMGAllowReporting" runat="server">
							<asp:ListItem Value="1" Text="Yes" />
							<asp:ListItem Value="0" Text="No" />
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Member Photos Upload Limit:</b><br />(kilobytes)</font>
					</td>
					<td align="left" valign="top">
						<asp:Textbox id="txtDMGMemberPhotoSize" size="3" maxlength="10" runat="server" /> <font size="2" color="<%=Settings.TopicsFontColor%>"><i>(Enter "0" to disable)</i></font>
					</td>
				</tr>
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Avatar Maximum Width/Height:</b><br />(pixels)</font>
					</td>
					<td align="left" valign="top">
						<asp:Textbox id="txtDMGAvatarSize" size="3" maxlength="5" runat="server" /></font>
					</td>
				</tr>
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Topic Upload File Types Allowed:</b><br />(MIME Types)</font>
					</td>
					<td align="left" valign="top">
						<asp:Textbox id="txtDMGMemberFileTypes" Columns="45" Rows="4" Textmode="multiline" runat="server" /> <font size="2" color="<%=Settings.TopicsFontColor%>"><br /><i>(Enter "all" for no restrictions)</i></font>
					</td>
				</tr>
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Topic Upload Size Limit:</b><br />(kilobytes)</font>
					</td>
					<td align="left" valign="top">
						<asp:Textbox id="txtDMGTopicUploadSize" size="3" maxlength="10" runat="server" />
					</td>
				</tr>
				</table>
			</td>
			</tr>
			<tr>
				<td align="center" valign="middle" colspan="2">
					<asp:Button type="submit" id="ForumConfigSubmit" onclick="SubmitForumConfig" text="Save Changes" CssClass="AdminButtons" runat="server" />
				</td>
			</tr>
			</table>
		</asp:Panel>


		<asp:Panel id="AdminVariableConfig" runat="server" visible="false">
			<table border="2" cellpadding="10" align="center" width="700">
			<tr class="TableRow2">
			<td align="center" valign="middle" height="40">
				<font size="4" color="<%=Settings.TopicsFontColor%>"><b>Custom Variables</b></font>
			</td>
			</tr>
			<tr>
			<td>
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					These are user-defined variables that can be set on this page and then used throughout the web page where custom HTML variables are allowed.  By entering [Var1] into an admin screen that allows HTML the page will display the contents held in [Var1].  The five Var fields can be used to hold a small piece of text while the five Text fields can hold large memo values.
				</font>
			</td>
			</tr>
			<tr>
			<td>
				<table border="0" cellpadding="6" align="center">
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>[Var1]:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGVar1" size="50" maxlength="50" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">

						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>[Var2]:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGVar2" size="50" maxlength="50" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>[Var3]:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGVar3" size="50" maxlength="50" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>[Var4]:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGVar4" size="50" maxlength="50" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>[Var5]:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGVar5" size="50" maxlength="50" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>[Text1]:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGText1" Columns="60" Rows="6" Textmode="multiline" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>[Text2]:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGText2" Columns="60" Rows="6" Textmode="multiline" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>[Text3]:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGText3" Columns="60" Rows="6" Textmode="multiline" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>[Text4]:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGText4" Columns="60" Rows="6" Textmode="multiline" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>[Text5]:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGText5" Columns="60" Rows="6" Textmode="multiline" runat="server" />
					</td>
				</tr>
				</table>
			</td>
			</tr>
			<tr>
				<td align="center" valign="middle" colspan="2">
					<asp:Button type="submit" id="SubmitVariables" onclick="SubmitVariableConfig" text="Save Changes" CssClass="AdminButtons" runat="server" />
				</td>
			</tr>
			</table>
		</asp:Panel>


		<asp:Panel id="AdminColorConfig" visible="false" runat="server">
			<table border="2" cellpadding="10" align="center">
			<tr class="TableRow2">
			<td align="center" valign="middle" height="40">
				<font size="4" color="<%=Settings.TopicsFontColor%>"><b>Colors & Fonts</b></font>
			</td>
			</tr>
			<tr>
			<td>
				<table border="0" cellpadding="6" align="center">
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Template Name:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGTemplateName" size="20" maxlength="50" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Use As The Forum's Template?</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtSetDefaultTemplate" runat="server">
							<asp:ListItem Value="1" Text="Yes" />
							<asp:ListItem Value="0" Text="No" />
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Font Face:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGFontFace" size="20" maxlength="50" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Font Size:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGFontSize" size="3" maxlength="1" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Font Color:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGFontColor" size="20" maxlength="20" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Link Color:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGLinkColor" size="20" maxlength="20" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Link Decoration:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGLinkDecoration" size="20" maxlength="20" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Visited Link Color:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGLinkVisitedColor" size="20" maxlength="20" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Visited Link Decoration:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGLinkVisitedDecoration" size="20" maxlength="20" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Active Link Color:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGLinkActiveColor" size="20" maxlength="20" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Active Link Decoration:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGLinkActiveDecoration" size="20" maxlength="20" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Hover Link Color:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGLinkHoverColor" size="20" maxlength="20" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Hover Link Decoration:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGLinkHoverDecoration" size="20" maxlength="20" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Background Color:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGBGColor" size="20" maxlength="20" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Background Image:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGBGImage2" size="20" maxlength="50" runat="server" /> <font size="2" color="<%=Settings.TopicsFontColor%>">(Leave Blank For None)</font>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Topics Font Size:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGTopicsFontSize" size="3" maxlength="1" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Topics Font Color:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGTopicsFontColor" size="20" maxlength="20" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Topics BG Color:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGTopicsBGColor1" size="20" maxlength="20" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Alternating BG Color:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGTopicsBGColor2" size="20" maxlength="20" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Button Color:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGButtonColor" size="20" maxlength="20" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Scrollbar Color:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGScrollbarColor" size="20" maxlength="20" runat="server" /> <font size="2" color="<%=Settings.TopicsFontColor%>">(Leave Blank For Default)</font>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Table Border Color:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGTableborderColor" size="20" maxlength="20" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Login Font Color:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGLoginFontColor" size="20" maxlength="20" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Header Size:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGHeaderSize" size="1" maxlength="3" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Header Color:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGHeaderColor" size="20" maxlength="20" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Header Font Color:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGHeaderFontColor" size="20" maxlength="20" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Sub-Header Color:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGSubheaderColor" size="20" maxlength="20" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Sub-Header Font Color:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGSubheaderFontColor" size="20" maxlength="20" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Footer Size:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGFooterSize" size="1" maxlength="3" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Footer Color:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGFooterColor" size="20" maxlength="20" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Footer Font Color:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGFooterFontColor" size="20" maxlength="20" runat="server" />
					</td>
				</tr>
				</table>
			</td>
			</tr>
			<tr>
				<td align="center" valign="middle">
					<asp:Button type="submit" id="ColorSubmit" onclick="SubmitColorConfig" text="Save Changes" CssClass="AdminButtons" runat="server" />
				</td>
			</tr>
			</table>
		</asp:Panel>


		<asp:Panel id="AdminCustomHTML" runat="server" visible="false">
			<table border="2" cellpadding="10" align="center" width="700">
			<tr class="TableRow2">
			<td align="center" valign="middle" height="40">
				<font size="4" color="<%=Settings.TopicsFontColor%>"><b>HTML Design Layout</b></font>
			</td>
			</tr>
			<tr>
			<td>
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					Full HTML code is allowed in the three large boxes.  You can also reference any global settings variables so that the code in these boxes stays dynamic and changes based on the color and font selections that are made for the rest of the forum.  For a full list of DMG Admin Codes that can be used in the text boxes, <a href="javascript:openHelp('DMGAdminCode.html')">Click Here</a>.
				</font>
			</td>
			</tr>
			<tr>
			<td>
				<table border="0" cellpadding="6" align="center">
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Template Name:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGTemplateName2" size="20" maxlength="50" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Use As The Forum's Template?</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtSetDefaultTemplate2" runat="server">
							<asp:ListItem Value="1" Text="Yes" />
							<asp:ListItem Value="0" Text="No" />
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Custom HTML Header:</b></font>
					</td>
					<td align="left" valign="top"><font size="2" color="<%=Settings.TopicsFontColor%>">(HTML Appearing Above Content)<br /><a href="javascript:openHelp('DMGAdminCode.html')">DMG Admin Code</a> Allowed</font></td>
				</tr>
				<tr>
					<td align="left" valign="middle" colspan="2">
						<asp:Textbox id="txtDMGCustomHeader" Columns="85" Rows="20" Textmode="multiline" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Custom HTML Footer:</b></font>
					</td>
					<td align="left" valign="top"><font size="2" color="<%=Settings.TopicsFontColor%>">(HTML Appearing Below Content)<br /><a href="javascript:openHelp('DMGAdminCode.html')">DMG Admin Code</a> Allowed</font></td>
				</tr>
				<tr>
					<td align="left" valign="middle" colspan="2">
						<asp:Textbox id="txtDMGCustomFooter" Columns="85" Rows="20" Textmode="multiline" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Custom CSS And JavaScript:</b></font>
					</td>
					<td align="left" valign="top"><font size="2" color="<%=Settings.TopicsFontColor%>">(HTML Appearing Between &lt;HEAD&gt;&lt;/HEAD&gt; Tags)<br /><a href="javascript:openHelp('DMGAdminCode.html')">DMG Admin Code</a> Allowed</font></td>
				</tr>
				<tr>
					<td align="left" valign="middle" colspan="2">
						<asp:Textbox id="txtDMGCustomCSS" Columns="85" Rows="20" Textmode="multiline" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Background Image:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGBGImage" size="30" maxlength="50" runat="server" /> <font size="2" color="<%=Settings.TopicsFontColor%>">(Leave Blank For None)</font>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Side Margins (pixels):</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGMarginSide" size="5" maxlength="5" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Top Margin (pixels):</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGMarginTop" size="5" maxlength="5" runat="server" />
					</td>
				</tr>
				</table>
			</td>
			</tr>
			<tr>
				<td align="center" valign="middle">
					<asp:Button type="submit" id="CustomHTMLSubmit" onclick="SubmitCustomHTML" text="Save Changes" CssClass="AdminButtons" runat="server" />
				</td>
			</tr>
			</table>
		</asp:Panel>

		<asp:Panel id="AdminMainMenuConfig" runat="server" visible="false">
			<table border="2" cellpadding="10" align="center" width="700">
			<tr class="TableRow2">
			<td align="center" valign="middle" height="40" colspan="6">
				<font size="4" color="<%=Settings.TopicsFontColor%>"><b>Main Menu Links</b></font>
			</td>
			</tr>
			<tr>
			<td colspan="6">
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					These are the links that will be displayed when you place the [menu] command inside a custom HTML textbox.  The vertical/horizontal link separators for the [menu=H] and [menu=V] commands can be modified using the form at the bottom of the page.  For custom URL's or links to content pages, use the parameter box to provide a URL or page ID.
				</font>
			</td>
			</tr>
			<tr>
				<td align="center">
					&nbsp;
				</td>
				<td align="center">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Link Text</b></font>
				</td>
				<td align="center">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Link Type <a href="javascript:openHelp('DMGMainMenu.html')">[?]</a></b></font>
				</td>
				<td align="center">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Link Parameter</b></font>
				</td>
				<td align="center">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>New Window?</b></font>
				</td>
				<td align="center">
					&nbsp;
				</td>
			</tr>
				<asp:Repeater id="MainMenuList" runat="server">
				<ItemTemplate>
					<tr>
					<td align="center">
						<asp:button id="LinkUpButton" onclick="MainMenuOrderUp" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "LINK_ORDER") %>' CssClass="dmgbuttons" runat="server" Text="UP" />&nbsp;&nbsp;<asp:button id="LinkDnButton" onclick="MainMenuOrderDown" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "LINK_ORDER") %>' CssClass="dmgbuttons" runat="server" Text="DN" />
					</td>
					<td align="center">
						<input size="20" type="text" name="txtLinkText<%# DataBinder.Eval(Container.DataItem, "LINK_ID") %>" value="<%# DataBinder.Eval(Container.DataItem, "LINK_TEXT") %>">
					</td>
					<td align="center">
						<select name="txtLinkType<%# DataBinder.Eval(Container.DataItem, "LINK_ID") %>">
						<option value="1" <%# IIF(DataBinder.Eval(Container.DataItem, "LINK_TYPE") = 1, "SELECTED", "") %>>Main Page</option>
						<option value="2" <%# IIF(DataBinder.Eval(Container.DataItem, "LINK_TYPE") = 2, "SELECTED", "") %>>Content Page</option>
						<option value="3" <%# IIF(DataBinder.Eval(Container.DataItem, "LINK_TYPE") = 3, "SELECTED", "") %>>Forums</option>
						<option value="4" <%# IIF(DataBinder.Eval(Container.DataItem, "LINK_TYPE") = 4, "SELECTED", "") %>>Register</option>
						<option value="5" <%# IIF(DataBinder.Eval(Container.DataItem, "LINK_TYPE") = 5, "SELECTED", "") %>>Active Topics</option>
						<option value="6" <%# IIF(DataBinder.Eval(Container.DataItem, "LINK_TYPE") = 6, "SELECTED", "") %>>Members</option>
						<option value="7" <%# IIF(DataBinder.Eval(Container.DataItem, "LINK_TYPE") = 7, "SELECTED", "") %>>Search</option>
						<option value="8" <%# IIF(DataBinder.Eval(Container.DataItem, "LINK_TYPE") = 8, "SELECTED", "") %>>Specific Category</option>
						<option value="9" <%# IIF(DataBinder.Eval(Container.DataItem, "LINK_TYPE") = 9, "SELECTED", "") %>>Specific Forum</option>
						<option value="10" <%# IIF(DataBinder.Eval(Container.DataItem, "LINK_TYPE") = 10, "SELECTED", "") %>>User CP</option>
						<option value="11" <%# IIF(DataBinder.Eval(Container.DataItem, "LINK_TYPE") = 11, "SELECTED", "") %>>Private Messages</option>
						<option value="12" <%# IIF(DataBinder.Eval(Container.DataItem, "LINK_TYPE") = 12, "SELECTED", "") %>>Edit Profile</option>
						<option value="15" <%# IIF(DataBinder.Eval(Container.DataItem, "LINK_TYPE") = 15, "SELECTED", "") %>>Login Page</option>
						<option value="13" <%# IIF(DataBinder.Eval(Container.DataItem, "LINK_TYPE") = 13, "SELECTED", "") %>>Administration</option>
						<option value="14" <%# IIF(DataBinder.Eval(Container.DataItem, "LINK_TYPE") = 14, "SELECTED", "") %>>Custom URL</option>
						</select>
					</td>
					<td align="center">
						<input size="20" maxlength="100" type="text" name="txtLinkParameter<%# DataBinder.Eval(Container.DataItem, "LINK_ID") %>" value="<%# DataBinder.Eval(Container.DataItem, "LINK_PARAMETER") %>">
					</td>
					<td align="center">
						<select name="txtLinkWindow<%# DataBinder.Eval(Container.DataItem, "LINK_ID") %>">
						<option value="1" <%# IIF(DataBinder.Eval(Container.DataItem, "LINK_WINDOW") = 1, "SELECTED", "") %>>Yes</option>
						<option value="0" <%# IIF(DataBinder.Eval(Container.DataItem, "LINK_WINDOW") = 0, "SELECTED", "") %>>No</option>
						</select>
					</td>
					<td align="center">
						<asp:button id="DeleteLinkButton" onclick="MainMenuDeleteLink" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "LINK_ID") %>' CssClass="dmgbuttons" runat="server" Text="DELETE" />
					</td>
					</tr>
				</ItemTemplate>
				</asp:Repeater>
				<tr class="TableRow2">
				<td align="center" valign="middle">
					<font color="<%=Settings.TopicsFontColor%>">New</font>
				</td>
				<td align="center">
					<asp:Textbox size="20" id="NewLinkText" runat="server" />
				</td>
				<td align="center">
					<asp:DropDownList id="NewLinkType" runat="server">
						<asp:ListItem Value="1" Text="Main Page" />
						<asp:ListItem Selected="True" Value="2" Text="Content Page" />
						<asp:ListItem Value="3" Text="Forums" />
						<asp:ListItem Value="4" Text="Register" />
						<asp:ListItem Value="5" Text="Active Topics" />
						<asp:ListItem Value="6" Text="Members" />
						<asp:ListItem Value="7" Text="Search" />
						<asp:ListItem Value="8" Text="Specific Category" />
						<asp:ListItem Value="9" Text="Specific Forum" />
						<asp:ListItem Value="10" Text="User CP" />
						<asp:ListItem Value="11" Text="Private Messages" />
						<asp:ListItem Value="12" Text="Edit Profile" />
						<asp:ListItem Value="15" Text="Login Page" />
						<asp:ListItem Value="13" Text="Administration" />
						<asp:ListItem Value="14" Text="Custom URL" />
					</asp:DropDownList>
				</td>
				<td align="center">
					<asp:Textbox size="20" maxlength="100" id="NewLinkParameter" runat="server" />
				</td>
				<td align="center">
					<asp:DropDownList id="NewLinkWindow" runat="server">
						<asp:ListItem Value="1" Text="Yes" />
						<asp:ListItem Selected="True" Value="0" Text="No" />
					</asp:DropDownList>
				</td>
				<td align="center">
					<asp:button id="NewMenuButton" onclick="MainMenuNewLink" CssClass="dmgbuttons" runat="server" Text="SUBMIT NEW" />
				</td>
				</tr>
				<tr>
				<td align="center" colspan="6">
					<table border="0" align="center">
					<tr>
					<td align="right">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Horizontal Separator:</b></font>
					</td>
					<td align="left">
						<asp:Textbox id="txtDMGHorizDivide" size="35" maxlength="50" runat="server" />
					</td>
					</tr>
					<tr>
					<td align="right">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Vertical Separator:</b></font>
					</td>
					<td align="left">
						<asp:Textbox id="txtDMGVertDivide" size="35" maxlength="50" runat="server" />
					</td>
					</tr>
					<tr>
					<td align="center" colspan="2">
						<br />
						<asp:button id="SaveLinkButton" onclick="MainMenuSaveLink" CssClass="AdminButtons" runat="server" Text="Save Changes" />
					</td>
					</tr>
					</table>
				</td>
				</tr>					
			</table>
		</asp:Panel>

		<asp:Panel id="AdminEmailSettings" runat="server" visible="false">
			<table border="2" cellpadding="10" align="center" width="700">
			<tr class="TableRow2">
			<td align="center" valign="middle" height="40">
				<font size="4" color="<%=Settings.TopicsFontColor%>"><b>E-Mail Configuration</b></font>
			</td>
			</tr>
			<tr>
			<td>
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					To use any of the e-mail functions, you must enter a valid SMTP server and a valid send address.  Before turning on e-mail functionality, be sure that the SMTP server you enter allows mail relay from your web server's IP address.
				</font>
			</td>
			</tr>
			<tr>
			<td>
				<table border="0" cellpadding="6" align="center">
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Send E-Mail From:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGEmailAddress" size="50" maxlength="50" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>SMTP Server:</b></font>	
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGEmailSmtp" size="50" maxlength="50" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>SMTP Port:</b></font>
	
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGEmailPort" size="10" maxlength="10" runat="server" /> <font color="<%=Settings.TopicsFontColor%>">(leave blank for default)</font>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>SMTP Username:</b></font>
	
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGEmailUsername" size="15" maxlength="30" runat="server" /> <font color="<%=Settings.TopicsFontColor%>">(leave blank for default)</font>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>SMTP Password:</b></font>
	
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtDMGEmailPassword" size="15" maxlength="30" runat="server" /> <font color="<%=Settings.TopicsFontColor%>">(leave blank for default)</font>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Allow Members To E-Mail Each Other?</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtDMGEmailAllowSend" runat="server">
							<asp:ListItem Value="1" Text="Yes" />
							<asp:ListItem Value="0" Text="No" />
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Allow E-Mail Notices On Subscribed Threads?</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtDMGEmailAllowSub" runat="server">
							<asp:ListItem Value="1" Text="Yes" />
							<asp:ListItem Value="0" Text="No" />
						</asp:DropDownList>
					</td>
				</tr>
				</table>
			</td>
			</tr>
			<tr>
				<td align="center" valign="middle">
					<asp:Button type="submit" id="AdminEmailSubmit" onclick="SubmitEmailConfig" text="Save Changes" CssClass="AdminButtons" runat="server" />
				</td>
			</tr>
			</table>
		</asp:Panel>

		<asp:Panel id="AdminEditCategories" runat="server" visible="false">
			<table border="0" cellpadding="6" align="center">
			<tr>
				<td colspan="2" align="center">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b><a href="newcategory.aspx">Create New Category</a></b></font>
				</td>
			</td>
			</tr>
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Select A Category To Edit:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:DropDownList id="CategoryList" AutoPostBack="true" OnSelectedIndexChanged="EditCategory" runat="server" />
				</td>
			</tr>
			</table>
		</asp:Panel>


		<asp:Panel id="AdminEditForums" runat="server" visible="false">
			<table border="0" cellpadding="6" align="center">
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Select A Category For A New Forum:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:DropDownList id="ForumCategoryList" AutoPostBack="true" OnSelectedIndexChanged="NewForum" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Select A Forum To Edit:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:DropDownList id="ForumList" AutoPostBack="true" OnSelectedIndexChanged="EditForum" runat="server" />
				</td>
			</tr>
			</table>
		</asp:Panel>


		<asp:Panel id="AdminEditMembers" runat="server" visible="false">
			<table border="0" cellpadding="6" align="center">
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Search For A Member:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="MemberSearch" size="30" maxlength="50" runat="server" />
				</td>
				<td align="left" valign="middle">
					<asp:Button type="submit" id="Submit6" onclick="SubmitMemberSearch" text="Search" runat="server" />
				</td>
			</tr>
			</table>
			<asp:Repeater id="MemberList" runat="server">
				<HeaderTemplate>
					<center>
					<font size="2" color="<%=Settings.TopicsFontColor%>"><br />Search Results<br /><br /></font>
					</center>
					<table border="1" cellpadding="10" align="center">
					<tr>
					<td><font color="<%=Settings.TopicsFontColor%>"><b>Username</b></font></td>
					<td><font color="<%=Settings.TopicsFontColor%>"><b>E-Mail Address</b></font></td>
					<td><font color="<%=Settings.TopicsFontColor%>"><b>Original IP</b></font></td>
					<td><font color="<%=Settings.TopicsFontColor%>"><b>Latest IP</b></font></td>
					<td><font color="<%=Settings.TopicsFontColor%>"><b>Registered</b></font></td>
					<td><font color="<%=Settings.TopicsFontColor%>"><b>Last Visit</b></font></td>
					</tr>
				</HeaderTemplate>
				<ItemTemplate>
					<tr>
					<td>
						<font size="2">
						<b><a href="usercp.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "MEMBER_ID") %>"><%# DataBinder.Eval(Container.DataItem, "MEMBER_USERNAME") %></a></b>
						</font>
					</td>
					<td>
						<font size="2" color="<%=Settings.TopicsFontColor%>">
						<%# DataBinder.Eval(Container.DataItem, "MEMBER_EMAIL") %>
						</font>
					</td>
					<td>
						<font size="2">
						<a target="_blank" href="http://ws.arin.net/cgi-bin/whois.pl?queryinput=<%# DataBinder.Eval(Container.DataItem, "MEMBER_IP_ORIGINAL") %>"><%# DataBinder.Eval(Container.DataItem, "MEMBER_IP_ORIGINAL") %></a>
						</font>
					</td>
					<td>
						<font size="2">
						<a target="_blank" href="http://ws.arin.net/cgi-bin/whois.pl?queryinput=<%# DataBinder.Eval(Container.DataItem, "MEMBER_IP_LAST") %>"><%# DataBinder.Eval(Container.DataItem, "MEMBER_IP_LAST") %></a>
						</font>
					</td>
					<td>
						<font size="2" color="<%=Settings.TopicsFontColor%>">
						<%# DataBinder.Eval(Container.DataItem, "MEMBER_DATE_JOINED") %>
						</font>
					</td>
					<td>
						<font size="2" color="<%=Settings.TopicsFontColor%>">
						<%# DataBinder.Eval(Container.DataItem, "MEMBER_DATE_LASTVISIT") %>
						</font>
					</td>
					</tr>
				</ItemTemplate>
				<FooterTemplate>
					</table>
					<br />
				</FooterTemplate>
			</asp:Repeater>
		</asp:Panel>

		<asp:Panel id="AdminVerifyMembers" runat="server" visible="false">
			<asp:Repeater id="VerificationList" runat="server">
				<HeaderTemplate>
					<center>
					<font size="2" color="<%=Settings.TopicsFontColor%>"><br />The following table is a list of registered users that are waiting to be authenticated.<br /><br /></font>
					</center>
					<table border="1" cellpadding="10" align="center">
					<tr>
					<td><font color="<%=Settings.TopicsFontColor%>"><b>Username</b></font></td>
					<td><font color="<%=Settings.TopicsFontColor%>"><b>E-Mail Address</b></font></td>
					<td><font color="<%=Settings.TopicsFontColor%>"><b>IP Address</b></font></td>
					<td><font color="<%=Settings.TopicsFontColor%>"><b>Registered</b></font></td>
					<td>&nbsp;</td>
					<td>&nbsp;</td>
					</tr>
				</HeaderTemplate>
				<ItemTemplate>
					<tr>
					<td>
						<font size="2" color="<%=Settings.TopicsFontColor%>">
						<b><%# DataBinder.Eval(Container.DataItem, "MEMBER_USERNAME") %></b>
						</font>
					</td>
					<td>
						<font size="2" color="<%=Settings.TopicsFontColor%>">
						<%# DataBinder.Eval(Container.DataItem, "MEMBER_EMAIL") %>
						</font>
					</td>
					<td>
						<font size="2" color="<%=Settings.TopicsFontColor%>">
						<a target="_blank" href="http://ws.arin.net/cgi-bin/whois.pl?queryinput=<%# DataBinder.Eval(Container.DataItem, "MEMBER_IP_ORIGINAL") %>"><%# DataBinder.Eval(Container.DataItem, "MEMBER_IP_ORIGINAL") %></a>
						</font>
					</td>
					<td>
						<font size="2" color="<%=Settings.TopicsFontColor%>">
						<%# DataBinder.Eval(Container.DataItem, "MEMBER_DATE_JOINED") %>
						</font>
					</td>
					<td>
						<asp:button id="VerifyButton" onclick="SubmitMemberVerification" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "MEMBER_ID") %>' CssClass="dmgbuttons" runat="server" Text="ACTIVATE" />
					</td>
					<td>
						<asp:button id="VerifyDeleteButton" onclick="DeleteMemberVerification" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "MEMBER_ID") %>' CssClass="dmgbuttons" runat="server" Text="DELETE" />
					</td>
					</tr>
				</ItemTemplate>
				<FooterTemplate>
					</table>
					<br />
				</FooterTemplate>
			</asp:Repeater>
		</asp:Panel>

		<asp:Panel id="AdminVerifyMembersConfirm" runat="server" visible="false">
			<br />
			<center>
				<font color="<%=Settings.TopicsFontColor%>">
				<b>Would you like to e-mail the user to inform them of their activation?</b>
				<br /><br />
				<asp:button id="VerifyYesButton" onclick="SubmitMemberVerificationEmail" runat="server" Text="Yes" />
				&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				<asp:button id="VerifyNoButton" onclick="SubmitMemberVerificationNoEmail" runat="server" Text="No" />
				</font>
			</center>
			<br />
		</asp:Panel>

		<asp:Panel id="AdminCurseFilter" runat="server" visible="false">
			<table border="1" cellpadding="7" align="center">
			<tr class="TableRow2">
			<td align="center" valign="middle" height="40" colspan="3">
				<font size="4" color="<%=Settings.TopicsFontColor%>"><b>Bad Word Filter</b></font>
			</td>
			</tr>
			<tr>
				<td align="left">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Bad Word</b></font>
				</td>
				<td align="left">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Replacement Text</b></font>
				</td>
				<td></td>
			</tr>
				<asp:Repeater id="CurseList" runat="server">
				<ItemTemplate>
					<tr>
					<td align="left">
						<input size="20" maxlength="20" type="text" name="txtCurseWord<%# DataBinder.Eval(Container.DataItem, "CURSE_ID") %>" value="<%# DataBinder.Eval(Container.DataItem, "CURSE_WORD") %>">
					</td>
					<td align="left">
						<input size="20" maxlength="20" type="text" name="txtCurseReplacement<%# DataBinder.Eval(Container.DataItem, "CURSE_ID") %>" value="<%# DataBinder.Eval(Container.DataItem, "CURSE_REPLACEMENT") %>">
					</td>
					<td align="center">
						<asp:button id="SaveCurseButton" onclick="SaveCurseWord" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CURSE_ID") %>' CssClass="dmgbuttons" runat="server" Text="SAVE" />&nbsp;&nbsp;<asp:button id="DeleteCurseButton" onclick="DeleteCurseWord" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CURSE_ID") %>' CssClass="dmgbuttons" runat="server" Text="DELETE" />
					</td>
					</tr>
				</ItemTemplate>
				</asp:Repeater>
			<tr class="TableRow2">
			<td>
				<asp:Textbox size="20" maxlength="20" id="NewCurse" runat="server" />
			</td>
			<td align="center">
				<asp:Textbox size="20" maxlength="20" id="NewCurseReplacement" runat="server" />
			</td>
			<td align="center">
				<asp:button id="NewCurseButton" onclick="NewCurseWord" CssClass="dmgbuttons" runat="server" Text="SUBMIT NEW" />
			</td>
			</tr>
			</table>
		</asp:Panel>

		<asp:Panel id="AdminAvatarConfig" runat="server" visible="false">
			<table border="0" align="center">
			<tr>
			<td>
				<font size="3"><b>
				<li /><a href="javascript:openUploader('upload.aspx?TYPE=avatar')">Upload New Avatar</a><br /><br />
				<li /><asp:LinkButton runat="server" ID="AvatarRefresh" CommandArgument="7" Text="Refresh List" onClick="OpenConfig" />
				</b></font>
			</td>
			</tr>
			</table>
			<center>
			<br />
			<hr width="50%" noshade">
			<br /><font size="3" color="<%=Settings.TopicsFontColor%>"><b>Uploaded Avatars</b></font><br /><br />
			<asp:DataList id="Avatars" runat="server" AutoGenerateColumns="False" border="0" CellPadding="10" RepeatColumns="5" RepeatDirection="Horizontal" ItemStyle-VerticalAlign="Bottom">
				<ItemTemplate>
					<font size="2" color="<%=Settings.TopicsFontColor%>">
					<center>
					<img border="0" src="avatars/<%# DataBinder.Eval(Container.DataItem, "AVATAR_IMAGE") %>"><br /><b><%# DataBinder.Eval(Container.DataItem, "AVATAR_NAME") %></b><br /><br /><asp:button id="DeleteAvButton" onclick="DeleteAvatar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "AVATAR_ID") %>' visible='<%# IIF(DataBinder.Eval(Container.DataItem, "AVATAR_ID") = 1, "False", "True") %>' CssClass="dmgbuttons" runat="server" Text="DELETE" />
					</center>
					</font>
				</ItemTemplate>
			</asp:DataList>
			</center>
		</asp:Panel>


		<asp:Panel id="AdminRankingConfig" runat="server" visible="false">
			<table border="2" cellpadding="10" align="center" width="700">
			<tr class="TableRow2">
			<td align="center" valign="middle" height="40" colspan="9">
				<font size="4" color="<%=Settings.TopicsFontColor%>"><b>Forum Rankings</b></font>
			</td>
			</tr>
			<tr>
			<td colspan="9">
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					To edit the current rankings, change the information in the fields below and press the save button for the ranking you wish to modify.  To delete a ranking, press the delete button.  To add a new ranking, fill in the blank boxes at the bottom of the form and press submit.  You will not be able to add an image to a new ranking until after you have saved it.
				</font>
			</td>
			</tr>
			<tr>
				<td align="center">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Rank Name</b></font>
				</td>
				<td align="center">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Posts To Reach</b></font>
				</td>
				<td align="center">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Rank Image</b></font>
				</td>
				<td align="center">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Allow Topics</b></font>
				</td>
				<td align="center">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Allow Avatars</b></font>
				</td>
				<td align="center">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Allow Custom Avatars</b></font>
				</td>
				<td align="center">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Allow Custom Title</b></font>
				</td>
				<td align="center">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Allow File Uploads</b></font>
				</td>
				<td></td>
			</tr>
				<asp:Repeater id="RankList" runat="server">
				<ItemTemplate>
					<tr>
					<td>
						<input size="15" type="text" name="txtRankName<%# DataBinder.Eval(Container.DataItem, "RANK_ID") %>" value="<%# DataBinder.Eval(Container.DataItem, "RANK_NAME") %>">
					</td>
					<td align="center">
						<input size="10" type="text" name="txtRankPosts<%# DataBinder.Eval(Container.DataItem, "RANK_ID") %>" value="<%# DataBinder.Eval(Container.DataItem, "RANK_POSTS") %>">
					</td>
					<td align="center">
						<%# IIF(DataBinder.Eval(Container.DataItem, "RANK_IMAGE") = "", "NONE<br />", "<img src=""rankimages/" & DataBinder.Eval(Container.DataItem, "RANK_IMAGE") & """><br />") %>
						<font size="2"><a href="javascript:openUploader('upload.aspx?TYPE=rankimage&RANK=<%# DataBinder.Eval(Container.DataItem, "RANK_ID") %>')">Change</a></font>
					</td>
					<td align="center">
						<select name="txtRankAllowTopics<%# DataBinder.Eval(Container.DataItem, "RANK_ID") %>">
						<option value="1" <%# IIF(DataBinder.Eval(Container.DataItem, "RANK_ALLOW_TOPICS") = 1, "SELECTED", "") %>>Yes</option>
						<option value="0" <%# IIF(DataBinder.Eval(Container.DataItem, "RANK_ALLOW_TOPICS") = 1, "", "SELECTED") %>>No</option>
						</select>
					</td>
					<td align="center">
						<select name="txtRankAllowAvatar<%# DataBinder.Eval(Container.DataItem, "RANK_ID") %>">
						<option value="1" <%# IIF(DataBinder.Eval(Container.DataItem, "RANK_ALLOW_AVATAR") = 1, "SELECTED", "") %>>Yes</option>
						<option value="0" <%# IIF(DataBinder.Eval(Container.DataItem, "RANK_ALLOW_AVATAR") = 1, "", "SELECTED") %>>No</option>
						</select>
					</td>
					<td align="center">
						<select name="txtRankAllowAvatarCustom<%# DataBinder.Eval(Container.DataItem, "RANK_ID") %>">
						<option value="1" <%# IIF(DataBinder.Eval(Container.DataItem, "RANK_ALLOW_AVATAR_CUSTOM") = 1, "SELECTED", "") %>>Yes</option>
						<option value="0" <%# IIF(DataBinder.Eval(Container.DataItem, "RANK_ALLOW_AVATAR_CUSTOM") = 1, "", "SELECTED") %>>No</option>
						</select>
					</td>
					<td align="center">
						<select name="txtRankAllowTitle<%# DataBinder.Eval(Container.DataItem, "RANK_ID") %>">
						<option value="1" <%# IIF(DataBinder.Eval(Container.DataItem, "RANK_ALLOW_TITLE") = 1, "SELECTED", "") %>>Yes</option>
						<option value="0" <%# IIF(DataBinder.Eval(Container.DataItem, "RANK_ALLOW_TITLE") = 1, "", "SELECTED") %>>No</option>
						</select>
					</td>
					<td align="center">
						<select name="txtRankAllowUploads<%# DataBinder.Eval(Container.DataItem, "RANK_ID") %>">
						<option value="1" <%# IIF(DataBinder.Eval(Container.DataItem, "RANK_ALLOW_UPLOADS") = 1, "SELECTED", "") %>>Yes</option>
						<option value="0" <%# IIF(DataBinder.Eval(Container.DataItem, "RANK_ALLOW_UPLOADS") = 1, "", "SELECTED") %>>No</option>
						</select>
					</td>
					<td align="center">
						<asp:button id="DeleteRankButton" onclick="DeleteRanking" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "RANK_ID") %>' CssClass="dmgbuttons" runat="server" Text="DELETE" />
					</td>
					</tr>
				</ItemTemplate>
				</asp:Repeater>
				<tr class="TableRow2">
				<td>
					<asp:Textbox size="15" id="NewRankName" runat="server" />
				</td>
				<td align="center">
					<asp:Textbox size="10" id="NewRankPosts" runat="server" />
				</td>
				<td align="center">
					<font color="<%=Settings.TopicsFontColor%>">
						Upload After Saving
					</font>
				</td>
				<td align="center">
					<asp:DropDownList id="NewRankAllowTopics" runat="server">
						<asp:ListItem Selected="True" Value="1" Text="Yes" />
						<asp:ListItem Value="0" Text="No" />
					</asp:DropDownList>
				</td>
				<td align="center">
					<asp:DropDownList id="NewRankAllowAvatar" runat="server">
						<asp:ListItem Selected="True" Value="1" Text="Yes" />
						<asp:ListItem Value="0" Text="No" />
					</asp:DropDownList>
				</td>
				<td align="center">
					<asp:DropDownList id="NewRankAllowAvatarCustom" runat="server">
						<asp:ListItem Value="1" Text="Yes" />
						<asp:ListItem Selected="True" Value="0" Text="No" />
					</asp:DropDownList>
				</td>
				<td align="center">
					<asp:DropDownList id="NewRankAllowTitle" runat="server">
						<asp:ListItem Value="1" Text="Yes" />
						<asp:ListItem Selected="True" Value="0" Text="No" />
					</asp:DropDownList>
				</td>
				<td align="center">
					<asp:DropDownList id="NewRankAllowUploads" runat="server">
						<asp:ListItem Value="1" Text="Yes" />
						<asp:ListItem Selected="True" Value="0" Text="No" />
					</asp:DropDownList>
				</td>
				<td align="center">
					<asp:button id="NewRankButton" onclick="NewRanking" CssClass="dmgbuttons" runat="server" Text="SUBMIT NEW" />
				</td>
				</tr>
			<tr>
				<td align="center" valign="middle" colspan="9">
					<asp:Button type="submit" id="EditRankButton" onclick="EditRanking" text="Save Changes" CssClass="AdminButtons" runat="server" />
				</td>
			</tr>
			</table>
		</asp:Panel>


		<asp:Panel id="AdminRotatorConfig" runat="server" visible="false">
			<table border="2" cellpadding="10" align="center" width="700">
			<tr class="TableRow2">
			<td align="center" valign="middle" height="40">
				<font size="4" color="<%=Settings.TopicsFontColor%>"><b>Image Rotators</b></font>
			</td>
			</tr>
			<tr>
			<td>
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					After creating a rotator and attaching images to it, use the [ImageRotator=#] command in a content page to display it.  Replace the # with the ID number of the rotator you would like to display.
				</font>
			</td>
			</tr>
			<tr>
			<td align="center">
				<asp:Button runat="server" ID="NewRotator" Text="Create New Image Rotator" onClick="CreateRotator" CssClass="AdminButtons" />
			</td>
			</tr>
			<asp:Repeater id="RotatorList" runat="server">
				<HeaderTemplate>
					<tr>
					<td>
						<br />
						<table border="1" cellpadding="6" align="center">
						<tr>
						<td colspan="3" align="center">
							<font size="3" color="<%=Settings.TopicsFontColor%>"><b>Existing Image Rotators</b></font>
						</td>
						</tr>
						<tr>
						<td align="center">
							<font size="2" color="<%=Settings.TopicsFontColor%>"><b>ID</b></font>
						</td>
						<td align="center">
							<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Name</b></font>
						</td>
						<td align="center">
							&nbsp;
						</td>
						</tr>
				</HeaderTemplate>
				<ItemTemplate>
						<tr>
						<td align="center" valign="middle">
							<font size="2" color="<%=Settings.TopicsFontColor%>"><%# DataBinder.Eval(Container.DataItem, "ROTATOR_ID") %></font>
						</td>
						<td align="center" valign="middle">
							<font size="2" color="<%=Settings.TopicsFontColor%>"><%# DataBinder.Eval(Container.DataItem, "ROTATOR_NAME") %></font>
						</td>
						<td>
							<asp:button id="EditRotatorButton" onclick="EditRotator" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ROTATOR_ID") %>' CssClass="dmgbuttons" runat="server" Text="EDIT IMAGES" />&nbsp;
							<asp:button id="DeleteRotatorButton" onclick="DeleteRotator" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ROTATOR_ID") %>' CssClass="dmgbuttons" runat="server" Text="DELETE" />
						</td>
						</tr>
				</ItemTemplate>
				<FooterTemplate>
						</table>
						<br />
					</td>
					</tr>
				</FooterTemplate>
			</asp:Repeater>
			</table>
		</asp:Panel>


		<asp:Panel id="AdminRotatorNew" runat="server" visible="false">
			<table border="0" cellpadding="6" align="center">
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Rotator Name:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="NewRotatorName" size="30" maxlength="30" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="top"></td>
				<td align="left" valign="middle">
					<asp:Button type="submit" id="AdminRotatorSubmit" onclick="SubmitNewRotator" text="Submit" runat="server" />
				</td>
			</tr>
			</table>
		</asp:Panel>


		<asp:Panel id="AdminRotatorNewConfirm" runat="server" visible="false">
			<br />
			<center>
				<font size="3" color="<%=Settings.TopicsFontColor%>"><b>New Image Rotator Created</b></font>
				<br /><br />
				<asp:button id="NewRotatorEdit" onclick="EditRotator" runat="server" Text="Add Images To Rotation" CssClass="AdminButtons" />
				<br /><br />
				<asp:button id="NewRotatorReturn" onclick="OpenConfig" CommandArgument="15" runat="server" Text="Return To Image Rotators" CssClass="AdminButtons" />
			</center>
			<br />
		</asp:Panel>


		<asp:Panel id="AdminRotatorDelete" runat="server" visible="false">
			<br />
			<center>
				<font color="<%=Settings.TopicsFontColor%>">
				<b>Are you sure you want to delete this image rotator?</b>
				<br /><br />
				Note: All images for this rotator will also be deleted.
				<br /><br />
				<asp:button id="RotatorYesButton" onclick="DeleteRotatorConfirm" runat="server" Text="Yes" />
				&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				<asp:button id="RotatorNoButton" onclick="OpenConfig" CommandArgument="15" runat="server" Text="No" />
				</font>
			</center>
			<br />
		</asp:Panel>


		<asp:Panel id="AdminRotatorEdit" runat="server" visible="false">
			<br />
			<table border="0" align="center">
			<tr>
			<td>
				<font size="3"><b>
				<li /><a href="javascript:openUploader('upload.aspx?TYPE=imagerotator&ROTATOR=<%=RotatorID%>')">Upload New Image</a><br /><br />
				<li /><asp:LinkButton runat="server" ID="RotatorRefresh" Text="Refresh List" onClick="EditRotator" />
				</b></font>
			</td>
			</tr>
			</table>
			<br />
			<asp:Repeater id="RotatorImages" runat="server">
				<HeaderTemplate>
					<hr width="50%" noshade">
					<br />
					<table border="1" cellpadding="6" align="center">
					<tr>
					<td colspan="2" align="center">
						<font size="3" color="<%=Settings.TopicsFontColor%>"><b>Images For Rotator ID (<%=RotatorID%>)</b></font>
					</td>
					</tr>
					<tr>
					<td align="center">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Image</b></font>
					</td>
					<td align="center">
						&nbsp;
					</td>
					</tr>
				</HeaderTemplate>
				<ItemTemplate>
					<tr>
					<td align="center" valign="middle">
						<font size="2">
							<a target="_blank" href="<%# DataBinder.Eval(Container.DataItem, "IMAGE_URL") %>"><img border="<%# DataBinder.Eval(Container.DataItem, "IMAGE_BORDER") %>" src="rotatorimages/<%# DataBinder.Eval(Container.DataItem, "IMAGE_ID") %>.<%# DataBinder.Eval(Container.DataItem, "IMAGE_EXTENSION") %>"><br /><%# DataBinder.Eval(Container.DataItem, "IMAGE_DESCRIPTION") %></a>
						</font>
					</td>
					<td>
						<asp:button id="DeleteRotatorImageButton" onclick="DeleteRotatorImage" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IMAGE_ID") %>' CssClass="dmgbuttons" runat="server" Text="DELETE" />
					</td>
					</tr>
				</ItemTemplate>
				<FooterTemplate>
					</table>
					<br />
				</FooterTemplate>
			</asp:Repeater>
		</asp:Panel>


		<asp:Panel id="ColorSettingsPanel" runat="server" visible="false">
			<table border="0" align="center">
			<tr>
			<td align="center" valign="top">
				<font size="3" color="<%=Settings.TopicsFontColor%>"><b>Choose A Template To Edit</b></font><br />
				<asp:DropDownList id="ColorSettingsDropdown" OnSelectedIndexChanged="ChangeColors" AutoPostBack="true" runat="server" />
			</td>
			</tr>
			</table>
		</asp:Panel>


		<asp:Panel id="CustomHTMLSettingsPanel" runat="server" visible="false">
			<table border="0" align="center">
			<tr>
			<td align="center" valign="top">
				<font size="3" color="<%=Settings.TopicsFontColor%>"><b>Choose A Template To Edit</b></font><br />
				<asp:DropDownList id="CustomHTMLSettingsDropdown" OnSelectedIndexChanged="ChangeCustomHTML" AutoPostBack="true" runat="server" />
			</td>
			</tr>
			</table>
		</asp:Panel>


		<asp:Panel id="AdminNewTemplate" runat="server" visible="false">
			<table border="0" cellpadding="6" align="center">
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Template Name:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewTemplateName" size="20" maxlength="50" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Use As The Forum's Template?</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:DropDownList id="txtNewSetDefaultTemplate" runat="server">
						<asp:ListItem Value="1" Text="Yes" />
						<asp:ListItem Value="0" Selected="True" Text="No" />
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Font Face:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewFontFace" size="20" maxlength="50" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Font Size:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewFontSize" size="3" maxlength="1" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Font Color:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewFontColor" size="20" maxlength="20" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Link Color:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewLinkColor" size="20" maxlength="20" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Link Decoration:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewLinkDecoration" size="20" maxlength="20" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Visited Link Color:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewLinkVisitedColor" size="20" maxlength="20" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Visited Link Decoration:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewLinkVisitedDecoration" size="20" maxlength="20" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Active Link Color:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewLinkActiveColor" size="20" maxlength="20" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Active Link Decoration:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewLinkActiveDecoration" size="20" maxlength="20" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Hover Link Color:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewLinkHoverColor" size="20" maxlength="20" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Visited Link Decoration:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewLinkHoverDecoration" size="20" maxlength="20" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Background Image URL:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewBGImage" size="20" maxlength="50" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Background Color:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewBGColor" size="20" maxlength="20" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Topics Font Size:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewTopicsFontSize" size="3" maxlength="1" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Topics Font Color:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewTopicsFontColor" size="20" maxlength="20" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Topics BG Color:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewTopicsBGColor1" size="20" maxlength="20" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Alternating BG Color:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewTopicsBGColor2" size="20" maxlength="20" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Button Color:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewButtonColor" size="20" maxlength="20" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Scrollbar Color:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewScrollbarColor" size="20" maxlength="20" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Table Border Color:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewTableborderColor" size="20" maxlength="20" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Login Font Color:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewLoginFontColor" size="20" maxlength="20" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Header Size:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewHeaderSize" size="1" maxlength="3" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Header Color:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewHeaderColor" size="20" maxlength="20" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Header Font Color:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewHeaderFontColor" size="20" maxlength="20" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Sub-Header Color:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewSubheaderColor" size="20" maxlength="20" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Sub-Header Font Color:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewSubheaderFontColor" size="20" maxlength="20" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Footer Size:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewFooterSize" size="1" maxlength="3" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Footer Color:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewFooterColor" size="20" maxlength="20" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Footer Font Color:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewFooterFontColor" size="20" maxlength="20" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Custom Header HTML:</b><br /><br />(HTML Appearing Above Content)<br /><br /><a href="javascript:openHelp('DMGAdminCode.html')">DMG Admin Code</a><br />Allowed</font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewCustomHeader" Columns="65" Rows="13" Textmode="multiline" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Custom Footer HTML:</b><br /><br />(HTML Appearing Below Content)<br /><br /><a href="javascript:openHelp('DMGAdminCode.html')">DMG Admin Code</a><br />Allowed</font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewCustomFooter" Columns="65" Rows="13" Textmode="multiline" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Custom CSS And JavaScript:</b><br /><br />(HTML Appearing Between &lt;HEAD&gt;&lt;/HEAD&gt; Tags)<br /><br /><a href="javascript:openHelp('DMGAdminCode.html')">DMG Admin Code</a><br />Allowed</font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewCustomCSS" Columns="65" Rows="13" Textmode="multiline" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Side Margins (pixels):</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewMarginSide" size="5" maxlength="5" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Top Margin (pixels):</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtDMGNewMarginTop" size="5" maxlength="5" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="top"></td>
				<td align="left" valign="middle">
					<asp:Button type="submit" id="NewTemplateSubmit" onclick="SubmitNewTemplate" text="Submit New Template" runat="server" />
				</td>
			</tr>
			</table>
		</asp:Panel>


		<asp:Panel id="AdminDeleteTemplatePanel" runat="server" visible="false">
			<table border="0" align="center">
			<tr>
			<td align="center" valign="top">
				<font size="3" color="<%=Settings.TopicsFontColor%>"><b>Click A Template To Delete</b></font><br /><br />
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Warning: This Can Not Be Undone</b><br /><br />
				<asp:Repeater id="TemplatesList" runat="server">
					<ItemTemplate>
						<asp:LinkButton runat="server" ID="Link" Text='<%# DataBinder.Eval(Container.DataItem, "DMG_TEMPLATE_NAME") %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' OnClick="DeleteTemplate" /><br /><br />
					</ItemTemplate>
				</asp:Repeater>
				</font>
			</td>
			</tr>
			</table>
		</asp:Panel>


		<asp:Panel id="AdminHtmlFormConfig" runat="server" visible="false">
			<asp:Repeater id="HtmlFormList" runat="server">
				<HeaderTemplate>
					<br />
					<table border="2" cellpadding="10" align="center">
					<tr class="TableRow2">
					<td align="center" valign="middle" height="40" colspan="3">
						<font size="4" color="<%=Settings.TopicsFontColor%>"><b>HTML Form Results <a href="javascript:openHelp('DMGHtmlForms.html')">[?]</a></b></font>
					</td>
					</tr>
					<tr>
					<td align="left">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Form Name</b></font>
					</td>
					<td align="center">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Date Submitted</b></font>
					</td>
					<td align="center">
						&nbsp;
					</td>
					</tr>
				</HeaderTemplate>
				<ItemTemplate>
					<tr>
					<td align="left" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><%# IIF(DataBinder.Eval(Container.DataItem, "FORM_NEW") = 1, "<b>[New]</b> ", "") %><asp:LinkButton runat="server" ID="HtmlFormView" onClick="ViewHtmlForm" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "FORM_ID") %>'><%# DataBinder.Eval(Container.DataItem, "FORM_NAME") %></asp:LinkButton></font>
					</td>
					<td align="center" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><%# DataBinder.Eval(Container.DataItem, "FORM_DATE") %></font>
					</td>
					<td>
						<asp:button id="DeleteHtmlFormButton" onclick="DeleteHtmlForm" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "FORM_ID") %>' CssClass="dmgbuttons" runat="server" Text="DELETE" />
					</td>
					</tr>
				</ItemTemplate>
				<FooterTemplate>
					</table>
					<br />
				</FooterTemplate>
			</asp:Repeater>
		</asp:Panel>


		<asp:Panel id="AdminHtmlFormView" runat="server" visible="false">
			<asp:Repeater id="HtmlFormResults" runat="server">
				<ItemTemplate>
					<table border="0" cellpadding="6" align="left">
					<tr>
						<td align="right" valign="middle">
							<font size="3" color="<%=Settings.TopicsFontColor%>"><b>Form Name:</b></font>
						</td>
						<td align="left" valign="middle">
							<font size="3" color="<%=Settings.TopicsFontColor%>"><%# DataBinder.Eval(Container.DataItem, "FORM_NAME") %></font>
						</td>
					</tr>
					<tr>
						<td align="right" valign="middle">
							<font size="3" color="<%=Settings.TopicsFontColor%>"><b>Date Submitted:</b></font>
						</td>
						<td align="left" valign="middle">
							<font size="3" color="<%=Settings.TopicsFontColor%>"><%# DataBinder.Eval(Container.DataItem, "FORM_DATE") %></font>
						</td>
					</tr>
					<tr>
						<td align="right" valign="middle">
							<font size="3" color="<%=Settings.TopicsFontColor%>"><b>Email Sent?</b></font>
						</td>
						<td align="left" valign="middle">
							<font size="3" color="<%=Settings.TopicsFontColor%>"><%# IIF(DataBinder.Eval(Container.DataItem, "FORM_EMAIL") = 1, "Yes", "No") %></font>
						</td>
					</tr>
					</table>
					<br clear="all" />
					<hr />
					<table border="0" cellpadding="6" align="left">
					<tr>
						<td>
							<font color="<%=Settings.TopicsFontColor%>">
								<%# DataBinder.Eval(Container.DataItem, "FORM_TEXT") %>
							</font>
						</td>
					</tr>
					</table>
					<br clear="all" />
					<hr />
					<table border="0" cellpadding="6" align="left">
					<tr>
						<td>
							<asp:button id="DeleteHtmlFormButton2" onclick="DeleteHtmlForm" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "FORM_ID") %>' runat="server" Text="DELETE FORM RESULTS" />
						</td>
					</tr>
					</table>
					<br />
				</ItemTemplate>
			</asp:Repeater>
		</asp:Panel>


		<asp:Panel id="AdminHtmlFormDelete" runat="server" visible="false">
			<br />
			<center>
				<font color="<%=Settings.TopicsFontColor%>">
				<b>Are you sure you want to delete these form results?</b>
				<br /><br />
				<asp:button id="HtmlFormYesButton" onclick="DeleteHtmlFormConfirm" runat="server" Text="Yes" />
				&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				<asp:button id="HtmlFormNoButton" onclick="OpenConfig" CommandArgument="16" runat="server" Text="No" />
				</font>
			</center>
			<br />
		</asp:Panel>

		<asp:Panel id="AdminBannedIPConfig" runat="server" visible="false">
			<br />
			<asp:Repeater id="IPList" runat="server">
				<HeaderTemplate>
					<table border="1" cellpadding="6" align="center">
					<tr class="TableRow2">
					<td align="center" valign="middle" height="40" colspan="3">
						<font size="4" color="<%=Settings.TopicsFontColor%>"><b>Banned IP Addresses</b></font>
					</td>
					</tr>
					<tr>
					<td align="center">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>IP Address</b></font>
					</td>
					<td align="center">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Members Banned</b></font>
					</td>
					<td align="center">
						&nbsp;
					</td>
					</tr>
				</HeaderTemplate>
				<ItemTemplate>
					<tr>
					<td align="center" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><%# DataBinder.Eval(Container.DataItem, "IP_ADDRESS") %></font>
					</td>
					<td align="left" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>">
							<asp:Repeater id="IPMembers" DataSource='<%# (CType(Container.DataItem,DataRowView)).Row.GetChildRows("IPRelation")%>' runat="server">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "(""MEMBER_USERNAME"")") %><br />
								</ItemTemplate>
							</asp:Repeater>
						</font>
					</td>
					<td align="center" valign="top">
						<asp:button id="RestoreIPButton" onclick="RestoreIP" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IP_ADDRESS") %>' CssClass="dmgbuttons" runat="server" Text="RESTORE" />
					</td>
					</tr>
				</ItemTemplate>

				<FooterTemplate>
					</table>
					<br />
				</FooterTemplate>
			</asp:Repeater>
		</asp:Panel>


		<asp:Panel id="AdminGalleryConfig" runat="server" visible="false">
			<table border="2" cellpadding="10" align="center" width="700">
			<tr class="TableRow2">
			<td align="center" valign="middle" height="40">
				<font size="4" color="<%=Settings.TopicsFontColor%>"><b>Photo Galleries</b></font>
			</td>
			</tr>
			<tr>
			<td>
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					After creating a gallery and attaching photos to it, use the [PhotoGallery=#] command on a content page to display the gallery, replacing the # with the ID number of the gallery you would like to display.  By default, the gallery will display with 5 photos per row.  To modify the rows, use the command [PhotoGallery ID=# Columns=#], replacing the ID and Columns values with the ID of the gallery you would like to show and the number of columns you would like to display.
				</font>
			</td>
			</tr>
			<tr>
			<td align="center">
				<asp:Button runat="server" ID="NewGallery" Text="Create New Photo Gallery" onClick="CreateGallery" CssClass="AdminButtons" />
			</td>
			</tr>
			<asp:Repeater id="GalleryList" runat="server">
				<HeaderTemplate>
					<tr>
					<td>
						<br />
						<table border="1" cellpadding="6" align="center">
						<tr>
						<td colspan="3" align="center">
							<font size="3" color="<%=Settings.TopicsFontColor%>"><b>Existing Photo Galleries</b></font>
						</td>
						</tr>
						<tr>
						<td align="center">
							<font size="2" color="<%=Settings.TopicsFontColor%>"><b>ID</b></font>
						</td>
						<td align="center">
							<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Name</b></font>
						</td>
						<td align="center">
							&nbsp;
						</td>
						</tr>
				</HeaderTemplate>
				<ItemTemplate>
						<tr>
						<td align="center" valign="middle">
							<font size="2" color="<%=Settings.TopicsFontColor%>"><%# DataBinder.Eval(Container.DataItem, "GALLERY_ID") %></font>
						</td>
						<td align="center" valign="middle">
							<font size="2" color="<%=Settings.TopicsFontColor%>"><%# DataBinder.Eval(Container.DataItem, "GALLERY_NAME") %></font>
						</td>
						<td>
							<asp:button id="EditGalleryButton" onclick="EditGallery" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "GALLERY_ID") %>' CssClass="dmgbuttons" runat="server" Text="EDIT IMAGES" />&nbsp;
							<asp:button id="DeleteGalleryButton" onclick="DeleteGallery" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "GALLERY_ID") %>' CssClass="dmgbuttons" runat="server" Text="DELETE" />
						</td>
						</tr>
				</ItemTemplate>
				<FooterTemplate>
						</table>
						<br />
					</td>
					</tr>
				</FooterTemplate>
			</asp:Repeater>
			</table>
		</asp:Panel>


		<asp:Panel id="AdminGalleryNew" runat="server" visible="false">
			<table border="0" cellpadding="6" align="center">
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Gallery Name:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="NewGalleryName" size="30" maxlength="30" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="top"></td>
				<td align="left" valign="middle">
					<asp:Button type="submit" id="AdminGallerySubmit" onclick="SubmitNewGallery" text="Submit" runat="server" />
				</td>
			</tr>
			</table>
		</asp:Panel>


		<asp:Panel id="AdminGalleryNewConfirm" runat="server" visible="false">
			<br />
			<center>
				<font size="3" color="<%=Settings.TopicsFontColor%>"><b>New Photo Gallery Created</b></font>
				<br /><br />
				<asp:button id="NewGalleryEdit" onclick="EditGallery" runat="server" Text="Add Images To Gallery" CssClass="AdminButtons" />
				<br /><br />
				<asp:button id="NewGalleryReturn" onclick="OpenConfig" CommandArgument="18" runat="server" Text="Return To Photo Galleries" CssClass="AdminButtons" />
			</center>
			<br />
		</asp:Panel>


		<asp:Panel id="AdminGalleryDelete" runat="server" visible="false">
			<br />
			<center>
				<font color="<%=Settings.TopicsFontColor%>">
				<b>Are you sure you want to delete this photo gallery?</b>
				<br /><br />
				Note: All images for this gallery will also be deleted.
				<br /><br />
				<asp:button id="GalleryYesButton" onclick="DeleteGalleryConfirm" runat="server" Text="Yes" />
				&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				<asp:button id="GalleryNoButton" onclick="OpenConfig" CommandArgument="18" runat="server" Text="No" />
				</font>
			</center>
			<br />
		</asp:Panel>


		<asp:Panel id="AdminGalleryEdit" runat="server" visible="false">
			<br />
			<table border="0" align="center">
			<tr>
			<td>
				<font size="3"><b>
				<li /><a href="javascript:openUploader('upload.aspx?TYPE=photogallery&GALLERY=<%=GalleryID%>')">Upload New Image</a><br /><br />
				<li /><asp:LinkButton runat="server" ID="GalleryRefresh" Text="Refresh List" onClick="EditGallery" />
				</b></font>
			</td>
			</tr>
			</table>
			<br />
				<center>
				<hr width="50%" noshade">
				<br />
				<font size="3" color="<%=Settings.TopicsFontColor%>"><b>Photos For Gallery ID (<%=GalleryID%>)</b></font>
				</center>
			<br />
			<asp:DataList id="GalleryPhotos" runat="server" AutoGenerateColumns="False" border="0" CellPadding="10" RepeatColumns="5" align="center">
				<ItemTemplate>
					<center>
					<font size="2">
						<a href="javascript:openPhoto('showphoto.aspx?PHOTO=photogalleries/<%# DataBinder.Eval(Container.DataItem, "PHOTO_ID") %>.<%# DataBinder.Eval(Container.DataItem, "PHOTO_EXTENSION") %>')"><img src="photogalleries/<%# DataBinder.Eval(Container.DataItem, "PHOTO_ID") %>_s.<%# DataBinder.Eval(Container.DataItem, "PHOTO_EXTENSION") %>"><br /><%# DataBinder.Eval(Container.DataItem, "PHOTO_DESCRIPTION") %></a><br />
						<asp:button id="DeleteGalleryPhotoButton" onclick="DeleteGalleryPhoto" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PHOTO_ID") %>' CssClass="dmgbuttons" runat="server" Text="DELETE" />
					</font>
					</center>
				</ItemTemplate>
			</asp:DataList>
			<br />
		</asp:Panel>


		<asp:Panel id="AdminCustomMessagesConfig" runat="server" visible="false">
			<table border="2" cellpadding="10" align="center">
			<tr class="TableRow2">
			<td width="50%" align="center" valign="middle" height="40">
				<font size="4" color="<%=Settings.TopicsFontColor%>"><b>Custom Message Dialogs</b></font>
			</td>
			<td width="50%" align="center" valign="middle" height="40">
				<font size="4" color="<%=Settings.TopicsFontColor%>"><b>Custom E-Mail Messages</b></font>
			</td>
			</tr>
			<tr>
			<td width="50%">
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					These messages appear when users perform certain actions on the site.  Some of them are related to functions that are not enabled on a default install, such as confirmation of a new user.  These functions must be enabled before the dialog boxes will appear on the site.
				</font>
			</td>
			<td width="50%">
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					These e-mail messages are sent after a user performs certain actions on the site.  As with custom dialogs, these e-mails are only enabled when the functions are activated.
				</font>
			</td>
			</tr>
			<tr>
			<td width="50%" valign="top">
				<table border="0" cellpadding="6" align="center">
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Privacy Notice</b><br /><br /></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtMessagePrivacyNotice" Columns="50" Rows="5" Textmode="multiline" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Normal Registration Confirmation</b><br /><br /></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtMessageRegistration" Columns="50" Rows="5" Textmode="multiline" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Registration Confirmation With E-Mail Validation Required</b><br /><br /></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtMessageSendKey" Columns="50" Rows="5" Textmode="multiline" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Registration Confirmation With Admin Approval Required</b><br /><br /></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtMessageAdminApproval" Columns="50" Rows="5" Textmode="multiline" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Successful Validation Key Message</b><br /><br /></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtMessageValidation" Columns="50" Rows="5" Textmode="multiline" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Topics/Replies Posted With Moderator Approval Required</b><br /><br /></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtMessageConfirmPost" Columns="50" Rows="5" Textmode="multiline" runat="server" />
					</td>
				</tr>
				</table>
			</td>
			<td width="50%" valign="top">
				<table border="0" cellpadding="6" align="center">
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Welcome Message</b><br /><br /></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtEmailWelcomeMessage" Columns="50" Rows="5" Textmode="multiline" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Admin Has Approved Your Registration</b><br /><br /></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtEmailAdminApproval" Columns="50" Rows="5" Textmode="multiline" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Verification Key For Confirming Registration Through Email</b><br /><br /></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtEmailSendKey" Columns="50" Rows="5" Textmode="multiline" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Notification Sent To Moderators When Post Requires Approval</b><br /><br /></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtEmailConfirmPost" Columns="50" Rows="5" Textmode="multiline" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Thread Subscription Update</b><br /><br /></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtEmailSubscription" Columns="50" Rows="5" Textmode="multiline" runat="server" />
					</td>
				</tr>
				</table>
			</td>
			</tr>
			<tr>
				<td align="center" valign="middle" colspan="2">
					<asp:Button type="submit" id="CustomMessageSubmitButton" onclick="SubmitCustomMessages" text="Save Changes" CssClass="AdminButtons" runat="server" />
				</td>
			</tr>
			</table>
		</asp:Panel>


		<asp:Panel id="AdminSearchConfig" runat="server" visible="false">
			<table border="2" cellpadding="10" align="center" width="700">
			<tr class="TableRow2">
			<td align="center" valign="middle" height="40">
				<font size="4" color="<%=Settings.TopicsFontColor%>"><b>Search Configuration</b></font>
			</td>
			</tr>
			<tr>
			<td>
				<table border="0" cellpadding="6" align="center">
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Topic Search:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtDMGSearchTopics" runat="server">
							<asp:ListItem Value="1" Text="On" />
							<asp:ListItem Value="0" Text="Off" />
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Member Search:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtDMGSearchMembers" runat="server">
							<asp:ListItem Value="1" Text="On" />
							<asp:ListItem Value="0" Text="Off" />
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Blog Search:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtDMGSearchBlogs" runat="server">
							<asp:ListItem Value="1" Text="On" />
							<asp:ListItem Value="0" Text="Off" />
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Page Search:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:DropDownList id="txtDMGSearchPages" runat="server">
							<asp:ListItem Value="1" Text="On" />
							<asp:ListItem Value="0" Text="Off" />
						</asp:DropDownList>
					</td>
				</tr>
				</table>
			</td>
			</tr>
			<tr>
				<td align="center" valign="middle">
					<asp:Button type="submit" id="AdminSearchConfigSubmit" onclick="SubmitSearchConfig" text="Save Changes" CssClass="AdminButtons" runat="server" />
				</td>
			</tr>
			</table>
		</asp:Panel>


		<asp:Panel id="AdminPMCleanup" runat="server" visible="false">
			<table border="2" cellpadding="10" align="center" width="700">
			<tr class="TableRow2">
			<td align="center" valign="middle" height="40">
				<font size="4" color="<%=Settings.TopicsFontColor%>"><b>Private Messages Cleanup Tool</b></font>
			</td>
			</tr>
			<td>
				<font size="2" color="<%=Settings.TopicsFontColor%>">
					This tool will allow you to delete private messages from the database.  The information will be deleted entirely and can not be recovered.
				</font>
			</td>
			</tr>
			<tr>
			<td align="center">
				<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Delete Messages That Have Not Been Updated In <asp:Textbox id="txtDMGPMCleanupDays" text="30" size="3" maxlength="10" runat="server" /> Days.</b></font><br /><br />
				<asp:Button type="submit" id="AdminPMCleanupSubmit" onclick="SubmitPMCleanup" text="Submit" CssClass="AdminButtons" runat="server" />
		</asp:Panel>


		<asp:Panel id="AboutDMG" runat="server" visible="false">
			<br />
			<table border="2" cellpadding="10" align="center">
			<tr class="TableRow2">
			<td width="50%" align="center" valign="middle" height="40">
				<font size="4" color="<%=Settings.TopicsFontColor%>"><b>About DMG Forums</b></font>
			</td>
			<td width="50%" align="center" valign="middle" height="40">
				<font size="4" color="<%=Settings.TopicsFontColor%>"><b>Database Information</b></font>
			</td>
			</tr>
			<tr>
			<td width="50%" valign="top">
				<br />
				<table border="1" align="center" width="100%">
				<tr>
				<td align="right">
					<font size="3" color="<%=Settings.TopicsFontColor%>"><b>Current Version:</b></font>
				</td>
				<td align="left">
					<font size="3" color="<%=Settings.TopicsFontColor%>"><%=Settings.DMGVersion%></font>
				</td>
				</tr>
				<tr>
				<td align="right">
					<font size="3" color="<%=Settings.TopicsFontColor%>"><b>Release Date:</b></font>
				</td>
				<td align="left">
					<font size="3" color="<%=Settings.TopicsFontColor%>"><%=Settings.DMGReleaseDate%></font>
				</td>
				</tr>
				<tr>
				<td align="right">
					<font size="3" color="<%=Settings.TopicsFontColor%>"><b>Platform:</b></font>
				</td>
				<td align="left">
					<font size="3" color="<%=Settings.TopicsFontColor%>">ASP.NET 2.0</font>
				</td>
				</tr>
				<tr>
				<td align="right">
					<font size="3" color="<%=Settings.TopicsFontColor%>"><b>License:</b></font>
				</td>
				<td align="left">
					<font size="3" color="<%=Settings.TopicsFontColor%>">Open Source (Visit Website For EULA)</font>
				</td>
				</tr>
				<tr>
				<td align="right">
					<font size="3" color="<%=Settings.TopicsFontColor%>"><b>Copyright:</b></font>
				</td>
				<td align="left">
					<font size="3" color="<%=Settings.TopicsFontColor%>">&copy; 2005-2009 DMG Development</font>
				</td>
				</tr>
				<tr>
				<td align="right">
					<font size="3" color="<%=Settings.TopicsFontColor%>"><b>Support:</b></font>
				</td>
				<td align="left">
					<font size="3" color="<%=Settings.TopicsFontColor%>"><a target="_blank" href="http://www.dmgforums.com">http://www.DMGForums.com</a></font>
				</td>
				</tr>
				</table>
				<br />
			</td>
			<td width="50%" valign="top">
				<br />
				<table border="1" align="center" width="100%">
				<tr>
				<td align="right">
					<font size="3" color="<%=Settings.TopicsFontColor%>"><b>Server:</b></font>
				</td>
				<td align="left">
					<font size="3" color="<%=Settings.TopicsFontColor%>"><%=DMGForums.Global.Database.GetServerName()%></font>
				</td>
				</tr>
				<tr>
				<td align="right">
					<font size="3" color="<%=Settings.TopicsFontColor%>"><b>Database:</b></font>
				</td>
				<td align="left">
					<font size="3" color="<%=Settings.TopicsFontColor%>"><%=DMGForums.Global.Database.GetDBName()%></font>
				</td>
				</tr>
				<tr>
				<td align="right">
					<font size="3" color="<%=Settings.TopicsFontColor%>"><b>DB Prefix:</b></font>
				</td>
				<td align="left">
					<font size="3" color="<%=Settings.TopicsFontColor%>"><%=DMGForums.Global.Database.DBPrefix%></font>
				</td>
				</tr>
				<tr>
				<td align="right">
					<font size="3" color="<%=Settings.TopicsFontColor%>"><b>ODBC Driver:</b></font>
				</td>
				<td align="left">
					<font size="3" color="<%=Settings.TopicsFontColor%>"><%=DMGForums.Global.Database.Type()%></font>
				</td>
				</tr>
				</table>
				<br />
			</td>
			</tr>
			</table>
			</br >
		</asp:Panel>

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