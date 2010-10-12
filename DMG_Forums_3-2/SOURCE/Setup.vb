'#########################################################################################################
' DMG Forums
' Copyright (c) 2005-2009
' by Dustin Grimmeissen and DMG Development
' 
' The DMG Forums application is free and open source software.  It may be modified and deployed in any way
' and in any environment.  The application may not be redistributed for profit without the express written
' consent of Dustin Grimmeissen and DMG Development, based in the United States of America and the State
' of Indiana.
' 
' All copyright notices within this program, including this one, must remain intact at all times.  The DMG
' Forums hyperlink with link back to http://www.dmgforums.com must remain visible in your outputted HTML.
' 
' For the full end-user license agreement, visit the license agreement section of http://www.dmgforums.com
'#########################################################################################################

Imports System
Imports System.Data
Imports System.Data.Odbc
Imports System.Web.UI.WebControls
Imports Microsoft.VisualBasic
Imports DMGForums.Global

Namespace DMGForums.Setup

	'---------------------------------------------------------------------------------------------------
	' SQLSetup - Codebehind For setup.aspx
	'---------------------------------------------------------------------------------------------------
	Public Class SQLSetup

		Inherits System.Web.UI.Page

		Public AdminUsername As System.Web.UI.WebControls.TextBox
		Public AdminPassword As System.Web.UI.WebControls.TextBox
		Public AdminEmail As System.Web.UI.WebControls.TextBox
		Public ForumTitle As System.Web.UI.WebControls.TextBox
		Public WebSiteURL As System.Web.UI.WebControls.TextBox
		Public InstallationType As System.Web.UI.WebControls.DropDownList
		Public InstallAllTemplates As System.Web.UI.WebControls.DropDownList
		Public CharacterEncoding As System.Web.UI.WebControls.DropDownList
		Public InformationPanel As System.Web.UI.WebControls.Panel
		Public TemplatePanel As System.Web.UI.WebControls.Panel
		Public FinalOptionsPanel As System.Web.UI.WebControls.Panel
		Public MessagePanel As System.Web.UI.WebControls.Panel
		Public Message As System.Web.UI.WebControls.Label

		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
			if Not Page.IsPostBack() then
				Dim DBColumn as String = ""
				Dim DBTable as String = ""
				if (Database.DBType = "MySQL") then
					DBColumn = "table_name"
					DBTable = "information_schema.tables WHERE table_schema = '" & Database.GetDBName & "' AND table_name = '" & Database.DBPrefix & "_SETTINGS'"
				else
					DBColumn = "NAME"
					DBTable = "sysobjects WHERE NAME = '" & Database.DBPrefix & "_SETTINGS'"
				end if

				Try
					Dim DatabaseCheck as OdbcDataReader = Database.Read("SELECT " & DBColumn & " FROM " & DBTable)
						if DatabaseCheck.HasRows() then
							InformationPanel.visible = "false"
							MessagePanel.visible = "true"
							Message.text = "You have already installed your database.  Please delete this file from your server for security.<br /><br />If you are attempting to update your forums to the latest version, <a href=""setup-update.aspx"">click here</a>."
						else
							InformationPanel.visible = "true"
						end if
					DatabaseCheck.Close()
				Catch e1 as System.Data.Odbc.OdbcException
					MessagePanel.visible = "true"
					Message.text = "<b>ERROR:</b> There was a SQL error during setup.  View the error message below and then <a href=""setup.aspx"">Click Here</a> to try again.<br /><br />" & e1.ToString()
				Catch e2 as Exception
					MessagePanel.visible = "true"
					Message.text = "<b>ERROR:</b> There was an error during setup.  View the error message below and then <a href=""setup.aspx"">Click Here</a> to try again.<br /><br />" & e2.ToString()
				End Try
			end if
		End Sub

		Sub SubmitInformation(sender As System.Object, e As System.EventArgs)
			InformationPanel.visible = "false"
			TemplatePanel.visible = "true"
			Application("AdminUsername") = Functions.RepairString(AdminUsername.text)
			Application("AdminPassword") = Functions.Encrypt(AdminPassword.text)
			Application("AdminEmail") = AdminEmail.text
			Application("ForumTitle") = Functions.RepairString(ForumTitle.text, 0)
			Application("WebSiteURL") = Functions.RepairString(WebSiteURL.text, 0)
		End Sub

		Sub SubmitTemplate(sender As System.Object, e As System.EventArgs)
			TemplatePanel.visible = "false"
			FinalOptionsPanel.visible = "true"
			Application("AdminUsername") = Application("AdminUsername")
			Application("AdminPassword") = Application("AdminPassword")
			Application("AdminEmail") = Application("AdminEmail")
			Application("ForumTitle") = Application("ForumTitle")
			Application("WebSiteURL") = Application("WebSiteURL")
			Application("DefaultTemplate") = Sender.CommandArgument
		End Sub

		Sub InstallDMGForums(sender As System.Object, e As System.EventArgs)
			InformationPanel.visible = "false"
			TemplatePanel.visible = "false"
			FinalOptionsPanel.visible = "false"
			MessagePanel.visible = "true"
			Dim txtAdminUsername as String = Application("AdminUsername")
			Dim txtAdminPassword as String = Application("AdminPassword")
			Dim txtAdminEmail as String = Application("AdminEmail")
			Dim txtForumTitle as String = Application("ForumTitle")
			Dim txtWebSiteURL as String = Application("WebSiteURL")
			Dim txtDefaultTemplate as String = Application("DefaultTemplate")
			Dim txtInstallationType as Integer = cLng(InstallationType.SelectedValue)
			Dim txtInstallAllTemplates as Integer = cLng(InstallAllTemplates.SelectedValue)
			Dim txtCharacterEncoding as Integer = cLng(CharacterEncoding.SelectedValue)
			Dim TextEncoding as String = "text"
			Dim CharEncoding as String = "varchar"
			Dim CharacterSet as String = ""
			if (txtCharacterEncoding = 2) then
				if (Database.DBType = "MySQL") then
					CharacterSet = " CHARACTER SET utf8"
				else
					TextEncoding = "ntext"
					CharEncoding = "nvarchar"
				end if
			end if
			Try
				Message.text = "You have successfully installed your forum.  <a href=""../default.aspx"">Click Here</a> to view the forums and log in to make changes to the layout and content.<br /><br /><b>Be sure to delete the install files from your server for security purposes!<br /><br />Also remember to change to a database security account that does not have full admin privileges.</b>"
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_AVATARS(AVATAR_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", AVATAR_NAME " & CharEncoding & "(30)" & CharacterSet & ", AVATAR_IMAGE " & CharEncoding & "(50)" & CharacterSet & ")")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_CATEGORIES(CATEGORY_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", CATEGORY_NAME " & CharEncoding & "(100)" & CharacterSet & ", CATEGORY_STATUS int, CATEGORY_SORTBY int, CATEGORY_CONTENT " & TextEncoding & CharacterSet & ", CATEGORY_SHOWHEADERS int, CATEGORY_SHOWLOGIN int)")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_FORUMS(FORUM_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", CATEGORY_ID int, FORUM_STATUS int, FORUM_SORTBY int, FORUM_NAME " & CharEncoding & "(100)" & CharacterSet & ", FORUM_DESCRIPTION " & TextEncoding & CharacterSet & ", FORUM_TOPICS int, FORUM_POSTS int, FORUM_LASTPOST_DATE datetime, FORUM_LASTPOST_AUTHOR int, FORUM_LASTPOST_TOPIC int, FORUM_TYPE int, FORUM_PASSWORD " & CharEncoding & "(50)" & CharacterSet & ", FORUM_FORCECONFIRM int, FORUM_SHOWLATEST int, FORUM_CONTENT " & TextEncoding & CharacterSet & ", FORUM_SHOWHEADERS int, FORUM_SHOWLOGIN int, FORUM_EMAIL_MODERATORS int)")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_MEMBERS(MEMBER_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", MEMBER_USERNAME " & CharEncoding & "(50)" & CharacterSet & ", MEMBER_PASSWORD " & CharEncoding & "(100)" & CharacterSet & ", MEMBER_LEVEL int, MEMBER_EMAIL " & CharEncoding & "(50)" & CharacterSet & ", MEMBER_LOCATION " & CharEncoding & "(50)" & CharacterSet & ", MEMBER_HOMEPAGE " & CharEncoding & "(100)" & CharacterSet & ", MEMBER_SIGNATURE " & TextEncoding & CharacterSet & ", MEMBER_SIGNATURE_SHOW int, MEMBER_IM_AOL " & CharEncoding & "(30)" & CharacterSet & ", MEMBER_IM_ICQ " & CharEncoding & "(30)" & CharacterSet & ", MEMBER_IM_MSN " & CharEncoding & "(30)" & CharacterSet & ", MEMBER_IM_YAHOO " & CharEncoding & "(30)" & CharacterSet & ", MEMBER_POSTS int, MEMBER_DATE_JOINED datetime, MEMBER_DATE_LASTVISIT datetime, MEMBER_DATE_LASTPOST datetime, MEMBER_TITLE " & CharEncoding & "(30)" & CharacterSet & ", MEMBER_TITLE_ALLOWCUSTOM int, MEMBER_TITLE_USECUSTOM int, MEMBER_EMAIL_SHOW int, MEMBER_IP_LAST " & CharEncoding & "(20)" & CharacterSet & ", MEMBER_IP_ORIGINAL " & CharEncoding & "(20)" & CharacterSet & ", MEMBER_REALNAME " & CharEncoding & "(50)" & CharacterSet & ", MEMBER_OCCUPATION " & CharEncoding & "(50)" & CharacterSet & ", MEMBER_SEX " & CharEncoding & "(10)" & CharacterSet & ", MEMBER_AGE " & CharEncoding & "(5)" & CharacterSet & ", MEMBER_BIRTHDAY " & CharEncoding & "(30)" & CharacterSet & ", MEMBER_NOTES " & TextEncoding & CharacterSet & ", MEMBER_FAVORITESITE " & CharEncoding & "(100)" & CharacterSet & ", MEMBER_PHOTO " & CharEncoding & "(100)" & CharacterSet & ", MEMBER_AVATAR int, MEMBER_AVATAR_SHOW int, MEMBER_AVATAR_ALLOWCUSTOM int, MEMBER_AVATAR_USECUSTOM int, MEMBER_AVATAR_CUSTOMLOADED int, MEMBER_AVATAR_CUSTOMTYPE " & CharEncoding & "(10)" & CharacterSet & ", MEMBER_VALIDATED int, MEMBER_VALIDATION_STRING " & CharEncoding & "(100)" & CharacterSet & ", MEMBER_RANKING int)")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_PRIVILEGED(PRIVILEGED_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", MEMBER_ID int, FORUM_ID int, PRIVILEGED_ACCESS int)")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_RANKINGS(RANK_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", RANK_NAME " & CharEncoding & "(30)" & CharacterSet & ", RANK_IMAGE " & CharEncoding & "(50)" & CharacterSet & ", RANK_POSTS int, RANK_ALLOW_TOPICS int, RANK_ALLOW_AVATAR int, RANK_ALLOW_AVATAR_CUSTOM int, RANK_ALLOW_TITLE int, RANK_ALLOW_UPLOADS int)")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_REPLIES(REPLY_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", TOPIC_ID int, REPLY_MESSAGE " & TextEncoding & CharacterSet & ", REPLY_DATE datetime, REPLY_AUTHOR int, REPLY_SIGNATURE int, REPLY_CONFIRMED int)")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_SETTINGS(ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", " & Database.DBPrefix & "_TITLE " & CharEncoding & "(100)" & CharacterSet & ", " & Database.DBPrefix & "_COPYRIGHT " & CharEncoding & "(100)" & CharacterSet & ", " & Database.DBPrefix & "_LOGO " & CharEncoding & "(100)" & CharacterSet & ", " & Database.DBPrefix & "_URL " & CharEncoding & "(100)" & CharacterSet & ", " & Database.DBPrefix & "_FONTFACE " & CharEncoding & "(50)" & CharacterSet & ", " & Database.DBPrefix & "_FONTSIZE int, " & Database.DBPrefix & "_BUTTON_COLOR " & CharEncoding & "(20)" & CharacterSet & ", " & Database.DBPrefix & "_LOGIN_FONTCOLOR " & CharEncoding & "(20)" & CharacterSet & ", " & Database.DBPrefix & "_HEADER_SIZE int, " & Database.DBPrefix & "_HEADER_COLOR " & CharEncoding & "(20)" & CharacterSet & ", " & Database.DBPrefix & "_HEADER_FONTCOLOR " & CharEncoding & "(20)" & CharacterSet & ", " & Database.DBPrefix & "_SUBHEADER_COLOR " & CharEncoding & "(20)" & CharacterSet & ", " & Database.DBPrefix & "_SUBHEADER_FONTCOLOR " & CharEncoding & "(20)" & CharacterSet & ", " & Database.DBPrefix & "_FOOTER_SIZE int, " & Database.DBPrefix & "_FOOTER_COLOR " & CharEncoding & "(20)" & CharacterSet & ", " & Database.DBPrefix & "_FOOTER_FONTCOLOR " & CharEncoding & "(20)" & CharacterSet & ", " & Database.DBPrefix & "_BGCOLOR " & CharEncoding & "(20)" & CharacterSet & ", " & Database.DBPrefix & "_FONT_COLOR " & CharEncoding & "(20)" & CharacterSet & ", " & Database.DBPrefix & "_LINK_COLOR " & CharEncoding & "(20)" & CharacterSet & ", " & Database.DBPrefix & "_LINK_DECORATION " & CharEncoding & "(20)" & CharacterSet & ", " & Database.DBPrefix & "_LINK_VISITED_COLOR " & CharEncoding & "(20)" & CharacterSet & ", " & Database.DBPrefix & "_LINK_VISITED_DECORATION " & CharEncoding & "(20)" & CharacterSet & ", " & Database.DBPrefix & "_LINK_ACTIVE_COLOR " & CharEncoding & "(20)" & CharacterSet & ", " & Database.DBPrefix & "_LINK_ACTIVE_DECORATION " & CharEncoding & "(20)" & CharacterSet & ", " & Database.DBPrefix & "_LINK_HOVER_COLOR " & CharEncoding & "(20)" & CharacterSet & ", " & Database.DBPrefix & "_LINK_HOVER_DECORATION " & CharEncoding & "(20)" & CharacterSet & ", " & Database.DBPrefix & "_TOPICS_BGCOLOR1 " & CharEncoding & "(20)" & CharacterSet & ", " & Database.DBPrefix & "_TOPICS_BGCOLOR2 " & CharEncoding & "(20)" & CharacterSet & ", " & Database.DBPrefix & "_TABLEBORDER_COLOR " & CharEncoding & "(20)" & CharacterSet & ", " & Database.DBPrefix & "_SCROLLBAR_COLOR " & CharEncoding & "(20)" & CharacterSet & ", " & Database.DBPrefix & "_CUSTOM_HEADER " & TextEncoding & CharacterSet & ", " & Database.DBPrefix & "_CUSTOM_FOOTER " & TextEncoding & CharacterSet & ", " & Database.DBPrefix & "_ITEMS_PER_PAGE int, " & Database.DBPrefix & "_SPAM_FILTER int, " & Database.DBPrefix & "_TEMPLATE_DEFAULT int, " & Database.DBPrefix & "_TEMPLATE_NAME " & CharEncoding & "(30)" & CharacterSet & ", " & Database.DBPrefix & "_FORUMS_DEFAULT int, " & Database.DBPrefix & "_MARGIN_SIDE int, " & Database.DBPrefix & "_MARGIN_TOP int, " & Database.DBPrefix & "_SHOWSTATISTICS int, " & Database.DBPrefix & "_CUSTOM_META " & TextEncoding & CharacterSet & ", " & Database.DBPrefix & "_MEMBER_VALIDATION int, " & Database.DBPrefix & "_EMAIL_SMTP " & CharEncoding & "(50)" & CharacterSet & ", " & Database.DBPrefix & "_EMAIL_ADDRESS " & CharEncoding & "(50)" & CharacterSet & ", " & Database.DBPrefix & "_EMAIL_ALLOWSEND int, " & Database.DBPrefix & "_EMAIL_ALLOWSUB int, " & Database.DBPrefix & "_BGIMAGE " & CharEncoding & "(50)" & CharacterSet & ", " & Database.DBPrefix & "_ALLOWSUB int, " & Database.DBPrefix & "_QUICK_REGISTRATION int, " & Database.DBPrefix & "_EMAIL_PORT " & CharEncoding & "(15)" & CharacterSet & ", " & Database.DBPrefix & "_EMAIL_USERNAME " & CharEncoding & "(30)" & CharacterSet & ", " & Database.DBPrefix & "_EMAIL_PASSWORD " & CharEncoding & "(30)" & CharacterSet & ", " & Database.DBPrefix & "_CURSE_FILTER int, " & Database.DBPrefix & "_RSS_FEEDS int, " & Database.DBPrefix & "_HORIZ_DIVIDE " & CharEncoding & "(50)" & CharacterSet & ", " & Database.DBPrefix & "_VERT_DIVIDE " & CharEncoding & "(50)" & CharacterSet & ", " & Database.DBPrefix & "_ALLOW_EDITS int, " & Database.DBPrefix & "_ALLOW_REGISTRATION int, " & Database.DBPrefix & "_ALLOW_MEDIA int, " & Database.DBPrefix & "_ALLOW_REPORTING int, " & Database.DBPrefix & "_HIDE_MEMBERS int, " & Database.DBPrefix & "_HIDE_LOGIN int, " & Database.DBPrefix & "_MEMBER_PHOTOSIZE int, " & Database.DBPrefix & "_CUSTOM_CSS " & TextEncoding & CharacterSet & ", " & Database.DBPrefix & "_HTML_TITLE " & CharEncoding & "(100)" & CharacterSet & ", " & Database.DBPrefix & "_THUMBNAIL_SIZE int, " & Database.DBPrefix & "_AVATAR_SIZE int, " & Database.DBPrefix & "_SEARCH_TOPICS int, " & Database.DBPrefix & "_SEARCH_MEMBERS int, " & Database.DBPrefix & "_SEARCH_BLOGS int, " & Database.DBPrefix & "_SEARCH_PAGES int, " & Database.DBPrefix & "_MEMBER_FILETYPES " & TextEncoding & CharacterSet & ", " & Database.DBPrefix & "_TOPIC_UPLOADSIZE int, " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE int, " & Database.DBPrefix & "_TOPICS_FONTSIZE int, " & Database.DBPrefix & "_TOPICS_FONTCOLOR " & CharEncoding & "(20)" & CharacterSet & ")")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_TOPICS(TOPIC_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", CATEGORY_ID int, FORUM_ID int, TOPIC_SUBJECT " & CharEncoding & "(100)" & CharacterSet & ", TOPIC_MESSAGE " & TextEncoding & CharacterSet & ", TOPIC_AUTHOR int, TOPIC_DATE datetime, TOPIC_REPLIES int, TOPIC_VIEWS int, TOPIC_LASTPOST_DATE datetime, TOPIC_LASTPOST_AUTHOR int, TOPIC_STICKY int, TOPIC_SIGNATURE int, TOPIC_STATUS int, TOPIC_NEWS int, TOPIC_CONFIRMED int, TOPIC_UNCONFIRMED_REPLIES int, TOPIC_FILEUPLOAD " & CharEncoding & "(100)" & CharacterSet & ")")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_PM_TOPICS(TOPIC_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", TOPIC_FROM int, TOPIC_TO int, TOPIC_SUBJECT " & CharEncoding & "(100)" & CharacterSet & ", TOPIC_MESSAGE " & TextEncoding & CharacterSet & ", TOPIC_DATE datetime, TOPIC_TO_READ int, TOPIC_FROM_READ int, TOPIC_LASTPOST_AUTHOR int, TOPIC_LASTPOST_DATE datetime, TOPIC_REPLIES int, TOPIC_SHOWSENDER int, TOPIC_SHOWRECEIVER int)")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_PM_REPLIES(REPLY_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", TOPIC_ID int, REPLY_AUTHOR int, REPLY_MESSAGE " & TextEncoding & CharacterSet & ", REPLY_DATE datetime)")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_VARIABLES(ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", VAR1 " & CharEncoding & "(50)" & CharacterSet & ", VAR2 " & CharEncoding & "(50)" & CharacterSet & ", VAR3 " & CharEncoding & "(50)" & CharacterSet & ", VAR4 " & CharEncoding & "(50)" & CharacterSet & ", VAR5 " & CharEncoding & "(50)" & CharacterSet & ", TEXT1 " & TextEncoding & CharacterSet & ", TEXT2 " & TextEncoding & CharacterSet & ", TEXT3 " & TextEncoding & CharacterSet & ", TEXT4 " & TextEncoding & CharacterSet & ", TEXT5 " & TextEncoding & CharacterSet & ")")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_PAGES(PAGE_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", PAGE_PARENT int, PAGE_NAME " & CharEncoding & "(100)" & CharacterSet & ", PAGE_TITLE " & CharEncoding & "(100)" & CharacterSet & ", PAGE_CONTENT " & TextEncoding & CharacterSet & ", PAGE_SHOWTITLE int, PAGE_SHOWHEADERS int, PAGE_SHOWLOGIN int, PAGE_STATUS int, PAGE_SORT int, PAGE_AUTOFORMAT int, PAGE_SECURITY int, PAGE_PASSWORD " & CharEncoding & "(100)" & CharacterSet & ", PAGE_SUB_TITLE " & CharEncoding & "(100)" & CharacterSet & ", PAGE_SUB_SHOWTITLE int, PAGE_SUB_COLUMNS int, PAGE_SUB_ALIGN int, PAGE_SUB_STATUS int, PAGE_THUMBNAIL " & CharEncoding & "(50)" & CharacterSet & ", PAGE_PHOTO " & CharEncoding & "(50)" & CharacterSet & ")")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_PAGES_PRIVILEGED(PRIVILEGED_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", MEMBER_ID int, PAGE_ID int, PRIVILEGED_ACCESS int)")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_SUBSCRIPTIONS(SUB_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", SUB_MEMBER int, SUB_TOPIC int, SUB_EMAIL int)")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_BLOG_TOPICS(BLOG_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", BLOG_AUTHOR int, BLOG_DATE datetime, BLOG_REPLIES int, BLOG_TITLE " & CharEncoding & "(100)" & CharacterSet & ", BLOG_TEXT " & TextEncoding & CharacterSet & ")")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_BLOG_REPLIES(BLOG_REPLY_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", BLOG_ID int, BLOG_REPLY_AUTHOR int, BLOG_REPLY_DATE datetime, BLOG_REPLY_TEXT " & TextEncoding & CharacterSet & ")")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_CURSE_FILTER(CURSE_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", CURSE_WORD " & CharEncoding & "(20)" & CharacterSet & ", CURSE_REPLACEMENT " & CharEncoding & "(20)" & CharacterSet & ")")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_MAIN_MENU(LINK_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", LINK_ORDER int, LINK_TEXT " & CharEncoding & "(50)" & CharacterSet & ", LINK_TYPE int, LINK_PARAMETER " & CharEncoding & "(100)" & CharacterSet & ", LINK_WINDOW int)")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_ROTATOR(ROTATOR_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", ROTATOR_NAME " & CharEncoding & "(30)" & CharacterSet & ")")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_ROTATOR_IMAGES(IMAGE_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", ROTATOR_ID int, IMAGE_EXTENSION " & CharEncoding & "(5)" & CharacterSet & ", IMAGE_URL " & CharEncoding & "(100)" & CharacterSet & ", IMAGE_DESCRIPTION " & CharEncoding & "(100)" & CharacterSet & ", IMAGE_WINDOW int, IMAGE_BORDER int)")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_GALLERY(GALLERY_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", GALLERY_NAME " & CharEncoding & "(30)" & CharacterSet & ")")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_GALLERY_PHOTOS(PHOTO_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", GALLERY_ID int, PHOTO_EXTENSION " & CharEncoding & "(5)" & CharacterSet & ", PHOTO_DESCRIPTION " & CharEncoding & "(100)" & CharacterSet & ")")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_HTML_FORMS(FORM_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", FORM_NAME " & CharEncoding & "(50)" & CharacterSet & ", FORM_DATE datetime, FORM_TEXT " & TextEncoding & CharacterSet & ", FORM_NEW int, FORM_EMAIL int)")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_MEMBER_PHOTOS(PHOTO_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", MEMBER_ID int, PHOTO_EXTENSION " & CharEncoding & "(5)" & CharacterSet & ", PHOTO_SIZE int, PHOTO_DESCRIPTION " & CharEncoding & "(100)" & CharacterSet & ")")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_BANNED_IP(ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", MEMBER_ID int, IP_ADDRESS " & CharEncoding & "(50)" & CharacterSet & ")")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_FOLDERS(FOLDER_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", FOLDER_CORE int, FOLDER_PARENT int, FOLDER_NAME " & CharEncoding & "(100)" & CharacterSet & ")")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_FILES(FILE_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", FILE_CORE int, FILE_FOLDER int, FILE_NAME " & CharEncoding & "(100)" & CharacterSet & ")")
				Database.Write("CREATE TABLE " & Database.DBPrefix & "_CUSTOM_MESSAGES(ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", EMAIL_ADMINAPPROVAL " & TextEncoding & CharacterSet & ", EMAIL_SENDKEY " & TextEncoding & CharacterSet & ", EMAIL_CONFIRMPOST " & TextEncoding & CharacterSet & ", EMAIL_SUBSCRIPTION " & TextEncoding & CharacterSet & ", MESSAGE_ADMINAPPROVAL " & TextEncoding & CharacterSet & ", MESSAGE_SENDKEY " & TextEncoding & CharacterSet & ", MESSAGE_REGISTRATION " & TextEncoding & CharacterSet & ", MESSAGE_VALIDATION " & TextEncoding & CharacterSet & ", MESSAGE_CONFIRMPOST " & TextEncoding & CharacterSet & ", EMAIL_WELCOMEMESSAGE " & TextEncoding & CharacterSet & ", MESSAGE_PRIVACYNOTICE " & TextEncoding & CharacterSet & ")")

				if ((txtDefaultTemplate = "BlankTemplate") or (txtInstallAllTemplates = 1)) then
					'BLANK TEMPLATE SETUP
					Database.Write("INSERT INTO " & Database.DBPrefix & "_SETTINGS (" & Database.DBPrefix & "_TITLE, " & Database.DBPrefix & "_COPYRIGHT, " & Database.DBPrefix & "_LOGO, " & Database.DBPrefix & "_URL, " & Database.DBPrefix & "_FONTFACE, " & Database.DBPrefix & "_FONTSIZE, " & Database.DBPrefix & "_BUTTON_COLOR, " & Database.DBPrefix & "_LOGIN_FONTCOLOR, " & Database.DBPrefix & "_HEADER_SIZE, " & Database.DBPrefix & "_HEADER_COLOR, " & Database.DBPrefix & "_HEADER_FONTCOLOR, " & Database.DBPrefix & "_SUBHEADER_COLOR, " & Database.DBPrefix & "_SUBHEADER_FONTCOLOR, " & Database.DBPrefix & "_FOOTER_SIZE, " & Database.DBPrefix & "_FOOTER_COLOR, " & Database.DBPrefix & "_FOOTER_FONTCOLOR, " & Database.DBPrefix & "_BGCOLOR, " & Database.DBPrefix & "_FONT_COLOR, " & Database.DBPrefix & "_LINK_COLOR, " & Database.DBPrefix & "_LINK_DECORATION, " & Database.DBPrefix & "_LINK_VISITED_COLOR, " & Database.DBPrefix & "_LINK_VISITED_DECORATION, " & Database.DBPrefix & "_LINK_ACTIVE_COLOR, " & Database.DBPrefix & "_LINK_ACTIVE_DECORATION, " & Database.DBPrefix & "_LINK_HOVER_COLOR, " & Database.DBPrefix & "_LINK_HOVER_DECORATION, " & Database.DBPrefix & "_TOPICS_BGCOLOR1, " & Database.DBPrefix & "_TOPICS_BGCOLOR2, " & Database.DBPrefix & "_TABLEBORDER_COLOR, " & Database.DBPrefix & "_SCROLLBAR_COLOR, " & Database.DBPrefix & "_CUSTOM_HEADER, " & Database.DBPrefix & "_CUSTOM_FOOTER, " & Database.DBPrefix & "_ITEMS_PER_PAGE, " & Database.DBPrefix & "_SPAM_FILTER, " & Database.DBPrefix & "_TEMPLATE_DEFAULT, " & Database.DBPrefix & "_TEMPLATE_NAME, " & Database.DBPrefix & "_FORUMS_DEFAULT, " & Database.DBPrefix & "_MARGIN_SIDE, " & Database.DBPrefix & "_MARGIN_TOP, " & Database.DBPrefix & "_SHOWSTATISTICS, " & Database.DBPrefix & "_CUSTOM_META, " & Database.DBPrefix & "_MEMBER_VALIDATION, " & Database.DBPrefix & "_EMAIL_SMTP, " & Database.DBPrefix & "_EMAIL_ADDRESS, " & Database.DBPrefix & "_EMAIL_ALLOWSEND, " & Database.DBPrefix & "_EMAIL_ALLOWSUB, " & Database.DBPrefix & "_BGIMAGE, " & Database.DBPrefix & "_ALLOWSUB, " & Database.DBPrefix & "_QUICK_REGISTRATION, " & Database.DBPrefix & "_EMAIL_PORT, " & Database.DBPrefix & "_EMAIL_USERNAME, " & Database.DBPrefix & "_EMAIL_PASSWORD, " & Database.DBPrefix & "_CURSE_FILTER, " & Database.DBPrefix & "_RSS_FEEDS, " & Database.DBPrefix & "_HORIZ_DIVIDE, " & Database.DBPrefix & "_VERT_DIVIDE, " & Database.DBPrefix & "_ALLOW_EDITS, " & Database.DBPrefix & "_ALLOW_REGISTRATION, " & Database.DBPrefix & "_ALLOW_MEDIA, " & Database.DBPrefix & "_ALLOW_REPORTING, " & Database.DBPrefix & "_HIDE_MEMBERS, " & Database.DBPrefix & "_HIDE_LOGIN, " & Database.DBPrefix & "_MEMBER_PHOTOSIZE, " & Database.DBPrefix & "_CUSTOM_CSS, " & Database.DBPrefix & "_HTML_TITLE, " & Database.DBPrefix & "_THUMBNAIL_SIZE, " & Database.DBPrefix & "_AVATAR_SIZE, " & Database.DBPrefix & "_SEARCH_TOPICS, " & Database.DBPrefix & "_SEARCH_MEMBERS, " & Database.DBPrefix & "_SEARCH_BLOGS, " & Database.DBPrefix & "_SEARCH_PAGES, " & Database.DBPrefix & "_MEMBER_FILETYPES, " & Database.DBPrefix & "_TOPIC_UPLOADSIZE, " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE, " & Database.DBPrefix & "_TOPICS_FONTSIZE, " & Database.DBPrefix & "_TOPICS_FONTCOLOR) VALUES ('" & txtForumTitle & "', 'Copyright DMG Development', '', '" & txtWebSiteURL & "', 'arial,helvetica', 2, 'darkblue', 'black', 3, 'darkblue', 'ghostwhite', '#B6B6B6', 'black', 2, 'darkblue', 'ghostwhite', 'silver', 'black', 'darkblue', 'none', 'darkblue', 'none', 'blue', 'underline', 'blue', 'underline', 'gainsboro', 'whitesmoke', '#060237', 'darkblue', '<br /><center><font size=""5"">[PageTitle]</font>" & CHR(10) & CHR(10) & "<br /><br />" & CHR(10) & CHR(10) & "[menu=H]</center>', '', 15, 30, 1, 'Blank Template', " & txtInstallationType & ", 10, 15, 1, 'forums, portal', 0, 'mail.yourserver.com', 'mail@yourserver.com', 0, 0, '', 1, 0, '', '', '', 0, 0, '&nbsp;&nbsp;<b>|</b>&nbsp;&nbsp;', '<br /><br />', 1, 1, 1, 1, 0, 0, 1024, '<STYLE TYPE=""text/css"">" & CHR(10) & "<!--" & CHR(10) & "A:link {text-decoration:[LinkDecoration];color:[LinkColor];}" & CHR(10) & "A:visited {text-decoration:[VLinkDecoration];color:[VLinkColor];}" & CHR(10) & "A:active {text-decoration:[ALinkDecoration];color:[ALinkColor];}" & CHR(10) & "A:hover {text-decoration:[HLinkDecoration];color:[HLinkColor];}" & CHR(10) & ".dmgbuttons{" & CHR(10) & "	font-family: Verdana;" & CHR(10) & "	font-size: 8px;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "	border-color: [ButtonColor];}" & CHR(10) & ".LoginButton{font-size: 12px; border-color: [ButtonColor];}" & CHR(10) & ".AdminButtons{font-size: 18px; width: 270px; border-color: [HeaderColor];}" & CHR(10) & ".LoginBox{border-color: [ButtonColor];}" & CHR(10) & ".ContentBox{border:2px solid [BorderColor];}" & CHR(10) & ".HeaderCell{background-color: [HeaderColor];}" & CHR(10) & ".SubHeaderCell{background-color: [SubHeaderColor];}" & CHR(10) & ".FooterCell{background-color: [FooterColor];}" & CHR(10) & ".TableRow1{background-color: [TableBGColor1];}" & CHR(10) & ".TableRow2{background-color: [TableBGColor2];}" & CHR(10) & ".MessageBox{font-size: 20px;}" & CHR(10) & ".PhotoGalleryTable {border: 0px;}" & CHR(10) & ".PhotoGalleryTable TD{font-size: 13px; padding: 10px; text-align: center;}" & CHR(10) & "-->" & CHR(10) & "</STYLE>', '" & txtForumTitle & "', 150, 125, 1, 1, 1, 1, 'image/gif, image/png, application/msword, image/jpeg, application/zip, text/plain, image/pjpeg, application/mspowerpoint, application/vnd.ms-excel, application/pdf', 1024, 0, 2, 'black')")
				end if

				if ((txtDefaultTemplate = "BlueGradients") or (txtInstallAllTemplates = 1)) then
					'BLUE GRADIENTS SETUP
					Database.Write("INSERT INTO " & Database.DBPrefix & "_SETTINGS (" & Database.DBPrefix & "_TITLE, " & Database.DBPrefix & "_COPYRIGHT, " & Database.DBPrefix & "_LOGO, " & Database.DBPrefix & "_URL, " & Database.DBPrefix & "_FONTFACE, " & Database.DBPrefix & "_FONTSIZE, " & Database.DBPrefix & "_BUTTON_COLOR, " & Database.DBPrefix & "_LOGIN_FONTCOLOR, " & Database.DBPrefix & "_HEADER_SIZE, " & Database.DBPrefix & "_HEADER_COLOR, " & Database.DBPrefix & "_HEADER_FONTCOLOR, " & Database.DBPrefix & "_SUBHEADER_COLOR, " & Database.DBPrefix & "_SUBHEADER_FONTCOLOR, " & Database.DBPrefix & "_FOOTER_SIZE, " & Database.DBPrefix & "_FOOTER_COLOR, " & Database.DBPrefix & "_FOOTER_FONTCOLOR, " & Database.DBPrefix & "_BGCOLOR, " & Database.DBPrefix & "_FONT_COLOR, " & Database.DBPrefix & "_LINK_COLOR, " & Database.DBPrefix & "_LINK_DECORATION, " & Database.DBPrefix & "_LINK_VISITED_COLOR, " & Database.DBPrefix & "_LINK_VISITED_DECORATION, " & Database.DBPrefix & "_LINK_ACTIVE_COLOR, " & Database.DBPrefix & "_LINK_ACTIVE_DECORATION, " & Database.DBPrefix & "_LINK_HOVER_COLOR, " & Database.DBPrefix & "_LINK_HOVER_DECORATION, " & Database.DBPrefix & "_TOPICS_BGCOLOR1, " & Database.DBPrefix & "_TOPICS_BGCOLOR2, " & Database.DBPrefix & "_TABLEBORDER_COLOR, " & Database.DBPrefix & "_SCROLLBAR_COLOR, " & Database.DBPrefix & "_CUSTOM_HEADER, " & Database.DBPrefix & "_CUSTOM_FOOTER, " & Database.DBPrefix & "_ITEMS_PER_PAGE, " & Database.DBPrefix & "_SPAM_FILTER, " & Database.DBPrefix & "_TEMPLATE_DEFAULT, " & Database.DBPrefix & "_TEMPLATE_NAME, " & Database.DBPrefix & "_FORUMS_DEFAULT, " & Database.DBPrefix & "_MARGIN_SIDE, " & Database.DBPrefix & "_MARGIN_TOP, " & Database.DBPrefix & "_SHOWSTATISTICS, " & Database.DBPrefix & "_CUSTOM_META, " & Database.DBPrefix & "_MEMBER_VALIDATION, " & Database.DBPrefix & "_EMAIL_SMTP, " & Database.DBPrefix & "_EMAIL_ADDRESS, " & Database.DBPrefix & "_EMAIL_ALLOWSEND, " & Database.DBPrefix & "_EMAIL_ALLOWSUB, " & Database.DBPrefix & "_BGIMAGE, " & Database.DBPrefix & "_ALLOWSUB, " & Database.DBPrefix & "_QUICK_REGISTRATION, " & Database.DBPrefix & "_EMAIL_PORT, " & Database.DBPrefix & "_EMAIL_USERNAME, " & Database.DBPrefix & "_EMAIL_PASSWORD, " & Database.DBPrefix & "_CURSE_FILTER, " & Database.DBPrefix & "_RSS_FEEDS, " & Database.DBPrefix & "_HORIZ_DIVIDE, " & Database.DBPrefix & "_VERT_DIVIDE, " & Database.DBPrefix & "_ALLOW_EDITS, " & Database.DBPrefix & "_ALLOW_REGISTRATION, " & Database.DBPrefix & "_ALLOW_MEDIA, " & Database.DBPrefix & "_ALLOW_REPORTING, " & Database.DBPrefix & "_HIDE_MEMBERS, " & Database.DBPrefix & "_HIDE_LOGIN, " & Database.DBPrefix & "_MEMBER_PHOTOSIZE, " & Database.DBPrefix & "_CUSTOM_CSS, " & Database.DBPrefix & "_HTML_TITLE, " & Database.DBPrefix & "_THUMBNAIL_SIZE, " & Database.DBPrefix & "_AVATAR_SIZE, " & Database.DBPrefix & "_SEARCH_TOPICS, " & Database.DBPrefix & "_SEARCH_MEMBERS, " & Database.DBPrefix & "_SEARCH_BLOGS, " & Database.DBPrefix & "_SEARCH_PAGES, " & Database.DBPrefix & "_MEMBER_FILETYPES, " & Database.DBPrefix & "_TOPIC_UPLOADSIZE, " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE, " & Database.DBPrefix & "_TOPICS_FONTSIZE, " & Database.DBPrefix & "_TOPICS_FONTCOLOR) VALUES ('" & txtForumTitle & "', 'Copyright DMG Development', '', '" & txtWebSiteURL & "', 'arial,helvetica', 2, '#004EBB', 'black', 3, '#004EBB', 'ghostwhite', '#B6B6B6', 'black', 2, '#004EBB', 'ghostwhite', 'darkgray', 'black', '#002657', 'none', '#002657', 'none', '#004EBB', 'underline', '#004EBB', 'underline', 'gainsboro', 'whitesmoke', '#060237', '#004EBB', '<table border=""2"" bordercolor=""[bordercolor]"" width=""100%"" align=""center"" cellpadding=""5"" cellspacing=""0"">" & CHR(10) & "<tr>" & CHR(10) & "<td width=""100%"" height=""100"" align=""center"" valign=""middle"" bgcolor=""[headercolor]"" background=""forumimages/dmgstyles/bluegradients-topbg.jpg"">" & CHR(10) & "	<span class=""PageTitle"">[PageTitle]</span>" & CHR(10) & "</td>" & CHR(10) & "</tr>" & CHR(10) & "<tr>" & CHR(10) & "<td width=""100%"" align=""left"">" & CHR(10) & CHR(10) & "<br /><center>[menu=H]</center>', '</td>" & CHR(10) & "</tr>" & CHR(10) & "</table>', 15, 30, 1, 'Blue Gradients', " & txtInstallationType & ", 0, 0, 1, 'forums, portal', 0, 'mail.yourserver.com', 'mail@yourserver.com', 0, 0, '', 1, 0, '', '', '', 0, 0, '&nbsp;&nbsp;<b>|</b>&nbsp;&nbsp;', '<br /><br />', 1, 1, 1, 1, 0, 0, 1024, '<STYLE TYPE=""text/css"">" & CHR(10) & "<!--" & CHR(10) & "A:link {text-decoration: [LinkDecoration]; color: [LinkColor]; font-weight: bold;}" & CHR(10) & "A:visited {text-decoration: [VLinkDecoration]; color: [VLinkColor]; font-weight: bold;}" & CHR(10) & "A:active {text-decoration: [ALinkDecoration]; color: [ALinkColor]; font-weight: bold;}" & CHR(10) & "A:hover {text-decoration: [HLinkDecoration]; color: [HLinkColor]; font-weight: bold;}" & CHR(10) & ".dmgbuttons{" & CHR(10) & "	font-family: Verdana;" & CHR(10) & "	font-size: 8px;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "	border-color: [ButtonColor];}" & CHR(10) & ".LoginButton{font-size: 12px; border-color: [ButtonColor];}" & CHR(10) & ".AdminButtons{font-size: 18px; width: 270px; border-color: [HeaderColor];}" & CHR(10) & ".LoginBox{border-color: [ButtonColor];}" & CHR(10) & ".ContentBox {" & CHR(10) & "	border-width: 1px;" & CHR(10) & "	border-style: dashed;" & CHR(10) & "	border-color: [BorderColor];" & CHR(10) & "	border-collapse: separate;" & CHR(10) & "}" & CHR(10) & ".HeaderCell{" & CHR(10) & "	background-color: [HeaderColor];" & CHR(10) & "	background-image: url(forumimages/dmgstyles/bluegradients-headerbg.jpg);" & CHR(10) & "}" & CHR(10) & ".SubHeaderCell{background-color: [SubHeaderColor];}" & CHR(10) & ".FooterCell{" & CHR(10) & "	background-color: [FooterColor];" & CHR(10) & "	background-image: url(forumimages/dmgstyles/bluegradients-headerbg.jpg);" & CHR(10) & "}" & CHR(10) & ".TableRow1{background-color: [TableBGColor1];}" & CHR(10) & ".TableRow2{background-color: [TableBGColor2];}" & CHR(10) & ".MessageBox{font-size: 20px;}" & CHR(10) & ".PageTitle{" & CHR(10) & "	font-size: 55px;" & CHR(10) & "	color: [headerfontcolor];" & CHR(10) & "	font-style: oblique;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "}" & CHR(10) & ".PhotoGalleryTable {border: 0px;}" & CHR(10) & ".PhotoGalleryTable TD{font-size: 13px; padding: 10px; text-align: center;}" & CHR(10) & "-->" & CHR(10) & "</STYLE>', '" & txtForumTitle & "', 150, 125, 1, 1, 1, 1, 'image/gif, image/png, application/msword, image/jpeg, application/zip, text/plain, image/pjpeg, application/mspowerpoint, application/vnd.ms-excel, application/pdf', 1024, 0, 2, 'black')")
				end if

				if ((txtDefaultTemplate = "RedGradients") or (txtInstallAllTemplates = 1)) then
					'RED GRADIENTS SETUP
					Database.Write("INSERT INTO " & Database.DBPrefix & "_SETTINGS (" & Database.DBPrefix & "_TITLE, " & Database.DBPrefix & "_COPYRIGHT, " & Database.DBPrefix & "_LOGO, " & Database.DBPrefix & "_URL, " & Database.DBPrefix & "_FONTFACE, " & Database.DBPrefix & "_FONTSIZE, " & Database.DBPrefix & "_BUTTON_COLOR, " & Database.DBPrefix & "_LOGIN_FONTCOLOR, " & Database.DBPrefix & "_HEADER_SIZE, " & Database.DBPrefix & "_HEADER_COLOR, " & Database.DBPrefix & "_HEADER_FONTCOLOR, " & Database.DBPrefix & "_SUBHEADER_COLOR, " & Database.DBPrefix & "_SUBHEADER_FONTCOLOR, " & Database.DBPrefix & "_FOOTER_SIZE, " & Database.DBPrefix & "_FOOTER_COLOR, " & Database.DBPrefix & "_FOOTER_FONTCOLOR, " & Database.DBPrefix & "_BGCOLOR, " & Database.DBPrefix & "_FONT_COLOR, " & Database.DBPrefix & "_LINK_COLOR, " & Database.DBPrefix & "_LINK_DECORATION, " & Database.DBPrefix & "_LINK_VISITED_COLOR, " & Database.DBPrefix & "_LINK_VISITED_DECORATION, " & Database.DBPrefix & "_LINK_ACTIVE_COLOR, " & Database.DBPrefix & "_LINK_ACTIVE_DECORATION, " & Database.DBPrefix & "_LINK_HOVER_COLOR, " & Database.DBPrefix & "_LINK_HOVER_DECORATION, " & Database.DBPrefix & "_TOPICS_BGCOLOR1, " & Database.DBPrefix & "_TOPICS_BGCOLOR2, " & Database.DBPrefix & "_TABLEBORDER_COLOR, " & Database.DBPrefix & "_SCROLLBAR_COLOR, " & Database.DBPrefix & "_CUSTOM_HEADER, " & Database.DBPrefix & "_CUSTOM_FOOTER, " & Database.DBPrefix & "_ITEMS_PER_PAGE, " & Database.DBPrefix & "_SPAM_FILTER, " & Database.DBPrefix & "_TEMPLATE_DEFAULT, " & Database.DBPrefix & "_TEMPLATE_NAME, " & Database.DBPrefix & "_FORUMS_DEFAULT, " & Database.DBPrefix & "_MARGIN_SIDE, " & Database.DBPrefix & "_MARGIN_TOP, " & Database.DBPrefix & "_SHOWSTATISTICS, " & Database.DBPrefix & "_CUSTOM_META, " & Database.DBPrefix & "_MEMBER_VALIDATION, " & Database.DBPrefix & "_EMAIL_SMTP, " & Database.DBPrefix & "_EMAIL_ADDRESS, " & Database.DBPrefix & "_EMAIL_ALLOWSEND, " & Database.DBPrefix & "_EMAIL_ALLOWSUB, " & Database.DBPrefix & "_BGIMAGE, " & Database.DBPrefix & "_ALLOWSUB, " & Database.DBPrefix & "_QUICK_REGISTRATION, " & Database.DBPrefix & "_EMAIL_PORT, " & Database.DBPrefix & "_EMAIL_USERNAME, " & Database.DBPrefix & "_EMAIL_PASSWORD, " & Database.DBPrefix & "_CURSE_FILTER, " & Database.DBPrefix & "_RSS_FEEDS, " & Database.DBPrefix & "_HORIZ_DIVIDE, " & Database.DBPrefix & "_VERT_DIVIDE, " & Database.DBPrefix & "_ALLOW_EDITS, " & Database.DBPrefix & "_ALLOW_REGISTRATION, " & Database.DBPrefix & "_ALLOW_MEDIA, " & Database.DBPrefix & "_ALLOW_REPORTING, " & Database.DBPrefix & "_HIDE_MEMBERS, " & Database.DBPrefix & "_HIDE_LOGIN, " & Database.DBPrefix & "_MEMBER_PHOTOSIZE, " & Database.DBPrefix & "_CUSTOM_CSS, " & Database.DBPrefix & "_HTML_TITLE, " & Database.DBPrefix & "_THUMBNAIL_SIZE, " & Database.DBPrefix & "_AVATAR_SIZE, " & Database.DBPrefix & "_SEARCH_TOPICS, " & Database.DBPrefix & "_SEARCH_MEMBERS, " & Database.DBPrefix & "_SEARCH_BLOGS, " & Database.DBPrefix & "_SEARCH_PAGES, " & Database.DBPrefix & "_MEMBER_FILETYPES, " & Database.DBPrefix & "_TOPIC_UPLOADSIZE, " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE, " & Database.DBPrefix & "_TOPICS_FONTSIZE, " & Database.DBPrefix & "_TOPICS_FONTCOLOR) VALUES ('" & txtForumTitle & "', 'Copyright DMG Development', '', '" & txtWebSiteURL & "', 'arial,helvetica', 2, 'darkred', 'black', 3, 'darkred', 'ghostwhite', '#B6B6B6', 'black', 2, 'darkred', 'ghostwhite', 'darkgray', 'black', 'darkred', 'none', 'darkred', 'none', 'firebrick', 'underline', 'firebrick', 'underline', 'gainsboro', 'whitesmoke', '#060237', 'darkred', '<table border=""2"" bordercolor=""[bordercolor]"" width=""100%"" align=""center"" cellpadding=""5"" cellspacing=""0"">" & CHR(10) & "<tr>" & CHR(10) & "<td width=""100%"" height=""100"" align=""center"" valign=""middle"" bgcolor=""[headercolor]"" background=""forumimages/dmgstyles/redgradients-topbg.jpg"">" & CHR(10) & "	<span class=""PageTitle"">[PageTitle]</span>" & CHR(10) & "</td>" & CHR(10) & "</tr>" & CHR(10) & "<tr>" & CHR(10) & "<td width=""100%"" align=""left"">" & CHR(10) & CHR(10) & "<br /><center>[menu=H]</center>', '</td>" & CHR(10) & "</tr>" & CHR(10) & "</table>', 15, 30, 1, 'Red Gradients', " & txtInstallationType & ", 0, 0, 1, 'forums, portal', 0, 'mail.yourserver.com', 'mail@yourserver.com', 0, 0, '', 1, 0, '', '', '', 0, 0, '&nbsp;&nbsp;<b>|</b>&nbsp;&nbsp;', '<br /><br />', 1, 1, 1, 1, 0, 0, 1024, '<STYLE TYPE=""text/css"">" & CHR(10) & "<!--" & CHR(10) & "A:link {text-decoration: [LinkDecoration]; color: [LinkColor]; font-weight: bold;}" & CHR(10) & "A:visited {text-decoration: [VLinkDecoration]; color: [VLinkColor]; font-weight: bold;}" & CHR(10) & "A:active {text-decoration: [ALinkDecoration]; color: [ALinkColor]; font-weight: bold;}" & CHR(10) & "A:hover {text-decoration: [HLinkDecoration]; color: [HLinkColor]; font-weight: bold;}" & CHR(10) & ".dmgbuttons{" & CHR(10) & "	font-family: Verdana;" & CHR(10) & "	font-size: 8px;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "	border-color: [ButtonColor];}" & CHR(10) & ".LoginButton{font-size: 12px; border-color: [ButtonColor];}" & CHR(10) & ".AdminButtons{font-size: 18px; width: 270px; border-color: [HeaderColor];}" & CHR(10) & ".LoginBox{border-color: [ButtonColor];}" & CHR(10) & ".ContentBox {" & CHR(10) & "	border-width: 1px;" & CHR(10) & "	border-style: dashed;" & CHR(10) & "	border-color: [BorderColor];" & CHR(10) & "	border-collapse: separate;" & CHR(10) & "}" & CHR(10) & ".HeaderCell{" & CHR(10) & "	background-color: [HeaderColor];" & CHR(10) & "	background-image: url(forumimages/dmgstyles/redgradients-headerbg.jpg);" & CHR(10) & "}" & CHR(10) & ".SubHeaderCell{background-color: [SubHeaderColor];}" & CHR(10) & ".FooterCell{" & CHR(10) & "	background-color: [FooterColor];" & CHR(10) & "	background-image: url(forumimages/dmgstyles/redgradients-headerbg.jpg);" & CHR(10) & "}" & CHR(10) & ".TableRow1{background-color: [TableBGColor1];}" & CHR(10) & ".TableRow2{background-color: [TableBGColor2];}" & CHR(10) & ".MessageBox{font-size: 20px;}" & CHR(10) & ".PageTitle{" & CHR(10) & "	font-size: 55px;" & CHR(10) & "	color: [headerfontcolor];" & CHR(10) & "	font-style: oblique;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "}" & CHR(10) & ".PhotoGalleryTable {border: 0px;}" & CHR(10) & ".PhotoGalleryTable TD{font-size: 13px; padding: 10px; text-align: center;}" & CHR(10) & "-->" & CHR(10) & "</STYLE>', '" & txtForumTitle & "', 150, 125, 1, 1, 1, 1, 'image/gif, image/png, application/msword, image/jpeg, application/zip, text/plain, image/pjpeg, application/mspowerpoint, application/vnd.ms-excel, application/pdf', 1024, 0, 2, 'black')")
				end if

				if ((txtDefaultTemplate = "GreenGradients") or (txtInstallAllTemplates = 1)) then
					'GREEN GRADIENTS SETUP
					Database.Write("INSERT INTO " & Database.DBPrefix & "_SETTINGS (" & Database.DBPrefix & "_TITLE, " & Database.DBPrefix & "_COPYRIGHT, " & Database.DBPrefix & "_LOGO, " & Database.DBPrefix & "_URL, " & Database.DBPrefix & "_FONTFACE, " & Database.DBPrefix & "_FONTSIZE, " & Database.DBPrefix & "_BUTTON_COLOR, " & Database.DBPrefix & "_LOGIN_FONTCOLOR, " & Database.DBPrefix & "_HEADER_SIZE, " & Database.DBPrefix & "_HEADER_COLOR, " & Database.DBPrefix & "_HEADER_FONTCOLOR, " & Database.DBPrefix & "_SUBHEADER_COLOR, " & Database.DBPrefix & "_SUBHEADER_FONTCOLOR, " & Database.DBPrefix & "_FOOTER_SIZE, " & Database.DBPrefix & "_FOOTER_COLOR, " & Database.DBPrefix & "_FOOTER_FONTCOLOR, " & Database.DBPrefix & "_BGCOLOR, " & Database.DBPrefix & "_FONT_COLOR, " & Database.DBPrefix & "_LINK_COLOR, " & Database.DBPrefix & "_LINK_DECORATION, " & Database.DBPrefix & "_LINK_VISITED_COLOR, " & Database.DBPrefix & "_LINK_VISITED_DECORATION, " & Database.DBPrefix & "_LINK_ACTIVE_COLOR, " & Database.DBPrefix & "_LINK_ACTIVE_DECORATION, " & Database.DBPrefix & "_LINK_HOVER_COLOR, " & Database.DBPrefix & "_LINK_HOVER_DECORATION, " & Database.DBPrefix & "_TOPICS_BGCOLOR1, " & Database.DBPrefix & "_TOPICS_BGCOLOR2, " & Database.DBPrefix & "_TABLEBORDER_COLOR, " & Database.DBPrefix & "_SCROLLBAR_COLOR, " & Database.DBPrefix & "_CUSTOM_HEADER, " & Database.DBPrefix & "_CUSTOM_FOOTER, " & Database.DBPrefix & "_ITEMS_PER_PAGE, " & Database.DBPrefix & "_SPAM_FILTER, " & Database.DBPrefix & "_TEMPLATE_DEFAULT, " & Database.DBPrefix & "_TEMPLATE_NAME, " & Database.DBPrefix & "_FORUMS_DEFAULT, " & Database.DBPrefix & "_MARGIN_SIDE, " & Database.DBPrefix & "_MARGIN_TOP, " & Database.DBPrefix & "_SHOWSTATISTICS, " & Database.DBPrefix & "_CUSTOM_META, " & Database.DBPrefix & "_MEMBER_VALIDATION, " & Database.DBPrefix & "_EMAIL_SMTP, " & Database.DBPrefix & "_EMAIL_ADDRESS, " & Database.DBPrefix & "_EMAIL_ALLOWSEND, " & Database.DBPrefix & "_EMAIL_ALLOWSUB, " & Database.DBPrefix & "_BGIMAGE, " & Database.DBPrefix & "_ALLOWSUB, " & Database.DBPrefix & "_QUICK_REGISTRATION, " & Database.DBPrefix & "_EMAIL_PORT, " & Database.DBPrefix & "_EMAIL_USERNAME, " & Database.DBPrefix & "_EMAIL_PASSWORD, " & Database.DBPrefix & "_CURSE_FILTER, " & Database.DBPrefix & "_RSS_FEEDS, " & Database.DBPrefix & "_HORIZ_DIVIDE, " & Database.DBPrefix & "_VERT_DIVIDE, " & Database.DBPrefix & "_ALLOW_EDITS, " & Database.DBPrefix & "_ALLOW_REGISTRATION, " & Database.DBPrefix & "_ALLOW_MEDIA, " & Database.DBPrefix & "_ALLOW_REPORTING, " & Database.DBPrefix & "_HIDE_MEMBERS, " & Database.DBPrefix & "_HIDE_LOGIN, " & Database.DBPrefix & "_MEMBER_PHOTOSIZE, " & Database.DBPrefix & "_CUSTOM_CSS, " & Database.DBPrefix & "_HTML_TITLE, " & Database.DBPrefix & "_THUMBNAIL_SIZE, " & Database.DBPrefix & "_AVATAR_SIZE, " & Database.DBPrefix & "_SEARCH_TOPICS, " & Database.DBPrefix & "_SEARCH_MEMBERS, " & Database.DBPrefix & "_SEARCH_BLOGS, " & Database.DBPrefix & "_SEARCH_PAGES, " & Database.DBPrefix & "_MEMBER_FILETYPES, " & Database.DBPrefix & "_TOPIC_UPLOADSIZE, " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE, " & Database.DBPrefix & "_TOPICS_FONTSIZE, " & Database.DBPrefix & "_TOPICS_FONTCOLOR) VALUES ('" & txtForumTitle & "', 'Copyright DMG Development', '', '" & txtWebSiteURL & "', 'arial,helvetica', 2, 'darkgreen', 'black', 3, 'darkgreen', 'ghostwhite', '#B6B6B6', 'black', 2, 'darkgreen', 'ghostwhite', 'darkgray', 'black', 'darkgreen', 'none', 'darkgreen', 'none', 'green', 'underline', 'green', 'underline', 'gainsboro', 'whitesmoke', '#060237', 'darkgreen', '<table border=""2"" bordercolor=""[bordercolor]"" width=""100%"" align=""center"" cellpadding=""5"" cellspacing=""0"">" & CHR(10) & "<tr>" & CHR(10) & "<td width=""100%"" height=""100"" align=""center"" valign=""middle"" bgcolor=""[headercolor]"" background=""forumimages/dmgstyles/greengradients-topbg.jpg"">" & CHR(10) & "	<span class=""PageTitle"">[PageTitle]</span>" & CHR(10) & "</td>" & CHR(10) & "</tr>" & CHR(10) & "<tr>" & CHR(10) & "<td width=""100%"" align=""left"">" & CHR(10) & CHR(10) & "<br /><center>[menu=H]</center>', '</td>" & CHR(10) & "</tr>" & CHR(10) & "</table>', 15, 30, 1, 'Green Gradients', " & txtInstallationType & ", 0, 0, 1, 'forums, portal', 0, 'mail.yourserver.com', 'mail@yourserver.com', 0, 0, '', 1, 0, '', '', '', 0, 0, '&nbsp;&nbsp;<b>|</b>&nbsp;&nbsp;', '<br /><br />', 1, 1, 1, 1, 0, 0, 1024, '<STYLE TYPE=""text/css"">" & CHR(10) & "<!--" & CHR(10) & "A:link {text-decoration: [LinkDecoration]; color: [LinkColor]; font-weight: bold;}" & CHR(10) & "A:visited {text-decoration: [VLinkDecoration]; color: [VLinkColor]; font-weight: bold;}" & CHR(10) & "A:active {text-decoration: [ALinkDecoration]; color: [ALinkColor]; font-weight: bold;}" & CHR(10) & "A:hover {text-decoration: [HLinkDecoration]; color: [HLinkColor]; font-weight: bold;}" & CHR(10) & ".dmgbuttons{" & CHR(10) & "	font-family: Verdana;" & CHR(10) & "	font-size: 8px;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "	border-color: [ButtonColor];}" & CHR(10) & ".LoginButton{font-size: 12px; border-color: [ButtonColor];}" & CHR(10) & ".AdminButtons{font-size: 18px; width: 270px; border-color: [HeaderColor];}" & CHR(10) & ".LoginBox{border-color: [ButtonColor];}" & CHR(10) & ".ContentBox {" & CHR(10) & "	border-width: 1px;" & CHR(10) & "	border-style: dashed;" & CHR(10) & "	border-color: [BorderColor];" & CHR(10) & "	border-collapse: separate;" & CHR(10) & "}" & CHR(10) & ".HeaderCell{" & CHR(10) & "	background-color: [HeaderColor];" & CHR(10) & "	background-image: url(forumimages/dmgstyles/greengradients-headerbg.jpg);" & CHR(10) & "}" & CHR(10) & ".SubHeaderCell{background-color: [SubHeaderColor];}" & CHR(10) & ".FooterCell{" & CHR(10) & "	background-color: [FooterColor];" & CHR(10) & "	background-image: url(forumimages/dmgstyles/greengradients-headerbg.jpg);" & CHR(10) & "}" & CHR(10) & ".TableRow1{background-color: [TableBGColor1];}" & CHR(10) & ".TableRow2{background-color: [TableBGColor2];}" & CHR(10) & ".MessageBox{font-size: 20px;}" & CHR(10) & ".PageTitle{" & CHR(10) & "	font-size: 55px;" & CHR(10) & "	color: [headerfontcolor];" & CHR(10) & "	font-style: oblique;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "}" & CHR(10) & ".PhotoGalleryTable {border: 0px;}" & CHR(10) & ".PhotoGalleryTable TD{font-size: 13px; padding: 10px; text-align: center;}" & CHR(10) & "-->" & CHR(10) & "</STYLE>', '" & txtForumTitle & "', 150, 125, 1, 1, 1, 1, 'image/gif, image/png, application/msword, image/jpeg, application/zip, text/plain, image/pjpeg, application/mspowerpoint, application/vnd.ms-excel, application/pdf', 1024, 0, 2, 'black')")
				end if

				if ((txtDefaultTemplate = "CenterScrollBlue") or (txtInstallAllTemplates = 1)) then
					'CENTER SCROLL BLUE SETUP
					Database.Write("INSERT INTO " & Database.DBPrefix & "_SETTINGS (" & Database.DBPrefix & "_TITLE, " & Database.DBPrefix & "_COPYRIGHT, " & Database.DBPrefix & "_LOGO, " & Database.DBPrefix & "_URL, " & Database.DBPrefix & "_FONTFACE, " & Database.DBPrefix & "_FONTSIZE, " & Database.DBPrefix & "_BUTTON_COLOR, " & Database.DBPrefix & "_LOGIN_FONTCOLOR, " & Database.DBPrefix & "_HEADER_SIZE, " & Database.DBPrefix & "_HEADER_COLOR, " & Database.DBPrefix & "_HEADER_FONTCOLOR, " & Database.DBPrefix & "_SUBHEADER_COLOR, " & Database.DBPrefix & "_SUBHEADER_FONTCOLOR, " & Database.DBPrefix & "_FOOTER_SIZE, " & Database.DBPrefix & "_FOOTER_COLOR, " & Database.DBPrefix & "_FOOTER_FONTCOLOR, " & Database.DBPrefix & "_BGCOLOR, " & Database.DBPrefix & "_FONT_COLOR, " & Database.DBPrefix & "_LINK_COLOR, " & Database.DBPrefix & "_LINK_DECORATION, " & Database.DBPrefix & "_LINK_VISITED_COLOR, " & Database.DBPrefix & "_LINK_VISITED_DECORATION, " & Database.DBPrefix & "_LINK_ACTIVE_COLOR, " & Database.DBPrefix & "_LINK_ACTIVE_DECORATION, " & Database.DBPrefix & "_LINK_HOVER_COLOR, " & Database.DBPrefix & "_LINK_HOVER_DECORATION, " & Database.DBPrefix & "_TOPICS_BGCOLOR1, " & Database.DBPrefix & "_TOPICS_BGCOLOR2, " & Database.DBPrefix & "_TABLEBORDER_COLOR, " & Database.DBPrefix & "_SCROLLBAR_COLOR, " & Database.DBPrefix & "_CUSTOM_HEADER, " & Database.DBPrefix & "_CUSTOM_FOOTER, " & Database.DBPrefix & "_ITEMS_PER_PAGE, " & Database.DBPrefix & "_SPAM_FILTER, " & Database.DBPrefix & "_TEMPLATE_DEFAULT, " & Database.DBPrefix & "_TEMPLATE_NAME, " & Database.DBPrefix & "_FORUMS_DEFAULT, " & Database.DBPrefix & "_MARGIN_SIDE, " & Database.DBPrefix & "_MARGIN_TOP, " & Database.DBPrefix & "_SHOWSTATISTICS, " & Database.DBPrefix & "_CUSTOM_META, " & Database.DBPrefix & "_MEMBER_VALIDATION, " & Database.DBPrefix & "_EMAIL_SMTP, " & Database.DBPrefix & "_EMAIL_ADDRESS, " & Database.DBPrefix & "_EMAIL_ALLOWSEND, " & Database.DBPrefix & "_EMAIL_ALLOWSUB, " & Database.DBPrefix & "_BGIMAGE, " & Database.DBPrefix & "_ALLOWSUB, " & Database.DBPrefix & "_QUICK_REGISTRATION, " & Database.DBPrefix & "_EMAIL_PORT, " & Database.DBPrefix & "_EMAIL_USERNAME, " & Database.DBPrefix & "_EMAIL_PASSWORD, " & Database.DBPrefix & "_CURSE_FILTER, " & Database.DBPrefix & "_RSS_FEEDS, " & Database.DBPrefix & "_HORIZ_DIVIDE, " & Database.DBPrefix & "_VERT_DIVIDE, " & Database.DBPrefix & "_ALLOW_EDITS, " & Database.DBPrefix & "_ALLOW_REGISTRATION, " & Database.DBPrefix & "_ALLOW_MEDIA, " & Database.DBPrefix & "_ALLOW_REPORTING, " & Database.DBPrefix & "_HIDE_MEMBERS, " & Database.DBPrefix & "_HIDE_LOGIN, " & Database.DBPrefix & "_MEMBER_PHOTOSIZE, " & Database.DBPrefix & "_CUSTOM_CSS, " & Database.DBPrefix & "_HTML_TITLE, " & Database.DBPrefix & "_THUMBNAIL_SIZE, " & Database.DBPrefix & "_AVATAR_SIZE, " & Database.DBPrefix & "_SEARCH_TOPICS, " & Database.DBPrefix & "_SEARCH_MEMBERS, " & Database.DBPrefix & "_SEARCH_BLOGS, " & Database.DBPrefix & "_SEARCH_PAGES, " & Database.DBPrefix & "_MEMBER_FILETYPES, " & Database.DBPrefix & "_TOPIC_UPLOADSIZE, " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE, " & Database.DBPrefix & "_TOPICS_FONTSIZE, " & Database.DBPrefix & "_TOPICS_FONTCOLOR) VALUES ('" & txtForumTitle & "', 'Copyright DMG Development', '', '" & txtWebSiteURL & "', 'arial,helvetica', 2, '#133991', 'black', 3, '#133991', 'gainsboro', '#B6B6B6', 'black', 2, '#133991', 'gainsboro', '#38579E', 'black', '#133991', 'none', '#133991', 'none', '#38579E', 'underline', '#38579E', 'underline', 'gainsboro', 'whitesmoke', '#060237', '#133991', '<table width=""965"" align=""center"" cellpadding=""0"" cellspacing=""0"">" & CHR(10) & "<tr>" & CHR(10) & "<td width=""100%"" height=""100"" align=""center"" valign=""middle"" bgcolor=""[headercolor]"">" & CHR(10) & "	<span class=""PageTitle"">[PageTitle]</span>" & CHR(10) & "</td>" & CHR(10) & "</tr>" & CHR(10) & "<tr>" & CHR(10) & "<td width=""100%"" height=""26"" align=""center"" valign=""middle"" class=""MainButtons"">" & CHR(10) & "	[menu=H]" & CHR(10) & "</td>" & CHR(10) & "</tr>" & CHR(10) & "<tr>" & CHR(10) & "<td width=""100%"" align=""left"">', '</td>" & CHR(10) & "</tr>" & CHR(10) & "</table>', 15, 30, 1, 'Center Scroll Blue', " & txtInstallationType & ", 10, 0, 1, 'forums, portal', 0, 'mail.yourserver.com', 'mail@yourserver.com', 0, 0, '', 1, 0, '', '', '', 0, 0, '&nbsp;&nbsp;<b>|</b>&nbsp;&nbsp;', '<br /><br />', 1, 1, 1, 1, 0, 0, 1024, '<STYLE TYPE=""text/css"">" & CHR(10) & "<!--" & CHR(10) & "A:link {text-decoration:[LinkDecoration];color:[LinkColor];}" & CHR(10) & "A:visited {text-decoration:[VLinkDecoration];color:[VLinkColor];}" & CHR(10) & "A:active {text-decoration:[ALinkDecoration];color:[ALinkColor];}" & CHR(10) & "A:hover {text-decoration:[HLinkDecoration];color:[HLinkColor];}" & CHR(10) & "body{" & CHR(10) & "	background: [BackgroundColor] url(forumimages/dmgstyles/centerscrollblue-bg.gif) repeat-y top center;" & CHR(10) & "}" & CHR(10) & ".PageTitle{" & CHR(10) & "	font-size: 55px;" & CHR(10) & "	color: [headerfontcolor];" & CHR(10) & "	font-weight: bold;" & CHR(10) & "}" & CHR(10) & ".MainButtons{" & CHR(10) & "	font-size: 17px;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "	background: silver url(forumimages/dmgstyles/centerscroll-buttonbg.jpg) repeat-x top left;" & CHR(10) & "}" & CHR(10) & ".dmgbuttons{" & CHR(10) & "	font-family: Verdana;" & CHR(10) & "	font-size: 8px;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "	border-color: [ButtonColor];}" & CHR(10) & ".LoginButton{font-size: 12px; border-color: [ButtonColor];}" & CHR(10) & ".AdminButtons{font-size: 18px; width: 270px; border-color: [HeaderColor];}" & CHR(10) & ".LoginBox{border-color: [ButtonColor];}" & CHR(10) & ".ContentBox{border:2px solid [BorderColor];}" & CHR(10) & ".HeaderCell{background-color: [HeaderColor];}" & CHR(10) & ".SubHeaderCell{background-color: [SubHeaderColor];}" & CHR(10) & ".FooterCell{background-color: [FooterColor];}" & CHR(10) & ".TableRow1{background-color: [TableBGColor1];}" & CHR(10) & ".TableRow2{background-color: [TableBGColor2];}" & CHR(10) & ".MessageBox{font-size: 20px;}" & CHR(10) & ".PhotoGalleryTable {border: 0px;}" & CHR(10) & ".PhotoGalleryTable TD{font-size: 13px; padding: 10px; text-align: center;}" & CHR(10) & "-->" & CHR(10) & "</STYLE>', '" & txtForumTitle & "', 150, 125, 1, 1, 1, 1, 'image/gif, image/png, application/msword, image/jpeg, application/zip, text/plain, image/pjpeg, application/mspowerpoint, application/vnd.ms-excel, application/pdf', 1024, 0, 2, 'black')")
				end if

				if ((txtDefaultTemplate = "CenterScrollRed") or (txtInstallAllTemplates = 1)) then
					'CENTER SCROLL RED SETUP
					Database.Write("INSERT INTO " & Database.DBPrefix & "_SETTINGS (" & Database.DBPrefix & "_TITLE, " & Database.DBPrefix & "_COPYRIGHT, " & Database.DBPrefix & "_LOGO, " & Database.DBPrefix & "_URL, " & Database.DBPrefix & "_FONTFACE, " & Database.DBPrefix & "_FONTSIZE, " & Database.DBPrefix & "_BUTTON_COLOR, " & Database.DBPrefix & "_LOGIN_FONTCOLOR, " & Database.DBPrefix & "_HEADER_SIZE, " & Database.DBPrefix & "_HEADER_COLOR, " & Database.DBPrefix & "_HEADER_FONTCOLOR, " & Database.DBPrefix & "_SUBHEADER_COLOR, " & Database.DBPrefix & "_SUBHEADER_FONTCOLOR, " & Database.DBPrefix & "_FOOTER_SIZE, " & Database.DBPrefix & "_FOOTER_COLOR, " & Database.DBPrefix & "_FOOTER_FONTCOLOR, " & Database.DBPrefix & "_BGCOLOR, " & Database.DBPrefix & "_FONT_COLOR, " & Database.DBPrefix & "_LINK_COLOR, " & Database.DBPrefix & "_LINK_DECORATION, " & Database.DBPrefix & "_LINK_VISITED_COLOR, " & Database.DBPrefix & "_LINK_VISITED_DECORATION, " & Database.DBPrefix & "_LINK_ACTIVE_COLOR, " & Database.DBPrefix & "_LINK_ACTIVE_DECORATION, " & Database.DBPrefix & "_LINK_HOVER_COLOR, " & Database.DBPrefix & "_LINK_HOVER_DECORATION, " & Database.DBPrefix & "_TOPICS_BGCOLOR1, " & Database.DBPrefix & "_TOPICS_BGCOLOR2, " & Database.DBPrefix & "_TABLEBORDER_COLOR, " & Database.DBPrefix & "_SCROLLBAR_COLOR, " & Database.DBPrefix & "_CUSTOM_HEADER, " & Database.DBPrefix & "_CUSTOM_FOOTER, " & Database.DBPrefix & "_ITEMS_PER_PAGE, " & Database.DBPrefix & "_SPAM_FILTER, " & Database.DBPrefix & "_TEMPLATE_DEFAULT, " & Database.DBPrefix & "_TEMPLATE_NAME, " & Database.DBPrefix & "_FORUMS_DEFAULT, " & Database.DBPrefix & "_MARGIN_SIDE, " & Database.DBPrefix & "_MARGIN_TOP, " & Database.DBPrefix & "_SHOWSTATISTICS, " & Database.DBPrefix & "_CUSTOM_META, " & Database.DBPrefix & "_MEMBER_VALIDATION, " & Database.DBPrefix & "_EMAIL_SMTP, " & Database.DBPrefix & "_EMAIL_ADDRESS, " & Database.DBPrefix & "_EMAIL_ALLOWSEND, " & Database.DBPrefix & "_EMAIL_ALLOWSUB, " & Database.DBPrefix & "_BGIMAGE, " & Database.DBPrefix & "_ALLOWSUB, " & Database.DBPrefix & "_QUICK_REGISTRATION, " & Database.DBPrefix & "_EMAIL_PORT, " & Database.DBPrefix & "_EMAIL_USERNAME, " & Database.DBPrefix & "_EMAIL_PASSWORD, " & Database.DBPrefix & "_CURSE_FILTER, " & Database.DBPrefix & "_RSS_FEEDS, " & Database.DBPrefix & "_HORIZ_DIVIDE, " & Database.DBPrefix & "_VERT_DIVIDE, " & Database.DBPrefix & "_ALLOW_EDITS, " & Database.DBPrefix & "_ALLOW_REGISTRATION, " & Database.DBPrefix & "_ALLOW_MEDIA, " & Database.DBPrefix & "_ALLOW_REPORTING, " & Database.DBPrefix & "_HIDE_MEMBERS, " & Database.DBPrefix & "_HIDE_LOGIN, " & Database.DBPrefix & "_MEMBER_PHOTOSIZE, " & Database.DBPrefix & "_CUSTOM_CSS, " & Database.DBPrefix & "_HTML_TITLE, " & Database.DBPrefix & "_THUMBNAIL_SIZE, " & Database.DBPrefix & "_AVATAR_SIZE, " & Database.DBPrefix & "_SEARCH_TOPICS, " & Database.DBPrefix & "_SEARCH_MEMBERS, " & Database.DBPrefix & "_SEARCH_BLOGS, " & Database.DBPrefix & "_SEARCH_PAGES, " & Database.DBPrefix & "_MEMBER_FILETYPES, " & Database.DBPrefix & "_TOPIC_UPLOADSIZE, " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE, " & Database.DBPrefix & "_TOPICS_FONTSIZE, " & Database.DBPrefix & "_TOPICS_FONTCOLOR) VALUES ('" & txtForumTitle & "', 'Copyright DMG Development', '', '" & txtWebSiteURL & "', 'arial,helvetica', 2, '#A81717', 'black', 3, '#A81717', 'gainsboro', '#B6B6B6', 'black', 2, '#A81717', 'gainsboro', '#B44141', 'black', '#A81717', 'none', '#A81717', 'none', '#B44141', 'underline', '#B44141', 'underline', 'gainsboro', 'whitesmoke', '#060237', '#A81717', '<table width=""965"" align=""center"" cellpadding=""0"" cellspacing=""0"">" & CHR(10) & "<tr>" & CHR(10) & "<td width=""100%"" height=""100"" align=""center"" valign=""middle"" bgcolor=""[headercolor]"">" & CHR(10) & "	<span class=""PageTitle"">[PageTitle]</span>" & CHR(10) & "</td>" & CHR(10) & "</tr>" & CHR(10) & "<tr>" & CHR(10) & "<td width=""100%"" height=""26"" align=""center"" valign=""middle"" class=""MainButtons"">" & CHR(10) & "	[menu=H]" & CHR(10) & "</td>" & CHR(10) & "</tr>" & CHR(10) & "<tr>" & CHR(10) & "<td width=""100%"" align=""left"">', '</td>" & CHR(10) & "</tr>" & CHR(10) & "</table>', 15, 30, 1, 'Center Scroll Red', " & txtInstallationType & ", 10, 0, 1, 'forums, portal', 0, 'mail.yourserver.com', 'mail@yourserver.com', 0, 0, '', 1, 0, '', '', '', 0, 0, '&nbsp;&nbsp;<b>|</b>&nbsp;&nbsp;', '<br /><br />', 1, 1, 1, 1, 0, 0, 1024, '<STYLE TYPE=""text/css"">" & CHR(10) & "<!--" & CHR(10) & "A:link {text-decoration:[LinkDecoration];color:[LinkColor];}" & CHR(10) & "A:visited {text-decoration:[VLinkDecoration];color:[VLinkColor];}" & CHR(10) & "A:active {text-decoration:[ALinkDecoration];color:[ALinkColor];}" & CHR(10) & "A:hover {text-decoration:[HLinkDecoration];color:[HLinkColor];}" & CHR(10) & "body{" & CHR(10) & "	background: [BackgroundColor] url(forumimages/dmgstyles/centerscrollred-bg.gif) repeat-y top center;" & CHR(10) & "}" & CHR(10) & ".PageTitle{" & CHR(10) & "	font-size: 55px;" & CHR(10) & "	color: [headerfontcolor];" & CHR(10) & "	font-weight: bold;" & CHR(10) & "}" & CHR(10) & ".MainButtons{" & CHR(10) & "	font-size: 17px;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "	background: silver url(forumimages/dmgstyles/centerscroll-buttonbg.jpg) repeat-x top left;" & CHR(10) & "}" & CHR(10) & ".dmgbuttons{" & CHR(10) & "	font-family: Verdana;" & CHR(10) & "	font-size: 8px;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "	border-color: [ButtonColor];}" & CHR(10) & ".LoginButton{font-size: 12px; border-color: [ButtonColor];}" & CHR(10) & ".AdminButtons{font-size: 18px; width: 270px; border-color: [HeaderColor];}" & CHR(10) & ".LoginBox{border-color: [ButtonColor];}" & CHR(10) & ".ContentBox{border:2px solid [BorderColor];}" & CHR(10) & ".HeaderCell{background-color: [HeaderColor];}" & CHR(10) & ".SubHeaderCell{background-color: [SubHeaderColor];}" & CHR(10) & ".FooterCell{background-color: [FooterColor];}" & CHR(10) & ".TableRow1{background-color: [TableBGColor1];}" & CHR(10) & ".TableRow2{background-color: [TableBGColor2];}" & CHR(10) & ".MessageBox{font-size: 20px;}" & CHR(10) & ".PhotoGalleryTable {border: 0px;}" & CHR(10) & ".PhotoGalleryTable TD{font-size: 13px; padding: 10px; text-align: center;}" & CHR(10) & "-->" & CHR(10) & "</STYLE>', '" & txtForumTitle & "', 150, 125, 1, 1, 1, 1, 'image/gif, image/png, application/msword, image/jpeg, application/zip, text/plain, image/pjpeg, application/mspowerpoint, application/vnd.ms-excel, application/pdf', 1024, 0, 2, 'black')")
				end if

				if ((txtDefaultTemplate = "CenterScrollGreen") or (txtInstallAllTemplates = 1)) then
					'CENTER SCROLL GREEN SETUP
					Database.Write("INSERT INTO " & Database.DBPrefix & "_SETTINGS (" & Database.DBPrefix & "_TITLE, " & Database.DBPrefix & "_COPYRIGHT, " & Database.DBPrefix & "_LOGO, " & Database.DBPrefix & "_URL, " & Database.DBPrefix & "_FONTFACE, " & Database.DBPrefix & "_FONTSIZE, " & Database.DBPrefix & "_BUTTON_COLOR, " & Database.DBPrefix & "_LOGIN_FONTCOLOR, " & Database.DBPrefix & "_HEADER_SIZE, " & Database.DBPrefix & "_HEADER_COLOR, " & Database.DBPrefix & "_HEADER_FONTCOLOR, " & Database.DBPrefix & "_SUBHEADER_COLOR, " & Database.DBPrefix & "_SUBHEADER_FONTCOLOR, " & Database.DBPrefix & "_FOOTER_SIZE, " & Database.DBPrefix & "_FOOTER_COLOR, " & Database.DBPrefix & "_FOOTER_FONTCOLOR, " & Database.DBPrefix & "_BGCOLOR, " & Database.DBPrefix & "_FONT_COLOR, " & Database.DBPrefix & "_LINK_COLOR, " & Database.DBPrefix & "_LINK_DECORATION, " & Database.DBPrefix & "_LINK_VISITED_COLOR, " & Database.DBPrefix & "_LINK_VISITED_DECORATION, " & Database.DBPrefix & "_LINK_ACTIVE_COLOR, " & Database.DBPrefix & "_LINK_ACTIVE_DECORATION, " & Database.DBPrefix & "_LINK_HOVER_COLOR, " & Database.DBPrefix & "_LINK_HOVER_DECORATION, " & Database.DBPrefix & "_TOPICS_BGCOLOR1, " & Database.DBPrefix & "_TOPICS_BGCOLOR2, " & Database.DBPrefix & "_TABLEBORDER_COLOR, " & Database.DBPrefix & "_SCROLLBAR_COLOR, " & Database.DBPrefix & "_CUSTOM_HEADER, " & Database.DBPrefix & "_CUSTOM_FOOTER, " & Database.DBPrefix & "_ITEMS_PER_PAGE, " & Database.DBPrefix & "_SPAM_FILTER, " & Database.DBPrefix & "_TEMPLATE_DEFAULT, " & Database.DBPrefix & "_TEMPLATE_NAME, " & Database.DBPrefix & "_FORUMS_DEFAULT, " & Database.DBPrefix & "_MARGIN_SIDE, " & Database.DBPrefix & "_MARGIN_TOP, " & Database.DBPrefix & "_SHOWSTATISTICS, " & Database.DBPrefix & "_CUSTOM_META, " & Database.DBPrefix & "_MEMBER_VALIDATION, " & Database.DBPrefix & "_EMAIL_SMTP, " & Database.DBPrefix & "_EMAIL_ADDRESS, " & Database.DBPrefix & "_EMAIL_ALLOWSEND, " & Database.DBPrefix & "_EMAIL_ALLOWSUB, " & Database.DBPrefix & "_BGIMAGE, " & Database.DBPrefix & "_ALLOWSUB, " & Database.DBPrefix & "_QUICK_REGISTRATION, " & Database.DBPrefix & "_EMAIL_PORT, " & Database.DBPrefix & "_EMAIL_USERNAME, " & Database.DBPrefix & "_EMAIL_PASSWORD, " & Database.DBPrefix & "_CURSE_FILTER, " & Database.DBPrefix & "_RSS_FEEDS, " & Database.DBPrefix & "_HORIZ_DIVIDE, " & Database.DBPrefix & "_VERT_DIVIDE, " & Database.DBPrefix & "_ALLOW_EDITS, " & Database.DBPrefix & "_ALLOW_REGISTRATION, " & Database.DBPrefix & "_ALLOW_MEDIA, " & Database.DBPrefix & "_ALLOW_REPORTING, " & Database.DBPrefix & "_HIDE_MEMBERS, " & Database.DBPrefix & "_HIDE_LOGIN, " & Database.DBPrefix & "_MEMBER_PHOTOSIZE, " & Database.DBPrefix & "_CUSTOM_CSS, " & Database.DBPrefix & "_HTML_TITLE, " & Database.DBPrefix & "_THUMBNAIL_SIZE, " & Database.DBPrefix & "_AVATAR_SIZE, " & Database.DBPrefix & "_SEARCH_TOPICS, " & Database.DBPrefix & "_SEARCH_MEMBERS, " & Database.DBPrefix & "_SEARCH_BLOGS, " & Database.DBPrefix & "_SEARCH_PAGES, " & Database.DBPrefix & "_MEMBER_FILETYPES, " & Database.DBPrefix & "_TOPIC_UPLOADSIZE, " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE, " & Database.DBPrefix & "_TOPICS_FONTSIZE, " & Database.DBPrefix & "_TOPICS_FONTCOLOR) VALUES ('" & txtForumTitle & "', 'Copyright DMG Development', '', '" & txtWebSiteURL & "', 'arial,helvetica', 2, '#A81717', 'black', 3, '#006729', 'gainsboro', '#B6B6B6', 'black', 2, '#006729', 'gainsboro', '#2F9156', 'black', '#006729', 'none', '#006729', 'none', '#2F9156', 'underline', '#2F9156', 'underline', 'gainsboro', 'whitesmoke', '#060237', '#006729', '<table width=""965"" align=""center"" cellpadding=""0"" cellspacing=""0"">" & CHR(10) & "<tr>" & CHR(10) & "<td width=""100%"" height=""100"" align=""center"" valign=""middle"" bgcolor=""[headercolor]"">" & CHR(10) & "	<span class=""PageTitle"">[PageTitle]</span>" & CHR(10) & "</td>" & CHR(10) & "</tr>" & CHR(10) & "<tr>" & CHR(10) & "<td width=""100%"" height=""26"" align=""center"" valign=""middle"" class=""MainButtons"">" & CHR(10) & "	[menu=H]" & CHR(10) & "</td>" & CHR(10) & "</tr>" & CHR(10) & "<tr>" & CHR(10) & "<td width=""100%"" align=""left"">', '</td>" & CHR(10) & "</tr>" & CHR(10) & "</table>', 15, 30, 1, 'Center Scroll Green', " & txtInstallationType & ", 10, 0, 1, 'forums, portal', 0, 'mail.yourserver.com', 'mail@yourserver.com', 0, 0, '', 1, 0, '', '', '', 0, 0, '&nbsp;&nbsp;<b>|</b>&nbsp;&nbsp;', '<br /><br />', 1, 1, 1, 1, 0, 0, 1024, '<STYLE TYPE=""text/css"">" & CHR(10) & "<!--" & CHR(10) & "A:link {text-decoration:[LinkDecoration];color:[LinkColor];}" & CHR(10) & "A:visited {text-decoration:[VLinkDecoration];color:[VLinkColor];}" & CHR(10) & "A:active {text-decoration:[ALinkDecoration];color:[ALinkColor];}" & CHR(10) & "A:hover {text-decoration:[HLinkDecoration];color:[HLinkColor];}" & CHR(10) & "body{" & CHR(10) & "	background: [BackgroundColor] url(forumimages/dmgstyles/centerscrollgreen-bg.gif) repeat-y top center;" & CHR(10) & "}" & CHR(10) & ".PageTitle{" & CHR(10) & "	font-size: 55px;" & CHR(10) & "	color: [headerfontcolor];" & CHR(10) & "	font-weight: bold;" & CHR(10) & "}" & CHR(10) & ".MainButtons{" & CHR(10) & "	font-size: 17px;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "	background: silver url(forumimages/dmgstyles/centerscroll-buttonbg.jpg) repeat-x top left;" & CHR(10) & "}" & CHR(10) & ".dmgbuttons{" & CHR(10) & "	font-family: Verdana;" & CHR(10) & "	font-size: 8px;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "	border-color: [ButtonColor];}" & CHR(10) & ".LoginButton{font-size: 12px; border-color: [ButtonColor];}" & CHR(10) & ".AdminButtons{font-size: 18px; width: 270px; border-color: [HeaderColor];}" & CHR(10) & ".LoginBox{border-color: [ButtonColor];}" & CHR(10) & ".ContentBox{border:2px solid [BorderColor];}" & CHR(10) & ".HeaderCell{background-color: [HeaderColor];}" & CHR(10) & ".SubHeaderCell{background-color: [SubHeaderColor];}" & CHR(10) & ".FooterCell{background-color: [FooterColor];}" & CHR(10) & ".TableRow1{background-color: [TableBGColor1];}" & CHR(10) & ".TableRow2{background-color: [TableBGColor2];}" & CHR(10) & ".MessageBox{font-size: 20px;}" & CHR(10) & ".PhotoGalleryTable {border: 0px;}" & CHR(10) & ".PhotoGalleryTable TD{font-size: 13px; padding: 10px; text-align: center;}" & CHR(10) & "-->" & CHR(10) & "</STYLE>', '" & txtForumTitle & "', 150, 125, 1, 1, 1, 1, 'image/gif, image/png, application/msword, image/jpeg, application/zip, text/plain, image/pjpeg, application/mspowerpoint, application/vnd.ms-excel, application/pdf', 1024, 0, 2, 'black')")
				end if

				if ((txtDefaultTemplate = "CenterScrollBlack") or (txtInstallAllTemplates = 1)) then
					'CENTER SCROLL BLACK SETUP
					Database.Write("INSERT INTO " & Database.DBPrefix & "_SETTINGS (" & Database.DBPrefix & "_TITLE, " & Database.DBPrefix & "_COPYRIGHT, " & Database.DBPrefix & "_LOGO, " & Database.DBPrefix & "_URL, " & Database.DBPrefix & "_FONTFACE, " & Database.DBPrefix & "_FONTSIZE, " & Database.DBPrefix & "_BUTTON_COLOR, " & Database.DBPrefix & "_LOGIN_FONTCOLOR, " & Database.DBPrefix & "_HEADER_SIZE, " & Database.DBPrefix & "_HEADER_COLOR, " & Database.DBPrefix & "_HEADER_FONTCOLOR, " & Database.DBPrefix & "_SUBHEADER_COLOR, " & Database.DBPrefix & "_SUBHEADER_FONTCOLOR, " & Database.DBPrefix & "_FOOTER_SIZE, " & Database.DBPrefix & "_FOOTER_COLOR, " & Database.DBPrefix & "_FOOTER_FONTCOLOR, " & Database.DBPrefix & "_BGCOLOR, " & Database.DBPrefix & "_FONT_COLOR, " & Database.DBPrefix & "_LINK_COLOR, " & Database.DBPrefix & "_LINK_DECORATION, " & Database.DBPrefix & "_LINK_VISITED_COLOR, " & Database.DBPrefix & "_LINK_VISITED_DECORATION, " & Database.DBPrefix & "_LINK_ACTIVE_COLOR, " & Database.DBPrefix & "_LINK_ACTIVE_DECORATION, " & Database.DBPrefix & "_LINK_HOVER_COLOR, " & Database.DBPrefix & "_LINK_HOVER_DECORATION, " & Database.DBPrefix & "_TOPICS_BGCOLOR1, " & Database.DBPrefix & "_TOPICS_BGCOLOR2, " & Database.DBPrefix & "_TABLEBORDER_COLOR, " & Database.DBPrefix & "_SCROLLBAR_COLOR, " & Database.DBPrefix & "_CUSTOM_HEADER, " & Database.DBPrefix & "_CUSTOM_FOOTER, " & Database.DBPrefix & "_ITEMS_PER_PAGE, " & Database.DBPrefix & "_SPAM_FILTER, " & Database.DBPrefix & "_TEMPLATE_DEFAULT, " & Database.DBPrefix & "_TEMPLATE_NAME, " & Database.DBPrefix & "_FORUMS_DEFAULT, " & Database.DBPrefix & "_MARGIN_SIDE, " & Database.DBPrefix & "_MARGIN_TOP, " & Database.DBPrefix & "_SHOWSTATISTICS, " & Database.DBPrefix & "_CUSTOM_META, " & Database.DBPrefix & "_MEMBER_VALIDATION, " & Database.DBPrefix & "_EMAIL_SMTP, " & Database.DBPrefix & "_EMAIL_ADDRESS, " & Database.DBPrefix & "_EMAIL_ALLOWSEND, " & Database.DBPrefix & "_EMAIL_ALLOWSUB, " & Database.DBPrefix & "_BGIMAGE, " & Database.DBPrefix & "_ALLOWSUB, " & Database.DBPrefix & "_QUICK_REGISTRATION, " & Database.DBPrefix & "_EMAIL_PORT, " & Database.DBPrefix & "_EMAIL_USERNAME, " & Database.DBPrefix & "_EMAIL_PASSWORD, " & Database.DBPrefix & "_CURSE_FILTER, " & Database.DBPrefix & "_RSS_FEEDS, " & Database.DBPrefix & "_HORIZ_DIVIDE, " & Database.DBPrefix & "_VERT_DIVIDE, " & Database.DBPrefix & "_ALLOW_EDITS, " & Database.DBPrefix & "_ALLOW_REGISTRATION, " & Database.DBPrefix & "_ALLOW_MEDIA, " & Database.DBPrefix & "_ALLOW_REPORTING, " & Database.DBPrefix & "_HIDE_MEMBERS, " & Database.DBPrefix & "_HIDE_LOGIN, " & Database.DBPrefix & "_MEMBER_PHOTOSIZE, " & Database.DBPrefix & "_CUSTOM_CSS, " & Database.DBPrefix & "_HTML_TITLE, " & Database.DBPrefix & "_THUMBNAIL_SIZE, " & Database.DBPrefix & "_AVATAR_SIZE, " & Database.DBPrefix & "_SEARCH_TOPICS, " & Database.DBPrefix & "_SEARCH_MEMBERS, " & Database.DBPrefix & "_SEARCH_BLOGS, " & Database.DBPrefix & "_SEARCH_PAGES, " & Database.DBPrefix & "_MEMBER_FILETYPES, " & Database.DBPrefix & "_TOPIC_UPLOADSIZE, " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE, " & Database.DBPrefix & "_TOPICS_FONTSIZE, " & Database.DBPrefix & "_TOPICS_FONTCOLOR) VALUES ('" & txtForumTitle & "', 'Copyright DMG Development', '', '" & txtWebSiteURL & "', 'arial,helvetica', 2, '#A81717', 'black', 3, '#3B3738', 'gainsboro', '#B6B6B6', 'black', 2, '#3B3738', 'gainsboro', '#8D928F', 'black', '#3B3738', 'none', '#3B3738', 'none', 'black', 'underline', 'black', 'underline', 'gainsboro', 'whitesmoke', '#060237', '#3B3738', '<table width=""965"" align=""center"" cellpadding=""0"" cellspacing=""0"">" & CHR(10) & "<tr>" & CHR(10) & "<td width=""100%"" height=""100"" align=""center"" valign=""middle"" bgcolor=""[headercolor]"">" & CHR(10) & "	<span class=""PageTitle"">[PageTitle]</span>" & CHR(10) & "</td>" & CHR(10) & "</tr>" & CHR(10) & "<tr>" & CHR(10) & "<td width=""100%"" height=""26"" align=""center"" valign=""middle"" class=""MainButtons"">" & CHR(10) & "	[menu=H]" & CHR(10) & "</td>" & CHR(10) & "</tr>" & CHR(10) & "<tr>" & CHR(10) & "<td width=""100%"" align=""left"">', '</td>" & CHR(10) & "</tr>" & CHR(10) & "</table>', 15, 30, 1, 'Center Scroll Black', " & txtInstallationType & ", 10, 0, 1, 'forums, portal', 0, 'mail.yourserver.com', 'mail@yourserver.com', 0, 0, '', 1, 0, '', '', '', 0, 0, '&nbsp;&nbsp;<b>|</b>&nbsp;&nbsp;', '<br /><br />', 1, 1, 1, 1, 0, 0, 1024, '<STYLE TYPE=""text/css"">" & CHR(10) & "<!--" & CHR(10) & "A:link {text-decoration:[LinkDecoration];color:[LinkColor];}" & CHR(10) & "A:visited {text-decoration:[VLinkDecoration];color:[VLinkColor];}" & CHR(10) & "A:active {text-decoration:[ALinkDecoration];color:[ALinkColor];}" & CHR(10) & "A:hover {text-decoration:[HLinkDecoration];color:[HLinkColor];}" & CHR(10) & "body{" & CHR(10) & "	background: [BackgroundColor] url(forumimages/dmgstyles/centerscrollblack-bg.gif) repeat-y top center;" & CHR(10) & "}" & CHR(10) & ".PageTitle{" & CHR(10) & "	font-size: 55px;" & CHR(10) & "	color: [headerfontcolor];" & CHR(10) & "	font-weight: bold;" & CHR(10) & "}" & CHR(10) & ".MainButtons{" & CHR(10) & "	font-size: 17px;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "	background: silver url(forumimages/dmgstyles/centerscroll-buttonbg.jpg) repeat-x top left;" & CHR(10) & "}" & CHR(10) & ".dmgbuttons{" & CHR(10) & "	font-family: Verdana;" & CHR(10) & "	font-size: 8px;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "	border-color: [ButtonColor];}" & CHR(10) & ".LoginButton{font-size: 12px; border-color: [ButtonColor];}" & CHR(10) & ".AdminButtons{font-size: 18px; width: 270px; border-color: [HeaderColor];}" & CHR(10) & ".LoginBox{border-color: [ButtonColor];}" & CHR(10) & ".ContentBox{border:2px solid [BorderColor];}" & CHR(10) & ".HeaderCell{background-color: [HeaderColor];}" & CHR(10) & ".SubHeaderCell{background-color: [SubHeaderColor];}" & CHR(10) & ".FooterCell{background-color: [FooterColor];}" & CHR(10) & ".TableRow1{background-color: [TableBGColor1];}" & CHR(10) & ".TableRow2{background-color: [TableBGColor2];}" & CHR(10) & ".MessageBox{font-size: 20px;}" & CHR(10) & ".PhotoGalleryTable {border: 0px;}" & CHR(10) & ".PhotoGalleryTable TD{font-size: 13px; padding: 10px; text-align: center;}" & CHR(10) & "-->" & CHR(10) & "</STYLE>', '" & txtForumTitle & "', 150, 125, 1, 1, 1, 1, 'image/gif, image/png, application/msword, image/jpeg, application/zip, text/plain, image/pjpeg, application/mspowerpoint, application/vnd.ms-excel, application/pdf', 1024, 0, 2, 'black')")
				end if

				if ((txtDefaultTemplate = "LeftButtonsBlue") or (txtInstallAllTemplates = 1)) then
					'LEFT BUTTONS BLUE SETUP
					Database.Write("INSERT INTO " & Database.DBPrefix & "_SETTINGS (" & Database.DBPrefix & "_TITLE, " & Database.DBPrefix & "_COPYRIGHT, " & Database.DBPrefix & "_LOGO, " & Database.DBPrefix & "_URL, " & Database.DBPrefix & "_FONTFACE, " & Database.DBPrefix & "_FONTSIZE, " & Database.DBPrefix & "_BUTTON_COLOR, " & Database.DBPrefix & "_LOGIN_FONTCOLOR, " & Database.DBPrefix & "_HEADER_SIZE, " & Database.DBPrefix & "_HEADER_COLOR, " & Database.DBPrefix & "_HEADER_FONTCOLOR, " & Database.DBPrefix & "_SUBHEADER_COLOR, " & Database.DBPrefix & "_SUBHEADER_FONTCOLOR, " & Database.DBPrefix & "_FOOTER_SIZE, " & Database.DBPrefix & "_FOOTER_COLOR, " & Database.DBPrefix & "_FOOTER_FONTCOLOR, " & Database.DBPrefix & "_BGCOLOR, " & Database.DBPrefix & "_FONT_COLOR, " & Database.DBPrefix & "_LINK_COLOR, " & Database.DBPrefix & "_LINK_DECORATION, " & Database.DBPrefix & "_LINK_VISITED_COLOR, " & Database.DBPrefix & "_LINK_VISITED_DECORATION, " & Database.DBPrefix & "_LINK_ACTIVE_COLOR, " & Database.DBPrefix & "_LINK_ACTIVE_DECORATION, " & Database.DBPrefix & "_LINK_HOVER_COLOR, " & Database.DBPrefix & "_LINK_HOVER_DECORATION, " & Database.DBPrefix & "_TOPICS_BGCOLOR1, " & Database.DBPrefix & "_TOPICS_BGCOLOR2, " & Database.DBPrefix & "_TABLEBORDER_COLOR, " & Database.DBPrefix & "_SCROLLBAR_COLOR, " & Database.DBPrefix & "_CUSTOM_HEADER, " & Database.DBPrefix & "_CUSTOM_FOOTER, " & Database.DBPrefix & "_ITEMS_PER_PAGE, " & Database.DBPrefix & "_SPAM_FILTER, " & Database.DBPrefix & "_TEMPLATE_DEFAULT, " & Database.DBPrefix & "_TEMPLATE_NAME, " & Database.DBPrefix & "_FORUMS_DEFAULT, " & Database.DBPrefix & "_MARGIN_SIDE, " & Database.DBPrefix & "_MARGIN_TOP, " & Database.DBPrefix & "_SHOWSTATISTICS, " & Database.DBPrefix & "_CUSTOM_META, " & Database.DBPrefix & "_MEMBER_VALIDATION, " & Database.DBPrefix & "_EMAIL_SMTP, " & Database.DBPrefix & "_EMAIL_ADDRESS, " & Database.DBPrefix & "_EMAIL_ALLOWSEND, " & Database.DBPrefix & "_EMAIL_ALLOWSUB, " & Database.DBPrefix & "_BGIMAGE, " & Database.DBPrefix & "_ALLOWSUB, " & Database.DBPrefix & "_QUICK_REGISTRATION, " & Database.DBPrefix & "_EMAIL_PORT, " & Database.DBPrefix & "_EMAIL_USERNAME, " & Database.DBPrefix & "_EMAIL_PASSWORD, " & Database.DBPrefix & "_CURSE_FILTER, " & Database.DBPrefix & "_RSS_FEEDS, " & Database.DBPrefix & "_HORIZ_DIVIDE, " & Database.DBPrefix & "_VERT_DIVIDE, " & Database.DBPrefix & "_ALLOW_EDITS, " & Database.DBPrefix & "_ALLOW_REGISTRATION, " & Database.DBPrefix & "_ALLOW_MEDIA, " & Database.DBPrefix & "_ALLOW_REPORTING, " & Database.DBPrefix & "_HIDE_MEMBERS, " & Database.DBPrefix & "_HIDE_LOGIN, " & Database.DBPrefix & "_MEMBER_PHOTOSIZE, " & Database.DBPrefix & "_CUSTOM_CSS, " & Database.DBPrefix & "_HTML_TITLE, " & Database.DBPrefix & "_THUMBNAIL_SIZE, " & Database.DBPrefix & "_AVATAR_SIZE, " & Database.DBPrefix & "_SEARCH_TOPICS, " & Database.DBPrefix & "_SEARCH_MEMBERS, " & Database.DBPrefix & "_SEARCH_BLOGS, " & Database.DBPrefix & "_SEARCH_PAGES, " & Database.DBPrefix & "_MEMBER_FILETYPES, " & Database.DBPrefix & "_TOPIC_UPLOADSIZE, " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE, " & Database.DBPrefix & "_TOPICS_FONTSIZE, " & Database.DBPrefix & "_TOPICS_FONTCOLOR) VALUES ('" & txtForumTitle & "', 'Copyright DMG Development', '', '" & txtWebSiteURL & "', 'arial,helvetica', 2, '#133991', 'black', 3, '#133991', 'gainsboro', '#B6B6B6', 'black', 2, '#133991', 'gainsboro', 'silver', 'black', '#133991', 'none', '#133991', 'none', '#38579E', 'underline', '#38579E', 'underline', 'gainsboro', 'whitesmoke', '#312D2E', '#133991', '<table width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"">" & CHR(10) & "<tr>" & CHR(10) & "<td width=""100%"" height=""100"" align=""center"" valign=""middle"" bgcolor=""[headercolor]"" background=""forumimages/dmgstyles/bluegradients-topbg.jpg"">" & CHR(10) & "	<span class=""PageTitle"">[PageTitle]</span>" & CHR(10) & "</td>" & CHR(10) & "</tr>" & CHR(10) & "</table>" & CHR(10) & "<table width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"">" & CHR(10) & "<tr>" & CHR(10) & "<td width=""160"" align=""left"" valign=""top"" class=""MainButtons"">" & CHR(10) & "	<div>[menu=V]</div>" & CHR(10) & "</td>" & CHR(10) & "<td align=""left"">', '</td>" & CHR(10) & "</tr>" & CHR(10) & "</table>', 15, 30, 1, 'Left Buttons Blue', " & txtInstallationType & ", 0, 0, 1, 'forums, portal', 0, 'mail.yourserver.com', 'mail@yourserver.com', 0, 0, '', 1, 0, '', '', '', 0, 0, '&nbsp;&nbsp;<b>|</b>&nbsp;&nbsp;', '<br /><br />', 1, 1, 1, 1, 0, 0, 1024, '<STYLE TYPE=""text/css"">" & CHR(10) & "<!--" & CHR(10) & "A:link {text-decoration:[LinkDecoration];color:[LinkColor];}" & CHR(10) & "A:visited {text-decoration:[VLinkDecoration];color:[VLinkColor];}" & CHR(10) & "A:active {text-decoration:[ALinkDecoration];color:[ALinkColor];}" & CHR(10) & "A:hover {text-decoration:[HLinkDecoration];color:[HLinkColor];}" & CHR(10) & "body{" & CHR(10) & "	background: [BackgroundColor] url(forumimages/dmgstyles/leftbuttons-bg.gif) repeat-y top left;" & CHR(10) & "}" & CHR(10) & ".PageTitle{" & CHR(10) & "	font-size: 55px;" & CHR(10) & "	color: [headerfontcolor];" & CHR(10) & "	font-weight: bold;" & CHR(10) & "}" & CHR(10) & ".MainButtons{" & CHR(10) & "	font-size: 17px;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "	background-color: [BorderColor];" & CHR(10) & "}" & CHR(10) & ".MainButtons div{padding: 6px;}" & CHR(10) & ".MainButtons div A:link{color:[TableBGColor1];}" & CHR(10) & ".MainButtons div A:visited{color:[TableBGColor1];}" & CHR(10) & ".MainButtons div A:active{color:[TableBGColor2];}" & CHR(10) & ".MainButtons div A:hover{color:[TableBGColor2];}" & CHR(10) & ".dmgbuttons{" & CHR(10) & "	font-family: Verdana;" & CHR(10) & "	font-size: 8px;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "	border-color: [ButtonColor];}" & CHR(10) & ".LoginButton{font-size: 12px; border-color: [ButtonColor];}" & CHR(10) & ".AdminButtons{font-size: 18px; width: 270px; border-color: [HeaderColor];}" & CHR(10) & ".LoginBox{border-color: [ButtonColor];}" & CHR(10) & ".ContentBox{border:2px solid [BorderColor];}" & CHR(10) & ".HeaderCell{background-color: [HeaderColor];}" & CHR(10) & ".SubHeaderCell{background-color: [SubHeaderColor];}" & CHR(10) & ".FooterCell{background-color: [FooterColor];}" & CHR(10) & ".TableRow1{background-color: [TableBGColor1];}" & CHR(10) & ".TableRow2{background-color: [TableBGColor2];}" & CHR(10) & ".MessageBox{font-size: 20px;}" & CHR(10) & ".PhotoGalleryTable {border: 0px;}" & CHR(10) & ".PhotoGalleryTable TD{font-size: 13px; padding: 10px; text-align: center;}" & CHR(10) & "-->" & CHR(10) & "</STYLE>', '" & txtForumTitle & "', 150, 125, 1, 1, 1, 1, 'image/gif, image/png, application/msword, image/jpeg, application/zip, text/plain, image/pjpeg, application/mspowerpoint, application/vnd.ms-excel, application/pdf', 1024, 0, 2, 'black')")
				end if

				if ((txtDefaultTemplate = "LeftButtonsRed") or (txtInstallAllTemplates = 1)) then
					'LEFT BUTTONS RED SETUP
					Database.Write("INSERT INTO " & Database.DBPrefix & "_SETTINGS (" & Database.DBPrefix & "_TITLE, " & Database.DBPrefix & "_COPYRIGHT, " & Database.DBPrefix & "_LOGO, " & Database.DBPrefix & "_URL, " & Database.DBPrefix & "_FONTFACE, " & Database.DBPrefix & "_FONTSIZE, " & Database.DBPrefix & "_BUTTON_COLOR, " & Database.DBPrefix & "_LOGIN_FONTCOLOR, " & Database.DBPrefix & "_HEADER_SIZE, " & Database.DBPrefix & "_HEADER_COLOR, " & Database.DBPrefix & "_HEADER_FONTCOLOR, " & Database.DBPrefix & "_SUBHEADER_COLOR, " & Database.DBPrefix & "_SUBHEADER_FONTCOLOR, " & Database.DBPrefix & "_FOOTER_SIZE, " & Database.DBPrefix & "_FOOTER_COLOR, " & Database.DBPrefix & "_FOOTER_FONTCOLOR, " & Database.DBPrefix & "_BGCOLOR, " & Database.DBPrefix & "_FONT_COLOR, " & Database.DBPrefix & "_LINK_COLOR, " & Database.DBPrefix & "_LINK_DECORATION, " & Database.DBPrefix & "_LINK_VISITED_COLOR, " & Database.DBPrefix & "_LINK_VISITED_DECORATION, " & Database.DBPrefix & "_LINK_ACTIVE_COLOR, " & Database.DBPrefix & "_LINK_ACTIVE_DECORATION, " & Database.DBPrefix & "_LINK_HOVER_COLOR, " & Database.DBPrefix & "_LINK_HOVER_DECORATION, " & Database.DBPrefix & "_TOPICS_BGCOLOR1, " & Database.DBPrefix & "_TOPICS_BGCOLOR2, " & Database.DBPrefix & "_TABLEBORDER_COLOR, " & Database.DBPrefix & "_SCROLLBAR_COLOR, " & Database.DBPrefix & "_CUSTOM_HEADER, " & Database.DBPrefix & "_CUSTOM_FOOTER, " & Database.DBPrefix & "_ITEMS_PER_PAGE, " & Database.DBPrefix & "_SPAM_FILTER, " & Database.DBPrefix & "_TEMPLATE_DEFAULT, " & Database.DBPrefix & "_TEMPLATE_NAME, " & Database.DBPrefix & "_FORUMS_DEFAULT, " & Database.DBPrefix & "_MARGIN_SIDE, " & Database.DBPrefix & "_MARGIN_TOP, " & Database.DBPrefix & "_SHOWSTATISTICS, " & Database.DBPrefix & "_CUSTOM_META, " & Database.DBPrefix & "_MEMBER_VALIDATION, " & Database.DBPrefix & "_EMAIL_SMTP, " & Database.DBPrefix & "_EMAIL_ADDRESS, " & Database.DBPrefix & "_EMAIL_ALLOWSEND, " & Database.DBPrefix & "_EMAIL_ALLOWSUB, " & Database.DBPrefix & "_BGIMAGE, " & Database.DBPrefix & "_ALLOWSUB, " & Database.DBPrefix & "_QUICK_REGISTRATION, " & Database.DBPrefix & "_EMAIL_PORT, " & Database.DBPrefix & "_EMAIL_USERNAME, " & Database.DBPrefix & "_EMAIL_PASSWORD, " & Database.DBPrefix & "_CURSE_FILTER, " & Database.DBPrefix & "_RSS_FEEDS, " & Database.DBPrefix & "_HORIZ_DIVIDE, " & Database.DBPrefix & "_VERT_DIVIDE, " & Database.DBPrefix & "_ALLOW_EDITS, " & Database.DBPrefix & "_ALLOW_REGISTRATION, " & Database.DBPrefix & "_ALLOW_MEDIA, " & Database.DBPrefix & "_ALLOW_REPORTING, " & Database.DBPrefix & "_HIDE_MEMBERS, " & Database.DBPrefix & "_HIDE_LOGIN, " & Database.DBPrefix & "_MEMBER_PHOTOSIZE, " & Database.DBPrefix & "_CUSTOM_CSS, " & Database.DBPrefix & "_HTML_TITLE, " & Database.DBPrefix & "_THUMBNAIL_SIZE, " & Database.DBPrefix & "_AVATAR_SIZE, " & Database.DBPrefix & "_SEARCH_TOPICS, " & Database.DBPrefix & "_SEARCH_MEMBERS, " & Database.DBPrefix & "_SEARCH_BLOGS, " & Database.DBPrefix & "_SEARCH_PAGES, " & Database.DBPrefix & "_MEMBER_FILETYPES, " & Database.DBPrefix & "_TOPIC_UPLOADSIZE, " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE, " & Database.DBPrefix & "_TOPICS_FONTSIZE, " & Database.DBPrefix & "_TOPICS_FONTCOLOR) VALUES ('" & txtForumTitle & "', 'Copyright DMG Development', '', '" & txtWebSiteURL & "', 'arial,helvetica', 2, '#133991', 'black', 3, '#A81717', 'gainsboro', '#B6B6B6', 'black', 2, '#A81717', 'gainsboro', 'silver', 'black', '#A81717', 'none', '#A81717', 'none', '#B44141', 'underline', '#B44141', 'underline', 'gainsboro', 'whitesmoke', '#312D2E', '#A81717', '<table width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"">" & CHR(10) & "<tr>" & CHR(10) & "<td width=""100%"" height=""100"" align=""center"" valign=""middle"" bgcolor=""[headercolor]"" background=""forumimages/dmgstyles/redgradients-topbg.jpg"">" & CHR(10) & "	<span class=""PageTitle"">[PageTitle]</span>" & CHR(10) & "</td>" & CHR(10) & "</tr>" & CHR(10) & "</table>" & CHR(10) & "<table width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"">" & CHR(10) & "<tr>" & CHR(10) & "<td width=""160"" align=""left"" valign=""top"" class=""MainButtons"">" & CHR(10) & "	<div>[menu=V]</div>" & CHR(10) & "</td>" & CHR(10) & "<td align=""left"">', '</td>" & CHR(10) & "</tr>" & CHR(10) & "</table>', 15, 30, 1, 'Left Buttons Red', " & txtInstallationType & ", 0, 0, 1, 'forums, portal', 0, 'mail.yourserver.com', 'mail@yourserver.com', 0, 0, '', 1, 0, '', '', '', 0, 0, '&nbsp;&nbsp;<b>|</b>&nbsp;&nbsp;', '<br /><br />', 1, 1, 1, 1, 0, 0, 1024, '<STYLE TYPE=""text/css"">" & CHR(10) & "<!--" & CHR(10) & "A:link {text-decoration:[LinkDecoration];color:[LinkColor];}" & CHR(10) & "A:visited {text-decoration:[VLinkDecoration];color:[VLinkColor];}" & CHR(10) & "A:active {text-decoration:[ALinkDecoration];color:[ALinkColor];}" & CHR(10) & "A:hover {text-decoration:[HLinkDecoration];color:[HLinkColor];}" & CHR(10) & "body{" & CHR(10) & "	background: [BackgroundColor] url(forumimages/dmgstyles/leftbuttons-bg.gif) repeat-y top left;" & CHR(10) & "}" & CHR(10) & ".PageTitle{" & CHR(10) & "	font-size: 55px;" & CHR(10) & "	color: [headerfontcolor];" & CHR(10) & "	font-weight: bold;" & CHR(10) & "}" & CHR(10) & ".MainButtons{" & CHR(10) & "	font-size: 17px;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "	background-color: [BorderColor];" & CHR(10) & "}" & CHR(10) & ".MainButtons div{padding: 6px;}" & CHR(10) & ".MainButtons div A:link{color:[TableBGColor1];}" & CHR(10) & ".MainButtons div A:visited{color:[TableBGColor1];}" & CHR(10) & ".MainButtons div A:active{color:[TableBGColor2];}" & CHR(10) & ".MainButtons div A:hover{color:[TableBGColor2];}" & CHR(10) & ".dmgbuttons{" & CHR(10) & "	font-family: Verdana;" & CHR(10) & "	font-size: 8px;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "	border-color: [ButtonColor];}" & CHR(10) & ".LoginButton{font-size: 12px; border-color: [ButtonColor];}" & CHR(10) & ".AdminButtons{font-size: 18px; width: 270px; border-color: [HeaderColor];}" & CHR(10) & ".LoginBox{border-color: [ButtonColor];}" & CHR(10) & ".ContentBox{border:2px solid [BorderColor];}" & CHR(10) & ".HeaderCell{background-color: [HeaderColor];}" & CHR(10) & ".SubHeaderCell{background-color: [SubHeaderColor];}" & CHR(10) & ".FooterCell{background-color: [FooterColor];}" & CHR(10) & ".TableRow1{background-color: [TableBGColor1];}" & CHR(10) & ".TableRow2{background-color: [TableBGColor2];}" & CHR(10) & ".MessageBox{font-size: 20px;}" & CHR(10) & ".PhotoGalleryTable {border: 0px;}" & CHR(10) & ".PhotoGalleryTable TD{font-size: 13px; padding: 10px; text-align: center;}" & CHR(10) & "-->" & CHR(10) & "</STYLE>', '" & txtForumTitle & "', 150, 125, 1, 1, 1, 1, 'image/gif, image/png, application/msword, image/jpeg, application/zip, text/plain, image/pjpeg, application/mspowerpoint, application/vnd.ms-excel, application/pdf', 1024, 0, 2, 'black')")
				end if

				if ((txtDefaultTemplate = "LeftButtonsGreen") or (txtInstallAllTemplates = 1)) then
					'LEFT BUTTONS GREEN SETUP
					Database.Write("INSERT INTO " & Database.DBPrefix & "_SETTINGS (" & Database.DBPrefix & "_TITLE, " & Database.DBPrefix & "_COPYRIGHT, " & Database.DBPrefix & "_LOGO, " & Database.DBPrefix & "_URL, " & Database.DBPrefix & "_FONTFACE, " & Database.DBPrefix & "_FONTSIZE, " & Database.DBPrefix & "_BUTTON_COLOR, " & Database.DBPrefix & "_LOGIN_FONTCOLOR, " & Database.DBPrefix & "_HEADER_SIZE, " & Database.DBPrefix & "_HEADER_COLOR, " & Database.DBPrefix & "_HEADER_FONTCOLOR, " & Database.DBPrefix & "_SUBHEADER_COLOR, " & Database.DBPrefix & "_SUBHEADER_FONTCOLOR, " & Database.DBPrefix & "_FOOTER_SIZE, " & Database.DBPrefix & "_FOOTER_COLOR, " & Database.DBPrefix & "_FOOTER_FONTCOLOR, " & Database.DBPrefix & "_BGCOLOR, " & Database.DBPrefix & "_FONT_COLOR, " & Database.DBPrefix & "_LINK_COLOR, " & Database.DBPrefix & "_LINK_DECORATION, " & Database.DBPrefix & "_LINK_VISITED_COLOR, " & Database.DBPrefix & "_LINK_VISITED_DECORATION, " & Database.DBPrefix & "_LINK_ACTIVE_COLOR, " & Database.DBPrefix & "_LINK_ACTIVE_DECORATION, " & Database.DBPrefix & "_LINK_HOVER_COLOR, " & Database.DBPrefix & "_LINK_HOVER_DECORATION, " & Database.DBPrefix & "_TOPICS_BGCOLOR1, " & Database.DBPrefix & "_TOPICS_BGCOLOR2, " & Database.DBPrefix & "_TABLEBORDER_COLOR, " & Database.DBPrefix & "_SCROLLBAR_COLOR, " & Database.DBPrefix & "_CUSTOM_HEADER, " & Database.DBPrefix & "_CUSTOM_FOOTER, " & Database.DBPrefix & "_ITEMS_PER_PAGE, " & Database.DBPrefix & "_SPAM_FILTER, " & Database.DBPrefix & "_TEMPLATE_DEFAULT, " & Database.DBPrefix & "_TEMPLATE_NAME, " & Database.DBPrefix & "_FORUMS_DEFAULT, " & Database.DBPrefix & "_MARGIN_SIDE, " & Database.DBPrefix & "_MARGIN_TOP, " & Database.DBPrefix & "_SHOWSTATISTICS, " & Database.DBPrefix & "_CUSTOM_META, " & Database.DBPrefix & "_MEMBER_VALIDATION, " & Database.DBPrefix & "_EMAIL_SMTP, " & Database.DBPrefix & "_EMAIL_ADDRESS, " & Database.DBPrefix & "_EMAIL_ALLOWSEND, " & Database.DBPrefix & "_EMAIL_ALLOWSUB, " & Database.DBPrefix & "_BGIMAGE, " & Database.DBPrefix & "_ALLOWSUB, " & Database.DBPrefix & "_QUICK_REGISTRATION, " & Database.DBPrefix & "_EMAIL_PORT, " & Database.DBPrefix & "_EMAIL_USERNAME, " & Database.DBPrefix & "_EMAIL_PASSWORD, " & Database.DBPrefix & "_CURSE_FILTER, " & Database.DBPrefix & "_RSS_FEEDS, " & Database.DBPrefix & "_HORIZ_DIVIDE, " & Database.DBPrefix & "_VERT_DIVIDE, " & Database.DBPrefix & "_ALLOW_EDITS, " & Database.DBPrefix & "_ALLOW_REGISTRATION, " & Database.DBPrefix & "_ALLOW_MEDIA, " & Database.DBPrefix & "_ALLOW_REPORTING, " & Database.DBPrefix & "_HIDE_MEMBERS, " & Database.DBPrefix & "_HIDE_LOGIN, " & Database.DBPrefix & "_MEMBER_PHOTOSIZE, " & Database.DBPrefix & "_CUSTOM_CSS, " & Database.DBPrefix & "_HTML_TITLE, " & Database.DBPrefix & "_THUMBNAIL_SIZE, " & Database.DBPrefix & "_AVATAR_SIZE, " & Database.DBPrefix & "_SEARCH_TOPICS, " & Database.DBPrefix & "_SEARCH_MEMBERS, " & Database.DBPrefix & "_SEARCH_BLOGS, " & Database.DBPrefix & "_SEARCH_PAGES, " & Database.DBPrefix & "_MEMBER_FILETYPES, " & Database.DBPrefix & "_TOPIC_UPLOADSIZE, " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE, " & Database.DBPrefix & "_TOPICS_FONTSIZE, " & Database.DBPrefix & "_TOPICS_FONTCOLOR) VALUES ('" & txtForumTitle & "', 'Copyright DMG Development', '', '" & txtWebSiteURL & "', 'arial,helvetica', 2, '#133991', 'black', 3, '#006729', 'gainsboro', '#B6B6B6', 'black', 2, '#006729', 'gainsboro', 'silver', 'black', '#006729', 'none', '#006729', 'none', '#2F9156', 'underline', '#2F9156', 'underline', 'gainsboro', 'whitesmoke', '#312D2E', '#006729', '<table width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"">" & CHR(10) & "<tr>" & CHR(10) & "<td width=""100%"" height=""100"" align=""center"" valign=""middle"" bgcolor=""[headercolor]"" background=""forumimages/dmgstyles/greengradients-topbg.jpg"">" & CHR(10) & "	<span class=""PageTitle"">[PageTitle]</span>" & CHR(10) & "</td>" & CHR(10) & "</tr>" & CHR(10) & "</table>" & CHR(10) & "<table width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"">" & CHR(10) & "<tr>" & CHR(10) & "<td width=""160"" align=""left"" valign=""top"" class=""MainButtons"">" & CHR(10) & "	<div>[menu=V]</div>" & CHR(10) & "</td>" & CHR(10) & "<td align=""left"">', '</td>" & CHR(10) & "</tr>" & CHR(10) & "</table>', 15, 30, 1, 'Left Buttons Green', " & txtInstallationType & ", 0, 0, 1, 'forums, portal', 0, 'mail.yourserver.com', 'mail@yourserver.com', 0, 0, '', 1, 0, '', '', '', 0, 0, '&nbsp;&nbsp;<b>|</b>&nbsp;&nbsp;', '<br /><br />', 1, 1, 1, 1, 0, 0, 1024, '<STYLE TYPE=""text/css"">" & CHR(10) & "<!--" & CHR(10) & "A:link {text-decoration:[LinkDecoration];color:[LinkColor];}" & CHR(10) & "A:visited {text-decoration:[VLinkDecoration];color:[VLinkColor];}" & CHR(10) & "A:active {text-decoration:[ALinkDecoration];color:[ALinkColor];}" & CHR(10) & "A:hover {text-decoration:[HLinkDecoration];color:[HLinkColor];}" & CHR(10) & "body{" & CHR(10) & "	background: [BackgroundColor] url(forumimages/dmgstyles/leftbuttons-bg.gif) repeat-y top left;" & CHR(10) & "}" & CHR(10) & ".PageTitle{" & CHR(10) & "	font-size: 55px;" & CHR(10) & "	color: [headerfontcolor];" & CHR(10) & "	font-weight: bold;" & CHR(10) & "}" & CHR(10) & ".MainButtons{" & CHR(10) & "	font-size: 17px;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "	background-color: [BorderColor];" & CHR(10) & "}" & CHR(10) & ".MainButtons div{padding: 6px;}" & CHR(10) & ".MainButtons div A:link{color:[TableBGColor1];}" & CHR(10) & ".MainButtons div A:visited{color:[TableBGColor1];}" & CHR(10) & ".MainButtons div A:active{color:[TableBGColor2];}" & CHR(10) & ".MainButtons div A:hover{color:[TableBGColor2];}" & CHR(10) & ".dmgbuttons{" & CHR(10) & "	font-family: Verdana;" & CHR(10) & "	font-size: 8px;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "	border-color: [ButtonColor];}" & CHR(10) & ".LoginButton{font-size: 12px; border-color: [ButtonColor];}" & CHR(10) & ".AdminButtons{font-size: 18px; width: 270px; border-color: [HeaderColor];}" & CHR(10) & ".LoginBox{border-color: [ButtonColor];}" & CHR(10) & ".ContentBox{border:2px solid [BorderColor];}" & CHR(10) & ".HeaderCell{background-color: [HeaderColor];}" & CHR(10) & ".SubHeaderCell{background-color: [SubHeaderColor];}" & CHR(10) & ".FooterCell{background-color: [FooterColor];}" & CHR(10) & ".TableRow1{background-color: [TableBGColor1];}" & CHR(10) & ".TableRow2{background-color: [TableBGColor2];}" & CHR(10) & ".MessageBox{font-size: 20px;}" & CHR(10) & ".PhotoGalleryTable {border: 0px;}" & CHR(10) & ".PhotoGalleryTable TD{font-size: 13px; padding: 10px; text-align: center;}" & CHR(10) & "-->" & CHR(10) & "</STYLE>', '" & txtForumTitle & "', 150, 125, 1, 1, 1, 1, 'image/gif, image/png, application/msword, image/jpeg, application/zip, text/plain, image/pjpeg, application/mspowerpoint, application/vnd.ms-excel, application/pdf', 1024, 0, 2, 'black')")
				end if

				if ((txtDefaultTemplate = "WhiteSmoke") or (txtInstallAllTemplates = 1)) then
					'WHITESMOKE GRADIENTS SETUP
					Database.Write("INSERT INTO " & Database.DBPrefix & "_SETTINGS (" & Database.DBPrefix & "_TITLE, " & Database.DBPrefix & "_COPYRIGHT, " & Database.DBPrefix & "_LOGO, " & Database.DBPrefix & "_URL, " & Database.DBPrefix & "_FONTFACE, " & Database.DBPrefix & "_FONTSIZE, " & Database.DBPrefix & "_BUTTON_COLOR, " & Database.DBPrefix & "_LOGIN_FONTCOLOR, " & Database.DBPrefix & "_HEADER_SIZE, " & Database.DBPrefix & "_HEADER_COLOR, " & Database.DBPrefix & "_HEADER_FONTCOLOR, " & Database.DBPrefix & "_SUBHEADER_COLOR, " & Database.DBPrefix & "_SUBHEADER_FONTCOLOR, " & Database.DBPrefix & "_FOOTER_SIZE, " & Database.DBPrefix & "_FOOTER_COLOR, " & Database.DBPrefix & "_FOOTER_FONTCOLOR, " & Database.DBPrefix & "_BGCOLOR, " & Database.DBPrefix & "_FONT_COLOR, " & Database.DBPrefix & "_LINK_COLOR, " & Database.DBPrefix & "_LINK_DECORATION, " & Database.DBPrefix & "_LINK_VISITED_COLOR, " & Database.DBPrefix & "_LINK_VISITED_DECORATION, " & Database.DBPrefix & "_LINK_ACTIVE_COLOR, " & Database.DBPrefix & "_LINK_ACTIVE_DECORATION, " & Database.DBPrefix & "_LINK_HOVER_COLOR, " & Database.DBPrefix & "_LINK_HOVER_DECORATION, " & Database.DBPrefix & "_TOPICS_BGCOLOR1, " & Database.DBPrefix & "_TOPICS_BGCOLOR2, " & Database.DBPrefix & "_TABLEBORDER_COLOR, " & Database.DBPrefix & "_SCROLLBAR_COLOR, " & Database.DBPrefix & "_CUSTOM_HEADER, " & Database.DBPrefix & "_CUSTOM_FOOTER, " & Database.DBPrefix & "_ITEMS_PER_PAGE, " & Database.DBPrefix & "_SPAM_FILTER, " & Database.DBPrefix & "_TEMPLATE_DEFAULT, " & Database.DBPrefix & "_TEMPLATE_NAME, " & Database.DBPrefix & "_FORUMS_DEFAULT, " & Database.DBPrefix & "_MARGIN_SIDE, " & Database.DBPrefix & "_MARGIN_TOP, " & Database.DBPrefix & "_SHOWSTATISTICS, " & Database.DBPrefix & "_CUSTOM_META, " & Database.DBPrefix & "_MEMBER_VALIDATION, " & Database.DBPrefix & "_EMAIL_SMTP, " & Database.DBPrefix & "_EMAIL_ADDRESS, " & Database.DBPrefix & "_EMAIL_ALLOWSEND, " & Database.DBPrefix & "_EMAIL_ALLOWSUB, " & Database.DBPrefix & "_BGIMAGE, " & Database.DBPrefix & "_ALLOWSUB, " & Database.DBPrefix & "_QUICK_REGISTRATION, " & Database.DBPrefix & "_EMAIL_PORT, " & Database.DBPrefix & "_EMAIL_USERNAME, " & Database.DBPrefix & "_EMAIL_PASSWORD, " & Database.DBPrefix & "_CURSE_FILTER, " & Database.DBPrefix & "_RSS_FEEDS, " & Database.DBPrefix & "_HORIZ_DIVIDE, " & Database.DBPrefix & "_VERT_DIVIDE, " & Database.DBPrefix & "_ALLOW_EDITS, " & Database.DBPrefix & "_ALLOW_REGISTRATION, " & Database.DBPrefix & "_ALLOW_MEDIA, " & Database.DBPrefix & "_ALLOW_REPORTING, " & Database.DBPrefix & "_HIDE_MEMBERS, " & Database.DBPrefix & "_HIDE_LOGIN, " & Database.DBPrefix & "_MEMBER_PHOTOSIZE, " & Database.DBPrefix & "_CUSTOM_CSS, " & Database.DBPrefix & "_HTML_TITLE, " & Database.DBPrefix & "_THUMBNAIL_SIZE, " & Database.DBPrefix & "_AVATAR_SIZE, " & Database.DBPrefix & "_SEARCH_TOPICS, " & Database.DBPrefix & "_SEARCH_MEMBERS, " & Database.DBPrefix & "_SEARCH_BLOGS, " & Database.DBPrefix & "_SEARCH_PAGES, " & Database.DBPrefix & "_MEMBER_FILETYPES, " & Database.DBPrefix & "_TOPIC_UPLOADSIZE, " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE, " & Database.DBPrefix & "_TOPICS_FONTSIZE, " & Database.DBPrefix & "_TOPICS_FONTCOLOR) VALUES ('" & txtForumTitle & "', 'Copyright DMG Development', '', '" & txtWebSiteURL & "', 'arial,helvetica', 2, '#002657', 'black', 3, '#C0C0C0', 'black', '#B6B6B6', 'black', 2, '#C0C0C0', 'black', 'MintCream', 'black', '#002657', 'none', '#002657', 'none', '#004EBB', 'underline', '#004EBB', 'underline', 'gainsboro', 'whitesmoke', '#060237', '#004EBB', '<table border=""0"" align=""left"" cellpadding=""8"" cellspacing=""8"">" & CHR(10) & "<tr>" & CHR(10) & "<td width=""100%"" align=""center"">" & CHR(10) & "	<font size=""[HeaderSize]"" color=""[FontColor]""><b>[PageTitle]</b></font>" & CHR(10) & "	<br /><font size=""1"">[menu=H]</font>" & CHR(10) & "</td>" & CHR(10) & "</tr>" & CHR(10) & "</table>" & CHR(10) & "<br clear=""all"" />', '', 15, 30, 1, 'White Smoke', " & txtInstallationType & ", 10, 15, 1, 'forums, portal', 0, 'mail.yourserver.com', 'mail@yourserver.com', 0, 0, '', 1, 0, '', '', '', 0, 0, '&nbsp;&nbsp;<b>|</b>&nbsp;&nbsp;', '<br /><br />', 1, 1, 1, 1, 0, 0, 1024, '<STYLE TYPE=""text/css"">" & CHR(10) & "<!--" & CHR(10) & "A:link {text-decoration:[LinkDecoration];color:[LinkColor];font-weight:bold;}" & CHR(10) & "A:visited {text-decoration:[VLinkDecoration];color:[VLinkColor];font-weight:bold;}" & CHR(10) & "A:active {text-decoration:[ALinkDecoration];color:[ALinkColor];font-weight:bold;}" & CHR(10) & "A:hover {text-decoration:[HLinkDecoration];color:[HLinkColor];font-weight:bold;}" & CHR(10) & ".dmgbuttons{" & CHR(10) & "	font-family: Verdana;" & CHR(10) & "	font-size: 8px;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "	border-color: [ButtonColor];}" & CHR(10) & ".LoginButton{font-size: 12px; border-color: [ButtonColor];}" & CHR(10) & ".AdminButtons{font-size: 18px; width: 270px; border-color: [HeaderColor];}" & CHR(10) & ".LoginBox{border-color: [ButtonColor];}" & CHR(10) & ".ContentBox {" & CHR(10) & "	border-width: 1px;" & CHR(10) & "	border-style: dashed;" & CHR(10) & "	border-color: [BorderColor];" & CHR(10) & "	border-collapse: separate;" & CHR(10) & "}" & CHR(10) & ".HeaderCell{" & CHR(10) & "	color: [HeaderFontColor];" & CHR(10) & "	background-color: [HeaderColor];" & CHR(10) & "	background-image: url(forumimages/dmgstyles/whitesmoke-headerbg.jpg);" & CHR(10) & "}" & CHR(10) & ".SubHeaderCell{background-color: [SubHeaderColor];}" & CHR(10) & ".FooterCell{" & CHR(10) & "	color: [FooterFontColor];" & CHR(10) & "	background-color: [FooterColor];" & CHR(10) & "	background-image: url(forumimages/dmgstyles/whitesmoke-headerbg.jpg);" & CHR(10) & "}" & CHR(10) & ".TableRow1{background-color: [TableBGColor1];}" & CHR(10) & ".TableRow2{background-color: [TableBGColor2];}" & CHR(10) & ".MessageBox{font-size: 20px;}" & CHR(10) & ".PhotoGalleryTable {border: 0px;}" & CHR(10) & ".PhotoGalleryTable TD{font-size: 13px; padding: 10px; text-align: center;}" & CHR(10) & "-->" & CHR(10) & "</STYLE>', '" & txtForumTitle & "', 150, 125, 1, 1, 1, 1, 'image/gif, image/png, application/msword, image/jpeg, application/zip, text/plain, image/pjpeg, application/mspowerpoint, application/vnd.ms-excel, application/pdf', 1024, 0, 2, 'black')")
				end if

				Dim TemplateReader as OdbcDataReader
				if (txtDefaultTemplate = "BlankTemplate") then
					TemplateReader = Database.Read("SELECT ID FROM " & Database.DBPrefix & "_SETTINGS WHERE " & Database.DBPrefix & "_TEMPLATE_NAME = 'Blank Template'", 1)
					While TemplateReader.Read()
						Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_TEMPLATE_DEFAULT = " & TemplateReader("ID"))
					End While
					TemplateReader.Close()
				end if
				if (txtDefaultTemplate = "BlueGradients") then
					TemplateReader = Database.Read("SELECT ID FROM " & Database.DBPrefix & "_SETTINGS WHERE " & Database.DBPrefix & "_TEMPLATE_NAME = 'Blue Gradients'", 1)
					While TemplateReader.Read()
						Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_TEMPLATE_DEFAULT = " & TemplateReader("ID"))
					End While
					TemplateReader.Close()
				end if
				if (txtDefaultTemplate = "RedGradients") then
					TemplateReader = Database.Read("SELECT ID FROM " & Database.DBPrefix & "_SETTINGS WHERE " & Database.DBPrefix & "_TEMPLATE_NAME = 'Red Gradients'", 1)
					While TemplateReader.Read()
						Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_TEMPLATE_DEFAULT = " & TemplateReader("ID"))
					End While
					TemplateReader.Close()
				end if
				if (txtDefaultTemplate = "GreenGradients") then
					TemplateReader = Database.Read("SELECT ID FROM " & Database.DBPrefix & "_SETTINGS WHERE " & Database.DBPrefix & "_TEMPLATE_NAME = 'Green Gradients'", 1)
					While TemplateReader.Read()
						Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_TEMPLATE_DEFAULT = " & TemplateReader("ID"))
					End While
					TemplateReader.Close()
				end if
				if (txtDefaultTemplate = "CenterScrollBlue") then
					TemplateReader = Database.Read("SELECT ID FROM " & Database.DBPrefix & "_SETTINGS WHERE " & Database.DBPrefix & "_TEMPLATE_NAME = 'Center Scroll Blue'", 1)
					While TemplateReader.Read()
						Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_TEMPLATE_DEFAULT = " & TemplateReader("ID"))
					End While
					TemplateReader.Close()
				end if
				if (txtDefaultTemplate = "CenterScrollRed") then
					TemplateReader = Database.Read("SELECT ID FROM " & Database.DBPrefix & "_SETTINGS WHERE " & Database.DBPrefix & "_TEMPLATE_NAME = 'Center Scroll Red'", 1)
					While TemplateReader.Read()
						Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_TEMPLATE_DEFAULT = " & TemplateReader("ID"))
					End While
					TemplateReader.Close()
				end if
				if (txtDefaultTemplate = "CenterScrollGreen") then
					TemplateReader = Database.Read("SELECT ID FROM " & Database.DBPrefix & "_SETTINGS WHERE " & Database.DBPrefix & "_TEMPLATE_NAME = 'Center Scroll Green'", 1)
					While TemplateReader.Read()
						Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_TEMPLATE_DEFAULT = " & TemplateReader("ID"))
					End While
					TemplateReader.Close()
				end if
				if (txtDefaultTemplate = "CenterScrollBlack") then
					TemplateReader = Database.Read("SELECT ID FROM " & Database.DBPrefix & "_SETTINGS WHERE " & Database.DBPrefix & "_TEMPLATE_NAME = 'Center Scroll Black'", 1)
					While TemplateReader.Read()
						Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_TEMPLATE_DEFAULT = " & TemplateReader("ID"))
					End While
					TemplateReader.Close()
				end if
				if (txtDefaultTemplate = "LeftButtonsBlue") then
					TemplateReader = Database.Read("SELECT ID FROM " & Database.DBPrefix & "_SETTINGS WHERE " & Database.DBPrefix & "_TEMPLATE_NAME = 'Left Buttons Blue'", 1)
					While TemplateReader.Read()
						Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_TEMPLATE_DEFAULT = " & TemplateReader("ID"))
					End While
					TemplateReader.Close()
				end if
				if (txtDefaultTemplate = "LeftButtonsRed") then
					TemplateReader = Database.Read("SELECT ID FROM " & Database.DBPrefix & "_SETTINGS WHERE " & Database.DBPrefix & "_TEMPLATE_NAME = 'Left Buttons Red'", 1)
					While TemplateReader.Read()
						Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_TEMPLATE_DEFAULT = " & TemplateReader("ID"))
					End While
					TemplateReader.Close()
				end if
				if (txtDefaultTemplate = "LeftButtonsGreen") then
					TemplateReader = Database.Read("SELECT ID FROM " & Database.DBPrefix & "_SETTINGS WHERE " & Database.DBPrefix & "_TEMPLATE_NAME = 'Left Buttons Green'", 1)
					While TemplateReader.Read()
						Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_TEMPLATE_DEFAULT = " & TemplateReader("ID"))
					End While
					TemplateReader.Close()
				end if
				if (txtDefaultTemplate = "WhiteSmoke") then
					TemplateReader = Database.Read("SELECT ID FROM " & Database.DBPrefix & "_SETTINGS WHERE " & Database.DBPrefix & "_TEMPLATE_NAME = 'White Smoke'", 1)
					While TemplateReader.Read()
						Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_TEMPLATE_DEFAULT = " & TemplateReader("ID"))
					End While
					TemplateReader.Close()
				end if

				Database.Write("INSERT INTO " & Database.DBPrefix & "_MEMBERS (MEMBER_USERNAME, MEMBER_PASSWORD, MEMBER_LEVEL, MEMBER_EMAIL, MEMBER_LOCATION, MEMBER_HOMEPAGE, MEMBER_SIGNATURE, MEMBER_SIGNATURE_SHOW, MEMBER_IM_AOL, MEMBER_IM_ICQ, MEMBER_IM_MSN, MEMBER_IM_YAHOO, MEMBER_POSTS, MEMBER_DATE_JOINED, MEMBER_DATE_LASTVISIT, MEMBER_DATE_LASTPOST, MEMBER_TITLE, MEMBER_TITLE_ALLOWCUSTOM, MEMBER_TITLE_USECUSTOM, MEMBER_EMAIL_SHOW, MEMBER_IP_LAST, MEMBER_IP_ORIGINAL, MEMBER_REALNAME, MEMBER_OCCUPATION, MEMBER_SEX, MEMBER_AGE, MEMBER_BIRTHDAY, MEMBER_NOTES, MEMBER_FAVORITESITE, MEMBER_PHOTO, MEMBER_AVATAR, MEMBER_AVATAR_SHOW, MEMBER_AVATAR_ALLOWCUSTOM, MEMBER_AVATAR_USECUSTOM, MEMBER_AVATAR_CUSTOMLOADED, MEMBER_AVATAR_CUSTOMTYPE, MEMBER_VALIDATED, MEMBER_VALIDATION_STRING, MEMBER_RANKING) VALUES ('" & txtAdminUsername & "','" & txtAdminPassword & "', 3, '" & txtAdminEmail & "', '', '" & txtWebSiteURL & "', '', 1, '', '', '', '', 1, " & Database.GetTimeStamp() & ", " & Database.GetTimeStamp() & ", " & Database.GetTimeStamp() & ", 'Administrator', 1, 1, 1, '" & Request.UserHostAddress() & "', '" & Request.UserHostAddress() & "', '', '', '', '', '', '', '', '', 1, 0, 1, 0, 0, 'jpg', 1, '', 0)")
				Database.Write("INSERT INTO " & Database.DBPrefix & "_CATEGORIES (CATEGORY_NAME, CATEGORY_STATUS, CATEGORY_SORTBY, CATEGORY_CONTENT, CATEGORY_SHOWHEADERS, CATEGORY_SHOWLOGIN) VALUES ('Main Forums', 1, 1, '', 1, 1)")
				Database.Write("INSERT INTO " & Database.DBPrefix & "_FORUMS (CATEGORY_ID, FORUM_STATUS, FORUM_SORTBY, FORUM_NAME, FORUM_DESCRIPTION, FORUM_TOPICS, FORUM_POSTS, FORUM_LASTPOST_AUTHOR, FORUM_LASTPOST_TOPIC, FORUM_LASTPOST_DATE, FORUM_TYPE, FORUM_PASSWORD, FORUM_FORCECONFIRM, FORUM_SHOWLATEST, FORUM_CONTENT, FORUM_SHOWHEADERS, FORUM_SHOWLOGIN, FORUM_EMAIL_MODERATORS) VALUES (1, 1, 1, 'General Discussion', 'This forum is for topics of all types for all members.', 1, 1, 1, 1, " & Database.GetTimeStamp() & ", 0, '', 0, 1, '', 1, 1, 0)")
				Database.Write("INSERT INTO " & Database.DBPrefix & "_TOPICS (CATEGORY_ID, FORUM_ID, TOPIC_SUBJECT, TOPIC_MESSAGE, TOPIC_AUTHOR, TOPIC_DATE, TOPIC_REPLIES, TOPIC_VIEWS, TOPIC_LASTPOST_DATE, TOPIC_LASTPOST_AUTHOR, TOPIC_STICKY, TOPIC_SIGNATURE, TOPIC_STATUS, TOPIC_NEWS, TOPIC_CONFIRMED, TOPIC_UNCONFIRMED_REPLIES, TOPIC_FILEUPLOAD) VALUES (1, 1, 'Welcome To The Forums', 'The forums are now open for all conversation.  Please feel free to join in on the discussion.', 1, " & Database.GetTimeStamp() & ", 0, 0, " & Database.GetTimeStamp() & ", 1, 0, 1, 1, 0, 1, 0, '')")
				Database.Write("INSERT INTO " & Database.DBPrefix & "_RANKINGS (RANK_NAME, RANK_IMAGE, RANK_POSTS, RANK_ALLOW_TOPICS, RANK_ALLOW_AVATAR, RANK_ALLOW_AVATAR_CUSTOM, RANK_ALLOW_TITLE, RANK_ALLOW_UPLOADS) VALUES ('Newbie', 'rank_0.gif', 0, 1, 1, 0, 0, 0)")
				Database.Write("INSERT INTO " & Database.DBPrefix & "_RANKINGS (RANK_NAME, RANK_IMAGE, RANK_POSTS, RANK_ALLOW_TOPICS, RANK_ALLOW_AVATAR, RANK_ALLOW_AVATAR_CUSTOM, RANK_ALLOW_TITLE, RANK_ALLOW_UPLOADS) VALUES ('Beginner', 'rank_1.gif', 10, 1, 1, 0, 0, 0)")
				Database.Write("INSERT INTO " & Database.DBPrefix & "_RANKINGS (RANK_NAME, RANK_IMAGE, RANK_POSTS, RANK_ALLOW_TOPICS, RANK_ALLOW_AVATAR, RANK_ALLOW_AVATAR_CUSTOM, RANK_ALLOW_TITLE, RANK_ALLOW_UPLOADS) VALUES ('Intermediate', 'rank_2.gif', 50, 1, 1, 0, 0, 0)")
				Database.Write("INSERT INTO " & Database.DBPrefix & "_RANKINGS (RANK_NAME, RANK_IMAGE, RANK_POSTS, RANK_ALLOW_TOPICS, RANK_ALLOW_AVATAR, RANK_ALLOW_AVATAR_CUSTOM, RANK_ALLOW_TITLE, RANK_ALLOW_UPLOADS) VALUES ('Forum Regular', 'rank_3.gif', 200, 1, 1, 1, 0, 0)")
				Database.Write("INSERT INTO " & Database.DBPrefix & "_RANKINGS (RANK_NAME, RANK_IMAGE, RANK_POSTS, RANK_ALLOW_TOPICS, RANK_ALLOW_AVATAR, RANK_ALLOW_AVATAR_CUSTOM, RANK_ALLOW_TITLE, RANK_ALLOW_UPLOADS) VALUES ('Forum Superstar', 'rank_4.gif', 500, 1, 1, 1, 0, 0)")
				Database.Write("INSERT INTO " & Database.DBPrefix & "_RANKINGS (RANK_NAME, RANK_IMAGE, RANK_POSTS, RANK_ALLOW_TOPICS, RANK_ALLOW_AVATAR, RANK_ALLOW_AVATAR_CUSTOM, RANK_ALLOW_TITLE, RANK_ALLOW_UPLOADS) VALUES ('Best of the Best', 'rank_5.gif', 1000, 1, 1, 1, 1, 0)")
				Database.Write("INSERT INTO " & Database.DBPrefix & "_AVATARS (AVATAR_NAME, AVATAR_IMAGE) VALUES ('Default Avatar', 'default.gif')")
				Database.Write("INSERT INTO " & Database.DBPrefix & "_VARIABLES (VAR1, VAR2, VAR3, VAR4, VAR5, TEXT1, TEXT2, TEXT3, TEXT4, TEXT5) VALUES ('Variable 1 Text', 'Variable 2 Text', 'Variable 3 Text', 'Variable 4 Text', 'Variable 5 Text', 'Memo Field 1', 'Memo Field 2', 'Memo Field 3', 'Memo Field 4', 'Memo Field 5')")

				if ((txtInstallationType = 1) or (txtInstallationType = 2)) then
					Database.Write("INSERT INTO " & Database.DBPrefix & "_PAGES (PAGE_PARENT, PAGE_NAME, PAGE_TITLE, PAGE_CONTENT, PAGE_SHOWTITLE, PAGE_SHOWHEADERS, PAGE_SHOWLOGIN, PAGE_STATUS, PAGE_SORT, PAGE_AUTOFORMAT, PAGE_SECURITY, PAGE_PASSWORD, PAGE_SUB_TITLE, PAGE_SUB_SHOWTITLE, PAGE_SUB_COLUMNS, PAGE_SUB_ALIGN, PAGE_SUB_STATUS, PAGE_THUMBNAIL, PAGE_PHOTO) VALUES (0, 'Main Page', 'Not Displayed (can be edited in HTML tags below)', '<table border=""0"" width=""100%"" cellpadding=""0"" cellspacing=""0"">" & CHR(10) & "<tr>" & CHR(10) & "<td valign=""top"">" & CHR(10) & "[ContentBox=Welcome To DMG Forums]" & CHR(10) & "Your forum is now installed and ready to use.  This content box can be edited by logging in as the administrator and going to ""Edit Pages"" in the Administration tool." & CHR(10) & "<br /><br />" & CHR(10) & "If you would not like to display custom text above the forums, go to the Main Configuration and choose ""Forums Only"" in the Main Page Options drop-down box." & CHR(10) & "<br /><br />" & CHR(10) & "Be sure to review the <a href=""javascript:openHelp(''DMGAdminCode.html'')"">DMG Admin Code</a> help page to find out all of the custom codes that can be placed in this text field." & CHR(10) & "<br /><br />" & CHR(10) & "To the right of this table is the latest topics box.  This was accomplished by using the &#91;LatestTopics=5&#93;Latest Topics&#91;/LatestTopics&#93; command.  This command draws the box that you see and will automatically display the last 5 (or whatever number you specify) topics that have been active.  After installation there will not be 5 topics displayed because only one sample thread has been generated for you." & CHR(10) & "<br /><br />" & CHR(10) & "The box that this text appears in is automatically formatted using the &#91;ContentBox&#93; code.  Check the HTML for the main page to see all of the custom codes that have been used to generate this page." & CHR(10) & "<br /><br />" & CHR(10) & "This is only a small sample of the widgets and options you can enable with <a href=""javascript:openHelp(''DMGAdminCode.html'')"">DMG Admin Codes</a>." & CHR(10) & "[/ContentBox]" & CHR(10) & "</td>" & CHR(10) & "<td width=""10"">&nbsp;</td>" & CHR(10) & "<td width=""150"" valign=""top"">" & CHR(10) & "[LatestTopics=5]" & CHR(10) & "</td>" & CHR(10) & "</tr>" & CHR(10) & "</table>', 0, 1, 1, 1, 0, 0, 0, '', 'Sub-Categories', 0, 1, 1, 1, '', '')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (1, 'Main Page', 1, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (2, 'Forums', 3, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (3, 'Register', 4, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (4, 'Active Topics', 5, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (5, 'Members', 6, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (6, 'Search', 7, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (7, 'User CP', 10, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (8, 'Private Messages', 11, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (9, 'Administration', 13, '0', 0)")
				else
					Database.Write("INSERT INTO " & Database.DBPrefix & "_PAGES (PAGE_PARENT, PAGE_NAME, PAGE_TITLE, PAGE_CONTENT, PAGE_SHOWTITLE, PAGE_SHOWHEADERS, PAGE_SHOWLOGIN, PAGE_STATUS, PAGE_SORT, PAGE_AUTOFORMAT, PAGE_SECURITY, PAGE_PASSWORD, PAGE_SUB_TITLE, PAGE_SUB_SHOWTITLE, PAGE_SUB_COLUMNS, PAGE_SUB_ALIGN, PAGE_SUB_STATUS, PAGE_THUMBNAIL, PAGE_PHOTO) VALUES (0, 'Main Page', 'Not Displayed (can be edited in HTML tags below)', '[ContentBox=Welcome To DMG Forums]" & CHR(10) & "Your web site is now installed and ready to use.  This content box can be edited by logging in as the administrator and going to ""Edit Pages"" in the Administration tool." & CHR(10) & "<br /><br />" & CHR(10) & "You can add other content pages by clicking ""Create New Page"" on the admin screen." & CHR(10) & "<br /><br />" & CHR(10) & "Be sure to review the <a href=""javascript:openHelp(''DMGAdminCode.html'')"">DMG Admin Code</a> help page to find out all of the custom codes that can be placed in this text field." & CHR(10) & "<br /><br />" & CHR(10) & "The box that this text appears in is automatically formatted using the &#91;ContentBox&#93; code.  Check the HTML for the main page to see all of the custom codes that have been used to generate this page." & CHR(10) & "<br /><br />" & CHR(10) & "This is only a small sample of the widgets and options you can enable with <a href=""javascript:openHelp(''DMGAdminCode.html'')"">DMG Admin Codes</a>." & CHR(10) & "[/ContentBox]', 0, 1, 1, 1, 0, 0, 0, '', 'Sub-Categories', 0, 1, 1, 1, '', '')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (1, 'Main Page', 1, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (3, 'Register', 4, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (6, 'Search', 7, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (7, 'User CP', 10, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (9, 'Administration', 13, '0', 0)")
					Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_SEARCH_TOPICS = 0")
					Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_SEARCH_MEMBERS = 0")
					Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_SEARCH_BLOGS = 0")
				end if

				Database.Write("INSERT INTO " & Database.DBPrefix & "_CUSTOM_MESSAGES (EMAIL_ADMINAPPROVAL, EMAIL_SENDKEY, EMAIL_CONFIRMPOST, EMAIL_SUBSCRIPTION, MESSAGE_ADMINAPPROVAL, MESSAGE_SENDKEY, MESSAGE_REGISTRATION, MESSAGE_VALIDATION, MESSAGE_CONFIRMPOST, EMAIL_WELCOMEMESSAGE, MESSAGE_PRIVACYNOTICE) VALUES ('Your account has been activated.  You can now return to the site and log in with the username/password you provided during registration.', 'You are now one step away from completing your registration.  Listed below is a unique verification key that must be entered on the web site before you can begin using the forums.  Copy the key and return to the web page.  Log in with the username and password that you provided and you will be prompted to enter the key.  After entering the key, you will be able to log in normally in the future.', 'A new post has been made and requires moderator approval.  Click the link below to view the thread.', 'A thread that you have subscribed to has been updated.  Click the link below to view the thread.', 'Thank you for registering.  Before you can begin using your account the administrator must validate your registration.  Once the admin has reviewed and accepted your registration, you will be able to log in and begin using your account.', 'Thank you for registering.  You have now been sent a confirmation e-mail message that provides a unique key for activating your account.  When the e-mail arrives, return to this site and log in with your username and password.  You will then be asked to provide the unique activation key.', 'Your registration is complete.  You may now begin using your account.<br /><br />To complete your profile or to add more information, click the User CP button.', 'Thank you for registering an account on this site.  Your registration is now complete and you are logged in.', 'This forum requires admin/moderator approval before posts appear.<br />Your post will appear once a moderator has reviewed/approved it.', 'Thank you for registering at our site.  You may now log in and begin using your account.<br /><br />To complete your profile or to add more information, log in and click the User CP button.', 'A unique username and e-mail address are required to access the forums.  All information shared in your member profile is available to the public and you should exercise caution before posting personal data.  The administrators and moderators of this web site are not responsible for the privacy of any user.<br /><br />Your browser must have cookies enabled in order to use the forums.  You will not be able to log in after registering without allowing the forum to store your user data in cookie files.<br /><br />We are not responsible for any materials posted on this site by its members.  We will, however, attempt to remove materials that we feel are inappropriate.  If you see posts or information in these forums with which you think is indecent, you are encouraged to contact us and let us know about it.<br /><br />By pressing the ""Agree"" button, you agree that you are 13 years of age or over.  You also agree that you will not post any copyrighted material that is not owned by yourself or the owners of these forums. In your use of these forums, you agree that you will not post any information which is vulgar, harassing, hateful, threatening, invading of others privacy, sexually oriented, or violates any laws.')")
				Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'avatars')")
				Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'customavatars')")
				Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'documents')")
				Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'forumimages')")
				Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'images')")
				Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'memberphotos')")
				Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'pageimages')")
				Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'photogalleries')")
				Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'rankimages')")
				Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'rotatorimages')")
				Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'topicfiles')")
				Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGAdminCode.html')")
				Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGCode.html')")
				Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGHtmlForms.html')")
				Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGMainMenu.html')")
				Dim Reader as OdbcDataReader = Database.Read("SELECT FOLDER_ID, FOLDER_NAME FROM " & Database.DBPrefix & "_FOLDERS ORDER BY FOLDER_NAME")
				While Reader.Read()
					if (Reader("FOLDER_NAME").ToString() = "avatars") then
						Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'default.gif')")
					end if
					if (Reader("FOLDER_NAME").ToString() = "forumimages") then
						Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'folder.gif')")
						Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'folder_home.gif')")
						Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'folder_up.gif')")
						Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_aol.gif')")
						Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_icq.gif')")
						Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_msn.gif')")
						Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_yahoo.gif')")
						Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'latestcomments.gif')")
						Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'lock.gif')")
						Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'page_icon.gif')")
						Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'pm_inbox.gif')")
						Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'pm_new.gif')")
						Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'rss.gif')")
						Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'icons')")
						Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'dmgstyles')")
						Dim IconReader as OdbcDataReader = Database.Read("SELECT FOLDER_ID FROM " & Database.DBPrefix & "_FOLDERS WHERE FOLDER_NAME = 'icons' ORDER BY FOLDER_ID DESC", 1)
						While IconReader.Read()
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'bz2.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'conf.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'css.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'csv.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'doc.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'file.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'gz.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'html.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'jpg.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'js.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'mov.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'mp3.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'mpg.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'pdf.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'php.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'ppt.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'rar.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'rtf.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'swf.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'tar.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'tgz.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'txt.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'wav.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'wma.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'wmv.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'xls.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'xml.png')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'zip.png')")
						End While
						IconReader.Close()
						Dim StylesReader as OdbcDataReader = Database.Read("SELECT FOLDER_ID FROM " & Database.DBPrefix & "_FOLDERS WHERE FOLDER_NAME = 'dmgstyles' ORDER BY FOLDER_ID DESC", 1)
						While StylesReader.Read()
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & StylesReader("FOLDER_ID") & ", 'bluegradients-headerbg.jpg')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & StylesReader("FOLDER_ID") & ", 'bluegradients-topbg.jpg')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & StylesReader("FOLDER_ID") & ", 'redgradients-headerbg.jpg')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & StylesReader("FOLDER_ID") & ", 'redgradients-topbg.jpg')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & StylesReader("FOLDER_ID") & ", 'greengradients-headerbg.jpg')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & StylesReader("FOLDER_ID") & ", 'greengradients-topbg.jpg')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & StylesReader("FOLDER_ID") & ", 'centerscrollblue-bg.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & StylesReader("FOLDER_ID") & ", 'centerscrollred-bg.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & StylesReader("FOLDER_ID") & ", 'centerscrollgreen-bg.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & StylesReader("FOLDER_ID") & ", 'centerscrollblack-bg.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & StylesReader("FOLDER_ID") & ", 'centerscroll-buttonbg.jpg')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & StylesReader("FOLDER_ID") & ", 'leftbuttons-bg.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & StylesReader("FOLDER_ID") & ", 'whitesmoke-headerbg.gif')")
						End While
						StylesReader.Close()
					end if
					if (Reader("FOLDER_NAME").ToString() = "rankimages") then
						Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_0.gif')")
						Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_1.gif')")
						Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_2.gif')")
						Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_3.gif')")
						Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_4.gif')")
						Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_5.gif')")
					end if
				End While
				Reader.Close()
			Catch e1 as System.Data.Odbc.OdbcException
				Message.text = "<b>ERROR:</b> There was a SQL error during setup.  View the error message below and then <a href=""setup.aspx"">Click Here</a> to try again.<br /><br />" & e1.ToString()
			Catch e2 as Exception
				Message.text = "<b>ERROR:</b> There was an error during setup.  View the error message below and then <a href=""setup.aspx"">Click Here</a> to try again.<br /><br />" & e2.ToString()
			End Try
		End Sub

	End Class


	'---------------------------------------------------------------------------------------------------
	' Update - Codebehind For setup-update.aspx
	'---------------------------------------------------------------------------------------------------
	Public Class Update

		Inherits System.Web.UI.Page

		Public SetupForm As System.Web.UI.WebControls.Panel
		Public MessagePanel As System.Web.UI.WebControls.Panel
		Public Message As System.Web.UI.WebControls.Label
		Public Submit As System.Web.UI.WebControls.Button

		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
			if Not Page.IsPostBack() then
				Dim DBColumn as String = ""
				Dim DBTable as String = ""
				if (Database.DBType = "MySQL") then
					DBColumn = "column_name"
					DBTable = "information_schema.columns WHERE table_schema = '" & Database.GetDBName & "' AND column_name ="
				else
					DBColumn = "NAME"
					DBTable = "syscolumns WHERE NAME ="
				end if

				Dim CurrentVersion as String = "-1"

				Try
					Dim DatabaseCheck as OdbcDataReader = Database.Read("SELECT " & DBColumn & " FROM " & DBTable & " '" & Database.DBPrefix & "_TITLE'")
						if DatabaseCheck.HasRows() then
							CurrentVersion = "1.1"
						end if
					DatabaseCheck.Close()

					DatabaseCheck = Database.Read("SELECT " & DBColumn & " FROM " & DBTable & " '" & Database.DBPrefix & "_SPAM_FILTER'")
						if DatabaseCheck.HasRows() then
							CurrentVersion = "1.2"
						end if
					DatabaseCheck.Close()

					DatabaseCheck = Database.Read("SELECT " & DBColumn & " FROM " & DBTable & " '" & Database.DBPrefix & "_TEMPLATE_NAME'")
						if DatabaseCheck.HasRows() then
							CurrentVersion = "1.3"
						end if
					DatabaseCheck.Close()

					DatabaseCheck = Database.Read("SELECT " & DBColumn & " FROM " & DBTable & " '" & Database.DBPrefix & "_MARGIN_SIDE'")
						if DatabaseCheck.HasRows() then
							CurrentVersion = "2.0"
						end if
					DatabaseCheck.Close()

					DatabaseCheck = Database.Read("SELECT " & DBColumn & " FROM " & DBTable & " '" & Database.DBPrefix & "_MEMBER_VALIDATION'")
						if DatabaseCheck.HasRows() then
							CurrentVersion = "2.2"
						end if
					DatabaseCheck.Close()

					DatabaseCheck = Database.Read("SELECT " & DBColumn & " FROM " & DBTable & " '" & Database.DBPrefix & "_MEMBER_PHOTOSIZE'")
						if DatabaseCheck.HasRows() then
							CurrentVersion = "3.0"
						end if
					DatabaseCheck.Close()

					DatabaseCheck = Database.Read("SELECT " & DBColumn & " FROM " & DBTable & " '" & Database.DBPrefix & "_AVATAR_SIZE'")
						if DatabaseCheck.HasRows() then
							CurrentVersion = "3.1"
						end if
					DatabaseCheck.Close()

					DatabaseCheck = Database.Read("SELECT " & DBColumn & " FROM " & DBTable & " '" & Database.DBPrefix & "_TOPICS_FONTCOLOR'")
						if DatabaseCheck.HasRows() then
							CurrentVersion = "3.2"
						end if
					DatabaseCheck.Close()

					if (CurrentVersion = "3.2") then


						SetupForm.visible = "false"
						MessagePanel.visible = "true"
						Message.text = "You have already installed version 3.2.  Please delete this file from your server for security."
					elseif (CurrentVersion = "-1") then
						SetupForm.visible = "false"
						MessagePanel.visible = "true"
						Message.text = "You have not run the full installation yet.  <a href=""setup.aspx"">Click Here</a> to install the latest version."
					else
						Submit.CommandArgument = CurrentVersion
						SetupForm.visible = "true"
					end if
				Catch e1 as System.Data.Odbc.OdbcException
					MessagePanel.visible = "true"
					Message.text = "<b>ERROR:</b> There was a SQL error during setup.  View the error message below and then <a href=""setup.aspx"">Click Here</a> to try again.<br /><br />" & e1.ToString()
				Catch e2 as Exception
					MessagePanel.visible = "true"
					Message.text = "<b>ERROR:</b> There was an error during setup.  View the error message below and then <a href=""setup.aspx"">Click Here</a> to try again.<br /><br />" & e2.ToString()
				End Try
			end if
		End Sub

		Sub UpdateForums(sender As System.Object, e As System.EventArgs)
			SetupForm.visible = "false"
			MessagePanel.visible = "true"

			Dim Reader as OdbcDataReader

			Try
				Message.text = "You have successfully upgraded your database to version 3.2.  <a href=""../default.aspx"">Click Here</a> to view the forums and log in to make changes to the layout and content.<br /><br /><b>Be sure to delete the install files from your server for security purposes!<br /><br />Also remember to change to a database security account that does not have full admin privileges.</b>"
				if (sender.CommandArgument = "1.1") then
					'------------  1.2 Additions
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SPAM_FILTER int")
					Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_SPAM_FILTER = 30 WHERE ID = 1")
					'------------  1.3 Additions
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_TEMPLATE_DEFAULT int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_TEMPLATE_NAME varchar(30)")
					Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_TEMPLATE_NAME = 'Default Template', " & Database.DBPrefix & "_TEMPLATE_DEFAULT = 1 WHERE ID = 1")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_PM(PM_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", PM_FROM int, PM_TO int, PM_SUBJECT varchar(100), PM_MESSAGE text, PM_READ int, PM_OUTBOX int, PM_DATE datetime)")
					'------------  2.0 Additions
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_VARIABLES(ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", VAR1 varchar(50), VAR2 varchar(50), VAR3 varchar(50), TEXT1 text, TEXT2 text)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_PAGES(PAGE_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", PAGE_PARENT int, PAGE_NAME varchar(30), PAGE_TITLE varchar(100), PAGE_CONTENT text, PAGE_SHOWTITLE int, PAGE_SHOWHEADERS int, PAGE_SHOWLOGIN int, PAGE_STATUS int, PAGE_SORT int)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_VARIABLES (VAR1, VAR2, VAR3, TEXT1, TEXT2) VALUES ('Variable 1 Text', 'Variable 2 Text', 'Variable 3 Text', 'Memo Field 1', 'Memo Field 2')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_PAGES (PAGE_PARENT, PAGE_NAME, PAGE_TITLE, PAGE_CONTENT, PAGE_SHOWTITLE, PAGE_SHOWHEADERS, PAGE_SHOWLOGIN, PAGE_STATUS, PAGE_SORT) VALUES (0, 'Main Page', 'Welcome To The Site!', '', 1, 1, 1, 1, 0)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_TOPICS ADD TOPIC_NEWS int")

					Database.Write("ALTER TABLE " & Database.DBPrefix & "_TOPICS ADD TOPIC_CONFIRMED int")
					Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_NEWS = 0, TOPIC_CONFIRMED = 1")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_FORUMS_DEFAULT int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_LATESTTOPICS_NUMBER int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_MARGIN_SIDE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_MARGIN_TOP int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SHOWSTATISTICS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_CUSTOM_META text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_NEWSDAYS int")
					Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_FORUMS_DEFAULT = 1, " & Database.DBPrefix & "_LATESTTOPICS_NUMBER = 8, " & Database.DBPrefix & "_MARGIN_SIDE = 10, " & Database.DBPrefix & "_MARGIN_TOP = 15, " & Database.DBPrefix & "_SHOWSTATISTICS = 1, " & Database.DBPrefix & "_CUSTOM_META = 'forums, portals', " & Database.DBPrefix & "_NEWSDAYS = 0")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_FORUMS ADD FORUM_FORCECONFIRM int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_FORUMS ADD FORUM_SHOWLATEST int")
					Database.Write("UPDATE " & Database.DBPrefix & "_FORUMS SET FORUM_FORCECONFIRM = 0, FORUM_SHOWLATEST = 1")
					'------------  2.2 Additions
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_MEMBER_VALIDATION int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_SMTP varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_ADDRESS varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_ALLOWSEND int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_ALLOWSUB int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_BGIMAGE varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_ALLOWSUB int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_LATESTBLOGS_NUMBER int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_QUICK_REGISTRATION int")
					Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_MEMBER_VALIDATION = 0, " & Database.DBPrefix & "_EMAIL_SMTP = 'mail.yourserver.com', " & Database.DBPrefix & "_EMAIL_ADDRESS = 'mail@yourserver.com', " & Database.DBPrefix & "_EMAIL_ALLOWSEND = 0, " & Database.DBPrefix & "_EMAIL_ALLOWSUB = 0, " & Database.DBPrefix & "_BGIMAGE = '', " & Database.DBPrefix & "_ALLOWSUB = 1, " & Database.DBPrefix & "_LATESTBLOGS_NUMBER = 8, " & Database.DBPrefix & "_QUICK_REGISTRATION = 0")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_MEMBERS ADD MEMBER_VALIDATED int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_MEMBERS ADD MEMBER_VALIDATION_STRING varchar(100)")
					Database.Write("UPDATE " & Database.DBPrefix & "_MEMBERS SET MEMBER_VALIDATED = 1, MEMBER_VALIDATION_STRING = '', MEMBER_AVATAR_USECUSTOM = 0, MEMBER_AVATAR_CUSTOMLOADED = 0")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_SUBSCRIPTIONS(SUB_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", SUB_MEMBER int, SUB_TOPIC int, SUB_EMAIL int)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_BLOG_TOPICS(BLOG_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", BLOG_AUTHOR int, BLOG_DATE datetime, BLOG_REPLIES int, BLOG_TITLE varchar(100), BLOG_TEXT text)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_BLOG_REPLIES(BLOG_REPLY_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", BLOG_ID int, BLOG_REPLY_AUTHOR int, BLOG_REPLY_DATE datetime, BLOG_REPLY_TEXT text)")
					'------------  3.0 Additions
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_PORT varchar(15)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_USERNAME varchar(30)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_PASSWORD varchar(30)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_CURSE_FILTER int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_RSS_FEEDS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_HORIZ_DIVIDE varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_VERT_DIVIDE varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_ALLOW_EDITS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_ALLOW_REGISTRATION int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_ALLOW_MEDIA int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_ALLOW_REPORTING int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_HIDE_MEMBERS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_HIDE_LOGIN int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_MEMBER_PHOTOSIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_CUSTOM_CSS text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_HTML_TITLE varchar(100)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_THUMBNAIL_SIZE int")
					Dim HTMLTitle as String = ""
					Reader = Database.Read("SELECT " & Database.DBPrefix & "_TITLE FROM " & Database.DBPrefix & "_SETTINGS WHERE ID = 1")
					While Reader.Read()
						HTMLTitle = Functions.RepairString(Reader(Database.DBPrefix & "_TITLE").ToString())
					End While
					Reader.Close()
					Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_EMAIL_PORT = '', " & Database.DBPrefix & "_EMAIL_USERNAME = '', " & Database.DBPrefix & "_EMAIL_PASSWORD = '', " & Database.DBPrefix & "_CURSE_FILTER = 0, " & Database.DBPrefix & "_RSS_FEEDS = 0, " & Database.DBPrefix & "_HORIZ_DIVIDE = '&nbsp;&nbsp;<b>|</b>&nbsp;&nbsp;', " & Database.DBPrefix & "_VERT_DIVIDE = '<br /><br />', " & Database.DBPrefix & "_ALLOW_EDITS = 1, " & Database.DBPrefix & "_ALLOW_REGISTRATION = 1, " & Database.DBPrefix & "_ALLOW_MEDIA = 1, " & Database.DBPrefix & "_ALLOW_REPORTING = 1, " & Database.DBPrefix & "_HIDE_MEMBERS = 0, " & Database.DBPrefix & "_HIDE_LOGIN = 0, " & Database.DBPrefix & "_MEMBER_PHOTOSIZE = 0, " & Database.DBPrefix & "_CUSTOM_CSS = '<STYLE TYPE=""text/css"">" & CHR(10) & "<!--" & CHR(10) & "A:link {text-decoration: [LinkDecoration]; color: [LinkColor];}" & CHR(10) & "A:visited {text-decoration: [VLinkDecoration]; color: [VLinkColor];}" & CHR(10) & "A:active {text-decoration: [ALinkDecoration]; color: [ALinkColor];}" & CHR(10) & "A:hover {text-decoration: [HLinkDecoration]; color: [HLinkColor];}" & CHR(10) & ".dmgbuttons{" & CHR(10) & "	font-family: Verdana;" & CHR(10) & "	font-size: 8px;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "	border-color: [ButtonColor];}" & CHR(10) & ".LoginButton{font-size: 12px; border-color: [ButtonColor];}" & CHR(10) & ".AdminButtons{font-size: 18px; width: 270px; border-color: [HeaderColor];}" & CHR(10) & ".LoginBox{border-color: [ButtonColor];}" & CHR(10) & ".ContentBox{border:2px solid [BorderColor];}" & CHR(10) & ".HeaderCell{background-color: [HeaderColor];}" & CHR(10) & ".SubHeaderCell{background-color: [SubHeaderColor];}" & CHR(10) & ".FooterCell{background-color: [FooterColor];}" & CHR(10) & ".TableRow1{background-color: [TableBGColor1];}" & CHR(10) & ".TableRow2{background-color: [TableBGColor2];}" & CHR(10) & ".MessageBox{font-size: 20px;}" & CHR(10) & ".PhotoGalleryTable {border: 0px;}" & CHR(10) & ".PhotoGalleryTable TD{font-size: 13px; padding: 10px; text-align: center;}" & CHR(10) & "-->" & CHR(10) & "</STYLE>', " & Database.DBPrefix & "_HTML_TITLE = '" & HTMLTitle & "', " & Database.DBPrefix & "_THUMBNAIL_SIZE = 150")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_MEMBERS ADD MEMBER_RANKING int")
					Database.Write("UPDATE " & Database.DBPrefix & "_MEMBERS SET MEMBER_RANKING = 0")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_AUTOFORMAT int")
					Database.Write("UPDATE " & Database.DBPrefix & "_PAGES SET PAGE_AUTOFORMAT = 0")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_FORUMS ADD FORUM_CONTENT text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_FORUMS ADD FORUM_SHOWHEADERS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_FORUMS ADD FORUM_SHOWLOGIN int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_FORUMS ADD FORUM_EMAIL_MODERATORS int")
					Database.Write("UPDATE " & Database.DBPrefix & "_FORUMS SET FORUM_CONTENT = '', FORUM_SHOWHEADERS = 1, FORUM_SHOWLOGIN = 1, FORUM_EMAIL_MODERATORS = 0")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CATEGORIES ADD CATEGORY_CONTENT text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CATEGORIES ADD CATEGORY_SHOWHEADERS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CATEGORIES ADD CATEGORY_SHOWLOGIN int")
					Database.Write("UPDATE " & Database.DBPrefix & "_CATEGORIES SET CATEGORY_CONTENT = '', CATEGORY_SHOWHEADERS = 1, CATEGORY_SHOWLOGIN = 1")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_VARIABLES ADD VAR4 varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_VARIABLES ADD VAR5 varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_VARIABLES ADD TEXT3 text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_VARIABLES ADD TEXT4 text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_VARIABLES ADD TEXT5 text")
					Database.Write("UPDATE " & Database.DBPrefix & "_VARIABLES SET VAR4 = 'Variable 4 Text', VAR5 = 'Variable 5 Text', TEXT3 = 'Memo Field 3', TEXT4 = 'Memo Field 4', TEXT5 = 'Memo Field 5'")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_REPLIES ADD REPLY_CONFIRMED int")
					Database.Write("UPDATE " & Database.DBPrefix & "_REPLIES SET REPLY_CONFIRMED = 1")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_TOPICS ADD TOPIC_UNCONFIRMED_REPLIES int")
					Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_UNCONFIRMED_REPLIES = 0")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_CURSE_FILTER(CURSE_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", CURSE_WORD varchar(20), CURSE_REPLACEMENT varchar(20))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_MAIN_MENU(LINK_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", LINK_ORDER int, LINK_TEXT varchar(50), LINK_TYPE int, LINK_PARAMETER varchar(100), LINK_WINDOW int)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_ROTATOR(ROTATOR_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", ROTATOR_NAME varchar(30))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_ROTATOR_IMAGES(IMAGE_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", ROTATOR_ID int, IMAGE_EXTENSION varchar(5), IMAGE_URL varchar(100), IMAGE_DESCRIPTION varchar(100), IMAGE_WINDOW int, IMAGE_BORDER int)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_GALLERY(GALLERY_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", GALLERY_NAME varchar(30))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_GALLERY_PHOTOS(PHOTO_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", GALLERY_ID int, PHOTO_EXTENSION varchar(5), PHOTO_DESCRIPTION varchar(100))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_HTML_FORMS(FORM_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", FORM_NAME varchar(50), FORM_DATE datetime, FORM_TEXT text, FORM_NEW int, FORM_EMAIL int)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_MEMBER_PHOTOS(PHOTO_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", MEMBER_ID int, PHOTO_EXTENSION varchar(5), PHOTO_SIZE int, PHOTO_DESCRIPTION varchar(100))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_BANNED_IP(ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", MEMBER_ID int, IP_ADDRESS varchar(50))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_CUSTOM_MESSAGES(ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", EMAIL_ADMINAPPROVAL text, EMAIL_SENDKEY text, EMAIL_CONFIRMPOST text, EMAIL_SUBSCRIPTION text, MESSAGE_ADMINAPPROVAL text, MESSAGE_SENDKEY text, MESSAGE_REGISTRATION text, MESSAGE_VALIDATION text, MESSAGE_CONFIRMPOST text)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_PM_TOPICS(TOPIC_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", TOPIC_FROM int, TOPIC_TO int, TOPIC_SUBJECT varchar(100), TOPIC_MESSAGE text, TOPIC_DATE datetime, TOPIC_TO_READ int, TOPIC_FROM_READ int, TOPIC_LASTPOST_AUTHOR int, TOPIC_LASTPOST_DATE datetime, TOPIC_REPLIES int, TOPIC_SHOWSENDER int)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_PM_REPLIES(REPLY_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", TOPIC_ID int, REPLY_AUTHOR int, REPLY_MESSAGE text, REPLY_DATE datetime)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (1, 'Main Page', 1, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (2, 'Forums', 3, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (3, 'Register', 4, '0', 0)")

					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (4, 'Active Topics', 5, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (5, 'Members', 6, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (6, 'Search', 7, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (7, 'User CP', 10, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (8, 'Private Messages', 11, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (9, 'Administration', 13, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_CUSTOM_MESSAGES (EMAIL_ADMINAPPROVAL, EMAIL_SENDKEY, EMAIL_CONFIRMPOST, EMAIL_SUBSCRIPTION, MESSAGE_ADMINAPPROVAL, MESSAGE_SENDKEY, MESSAGE_REGISTRATION, MESSAGE_VALIDATION, MESSAGE_CONFIRMPOST) VALUES ('Your account has been activated.  You can now return to the site and log in with the username/password you provided during registration.', 'You are now one step away from completing your registration.  Listed below is a unique verification key that must be entered on the web site before you can begin using the forums.  Copy the key and return to the web page.  Log in with the username and password that you provided and you will be prompted to enter the key.  After entering the key, you will be able to log in normally in the future.', 'A new post has been made and requires moderator approval.  Click the link below to view the thread.', 'A thread that you have subscribed to has been updated.  Click the link below to view the thread.', 'Thank you for registering.  Before you can begin using your account the administrator must validate your registration.  Once the admin has reviewed and accepted your registration, you will be able to log in and begin using your account.', 'Thank you for registering.  You have now been sent a confirmation e-mail message that provides a unique key for activating your account.  When the e-mail arrives, return to this site and log in with your username and password.  You will then be asked to provide the unique activation key.', 'Your registration is complete.  You may now log into the forums and begin using your account.<br /><br />To complete your profile or to add more information, log in and click the User CP button.', 'Thank you for registering an account on this site.  Your registration is now complete and you are logged in.', 'This forum requires admin/moderator approval before posts appear.<br />Your post will appear once a moderator has reviewed/approved it.')")
					Reader = Database.Read("SELECT * FROM " & Database.DBPrefix & "_PM")
					While Reader.Read()
						Database.Write("INSERT INTO " & Database.DBPrefix & "_PM_TOPICS (TOPIC_FROM, TOPIC_TO, TOPIC_SUBJECT, TOPIC_MESSAGE, TOPIC_DATE, TOPIC_TO_READ, TOPIC_FROM_READ, TOPIC_LASTPOST_AUTHOR, TOPIC_LASTPOST_DATE, TOPIC_REPLIES, TOPIC_SHOWSENDER) VALUES (" & Reader("PM_FROM") & ", " & Reader("PM_TO") & ", '" & Functions.RepairString(Reader("PM_SUBJECT").ToString()) & "', '" & Functions.RepairString(Reader("PM_MESSAGE").ToString()) & "', '" & Reader("PM_DATE").ToString() & "', " & Reader("PM_READ") & ", 1, " & Reader("PM_FROM") & ", '" & Reader("PM_DATE").ToString() & "', 0, 0)")
					End While
					Reader.Close()
					'------------  3.1 Additions
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PM_TOPICS ADD TOPIC_SHOWRECEIVER int")
					Database.Write("UPDATE " & Database.DBPrefix & "_PM_TOPICS SET TOPIC_SHOWRECEIVER = 1")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SECURITY int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_PASSWORD varchar(100)")
					Database.Write("UPDATE " & Database.DBPrefix & "_PAGES SET PAGE_SECURITY = 0, PAGE_PASSWORD = ''")
					Database.Write("UPDATE " & Database.DBPrefix & "_PAGES SET PAGE_PARENT = 0 WHERE PAGE_PARENT = -1")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_PAGES_PRIVILEGED(PRIVILEGED_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", MEMBER_ID int, PAGE_ID int, PRIVILEGED_ACCESS int)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_PHOTO_SIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_AVATAR_SIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SEARCH_TOPICS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SEARCH_MEMBERS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SEARCH_BLOGS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SEARCH_PAGES int")
					Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_PHOTO_SIZE = 775, " & Database.DBPrefix & "_AVATAR_SIZE = 125, " & Database.DBPrefix & "_SEARCH_TOPICS = 1, " & Database.DBPrefix & "_SEARCH_MEMBERS = 1, " & Database.DBPrefix & "_SEARCH_BLOGS = 1, " & Database.DBPrefix & "_SEARCH_PAGES = 1")
					'------------  3.2 Additions
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CUSTOM_MESSAGES ADD EMAIL_WELCOMEMESSAGE text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CUSTOM_MESSAGES ADD MESSAGE_PRIVACYNOTICE text")
					Database.Write("UPDATE " & Database.DBPrefix & "_CUSTOM_MESSAGES SET EMAIL_WELCOMEMESSAGE = 'Thank you for registering at our site.  You may now log in and begin using your account.<br /><br />To complete your profile or to add more information, log in and click the User CP button.', MESSAGE_PRIVACYNOTICE = 'A unique username and e-mail address are required to access the forums.  All information shared in your member profile is available to the public and you should exercise caution before posting personal data.  The administrators and moderators of this web site are not responsible for the privacy of any user.<br /><br />Your browser must have cookies enabled in order to use the forums.  You will not be able to log in after registering without allowing the forum to store your user data in cookie files.<br /><br />We are not responsible for any materials posted on this site by its members.  We will, however, attempt to remove materials that we feel are inappropriate.  If you see posts or information in these forums with which you think is indecent, you are encouraged to contact us and let us know about it.<br /><br />By pressing the ""Agree"" button, you agree that you are 13 years of age or over.  You also agree that you will not post any copyrighted material that is not owned by yourself or the owners of these forums. In your use of these forums, you agree that you will not post any information which is vulgar, harassing, hateful, threatening, invading of others privacy, sexually oriented, or violates any laws.'")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_MEMBER_FILETYPES text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_TOPIC_UPLOADSIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_TOPICS_FONTSIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_TOPICS_FONTCOLOR varchar(20)")
					Reader = Database.Read("SELECT " & Database.DBPrefix & "_FONTSIZE, " & Database.DBPrefix & "_FONT_COLOR FROM " & Database.DBPrefix & "_SETTINGS", 1)
					While Reader.Read()
						Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_MEMBER_FILETYPES = 'image/gif, image/png, application/msword, image/jpeg, application/zip, text/plain, image/pjpeg, application/mspowerpoint, application/vnd.ms-excel, application/pdf', " & Database.DBPrefix & "_TOPIC_UPLOADSIZE = 1024, " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE = 0, " & Database.DBPrefix & "_TOPICS_FONTSIZE = " & Reader(Database.DBPrefix & "_FONTSIZE") & ", " & Database.DBPrefix & "_TOPICS_FONTCOLOR = '" & Reader(Database.DBPrefix & "_FONT_COLOR").ToString() & "'")
					End While
					Reader.Close()
					Reader = Database.Read("SELECT ID, " & Database.DBPrefix & "_CUSTOM_CSS FROM " & Database.DBPrefix & "_SETTINGS")
					While Reader.Read()
						Dim TheCustomCSS as String = Reader(Database.DBPrefix & "_CUSTOM_CSS").ToString()
						TheCustomCSS = TheCustomCSS.Replace(".MessageBox{", ".AdminButtons{font-size: 18px; width: 270px; border-color: [HeaderColor];}" & CHR(10) & ".MessageBox{")
						Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_CUSTOM_CSS = '" & TheCustomCSS & "' WHERE ID = " & Reader("ID"))
					End While
					Reader.Close()
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_TOPICS ADD TOPIC_FILEUPLOAD varchar(100)")
					Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_FILEUPLOAD = ''")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_RANKINGS ADD RANK_ALLOW_UPLOADS int")
					Database.Write("UPDATE " & Database.DBPrefix & "_RANKINGS SET RANK_ALLOW_UPLOADS = 0")
					if (Database.DBType = "MySQL") then
						Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES MODIFY COLUMN PAGE_NAME varchar(100)")
					else
						Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ALTER COLUMN PAGE_NAME varchar(100)")
					end if
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_TITLE varchar(100)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_SHOWTITLE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_COLUMNS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_ALIGN int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_STATUS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_THUMBNAIL varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_PHOTO varchar(50)")
					Database.Write("UPDATE " & Database.DBPrefix & "_PAGES SET PAGE_SUB_TITLE = 'Sub-Categories', PAGE_SUB_SHOWTITLE = 0, PAGE_SUB_COLUMNS = 1, PAGE_SUB_ALIGN = 1, PAGE_SUB_STATUS = 1, PAGE_THUMBNAIL = '', PAGE_PHOTO = ''")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_FOLDERS(FOLDER_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", FOLDER_CORE int, FOLDER_PARENT int, FOLDER_NAME varchar(100))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_FILES(FILE_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", FILE_CORE int, FILE_FOLDER int, FILE_NAME varchar(100))")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'avatars')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'customavatars')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'documents')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'forumimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'images')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'memberphotos')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'pageimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'photogalleries')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'rankimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'rotatorimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'topicfiles')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGAdminCode.html')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGCode.html')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGHtmlForms.html')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGMainMenu.html')")
					Reader = Database.Read("SELECT FOLDER_ID, FOLDER_NAME FROM " & Database.DBPrefix & "_FOLDERS ORDER BY FOLDER_NAME")
					While Reader.Read()
						if (Reader("FOLDER_NAME").ToString() = "avatars") then
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'default.gif')")
							Dim AvatarReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'avatars/%'")
							While AvatarReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (AvatarReader("UPLOAD_NAME").ToString()).Replace("avatars/", "") & "')")
							End While
							AvatarReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "customavatars") then
							Dim CustomAvatarReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'customavatars/%'")
							While CustomAvatarReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (CustomAvatarReader("UPLOAD_NAME").ToString()).Replace("customavatars/", "") & "')")
							End While
							CustomAvatarReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "documents") then
							Dim DocumentReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'documents/%'")
							While DocumentReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (DocumentReader("UPLOAD_NAME").ToString()).Replace("documents/", "") & "')")
							End While
							DocumentReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "forumimages") then
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'folder.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'folder_home.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'folder_up.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_aol.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_icq.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_msn.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_yahoo.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'latestcomments.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'lock.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'page_icon.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'pm_inbox.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'pm_new.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'rss.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'icons')")
							Dim IconReader as OdbcDataReader = Database.Read("SELECT FOLDER_ID FROM " & Database.DBPrefix & "_FOLDERS WHERE FOLDER_NAME = 'icons' ORDER BY FOLDER_ID DESC", 1)
							While IconReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'bz2.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'conf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'css.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'csv.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'doc.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'file.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'gz.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'html.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'jpg.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'js.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'mov.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'mp3.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'mpg.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'pdf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'php.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'ppt.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'rar.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'rtf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'swf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'tar.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'tgz.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'txt.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'wav.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'wma.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'wmv.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'xls.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'xml.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'zip.png')")
							End While
							IconReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "images") then
							Dim ImagesReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'images/%'")
							While ImagesReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (ImagesReader("UPLOAD_NAME").ToString()).Replace("images/", "") & "')")
							End While
							ImagesReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "memberphotos") then
							Dim MemberPhotosReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'memberphotos/%'")
							While MemberPhotosReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (MemberPhotosReader("UPLOAD_NAME").ToString()).Replace("memberphotos/", "") & "')")
							End While
							MemberPhotosReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "pageimages") then
							Dim PageImagesReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'pageimages/%'")
							While PageImagesReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (PageImagesReader("UPLOAD_NAME").ToString()).Replace("pageimages/", "") & "')")
							End While
							PageImagesReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "photogalleries") then
							Dim GalleryReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'photogalleries/%'")
							While GalleryReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (GalleryReader("UPLOAD_NAME").ToString()).Replace("photogalleries/", "") & "')")
							End While
							GalleryReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "rankimages") then
							Dim RankReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'rankimages/%'")
							if (RankReader.HasRows()) then
								While RankReader.Read()
									Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (RankReader("UPLOAD_NAME").ToString()).Replace("rankimages/", "") & "')")
								End While
							else
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_0.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_1.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_2.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_3.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_4.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_5.gif')")
							end if
							RankReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "rotatorimages") then
							Dim RotatorReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'rotatorimages/%'")
							While RotatorReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (RotatorReader("UPLOAD_NAME").ToString()).Replace("rotatorimages/", "") & "')")
							End While
							RotatorReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "topicfiles") then
							Dim TopicFileReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'topicfiles/%'")
							While TopicFileReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (TopicFileReader("UPLOAD_NAME").ToString()).Replace("topicfiles/", "") & "')")
							End While
							TopicFileReader.Close()
						end if
					End While
					Reader.Close()
				elseif (sender.CommandArgument = "1.2") then
					'------------  1.3 Additions
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_TEMPLATE_DEFAULT int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_TEMPLATE_NAME varchar(30)")
					Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_TEMPLATE_NAME = 'Default Template', " & Database.DBPrefix & "_TEMPLATE_DEFAULT = 1 WHERE ID = 1")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_PM(PM_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", PM_FROM int, PM_TO int, PM_SUBJECT varchar(100), PM_MESSAGE text, PM_READ int, PM_OUTBOX int, PM_DATE datetime)")
					'------------  2.0 Additions
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_VARIABLES(ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", VAR1 varchar(50), VAR2 varchar(50), VAR3 varchar(50), TEXT1 text, TEXT2 text)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_PAGES(PAGE_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", PAGE_PARENT int, PAGE_NAME varchar(30), PAGE_TITLE varchar(100), PAGE_CONTENT text, PAGE_SHOWTITLE int, PAGE_SHOWHEADERS int, PAGE_SHOWLOGIN int, PAGE_STATUS int, PAGE_SORT int)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_VARIABLES (VAR1, VAR2, VAR3, TEXT1, TEXT2) VALUES ('Variable 1 Text', 'Variable 2 Text', 'Variable 3 Text', 'Memo Field 1', 'Memo Field 2')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_PAGES (PAGE_PARENT, PAGE_NAME, PAGE_TITLE, PAGE_CONTENT, PAGE_SHOWTITLE, PAGE_SHOWHEADERS, PAGE_SHOWLOGIN, PAGE_STATUS, PAGE_SORT) VALUES (0, 'Main Page', 'Welcome To The Site!', '', 1, 1, 1, 1, 0)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_TOPICS ADD TOPIC_NEWS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_TOPICS ADD TOPIC_CONFIRMED int")
					Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_NEWS = 0, TOPIC_CONFIRMED = 1")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_FORUMS_DEFAULT int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_LATESTTOPICS_NUMBER int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_MARGIN_SIDE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_MARGIN_TOP int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SHOWSTATISTICS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_CUSTOM_META text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_NEWSDAYS int")
					Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_FORUMS_DEFAULT = 1, " & Database.DBPrefix & "_LATESTTOPICS_NUMBER = 8, " & Database.DBPrefix & "_MARGIN_SIDE = 10, " & Database.DBPrefix & "_MARGIN_TOP = 15, " & Database.DBPrefix & "_SHOWSTATISTICS = 1, " & Database.DBPrefix & "_CUSTOM_META = 'forums, portals', " & Database.DBPrefix & "_NEWSDAYS = 0")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_FORUMS ADD FORUM_FORCECONFIRM int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_FORUMS ADD FORUM_SHOWLATEST int")
					Database.Write("UPDATE " & Database.DBPrefix & "_FORUMS SET FORUM_FORCECONFIRM = 0, FORUM_SHOWLATEST = 1")
					'------------  2.2 Additions
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_MEMBER_VALIDATION int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_SMTP varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_ADDRESS varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_ALLOWSEND int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_ALLOWSUB int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_BGIMAGE varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_ALLOWSUB int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_LATESTBLOGS_NUMBER int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_QUICK_REGISTRATION int")
					Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_MEMBER_VALIDATION = 0, " & Database.DBPrefix & "_EMAIL_SMTP = 'mail.yourserver.com', " & Database.DBPrefix & "_EMAIL_ADDRESS = 'mail@yourserver.com', " & Database.DBPrefix & "_EMAIL_ALLOWSEND = 0, " & Database.DBPrefix & "_EMAIL_ALLOWSUB = 0, " & Database.DBPrefix & "_BGIMAGE = '', " & Database.DBPrefix & "_ALLOWSUB = 1, " & Database.DBPrefix & "_LATESTBLOGS_NUMBER = 8, " & Database.DBPrefix & "_QUICK_REGISTRATION = 0")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_MEMBERS ADD MEMBER_VALIDATED int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_MEMBERS ADD MEMBER_VALIDATION_STRING varchar(100)")
					Database.Write("UPDATE " & Database.DBPrefix & "_MEMBERS SET MEMBER_VALIDATED = 1, MEMBER_VALIDATION_STRING = '', MEMBER_AVATAR_USECUSTOM = 0, MEMBER_AVATAR_CUSTOMLOADED = 0")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_SUBSCRIPTIONS(SUB_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", SUB_MEMBER int, SUB_TOPIC int, SUB_EMAIL int)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_BLOG_TOPICS(BLOG_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", BLOG_AUTHOR int, BLOG_DATE datetime, BLOG_REPLIES int, BLOG_TITLE varchar(100), BLOG_TEXT text)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_BLOG_REPLIES(BLOG_REPLY_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", BLOG_ID int, BLOG_REPLY_AUTHOR int, BLOG_REPLY_DATE datetime, BLOG_REPLY_TEXT text)")
					'------------  3.0 Additions
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_PORT varchar(15)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_USERNAME varchar(30)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_PASSWORD varchar(30)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_CURSE_FILTER int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_RSS_FEEDS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_HORIZ_DIVIDE varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_VERT_DIVIDE varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_ALLOW_EDITS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_ALLOW_REGISTRATION int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_ALLOW_MEDIA int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_ALLOW_REPORTING int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_HIDE_MEMBERS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_HIDE_LOGIN int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_MEMBER_PHOTOSIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_CUSTOM_CSS text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_HTML_TITLE varchar(100)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_THUMBNAIL_SIZE int")
					Dim HTMLTitle as String = ""
					Reader = Database.Read("SELECT " & Database.DBPrefix & "_TITLE FROM " & Database.DBPrefix & "_SETTINGS WHERE ID = 1")
					While Reader.Read()
						HTMLTitle = Functions.RepairString(Reader(Database.DBPrefix & "_TITLE").ToString())
					End While
					Reader.Close()
					Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_EMAIL_PORT = '', " & Database.DBPrefix & "_EMAIL_USERNAME = '', " & Database.DBPrefix & "_EMAIL_PASSWORD = '', " & Database.DBPrefix & "_CURSE_FILTER = 0, " & Database.DBPrefix & "_RSS_FEEDS = 0, " & Database.DBPrefix & "_HORIZ_DIVIDE = '&nbsp;&nbsp;<b>|</b>&nbsp;&nbsp;', " & Database.DBPrefix & "_VERT_DIVIDE = '<br /><br />', " & Database.DBPrefix & "_ALLOW_EDITS = 1, " & Database.DBPrefix & "_ALLOW_REGISTRATION = 1, " & Database.DBPrefix & "_ALLOW_MEDIA = 1, " & Database.DBPrefix & "_ALLOW_REPORTING = 1, " & Database.DBPrefix & "_HIDE_MEMBERS = 0, " & Database.DBPrefix & "_HIDE_LOGIN = 0, " & Database.DBPrefix & "_MEMBER_PHOTOSIZE = 0, " & Database.DBPrefix & "_CUSTOM_CSS = '<STYLE TYPE=""text/css"">" & CHR(10) & "<!--" & CHR(10) & "A:link {text-decoration: [LinkDecoration]; color: [LinkColor];}" & CHR(10) & "A:visited {text-decoration: [VLinkDecoration]; color: [VLinkColor];}" & CHR(10) & "A:active {text-decoration: [ALinkDecoration]; color: [ALinkColor];}" & CHR(10) & "A:hover {text-decoration: [HLinkDecoration]; color: [HLinkColor];}" & CHR(10) & ".dmgbuttons{" & CHR(10) & "	font-family: Verdana;" & CHR(10) & "	font-size: 8px;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "	border-color: [ButtonColor];}" & CHR(10) & ".LoginButton{font-size: 12px; border-color: [ButtonColor];}" & CHR(10) & ".AdminButtons{font-size: 18px; width: 270px; border-color: [HeaderColor];}" & CHR(10) & ".LoginBox{border-color: [ButtonColor];}" & CHR(10) & ".ContentBox{border:2px solid [BorderColor];}" & CHR(10) & ".HeaderCell{background-color: [HeaderColor];}" & CHR(10) & ".SubHeaderCell{background-color: [SubHeaderColor];}" & CHR(10) & ".FooterCell{background-color: [FooterColor];}" & CHR(10) & ".TableRow1{background-color: [TableBGColor1];}" & CHR(10) & ".TableRow2{background-color: [TableBGColor2];}" & CHR(10) & ".MessageBox{font-size: 20px;}" & CHR(10) & ".PhotoGalleryTable {border: 0px;}" & CHR(10) & ".PhotoGalleryTable TD{font-size: 13px; padding: 10px; text-align: center;}" & CHR(10) & "-->" & CHR(10) & "</STYLE>', " & Database.DBPrefix & "_HTML_TITLE = '" & HTMLTitle & "', " & Database.DBPrefix & "_THUMBNAIL_SIZE = 150")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_MEMBERS ADD MEMBER_RANKING int")
					Database.Write("UPDATE " & Database.DBPrefix & "_MEMBERS SET MEMBER_RANKING = 0")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_AUTOFORMAT int")
					Database.Write("UPDATE " & Database.DBPrefix & "_PAGES SET PAGE_AUTOFORMAT = 0")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_FORUMS ADD FORUM_CONTENT text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_FORUMS ADD FORUM_SHOWHEADERS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_FORUMS ADD FORUM_SHOWLOGIN int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_FORUMS ADD FORUM_EMAIL_MODERATORS int")
					Database.Write("UPDATE " & Database.DBPrefix & "_FORUMS SET FORUM_CONTENT = '', FORUM_SHOWHEADERS = 1, FORUM_SHOWLOGIN = 1, FORUM_EMAIL_MODERATORS = 0")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CATEGORIES ADD CATEGORY_CONTENT text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CATEGORIES ADD CATEGORY_SHOWHEADERS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CATEGORIES ADD CATEGORY_SHOWLOGIN int")
					Database.Write("UPDATE " & Database.DBPrefix & "_CATEGORIES SET CATEGORY_CONTENT = '', CATEGORY_SHOWHEADERS = 1, CATEGORY_SHOWLOGIN = 1")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_VARIABLES ADD VAR4 varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_VARIABLES ADD VAR5 varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_VARIABLES ADD TEXT3 text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_VARIABLES ADD TEXT4 text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_VARIABLES ADD TEXT5 text")
					Database.Write("UPDATE " & Database.DBPrefix & "_VARIABLES SET VAR4 = 'Variable 4 Text', VAR5 = 'Variable 5 Text', TEXT3 = 'Memo Field 3', TEXT4 = 'Memo Field 4', TEXT5 = 'Memo Field 5'")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_REPLIES ADD REPLY_CONFIRMED int")
					Database.Write("UPDATE " & Database.DBPrefix & "_REPLIES SET REPLY_CONFIRMED = 1")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_TOPICS ADD TOPIC_UNCONFIRMED_REPLIES int")
					Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_UNCONFIRMED_REPLIES = 0")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_CURSE_FILTER(CURSE_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", CURSE_WORD varchar(20), CURSE_REPLACEMENT varchar(20))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_MAIN_MENU(LINK_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", LINK_ORDER int, LINK_TEXT varchar(50), LINK_TYPE int, LINK_PARAMETER varchar(100), LINK_WINDOW int)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_ROTATOR(ROTATOR_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", ROTATOR_NAME varchar(30))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_ROTATOR_IMAGES(IMAGE_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", ROTATOR_ID int, IMAGE_EXTENSION varchar(5), IMAGE_URL varchar(100), IMAGE_DESCRIPTION varchar(100), IMAGE_WINDOW int, IMAGE_BORDER int)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_GALLERY(GALLERY_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", GALLERY_NAME varchar(30))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_GALLERY_PHOTOS(PHOTO_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", GALLERY_ID int, PHOTO_EXTENSION varchar(5), PHOTO_DESCRIPTION varchar(100))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_HTML_FORMS(FORM_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", FORM_NAME varchar(50), FORM_DATE datetime, FORM_TEXT text, FORM_NEW int, FORM_EMAIL int)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_MEMBER_PHOTOS(PHOTO_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", MEMBER_ID int, PHOTO_EXTENSION varchar(5), PHOTO_SIZE int, PHOTO_DESCRIPTION varchar(100))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_BANNED_IP(ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", MEMBER_ID int, IP_ADDRESS varchar(50))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_CUSTOM_MESSAGES(ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", EMAIL_ADMINAPPROVAL text, EMAIL_SENDKEY text, EMAIL_CONFIRMPOST text, EMAIL_SUBSCRIPTION text, MESSAGE_ADMINAPPROVAL text, MESSAGE_SENDKEY text, MESSAGE_REGISTRATION text, MESSAGE_VALIDATION text, MESSAGE_CONFIRMPOST text)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_PM_TOPICS(TOPIC_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", TOPIC_FROM int, TOPIC_TO int, TOPIC_SUBJECT varchar(100), TOPIC_MESSAGE text, TOPIC_DATE datetime, TOPIC_TO_READ int, TOPIC_FROM_READ int, TOPIC_LASTPOST_AUTHOR int, TOPIC_LASTPOST_DATE datetime, TOPIC_REPLIES int, TOPIC_SHOWSENDER int)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_PM_REPLIES(REPLY_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", TOPIC_ID int, REPLY_AUTHOR int, REPLY_MESSAGE text, REPLY_DATE datetime)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (1, 'Main Page', 1, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (2, 'Forums', 3, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (3, 'Register', 4, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (4, 'Active Topics', 5, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (5, 'Members', 6, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (6, 'Search', 7, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (7, 'User CP', 10, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (8, 'Private Messages', 11, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (9, 'Administration', 13, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_CUSTOM_MESSAGES (EMAIL_ADMINAPPROVAL, EMAIL_SENDKEY, EMAIL_CONFIRMPOST, EMAIL_SUBSCRIPTION, MESSAGE_ADMINAPPROVAL, MESSAGE_SENDKEY, MESSAGE_REGISTRATION, MESSAGE_VALIDATION, MESSAGE_CONFIRMPOST) VALUES ('Your account has been activated.  You can now return to the site and log in with the username/password you provided during registration.', 'You are now one step away from completing your registration.  Listed below is a unique verification key that must be entered on the web site before you can begin using the forums.  Copy the key and return to the web page.  Log in with the username and password that you provided and you will be prompted to enter the key.  After entering the key, you will be able to log in normally in the future.', 'A new post has been made and requires moderator approval.  Click the link below to view the thread.', 'A thread that you have subscribed to has been updated.  Click the link below to view the thread.', 'Thank you for registering.  Before you can begin using your account the administrator must validate your registration.  Once the admin has reviewed and accepted your registration, you will be able to log in and begin using your account.', 'Thank you for registering.  You have now been sent a confirmation e-mail message that provides a unique key for activating your account.  When the e-mail arrives, return to this site and log in with your username and password.  You will then be asked to provide the unique activation key.', 'Your registration is complete.  You may now log into the forums and begin using your account.<br /><br />To complete your profile or to add more information, log in and click the User CP button.', 'Thank you for registering an account on this site.  Your registration is now complete and you are logged in.', 'This forum requires admin/moderator approval before posts appear.<br />Your post will appear once a moderator has reviewed/approved it.')")
					Reader = Database.Read("SELECT * FROM " & Database.DBPrefix & "_PM")
					While Reader.Read()
						Database.Write("INSERT INTO " & Database.DBPrefix & "_PM_TOPICS (TOPIC_FROM, TOPIC_TO, TOPIC_SUBJECT, TOPIC_MESSAGE, TOPIC_DATE, TOPIC_TO_READ, TOPIC_FROM_READ, TOPIC_LASTPOST_AUTHOR, TOPIC_LASTPOST_DATE, TOPIC_REPLIES, TOPIC_SHOWSENDER) VALUES (" & Reader("PM_FROM") & ", " & Reader("PM_TO") & ", '" & Functions.RepairString(Reader("PM_SUBJECT").ToString()) & "', '" & Functions.RepairString(Reader("PM_MESSAGE").ToString()) & "', '" & Reader("PM_DATE").ToString() & "', " & Reader("PM_READ") & ", 1, " & Reader("PM_FROM") & ", '" & Reader("PM_DATE").ToString() & "', 0, 0)")
					End While
					Reader.Close()
					'------------  3.1 Additions
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PM_TOPICS ADD TOPIC_SHOWRECEIVER int")
					Database.Write("UPDATE " & Database.DBPrefix & "_PM_TOPICS SET TOPIC_SHOWRECEIVER = 1")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SECURITY int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_PASSWORD varchar(100)")
					Database.Write("UPDATE " & Database.DBPrefix & "_PAGES SET PAGE_SECURITY = 0, PAGE_PASSWORD = ''")
					Database.Write("UPDATE " & Database.DBPrefix & "_PAGES SET PAGE_PARENT = 0 WHERE PAGE_PARENT = -1")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_PAGES_PRIVILEGED(PRIVILEGED_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", MEMBER_ID int, PAGE_ID int, PRIVILEGED_ACCESS int)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_PHOTO_SIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_AVATAR_SIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SEARCH_TOPICS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SEARCH_MEMBERS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SEARCH_BLOGS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SEARCH_PAGES int")
					Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_PHOTO_SIZE = 775, " & Database.DBPrefix & "_AVATAR_SIZE = 125, " & Database.DBPrefix & "_SEARCH_TOPICS = 1, " & Database.DBPrefix & "_SEARCH_MEMBERS = 1, " & Database.DBPrefix & "_SEARCH_BLOGS = 1, " & Database.DBPrefix & "_SEARCH_PAGES = 1")
					'------------  3.2 Additions
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CUSTOM_MESSAGES ADD EMAIL_WELCOMEMESSAGE text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CUSTOM_MESSAGES ADD MESSAGE_PRIVACYNOTICE text")
					Database.Write("UPDATE " & Database.DBPrefix & "_CUSTOM_MESSAGES SET EMAIL_WELCOMEMESSAGE = 'Thank you for registering at our site.  You may now log in and begin using your account.<br /><br />To complete your profile or to add more information, log in and click the User CP button.', MESSAGE_PRIVACYNOTICE = 'A unique username and e-mail address are required to access the forums.  All information shared in your member profile is available to the public and you should exercise caution before posting personal data.  The administrators and moderators of this web site are not responsible for the privacy of any user.<br /><br />Your browser must have cookies enabled in order to use the forums.  You will not be able to log in after registering without allowing the forum to store your user data in cookie files.<br /><br />We are not responsible for any materials posted on this site by its members.  We will, however, attempt to remove materials that we feel are inappropriate.  If you see posts or information in these forums with which you think is indecent, you are encouraged to contact us and let us know about it.<br /><br />By pressing the ""Agree"" button, you agree that you are 13 years of age or over.  You also agree that you will not post any copyrighted material that is not owned by yourself or the owners of these forums. In your use of these forums, you agree that you will not post any information which is vulgar, harassing, hateful, threatening, invading of others privacy, sexually oriented, or violates any laws.'")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_MEMBER_FILETYPES text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_TOPIC_UPLOADSIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_TOPICS_FONTSIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_TOPICS_FONTCOLOR varchar(20)")
					Reader = Database.Read("SELECT " & Database.DBPrefix & "_FONTSIZE, " & Database.DBPrefix & "_FONT_COLOR FROM " & Database.DBPrefix & "_SETTINGS", 1)
					While Reader.Read()
						Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_MEMBER_FILETYPES = 'image/gif, image/png, application/msword, image/jpeg, application/zip, text/plain, image/pjpeg, application/mspowerpoint, application/vnd.ms-excel, application/pdf', " & Database.DBPrefix & "_TOPIC_UPLOADSIZE = 1024, " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE = 0, " & Database.DBPrefix & "_TOPICS_FONTSIZE = " & Reader(Database.DBPrefix & "_FONTSIZE") & ", " & Database.DBPrefix & "_TOPICS_FONTCOLOR = '" & Reader(Database.DBPrefix & "_FONT_COLOR").ToString() & "'")
					End While
					Reader.Close()
					Reader = Database.Read("SELECT ID, " & Database.DBPrefix & "_CUSTOM_CSS FROM " & Database.DBPrefix & "_SETTINGS")
					While Reader.Read()
						Dim TheCustomCSS as String = Reader(Database.DBPrefix & "_CUSTOM_CSS").ToString()
						TheCustomCSS = TheCustomCSS.Replace(".MessageBox{", ".AdminButtons{font-size: 18px; width: 270px; border-color: [HeaderColor];}" & CHR(10) & ".MessageBox{")
						Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_CUSTOM_CSS = '" & TheCustomCSS & "' WHERE ID = " & Reader("ID"))
					End While
					Reader.Close()
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_TOPICS ADD TOPIC_FILEUPLOAD varchar(100)")
					Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_FILEUPLOAD = ''")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_RANKINGS ADD RANK_ALLOW_UPLOADS int")
					Database.Write("UPDATE " & Database.DBPrefix & "_RANKINGS SET RANK_ALLOW_UPLOADS = 0")
					if (Database.DBType = "MySQL") then
						Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES MODIFY COLUMN PAGE_NAME varchar(100)")
					else
						Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ALTER COLUMN PAGE_NAME varchar(100)")
					end if
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_TITLE varchar(100)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_SHOWTITLE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_COLUMNS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_ALIGN int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_STATUS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_THUMBNAIL varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_PHOTO varchar(50)")
					Database.Write("UPDATE " & Database.DBPrefix & "_PAGES SET PAGE_SUB_TITLE = 'Sub-Categories', PAGE_SUB_SHOWTITLE = 0, PAGE_SUB_COLUMNS = 1, PAGE_SUB_ALIGN = 1, PAGE_SUB_STATUS = 1, PAGE_THUMBNAIL = '', PAGE_PHOTO = ''")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_FOLDERS(FOLDER_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", FOLDER_CORE int, FOLDER_PARENT int, FOLDER_NAME varchar(100))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_FILES(FILE_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", FILE_CORE int, FILE_FOLDER int, FILE_NAME varchar(100))")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'avatars')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'customavatars')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'documents')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'forumimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'images')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'memberphotos')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'pageimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'photogalleries')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'rankimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'rotatorimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'topicfiles')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGAdminCode.html')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGCode.html')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGHtmlForms.html')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGMainMenu.html')")
					Reader = Database.Read("SELECT FOLDER_ID, FOLDER_NAME FROM " & Database.DBPrefix & "_FOLDERS ORDER BY FOLDER_NAME")
					While Reader.Read()
						if (Reader("FOLDER_NAME").ToString() = "avatars") then
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'default.gif')")
							Dim AvatarReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'avatars/%'")
							While AvatarReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (AvatarReader("UPLOAD_NAME").ToString()).Replace("avatars/", "") & "')")
							End While
							AvatarReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "customavatars") then
							Dim CustomAvatarReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'customavatars/%'")
							While CustomAvatarReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (CustomAvatarReader("UPLOAD_NAME").ToString()).Replace("customavatars/", "") & "')")
							End While
							CustomAvatarReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "documents") then
							Dim DocumentReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'documents/%'")
							While DocumentReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (DocumentReader("UPLOAD_NAME").ToString()).Replace("documents/", "") & "')")
							End While
							DocumentReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "forumimages") then
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'folder.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'folder_home.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'folder_up.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_aol.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_icq.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_msn.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_yahoo.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'latestcomments.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'lock.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'page_icon.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'pm_inbox.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'pm_new.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'rss.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'icons')")
							Dim IconReader as OdbcDataReader = Database.Read("SELECT FOLDER_ID FROM " & Database.DBPrefix & "_FOLDERS WHERE FOLDER_NAME = 'icons' ORDER BY FOLDER_ID DESC", 1)
							While IconReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'bz2.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'conf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'css.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'csv.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'doc.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'file.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'gz.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'html.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'jpg.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'js.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'mov.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'mp3.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'mpg.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'pdf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'php.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'ppt.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'rar.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'rtf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'swf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'tar.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'tgz.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'txt.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'wav.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'wma.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'wmv.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'xls.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'xml.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'zip.png')")
							End While
							IconReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "images") then
							Dim ImagesReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'images/%'")
							While ImagesReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (ImagesReader("UPLOAD_NAME").ToString()).Replace("images/", "") & "')")
							End While

							ImagesReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "memberphotos") then
							Dim MemberPhotosReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'memberphotos/%'")
							While MemberPhotosReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (MemberPhotosReader("UPLOAD_NAME").ToString()).Replace("memberphotos/", "") & "')")
							End While
							MemberPhotosReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "pageimages") then
							Dim PageImagesReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'pageimages/%'")
							While PageImagesReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (PageImagesReader("UPLOAD_NAME").ToString()).Replace("pageimages/", "") & "')")
							End While
							PageImagesReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "photogalleries") then
							Dim GalleryReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'photogalleries/%'")
							While GalleryReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (GalleryReader("UPLOAD_NAME").ToString()).Replace("photogalleries/", "") & "')")
							End While
							GalleryReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "rankimages") then
							Dim RankReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'rankimages/%'")
							if (RankReader.HasRows()) then
								While RankReader.Read()
									Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (RankReader("UPLOAD_NAME").ToString()).Replace("rankimages/", "") & "')")
								End While
							else
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_0.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_1.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_2.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_3.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_4.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_5.gif')")
							end if
							RankReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "rotatorimages") then
							Dim RotatorReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'rotatorimages/%'")
							While RotatorReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (RotatorReader("UPLOAD_NAME").ToString()).Replace("rotatorimages/", "") & "')")
							End While
							RotatorReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "topicfiles") then
							Dim TopicFileReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'topicfiles/%'")
							While TopicFileReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (TopicFileReader("UPLOAD_NAME").ToString()).Replace("topicfiles/", "") & "')")
							End While
							TopicFileReader.Close()
						end if
					End While
					Reader.Close()
				elseif (sender.CommandArgument = "1.3") then
					'------------  2.0 Additions
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_VARIABLES(ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", VAR1 varchar(50), VAR2 varchar(50), VAR3 varchar(50), TEXT1 text, TEXT2 text)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_PAGES(PAGE_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", PAGE_PARENT int, PAGE_NAME varchar(30), PAGE_TITLE varchar(100), PAGE_CONTENT text, PAGE_SHOWTITLE int, PAGE_SHOWHEADERS int, PAGE_SHOWLOGIN int, PAGE_STATUS int, PAGE_SORT int)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_VARIABLES (VAR1, VAR2, VAR3, TEXT1, TEXT2) VALUES ('Variable 1 Text', 'Variable 2 Text', 'Variable 3 Text', 'Memo Field 1', 'Memo Field 2')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_PAGES (PAGE_PARENT, PAGE_NAME, PAGE_TITLE, PAGE_CONTENT, PAGE_SHOWTITLE, PAGE_SHOWHEADERS, PAGE_SHOWLOGIN, PAGE_STATUS, PAGE_SORT) VALUES (0, 'Main Page', 'Welcome To The Site!', '', 1, 1, 1, 1, 0)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_TOPICS ADD TOPIC_NEWS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_TOPICS ADD TOPIC_CONFIRMED int")
					Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_NEWS = 0, TOPIC_CONFIRMED = 1")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_FORUMS_DEFAULT int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_LATESTTOPICS_NUMBER int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_MARGIN_SIDE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_MARGIN_TOP int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SHOWSTATISTICS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_CUSTOM_META text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_NEWSDAYS int")
					Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_FORUMS_DEFAULT = 1, " & Database.DBPrefix & "_LATESTTOPICS_NUMBER = 8, " & Database.DBPrefix & "_MARGIN_SIDE = 10, " & Database.DBPrefix & "_MARGIN_TOP = 15, " & Database.DBPrefix & "_SHOWSTATISTICS = 1, " & Database.DBPrefix & "_CUSTOM_META = 'forums, portals', " & Database.DBPrefix & "_NEWSDAYS = 0")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_FORUMS ADD FORUM_FORCECONFIRM int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_FORUMS ADD FORUM_SHOWLATEST int")
					Database.Write("UPDATE " & Database.DBPrefix & "_FORUMS SET FORUM_FORCECONFIRM = 0, FORUM_SHOWLATEST = 1")
					'------------  2.2 Additions
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_MEMBER_VALIDATION int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_SMTP varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_ADDRESS varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_ALLOWSEND int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_ALLOWSUB int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_BGIMAGE varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_ALLOWSUB int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_LATESTBLOGS_NUMBER int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_QUICK_REGISTRATION int")
					Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_MEMBER_VALIDATION = 0, " & Database.DBPrefix & "_EMAIL_SMTP = 'mail.yourserver.com', " & Database.DBPrefix & "_EMAIL_ADDRESS = 'mail@yourserver.com', " & Database.DBPrefix & "_EMAIL_ALLOWSEND = 0, " & Database.DBPrefix & "_EMAIL_ALLOWSUB = 0, " & Database.DBPrefix & "_BGIMAGE = '', " & Database.DBPrefix & "_ALLOWSUB = 1, " & Database.DBPrefix & "_LATESTBLOGS_NUMBER = 8, " & Database.DBPrefix & "_QUICK_REGISTRATION = 0")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_MEMBERS ADD MEMBER_VALIDATED int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_MEMBERS ADD MEMBER_VALIDATION_STRING varchar(100)")
					Database.Write("UPDATE " & Database.DBPrefix & "_MEMBERS SET MEMBER_VALIDATED = 1, MEMBER_VALIDATION_STRING = '', MEMBER_AVATAR_USECUSTOM = 0, MEMBER_AVATAR_CUSTOMLOADED = 0")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_SUBSCRIPTIONS(SUB_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", SUB_MEMBER int, SUB_TOPIC int, SUB_EMAIL int)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_BLOG_TOPICS(BLOG_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", BLOG_AUTHOR int, BLOG_DATE datetime, BLOG_REPLIES int, BLOG_TITLE varchar(100), BLOG_TEXT text)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_BLOG_REPLIES(BLOG_REPLY_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", BLOG_ID int, BLOG_REPLY_AUTHOR int, BLOG_REPLY_DATE datetime, BLOG_REPLY_TEXT text)")
					'------------  3.0 Additions
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_PORT varchar(15)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_USERNAME varchar(30)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_PASSWORD varchar(30)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_CURSE_FILTER int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_RSS_FEEDS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_HORIZ_DIVIDE varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_VERT_DIVIDE varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_ALLOW_EDITS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_ALLOW_REGISTRATION int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_ALLOW_MEDIA int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_ALLOW_REPORTING int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_HIDE_MEMBERS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_HIDE_LOGIN int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_MEMBER_PHOTOSIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_CUSTOM_CSS text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_HTML_TITLE varchar(100)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_THUMBNAIL_SIZE int")
					Dim HTMLTitle as String = ""
					Reader = Database.Read("SELECT " & Database.DBPrefix & "_TITLE FROM " & Database.DBPrefix & "_SETTINGS WHERE ID = 1")
					While Reader.Read()
						HTMLTitle = Functions.RepairString(Reader(Database.DBPrefix & "_TITLE").ToString())
					End While
					Reader.Close()
					Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_EMAIL_PORT = '', " & Database.DBPrefix & "_EMAIL_USERNAME = '', " & Database.DBPrefix & "_EMAIL_PASSWORD = '', " & Database.DBPrefix & "_CURSE_FILTER = 0, " & Database.DBPrefix & "_RSS_FEEDS = 0, " & Database.DBPrefix & "_HORIZ_DIVIDE = '&nbsp;&nbsp;<b>|</b>&nbsp;&nbsp;', " & Database.DBPrefix & "_VERT_DIVIDE = '<br /><br />', " & Database.DBPrefix & "_ALLOW_EDITS = 1, " & Database.DBPrefix & "_ALLOW_REGISTRATION = 1, " & Database.DBPrefix & "_ALLOW_MEDIA = 1, " & Database.DBPrefix & "_ALLOW_REPORTING = 1, " & Database.DBPrefix & "_HIDE_MEMBERS = 0, " & Database.DBPrefix & "_HIDE_LOGIN = 0, " & Database.DBPrefix & "_MEMBER_PHOTOSIZE = 0, " & Database.DBPrefix & "_CUSTOM_CSS = '<STYLE TYPE=""text/css"">" & CHR(10) & "<!--" & CHR(10) & "A:link {text-decoration: [LinkDecoration]; color: [LinkColor];}" & CHR(10) & "A:visited {text-decoration: [VLinkDecoration]; color: [VLinkColor];}" & CHR(10) & "A:active {text-decoration: [ALinkDecoration]; color: [ALinkColor];}" & CHR(10) & "A:hover {text-decoration: [HLinkDecoration]; color: [HLinkColor];}" & CHR(10) & ".dmgbuttons{" & CHR(10) & "	font-family: Verdana;" & CHR(10) & "	font-size: 8px;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "	border-color: [ButtonColor];}" & CHR(10) & ".LoginButton{font-size: 12px; border-color: [ButtonColor];}" & CHR(10) & ".AdminButtons{font-size: 18px; width: 270px; border-color: [HeaderColor];}" & CHR(10) & ".LoginBox{border-color: [ButtonColor];}" & CHR(10) & ".ContentBox{border:2px solid [BorderColor];}" & CHR(10) & ".HeaderCell{background-color: [HeaderColor];}" & CHR(10) & ".SubHeaderCell{background-color: [SubHeaderColor];}" & CHR(10) & ".FooterCell{background-color: [FooterColor];}" & CHR(10) & ".TableRow1{background-color: [TableBGColor1];}" & CHR(10) & ".TableRow2{background-color: [TableBGColor2];}" & CHR(10) & ".MessageBox{font-size: 20px;}" & CHR(10) & ".PhotoGalleryTable {border: 0px;}" & CHR(10) & ".PhotoGalleryTable TD{font-size: 13px; padding: 10px; text-align: center;}" & CHR(10) & "-->" & CHR(10) & "</STYLE>', " & Database.DBPrefix & "_HTML_TITLE = '" & HTMLTitle & "', " & Database.DBPrefix & "_THUMBNAIL_SIZE = 150")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_MEMBERS ADD MEMBER_RANKING int")
					Database.Write("UPDATE " & Database.DBPrefix & "_MEMBERS SET MEMBER_RANKING = 0")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_AUTOFORMAT int")
					Database.Write("UPDATE " & Database.DBPrefix & "_PAGES SET PAGE_AUTOFORMAT = 0")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_FORUMS ADD FORUM_CONTENT text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_FORUMS ADD FORUM_SHOWHEADERS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_FORUMS ADD FORUM_SHOWLOGIN int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_FORUMS ADD FORUM_EMAIL_MODERATORS int")
					Database.Write("UPDATE " & Database.DBPrefix & "_FORUMS SET FORUM_CONTENT = '', FORUM_SHOWHEADERS = 1, FORUM_SHOWLOGIN = 1, FORUM_EMAIL_MODERATORS = 0")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CATEGORIES ADD CATEGORY_CONTENT text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CATEGORIES ADD CATEGORY_SHOWHEADERS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CATEGORIES ADD CATEGORY_SHOWLOGIN int")
					Database.Write("UPDATE " & Database.DBPrefix & "_CATEGORIES SET CATEGORY_CONTENT = '', CATEGORY_SHOWHEADERS = 1, CATEGORY_SHOWLOGIN = 1")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_VARIABLES ADD VAR4 varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_VARIABLES ADD VAR5 varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_VARIABLES ADD TEXT3 text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_VARIABLES ADD TEXT4 text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_VARIABLES ADD TEXT5 text")
					Database.Write("UPDATE " & Database.DBPrefix & "_VARIABLES SET VAR4 = 'Variable 4 Text', VAR5 = 'Variable 5 Text', TEXT3 = 'Memo Field 3', TEXT4 = 'Memo Field 4', TEXT5 = 'Memo Field 5'")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_REPLIES ADD REPLY_CONFIRMED int")
					Database.Write("UPDATE " & Database.DBPrefix & "_REPLIES SET REPLY_CONFIRMED = 1")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_TOPICS ADD TOPIC_UNCONFIRMED_REPLIES int")
					Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_UNCONFIRMED_REPLIES = 0")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_CURSE_FILTER(CURSE_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", CURSE_WORD varchar(20), CURSE_REPLACEMENT varchar(20))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_MAIN_MENU(LINK_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", LINK_ORDER int, LINK_TEXT varchar(50), LINK_TYPE int, LINK_PARAMETER varchar(100), LINK_WINDOW int)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_ROTATOR(ROTATOR_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", ROTATOR_NAME varchar(30))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_ROTATOR_IMAGES(IMAGE_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", ROTATOR_ID int, IMAGE_EXTENSION varchar(5), IMAGE_URL varchar(100), IMAGE_DESCRIPTION varchar(100), IMAGE_WINDOW int, IMAGE_BORDER int)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_GALLERY(GALLERY_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", GALLERY_NAME varchar(30))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_GALLERY_PHOTOS(PHOTO_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", GALLERY_ID int, PHOTO_EXTENSION varchar(5), PHOTO_DESCRIPTION varchar(100))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_HTML_FORMS(FORM_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", FORM_NAME varchar(50), FORM_DATE datetime, FORM_TEXT text, FORM_NEW int, FORM_EMAIL int)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_MEMBER_PHOTOS(PHOTO_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", MEMBER_ID int, PHOTO_EXTENSION varchar(5), PHOTO_SIZE int, PHOTO_DESCRIPTION varchar(100))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_BANNED_IP(ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", MEMBER_ID int, IP_ADDRESS varchar(50))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_CUSTOM_MESSAGES(ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", EMAIL_ADMINAPPROVAL text, EMAIL_SENDKEY text, EMAIL_CONFIRMPOST text, EMAIL_SUBSCRIPTION text, MESSAGE_ADMINAPPROVAL text, MESSAGE_SENDKEY text, MESSAGE_REGISTRATION text, MESSAGE_VALIDATION text, MESSAGE_CONFIRMPOST text)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_PM_TOPICS(TOPIC_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", TOPIC_FROM int, TOPIC_TO int, TOPIC_SUBJECT varchar(100), TOPIC_MESSAGE text, TOPIC_DATE datetime, TOPIC_TO_READ int, TOPIC_FROM_READ int, TOPIC_LASTPOST_AUTHOR int, TOPIC_LASTPOST_DATE datetime, TOPIC_REPLIES int, TOPIC_SHOWSENDER int)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_PM_REPLIES(REPLY_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", TOPIC_ID int, REPLY_AUTHOR int, REPLY_MESSAGE text, REPLY_DATE datetime)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (1, 'Main Page', 1, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (2, 'Forums', 3, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (3, 'Register', 4, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (4, 'Active Topics', 5, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (5, 'Members', 6, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (6, 'Search', 7, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (7, 'User CP', 10, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (8, 'Private Messages', 11, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (9, 'Administration', 13, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_CUSTOM_MESSAGES (EMAIL_ADMINAPPROVAL, EMAIL_SENDKEY, EMAIL_CONFIRMPOST, EMAIL_SUBSCRIPTION, MESSAGE_ADMINAPPROVAL, MESSAGE_SENDKEY, MESSAGE_REGISTRATION, MESSAGE_VALIDATION, MESSAGE_CONFIRMPOST) VALUES ('Your account has been activated.  You can now return to the site and log in with the username/password you provided during registration.', 'You are now one step away from completing your registration.  Listed below is a unique verification key that must be entered on the web site before you can begin using the forums.  Copy the key and return to the web page.  Log in with the username and password that you provided and you will be prompted to enter the key.  After entering the key, you will be able to log in normally in the future.', 'A new post has been made and requires moderator approval.  Click the link below to view the thread.', 'A thread that you have subscribed to has been updated.  Click the link below to view the thread.', 'Thank you for registering.  Before you can begin using your account the administrator must validate your registration.  Once the admin has reviewed and accepted your registration, you will be able to log in and begin using your account.', 'Thank you for registering.  You have now been sent a confirmation e-mail message that provides a unique key for activating your account.  When the e-mail arrives, return to this site and log in with your username and password.  You will then be asked to provide the unique activation key.', 'Your registration is complete.  You may now log into the forums and begin using your account.<br /><br />To complete your profile or to add more information, log in and click the User CP button.', 'Thank you for registering an account on this site.  Your registration is now complete and you are logged in.', 'This forum requires admin/moderator approval before posts appear.<br />Your post will appear once a moderator has reviewed/approved it.')")
					Reader = Database.Read("SELECT * FROM " & Database.DBPrefix & "_PM")
					While Reader.Read()
						Database.Write("INSERT INTO " & Database.DBPrefix & "_PM_TOPICS (TOPIC_FROM, TOPIC_TO, TOPIC_SUBJECT, TOPIC_MESSAGE, TOPIC_DATE, TOPIC_TO_READ, TOPIC_FROM_READ, TOPIC_LASTPOST_AUTHOR, TOPIC_LASTPOST_DATE, TOPIC_REPLIES, TOPIC_SHOWSENDER) VALUES (" & Reader("PM_FROM") & ", " & Reader("PM_TO") & ", '" & Functions.RepairString(Reader("PM_SUBJECT").ToString()) & "', '" & Functions.RepairString(Reader("PM_MESSAGE").ToString()) & "', '" & Reader("PM_DATE").ToString() & "', " & Reader("PM_READ") & ", 1, " & Reader("PM_FROM") & ", '" & Reader("PM_DATE").ToString() & "', 0, 0)")
					End While
					Reader.Close()
					'------------  3.1 Additions
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PM_TOPICS ADD TOPIC_SHOWRECEIVER int")
					Database.Write("UPDATE " & Database.DBPrefix & "_PM_TOPICS SET TOPIC_SHOWRECEIVER = 1")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SECURITY int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_PASSWORD varchar(100)")
					Database.Write("UPDATE " & Database.DBPrefix & "_PAGES SET PAGE_SECURITY = 0, PAGE_PASSWORD = ''")
					Database.Write("UPDATE " & Database.DBPrefix & "_PAGES SET PAGE_PARENT = 0 WHERE PAGE_PARENT = -1")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_PAGES_PRIVILEGED(PRIVILEGED_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", MEMBER_ID int, PAGE_ID int, PRIVILEGED_ACCESS int)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_PHOTO_SIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_AVATAR_SIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SEARCH_TOPICS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SEARCH_MEMBERS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SEARCH_BLOGS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SEARCH_PAGES int")
					Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_PHOTO_SIZE = 775, " & Database.DBPrefix & "_AVATAR_SIZE = 125, " & Database.DBPrefix & "_SEARCH_TOPICS = 1, " & Database.DBPrefix & "_SEARCH_MEMBERS = 1, " & Database.DBPrefix & "_SEARCH_BLOGS = 1, " & Database.DBPrefix & "_SEARCH_PAGES = 1")
					'------------  3.2 Additions
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CUSTOM_MESSAGES ADD EMAIL_WELCOMEMESSAGE text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CUSTOM_MESSAGES ADD MESSAGE_PRIVACYNOTICE text")
					Database.Write("UPDATE " & Database.DBPrefix & "_CUSTOM_MESSAGES SET EMAIL_WELCOMEMESSAGE = 'Thank you for registering at our site.  You may now log in and begin using your account.<br /><br />To complete your profile or to add more information, log in and click the User CP button.', MESSAGE_PRIVACYNOTICE = 'A unique username and e-mail address are required to access the forums.  All information shared in your member profile is available to the public and you should exercise caution before posting personal data.  The administrators and moderators of this web site are not responsible for the privacy of any user.<br /><br />Your browser must have cookies enabled in order to use the forums.  You will not be able to log in after registering without allowing the forum to store your user data in cookie files.<br /><br />We are not responsible for any materials posted on this site by its members.  We will, however, attempt to remove materials that we feel are inappropriate.  If you see posts or information in these forums with which you think is indecent, you are encouraged to contact us and let us know about it.<br /><br />By pressing the ""Agree"" button, you agree that you are 13 years of age or over.  You also agree that you will not post any copyrighted material that is not owned by yourself or the owners of these forums. In your use of these forums, you agree that you will not post any information which is vulgar, harassing, hateful, threatening, invading of others privacy, sexually oriented, or violates any laws.'")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_MEMBER_FILETYPES text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_TOPIC_UPLOADSIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_TOPICS_FONTSIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_TOPICS_FONTCOLOR varchar(20)")
					Reader = Database.Read("SELECT " & Database.DBPrefix & "_FONTSIZE, " & Database.DBPrefix & "_FONT_COLOR FROM " & Database.DBPrefix & "_SETTINGS", 1)
					While Reader.Read()
						Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_MEMBER_FILETYPES = 'image/gif, image/png, application/msword, image/jpeg, application/zip, text/plain, image/pjpeg, application/mspowerpoint, application/vnd.ms-excel, application/pdf', " & Database.DBPrefix & "_TOPIC_UPLOADSIZE = 1024, " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE = 0, " & Database.DBPrefix & "_TOPICS_FONTSIZE = " & Reader(Database.DBPrefix & "_FONTSIZE") & ", " & Database.DBPrefix & "_TOPICS_FONTCOLOR = '" & Reader(Database.DBPrefix & "_FONT_COLOR").ToString() & "'")
					End While
					Reader.Close()
					Reader = Database.Read("SELECT ID, " & Database.DBPrefix & "_CUSTOM_CSS FROM " & Database.DBPrefix & "_SETTINGS")
					While Reader.Read()
						Dim TheCustomCSS as String = Reader(Database.DBPrefix & "_CUSTOM_CSS").ToString()
						TheCustomCSS = TheCustomCSS.Replace(".MessageBox{", ".AdminButtons{font-size: 18px; width: 270px; border-color: [HeaderColor];}" & CHR(10) & ".MessageBox{")
						Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_CUSTOM_CSS = '" & TheCustomCSS & "' WHERE ID = " & Reader("ID"))
					End While
					Reader.Close()
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_TOPICS ADD TOPIC_FILEUPLOAD varchar(100)")
					Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_FILEUPLOAD = ''")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_RANKINGS ADD RANK_ALLOW_UPLOADS int")
					Database.Write("UPDATE " & Database.DBPrefix & "_RANKINGS SET RANK_ALLOW_UPLOADS = 0")
					if (Database.DBType = "MySQL") then
						Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES MODIFY COLUMN PAGE_NAME varchar(100)")
					else
						Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ALTER COLUMN PAGE_NAME varchar(100)")
					end if
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_TITLE varchar(100)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_SHOWTITLE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_COLUMNS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_ALIGN int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_STATUS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_THUMBNAIL varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_PHOTO varchar(50)")
					Database.Write("UPDATE " & Database.DBPrefix & "_PAGES SET PAGE_SUB_TITLE = 'Sub-Categories', PAGE_SUB_SHOWTITLE = 0, PAGE_SUB_COLUMNS = 1, PAGE_SUB_ALIGN = 1, PAGE_SUB_STATUS = 1, PAGE_THUMBNAIL = '', PAGE_PHOTO = ''")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_FOLDERS(FOLDER_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", FOLDER_CORE int, FOLDER_PARENT int, FOLDER_NAME varchar(100))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_FILES(FILE_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", FILE_CORE int, FILE_FOLDER int, FILE_NAME varchar(100))")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'avatars')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'customavatars')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'documents')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'forumimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'images')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'memberphotos')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'pageimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'photogalleries')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'rankimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'rotatorimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'topicfiles')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGAdminCode.html')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGCode.html')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGHtmlForms.html')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGMainMenu.html')")
					Reader = Database.Read("SELECT FOLDER_ID, FOLDER_NAME FROM " & Database.DBPrefix & "_FOLDERS ORDER BY FOLDER_NAME")
					While Reader.Read()
						if (Reader("FOLDER_NAME").ToString() = "avatars") then
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'default.gif')")
							Dim AvatarReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'avatars/%'")
							While AvatarReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (AvatarReader("UPLOAD_NAME").ToString()).Replace("avatars/", "") & "')")
							End While
							AvatarReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "customavatars") then
							Dim CustomAvatarReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'customavatars/%'")
							While CustomAvatarReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (CustomAvatarReader("UPLOAD_NAME").ToString()).Replace("customavatars/", "") & "')")
							End While
							CustomAvatarReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "documents") then
							Dim DocumentReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'documents/%'")
							While DocumentReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (DocumentReader("UPLOAD_NAME").ToString()).Replace("documents/", "") & "')")
							End While
							DocumentReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "forumimages") then
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'folder.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'folder_home.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'folder_up.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_aol.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_icq.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_msn.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_yahoo.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'latestcomments.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'lock.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'page_icon.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'pm_inbox.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'pm_new.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'rss.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'icons')")
							Dim IconReader as OdbcDataReader = Database.Read("SELECT FOLDER_ID FROM " & Database.DBPrefix & "_FOLDERS WHERE FOLDER_NAME = 'icons' ORDER BY FOLDER_ID DESC", 1)
							While IconReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'bz2.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'conf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'css.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'csv.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'doc.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'file.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'gz.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'html.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'jpg.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'js.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'mov.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'mp3.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'mpg.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'pdf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'php.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'ppt.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'rar.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'rtf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'swf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'tar.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'tgz.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'txt.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'wav.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'wma.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'wmv.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'xls.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'xml.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'zip.png')")
							End While
							IconReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "images") then
							Dim ImagesReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'images/%'")
							While ImagesReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (ImagesReader("UPLOAD_NAME").ToString()).Replace("images/", "") & "')")
							End While
							ImagesReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "memberphotos") then
							Dim MemberPhotosReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'memberphotos/%'")
							While MemberPhotosReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (MemberPhotosReader("UPLOAD_NAME").ToString()).Replace("memberphotos/", "") & "')")
							End While
							MemberPhotosReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "pageimages") then
							Dim PageImagesReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'pageimages/%'")
							While PageImagesReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (PageImagesReader("UPLOAD_NAME").ToString()).Replace("pageimages/", "") & "')")
							End While
							PageImagesReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "photogalleries") then
							Dim GalleryReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'photogalleries/%'")
							While GalleryReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (GalleryReader("UPLOAD_NAME").ToString()).Replace("photogalleries/", "") & "')")
							End While
							GalleryReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "rankimages") then
							Dim RankReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'rankimages/%'")
							if (RankReader.HasRows()) then
								While RankReader.Read()
									Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (RankReader("UPLOAD_NAME").ToString()).Replace("rankimages/", "") & "')")
								End While
							else
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_0.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_1.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_2.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_3.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_4.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_5.gif')")
							end if
							RankReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "rotatorimages") then
							Dim RotatorReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'rotatorimages/%'")
							While RotatorReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (RotatorReader("UPLOAD_NAME").ToString()).Replace("rotatorimages/", "") & "')")
							End While
							RotatorReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "topicfiles") then
							Dim TopicFileReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'topicfiles/%'")
							While TopicFileReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (TopicFileReader("UPLOAD_NAME").ToString()).Replace("topicfiles/", "") & "')")
							End While
							TopicFileReader.Close()
						end if
					End While
					Reader.Close()
				elseif (sender.CommandArgument = "2.0") then
					'------------  2.2 Additions
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_MEMBER_VALIDATION int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_SMTP varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_ADDRESS varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_ALLOWSEND int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_ALLOWSUB int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_BGIMAGE varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_ALLOWSUB int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_LATESTBLOGS_NUMBER int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_QUICK_REGISTRATION int")
					Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_MEMBER_VALIDATION = 0, " & Database.DBPrefix & "_EMAIL_SMTP = 'mail.yourserver.com', " & Database.DBPrefix & "_EMAIL_ADDRESS = 'mail@yourserver.com', " & Database.DBPrefix & "_EMAIL_ALLOWSEND = 0, " & Database.DBPrefix & "_EMAIL_ALLOWSUB = 0, " & Database.DBPrefix & "_BGIMAGE = '', " & Database.DBPrefix & "_ALLOWSUB = 1, " & Database.DBPrefix & "_LATESTBLOGS_NUMBER = 8, " & Database.DBPrefix & "_QUICK_REGISTRATION = 0")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_MEMBERS ADD MEMBER_VALIDATED int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_MEMBERS ADD MEMBER_VALIDATION_STRING varchar(100)")
					Database.Write("UPDATE " & Database.DBPrefix & "_MEMBERS SET MEMBER_VALIDATED = 1, MEMBER_VALIDATION_STRING = '', MEMBER_AVATAR_USECUSTOM = 0, MEMBER_AVATAR_CUSTOMLOADED = 0")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_SUBSCRIPTIONS(SUB_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", SUB_MEMBER int, SUB_TOPIC int, SUB_EMAIL int)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_BLOG_TOPICS(BLOG_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", BLOG_AUTHOR int, BLOG_DATE datetime, BLOG_REPLIES int, BLOG_TITLE varchar(100), BLOG_TEXT text)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_BLOG_REPLIES(BLOG_REPLY_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", BLOG_ID int, BLOG_REPLY_AUTHOR int, BLOG_REPLY_DATE datetime, BLOG_REPLY_TEXT text)")
					'------------  3.0 Additions
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_PORT varchar(15)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_USERNAME varchar(30)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_PASSWORD varchar(30)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_CURSE_FILTER int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_RSS_FEEDS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_HORIZ_DIVIDE varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_VERT_DIVIDE varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_ALLOW_EDITS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_ALLOW_REGISTRATION int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_ALLOW_MEDIA int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_ALLOW_REPORTING int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_HIDE_MEMBERS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_HIDE_LOGIN int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_MEMBER_PHOTOSIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_CUSTOM_CSS text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_HTML_TITLE varchar(100)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_THUMBNAIL_SIZE int")
					Dim HTMLTitle as String = ""
					Reader = Database.Read("SELECT " & Database.DBPrefix & "_TITLE FROM " & Database.DBPrefix & "_SETTINGS WHERE ID = 1")
					While Reader.Read()
						HTMLTitle = Functions.RepairString(Reader(Database.DBPrefix & "_TITLE").ToString())
					End While
					Reader.Close()
					Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_EMAIL_PORT = '', " & Database.DBPrefix & "_EMAIL_USERNAME = '', " & Database.DBPrefix & "_EMAIL_PASSWORD = '', " & Database.DBPrefix & "_CURSE_FILTER = 0, " & Database.DBPrefix & "_RSS_FEEDS = 0, " & Database.DBPrefix & "_HORIZ_DIVIDE = '&nbsp;&nbsp;<b>|</b>&nbsp;&nbsp;', " & Database.DBPrefix & "_VERT_DIVIDE = '<br /><br />', " & Database.DBPrefix & "_ALLOW_EDITS = 1, " & Database.DBPrefix & "_ALLOW_REGISTRATION = 1, " & Database.DBPrefix & "_ALLOW_MEDIA = 1, " & Database.DBPrefix & "_ALLOW_REPORTING = 1, " & Database.DBPrefix & "_HIDE_MEMBERS = 0, " & Database.DBPrefix & "_HIDE_LOGIN = 0, " & Database.DBPrefix & "_MEMBER_PHOTOSIZE = 0, " & Database.DBPrefix & "_CUSTOM_CSS = '<STYLE TYPE=""text/css"">" & CHR(10) & "<!--" & CHR(10) & "A:link {text-decoration: [LinkDecoration]; color: [LinkColor];}" & CHR(10) & "A:visited {text-decoration: [VLinkDecoration]; color: [VLinkColor];}" & CHR(10) & "A:active {text-decoration: [ALinkDecoration]; color: [ALinkColor];}" & CHR(10) & "A:hover {text-decoration: [HLinkDecoration]; color: [HLinkColor];}" & CHR(10) & ".dmgbuttons{" & CHR(10) & "	font-family: Verdana;" & CHR(10) & "	font-size: 8px;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "	border-color: [ButtonColor];}" & CHR(10) & ".LoginButton{font-size: 12px; border-color: [ButtonColor];}" & CHR(10) & ".AdminButtons{font-size: 18px; width: 270px; border-color: [HeaderColor];}" & CHR(10) & ".LoginBox{border-color: [ButtonColor];}" & CHR(10) & ".ContentBox{border:2px solid [BorderColor];}" & CHR(10) & ".HeaderCell{background-color: [HeaderColor];}" & CHR(10) & ".SubHeaderCell{background-color: [SubHeaderColor];}" & CHR(10) & ".FooterCell{background-color: [FooterColor];}" & CHR(10) & ".TableRow1{background-color: [TableBGColor1];}" & CHR(10) & ".TableRow2{background-color: [TableBGColor2];}" & CHR(10) & ".MessageBox{font-size: 20px;}" & CHR(10) & ".PhotoGalleryTable {border: 0px;}" & CHR(10) & ".PhotoGalleryTable TD{font-size: 13px; padding: 10px; text-align: center;}" & CHR(10) & "-->" & CHR(10) & "</STYLE>', " & Database.DBPrefix & "_HTML_TITLE = '" & HTMLTitle & "', " & Database.DBPrefix & "_THUMBNAIL_SIZE = 150")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_MEMBERS ADD MEMBER_RANKING int")
					Database.Write("UPDATE " & Database.DBPrefix & "_MEMBERS SET MEMBER_RANKING = 0")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_AUTOFORMAT int")
					Database.Write("UPDATE " & Database.DBPrefix & "_PAGES SET PAGE_AUTOFORMAT = 0")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_FORUMS ADD FORUM_CONTENT text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_FORUMS ADD FORUM_SHOWHEADERS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_FORUMS ADD FORUM_SHOWLOGIN int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_FORUMS ADD FORUM_EMAIL_MODERATORS int")
					Database.Write("UPDATE " & Database.DBPrefix & "_FORUMS SET FORUM_CONTENT = '', FORUM_SHOWHEADERS = 1, FORUM_SHOWLOGIN = 1, FORUM_EMAIL_MODERATORS = 0")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CATEGORIES ADD CATEGORY_CONTENT text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CATEGORIES ADD CATEGORY_SHOWHEADERS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CATEGORIES ADD CATEGORY_SHOWLOGIN int")
					Database.Write("UPDATE " & Database.DBPrefix & "_CATEGORIES SET CATEGORY_CONTENT = '', CATEGORY_SHOWHEADERS = 1, CATEGORY_SHOWLOGIN = 1")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_VARIABLES ADD VAR4 varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_VARIABLES ADD VAR5 varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_VARIABLES ADD TEXT3 text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_VARIABLES ADD TEXT4 text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_VARIABLES ADD TEXT5 text")
					Database.Write("UPDATE " & Database.DBPrefix & "_VARIABLES SET VAR4 = 'Variable 4 Text', VAR5 = 'Variable 5 Text', TEXT3 = 'Memo Field 3', TEXT4 = 'Memo Field 4', TEXT5 = 'Memo Field 5'")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_REPLIES ADD REPLY_CONFIRMED int")
					Database.Write("UPDATE " & Database.DBPrefix & "_REPLIES SET REPLY_CONFIRMED = 1")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_TOPICS ADD TOPIC_UNCONFIRMED_REPLIES int")
					Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_UNCONFIRMED_REPLIES = 0")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_CURSE_FILTER(CURSE_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", CURSE_WORD varchar(20), CURSE_REPLACEMENT varchar(20))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_MAIN_MENU(LINK_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", LINK_ORDER int, LINK_TEXT varchar(50), LINK_TYPE int, LINK_PARAMETER varchar(100), LINK_WINDOW int)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_ROTATOR(ROTATOR_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", ROTATOR_NAME varchar(30))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_ROTATOR_IMAGES(IMAGE_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", ROTATOR_ID int, IMAGE_EXTENSION varchar(5), IMAGE_URL varchar(100), IMAGE_DESCRIPTION varchar(100), IMAGE_WINDOW int, IMAGE_BORDER int)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_GALLERY(GALLERY_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", GALLERY_NAME varchar(30))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_GALLERY_PHOTOS(PHOTO_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", GALLERY_ID int, PHOTO_EXTENSION varchar(5), PHOTO_DESCRIPTION varchar(100))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_HTML_FORMS(FORM_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", FORM_NAME varchar(50), FORM_DATE datetime, FORM_TEXT text, FORM_NEW int, FORM_EMAIL int)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_MEMBER_PHOTOS(PHOTO_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", MEMBER_ID int, PHOTO_EXTENSION varchar(5), PHOTO_SIZE int, PHOTO_DESCRIPTION varchar(100))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_BANNED_IP(ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", MEMBER_ID int, IP_ADDRESS varchar(50))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_CUSTOM_MESSAGES(ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", EMAIL_ADMINAPPROVAL text, EMAIL_SENDKEY text, EMAIL_CONFIRMPOST text, EMAIL_SUBSCRIPTION text, MESSAGE_ADMINAPPROVAL text, MESSAGE_SENDKEY text, MESSAGE_REGISTRATION text, MESSAGE_VALIDATION text, MESSAGE_CONFIRMPOST text)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_PM_TOPICS(TOPIC_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", TOPIC_FROM int, TOPIC_TO int, TOPIC_SUBJECT varchar(100), TOPIC_MESSAGE text, TOPIC_DATE datetime, TOPIC_TO_READ int, TOPIC_FROM_READ int, TOPIC_LASTPOST_AUTHOR int, TOPIC_LASTPOST_DATE datetime, TOPIC_REPLIES int, TOPIC_SHOWSENDER int)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_PM_REPLIES(REPLY_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", TOPIC_ID int, REPLY_AUTHOR int, REPLY_MESSAGE text, REPLY_DATE datetime)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (1, 'Main Page', 1, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (2, 'Forums', 3, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (3, 'Register', 4, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (4, 'Active Topics', 5, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (5, 'Members', 6, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (6, 'Search', 7, '0', 0)")

					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (7, 'User CP', 10, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (8, 'Private Messages', 11, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (9, 'Administration', 13, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_CUSTOM_MESSAGES (EMAIL_ADMINAPPROVAL, EMAIL_SENDKEY, EMAIL_CONFIRMPOST, EMAIL_SUBSCRIPTION, MESSAGE_ADMINAPPROVAL, MESSAGE_SENDKEY, MESSAGE_REGISTRATION, MESSAGE_VALIDATION, MESSAGE_CONFIRMPOST) VALUES ('Your account has been activated.  You can now return to the site and log in with the username/password you provided during registration.', 'You are now one step away from completing your registration.  Listed below is a unique verification key that must be entered on the web site before you can begin using the forums.  Copy the key and return to the web page.  Log in with the username and password that you provided and you will be prompted to enter the key.  After entering the key, you will be able to log in normally in the future.', 'A new post has been made and requires moderator approval.  Click the link below to view the thread.', 'A thread that you have subscribed to has been updated.  Click the link below to view the thread.', 'Thank you for registering.  Before you can begin using your account the administrator must validate your registration.  Once the admin has reviewed and accepted your registration, you will be able to log in and begin using your account.', 'Thank you for registering.  You have now been sent a confirmation e-mail message that provides a unique key for activating your account.  When the e-mail arrives, return to this site and log in with your username and password.  You will then be asked to provide the unique activation key.', 'Your registration is complete.  You may now log into the forums and begin using your account.<br /><br />To complete your profile or to add more information, log in and click the User CP button.', 'Thank you for registering an account on this site.  Your registration is now complete and you are logged in.', 'This forum requires admin/moderator approval before posts appear.<br />Your post will appear once a moderator has reviewed/approved it.')")
					Reader = Database.Read("SELECT * FROM " & Database.DBPrefix & "_PM")
					While Reader.Read()
						Database.Write("INSERT INTO " & Database.DBPrefix & "_PM_TOPICS (TOPIC_FROM, TOPIC_TO, TOPIC_SUBJECT, TOPIC_MESSAGE, TOPIC_DATE, TOPIC_TO_READ, TOPIC_FROM_READ, TOPIC_LASTPOST_AUTHOR, TOPIC_LASTPOST_DATE, TOPIC_REPLIES, TOPIC_SHOWSENDER) VALUES (" & Reader("PM_FROM") & ", " & Reader("PM_TO") & ", '" & Functions.RepairString(Reader("PM_SUBJECT").ToString()) & "', '" & Functions.RepairString(Reader("PM_MESSAGE").ToString()) & "', '" & Reader("PM_DATE").ToString() & "', " & Reader("PM_READ") & ", 1, " & Reader("PM_FROM") & ", '" & Reader("PM_DATE").ToString() & "', 0, 0)")
					End While
					Reader.Close()
					'------------  3.1 Additions
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PM_TOPICS ADD TOPIC_SHOWRECEIVER int")
					Database.Write("UPDATE " & Database.DBPrefix & "_PM_TOPICS SET TOPIC_SHOWRECEIVER = 1")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SECURITY int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_PASSWORD varchar(100)")
					Database.Write("UPDATE " & Database.DBPrefix & "_PAGES SET PAGE_SECURITY = 0, PAGE_PASSWORD = ''")
					Database.Write("UPDATE " & Database.DBPrefix & "_PAGES SET PAGE_PARENT = 0 WHERE PAGE_PARENT = -1")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_PAGES_PRIVILEGED(PRIVILEGED_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", MEMBER_ID int, PAGE_ID int, PRIVILEGED_ACCESS int)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_PHOTO_SIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_AVATAR_SIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SEARCH_TOPICS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SEARCH_MEMBERS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SEARCH_BLOGS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SEARCH_PAGES int")
					Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_PHOTO_SIZE = 775, " & Database.DBPrefix & "_AVATAR_SIZE = 125, " & Database.DBPrefix & "_SEARCH_TOPICS = 1, " & Database.DBPrefix & "_SEARCH_MEMBERS = 1, " & Database.DBPrefix & "_SEARCH_BLOGS = 1, " & Database.DBPrefix & "_SEARCH_PAGES = 1")
					'------------  3.2 Additions
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CUSTOM_MESSAGES ADD EMAIL_WELCOMEMESSAGE text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CUSTOM_MESSAGES ADD MESSAGE_PRIVACYNOTICE text")
					Database.Write("UPDATE " & Database.DBPrefix & "_CUSTOM_MESSAGES SET EMAIL_WELCOMEMESSAGE = 'Thank you for registering at our site.  You may now log in and begin using your account.<br /><br />To complete your profile or to add more information, log in and click the User CP button.', MESSAGE_PRIVACYNOTICE = 'A unique username and e-mail address are required to access the forums.  All information shared in your member profile is available to the public and you should exercise caution before posting personal data.  The administrators and moderators of this web site are not responsible for the privacy of any user.<br /><br />Your browser must have cookies enabled in order to use the forums.  You will not be able to log in after registering without allowing the forum to store your user data in cookie files.<br /><br />We are not responsible for any materials posted on this site by its members.  We will, however, attempt to remove materials that we feel are inappropriate.  If you see posts or information in these forums with which you think is indecent, you are encouraged to contact us and let us know about it.<br /><br />By pressing the ""Agree"" button, you agree that you are 13 years of age or over.  You also agree that you will not post any copyrighted material that is not owned by yourself or the owners of these forums. In your use of these forums, you agree that you will not post any information which is vulgar, harassing, hateful, threatening, invading of others privacy, sexually oriented, or violates any laws.'")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_MEMBER_FILETYPES text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_TOPIC_UPLOADSIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_TOPICS_FONTSIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_TOPICS_FONTCOLOR varchar(20)")
					Reader = Database.Read("SELECT " & Database.DBPrefix & "_FONTSIZE, " & Database.DBPrefix & "_FONT_COLOR FROM " & Database.DBPrefix & "_SETTINGS", 1)
					While Reader.Read()
						Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_MEMBER_FILETYPES = 'image/gif, image/png, application/msword, image/jpeg, application/zip, text/plain, image/pjpeg, application/mspowerpoint, application/vnd.ms-excel, application/pdf', " & Database.DBPrefix & "_TOPIC_UPLOADSIZE = 1024, " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE = 0, " & Database.DBPrefix & "_TOPICS_FONTSIZE = " & Reader(Database.DBPrefix & "_FONTSIZE") & ", " & Database.DBPrefix & "_TOPICS_FONTCOLOR = '" & Reader(Database.DBPrefix & "_FONT_COLOR").ToString() & "'")
					End While
					Reader.Close()
					Reader = Database.Read("SELECT ID, " & Database.DBPrefix & "_CUSTOM_CSS FROM " & Database.DBPrefix & "_SETTINGS")
					While Reader.Read()
						Dim TheCustomCSS as String = Reader(Database.DBPrefix & "_CUSTOM_CSS").ToString()
						TheCustomCSS = TheCustomCSS.Replace(".MessageBox{", ".AdminButtons{font-size: 18px; width: 270px; border-color: [HeaderColor];}" & CHR(10) & ".MessageBox{")
						Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_CUSTOM_CSS = '" & TheCustomCSS & "' WHERE ID = " & Reader("ID"))
					End While
					Reader.Close()
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_TOPICS ADD TOPIC_FILEUPLOAD varchar(100)")
					Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_FILEUPLOAD = ''")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_RANKINGS ADD RANK_ALLOW_UPLOADS int")
					Database.Write("UPDATE " & Database.DBPrefix & "_RANKINGS SET RANK_ALLOW_UPLOADS = 0")
					if (Database.DBType = "MySQL") then
						Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES MODIFY COLUMN PAGE_NAME varchar(100)")
					else
						Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ALTER COLUMN PAGE_NAME varchar(100)")
					end if
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_TITLE varchar(100)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_SHOWTITLE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_COLUMNS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_ALIGN int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_STATUS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_THUMBNAIL varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_PHOTO varchar(50)")
					Database.Write("UPDATE " & Database.DBPrefix & "_PAGES SET PAGE_SUB_TITLE = 'Sub-Categories', PAGE_SUB_SHOWTITLE = 0, PAGE_SUB_COLUMNS = 1, PAGE_SUB_ALIGN = 1, PAGE_SUB_STATUS = 1, PAGE_THUMBNAIL = '', PAGE_PHOTO = ''")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_FOLDERS(FOLDER_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", FOLDER_CORE int, FOLDER_PARENT int, FOLDER_NAME varchar(100))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_FILES(FILE_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", FILE_CORE int, FILE_FOLDER int, FILE_NAME varchar(100))")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'avatars')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'customavatars')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'documents')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'forumimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'images')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'memberphotos')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'pageimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'photogalleries')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'rankimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'rotatorimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'topicfiles')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGAdminCode.html')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGCode.html')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGHtmlForms.html')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGMainMenu.html')")
					Reader = Database.Read("SELECT FOLDER_ID, FOLDER_NAME FROM " & Database.DBPrefix & "_FOLDERS ORDER BY FOLDER_NAME")
					While Reader.Read()
						if (Reader("FOLDER_NAME").ToString() = "avatars") then
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'default.gif')")
							Dim AvatarReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'avatars/%'")
							While AvatarReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (AvatarReader("UPLOAD_NAME").ToString()).Replace("avatars/", "") & "')")
							End While
							AvatarReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "customavatars") then
							Dim CustomAvatarReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'customavatars/%'")
							While CustomAvatarReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (CustomAvatarReader("UPLOAD_NAME").ToString()).Replace("customavatars/", "") & "')")
							End While
							CustomAvatarReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "documents") then
							Dim DocumentReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'documents/%'")
							While DocumentReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (DocumentReader("UPLOAD_NAME").ToString()).Replace("documents/", "") & "')")
							End While
							DocumentReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "forumimages") then
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'folder.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'folder_home.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'folder_up.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_aol.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_icq.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_msn.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_yahoo.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'latestcomments.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'lock.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'page_icon.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'pm_inbox.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'pm_new.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'rss.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'icons')")
							Dim IconReader as OdbcDataReader = Database.Read("SELECT FOLDER_ID FROM " & Database.DBPrefix & "_FOLDERS WHERE FOLDER_NAME = 'icons' ORDER BY FOLDER_ID DESC", 1)
							While IconReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'bz2.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'conf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'css.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'csv.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'doc.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'file.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'gz.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'html.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'jpg.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'js.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'mov.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'mp3.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'mpg.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'pdf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'php.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'ppt.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'rar.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'rtf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'swf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'tar.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'tgz.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'txt.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'wav.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'wma.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'wmv.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'xls.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'xml.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'zip.png')")
							End While
							IconReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "images") then
							Dim ImagesReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'images/%'")
							While ImagesReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (ImagesReader("UPLOAD_NAME").ToString()).Replace("images/", "") & "')")
							End While
							ImagesReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "memberphotos") then
							Dim MemberPhotosReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'memberphotos/%'")
							While MemberPhotosReader.Read()

								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (MemberPhotosReader("UPLOAD_NAME").ToString()).Replace("memberphotos/", "") & "')")
							End While
							MemberPhotosReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "pageimages") then
							Dim PageImagesReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'pageimages/%'")
							While PageImagesReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (PageImagesReader("UPLOAD_NAME").ToString()).Replace("pageimages/", "") & "')")
							End While
							PageImagesReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "photogalleries") then
							Dim GalleryReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'photogalleries/%'")
							While GalleryReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (GalleryReader("UPLOAD_NAME").ToString()).Replace("photogalleries/", "") & "')")
							End While
							GalleryReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "rankimages") then
							Dim RankReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'rankimages/%'")
							if (RankReader.HasRows()) then
								While RankReader.Read()
									Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (RankReader("UPLOAD_NAME").ToString()).Replace("rankimages/", "") & "')")
								End While
							else
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_0.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_1.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_2.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_3.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_4.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_5.gif')")
							end if
							RankReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "rotatorimages") then
							Dim RotatorReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'rotatorimages/%'")
							While RotatorReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (RotatorReader("UPLOAD_NAME").ToString()).Replace("rotatorimages/", "") & "')")
							End While
							RotatorReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "topicfiles") then
							Dim TopicFileReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'topicfiles/%'")
							While TopicFileReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (TopicFileReader("UPLOAD_NAME").ToString()).Replace("topicfiles/", "") & "')")
							End While
							TopicFileReader.Close()
						end if
					End While
					Reader.Close()
				elseif (sender.CommandArgument = "2.2") then
					'------------  3.0 Additions
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_PORT varchar(15)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_USERNAME varchar(30)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_PASSWORD varchar(30)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_CURSE_FILTER int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_RSS_FEEDS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_HORIZ_DIVIDE varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_VERT_DIVIDE varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_ALLOW_EDITS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_ALLOW_REGISTRATION int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_ALLOW_MEDIA int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_ALLOW_REPORTING int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_HIDE_MEMBERS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_HIDE_LOGIN int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_MEMBER_PHOTOSIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_CUSTOM_CSS text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_HTML_TITLE varchar(100)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_THUMBNAIL_SIZE int")
					Dim HTMLTitle as String = ""
					Reader = Database.Read("SELECT " & Database.DBPrefix & "_TITLE FROM " & Database.DBPrefix & "_SETTINGS WHERE ID = 1")
					While Reader.Read()
						HTMLTitle = Functions.RepairString(Reader(Database.DBPrefix & "_TITLE").ToString())
					End While
					Reader.Close()
					Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_EMAIL_PORT = '', " & Database.DBPrefix & "_EMAIL_USERNAME = '', " & Database.DBPrefix & "_EMAIL_PASSWORD = '', " & Database.DBPrefix & "_CURSE_FILTER = 0, " & Database.DBPrefix & "_RSS_FEEDS = 0, " & Database.DBPrefix & "_HORIZ_DIVIDE = '&nbsp;&nbsp;<b>|</b>&nbsp;&nbsp;', " & Database.DBPrefix & "_VERT_DIVIDE = '<br /><br />', " & Database.DBPrefix & "_ALLOW_EDITS = 1, " & Database.DBPrefix & "_ALLOW_REGISTRATION = 1, " & Database.DBPrefix & "_ALLOW_MEDIA = 1, " & Database.DBPrefix & "_ALLOW_REPORTING = 1, " & Database.DBPrefix & "_HIDE_MEMBERS = 0, " & Database.DBPrefix & "_HIDE_LOGIN = 0, " & Database.DBPrefix & "_MEMBER_PHOTOSIZE = 0, " & Database.DBPrefix & "_CUSTOM_CSS = '<STYLE TYPE=""text/css"">" & CHR(10) & "<!--" & CHR(10) & "A:link {text-decoration: [LinkDecoration]; color: [LinkColor];}" & CHR(10) & "A:visited {text-decoration: [VLinkDecoration]; color: [VLinkColor];}" & CHR(10) & "A:active {text-decoration: [ALinkDecoration]; color: [ALinkColor];}" & CHR(10) & "A:hover {text-decoration: [HLinkDecoration]; color: [HLinkColor];}" & CHR(10) & ".dmgbuttons{" & CHR(10) & "	font-family: Verdana;" & CHR(10) & "	font-size: 8px;" & CHR(10) & "	font-weight: bold;" & CHR(10) & "	border-color: [ButtonColor];}" & CHR(10) & ".LoginButton{font-size: 12px; border-color: [ButtonColor];}" & CHR(10) & ".AdminButtons{font-size: 18px; width: 270px; border-color: [HeaderColor];}" & CHR(10) & ".LoginBox{border-color: [ButtonColor];}" & CHR(10) & ".ContentBox{border:2px solid [BorderColor];}" & CHR(10) & ".HeaderCell{background-color: [HeaderColor];}" & CHR(10) & ".SubHeaderCell{background-color: [SubHeaderColor];}" & CHR(10) & ".FooterCell{background-color: [FooterColor];}" & CHR(10) & ".TableRow1{background-color: [TableBGColor1];}" & CHR(10) & ".TableRow2{background-color: [TableBGColor2];}" & CHR(10) & ".MessageBox{font-size: 20px;}" & CHR(10) & ".PhotoGalleryTable {border: 0px;}" & CHR(10) & ".PhotoGalleryTable TD{font-size: 13px; padding: 10px; text-align: center;}" & CHR(10) & "-->" & CHR(10) & "</STYLE>', " & Database.DBPrefix & "_HTML_TITLE = '" & HTMLTitle & "', " & Database.DBPrefix & "_THUMBNAIL_SIZE = 150")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_MEMBERS ADD MEMBER_RANKING int")
					Database.Write("UPDATE " & Database.DBPrefix & "_MEMBERS SET MEMBER_RANKING = 0")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_AUTOFORMAT int")
					Database.Write("UPDATE " & Database.DBPrefix & "_PAGES SET PAGE_AUTOFORMAT = 0")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_FORUMS ADD FORUM_CONTENT text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_FORUMS ADD FORUM_SHOWHEADERS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_FORUMS ADD FORUM_SHOWLOGIN int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_FORUMS ADD FORUM_EMAIL_MODERATORS int")
					Database.Write("UPDATE " & Database.DBPrefix & "_FORUMS SET FORUM_CONTENT = '', FORUM_SHOWHEADERS = 1, FORUM_SHOWLOGIN = 1, FORUM_EMAIL_MODERATORS = 0")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CATEGORIES ADD CATEGORY_CONTENT text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CATEGORIES ADD CATEGORY_SHOWHEADERS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CATEGORIES ADD CATEGORY_SHOWLOGIN int")
					Database.Write("UPDATE " & Database.DBPrefix & "_CATEGORIES SET CATEGORY_CONTENT = '', CATEGORY_SHOWHEADERS = 1, CATEGORY_SHOWLOGIN = 1")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_VARIABLES ADD VAR4 varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_VARIABLES ADD VAR5 varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_VARIABLES ADD TEXT3 text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_VARIABLES ADD TEXT4 text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_VARIABLES ADD TEXT5 text")
					Database.Write("UPDATE " & Database.DBPrefix & "_VARIABLES SET VAR4 = 'Variable 4 Text', VAR5 = 'Variable 5 Text', TEXT3 = 'Memo Field 3', TEXT4 = 'Memo Field 4', TEXT5 = 'Memo Field 5'")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_REPLIES ADD REPLY_CONFIRMED int")
					Database.Write("UPDATE " & Database.DBPrefix & "_REPLIES SET REPLY_CONFIRMED = 1")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_TOPICS ADD TOPIC_UNCONFIRMED_REPLIES int")
					Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_UNCONFIRMED_REPLIES = 0")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_CURSE_FILTER(CURSE_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", CURSE_WORD varchar(20), CURSE_REPLACEMENT varchar(20))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_MAIN_MENU(LINK_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", LINK_ORDER int, LINK_TEXT varchar(50), LINK_TYPE int, LINK_PARAMETER varchar(100), LINK_WINDOW int)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_ROTATOR(ROTATOR_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", ROTATOR_NAME varchar(30))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_ROTATOR_IMAGES(IMAGE_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", ROTATOR_ID int, IMAGE_EXTENSION varchar(5), IMAGE_URL varchar(100), IMAGE_DESCRIPTION varchar(100), IMAGE_WINDOW int, IMAGE_BORDER int)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_GALLERY(GALLERY_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", GALLERY_NAME varchar(30))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_GALLERY_PHOTOS(PHOTO_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", GALLERY_ID int, PHOTO_EXTENSION varchar(5), PHOTO_DESCRIPTION varchar(100))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_HTML_FORMS(FORM_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", FORM_NAME varchar(50), FORM_DATE datetime, FORM_TEXT text, FORM_NEW int, FORM_EMAIL int)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_MEMBER_PHOTOS(PHOTO_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", MEMBER_ID int, PHOTO_EXTENSION varchar(5), PHOTO_SIZE int, PHOTO_DESCRIPTION varchar(100))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_BANNED_IP(ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", MEMBER_ID int, IP_ADDRESS varchar(50))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_CUSTOM_MESSAGES(ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", EMAIL_ADMINAPPROVAL text, EMAIL_SENDKEY text, EMAIL_CONFIRMPOST text, EMAIL_SUBSCRIPTION text, MESSAGE_ADMINAPPROVAL text, MESSAGE_SENDKEY text, MESSAGE_REGISTRATION text, MESSAGE_VALIDATION text, MESSAGE_CONFIRMPOST text)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_PM_TOPICS(TOPIC_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", TOPIC_FROM int, TOPIC_TO int, TOPIC_SUBJECT varchar(100), TOPIC_MESSAGE text, TOPIC_DATE datetime, TOPIC_TO_READ int, TOPIC_FROM_READ int, TOPIC_LASTPOST_AUTHOR int, TOPIC_LASTPOST_DATE datetime, TOPIC_REPLIES int, TOPIC_SHOWSENDER int)")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_PM_REPLIES(REPLY_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", TOPIC_ID int, REPLY_AUTHOR int, REPLY_MESSAGE text, REPLY_DATE datetime)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (1, 'Main Page', 1, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (2, 'Forums', 3, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (3, 'Register', 4, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (4, 'Active Topics', 5, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (5, 'Members', 6, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (6, 'Search', 7, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (7, 'User CP', 10, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (8, 'Private Messages', 11, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (9, 'Administration', 13, '0', 0)")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_CUSTOM_MESSAGES (EMAIL_ADMINAPPROVAL, EMAIL_SENDKEY, EMAIL_CONFIRMPOST, EMAIL_SUBSCRIPTION, MESSAGE_ADMINAPPROVAL, MESSAGE_SENDKEY, MESSAGE_REGISTRATION, MESSAGE_VALIDATION, MESSAGE_CONFIRMPOST) VALUES ('Your account has been activated.  You can now return to the site and log in with the username/password you provided during registration.', 'You are now one step away from completing your registration.  Listed below is a unique verification key that must be entered on the web site before you can begin using the forums.  Copy the key and return to the web page.  Log in with the username and password that you provided and you will be prompted to enter the key.  After entering the key, you will be able to log in normally in the future.', 'A new post has been made and requires moderator approval.  Click the link below to view the thread.', 'A thread that you have subscribed to has been updated.  Click the link below to view the thread.', 'Thank you for registering.  Before you can begin using your account the administrator must validate your registration.  Once the admin has reviewed and accepted your registration, you will be able to log in and begin using your account.', 'Thank you for registering.  You have now been sent a confirmation e-mail message that provides a unique key for activating your account.  When the e-mail arrives, return to this site and log in with your username and password.  You will then be asked to provide the unique activation key.', 'Your registration is complete.  You may now log into the forums and begin using your account.<br /><br />To complete your profile or to add more information, log in and click the User CP button.', 'Thank you for registering an account on this site.  Your registration is now complete and you are logged in.', 'This forum requires admin/moderator approval before posts appear.<br />Your post will appear once a moderator has reviewed/approved it.')")
					Reader = Database.Read("SELECT * FROM " & Database.DBPrefix & "_PM")
					While Reader.Read()
						Database.Write("INSERT INTO " & Database.DBPrefix & "_PM_TOPICS (TOPIC_FROM, TOPIC_TO, TOPIC_SUBJECT, TOPIC_MESSAGE, TOPIC_DATE, TOPIC_TO_READ, TOPIC_FROM_READ, TOPIC_LASTPOST_AUTHOR, TOPIC_LASTPOST_DATE, TOPIC_REPLIES, TOPIC_SHOWSENDER) VALUES (" & Reader("PM_FROM") & ", " & Reader("PM_TO") & ", '" & Functions.RepairString(Reader("PM_SUBJECT").ToString()) & "', '" & Functions.RepairString(Reader("PM_MESSAGE").ToString()) & "', '" & Reader("PM_DATE").ToString() & "', " & Reader("PM_READ") & ", 1, " & Reader("PM_FROM") & ", '" & Reader("PM_DATE").ToString() & "', 0, 0)")
					End While
					Reader.Close()
					'------------  3.1 Additions
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PM_TOPICS ADD TOPIC_SHOWRECEIVER int")
					Database.Write("UPDATE " & Database.DBPrefix & "_PM_TOPICS SET TOPIC_SHOWRECEIVER = 1")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SECURITY int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_PASSWORD varchar(100)")
					Database.Write("UPDATE " & Database.DBPrefix & "_PAGES SET PAGE_SECURITY = 0, PAGE_PASSWORD = ''")
					Database.Write("UPDATE " & Database.DBPrefix & "_PAGES SET PAGE_PARENT = 0 WHERE PAGE_PARENT = -1")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_PAGES_PRIVILEGED(PRIVILEGED_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", MEMBER_ID int, PAGE_ID int, PRIVILEGED_ACCESS int)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_PHOTO_SIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_AVATAR_SIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SEARCH_TOPICS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SEARCH_MEMBERS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SEARCH_BLOGS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SEARCH_PAGES int")
					Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_PHOTO_SIZE = 775, " & Database.DBPrefix & "_AVATAR_SIZE = 125, " & Database.DBPrefix & "_SEARCH_TOPICS = 1, " & Database.DBPrefix & "_SEARCH_MEMBERS = 1, " & Database.DBPrefix & "_SEARCH_BLOGS = 1, " & Database.DBPrefix & "_SEARCH_PAGES = 1")
					'------------  3.2 Additions
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CUSTOM_MESSAGES ADD EMAIL_WELCOMEMESSAGE text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CUSTOM_MESSAGES ADD MESSAGE_PRIVACYNOTICE text")
					Database.Write("UPDATE " & Database.DBPrefix & "_CUSTOM_MESSAGES SET EMAIL_WELCOMEMESSAGE = 'Thank you for registering at our site.  You may now log in and begin using your account.<br /><br />To complete your profile or to add more information, log in and click the User CP button.', MESSAGE_PRIVACYNOTICE = 'A unique username and e-mail address are required to access the forums.  All information shared in your member profile is available to the public and you should exercise caution before posting personal data.  The administrators and moderators of this web site are not responsible for the privacy of any user.<br /><br />Your browser must have cookies enabled in order to use the forums.  You will not be able to log in after registering without allowing the forum to store your user data in cookie files.<br /><br />We are not responsible for any materials posted on this site by its members.  We will, however, attempt to remove materials that we feel are inappropriate.  If you see posts or information in these forums with which you think is indecent, you are encouraged to contact us and let us know about it.<br /><br />By pressing the ""Agree"" button, you agree that you are 13 years of age or over.  You also agree that you will not post any copyrighted material that is not owned by yourself or the owners of these forums. In your use of these forums, you agree that you will not post any information which is vulgar, harassing, hateful, threatening, invading of others privacy, sexually oriented, or violates any laws.'")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_MEMBER_FILETYPES text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_TOPIC_UPLOADSIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_TOPICS_FONTSIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_TOPICS_FONTCOLOR varchar(20)")
					Reader = Database.Read("SELECT " & Database.DBPrefix & "_FONTSIZE, " & Database.DBPrefix & "_FONT_COLOR FROM " & Database.DBPrefix & "_SETTINGS", 1)
					While Reader.Read()
						Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_MEMBER_FILETYPES = 'image/gif, image/png, application/msword, image/jpeg, application/zip, text/plain, image/pjpeg, application/mspowerpoint, application/vnd.ms-excel, application/pdf', " & Database.DBPrefix & "_TOPIC_UPLOADSIZE = 1024, " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE = 0, " & Database.DBPrefix & "_TOPICS_FONTSIZE = " & Reader(Database.DBPrefix & "_FONTSIZE") & ", " & Database.DBPrefix & "_TOPICS_FONTCOLOR = '" & Reader(Database.DBPrefix & "_FONT_COLOR").ToString() & "'")
					End While
					Reader.Close()
					Reader = Database.Read("SELECT ID, " & Database.DBPrefix & "_CUSTOM_CSS FROM " & Database.DBPrefix & "_SETTINGS")
					While Reader.Read()
						Dim TheCustomCSS as String = Reader(Database.DBPrefix & "_CUSTOM_CSS").ToString()
						TheCustomCSS = TheCustomCSS.Replace(".MessageBox{", ".AdminButtons{font-size: 18px; width: 270px; border-color: [HeaderColor];}" & CHR(10) & ".MessageBox{")
						Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_CUSTOM_CSS = '" & TheCustomCSS & "' WHERE ID = " & Reader("ID"))
					End While
					Reader.Close()
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_TOPICS ADD TOPIC_FILEUPLOAD varchar(100)")
					Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_FILEUPLOAD = ''")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_RANKINGS ADD RANK_ALLOW_UPLOADS int")
					Database.Write("UPDATE " & Database.DBPrefix & "_RANKINGS SET RANK_ALLOW_UPLOADS = 0")
					if (Database.DBType = "MySQL") then
						Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES MODIFY COLUMN PAGE_NAME varchar(100)")
					else
						Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ALTER COLUMN PAGE_NAME varchar(100)")
					end if
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_TITLE varchar(100)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_SHOWTITLE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_COLUMNS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_ALIGN int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_STATUS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_THUMBNAIL varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_PHOTO varchar(50)")
					Database.Write("UPDATE " & Database.DBPrefix & "_PAGES SET PAGE_SUB_TITLE = 'Sub-Categories', PAGE_SUB_SHOWTITLE = 0, PAGE_SUB_COLUMNS = 1, PAGE_SUB_ALIGN = 1, PAGE_SUB_STATUS = 1, PAGE_THUMBNAIL = '', PAGE_PHOTO = ''")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_FOLDERS(FOLDER_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", FOLDER_CORE int, FOLDER_PARENT int, FOLDER_NAME varchar(100))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_FILES(FILE_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", FILE_CORE int, FILE_FOLDER int, FILE_NAME varchar(100))")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'avatars')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'customavatars')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'documents')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'forumimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'images')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'memberphotos')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'pageimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'photogalleries')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'rankimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'rotatorimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'topicfiles')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGAdminCode.html')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGCode.html')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGHtmlForms.html')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGMainMenu.html')")
					Reader = Database.Read("SELECT FOLDER_ID, FOLDER_NAME FROM " & Database.DBPrefix & "_FOLDERS ORDER BY FOLDER_NAME")
					While Reader.Read()
						if (Reader("FOLDER_NAME").ToString() = "avatars") then
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'default.gif')")
							Dim AvatarReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'avatars/%'")
							While AvatarReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (AvatarReader("UPLOAD_NAME").ToString()).Replace("avatars/", "") & "')")
							End While
							AvatarReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "customavatars") then
							Dim CustomAvatarReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'customavatars/%'")
							While CustomAvatarReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (CustomAvatarReader("UPLOAD_NAME").ToString()).Replace("customavatars/", "") & "')")
							End While
							CustomAvatarReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "documents") then
							Dim DocumentReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'documents/%'")
							While DocumentReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (DocumentReader("UPLOAD_NAME").ToString()).Replace("documents/", "") & "')")
							End While
							DocumentReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "forumimages") then
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'folder.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'folder_home.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'folder_up.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_aol.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_icq.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_msn.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_yahoo.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'latestcomments.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'lock.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'page_icon.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'pm_inbox.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'pm_new.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'rss.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'icons')")
							Dim IconReader as OdbcDataReader = Database.Read("SELECT FOLDER_ID FROM " & Database.DBPrefix & "_FOLDERS WHERE FOLDER_NAME = 'icons' ORDER BY FOLDER_ID DESC", 1)
							While IconReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'bz2.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'conf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'css.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'csv.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'doc.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'file.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'gz.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'html.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'jpg.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'js.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'mov.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'mp3.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'mpg.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'pdf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'php.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'ppt.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'rar.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'rtf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'swf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'tar.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'tgz.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'txt.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'wav.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'wma.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'wmv.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'xls.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'xml.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'zip.png')")
							End While
							IconReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "images") then
							Dim ImagesReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'images/%'")
							While ImagesReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (ImagesReader("UPLOAD_NAME").ToString()).Replace("images/", "") & "')")
							End While
							ImagesReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "memberphotos") then
							Dim MemberPhotosReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'memberphotos/%'")
							While MemberPhotosReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (MemberPhotosReader("UPLOAD_NAME").ToString()).Replace("memberphotos/", "") & "')")
							End While
							MemberPhotosReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "pageimages") then
							Dim PageImagesReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'pageimages/%'")
							While PageImagesReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (PageImagesReader("UPLOAD_NAME").ToString()).Replace("pageimages/", "") & "')")
							End While
							PageImagesReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "photogalleries") then
							Dim GalleryReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'photogalleries/%'")
							While GalleryReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (GalleryReader("UPLOAD_NAME").ToString()).Replace("photogalleries/", "") & "')")
							End While
							GalleryReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "rankimages") then
							Dim RankReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'rankimages/%'")
							if (RankReader.HasRows()) then
								While RankReader.Read()
									Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (RankReader("UPLOAD_NAME").ToString()).Replace("rankimages/", "") & "')")
								End While
							else
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_0.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_1.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_2.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_3.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_4.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_5.gif')")
							end if
							RankReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "rotatorimages") then
							Dim RotatorReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'rotatorimages/%'")
							While RotatorReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (RotatorReader("UPLOAD_NAME").ToString()).Replace("rotatorimages/", "") & "')")
							End While
							RotatorReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "topicfiles") then
							Dim TopicFileReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'topicfiles/%'")
							While TopicFileReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (TopicFileReader("UPLOAD_NAME").ToString()).Replace("topicfiles/", "") & "')")
							End While
							TopicFileReader.Close()
						end if
					End While
					Reader.Close()
				elseif (sender.CommandArgument = "3.0") then
					'------------  3.1 Additions
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PM_TOPICS ADD TOPIC_SHOWRECEIVER int")
					Database.Write("UPDATE " & Database.DBPrefix & "_PM_TOPICS SET TOPIC_SHOWRECEIVER = 1")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SECURITY int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_PASSWORD varchar(100)")
					Database.Write("UPDATE " & Database.DBPrefix & "_PAGES SET PAGE_SECURITY = 0, PAGE_PASSWORD = ''")
					Database.Write("UPDATE " & Database.DBPrefix & "_PAGES SET PAGE_PARENT = 0 WHERE PAGE_PARENT = -1")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_PAGES_PRIVILEGED(PRIVILEGED_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", MEMBER_ID int, PAGE_ID int, PRIVILEGED_ACCESS int)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_PHOTO_SIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_AVATAR_SIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SEARCH_TOPICS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SEARCH_MEMBERS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SEARCH_BLOGS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_SEARCH_PAGES int")
					Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_PHOTO_SIZE = 775, " & Database.DBPrefix & "_AVATAR_SIZE = 125, " & Database.DBPrefix & "_SEARCH_TOPICS = 1, " & Database.DBPrefix & "_SEARCH_MEMBERS = 1, " & Database.DBPrefix & "_SEARCH_BLOGS = 1, " & Database.DBPrefix & "_SEARCH_PAGES = 1")
					'------------  3.2 Additions
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CUSTOM_MESSAGES ADD EMAIL_WELCOMEMESSAGE text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CUSTOM_MESSAGES ADD MESSAGE_PRIVACYNOTICE text")
					Database.Write("UPDATE " & Database.DBPrefix & "_CUSTOM_MESSAGES SET EMAIL_WELCOMEMESSAGE = 'Thank you for registering at our site.  You may now log in and begin using your account.<br /><br />To complete your profile or to add more information, log in and click the User CP button.', MESSAGE_PRIVACYNOTICE = 'A unique username and e-mail address are required to access the forums.  All information shared in your member profile is available to the public and you should exercise caution before posting personal data.  The administrators and moderators of this web site are not responsible for the privacy of any user.<br /><br />Your browser must have cookies enabled in order to use the forums.  You will not be able to log in after registering without allowing the forum to store your user data in cookie files.<br /><br />We are not responsible for any materials posted on this site by its members.  We will, however, attempt to remove materials that we feel are inappropriate.  If you see posts or information in these forums with which you think is indecent, you are encouraged to contact us and let us know about it.<br /><br />By pressing the ""Agree"" button, you agree that you are 13 years of age or over.  You also agree that you will not post any copyrighted material that is not owned by yourself or the owners of these forums. In your use of these forums, you agree that you will not post any information which is vulgar, harassing, hateful, threatening, invading of others privacy, sexually oriented, or violates any laws.'")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_MEMBER_FILETYPES text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_TOPIC_UPLOADSIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_TOPICS_FONTSIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_TOPICS_FONTCOLOR varchar(20)")
					Reader = Database.Read("SELECT " & Database.DBPrefix & "_FONTSIZE, " & Database.DBPrefix & "_FONT_COLOR FROM " & Database.DBPrefix & "_SETTINGS", 1)
					While Reader.Read()
						Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_MEMBER_FILETYPES = 'image/gif, image/png, application/msword, image/jpeg, application/zip, text/plain, image/pjpeg, application/mspowerpoint, application/vnd.ms-excel, application/pdf', " & Database.DBPrefix & "_TOPIC_UPLOADSIZE = 1024, " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE = 0, " & Database.DBPrefix & "_TOPICS_FONTSIZE = " & Reader(Database.DBPrefix & "_FONTSIZE") & ", " & Database.DBPrefix & "_TOPICS_FONTCOLOR = '" & Reader(Database.DBPrefix & "_FONT_COLOR").ToString() & "'")
					End While
					Reader.Close()
					Reader = Database.Read("SELECT ID, " & Database.DBPrefix & "_CUSTOM_CSS FROM " & Database.DBPrefix & "_SETTINGS")
					While Reader.Read()
						Dim TheCustomCSS as String = Reader(Database.DBPrefix & "_CUSTOM_CSS").ToString()
						TheCustomCSS = TheCustomCSS.Replace(".MessageBox{", ".AdminButtons{font-size: 18px; width: 270px; border-color: [HeaderColor];}" & CHR(10) & ".MessageBox{")
						Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_CUSTOM_CSS = '" & TheCustomCSS & "' WHERE ID = " & Reader("ID"))
					End While
					Reader.Close()
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_TOPICS ADD TOPIC_FILEUPLOAD varchar(100)")
					Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_FILEUPLOAD = ''")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_RANKINGS ADD RANK_ALLOW_UPLOADS int")

					Database.Write("UPDATE " & Database.DBPrefix & "_RANKINGS SET RANK_ALLOW_UPLOADS = 0")
					if (Database.DBType = "MySQL") then
						Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES MODIFY COLUMN PAGE_NAME varchar(100)")
					else
						Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ALTER COLUMN PAGE_NAME varchar(100)")
					end if
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_TITLE varchar(100)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_SHOWTITLE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_COLUMNS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_ALIGN int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_STATUS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_THUMBNAIL varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_PHOTO varchar(50)")
					Database.Write("UPDATE " & Database.DBPrefix & "_PAGES SET PAGE_SUB_TITLE = 'Sub-Categories', PAGE_SUB_SHOWTITLE = 0, PAGE_SUB_COLUMNS = 1, PAGE_SUB_ALIGN = 1, PAGE_SUB_STATUS = 1, PAGE_THUMBNAIL = '', PAGE_PHOTO = ''")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_FOLDERS(FOLDER_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", FOLDER_CORE int, FOLDER_PARENT int, FOLDER_NAME varchar(100))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_FILES(FILE_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", FILE_CORE int, FILE_FOLDER int, FILE_NAME varchar(100))")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'avatars')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'customavatars')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'documents')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'forumimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'images')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'memberphotos')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'pageimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'photogalleries')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'rankimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'rotatorimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'topicfiles')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGAdminCode.html')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGCode.html')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGHtmlForms.html')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGMainMenu.html')")
					Reader = Database.Read("SELECT FOLDER_ID, FOLDER_NAME FROM " & Database.DBPrefix & "_FOLDERS ORDER BY FOLDER_NAME")
					While Reader.Read()
						if (Reader("FOLDER_NAME").ToString() = "avatars") then
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'default.gif')")
							Dim AvatarReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'avatars/%'")
							While AvatarReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (AvatarReader("UPLOAD_NAME").ToString()).Replace("avatars/", "") & "')")
							End While
							AvatarReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "customavatars") then
							Dim CustomAvatarReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'customavatars/%'")
							While CustomAvatarReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (CustomAvatarReader("UPLOAD_NAME").ToString()).Replace("customavatars/", "") & "')")
							End While
							CustomAvatarReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "documents") then
							Dim DocumentReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'documents/%'")
							While DocumentReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (DocumentReader("UPLOAD_NAME").ToString()).Replace("documents/", "") & "')")
							End While
							DocumentReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "forumimages") then
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'folder.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'folder_home.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'folder_up.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_aol.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_icq.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_msn.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_yahoo.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'latestcomments.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'lock.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'page_icon.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'pm_inbox.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'pm_new.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'rss.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'icons')")
							Dim IconReader as OdbcDataReader = Database.Read("SELECT FOLDER_ID FROM " & Database.DBPrefix & "_FOLDERS WHERE FOLDER_NAME = 'icons' ORDER BY FOLDER_ID DESC", 1)
							While IconReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'bz2.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'conf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'css.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'csv.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'doc.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'file.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'gz.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'html.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'jpg.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'js.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'mov.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'mp3.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'mpg.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'pdf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'php.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'ppt.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'rar.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'rtf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'swf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'tar.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'tgz.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'txt.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'wav.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'wma.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'wmv.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'xls.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'xml.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'zip.png')")
							End While
							IconReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "images") then
							Dim ImagesReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'images/%'")
							While ImagesReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (ImagesReader("UPLOAD_NAME").ToString()).Replace("images/", "") & "')")
							End While
							ImagesReader.Close()
						end if

						if (Reader("FOLDER_NAME").ToString() = "memberphotos") then
							Dim MemberPhotosReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'memberphotos/%'")
							While MemberPhotosReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (MemberPhotosReader("UPLOAD_NAME").ToString()).Replace("memberphotos/", "") & "')")
							End While
							MemberPhotosReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "pageimages") then
							Dim PageImagesReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'pageimages/%'")
							While PageImagesReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (PageImagesReader("UPLOAD_NAME").ToString()).Replace("pageimages/", "") & "')")
							End While
							PageImagesReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "photogalleries") then
							Dim GalleryReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'photogalleries/%'")
							While GalleryReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (GalleryReader("UPLOAD_NAME").ToString()).Replace("photogalleries/", "") & "')")
							End While
							GalleryReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "rankimages") then
							Dim RankReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'rankimages/%'")
							if (RankReader.HasRows()) then
								While RankReader.Read()
									Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (RankReader("UPLOAD_NAME").ToString()).Replace("rankimages/", "") & "')")
								End While
							else
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_0.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_1.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_2.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_3.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_4.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_5.gif')")
							end if
							RankReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "rotatorimages") then
							Dim RotatorReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'rotatorimages/%'")
							While RotatorReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (RotatorReader("UPLOAD_NAME").ToString()).Replace("rotatorimages/", "") & "')")
							End While
							RotatorReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "topicfiles") then
							Dim TopicFileReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'topicfiles/%'")
							While TopicFileReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (TopicFileReader("UPLOAD_NAME").ToString()).Replace("topicfiles/", "") & "')")
							End While
							TopicFileReader.Close()
						end if
					End While
					Reader.Close()
				elseif (sender.CommandArgument = "3.1") then
					'------------  3.2 Additions
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CUSTOM_MESSAGES ADD EMAIL_WELCOMEMESSAGE text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_CUSTOM_MESSAGES ADD MESSAGE_PRIVACYNOTICE text")
					Database.Write("UPDATE " & Database.DBPrefix & "_CUSTOM_MESSAGES SET EMAIL_WELCOMEMESSAGE = 'Thank you for registering at our site.  You may now log in and begin using your account.<br /><br />To complete your profile or to add more information, log in and click the User CP button.', MESSAGE_PRIVACYNOTICE = 'A unique username and e-mail address are required to access the forums.  All information shared in your member profile is available to the public and you should exercise caution before posting personal data.  The administrators and moderators of this web site are not responsible for the privacy of any user.<br /><br />Your browser must have cookies enabled in order to use the forums.  You will not be able to log in after registering without allowing the forum to store your user data in cookie files.<br /><br />We are not responsible for any materials posted on this site by its members.  We will, however, attempt to remove materials that we feel are inappropriate.  If you see posts or information in these forums with which you think is indecent, you are encouraged to contact us and let us know about it.<br /><br />By pressing the ""Agree"" button, you agree that you are 13 years of age or over.  You also agree that you will not post any copyrighted material that is not owned by yourself or the owners of these forums. In your use of these forums, you agree that you will not post any information which is vulgar, harassing, hateful, threatening, invading of others privacy, sexually oriented, or violates any laws.'")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_MEMBER_FILETYPES text")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_TOPIC_UPLOADSIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_TOPICS_FONTSIZE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_SETTINGS ADD " & Database.DBPrefix & "_TOPICS_FONTCOLOR varchar(20)")
					Reader = Database.Read("SELECT " & Database.DBPrefix & "_FONTSIZE, " & Database.DBPrefix & "_FONT_COLOR FROM " & Database.DBPrefix & "_SETTINGS", 1)
					While Reader.Read()
						Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_MEMBER_FILETYPES = 'image/gif, image/png, application/msword, image/jpeg, application/zip, text/plain, image/pjpeg, application/mspowerpoint, application/vnd.ms-excel, application/pdf', " & Database.DBPrefix & "_TOPIC_UPLOADSIZE = 1024, " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE = 0, " & Database.DBPrefix & "_TOPICS_FONTSIZE = " & Reader(Database.DBPrefix & "_FONTSIZE") & ", " & Database.DBPrefix & "_TOPICS_FONTCOLOR = '" & Reader(Database.DBPrefix & "_FONT_COLOR").ToString() & "'")
					End While
					Reader.Close()
					Reader = Database.Read("SELECT ID, " & Database.DBPrefix & "_CUSTOM_CSS FROM " & Database.DBPrefix & "_SETTINGS")
					While Reader.Read()
						Dim TheCustomCSS as String = Reader(Database.DBPrefix & "_CUSTOM_CSS").ToString()
						TheCustomCSS = TheCustomCSS.Replace(".MessageBox{", ".AdminButtons{font-size: 18px; width: 270px; border-color: [HeaderColor];}" & CHR(10) & ".MessageBox{")
						Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_CUSTOM_CSS = '" & TheCustomCSS & "' WHERE ID = " & Reader("ID"))
					End While
					Reader.Close()
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_TOPICS ADD TOPIC_FILEUPLOAD varchar(100)")
					Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_FILEUPLOAD = ''")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_RANKINGS ADD RANK_ALLOW_UPLOADS int")
					Database.Write("UPDATE " & Database.DBPrefix & "_RANKINGS SET RANK_ALLOW_UPLOADS = 0")
					if (Database.DBType = "MySQL") then
						Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES MODIFY COLUMN PAGE_NAME varchar(100)")
					else
						Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ALTER COLUMN PAGE_NAME varchar(100)")
					end if
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_TITLE varchar(100)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_SHOWTITLE int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_COLUMNS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_ALIGN int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_SUB_STATUS int")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_THUMBNAIL varchar(50)")
					Database.Write("ALTER TABLE " & Database.DBPrefix & "_PAGES ADD PAGE_PHOTO varchar(50)")
					Database.Write("UPDATE " & Database.DBPrefix & "_PAGES SET PAGE_SUB_TITLE = 'Sub-Categories', PAGE_SUB_SHOWTITLE = 0, PAGE_SUB_COLUMNS = 1, PAGE_SUB_ALIGN = 1, PAGE_SUB_STATUS = 1, PAGE_THUMBNAIL = '', PAGE_PHOTO = ''")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_FOLDERS(FOLDER_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", FOLDER_CORE int, FOLDER_PARENT int, FOLDER_NAME varchar(100))")
					Database.Write("CREATE TABLE " & Database.DBPrefix & "_FILES(FILE_ID int PRIMARY KEY " & Database.GetAutoIncrement() & ", FILE_CORE int, FILE_FOLDER int, FILE_NAME varchar(100))")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'avatars')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'customavatars')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'documents')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'forumimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'images')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'memberphotos')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'pageimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'photogalleries')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'rankimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'rotatorimages')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, 0, 'topicfiles')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGAdminCode.html')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGCode.html')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGHtmlForms.html')")
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, 0, 'DMGMainMenu.html')")
					Reader = Database.Read("SELECT FOLDER_ID, FOLDER_NAME FROM " & Database.DBPrefix & "_FOLDERS ORDER BY FOLDER_NAME")
					While Reader.Read()
						if (Reader("FOLDER_NAME").ToString() = "avatars") then
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'default.gif')")
							Dim AvatarReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'avatars/%'")
							While AvatarReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (AvatarReader("UPLOAD_NAME").ToString()).Replace("avatars/", "") & "')")
							End While
							AvatarReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "customavatars") then
							Dim CustomAvatarReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'customavatars/%'")
							While CustomAvatarReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (CustomAvatarReader("UPLOAD_NAME").ToString()).Replace("customavatars/", "") & "')")
							End While
							CustomAvatarReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "documents") then
							Dim DocumentReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'documents/%'")
							While DocumentReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (DocumentReader("UPLOAD_NAME").ToString()).Replace("documents/", "") & "')")
							End While
							DocumentReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "forumimages") then
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'folder.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'folder_home.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'folder_up.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_aol.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_icq.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_msn.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'im_yahoo.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'latestcomments.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'lock.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'page_icon.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'pm_inbox.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'pm_new.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'rss.gif')")
							Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_CORE, FOLDER_PARENT, FOLDER_NAME) VALUES (1, " & Reader("FOLDER_ID") & ", 'icons')")
							Dim IconReader as OdbcDataReader = Database.Read("SELECT FOLDER_ID FROM " & Database.DBPrefix & "_FOLDERS WHERE FOLDER_NAME = 'icons' ORDER BY FOLDER_ID DESC", 1)
							While IconReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'bz2.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'conf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'css.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'csv.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'doc.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'file.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'gz.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'html.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'jpg.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'js.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'mov.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'mp3.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'mpg.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'pdf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'php.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'ppt.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'rar.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'rtf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'swf.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'tar.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'tgz.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'txt.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'wav.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'wma.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'wmv.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'xls.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'xml.png')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (1, " & IconReader("FOLDER_ID") & ", 'zip.png')")
							End While
							IconReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "images") then
							Dim ImagesReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'images/%'")
							While ImagesReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (ImagesReader("UPLOAD_NAME").ToString()).Replace("images/", "") & "')")
							End While
							ImagesReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "memberphotos") then
							Dim MemberPhotosReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'memberphotos/%'")
							While MemberPhotosReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (MemberPhotosReader("UPLOAD_NAME").ToString()).Replace("memberphotos/", "") & "')")
							End While
							MemberPhotosReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "pageimages") then
							Dim PageImagesReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'pageimages/%'")
							While PageImagesReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (PageImagesReader("UPLOAD_NAME").ToString()).Replace("pageimages/", "") & "')")
							End While
							PageImagesReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "photogalleries") then
							Dim GalleryReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'photogalleries/%'")
							While GalleryReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (GalleryReader("UPLOAD_NAME").ToString()).Replace("photogalleries/", "") & "')")
							End While
							GalleryReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "rankimages") then
							Dim RankReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'rankimages/%'")
							if (RankReader.HasRows()) then
								While RankReader.Read()
									Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (RankReader("UPLOAD_NAME").ToString()).Replace("rankimages/", "") & "')")
								End While
							else
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_0.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_1.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_2.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_3.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_4.gif')")
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", 'rank_5.gif')")
							end if
							RankReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "rotatorimages") then
							Dim RotatorReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'rotatorimages/%'")
							While RotatorReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (RotatorReader("UPLOAD_NAME").ToString()).Replace("rotatorimages/", "") & "')")
							End While
							RotatorReader.Close()
						end if
						if (Reader("FOLDER_NAME").ToString() = "topicfiles") then
							Dim TopicFileReader as OdbcDataReader = Database.Read("SELECT UPLOAD_NAME FROM " & Database.DBPrefix & "_UPLOADS WHERE UPLOAD_NAME LIKE 'topicfiles/%'")
							While TopicFileReader.Read()
								Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_CORE, FILE_FOLDER, FILE_NAME) VALUES (0, " & Reader("FOLDER_ID") & ", '" & (TopicFileReader("UPLOAD_NAME").ToString()).Replace("topicfiles/", "") & "')")
							End While
							TopicFileReader.Close()
						end if
					End While
					Reader.Close()
				else
					Message.text = "Your database has already been upgraded to version 3.2."
				end if
			Catch e1 as System.Data.Odbc.OdbcException
				Message.text = "<b>ERROR:</b> There was a SQL error during setup.  View the error message below and then <a href=""setup.aspx"">Click Here</a> to try again.<br /><br />" & e1.ToString()
			Catch e2 as Exception
				Message.text = "<b>ERROR:</b> There was an error during setup.  View the error message below and then <a href=""setup.aspx"">Click Here</a> to try again.<br /><br />" & e2.ToString()
			End Try
		End Sub

	End Class

End Namespace
