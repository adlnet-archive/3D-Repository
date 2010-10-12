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
Imports System.Math
Imports System.Collections
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Microsoft.VisualBasic
Imports DMGForums.Global

Namespace DMGForums.Topics

	'---------------------------------------------------------------------------------------------------
	' TopicsPage - Codebehind For topics.aspx
	'---------------------------------------------------------------------------------------------------
	Public Class TopicsPage
		Inherits System.Web.UI.Page

		Public Topic As System.Web.UI.WebControls.Repeater
		Public Replies As System.Web.UI.WebControls.Repeater
		Public txtSignature As System.Web.UI.WebControls.CheckBox
		Public txtAuthor As System.Web.UI.WebControls.TextBox
		Public txtPosts As System.Web.UI.WebControls.TextBox
		Public txtReplyMessage As System.Web.UI.WebControls.TextBox
		Public txtForumID As System.Web.UI.WebControls.TextBox
		Public txtTopicID As System.Web.UI.WebControls.TextBox
		Public QuickReply As System.Web.UI.WebControls.Panel
		Public ReplyButton As System.Web.UI.WebControls.Panel
		Public PagingPanel As System.Web.UI.WebControls.Panel
		Public JumpPage As System.Web.UI.WebControls.DropDownList
		Public FirstLink As System.Web.UI.WebControls.LinkButton
		Public PreviousLink As System.Web.UI.WebControls.LinkButton
		Public NextLink As System.Web.UI.WebControls.LinkButton
		Public LastLink As System.Web.UI.WebControls.LinkButton
		Public PageCountLabel As System.Web.UI.WebControls.Label
		Public PagingPanel2 As System.Web.UI.WebControls.Panel
		Public JumpPage2 As System.Web.UI.WebControls.DropDownList
		Public PageCountLabel2 As System.Web.UI.WebControls.Label
		Public YesButton As System.Web.UI.WebControls.Button
		Public SubmitButton As System.Web.UI.WebControls.Button
		Public BanMemberPanel As System.Web.UI.WebControls.Panel
		Public PagePanel As System.Web.UI.WebControls.Panel
		Public NoItemsDiv As System.Web.UI.HtmlControls.HtmlGenericControl

		Public ConfirmTopicForm As System.Web.UI.WebControls.Panel
		Public ConfirmTopicDropdown As System.Web.UI.WebControls.DropDownList
		Public ConfirmTopicSubmitButton As System.Web.UI.WebControls.Button

		Public ConfirmReplyForm As System.Web.UI.WebControls.Panel
		Public ConfirmReplyDropdown As System.Web.UI.WebControls.DropDownList
		Public ConfirmReplySubmitButton As System.Web.UI.WebControls.Button

		Public ReportTopicForm As System.Web.UI.WebControls.Panel
		Public ReportTopicSubmitButton As System.Web.UI.WebControls.Button
		Public txtReportTopicMessage As System.Web.UI.WebControls.Textbox

		Public ForumID as String
		Public ForumName as String
		Public CategoryID as String
		Public CategoryName as String
		Public TopicID as String
		Public TopicSubject as String
		Public AllowModeration as Boolean
		Public TopicStatusSave as Integer
		Public TopicReplies as Integer
		Public TopicUnconfirmedReplies as Integer = 0
		Public ShowQuoteButton as Integer = 1

		Public DMGHeader As DMGForums.Global.Header
		Public DMGFooter As DMGForums.Global.Footer
		Public DMGLogin As DMGForums.Global.Login
		Public DMGSettings As DMGForums.Global.Settings

		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
			if Not Page.IsPostBack() then
				SetSession()

				If (Not Functions.IsInteger(Request.QueryString("ID"))) then
					Response.Redirect("default.aspx")
				else
					Dim Failure as Integer = 0
					Dim ForumType as Integer
					Dim TopicConfirmed as Integer = 1
					Dim TopicReader as OdbcDataReader = Database.Read("SELECT T.TOPIC_ID, T.TOPIC_SUBJECT, T.TOPIC_VIEWS, T.TOPIC_REPLIES, T.TOPIC_UNCONFIRMED_REPLIES, T.FORUM_ID, F.FORUM_NAME, F.FORUM_TYPE, F.FORUM_STATUS, F.FORUM_SHOWHEADERS, F.FORUM_SHOWLOGIN, T.CATEGORY_ID, T.TOPIC_STATUS, T.TOPIC_CONFIRMED, C.CATEGORY_NAME FROM (" & Database.DBPrefix & "_CATEGORIES C INNER JOIN " & Database.DBPrefix & "_TOPICS T ON C.CATEGORY_ID = T.CATEGORY_ID) INNER JOIN " & Database.DBPrefix & "_FORUMS F ON T.FORUM_ID = F.FORUM_ID WHERE T.TOPIC_ID = " & Request.Querystring("ID"))
						Dim TopicViews as Integer
						if TopicReader.HasRows() then
							While(TopicReader.Read())
							      ForumID = TopicReader("FORUM_ID").ToString()
								ForumName = TopicReader("FORUM_NAME").ToString()
								ForumType = TopicReader("FORUM_TYPE")
							      CategoryID = TopicReader("CATEGORY_ID").ToString()
								CategoryName = TopicReader("CATEGORY_NAME").ToString()
								TopicID = TopicReader("TOPIC_ID").ToString()
								TopicSubject = Functions.CurseFilter(TopicReader("TOPIC_SUBJECT").ToString())
								DMGSettings.CustomTitle = TopicSubject
								TopicViews = TopicReader("TOPIC_VIEWS")
								if ((TopicReader("TOPIC_STATUS") <> 1) and (Session("UserLevel") <> "3")) or ((TopicReader("FORUM_STATUS") <> 1) and (Session("UserLevel") <> "3")) or (Session("UserLogged") = "0") then
									QuickReply.Visible = "false"
									ReplyButton.Visible = "false"
									ShowQuoteButton = 0
								end if
								if (TopicReader("TOPIC_STATUS") = 0) and (Session("UserLevel") <> "3") then
									PagePanel.visible = "false"
									NoItemsDiv.InnerHtml = "This Topic Has Been Removed<br /><br />"
									Failure = 1
								else
									Failure = 0
								end if
								txtForumID.text = ForumID
								txtTopicID.text = TopicID
								TopicStatusSave = TopicReader("TOPIC_STATUS")
								TopicReplies = TopicReader("TOPIC_REPLIES")
								TopicUnconfirmedReplies = TopicReader("TOPIC_UNCONFIRMED_REPLIES")
								TopicConfirmed = TopicReader("TOPIC_CONFIRMED")
								if (TopicReader("FORUM_SHOWHEADERS") <> 1) then
									DMGHeader.visible = "false"
									DMGFooter.visible = "false"
								end if
								DMGLogin.ShowLogin() = TopicReader("FORUM_SHOWLOGIN")
							End While
						else
							Response.Redirect("default.aspx")
						end if
					TopicReader.close()

					AllowModeration = Functions.IsModerator(Session("UserID"), Session("UserLevel"), ForumID)

					if (AllowModeration) then
						TopicReplies += TopicUnconfirmedReplies
					end if

					if (TopicConfirmed = 0) then
						if (AllowModeration) then
							NoItemsDiv.InnerHTML = "This Topic Has Not Been Confirmed<br /><br />"
						else
							Response.Redirect("default.aspx")
						end if
					end if

					if (ForumType = 1) or (ForumType = 3) or (ForumType = 4) then
						if Not Functions.IsPrivileged(ForumID, ForumType, Session("UserID"), Session("UserLevel"), Session("UserLogged")) then
							PagePanel.visible = "false"
							NoItemsDiv.InnerHtml = "You Do Not Have Access To This Forum<br /><br />"
							Failure = 1
						else
							Failure = 0
						end if
					elseif (ForumType = 2) then
						if Not Session("FORUM_" & ForumID) = "logged" then
							PagePanel.visible = "false"
							NoItemsDiv.InnerHtml = "You Do Not Have Access To This Forum<br /><br />"
							Failure = 1
						else
							Failure = 0
						end if
					end if

					if Failure = 0 then
						if (Session("VIEWTOPIC_" & TopicID) <> "Yes") then
							Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_VIEWS = " & TopicViews + 1 & " WHERE TOPIC_ID = " & TopicID)
							Session("VIEWTOPIC_" & TopicID) = "Yes"
						end if

						if QuickReply.visible = "true"
							Dim UserReader as OdbcDataReader = Database.Read("SELECT MEMBER_ID, MEMBER_POSTS, MEMBER_SIGNATURE_SHOW FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & Session("UserID"))
								While UserReader.Read()
									if UserReader("MEMBER_SIGNATURE_SHOW") = 1 then
										txtSignature.Checked = "true"
									end if
									txtAuthor.text = UserReader("MEMBER_ID")
									txtPosts.text = UserReader("MEMBER_POSTS")
								End While
							UserReader.Close()
						end if

						if (Request.Querystring("PAGE") <> "") then
							ListPosts(Request.Querystring("PAGE"), Settings.ItemsPerPage)
						else
							ListPosts(1, Settings.ItemsPerPage)
						end if
					end if
				end if
			end if
		End Sub

		Sub ListPosts(Optional CurrentPage As Integer = 1, Optional ItemsPerPage As Integer = 15)
			Dim NumPages, NumItems, NumWholePages, Leftover as Integer
			Dim IDList as New ArrayList

			Dim TopicStatus as String = ""
			Dim ReplyStatus as String = ""
			if Not AllowModeration then
				TopicStatus = " and T.TOPIC_STATUS <> 0 and T.TOPIC_CONFIRMED <> 0"
				ReplyStatus = " and REPLY_CONFIRMED <> 0"
			end if

			Topic.DataSource = Database.Read("SELECT T.TOPIC_ID, T.TOPIC_SUBJECT, T.TOPIC_MESSAGE, T.TOPIC_AUTHOR, T.TOPIC_DATE, T.TOPIC_SIGNATURE, T.TOPIC_CONFIRMED, T.TOPIC_FILEUPLOAD, M.MEMBER_USERNAME, M.MEMBER_LOCATION, M.MEMBER_SIGNATURE, M.MEMBER_POSTS, M.MEMBER_DATE_JOINED, M.MEMBER_AVATAR_USECUSTOM, M.MEMBER_TITLE, A.AVATAR_IMAGE, M.MEMBER_TITLE_ALLOWCUSTOM, M.MEMBER_AVATAR_ALLOWCUSTOM, M.MEMBER_TITLE_USECUSTOM, M.MEMBER_AVATAR_CUSTOMTYPE, M.MEMBER_AVATAR_CUSTOMLOADED, M.MEMBER_AVATAR_SHOW, M.MEMBER_PHOTO, M.MEMBER_LEVEL, M.MEMBER_RANKING, M.MEMBER_ID FROM (" & Database.DBPrefix & "_TOPICS T INNER JOIN " & Database.DBPrefix & "_MEMBERS M ON T.TOPIC_AUTHOR = M.MEMBER_ID) LEFT OUTER JOIN " & Database.DBPrefix & "_AVATARS A ON M.MEMBER_AVATAR = A.AVATAR_ID WHERE T.TOPIC_ID = " & Request.QueryString("ID") & TopicStatus)

			Topic.DataBind()

			if (Topic.Items.Count = 0) then
				NoItemsDiv.InnerHtml = "There Are No Items To Display<br /><br />"
				Topic.Visible = "false"
				Replies.Visible = "false"
			else

				if TopicReplies = 0 then
					Replies.visible = "false"
					PagingPanel.visible = "false"
					PagingPanel2.visible = "false"
				else
					Dim ReplyReader as OdbcDataReader = Database.Read("SELECT REPLY_ID FROM " & Database.DBPrefix & "_REPLIES R WHERE R.TOPIC_ID = " & Request.QueryString("ID") & ReplyStatus & " ORDER BY R.REPLY_DATE")
						While(ReplyReader.Read())
							IDList.Add(ReplyReader("REPLY_ID"))
						End While
					ReplyReader.close()

					NumItems = IDList.Count
					NumPages = NumItems \ ItemsPerPage
					NumWholePages = NumItems \ ItemsPerPage
					Leftover = NumItems Mod ItemsPerPage

					If Leftover > 0 then
						NumPages += 1
					end if

					if (CurrentPage < 0) or (CurrentPage > NumPages) then
						ListPosts(1, ItemsPerPage)
					else
						if CurrentPage = NumPages then
							NextLink.Visible = false
							LastLink.Visible = false
						else
							NextLink.Visible = true
							LastLink.Visible = true
							NextLink.CommandArgument = CurrentPage + 1
							LastLink.CommandArgument = NumPages
						end if

						if CurrentPage = 1 then
							PreviousLink.Visible = false
							FirstLink.Visible = false
						else
							PreviousLink.Visible = true
							FirstLink.Visible = true
							PreviousLink.CommandArgument = CurrentPage - 1
							FirstLink.CommandArgument = 1
							Topic.visible = "false"
						end if
		
						if NumPages = 1 then
							PagingPanel.visible = "false"
							PagingPanel2.visible = "false"
						end if
	
						Dim JumpPageList As ArrayList = new ArrayList
						Dim x As Integer
						For x = 1 to NumPages
							JumpPageList.Add(x)
						Next
	


						JumpPage.DataSource = JumpPageList
						JumpPage.Databind()
						JumpPage.SelectedIndex = CurrentPage - 1

						JumpPage2.DataSource = JumpPageList
						JumpPage2.Databind()
						JumpPage2.SelectedIndex = CurrentPage - 1
	
						PageCountLabel.Text = NumPages
						PageCountLabel2.Text = NumPages

						Dim StartOfPage as Integer = ItemsPerPage * (CurrentPage - 1)
						Dim EndOfPage as Integer = Min((ItemsPerPage * (CurrentPage - 1)) + (ItemsPerPage - 1), ((NumWholePages * ItemsPerPage) + Leftover - 1))

						Dim CurrentSubset As String = Join( IDList.GetRange( StartOfPage , (EndOfPage - StartOfPage + 1) ).ToArray , "," )

						Replies.DataSource = Database.Read("SELECT R.REPLY_ID, R.REPLY_MESSAGE, R.REPLY_AUTHOR, R.REPLY_DATE, R.REPLY_SIGNATURE, R.REPLY_CONFIRMED, M.MEMBER_USERNAME, M.MEMBER_LOCATION, M.MEMBER_SIGNATURE, M.MEMBER_POSTS, M.MEMBER_DATE_JOINED, M.MEMBER_AVATAR_USECUSTOM, M.MEMBER_TITLE, A.AVATAR_IMAGE, M.MEMBER_TITLE_ALLOWCUSTOM, M.MEMBER_AVATAR_ALLOWCUSTOM, M.MEMBER_TITLE_USECUSTOM, M.MEMBER_AVATAR_CUSTOMTYPE, M.MEMBER_AVATAR_CUSTOMLOADED, M.MEMBER_AVATAR_SHOW, M.MEMBER_PHOTO, M.MEMBER_LEVEL, M.MEMBER_RANKING, M.MEMBER_ID FROM (" & Database.DBPrefix & "_REPLIES R INNER JOIN " & Database.DBPrefix & "_MEMBERS M ON R.REPLY_AUTHOR = M.MEMBER_ID) LEFT OUTER JOIN " & Database.DBPrefix & "_AVATARS A ON M.MEMBER_AVATAR = A.AVATAR_ID WHERE R.REPLY_ID IN (" & CurrentSubSet & ") ORDER BY R.REPLY_DATE")
						Replies.DataBind()
						if (Replies.Items.Count = 0) then
							Replies.Visible = "false"
						end if
						Replies.Datasource.Close()
					end if
				end if
			end if

			Topic.DataSource.Close()
		End Sub

		Sub ChangePage(sender As System.Object, e As System.EventArgs)
			If sender.ToString() = "System.Web.UI.WebControls.LinkButton" Then
				Response.Redirect("topics.aspx?ID=" & Request.Querystring("ID") & "&PAGE=" & sender.CommandArgument)
			else
				Response.Redirect("topics.aspx?ID=" & Request.Querystring("ID") & "&PAGE=" & JumpPage.SelectedValue)
			end if	
		End Sub

		Sub ChangePage2(sender As System.Object, e As System.EventArgs)
			Response.Redirect("topics.aspx?ID=" & Request.Querystring("ID") & "&PAGE=" & JumpPage2.SelectedValue)
		End Sub

		Sub SubmitReply(sender As System.Object, e As System.EventArgs)
			Dim Failure as Integer = 0
			Dim TopicReplies as Integer = 0

			if (txtReplyMessage.text = "") or (txtReplyMessage.text = " ") then
				Failure = 1
				Functions.Messagebox("No Message Entered!")
			end if

			if Failure = 0 then
				Dim Signature as Integer = 0
				Dim SpamSeconds as Integer
				if (txtSignature.Checked) then
					Signature = 1
				end if

				PagePanel.Visible = "false"

				Dim SpamReader as OdbcDataReader
				SpamReader = Database.Read("SELECT " & Database.GetDateDiff("ss", "MEMBER_DATE_LASTPOST", Database.GetTimeStamp()) & " as PostSeconds FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & txtAuthor.text)
				While SpamReader.Read()
					if Functions.IsDBNull(SpamReader("PostSeconds")) then
						SpamSeconds = -99
					else
						SpamSeconds = SpamReader("PostSeconds")
					end if
				End While
				SpamReader.Close()

				if (SpamSeconds > Settings.SpamFilter) or (SpamSeconds = -99) or (AllowModeration) or (Session("UserLevel") = "3") then
					Dim ForceConfirm as Integer = 0
					Dim ForumReader as OdbcDataReader = Database.Read("SELECT FORUM_FORCECONFIRM FROM " & Database.DBPrefix & "_FORUMS WHERE FORUM_ID = " & txtForumID.text)
					While ForumReader.Read()
						ForceConfirm = ForumReader("FORUM_FORCECONFIRM")
					End While
					ForumReader.Close()

					Dim ReplyConfirm as Integer = 1
					if (ForceConfirm = 1) and (Session("UserLevel") <> "3") and (Session("UserLevel") <> "2") then
						ReplyConfirm = 0
					end if

					Database.Write("INSERT INTO " & Database.DBPrefix & "_REPLIES (TOPIC_ID, REPLY_MESSAGE, REPLY_DATE, REPLY_AUTHOR, REPLY_SIGNATURE, REPLY_CONFIRMED) VALUES (" & txtTopicID.text & ", '" & Functions.RepairString(txtReplyMessage.text) & "', " & Database.GetTimeStamp() & ", " & txtAuthor.text & ", " & Signature & ", " & ReplyConfirm & ")")
					Database.Write("UPDATE " & Database.DBPrefix & "_MEMBERS SET MEMBER_POSTS = (MEMBER_POSTS + 1), MEMBER_DATE_LASTPOST = " & Database.GetTimeStamp() & " WHERE MEMBER_ID = " & txtAuthor.text)

					if (ReplyConfirm = 1) then
						Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_REPLIES = (TOPIC_REPLIES + 1), TOPIC_LASTPOST_DATE = " & Database.GetTimeStamp() & ", TOPIC_LASTPOST_AUTHOR = " & txtAuthor.text & " WHERE TOPIC_ID = " & txtTopicID.text)
						Database.Write("UPDATE " & Database.DBPrefix & "_FORUMS SET FORUM_POSTS = (FORUM_POSTS + 1), FORUM_LASTPOST_DATE = " & Database.GetTimeStamp() & ", FORUM_LASTPOST_AUTHOR = " & txtAuthor.text & " WHERE FORUM_ID = " & txtForumID.text)
	
						Functions.SendToSubscribers(txtTopicID.text)
	
						Dim Reader as OdbcDataReader = Database.Read("SELECT TOPIC_REPLIES FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_ID = " & txtTopicID.text)
						While Reader.Read()
							TopicReplies = Reader("TOPIC_REPLIES")
						End While
						Reader.Close()

							Dim PageItems as Integer = Settings.ItemsPerPage
							Dim NumPages as Integer = TopicReplies \ PageItems
							Dim Leftover as Integer = TopicReplies Mod PageItems
							If Leftover > 0 then
								NumPages += 1
							end if

						NoItemsDiv.InnerHtml = "Reply Posted Successfully<br /><br /><a href=""topics.aspx?ID=" & txtTopicID.text & "&PAGE=" & NumPages & """>Click Here</a> To Return To The Thread<br /><br />"
					else
						Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_UNCONFIRMED_REPLIES = (TOPIC_UNCONFIRMED_REPLIES + 1) WHERE TOPIC_ID = " & txtTopicID.text)
						Functions.SendToModerators(2, txtTopicID.text, txtForumID.text)
						NoItemsDiv.InnerHtml = Functions.CustomMessage("MESSAGE_CONFIRMPOST") & "<br /><br /><a href=""forums.aspx?ID=" & txtForumID.text & """>Click Here</a> To Return To The Forum<br /><br />"
					end if
				else
					NoItemsDiv.InnerHtml = "You Can Not Post More Than Once In " & Settings.SpamFilter & " Seconds.<br /><br /><a href=""topics.aspx?ID=" & txtTopicID.text & """>Click Here</a> To Return To The Thread<br /><br />"
				end if
			end if
		End Sub

		Sub ConfirmTopic(sender As System.Object, e As System.EventArgs)
			PagePanel.visible = "false"
			ConfirmTopicForm.visible = "true"
			ConfirmTopicSubmitButton.CommandArgument = sender.CommandArgument
			NoItemsDiv.InnerHtml = ""
		End Sub

		Sub ApplyTopicConfirmation(sender As System.Object, e As System.EventArgs)
			Dim ForumID as Integer
			Dim ForumReader as OdbcDataReader = Database.Read("SELECT FORUM_ID FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_ID = " & sender.CommandArgument)
			While ForumReader.Read()
				ForumID = ForumReader("FORUM_ID")
			End While
			ForumReader.Close()

			if (ConfirmTopicDropdown.SelectedValue = 1) then
				Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_CONFIRMED = 1 WHERE TOPIC_ID = " & sender.CommandArgument)
				Functions.UpdateCounts(2, ForumID, 0, 0)
				PagePanel.visible = "false"
				ConfirmTopicForm.visible = "false"
				NoItemsDiv.InnerHtml = "The Topic Has Been Confirmed Successfully<br /><br /><a href=""forums.aspx?ID=" & ForumID & """>Click Here</a> To Return To The Forum<br /><br />"
			else
				Response.Redirect("topics.aspx?ID=" & sender.CommandArgument)
			end if
		End Sub

		Sub ConfirmReply(sender As System.Object, e As System.EventArgs)
			PagePanel.visible = "false"
			ConfirmReplyForm.visible = "true"
			ConfirmReplySubmitButton.CommandArgument = sender.CommandArgument
			NoItemsDiv.InnerHtml = ""
		End Sub

		Sub ApplyReplyConfirmation(sender As System.Object, e As System.EventArgs)
			Dim TopicID, ForumID as Integer
			Dim Reader as OdbcDataReader = Database.Read("SELECT R.TOPIC_ID, T.FORUM_ID FROM " & Database.DBPrefix & "_REPLIES R Left Outer Join " & Database.DBPrefix & "_TOPICS T On R.TOPIC_ID = T.TOPIC_ID WHERE R.REPLY_ID = " & sender.CommandArgument)
			While Reader.Read()
				TopicID = Reader("TOPIC_ID")
				ForumID = Reader("FORUM_ID")
			End While
			Reader.Close()

			if (ConfirmReplyDropdown.SelectedValue = 1) then
				Database.Write("UPDATE " & Database.DBPrefix & "_REPLIES SET REPLY_CONFIRMED = 1 WHERE REPLY_ID = " & sender.CommandArgument)
				Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_UNCONFIRMED_REPLIES = (TOPIC_UNCONFIRMED_REPLIES-1), TOPIC_REPLIES = (TOPIC_REPLIES + 1) WHERE TOPIC_ID = " & TopicID)

				Reader = Database.Read("SELECT REPLY_DATE, REPLY_AUTHOR FROM " & Database.DBPrefix & "_REPLIES WHERE TOPIC_ID = " & TopicID & " and REPLY_CONFIRMED = 1 ORDER BY REPLY_ID DESC", 1)
				While Reader.Read()
					Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_LASTPOST_DATE = '" & Reader("REPLY_DATE") & "', TOPIC_LASTPOST_AUTHOR = " & Reader("REPLY_AUTHOR") & " WHERE TOPIC_ID = " & TopicID)
				End While
				Reader.Close()

				Functions.SendToSubscribers(TopicID)
				Functions.UpdateCounts(3, ForumID, 0, 0)

				PagePanel.visible = "false"
				ConfirmReplyForm.visible = "false"
				NoItemsDiv.InnerHtml = "The Reply Has Been Confirmed Successfully<br /><br /><a href=""topics.aspx?ID=" & TopicID & """>Click Here</a> To Return To The Thread<br /><br />"
			else
				Response.Redirect("topics.aspx?ID=" & TopicID)
			end if
		End Sub

		Sub ReportTopic(sender As System.Object, e As System.EventArgs)
			PagePanel.visible = "false"
			ReportTopicForm.visible = "true"
			ReportTopicSubmitButton.CommandArgument = sender.CommandArgument
			NoItemsDiv.InnerHtml = ""
		End Sub

		Sub ReportTopicConfirmation(sender As System.Object, e As System.EventArgs)
			Dim TopicSubject as String = ""
			Dim ForumID as Integer = 0
			Dim Reader as OdbcDataReader = Database.Read("SELECT TOPIC_SUBJECT, FORUM_ID FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_ID = " & sender.CommandArgument)
			While Reader.Read()
				TopicSubject = Reader("TOPIC_SUBJECT").ToString()
				ForumID = Reader("FORUM_ID")
			End While
			Reader.Close()

			Dim Message as String = Functions.RepairString("TOPIC ALERT: [urlnopop=topics.aspx?ID=" & sender.CommandArgument & "]" & TopicSubject & "[/urlnopop][br][br]This topic has been reported for Admin/Moderator review.  The user's custom message is displayed below.[br][br][hr][br]" & txtReportTopicMessage.text)

			Reader = Database.Read("SELECT P.MEMBER_ID FROM " & Database.DBPrefix & "_PRIVILEGED P Left Outer Join " & Database.DBPrefix & "_MEMBERS M On P.MEMBER_ID = M.MEMBER_ID WHERE P.FORUM_ID = " & ForumID & " AND M.MEMBER_LEVEL <> 3")
			While Reader.Read()
				Database.Write("INSERT INTO " & Database.DBPrefix & "_PM_TOPICS (TOPIC_FROM, TOPIC_TO, TOPIC_SUBJECT, TOPIC_MESSAGE, TOPIC_DATE, TOPIC_TO_READ, TOPIC_FROM_READ, TOPIC_LASTPOST_AUTHOR, TOPIC_LASTPOST_DATE, TOPIC_REPLIES, TOPIC_SHOWSENDER, TOPIC_SHOWRECEIVER) VALUES (" & Session("UserID") & ", " & Reader("MEMBER_ID") & ", 'Topic To Report For Admin/Moderator Review', '" & Message & "', " & Database.GetTimeStamp() & ", 0, 1, " & Session("UserID") & ", " & Database.GetTimeStamp() & ", 0, 0, 1)")
			End While
			Reader.Close()

			Reader = Database.Read("SELECT MEMBER_ID FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_LEVEL = 3")
			While Reader.Read()
				Database.Write("INSERT INTO " & Database.DBPrefix & "_PM_TOPICS (TOPIC_FROM, TOPIC_TO, TOPIC_SUBJECT, TOPIC_MESSAGE, TOPIC_DATE, TOPIC_TO_READ, TOPIC_FROM_READ, TOPIC_LASTPOST_AUTHOR, TOPIC_LASTPOST_DATE, TOPIC_REPLIES, TOPIC_SHOWSENDER, TOPIC_SHOWRECEIVER) VALUES (" & Session("UserID") & ", " & Reader("MEMBER_ID") & ", 'Topic To Report For Admin/Moderator Review', '" & Message & "', " & Database.GetTimeStamp() & ", 0, 1, " & Session("UserID") & ", " & Database.GetTimeStamp() & ", 0, 0, 1)")
			End While
			Reader.Close()

			PagePanel.visible = "false"
			ReportTopicForm.visible = "false"
			NoItemsDiv.InnerHtml = "The Admins/Moderators Have Been Alerted About This Topic<br /><br /><a href=""topics.aspx?ID=" & sender.CommandArgument & """>Click Here</a> To Return To The Thread<br /><br />"
		End Sub

		Sub EditTopic(sender As System.Object, e As System.EventArgs)
			Response.Redirect("edittopic.aspx?ID=" & sender.CommandArgument)
		End Sub

		Sub DeleteTopic(sender As System.Object, e As System.EventArgs)
			Response.Redirect("deletetopic.aspx?ID=" & sender.CommandArgument)
		End Sub

		Sub EditReply(sender As System.Object, e As System.EventArgs)
			Response.Redirect("editreply.aspx?ID=" & sender.CommandArgument)
		End Sub

		Sub DeleteReply(sender As System.Object, e As System.EventArgs)
			Response.Redirect("deletereply.aspx?ID=" & sender.CommandArgument)
		End Sub

		Sub QuoteTopic(sender As System.Object, e As System.EventArgs)
			Response.Redirect("newreply.aspx?ID=" & Request.Querystring("ID") & "&TQ=" & sender.CommandArgument)
		End Sub

		Sub QuoteReply(sender As System.Object, e As System.EventArgs)
			Response.Redirect("newreply.aspx?ID=" & Request.Querystring("ID") & "&RQ=" & sender.CommandArgument)
		End Sub

		Sub SendPM(sender As System.Object, e As System.EventArgs)
			Response.Redirect("pm_send.aspx?SendTo=" & sender.CommandArgument)
		End Sub

		Sub SubscribeTopic(sender As System.Object, e As System.EventArgs)
			Response.Redirect("subscribe.aspx?ID=" & sender.CommandArgument)
		End Sub

		Sub BanMemberConfirm(sender As System.Object, e As System.EventArgs)
			Dim MemberID as Integer = sender.CommandArgument
			Dim MemberUsername as String = ""
			Dim MemberLevel as Integer = 0
			Dim MemberReader as OdbcDataReader = Database.Read("SELECT MEMBER_USERNAME, MEMBER_LEVEL FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & MemberID)
			While MemberReader.Read()
				MemberLevel = MemberReader("MEMBER_LEVEL")
				MemberUsername = MemberReader("MEMBER_USERNAME").ToString()
			End While
			MemberReader.Close()

			if (Session("UserLevel") > MemberLevel) then
				YesButton.CommandArgument = MemberID
				PagePanel.visible = "false"
				BanMemberPanel.visible = "true"
				NoItemsDiv.InnerHtml = "Are you sure you want to ban <a href=""profile.aspx?id=" & MemberID & """>" & MemberUsername & "</a><br /><br />"
			else
				PagePanel.visible = "false"
				NoItemsDiv.InnerHtml = "You Do Not Have Rights To Ban This Member.<br /><br /><a href=""default.aspx"">Click Here</a> To Return To The Main Page<br /><br />"
			end if
		End Sub

		Sub CancelBanMember(sender As System.Object, e As System.EventArgs)
			Response.Redirect("default.aspx")
		End Sub

		Sub BanMember(sender As System.Object, e As System.EventArgs)
			Database.Write("UPDATE " & Database.DBPrefix & "_MEMBERS SET MEMBER_LEVEL = 0 WHERE MEMBER_ID = " & sender.CommandArgument)
			Database.Write("DELETE FROM " & Database.DBPrefix & "_PRIVILEGED WHERE MEMBER_ID = " & sender.CommandArgument)
			PagePanel.visible = "false"
			BanMemberPanel.visible = "false"
			NoItemsDiv.InnerHtml = "The Member Has Been Successfully Banned.<br /><br /><a href=""default.aspx"">Click Here</a> To Return To The Main Page<br /><br />"
		End Sub

		Sub SetSession()
			if Not Request.Cookies("dmgforums") Is Nothing then
				Dim aCookie As New System.Web.HttpCookie("dmgforums")

				Dim UserReader as OdbcDataReader = Database.Read("SELECT MEMBER_ID, MEMBER_USERNAME, MEMBER_PASSWORD, MEMBER_LEVEL FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & Server.HtmlEncode(Request.Cookies("dmgforums")("mukul")) & " AND MEMBER_PASSWORD = '" & Server.HtmlEncode(Request.Cookies("dmgforums")("gupta")) & "'", 1)
					While(UserReader.Read())
						Session("UserID") = UserReader("MEMBER_ID").ToString()
						Session("UserName") = UserReader("MEMBER_USERNAME").ToString()
						Session("UserLevel") = UserReader("MEMBER_LEVEL").ToString()
						Session("UserLogged") = "1"
						aCookie.Values("fighter") = Functions.Encrypt(DateTime.Now())
						aCookie.Values("mukul") = UserReader("MEMBER_ID").ToString()
						aCookie.Values("dooder") = Functions.Encrypt(DateTime.Now())
						aCookie.Values("gupta") = UserReader("MEMBER_PASSWORD").ToString()
						aCookie.Expires = DateTime.Now.AddDays(30)
						Response.Cookies.Add(aCookie)
					End While
				UserReader.Close()

				if ((Session("UserLevel") = 0) or (Session("UserLevel") = -1)) then
					aCookie.Values("fighter") = Functions.Encrypt(DateTime.Now())
					aCookie.Values("mukul") = "-3"
					aCookie.Values("dooder") = Functions.Encrypt(DateTime.Now())
					aCookie.Values("gupta") = Functions.Encrypt(DateTime.Now())
					aCookie.Expires = DateTime.Now.AddDays(-1)
					Response.Cookies.Add(aCookie)
					Session("UserID") = "-1"
					Session("UserName") = ""
					Session("UserLogged") = "0"
					Session("UserLevel") = "0"
				end if
			else
				if (Session("UserLogged") = "1") then
					Dim UserReader as OdbcDataReader = Database.Read("SELECT MEMBER_ID, MEMBER_USERNAME, MEMBER_PASSWORD, MEMBER_LEVEL FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & Session("UserID"), 1)
						While(UserReader.Read())
							Session("UserID") = UserReader("MEMBER_ID").ToString()
							Session("UserName") = UserReader("MEMBER_USERNAME").ToString()
							Session("UserLevel") = UserReader("MEMBER_LEVEL").ToString()
							Session("UserLogged") = "1"
						End While
					UserReader.Close()

					if ((Session("UserLevel") = 0) or (Session("UserLevel") = -1)) then
						Dim aCookie As New System.Web.HttpCookie("dmgforums")
						aCookie.Values("fighter") = Functions.Encrypt(DateTime.Now())
						aCookie.Values("mukul") = "-3"
						aCookie.Values("dooder") = Functions.Encrypt(DateTime.Now() & "bbb")
						aCookie.Values("gupta") = Functions.Encrypt(DateTime.Now() & "ccc")
						aCookie.Expires = DateTime.Now.AddDays(-1)
						Response.Cookies.Add(aCookie)
						Session("UserID") = "-1"
						Session("UserName") = ""
						Session("UserLogged") = "0"
						Session("UserLevel") = "0"
					end if
				else
					Session("UserID") = "-1"
					Session("UserName") = ""
					Session("UserLogged") = "0"
					Session("UserLevel") = "0"
				end if
			end if
		End Sub
	End Class


	'---------------------------------------------------------------------------------------------------
	' NewTopic - Codebehind For newtopic.aspx
	'---------------------------------------------------------------------------------------------------
	Public Class NewTopic
		Inherits System.Web.UI.Page

		Public file As System.Web.UI.HtmlControls.HtmlInputFile
		Public ForumName As System.Web.UI.WebControls.Label
		Public txtForumID As System.Web.UI.WebControls.TextBox
		Public txtAuthor As System.Web.UI.WebControls.TextBox
		Public txtCategoryID As System.Web.UI.WebControls.TextBox
		Public txtSticky As System.Web.UI.WebControls.CheckBox
		Public txtSignature As System.Web.UI.WebControls.CheckBox
		Public txtSubscribe As System.Web.UI.WebControls.CheckBox
		Public txtNews As System.Web.UI.WebControls.CheckBox
		Public txtSubject As System.Web.UI.WebControls.TextBox
		Public txtMessage As System.Web.UI.WebControls.TextBox
		Public TopicPreview As System.Web.UI.WebControls.PlaceHolder
		Public FileUploadPanel As System.Web.UI.WebControls.PlaceHolder
		Public PagePanel As System.Web.UI.WebControls.Panel
		Public NoItemsDiv As System.Web.UI.HtmlControls.HtmlGenericControl

		Public PrevSubject as String = ""
		Public PrevDate as String = ""
		Public PrevMessage as String = ""
		Public PrevSignature as String = ""

		Public DMGHeader As DMGForums.Global.Header
		Public DMGFooter As DMGForums.Global.Footer
		Public DMGLogin As DMGForums.Global.Login

		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
			if Not Page.IsPostBack() then
				if (Session("UserLogged") = "1") then
					if (Functions.IsInteger(Request.QueryString("ID"))) then
						if ((Settings.AllowSub = 1) or (Session("UserLevel") = "3")) then
							txtSubscribe.visible = "true"
						end if

						Dim StatusCheck as String
						if Session("UserLevel") = 3 then
							StatusCheck = ""
						else
							StatusCheck = "FORUM_STATUS = 1 AND "
						end if

						Dim ForumID, ForumType as Integer
						Dim ForumReader as OdbcDataReader = Database.Read("SELECT FORUM_ID, FORUM_NAME, FORUM_SHOWHEADERS, FORUM_SHOWLOGIN, CATEGORY_ID, FORUM_TYPE FROM " & Database.DBPrefix & "_FORUMS WHERE " & StatusCheck & "FORUM_ID = " & Request.Querystring("ID"))
							if (Not ForumReader.HasRows) then
								PagePanel.visible = "false"
								NoItemsDiv.InnerHtml = "No Forums To Post To<br /><br />"
							else
								While ForumReader.Read()
									ForumID = ForumReader("FORUM_ID")
									ForumName.text = ForumReader("FORUM_NAME").ToString()
									ForumType = ForumReader("FORUM_TYPE")
									txtForumID.text = ForumReader("FORUM_ID")
									txtCategoryID.text = ForumReader("CATEGORY_ID")
									if (ForumReader("FORUM_SHOWHEADERS") <> 1) then
										DMGHeader.visible = "false"
										DMGFooter.visible = "false"
									end if
									DMGLogin.ShowLogin() = ForumReader("FORUM_SHOWLOGIN")
								End While

								if (ForumType = 1) or (ForumType = 3) or (ForumType = 4) then
									if Not Functions.IsPrivileged(ForumID, ForumType, Session("UserID"), Session("UserLevel"), Session("UserLogged")) then
										PagePanel.visible = "false"
										NoItemsDiv.InnerHtml = "You Do Not Have Access To This Forum<br /><br />"
									end if
								elseif (ForumType = 2) then
									if Not Session("FORUM_" & Request.Querystring("ID")) = "logged" then
										PagePanel.visible = "false"
										NoItemsDiv.InnerHtml = "You Do Not Have Access To This Forum<br /><br />"
									end if
								end if

								Dim UserReader as OdbcDataReader = Database.Read("SELECT MEMBER_ID, MEMBER_LEVEL, MEMBER_SIGNATURE_SHOW, MEMBER_POSTS, MEMBER_RANKING FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & Session("UserID"))
									While UserReader.Read()
										txtAuthor.text = UserReader("MEMBER_ID")
										if (Functions.IsModerator(UserReader("MEMBER_ID"), UserReader("MEMBER_LEVEL"), ForumID)) then
											txtSticky.visible = "true"
										end if
										if UserReader("MEMBER_LEVEL") = 3 then
											txtNews.visible = "true"
											FileUploadPanel.visible = "true"
										end if
										if UserReader("MEMBER_SIGNATURE_SHOW") = 1 then
											txtSignature.checked = "true"
										end if
										if (Not Functions.AllowCustom(UserReader("MEMBER_RANKING"), UserReader("MEMBER_POSTS"), 0, "Topics")) then
											PagePanel.visible = "false"
											NoItemsDiv.InnerHtml = "Your ranking only allows you to reply to current topics.<br /><br />"
										end if
										if (Functions.AllowCustom(UserReader("MEMBER_RANKING"), UserReader("MEMBER_POSTS"), 0, "Uploads")) then
											FileUploadPanel.visible = "true"
										end if
									End While
								UserReader.Close()
							end if
						ForumReader.Close()
					else
						PagePanel.visible = "false"
						NoItemsDiv.InnerHtml = "No Forums To Post To<br /><br />"
					end if
				else
					PagePanel.visible = "false"
					NoItemsDiv.InnerHtml = "You Must Be Logged In To Post<br /><br />"
				end if
			end if
		End Sub

		Sub SubmitTopic(sender As Object, e As EventArgs)
			Dim Failure as Integer = 0
			Dim TopicID as Integer

			if (txtSubject.text = "") or (txtSubject.text = " ") then
				Failure = 1
				Functions.Messagebox("No Subject Entered!")
			end if
			if (txtMessage.text = "") or (txtMessage.text = " ") then
				Failure = 1
				Functions.Messagebox("No Message Entered!")
			end if

			Dim SpamSeconds as Integer = -99
			Dim MemberRanking as Integer = 0
			Dim MemberPosts as Integer = 0
			Dim SpamReader as OdbcDataReader = Database.Read("SELECT " & Database.GetDateDiff("ss", "MEMBER_DATE_LASTPOST", Database.GetTimeStamp()) & " as PostSeconds, MEMBER_RANKING, MEMBER_POSTS FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & txtAuthor.text, 1)
			While SpamReader.Read()
				if Functions.IsDBNull(SpamReader("PostSeconds")) then
					SpamSeconds = -99
				else
					SpamSeconds = SpamReader("PostSeconds")
				end if
				MemberRanking = SpamReader("MEMBER_RANKING")
				MemberPosts = SpamReader("MEMBER_POSTS")
			End While
			SpamReader.Close()

			if (Not ((SpamSeconds > Settings.SpamFilter) or (SpamSeconds = -99) or (Session("UserLevel") = "3"))) then
				Failure = 1
				Functions.Messagebox("You Can Not Post More Than Once In " & Settings.SpamFilter & " Seconds.")
			end if

			Dim UploadedFileName as String = ""
			if (Not file.PostedFile Is Nothing) then
				Dim ReturnFile as HttpPostedFile = file.PostedFile
				if (((Functions.AllowCustom(MemberRanking, MemberPosts, 0, "Uploads")) or (Session("UserLevel") = "3")) and (ReturnFile.ContentLength > 0)) then
					Dim Reader as OdbcDataReader = Database.Read("SELECT " & Database.DBPrefix & "_TOPIC_UPLOADSIZE, " & Database.DBPrefix & "_MEMBER_FILETYPES FROM " & Database.DBPrefix & "_SETTINGS WHERE ID = " & Settings.DefaultTemplate, 1)
					While Reader.Read()
						if ((((Reader(Database.DBPrefix & "_MEMBER_FILETYPES").ToString()).Contains(ReturnFile.ContentType)) and (ReturnFile.ContentLength <= Reader(Database.DBPrefix & "_TOPIC_UPLOADSIZE")*1024)) or (Session("UserLevel") = "3")) then
							Dim Timestamp as DateTime = DateTime.Now()
							Dim TimeString as String = Timestamp.ToString("ddMMyyyyhhmmss")

							UploadedFileName = System.IO.Path.GetFileName(ReturnFile.FileName)
							UploadedFileName = TimeString & UploadedFileName
							UploadedFileName = UploadedFileName.Replace(" ", "")
							Dim FolderID as Integer = 0
							Dim Reader2 as OdbcDataReader = Database.Read("SELECT FOLDER_ID FROM " & Database.DBPrefix & "_FOLDERS WHERE FOLDER_NAME = 'topicfiles' AND FOLDER_PARENT = 0")
							While Reader2.Read()
								FolderID = Reader2("FOLDER_ID")
							End While
							Reader2.Close()

							Dim FilePath as String = MapPath("topicfiles/" & UploadedFileName)
							ReturnFile.SaveAs(FilePath)

							Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_FOLDER, FILE_NAME, FILE_CORE) VALUES (" & FolderID & ", '" & UploadedFileName & "', 0)")
						else
							Failure = 1
							if (Not (Reader(Database.DBPrefix & "_MEMBER_FILETYPES").ToString()).Contains(ReturnFile.ContentType)) then
								Functions.Messagebox("This file format is not allowed." & ReturnFile.ContentType)
							elseif (Not (ReturnFile.ContentLength <= Reader(Database.DBPrefix & "_TOPIC_UPLOADSIZE")*1024))
								Functions.Messagebox("File size must be less than " & Reader(Database.DBPrefix & "_TOPIC_UPLOADSIZE") & " Kb.")
							end if
						end if
					End While
					Reader.Close()
				end if
			end if

			if Failure = 0 then
				PagePanel.visible = "false"

				Dim Sticky as Integer = 0
				Dim News as Integer = 0
				Dim Signature as Integer = 0
				if (txtSticky.Checked) then
					Sticky = 1
				end if
				if (txtNews.Checked) then
					News = 1
				end if
				if (txtSignature.Checked) then
					Signature = 1
				end if

				Dim ForceConfirm as Integer = 0
				Dim ForumReader as OdbcDataReader = Database.Read("SELECT FORUM_FORCECONFIRM FROM " & Database.DBPrefix & "_FORUMS WHERE FORUM_ID = " & txtForumID.text)
				While ForumReader.Read()
					ForceConfirm = ForumReader("FORUM_FORCECONFIRM")
				End While
				ForumReader.Close()

				Dim TopicConfirm as Integer = 1
				if (ForceConfirm = 1) and (Session("UserLevel") <> "3") and (Session("UserLevel") <> "2") then
					TopicConfirm = 0
				end if

				Database.Write("INSERT INTO " & Database.DBPrefix & "_TOPICS (CATEGORY_ID, FORUM_ID, TOPIC_SUBJECT, TOPIC_MESSAGE, TOPIC_AUTHOR, TOPIC_DATE, TOPIC_REPLIES, TOPIC_VIEWS, TOPIC_LASTPOST_DATE, TOPIC_LASTPOST_AUTHOR, TOPIC_STICKY, TOPIC_SIGNATURE, TOPIC_STATUS, TOPIC_NEWS, TOPIC_CONFIRMED, TOPIC_UNCONFIRMED_REPLIES, TOPIC_FILEUPLOAD) VALUES (" & txtCategoryID.text & ", " & txtForumID.text & ", '" & Functions.RepairString(txtSubject.text) & "', '" & Functions.RepairString(txtMessage.text) & "', " & txtAuthor.text & ", " & Database.GetTimeStamp() & ", 0, 0, " & Database.GetTimeStamp() & ", " & txtAuthor.text & ", " & Sticky & ", " & Signature & ", 1, " & News & ", " & TopicConfirm & ", 0, '" & UploadedFileName & "')")
				Database.Write("UPDATE " & Database.DBPrefix & "_MEMBERS SET MEMBER_POSTS = (MEMBER_POSTS + 1), MEMBER_DATE_LASTPOST = " & Database.GetTimeStamp() & " WHERE MEMBER_ID = " & txtAuthor.text)

				Dim TopicPostback as OdbcDataReader = Database.Read("SELECT TOPIC_ID FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_AUTHOR = " & txtAuthor.text & " ORDER BY TOPIC_ID DESC", 1)
				While TopicPostback.Read()
					TopicID = TopicPostback("TOPIC_ID")
				End While
				TopicPostback.Close()

				if (TopicConfirm = 1) then
					Database.Write("UPDATE " & Database.DBPrefix & "_FORUMS SET FORUM_TOPICS = (FORUM_TOPICS + 1), FORUM_POSTS = (FORUM_POSTS + 1), FORUM_LASTPOST_DATE = " & Database.GetTimeStamp() & ", FORUM_LASTPOST_AUTHOR = '" & txtAuthor.text & "', FORUM_LASTPOST_TOPIC = " & TopicID & " WHERE FORUM_ID = " & txtForumID.text)

					if (txtSubscribe.Checked) then
						Database.Write("INSERT INTO " & Database.DBPrefix & "_SUBSCRIPTIONS (SUB_MEMBER, SUB_TOPIC, SUB_EMAIL) VALUES (" & Session("UserID") & ", " & TopicID & ", 0)")
					end if

					NoItemsDiv.InnerHtml = "Topic Posted Successfully<br /><br /><a href=""forums.aspx?ID=" & txtForumID.text & """>Click Here</a> To Return To The Forum<br /><br />"
				else
					NoItemsDiv.InnerHtml = Functions.CustomMessage("MESSAGE_CONFIRMPOST") & "<br /><br /><a href=""forums.aspx?ID=" & txtForumID.text & """>Click Here</a> To Return To The Forum<br /><br />"
					Functions.SendToModerators(1, TopicID, txtForumID.text)
				end if
			end if
		End Sub

		Sub PreviewTopic(sender As Object, e As EventArgs)
			Dim Failure as Integer = 0
			Dim ShowSig as Integer = 0

			if (txtSubject.text = "") or (txtSubject.text = " ") then
				Failure = 1
				Functions.Messagebox("No Subject Entered!")
			end if
			if (txtMessage.text = "") or (txtMessage.text = " ") then
				Failure = 1
				Functions.Messagebox("No Message Entered!")
			end if
			if (txtSignature.Checked) then
				ShowSig = 1
			end if

			if Failure = 0 then
				TopicPreview.visible = "true"
				PrevSubject = Functions.RepairString(txtSubject.text)
				PrevMessage = Functions.FormatString(Functions.RepairString(txtMessage.text))
				PrevDate = DateTime.Now()

				if (ShowSig = 1) then
					Dim Reader as OdbcDataReader = Database.Read("SELECT MEMBER_SIGNATURE FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & Session("UserID"))
					While Reader.Read()
						PrevSignature = Functions.Signature(1, Reader("MEMBER_SIGNATURE").ToString())
					End While
					Reader.Close()
				end if
			end if
		End Sub

	End Class


	'---------------------------------------------------------------------------------------------------
	' EditTopic - Codebehind For edittopic.aspx
	'---------------------------------------------------------------------------------------------------
	Public Class EditTopic
		Inherits System.Web.UI.Page

		Public txtForumID As System.Web.UI.WebControls.DropDownList
		Public txtSubject As System.Web.UI.WebControls.TextBox
		Public txtMessage As System.Web.UI.WebControls.TextBox
		Public txtSticky As System.Web.UI.WebControls.CheckBox
		Public txtSignature As System.Web.UI.WebControls.CheckBox
		Public txtStatus As System.Web.UI.WebControls.DropDownList
		Public txtNews As System.Web.UI.WebControls.CheckBox
		Public ForumPanel As System.Web.UI.WebControls.Panel
		Public StatusPanel As System.Web.UI.WebControls.Panel
		Public OldForumID As System.Web.UI.WebControls.TextBox
		Public PagePanel As System.Web.UI.WebControls.Panel
		Public NoItemsDiv As System.Web.UI.HtmlControls.HtmlGenericControl

		Dim ForumID as Integer = 0

		Public DMGHeader As DMGForums.Global.Header
		Public DMGFooter As DMGForums.Global.Footer
		Public DMGLogin As DMGForums.Global.Login

		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
			if Not Page.IsPostBack() then
				SetSession()

				if (Functions.IsInteger(Request.QueryString("ID"))) then
					Dim AllowModeration as Boolean
					Dim ModeratorReader as OdbcDataReader = Database.Read("SELECT FORUM_ID, TOPIC_AUTHOR FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_ID = " & Request.Querystring("ID"))
						While ModeratorReader.Read()
							AllowModeration = Functions.IsModerator(Session("UserID"), Session("UserLevel"), ModeratorReader("FORUM_ID"))
							if (Not AllowModeration) and (ModeratorReader("TOPIC_AUTHOR").ToString() = Session("UserID")) then
								AllowModeration = True
							end if
						End While
					ModeratorReader.Close()

					if AllowModeration then
						Dim TopicReader as OdbcDataReader = Database.Read("SELECT T.FORUM_ID, T.TOPIC_SUBJECT, T.TOPIC_MESSAGE, T.TOPIC_STICKY, T.TOPIC_SIGNATURE, T.TOPIC_STATUS, T.TOPIC_NEWS, F.FORUM_SHOWHEADERS, F.FORUM_SHOWLOGIN FROM " & Database.DBPrefix & "_TOPICS T Left Outer Join " & Database.DBPrefix & "_FORUMS F On T.FORUM_ID = F.FORUM_ID WHERE T.TOPIC_ID = " & Request.Querystring("ID"))
							While TopicReader.Read()
								txtSubject.text = Server.HTMLDecode(TopicReader("TOPIC_SUBJECT"))
								txtMessage.text = Server.HTMLDecode(TopicReader("TOPIC_MESSAGE"))
								if TopicReader("TOPIC_STICKY") = 1 then
									txtSticky.Checked = "true"
								end if
								if TopicReader("TOPIC_NEWS") = 1 then
									txtNews.Checked = "true"
								end if
								if TopicReader("TOPIC_SIGNATURE") = 1 then
									txtSignature.Checked = "true"
								end if
								ForumID = TopicReader("FORUM_ID")
								txtStatus.SelectedValue = TopicReader("TOPIC_STATUS")
								if (TopicReader("FORUM_SHOWHEADERS") <> 1) then
									DMGHeader.visible = "false"
									DMGFooter.visible = "false"
								end if
								DMGLogin.ShowLogin() = TopicReader("FORUM_SHOWLOGIN")
							End While
						TopicReader.Close()

						txtForumID.Datasource = Database.Read("SELECT FORUM_ID, FORUM_NAME FROM " & Database.DBPrefix & "_FORUMS")
						txtForumID.Databind()
							txtForumID.SelectedValue = ForumID
						txtForumID.Datasource.Close()

						OldForumID.text = ForumID

						if Functions.IsModerator(Session("UserID"), Session("UserLevel"), ForumID) then
							txtSticky.visible = "true"
							txtNews.visible = "true"
							ForumPanel.visible = "true"
							StatusPanel.visible = "true"
						end if
					else
						PagePanel.visible = "false"
						NoItemsDiv.InnerHtml = "Access Denied<br /><br />"
					end if
				else
					Response.Redirect("default.aspx")
				end if
			end if
		End Sub

		Sub SubmitTopic(sender As System.Object, e As System.EventArgs)
			Dim Sticky as Integer = 0
			Dim News as Integer = 0
			Dim Signature as Integer = 0
			Dim Status as Integer = 1
			Dim theForumID as Integer = 0

			if (txtSticky.Checked) then
				Sticky = 1
			end if
			if (txtNews.Checked) then
				News = 1
			end if
			if (txtSignature.Checked) then
				Signature = 1
			end if

			theForumID = txtForumID.SelectedValue
			Status = txtStatus.SelectedValue

			PagePanel.visible = "false"

			Dim CategoryID as String = "-1"
			Dim CategoryReader as OdbcDataReader = Database.Read("SELECT CATEGORY_ID FROM " & Database.DBPrefix & "_FORUMS WHERE FORUM_ID = " & theForumID)
				While CategoryReader.Read
					CategoryID = CategoryReader("CATEGORY_ID")
				End While
			CategoryReader.Close()

			Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET FORUM_ID = " & theForumID & ", CATEGORY_ID = " & CategoryID & ", TOPIC_SUBJECT = '" & Functions.RepairString(txtSubject.Text) & "', TOPIC_MESSAGE = '" & Functions.RepairString(txtMessage.Text) & "', TOPIC_STICKY = " & Sticky & ", TOPIC_SIGNATURE = " & Signature & ", TOPIC_STATUS = " & Status & ", TOPIC_NEWS = " & News & " WHERE TOPIC_ID = " & Request.Querystring("ID"))

			if (theForumID <> OldForumID.text) then
				Functions.UpdateCounts(4, OldForumID.text, theForumID, Request.Querystring("ID"))
			end if

			NoItemsDiv.InnerHtml = "Topic Edited Successfully<br /><br /><a href=""topics.aspx?ID=" & Request.Querystring("ID") & """>Click Here</a> To Return To The Thread<br /><br />"
		End Sub

		Sub SetSession()
			if Not Request.Cookies("dmgforums") Is Nothing then
				Dim aCookie As New System.Web.HttpCookie("dmgforums")

				Dim UserReader as OdbcDataReader = Database.Read("SELECT MEMBER_ID, MEMBER_USERNAME, MEMBER_PASSWORD, MEMBER_LEVEL FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & Server.HtmlEncode(Request.Cookies("dmgforums")("mukul")) & " AND MEMBER_PASSWORD = '" & Server.HtmlEncode(Request.Cookies("dmgforums")("gupta")) & "'", 1)
					While(UserReader.Read())
						Session("UserID") = UserReader("MEMBER_ID").ToString()
						Session("UserName") = UserReader("MEMBER_USERNAME").ToString()
						Session("UserLevel") = UserReader("MEMBER_LEVEL").ToString()
						Session("UserLogged") = "1"
						aCookie.Values("fighter") = Functions.Encrypt(DateTime.Now())
						aCookie.Values("mukul") = UserReader("MEMBER_ID").ToString()
						aCookie.Values("dooder") = Functions.Encrypt(DateTime.Now())
						aCookie.Values("gupta") = UserReader("MEMBER_PASSWORD").ToString()
						aCookie.Expires = DateTime.Now.AddDays(30)
						Response.Cookies.Add(aCookie)
					End While
				UserReader.Close()

				if ((Session("UserLevel") = 0) or (Session("UserLevel") = -1)) then
					aCookie.Values("fighter") = Functions.Encrypt(DateTime.Now())
					aCookie.Values("mukul") = "-3"
					aCookie.Values("dooder") = Functions.Encrypt(DateTime.Now())
					aCookie.Values("gupta") = Functions.Encrypt(DateTime.Now())
					aCookie.Expires = DateTime.Now.AddDays(-1)
					Response.Cookies.Add(aCookie)
					Session("UserID") = "-1"
					Session("UserName") = ""
					Session("UserLogged") = "0"
					Session("UserLevel") = "0"
				end if
			else
				if (Session("UserLogged") = "1") then
					Dim UserReader as OdbcDataReader = Database.Read("SELECT MEMBER_ID, MEMBER_USERNAME, MEMBER_PASSWORD, MEMBER_LEVEL FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & Session("UserID"), 1)
						While(UserReader.Read())
							Session("UserID") = UserReader("MEMBER_ID").ToString()
							Session("UserName") = UserReader("MEMBER_USERNAME").ToString()
							Session("UserLevel") = UserReader("MEMBER_LEVEL").ToString()
							Session("UserLogged") = "1"
						End While
					UserReader.Close()

					if ((Session("UserLevel") = 0) or (Session("UserLevel") = -1)) then
						Dim aCookie As New System.Web.HttpCookie("dmgforums")
						aCookie.Values("fighter") = Functions.Encrypt(DateTime.Now())
						aCookie.Values("mukul") = "-3"
						aCookie.Values("dooder") = Functions.Encrypt(DateTime.Now() & "bbb")
						aCookie.Values("gupta") = Functions.Encrypt(DateTime.Now() & "ccc")
						aCookie.Expires = DateTime.Now.AddDays(-1)
						Response.Cookies.Add(aCookie)
						Session("UserID") = "-1"
						Session("UserName") = ""
						Session("UserLogged") = "0"
						Session("UserLevel") = "0"
					end if
				else
					Session("UserID") = "-1"
					Session("UserName") = ""
					Session("UserLogged") = "0"
					Session("UserLevel") = "0"
				end if
			end if
		End Sub
	End Class


	'---------------------------------------------------------------------------------------------------
	' DeleteTopic - Codebehind For deletetopic.aspx
	'---------------------------------------------------------------------------------------------------
	Public Class DeleteTopic
		Inherits System.Web.UI.Page

		Public DeleteButton As System.Web.UI.WebControls.Button
		Public TopicSubject As System.Web.UI.WebControls.Label
		Public ForumID As System.Web.UI.WebControls.TextBox
		Public PagePanel As System.Web.UI.WebControls.Panel
		Public NoItemsDiv As System.Web.UI.HtmlControls.HtmlGenericControl

		Public DMGHeader As DMGForums.Global.Header
		Public DMGFooter As DMGForums.Global.Footer
		Public DMGLogin As DMGForums.Global.Login

		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
			if Not Page.IsPostBack() then
				if (Functions.IsInteger(Request.QueryString("ID"))) then
					Dim AllowModeration as Boolean
					Dim ModeratorReader as OdbcDataReader = Database.Read("SELECT FORUM_ID FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_ID = " & Request.Querystring("ID"))
						While ModeratorReader.Read()
							AllowModeration = Functions.IsModerator(Session("UserID"), Session("UserLevel"), ModeratorReader("FORUM_ID"))
						End While
					ModeratorReader.Close()

					if AllowModeration then
						Dim TopicReader as OdbcDataReader = Database.Read("SELECT T.TOPIC_ID, T.TOPIC_SUBJECT, T.FORUM_ID, F.FORUM_SHOWHEADERS, F.FORUM_SHOWLOGIN FROM " & Database.DBPrefix & "_TOPICS T Left Outer Join " & Database.DBPrefix & "_FORUMS F On T.FORUM_ID = F.FORUM_ID WHERE T.TOPIC_ID = " & Request.QueryString("ID"))
							if TopicReader.HasRows then
								While(TopicReader.Read())
									DeleteButton.CommandArgument = TopicReader("TOPIC_ID")
									TopicSubject.text = TopicReader("TOPIC_SUBJECT").ToString()
									ForumID.text = TopicReader("FORUM_ID")
									if (TopicReader("FORUM_SHOWHEADERS") <> 1) then
										DMGHeader.visible = "false"
										DMGFooter.visible = "false"
									end if
									DMGLogin.ShowLogin() = TopicReader("FORUM_SHOWLOGIN")
								End While
							else
								PagePanel.visible = "false"
								NoItemsDiv.InnerHtml = "Invalid Topic ID<br /><br />"
							end if
						TopicReader.close()
					else
						PagePanel.visible = "false"
						NoItemsDiv.InnerHtml = "Access Denied<br /><br />"
					end if
				else
					Response.Redirect("default.aspx")
				end if
			end if
		End Sub

		Sub DeleteTopic(sender As System.Object, e As System.EventArgs)
			PagePanel.visible = "false"
			Dim Replies, ForumTopics, ForumPosts as Integer

			Dim Reader as OdbcDataReader = Database.Read("SELECT TOPIC_FILEUPLOAD FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_ID = " & sender.CommandArgument, 1)
			While Reader.Read()
				if Reader("TOPIC_FILEUPLOAD").ToString() <> "" then
					Database.Write("DELETE " & Database.DBPrefix & "_FILES FROM " & Database.DBPrefix & "_FILES, " & Database.DBPrefix & "_FOLDERS WHERE " & Database.DBPrefix & "_FILES.FILE_FOLDER = " & Database.DBPrefix & "_FOLDERS.FOLDER_ID AND " & Database.DBPrefix & "_FILES.FILE_NAME = '" & Reader("TOPIC_FILEUPLOAD").ToString() & "' AND " & Database.DBPrefix & "_FOLDERS.FOLDER_NAME = 'topicfiles' AND " & Database.DBPrefix & "_FOLDERS.FOLDER_PARENT = 0")
					File.Delete(MapPath("topicfiles/" & Reader("TOPIC_FILEUPLOAD").ToString()))
				end if
			End While
			Reader.Close()

			Dim ReplyCount as OdbcDataReader = Database.Read("SELECT COUNT(*) AS ReplyCounter FROM " & Database.DBPrefix & "_REPLIES WHERE TOPIC_ID = " & sender.CommandArgument)
				While ReplyCount.Read()
					Replies = ReplyCount("ReplyCounter")
				End While
			ReplyCount.Close()

			Dim ForumCounts as OdbcDataReader = Database.Read("SELECT FORUM_TOPICS, FORUM_POSTS FROM " & Database.DBPrefix & "_FORUMS WHERE FORUM_ID = " & ForumID.text)
				While ForumCounts.Read()
					ForumTopics = ForumCounts("FORUM_TOPICS")
					ForumPosts = ForumCounts("FORUM_POSTS")
				End While
			ForumCounts.Close()

			Database.Write("DELETE FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_ID = " & sender.CommandArgument)
			Database.Write("DELETE FROM " & Database.DBPrefix & "_REPLIES WHERE TOPIC_ID = " & sender.CommandArgument)

			if ForumTopics = 1 then
				Database.Write("UPDATE " & Database.DBPrefix & "_FORUMS SET FORUM_TOPICS = " & ForumTopics - 1 & ", FORUM_POSTS = " & ForumPosts - Replies - 1 & ", FORUM_LASTPOST_AUTHOR = 0 WHERE FORUM_ID = " & ForumID.text)
			else
				Dim LastPostAuthor as String = ""
				Dim LastPostDate as String = ""
				Dim AuthorReader as OdbcDataReader = Database.Read("SELECT TOPIC_LASTPOST_AUTHOR, TOPIC_LASTPOST_DATE FROM " & Database.DBPrefix & "_TOPICS WHERE FORUM_ID = " & ForumID.text & " ORDER BY TOPIC_LASTPOST_DATE DESC", 1)
					While AuthorReader.Read()
						LastPostAuthor = AuthorReader("TOPIC_LASTPOST_AUTHOR").ToString()
						LastPostDate = AuthorReader("TOPIC_LASTPOST_DATE").ToString()
					End While
				AuthorReader.Close()
				Database.Write("UPDATE " & Database.DBPrefix & "_FORUMS SET FORUM_TOPICS = " & ForumTopics - 1 & ", FORUM_POSTS = " & ForumPosts - Replies - 1 & ", FORUM_LASTPOST_AUTHOR = " & LastPostAuthor & ", FORUM_LASTPOST_DATE = '" & LastPostDate & "' WHERE FORUM_ID = " & ForumID.text)
			end if

			NoItemsDiv.InnerHtml = "Topic Deleted Successfully<br /><br /><a href=""forums.aspx?ID=" & ForumID.text & """>Click Here</a> To Return To The Forum<br /><br />"
		End Sub
	End Class


	'---------------------------------------------------------------------------------------------------
	' Active - Codebehind For active.aspx
	'---------------------------------------------------------------------------------------------------
	Public Class Active
		Inherits System.Web.UI.Page

		Public Forum As System.Web.UI.WebControls.Repeater
		Public NoItemsDiv As System.Web.UI.HtmlControls.HtmlGenericControl

		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
			if Not Page.IsPostBack() then
				if (Session("ActiveLevel") is Nothing) or (Session("ActiveLevel") = 3) or (Session("ActiveTime") is Nothing) then
					Session("ActiveTime") = Database.DatabaseTimestamp(3)
				end if

				Dim TimeFrame as String = "'" & Session("ActiveTime") & "'"

				Dim DataSet1 As new DataSet()
				Dim strSql As new OdbcCommand("SELECT F.FORUM_ID, F.FORUM_NAME, C.CATEGORY_ID, C.CATEGORY_NAME FROM " & Database.DBPrefix & "_FORUMS F LEFT OUTER JOIN " & Database.DBPrefix & "_CATEGORIES C ON F.CATEGORY_ID = C.CATEGORY_ID WHERE F.FORUM_TOPICS > 0 AND F.FORUM_STATUS <> 0 AND " & Database.GetDateDiff("ss", "F.FORUM_LASTPOST_DATE", TimeFrame) & " <= 0 ORDER BY C.CATEGORY_SORTBY, F.FORUM_SORTBY, F.FORUM_NAME", Database.DatabaseConnection())

				Dim DataAdapter1 As new OdbcDataAdapter()
				DataAdapter1.SelectCommand = strSql
				DataAdapter1.Fill(DataSet1, "FORUMS")

				strSql = new OdbcCommand("SELECT T.FORUM_ID, T.TOPIC_ID, T.TOPIC_SUBJECT, T.TOPIC_AUTHOR, T.TOPIC_STATUS, T.TOPIC_CONFIRMED, M.MEMBER_USERNAME as TOPIC_AUTHOR_NAME, T.TOPIC_REPLIES, T.TOPIC_VIEWS, T.TOPIC_STICKY, T.TOPIC_LASTPOST_AUTHOR, MEMBERS_1.MEMBER_USERNAME as TOPIC_LASTPOST_NAME, T.TOPIC_LASTPOST_DATE FROM " & Database.DBPrefix & "_MEMBERS M, " & Database.DBPrefix & "_TOPICS T, " & Database.DBPrefix & "_MEMBERS as MEMBERS_1, " & Database.DBPrefix & "_FORUMS F WHERE M.MEMBER_ID = T.TOPIC_AUTHOR and T.TOPIC_LASTPOST_AUTHOR = MEMBERS_1.MEMBER_ID and T.FORUM_ID = F.FORUM_ID and T.TOPIC_STATUS <> 0 and T.TOPIC_CONFIRMED = 1 and F.FORUM_STATUS <> 0 and F.FORUM_TOPICS > 0 AND " & Database.GetDateDiff("ss", "F.FORUM_LASTPOST_DATE", TimeFrame) & " <= 0 and " & Database.GetDateDiff("ss", "T.TOPIC_LASTPOST_DATE", TimeFrame) & " <= 0 ORDER BY T.TOPIC_STICKY DESC, T.TOPIC_LASTPOST_DATE DESC", Database.DatabaseConnection())
				DataAdapter1.SelectCommand = strSql
				DataAdapter1.Fill(DataSet1, "TOPICS")

				DataSet1.Relations.Add("TopicRelation", DataSet1.Tables("FORUMS").Columns("FORUM_ID"), DataSet1.Tables("TOPICS").Columns("FORUM_ID"))

				Forum.DataSource = DataSet1
				Forum.DataBind()

				if (Forum.Items.Count = 0) then
					NoItemsDiv.InnerHtml = "There Are No Items To Display<br /><br />"
				end if
			end if
		End Sub
	End Class


	'---------------------------------------------------------------------------------------------------
	' NewReply - Codebehind For newreply.aspx
	'---------------------------------------------------------------------------------------------------
	Public Class NewReply
		Inherits System.Web.UI.Page

		Public txtTopicID As System.Web.UI.WebControls.TextBox
		Public txtForumID As System.Web.UI.WebControls.TextBox
		Public txtAuthor As System.Web.UI.WebControls.TextBox
		Public txtMessage As System.Web.UI.WebControls.TextBox
		Public txtSignature As System.Web.UI.WebControls.CheckBox
		Public txtTopicSubject As System.Web.UI.WebControls.Label
		Public ReplyPreview As System.Web.UI.WebControls.PlaceHolder
		Public PagePanel As System.Web.UI.WebControls.Panel
		Public NoItemsDiv As System.Web.UI.HtmlControls.HtmlGenericControl

		Public PrevSubject as String = ""
		Public PrevDate as String = ""
		Public PrevMessage as String = ""
		Public PrevSignature as String = ""

		Public DMGHeader As DMGForums.Global.Header
		Public DMGFooter As DMGForums.Global.Footer
		Public DMGLogin As DMGForums.Global.Login

		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
			if Not Page.IsPostBack() then
				SetSession()

				if (Session("UserLogged") = "1") then
					if (Functions.IsInteger(Request.QueryString("ID"))) then
						Dim StatusCheck as String
						if Session("UserLevel") = 3 then
							StatusCheck = ""
						else
							StatusCheck = "T.TOPIC_STATUS = 1 AND F.FORUM_STATUS = 1 AND "
						end if
							
						Dim ForumType, ForumID as Integer
						Dim TopicReader as OdbcDataReader = Database.Read("SELECT T.TOPIC_ID, T.TOPIC_SUBJECT, T.FORUM_ID, F.FORUM_TYPE, F.FORUM_STATUS, F.FORUM_SHOWHEADERS, F.FORUM_SHOWLOGIN FROM " & Database.DBPrefix & "_TOPICS T LEFT OUTER JOIN " & Database.DBPrefix & "_FORUMS F ON T.FORUM_ID = F.FORUM_ID WHERE " & StatusCheck & "T.TOPIC_ID = " & Request.Querystring("ID"))
							if (Not TopicReader.HasRows) then
								PagePanel.visible = "false"
								NoItemsDiv.InnerHtml = "No Topic To Post To<br /><br />"
							else
								While TopicReader.Read()
									txtTopicID.text = TopicReader("TOPIC_ID")
									txtForumID.text = TopicReader("FORUM_ID")
									txtTopicSubject.text = TopicReader("TOPIC_SUBJECT").ToString()
									ForumType = TopicReader("FORUM_TYPE")
									ForumID = TopicReader("FORUM_ID")
									if (TopicReader("FORUM_SHOWHEADERS") <> 1) then
										DMGHeader.visible = "false"
										DMGFooter.visible = "false"
									end if
									DMGLogin.ShowLogin() = TopicReader("FORUM_SHOWLOGIN")
								End While

								if (ForumType = 1) or (ForumType = 3) or (ForumType = 4) then
									if Not Functions.IsPrivileged(ForumID, ForumType, Session("UserID"), Session("UserLevel"), Session("UserLogged")) then
										PagePanel.visible = "false"
										NoItemsDiv.InnerHtml = "You Do Not Have Access To This Forum<br /><br />"
									end if
								elseif (ForumType = 2) then
									if Not Session("FORUM_" & ForumID) = "logged" then
										PagePanel.visible = "false"
										NoItemsDiv.InnerHtml = "You Do Not Have Access To This Forum<br /><br />"
									end if
								end if

								Dim UserReader as OdbcDataReader = Database.Read("SELECT MEMBER_ID, MEMBER_LEVEL, MEMBER_SIGNATURE_SHOW FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & Session("UserID"))
									While UserReader.Read()
										txtAuthor.text = UserReader("MEMBER_ID")
										if UserReader("MEMBER_SIGNATURE_SHOW") = 1 then
											txtSignature.checked = "true"
										end if
									End While
								UserReader.Close()
							end if
						TopicReader.Close()

						Dim Reader as OdbcDataReader
						if (Functions.IsInteger(Request.QueryString("RQ"))) then
							Dim ReplyQuoteMessage as String = ""
							Reader = Database.Read("SELECT M.MEMBER_USERNAME, R.REPLY_MESSAGE FROM " & Database.DBPrefix & "_MEMBERS M Left Outer Join " & Database.DBPrefix & "_REPLIES R On R.REPLY_AUTHOR = M.MEMBER_ID WHERE R.REPLY_ID = " & Request.QueryString("RQ"))
							While Reader.Read()
								ReplyQuoteMessage = Reader("REPLY_MESSAGE").ToString()
								ReplyQuoteMessage = Regex.Replace(ReplyQuoteMessage, "(\[quote\])((.|\n)*?)(\[\/quote\])(\r\n\r\n)", "")
								ReplyQuoteMessage = Regex.Replace(ReplyQuoteMessage, "(\[quote\])((.|\n)*?)(\[\/quote\])", "")
								txtMessage.text = "[quote][b]Quoted From " & Reader("MEMBER_USERNAME") & ":[/b] " & CHR(10) & CHR(10) & Server.HTMLDecode(ReplyQuoteMessage) & CHR(10) & "[/quote]" & CHR(10) & CHR(10)
							End While
							Reader.Close()
						end if

						if (Functions.IsInteger(Request.QueryString("TQ"))) then
							Dim TopicQuoteMessage as String = ""
							Reader = Database.Read("SELECT M.MEMBER_USERNAME, T.TOPIC_MESSAGE FROM " & Database.DBPrefix & "_MEMBERS M Left Outer Join " & Database.DBPrefix & "_TOPICS T On T.TOPIC_AUTHOR = M.MEMBER_ID WHERE T.TOPIC_ID = " & Request.QueryString("TQ"))
							While Reader.Read()
								TopicQuoteMessage = Reader("TOPIC_MESSAGE").ToString()
								TopicQuoteMessage = Regex.Replace(TopicQuoteMessage, "(\[quote\])((.|\n)*?)(\[\/quote\])(\r\n\r\n)", "")
								TopicQuoteMessage = Regex.Replace(TopicQuoteMessage, "(\[quote\])((.|\n)*?)(\[\/quote\])", "")
								txtMessage.text = "[quote][b]Quoted From " & Reader("MEMBER_USERNAME") & ":[/b] " & CHR(10) & CHR(10) & Server.HTMLDecode(TopicQuoteMessage) & CHR(10) & "[/quote]" & CHR(10) & CHR(10)
							End While
							Reader.Close()
						end if
					else
						PagePanel.visible = "false"
						NoItemsDiv.InnerHtml = "No Forums To Post To<br /><br />"
					end if
				else
					PagePanel.visible = "false"
					NoItemsDiv.InnerHtml = "You Must Be Logged In To Post<br /><br />"
				end if
			end if
		End Sub

		Sub SubmitReply(sender As System.Object, e As System.EventArgs)
			Dim Failure as Integer = 0
			Dim SpamSeconds as Integer

			if (txtMessage.text = "") or (txtMessage.text = " ") then
				Failure = 1
				Functions.Messagebox("No Message Entered!")
			end if

			if Failure = 0 then
				Dim TopicReplies, ReplyID as Integer
				Dim Signature as Integer = 0
				if (txtSignature.Checked) then
					Signature = 1
				end if

				PagePanel.visible = "false"

				Dim SpamReader as OdbcDataReader
				SpamReader = Database.Read("SELECT " & Database.GetDateDiff("ss", "MEMBER_DATE_LASTPOST", Database.GetTimeStamp()) & " as PostSeconds FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & txtAuthor.text)
				While SpamReader.Read()
					if Functions.IsDBNull(SpamReader("PostSeconds")) then
						SpamSeconds = -99
					else
						SpamSeconds = SpamReader("PostSeconds")
					end if
				End While
				SpamReader.Close()

				if (SpamSeconds > Settings.SpamFilter) or (SpamSeconds = -99) or (Session("UserLevel") = "3") then
					Dim ForceConfirm as Integer = 0
					Dim ForumReader as OdbcDataReader = Database.Read("SELECT FORUM_FORCECONFIRM FROM " & Database.DBPrefix & "_FORUMS WHERE FORUM_ID = " & txtForumID.text)
					While ForumReader.Read()
						ForceConfirm = ForumReader("FORUM_FORCECONFIRM")
					End While
					ForumReader.Close()

					Dim ReplyConfirm as Integer = 1
					if (ForceConfirm = 1) and (Session("UserLevel") <> "3") and (Session("UserLevel") <> "2") then
						ReplyConfirm = 0
					end if

					Database.Write("INSERT INTO " & Database.DBPrefix & "_REPLIES (TOPIC_ID, REPLY_MESSAGE, REPLY_DATE, REPLY_AUTHOR, REPLY_SIGNATURE, REPLY_CONFIRMED) VALUES (" & txtTopicID.text & ", '" & Functions.RepairString(txtMessage.text) & "', " & Database.GetTimeStamp() & ", " & txtAuthor.text & ", " & Signature & ", " & ReplyConfirm & ")")
					Database.Write("UPDATE " & Database.DBPrefix & "_MEMBERS SET MEMBER_POSTS = (MEMBER_POSTS + 1), MEMBER_DATE_LASTPOST = " & Database.GetTimeStamp() & " WHERE MEMBER_ID = " & txtAuthor.text)

					if (ReplyConfirm = 1) then
						Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_REPLIES = (TOPIC_REPLIES + 1), TOPIC_LASTPOST_DATE = " & Database.GetTimeStamp() & ", TOPIC_LASTPOST_AUTHOR = " & txtAuthor.text & " WHERE TOPIC_ID = " & txtTopicID.text)
						Database.Write("UPDATE " & Database.DBPrefix & "_FORUMS SET FORUM_POSTS = (FORUM_POSTS + 1), FORUM_LASTPOST_DATE = " & Database.GetTimeStamp() & ", FORUM_LASTPOST_AUTHOR = " & txtAuthor.text & " WHERE FORUM_ID = " & txtForumID.text)
	
						Functions.SendToSubscribers(txtTopicID.text)
	
							Dim ReplyPostback as OdbcDataReader = Database.Read("SELECT REPLY_ID FROM " & Database.DBPrefix & "_REPLIES WHERE TOPIC_ID = " & txtTopicID.text & " AND REPLY_AUTHOR = " & txtAuthor.text & " ORDER BY REPLY_ID DESC", 1)
								While ReplyPostback.Read()
									ReplyID = ReplyPostback("REPLY_ID")
								End While
							ReplyPostback.Close()

							Dim Reader as OdbcDataReader = Database.Read("SELECT TOPIC_REPLIES FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_ID = " & txtTopicID.text)
							While Reader.Read()
								TopicReplies = Reader("TOPIC_REPLIES")
							End While
							Reader.Close()

							Dim PageItems as Integer = Settings.ItemsPerPage
							Dim NumPages as Integer = TopicReplies \ PageItems
							Dim Leftover as Integer = TopicReplies Mod PageItems
							If Leftover > 0 then
								NumPages += 1
							end if

						NoItemsDiv.InnerHtml = "Reply Posted Successfully<br /><br /><a href=""topics.aspx?ID=" & txtTopicID.text & "&PAGE=" & NumPages & "#reply-" & ReplyID & """>Click Here</a> To Return To The Thread<br /><br />"
					else
						Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_UNCONFIRMED_REPLIES = (TOPIC_UNCONFIRMED_REPLIES + 1) WHERE TOPIC_ID = " & txtTopicID.text)
						Functions.SendToModerators(2, txtTopicID.text, txtForumID.text)
						NoItemsDiv.InnerHtml = Functions.CustomMessage("MESSAGE_CONFIRMPOST") & "<br /><br /><a href=""forums.aspx?ID=" & txtForumID.text & """>Click Here</a> To Return To The Forum<br /><br />"
					end if
				else
					NoItemsDiv.InnerHtml = "You Can Not Post More Than Once In " & Settings.SpamFilter & " Seconds.<br /><br /><a href=""topics.aspx?ID=" & txtTopicID.text & """>Click Here</a> To Return To The Thread<br /><br />"
				end if
			end if
		End Sub

		Sub PreviewReply(sender As Object, e As EventArgs)
			Dim Failure as Integer = 0
			Dim ShowSig as Integer = 0

			if (txtMessage.text = "") or (txtMessage.text = " ") then
				Failure = 1
				Functions.Messagebox("No Message Entered!")
			end if
			if (txtSignature.Checked) then
				ShowSig = 1
			end if

			if Failure = 0 then
				ReplyPreview.visible = "true"
				PrevSubject = txtTopicSubject.text
				PrevMessage = Functions.FormatString(Functions.RepairString(txtMessage.text))
				PrevDate = DateTime.Now()

				if (ShowSig = 1) then
					Dim Reader as OdbcDataReader = Database.Read("SELECT MEMBER_SIGNATURE FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & Session("UserID"))
					While Reader.Read()
						PrevSignature = Functions.Signature(1, Reader("MEMBER_SIGNATURE").ToString())
					End While
					Reader.Close()
				end if
			end if
		End Sub

		Sub SetSession()
			if Not Request.Cookies("dmgforums") Is Nothing then
				Dim aCookie As New System.Web.HttpCookie("dmgforums")

				Dim UserReader as OdbcDataReader = Database.Read("SELECT MEMBER_ID, MEMBER_USERNAME, MEMBER_PASSWORD, MEMBER_LEVEL FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & Server.HtmlEncode(Request.Cookies("dmgforums")("mukul")) & " AND MEMBER_PASSWORD = '" & Server.HtmlEncode(Request.Cookies("dmgforums")("gupta")) & "'", 1)
					While(UserReader.Read())
						Session("UserID") = UserReader("MEMBER_ID").ToString()
						Session("UserName") = UserReader("MEMBER_USERNAME").ToString()
						Session("UserLevel") = UserReader("MEMBER_LEVEL").ToString()
						Session("UserLogged") = "1"
						aCookie.Values("fighter") = Functions.Encrypt(DateTime.Now())
						aCookie.Values("mukul") = UserReader("MEMBER_ID").ToString()
						aCookie.Values("dooder") = Functions.Encrypt(DateTime.Now())
						aCookie.Values("gupta") = UserReader("MEMBER_PASSWORD").ToString()
						aCookie.Expires = DateTime.Now.AddDays(30)
						Response.Cookies.Add(aCookie)
					End While
				UserReader.Close()

				if ((Session("UserLevel") = 0) or (Session("UserLevel") = -1)) then
					aCookie.Values("fighter") = Functions.Encrypt(DateTime.Now())
					aCookie.Values("mukul") = "-3"
					aCookie.Values("dooder") = Functions.Encrypt(DateTime.Now())
					aCookie.Values("gupta") = Functions.Encrypt(DateTime.Now())
					aCookie.Expires = DateTime.Now.AddDays(-1)
					Response.Cookies.Add(aCookie)
					Session("UserID") = "-1"
					Session("UserName") = ""
					Session("UserLogged") = "0"
					Session("UserLevel") = "0"
				end if
			else
				if (Session("UserLogged") = "1") then
					Dim UserReader as OdbcDataReader = Database.Read("SELECT MEMBER_ID, MEMBER_USERNAME, MEMBER_PASSWORD, MEMBER_LEVEL FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & Session("UserID"), 1)
						While(UserReader.Read())
							Session("UserID") = UserReader("MEMBER_ID").ToString()
							Session("UserName") = UserReader("MEMBER_USERNAME").ToString()
							Session("UserLevel") = UserReader("MEMBER_LEVEL").ToString()
							Session("UserLogged") = "1"
						End While
					UserReader.Close()

					if ((Session("UserLevel") = 0) or (Session("UserLevel") = -1)) then
						Dim aCookie As New System.Web.HttpCookie("dmgforums")
						aCookie.Values("fighter") = Functions.Encrypt(DateTime.Now())
						aCookie.Values("mukul") = "-3"
						aCookie.Values("dooder") = Functions.Encrypt(DateTime.Now() & "bbb")
						aCookie.Values("gupta") = Functions.Encrypt(DateTime.Now() & "ccc")
						aCookie.Expires = DateTime.Now.AddDays(-1)
						Response.Cookies.Add(aCookie)
						Session("UserID") = "-1"
						Session("UserName") = ""
						Session("UserLogged") = "0"
						Session("UserLevel") = "0"
					end if
				else
					Session("UserID") = "-1"
					Session("UserName") = ""
					Session("UserLogged") = "0"
					Session("UserLevel") = "0"
				end if
			end if
		End Sub
	End Class


	'---------------------------------------------------------------------------------------------------
	' EditReply - Codebehind For editreply.aspx
	'---------------------------------------------------------------------------------------------------
	Public Class EditReply
		Inherits System.Web.UI.Page

		Public txtTopicID As System.Web.UI.WebControls.TextBox
		Public txtMessage As System.Web.UI.WebControls.TextBox
		Public txtSignature As System.Web.UI.WebControls.CheckBox
		Public txtTopicSubject As System.Web.UI.WebControls.Label
		Public PagePanel As System.Web.UI.WebControls.Panel
		Public NoItemsDiv As System.Web.UI.HtmlControls.HtmlGenericControl

		Public DMGHeader As DMGForums.Global.Header
		Public DMGFooter As DMGForums.Global.Footer
		Public DMGLogin As DMGForums.Global.Login

		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
			if Not Page.IsPostBack() then
				SetSession()

				if (Functions.IsInteger(Request.QueryString("ID"))) then
					Dim AllowModeration as Boolean
					Dim ModeratorReader as OdbcDataReader = Database.Read("SELECT T.FORUM_ID, R.REPLY_AUTHOR FROM " & Database.DBPrefix & "_REPLIES R Left Outer Join " & Database.DBPrefix & "_TOPICS T on R.TOPIC_ID = T.TOPIC_ID WHERE R.REPLY_ID = " & Request.Querystring("ID"))
						While ModeratorReader.Read()
							AllowModeration = Functions.IsModerator(Session("UserID"), Session("UserLevel"), ModeratorReader("FORUM_ID"))
							if (Not AllowModeration) and (ModeratorReader("REPLY_AUTHOR").ToString() = Session("UserID")) then
								AllowModeration = True
							end if
						End While
					ModeratorReader.Close()

					if AllowModeration then
						Dim ReplyReader as OdbcDataReader = Database.Read("SELECT T.TOPIC_SUBJECT, R.REPLY_MESSAGE, R.REPLY_SIGNATURE, R.TOPIC_ID, F.FORUM_SHOWHEADERS, F.FORUM_SHOWLOGIN FROM " & Database.DBPrefix & "_REPLIES R LEFT OUTER JOIN " & Database.DBPrefix & "_TOPICS T ON R.TOPIC_ID = T.TOPIC_ID Left Outer Join " & Database.DBPrefix & "_FORUMS F On T.FORUM_ID = F.FORUM_ID WHERE R.REPLY_ID = " & Request.Querystring("ID"))
							While ReplyReader.Read()
								txtTopicSubject.text = ReplyReader("TOPIC_SUBJECT").ToString()
								txtMessage.text = Server.HTMLDecode(ReplyReader("REPLY_MESSAGE").ToString())
								if ReplyReader("REPLY_SIGNATURE") = 1 then
									txtSignature.Checked = "true"
								end if
								txtTopicID.text = ReplyReader("TOPIC_ID")
								if (ReplyReader("FORUM_SHOWHEADERS") <> 1) then
									DMGHeader.visible = "false"
									DMGFooter.visible = "false"
								end if
								DMGLogin.ShowLogin() = ReplyReader("FORUM_SHOWLOGIN")
							End While
						ReplyReader.Close()
					else
						PagePanel.visible = "false"
						NoItemsDiv.InnerHtml = "Access Denied<br /><br />"
					end if
				else
					Response.Redirect("default.aspx")
				end if
			end if
		End Sub

		Sub SubmitReply(sender As System.Object, e As System.EventArgs)
			Dim Signature as Integer = 0
			if (txtSignature.Checked) then
				Signature = 1
			end if

			PagePanel.visible = "false"

			Database.Write("UPDATE " & Database.DBPrefix & "_REPLIES SET REPLY_MESSAGE = '" & Functions.RepairString(txtMessage.Text) & "', REPLY_SIGNATURE = " & Signature & " WHERE REPLY_ID = " & Request.Querystring("ID"))

			NoItemsDiv.InnerHtml = "Reply Edited Successfully<br /><br /><a href=""topics.aspx?ID=" & txtTopicID.text & """>Click Here</a> To Return To The Thread<br /><br />"
		End Sub

		Sub SetSession()
			if Not Request.Cookies("dmgforums") Is Nothing then
				Dim aCookie As New System.Web.HttpCookie("dmgforums")

				Dim UserReader as OdbcDataReader = Database.Read("SELECT MEMBER_ID, MEMBER_USERNAME, MEMBER_PASSWORD, MEMBER_LEVEL FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & Server.HtmlEncode(Request.Cookies("dmgforums")("mukul")) & " AND MEMBER_PASSWORD = '" & Server.HtmlEncode(Request.Cookies("dmgforums")("gupta")) & "'", 1)
					While(UserReader.Read())
						Session("UserID") = UserReader("MEMBER_ID").ToString()
						Session("UserName") = UserReader("MEMBER_USERNAME").ToString()
						Session("UserLevel") = UserReader("MEMBER_LEVEL").ToString()
						Session("UserLogged") = "1"
						aCookie.Values("fighter") = Functions.Encrypt(DateTime.Now())
						aCookie.Values("mukul") = UserReader("MEMBER_ID").ToString()
						aCookie.Values("dooder") = Functions.Encrypt(DateTime.Now())
						aCookie.Values("gupta") = UserReader("MEMBER_PASSWORD").ToString()
						aCookie.Expires = DateTime.Now.AddDays(30)
						Response.Cookies.Add(aCookie)
					End While
				UserReader.Close()

				if ((Session("UserLevel") = 0) or (Session("UserLevel") = -1)) then
					aCookie.Values("fighter") = Functions.Encrypt(DateTime.Now())
					aCookie.Values("mukul") = "-3"
					aCookie.Values("dooder") = Functions.Encrypt(DateTime.Now())
					aCookie.Values("gupta") = Functions.Encrypt(DateTime.Now())
					aCookie.Expires = DateTime.Now.AddDays(-1)
					Response.Cookies.Add(aCookie)
					Session("UserID") = "-1"
					Session("UserName") = ""
					Session("UserLogged") = "0"
					Session("UserLevel") = "0"
				end if
			else
				if (Session("UserLogged") = "1") then
					Dim UserReader as OdbcDataReader = Database.Read("SELECT MEMBER_ID, MEMBER_USERNAME, MEMBER_PASSWORD, MEMBER_LEVEL FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & Session("UserID"), 1)
						While(UserReader.Read())
							Session("UserID") = UserReader("MEMBER_ID").ToString()
							Session("UserName") = UserReader("MEMBER_USERNAME").ToString()
							Session("UserLevel") = UserReader("MEMBER_LEVEL").ToString()
							Session("UserLogged") = "1"
						End While
					UserReader.Close()

					if ((Session("UserLevel") = 0) or (Session("UserLevel") = -1)) then
						Dim aCookie As New System.Web.HttpCookie("dmgforums")
						aCookie.Values("fighter") = Functions.Encrypt(DateTime.Now())
						aCookie.Values("mukul") = "-3"
						aCookie.Values("dooder") = Functions.Encrypt(DateTime.Now() & "bbb")
						aCookie.Values("gupta") = Functions.Encrypt(DateTime.Now() & "ccc")
						aCookie.Expires = DateTime.Now.AddDays(-1)
						Response.Cookies.Add(aCookie)
						Session("UserID") = "-1"
						Session("UserName") = ""
						Session("UserLogged") = "0"
						Session("UserLevel") = "0"
					end if
				else
					Session("UserID") = "-1"
					Session("UserName") = ""
					Session("UserLogged") = "0"
					Session("UserLevel") = "0"
				end if
			end if
		End Sub
	End Class


	'---------------------------------------------------------------------------------------------------
	' DeleteReply - Codebehind For deletereply.aspx
	'---------------------------------------------------------------------------------------------------
	Public Class DeleteReply
		Inherits System.Web.UI.Page

		Public DeleteButton As System.Web.UI.WebControls.Button
		Public TopicSubject As System.Web.UI.WebControls.Label
		Public TopicID As System.Web.UI.WebControls.TextBox
		Public PagePanel As System.Web.UI.WebControls.Panel
		Public NoItemsDiv As System.Web.UI.HtmlControls.HtmlGenericControl

		Public DMGHeader As DMGForums.Global.Header
		Public DMGFooter As DMGForums.Global.Footer
		Public DMGLogin As DMGForums.Global.Login

		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
			if Not Page.IsPostBack() then
				if (Functions.IsInteger(Request.QueryString("ID"))) then
					Dim AllowModeration as Boolean
					Dim ModeratorReader as OdbcDataReader = Database.Read("SELECT T.FORUM_ID FROM " & Database.DBPrefix & "_REPLIES R Left Outer Join " & Database.DBPrefix & "_TOPICS T On R.TOPIC_ID = T.TOPIC_ID WHERE R.REPLY_ID = " & Request.Querystring("ID"))
						While ModeratorReader.Read()
							AllowModeration = Functions.IsModerator(Session("UserID"), Session("UserLevel"), ModeratorReader("FORUM_ID"))
						End While
					ModeratorReader.Close()

					if AllowModeration then
						Dim ReplyReader as OdbcDataReader = Database.Read("SELECT R.REPLY_ID, R.TOPIC_ID, T.TOPIC_SUBJECT, F.FORUM_SHOWHEADERS, F.FORUM_SHOWLOGIN FROM " & Database.DBPrefix & "_REPLIES R Left Outer Join " & Database.DBPrefix & "_TOPICS T On R.TOPIC_ID = T.TOPIC_ID Left Outer Join " & Database.DBPrefix & "_FORUMS F On T.FORUM_ID = F.FORUM_ID WHERE R.REPLY_ID = " & Request.QueryString("ID"))
							if ReplyReader.HasRows then
								While(ReplyReader.Read())
									DeleteButton.CommandArgument = ReplyReader("REPLY_ID")
									TopicSubject.text = "Topic: " & ReplyReader("TOPIC_SUBJECT").ToString()
									TopicID.text = ReplyReader("TOPIC_ID")
									if (ReplyReader("FORUM_SHOWHEADERS") <> 1) then
										DMGHeader.visible = "false"
										DMGFooter.visible = "false"
									end if
									DMGLogin.ShowLogin() = ReplyReader("FORUM_SHOWLOGIN")
								End While
							else
								PagePanel.visible = "false"
								NoItemsDiv.InnerHtml = "Invalid Reply ID<br /><br />"
							end if
						ReplyReader.close()
					else
						PagePanel.visible = "false"
						NoItemsDiv.InnerHtml = "Access Denied<br /><br />"
					end if
				else
					Response.Redirect("default.aspx")
				end if
			end if
		End Sub

		Sub DeleteReply(sender As System.Object, e As System.EventArgs)
			PagePanel.visible = "false"
			Dim Replies as Integer = 0
			Dim ForumPosts as Integer = 0
			Dim TopicAuthor as String = ""
			Dim TopicDate as String = ""
			Dim ForumID as String = ""
			Dim ReplyConfirmed as Integer = 1

			Dim TopicReader as OdbcDataReader = Database.Read("SELECT T.FORUM_ID, T.TOPIC_AUTHOR, T.TOPIC_DATE, T.TOPIC_REPLIES, F.FORUM_POSTS FROM " & Database.DBPrefix & "_TOPICS T LEFT OUTER JOIN " & Database.DBPrefix & "_FORUMS F ON T.FORUM_ID = F.FORUM_ID WHERE T.TOPIC_ID = " & TopicID.text)
				While TopicReader.Read()
					Replies = TopicReader("TOPIC_REPLIES")
					ForumPosts = TopicReader("FORUM_POSTS")
					TopicAuthor = TopicReader("TOPIC_AUTHOR").ToString()
					TopicDate = TopicReader("TOPIC_DATE").ToString()
					ForumID = TopicReader("FORUM_ID").ToString()
				End While
			TopicReader.Close()

			Dim ReplyReader as OdbcDataReader = Database.Read("SELECT REPLY_CONFIRMED FROM " & Database.DBPrefix & "_REPLIES WHERE REPLY_ID = " & sender.CommandArgument)
				While ReplyReader.Read()
					ReplyConfirmed = ReplyReader("REPLY_CONFIRMED")
				End While
			ReplyReader.Close()

			Database.Write("DELETE FROM " & Database.DBPrefix & "_REPLIES WHERE REPLY_ID = " & sender.CommandArgument)

			if (ReplyConfirmed = 1) then
				if Replies = 1 then
					Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_REPLIES = " & Replies - 1 & ", TOPIC_LASTPOST_AUTHOR = " & TopicAuthor & ", TOPIC_LASTPOST_DATE = '" & TopicDate & "' WHERE TOPIC_ID = " & TopicID.text)
				else
					Dim TopicLastAuthor as String = ""
					Dim TopicLastDate as String = ""
					Dim AuthorReader as OdbcDataReader = Database.Read("SELECT REPLY_AUTHOR, REPLY_DATE FROM " & Database.DBPrefix & "_REPLIES WHERE TOPIC_ID = " & TopicID.text & " and REPLY_CONFIRMED = 1 ORDER BY REPLY_DATE DESC", 1)
						While AuthorReader.Read()
							TopicLastAuthor = AuthorReader("REPLY_AUTHOR").ToString()
							TopicLastDate = AuthorReader("REPLY_DATE").ToString()
						End While
					AuthorReader.Close()
					Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_REPLIES = " & Replies - 1 & ", TOPIC_LASTPOST_AUTHOR = " & TopicLastAuthor & ", TOPIC_LASTPOST_DATE = '" & TopicLastDate & "' WHERE TOPIC_ID = " & TopicID.text)
				end if
				Functions.UpdateCounts(5, ForumID, 0, 0)
			else
				Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_UNCONFIRMED_REPLIES = (TOPIC_UNCONFIRMED_REPLIES-1) WHERE TOPIC_ID = " & TopicID.text)
			end if

			NoItemsDiv.InnerHtml = "Reply Deleted Successfully<br /><br /><a href=""topics.aspx?ID=" & TopicID.text & """>Click Here</a> To Return To The Thread<br /><br />"
		End Sub
	End Class


	'---------------------------------------------------------------------------------------------------
	' PMInbox - Codebehind For pm_inbox.aspx
	'---------------------------------------------------------------------------------------------------
	Public Class PMInbox
		Inherits System.Web.UI.Page

		Public ConfirmDeletePM As System.Web.UI.WebControls.Panel
		Public ConfirmDeletePMButton As System.Web.UI.WebControls.Button
		Public ConfirmDeletePMDropDown As System.Web.UI.WebControls.DropDownList
		Public PagePanel As System.Web.UI.WebControls.PlaceHolder
		Public PMList As System.Web.UI.WebControls.Repeater
		Public NoItemsDiv As System.Web.UI.HtmlControls.HtmlGenericControl

		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
			if Not Page.IsPostBack() then
				if Session("UserLogged") = "1" then
					PMList.Datasource = Database.Read("SELECT T.TOPIC_ID, T.TOPIC_TO, T.TOPIC_FROM, T.TOPIC_FROM_READ, T.TOPIC_TO_READ, T.TOPIC_SUBJECT, T.TOPIC_REPLIES, T.TOPIC_LASTPOST_AUTHOR, T.TOPIC_LASTPOST_DATE, M.MEMBER_USERNAME, LP.MEMBER_USERNAME as TOPIC_LASTPOST_NAME, TT.MEMBER_USERNAME as TOPIC_TO_NAME FROM " & Database.DBPrefix & "_PM_TOPICS T, " & Database.DBPrefix & "_MEMBERS M, " & Database.DBPrefix & "_MEMBERS as LP, " & Database.DBPrefix & "_MEMBERS as TT WHERE (T.TOPIC_FROM = M.MEMBER_ID) and (T.TOPIC_LASTPOST_AUTHOR = LP.MEMBER_ID) and (T.TOPIC_TO = TT.MEMBER_ID) and ((T.TOPIC_TO = " & Session("UserID") & " and T.TOPIC_SHOWRECEIVER = 1) or (T.TOPIC_FROM = " & Session("UserID") & " and T.TOPIC_SHOWSENDER = 1)) ORDER BY T.TOPIC_LASTPOST_DATE DESC")
					PMList.DataBind()
					if (PMList.Items.Count = 0) then
						PMList.visible = "false"
						NoItemsDiv.InnerHtml = "There Are No Items In The Inbox<br /><br />"
					end if
					PMList.Datasource.Close()
				else
					PagePanel.visible = "false"
					NoItemsDiv.InnerHTML = "You Must Be Logged In To Use Private Messaging<br /><br />"
				end if
			end if
		End Sub

		Sub DeletePMConfirm(sender As System.Object, e As System.EventArgs)
			PagePanel.visible = "false"
			ConfirmDeletePM.visible = "true"
			ConfirmDeletePMButton.CommandArgument = sender.CommandArgument
		End Sub

		Sub DeletePM(sender As System.Object, e As System.EventArgs)
			PagePanel.visible = "false"
			ConfirmDeletePM.visible = "false"

			Dim PMTopicID as Integer = sender.CommandArgument
			Dim Confirmation as Integer = ConfirmDeletePMDropDown.SelectedValue
			Dim PMTo as Integer = 0
			Dim PMFrom as Integer = 0
			Dim PMShowSender as Integer = 0
			Dim PMShowReceiver as Integer = 0

			if (Confirmation = 1) then
				Dim Reader as OdbcDataReader = Database.Read("SELECT TOPIC_TO, TOPIC_FROM, TOPIC_SHOWSENDER, TOPIC_SHOWRECEIVER FROM " & Database.DBPrefix & "_PM_TOPICS WHERE TOPIC_ID = " & PMTopicID)
					While Reader.Read()
						PMTo = Reader("TOPIC_TO")
						PMFrom = Reader("TOPIC_FROM")
						PMShowSender = Reader("TOPIC_SHOWSENDER")
						PMShowReceiver = Reader("TOPIC_SHOWRECEIVER")
					End While
				Reader.Close()

				if (PMTo = Session("UserID")) then
					if (PMShowSender = 1) then
						Database.Write("UPDATE " & Database.DBPrefix & "_PM_TOPICS SET TOPIC_SHOWRECEIVER = 0 WHERE TOPIC_ID = " & PMTopicID)
					else
						Database.Write("DELETE FROM " & Database.DBPrefix & "_PM_REPLIES WHERE TOPIC_ID = " & PMTopicID)
						Database.Write("DELETE FROM " & Database.DBPrefix & "_PM_TOPICS WHERE TOPIC_ID = " & PMTopicID)
					end if
				elseif (PMFrom = Session("UserID")) then
					if (PMShowReceiver = 1) then
						Database.Write("UPDATE " & Database.DBPrefix & "_PM_TOPICS SET TOPIC_SHOWSENDER = 0 WHERE TOPIC_ID = " & PMTopicID)
					else
						Database.Write("DELETE FROM " & Database.DBPrefix & "_PM_REPLIES WHERE TOPIC_ID = " & PMTopicID)
						Database.Write("DELETE FROM " & Database.DBPrefix & "_PM_TOPICS WHERE TOPIC_ID = " & PMTopicID)
					end if
				end if

				NoItemsDiv.InnerHtml = "Private Message Deleted Successfully<br /><br /><a href=""pm_inbox.aspx"">Click Here</a> To Return To Your Inbox<br /><br />"
			else
				Response.Redirect("pm_inbox.aspx")
			end if
		End Sub
	End Class


	'---------------------------------------------------------------------------------------------------
	' PMTopic - Codebehind For pm_topic.aspx
	'---------------------------------------------------------------------------------------------------
	Public Class PMTopic
		Inherits System.Web.UI.Page

		Public PagingPanel As System.Web.UI.WebControls.PlaceHolder
		Public PageCountLabel As System.Web.UI.WebControls.Label
		Public JumpPage As System.Web.UI.WebControls.DropDownList
		Public FirstLink As System.Web.UI.WebControls.LinkButton
		Public PreviousLink As System.Web.UI.WebControls.LinkButton
		Public NextLink As System.Web.UI.WebControls.LinkButton
		Public LastLink As System.Web.UI.WebControls.LinkButton
		Public QuickReply As System.Web.UI.WebControls.PlaceHolder
		Public txtReplyMessage As System.Web.UI.WebControls.Textbox
		Public PagePanel As System.Web.UI.WebControls.PlaceHolder
		Public TopicSubject As System.Web.UI.WebControls.Label
		Public TopicFromTo As System.Web.UI.WebControls.Label
		Public Topic As System.Web.UI.WebControls.Repeater
		Public Replies As System.Web.UI.WebControls.Repeater
		Public NoItemsDiv As System.Web.UI.HtmlControls.HtmlGenericControl

		Dim TopicReplies as Integer = 0

		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
			if Not Page.IsPostBack() then
				if Session("UserLogged") = "1" then
					If (Not Functions.IsInteger(Request.QueryString("ID"))) then
						Response.Redirect("default.aspx")
					else
						Dim AllowRead as Boolean = false
						Dim Reader as OdbcDataReader = Database.Read("SELECT T.TOPIC_ID, T.TOPIC_SUBJECT, T.TOPIC_TO, T.TOPIC_FROM, T.TOPIC_REPLIES, TF.MEMBER_USERNAME as TOPIC_FROM_NAME, TT.MEMBER_USERNAME as TOPIC_TO_NAME FROM " & Database.DBPrefix & "_PM_TOPICS T, " & Database.DBPrefix & "_MEMBERS TF, " & Database.DBPrefix & "_MEMBERS as TT WHERE (T.TOPIC_FROM =  TF.MEMBER_ID) and (T.TOPIC_TO = TT.MEMBER_ID) and (T.TOPIC_ID = " & Request.QueryString("ID") & ") and ((T.TOPIC_TO = " & Session("UserID") & " and T.TOPIC_SHOWRECEIVER = 1) or (T.TOPIC_FROM = " & Session("UserID") & " and T.TOPIC_SHOWSENDER = 1))")
						if (Reader.HasRows()) then
							AllowRead = true
						else
							AllowRead = false
						end if
						While Reader.Read()
							TopicReplies = Reader("TOPIC_REPLIES")
							TopicSubject.text = Reader("TOPIC_SUBJECT").ToString()
							if (Reader("TOPIC_TO").ToString() = Session("UserID")) then
								TopicFromTo.Text = "Received From " & Reader("TOPIC_FROM_NAME").ToString()
								Database.Write("UPDATE " & Database.DBPrefix & "_PM_TOPICS SET TOPIC_TO_READ = 1 WHERE TOPIC_ID = " & Reader("TOPIC_ID"))
							end if
							if (Reader("TOPIC_FROM").ToString() = Session("UserID")) then
								TopicFromTo.Text = "Sent To " & Reader("TOPIC_TO_NAME").ToString()
								Database.Write("UPDATE " & Database.DBPrefix & "_PM_TOPICS SET TOPIC_FROM_READ = 1 WHERE TOPIC_ID = " & Reader("TOPIC_ID"))
							end if
						End While
						Reader.Close()

						if (AllowRead) then
							if (Functions.IsInteger(Request.QueryString("PAGE"))) then
								ListPosts(Request.Querystring("PAGE"), Settings.ItemsPerPage)
							else
								ListPosts(1, Settings.ItemsPerPage)
							end if
						else
							PagePanel.visible = "false"
							NoItemsDiv.InnerHTML = "Access Denied<br /><br />"
						end if
					end if
				else
					PagePanel.visible = "false"
					NoItemsDiv.InnerHTML = "You Must Be Logged In To Use Private Messaging<br /><br />"
				end if
			end if
		End Sub

		Sub ListPosts(Optional CurrentPage As Integer = 1, Optional ItemsPerPage As Integer = 15)
			Dim NumPages, NumItems, NumWholePages, Leftover as Integer
			Dim IDList as New ArrayList

			Topic.DataSource = Database.Read("SELECT T.TOPIC_ID, T.TOPIC_SUBJECT, T.TOPIC_MESSAGE, T.TOPIC_FROM, T.TOPIC_TO, T.TOPIC_DATE, M.MEMBER_USERNAME, M.MEMBER_LOCATION, M.MEMBER_POSTS, M.MEMBER_DATE_JOINED, M.MEMBER_AVATAR_USECUSTOM, M.MEMBER_TITLE, A.AVATAR_IMAGE, M.MEMBER_TITLE_ALLOWCUSTOM, M.MEMBER_AVATAR_ALLOWCUSTOM, M.MEMBER_TITLE_USECUSTOM, M.MEMBER_AVATAR_CUSTOMTYPE, M.MEMBER_AVATAR_CUSTOMLOADED, M.MEMBER_AVATAR_SHOW, M.MEMBER_PHOTO, M.MEMBER_LEVEL, M.MEMBER_RANKING, M.MEMBER_ID FROM (" & Database.DBPrefix & "_PM_TOPICS T INNER JOIN " & Database.DBPrefix & "_MEMBERS M ON T.TOPIC_FROM = M.MEMBER_ID) LEFT OUTER JOIN " & Database.DBPrefix & "_AVATARS A ON M.MEMBER_AVATAR = A.AVATAR_ID WHERE T.TOPIC_ID = " & Request.QueryString("ID"))

			Topic.DataBind()

			if (Topic.Items.Count = 0) then
				NoItemsDiv.InnerHtml = "There Are No Items To Display<br /><br />"
				Topic.Visible = "false"
				Replies.Visible = "false"
			else

				if TopicReplies = 0 then
					Replies.visible = "false"
					PagingPanel.visible = "false"
				else
					Dim ReplyReader as OdbcDataReader = Database.Read("SELECT REPLY_ID FROM " & Database.DBPrefix & "_PM_REPLIES R WHERE R.TOPIC_ID = " & Request.QueryString("ID") & " ORDER BY R.REPLY_DATE")
						While(ReplyReader.Read())
							IDList.Add(ReplyReader("REPLY_ID"))
						End While
					ReplyReader.close()

					NumItems = IDList.Count
					NumPages = NumItems \ ItemsPerPage
					NumWholePages = NumItems \ ItemsPerPage
					Leftover = NumItems Mod ItemsPerPage

					If Leftover > 0 then
						NumPages += 1
					end if

					if (CurrentPage < 0) or (CurrentPage > NumPages) then
						ListPosts(1, ItemsPerPage)
					else
						if CurrentPage = NumPages then
							NextLink.Visible = false
							LastLink.Visible = false
						else
							NextLink.Visible = true
							LastLink.Visible = true
							NextLink.CommandArgument = CurrentPage + 1
							LastLink.CommandArgument = NumPages
						end if

						if CurrentPage = 1 then
							PreviousLink.Visible = false
							FirstLink.Visible = false
						else
							PreviousLink.Visible = true
							FirstLink.Visible = true
							PreviousLink.CommandArgument = CurrentPage - 1
							FirstLink.CommandArgument = 1
							Topic.visible = "false"
						end if
		
						if NumPages = 1 then
							PagingPanel.visible = "false"
						end if
	
						Dim JumpPageList As ArrayList = new ArrayList
						Dim x As Integer
						For x = 1 to NumPages
							JumpPageList.Add(x)
						Next
	
						JumpPage.DataSource = JumpPageList
						JumpPage.Databind()
						JumpPage.SelectedIndex = CurrentPage - 1

						PageCountLabel.Text = NumPages

						Dim StartOfPage as Integer = ItemsPerPage * (CurrentPage - 1)
						Dim EndOfPage as Integer = Min((ItemsPerPage * (CurrentPage - 1)) + (ItemsPerPage - 1), ((NumWholePages * ItemsPerPage) + Leftover - 1))

						Dim CurrentSubset As String = Join( IDList.GetRange( StartOfPage , (EndOfPage - StartOfPage + 1) ).ToArray , "," )

						Replies.DataSource = Database.Read("SELECT R.REPLY_ID, R.REPLY_MESSAGE, R.REPLY_AUTHOR, R.REPLY_DATE, M.MEMBER_USERNAME, M.MEMBER_LOCATION, M.MEMBER_POSTS, M.MEMBER_DATE_JOINED, M.MEMBER_AVATAR_USECUSTOM, M.MEMBER_TITLE, A.AVATAR_IMAGE, M.MEMBER_TITLE_ALLOWCUSTOM, M.MEMBER_AVATAR_ALLOWCUSTOM, M.MEMBER_TITLE_USECUSTOM, M.MEMBER_AVATAR_CUSTOMTYPE, M.MEMBER_AVATAR_CUSTOMLOADED, M.MEMBER_AVATAR_SHOW, M.MEMBER_PHOTO, M.MEMBER_LEVEL, M.MEMBER_RANKING, M.MEMBER_ID FROM (" & Database.DBPrefix & "_PM_REPLIES R INNER JOIN " & Database.DBPrefix & "_MEMBERS M ON R.REPLY_AUTHOR = M.MEMBER_ID) LEFT OUTER JOIN " & Database.DBPrefix & "_AVATARS A ON M.MEMBER_AVATAR = A.AVATAR_ID WHERE R.REPLY_ID IN (" & CurrentSubSet & ") ORDER BY R.REPLY_DATE")
						Replies.DataBind()
						if (Replies.Items.Count = 0) then
							Replies.Visible = "false"
							PagingPanel.visible = "false"
						end if
						Replies.Datasource.Close()
					end if
				end if
			end if

			Topic.DataSource.Close()
		End Sub

		Sub ChangePage(sender As System.Object, e As System.EventArgs)
			If sender.ToString() = "System.Web.UI.WebControls.LinkButton" Then
				Response.Redirect("pm_topic.aspx?ID=" & Request.Querystring("ID") & "&PAGE=" & sender.CommandArgument)
			else
				Response.Redirect("pm_topic.aspx?ID=" & Request.Querystring("ID") & "&PAGE=" & JumpPage.SelectedValue)
			end if	
		End Sub

		Sub SubmitReply(sender As System.Object, e As System.EventArgs)
			Dim Failure as Integer = 0

			if (txtReplyMessage.text = "") or (txtReplyMessage.text = " ") then
				Failure = 1
				Functions.Messagebox("No Message Entered!")
			end if

			if Failure = 0 then
				PagePanel.Visible = "false"

				Database.Write("INSERT INTO " & Database.DBPrefix & "_PM_REPLIES (TOPIC_ID, REPLY_MESSAGE, REPLY_DATE, REPLY_AUTHOR) VALUES (" & Request.Querystring("ID") & ", '" & Functions.RepairString(txtReplyMessage.text) & "', " & Database.GetTimeStamp() & ", " & Session("UserID") & ")")
				Database.Write("UPDATE " & Database.DBPrefix & "_PM_TOPICS SET TOPIC_REPLIES = (TOPIC_REPLIES + 1), TOPIC_LASTPOST_DATE = " & Database.GetTimeStamp() & ", TOPIC_LASTPOST_AUTHOR = " & Session("UserID") & ", TOPIC_SHOWSENDER = 1, TOPIC_SHOWRECEIVER = 1 WHERE TOPIC_ID = " & Request.Querystring("ID"))

				Dim Reader as OdbcDataReader = Database.Read("SELECT TOPIC_ID, TOPIC_TO, TOPIC_FROM FROM " & Database.DBPrefix & "_PM_TOPICS WHERE (TOPIC_ID = " & Request.QueryString("ID") & ") and ((TOPIC_TO = " & Session("UserID") & " and TOPIC_SHOWRECEIVER = 1) or (TOPIC_FROM = " & Session("UserID") & " and TOPIC_SHOWSENDER = 1))")
				While Reader.Read()
					if (Reader("TOPIC_TO").ToString() <> Session("UserID")) then
						Database.Write("UPDATE " & Database.DBPrefix & "_PM_TOPICS SET TOPIC_TO_READ = 0 WHERE TOPIC_ID = " & Reader("TOPIC_ID"))
					end if
					if (Reader("TOPIC_FROM").ToString() <> Session("UserID")) then
						Database.Write("UPDATE " & Database.DBPrefix & "_PM_TOPICS SET TOPIC_FROM_READ = 0 WHERE TOPIC_ID = " & Reader("TOPIC_ID"))
					end if
				End While
				Reader.Close()

				Dim TopicReplies as Integer = 0
				Reader = Database.Read("SELECT TOPIC_REPLIES FROM " & Database.DBPrefix & "_PM_TOPICS WHERE TOPIC_ID = " & Request.Querystring("ID"))
				While Reader.Read()
					TopicReplies = Reader("TOPIC_REPLIES")
				End While
				Reader.Close()

					Dim PageItems as Integer = Settings.ItemsPerPage
					Dim NumPages as Integer = TopicReplies \ PageItems
					Dim Leftover as Integer = TopicReplies Mod PageItems
					If Leftover > 0 then
						NumPages += 1
					end if

				NoItemsDiv.InnerHtml = "Reply Posted Successfully<br /><br /><a href=""pm_topic.aspx?ID=" & Request.Querystring("ID") & "&PAGE=" & NumPages & """>Click Here</a> To Return To The Thread<br /><br />"
			end if
		End Sub
	End Class


	'---------------------------------------------------------------------------------------------------
	' PMSend - Codebehind For pm_send.aspx
	'---------------------------------------------------------------------------------------------------
	Public Class PMSend
		Inherits System.Web.UI.Page

		Public txtTo As System.Web.UI.WebControls.TextBox
		Public txtSubject As System.Web.UI.WebControls.TextBox
		Public txtMessage As System.Web.UI.WebControls.TextBox
		Public txtSaveCopy As System.Web.UI.WebControls.CheckBox
		Public PagePanel As System.Web.UI.WebControls.PlaceHolder
		Public NoItemsDiv As System.Web.UI.HtmlControls.HtmlGenericControl

		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
			if Not Page.IsPostBack() then
				if Session("UserLogged") = "1" then
					if Functions.IsInteger(Request.QueryString("SendTo")) then
						Dim SendReader as OdbcDataReader = Database.Read("SELECT MEMBER_USERNAME FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & Request.QueryString("SendTo"))
						While SendReader.Read()
							txtTo.text = SendReader("MEMBER_USERNAME").ToString()
						End While
						SendReader.Close()
					end if
				else
					PagePanel.visible = "false"
					NoItemsDiv.InnerHTML = "You Must Be Logged In To Use Private Messaging<br /><br />"
				end if
			end if
		End Sub

		Sub SubmitTopic(sender As Object, e As EventArgs)
			Dim Failure as Integer = 0

			Dim PMShowSender as Integer = 1
			if (txtSaveCopy.Checked = "false") then
				PMShowSender = 0
			end if

			if (txtSubject.text = "") or (txtSubject.text = " ") then
				Failure = 1
				Functions.Messagebox("No Subject Entered!")
			end if
			if (txtMessage.text = "") or (txtMessage.text = " ") then
				Failure = 1
				Functions.Messagebox("No Message Entered!")
			end if
			if (txtTo.text = "") or (txtTo.text = " ") then
				Failure = 1
				Functions.Messagebox("You Must Pick Someone To Send To!")
			end if

			Dim MemberID as Integer = 0
			Dim MemberString as String = Functions.RepairString(txtTo.text.ToString())
			Dim MemberReader as OdbcDataReader = Database.Read("SELECT MEMBER_ID FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_USERNAME = '" & MemberString & "' and MEMBER_LEVEL <> 0")
			if MemberReader.HasRows() then
				While MemberReader.Read()
					MemberID = MemberReader("MEMBER_ID")
				End While
			else
				Failure = 1
				Functions.Messagebox("Member Not Found!")
			end if
			MemberReader.Close()

			if Failure = 0 then
				PagePanel.visible = "false"
				Database.Write("INSERT INTO " & Database.DBPrefix & "_PM_TOPICS (TOPIC_FROM, TOPIC_TO, TOPIC_SUBJECT, TOPIC_MESSAGE, TOPIC_DATE, TOPIC_TO_READ, TOPIC_FROM_READ, TOPIC_LASTPOST_AUTHOR, TOPIC_LASTPOST_DATE, TOPIC_REPLIES, TOPIC_SHOWSENDER, TOPIC_SHOWRECEIVER) VALUES (" & Session("UserID") & ", " & MemberID & ", '" & Functions.RepairString(txtSubject.text) & "', '" & Functions.RepairString(txtMessage.text) & "', " & Database.GetTimeStamp() & ", 0, 1, " & Session("UserID") & ", " & Database.GetTimeStamp() & ", 0, " & PMShowSender & ", 1)")
				NoItemsDiv.InnerHtml = "Private Message Posted Successfully<br /><br /><a href=""pm_inbox.aspx"">Click Here</a> To Return To Your Inbox<br /><br />"
			end if
		End Sub
	End Class


	'---------------------------------------------------------------------------------------------------
	' PMSelectUser - Codebehind For pm_selectuser.aspx
	'---------------------------------------------------------------------------------------------------
	Public Class PMSelectUser
		Inherits System.Web.UI.Page

		Public MemberSearch As System.Web.UI.WebControls.Panel
		Public MemberSearchString As System.Web.UI.WebControls.TextBox
		Public MemberList As System.Web.UI.WebControls.Repeater
		Public PreMessage As System.Web.UI.WebControls.PlaceHolder
		Public NoItemsDiv As System.Web.UI.HtmlControls.HtmlGenericControl

		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
			if Not Page.IsPostBack() then
				MemberSearch.visible = "true"
			end if
		End Sub

		Sub SubmitMemberSearch(sender As System.Object, e As System.EventArgs)
			PreMessage.visible = "false"
			MemberSearch.visible = "true"
			Dim SearchString as String = Functions.RepairString(MemberSearchString.Text)
			MemberList.Datasource = Database.Read("SELECT MEMBER_ID, MEMBER_USERNAME FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_USERNAME LIKE '" & SearchString & "%' AND MEMBER_LEVEL > 0 ORDER BY MEMBER_USERNAME")
			MemberList.Databind()
			MemberList.Datasource.Close()
		End Sub
	End Class


	'---------------------------------------------------------------------------------------------------
	' SelectForum - Codebehind For newtopic_selectforum.aspx
	'---------------------------------------------------------------------------------------------------
	Public Class SelectForum
		Inherits System.Web.UI.Page

		Public ForumList As System.Web.UI.WebControls.DropDownList
		Public PagePanel As System.Web.UI.WebControls.Panel
		Public NoItemsDiv As System.Web.UI.HtmlControls.HtmlGenericControl

		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
			if Not Page.IsPostBack() then
				SetSession()

				if (Session("UserLogged") = "0") then
					PagePanel.visible = "false"
					NoItemsDiv.InnerHTML = "You Must Be A Registered Member To Post<br /><br />"
				else
					Dim LItem as ListItem
					Dim ForumReader as OdbcDataReader = Database.Read("SELECT F.FORUM_ID, F.FORUM_NAME, F.FORUM_STATUS FROM " & Database.DBPrefix & "_FORUMS F Left Outer Join " & Database.DBPrefix & "_CATEGORIES C On F.CATEGORY_ID = C.CATEGORY_ID WHERE (F.FORUM_TYPE = 0 OR F.FORUM_TYPE = 2 OR F.FORUM_TYPE = 4) and (C.CATEGORY_STATUS <> 0 and F.FORUM_STATUS = 1) ORDER BY C.CATEGORY_ID, F.FORUM_SORTBY")
						if ForumReader.HasRows() then
							LItem = New ListItem("", 0)
							ForumList.Items.Add(LItem)
							While ForumReader.Read()
								if ((ForumReader("FORUM_STATUS") = 1) or (Functions.IsModerator(Session("UserID"), Session("UserLevel"), ForumReader("FORUM_ID")))) then
									LItem = New ListItem(Server.HTMLDecode(ForumReader("FORUM_NAME").ToString()), ForumReader("FORUM_ID"))
									ForumList.Items.Add(LItem)
								end if
							End While
						else
							PagePanel.visible = "false"
							NoItemsDiv.InnerHTML = "There Are No Public Forums To Post To<br /><br />"
						end if
					ForumReader.Close()
				end if
			end if
		End Sub

		Sub SelectForum(sender As System.Object, e As System.EventArgs)
			Response.Redirect("newtopic.aspx?ID=" & sender.SelectedValue)
		End Sub

		Sub SetSession()
			if Not Request.Cookies("dmgforums") Is Nothing then
				Dim aCookie As New System.Web.HttpCookie("dmgforums")

				Dim UserReader as OdbcDataReader = Database.Read("SELECT MEMBER_ID, MEMBER_USERNAME, MEMBER_PASSWORD, MEMBER_LEVEL FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & Server.HtmlEncode(Request.Cookies("dmgforums")("mukul")) & " AND MEMBER_PASSWORD = '" & Server.HtmlEncode(Request.Cookies("dmgforums")("gupta")) & "'", 1)
					While(UserReader.Read())
						Session("UserID") = UserReader("MEMBER_ID").ToString()
						Session("UserName") = UserReader("MEMBER_USERNAME").ToString()
						Session("UserLevel") = UserReader("MEMBER_LEVEL").ToString()
						Session("UserLogged") = "1"
						aCookie.Values("fighter") = Functions.Encrypt(DateTime.Now())
						aCookie.Values("mukul") = UserReader("MEMBER_ID").ToString()
						aCookie.Values("dooder") = Functions.Encrypt(DateTime.Now())
						aCookie.Values("gupta") = UserReader("MEMBER_PASSWORD").ToString()
						aCookie.Expires = DateTime.Now.AddDays(30)
						Response.Cookies.Add(aCookie)
					End While
				UserReader.Close()

				if ((Session("UserLevel") = 0) or (Session("UserLevel") = -1)) then
					aCookie.Values("fighter") = Functions.Encrypt(DateTime.Now())
					aCookie.Values("mukul") = "-3"
					aCookie.Values("dooder") = Functions.Encrypt(DateTime.Now())
					aCookie.Values("gupta") = Functions.Encrypt(DateTime.Now())
					aCookie.Expires = DateTime.Now.AddDays(-1)
					Response.Cookies.Add(aCookie)
					Session("UserID") = "-1"
					Session("UserName") = ""
					Session("UserLogged") = "0"
					Session("UserLevel") = "0"
				end if
			else
				if (Session("UserLogged") = "1") then
					Dim UserReader as OdbcDataReader = Database.Read("SELECT MEMBER_ID, MEMBER_USERNAME, MEMBER_PASSWORD, MEMBER_LEVEL FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & Session("UserID"), 1)
						While(UserReader.Read())
							Session("UserID") = UserReader("MEMBER_ID").ToString()
							Session("UserName") = UserReader("MEMBER_USERNAME").ToString()
							Session("UserLevel") = UserReader("MEMBER_LEVEL").ToString()
							Session("UserLogged") = "1"
						End While
					UserReader.Close()

					if ((Session("UserLevel") = 0) or (Session("UserLevel") = -1)) then
						Dim aCookie As New System.Web.HttpCookie("dmgforums")
						aCookie.Values("fighter") = Functions.Encrypt(DateTime.Now())
						aCookie.Values("mukul") = "-3"
						aCookie.Values("dooder") = Functions.Encrypt(DateTime.Now() & "bbb")
						aCookie.Values("gupta") = Functions.Encrypt(DateTime.Now() & "ccc")
						aCookie.Expires = DateTime.Now.AddDays(-1)
						Response.Cookies.Add(aCookie)
						Session("UserID") = "-1"
						Session("UserName") = ""
						Session("UserLogged") = "0"
						Session("UserLevel") = "0"
					end if
				else
					Session("UserID") = "-1"
					Session("UserName") = ""
					Session("UserLogged") = "0"
					Session("UserLevel") = "0"
				end if
			end if
		End Sub
	End Class


	'---------------------------------------------------------------------------------------------------
	' Subscribe - Codebehind For subscribe.aspx
	'---------------------------------------------------------------------------------------------------
	Public Class Subscribe
		Inherits System.Web.UI.Page

		Public txtSendEmail As System.Web.UI.WebControls.DropDownList
		Public SubForm As System.Web.UI.WebControls.PlaceHolder
		Public UnSubForm As System.Web.UI.WebControls.PlaceHolder
		Public PagePanel As System.Web.UI.WebControls.PlaceHolder
		Public NoItemsDiv As System.Web.UI.HtmlControls.HtmlGenericControl

		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
			if ((Functions.IsInteger(Request.QueryString("ID"))) and (Session("UserLogged") = "1")) then
				if ((Settings.AllowSub <> 1) and (Session("UserLevel") <> "3")) then
					PagePanel.visible = "false"
					NoItemsDiv.InnerHtml = "Thread Subscriptions Are Disabled On This Site.<br /><br />"
				else
					Dim Reader as OdbcDataReader = Database.Read("SELECT SUB_ID FROM " & Database.DBPrefix & "_SUBSCRIPTIONS WHERE SUB_TOPIC = " & Request.QueryString("ID") & " AND SUB_MEMBER = " & Session("UserID"))
					if Reader.HasRows() then
						SubForm.visible = "false"
						UnSubForm.visible = "true"
					else
						if ((Settings.EmailAllowSub = 0) and (Session("UserLevel") <> "3")) then
							txtSendEmail.SelectedValue = 0
							txtSendEmail.enabled = "false"
						end if
					end if
					Reader.Close()
				end if
			else
				Response.Redirect("default.aspx")
			end if
		End Sub

		Sub SubmitSubscription(sender As System.Object, e As System.EventArgs)
			Dim SubMember as Integer = cLng(Session("UserID"))
			Dim SubTopic as Integer = cLng(Request.QueryString("ID"))
			Dim SubEmail as Integer = cLng(txtSendEmail.SelectedValue)
			Database.Write("INSERT INTO " & Database.DBPrefix & "_SUBSCRIPTIONS (SUB_MEMBER, SUB_TOPIC, SUB_EMAIL) VALUES (" & SubMember & ", " & SubTopic & ", " & SubEmail & ")")

			PagePanel.visible = "false"
			NoItemsDiv.InnerHtml = "You Have Subscribed To This Thread<br /><br /><a href=""topics.aspx?ID=" & SubTopic & """>Click Here</a> To Return To The Topic Page<br /><br />"
		End Sub

		Sub SubmitUnSubscription(sender As System.Object, e As System.EventArgs)
			Dim SubMember as Integer = cLng(Session("UserID"))
			Dim SubTopic as Integer = cLng(Request.QueryString("ID"))
			Database.Write("DELETE FROM " & Database.DBPrefix & "_SUBSCRIPTIONS WHERE SUB_TOPIC = " & SubTopic & " AND SUB_MEMBER = " & SubMember)

			PagePanel.visible = "false"
			NoItemsDiv.InnerHtml = "You Have Unsubscribed From This Thread<br /><br /><a href=""topics.aspx?ID=" & SubTopic & """>Click Here</a> To Return To The Topic Page<br /><br />"
		End Sub

		Sub CancelUnSubscription(sender As System.Object, e As System.EventArgs)
			Response.Redirect("topics.aspx?ID=" & Request.QueryString("ID"))
		End Sub
	End Class

End Namespace
