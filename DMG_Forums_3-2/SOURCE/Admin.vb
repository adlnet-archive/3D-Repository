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
Imports System.IO
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports DMGForums.Global

Namespace DMGForums.Admin

	'---------------------------------------------------------------------------------------------------
	' AdminPage - Codebehind For admin.aspx
	'---------------------------------------------------------------------------------------------------
	Public Class AdminPage
		Inherits System.Web.UI.Page

		Public txtDMGTitle As System.Web.UI.WebControls.TextBox
		Public txtDMGCopyright As System.Web.UI.WebControls.TextBox
		Public txtDMGLogo As System.Web.UI.WebControls.TextBox
		Public txtDMGURL As System.Web.UI.WebControls.TextBox
		Public txtDMGItemsPerPage As System.Web.UI.WebControls.TextBox
		Public txtDMGSpamFilter As System.Web.UI.WebControls.TextBox
		Public txtDMGFontFace As System.Web.UI.WebControls.TextBox
		Public txtDMGFontSize As System.Web.UI.WebControls.TextBox
		Public txtDMGButtonColor As System.Web.UI.WebControls.TextBox
		Public txtDMGLoginFontColor As System.Web.UI.WebControls.TextBox
		Public txtDMGHeaderSize As System.Web.UI.WebControls.TextBox
		Public txtDMGHeaderColor As System.Web.UI.WebControls.TextBox
		Public txtDMGHeaderFontColor As System.Web.UI.WebControls.TextBox
		Public txtDMGSubheaderColor As System.Web.UI.WebControls.TextBox
		Public txtDMGSubheaderFontColor As System.Web.UI.WebControls.TextBox
		Public txtDMGFooterSize As System.Web.UI.WebControls.TextBox
		Public txtDMGFooterColor As System.Web.UI.WebControls.TextBox
		Public txtDMGFooterFontColor As System.Web.UI.WebControls.TextBox
		Public txtDMGBGColor As System.Web.UI.WebControls.TextBox
		Public txtDMGBGImage As System.Web.UI.WebControls.TextBox
		Public txtDMGBGImage2 As System.Web.UI.WebControls.TextBox
		Public txtDMGFontColor As System.Web.UI.WebControls.TextBox
		Public txtDMGLinkColor As System.Web.UI.WebControls.TextBox
		Public txtDMGLinkDecoration As System.Web.UI.WebControls.TextBox
		Public txtDMGLinkVisitedColor As System.Web.UI.WebControls.TextBox
		Public txtDMGLinkVisitedDecoration As System.Web.UI.WebControls.TextBox
		Public txtDMGLinkActiveColor As System.Web.UI.WebControls.TextBox
		Public txtDMGLinkActiveDecoration As System.Web.UI.WebControls.TextBox
		Public txtDMGLinkHoverColor As System.Web.UI.WebControls.TextBox
		Public txtDMGLinkHoverDecoration As System.Web.UI.WebControls.TextBox
		Public txtDMGTopicsFontSize As System.Web.UI.WebControls.TextBox
		Public txtDMGTopicsFontColor As System.Web.UI.WebControls.TextBox
		Public txtDMGTopicsBGColor1 As System.Web.UI.WebControls.TextBox
		Public txtDMGTopicsBGColor2 As System.Web.UI.WebControls.TextBox
		Public txtDMGTableborderColor As System.Web.UI.WebControls.TextBox
		Public txtDMGScrollbarColor As System.Web.UI.WebControls.TextBox
		Public txtDMGCustomHeader As System.Web.UI.WebControls.TextBox
		Public txtDMGCustomFooter As System.Web.UI.WebControls.TextBox
		Public txtDMGCustomCSS As System.Web.UI.WebControls.TextBox
		Public txtDMGCustomMeta As System.Web.UI.WebControls.TextBox
		Public txtDMGMarginSide As System.Web.UI.WebControls.TextBox
		Public txtDMGMarginTop As System.Web.UI.WebControls.TextBox

		Public txtDMGNewTemplateName As System.Web.UI.WebControls.TextBox
		Public txtDMGNewFontFace As System.Web.UI.WebControls.TextBox
		Public txtDMGNewFontSize As System.Web.UI.WebControls.TextBox
		Public txtDMGNewButtonColor As System.Web.UI.WebControls.TextBox
		Public txtDMGNewLoginFontColor As System.Web.UI.WebControls.TextBox
		Public txtDMGNewHeaderSize As System.Web.UI.WebControls.TextBox
		Public txtDMGNewHeaderColor As System.Web.UI.WebControls.TextBox
		Public txtDMGNewHeaderFontColor As System.Web.UI.WebControls.TextBox
		Public txtDMGNewSubheaderColor As System.Web.UI.WebControls.TextBox
		Public txtDMGNewSubheaderFontColor As System.Web.UI.WebControls.TextBox
		Public txtDMGNewFooterSize As System.Web.UI.WebControls.TextBox
		Public txtDMGNewFooterColor As System.Web.UI.WebControls.TextBox
		Public txtDMGNewFooterFontColor As System.Web.UI.WebControls.TextBox
		Public txtDMGNewBGColor As System.Web.UI.WebControls.TextBox
		Public txtDMGNewBGImage As System.Web.UI.WebControls.TextBox
		Public txtDMGNewFontColor As System.Web.UI.WebControls.TextBox
		Public txtDMGNewLinkColor As System.Web.UI.WebControls.TextBox
		Public txtDMGNewLinkDecoration As System.Web.UI.WebControls.TextBox
		Public txtDMGNewLinkVisitedColor As System.Web.UI.WebControls.TextBox
		Public txtDMGNewLinkVisitedDecoration As System.Web.UI.WebControls.TextBox
		Public txtDMGNewLinkActiveColor As System.Web.UI.WebControls.TextBox
		Public txtDMGNewLinkActiveDecoration As System.Web.UI.WebControls.TextBox
		Public txtDMGNewLinkHoverColor As System.Web.UI.WebControls.TextBox
		Public txtDMGNewLinkHoverDecoration As System.Web.UI.WebControls.TextBox
		Public txtDMGNewTopicsFontSize As System.Web.UI.WebControls.TextBox
		Public txtDMGNewTopicsFontColor As System.Web.UI.WebControls.TextBox
		Public txtDMGNewTopicsBGColor1 As System.Web.UI.WebControls.TextBox
		Public txtDMGNewTopicsBGColor2 As System.Web.UI.WebControls.TextBox
		Public txtDMGNewTableborderColor As System.Web.UI.WebControls.TextBox
		Public txtDMGNewScrollbarColor As System.Web.UI.WebControls.TextBox
		Public txtDMGNewCustomHeader As System.Web.UI.WebControls.TextBox
		Public txtDMGNewCustomFooter As System.Web.UI.WebControls.TextBox
		Public txtDMGNewCustomCSS As System.Web.UI.WebControls.TextBox
		Public txtDMGNewMarginSide As System.Web.UI.WebControls.TextBox
		Public txtDMGNewMarginTop As System.Web.UI.WebControls.TextBox

		Public txtDMGVar1 As System.Web.UI.WebControls.TextBox
		Public txtDMGVar2 As System.Web.UI.WebControls.TextBox
		Public txtDMGVar3 As System.Web.UI.WebControls.TextBox
		Public txtDMGVar4 As System.Web.UI.WebControls.TextBox
		Public txtDMGVar5 As System.Web.UI.WebControls.TextBox
		Public txtDMGText1 As System.Web.UI.WebControls.TextBox
		Public txtDMGText2 As System.Web.UI.WebControls.TextBox
		Public txtDMGText3 As System.Web.UI.WebControls.TextBox
		Public txtDMGText4 As System.Web.UI.WebControls.TextBox
		Public txtDMGText5 As System.Web.UI.WebControls.TextBox

		Public txtTemplateID As System.Web.UI.WebControls.TextBox
		Public CategoryList As System.Web.UI.WebControls.DropDownList
		Public ForumList As System.Web.UI.WebControls.DropDownList
		Public ForumCategoryList As System.Web.UI.WebControls.DropDownList
		Public MemberList As System.Web.UI.WebControls.Repeater
		Public RankList As System.Web.UI.WebControls.Repeater
		Public Avatars As System.Web.UI.WebControls.DataList
		Public AdminLinks As System.Web.UI.WebControls.Panel
		Public AdminMainConfig As System.Web.UI.WebControls.Panel
		Public AdminForumMainConfig As System.Web.UI.WebControls.Panel
		Public AdminVariableConfig As System.Web.UI.WebControls.Panel
		Public AdminColorConfig As System.Web.UI.WebControls.Panel
		Public AdminCustomHTML As System.Web.UI.WebControls.Panel
		Public AdminEmailSettings As System.Web.UI.WebControls.Panel
		Public AdminEditCategories As System.Web.UI.WebControls.Panel
		Public AdminEditForums As System.Web.UI.WebControls.Panel
		Public AdminEditMembers As System.Web.UI.WebControls.Panel
		Public AdminVerifyMembers As System.Web.UI.WebControls.Panel
		Public AdminVerifyMembersConfirm As System.Web.UI.WebControls.Panel
		Public AdminAvatarConfig As System.Web.UI.WebControls.Panel
		Public AdminRankingConfig As System.Web.UI.WebControls.Panel
		Public AdminNewTemplate As System.Web.UI.WebControls.Panel
		Public AdminDeleteTemplatePanel As System.Web.UI.WebControls.Panel
		Public AdminCurseFilter As System.Web.UI.WebControls.Panel
		Public AdminMainMenuConfig As System.Web.UI.WebControls.Panel
		Public ColorSettingsPanel As System.Web.UI.WebControls.Panel
		Public ColorSubmit As System.Web.UI.WebControls.Button
		Public ColorSettingsDropdown As System.Web.UI.WebControls.DropDownList
		Public CustomHTMLSettingsPanel As System.Web.UI.WebControls.Panel
		Public CustomHTMLSubmit As System.Web.UI.WebControls.Button
		Public CustomHTMLSettingsDropdown As System.Web.UI.WebControls.DropDownList
		Public txtSetDefaultTemplate As System.Web.UI.WebControls.DropDownList
		Public txtDMGTemplateName As System.Web.UI.WebControls.TextBox
		Public txtSetDefaultTemplate2 As System.Web.UI.WebControls.DropDownList
		Public txtNewSetDefaultTemplate As System.Web.UI.WebControls.DropDownList
		Public txtDMGTemplateName2 As System.Web.UI.WebControls.TextBox
		Public txtDMGLayoutTemplate As System.Web.UI.WebControls.DropDownList
		Public txtDMGSetDefaultPage As System.Web.UI.WebControls.DropDownList
		Public txtDMGShowStatistics As System.Web.UI.WebControls.DropDownList
		Public txtDMGQuickRegistration As System.Web.UI.WebControls.DropDownList
		Public txtDMGCurseFilter As System.Web.UI.WebControls.DropDownList
		Public txtDMGRSSFeeds As System.Web.UI.WebControls.DropDownList
		Public txtDMGAllowSub As System.Web.UI.WebControls.DropDownList
		Public txtDMGAllowEdits As System.Web.UI.WebControls.DropDownList
		Public txtDMGAllowRegistration As System.Web.UI.WebControls.DropDownList
		Public txtDMGAllowMedia As System.Web.UI.WebControls.DropDownList
		Public txtDMGAllowReporting As System.Web.UI.WebControls.DropDownList
		Public txtDMGHideMembers As System.Web.UI.WebControls.DropDownList
		Public txtDMGHideLogin As System.Web.UI.WebControls.DropDownList
		Public txtDMGMemberValidation As System.Web.UI.WebControls.DropDownList
		Public txtDMGEnableEmailWelcomeMessage As System.Web.UI.WebControls.DropDownList
		Public txtDMGMemberPhotoSize As System.Web.UI.WebControls.TextBox
		Public txtDMGMemberFileTypes As System.Web.UI.WebControls.TextBox
		Public txtDMGTopicUploadSize As System.Web.UI.WebControls.TextBox
		Public txtDMGHtmlTitle As System.Web.UI.WebControls.TextBox
		Public txtDMGThumbnailSize As System.Web.UI.WebControls.TextBox
		Public txtDMGPhotoSize As System.Web.UI.WebControls.TextBox
		Public txtDMGAvatarSize As System.Web.UI.WebControls.TextBox
		Public TemplatesList As System.Web.UI.WebControls.Repeater
		Public VerificationList As System.Web.UI.WebControls.Repeater
		Public MemberSearch As System.Web.UI.WebControls.TextBox

		Public txtDMGEmailSmtp As System.Web.UI.WebControls.TextBox
		Public txtDMGEmailPort As System.Web.UI.WebControls.TextBox
		Public txtDMGEmailUsername As System.Web.UI.WebControls.TextBox
		Public txtDMGEmailPassword As System.Web.UI.WebControls.TextBox
		Public txtDMGEmailAddress As System.Web.UI.WebControls.TextBox
		Public txtDMGEmailAllowSend As System.Web.UI.WebControls.DropDownList
		Public txtDMGEmailAllowSub As System.Web.UI.WebControls.DropDownList

		Public VerifyMembersButton As System.Web.UI.WebControls.Button
		Public VerifyYesButton As System.Web.UI.WebControls.Button
		Public VerifyNoButton As System.Web.UI.WebControls.Button

		Public CurseList As System.Web.UI.WebControls.Repeater
		Public NewCurse As System.Web.UI.WebControls.TextBox
		Public NewCurseReplacement As System.Web.UI.WebControls.TextBox

		Public AdminRotatorConfig As System.Web.UI.WebControls.Panel
		Public AdminRotatorNew As System.Web.UI.WebControls.Panel
		Public AdminRotatorNewConfirm As System.Web.UI.WebControls.Panel
		Public AdminRotatorDelete As System.Web.UI.WebControls.Panel
		Public AdminRotatorEdit As System.Web.UI.WebControls.Panel
		Public RotatorList As System.Web.UI.WebControls.Repeater
		Public RotatorImages As System.Web.UI.WebControls.Repeater
		Public NewRotatorName As System.Web.UI.WebControls.TextBox

		Public RotatorRefresh As System.Web.UI.WebControls.LinkButton
		Public RotatorYesButton As System.Web.UI.WebControls.Button
		Public NewRotatorEdit As System.Web.UI.WebControls.Button
		Public RotatorID As String = "0"

		Public AdminGalleryConfig As System.Web.UI.WebControls.Panel
		Public AdminGalleryNew As System.Web.UI.WebControls.Panel
		Public AdminGalleryNewConfirm As System.Web.UI.WebControls.Panel
		Public AdminGalleryDelete As System.Web.UI.WebControls.Panel
		Public AdminGalleryEdit As System.Web.UI.WebControls.Panel
		Public GalleryList As System.Web.UI.WebControls.Repeater
		Public GalleryPhotos As System.Web.UI.WebControls.DataList
		Public NewGalleryName As System.Web.UI.WebControls.TextBox
		Public GalleryRefresh As System.Web.UI.WebControls.LinkButton
		Public GalleryYesButton As System.Web.UI.WebControls.Button
		Public NewGalleryEdit As System.Web.UI.WebControls.Button
		Public GalleryID As String = "0"

		Public AdminHtmlFormConfig As System.Web.UI.WebControls.Panel
		Public AdminHtmlFormView As System.Web.UI.WebControls.Panel
		Public AdminHtmlFormDelete As System.Web.UI.WebControls.Panel
		Public HtmlFormList As System.Web.UI.WebControls.Repeater
		Public HtmlFormResults As System.Web.UI.WebControls.Repeater
		Public HtmlFormYesButton As System.Web.UI.WebControls.Button

		Public AdminBannedIPConfig As System.Web.UI.WebControls.Panel
		Public IPList As System.Web.UI.WebControls.Repeater

		Public AdminPMCleanup As System.Web.UI.WebControls.Panel
		Public txtDMGPMCleanupDays As System.Web.UI.WebControls.Textbox

		Public AdminCustomMessagesConfig As System.Web.UI.WebControls.Panel
		Public txtEmailAdminApproval As System.Web.UI.WebControls.TextBox
		Public txtEmailSendKey As System.Web.UI.WebControls.TextBox
		Public txtEmailConfirmPost As System.Web.UI.WebControls.TextBox
		Public txtEmailSubscription As System.Web.UI.WebControls.TextBox
		Public txtEmailWelcomeMessage As System.Web.UI.WebControls.TextBox
		Public txtMessageAdminApproval As System.Web.UI.WebControls.TextBox
		Public txtMessageSendKey As System.Web.UI.WebControls.TextBox
		Public txtMessageRegistration As System.Web.UI.WebControls.TextBox
		Public txtMessageValidation As System.Web.UI.WebControls.TextBox
		Public txtMessageConfirmPost As System.Web.UI.WebControls.TextBox
		Public txtMessagePrivacyNotice As System.Web.UI.WebControls.TextBox

		Public AdminSearchConfig As System.Web.UI.WebControls.Panel
		Public txtDMGSearchTopics As System.Web.UI.WebControls.DropDownList
		Public txtDMGSearchMembers As System.Web.UI.WebControls.DropDownList
		Public txtDMGSearchBlogs As System.Web.UI.WebControls.DropDownList
		Public txtDMGSearchPages As System.Web.UI.WebControls.DropDownList

		Public MainMenuList As System.Web.UI.WebControls.Repeater
		Public NewLinkText As System.Web.UI.WebControls.TextBox
		Public NewLinkType As System.Web.UI.WebControls.DropDownList
		Public NewLinkParameter As System.Web.UI.WebControls.TextBox
		Public NewLinkWindow As System.Web.UI.WebControls.DropDownList
		Public MenuHorizDivide As System.Web.UI.WebControls.TextBox
		Public MenuVertDivide As System.Web.UI.WebControls.TextBox
		Public txtDMGHorizDivide As System.Web.UI.WebControls.TextBox
		Public txtDMGVertDivide As System.Web.UI.WebControls.TextBox

		Public NewRankName As System.Web.UI.WebControls.TextBox
		Public NewRankPosts As System.Web.UI.WebControls.TextBox
		Public NewRankAllowTopics As System.Web.UI.WebControls.DropDownList
		Public NewRankAllowAvatar As System.Web.UI.WebControls.DropDownList
		Public NewRankAllowAvatarCustom As System.Web.UI.WebControls.DropDownList
		Public NewRankAllowTitle As System.Web.UI.WebControls.DropDownList
		Public NewRankAllowUploads As System.Web.UI.WebControls.DropDownList

		Public AboutDMG As System.Web.UI.WebControls.Panel

		Public PagePanel As System.Web.UI.WebControls.Panel
		Public NoItemsDiv As System.Web.UI.HtmlControls.HtmlGenericControl

		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
			if Not Page.IsPostBack() then
				if (Session("UserLevel") <> "3") then
					PagePanel.visible = "false"
					NoItemsDiv.InnerHtml = "Access Denied<br /><br />"
				else
					AdminLinks.visible = "true"
					Dim UnVerifiedMembers as Integer = 0
					Dim Reader as OdbcDataReader = Database.Read("SELECT Count(*) as UnVerifiedMembers FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_VALIDATED = 0")
					While Reader.Read()
						UnVerifiedMembers = Reader("UnVerifiedMembers")
					End While
					Reader.Close()
					if (UnVerifiedMembers > 0) then
						VerifyMembersButton.Text = "Members Pending Validation (" & UnVerifiedMembers & ")"
					else
						VerifyMembersButton.Text = "Members Pending Validation (0)"
					end if
				end if
			end if
		End Sub

		Sub OpenConfig(sender As System.Object, e As System.EventArgs)
			if sender.CommandArgument = 1 then
				AdminLinks.visible = "false"
				AdminMainConfig.visible = "true"

				Dim LItem as ListItem
				Dim LayoutReader as OdbcDataReader = Database.Read("SELECT ID, " & Database.DBPrefix & "_TEMPLATE_NAME FROM " & Database.DBPrefix & "_SETTINGS ORDER BY ID")
				While LayoutReader.Read()
					LItem = new ListItem(LayoutReader(Database.DBPrefix & "_TEMPLATE_NAME"), LayoutReader("ID"))
					txtDMGLayoutTemplate.Items.Add(LItem)
				End While
				LayoutReader.close()

				Dim ConfigReader as OdbcDataReader = Database.Read("SELECT " & Database.DBPrefix & "_TITLE, " & Database.DBPrefix & "_COPYRIGHT, " & Database.DBPrefix & "_LOGO, " & Database.DBPrefix & "_URL, " & Database.DBPrefix & "_TEMPLATE_DEFAULT, " & Database.DBPrefix & "_FORUMS_DEFAULT, " & Database.DBPrefix & "_CUSTOM_META, " & Database.DBPrefix & "_HTML_TITLE, " & Database.DBPrefix & "_THUMBNAIL_SIZE FROM " & Database.DBPrefix & "_SETTINGS WHERE ID = 1")
					While ConfigReader.Read()
						txtDMGTitle.text = ConfigReader(Database.DBPrefix & "_TITLE").ToString()
						txtDMGCopyright.text = ConfigReader(Database.DBPrefix & "_COPYRIGHT").ToString()
						txtDMGLogo.text = ConfigReader(Database.DBPrefix & "_LOGO").ToString()
						txtDMGURL.text = ConfigReader(Database.DBPrefix & "_URL").ToString()
						txtDMGLayoutTemplate.SelectedValue = ConfigReader(Database.DBPrefix & "_TEMPLATE_DEFAULT")
						txtDMGSetDefaultPage.SelectedValue = ConfigReader(Database.DBPrefix & "_FORUMS_DEFAULT")
						txtDMGThumbnailSize.text = ConfigReader(Database.DBPrefix & "_THUMBNAIL_SIZE").ToString()
						txtDMGHtmlTitle.text = ConfigReader(Database.DBPrefix & "_HTML_TITLE").ToString()
						txtDMGCustomMeta.text = ConfigReader(Database.DBPrefix & "_CUSTOM_META").ToString()
					End While
				ConfigReader.Close()
			elseif sender.CommandArgument = 2 then
				SelectTemplate(1)				
			elseif sender.CommandArgument = 3 then
				SelectTemplate(2)
			elseif sender.CommandArgument = 4 then
				AdminLinks.visible = "false"
				AdminEditCategories.visible = "true"
				Dim LItem as ListItem = new ListItem("", 0)
				CategoryList.Items.Add(LItem)
				Dim Reader as OdbcDataReader = Database.Read("SELECT CATEGORY_ID, CATEGORY_NAME FROM " & Database.DBPrefix & "_CATEGORIES")
				While Reader.Read()
					LItem = new ListItem(Reader("CATEGORY_NAME").ToString(), READER("CATEGORY_ID"))
					CategoryList.Items.Add(LItem)
				End While
				Reader.Close()
			elseif sender.CommandArgument = 5 then
				AdminLinks.visible = "false"
				AdminEditForums.visible = "true"
				Dim LItem as ListItem = new ListItem("", 0)
				ForumList.Items.Add(LItem)
				ForumCategoryList.Items.Add(LItem)
				Dim Reader as OdbcDataReader = Database.Read("SELECT FORUM_ID, FORUM_NAME FROM " & Database.DBPrefix & "_FORUMS")
				While Reader.Read()
					LItem = new ListItem(Reader("FORUM_NAME").ToString(), READER("FORUM_ID"))
					ForumList.Items.Add(LItem)
				End While
				Reader.Close()
				Reader = Database.Read("SELECT CATEGORY_ID, CATEGORY_NAME FROM " & Database.DBPrefix & "_CATEGORIES")
				While Reader.Read()
					LItem = new ListItem(Reader("CATEGORY_NAME").ToString(), READER("CATEGORY_ID"))
					ForumCategoryList.Items.Add(LItem)
				End While
				Reader.Close()
			elseif sender.CommandArgument = 6 then
				AdminLinks.visible = "false"
				AdminEditMembers.visible = "true"
			elseif sender.CommandArgument = 7 then
				AdminLinks.visible = "false"
				AdminAvatarConfig.visible = "true"
				Avatars.Datasource = Database.Read("SELECT AVATAR_ID, AVATAR_IMAGE, AVATAR_NAME FROM " & Database.DBPrefix & "_AVATARS ORDER BY AVATAR_NAME")
				Avatars.Databind()
				Avatars.Datasource.Close()
			elseif sender.CommandArgument = 8 then
				AdminLinks.visible = "false"
				AdminRankingConfig.visible = "true"
				RankList.Datasource = Database.Read("SELECT * FROM " & Database.DBPrefix & "_RANKINGS ORDER BY RANK_POSTS")
				RankList.Databind()
				RankList.Datasource.Close()
			elseif sender.CommandArgument = 10 then
				AdminLinks.visible = "false"
				AdminVariableConfig.visible = "true"
				Dim VarReader as OdbcDataReader = Database.Read("SELECT * FROM " & Database.DBPrefix & "_VARIABLES WHERE ID = 1")
				While VarReader.Read()
					txtDMGVar1.text = VarReader("VAR1").ToString()
					txtDMGVar2.text = VarReader("VAR2").ToString()
					txtDMGVar3.text = VarReader("VAR3").ToString()
					txtDMGVar4.text = VarReader("VAR4").ToString()
					txtDMGVar5.text = VarReader("VAR5").ToString()
					txtDMGText1.text = VarReader("TEXT1").ToString()
					txtDMGText2.text = VarReader("TEXT2").ToString()
					txtDMGText3.text = VarReader("TEXT3").ToString()
					txtDMGText4.text = VarReader("TEXT4").ToString()
					txtDMGText5.text = VarReader("TEXT5").ToString()
				End While
				VarReader.Close()
			elseif sender.CommandArgument = 12 then
				AdminLinks.visible = "false"
				AdminEmailSettings.visible = "true"
				Dim ConfigReader as OdbcDataReader = Database.Read("SELECT " & Database.DBPrefix & "_EMAIL_SMTP, " & Database.DBPrefix & "_EMAIL_PORT, " & Database.DBPrefix & "_EMAIL_USERNAME, " & Database.DBPrefix & "_EMAIL_PASSWORD, " & Database.DBPrefix & "_EMAIL_ADDRESS, " & Database.DBPrefix & "_EMAIL_ALLOWSEND, " & Database.DBPrefix & "_EMAIL_ALLOWSUB FROM " & Database.DBPrefix & "_SETTINGS WHERE ID = 1")
					While ConfigReader.Read()
						txtDMGEmailSmtp.text = ConfigReader(Database.DBPrefix & "_EMAIL_SMTP").ToString()
						txtDMGEmailPort.text = ConfigReader(Database.DBPrefix & "_EMAIL_PORT").ToString()
						txtDMGEmailUsername.text = ConfigReader(Database.DBPrefix & "_EMAIL_USERNAME").ToString()
						txtDMGEmailPassword.text = ConfigReader(Database.DBPrefix & "_EMAIL_PASSWORD").ToString()
						txtDMGEmailAddress.text = ConfigReader(Database.DBPrefix & "_EMAIL_ADDRESS").ToString()
						txtDMGEmailAllowSend.SelectedValue = ConfigReader(Database.DBPrefix & "_EMAIL_ALLOWSEND")
						txtDMGEmailAllowSub.SelectedValue = ConfigReader(Database.DBPrefix & "_EMAIL_ALLOWSUB")
					End While
				ConfigReader.Close()
			elseif sender.CommandArgument = 13 then
				AdminLinks.visible = "false"
				VerificationList.Datasource = Database.Read("SELECT MEMBER_ID, MEMBER_USERNAME, MEMBER_EMAIL, MEMBER_IP_ORIGINAL, MEMBER_DATE_JOINED FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_VALIDATED = 0 ORDER BY MEMBER_USERNAME")
				VerificationList.Databind()
				if (VerificationList.Items.Count = 0) then
					PagePanel.visible = "false"
					NoItemsDiv.InnerHtml = "There Are No Unverified Members<br /><br />"
				else
					AdminVerifyMembers.visible = "True"
				end if
				VerificationList.Datasource.Close()
			elseif sender.CommandArgument = 14 then
				AdminLinks.visible = "false"
				AdminCurseFilter.visible = "true"
				CurseList.Datasource = Database.Read("SELECT CURSE_ID, CURSE_WORD, CURSE_REPLACEMENT FROM " & Database.DBPrefix & "_CURSE_FILTER ORDER BY CURSE_WORD")
				CurseList.Databind()
				CurseList.Datasource.Close()
			elseif sender.CommandArgument = 15 then
				AdminLinks.visible = "false"
				AdminRotatorConfig.visible = "true"
				AdminRotatorNew.visible = "false"
				AdminRotatorDelete.visible = "false"
				AdminRotatorEdit.visible = "false"
				AdminRotatorNewConfirm.visible = "false"
				RotatorList.Datasource = Database.Read("SELECT ROTATOR_ID, ROTATOR_NAME FROM " & Database.DBPrefix & "_ROTATOR ORDER BY ROTATOR_ID")
				RotatorList.Databind()
				if (RotatorList.Items.Count = 0) then
					RotatorList.visible = "false"
				end if
				RotatorList.Datasource.Close()
			elseif sender.CommandArgument = 16 then
				AdminLinks.visible = "false"
				AdminHtmlFormConfig.visible = "true"
				AdminHtmlFormView.visible = "false"
				AdminHtmlFormDelete.visible = "false"
				HtmlFormList.Datasource = Database.Read("SELECT FORM_ID, FORM_NAME, FORM_DATE, FORM_NEW FROM " & Database.DBPrefix & "_HTML_FORMS ORDER BY FORM_DATE DESC")
				HtmlFormList.Databind()
				if (HtmlFormList.Items.Count = 0) then
					PagePanel.visible = "false"
					NoItemsDiv.InnerHtml = "No HTML Forms Have Been Submitted<br /><br /><a href=""javascript:openHelp('DMGHtmlForms.html')"">Click Here</a> to learn how to use this feature.<br /><br />"
				end if
				HtmlFormList.Datasource.Close()
			elseif sender.CommandArgument = 17 then
				AdminLinks.visible = "false"
				AdminBannedIPConfig.visible = "true"
				Dim DataSet1 as new DataSet()
				Dim DataAdapter1 as new OdbcDataAdapter()
				DataAdapter1.SelectCommand = new OdbcCommand("SELECT DISTINCT IP_ADDRESS FROM " & Database.DBPrefix & "_BANNED_IP ORDER BY IP_ADDRESS", Database.DatabaseConnection())
				DataAdapter1.Fill(DataSet1, "IPADDRESSES")
				DataAdapter1.SelectCommand = new OdbcCommand("SELECT I.IP_ADDRESS, I.MEMBER_ID, M.MEMBER_USERNAME FROM " & Database.DBPrefix & "_BANNED_IP I Left Outer Join " & Database.DBPrefix & "_MEMBERS M On I.MEMBER_ID = M.MEMBER_ID ORDER BY I.IP_ADDRESS", Database.DatabaseConnection())
				DataAdapter1.Fill(DataSet1, "IPMEMBERS")
				DataSet1.Relations.Add("IPRelation", DataSet1.Tables("IPADDRESSES").Columns("IP_ADDRESS"), DataSet1.Tables("IPMEMBERS").Columns("IP_ADDRESS"))
				IPList.DataSource = DataSet1
				IPLIst.DataBind()
			elseif sender.CommandArgument = 18 then
				AdminLinks.visible = "false"
				AdminGalleryConfig.visible = "true"
				AdminGalleryNew.visible = "false"
				AdminGalleryDelete.visible = "false"
				AdminGalleryEdit.visible = "false"
				AdminGalleryNewConfirm.visible = "false"
				GalleryList.Datasource = Database.Read("SELECT GALLERY_ID, GALLERY_NAME FROM " & Database.DBPrefix & "_GALLERY ORDER BY GALLERY_ID")
				GalleryList.Databind()
				if (GalleryList.Items.Count = 0) then
					GalleryList.visible = "false"
				end if
				GalleryList.Datasource.Close()
			elseif sender.CommandArgument = 19 then
				AdminLinks.visible = "false"
				AdminCustomMessagesConfig.visible = "true"
				Dim MessageReader as OdbcDataReader = Database.Read("SELECT * FROM " & Database.DBPrefix & "_CUSTOM_MESSAGES WHERE ID = 1")
				While MessageReader.Read()
					txtEmailAdminApproval.text = MessageReader("EMAIL_ADMINAPPROVAL").ToString()
					txtEmailSendKey.text = MessageReader("EMAIL_SENDKEY").ToString()
					txtEmailConfirmPost.text = MessageReader("EMAIL_CONFIRMPOST").ToString()
					txtEmailSubscription.text = MessageReader("EMAIL_SUBSCRIPTION").ToString()
					txtEmailWelcomeMessage.text = MessageReader("EMAIL_WELCOMEMESSAGE").ToString()
					txtMessageAdminApproval.text = MessageReader("MESSAGE_ADMINAPPROVAL").ToString()
					txtMessageSendKey.text = MessageReader("MESSAGE_SENDKEY").ToString()
					txtMessageRegistration.text = MessageReader("MESSAGE_REGISTRATION").ToString()
					txtMessageValidation.text = MessageReader("MESSAGE_VALIDATION").ToString()
					txtMessageConfirmPost.text = MessageReader("MESSAGE_CONFIRMPOST").ToString()
					txtMessagePrivacyNotice.text = MessageReader("MESSAGE_PRIVACYNOTICE").ToString()
				End While
				MessageReader.Close()
			elseif sender.CommandArgument = 20 then
				AdminLinks.visible = "false"
				AboutDMG.visible = "true"
			elseif sender.CommandArgument = 21 then
				AdminLinks.visible = "false"
				AdminForumMainConfig.visible = "true"

				Dim ConfigReader as OdbcDataReader = Database.Read("SELECT " & Database.DBPrefix & "_ITEMS_PER_PAGE, " & Database.DBPrefix & "_SPAM_FILTER, " & Database.DBPrefix & "_SHOWSTATISTICS, " & Database.DBPrefix & "_ALLOWSUB, " & Database.DBPrefix & "_QUICK_REGISTRATION, " & Database.DBPrefix & "_MEMBER_VALIDATION, " & Database.DBPrefix & "_CURSE_FILTER, " & Database.DBPrefix & "_RSS_FEEDS, " & Database.DBPrefix & "_ALLOW_REGISTRATION, " & Database.DBPrefix & "_ALLOW_EDITS, " & Database.DBPrefix & "_ALLOW_MEDIA, " & Database.DBPrefix & "_HIDE_MEMBERS, " & Database.DBPrefix & "_HIDE_LOGIN, " & Database.DBPrefix & "_MEMBER_PHOTOSIZE, " & Database.DBPrefix & "_AVATAR_SIZE, " & Database.DBPrefix & "_ALLOW_REPORTING, " & Database.DBPrefix & "_MEMBER_FILETYPES, " & Database.DBPrefix & "_TOPIC_UPLOADSIZE, " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE FROM " & Database.DBPrefix & "_SETTINGS WHERE ID = 1")
					While ConfigReader.Read()
						txtDMGItemsPerPage.text = ConfigReader(Database.DBPrefix & "_ITEMS_PER_PAGE").ToString()
						txtDMGSpamFilter.text = ConfigReader(Database.DBPrefix & "_SPAM_FILTER").ToString()
						txtDMGShowStatistics.SelectedValue = ConfigReader(Database.DBPrefix & "_SHOWSTATISTICS")
						txtDMGAllowSub.SelectedValue = ConfigReader(Database.DBPrefix & "_ALLOWSUB")
						txtDMGQuickRegistration.SelectedValue = ConfigReader(Database.DBPrefix & "_QUICK_REGISTRATION")
						txtDMGCurseFilter.SelectedValue = ConfigReader(Database.DBPrefix & "_CURSE_FILTER")
						txtDMGRSSFeeds.SelectedValue = ConfigReader(Database.DBPrefix & "_RSS_FEEDS")
						txtDMGAllowRegistration.SelectedValue = ConfigReader(Database.DBPrefix & "_ALLOW_REGISTRATION")
						txtDMGAllowEdits.SelectedValue = ConfigReader(Database.DBPrefix & "_ALLOW_EDITS")
						txtDMGAllowMedia.SelectedValue = ConfigReader(Database.DBPrefix & "_ALLOW_MEDIA")
						txtDMGAllowReporting.SelectedValue = ConfigReader(Database.DBPrefix & "_ALLOW_REPORTING")
						txtDMGHideMembers.SelectedValue = ConfigReader(Database.DBPrefix & "_HIDE_MEMBERS")
						txtDMGHideLogin.SelectedValue = ConfigReader(Database.DBPrefix & "_HIDE_LOGIN")
						txtDMGMemberValidation.SelectedValue = ConfigReader(Database.DBPrefix & "_MEMBER_VALIDATION")
						txtDMGEnableEmailWelcomeMessage.SelectedValue = ConfigReader(Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE")
						txtDMGMemberPhotoSize.text = ConfigReader(Database.DBPrefix & "_MEMBER_PHOTOSIZE").ToString()
						txtDMGAvatarSize.text = ConfigReader(Database.DBPrefix & "_AVATAR_SIZE").ToString()
						txtDMGMemberFileTypes.text = ConfigReader(Database.DBPrefix & "_MEMBER_FILETYPES").ToString()
						txtDMGTopicUploadSize.text = ConfigReader(Database.DBPrefix & "_TOPIC_UPLOADSIZE").ToString()
					End While
				ConfigReader.Close()
			elseif sender.CommandArgument = 22 then
				AdminLinks.visible = "false"
				AdminPMCleanup.visible = "true"
			elseif sender.CommandArgument = 23 then
				AdminLinks.visible = "false"
				AdminSearchConfig.visible = "true"

				Dim ConfigReader as OdbcDataReader = Database.Read("SELECT " & Database.DBPrefix & "_SEARCH_TOPICS, " & Database.DBPrefix & "_SEARCH_MEMBERS, " & Database.DBPrefix & "_SEARCH_BLOGS, " & Database.DBPrefix & "_SEARCH_PAGES FROM " & Database.DBPrefix & "_SETTINGS WHERE ID = 1")
					While ConfigReader.Read()
						txtDMGSearchTopics.SelectedValue = ConfigReader(Database.DBPrefix & "_SEARCH_TOPICS")
						txtDMGSearchMembers.SelectedValue = ConfigReader(Database.DBPrefix & "_SEARCH_MEMBERS")
						txtDMGSearchBlogs.SelectedValue = ConfigReader(Database.DBPrefix & "_SEARCH_BLOGS")
						txtDMGSearchPages.SelectedValue = ConfigReader(Database.DBPrefix & "_SEARCH_PAGES")
					End While
				ConfigReader.Close()
			end if
		End Sub

		Sub SubmitMainConfig(sender As System.Object, e As System.EventArgs)
			Dim Title as String = Functions.RepairString(txtDMGTitle.Text, 0)
			Dim Copyright as String = Functions.RepairString(txtDMGCopyright.Text, 0)
			Dim CustomMeta as String = Functions.RepairString(txtDMGCustomMeta.Text, 0)
			Dim Logo as String = Functions.RepairString(txtDMGLogo.Text, 0)
			Dim URL as String = Functions.RepairString(txtDMGURL.Text, 0)
			Dim DefaultTemplate as Integer = txtDMGLayoutTemplate.SelectedValue
			Dim DefaultPage as Integer = txtDMGSetDefaultPage.SelectedValue
			Dim HtmlTitle as String = Functions.RepairString(txtDMGHtmlTitle.text, 0)
			Dim ThumbnailSize as String = CLng(txtDMGThumbnailSize.text)

			Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_TITLE = '" & Title & "', " & Database.DBPrefix & "_COPYRIGHT = '" & Copyright & "', " & Database.DBPrefix & "_LOGO = '" & Logo & "', " & Database.DBPrefix & "_URL = '" & URL & "', " & Database.DBPrefix & "_TEMPLATE_DEFAULT = " & DefaultTemplate & ", " & Database.DBPrefix & "_FORUMS_DEFAULT = " & DefaultPage & ", " & Database.DBPrefix & "_CUSTOM_META = '" & CustomMeta & "', " & Database.DBPrefix & "_HTML_TITLE = '" & HtmlTitle & "', " & Database.DBPrefix & "_THUMBNAIL_SIZE = " & ThumbnailSize)
			PagePanel.Visible = "false"
			NoItemsDiv.InnerHtml = "Main Configuration Edited.<br /><a href=""admin.aspx"">Click Here</a> To Return To The Admin Page.<br /><br />"
		End Sub

		Sub SubmitForumConfig(sender As System.Object, e As System.EventArgs)
			Dim ItemsPerPage as String = CLng(txtDMGItemsPerPage.text)
			Dim SpamFilter as String = CLng(txtDMGSpamFilter.text)
			Dim ShowStats as Integer = txtDMGShowStatistics.SelectedValue
			Dim AllowSub as Integer = txtDMGAllowSub.SelectedValue
			Dim QuickReg as Integer = txtDMGQuickRegistration.SelectedValue
			Dim CurseFilt as Integer = txtDMGCurseFilter.SelectedValue
			Dim RSSFeed as Integer = txtDMGRSSFeeds.SelectedValue
			Dim MemberValidate as Integer = txtDMGMemberValidation.SelectedValue
			Dim EnableEmailWelcomeMessage as Integer = txtDMGEnableEmailWelcomeMessage.SelectedValue
			Dim AllowRegistration as Integer = txtDMGAllowRegistration.SelectedValue
			Dim AllowEdits as Integer = txtDMGAllowEdits.SelectedValue
			Dim AllowMedia as Integer = txtDMGAllowMedia.SelectedValue
			Dim AllowReporting as Integer = txtDMGAllowReporting.SelectedValue
			Dim HideMembers as Integer = txtDMGHideMembers.SelectedValue
			Dim HideLogin as Integer = txtDMGHideLogin.SelectedValue
			Dim MemberPhotoSize as String = CLng(txtDMGMemberPhotoSize.text)
			Dim AvatarSize as String = CLng(txtDMGAvatarSize.text)
			Dim MemberFileTypes as String = Functions.RepairString(txtDMGMemberFileTypes.text, 0)
			Dim TopicUploadSize as String = CLng(txtDMGTopicUploadSize.text)

			Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_ITEMS_PER_PAGE = " & ItemsPerPage & ", " & Database.DBPrefix & "_SPAM_FILTER = " & SpamFilter & ", " & Database.DBPrefix & "_SHOWSTATISTICS = " & ShowStats & ", " & Database.DBPrefix & "_ALLOWSUB = " & AllowSub & ", " & Database.DBPrefix & "_QUICK_REGISTRATION = " & QuickReg & ", " & Database.DBPrefix & "_CURSE_FILTER = " & CurseFilt & ", " & Database.DBPrefix & "_MEMBER_VALIDATION = " & MemberValidate & ", " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE = " & EnableEmailWelcomeMessage & ", " & Database.DBPrefix & "_RSS_FEEDS = " & RSSFeed & ", " & Database.DBPrefix & "_ALLOW_REGISTRATION = " & AllowRegistration & ", " & Database.DBPrefix & "_ALLOW_EDITS = " & AllowEdits & ", " & Database.DBPrefix & "_ALLOW_MEDIA = " & AllowMedia & ", " & Database.DBPrefix & "_ALLOW_REPORTING = " & AllowReporting & ", " & Database.DBPrefix & "_HIDE_MEMBERS = " & HideMembers & ", " & Database.DBPrefix & "_HIDE_LOGIN = " & HideLogin & ", " & Database.DBPrefix & "_MEMBER_PHOTOSIZE = " & MemberPhotoSize & ", " & Database.DBPrefix & "_AVATAR_SIZE = " & AvatarSize & ", " & Database.DBPrefix & "_MEMBER_FILETYPES = '" & MemberFileTypes & "', " & Database.DBPrefix & "_TOPIC_UPLOADSIZE = " & TopicUploadSize)
			PagePanel.Visible = "false"
			NoItemsDiv.InnerHtml = "Forum Options Edited.<br /><a href=""admin.aspx"">Click Here</a> To Return To The Admin Page.<br /><br />"
		End Sub

		Sub CreateNewTemplate(sender As System.Object, e As System.EventArgs)
			AdminLinks.visible = "false"
			AdminNewTemplate.visible = "true"
			txtDMGNewTemplateName.text = "New Template"
			txtDMGNewFontFace.text = Settings.FontFace
			txtDMGNewFontSize.text = Settings.FontSize
			txtDMGNewButtonColor.text = Settings.ButtonColor
			txtDMGNewLoginFontColor.text = Settings.LoginFontColor
			txtDMGNewHeaderSize.text = Settings.HeaderSize
			txtDMGNewHeaderColor.text = Settings.HeaderColor
			txtDMGNewHeaderFontColor.text = Settings.HeaderFontColor
			txtDMGNewSubheaderColor.text = Settings.SubheaderColor
			txtDMGNewSubheaderFontColor.text = Settings.SubheaderFontColor
			txtDMGNewFooterSize.text = Settings.FooterSize
			txtDMGNewFooterColor.text = Settings.FooterColor
			txtDMGNewFooterFontColor.text = Settings.FooterFontColor
			txtDMGNewBGColor.text = Settings.BGColor
			txtDMGNewBGImage.text = Settings.BGImage
			txtDMGNewFontColor.text = Settings.FontColor
			txtDMGNewLinkColor.text = Settings.LinkColor
			txtDMGNewLinkDecoration.text = Settings.LinkDecoration
			txtDMGNewLinkVisitedColor.text = Settings.VLinkColor
			txtDMGNewLinkVisitedDecoration.text = Settings.VLinkDecoration
			txtDMGNewLinkActiveColor.text = Settings.ALinkColor
			txtDMGNewLinkActiveDecoration.text = Settings.ALinkDecoration
			txtDMGNewLinkHoverColor.text = Settings.HLinkColor
			txtDMGNewLinkHoverDecoration.text = Settings.HLinkDecoration
			txtDMGNewTopicsFontSize.text = Settings.TopicsFontSize
			txtDMGNewTopicsFontColor.text = Settings.TopicsFontColor
			txtDMGNewTopicsBGColor1.text = Settings.TableBGColor1
			txtDMGNewTopicsBGColor2.text = Settings.TableBGColor2
			txtDMGNewTableborderColor.text = Settings.TableborderColor
			txtDMGNewScrollbarColor.text = Settings.ScrollbarColor
			txtDMGNewCustomHeader.text = Settings.CustomHeader
			txtDMGNewCustomFooter.text = Settings.CustomFooter
			txtDMGNewCustomCSS.text = Settings.CustomCSS
			txtDMGNewMarginSide.text = Settings.SideMargin
			txtDMGNewMarginTop.text = Settings.TopMargin
		End Sub

		Sub DeleteTemplateMenu(sender As System.Object, e As System.EventArgs)
			AdminLinks.visible = "false"
			AdminDeleteTemplatePanel.visible = "true"

			TemplatesList.DataSource = Database.Read("SELECT ID, " & Database.DBPrefix & "_TEMPLATE_NAME FROM " & Database.DBPrefix & "_SETTINGS WHERE (ID <> " & Settings.DefaultTemplate & ") AND (ID <> 1) ORDER BY ID")
			TemplatesList.Databind()
				if (TemplatesList.Items.Count = 0) then
					PagePanel.visible = "false"
					NoItemsDiv.InnerHtml = "There are no unused or non-default templates.<br /><a href=""admin.aspx"">Click Here</a> To Return To The Admin Page.<br /><br />"
				end if
			TemplatesList.Datasource.Close()
		End Sub

		Sub DeleteTemplate(sender As System.Object, e As System.EventArgs)
			Database.Write("DELETE FROM " & Database.DBPrefix & "_SETTINGS WHERE ID = " & sender.CommandArgument)
			PagePanel.visible = "false"
			NoItemsDiv.InnerHtml = "Template Deleted Successfully.<br /><a href=""admin.aspx"">Click Here</a> To Return To The Admin Page.<br /><br />"
		End Sub

		Sub SelectTemplate(Type as Integer)
			AdminLinks.visible = "false"

			Dim SettingsCount as Integer
			Dim SettingsReader as OdbcDataReader = Database.Read("SELECT count(*) as TemplateCount FROM " & Database.DBPrefix & "_SETTINGS")
			While SettingsReader.Read()
				SettingsCount = SettingsReader("TemplateCount")
			End While
			SettingsReader.Close()

			if (Type = 1) then
				ColorSettingsPanel.visible = "true"

				if (SettingsCount > 1)
					Dim LItem as new ListItem("", 0)
					ColorSettingsDropDown.Items.Add(LItem)

					SettingsReader = Database.Read("SELECT ID, " & Database.DBPrefix & "_TEMPLATE_NAME FROM " & Database.DBPrefix & "_SETTINGS ORDER BY ID")
					While SettingsReader.Read()
						if (SettingsReader("ID") = Settings.DefaultTemplate) then
							LItem = new ListItem(SettingsReader(Database.DBPrefix & "_TEMPLATE_NAME") & " (Active)", SettingsReader("ID"))
						else
							LItem = new ListItem(SettingsReader(Database.DBPrefix & "_TEMPLATE_NAME"), SettingsReader("ID"))
						end if
						ColorSettingsDropDown.Items.Add(LItem)
					End While
					SettingsReader.Close()
				else
					Dim temp as System.EventArgs = new System.EventArgs()
					ChangeColors(ColorSettingsPanel, temp)
				end if
			else
				CustomHTMLSettingsPanel.visible = "true"

				if (SettingsCount > 1)
					Dim LItem as new ListItem("", 0)
					CustomHTMLSettingsDropDown.Items.Add(LItem)

					SettingsReader = Database.Read("SELECT ID, " & Database.DBPrefix & "_TEMPLATE_NAME FROM " & Database.DBPrefix & "_SETTINGS ORDER BY ID")
					While SettingsReader.Read()
						if (SettingsReader("ID") = Settings.DefaultTemplate) then
							LItem = new ListItem(SettingsReader(Database.DBPrefix & "_TEMPLATE_NAME") & " (Active)", SettingsReader("ID"))
						else
							LItem = new ListItem(SettingsReader(Database.DBPrefix & "_TEMPLATE_NAME"), SettingsReader("ID"))
						end if
						CustomHTMLSettingsDropDown.Items.Add(LItem)
					End While
					SettingsReader.Close()
				else
					Dim temp as System.EventArgs = new System.EventArgs()
					ChangeCustomHTML(ColorSettingsPanel, temp)
				end if
			end if
		End Sub

		Sub EditMainMenu(sender As System.Object, e As System.EventArgs)
			AdminLinks.visible = "false"
			AdminMainMenuConfig.visible = "true"
			txtDMGHorizDivide.Text = Settings.HorizDivide
			txtDMGVertDivide.Text = Settings.VertDivide
			MainMenuList.Datasource = Database.Read("SELECT * FROM " & Database.DBPrefix & "_MAIN_MENU ORDER BY LINK_ORDER")
			MainMenuList.Databind()
			MainMenuList.Datasource.Close()
		End Sub

		Sub MainMenuOrderUp(sender As System.Object, e As System.EventArgs)
			AdminLinks.visible = "false"
			AdminMainMenuConfig.visible = "true"
			Dim CurrentLink as Integer = sender.CommandArgument
			if (CurrentLink > 1) then
				Dim PreviousLink as Integer = CurrentLink - 1
				Database.Write("UPDATE " & Database.DBPrefix & "_MAIN_MENU SET LINK_ORDER = -1 WHERE LINK_ORDER = " & PreviousLink)
				Database.Write("UPDATE " & Database.DBPrefix & "_MAIN_MENU SET LINK_ORDER = " & PreviousLink & " WHERE LINK_ORDER = " & CurrentLink)
				Database.Write("UPDATE " & Database.DBPrefix & "_MAIN_MENU SET LINK_ORDER = " & CurrentLink & " WHERE LINK_ORDER = -1")
			end if
			MainMenuList.Datasource = Database.Read("SELECT * FROM " & Database.DBPrefix & "_MAIN_MENU ORDER BY LINK_ORDER")
			MainMenuList.Databind()
			MainMenuList.Datasource.Close()
		End Sub

		Sub MainMenuOrderDown(sender As System.Object, e As System.EventArgs)
			AdminLinks.visible = "false"
			AdminMainMenuConfig.visible = "true"
			Dim CurrentLink as Integer = sender.CommandArgument

			Dim Reader as OdbcDataReader = Database.Read("SELECT LINK_ORDER FROM " & Database.DBPrefix & "_MAIN_MENU WHERE LINK_ORDER = (" & CurrentLink & "+1)", 1)
			if Reader.HasRows() then
				Dim NextLink as Integer = CurrentLink + 1
				Database.Write("UPDATE " & Database.DBPrefix & "_MAIN_MENU SET LINK_ORDER = -1 WHERE LINK_ORDER = " & NextLink)
				Database.Write("UPDATE " & Database.DBPrefix & "_MAIN_MENU SET LINK_ORDER = " & NextLink & " WHERE LINK_ORDER = " & CurrentLink)
				Database.Write("UPDATE " & Database.DBPrefix & "_MAIN_MENU SET LINK_ORDER = " & CurrentLink & " WHERE LINK_ORDER = -1")
			end if
			Reader.Close()

			MainMenuList.Datasource = Database.Read("SELECT * FROM " & Database.DBPrefix & "_MAIN_MENU ORDER BY LINK_ORDER")
			MainMenuList.Databind()
			MainMenuList.Datasource.Close()
		End Sub

		Sub MainMenuNewLink(sender As System.Object, e As System.EventArgs)
			AdminLinks.visible = "false"
			AdminMainMenuConfig.visible = "true"

			Dim NewLinkOrder as Integer = 1
			Dim Reader as OdbcDataReader = Database.Read("SELECT LINK_ORDER FROM " & Database.DBPrefix & "_MAIN_MENU ORDER BY LINK_ORDER DESC", 1)
			While Reader.Read()
				NewLinkOrder = Reader("LINK_ORDER") + 1
			End While
			Reader.Close()

			Dim NewText as String = Functions.RepairString(NewLinkText.Text, 0)
			Dim NewType as String = cLng(NewLinkType.SelectedValue)
			Dim NewWindow as String = cLng(NewLinkWindow.SelectedValue)
			Dim NewParameter as String

			if (NewLinkType.SelectedValue = 14) then
				NewParameter = NewLinkParameter.Text
			else
				if (Functions.IsInteger(NewLinkParameter.Text)) then
					NewParameter = NewLinkParameter.Text
				else
					NewParameter = "0"
				end if
			end if

			Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (" & NewLinkOrder & ", '" & NewText & "', " & NewType & ", '" & NewParameter & "', " & NewWindow & ")")

			MainMenuList.Datasource = Database.Read("SELECT * FROM " & Database.DBPrefix & "_MAIN_MENU ORDER BY LINK_ORDER")
			MainMenuList.Databind()
			MainMenuList.Datasource.Close()

			NewLinkText.Text = ""
			NewLinkType.SelectedValue = 2
			NewLinkParameter.Text = ""
			NewLinkWindow.SelectedValue = 0
		End Sub

		Sub MainMenuSaveLink(sender As System.Object, e As System.EventArgs)
			AdminLinks.visible = "false"
			AdminMainMenuConfig.visible = "true"

			Dim Reader as OdbcDataReader
			Dim NewText, NewType, NewWindow, NewParameter as String

			Reader = Database.Read("SELECT LINK_ID FROM " & Database.DBPrefix & "_MAIN_MENU ORDER BY LINK_ID")
			While Reader.Read()
				Dim LinkID as Integer = Reader("LINK_ID")

				NewText = Functions.RepairString(Request.Form("txtLinkText" & LinkID), 0)
				NewType = cLng(Request.Form("txtLinkType" & LinkID))
				NewWindow = cLng(Request.Form("txtLinkWindow" & LinkID))

				if (Request.Form("txtLinkType" & LinkID) = 14) then
					NewParameter = Request.Form("txtLinkParameter" & LinkID)
				else
					if (Functions.IsInteger(Request.Form("txtLinkParameter" & LinkID))) then
						NewParameter = Request.Form("txtLinkParameter" & LinkID)
					else
						NewParameter = "0"
					end if
				end if

				Database.Write("UPDATE " & Database.DBPrefix & "_MAIN_MENU SET LINK_TEXT = '" & NewText & "', LINK_TYPE = " & NewType & ", LINK_PARAMETER = '" & NewParameter & "', LINK_WINDOW = " & NewWindow & " WHERE LINK_ID = " & LinkID)
			End While
			Reader.Close()

			if (NewLinkText.Text <> "") then
				Dim NewLinkOrder as Integer = 1
				Reader = Database.Read("SELECT LINK_ORDER FROM " & Database.DBPrefix & "_MAIN_MENU ORDER BY LINK_ORDER DESC", 1)
				While Reader.Read()
					NewLinkOrder = Reader("LINK_ORDER") + 1
				End While
				Reader.Close()

				NewText = Functions.RepairString(NewLinkText.Text, 0)
				NewType = cLng(NewLinkType.SelectedValue)
				NewWindow = cLng(NewLinkWindow.SelectedValue)

				if (NewLinkType.SelectedValue = 14) then
					NewParameter = NewLinkParameter.Text
				else
					if (Functions.IsInteger(NewLinkParameter.Text)) then
						NewParameter = NewLinkParameter.Text
					else
						NewParameter = "0"
					end if
				end if

				Database.Write("INSERT INTO " & Database.DBPrefix & "_MAIN_MENU (LINK_ORDER, LINK_TEXT, LINK_TYPE, LINK_PARAMETER, LINK_WINDOW) VALUES (" & NewLinkOrder & ", '" & NewText & "', " & NewType & ", '" & NewParameter & "', " & NewWindow & ")")
			end if

			Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_HORIZ_DIVIDE = '" & txtDMGHorizDivide.Text & "', " & Database.DBPrefix & "_VERT_DIVIDE = '" & txtDMGVertDivide.Text & "'")

			MainMenuList.Datasource = Database.Read("SELECT * FROM " & Database.DBPrefix & "_MAIN_MENU ORDER BY LINK_ORDER")
			MainMenuList.Databind()
			MainMenuList.Datasource.Close()

			NewLinkText.Text = ""
			NewLinkType.SelectedValue = 2
			NewLinkParameter.Text = ""
			NewLinkWindow.SelectedValue = 0
		End Sub

		Sub MainMenuDeleteLink(sender As System.Object, e As System.EventArgs)
			AdminLinks.visible = "false"
			AdminMainMenuConfig.visible = "true"

			Dim LinkID as Integer = sender.CommandArgument
			Dim LinkOrder as Integer = 100
			Dim Reader as OdbcDataReader = Database.Read("SELECT LINK_ORDER FROM " & Database.DBPrefix & "_MAIN_MENU WHERE LINK_ID = " & LinkID)
			While Reader.Read()
				LinkOrder = Reader("LINK_ORDER")
			End While
			Reader.Close()

			Database.Write("DELETE FROM " & Database.DBPrefix & "_MAIN_MENU WHERE LINK_ID = " & LinkID)
			Database.Write("UPDATE " & Database.DBPrefix & "_MAIN_MENU SET LINK_ORDER = (LINK_ORDER-1) WHERE LINK_ORDER > " & LinkOrder)

			MainMenuList.Datasource = Database.Read("SELECT * FROM " & Database.DBPrefix & "_MAIN_MENU ORDER BY LINK_ORDER")
			MainMenuList.Databind()
			MainMenuList.Datasource.Close()
		End Sub

		Sub ChangeCustomHTML(sender As System.Object, e As System.EventArgs)
			CustomHTMLSettingsPanel.visible = "false"
			AdminCustomHTML.visible = "true"

			Dim TemplateID as Integer
			Dim TemplateName as String = ""
			if (sender.ToString() = "System.Web.UI.WebControls.DropDownList") then
				TemplateID = sender.SelectedValue
				Dim Reader as OdbcDataReader = Database.Read("SELECT " & Database.DBPrefix & "_TEMPLATE_NAME FROM " & Database.DBPrefix & "_SETTINGS WHERE ID = " & TemplateID)
				While Reader.Read()
					TemplateName = Reader(Database.DBPrefix & "_TEMPLATE_NAME")
				End While
				Reader.Close()
			else
				TemplateID = 1
				TemplateName = "Default Template"
				txtSetDefaultTemplate2.enabled = "false"
				txtDMGTemplateName2.enabled = "false"
			end if

			CustomHTMLSubmit.CommandArgument = TemplateID

			if (TemplateID = Settings.DefaultTemplate) then
				txtSetDefaultTemplate2.SelectedValue = "1"
			else
				txtSetDefaultTemplate2.SelectedValue = "0"
			end if

			txtDMGTemplateName2.text = TemplateName

			Dim ConfigReader as OdbcDataReader = Database.Read("SELECT " & Database.DBPrefix & "_CUSTOM_HEADER, " & Database.DBPrefix & "_CUSTOM_FOOTER, " & Database.DBPrefix & "_CUSTOM_CSS, " & Database.DBPrefix & "_BGIMAGE, " & Database.DBPrefix & "_MARGIN_SIDE, " & Database.DBPrefix & "_MARGIN_TOP FROM " & Database.DBPrefix & "_SETTINGS WHERE ID = " & TemplateID)
				While ConfigReader.Read()
					txtDMGCustomHeader.text = ConfigReader(Database.DBPrefix & "_CUSTOM_HEADER").ToString()
					txtDMGCustomFooter.text = ConfigReader(Database.DBPrefix & "_CUSTOM_FOOTER").ToString()
					txtDMGCustomCSS.text = ConfigReader(Database.DBPrefix & "_CUSTOM_CSS").ToString()
					txtDMGBGImage.text = ConfigReader(Database.DBPrefix & "_BGIMAGE").ToString()
					txtDMGMarginSide.text = ConfigReader(Database.DBPrefix & "_MARGIN_SIDE").ToString()
					txtDMGMarginTop.text = ConfigReader(Database.DBPrefix & "_MARGIN_TOP").ToString()
				End While
			ConfigReader.Close()
		End Sub

		Sub ChangeColors(sender As System.Object, e As System.EventArgs)
			ColorSettingsPanel.visible = "false"
			AdminColorConfig.visible = "true"

			Dim TemplateID as Integer
			Dim TemplateName as String = ""
			if (sender.ToString() = "System.Web.UI.WebControls.DropDownList") then
				TemplateID = sender.SelectedValue
				Dim Reader as OdbcDataReader = Database.Read("SELECT " & Database.DBPrefix & "_TEMPLATE_NAME FROM " & Database.DBPrefix & "_SETTINGS WHERE ID = " & TemplateID)
				While Reader.Read()
					TemplateName = Reader(Database.DBPrefix & "_TEMPLATE_NAME")
				End While
				Reader.Close()
			else
				TemplateID = 1
				TemplateName = "Default Template"
				txtSetDefaultTemplate.enabled = "false"
				txtDMGTemplateName.enabled = "false"
			end if

			ColorSubmit.CommandArgument = TemplateID

			if (TemplateID = Settings.DefaultTemplate) then
				txtSetDefaultTemplate.SelectedValue = "1"
			else
				txtSetDefaultTemplate.SelectedValue = "0"
			end if

			txtDMGTemplateName.text = TemplateName

			Dim ConfigReader as OdbcDataReader = Database.Read("SELECT " & Database.DBPrefix & "_FONTFACE, " & Database.DBPrefix & "_FONTSIZE, " & Database.DBPrefix & "_BUTTON_COLOR, " & Database.DBPrefix & "_LOGIN_FONTCOLOR, " & Database.DBPrefix & "_HEADER_SIZE, " & Database.DBPrefix & "_HEADER_COLOR, " & Database.DBPrefix & "_HEADER_FONTCOLOR, " & Database.DBPrefix & "_SUBHEADER_COLOR, " & Database.DBPrefix & "_SUBHEADER_FONTCOLOR, " & Database.DBPrefix & "_FOOTER_SIZE, " & Database.DBPrefix & "_FOOTER_COLOR, " & Database.DBPrefix & "_FOOTER_FONTCOLOR, " & Database.DBPrefix & "_BGCOLOR, " & Database.DBPrefix & "_BGIMAGE, " & Database.DBPrefix & "_FONT_COLOR, " & Database.DBPrefix & "_LINK_COLOR, " & Database.DBPrefix & "_LINK_DECORATION, " & Database.DBPrefix & "_LINK_VISITED_COLOR, " & Database.DBPrefix & "_LINK_VISITED_DECORATION, " & Database.DBPrefix & "_LINK_ACTIVE_COLOR, " & Database.DBPrefix & "_LINK_ACTIVE_DECORATION, " & Database.DBPrefix & "_LINK_HOVER_COLOR, " & Database.DBPrefix & "_LINK_HOVER_DECORATION, " & Database.DBPrefix & "_TOPICS_FONTSIZE, " & Database.DBPrefix & "_TOPICS_FONTCOLOR, " & Database.DBPrefix & "_TOPICS_BGCOLOR1, " & Database.DBPrefix & "_TOPICS_BGCOLOR2, " & Database.DBPrefix & "_TABLEBORDER_COLOR, " & Database.DBPrefix & "_SCROLLBAR_COLOR FROM " & Database.DBPrefix & "_SETTINGS WHERE ID = " & TemplateID)
				While ConfigReader.Read()
					txtDMGFontFace.text = ConfigReader(Database.DBPrefix & "_FONTFACE").ToString()
					txtDMGFontSize.text = ConfigReader(Database.DBPrefix & "_FONTSIZE")
					txtDMGButtonColor.text = ConfigReader(Database.DBPrefix & "_BUTTON_COLOR").ToString()
					txtDMGLoginFontColor.text = ConfigReader(Database.DBPrefix & "_LOGIN_FONTCOLOR").ToString()
					txtDMGHeaderSize.text = ConfigReader(Database.DBPrefix & "_HEADER_SIZE")
					txtDMGHeaderColor.text = ConfigReader(Database.DBPrefix & "_HEADER_COLOR").ToString()
					txtDMGHeaderFontColor.text = ConfigReader(Database.DBPrefix & "_HEADER_FONTCOLOR").ToString()
					txtDMGSubheaderColor.text = ConfigReader(Database.DBPrefix & "_SUBHEADER_COLOR").ToString()
					txtDMGSubheaderFontColor.text = ConfigReader(Database.DBPrefix & "_SUBHEADER_FONTCOLOR").ToString()
					txtDMGFooterSize.text = ConfigReader(Database.DBPrefix & "_FOOTER_SIZE")
					txtDMGFooterColor.text = ConfigReader(Database.DBPrefix & "_FOOTER_COLOR").ToString()
					txtDMGFooterFontColor.text = ConfigReader(Database.DBPrefix & "_FOOTER_FONTCOLOR").ToString()
					txtDMGBGColor.text = ConfigReader(Database.DBPrefix & "_BGCOLOR").ToString()
					txtDMGBGImage2.text = ConfigReader(Database.DBPrefix & "_BGIMAGE").ToString()
					txtDMGFontColor.text = ConfigReader(Database.DBPrefix & "_FONT_COLOR").ToString()
					txtDMGLinkColor.text = ConfigReader(Database.DBPrefix & "_LINK_COLOR").ToString()
					txtDMGLinkDecoration.text = ConfigReader(Database.DBPrefix & "_LINK_DECORATION").ToString()
					txtDMGLinkVisitedColor.text = ConfigReader(Database.DBPrefix & "_LINK_VISITED_COLOR").ToString()
					txtDMGLinkVisitedDecoration.text = ConfigReader(Database.DBPrefix & "_LINK_VISITED_DECORATION").ToString()
					txtDMGLinkActiveColor.text = ConfigReader(Database.DBPrefix & "_LINK_ACTIVE_COLOR").ToString()
					txtDMGLinkActiveDecoration.text = ConfigReader(Database.DBPrefix & "_LINK_ACTIVE_DECORATION").ToString()
					txtDMGLinkHoverColor.text = ConfigReader(Database.DBPrefix & "_LINK_HOVER_COLOR").ToString()
					txtDMGLinkHoverDecoration.text = ConfigReader(Database.DBPrefix & "_LINK_HOVER_DECORATION").ToString()
					txtDMGTopicsFontSize.text = ConfigReader(Database.DBPrefix & "_TOPICS_FONTSIZE").ToString()
					txtDMGTopicsFontColor.text = ConfigReader(Database.DBPrefix & "_TOPICS_FONTCOLOR").ToString()
					txtDMGTopicsBGColor1.text = ConfigReader(Database.DBPrefix & "_TOPICS_BGCOLOR1").ToString()
					txtDMGTopicsBGColor2.text = ConfigReader(Database.DBPrefix & "_TOPICS_BGCOLOR2").ToString()
					txtDMGTableborderColor.text = ConfigReader(Database.DBPrefix & "_TABLEBORDER_COLOR").ToString()
					txtDMGScrollbarColor.text = ConfigReader(Database.DBPrefix & "_SCROLLBAR_COLOR").ToString()
				End While
			ConfigReader.Close()
		End Sub

		Sub SubmitColorConfig(sender As System.Object, e As System.EventArgs)
			Dim TemplateID as Integer = sender.CommandArgument
			Dim TemplateName as String = Functions.RepairString(txtDMGTemplateName.text, 0)
			Dim FontFace as String = Functions.RepairString(txtDMGFontFace.Text, 0)
			Dim FontSize as String = CLng(txtDMGFontSize.Text)
			Dim ButtonColor as String = Functions.RepairString(txtDMGButtonColor.Text, 0)
			Dim LoginFontColor as String = Functions.RepairString(txtDMGLoginFontColor.Text, 0)
			Dim HeaderSize as String = CLng(txtDMGHeaderSize.Text)
			Dim HeaderColor as String = Functions.RepairString(txtDMGHeaderColor.Text, 0)
			Dim HeaderFontColor as String = Functions.RepairString(txtDMGHeaderFontColor.Text, 0)
			Dim SubheaderColor as String = Functions.RepairString(txtDMGSubheaderColor.Text, 0)
			Dim SubheaderFontColor as String = Functions.RepairString(txtDMGSubheaderFontColor.Text, 0)
			Dim FooterSize as String = CLng(txtDMGFooterSize.Text)
			Dim FooterColor as String = Functions.RepairString(txtDMGFooterColor.Text, 0)
			Dim FooterFontColor as String = Functions.RepairString(txtDMGFooterFontColor.Text, 0)
			Dim BGColor as String = Functions.RepairString(txtDMGBGColor.Text, 0)
			Dim BGImage as String = Functions.RepairString(txtDMGBGImage2.Text, 0)
			Dim FontColor as String = Functions.RepairString(txtDMGFontColor.Text, 0)
			Dim LinkColor as String = Functions.RepairString(txtDMGLinkColor.Text, 0)
			Dim LinkDecoration as String = Functions.RepairString(txtDMGLinkDecoration.Text, 0)
			Dim LinkVisitedColor as String = Functions.RepairString(txtDMGLinkVisitedColor.Text, 0)
			Dim LinkVisitedDecoration as String = Functions.RepairString(txtDMGLinkVisitedDecoration.Text, 0)
			Dim LinkActiveColor as String = Functions.RepairString(txtDMGLinkActiveColor.Text, 0)
			Dim LinkActiveDecoration as String = Functions.RepairString(txtDMGLinkActiveDecoration.Text, 0)
			Dim LinkHoverColor as String = Functions.RepairString(txtDMGLinkHoverColor.Text, 0)
			Dim LinkHoverDecoration as String = Functions.RepairString(txtDMGLinkHoverDecoration.Text, 0)
			Dim TopicsFontSize as String = cLng(txtDMGTopicsFontSize.Text)
			Dim TopicsFontColor as String = Functions.RepairString(txtDMGTopicsFontColor.Text, 0)
			Dim TopicsBGColor1 as String = Functions.RepairString(txtDMGTopicsBGColor1.Text, 0)
			Dim TopicsBGColor2 as String = Functions.RepairString(txtDMGTopicsBGColor2.Text)
			Dim TableborderColor as String = Functions.RepairString(txtDMGTableborderColor.Text, 0)
			Dim ScrollbarColor as String = Functions.RepairString(txtDMGScrollbarColor.Text, 0)
			Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_TEMPLATE_NAME = '" & TemplateName & "', " & Database.DBPrefix & "_FONTFACE = '" & FontFace & "', " & Database.DBPrefix & "_FONTSIZE = " & FontSize & ", " & Database.DBPrefix & "_BUTTON_COLOR = '" & ButtonColor & "', " & Database.DBPrefix & "_LOGIN_FONTCOLOR = '" & LoginFontColor & "', " & Database.DBPrefix & "_HEADER_SIZE = " & HeaderSize & ", " & Database.DBPrefix & "_HEADER_COLOR = '" & HeaderColor & "', " & Database.DBPrefix & "_HEADER_FONTCOLOR = '" & HeaderFontColor & "', " & Database.DBPrefix & "_SUBHEADER_COLOR = '" & SubheaderColor & "', " & Database.DBPrefix & "_SUBHEADER_FONTCOLOR = '" & SubheaderFontColor & "', " & Database.DBPrefix & "_FOOTER_SIZE = " & FooterSize & ", " & Database.DBPrefix & "_FOOTER_COLOR = '" & FooterColor & "', " & Database.DBPrefix & "_FOOTER_FONTCOLOR = '" & FooterFontColor & "', " & Database.DBPrefix & "_BGCOLOR = '" & BGColor & "', " & Database.DBPrefix & "_BGIMAGE = '" & BGImage & "', " & Database.DBPrefix & "_FONT_COLOR = '" & FontColor & "', " & Database.DBPrefix & "_LINK_COLOR = '" & LinkColor & "', " & Database.DBPrefix & "_LINK_DECORATION = '" & LinkDecoration & "', " & Database.DBPrefix & "_LINK_VISITED_COLOR = '" & LinkVisitedColor & "', " & Database.DBPrefix & "_LINK_VISITED_DECORATION = '" & LinkVisitedDecoration & "', " & Database.DBPrefix & "_LINK_ACTIVE_COLOR = '" & LinkActiveColor & "', " & Database.DBPrefix & "_LINK_ACTIVE_DECORATION = '" & LinkActiveDecoration & "', " & Database.DBPrefix & "_LINK_HOVER_COLOR = '" & LinkHoverColor & "', " & Database.DBPrefix & "_LINK_HOVER_DECORATION = '" & LinkHoverDecoration & "', " & Database.DBPrefix & "_TOPICS_FONTSIZE = " & TopicsFontSize & ", " & Database.DBPrefix & "_TOPICS_FONTCOLOR = '" & TopicsFontColor & "', " & Database.DBPrefix & "_TOPICS_BGCOLOR1 = '" & TopicsBGColor1 & "', " & Database.DBPrefix & "_TOPICS_BGCOLOR2 = '" & TopicsBGColor2 & "', " & Database.DBPrefix & "_TABLEBORDER_COLOR = '" & TableborderColor & "', " & Database.DBPrefix & "_SCROLLBAR_COLOR = '" & ScrollbarColor & "' WHERE ID = " & TemplateID)

			if (txtSetDefaultTemplate.SelectedValue = "1") then
				Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_TEMPLATE_DEFAULT = " & TemplateID)
			end if

			PagePanel.Visible = "false"
			NoItemsDiv.InnerHtml = "Colors & Fonts Configuration Edited.<br /><a href=""admin.aspx"">Click Here</a> To Return To The Admin Page.<br /><br />"
		End Sub

		Sub SubmitCustomHTML(sender As System.Object, e As System.EventArgs)
			Dim MarginSide as String = "0"
			Dim MarginTop as String = "0"

			Dim TemplateID as Integer = sender.CommandArgument
			Dim TemplateName as String = Functions.RepairString(txtDMGTemplateName2.text, 0)
			Dim CustomHeader as String = Functions.RepairString(txtDMGCustomHeader.Text, 0)
			Dim CustomFooter as String = Functions.RepairString(txtDMGCustomFooter.Text, 0)
			Dim CustomCSS as String = Functions.RepairString(txtDMGCustomCSS.Text, 0)
			Dim BackgroundImage as String = Functions.RepairString(txtDMGBGImage.text)

			if (Functions.IsInteger(txtDMGMarginSide.Text)) then
				MarginSide = cLng(txtDMGMarginSide.Text)
			end if
			if (Functions.IsInteger(txtDMGMarginTop.Text)) then
				MarginTop = cLng(txtDMGMarginTop.Text)
			end if

			Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_TEMPLATE_NAME = '" & TemplateName & "', " & Database.DBPrefix & "_CUSTOM_HEADER = '" & CustomHeader & "', " & Database.DBPrefix & "_CUSTOM_FOOTER = '" & CustomFooter & "', " & Database.DBPrefix & "_CUSTOM_CSS = '" & CustomCSS & "', " & Database.DBPrefix & "_MARGIN_SIDE = " & MarginSide & ", " & Database.DBPrefix & "_BGIMAGE = '" & BackgroundImage & "', " & Database.DBPrefix & "_MARGIN_TOP = " & MarginTop & " WHERE ID = " & TemplateID)

			if (txtSetDefaultTemplate2.SelectedValue = "1") then
				Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_TEMPLATE_DEFAULT = " & TemplateID)
			end if

			PagePanel.Visible = "false"
			NoItemsDiv.InnerHtml = "Custom HTML Edited.<br /><a href=""admin.aspx"">Click Here</a> To Return To The Admin Page.<br /><br />"
		End Sub

		Sub SubmitEmailConfig(sender As System.Object, e As System.EventArgs)
			Dim EmailSmtp as String = Functions.RepairString(txtDMGEmailSmtp.Text, 0)
			Dim EmailPort as String = Functions.RepairString(txtDMGEmailPort.Text, 0)
			Dim EmailUsername as String = Functions.RepairString(txtDMGEmailUsername.Text, 0)
			Dim EmailPassword as String = Functions.RepairString(txtDMGEmailPassword.Text, 0)
			Dim EmailAddress as String = Functions.RepairString(txtDMGEmailAddress.Text, 0)
			Dim EmailAllowSend as Integer = txtDMGEmailAllowSend.SelectedValue
			Dim EmailAllowSub as Integer = txtDMGEmailAllowSub.SelectedValue

			Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_EMAIL_SMTP = '" & EmailSmtp & "', " & Database.DBPrefix & "_EMAIL_PORT = '" & EmailPort & "', " & Database.DBPrefix & "_EMAIL_USERNAME = '" & EmailUsername & "', " & Database.DBPrefix & "_EMAIL_PASSWORD = '" & EmailPassword & "', " & Database.DBPrefix & "_EMAIL_ADDRESS = '" & EmailAddress & "', " & Database.DBPrefix & "_EMAIL_ALLOWSEND = " & EmailAllowSend & ", " & Database.DBPrefix & "_EMAIL_ALLOWSUB = " & EmailAllowSub)
			PagePanel.Visible = "false"
			NoItemsDiv.InnerHtml = "E-Mail Configuration Edited.<br /><a href=""admin.aspx"">Click Here</a> To Return To The Admin Page.<br /><br />"
		End Sub

		Sub SubmitVariableConfig(sender As System.Object, e As System.EventArgs)
			Dim Var1 as String = Functions.RepairString(txtDMGVar1.text, 0)
			Dim Var2 as String = Functions.RepairString(txtDMGVar2.text, 0)
			Dim Var3 as String = Functions.RepairString(txtDMGVar3.text, 0)
			Dim Var4 as String = Functions.RepairString(txtDMGVar4.text, 0)
			Dim Var5 as String = Functions.RepairString(txtDMGVar5.text, 0)
			Dim Text1 as String = Functions.RepairString(txtDMGText1.text, 0)
			Dim Text2 as String = Functions.RepairString(txtDMGText2.text, 0)
			Dim Text3 as String = Functions.RepairString(txtDMGText3.text, 0)
			Dim Text4 as String = Functions.RepairString(txtDMGText4.text, 0)
			Dim Text5 as String = Functions.RepairString(txtDMGText5.text, 0)
			Database.Write("UPDATE " & Database.DBPrefix & "_VARIABLES SET VAR1 = '" & Var1 & "', VAR2 = '" & Var2 & "', VAR3 = '" & Var3 & "', VAR4 = '" & Var4 & "', VAR5 = '" & Var5 & "', TEXT1 = '" & Text1 & "', TEXT2 = '" & Text2 & "', TEXT3 = '" & Text3 & "', TEXT4 = '" & Text4 & "', TEXT5 = '" & Text5 & "' WHERE ID = 1")
			PagePanel.Visible = "false"
			NoItemsDiv.InnerHtml = "Custom Variables Edited.<br /><a href=""admin.aspx"">Click Here</a> To Return To The Admin Page.<br /><br />"
		End Sub

		Sub SubmitSearchConfig(sender As System.Object, e As System.EventArgs)
			Dim SearchTopics as Integer = cLng(txtDMGSearchTopics.SelectedValue)
			Dim SearchMembers as Integer = cLng(txtDMGSearchMembers.SelectedValue)
			Dim SearchBlogs as Integer = cLng(txtDMGSearchBlogs.SelectedValue)
			Dim SearchPages as Integer = cLng(txtDMGSearchPages.SelectedValue)

			Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_SEARCH_TOPICS = " & SearchTopics & ", " & Database.DBPrefix & "_SEARCH_MEMBERS = " & SearchMembers & ", " & Database.DBPrefix & "_SEARCH_BLOGS = " & SearchBlogs & ", " & Database.DBPrefix & "_SEARCH_PAGES = " & SearchPages)
			PagePanel.Visible = "false"
			NoItemsDiv.InnerHtml = "Search Configuration Saved.<br /><a href=""admin.aspx"">Click Here</a> To Return To The Admin Page.<br /><br />"
		End Sub

		Sub SubmitNewTemplate(sender As System.Object, e As System.EventArgs)
			Dim NewDefaultTemplate as String = Settings.DefaultTemplate
			Dim Title as String = Functions.RepairString(Settings.PageTitle, 0)
			Dim Copyright as String = Functions.RepairString(Settings.Copyright, 0)
			Dim Logo as String = Functions.RepairString(Settings.ForumLogo, 0)
			Dim URL as String = Functions.RepairString(Settings.SiteURL, 0)
			Dim ItemsPerPage as String = CLng(Settings.ItemsPerPage)
			Dim SpamFilter as String = CLng(Settings.SpamFilter)
			Dim TemplateName as String = Functions.RepairString(txtDMGNewTemplateName.Text, 0)
			Dim FontFace as String = Functions.RepairString(txtDMGNewFontFace.Text, 0)
			Dim FontSize as String = CLng(txtDMGNewFontSize.Text)
			Dim ButtonColor as String = Functions.RepairString(txtDMGNewButtonColor.Text, 0)
			Dim LoginFontColor as String = Functions.RepairString(txtDMGNewLoginFontColor.Text, 0)
			Dim HeaderSize as String = CLng(txtDMGNewHeaderSize.Text)
			Dim HeaderColor as String = Functions.RepairString(txtDMGNewHeaderColor.Text, 0)
			Dim HeaderFontColor as String = Functions.RepairString(txtDMGNewHeaderFontColor.Text, 0)
			Dim SubheaderColor as String = Functions.RepairString(txtDMGNewSubheaderColor.Text, 0)
			Dim SubheaderFontColor as String = Functions.RepairString(txtDMGNewSubheaderFontColor.Text, 0)
			Dim FooterSize as String = CLng(txtDMGNewFooterSize.Text)
			Dim FooterColor as String = Functions.RepairString(txtDMGNewFooterColor.Text, 0)
			Dim FooterFontColor as String = Functions.RepairString(txtDMGNewFooterFontColor.Text, 0)
			Dim BGColor as String = Functions.RepairString(txtDMGNewBGColor.Text, 0)
			Dim BGImage as String = Functions.RepairString(txtDMGNewBGImage.Text, 0)
			Dim FontColor as String = Functions.RepairString(txtDMGNewFontColor.Text, 0)
			Dim LinkColor as String = Functions.RepairString(txtDMGNewLinkColor.Text, 0)
			Dim LinkDecoration as String = Functions.RepairString(txtDMGNewLinkDecoration.Text, 0)
			Dim LinkVisitedColor as String = Functions.RepairString(txtDMGNewLinkVisitedColor.Text, 0)
			Dim LinkVisitedDecoration as String = Functions.RepairString(txtDMGNewLinkVisitedDecoration.Text, 0)
			Dim LinkActiveColor as String = Functions.RepairString(txtDMGNewLinkActiveColor.Text, 0)
			Dim LinkActiveDecoration as String = Functions.RepairString(txtDMGNewLinkActiveDecoration.Text, 0)
			Dim LinkHoverColor as String = Functions.RepairString(txtDMGNewLinkHoverColor.Text, 0)
			Dim LinkHoverDecoration as String = Functions.RepairString(txtDMGNewLinkHoverDecoration.Text, 0)
			Dim TopicsFontSize as String = CLng(txtDMGNewTopicsFontSize.Text)
			Dim TopicsFontColor as String = Functions.RepairString(txtDMGNewTopicsFontColor.Text, 0)
			Dim TopicsBGColor1 as String = Functions.RepairString(txtDMGNewTopicsBGColor1.Text, 0)
			Dim TopicsBGColor2 as String = Functions.RepairString(txtDMGNewTopicsBGColor2.Text, 0)
			Dim TableborderColor as String = Functions.RepairString(txtDMGNewTableborderColor.Text, 0)
			Dim ScrollbarColor as String = Functions.RepairString(txtDMGNewScrollbarColor.Text, 0)
			Dim CustomHeader as String = Functions.RepairString(txtDMGNewCustomHeader.Text, 0)
			Dim CustomFooter as String = Functions.RepairString(txtDMGNewCustomFooter.Text, 0)
			Dim CustomCSS as String = Functions.RepairString(txtDMGNewCustomCSS.Text, 0)
			Dim MarginSide as String = cLng(txtDMGNewMarginSide.Text)
			Dim MarginTop as String = cLng(txtDMGNewMarginTop.Text)
			Dim NewForumsDefault as String = Settings.ForumsDefault
			Dim NewShowStatistics as String = Settings.ShowStatistics
			Dim NewCustomMeta as String = Settings.MetaKeywords
			Dim NewMemberValidation as Integer = Settings.MemberValidation
			Dim NewEnableEmailWelcomeMessage as Integer = Settings.EmailWelcomeMessage
			Dim NewEmailSmtp as String = Settings.EmailSmtp
			Dim NewEmailAddress as String = Settings.EmailAddress
			Dim NewEmailAllowSend as Integer = Settings.EmailAllowSend
			Dim NewEmailAllowSub as Integer = Settings.EmailAllowSub
			Dim NewEmailPort as String = Settings.EmailPort
			Dim NewEmailUsername as String = Settings.EmailUsername
			Dim NewEmailPassword as String = Settings.EmailPassword
			Dim NewAllowSub as Integer = Settings.AllowSub
			Dim NewQuickReg as Integer = Settings.QuickRegistration
			Dim NewCurseFilter as Integer = Settings.CurseFilter
			Dim NewRSSFeeds as Integer = Settings.RSSFeeds
			Dim NewHorizDivide as String = Settings.HorizDivide
			Dim NewVertDivide as String = Settings.VertDivide
			Dim NewAllowEdits as Integer = Settings.AllowEdits
			Dim NewAllowRegistration as Integer = Settings.AllowRegistration
			Dim NewAllowMedia as Integer = Settings.AllowMedia
			Dim NewAllowReporting as Integer = Settings.AllowReporting
			Dim NewHideMembers as Integer = Settings.HideMembers
			Dim NewHideLogin as Integer = Settings.HideLogin
			Dim NewMemberPhotoSize as Integer = Settings.MemberPhotoSize
			Dim NewHtmlTitle as String = Settings.HtmlTitle
			Dim NewThumbnailSize as Integer = Settings.ThumbnailSize
			Dim NewAvatarSize as Integer = Settings.AvatarSize
			Dim NewSearchTopics as Integer = Settings.SearchTopics
			Dim NewSearchMembers as Integer = Settings.SearchMembers
			Dim NewSearchBlogs as Integer = Settings.SearchBlogs
			Dim NewSearchPages as Integer = Settings.SearchPages
			Dim NewMemberFileTypes as String = ""
			Dim NewTopicUploadSize as Integer = 1024

			Dim Reader as OdbcDataReader = Database.Read("SELECT " & Database.DBPrefix & "_MEMBER_FILETYPES, " & Database.DBPrefix & "_TOPIC_UPLOADSIZE FROM " & Database.DBPrefix & "_SETTINGS", 1)
			While Reader.Read()
				NewMemberFileTypes = Reader(Database.DBPrefix & "_MEMBER_FILETYPES").ToString()
				NewTopicUploadSize = Reader(Database.DBPrefix & "_TOPIC_UPLOADSIZE").ToString()
			End While
			Reader.Close()

			Database.Write("INSERT INTO " & Database.DBPrefix & "_SETTINGS (" & Database.DBPrefix & "_TEMPLATE_DEFAULT, " & Database.DBPrefix & "_TITLE, " & Database.DBPrefix & "_COPYRIGHT, " & Database.DBPrefix & "_LOGO, " & Database.DBPrefix & "_URL, " & Database.DBPrefix & "_ITEMS_PER_PAGE, " & Database.DBPrefix & "_SPAM_FILTER, " & Database.DBPrefix & "_TEMPLATE_NAME, " & Database.DBPrefix & "_FONTFACE, " & Database.DBPrefix & "_FONTSIZE, " & Database.DBPrefix & "_BUTTON_COLOR, " & Database.DBPrefix & "_LOGIN_FONTCOLOR, " & Database.DBPrefix & "_HEADER_SIZE, " & Database.DBPrefix & "_HEADER_COLOR, " & Database.DBPrefix & "_HEADER_FONTCOLOR, " & Database.DBPrefix & "_SUBHEADER_COLOR, " & Database.DBPrefix & "_SUBHEADER_FONTCOLOR, " & Database.DBPrefix & "_FOOTER_SIZE, " & Database.DBPrefix & "_FOOTER_COLOR, " & Database.DBPrefix & "_FOOTER_FONTCOLOR, " & Database.DBPrefix & "_BGCOLOR, " & Database.DBPrefix & "_FONT_COLOR, " & Database.DBPrefix & "_LINK_COLOR, " & Database.DBPrefix & "_LINK_DECORATION, " & Database.DBPrefix & "_LINK_VISITED_COLOR, " & Database.DBPrefix & "_LINK_VISITED_DECORATION, " & Database.DBPrefix & "_LINK_ACTIVE_COLOR, " & Database.DBPrefix & "_LINK_ACTIVE_DECORATION, " & Database.DBPrefix & "_LINK_HOVER_COLOR, " & Database.DBPrefix & "_LINK_HOVER_DECORATION, " & Database.DBPrefix & "_TOPICS_FONTSIZE, " & Database.DBPrefix & "_TOPICS_FONTCOLOR, " & Database.DBPrefix & "_TOPICS_BGCOLOR1, " & Database.DBPrefix & "_TOPICS_BGCOLOR2, " & Database.DBPrefix & "_TABLEBORDER_COLOR, " & Database.DBPrefix & "_SCROLLBAR_COLOR, " & Database.DBPrefix & "_CUSTOM_HEADER, " & Database.DBPrefix & "_CUSTOM_FOOTER, " & Database.DBPrefix & "_CUSTOM_CSS, " & Database.DBPrefix & "_FORUMS_DEFAULT, " & Database.DBPrefix & "_MARGIN_SIDE, " & Database.DBPrefix & "_MARGIN_TOP, " & Database.DBPrefix & "_SHOWSTATISTICS, " & Database.DBPrefix & "_CUSTOM_META, " & Database.DBPrefix & "_BGIMAGE, " & Database.DBPrefix & "_MEMBER_VALIDATION, " & Database.DBPrefix & "_EMAIL_SMTP, " & Database.DBPrefix & "_EMAIL_ADDRESS, " & Database.DBPrefix & "_EMAIL_ALLOWSEND, " & Database.DBPrefix & "_EMAIL_ALLOWSUB, " & Database.DBPrefix & "_EMAIL_PORT, " & Database.DBPrefix & "_EMAIL_USERNAME, " & Database.DBPrefix & "_EMAIL_PASSWORD, " & Database.DBPrefix & "_ALLOWSUB, " & Database.DBPrefix & "_QUICK_REGISTRATION, " & Database.DBPrefix & "_CURSE_FILTER, " & Database.DBPrefix & "_RSS_FEEDS, " & Database.DBPrefix & "_HORIZ_DIVIDE, " & Database.DBPrefix & "_VERT_DIVIDE, " & Database.DBPrefix & "_ALLOW_EDITS, " & Database.DBPrefix & "_ALLOW_REGISTRATION, " & Database.DBPrefix & "_ALLOW_MEDIA, " & Database.DBPrefix & "_ALLOW_REPORTING, " & Database.DBPrefix & "_HIDE_MEMBERS, " & Database.DBPrefix & "_HIDE_LOGIN, " & Database.DBPrefix & "_MEMBER_PHOTOSIZE, " & Database.DBPrefix & "_HTML_TITLE, " & Database.DBPrefix & "_THUMBNAIL_SIZE, " & Database.DBPrefix & "_AVATAR_SIZE, " & Database.DBPrefix & "_SEARCH_TOPICS, " & Database.DBPrefix & "_SEARCH_MEMBERS, " & Database.DBPrefix & "_SEARCH_BLOGS, " & Database.DBPrefix & "_SEARCH_PAGES, " & Database.DBPrefix & "_MEMBER_FILETYPES, " & Database.DBPrefix & "_TOPIC_UPLOADSIZE, " & Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE) VALUES (" & NewDefaultTemplate & ", '" & Title & "', '" & Copyright & "', '" & Logo & "', '" & URL & "', " & ItemsPerPage & ", " & SpamFilter & ", '" & TemplateName & "', '" & FontFace & "', " & FontSize & ", '" & ButtonColor & "', '" & LoginFontColor & "', " & HeaderSize & ", '" & HeaderColor & "', '" & HeaderFontColor & "', '" & SubheaderColor & "', '" & SubheaderFontcolor & "', " & FooterSize & ", '" & FooterColor & "', '" & FooterFontColor & "', '" & BGColor & "', '" & FontColor & "', '" & LinkColor & "', '" & LinkDecoration & "', '" & LinkVisitedColor & "', '" & LinkVisitedDecoration & "', '" & LinkActiveColor & "', '" & LinkActiveDecoration & "', '" & LinkHoverColor & "', '" & LinkHoverDecoration & "', " & TopicsFontSize & ", '" & TopicsFontColor & "', '" & TopicsBGColor1 & "', '" & TopicsBGColor2 & "', '" & TableborderColor & "', '" & ScrollbarColor & "', '" & CustomHeader & "', '" & CustomFooter & "', '" & CustomCSS & "', " & NewForumsDefault & ", " & MarginSide & ", " & MarginTop & ", " & NewShowStatistics & ", '" & NewCustomMeta & "', '" & BGImage & "', " & NewMemberValidation & ", '" & NewEmailSmtp & "', '" & NewEmailAddress & "', " & NewEmailAllowSend & ", " & NewEmailAllowSub & ", '" & NewEmailPort & "', '" & NewEmailUsername & "', '" & NewEmailPassword & "', " & NewAllowSub & ", " & NewQuickReg & ", " & NewCurseFilter & ", " & NewRSSFeeds & ", '" & NewHorizDivide & "', '" & NewVertDivide & "', " & NewAllowEdits & ", " & NewAllowRegistration & ", " & NewAllowMedia & ", " & NewAllowReporting & ", " & NewHideMembers & ", " & NewHideLogin & ", " & NewMemberPhotoSize & ", '" & NewHtmlTitle & "', " & NewThumbnailSize & ", " & NewAvatarSize & ", " & NewSearchTopics & ", " & NewSearchMembers & ", " & NewSearchBlogs & ", " & NewSearchPages & ", '" & NewMemberFileTypes & "', " & NewTopicUploadSize & ", " & NewEnableEmailWelcomeMessage & ")")

			if (txtNewSetDefaultTemplate.SelectedValue = "1") then
				Dim DefaultTemplateReader as OdbcDataReader = Database.Read("SELECT ID FROM " & Database.DBPrefix & "_SETTINGS ORDER BY ID DESC", 1)
				While DefaultTemplateReader.Read()
					Database.Write("UPDATE " & Database.DBPrefix & "_SETTINGS SET " & Database.DBPrefix & "_TEMPLATE_DEFAULT = " & DefaultTemplateReader("ID"))
				End While
				DefaultTemplateReader.Close()
			end if

			PagePanel.Visible = "false"
			NoItemsDiv.InnerHtml = "New Template Created Succesfully.<br /><a href=""admin.aspx"">Click Here</a> To Return To The Admin Page.<br /><br />"
		End Sub

		Sub CreateNewPage(sender As System.Object, e As System.EventArgs)
			Response.Redirect("newpage.aspx")
		End Sub

		Sub EditPages(sender As System.Object, e As System.EventArgs)
			Response.Redirect("editpages.aspx?ID=0")
		End Sub

		Sub EditCategory(sender As System.Object, e As System.EventArgs)
			Response.Redirect("editcategory.aspx?ID=" & sender.SelectedValue.ToString())
		End Sub

		Sub EditForum(sender As System.Object, e As System.EventArgs)
			Response.Redirect("editforum.aspx?ID=" & sender.SelectedValue.ToString())
		End Sub

		Sub NewForum(sender As System.Object, e As System.EventArgs)
			Response.Redirect("newforum.aspx?ID=" & sender.SelectedValue.ToString())
		End Sub

		Sub GoToFileManager(sender As System.Object, e As System.EventArgs)
			Response.Redirect("admin_filemanager.aspx?ID=0")
		End Sub

		Sub CreateNewMember(sender As System.Object, e As System.EventArgs)
			Response.Redirect("admin_createmember.aspx")
		End Sub

		Sub SubmitMemberSearch(sender As System.Object, e As System.EventArgs)
			AdminLinks.visible = "false"
			AdminEditMembers.visible = "true"
			Dim SearchString as String = Functions.RepairString(MemberSearch.Text)
			MemberList.Datasource = Database.Read("SELECT MEMBER_ID, MEMBER_USERNAME, MEMBER_EMAIL, MEMBER_IP_ORIGINAL, MEMBER_IP_LAST, MEMBER_DATE_JOINED, MEMBER_DATE_LASTVISIT FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_USERNAME LIKE '%" & SearchString & "%' AND MEMBER_LEVEL <> -1 ORDER BY MEMBER_USERNAME")
			MemberList.Databind()
			MemberList.Datasource.Close()
		End Sub

		Sub SubmitMemberVerification(sender As System.Object, e As System.EventArgs)
			AdminVerifyMembers.visible = "false"
			AdminVerifyMembersConfirm.visible = "true"
			VerifyYesButton.CommandArgument = sender.CommandArgument
			VerifyNoButton.CommandArgument = sender.CommandArgument
		End Sub

		Sub SubmitMemberVerificationEmail(sender As System.Object, e As System.EventArgs)
			Database.Write("UPDATE " & Database.DBPrefix & "_MEMBERS SET MEMBER_LEVEL = 1, MEMBER_VALIDATED = 1 WHERE MEMBER_ID = " & sender.CommandArgument)
			PagePanel.Visible = "false"

			Dim EmailAdd as String = ""
			Dim Reader as OdbcDataReader = Database.Read("SELECT MEMBER_EMAIL FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & sender.CommandArgument, 1)
			While Reader.Read()
				EmailAdd = Reader("MEMBER_EMAIL").ToString()
			End While
			Reader.Close()

			Dim err As Integer = Functions.SendEmail(EmailAdd, Settings.EmailAddress, "Thank you for registering at " & Settings.PageTitle, Functions.CustomMessage("EMAIL_ADMINAPPROVAL"))
			if (err = 0) then
				NoItemsDiv.InnerHtml = "Member Verified Succesfully.<br /><a href=""admin.aspx"">Click Here</a> To Return To The Admin Page.<br /><br />"
			else
				NoItemsDiv.InnerHtml = "The member was verified, however, the e-mail could not be delivered.<br /><br />There is either a problem with your mail setup or the user's e-mail address is not valid.<br /><br /><a href=""admin.aspx"">Click Here</a> To Return To The Admin Page.<br /><br />"
			end if
		End Sub

		Sub SubmitMemberVerificationNoEmail(sender As System.Object, e As System.EventArgs)
			Database.Write("UPDATE " & Database.DBPrefix & "_MEMBERS SET MEMBER_LEVEL = 1, MEMBER_VALIDATED = 1 WHERE MEMBER_ID = " & sender.CommandArgument)
			PagePanel.Visible = "false"
			NoItemsDiv.InnerHtml = "Member Verified Succesfully.<br /><a href=""admin.aspx"">Click Here</a> To Return To The Admin Page.<br /><br />"
		End Sub

		Sub DeleteMemberVerification(sender As System.Object, e As System.EventArgs)
			Database.Write("DELETE FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & sender.CommandArgument)
			PagePanel.Visible = "false"
			NoItemsDiv.InnerHtml = "Member Deleted Succesfully.<br /><a href=""admin.aspx"">Click Here</a> To Return To The Admin Page.<br /><br />"
		End Sub

		Sub DeleteAvatar(sender As System.Object, e As System.EventArgs)
			Dim ID as String = sender.CommandArgument
			Dim AvatarReader as OdbcDataReader = Database.Read("SELECT AVATAR_IMAGE FROM " & Database.DBPrefix & "_AVATARS WHERE AVATAR_ID = " & ID)
				While AvatarReader.Read()
					Database.Write("DELETE " & Database.DBPrefix & "_FILES FROM " & Database.DBPrefix & "_FILES, " & Database.DBPrefix & "_FOLDERS WHERE " & Database.DBPrefix & "_FILES.FILE_FOLDER = " & Database.DBPrefix & "_FOLDERS.FOLDER_ID AND " & Database.DBPrefix & "_FILES.FILE_NAME = '" & AvatarReader("AVATAR_IMAGE") & "' AND " & Database.DBPrefix & "_FOLDERS.FOLDER_NAME = 'avatars' AND " & Database.DBPrefix & "_FOLDERS.FOLDER_PARENT = 0")
					File.Delete(MapPath("avatars/" & AvatarReader("AVATAR_IMAGE")))
				End While
			AvatarReader.Close()
			Database.Write("DELETE FROM " & Database.DBPrefix & "_AVATARS WHERE AVATAR_ID = " & ID)
			Database.Write("UPDATE " & Database.DBPrefix & "_MEMBERS SET MEMBER_AVATAR = 1 WHERE MEMBER_AVATAR = " & ID)
	
			AdminLinks.visible = "false"
			AdminAvatarConfig.visible = "true"
			Avatars.Datasource = Database.Read("SELECT AVATAR_ID, AVATAR_IMAGE, AVATAR_NAME FROM " & Database.DBPrefix & "_AVATARS ORDER BY AVATAR_NAME")
			Avatars.Databind()
			Avatars.Datasource.Close()
		End Sub

		Sub EditRanking(sender As System.Object, e As System.EventArgs)
			AdminLinks.visible = "false"
			AdminRankingConfig.visible = "true"

			Dim Reader as OdbcDataReader = Database.Read("SELECT RANK_ID FROM " & Database.DBPrefix & "_RANKINGS ORDER BY RANK_ID")
			While Reader.Read()
				Database.Write("UPDATE " & Database.DBPrefix & "_RANKINGS SET RANK_NAME = '" & Request.Form("txtRankName" & Reader("RANK_ID")) & "', RANK_POSTS = " & Request.Form("txtRankPosts" & Reader("RANK_ID")) & ", RANK_ALLOW_TOPICS = " & Request.Form("txtRankAllowTopics" & Reader("RANK_ID")) & ", RANK_ALLOW_AVATAR = " & Request.Form("txtRankAllowAvatar" & Reader("RANK_ID")) & ", RANK_ALLOW_AVATAR_CUSTOM = " & Request.Form("txtRankAllowAvatarCustom" & Reader("RANK_ID")) & ", RANK_ALLOW_TITLE = " & Request.Form("txtRankAllowTitle" & Reader("RANK_ID")) & ", RANK_ALLOW_UPLOADS = " & Request.Form("txtRankAllowUploads" & Reader("RANK_ID")) & " WHERE RANK_ID = " & Reader("RANK_ID"))
			End While
			Reader.Close()

			if ((NewRankName.Text <> "") and (Functions.IsInteger(NewRankPosts.Text))) then
				Dim RankName as String = Functions.Repairstring(NewRankName.Text, 0)
				Database.Write("INSERT INTO " & Database.DBPrefix & "_RANKINGS (RANK_NAME, RANK_POSTS, RANK_IMAGE, RANK_ALLOW_TOPICS, RANK_ALLOW_AVATAR, RANK_ALLOW_AVATAR_CUSTOM, RANK_ALLOW_TITLE, RANK_ALLOW_UPLOADS) VALUES ('" & RankName & "', " & NewRankPosts.Text & ", '', " & NewRankAllowTopics.SelectedValue & ", " & NewRankAllowAvatar.SelectedValue & ", " & NewRankAllowAvatarCustom.SelectedValue & ", " & NewRankAllowTitle.SelectedValue & ", " & NewRankAllowUploads.SelectedValue & ")")
			end if

			RankList.Datasource = Database.Read("SELECT * FROM " & Database.DBPrefix & "_RANKINGS ORDER BY RANK_POSTS")
			RankList.Databind()
			RankList.Datasource.Close()

			NewRankName.Text = ""
			NewRankPosts.Text = ""
		End Sub

		Sub DeleteRanking(sender As System.Object, e As System.EventArgs)
			AdminLinks.visible = "false"
			AdminRankingConfig.visible = "true"
			Database.Write("DELETE FROM " & Database.DBPrefix & "_RANKINGS WHERE RANK_ID = " & sender.CommandArgument)
			Database.Write("UPDATE " & Database.DBPrefix & "_MEMBERS SET MEMBER_RANKING = 0 WHERE MEMBER_RANKING = " & sender.CommandArgument)
			RankList.Datasource = Database.Read("SELECT * FROM " & Database.DBPrefix & "_RANKINGS ORDER BY RANK_POSTS")
			RankList.Databind()
			RankList.Datasource.Close()
		End Sub

		Sub NewRanking(sender As System.Object, e As System.EventArgs)
			AdminLinks.visible = "false"
			AdminRankingConfig.visible = "true"
			Dim RankName as String = Functions.Repairstring(NewRankName.Text, 0)
			Dim intRankPosts as Integer = 0
			if (Functions.IsInteger(NewRankPosts.Text)) then
				intRankPosts = NewRankPosts.Text
			end if
			Database.Write("INSERT INTO " & Database.DBPrefix & "_RANKINGS (RANK_NAME, RANK_POSTS, RANK_IMAGE, RANK_ALLOW_TOPICS, RANK_ALLOW_AVATAR, RANK_ALLOW_AVATAR_CUSTOM, RANK_ALLOW_TITLE, RANK_ALLOW_UPLOADS) VALUES ('" & RankName & "', " & intRankPosts & ", '', " & NewRankAllowTopics.SelectedValue & ", " & NewRankAllowAvatar.SelectedValue & ", " & NewRankAllowAvatarCustom.SelectedValue & ", " & NewRankAllowTitle.SelectedValue & ", " & NewRankAllowUploads.SelectedValue & ")")
			RankList.Datasource = Database.Read("SELECT * FROM " & Database.DBPrefix & "_RANKINGS ORDER BY RANK_POSTS")
			RankList.Databind()
			RankList.Datasource.Close()
			NewRankName.Text = ""
			NewRankPosts.Text = ""
		End Sub

		Sub UpdateCounts(sender As System.Object, e As System.EventArgs)
			Functions.UpdateCounts(1, 0, 0, 0)
			PagePanel.Visible = "false"
			NoItemsDiv.InnerHtml = "Forum Counts Updated.<br /><a href=""admin.aspx"">Click Here</a> To Return To The Admin Page.<br /><br />"
		End Sub

		Sub SaveCurseWord(sender As System.Object, e As System.EventArgs)
			AdminLinks.visible = "false"
			AdminCurseFilter.visible = "true"
			Database.Write("UPDATE " & Database.DBPrefix & "_CURSE_FILTER SET CURSE_WORD = '" & Request.Form("txtCurseWord" & sender.commandargument) & "', CURSE_REPLACEMENT = '" & Request.Form("txtCurseReplacement" & sender.commandargument) & "' WHERE CURSE_ID = " & sender.CommandArgument)
			CurseList.Datasource = Database.Read("SELECT CURSE_ID, CURSE_WORD, CURSE_REPLACEMENT FROM " & Database.DBPrefix & "_CURSE_FILTER ORDER BY CURSE_WORD")
			CurseList.Databind()
			CurseList.Datasource.Close()
		End Sub

		Sub DeleteCurseWord(sender As System.Object, e As System.EventArgs)
			AdminLinks.visible = "false"
			AdminCurseFilter.visible = "true"
			Database.Write("DELETE FROM " & Database.DBPrefix & "_CURSE_FILTER WHERE CURSE_ID = " & sender.CommandArgument)
			CurseList.Datasource = Database.Read("SELECT CURSE_ID, CURSE_WORD, CURSE_REPLACEMENT FROM " & Database.DBPrefix & "_CURSE_FILTER ORDER BY CURSE_WORD")
			CurseList.Databind()
			CurseList.Datasource.Close()
		End Sub

		Sub NewCurseWord(sender As System.Object, e As System.EventArgs)
			AdminLinks.visible = "false"
			AdminCurseFilter.visible = "true"
			Database.Write("INSERT INTO " & Database.DBPrefix & "_CURSE_FILTER (CURSE_WORD, CURSE_REPLACEMENT) VALUES ('" & NewCurse.text & "', '" & NewCurseReplacement.text & "')")
			CurseList.Datasource = Database.Read("SELECT CURSE_ID, CURSE_WORD, CURSE_REPLACEMENT FROM " & Database.DBPrefix & "_CURSE_FILTER ORDER BY CURSE_WORD")
			CurseList.Databind()
			CurseList.Datasource.Close()
			NewCurse.text = ""
			NewCurseReplacement.text = ""
		End Sub

		Sub CreateRotator(sender As System.Object, e As System.EventArgs)
			AdminLinks.visible = "false"
			AdminRotatorConfig.visible = "false"
			AdminRotatorNew.visible = "true"
		End Sub

		Sub SubmitNewRotator(sender As System.Object, e As System.EventArgs)
			AdminRotatorConfig.visible = "false"
			AdminRotatorNew.visible = "false"
			AdminRotatorNewConfirm.visible = "true"
			Database.Write("INSERT INTO " & Database.DBPrefix & "_ROTATOR (ROTATOR_NAME) VALUES ('" & Functions.RepairString(NewRotatorName.text, 0) & "')")
			Dim Reader as OdbcDataReader = Database.Read("SELECT ROTATOR_ID FROM " & Database.DBPrefix & "_ROTATOR WHERE ROTATOR_NAME = '" & Functions.RepairString(NewRotatorName.text, 0) & "' ORDER BY ROTATOR_ID DESC", 1)
			While Reader.Read()
				NewRotatorEdit.CommandArgument = Reader("ROTATOR_ID")
			End While
			Reader.Close()
		End Sub

		Sub DeleteRotator(sender As System.Object, e As System.EventArgs)
			AdminLinks.visible = "false"
			AdminRotatorConfig.visible = "false"
			AdminRotatorDelete.visible = "true"
			RotatorYesButton.CommandArgument = sender.CommandArgument
		End Sub

		Sub DeleteRotatorConfirm(sender As System.Object, e As System.EventArgs)
			Dim RotatorID as String = sender.CommandArgument

			Dim Reader as OdbcDataReader = Database.Read("SELECT IMAGE_ID, IMAGE_EXTENSION FROM " & Database.DBPrefix & "_ROTATOR_IMAGES WHERE ROTATOR_ID = " & RotatorID)
			While Reader.Read()
				Database.Write("DELETE " & Database.DBPrefix & "_FILES FROM " & Database.DBPrefix & "_FILES, " & Database.DBPrefix & "_FOLDERS WHERE " & Database.DBPrefix & "_FILES.FILE_FOLDER = " & Database.DBPrefix & "_FOLDERS.FOLDER_ID AND " & Database.DBPrefix & "_FILES.FILE_NAME = '" & Reader("IMAGE_ID").ToString() & "." & Reader("IMAGE_EXTENSION").ToString() & "' AND " & Database.DBPrefix & "_FOLDERS.FOLDER_NAME = 'rotatorimages' AND " & Database.DBPrefix & "_FOLDERS.FOLDER_PARENT = 0")
				File.Delete(MapPath("rotatorimages/" & Reader("IMAGE_ID").ToString() & "." & Reader("IMAGE_EXTENSION").ToString()))
			End While
			Reader.Close()

			Database.Write("DELETE FROM " & Database.DBPrefix & "_ROTATOR_IMAGES WHERE ROTATOR_ID = " & RotatorID)
			Database.Write("DELETE FROM " & Database.DBPrefix & "_ROTATOR WHERE ROTATOR_ID = " & RotatorID)
			PagePanel.visible = "false"
			NoItemsDiv.InnerHtml = "Image Rotator Deleted Successfully.<br /><a href=""admin.aspx"">Click Here</a> To Return To The Admin Page.<br /><br />"
		End Sub

		Sub EditRotator(sender As System.Object, e As System.EventArgs)
			AdminLinks.visible = "false"
			AdminRotatorConfig.visible = "false"
			AdminRotatorNewConfirm.visible = "false"
			AdminRotatorEdit.visible = "true"
			RotatorID = (Sender.CommandArgument).ToString()
			RotatorRefresh.CommandArgument = RotatorID
			RotatorImages.Datasource = Database.Read("SELECT * FROM " & Database.DBPrefix & "_ROTATOR_IMAGES WHERE ROTATOR_ID = " & RotatorID & " ORDER BY IMAGE_ID")
			RotatorImages.Databind()
			RotatorImages.Datasource.Close()
		End Sub

		Sub DeleteRotatorImage(sender As System.Object, e As System.EventArgs)
			Dim Reader as OdbcDataReader = Database.Read("SELECT IMAGE_ID, IMAGE_EXTENSION FROM " & Database.DBPrefix & "_ROTATOR_IMAGES WHERE IMAGE_ID = " & sender.CommandArgument)
			While Reader.Read()
				Database.Write("DELETE " & Database.DBPrefix & "_FILES FROM " & Database.DBPrefix & "_FILES, " & Database.DBPrefix & "_FOLDERS WHERE " & Database.DBPrefix & "_FILES.FILE_FOLDER = " & Database.DBPrefix & "_FOLDERS.FOLDER_ID AND " & Database.DBPrefix & "_FILES.FILE_NAME = '" & Reader("IMAGE_ID").ToString() & "." & Reader("IMAGE_EXTENSION").ToString() & "' AND " & Database.DBPrefix & "_FOLDERS.FOLDER_NAME = 'rotatorimages' AND " & Database.DBPrefix & "_FOLDERS.FOLDER_PARENT = 0")
				File.Delete(MapPath("rotatorimages/" & Reader("IMAGE_ID").ToString() & "." & Reader("IMAGE_EXTENSION").ToString()))
			End While
			Reader.Close()

			Database.Write("DELETE FROM " & Database.DBPrefix & "_ROTATOR_IMAGES WHERE IMAGE_ID = " & sender.CommandArgument)
			EditRotator(RotatorRefresh, new System.EventArgs())
		End Sub

		Sub ViewHtmlForm(sender As System.Object, e As System.EventArgs)
			AdminLinks.visible = "false"
			AdminHtmlFormConfig.visible = "false"
			AdminHtmlFormDelete.visible = "false"
			AdminHtmlFormView.visible = "true"
			HtmlFormResults.Datasource = Database.Read("SELECT * FROM " & Database.DBPrefix & "_HTML_FORMS WHERE FORM_ID = " & sender.CommandArgument)
			HtmlFormResults.Databind()
			if (HtmlFormResults.Items.Count = 0) then
				Response.Redirect("admin.aspx")
			end if
			HtmlFormResults.Datasource.Close()
			Database.Write("UPDATE " & Database.DBPrefix & "_HTML_FORMS SET FORM_NEW = 0 WHERE FORM_ID = " & sender.CommandArgument)
		End Sub

		Sub DeleteHtmlForm(sender As System.Object, e As System.EventArgs)
			AdminLinks.visible = "false"
			AdminHtmlFormConfig.visible = "false"
			AdminHtmlFormView.visible = "false"
			AdminHtmlFormDelete.visible = "true"
			HtmlFormYesButton.CommandArgument = sender.CommandArgument
		End Sub

		Sub DeleteHtmlFormConfirm(sender As System.Object, e As System.EventArgs)
			Database.Write("DELETE FROM " & Database.DBPrefix & "_HTML_FORMS WHERE FORM_ID = " & sender.CommandArgument)
			PagePanel.visible = "false"
			NoItemsDiv.InnerHtml = "Html Form Results Deleted Successfully.<br /><a href=""admin.aspx"">Click Here</a> To Return To The Admin Page.<br /><br />"
		End Sub

		Sub RestoreIP(sender As System.Object, e As System.EventArgs)
			Dim Reader as OdbcDataReader = Database.Read("SELECT MEMBER_ID FROM " & Database.DBPrefix & "_BANNED_IP WHERE IP_ADDRESS = '" & sender.CommandArgument & "'")
			While Reader.Read()
				Database.Write("UPDATE " & Database.DBPrefix & "_MEMBERS SET MEMBER_LEVEL = 1 WHERE MEMBER_ID = " & Reader("MEMBER_ID"))
			End While
			Reader.Close()
			Database.Write("DELETE FROM " & Database.DBPrefix & "_BANNED_IP WHERE IP_ADDRESS = '" & sender.CommandArgument & "'")

			PagePanel.visible = "false"
			NoItemsDiv.InnerHtml = "IP Address And Members Restored Successfully.<br /><a href=""admin.aspx"">Click Here</a> To Return To The Admin Page.<br /><br />"
		End Sub

		Sub CreateGallery(sender As System.Object, e As System.EventArgs)
			AdminLinks.visible = "false"
			AdminGalleryConfig.visible = "false"
			AdminGalleryNew.visible = "true"
		End Sub

		Sub SubmitNewGallery(sender As System.Object, e As System.EventArgs)
			AdminLinks.visible = "false"
			AdminGalleryConfig.visible = "false"
			AdminGalleryNew.visible = "false"
			AdminGalleryNewConfirm.visible = "true"
			Database.Write("INSERT INTO " & Database.DBPrefix & "_GALLERY (GALLERY_NAME) VALUES ('" & Functions.RepairString(NewGalleryName.text, 0) & "')")
			Dim Reader as OdbcDataReader = Database.Read("SELECT GALLERY_ID FROM " & Database.DBPrefix & "_GALLERY WHERE GALLERY_NAME = '" & Functions.RepairString(NewGalleryName.text, 0) & "' ORDER BY GALLERY_ID DESC", 1)
			While Reader.Read()
				NewGalleryEdit.CommandArgument = Reader("GALLERY_ID")
			End While
			Reader.Close()
		End Sub

		Sub DeleteGallery(sender As System.Object, e As System.EventArgs)
			AdminLinks.visible = "false"
			AdminGalleryConfig.visible = "false"
			AdminGalleryDelete.visible = "true"
			GalleryYesButton.CommandArgument = sender.CommandArgument
		End Sub

		Sub DeleteGalleryConfirm(sender As System.Object, e As System.EventArgs)
			Dim GalleryID as String = sender.CommandArgument

			Dim Reader as OdbcDataReader = Database.Read("SELECT PHOTO_ID, PHOTO_EXTENSION FROM " & Database.DBPrefix & "_GALLERY_PHOTOS WHERE GALLERY_ID = " & GalleryID)
			While Reader.Read()
				Database.Write("DELETE " & Database.DBPrefix & "_FILES FROM " & Database.DBPrefix & "_FILES, " & Database.DBPrefix & "_FOLDERS WHERE " & Database.DBPrefix & "_FILES.FILE_FOLDER = " & Database.DBPrefix & "_FOLDERS.FOLDER_ID AND " & Database.DBPrefix & "_FILES.FILE_NAME = '" & Reader("PHOTO_ID").ToString() & "." & Reader("PHOTO_EXTENSION").ToString() & "' AND " & Database.DBPrefix & "_FOLDERS.FOLDER_NAME = 'photogalleries' AND " & Database.DBPrefix & "_FOLDERS.FOLDER_PARENT = 0")
				Database.Write("DELETE " & Database.DBPrefix & "_FILES FROM " & Database.DBPrefix & "_FILES, " & Database.DBPrefix & "_FOLDERS WHERE " & Database.DBPrefix & "_FILES.FILE_FOLDER = " & Database.DBPrefix & "_FOLDERS.FOLDER_ID AND " & Database.DBPrefix & "_FILES.FILE_NAME = '" & Reader("PHOTO_ID").ToString() & "_s." & Reader("PHOTO_EXTENSION").ToString() & "' AND " & Database.DBPrefix & "_FOLDERS.FOLDER_NAME = 'photogalleries' AND " & Database.DBPrefix & "_FOLDERS.FOLDER_PARENT = 0")
				File.Delete(MapPath("photogalleries/" & Reader("PHOTO_ID").ToString() & "." & Reader("PHOTO_EXTENSION").ToString()))
				File.Delete(MapPath("photogalleries/" & Reader("PHOTO_ID").ToString() & "_s." & Reader("PHOTO_EXTENSION").ToString()))
			End While
			Reader.Close()

			Database.Write("DELETE FROM " & Database.DBPrefix & "_GALLERY_PHOTOS WHERE GALLERY_ID = " & GalleryID)
			Database.Write("DELETE FROM " & Database.DBPrefix & "_GALLERY WHERE GALLERY_ID = " & GalleryID)
			PagePanel.visible = "false"
			NoItemsDiv.InnerHtml = "Photo Gallery Deleted Successfully.<br /><a href=""admin.aspx"">Click Here</a> To Return To The Admin Page.<br /><br />"
		End Sub

		Sub EditGallery(sender As System.Object, e As System.EventArgs)
			AdminLinks.visible = "false"
			AdminGalleryConfig.visible = "false"
			AdminGalleryNewConfirm.visible = "false"

			AdminGalleryEdit.visible = "true"
			GalleryID = (Sender.CommandArgument).ToString()
			GalleryRefresh.CommandArgument = GalleryID
			GalleryPhotos.Datasource = Database.Read("SELECT * FROM " & Database.DBPrefix & "_GALLERY_PHOTOS WHERE GALLERY_ID = " & GalleryID & " ORDER BY PHOTO_ID")
			GalleryPhotos.Databind()
			GalleryPhotos.Datasource.Close()
		End Sub

		Sub DeleteGalleryPhoto(sender As System.Object, e As System.EventArgs)
			Dim Reader as OdbcDataReader = Database.Read("SELECT PHOTO_ID, PHOTO_EXTENSION FROM " & Database.DBPrefix & "_GALLERY_PHOTOS WHERE PHOTO_ID = " & sender.CommandArgument)
			While Reader.Read()
				Database.Write("DELETE " & Database.DBPrefix & "_FILES FROM " & Database.DBPrefix & "_FILES, " & Database.DBPrefix & "_FOLDERS WHERE " & Database.DBPrefix & "_FILES.FILE_FOLDER = " & Database.DBPrefix & "_FOLDERS.FOLDER_ID AND " & Database.DBPrefix & "_FILES.FILE_NAME = '" & Reader("PHOTO_ID").ToString() & "." & Reader("PHOTO_EXTENSION").ToString() & "' AND " & Database.DBPrefix & "_FOLDERS.FOLDER_NAME = 'photogalleries' AND " & Database.DBPrefix & "_FOLDERS.FOLDER_PARENT = 0")
				Database.Write("DELETE " & Database.DBPrefix & "_FILES FROM " & Database.DBPrefix & "_FILES, " & Database.DBPrefix & "_FOLDERS WHERE " & Database.DBPrefix & "_FILES.FILE_FOLDER = " & Database.DBPrefix & "_FOLDERS.FOLDER_ID AND " & Database.DBPrefix & "_FILES.FILE_NAME = '" & Reader("PHOTO_ID").ToString() & "_s." & Reader("PHOTO_EXTENSION").ToString() & "' AND " & Database.DBPrefix & "_FOLDERS.FOLDER_NAME = 'photogalleries' AND " & Database.DBPrefix & "_FOLDERS.FOLDER_PARENT = 0")
				File.Delete(MapPath("photogalleries/" & Reader("PHOTO_ID").ToString() & "." & Reader("PHOTO_EXTENSION").ToString()))
				File.Delete(MapPath("photogalleries/" & Reader("PHOTO_ID").ToString() & "_s." & Reader("PHOTO_EXTENSION").ToString()))
			End While
			Reader.Close()

			Database.Write("DELETE FROM " & Database.DBPrefix & "_GALLERY_PHOTOS WHERE PHOTO_ID = " & sender.CommandArgument)
			EditGallery(GalleryRefresh, new System.EventArgs())
		End Sub

		Sub SubmitCustomMessages(sender As System.Object, e As System.EventArgs)
			Database.Write("UPDATE " & Database.DBPrefix & "_CUSTOM_MESSAGES SET EMAIL_ADMINAPPROVAL = '" & txtEmailAdminApproval.text & "', EMAIL_SENDKEY = '" & txtEmailSendKey.text & "', EMAIL_CONFIRMPOST = '" & txtEmailConfirmPost.text & "', EMAIL_SUBSCRIPTION = '" & txtEmailSubscription.text & "', EMAIL_WELCOMEMESSAGE = '" & txtEmailWelcomeMessage.text & "', MESSAGE_ADMINAPPROVAL = '" & txtMessageAdminApproval.text & "', MESSAGE_SENDKEY = '" & txtMessageSendKey.text & "', MESSAGE_REGISTRATION = '" & txtMessageRegistration.text & "', MESSAGE_VALIDATION = '" & txtMessageValidation.text & "', MESSAGE_CONFIRMPOST = '" & txtMessageConfirmPost.text & "', MESSAGE_PRIVACYNOTICE = '" & txtMessagePrivacyNotice.text & "' WHERE ID = 1")
			PagePanel.visible = "false"
			NoItemsDiv.InnerHtml = "Custom Messages Edited Successfully.<br /><a href=""admin.aspx"">Click Here</a> To Return To The Admin Page.<br /><br />"
		End Sub

		Sub SubmitPMCleanup(sender As System.Object, e As System.EventArgs)
			Dim CleanupDays as Integer = cLng(txtDMGPMCleanupDays.Text)
			Dim PMCount as Integer = 0
			Dim Reader as OdbcDataReader = Database.Read("SELECT TOPIC_ID FROM " & Database.DBPrefix & "_PM_TOPICS WHERE " & Database.GetDateDiff("dd", "TOPIC_LASTPOST_DATE", Database.GetTimeStamp()) & " > " & CleanupDays)
			While Reader.Read()
				PMCount = PMCount + 1
				Database.Write("DELETE FROM " & Database.DBPrefix & "_PM_TOPICS WHERE TOPIC_ID = " & Reader("TOPIC_ID"))
				Database.Write("DELETE FROM " & Database.DBPrefix & "_PM_REPLIES WHERE TOPIC_ID = " & Reader("TOPIC_ID"))
			End While
			Reader.Close()

			PagePanel.visible = "false"
			NoItemsDiv.InnerHtml = PMCount & " Private Messages Were Deleted.<br /><a href=""admin.aspx"">Click Here</a> To Return To The Admin Page.<br /><br />"
		End Sub

	End Class


	'---------------------------------------------------------------------------------------------------
	' FileManager - Codebehind For admin_filemanager.aspx
	'---------------------------------------------------------------------------------------------------
	Public Class FileManager
		Inherits System.Web.UI.Page

		Public file As System.Web.UI.HtmlControls.HtmlInputFile
		Public FolderParent as Integer
		Public FolderPath As String
		Public UpIcon As System.Web.UI.WebControls.PlaceHolder
		Public FolderPathLabel As System.Web.UI.WebControls.Label
		Public FolderList As System.Web.UI.WebControls.Repeater
		Public FileList As System.Web.UI.WebControls.Repeater
		Public NewFolder As System.Web.UI.WebControls.TextBox
		Public FileDeleteButton As System.Web.UI.WebControls.Button
		Public FileManagerPanel As System.Web.UI.WebControls.PlaceHolder
		Public ConfirmFileDelete As System.Web.UI.WebControls.PlaceHolder
		Public PagePanel As System.Web.UI.WebControls.PlaceHolder
		Public NoItemsDiv As System.Web.UI.HtmlControls.HtmlGenericControl

		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
			if Not Page.IsPostBack() then
				if (Session("UserLevel") <> "3") or (Not Functions.IsInteger(Request.QueryString("ID"))) then
					PagePanel.visible = "false"
					NoItemsDiv.InnerHtml = "Access Denied<br /><br />"
				else
					if (Request.QueryString("ID") = "0") then
						FolderPath = ""
						UpIcon.Visible = "false"
					else
						Dim Reader as OdbcDataReader = Database.Read("SELECT FOLDER_PARENT, FOLDER_NAME FROM " & Database.DBPrefix & "_FOLDERS WHERE FOLDER_ID = " & Request.QueryString("ID"), 1)
						if (Reader.HasRows()) then
							While Reader.Read()
								FolderParent = Reader("FOLDER_PARENT")
								FolderPathLabel.Text = BuildFolderPath(Reader("FOLDER_PARENT"), Reader("FOLDER_NAME").ToString())
								FolderPath = FolderPathLabel.Text & "/"
							End While
						else
							PagePanel.Visible = "false"
							NoItemsDiv.InnerHtml = "Folder Does Not Exist<br /><br />"
						end if
						Reader.Close()
					end if

					FolderList.Datasource = Database.Read("SELECT FOLDER_ID, FOLDER_NAME, FOLDER_CORE FROM " & Database.DBPrefix & "_FOLDERS WHERE FOLDER_PARENT = " & Request.QueryString("ID") & " ORDER BY FOLDER_NAME")
					FolderList.Databind()
					FolderList.Datasource.Close()

					FileList.Datasource = Database.Read("SELECT FILE_ID, FILE_NAME, FILE_CORE FROM " & Database.DBPrefix & "_FILES WHERE FILE_FOLDER = " & Request.QueryString("ID") & " ORDER BY FILE_NAME")
					FileList.Databind()
					FileList.Datasource.Close()
				end if
			end if
		End Sub

		Function BuildFolderPath(FID as Integer, FName as String) as String
			Dim CurrentFolderID as Integer = FID
			Dim FullPath as String = FName

			While (CurrentFolderID > 0)
				Dim Reader as OdbcDataReader = Database.Read("SELECT FOLDER_PARENT, FOLDER_NAME FROM " & Database.DBPrefix & "_FOLDERS WHERE FOLDER_ID = " & CurrentFolderID, 1)
				While Reader.Read()
					FullPath = Reader("FOLDER_NAME").ToString() & "/" & FullPath
					CurrentFolderID = Reader("FOLDER_PARENT")
				End While
				Reader.Close()
			End While

			Return FullPath
		End Function

		Sub DeleteFolder(sender As System.Object, e As System.EventArgs)
			Dim FolderID as Integer = Sender.CommandArgument
			Dim HasChildren as Boolean = false

			Dim Reader as OdbcDataReader = Database.Read("SELECT FOLDER_ID FROM " & Database.DBPrefix & "_FOLDERS WHERE FOLDER_PARENT = " & FolderID, 1)
			if (Reader.HasRows()) then
				HasChildren = true
			end if
			Reader.Close()

			Reader = Database.Read("SELECT FILE_ID FROM " & Database.DBPrefix & "_FILES WHERE FILE_FOLDER = " & FolderID, 1)
			if (Reader.HasRows()) then
				HasChildren = true
			end if
			Reader.Close()

			if (HasChildren) then
				PagePanel.visible = "false"
				NoItemsDiv.InnerHtml = "This Folder Can't Be Deleted.  It Is Not Empty.<br /><br /><a href=""admin_filemanager.aspx?ID=" & Request.QueryString("ID") & """>Return To The File Manager</a><br /><br />"
			else
				Dim PhysicalUrl as String = System.Web.HttpContext.Current.Request.PhysicalPath
				Dim PhysicalPath as String = Left(PhysicalUrl, Len(PhysicalUrl) - InStr(1, StrReverse(PhysicalUrl), "\"))

				Dim CurrentFolderPath as String = ""
				Reader = Database.Read("SELECT FOLDER_PARENT, FOLDER_NAME FROM " & Database.DBPrefix & "_FOLDERS WHERE FOLDER_ID = " & FolderID, 1)
				While Reader.Read()
					CurrentFolderPath = BuildFolderPath(Reader("FOLDER_PARENT"), Reader("FOLDER_NAME").ToString())
				End While
				Reader.Close()
				CurrentFolderPath = CurrentFolderPath.Replace("/", "\")

				Dim FullFolderPath as String = PhysicalPath & "\" & CurrentFolderPath

				NoItemsDiv.InnerHTML = FullFolderPath

				Try
					if Not (System.IO.Directory.Exists(FullFolderPath)) then
						System.IO.Directory.Delete(FullFolderPath)
					end if
				Catch e1 as System.Security.SecurityException
				Catch e2 as Exception
				End Try

				Database.Write("DELETE FROM " & Database.DBPrefix & "_FOLDERS WHERE FOLDER_ID = " & FolderID)
				Response.Redirect("admin_filemanager.aspx?ID=" & Request.QueryString("ID"))
			end if
		End Sub

		Sub DeleteFileConfirmation(sender As System.Object, e As System.EventArgs)
			FileManagerPanel.visible = "false"
			ConfirmFileDelete.visible = "true"
			FileDeleteButton.CommandArgument = Sender.CommandArgument
		End Sub

		Sub DeleteFile(sender As System.Object, e As System.EventArgs)
			Dim FileID as Integer = Sender.CommandArgument

			Dim PhysicalUrl as String = System.Web.HttpContext.Current.Request.PhysicalPath
			Dim PhysicalPath as String = Left(PhysicalUrl, Len(PhysicalUrl) - InStr(1, StrReverse(PhysicalUrl), "\"))

			Dim FileFolder as Integer = 0
			Dim FileName as String = ""
			Dim Reader as OdbcDataReader = Database.Read("SELECT FILE_FOLDER, FILE_NAME FROM " & Database.DBPrefix & "_FILES WHERE FILE_ID = " & FileID, 1)
			While Reader.Read()
				FileFolder = Reader("FILE_FOLDER")
				FileName = Reader("FILE_NAME").ToString()
			End While
			Reader.Close()

			Dim FolderName as String = ""
			Dim FolderParent as Integer = 0
			Dim CurrentFolderPath as String = ""
			Reader = Database.Read("SELECT FOLDER_PARENT, FOLDER_NAME FROM " & Database.DBPrefix & "_FOLDERS WHERE FOLDER_ID = " & FileFolder, 1)
			While Reader.Read()
				FolderParent = Reader("FOLDER_PARENT")
				FolderName = Reader("FOLDER_NAME").ToString()
				CurrentFolderPath = BuildFolderPath(Reader("FOLDER_PARENT"), Reader("FOLDER_NAME").ToString())
			End While
			Reader.Close()
			CurrentFolderPath = CurrentFolderPath.Replace("/", "\")

			Dim FullFilePath as String = ""
			if (CurrentFolderPath = "") then
				FullFilePath = PhysicalPath & "\" & FileName
			else
				FullFilePath = PhysicalPath & "\" & CurrentFolderPath & "\" & FileName
			end if

			NoItemsDiv.InnerHTML = FullFilePath

			Try
				System.IO.File.Delete(FullFilePath)
				Database.Write("DELETE FROM " & Database.DBPrefix & "_FILES WHERE FILE_ID = " & FileID)

				if (FolderName = "avatars") and (FolderParent = 0) then
					Reader = Database.Read("SELECT AVATAR_ID FROM " & Database.DBPrefix & "_AVATARS WHERE AVATAR_IMAGE = '" & FileName & "'", 1)
					While Reader.Read()
						Database.Write("DELETE FROM " & Database.DBPrefix & "_AVATARS WHERE AVATAR_ID = " & Reader("AVATAR_ID"))
						Database.Write("UPDATE " & Database.DBPrefix & "_MEMBERS SET MEMBER_AVATAR = 1 WHERE MEMBER_AVATAR = " & Reader("AVATAR_ID"))
					End While
					Reader.Close()
				end if
				if (FolderName = "customavatars") and (FolderParent = 0) then
					if (Functions.IsInteger(Left(FileName, FileName.Length()-4))) then
						Database.Write("UPDATE " & Database.DBPrefix & "_MEMBERS SET MEMBER_AVATAR_USECUSTOM = 0, MEMBER_AVATAR_CUSTOMLOADED = 0 WHERE MEMBER_ID = " & Left(FileName, FileName.Length()-4))
					end if
				end if
				if (FolderName = "memberphotos") and (FolderParent = 0) then
					Dim MemberPhotoName as String = Left(FileName, FileName.Length()-4)
					Dim PhotoID as String = ""
					if (Right(MemberPhotoName, 2) = "_s") then
						PhotoID = Left(MemberPhotoName, MemberPhotoName.Length()-2)
					else
						PhotoID = MemberPhotoName
					end if
					if (Functions.IsInteger(PhotoID)) then
						Database.Write("DELETE FROM " & Database.DBPrefix & "_MEMBER_PHOTOS WHERE PHOTO_ID = " & PhotoID)
					end if
				end if
				if (FolderName = "photogalleries") and (FolderParent = 0) then
					Dim GalleryPhotoName as String = Left(FileName, FileName.Length()-4)
					Dim PhotoID as String = ""
					if (Right(GalleryPhotoName, 2) = "_s") then
						PhotoID = Left(GalleryPhotoName, GalleryPhotoName.Length()-2)
					else
						PhotoID = GalleryPhotoName
					end if
					if (Functions.IsInteger(PhotoID)) then
						Database.Write("DELETE FROM " & Database.DBPrefix & "_GALLERY_PHOTOS WHERE PHOTO_ID = " & PhotoID)
					end if
				end if
				if (FolderName = "rankimages") and (FolderParent = 0) then
					Database.Write("UPDATE " & Database.DBPrefix & "_RANKINGS SET RANK_IMAGE = '' WHERE RANK_IMAGE = '" & FileName & "'")
				end if
				if (FolderName = "rotatorimages") and (FolderParent = 0) then
					if (Functions.IsInteger(Left(FileName, FileName.Length()-4))) then
						Database.Write("DELETE FROM " & Database.DBPrefix & "_ROTATOR_IMAGES WHERE IMAGE_ID = " & Left(FileName, FileName.Length()-4))
					end if
				end if
				if (FolderName = "pageimages") and (FolderParent = 0) then
					Database.Write("UPDATE " & Database.DBPrefix & "_PAGES SET PAGE_THUMBNAIL = '' WHERE PAGE_THUMBNAIL = '" & FileName & "'")
					Database.Write("UPDATE " & Database.DBPrefix & "_PAGES SET PAGE_PHOTO = '' WHERE PAGE_PHOTO = '" & FileName & "'")
				end if
				if (FolderName = "topicfiles") and (FolderParent = 0) then
					Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_FILEUPLOAD = '' WHERE TOPIC_FILEUPLOAD = '" & FileName & "'")
				end if
			Catch e1 as System.Security.SecurityException
			Catch e2 as Exception
			End Try

			Response.Redirect("admin_filemanager.aspx?ID=" & Request.QueryString("ID"))
		End Sub

		Sub CancelDeleteFile(sender As System.Object, e As System.EventArgs)
			Response.Redirect("admin_filemanager.aspx?ID=" & Request.QueryString("ID"))
		End Sub

		Sub UploadFolder(sender As System.Object, e As System.EventArgs)
			Dim PhysicalUrl as String = System.Web.HttpContext.Current.Request.PhysicalPath
			Dim PhysicalPath as String = Left(PhysicalUrl, Len(PhysicalUrl) - InStr(1, StrReverse(PhysicalUrl), "\"))

			Dim CurrentFolderPath as String = FolderPathLabel.Text
			CurrentFolderPath = CurrentFolderPath.Replace("/", "\")
			
			Dim FullFolderPath as String = ""
			if (CurrentFolderPath = "") then
				FullFolderPath = PhysicalPath & "\" & NewFolder.Text
			else
				FullFolderPath = PhysicalPath & "\" & CurrentFolderPath & "\" & NewFolder.Text
			end if

			Try
				if Not (System.IO.Directory.Exists(FullFolderPath)) then
					System.IO.Directory.CreateDirectory(FullFolderPath)
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FOLDERS (FOLDER_PARENT, FOLDER_CORE, FOLDER_NAME) VALUES (" & Request.QueryString("ID") & ", 0, '" & NewFolder.Text & "')")
				end if
			Catch e1 as System.Security.SecurityException
			Catch e2 as Exception
			End Try

			Response.Redirect("admin_filemanager.aspx?ID=" & Request.QueryString("ID"))
		End Sub

		Sub UploadFile(sender As System.Object, e As System.EventArgs)
			Dim ReturnFile as HttpPostedFile = file.PostedFile
			Dim FileName as String = System.IO.Path.GetFileName(ReturnFile.FileName)
			Dim FolderID as Integer = Request.QueryString("ID")
			Dim FilePath as String = ""

			if (FolderPathLabel.Text <> "") then
				FilePath = MapPath(FolderPathLabel.Text & "/" & FileName)
			else
				FilePath = MapPath(FileName)
			end if

			if ((ReturnFile.ContentLength > 0) and (Functions.IsInteger(FolderID))) then
				Dim Reader as OdbcDataReader = Database.Read("SELECT FILE_CORE FROM " & Database.DBPrefix & "_FILES WHERE FILE_NAME = '" & FileName & "' AND FILE_FOLDER = " & FolderID, 1)
				if (Reader.HasRows()) then
					While Reader.Read()
						if (Reader("FILE_CORE") <> 1) then
							ReturnFile.SaveAs(FilePath)
						end if
					End While
				else
					ReturnFile.SaveAs(FilePath)
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_FOLDER, FILE_NAME, FILE_CORE) VALUES (" & Request.QueryString("ID") & ", '" & FileName & "', 0)")
				end if
				Reader.Close()
			end if

			Response.Redirect("admin_filemanager.aspx?ID=" & Request.QueryString("ID"))
		End Sub
	End Class


	'---------------------------------------------------------------------------------------------------
	' CreateMember - Codebehind For admin_createmember.aspx
	'---------------------------------------------------------------------------------------------------
	Public Class CreateMember
		Inherits System.Web.UI.Page

		Public txtUsername As System.Web.UI.WebControls.TextBox
		Public txtPassword As System.Web.UI.WebControls.TextBox
		Public txtLevel As System.Web.UI.WebControls.DropDownList
		Public txtTitle As System.Web.UI.WebControls.TextBox
		Public txtUseTitle As System.Web.UI.WebControls.DropDownList
		Public txtAllowTitle As System.Web.UI.WebControls.DropDownList
		Public txtRanking As System.Web.UI.WebControls.DropDownList
		Public txtEmail As System.Web.UI.WebControls.TextBox
		Public txtShowEmail As System.Web.UI.WebControls.DropDownList
		Public txtAllowCustomAvatar As System.Web.UI.WebControls.DropDownList
		Public PagePanel As System.Web.UI.WebControls.PlaceHolder
		Public NoItemsDiv As System.Web.UI.HtmlControls.HtmlGenericControl

		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
			if Not Page.IsPostBack() then
				if (Session("UserLevel") <> "3") then
					PagePanel.visible = "false"
					NoItemsDiv.InnerHtml = "Access Denied<br /><br />"
				else
					Dim LItem as New ListItem("Based On Post Count", "0")
					txtRanking.Items.Add(LItem)
					LItem = New ListItem("-------------------", "0")
					txtRanking.Items.Add(LItem)
					Dim Reader as OdbcDataReader = Database.Read("SELECT RANK_ID, RANK_NAME FROM " & Database.DBPrefix & "_RANKINGS ORDER BY RANK_POSTS")
					While Reader.Read()
						LItem = New ListItem(Reader("RANK_NAME").ToString(), Reader("RANK_ID"))
						txtRanking.Items.Add(LItem)
					End While
					Reader.Close()
				end if
			end if
		End Sub

		Sub SubmitNewMember(sender As System.Object, e As System.EventArgs)
			Dim Retry as Integer = 0

			Dim TheUsername as String = txtUsername.text
			TheUsername = TheUsername.TrimStart(" ")
			TheUsername = TheUsername.TrimEnd(" ")

			if (Len(TheUsername) = 0) then
				Functions.MessageBox("NO BLANK USERNAMES!")
				Retry = 1
			end if			

			Dim UsernameReader as OdbcDataReader = Database.Read("SELECT MEMBER_USERNAME FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_USERNAME = '" & Functions.RepairString(TheUsername) & "'")
				if (UsernameReader.HasRows) then
					Functions.MessageBox("USERNAME ALREADY EXISTS!")
					Retry = 1
				end if
			UsernameReader.Close()

			Dim EmailReader as OdbcDataReader = Database.Read("SELECT MEMBER_EMAIL FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_EMAIL = '" & Functions.RepairString(txtEmail.text) & "'")
				if (EmailReader.HasRows) then
					Functions.MessageBox("ACCOUNT ALREADY OPEN FOR THIS E-MAIL!")
					Retry = 1
				end if
			EmailReader.Close()

			if (Retry = 0) then
				PagePanel.visible = "false"

				Dim Username as String = Functions.RepairString(TheUsername)
				Dim Password as String = Functions.RepairString(txtPassword.text)
				Password = Functions.Encrypt(Password)
				Dim Email as String = Functions.RepairString(txtEmail.text)
				Dim ShowEmail as Integer = CLng(txtShowEmail.SelectedValue)
				Dim Title as String = Functions.RepairString(txtTitle.text)
				Dim UseTitle as Integer = CLng(txtUseTitle.SelectedValue)
				Dim AllowTitle as Integer = CLng(txtAllowTitle.SelectedValue)
				Dim MemberLevel as Integer = CLng(txtLevel.SelectedValue)
				Dim AllowCustomAvatar as Integer = CLng(txtAllowCustomAvatar.SelectedValue)
				Dim Ranking as Integer = CLng(txtRanking.SelectedValue)

				Database.Write("INSERT INTO " & Database.DBPrefix & "_MEMBERS (MEMBER_USERNAME, MEMBER_PASSWORD, MEMBER_LEVEL, MEMBER_EMAIL, MEMBER_LOCATION, MEMBER_HOMEPAGE, MEMBER_SIGNATURE, MEMBER_SIGNATURE_SHOW, MEMBER_IM_AOL, MEMBER_IM_ICQ, MEMBER_IM_MSN, MEMBER_IM_YAHOO, MEMBER_POSTS, MEMBER_DATE_JOINED, MEMBER_DATE_LASTVISIT, MEMBER_TITLE, MEMBER_TITLE_ALLOWCUSTOM, MEMBER_TITLE_USECUSTOM, MEMBER_EMAIL_SHOW, MEMBER_IP_LAST, MEMBER_IP_ORIGINAL, MEMBER_REALNAME, MEMBER_OCCUPATION, MEMBER_SEX, MEMBER_AGE, MEMBER_BIRTHDAY, MEMBER_NOTES, MEMBER_FAVORITESITE, MEMBER_PHOTO, MEMBER_AVATAR, MEMBER_AVATAR_SHOW, MEMBER_AVATAR_ALLOWCUSTOM, MEMBER_AVATAR_USECUSTOM, MEMBER_AVATAR_CUSTOMLOADED, MEMBER_AVATAR_CUSTOMTYPE, MEMBER_VALIDATED, MEMBER_VALIDATION_STRING, MEMBER_RANKING) VALUES ('" & Username & "','" & Password & "', " & MemberLevel & ", '" & Email & "', '', '', '', 1, '', '', '', '', 0, " & Database.GetTimeStamp()  & ", " & Database.GetTimeStamp() & ", '" & Title & "', " & AllowTitle & ", " & UseTitle & ", " & ShowEmail & ", '" & Request.UserHostAddress() & "', '" & Request.UserHostAddress() & "', '', '', '', '', '', '', '', '', 1, 0, " & AllowCustomAvatar & ", 0, 0, 'jpg', 1, '', " & Ranking & ")")

				PagePanel.visible = "false"
				NoItemsDiv.InnerHtml = "Member Created Successfully.<br /><a href=""admin.aspx"">Click Here</a> To Return To The Admin Page.<br /><br />"
			end if
		End Sub
	End Class

End Namespace
