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
Imports System.Text
Imports System.Xml
Imports System.Web.UI.WebControls
Imports Microsoft.VisualBasic
Imports DMGForums.Global

Namespace DMGForums.Forums

	'---------------------------------------------------------------------------------------------------
	' NewCategory - Codebehind For newcategory.aspx
	'---------------------------------------------------------------------------------------------------
	Public Class NewCategory
		Inherits System.Web.UI.Page

		Public txtName As System.Web.UI.WebControls.Textbox
		Public txtContent As System.Web.UI.WebControls.Textbox
		Public txtShowHeaders As System.Web.UI.WebControls.DropDownList
		Public txtShowLogin As System.Web.UI.WebControls.DropDownList
		Public txtSortBy As System.Web.UI.WebControls.Textbox
		Public txtStatus As System.Web.UI.WebControls.DropDownList
		Public PagePanel As System.Web.UI.WebControls.Panel
		Public NoItemsDiv As System.Web.UI.HtmlControls.HtmlGenericControl

		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
			if Not Page.IsPostBack() then
				if (Session("UserLevel") <> "3") then
					PagePanel.visible = "false"
					NoItemsDiv.InnerHtml = "Access Denied<br /><br />"
				end if

				if (Settings.HideLogin = 1) then
					txtShowLogin.SelectedValue = 0
				else
					txtShowLogin.SelectedValue = 1
				end if
			end if
		End Sub

		Sub SubmitCategory(sender As System.Object, e As System.EventArgs)
			Dim Failure as Integer = 0

			if (txtName.text = "") or (txtName.text = " ") then
				Failure = 1
				Functions.Messagebox("No Name Entered!")
			end if

			if Failure = 0 then
				if (txtSortBy.text = "") or (txtSortBy.text = " ") then
					txtSortBy.text = "1"
				end if

				PagePanel.visible = "false"

				Dim CategoryName as String = Functions.RepairString(txtName.text, 0)
				Dim CategoryContent as String = Functions.RepairString(txtContent.text, 0)
				Dim CategoryStatus as String = txtStatus.SelectedValue
				Dim CategorySortBy as String = CLng(txtSortBy.text)
				Dim CategoryShowHeaders as String = txtShowHeaders.SelectedValue
				Dim CategoryShowLogin as String = txtShowLogin.SelectedValue

				Database.Write("INSERT INTO " & Database.DBPrefix & "_CATEGORIES (CATEGORY_NAME, CATEGORY_CONTENT, CATEGORY_STATUS, CATEGORY_SORTBY, CATEGORY_SHOWHEADERS, CATEGORY_SHOWLOGIN) VALUES ('" & CategoryName & "', '" & CategoryContent & "', " & CategoryStatus & ", " & CategorySortBy & ", " & CategoryShowHeaders & ", " & CategoryShowLogin & ")")

				NoItemsDiv.InnerHtml = "Category Created Successfully<br /><br /><a href=""ForumHome.aspx"">Click Here</a> To Return To The Forums<br /><br />"
			end if
		End Sub
	End Class


	'---------------------------------------------------------------------------------------------------
	' EditCategory - Codebehind For editcategory.aspx
	'---------------------------------------------------------------------------------------------------
	Public Class EditCategory
		Inherits System.Web.UI.Page

		Public txtID As System.Web.UI.WebControls.Textbox
		Public txtName As System.Web.UI.WebControls.Textbox
		Public txtContent As System.Web.UI.WebControls.Textbox
		Public txtShowHeaders As System.Web.UI.WebControls.DropDownList
		Public txtShowLogin As System.Web.UI.WebControls.DropDownList
		Public txtSortBy As System.Web.UI.WebControls.Textbox
		Public txtStatus As System.Web.UI.WebControls.DropDownList
		Public PagePanel As System.Web.UI.WebControls.Panel
		Public NoItemsDiv As System.Web.UI.HtmlControls.HtmlGenericControl

		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
			if Not Page.IsPostBack() then
				if (Session("UserLevel") <> "3") or (Not Functions.IsInteger(Request.QueryString("ID"))) then
					PagePanel.visible = "false"
					NoItemsDiv.InnerHtml = "Access Denied<br /><br />"
				else
					Dim CatReader as OdbcDataReader = Database.Read("SELECT * FROM " & Database.DBPrefix & "_CATEGORIES WHERE CATEGORY_ID = " & Request.QueryString("ID"))
						While(CatReader.Read())
							txtID.text = CatReader("CATEGORY_ID")
							txtName.text = Server.HTMLDecode(CatReader("CATEGORY_NAME").ToString())
							txtContent.text = CatReader("CATEGORY_CONTENT").ToString()
							txtSortBy.text = CatReader("CATEGORY_SORTBY")
							txtStatus.Items.FindByValue(CatReader("CATEGORY_STATUS").ToString()).Selected = "True"
							txtShowHeaders.Items.FindByValue(CatReader("CATEGORY_SHOWHEADERS").ToString()).Selected = "True"
							txtShowLogin.Items.FindByValue(CatReader("CATEGORY_SHOWLOGIN").ToString()).Selected = "True"
						End While
					CatReader.close()
				end if
			end if
		End Sub

		Sub EditCategory(sender As System.Object, e As System.EventArgs)
			Dim Failure as Integer = 0

			if (txtName.text = "") or (txtName.text = " ") then
				Failure = 1
				Functions.Messagebox("No Name Entered!")
			end if

			if Failure = 0 then
				PagePanel.visible = "false"
	
				Dim CategoryName as String = Functions.RepairString(txtName.text, 0)
				Dim CategoryContent as String = Functions.RepairString(txtContent.text, 0)
				Dim CategoryStatus as String = txtStatus.SelectedValue
				Dim CategorySortBy as String = CLng(txtSortBy.Text)
				Dim CategoryShowHeaders as String = txtShowHeaders.SelectedValue
				Dim CategoryShowLogin as String = txtShowLogin.SelectedValue
	
				Database.Write("UPDATE " & Database.DBPrefix & "_CATEGORIES SET CATEGORY_NAME = '" & CategoryName & "', CATEGORY_CONTENT = '" & CategoryContent & "', CATEGORY_STATUS = " & CategoryStatus & ", CATEGORY_SORTBY = " & CategorySortBy & ", CATEGORY_SHOWHEADERS = " & CategoryShowHeaders & ", CATEGORY_SHOWLOGIN = " & CategoryShowLogin & " WHERE CATEGORY_ID = " & txtID.text)

				NoItemsDiv.InnerHtml = "Category Updated Successfully<br /><br /><a href=""ForumHome.aspx"">Click Here</a> To Return To The Forums<br /><br />"
			end if
		End Sub
	End Class


	'---------------------------------------------------------------------------------------------------
	' DeleteCategory - Codebehind For deletecategory.aspx
	'---------------------------------------------------------------------------------------------------
	Public Class DeleteCategory
		Inherits System.Web.UI.Page

		Public DeleteButton As System.Web.UI.WebControls.Button
		Public CategoryName As System.Web.UI.WebControls.Label	
		Public PagePanel As System.Web.UI.WebControls.Panel
		Public NoItemsDiv As System.Web.UI.HtmlControls.HtmlGenericControl

		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
			if Not Page.IsPostBack() then
				if (Session("UserLevel") <> "3") or (Not Functions.IsInteger(Request.QueryString("ID"))) then
					PagePanel.visible = "false"
					NoItemsDiv.InnerHtml = "Access Denied<br /><br />"
				else
					Dim CatReader as OdbcDataReader = Database.Read("SELECT CATEGORY_ID, CATEGORY_NAME FROM " & Database.DBPrefix & "_CATEGORIES WHERE CATEGORY_ID = " & Request.QueryString("ID"))
						if CatReader.HasRows then
							While(CatReader.Read())
								DeleteButton.CommandArgument = CatReader("CATEGORY_ID")
								CategoryName.text = CatReader("CATEGORY_NAME")
							End While
						else
							PagePanel.visible = "false"
							NoItemsDiv.InnerHtml = "Invalid Category ID<br /><br />"
						end if
					CatReader.close()
				end if
			end if
		End Sub

		Sub DeleteCategory(sender As System.Object, e As System.EventArgs)
			PagePanel.visible = "false"

			Database.Write("DELETE FROM " & Database.DBPrefix & "_CATEGORIES WHERE CATEGORY_ID = " & sender.CommandArgument)
			Database.Write("DELETE FROM " & Database.DBPrefix & "_FORUMS WHERE CATEGORY_ID = " & sender.CommandArgument)

			Dim TopicReader as OdbcDataReader = Database.Read("SELECT TOPIC_ID FROM " & Database.DBPrefix & "_TOPICS WHERE CATEGORY_ID = " & sender.CommandArgument)
				While(TopicReader.Read())
					Database.Write("DELETE FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_ID = " & TopicReader("TOPIC_ID"))
					Database.Write("DELETE FROM " & Database.DBPrefix & "_REPLIES WHERE TOPIC_ID = " & TopicReader("TOPIC_ID"))
				End While
			TopicReader.Close()

			NoItemsDiv.InnerHtml = "Category Deleted Successfully<br /><br /><a href=""ForumHome.aspx"">Click Here</a> To Return To The Forums<br /><br />"
		End Sub
	End Class


	'---------------------------------------------------------------------------------------------------
	' ForumsPage - Codebehind For forums.aspx
	'---------------------------------------------------------------------------------------------------
	Public Class ForumsPage
		Inherits System.Web.UI.Page

		Public Topics As System.Web.UI.WebControls.Repeater
		Public ModeratorsList As System.Web.UI.WebControls.Repeater
		Public PagePanel As System.Web.UI.WebControls.Panel
		Public PagingPanel As System.Web.UI.WebControls.Panel
		Public JumpPage As System.Web.UI.WebControls.DropDownList
		Public PasswordPanel As System.Web.UI.WebControls.Panel
		Public FirstLink As System.Web.UI.WebControls.LinkButton
		Public PreviousLink As System.Web.UI.WebControls.LinkButton
		Public NextLink As System.Web.UI.WebControls.LinkButton
		Public LastLink As System.Web.UI.WebControls.LinkButton
		Public PasswordBox As System.Web.UI.WebControls.Textbox
		Public PageCountLabel As System.Web.UI.WebControls.Label
		Public ConfirmTopicForm As System.Web.UI.WebControls.Panel
		Public ConfirmTopicDropdown As System.Web.UI.WebControls.DropDownList
		Public ConfirmButton As System.Web.UI.WebControls.Button
		Public ForumContentPanel As System.Web.UI.WebControls.PlaceHolder
		Public ForumContent As System.Web.UI.WebControls.Literal
		Public NoItemsDiv As System.Web.UI.HtmlControls.HtmlGenericControl

		Public ForumID as String
		Public ForumName as String
		Public ForumStatus as Integer
		Public CategoryID as String
		Public CategoryName as String
		Public AllowModeration as Boolean

		Public DMGHeader As DMGForums.Global.Header
		Public DMGFooter As DMGForums.Global.Footer
		Public DMGLogin As DMGForums.Global.Login
		Public DMGSettings As DMGForums.Global.Settings

		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
			Dim ForumType, ForumTopics as Integer

			If (Functions.IsInteger(Request.QueryString("ID"))) then
				Dim ForumReader as OdbcDataReader = Database.Read("SELECT F.FORUM_ID, F.FORUM_NAME, F.FORUM_CONTENT, F.FORUM_TYPE, F.FORUM_TOPICS, F.FORUM_STATUS, F.FORUM_SHOWHEADERS, F.FORUM_SHOWLOGIN, C.CATEGORY_ID, C.CATEGORY_NAME FROM " & Database.DBPrefix & "_FORUMS F Left Outer Join " & Database.DBPrefix & "_CATEGORIES C on F.CATEGORY_ID = C.CATEGORY_ID WHERE FORUM_STATUS <> 0 AND FORUM_ID = " & Request.Querystring("ID"))
					if ForumReader.HasRows() then
						While(ForumReader.Read())
						      ForumID = ForumReader("FORUM_ID").ToString()
							ForumName = ForumReader("FORUM_NAME").ToString()
							ForumType = ForumReader("FORUM_TYPE")
							ForumStatus = ForumReader("FORUM_STATUS")
							ForumTopics = ForumReader("FORUM_TOPICS")
						      CategoryID = ForumReader("CATEGORY_ID").ToString()
							CategoryName = ForumReader("CATEGORY_NAME").ToString()
							DMGSettings.CustomTitle = ForumName
							if (ForumReader("FORUM_CONTENT").ToString().Length() > 0)
								ForumContent.Text = Functions.CustomHTMLVariables(ForumReader("FORUM_CONTENT").ToString(), Session("UserID"), Session("UserLogged"), Session("UserLevel"))
							else
								ForumContentPanel.visible = "False"
							end if
							if (ForumReader("FORUM_SHOWHEADERS") <> 1) then
								DMGHeader.visible = "false"
								DMGFooter.visible = "false"
							end if
							DMGLogin.ShowLogin() = ForumReader("FORUM_SHOWLOGIN")
						End While
					else
                    Response.Redirect("community/default.aspx")
                End If
                ForumReader.close()
            End If

            If Not Page.IsPostBack() Then
                SetSession()

                If (Not Functions.IsInteger(Request.QueryString("ID"))) Then
                    Response.Redirect("community/default.aspx")
                Else
                    AllowModeration = Functions.IsModerator(Session("UserID"), Session("UserLevel"), ForumID)

                    ModeratorsList.Datasource = Database.Read("SELECT M.MEMBER_USERNAME FROM " & Database.DBPrefix & "_PRIVILEGED P LEFT OUTER JOIN " & Database.DBPrefix & "_MEMBERS M ON P.MEMBER_ID = M.MEMBER_ID WHERE P.FORUM_ID = " & Request.Querystring("ID") & " AND P.PRIVILEGED_ACCESS = 2 ORDER BY M.MEMBER_USERNAME ")
                    ModeratorsList.Databind()
                    If ModeratorsList.Items.Count = 0 Then
                        ModeratorsList.Visible = "false"
                    End If
                    ModeratorsList.Datasource.Close()

                    If ((AllowModeration) And (ForumTopics = 0)) Then
                        Dim Reader As OdbcDataReader = Database.Read("SELECT Count(*) as TheCount FROM " & Database.DBPrefix & "_TOPICS WHERE FORUM_ID = " & Request.Querystring("ID") & " AND TOPIC_CONFIRMED = 0")
                        While Reader.Read()
                            ForumTopics = Reader("TheCount")
                        End While
                        Reader.Close()
                    End If

                    If (ForumType = 2) Then
                        If (Session("FORUM_" & Request.Querystring("ID")) = "logged") Then
                            If (ForumTopics > 0) Then
                                If (Request.Querystring("PAGE") <> "") Then
                                    ListTopics(Request.Querystring("PAGE"), Settings.ItemsPerPage)
                                Else
                                    ListTopics(1, Settings.ItemsPerPage)
                                End If
                            Else
                                PagingPanel.visible = "false"
                                NoItemsDiv.InnerHtml = "There Are No Topics Posted To This Forum<br /><br />"
                            End If
                        Else
                            PagePanel.visible = "false"
                            PasswordPanel.visible = "true"
                            ModeratorsList.visible = "false"
                        End If
                    ElseIf (ForumType = 1) Or (ForumType = 3) Or (ForumType = 4) Then
                        If Functions.IsPrivileged(ForumID, ForumType, Session("UserID"), Session("UserLevel"), Session("UserLogged")) Then
                            If (ForumTopics > 0) Then
                                If (Request.Querystring("PAGE") <> "") Then
                                    ListTopics(Request.Querystring("PAGE"), Settings.ItemsPerPage)
                                Else
                                    ListTopics(1, Settings.ItemsPerPage)
                                End If
                            Else
                                PagingPanel.visible = "false"
                                NoItemsDiv.InnerHtml = "There Are No Topics Posted To This Forum<br /><br />"
                            End If
                        Else
                            PagePanel.visible = "false"
                            NoItemsDiv.InnerHtml = "You Do Not Have Access To This Forum<br /><br />"
                        End If
                    Else
                        If (ForumTopics > 0) Then
                            If (Request.Querystring("PAGE") <> "") Then
                                ListTopics(Request.Querystring("PAGE"), Settings.ItemsPerPage)
                            Else
                                ListTopics(1, Settings.ItemsPerPage)
                            End If
                        Else
                            PagingPanel.visible = "false"
                            NoItemsDiv.InnerHtml = "There Are No Topics Posted To This Forum<br /><br />"
                        End If
                    End If
                End If
            End If
        End Sub

        Sub ListTopics(Optional ByVal CurrentPage As Integer = 1, Optional ByVal ItemsPerPage As Integer = 15)
            Dim NumPages, NumItems, NumWholePages, Leftover As Integer
            Dim IDList As New ArrayList

            If ForumID Is Nothing Then
                ForumID = "-1"
                Topics.Visible = "false"
            End If

            Dim TopicStatus As String = ""
            If Not AllowModeration Then
                TopicStatus = " and T.TOPIC_STATUS <> 0 and T.TOPIC_CONFIRMED = 1"
            End If

            Dim TopicReader As OdbcDataReader = Database.Read("SELECT TOPIC_ID FROM " & Database.DBPrefix & "_TOPICS T WHERE T.FORUM_ID = " & ForumID & TopicStatus & " ORDER BY T.TOPIC_STICKY DESC, T.TOPIC_LASTPOST_DATE DESC")
            While (TopicReader.Read())
                IDList.Add(TopicReader("TOPIC_ID"))
            End While
            TopicReader.close()

            NumItems = IDList.Count
            NumPages = NumItems \ ItemsPerPage
            NumWholePages = NumItems \ ItemsPerPage
            Leftover = NumItems Mod ItemsPerPage

            If Leftover > 0 Then
                NumPages += 1
            End If

            If (CurrentPage < 0) Or (CurrentPage > NumPages) Then
                ListTopics(1, ItemsPerPage)
            Else
                If CurrentPage = NumPages Then
                    NextLink.Visible = False
                    LastLink.Visible = False
                Else
                    NextLink.Visible = True
                    LastLink.Visible = True
                    NextLink.CommandArgument = CurrentPage + 1
                    LastLink.CommandArgument = NumPages
                End If

                If CurrentPage = 1 Then
                    PreviousLink.Visible = False
                    FirstLink.Visible = False
                Else
                    PreviousLink.Visible = True
                    FirstLink.Visible = True
                    PreviousLink.CommandArgument = CurrentPage - 1
                    FirstLink.CommandArgument = 1
                End If

                If NumPages = 1 Then
                    PagingPanel.visible = "false"
                End If

                Dim JumpPageList As New ArrayList
                Dim x As Integer
                For x = 1 To NumPages
                    JumpPageList.Add(x)
                Next

                JumpPage.DataSource = JumpPageList
                JumpPage.Databind()
                JumpPage.SelectedIndex = CurrentPage - 1

                PageCountLabel.Text = NumPages

                Dim StartOfPage As Integer = ItemsPerPage * (CurrentPage - 1)
                Dim EndOfPage As Integer = Min((ItemsPerPage * (CurrentPage - 1)) + (ItemsPerPage - 1), ((NumWholePages * ItemsPerPage) + Leftover - 1))

                Dim CurrentSubset As String = Join(IDList.GetRange(StartOfPage, (EndOfPage - StartOfPage + 1)).ToArray, ",")

                Topics.DataSource = Database.Read("SELECT T.TOPIC_ID, T.TOPIC_CONFIRMED, T.TOPIC_UNCONFIRMED_REPLIES, T.TOPIC_SUBJECT, T.TOPIC_AUTHOR, M.MEMBER_USERNAME as TOPIC_AUTHOR_NAME, M.MEMBER_LEVEL, T.TOPIC_REPLIES, T.TOPIC_VIEWS, T.TOPIC_STICKY, T.TOPIC_STATUS, T.TOPIC_LASTPOST_AUTHOR, MEMBERS_1.MEMBER_USERNAME as TOPIC_LASTPOST_NAME, T.TOPIC_LASTPOST_DATE FROM " & Database.DBPrefix & "_MEMBERS M, " & Database.DBPrefix & "_TOPICS T, " & Database.DBPrefix & "_MEMBERS as MEMBERS_1 WHERE M.MEMBER_ID = T.TOPIC_AUTHOR and T.TOPIC_LASTPOST_AUTHOR = MEMBERS_1.MEMBER_ID and T.TOPIC_ID IN (" & CurrentSubSet & ")" & TopicStatus & " ORDER BY T.TOPIC_STICKY DESC, T.TOPIC_LASTPOST_DATE DESC")
                Topics.DataBind()
                Topics.DataSource.Close()
            End If
        End Sub

        Sub ChangePage(ByVal sender As System.Object, ByVal e As System.EventArgs)
            If sender.ToString() = "System.Web.UI.WebControls.LinkButton" Then
                Response.Redirect("community/forums.aspx?ID=" & Request.Querystring("ID") & "&PAGE=" & sender.CommandArgument)
            Else
                Response.Redirect("community/forums.aspx?ID=" & Request.Querystring("ID") & "&PAGE=" & JumpPage.SelectedValue)
            End If
        End Sub

        Sub ApplyForumPassword(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Dim thePassword As String = Functions.RepairString(PasswordBox.text)
            thePassword = Functions.Encrypt(thePassword)
            Dim PasswordReader As OdbcDataReader = Database.Read("SELECT FORUM_ID FROM " & Database.DBPrefix & "_FORUMS WHERE FORUM_PASSWORD = '" & thePassword & "' AND FORUM_ID = " & Request.Querystring("ID"))
            If PasswordReader.HasRows Then
                Session("FORUM_" & Request.Querystring("ID")) = "logged"
                Response.Redirect(Page.ResolveUrl(Request.Url.ToString()))
            Else
                PasswordPanel.visible = "false"
                PagePanel.visible = "false"
                ModeratorsList.visible = "false"
                NoItemsDiv.InnerHtml = "Incorrect Password<br /><br />"
            End If
            PasswordReader.Close()
        End Sub

        Sub EditTopic(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Response.Redirect("community/edittopic.aspx?ID=" & sender.CommandArgument)
        End Sub

        Sub DeleteTopic(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Response.Redirect("community/deletetopic.aspx?ID=" & sender.CommandArgument)
        End Sub

        Sub ConfirmTopic(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Topics.visible = "false"
            PagingPanel.visible = "false"
            ModeratorsList.visible = "false"
            ConfirmTopicForm.visible = "true"
            ConfirmButton.CommandArgument = sender.CommandArgument
        End Sub

        Sub ApplyConfirmation(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Topics.visible = "false"
            PagingPanel.visible = "false"
            ModeratorsList.visible = "false"
            ConfirmTopicForm.visible = "true"

            Dim ForumID As Integer
            Dim ForumReader As OdbcDataReader = Database.Read("SELECT FORUM_ID FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_ID = " & sender.CommandArgument)
            While ForumReader.Read()
                ForumID = ForumReader("FORUM_ID")
            End While
            ForumReader.Close()

            If (ConfirmTopicDropdown.SelectedValue = 1) Then
                Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_CONFIRMED = 1 WHERE TOPIC_ID = " & sender.CommandArgument)
                Functions.UpdateCounts(2, ForumID, 0, 0)
                PagePanel.visible = "false"
                NoItemsDiv.InnerHtml = "The Topic Has Been Confirmed Successfully<br /><br /><a href=""forums.aspx?ID=" & ForumID & """>Click Here</a> To Return To The Forum<br /><br />"
            Else
                Response.Redirect("community/forums.aspx?ID=" & ForumID)
            End If
        End Sub

        Sub NewReply(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Response.Redirect("community/newreply.aspx?ID=" & sender.CommandArgument)
        End Sub

        Sub SetSession()
            If Not Request.Cookies("dmgforums") Is Nothing Then
                Dim aCookie As New System.Web.HttpCookie("dmgforums")

                Dim UserReader As OdbcDataReader = Database.Read("SELECT MEMBER_ID, MEMBER_USERNAME, MEMBER_PASSWORD, MEMBER_LEVEL FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & Server.HtmlEncode(Request.Cookies("dmgforums")("mukul")) & " AND MEMBER_PASSWORD = '" & Server.HtmlEncode(Request.Cookies("dmgforums")("gupta")) & "'", 1)
                While (UserReader.Read())
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

                If ((Session("UserLevel") = 0) Or (Session("UserLevel") = -1)) Then
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
                End If
            Else
                If (Session("UserLogged") = "1") Then
                    Dim UserReader As OdbcDataReader = Database.Read("SELECT MEMBER_ID, MEMBER_USERNAME, MEMBER_PASSWORD, MEMBER_LEVEL FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & Session("UserID"), 1)
                    While (UserReader.Read())
                        Session("UserID") = UserReader("MEMBER_ID").ToString()
                        Session("UserName") = UserReader("MEMBER_USERNAME").ToString()
                        Session("UserLevel") = UserReader("MEMBER_LEVEL").ToString()
                        Session("UserLogged") = "1"
                    End While
                    UserReader.Close()

                    If ((Session("UserLevel") = 0) Or (Session("UserLevel") = -1)) Then
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
                    End If
                Else
                    Session("UserID") = "-1"
                    Session("UserName") = ""
                    Session("UserLogged") = "0"
                    Session("UserLevel") = "0"
                End If
            End If
        End Sub
    End Class


    '---------------------------------------------------------------------------------------------------
    ' NewForum - Codebehind For newforum.aspx
    '---------------------------------------------------------------------------------------------------
    Public Class NewForum
        Inherits System.Web.UI.Page

        Public txtCategoryID As System.Web.UI.WebControls.TextBox
        Public txtName As System.Web.UI.WebControls.TextBox
        Public txtDescription As System.Web.UI.WebControls.TextBox
        Public txtContent As System.Web.UI.WebControls.TextBox
        Public txtShowHeaders As System.Web.UI.WebControls.DropDownList
        Public txtShowLogin As System.Web.UI.WebControls.DropDownList
        Public txtPassword As System.Web.UI.WebControls.TextBox
        Public txtSortBy As System.Web.UI.WebControls.TextBox
        Public txtModerators As System.Web.UI.WebControls.ListBox
        Public txtAllowedModerators As System.Web.UI.WebControls.ListBox
        Public txtMembers As System.Web.UI.WebControls.ListBox
        Public txtAllowedMembers As System.Web.UI.WebControls.ListBox
        Public txtForceConfirm As System.Web.UI.WebControls.DropDownList
        Public txtEmailModerators As System.Web.UI.WebControls.DropDownList
        Public txtShowLatest As System.Web.UI.WebControls.DropDownList
        Public txtStatus As System.Web.UI.WebControls.DropDownList
        Public txtType As System.Web.UI.WebControls.DropDownList
        Public CategoryName As System.Web.UI.WebControls.Label
        Public AllowedUsersPanel As System.Web.UI.WebControls.PlaceHolder
        Public PasswordPanel As System.Web.UI.WebControls.PlaceHolder
        Public EmailModeratorsPanel As System.Web.UI.WebControls.PlaceHolder
        Public PagePanel As System.Web.UI.WebControls.Panel
        Public NoItemsDiv As System.Web.UI.HtmlControls.HtmlGenericControl

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
            If Not Page.IsPostBack() Then
                If (Session("UserLevel") <> "3") Then
                    PagePanel.visible = "false"
                    NoItemsDiv.InnerHtml = "Access Denied<br /><br />"
                Else
                    If (Functions.IsInteger(Request.QueryString("ID"))) Then
                        Dim CatReader As OdbcDataReader = Database.Read("SELECT CATEGORY_NAME, CATEGORY_ID FROM " & Database.DBPrefix & "_CATEGORIES WHERE CATEGORY_STATUS = 1 AND CATEGORY_ID = " & Request.Querystring("ID"))
                        If (Not CatReader.HasRows) Then
                            PagePanel.visible = "false"
                            NoItemsDiv.InnerHtml = "No Category To Post To<br /><br />"
                        Else
                            While CatReader.Read()
                                CategoryName.text = CatReader("CATEGORY_NAME").ToString()
                                txtCategoryID.text = CatReader("CATEGORY_ID")
                            End While
                            If Not Page.IsPostBack Then
                                txtModerators.Datasource = Database.Read("SELECT MEMBER_ID, MEMBER_USERNAME FROM " & Database.DBPrefix & "_MEMBERS WHERE (MEMBER_LEVEL = 2) or (MEMBER_LEVEL = 3)")
                                txtModerators.Databind()
                                txtModerators.Datasource.Close()
                                txtMembers.Datasource = Database.Read("SELECT MEMBER_ID, MEMBER_USERNAME FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_LEVEL <> 0")
                                txtMembers.Databind()
                                txtMembers.Datasource.Close()
                            End If
                        End If
                        CatReader.Close()

                        If (Settings.HideLogin = 1) Then
                            txtShowLogin.SelectedValue = 0
                        Else
                            txtShowLogin.SelectedValue = 1
                        End If
                    Else
                        PagePanel.visible = "false"
                        NoItemsDiv.InnerHtml = "No Category To Post To<br /><br />"
                    End If
                End If
            End If
        End Sub

        Sub SubmitForum(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Dim Failure As Integer = 0

            If (txtName.text = "") Or (txtName.text = " ") Then
                Failure = 1
                Functions.Messagebox("No Name Entered!")
            End If

            If Failure = 0 Then
                If (txtSortBy.text = "") Or (txtSortBy.text = " ") Then
                    txtSortBy.text = "1"
                End If

                PagePanel.visible = "false"

                Dim CategoryID As String = CLng(txtCategoryID.text)
                Dim ForumStatus As String = txtStatus.SelectedValue.ToString()
                Dim ForceConfirm As String = txtForceConfirm.SelectedValue.ToString()
                Dim EmailModerators As String = txtEmailModerators.SelectedValue.ToString()
                Dim ShowLatest As String = txtShowLatest.SelectedValue.ToString()
                Dim ForumSortby As String = CLng(txtSortBy.text)
                Dim ForumName As String = Functions.RepairString(txtName.text, 0)
                Dim ForumContent As String = Functions.RepairString(txtContent.text, 0)
                Dim ForumDescription As String = Functions.RepairString(txtDescription.text, 0)
                Dim ForumType As String = txtType.SelectedValue.ToString()
                Dim ForumShowHeaders As String = txtShowHeaders.SelectedValue.ToString()
                Dim ForumShowLogin As String = txtShowLogin.SelectedValue.ToString()
                Dim ForumPassword As String = ""
                Dim NewForumID As Integer

                If (txtType.SelectedValue = 2) Then
                    ForumPassword = Functions.Encrypt(txtPassword.Text)
                End If

                Database.Write("INSERT INTO " & Database.DBPrefix & "_FORUMS (CATEGORY_ID, FORUM_STATUS, FORUM_SORTBY, FORUM_NAME, FORUM_CONTENT, FORUM_DESCRIPTION, FORUM_TOPICS, FORUM_POSTS, FORUM_LASTPOST_AUTHOR, FORUM_LASTPOST_TOPIC, FORUM_TYPE, FORUM_PASSWORD, FORUM_FORCECONFIRM, FORUM_EMAIL_MODERATORS, FORUM_SHOWLATEST, FORUM_SHOWHEADERS, FORUM_SHOWLOGIN) VALUES (" & CategoryID & ", " & ForumStatus & ", " & ForumSortby & ", '" & ForumName & "', '" & ForumContent & "', '" & ForumDescription & "', 0, 0, 0, 0, " & ForumType & ", '" & ForumPassword & "', " & ForceConfirm & ", " & EmailModerators & ", " & ShowLatest & ", " & ForumShowHeaders & ", " & ForumShowLogin & ")")

                Dim ForumPostback As OdbcDataReader = Database.Read("SELECT FORUM_ID FROM " & Database.DBPrefix & "_FORUMS ORDER BY FORUM_ID DESC", 1)
                While ForumPostback.Read()
                    NewForumID = ForumPostback("FORUM_ID")
                End While
                ForumPostback.Close()

                Dim Count1 As Integer
                For Count1 = 0 To (txtAllowedModerators.Items.Count - 1)
                    Database.Write("INSERT INTO " & Database.DBPrefix & "_PRIVILEGED (MEMBER_ID, FORUM_ID, PRIVILEGED_ACCESS) VALUES (" & txtAllowedModerators.Items.Item(Count1).Value.ToString() & ", " & NewForumID & ", 2)")
                Next

                If (txtType.SelectedValue = 1) Then
                    Dim Count2 As Integer
                    For Count2 = 0 To (txtAllowedMembers.Items.Count - 1)
                        Database.Write("INSERT INTO " & Database.DBPrefix & "_PRIVILEGED (MEMBER_ID, FORUM_ID, PRIVILEGED_ACCESS) VALUES (" & txtAllowedMembers.Items.Item(Count2).Value.ToString() & ", " & NewForumID & ", 1)")
                    Next
                End If

                NoItemsDiv.InnerHtml = "Forum Created Successfully<br /><br /><a href=""ForumHome.aspx"">Click Here</a> To Return To The Forums<br /><br />"
            End If
        End Sub

        Sub SecurityChange(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Select Case (CType(sender.SelectedIndex, Integer))
                Case 1
                    AllowedUsersPanel.visible = "true"
                    PasswordPanel.visible = "false"
                Case 2
                    PasswordPanel.visible = "true"
                    AllowedUsersPanel.visible = "false"
                Case Else
                    AllowedUsersPanel.visible = "false"
                    PasswordPanel.visible = "false"
            End Select
        End Sub

        Sub ConfirmationChange(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Select Case (CType(sender.SelectedIndex, Integer))
                Case 0
                    EmailModeratorsPanel.visible = "true"
                Case Else
                    EmailModeratorsPanel.visible = "false"
            End Select
        End Sub

        Sub AddMember(ByVal sender As System.Object, ByVal e As System.EventArgs)
            If Functions.IsInteger(txtMembers.SelectedValue) Then
                Dim LItem As New ListItem(txtMembers.SelectedItem.Text, txtMembers.SelectedValue)
                txtAllowedMembers.Items.Add(LItem)
                txtMembers.Items.Remove(LItem)
            Else
                Functions.Messagebox("No Item Selected")
            End If
        End Sub

        Sub RemoveMember(ByVal sender As System.Object, ByVal e As System.EventArgs)
            If Functions.IsInteger(txtAllowedMembers.SelectedValue) Then
                Dim LItem As New ListItem(txtAllowedMembers.SelectedItem.Text, txtAllowedMembers.SelectedValue)
                txtAllowedMembers.Items.Remove(LItem)
                txtMembers.Items.Add(LItem)
            Else
                Functions.Messagebox("No Item Selected")
            End If
        End Sub

        Sub AddModerator(ByVal sender As System.Object, ByVal e As System.EventArgs)
            If Functions.IsInteger(txtModerators.SelectedValue) Then
                Dim LItem As New ListItem(txtModerators.SelectedItem.Text, txtModerators.SelectedValue)
                txtAllowedModerators.Items.Add(LItem)
                txtModerators.Items.Remove(LItem)
            Else
                Functions.Messagebox("No Item Selected")
            End If
        End Sub

        Sub RemoveModerator(ByVal sender As System.Object, ByVal e As System.EventArgs)
            If Functions.IsInteger(txtAllowedModerators.SelectedValue) Then
                Dim LItem As New ListItem(txtAllowedModerators.SelectedItem.Text, txtAllowedModerators.SelectedValue)
                txtAllowedModerators.Items.Remove(LItem)
                txtModerators.Items.Add(LItem)
            Else
                Functions.Messagebox("No Item Selected")
            End If
        End Sub
    End Class


    '---------------------------------------------------------------------------------------------------
    ' EditForum - Codebehind For editforum.aspx
    '---------------------------------------------------------------------------------------------------
    Public Class EditForum
        Inherits System.Web.UI.Page

        Public txtCategoryID As System.Web.UI.WebControls.DropDownList
        Public txtName As System.Web.UI.WebControls.TextBox
        Public txtContent As System.Web.UI.WebControls.TextBox
        Public txtShowHeaders As System.Web.UI.WebControls.DropDownList
        Public txtShowLogin As System.Web.UI.WebControls.DropDownList
        Public txtSortBy As System.Web.UI.WebControls.TextBox
        Public txtDescription As System.Web.UI.WebControls.TextBox
        Public txtPassword As System.Web.UI.WebControls.TextBox
        Public txtStatus As System.Web.UI.WebControls.DropDownList
        Public txtForceConfirm As System.Web.UI.WebControls.DropDownList
        Public txtEmailModerators As System.Web.UI.WebControls.DropDownList
        Public txtShowLatest As System.Web.UI.WebControls.DropDownList
        Public txtType As System.Web.UI.WebControls.DropDownList
        Public txtModerators As System.Web.UI.WebControls.ListBox
        Public txtAllowedModerators As System.Web.UI.WebControls.ListBox
        Public txtMembers As System.Web.UI.WebControls.ListBox
        Public txtAllowedMembers As System.Web.UI.WebControls.ListBox
        Public CategoryName As System.Web.UI.WebControls.Label
        Public SelectedMemberList As System.Web.UI.WebControls.TextBox
        Public SelectedModeratorList As System.Web.UI.WebControls.TextBox
        Public AllowedUsersPanel As System.Web.UI.WebControls.PlaceHolder
        Public PasswordPanel As System.Web.UI.WebControls.PlaceHolder
        Public EmailModeratorsPanel As System.Web.UI.WebControls.PlaceHolder
        Public PagePanel As System.Web.UI.WebControls.Panel
        Public NoItemsDiv As System.Web.UI.HtmlControls.HtmlGenericControl

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
            If Not Page.IsPostBack() Then
                If (Session("UserLevel") = "3") And (Functions.IsInteger(Request.QueryString("ID"))) Then
                    Dim ForumReader As OdbcDataReader = Database.Read("SELECT * FROM " & Database.DBPrefix & "_FORUMS WHERE FORUM_ID = " & Request.QueryString("ID"))
                    If ForumReader.HasRows Then
                        Dim PrivilegedReader As OdbcDataReader = Database.Read("SELECT MEMBER_ID FROM " & Database.DBPrefix & "_PRIVILEGED WHERE FORUM_ID = " & Request.QueryString("ID") & " AND PRIVILEGED_ACCESS = 1")
                        Dim SelectedMembers As String = ""
                        While PrivilegedReader.Read()
                            If SelectedMembers = "" Then
                                SelectedMembers &= PrivilegedReader("MEMBER_ID")
                            Else
                                SelectedMembers &= "', '" & PrivilegedReader("MEMBER_ID")
                            End If
                        End While
                        SelectedMemberList.text = SelectedMembers
                        PrivilegedReader.Close()
                        txtMembers.Datasource = Database.Read("SELECT MEMBER_ID, MEMBER_USERNAME FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_LEVEL <> 0 AND MEMBER_ID NOT IN ('" & SelectedMemberList.text & "')")
                        txtMembers.Databind()
                        txtMembers.Datasource.Close()
                        txtAllowedMembers.Datasource = Database.Read("SELECT MEMBER_ID, MEMBER_USERNAME FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_LEVEL <> 0 AND MEMBER_ID IN ('" & SelectedMemberList.text & "')")
                        txtAllowedMembers.Databind()
                        txtAllowedMembers.Datasource.Close()

                        Dim PrivilegedReader2 As OdbcDataReader = Database.Read("SELECT MEMBER_ID FROM " & Database.DBPrefix & "_PRIVILEGED WHERE FORUM_ID = " & Request.QueryString("ID") & " AND PRIVILEGED_ACCESS = 2")
                        Dim SelectedModerators As String = ""
                        While PrivilegedReader2.Read()
                            If SelectedModerators = "" Then
                                SelectedModerators &= PrivilegedReader2("MEMBER_ID")
                            Else
                                SelectedModerators &= "', '" & PrivilegedReader2("MEMBER_ID")
                            End If
                        End While
                        SelectedModeratorList.text = SelectedModerators
                        PrivilegedReader2.Close()
                        txtModerators.Datasource = Database.Read("SELECT MEMBER_ID, MEMBER_USERNAME FROM " & Database.DBPrefix & "_MEMBERS WHERE (MEMBER_LEVEL = 2 OR MEMBER_LEVEL = 3) AND MEMBER_ID NOT IN ('" & SelectedModeratorList.text & "')")
                        txtModerators.Databind()
                        txtModerators.Datasource.Close()
                        txtAllowedModerators.Datasource = Database.Read("SELECT MEMBER_ID, MEMBER_USERNAME FROM " & Database.DBPrefix & "_MEMBERS WHERE (MEMBER_LEVEL = 2 OR MEMBER_LEVEL = 3) AND MEMBER_ID IN ('" & SelectedModeratorList.text & "')")
                        txtAllowedModerators.Databind()
                        txtAllowedModerators.Datasource.Close()

                        While (ForumReader.Read())
                            txtCategoryID.Datasource = Database.Read("SELECT CATEGORY_ID, CATEGORY_NAME FROM " & Database.DBPrefix & "_CATEGORIES")
                            txtCategoryID.Databind()
                            txtCategoryID.Datasource.Close()

                            txtName.text = Server.HTMLDecode(ForumReader("FORUM_NAME").ToString())
                            txtContent.text = ForumReader("FORUM_CONTENT").ToString()
                            txtSortBy.text = ForumReader("FORUM_SORTBY")
                            txtDescription.text = Server.HTMLDecode(ForumReader("FORUM_DESCRIPTION").ToString())

                            txtCategoryID.Items.FindByValue(ForumReader("CATEGORY_ID").ToString()).Selected = "True"
                            txtStatus.Items.FindByValue(ForumReader("FORUM_STATUS").ToString()).Selected = "True"
                            txtForceConfirm.Items.FindByValue(ForumReader("FORUM_FORCECONFIRM").ToString()).Selected = "True"
                            txtEmailModerators.Items.FindByValue(ForumReader("FORUM_EMAIL_MODERATORS").ToString()).Selected = "True"
                            txtShowLatest.Items.FindByValue(ForumReader("FORUM_SHOWLATEST").ToString()).Selected = "True"
                            txtShowHeaders.Items.FindByValue(ForumReader("FORUM_SHOWHEADERS").ToString()).Selected = "True"
                            txtShowLogin.Items.FindByValue(ForumReader("FORUM_SHOWLOGIN").ToString()).Selected = "True"
                            txtType.Items.FindByValue(ForumReader("FORUM_TYPE").ToString()).Selected = "True"

                            If (ForumReader("FORUM_FORCECONFIRM") = 1) Then
                                EmailModeratorsPanel.visible = "true"
                            End If

                            If ForumReader("FORUM_TYPE") = 1 Then
                                AllowedUsersPanel.visible = "true"
                                PasswordPanel.visible = "false"
                            ElseIf ForumReader("FORUM_TYPE") = 2 Then
                                AllowedUsersPanel.visible = "false"
                                PasswordPanel.visible = "true"
                            End If
                        End While
                    Else
                        PagePanel.visible = "false"
                        NoItemsDiv.InnerHtml = "Incorrect Forum ID<br /><br />"
                    End If
                    ForumReader.close()
                Else
                    PagePanel.visible = "false"
                    NoItemsDiv.InnerHtml = "Access Denied<br /><br />"
                End If
            End If
        End Sub

        Sub EditForum(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Dim Failure As Integer = 0

            If (txtName.text = "") Or (txtName.text = " ") Then
                Failure = 1
                Functions.Messagebox("No Name Entered!")
            End If

            If Failure = 0 Then
                PagePanel.visible = "false"

                Dim CategoryID As String = txtCategoryID.SelectedValue.ToString()
                Dim ForumStatus As String = txtStatus.SelectedValue.ToString()
                Dim ForceConfirm As String = txtForceConfirm.SelectedValue.ToString()
                Dim EmailModerators As String = txtEmailModerators.SelectedValue.ToString()
                Dim ShowLatest As String = txtShowLatest.SelectedValue.ToString()
                Dim ShowHeaders As String = txtShowHeaders.SelectedValue.ToString()
                Dim ShowLogin As String = txtShowLogin.SelectedValue.ToString()
                Dim ForumSortby As String = CLng(txtSortby.text)
                Dim ForumName As String = Functions.RepairString(txtName.text, 0)
                Dim ForumContent As String = Functions.RepairString(txtContent.text, 0)
                Dim ForumDescription As String = Functions.RepairString(txtDescription.text, 0)
                Dim ForumType As String = CLng(txtType.SelectedValue.ToString())
                Dim ForumPassword As String = ""

                If (txtType.SelectedValue = 2) Then
                    ForumPassword = Functions.Encrypt(txtPassword.Text)
                End If

                Database.Write("UPDATE " & Database.DBPrefix & "_FORUMS SET CATEGORY_ID = " & CategoryID & ", FORUM_STATUS = " & ForumStatus & ", FORUM_SORTBY = " & ForumSortby & ", FORUM_NAME = '" & ForumName & "', FORUM_CONTENT = '" & ForumContent & "', FORUM_DESCRIPTION = '" & ForumDescription & "', FORUM_TYPE = " & ForumType & ", FORUM_PASSWORD = '" & ForumPassword & "', FORUM_FORCECONFIRM = " & ForceConfirm & ", FORUM_EMAIL_MODERATORS = " & EmailModerators & ", FORUM_SHOWLATEST = " & ShowLatest & ", FORUM_SHOWHEADERS = " & ShowHeaders & ", FORUM_SHOWLOGIN = " & ShowLogin & " WHERE FORUM_ID = " & Request.Querystring("ID"))

                Database.Write("DELETE FROM " & Database.DBPrefix & "_PRIVILEGED WHERE FORUM_ID = " & Request.Querystring("ID"))

                Dim Count1 As Integer
                For Count1 = 0 To (txtAllowedModerators.Items.Count - 1)
                    Database.Write("INSERT INTO " & Database.DBPrefix & "_PRIVILEGED (MEMBER_ID, FORUM_ID, PRIVILEGED_ACCESS) VALUES (" & txtAllowedModerators.Items.Item(Count1).Value.ToString() & ", " & Request.Querystring("ID") & ", 2)")
                Next

                If (txtType.SelectedValue = 1) Then
                    Dim Count2 As Integer
                    For Count2 = 0 To (txtAllowedMembers.Items.Count - 1)
                        Database.Write("INSERT INTO " & Database.DBPrefix & "_PRIVILEGED (MEMBER_ID, FORUM_ID, PRIVILEGED_ACCESS) VALUES (" & txtAllowedMembers.Items.Item(Count2).Value.ToString() & ", " & Request.Querystring("ID") & ", 1)")
                    Next
                End If

                NoItemsDiv.InnerHtml = "Forum Edited Successfully<br /><br /><a href=""ForumHome.aspx"">Click Here</a> To Return To The Forums<br /><br />"
            End If
        End Sub

        Sub SecurityChange(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Select Case (CType(sender.SelectedIndex, Integer))
                Case 1
                    AllowedUsersPanel.visible = "true"
                    PasswordPanel.visible = "false"
                Case 2
                    PasswordPanel.visible = "true"
                    AllowedUsersPanel.visible = "false"
                Case Else
                    AllowedUsersPanel.visible = "false"
                    PasswordPanel.visible = "false"
            End Select
        End Sub

        Sub ConfirmationChange(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Select Case (CType(sender.SelectedIndex, Integer))
                Case 0
                    EmailModeratorsPanel.visible = "true"
                Case Else
                    EmailModeratorsPanel.visible = "false"
            End Select
        End Sub

        Sub AddMember(ByVal sender As System.Object, ByVal e As System.EventArgs)
            If Functions.IsInteger(txtMembers.SelectedValue) Then
                Dim LItem As New ListItem(txtMembers.SelectedItem.Text, txtMembers.SelectedValue)
                txtAllowedMembers.Items.Add(LItem)
                txtMembers.Items.Remove(LItem)
            Else
                Functions.Messagebox("No Item Selected")
            End If
        End Sub

        Sub RemoveMember(ByVal sender As System.Object, ByVal e As System.EventArgs)
            If Functions.IsInteger(txtAllowedMembers.SelectedValue) Then
                Dim LItem As New ListItem(txtAllowedMembers.SelectedItem.Text, txtAllowedMembers.SelectedValue)
                txtAllowedMembers.Items.Remove(LItem)
                txtMembers.Items.Add(LItem)
            Else
                Functions.Messagebox("No Item Selected")
            End If
        End Sub

        Sub AddModerator(ByVal sender As System.Object, ByVal e As System.EventArgs)
            If Functions.IsInteger(txtModerators.SelectedValue) Then
                Dim LItem As New ListItem(txtModerators.SelectedItem.Text, txtModerators.SelectedValue)
                txtAllowedModerators.Items.Add(LItem)
                txtModerators.Items.Remove(LItem)
            Else
                Functions.Messagebox("No Item Selected")
            End If
        End Sub

        Sub RemoveModerator(ByVal sender As System.Object, ByVal e As System.EventArgs)
            If Functions.IsInteger(txtAllowedModerators.SelectedValue) Then
                Dim LItem As New ListItem(txtAllowedModerators.SelectedItem.Text, txtAllowedModerators.SelectedValue)
                txtAllowedModerators.Items.Remove(LItem)
                txtModerators.Items.Add(LItem)
            Else
                Functions.Messagebox("No Item Selected")
            End If
        End Sub
    End Class


    '---------------------------------------------------------------------------------------------------
    ' DeleteForum - Codebehind For deleteforum.aspx
    '---------------------------------------------------------------------------------------------------
    Public Class DeleteForum
        Inherits System.Web.UI.Page

        Public DeleteButton As System.Web.UI.WebControls.Button
        Public ForumName As System.Web.UI.WebControls.Label
        Public PagePanel As System.Web.UI.WebControls.Panel
        Public NoItemsDiv As System.Web.UI.HtmlControls.HtmlGenericControl

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
            If Not Page.IsPostBack() Then
                If (Session("UserLevel") = "3") And (Functions.IsInteger(Request.QueryString("ID"))) Then
                    Dim ForumReader As OdbcDataReader = Database.Read("SELECT FORUM_ID, FORUM_NAME FROM " & Database.DBPrefix & "_FORUMS WHERE FORUM_ID = " & Request.QueryString("ID"))
                    If ForumReader.HasRows Then
                        While (ForumReader.Read())
                            DeleteButton.CommandArgument = ForumReader("FORUM_ID")
                            ForumName.text = ForumReader("FORUM_NAME")
                        End While
                    Else
                        PagePanel.visible = "false"
                        NoItemsDiv.InnerHtml = "Invalid Forum ID<br /><br />"
                    End If
                    ForumReader.close()
                Else
                    PagePanel.visible = "false"
                    NoItemsDiv.InnerHtml = "Access Denied<br /><br />"
                End If
            End If
        End Sub

        Sub DeleteForum(ByVal sender As System.Object, ByVal e As System.EventArgs)
            PagePanel.visible = "false"

            Database.Write("DELETE FROM " & Database.DBPrefix & "_FORUMS WHERE FORUM_ID = " & sender.CommandArgument)
            Database.Write("DELETE FROM " & Database.DBPrefix & "_PRIVILEGED WHERE FORUM_ID = " & sender.CommandArgument)

            Dim TopicReader As OdbcDataReader = Database.Read("SELECT TOPIC_ID FROM " & Database.DBPrefix & "_TOPICS WHERE FORUM_ID = " & sender.CommandArgument)
            While (TopicReader.Read())
                Database.Write("DELETE FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_ID = " & TopicReader("TOPIC_ID"))
                Database.Write("DELETE FROM " & Database.DBPrefix & "_REPLIES WHERE TOPIC_ID = " & TopicReader("TOPIC_ID"))
            End While
            TopicReader.Close()

            NoItemsDiv.InnerHtml = "Forum Deleted Successfully<br /><br /><a href=""ForumHome.aspx"">Click Here</a> To Return To The Forums<br /><br />"
        End Sub
    End Class


    '---------------------------------------------------------------------------------------------------
    ' RSSFeed - Codebehind For rssfeed.aspx
    '---------------------------------------------------------------------------------------------------
    Public Class RSSFeed
        Inherits System.Web.UI.Page

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
            If (Functions.IsInteger(Request.QueryString("ID"))) Then
                Response.Clear()
                Response.ContentType = "text/xml"

                Dim TextFeed As XmlTextWriter = New XmlTextWriter(Response.OutputStream, Encoding.UTF8)
                TextFeed.WriteStartDocument()
                TextFeed.WriteStartElement("rss")
                TextFeed.WriteAttributeString("version", "2.0")

                Dim ForumName As String = ""
                Dim ForumDesc As String = ""
                Dim Reader As OdbcDataReader = Database.Read("SELECT FORUM_NAME, FORUM_DESCRIPTION FROM " & Database.DBPrefix & "_FORUMS WHERE FORUM_ID = " & Request.Querystring("ID"), 1)
                While Reader.Read()
                    ForumName = Reader("FORUM_NAME").ToString()
                    ForumDesc = Reader("FORUM_DESCRIPTION").ToString()
                End While
                Reader.Close()

                TextFeed.WriteStartElement("channel")
                TextFeed.WriteElementString("title", Settings.PageTitle & " - " & ForumName)
                TextFeed.WriteElementString("link", Functions.GetFullURLPath() & "/forums.aspx?ID=" & Request.QueryString("ID"))
                TextFeed.WriteElementString("description", ForumDesc)
                TextFeed.WriteElementString("copyright", Settings.Copyright)

                Dim Message As String = ""
                Reader = Database.Read("SELECT T.TOPIC_ID, T.TOPIC_DATE, T.TOPIC_SUBJECT, T.TOPIC_MESSAGE, T.TOPIC_LASTPOST_DATE, M.MEMBER_USERNAME as TOPIC_AUTHOR_NAME FROM " & Database.DBPrefix & "_MEMBERS M, " & Database.DBPrefix & "_TOPICS T WHERE M.MEMBER_ID = T.TOPIC_AUTHOR and T.FORUM_ID = " & Request.Querystring("ID") & " and T.TOPIC_STATUS <> 0 and T.TOPIC_CONFIRMED = 1 ORDER BY T.TOPIC_DATE DESC", 50)
                While Reader.Read()
                    TextFeed.WriteStartElement("item")
                    TextFeed.WriteElementString("title", Reader("TOPIC_SUBJECT").ToString())
                    TextFeed.WriteElementString("author", Reader("TOPIC_AUTHOR_NAME").ToString())
                    TextFeed.WriteElementString("description", "Posted by " & Reader("TOPIC_AUTHOR_NAME").ToString() & " on " & Reader("TOPIC_DATE").ToString() & ".  " & Left(Reader("TOPIC_MESSAGE").ToString(), 200))
                    TextFeed.WriteElementString("link", Functions.GetFullURLPath() & "/topics.aspx?ID=" & Reader("TOPIC_ID").ToString())
                    TextFeed.WriteElementString("pubDate", Reader("TOPIC_LASTPOST_DATE").ToString())
                    TextFeed.WriteEndElement()
                End While
                Reader.Close()

                TextFeed.WriteEndElement()
                TextFeed.WriteEndElement()
                TextFeed.WriteEndDocument()
                TextFeed.Flush()
                TextFeed.Close()

                Response.End()
            Else
                If (Request.QueryString("ID") = "news") Then
                    Response.Clear()
                    Response.ContentType = "text/xml"

                    Dim TextFeed As XmlTextWriter = New XmlTextWriter(Response.OutputStream, Encoding.UTF8)
                    TextFeed.WriteStartDocument()
                    TextFeed.WriteStartElement("rss")
                    TextFeed.WriteAttributeString("version", "2.0")

                    TextFeed.WriteStartElement("channel")
                    TextFeed.WriteElementString("title", Settings.PageTitle & " - Featured Topics")
                    TextFeed.WriteElementString("link", Functions.GetFullURLPath())
                    TextFeed.WriteElementString("description", "Featured Topics")
                    TextFeed.WriteElementString("copyright", Settings.Copyright)

                    Dim Message As String = ""
                    Dim Reader As OdbcDataReader = Database.Read("SELECT T.TOPIC_ID, T.TOPIC_DATE, T.TOPIC_SUBJECT, T.TOPIC_MESSAGE, T.TOPIC_LASTPOST_DATE, M.MEMBER_USERNAME as TOPIC_AUTHOR_NAME FROM " & Database.DBPrefix & "_MEMBERS M, " & Database.DBPrefix & "_TOPICS T WHERE M.MEMBER_ID = T.TOPIC_AUTHOR and T.TOPIC_STATUS <> 0 and T.TOPIC_CONFIRMED = 1 and T.TOPIC_NEWS = 1 ORDER BY T.TOPIC_DATE DESC", 50)
                    While Reader.Read()
                        TextFeed.WriteStartElement("item")
                        TextFeed.WriteElementString("title", Reader("TOPIC_SUBJECT").ToString())
                        TextFeed.WriteElementString("author", Reader("TOPIC_AUTHOR_NAME").ToString())
                        TextFeed.WriteElementString("description", "Posted by " & Reader("TOPIC_AUTHOR_NAME").ToString() & " on " & Reader("TOPIC_DATE").ToString() & ".  " & Left(Reader("TOPIC_MESSAGE").ToString(), 200))
                        TextFeed.WriteElementString("link", Functions.GetFullURLPath() & "/topics.aspx?ID=" & Reader("TOPIC_ID").ToString())
                        TextFeed.WriteElementString("pubDate", Reader("TOPIC_LASTPOST_DATE").ToString())
                        TextFeed.WriteEndElement()
                    End While
                    Reader.Close()

                    TextFeed.WriteEndElement()
                    TextFeed.WriteEndElement()
                    TextFeed.WriteEndDocument()
                    TextFeed.Flush()
                    TextFeed.Close()

                    Response.End()
                Else
                    Response.Redirect("community/default.aspx")
                End If
            End If
        End Sub
    End Class

End Namespace
