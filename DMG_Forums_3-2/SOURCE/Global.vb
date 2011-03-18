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
Imports System.Configuration
Imports System.Data
Imports System.Data.Odbc
Imports System.Environment
Imports System.Net.Mail
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Security.Cryptography
Imports System.Web
Imports System.Web.HttpUtility
Imports System.Web.SessionState
Imports Microsoft.VisualBasic
Imports System.IO

Namespace DMGForums.Global


    Public Class ForumnHandler
        Inherits System.Web.Security.MembershipProvider

        Public Overrides Property ApplicationName As String
            Get
                Return Nothing
            End Get
            Set(ByVal value As String)

            End Set
        End Property

        Public Overrides Function ChangePassword(ByVal username As String, ByVal oldPassword As String, ByVal newPassword As String) As Boolean
            Return Nothing
        End Function

        Public Overrides Function ChangePasswordQuestionAndAnswer(ByVal username As String, ByVal password As String, ByVal newPasswordQuestion As String, ByVal newPasswordAnswer As String) As Boolean
            Return Nothing
        End Function

        Public Overrides Function CreateUser(ByVal username As String, ByVal password As String, ByVal email As String, ByVal passwordQuestion As String, ByVal passwordAnswer As String, ByVal isApproved As Boolean, ByVal providerUserKey As Object, ByRef status As System.Web.Security.MembershipCreateStatus) As System.Web.Security.MembershipUser
            Return Nothing
        End Function

        Public Overrides Function DeleteUser(ByVal username As String, ByVal deleteAllRelatedData As Boolean) As Boolean
            Return Nothing
        End Function

        Public Overrides ReadOnly Property EnablePasswordReset As Boolean
            Get
                Return Nothing
            End Get
        End Property

        Public Overrides ReadOnly Property EnablePasswordRetrieval As Boolean
            Get
                Return Nothing
            End Get
        End Property

        Public Overrides Function FindUsersByEmail(ByVal emailToMatch As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByRef totalRecords As Integer) As System.Web.Security.MembershipUserCollection
            Return Nothing
        End Function

        Public Overrides Function FindUsersByName(ByVal usernameToMatch As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByRef totalRecords As Integer) As System.Web.Security.MembershipUserCollection
            Return Nothing
        End Function

        Public Overrides Function GetAllUsers(ByVal pageIndex As Integer, ByVal pageSize As Integer, ByRef totalRecords As Integer) As System.Web.Security.MembershipUserCollection
            Return Nothing
        End Function

        Public Overrides Function GetNumberOfUsersOnline() As Integer
            Return Nothing
        End Function

        Public Overrides Function GetPassword(ByVal username As String, ByVal answer As String) As String
            Return Nothing
        End Function

        Public Overloads Overrides Function GetUser(ByVal providerUserKey As Object, ByVal userIsOnline As Boolean) As System.Web.Security.MembershipUser
            Dim Query As String = "SELECT MEMBER_USERNAME, MEMBER_ID, MEMBER_PASSWORD, MEMBER_LEVEL, MEMBER_VALIDATED, MEMBER_VALIDATION_STRING, MEMBER_DATE_LASTVISIT FROM " & Database.DBPrefix & "_MEMBERS INNER JOIN openid ON openid.openId_Url LIKE CONCAT('%',MEMBER_USERNAME,'%') WHERE user_id = '" & providerUserKey.ToString() & "' and (MEMBER_LEVEL <> 0) and ((MEMBER_LEVEL <> -1 and MEMBER_VALIDATED = 1) or (MEMBER_LEVEL = -1 and MEMBER_VALIDATED = 0))"
            Dim LoginReader As OdbcDataReader = Database.Read(Query, 1)
            LoginUser(LoginReader)
            Return System.Web.Security.Membership.Providers("MysqlMembershipProvider").GetUser(providerUserKey, userIsOnline)
        End Function
        Private Sub LoginUser(ByVal LoginReader As OdbcDataReader)
            Dim aCookie As New System.Web.HttpCookie("dmgforums")
            Dim Session As HttpSessionState = HttpContext.Current.Session
            Dim Response As HttpResponse = HttpContext.Current.Response
            Dim Request As HttpRequest = HttpContext.Current.Request
            If LoginReader.HasRows Then
                While (LoginReader.Read())
                    Dim strUserName As String = LoginReader("MEMBER_USERNAME")
                    If (LoginReader("MEMBER_VALIDATED") = 1) Then
                        If (False) Then
                            aCookie.Values("fighter") = Functions.Encrypt(DateTime.Now())
                            aCookie.Values("mukul") = LoginReader("MEMBER_ID").ToString()
                            aCookie.Values("dooder") = Functions.Encrypt(DateTime.Now() & "aaa")
                            aCookie.Values("gupta") = LoginReader("MEMBER_PASSWORD").ToString()
                            aCookie.Expires = DateTime.Now.AddDays(30)
                            Response.Cookies.Add(aCookie)
                        End If
                        Session("UserName") = strUserName
                        Session("UserLogged") = "1"
                        Session("UserID") = LoginReader("MEMBER_ID").ToString()
                        LogMessage(Session("UserID"))
                        Session("UserLevel") = LoginReader("MEMBER_LEVEL").ToString()
                        If (Session("ActiveLevel") Is Nothing) Or (Session("ActiveLevel") = 3) Then
                            Session("ActiveLevel") = 1
                            If (Database.DBType = "MySQL") Then
                                Session("ActiveTime") = Functions.FormatDate(LoginReader("MEMBER_DATE_LASTVISIT"), 4)
                            Else
                                Session("ActiveTime") = LoginReader("MEMBER_DATE_LASTVISIT")
                            End If
                        End If
                        Database.Write("UPDATE " & Database.DBPrefix & "_MEMBERS SET MEMBER_DATE_LASTVISIT = " & Database.GetTimeStamp() & ", MEMBER_IP_LAST = '" & Request.UserHostAddress() & "' WHERE MEMBER_ID = " & Session("UserID"))
                        Response.Redirect(Request.Url.ToString())
                    ElseIf ((LoginReader("MEMBER_VALIDATED") = 0) And (Settings.MemberValidation = 2)) Then
                        Functions.MessageBox("Incorrect Username/Password")
                        aCookie.Values("fighter") = Functions.Encrypt(DateTime.Now())
                        aCookie.Values("mukul") = "-3"
                        aCookie.Values("dooder") = Functions.Encrypt(DateTime.Now() & "aaa")
                        aCookie.Values("gupta") = Functions.Encrypt(DateTime.Now() & "bbb")
                        aCookie.Expires = DateTime.Now.AddDays(-1)
                        Response.Cookies.Add(aCookie)
                        Session("UserID") = "-1"
                        Session("UserName") = ""
                        Session("UserLogged") = "0"
                        Session("UserLevel") = "0"
                    Else
                        Session("ValidateUserID") = LoginReader("MEMBER_ID").ToString()
                        Response.Redirect("community/validate.aspx")
                    End If
                End While
            Else
                Functions.MessageBox("Incorrect Username/Password")
                aCookie.Values("fighter") = Functions.Encrypt(DateTime.Now() & "aaa")
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
        End Sub
        Private Sub LogMessage(ByRef message As String)
        End Sub
        Public Overloads Overrides Function GetUser(ByVal username As String, ByVal userIsOnline As Boolean) As System.Web.Security.MembershipUser
            Return Nothing
        End Function

        Public Overrides Function GetUserNameByEmail(ByVal email As String) As String
            Return Nothing
        End Function

        Public Overrides ReadOnly Property MaxInvalidPasswordAttempts As Integer
            Get
                Return Nothing
            End Get
        End Property

        Public Overrides ReadOnly Property MinRequiredNonAlphanumericCharacters As Integer
            Get
                Return Nothing
            End Get
        End Property

        Public Overrides ReadOnly Property MinRequiredPasswordLength As Integer
            Get
                Return Nothing
            End Get
        End Property

        Public Overrides ReadOnly Property PasswordAttemptWindow As Integer
            Get
                Return Nothing
            End Get
        End Property

        Public Overrides ReadOnly Property PasswordFormat As System.Web.Security.MembershipPasswordFormat
            Get
                Return Nothing
            End Get
        End Property

        Public Overrides ReadOnly Property PasswordStrengthRegularExpression As String
            Get
                Return Nothing
            End Get
        End Property

        Public Overrides ReadOnly Property RequiresQuestionAndAnswer As Boolean
            Get
                Return Nothing
            End Get
        End Property

        Public Overrides ReadOnly Property RequiresUniqueEmail As Boolean
            Get
                Return Nothing
            End Get
        End Property

        Public Overrides Function ResetPassword(ByVal username As String, ByVal answer As String) As String
            Return Nothing
        End Function

        Public Overrides Function UnlockUser(ByVal userName As String) As Boolean
            Return Nothing
        End Function

        Public Overrides Sub UpdateUser(ByVal user As System.Web.Security.MembershipUser)

        End Sub

        Public Overrides Function ValidateUser(ByVal username As String, ByVal password As String) As Boolean
            Return True
        End Function
    End Class

    '---------------------------------------------------------------------------------------------------
    ' Database - Class For Connecting To The Database
    '---------------------------------------------------------------------------------------------------
    Public Class Database
        Public Shared ConnString As String = System.Configuration.ConfigurationManager.AppSettings("DatabaseString")
        Public Shared DBPrefix As String = System.Configuration.ConfigurationManager.AppSettings("DatabasePrefix")
        Public Shared DBType As String = System.Configuration.ConfigurationManager.AppSettings("DatabaseType")

        Public Shared Function Read(ByVal strSql As String, Optional ByVal Rows As Integer = 0) As OdbcDataReader
            If (Rows <> 0) Then
                If (DBType = "MySQL") Then
                    strSql = strSql & " LIMIT " & Rows
                Else
                    strSql = strSql.Replace("SELECT", "SELECT TOP " & Rows)
                End If
            End If

            Dim Connection As OdbcConnection = New OdbcConnection(ConnString)
            Connection.Open()
            Dim Command As OdbcCommand = New OdbcCommand(strSql, Connection)
            Return Command.ExecuteReader(CommandBehavior.CloseConnection Or CommandBehavior.SingleResult)
            Connection.Close()
        End Function

        Public Shared Sub Write(ByVal strSql As String)
            Dim Connection As OdbcConnection = New OdbcConnection(ConnString)
            Connection.Open()
            Dim Command As OdbcCommand = New OdbcCommand(strSql, Connection)
            Command.ExecuteNonQuery()
            Connection.Close()
        End Sub

        Public Shared Function GetTimeStamp(Optional ByVal SubtractDays As Integer = 0) As String
            If (DBType = "MySQL") Then
                If (SubtractDays <> 0) Then
                    Return "DATE_SUB(CURRENT_TIMESTAMP(), INTERVAL " & SubtractDays & " DAY)"
                Else
                    Return "CURRENT_TIMESTAMP()"
                End If
            Else
                If (SubtractDays <> 0) Then
                    Return "GETDATE()-" & SubtractDays
                Else
                    Return "GETDATE()"
                End If
            End If
        End Function

        Public Shared Function GetDateDiff(ByVal Units As String, ByVal FirstDate As String, ByVal SecondDate As String) As String
            If (DBType = "MySQL") Then
                If (Units = "ss") Then
                    Return "TIME_TO_SEC(TIMEDIFF(" & SecondDate & "," & FirstDate & "))"
                Else
                    Return "DATEDIFF(" & SecondDate & ", " & FirstDate & ")"
                End If
            Else
                Return "DATEDIFF(""" & Units & """, " & FirstDate & ", " & SecondDate & ")"
            End If
        End Function

        Public Shared Function DatabaseTimestamp(Optional ByVal SubtractDays As Integer = 0) As String
            Dim ReturnString As String = ""
            Dim Reader As OdbcDataReader
            If (DBType = "MySQL") Then
                If (SubtractDays <> 0) Then
                    ReturnString = (DateTime.Now.AddDays((-1) * SubtractDays)).ToString("yyyy-MM-dd hh:mm:ss")
                Else
                    ReturnString = (DateTime.Now()).ToString("yyyy-MM-dd hh:mm:ss")
                End If
            Else
                If (SubtractDays <> 0) Then
                    Reader = Database.Read("SELECT GETDATE()-" & SubtractDays & " as TheTime")
                    While Reader.Read()
                        ReturnString = Reader("TheTime").ToString()
                    End While
                    Reader.Close()
                Else
                    Reader = Database.Read("SELECT GETDATE() as TheTime")
                    While Reader.Read()
                        ReturnString = Reader("TheTime").ToString()
                    End While
                    Reader.Close()
                End If
            End If
            Return ReturnString
        End Function

        Public Shared Function GetAutoIncrement() As String
            If (DBType = "MySQL") Then
                Return "AUTO_INCREMENT"
            Else
                Return "IDENTITY"
            End If
        End Function

        Public Shared Function GetDBName() As String
            Dim ReturnString As String = ""
            Dim Connection As OdbcConnection = New OdbcConnection(ConnString)
            Connection.Open()
            ReturnString = Connection.Database.ToString()
            Connection.Close()
            Return ReturnString
        End Function

        Public Shared Function GetServerName() As String
            Dim ReturnString As String = ""
            Dim Connection As OdbcConnection = New OdbcConnection(ConnString)
            Connection.Open()
            ReturnString = Connection.DataSource.ToString()
            Connection.Close()
            Return ReturnString
        End Function

        Public Shared Function Type() As String
            Dim ReturnString As String = ""
            Dim Connection As OdbcConnection = New OdbcConnection(ConnString)
            Connection.Open()
            If Connection.Driver.ToString() = "SQLSRV32.DLL" Then
                ReturnString = "SQL Server"
            Else
                ReturnString = Connection.Driver.ToString()
            End If
            Connection.Close()
            Return ReturnString
        End Function

        Public Shared Function DatabaseConnection() As OdbcConnection
            Dim TheDB As OdbcConnection = New OdbcConnection(ConnString)
            Return TheDB
        End Function
    End Class


    '---------------------------------------------------------------------------------------------------
    ' Header - Codebehind for inc_header.aspx
    '---------------------------------------------------------------------------------------------------
    Public Class Header
        Inherits System.Web.UI.UserControl
    End Class


    '---------------------------------------------------------------------------------------------------
    ' Footer - Codebehind for inc_footer.aspx
    '---------------------------------------------------------------------------------------------------
    Public Class Footer
        Inherits System.Web.UI.UserControl
    End Class


    '---------------------------------------------------------------------------------------------------
    ' Settings - Codebehind for inc_settings.aspx
    '---------------------------------------------------------------------------------------------------
    Public Class Settings
        Inherits System.Web.UI.UserControl

        Public Shared DefaultTemplate As Integer = 1
        Public Shared PageTitle As String = "DMG Forums"
        Public Shared ForumLogo As String = ""
        Public Shared SiteURL As String = ""
        Public Shared FontFace As String = "arial,helvetica"
        Public Shared FontSize As String = "2"
        Public Shared BGColor As String = "silver"
        Public Shared BGImage As String = ""
        Public Shared FontColor As String = "black"
        Public Shared LinkColor As String = "#190EAF"
        Public Shared LinkDecoration As String = "none"
        Public Shared ALinkColor As String = "#190EAF"
        Public Shared ALinkDecoration As String = "underline"
        Public Shared VLinkColor As String = "#190EAF"
        Public Shared VLinkDecoration As String = "none"
        Public Shared HLinkColor As String = "#190EAF"
        Public Shared HLinkDecoration As String = "underline"
        Public Shared ScrollbarColor As String = "#190EAF"
        Public Shared TopicsFontSize As String = "2"
        Public Shared TopicsFontColor As String = "black"
        Public Shared TableBGColor1 As String = "gainsboro"
        Public Shared TableBGColor2 As String = "whitesmoke"
        Public Shared TableBorderColor As String = "#060237"
        Public Shared LoginFontColor As String = "black"
        Public Shared HeaderColor As String = "#190EAF"
        Public Shared HeaderSize As String = "3"
        Public Shared HeaderFontColor As String = "#CDDC20"
        Public Shared SubHeaderColor As String = "#B6B6B6"
        Public Shared SubHeaderFontColor As String = "black"
        Public Shared FooterColor As String = "#190EAF"
        Public Shared FooterSize As String = "2"
        Public Shared FooterFontColor As String = "#CDDC20"
        Public Shared Copyright As String = "Copyright DMG Forums"
        Public Shared CustomHeader As String = ""
        Public Shared CustomFooter As String = ""
        Public Shared CustomCSS As String = ""
        Public Shared ItemsPerPage As Integer = 15
        Public Shared ButtonColor As String = "#190EAF"
        Public Shared SpamFilter As String = "30"
        Public Shared MetaKeywords As String = ""
        Public Shared ForumsDefault As Integer = 1
        Public Shared SideMargin As Integer = 10
        Public Shared TopMargin As Integer = 10
        Public Shared ShowStatistics As Integer = 1
        Public Shared MemberValidation As Integer = 0
        Public Shared MemberPhotoSize As Integer = 0
        Public Shared ThumbnailSize As Integer = 150
        Public Shared AvatarSize As Integer = 125
        Public Shared SearchTopics As Integer = 1
        Public Shared SearchMembers As Integer = 1
        Public Shared SearchBlogs As Integer = 1
        Public Shared SearchPages As Integer = 1
        Public Shared EmailSmtp As String = "mail.yourserver.com"
        Public Shared EmailPort As String = ""
        Public Shared EmailUsername As String = ""
        Public Shared EmailPassword As String = ""
        Public Shared EmailAddress As String = "mail@yourserver.com"
        Public Shared EmailAllowSend As Integer = 0
        Public Shared EmailAllowSub As Integer = 0
        Public Shared EmailWelcomeMessage As Integer = 0
        Public Shared AllowSub As Integer = 1
        Public Shared QuickRegistration As Integer = 0
        Public Shared CurseFilter As Integer = 0
        Public Shared RSSFeeds As Integer = 0
        Public Shared AllowRegistration As Integer = 1
        Public Shared AllowEdits As Integer = 1
        Public Shared AllowMedia As Integer = 1
        Public Shared HideMembers As Integer = 0
        Public Shared AllowReporting As Integer = 1
        Public Shared HideLogin As Integer = 0
        Public Shared HtmlTitle As String = "DMG Forums"
        Public Shared HorizDivide As String = "&nbsp;&nbsp;<b>|</b>&nbsp;&nbsp;"
        Public Shared VertDivide As String = "<br /><br />"
        Public Shared DMGVersion As String = "3.2"
        Public Shared DMGReleaseDate As String = "January 31, 2009"
        Public Shared DMGVersionText As String = "DMG Forums " & DMGVersion

        Private ShowBack As Boolean = True
        Public Property ShowBackground() As Boolean
            Get
                Return ShowBack
            End Get
            Set(ByVal Value As Boolean)
                If (Value = "false") Then
                    ShowBack = False
                    CustomCSS = CustomCSS.Replace("BODY", ".DMGOldBody")
                Else
                    ShowBack = True
                End If
            End Set
        End Property

        Private CustTitle As String = ""
        Public Property CustomTitle() As String
            Get
                Return CustTitle
            End Get
            Set(ByVal Value As String)
                CustTitle = Value
            End Set
        End Property

        Public Sub New()
            Dim TemplateReader As OdbcDataReader = Database.Read("SELECT " & Database.DBPrefix & "_TEMPLATE_DEFAULT FROM " & Database.DBPrefix & "_SETTINGS WHERE ID = 1", 1)
            While (TemplateReader.Read())
                DefaultTemplate = TemplateReader(Database.DBPrefix & "_TEMPLATE_DEFAULT")
            End While
            TemplateReader.Close()

            Dim SettingsReader As OdbcDataReader = Database.Read("SELECT * FROM " & Database.DBPrefix & "_SETTINGS WHERE ID = " & DefaultTemplate)
            While (SettingsReader.Read())
                PageTitle = SettingsReader(Database.DBPrefix & "_TITLE").ToString()
                FontFace = SettingsReader(Database.DBPrefix & "_FONTFACE").ToString()
                FontSize = SettingsReader(Database.DBPrefix & "_FONTSIZE").ToString()
                FontColor = SettingsReader(Database.DBPrefix & "_FONT_COLOR").ToString()
                BGColor = SettingsReader(Database.DBPrefix & "_BGCOLOR").ToString()
                BGImage = SettingsReader(Database.DBPrefix & "_BGIMAGE").ToString()
                LinkColor = SettingsReader(Database.DBPrefix & "_LINK_COLOR").ToString()
                LinkDecoration = SettingsReader(Database.DBPrefix & "_LINK_DECORATION").ToString()
                ALinkColor = SettingsReader(Database.DBPrefix & "_LINK_ACTIVE_COLOR").ToString()
                ALinkDecoration = SettingsReader(Database.DBPrefix & "_LINK_ACTIVE_DECORATION").ToString()
                VLinkColor = SettingsReader(Database.DBPrefix & "_LINK_VISITED_COLOR").ToString()
                VLinkDecoration = SettingsReader(Database.DBPrefix & "_LINK_VISITED_DECORATION").ToString()
                HLinkColor = SettingsReader(Database.DBPrefix & "_LINK_HOVER_COLOR").ToString()
                HLinkDecoration = SettingsReader(Database.DBPrefix & "_LINK_HOVER_DECORATION").ToString()
                ScrollbarColor = SettingsReader(Database.DBPrefix & "_SCROLLBAR_COLOR").ToString()
                TopicsFontSize = SettingsReader(Database.DBPrefix & "_TOPICS_FONTSIZE").ToString()
                TopicsFontColor = SettingsReader(Database.DBPrefix & "_TOPICS_FONTCOLOR").ToString()
                TableBGColor1 = SettingsReader(Database.DBPrefix & "_TOPICS_BGCOLOR1").ToString()
                TableBGColor2 = SettingsReader(Database.DBPrefix & "_TOPICS_BGCOLOR2").ToString()
                TableBorderColor = SettingsReader(Database.DBPrefix & "_TABLEBORDER_COLOR").ToString()
                LoginFontColor = SettingsReader(Database.DBPrefix & "_LOGIN_FONTCOLOR").ToString()
                HeaderColor = SettingsReader(Database.DBPrefix & "_HEADER_COLOR").ToString()
                HeaderSize = SettingsReader(Database.DBPrefix & "_HEADER_SIZE").ToString()
                HeaderFontColor = SettingsReader(Database.DBPrefix & "_HEADER_FONTCOLOR").ToString()
                SubHeaderColor = SettingsReader(Database.DBPrefix & "_SUBHEADER_COLOR").ToString()
                SubHeaderFontColor = SettingsReader(Database.DBPrefix & "_SUBHEADER_FONTCOLOR").ToString()
                FooterColor = SettingsReader(Database.DBPrefix & "_FOOTER_COLOR").ToString()
                FooterSize = SettingsReader(Database.DBPrefix & "_FOOTER_SIZE").ToString()
                FooterFontColor = SettingsReader(Database.DBPrefix & "_FOOTER_FONTCOLOR").ToString()
                Copyright = SettingsReader(Database.DBPrefix & "_COPYRIGHT").ToString()
                CustomHeader = SettingsReader(Database.DBPrefix & "_CUSTOM_HEADER").ToString()
                CustomFooter = SettingsReader(Database.DBPrefix & "_CUSTOM_FOOTER").ToString()
                CustomCSS = SettingsReader(Database.DBPrefix & "_CUSTOM_CSS").ToString()
                ItemsPerPage = SettingsReader(Database.DBPrefix & "_ITEMS_PER_PAGE")
                ButtonColor = SettingsReader(Database.DBPrefix & "_BUTTON_COLOR").ToString()
                ForumLogo = SettingsReader(Database.DBPrefix & "_LOGO").ToString()
                SiteURL = SettingsReader(Database.DBPrefix & "_URL").ToString()
                SpamFilter = SettingsReader(Database.DBPrefix & "_SPAM_FILTER").ToString()
                ForumsDefault = SettingsReader(Database.DBPrefix & "_FORUMS_DEFAULT")
                SideMargin = SettingsReader(Database.DBPrefix & "_MARGIN_SIDE")
                TopMargin = SettingsReader(Database.DBPrefix & "_MARGIN_TOP")
                ShowStatistics = SettingsReader(Database.DBPrefix & "_SHOWSTATISTICS")
                MetaKeywords = SettingsReader(Database.DBPrefix & "_CUSTOM_META").ToString()
                MemberValidation = SettingsReader(Database.DBPrefix & "_MEMBER_VALIDATION")
                MemberPhotoSize = SettingsReader(Database.DBPrefix & "_MEMBER_PHOTOSIZE")
                ThumbnailSize = SettingsReader(Database.DBPrefix & "_THUMBNAIL_SIZE")
                AvatarSize = SettingsReader(Database.DBPrefix & "_AVATAR_SIZE")
                SearchTopics = SettingsReader(Database.DBPrefix & "_SEARCH_TOPICS")
                SearchMembers = SettingsReader(Database.DBPrefix & "_SEARCH_MEMBERS")
                SearchBlogs = SettingsReader(Database.DBPrefix & "_SEARCH_BLOGS")
                SearchPages = SettingsReader(Database.DBPrefix & "_SEARCH_PAGES")
                EmailSmtp = SettingsReader(Database.DBPrefix & "_EMAIL_SMTP").ToString()
                EmailPort = SettingsReader(Database.DBPrefix & "_EMAIL_PORT").ToString()
                EmailUsername = SettingsReader(Database.DBPrefix & "_EMAIL_USERNAME").ToString()
                EmailPassword = SettingsReader(Database.DBPrefix & "_EMAIL_PASSWORD").ToString()
                EmailAddress = SettingsReader(Database.DBPrefix & "_EMAIL_ADDRESS").ToString()
                EmailAllowSend = SettingsReader(Database.DBPrefix & "_EMAIL_ALLOWSEND")
                EmailAllowSub = SettingsReader(Database.DBPrefix & "_EMAIL_ALLOWSUB")
                EmailWelcomeMessage = SettingsReader(Database.DBPrefix & "_EMAIL_WELCOMEMESSAGE")
                AllowSub = SettingsReader(Database.DBPrefix & "_ALLOWSUB")
                QuickRegistration = SettingsReader(Database.DBPrefix & "_QUICK_REGISTRATION")
                CurseFilter = SettingsReader(Database.DBPrefix & "_CURSE_FILTER")
                RSSFeeds = SettingsReader(Database.DBPrefix & "_RSS_FEEDS")
                AllowRegistration = SettingsReader(Database.DBPrefix & "_ALLOW_REGISTRATION")
                AllowEdits = SettingsReader(Database.DBPrefix & "_ALLOW_EDITS")
                AllowMedia = SettingsReader(Database.DBPrefix & "_ALLOW_MEDIA")
                HideMembers = SettingsReader(Database.DBPrefix & "_HIDE_MEMBERS")
                AllowReporting = SettingsReader(Database.DBPrefix & "_ALLOW_REPORTING")
                HtmlTitle = SettingsReader(Database.DBPrefix & "_HTML_TITLE")
                HideLogin = SettingsReader(Database.DBPrefix & "_HIDE_LOGIN")
                HorizDivide = SettingsReader(Database.DBPrefix & "_HORIZ_DIVIDE")
                VertDivide = SettingsReader(Database.DBPrefix & "_VERT_DIVIDE")
                DMGVersionText = "<a target=""_blank"" href=""./http://www.dmgforums.com/"" style=""color: " & FooterFontColor & ";"">DMG Forums " & DMGVersion & "</a>"
            End While
            SettingsReader.Close()
        End Sub
    End Class


    '---------------------------------------------------------------------------------------------------
    ' Login - Codebehind For inc_login.ascx
    '---------------------------------------------------------------------------------------------------
    Public Class Login
        Inherits System.Web.UI.UserControl

        Public loginbutton As System.Web.UI.WebControls.Button
        Public logoutbutton As System.Web.UI.WebControls.Button
        Public usernamebox As System.Web.UI.WebControls.TextBox
        Public passwordbox As System.Web.UI.WebControls.TextBox
        Public rememberbox As System.Web.UI.WebControls.CheckBox
        Public openIdLogin1 As OrbitOne.OpenId.Controls.OpenIdLogin
        Public ShowLoginPanel As Boolean = "true"

        Private ShowLoginBox As String = 2
        Public Property ShowLogin() As String
            Get
                Return ShowLoginBox
            End Get
            Set(ByVal Value As String)
                ShowLoginBox = Value
            End Set
        End Property

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
            If (Session("ActiveLevel") Is Nothing) Or (Session("ActiveLevel") = 3) Then
                Session("ActiveTime") = Database.DatabaseTimestamp(3)
            End If

            If (((Settings.HideLogin = 1) And (ShowLoginBox = 2)) Or (ShowLoginBox = 0)) Then
                ShowLoginPanel = "false"
            End If
            If Not Request.Cookies("dmgforums") Is Nothing Then
                Dim aCookie As New System.Web.HttpCookie("dmgforums")

                Dim UserReader As OdbcDataReader = Database.Read("SELECT MEMBER_ID, MEMBER_USERNAME, MEMBER_PASSWORD, MEMBER_LEVEL, MEMBER_DATE_LASTVISIT FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & Server.HtmlEncode(Request.Cookies("dmgforums")("mukul")) & " AND MEMBER_PASSWORD = '" & Server.HtmlEncode(Request.Cookies("dmgforums")("gupta")) & "'", 1)
                While (UserReader.Read())
                    Session("UserID") = UserReader("MEMBER_ID").ToString()
                    Session("UserName") = UserReader("MEMBER_USERNAME").ToString()
                    Session("UserLevel") = UserReader("MEMBER_LEVEL").ToString()
                    Session("UserLogged") = "1"
                    If (Session("ActiveLevel") Is Nothing) Or (Session("ActiveLevel") = 3) Then
                        Session("ActiveLevel") = 1
                        If (Database.DBType = "MySQL") Then
                            Session("ActiveTime") = Functions.FormatDate(UserReader("MEMBER_DATE_LASTVISIT"), 4)
                        Else
                            Session("ActiveTime") = UserReader("MEMBER_DATE_LASTVISIT")
                        End If
                    End If
                    aCookie.Values("fighter") = Functions.Encrypt(DateTime.Now())
                    aCookie.Values("mukul") = UserReader("MEMBER_ID").ToString()
                    aCookie.Values("dooder") = Functions.Encrypt(DateTime.Now() & "aaa")
                    aCookie.Values("gupta") = UserReader("MEMBER_PASSWORD").ToString()
                    aCookie.Expires = DateTime.Now.AddDays(30)
                    Response.Cookies.Add(aCookie)
                End While
                UserReader.Close()

                Database.Write("UPDATE " & Database.DBPrefix & "_MEMBERS SET MEMBER_DATE_LASTVISIT = " & Database.GetTimeStamp() & ", MEMBER_IP_LAST = '" & Request.UserHostAddress() & "' WHERE MEMBER_ID = " & Session("UserID"))

                If ((Session("UserLevel") = 0) Or (Session("UserLevel") = -1)) Then
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
                If (Session("UserLogged") = "1") Then
                    Dim UserReader As OdbcDataReader = Database.Read("SELECT MEMBER_ID, MEMBER_USERNAME, MEMBER_PASSWORD, MEMBER_LEVEL, MEMBER_DATE_LASTVISIT FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & Session("UserID"), 1)
                    While (UserReader.Read())
                        Session("UserID") = UserReader("MEMBER_ID").ToString()
                        Session("UserName") = UserReader("MEMBER_USERNAME").ToString()
                        Session("UserLevel") = UserReader("MEMBER_LEVEL").ToString()
                        Session("UserLogged") = "1"
                        If (Session("ActiveLevel") Is Nothing) Or (Session("ActiveLevel") = 3) Then
                            Session("ActiveLevel") = 1
                            If (Database.DBType = "MySQL") Then
                                Session("ActiveTime") = Functions.FormatDate(UserReader("MEMBER_DATE_LASTVISIT"), 4)
                            Else
                                Session("ActiveTime") = UserReader("MEMBER_DATE_LASTVISIT")
                            End If
                        End If
                    End While
                    UserReader.Close()

                    Database.Write("UPDATE " & Database.DBPrefix & "_MEMBERS SET MEMBER_DATE_LASTVISIT = " & Database.GetTimeStamp() & ", MEMBER_IP_LAST = '" & Request.UserHostAddress() & "' WHERE MEMBER_ID = " & Session("UserID"))

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
        Sub LoginOpenIdUser(ByVal sender As System.Object, ByVal e As System.EventArgs)

        End Sub
        Private Sub LoginUser(ByVal LoginReader As OdbcDataReader, ByVal strUserName As String)
            Dim aCookie As New System.Web.HttpCookie("dmgforums")
            If LoginReader.HasRows Then
                While (LoginReader.Read())
                    If (LoginReader("MEMBER_VALIDATED") = 1) Then
                        If (rememberbox.Checked()) Then
                            aCookie.Values("fighter") = Functions.Encrypt(DateTime.Now())
                            aCookie.Values("mukul") = LoginReader("MEMBER_ID").ToString()
                            aCookie.Values("dooder") = Functions.Encrypt(DateTime.Now() & "aaa")
                            aCookie.Values("gupta") = LoginReader("MEMBER_PASSWORD").ToString()
                            aCookie.Expires = DateTime.Now.AddDays(30)
                            Response.Cookies.Add(aCookie)
                        End If
                        Session("UserName") = strUserName
                        Session("UserLogged") = "1"
                        Session("UserID") = LoginReader("MEMBER_ID").ToString()
                        Session("UserLevel") = LoginReader("MEMBER_LEVEL").ToString()
                        If (Session("ActiveLevel") Is Nothing) Or (Session("ActiveLevel") = 3) Then
                            Session("ActiveLevel") = 1
                            If (Database.DBType = "MySQL") Then
                                Session("ActiveTime") = Functions.FormatDate(LoginReader("MEMBER_DATE_LASTVISIT"), 4)
                            Else
                                Session("ActiveTime") = LoginReader("MEMBER_DATE_LASTVISIT")
                            End If
                        End If
                        Database.Write("UPDATE " & Database.DBPrefix & "_MEMBERS SET MEMBER_DATE_LASTVISIT = " & Database.GetTimeStamp() & ", MEMBER_IP_LAST = '" & Request.UserHostAddress() & "' WHERE MEMBER_ID = " & Session("UserID"))
                        Response.Redirect(Page.ResolveUrl(Request.Url.ToString()))
                    ElseIf ((LoginReader("MEMBER_VALIDATED") = 0) And (Settings.MemberValidation = 2)) Then
                        Functions.MessageBox("Incorrect Username/Password")
                        aCookie.Values("fighter") = Functions.Encrypt(DateTime.Now())
                        aCookie.Values("mukul") = "-3"
                        aCookie.Values("dooder") = Functions.Encrypt(DateTime.Now() & "aaa")
                        aCookie.Values("gupta") = Functions.Encrypt(DateTime.Now() & "bbb")
                        aCookie.Expires = DateTime.Now.AddDays(-1)
                        Response.Cookies.Add(aCookie)
                        Session("UserID") = "-1"
                        Session("UserName") = ""
                        Session("UserLogged") = "0"
                        Session("UserLevel") = "0"
                        usernamebox.Text = ""
                        passwordbox.Text = ""
                    Else
                        Session("ValidateUserID") = LoginReader("MEMBER_ID").ToString()
                        Response.Redirect("community/validate.aspx")
                    End If
                End While
            Else
                Functions.MessageBox("Incorrect Username/Password")
                aCookie.Values("fighter") = Functions.Encrypt(DateTime.Now() & "aaa")
                aCookie.Values("mukul") = "-3"
                aCookie.Values("dooder") = Functions.Encrypt(DateTime.Now() & "bbb")
                aCookie.Values("gupta") = Functions.Encrypt(DateTime.Now() & "ccc")
                aCookie.Expires = DateTime.Now.AddDays(-1)
                Response.Cookies.Add(aCookie)
                Session("UserID") = "-1"
                Session("UserName") = ""
                Session("UserLogged") = "0"
                Session("UserLevel") = "0"
                usernamebox.Text = ""
                passwordbox.Text = ""
            End If
        End Sub
        Sub LoginUser(ByVal sender As System.Object, ByVal e As System.EventArgs)

            Dim strUserName As String = Functions.RepairString(usernamebox.Text)
            Dim strPassword As String = Functions.RepairString(passwordbox.Text)
            strPassword = Functions.Encrypt(strPassword)
            Dim LoginReader As OdbcDataReader = Database.Read("SELECT MEMBER_ID, MEMBER_PASSWORD, MEMBER_LEVEL, MEMBER_VALIDATED, MEMBER_VALIDATION_STRING, MEMBER_DATE_LASTVISIT FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_USERNAME = '" & strUserName & "' and MEMBER_PASSWORD = '" & strPassword & "' and (MEMBER_LEVEL <> 0) and ((MEMBER_LEVEL <> -1 and MEMBER_VALIDATED = 1) or (MEMBER_LEVEL = -1 and MEMBER_VALIDATED = 0))", 1)
            LoginUser(LoginReader, strUserName)
            LoginReader.Close()
        End Sub

        Sub LogoutUser(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Session("ActiveLevel") = 3
            Dim aCookie As New System.Web.HttpCookie("dmgforums")
            aCookie.Values("fighter") = Functions.Encrypt(DateTime.Now() & "aaa")
            aCookie.Values("mukul") = "-3"
            aCookie.Values("dooder") = Functions.Encrypt(DateTime.Now() & "bbb")
            aCookie.Values("gupta") = Functions.Encrypt(DateTime.Now() & "ccc")
            aCookie.Expires = DateTime.Now.AddDays(-1)
            Response.Cookies.Add(aCookie)
            Session("UserID") = "-1"
            Session("UserName") = ""
            Session("UserLogged") = "0"
            Session("UserLevel") = "0"
            Response.Redirect("community/default.aspx")
        End Sub
    End Class


    '---------------------------------------------------------------------------------------------------
    ' Functions - Class To Store Global Functions
    '---------------------------------------------------------------------------------------------------
    Public Class Functions
        Public Shared Function IsDBNull(ByVal dbvalue As Object) As Boolean
            Return dbvalue Is DBNull.Value
        End Function

        Public Shared Function IsInteger(ByVal TheString As String) As Boolean
            If TheString Is Nothing Then
                Return False
            Else
                Dim RegInteger As Regex = New Regex("^-[1-9]+$|^[0-9]+$")
                Return RegInteger.IsMatch(TheString)
            End If
        End Function

        Public Shared Function IsImage(ByVal file As HttpPostedFile) As Boolean
            If (file.ContentLength > 0) And ((file.ContentType = "image/gif") Or (file.ContentType = "image/jpeg") Or (file.ContentType = "image/pjpeg") Or (file.ContentType = "image/png")) Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function LastTopicBy(ByVal theuserid As Object, ByVal theusername As Object, ByVal thedate As Object) As String
            If (IsDBNull(theuserid)) Then
                Return "&nbsp;"
            Else
                Return "Posted By <a href=""./profile.aspx?ID=" & theuserid & """>" & theusername & "</a><br />" & FormatDate(thedate, 3)
            End If
        End Function

        Public Shared Function IsModerator(ByVal theUserID As String, ByVal theUserLevel As String, ByVal theForumID As Integer) As Boolean
            If theUserLevel = "3" Then
                Return True
            Else
                Dim PrivilegedReader As OdbcDataReader = Database.Read("SELECT PRIVILEGED_ID FROM " & Database.DBPrefix & "_PRIVILEGED WHERE MEMBER_ID = " & theUserID & " AND FORUM_ID = " & theForumID & " AND PRIVILEGED_ACCESS = 2")
                If PrivilegedReader.HasRows Then
                    Return True
                Else
                    Return False
                End If
                PrivilegedReader.Close()
            End If
        End Function

        Public Shared Function IsAuthor(ByVal theUserID As String, ByVal theTopicID As Integer) As Boolean
            Dim AuthorReader As OdbcDataReader = Database.Read("SELECT TOPIC_ID FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_ID = " & theTopicID & " AND TOPIC_AUTHOR = " & theUserID)
            If AuthorReader.HasRows Then
                Return True
            Else
                Return False
            End If
            AuthorReader.Close()
        End Function

        Public Shared Function IsPrivileged(ByVal theForumID As Integer, ByVal theForumType As Integer, ByVal theUserID As String, ByVal theUserLevel As String, ByVal theUserLogged As String) As Boolean
            If (theUserLevel = "3") Or (theForumType = 0) Then
                Return True
            Else
                If (theForumType = 1) Then
                    Dim PrivilegedReader As OdbcDataReader = Database.Read("SELECT PRIVILEGED_ID FROM " & Database.DBPrefix & "_PRIVILEGED WHERE MEMBER_ID = " & theUserID & " AND FORUM_ID = " & theForumID & " AND ((PRIVILEGED_ACCESS = 1) or (PRIVILEGED_ACCESS = 2))")
                    If PrivilegedReader.HasRows Then
                        Return True
                    Else
                        Return False
                    End If
                    PrivilegedReader.Close()
                ElseIf (theForumType = 3) Then
                    If theUserLevel = "2" Then
                        Return True
                    Else
                        Return False
                    End If
                ElseIf (theForumType = 4) Then
                    If theUserLogged = "1" Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return True
                End If
            End If
        End Function

        Public Shared Function IsPagePrivileged(ByVal thePageID As Integer, ByVal thePageSecurity As Integer, ByVal theUserID As String, ByVal theUserLevel As String, ByVal theUserLogged As String) As Boolean
            If (theUserLevel = "3") Or (thePageSecurity = 0) Then
                Return True
            Else
                If (thePageSecurity = 1) Then
                    Dim PrivilegedReader As OdbcDataReader = Database.Read("SELECT PRIVILEGED_ID FROM " & Database.DBPrefix & "_PAGES_PRIVILEGED WHERE MEMBER_ID = " & theUserID & " AND PAGE_ID = " & thePageID & " AND PRIVILEGED_ACCESS = 1")
                    If PrivilegedReader.HasRows Then
                        Return True
                    Else
                        Return False
                    End If
                    PrivilegedReader.Close()
                ElseIf (thePageSecurity = 3) Then
                    If theUserLevel = "2" Then
                        Return True
                    Else
                        Return False
                    End If
                ElseIf (thePageSecurity = 4) Then
                    If theUserLogged = "1" Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return True
                End If
            End If
        End Function

        Public Shared Function QuickPaging(ByVal TopicID As Integer, ByVal TopicReplies As Integer, Optional ByVal PageItems As Integer = 15) As String
            Dim ReturnString As String
            Dim PageJumps As Integer
            Dim NumPages As Integer = TopicReplies \ PageItems
            Dim Leftover As Integer = TopicReplies Mod PageItems
            If Leftover > 0 Then
                NumPages += 1
            End If

            If NumPages > 1 Then
                If NumPages >= 5 Then
                    PageJumps = 5
                Else
                    PageJumps = NumPages
                End If

                ReturnString = "<nobr>&nbsp;&nbsp;(&nbsp;<img src=""forumimages/page_icon.gif"">"

                Dim x As Integer
                For x = 1 To PageJumps
                    ReturnString &= "&nbsp;<a href=""./topics.aspx?ID=" & TopicID & "&PAGE=" & x & """>" & x & "</a>"
                Next

                If NumPages > 5 Then
                    ReturnString &= "&nbsp;...&nbsp;<a href=""./topics.aspx?ID=" & TopicID & "&PAGE=" & NumPages & """>Last Page</a>"
                End If

                ReturnString &= "&nbsp;)</nobr>"
            Else
                ReturnString = ""
            End If

            Return ReturnString
        End Function

        Public Shared Function Signature(ByVal ShowSig As Integer, ByVal Sig As String) As String
            If (ShowSig = 1) Then
                Return "<br clear=""all"" /><br />" & FormatString(Sig)
            Else
                Return ""
            End If
        End Function

        Public Shared Function ShowTopicFileUpload(ByVal UploadedFile As String, Optional ByVal FeaturedTopic As Integer = 0) As String
            Dim ReturnString As String = ""

            If (UploadedFile.Length() > 14) Then
                If (FeaturedTopic = 0) Then
                    ReturnString &= "<table border=""0"" width=""90%"" align=""center"" cellpadding=""0"" cellspacing=""0""><tr><td width=""100%""><br /><font size=""" & Settings.TopicsFontSize & """ color=""" & Settings.TopicsFontColor & """><b>Attached File</b></font><br /><table width=""100%"" style=""border-top:2px solid " & Settings.TableBorderColor & ";border-bottom:2px solid " & Settings.TableBorderColor & ";"" cellpadding=""5"" cellspacing=""0""><tr class=""TableRow2""><td width=""100%"" align=""left"">"
                End If

                If ((Right(UploadedFile.ToLower(), 4) = ".jpg") Or (Right(UploadedFile.ToLower(), 4) = ".gif") Or (Right(UploadedFile.ToLower(), 4) = ".png")) Then
                    ReturnString &= "<img src=""topicfiles/" & UploadedFile & """ border=""0"">"
                Else
                    ReturnString &= GetFileIcon(UploadedFile) & "&nbsp;<a href=""./topicfiles/" & UploadedFile & """ target=""_blank"">" & Right(UploadedFile, Len(UploadedFile) - 14) & "</a>"
                End If

                If (FeaturedTopic = 0) Then
                    ReturnString &= "</td></tr></table></td></tr></table>"
                End If
            End If

            Return ReturnString
        End Function

        Public Shared Function GetTitle(ByVal Ranking As Integer, ByVal Posts As Integer, ByVal UseCustomTitle As Integer, ByVal Title As String, Optional ByVal MemberLevel As Integer = 1) As String
            Dim RankName As String = "Member"

            If (Ranking = 0) Then
                Dim RankingReader As OdbcDataReader = Database.Read("SELECT * FROM " & Database.DBPrefix & "_RANKINGS WHERE RANK_POSTS <= " & Posts & " ORDER BY RANK_POSTS DESC, RANK_ID", 1)
                While (RankingReader.Read())
                    RankName = RankingReader("RANK_NAME")
                End While
                RankingReader.Close()
            Else
                Dim RankingReader As OdbcDataReader = Database.Read("SELECT * FROM " & Database.DBPrefix & "_RANKINGS WHERE RANK_ID = " & Ranking & " ORDER BY RANK_ID", 1)
                While (RankingReader.Read())
                    RankName = RankingReader("RANK_NAME")
                End While
                RankingReader.Close()
            End If

            If (MemberLevel = 0) Then
                RankName = "Banned"
            End If

            If (UseCustomTitle = 1) And (MemberLevel <> 0) Then
                Return Title
            Else
                Return RankName
            End If
        End Function

        Public Shared Function AllowCustom(ByVal Ranking As Integer, ByVal Posts As Integer, ByVal Allow As Integer, ByVal Type As String) As Boolean
            Dim TypeString As String
            Dim RankAllowCustom As Integer = 0

            If Type = "CustomTitle" Then
                TypeString = "RANK_ALLOW_TITLE"
            ElseIf Type = "Avatar" Then
                TypeString = "RANK_ALLOW_AVATAR"
            ElseIf Type = "Topics" Then
                TypeString = "RANK_ALLOW_TOPICS"
            ElseIf Type = "Uploads" Then
                TypeString = "RANK_ALLOW_UPLOADS"
            Else
                TypeString = "RANK_ALLOW_AVATAR_CUSTOM"
            End If

            If (Ranking = 0) Then
                Dim RankingReader As OdbcDataReader = Database.Read("SELECT * FROM " & Database.DBPrefix & "_RANKINGS WHERE RANK_POSTS <= " & Posts & " ORDER BY RANK_POSTS DESC, RANK_ID", 1)
                While (RankingReader.Read())
                    RankAllowCustom = RankingReader(TypeString)
                End While
                RankingReader.Close()
            Else
                Dim RankingReader As OdbcDataReader = Database.Read("SELECT * FROM " & Database.DBPrefix & "_RANKINGS WHERE RANK_ID = " & Ranking & " ORDER BY RANK_ID", 1)
                While (RankingReader.Read())
                    RankAllowCustom = RankingReader(TypeString)
                End While
                RankingReader.Close()
            End If

            If (RankAllowCustom = 1) Or (Allow = 1) Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function PosterDetails(ByVal UserID As Integer, ByVal Location As String, ByVal Posts As Object, ByVal JoinDate As Object, ByVal UseCustomAvatar As Integer, ByVal AllowCustomAvatar As Integer, ByVal CustomAvatarLoaded As Integer, ByVal UseCustomTitle As Integer, ByVal AllowCustomTitle As Integer, ByVal Title As String, ByVal AvatarImage As Object, ByVal AvatarImageType As Object, ByVal UseAvatar As Integer, ByVal Photo As String, ByVal MemberLevel As Integer, ByVal MemberRanking As Integer, ByVal ListType As Integer) As String
            Dim ReturnString As String
            Dim RankName As String = "Member"
            Dim RankImage As String = ""
            Dim RankAllowAvatar As String = "0"
            Dim Folder As String = "avatars"

            ReturnString = "<center>"

            If (MemberLevel <> 0) And (MemberLevel <> -1) Then
                If (MemberRanking = 0) Then
                    Dim RankingReader As OdbcDataReader = Database.Read("SELECT * FROM " & Database.DBPrefix & "_RANKINGS WHERE RANK_POSTS <= " & Posts & " ORDER BY RANK_POSTS DESC", 1)
                    While (RankingReader.Read())
                        RankName = RankingReader("RANK_NAME").ToString()
                        RankImage = RankingReader("RANK_IMAGE").ToString()
                        RankAllowAvatar = RankingReader("RANK_ALLOW_AVATAR").ToString()
                    End While
                    RankingReader.Close()
                Else
                    Dim RankingReader As OdbcDataReader = Database.Read("SELECT * FROM " & Database.DBPrefix & "_RANKINGS WHERE RANK_ID = " & MemberRanking & " ORDER BY RANK_ID DESC", 1)
                    While (RankingReader.Read())
                        RankName = RankingReader("RANK_NAME").ToString()
                        RankImage = RankingReader("RANK_IMAGE").ToString()
                        RankAllowAvatar = RankingReader("RANK_ALLOW_AVATAR").ToString()
                    End While
                    RankingReader.Close()
                End If

                If (UseCustomTitle = 1) Then
                    RankName = Title
                End If

                If (UseCustomAvatar = 1) And (CustomAvatarLoaded = 1) Then
                    AvatarImage = UserID & "." & AvatarImageType
                    Folder = "customavatars"
                End If

                If (ListType = 2) And (Photo <> "") And (Photo <> " ") And (Photo <> "None") Then
                    ReturnString &= "<br /><img src=""" & Photo & """><br />"
                End If


                If (RankImage <> "") And (RankImage <> " ") And (RankImage <> "None") Then
                    ReturnString &= "<br /><img src=""rankimages/" & RankImage & """>"
                End If
            Else
                RankName = "Banned"
            End If

            ReturnString &= "<br />" & RankName

            If (RankAllowAvatar = "1") And (UseAvatar = 1) And (MemberLevel <> 0) Then
                ReturnString &= "<br /><br /><img src=""" & Folder & "/" & AvatarImage & """>"
            End If

            ReturnString &= "</center><br />"

            If ListType = 1 Then
                ReturnString &= "Join Date: " & FormatDate(JoinDate, 2) & "<br />Posts: " & Posts

                If (Location <> "") And (Location <> " ") Then
                    ReturnString &= "<br />Location: " & Location
                End If
            End If

            If (MemberLevel = -1) Then
                ReturnString = ""
            End If

            Return ReturnString
        End Function

        Public Shared Function FormatDate(ByVal TheDate As DateTime, Optional ByVal Format As Integer = 1) As String
            Dim LongDateFormat As String = System.Configuration.ConfigurationManager.AppSettings("LongDateFormat")
            Dim ShortDateFormat As String = System.Configuration.ConfigurationManager.AppSettings("ShortDateFormat")
            Dim DateTimeFormat As String = System.Configuration.ConfigurationManager.AppSettings("DateTimeFormat")

            If Format = 1 Then
                Return TheDate.ToString(LongDateFormat)
            ElseIf Format = 2 Then
                Return TheDate.ToString(ShortDateFormat)
            ElseIf Format = 3 Then
                Return TheDate.ToString(DateTimeFormat)
            ElseIf Format = 4 Then
                Return TheDate.ToString("yyyy-MM-dd hh:mm:ss")
            Else
                Return TheDate.ToString(ShortDateFormat)
            End If
        End Function

        Public Shared Sub MessageBox(ByVal Message As String)
            Dim txtBox As String
            txtBox = "<script language=javascript>"
            txtBox &= "alert('" & Message & "');"
            txtBox &= "</script" & ">"
            System.Web.HttpContext.Current.Response.Write(txtBox)
        End Sub

        Public Shared Function GetFileIcon(ByVal FileName As String) As String
            Dim icon As String = "file.png"

            FileName = FileName.ToLower()

            If (Right(FileName, 3) = ".gz") Then
                icon = "gz.png"
            End If
            If (Right(FileName, 3) = ".js") Then
                icon = "js.png"
            End If
            If (Right(FileName, 4) = ".zip") Then
                icon = "zip.png"
            End If
            If (Right(FileName, 4) = ".xml") Then
                icon = "xml.png"
            End If
            If (Right(FileName, 4) = ".xls") Then
                icon = "xls.png"
            End If
            If (Right(FileName, 4) = ".wmv") Then
                icon = "wmv.png"
            End If
            If (Right(FileName, 4) = ".wma") Then
                icon = "wma.png"
            End If
            If (Right(FileName, 4) = ".txt") Then
                icon = "txt.png"
            End If
            If (Right(FileName, 4) = ".wav") Then
                icon = "wav.png"
            End If
            If (Right(FileName, 4) = ".bz2") Then
                icon = "bz2.png"
            End If
            If (Right(FileName, 4) = ".swf") Then
                icon = "swf.png"
            End If
            If (Right(FileName, 4) = ".tar") Then
                icon = "tar.png"
            End If
            If (Right(FileName, 4) = ".tgz") Then
                icon = "tgz.png"
            End If
            If (Right(FileName, 4) = ".mov") Then
                icon = "mov.png"
            End If
            If (Right(FileName, 4) = ".mp3") Then
                icon = "mp3.png"
            End If
            If (Right(FileName, 4) = ".mpg") Then
                icon = "mpg.png"
            End If
            If (Right(FileName, 4) = ".pdf") Then
                icon = "pdf.png"
            End If
            If (Right(FileName, 4) = ".php") Then
                icon = "php.png"
            End If
            If (Right(FileName, 4) = ".png") Then
                icon = "jpg.png"
            End If
            If (Right(FileName, 4) = ".ppt") Then
                icon = "ppt.png"
            End If
            If (Right(FileName, 4) = ".rar") Then
                icon = "rar.png"
            End If
            If (Right(FileName, 4) = ".rtf") Then
                icon = "rtf.png"
            End If
            If (Right(FileName, 4) = ".jpg") Then
                icon = "jpg.png"
            End If
            If (Right(FileName, 4) = ".htm") Then
                icon = "html.png"
            End If
            If (Right(FileName, 4) = ".gif") Then
                icon = "jpg.png"
            End If
            If (Right(FileName, 4) = ".doc") Then
                icon = "doc.png"
            End If
            If (Right(FileName, 4) = ".csv") Then
                icon = "csv.png"
            End If
            If (Right(FileName, 4) = ".css") Then
                icon = "css.png"
            End If
            If (Right(FileName, 5) = ".conf") Then
                icon = "conf.png"
            End If
            If (Right(FileName, 5) = ".html") Then
                icon = "html.png"
            End If
            If (Right(FileName, 5) = ".docx") Then
                icon = "doc.png"
            End If
            If (Right(FileName, 5) = ".xlsx") Then
                icon = "xls.png"
            End If
            If (Right(FileName, 5) = ".pptx") Then
                icon = "ppt.png"
            End If
            If (Right(FileName, 7) = ".tar.gz") Then
                icon = "tgz.png"
            End If

            Dim ReturnString As String = "<img src=""forumimages/icons/" & icon & """ width=""16"" height=""16"">"
            Return ReturnString
        End Function

        Public Shared Function SendEmail(ByVal MailTo As String, ByVal MailFrom As String, ByVal Subject As String, ByVal Message As String) As Integer
            Try
                Dim Mail As MailMessage = New MailMessage()
                Mail.From = New MailAddress(MailFrom)
                Mail.To.Add(MailTo)
                Mail.Subject = Subject
                Mail.Body = Message
                Mail.IsBodyHtml = True
                Dim SMTP As New SmtpClient(Settings.EmailSmtp)
                If (Settings.EmailPort <> "" And Settings.EmailPort <> " ") Then
                    SMTP.Port = Settings.EmailPort
                End If
                If (Settings.EmailUsername <> "" And Settings.EmailUsername <> " ") Then
                    SMTP.Credentials = New System.Net.NetworkCredential(Settings.EmailUsername, Settings.EmailPassword)
                End If
                SMTP.Send(Mail)
                Return 0
            Catch e1 As System.Net.Sockets.SocketException
                ' The SMTP server does not exist or the port is invalid
                Return 1
            Catch e2 As System.Net.Mail.SmtpFailedRecipientException
                ' The SMTP Server is refusing to forward the message
                Return 2
            Catch e3 As SmtpException
                ' Communication with SMTP Server Failed
                Return 3
            Catch e4 As Exception
                ' Some other problem occurred
                Return 4
            End Try
        End Function

        Public Shared Sub SendToSubscribers(ByVal TopicID As Integer)
            If ((Settings.EmailAllowSub = 1) And (Settings.AllowSub = 1)) Then
                Dim TopicSubject As String = ""
                Dim Reader As OdbcDataReader = Database.Read("SELECT M.MEMBER_EMAIL FROM " & Database.DBPrefix & "_SUBSCRIPTIONS S Left Outer Join " & Database.DBPrefix & "_MEMBERS M On S.SUB_MEMBER = M.MEMBER_ID WHERE S.SUB_TOPIC = " & TopicID & " AND S.SUB_EMAIL = 1")
                If Reader.HasRows() Then
                    Dim Reader2 As OdbcDataReader = Database.Read("SELECT TOPIC_SUBJECT FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_ID = " & TopicID)
                    While Reader2.Read()
                        TopicSubject = Reader2("TOPIC_SUBJECT").ToString()
                    End While
                    Reader2.Close()

                    While Reader.Read()
                        Dim FullPath As String = GetFullURLPath()
                        Dim Mailer As Integer = SendEmail(Reader("MEMBER_EMAIL").ToString(), Settings.EmailAddress, Settings.PageTitle & ": Thread Updated", Functions.CustomMessage("EMAIL_SUBSCRIPTION") & "<br /><br />THREAD: " & TopicSubject & "<br /><br /><a href=""./" & FullPath & "/topics.aspx?ID=" & TopicID & """>" & FullPath & "/topics.aspx?ID=" & TopicID & "</a>")
                    End While
                End If
                Reader.Close()
            End If
        End Sub

        Public Shared Sub SendToModerators(ByVal SendType As Integer, ByVal TopicID As Integer, ByVal ForumID As Integer)
            Dim FullPath As String = GetFullURLPath()
            Dim MessageTitle As String = Settings.PageTitle
            Dim MessageText As String = Functions.CustomMessage("EMAIL_CONFIRMPOST") & "<br /><br /><a href=""./" & FullPath & "/topics.aspx?ID=" & TopicID & """>" & FullPath & "/topics.aspx?ID=" & TopicID & "</a>"
            If (SendType = 1) Then
                MessageTitle &= ": New Topic Posted"
            Else
                MessageTitle &= ": New Reply Posted"
            End If
            Dim Reader As OdbcDataReader = Database.Read("SELECT M.MEMBER_EMAIL FROM " & Database.DBPrefix & "_PRIVILEGED P Left Outer Join " & Database.DBPrefix & "_MEMBERS M On P.MEMBER_ID = M.MEMBER_ID WHERE P.PRIVILEGED_ACCESS = 2 AND P.FORUM_ID = " & ForumID)
            While Reader.Read()
                Dim Mailer As Integer = SendEmail(Reader("MEMBER_EMAIL").ToString(), Settings.EmailAddress, MessageTitle, MessageText)
            End While
            Reader.Close()
        End Sub

        Public Shared Function GetFullURLPath() As String
            Dim FullPath As String = System.Web.HttpContext.Current.Request.Url.ToString()
            FullPath = Left(FullPath, Len(FullPath) - InStr(1, StrReverse(FullPath), "/"))
            Return FullPath
        End Function

        Public Shared Function LeftText(ByVal TheString As String, ByVal LeftLength As Integer) As String
            If (LeftLength = 0) Then
                Return TheString
            Else
                If (TheString.Length() < LeftLength) Then
                    Return TheString
                Else
                    Return Left(TheString, LeftLength) & "..."
                End If
            End If
        End Function

        Public Shared Function RepairString(ByVal TheString As String, Optional ByVal Validate As Integer = 1) As String
            Dim ReturnString As String = TheString
            ReturnString = ReturnString.Replace("'", "''")
            If (Validate = 1) Then
                ReturnString = HtmlEncode(ReturnString)
            End If
            Return ReturnString
        End Function

        Public Shared Function Encrypt(ByVal TheString As String) As String
            Dim ClearBytes As Byte() = New UnicodeEncoding().GetBytes(TheString)
            Dim MyHash As HashAlgorithm = CryptoConfig.CreateFromName("MD5")
            Dim HashedBytes As Byte() = MyHash.ComputeHash(ClearBytes)
            Return BitConverter.ToString(HashedBytes)
        End Function

        Public Shared Function GetUniqueKey() As String
            Dim UniqueKey As String = Encrypt(Guid.NewGuid().ToString())
            Return UniqueKey
        End Function

        Public Shared Function FormatURL(ByVal TheString As String) As String
            If (Regex.IsMatch(TheString, "([\w]+?://[^ ,""\s<]*)")) Then
                Return "<a target=""_blank"" href=""./" & TheString & """>" & TheString & "</a>"
            ElseIf (Regex.IsMatch(TheString, "((www|ftp)\.[^ ,""\s<]*)")) Then
                Return "<a target=""_blank"" href=""http://" & TheString & """>" & TheString & "</a>"
            Else
                Return TheString
            End If
        End Function

        Public Shared Function FormatString(ByVal TheString As String) As String
            Dim ReturnString As String = TheString
            ReturnString = ReturnString.Replace(Chr(13), "")
            ReturnString = ReturnString.Replace(Chr(10), "<br />")
            ReturnString = ReturnString.Replace(Chr(10) & Chr(10), "<br /><br />")
            ReturnString = Regex.Replace(ReturnString, "(^|[\n ])([\w]+?://[^ ,""\s<]*)", "$1<a target=""_blank"" href=""./$2"">$2</a>")
            ReturnString = Regex.Replace(ReturnString, "(^|[\n ])((www|ftp)\.[^ ,""\s<]*)", "$1<a target=""_blank"" href=""./http://$2"">$2</a>")
            ReturnString = Regex.Replace(ReturnString, "(^|[\n ])([a-z0-9&\-_.]+?)@([\w\-]+\.([\w\-\.]+\.)*[\w]+)", "$1<a href=""mailto:$2@$3"">$2@$3</a>")
            ReturnString = ForumCode(ReturnString)
            If (Settings.CurseFilter = 1) Then
                ReturnString = ReplaceCurseWords(ReturnString)
            End If
            Return ReturnString
        End Function

        Public Shared Function CurseFilter(ByVal TheString As String) As String
            If (Settings.CurseFilter = 1) Then
                Return ReplaceCurseWords(TheString)
            Else
                Return TheString
            End If
        End Function

        Public Shared Function ReplaceCurseWords(ByVal TheString As String) As String
            Dim ReturnString As String = TheString
            Dim Reader As OdbcDataReader = Database.Read("SELECT CURSE_WORD, CURSE_REPLACEMENT FROM " & Database.DBPrefix & "_CURSE_FILTER ORDER BY CURSE_ID")
            While Reader.Read()
                ReturnString = Regex.Replace(ReturnString, Reader("CURSE_WORD").ToString(), Reader("CURSE_REPLACEMENT").ToString(), RegexOptions.IgnoreCase)
            End While
            Reader.Close()
            Return ReturnString
        End Function

        Public Shared Function ForumCode(ByVal TheString As String) As String
            Dim ReturnString As String = TheString
            ReturnString = Regex.Replace(ReturnString, "(\[b\])((.|\n)*?)(\[\/b\])", "<b>$2</b>")
            ReturnString = Regex.Replace(ReturnString, "(\[u\])((.|\n)*?)(\[\/u\])", "<u>$2</u>")
            ReturnString = Regex.Replace(ReturnString, "(\[i\])((.|\n)*?)(\[\/i\])", "<i>$2</i>")
            ReturnString = Regex.Replace(ReturnString, "(\[marquee\])((.|\n)*?)(\[\/marquee\])", "<marquee>$2</marquee>")
            ReturnString = Regex.Replace(ReturnString, "(\[sup\])((.|\n)*?)(\[\/sup\])", "<sup>$2</sup>")
            ReturnString = Regex.Replace(ReturnString, "(\[sub\])((.|\n)*?)(\[\/sub\])", "<sub>$2</sub>")
            ReturnString = Regex.Replace(ReturnString, "(\[highlight\])((.|\n)*?)(\[\/highlight\])", "<span style=""background-color: #FFFF00"">$2</span>")
            ReturnString = Regex.Replace(ReturnString, "(\[highlight\=)((.|\n)*?)(\])((.|\n)*?)(\[\/highlight\])", "<span style=""background-color: $2"">$5</span>")
            ReturnString = Regex.Replace(ReturnString, "(\[pre\])((.|\n)*?)(\[\/pre\])", "<pre id=""pref""><font size=""2"" id=""pref"">$2</font id=""pref""></pre id=""pref"">")
            ReturnString = Regex.Replace(ReturnString, "(\[hr\])", "<hr />")
            ReturnString = Regex.Replace(ReturnString, "(\[br\])", "<br />")
            ReturnString = Regex.Replace(ReturnString, "(\[s\])((.|\n)*?)(\[\/s\])", "<s>$2</s>")
            ReturnString = Regex.Replace(ReturnString, "(\[font\=)((.|\n)*?)(\])((.|\n)*?)(\[\/font\])", "<font face=""$2"">$5</font id=""$2"">")
            ReturnString = Regex.Replace(ReturnString, "(\[color\=)((.|\n)*?)(\])((.|\n)*?)(\[\/color\])", "<font color=""$2"">$5</font id=""$2"">")
            ReturnString = Regex.Replace(ReturnString, "(\[size\=)((.|\n)*?)(\])((.|\n)*?)(\[\/size\])", "<font size=""$2"">$5</font id=""size$2"">")
            ReturnString = Regex.Replace(ReturnString, "(\[h1\])((.|\n)*?)(\[\/h1\])", "<h1>$2</h1>")
            ReturnString = Regex.Replace(ReturnString, "(\[h2\])((.|\n)*?)(\[\/h2\])", "<h2>$2</h2>")
            ReturnString = Regex.Replace(ReturnString, "(\[h3\])((.|\n)*?)(\[\/h3\])", "<h3>$2</h3>")
            ReturnString = Regex.Replace(ReturnString, "(\[h4\])((.|\n)*?)(\[\/h4\])", "<h4>$2</h4>")
            ReturnString = Regex.Replace(ReturnString, "(\[h5\])((.|\n)*?)(\[\/h5\])", "<h5>$2</h5>")
            ReturnString = Regex.Replace(ReturnString, "(\[h6\])((.|\n)*?)(\[\/h6\])", "<h6>$2</h6>")
            ReturnString = Regex.Replace(ReturnString, "(\[list\])((.|\n)*?)(\[\/list\])", "<ul>$2</ul>")
            ReturnString = Regex.Replace(ReturnString, "(\[list\=numbers)(\])((.|\n)*?)(\[\/list\])", "<ol type=""1"">$3</ol>")
            ReturnString = Regex.Replace(ReturnString, "(\[list\=letters)(\])((.|\n)*?)(\[\/list\])", "<ol type=""A"">$3</ol>")
            ReturnString = Regex.Replace(ReturnString, "(\[\*\])", "<li />")
            ReturnString = Regex.Replace(ReturnString, "(\[left\])((.|\n)*?)(\[\/left\])", "<div align=""left"">$2</div>")
            ReturnString = Regex.Replace(ReturnString, "(\[right\])((.|\n)*?)(\[\/right\])", "<div align=""right"">$2</div>")
            ReturnString = Regex.Replace(ReturnString, "(\[center\])((.|\n)*?)(\[\/center\])", "<center>$2</center>")
            ReturnString = Regex.Replace(ReturnString, "(\[code\])((.|\n)*?)(\[\/code\])", "<div style=""margin:20px; margin-top:5px""><div style=""margin-bottom:2px"">Code:</div><pre style=""margin: 0px; padding: 6px; border: 1px inset; text-align: left;""><font size=""2"" id=""code"">$2</font id=""code""></pre></div>")
            ReturnString = Regex.Replace(ReturnString, "(\[url\])((.|\n)*?)(\[\/url\])", "<a target=""_blank"" href=""./$2"">$2</a>")
            ReturnString = Regex.Replace(ReturnString, "(\[url\=)((.|\n)*?)(\])((.|\n)*?)(\[\/url\])", "<a target=""_blank"" href=""./$2"">$5</a>")
            ReturnString = Regex.Replace(ReturnString, "(\[urlnopop\])((.|\n)*?)(\[\/urlnopop\])", "<a href=""./$2"">$2</a>")
            ReturnString = Regex.Replace(ReturnString, "(\[urlnopop\=)((.|\n)*?)(\])((.|\n)*?)(\[\/urlnopop\])", "<a href=""./$2"">$5</a>")
            ReturnString = Regex.Replace(ReturnString, "\[MemberPhoto\=(?<num>\d+)\]", AddressOf MemberPhoto, System.Text.RegularExpressions.RegexOptions.IgnoreCase)

            Dim exp1 As New Regex("\[quote\]")
            Dim exp2 As New Regex("\[\/quote\]")
            Dim occur1 As Integer = exp1.Matches(ReturnString).Count
            Dim occur2 As Integer = exp2.Matches(ReturnString).Count
            If ((occur1 <> 0) Or (occur2 <> 0)) Then
                If (occur1 = occur2) Then
                    ReturnString = Regex.Replace(ReturnString, "(\[quote\])", "<div style=""margin:20px; margin-top:5px;""><div style=""margin-bottom:2px;"">Quote:</div><div style=""border-top: solid 1px; border-bottom: solid 1px; padding: 5px;"">")
                    ReturnString = Regex.Replace(ReturnString, "(\[\/quote\])", "</div></div>")
                Else
                    ReturnString = Regex.Replace(ReturnString, "(\[quote\])((.|\n)*?)(\[\/quote\])", "<div style=""margin:20px; margin-top:5px;""><div style=""margin-bottom:2px;"">Quote:</div><div style=""border-top: solid 1px; border-bottom: solid 1px; padding: 5px;"">$2</span></div>")
                End If
            End If

            If (Settings.AllowMedia = 1) Then
                ReturnString = Regex.Replace(ReturnString, "(\[)(image|img)(\])((.|\n)*?)(\[\/(image|img)\])", "<img src=""$4"">")
                ReturnString = Regex.Replace(ReturnString, "(\[)(image|img)(\=left\])((.|\n)*?)(\[\/(image|img)\])", "<img align=""left"" src=""$4"">")
                ReturnString = Regex.Replace(ReturnString, "(\[)(image|img)(\=right\])((.|\n)*?)(\[\/(image|img)\])", "<img align=""right"" src=""$4"">")
                ReturnString = Regex.Replace(ReturnString, "(\[)(image|img)(\=center\])((.|\n)*?)(\[\/(image|img)\])", "<center><img src=""$4""></center>")
                ReturnString = Regex.Replace(ReturnString, "(\[flash\])((.|\n)*?)(\[\/flash\])", "<object classid=""clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"" width=""425"" height=""350""><param name=""movie"" value=""$2""><embed src=""$2"" width=""425"" height=""350""></embed></object>")
                ReturnString = Regex.Replace(ReturnString, "(\[flash)( width\=)((.|\n)*?)( height\=)((.|\n)*?)(\])((.|\n)*?)(\[\/flash\])", "<object classid=""clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"" width=""$3"" height=""$6""><param name=""movie"" value=""$9""><embed src=""$9"" width=""$3"" height=""$6""></embed></object>")
                ReturnString = Regex.Replace(ReturnString, "(\[flash)( height\=)((.|\n)*?)( width\=)((.|\n)*?)(\])((.|\n)*?)(\[\/flash\])", "<object classid=""clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"" width=""$6"" height=""$3""><param name=""movie"" value=""$9""><embed src=""$9"" width=""$6"" height=""$3""></embed></object>")
                ReturnString = Regex.Replace(ReturnString, "(\[YouTube\])((.|\n)*?)(youtube.com\/watch\?v\=)((.|\n)*?)(\[\/YouTube\])", "<object width=""640"" height=""385""><param name=""movie"" value=""http://www.youtube.com/v/$5&fs=1&ap=%2526fmt%3D18""></param><param name=""wmode"" value=""transparent""></param><param name=""allowFullScreen"" value=""true""></param><param name=""allowscriptaccess"" value=""always""></param><embed src=""http://www.youtube.com/v/$5&fs=1&ap=%2526fmt%3D18"" type=""application/x-shockwave-flash"" wmode=""transparent"" allowscriptaccess=""always"" allowfullscreen=""true"" width=""640"" height=""385""></embed></object>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                ReturnString = Regex.Replace(ReturnString, "(\[YouTubeLite\])((.|\n)*?)(youtube.com\/watch\?v\=)((.|\n)*?)(\[\/YouTubeLite\])", "<object width=""425"" height=""344""><param name=""movie"" value=""http://www.youtube.com/v/$5&fs=1""></param><param name=""wmode"" value=""transparent""></param><param name=""allowFullScreen"" value=""true""></param><param name=""allowscriptaccess"" value=""always""></param><embed src=""http://www.youtube.com/v/$5&fs=1"" type=""application/x-shockwave-flash"" wmode=""transparent"" allowscriptaccess=""always"" allowfullscreen=""true"" width=""425"" height=""344""></embed></object>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                ReturnString = Regex.Replace(ReturnString, "(\[YouTubeHD\])((.|\n)*?)(youtube.com\/watch\?v\=)((.|\n)*?)(\[\/YouTubeHD\])", "<object width=""855"" height=""480""><param name=""movie"" value=""http://www.youtube.com/v/$5&fs=1&ap=%2526fmt%3D22""></param><param name=""wmode"" value=""transparent""></param><param name=""allowFullScreen"" value=""true""></param><param name=""allowscriptaccess"" value=""always""></param><embed src=""http://www.youtube.com/v/$5&fs=1&ap=%2526fmt%3D22"" type=""application/x-shockwave-flash"" wmode=""transparent"" allowscriptaccess=""always"" allowfullscreen=""true"" width=""855"" height=""480""></embed></object>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            End If
            Return ReturnString
        End Function

        Public Shared Function CustomHTMLVariables(ByVal TheString As String, ByVal TheUserID As String, ByVal TheUserLogged As String, ByVal TheUserLevel As String) As String
            Dim ReturnString As String = TheString
            ReturnString = Regex.Replace(ReturnString, "(\[ContentBox\=)((.|\n)*?)(\])((.|\n)*?)(\[\/ContentBox\])", "<table width=""100%"" align=""center"" class=""ContentBox"" cellpadding=""5"" cellspacing=""0""><tr class=""HeaderCell""><td align=""left""><font size=""" & Settings.HeaderSize & """ color=""" & Settings.HeaderFontColor & """><b>$2</b></font></td></tr><tr class=""TableRow1""><td style=""border-top:1px solid " & Settings.TableBorderColor & ";""><font size=""" & Settings.FontSize & """ color=""" & Settings.TopicsFontColor & """>$5</font></td></tr></table>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[ContentBox\])((.|\n)*?)(\[\/ContentBox\])", "<table width=""100%"" align=""center"" class=""ContentBox"" cellpadding=""5"" cellspacing=""0""><tr class=""TableRow1""><td><font size=""" & Settings.FontSize & """ color=""" & Settings.TopicsFontColor & """>$2</font></td></tr></table>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[Text1\])", UserVariables("TEXT1"), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[Text2\])", UserVariables("TEXT2"), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[Text3\])", UserVariables("TEXT3"), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[Text4\])", UserVariables("TEXT4"), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[Text5\])", UserVariables("TEXT5"), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[Var1\])", UserVariables("VAR1"), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[Var2\])", UserVariables("VAR2"), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[Var3\])", UserVariables("VAR3"), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[Var4\])", UserVariables("VAR4"), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[Var5\])", UserVariables("VAR5"), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[p\])", "<br clear=""all"" /><br />", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[br\])", "<br />", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[pagetitle\])", Settings.PageTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[forumlogo\])", Settings.ForumLogo, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[homeurl\])", Settings.SiteURL, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[fontface\])", Settings.FontFace, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[fontcolor\])", Settings.FontColor, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[headercolor\])", Settings.HeaderColor, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[headerfontcolor\])", Settings.HeaderFontColor, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[bordercolor\])", Settings.TableBorderColor, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[LinkDecoration\])", Settings.LinkDecoration, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[LinkColor\])", Settings.LinkColor, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[ALinkDecoration\])", Settings.ALinkDecoration, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[ALinkColor\])", Settings.ALinkColor, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[VLinkDecoration\])", Settings.VLinkDecoration, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[VLinkColor\])", Settings.VLinkColor, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[HLinkDecoration\])", Settings.HLinkDecoration, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[HLinkColor\])", Settings.HLinkColor, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[ButtonColor\])", Settings.ButtonColor, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[FontSize\])", Settings.FontSize, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[TopicsFontSize\])", Settings.TopicsFontSize, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[TopicsFontColor\])", Settings.TopicsFontColor, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[BackgroundColor\])", Settings.BGColor, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[ScrollbarColor\])", Settings.ScrollbarColor, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[TableBGColor1\])", Settings.TableBGColor1, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[TableBGColor2\])", Settings.TableBGColor2, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[LoginFontColor\])", Settings.LoginFontColor, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[HeaderFontSize\])", Settings.HeaderSize, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[SubHeaderColor\])", Settings.SubHeaderColor, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[SubHeaderFontColor\])", Settings.SubHeaderFontColor, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[FooterColor\])", Settings.FooterColor, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[FooterFontSize\])", Settings.FooterSize, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[FooterFontColor\])", Settings.FooterFontColor, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[Forums\])((.|\n)*?)(\[\/Forums\])", "<a href=""./ForumHome.aspx"">$2</a>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[Forum\=)((.|\n)*?)(\])((.|\n)*?)(\[\/Forum\])", "<a href=""./forums.aspx?ID=$2"">$5</a>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[Page\=)((.|\n)*?)(\])((.|\n)*?)(\[\/Page\])", "<a href=""./page.aspx?ID=$2"">$5</a>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[menu\])", MenuHTML(TheUserID, TheUserLogged, TheUserLevel, 1), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[menu\=H\])", MenuHTML(TheUserID, TheUserLogged, TheUserLevel, 1), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[menu\=V\])", MenuHTML(TheUserID, TheUserLogged, TheUserLevel, 2), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[ForumMenu\])", ForumMenu(TheUserID, TheUserLogged, TheUserLevel, 1), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[ForumMenu\=H\])", ForumMenu(TheUserID, TheUserLogged, TheUserLevel, 1), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[ForumMenu\=V\])", ForumMenu(TheUserID, TheUserLogged, TheUserLevel, 2), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[PageMenu\])", PageMenu(1, 0), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[PageMenu\=H\])", PageMenu(2, 0), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[PageMenu\=V\])", PageMenu(1, 0), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[PageMenu Align\=H\])", PageMenu(2, 0), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[PageMenu Align\=V\])", PageMenu(1, 0), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[PageMenu Parent\=(?<num>\d+)\])", AddressOf PageMenuParentV, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[PageMenu Align\=H Parent\=(?<num>\d+)\])", AddressOf PageMenuParentH, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[PageMenu Align\=V Parent\=(?<num>\d+)\])", AddressOf PageMenuParentV, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[PageMenu Parent\=(?<num>\d+) Align\=H\])", AddressOf PageMenuParentH, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[PageMenu Parent\=(?<num>\d+) Align\=V\])", AddressOf PageMenuParentV, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[CreateTopicLink\])((.|\n)*?)(\[\/CreateTopicLink\])", "<a href=""./newtopic_selectforum.aspx"">$2</a>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            'ReturnString = Regex.Replace(ReturnString, "(\[RegisterLink\])", ShowLink(TheUserID, TheUserLogged, TheUserLevel, 1), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[AdminLink\])", ShowLink(TheUserID, TheUserLogged, TheUserLevel, 2), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[PMLink\])", ShowLink(TheUserID, TheUserLogged, TheUserLevel, 3), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[ImageRotator\=(?<num>\d+)\]", AddressOf ImageRotator, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[LatestTopics Topics\=(?<num>\d+)\](?<title>.+)\[\/LatestTopics\]", AddressOf LatestTopicsNumberTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[LatestTopics ForumID\=(?<forumid>\d+)\](?<title>.+)\[\/LatestTopics\]", AddressOf LatestTopicsForumTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[LatestTopics Topics\=(?<num>\d+)\]", AddressOf LatestTopicsNumber, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[LatestTopics ForumID\=(?<forumid>\d+)\]", AddressOf LatestTopicsForum, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[LatestTopics Topics\=(?<num>\d+) ForumID\=(?<forumid>\d+)\](?<title>.+)\[\/LatestTopics\]", AddressOf LatestTopicsNumberForumTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[LatestTopics ForumID\=(?<forumid>\d+) Topics\=(?<num>\d+)\](?<title>.+)\[\/LatestTopics\]", AddressOf LatestTopicsNumberForumTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[LatestTopics Topics\=(?<num>\d+) ForumID\=(?<forumid>\d+)\]", AddressOf LatestTopicsNumberForum, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[LatestTopics ForumID\=(?<forumid>\d+) Topics\=(?<num>\d+)\]", AddressOf LatestTopicsNumberForum, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[LatestTopics\=(?<num>\d+)\](?<title>.+)\[\/LatestTopics\]", AddressOf LatestTopicsNumberTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[LatestTopics\=(?<num>\d+)\]", AddressOf LatestTopicsNumber, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[LatestTopics\](?<title>.+)\[\/LatestTopics\]", AddressOf LatestTopicsTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[LatestTopics\])", LatestTopics(10, 1, "Latest Topics"), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[LatestTopicsNoBox\])", LatestTopics(10, 0, "Latest Topics"), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[LatestTopicsNoBox\=(?<num>\d+)\]", AddressOf LatestTopicsNumberNoBox, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[LatestTopicsNoBox Topics\=(?<num>\d+)\]", AddressOf LatestTopicsNumberNoBox, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[LatestTopicsNoBox ForumID\=(?<forumid>\d+)\]", AddressOf LatestTopicsForumNoBox, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[LatestTopicsNoBox Topics\=(?<num>\d+) ForumID\=(?<forumid>\d+)\]", AddressOf LatestTopicsNumberForumNoBox, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[LatestTopicsNoBox ForumID\=(?<forumid>\d+) Topics\=(?<num>\d+)\]", AddressOf LatestTopicsNumberForumNoBox, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[LatestBlogs\=(?<num>\d+)\](?<title>.+)\[\/LatestBlogs\]", AddressOf LatestBlogsNumberTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[LatestBlogs\=)(?<num>\d+)(\])", AddressOf LatestBlogsNumber, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[LatestBlogsNoBox\=(?<num>\d+)\]", AddressOf LatestBlogsNumberNoBox, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[LatestBlogs\](?<title>.+)\[\/LatestBlogs\]", AddressOf LatestBlogsTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[LatestBlogs\])", LatestBlogs(10, 1), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[LatestBlogsNoBox\])", LatestBlogs(10, 0), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[NewsTopics\=(?<days>\d+)\](?<title>.+)\[\/NewsTopics\]", AddressOf FeaturedTopicsDaysTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[NewsTopics\=(?<days>\d+)\]", AddressOf FeaturedTopicsDays, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[NewsTopics\](?<title>.+)\[\/NewsTopics\]", AddressOf FeaturedTopicsTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[NewsTopics\])", FeaturedTopics(0, 0, 500), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Days\=(?<days>\d+) Max\=(?<max>\d+) Chars\=(?<chars>\d+)\](?<title>.+)\[\/FeaturedTopics\]", AddressOf FeaturedTopicsMaxDaysCharsTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Max\=(?<max>\d+) Days\=(?<days>\d+) Chars\=(?<chars>\d+)\](?<title>.+)\[\/FeaturedTopics\]", AddressOf FeaturedTopicsMaxDaysCharsTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Days\=(?<days>\d+) Chars\=(?<chars>\d+) Max\=(?<max>\d+)\](?<title>.+)\[\/FeaturedTopics\]", AddressOf FeaturedTopicsMaxDaysCharsTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Max\=(?<max>\d+) Chars\=(?<chars>\d+) Days\=(?<days>\d+)\](?<title>.+)\[\/FeaturedTopics\]", AddressOf FeaturedTopicsMaxDaysCharsTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Chars\=(?<chars>\d+) Days\=(?<days>\d+) Max\=(?<max>\d+)\](?<title>.+)\[\/FeaturedTopics\]", AddressOf FeaturedTopicsMaxDaysCharsTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Chars\=(?<chars>\d+) Max\=(?<max>\d+) Days\=(?<days>\d+)\](?<title>.+)\[\/FeaturedTopics\]", AddressOf FeaturedTopicsMaxDaysCharsTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Days\=(?<days>\d+) Max\=(?<max>\d+)\](?<title>.+)\[\/FeaturedTopics\]", AddressOf FeaturedTopicsMaxDaysTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Max\=(?<max>\d+) Days\=(?<days>\d+)\](?<title>.+)\[\/FeaturedTopics\]", AddressOf FeaturedTopicsMaxDaysTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Chars\=(?<chars>\d+) Max\=(?<max>\d+)\](?<title>.+)\[\/FeaturedTopics\]", AddressOf FeaturedTopicsMaxCharsTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Max\=(?<max>\d+) Chars\=(?<chars>\d+)\](?<title>.+)\[\/FeaturedTopics\]", AddressOf FeaturedTopicsMaxCharsTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Chars\=(?<chars>\d+) Days\=(?<days>\d+)\](?<title>.+)\[\/FeaturedTopics\]", AddressOf FeaturedTopicsDaysCharsTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Days\=(?<days>\d+) Chars\=(?<chars>\d+)\](?<title>.+)\[\/FeaturedTopics\]", AddressOf FeaturedTopicsDaysCharsTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Days\=(?<days>\d+)\](?<title>.+)\[\/FeaturedTopics\]", AddressOf FeaturedTopicsDaysTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Chars\=(?<chars>\d+)\](?<title>.+)\[\/FeaturedTopics\]", AddressOf FeaturedTopicsCharsTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Max\=(?<max>\d+)\](?<title>.+)\[\/FeaturedTopics\]", AddressOf FeaturedTopicsMaxTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Days\=(?<days>\d+) Max\=(?<max>\d+) Chars\=(?<chars>\d+)\]", AddressOf FeaturedTopicsMaxDaysChars, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Max\=(?<max>\d+) Days\=(?<days>\d+) Chars\=(?<chars>\d+)\]", AddressOf FeaturedTopicsMaxDaysChars, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Days\=(?<days>\d+) Chars\=(?<chars>\d+) Max\=(?<max>\d+)\]", AddressOf FeaturedTopicsMaxDaysChars, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Max\=(?<max>\d+) Chars\=(?<chars>\d+) Days\=(?<days>\d+)\]", AddressOf FeaturedTopicsMaxDaysChars, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Chars\=(?<chars>\d+) Days\=(?<days>\d+) Max\=(?<max>\d+)\]", AddressOf FeaturedTopicsMaxDaysChars, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Chars\=(?<chars>\d+) Max\=(?<max>\d+) Days\=(?<days>\d+)\]", AddressOf FeaturedTopicsMaxDaysChars, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Max\=(?<max>\d+) Days\=(?<days>\d+)\]", AddressOf FeaturedTopicsMaxDays, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Days\=(?<days>\d+) Max\=(?<max>\d+)\]", AddressOf FeaturedTopicsMaxDays, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Max\=(?<max>\d+) Chars\=(?<chars>\d+)\]", AddressOf FeaturedTopicsMaxChars, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Chars\=(?<chars>\d+) Max\=(?<max>\d+)\]", AddressOf FeaturedTopicsMaxChars, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Days\=(?<days>\d+) Chars\=(?<chars>\d+)\]", AddressOf FeaturedTopicsDaysChars, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Chars\=(?<chars>\d+) Days\=(?<days>\d+)\]", AddressOf FeaturedTopicsDaysChars, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Max\=(?<max>\d+)\]", AddressOf FeaturedTopicsMax, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Days\=(?<days>\d+)\]", AddressOf FeaturedTopicsDays, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics Chars\=(?<chars>\d+)\]", AddressOf FeaturedTopicsChars, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[FeaturedTopics\](?<title>.+)\[\/FeaturedTopics\]", AddressOf FeaturedTopicsTitle, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "(\[FeaturedTopics\])", FeaturedTopics(0, 0, 500), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[\/LatestBlogs\]", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[\/LatestTopics\]", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[\/NewsTopics\]", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[PhotoGallery Columns\=(?<cols>\d+) ID\=(?<num>\d+)\]", AddressOf PhotoGalleryWithCols, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[PhotoGallery ID\=(?<num>\d+) Columns\=(?<cols>\d+)\]", AddressOf PhotoGalleryWithCols, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\[PhotoGallery\=(?<num>\d+)\]", AddressOf PhotoGalleryNoCols, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = Regex.Replace(ReturnString, "\<form(?<text>(.|\n)*?)\<\/form\>", AddressOf HtmlFormBuilder, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ReturnString = ForumCode(ReturnString)
            Return ReturnString
        End Function

        Public Shared Function HtmlFormBuilder(ByVal m As Match) As String
            Dim FormText As String = (m.Groups("text").Value).ToString()
            FormText = Regex.Replace(FormText, """", "'")
            Dim FormTarget As String = " target='_top'"
            If (FormText.IndexOf("target=") > 0) Then
                FormTarget = ""
            End If
            FormText = "<iframe id=""DMGForm"" src=""htmlform.aspx?TEXT=<form" & FormTarget & FormText & "</form>"" frameborder=""0"" scrolling=""no"" allowtransparency=""true""></iframe>"
            Return FormText
        End Function

        Public Shared Function MemberPhoto(ByVal m As Match) As String
            Dim PhotoID As Integer = CLng((m.Groups("num").Value).ToString())
            Dim ReturnString As String = ""
            If (IsInteger(PhotoID)) Then
                Dim Reader As OdbcDataReader = Database.Read("SELECT PHOTO_ID, PHOTO_EXTENSION FROM " & Database.DBPrefix & "_MEMBER_PHOTOS WHERE PHOTO_ID = " & PhotoID, 1)
                While Reader.Read()
                    ReturnString = "<a href=""javascript:openPhoto('./showphoto.aspx?PHOTO=memberphotos/" & Reader("PHOTO_ID") & "." & Reader("PHOTO_EXTENSION").ToString() & "')""><img src=""memberphotos/" & Reader("PHOTO_ID") & "_s." & Reader("PHOTO_EXTENSION").ToString() & """></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                End While
                Reader.Close()
            End If
            Return ReturnString
        End Function

        Public Shared Function PhotoGalleryNoCols(ByVal m As Match) As String
            Dim GalleryID As Integer = CLng((m.Groups("num").Value).ToString())
            Return PhotoGallery(GalleryID, 5)
        End Function

        Public Shared Function PhotoGalleryWithCols(ByVal m As Match) As String
            Dim GalleryID As Integer = CLng((m.Groups("num").Value).ToString())
            Dim NumCols As Integer = CLng((m.Groups("cols").Value).ToString())
            Return PhotoGallery(GalleryID, NumCols)
        End Function

        Public Shared Function LatestTopicsNumber(ByVal m As Match) As String
            Dim NumTopics As Integer = CLng((m.Groups("num").Value).ToString())
            Return LatestTopics(NumTopics, 1, "Latest Topics")
        End Function

        Public Shared Function LatestTopicsForum(ByVal m As Match) As String
            Dim ForumID As Integer = CLng((m.Groups("forumid").Value).ToString())
            Return LatestTopics(10, 1, "Latest Topics", ForumID)
        End Function

        Public Shared Function LatestTopicsTitle(ByVal m As Match) As String
            Dim BoxTitle As String = (m.Groups("title").Value).ToString()
            Return LatestTopics(10, 1, BoxTitle)
        End Function

        Public Shared Function LatestTopicsNumberTitle(ByVal m As Match) As String
            Dim NumTopics As Integer = CLng((m.Groups("num").Value).ToString())
            Dim BoxTitle As String = (m.Groups("title").Value).ToString()
            Return LatestTopics(NumTopics, 1, BoxTitle)
        End Function

        Public Shared Function LatestTopicsForumTitle(ByVal m As Match) As String
            Dim ForumID As Integer = CLng((m.Groups("forumid").Value).ToString())
            Dim BoxTitle As String = (m.Groups("title").Value).ToString()
            Return LatestTopics(10, 1, BoxTitle, ForumID)
        End Function

        Public Shared Function LatestTopicsNumberForum(ByVal m As Match) As String
            Dim NumTopics As Integer = CLng((m.Groups("num").Value).ToString())
            Dim ForumID As Integer = CLng((m.Groups("forumid").Value).ToString())
            Return LatestTopics(NumTopics, 1, "Latest Topics", ForumID)
        End Function

        Public Shared Function LatestTopicsNumberForumTitle(ByVal m As Match) As String
            Dim NumTopics As Integer = CLng((m.Groups("num").Value).ToString())
            Dim ForumID As Integer = CLng((m.Groups("forumid").Value).ToString())
            Dim BoxTitle As String = (m.Groups("title").Value).ToString()
            Return LatestTopics(NumTopics, 1, BoxTitle, ForumID)
        End Function

        Public Shared Function LatestTopicsNumberNoBox(ByVal m As Match) As String
            Dim NumTopics As Integer = CLng((m.Groups("num").Value).ToString())
            Return LatestTopics(NumTopics, 0, "Latest Topics")
        End Function

        Public Shared Function LatestTopicsForumNoBox(ByVal m As Match) As String
            Dim ForumID As Integer = CLng((m.Groups("forumid").Value).ToString())
            Return LatestTopics(10, 0, "Latest Topics", ForumID)
        End Function

        Public Shared Function LatestTopicsNumberForumNoBox(ByVal m As Match) As String
            Dim NumTopics As Integer = CLng((m.Groups("num").Value).ToString())
            Dim ForumID As Integer = CLng((m.Groups("forumid").Value).ToString())
            Return LatestTopics(NumTopics, 0, "Latest Topics", ForumID)
        End Function

        Public Shared Function LatestTopics(ByVal NumTopics As Integer, ByVal ShowBox As Integer, ByVal BoxTitle As String, Optional ByVal ForumID As Integer = 0) As String
            Dim ReturnString As String = ""

            Dim WhereClause As String = ""
            Dim NewTopicLink As String = "newtopic_selectforum.aspx"
            If (ForumID <> 0) Then
                WhereClause = " F.FORUM_ID = " & ForumID & " AND"
                NewTopicLink = "newtopic.aspx?ID=" & ForumID
            End If

            Dim TheFontColor As String = ""

            If (ShowBox = 1) Then
                ReturnString &= "<table width=""100%"" class=""ContentBox"" cellpadding=""5"" cellspacing=""0"">"
                ReturnString &= "<tr class=""HeaderCell"">"
                ReturnString &= "<td align=""left"">"
                ReturnString &= "<font size=""" & Settings.HeaderSize & """ color=""" & Settings.HeaderFontColor & """><b>"
                ReturnString &= BoxTitle
                ReturnString &= "</b></font></td></tr><tr class=""TableRow1""><td>"
                TheFontColor = " color=""" & Settings.TopicsFontColor & """"
            End If

            Dim count As Integer = 0
            Dim HTReader As OdbcDataReader = Database.Read("SELECT T.TOPIC_ID, T.TOPIC_SUBJECT, T.TOPIC_AUTHOR, T.TOPIC_DATE, T.TOPIC_LASTPOST_AUTHOR, T.TOPIC_LASTPOST_DATE, T.TOPIC_REPLIES, M1.MEMBER_USERNAME as TheAuthor, M2.MEMBER_USERNAME as LastPoster FROM (" & Database.DBPrefix & "_TOPICS T INNER JOIN " & Database.DBPrefix & "_MEMBERS M1 ON T.TOPIC_AUTHOR = M1.MEMBER_ID INNER JOIN " & Database.DBPrefix & "_MEMBERS M2 ON T.TOPIC_LASTPOST_AUTHOR = M2.MEMBER_ID) LEFT OUTER JOIN " & Database.DBPrefix & "_FORUMS F ON T.FORUM_ID = F.FORUM_ID WHERE" & WhereClause & " F.FORUM_TYPE = 0 AND F.FORUM_STATUS <> 0 AND F.FORUM_SHOWLATEST = 1 AND T.TOPIC_STATUS <> 0 AND T.TOPIC_CONFIRMED = 1 ORDER BY T.TOPIC_LASTPOST_DATE DESC", NumTopics)
            While HTReader.Read()
                If count > 0 Then
                    ReturnString &= "<br /><br />"
                End If
                ReturnString &= "<font size=""2""" & TheFontColor & "><b><a href=""./topics.aspx?ID=" & HTReader("TOPIC_ID") & """>" & Left(HTReader("TOPIC_SUBJECT").ToString(), 60)
                If Len(HTReader("TOPIC_SUBJECT").ToString()) > 60 Then
                    ReturnString &= "..."
                End If
                ReturnString &= "</a></b></font><font size=""1""" & TheFontColor & "><br />"
                ReturnString &= "Posted by <a href=""./profile.aspx?ID=" & HTReader("TOPIC_AUTHOR") & """>" & HTReader("TheAuthor").ToString() & "</a>"
                If HTReader("TOPIC_REPLIES") = 0 Then
                    ReturnString &= " on " & FormatDate(HTReader("TOPIC_DATE"), 2) & "."
                Else
                    ReturnString &= ".  Last Replied by <a href=""./profile.aspx?ID=" & HTReader("TOPIC_LASTPOST_AUTHOR") & """>" & HTReader("LastPoster").ToString() & "</a> on " & FormatDate(HTReader("TOPIC_LASTPOST_DATE"), 2) & ".  (" & HTReader("TOPIC_REPLIES").ToString() & " Replies)"
                End If
                ReturnString &= "</font>"
                count = count + 1
            End While
            HTReader.Close()

            If (ShowBox = 1) Then
                ReturnString &= "<center><br /><font size=""2""><a href=""./" & NewTopicLink & """><b>Create New Topic</b></a></font></center>"
                ReturnString &= "</td></tr></table>"
            Else
                ReturnString &= "<br /><br /><li /><font size=""2""><a href=""./" & NewTopicLink & """><b>Create New Topic</b></a></font>"
            End If

            Return ReturnString
        End Function

        Public Shared Function LatestBlogsNumber(ByVal m As Match) As String
            Dim NumBlogs As Integer = CLng((m.Groups("num").Value).ToString())
            Return LatestBlogs(NumBlogs, 1)
        End Function

        Public Shared Function LatestBlogsNumberTitle(ByVal m As Match) As String
            Dim NumBlogs As Integer = CLng((m.Groups("num").Value).ToString())
            Dim BoxTitle As String = (m.Groups("title").Value).ToString()
            Return LatestBlogs(NumBlogs, 1, BoxTitle)
        End Function

        Public Shared Function LatestBlogsTitle(ByVal m As Match) As String
            Dim BoxTitle As String = (m.Groups("title").Value).ToString()
            Return LatestBlogs(10, 1, BoxTitle)
        End Function

        Public Shared Function LatestBlogsNumberNoBox(ByVal m As Match) As String
            Dim NumBlogs As Integer = CLng((m.Groups("num").Value).ToString())
            Return LatestBlogs(NumBlogs, 0)
        End Function

        Public Shared Function LatestBlogs(ByVal NumBlogs As Integer, ByVal ShowBox As Integer, Optional ByVal BoxTitle As String = "Latest Blog Entries") As String
            Dim ReturnString As String = ""

            Dim TheFontColor As String = ""

            If (ShowBox = 1) Then
                ReturnString &= "<table width=""100%"" class=""ContentBox"" cellpadding=""5"" cellspacing=""0"">"
                ReturnString &= "<tr class=""HeaderCell"">"
                ReturnString &= "<td align=""left"">"
                ReturnString &= "<font size=""" & Settings.HeaderSize & """ color=""" & Settings.HeaderFontColor & """><b>"
                ReturnString &= BoxTitle
                ReturnString &= "</b></font></td></tr><tr class=""TableRow1""><td>"
                TheFontColor = " color=""" & Settings.TopicsFontColor & """"
            End If

            Dim count As Integer = 0
            Dim HTReader As OdbcDataReader = Database.Read("SELECT B.BLOG_ID, B.BLOG_AUTHOR, B.BLOG_DATE, B.BLOG_REPLIES, B.BLOG_TITLE, M.MEMBER_USERNAME FROM " & Database.DBPrefix & "_BLOG_TOPICS B Left Outer Join " & Database.DBPrefix & "_MEMBERS M On B.BLOG_AUTHOR = M.MEMBER_ID ORDER BY BLOG_DATE DESC", NumBlogs)
            While HTReader.Read()
                If count > 0 Then
                    ReturnString &= "<br /><br />"
                End If
                ReturnString &= "<font size=""2""" & TheFontColor & "><b><a href=""./blogs.aspx?ID=" & HTReader("BLOG_ID") & """>" & Left(HTReader("BLOG_TITLE").ToString(), 60)
                If Len(HTReader("BLOG_TITLE").ToString()) > 60 Then
                    ReturnString &= "..."
                End If
                ReturnString &= "</a></b></font><font size=""1""" & TheFontColor & "><br />"
                ReturnString &= "Posted by <a href=""./profile.aspx?ID=" & HTReader("BLOG_AUTHOR") & """>" & HTReader("MEMBER_USERNAME").ToString() & "</a> on " & FormatDate(HTReader("BLOG_DATE"), 2) & "."
                If HTReader("BLOG_REPLIES") > 0 Then
                    ReturnString &= "  (" & HTReader("BLOG_REPLIES").ToString() & " Comments)"
                End If
                ReturnString &= "</font>"
                count = count + 1
            End While
            HTReader.Close()

            If (ShowBox = 1) Then
                ReturnString &= "</td></tr></table>"
            End If

            Return ReturnString
        End Function

        Public Shared Function ImageRotator(ByVal m As Match) As String
            Dim RotatorID As String = (m.Groups("num").Value).ToString()
            Dim ReturnString As String = ""

            Dim Reader As OdbcDataReader = Database.Read("SELECT * FROM " & Database.DBPrefix & "_ROTATOR_IMAGES WHERE ROTATOR_ID = " & RotatorID & " ORDER BY NEWID()", 1)
            While Reader.Read()
                If ((Reader("IMAGE_URL").ToString()).Length() > 0) Then
                    ReturnString &= "<a"
                    If (Reader("IMAGE_WINDOW") = 1) Then
                        ReturnString &= " target=""_blank"""
                    End If
                    ReturnString &= " href=""./" & Reader("IMAGE_URL").ToString() & """>"
                End If
                ReturnString &= "<img src=""rotatorimages/" & Reader("IMAGE_ID").ToString() & "." & Reader("IMAGE_EXTENSION").ToString() & """ border=""" & Reader("IMAGE_BORDER").ToString() & """>"
                If ((Reader("IMAGE_DESCRIPTION").ToString()).Length() > 0) Then
                    ReturnString &= "<br />" & Reader("IMAGE_DESCRIPTION").ToString()
                End If
                If ((Reader("IMAGE_URL").ToString()).Length() > 0) Then
                    ReturnString &= "</a>"
                End If
            End While
            Reader.Close()

            Return ReturnString
        End Function

        Public Shared Function PhotoGallery(ByVal GalleryID As Integer, ByVal NumCols As Integer) As String
            Dim ReturnString As String = "<table class=""PhotoGalleryTable"">"
            Dim ColumnWidth As Integer = System.Math.Floor(100.0 / NumCols)
            Dim Count As Integer = 1

            Dim Reader As OdbcDataReader = Database.Read("SELECT * FROM " & Database.DBPrefix & "_GALLERY_PHOTOS WHERE GALLERY_ID = " & GalleryID & " ORDER BY PHOTO_ID")
            While Reader.Read()
                If (Count = 1) Then
                    ReturnString &= "<tr>"
                End If
                ReturnString &= "<td width=""" & ColumnWidth & "%"">"
                ReturnString &= "<a href=""javascript:openPhoto('./showphoto.aspx?PHOTO=photogalleries/" & Reader("PHOTO_ID").ToString() & "." & Reader("PHOTO_EXTENSION").ToString() & "')"">"
                ReturnString &= "<img border=""0"" src=""photogalleries/" & Reader("PHOTO_ID").ToString() & "_s." & Reader("PHOTO_EXTENSION").ToString() & """>"
                If ((Reader("PHOTO_DESCRIPTION").ToString()).Length() > 0) Then
                    ReturnString &= "<br />" & Reader("PHOTO_DESCRIPTION").ToString()
                End If
                ReturnString &= "</a>"
                ReturnString &= "</td>"
                If (Count = NumCols) Then
                    ReturnString &= "</tr>"
                    Count = 1
                Else
                    Count = Count + 1
                End If
            End While
            Reader.Close()

            If (Count <> 1) Then
                ReturnString &= "</tr>"
            End If

            ReturnString &= "</table>"
            Return ReturnString
        End Function

        Public Shared Function UserVariables(ByVal Field As String) As String
            Dim ReturnString As String = ""
            Dim VarReader As OdbcDataReader = Database.Read("SELECT " & Field & " as TheField FROM " & Database.DBPrefix & "_VARIABLES WHERE ID = 1")
            While VarReader.Read()
                ReturnString = VarReader("TheField").ToString()
            End While
            VarReader.Close()
            Return ReturnString
        End Function

        Public Shared Function CustomMessage(ByVal Field As String) As String
            Dim ReturnString As String = ""
            Dim Reader As OdbcDataReader = Database.Read("SELECT " & Field & " as TheField FROM " & Database.DBPrefix & "_CUSTOM_MESSAGES WHERE ID = 1")
            While Reader.Read()
                ReturnString = Reader("TheField").ToString()
            End While
            Reader.Close()
            Return ReturnString
        End Function

        Public Shared Function ShowLink(ByVal mID As String, ByVal mLogged As String, ByVal mLevel As String, ByVal LinkType As Integer) As String
            Dim ReturnString As String = ""
            If (LinkType = 1) Then
                If (mLogged = "1") Then
                    ReturnString = "<a href=""./usercp.aspx?ID=" & mID & """>User CP</a>"
                Else
                    'ReturnString = "<a href=""./register.aspx"">Register</a>"
                End If
            ElseIf (LinkType = 2) Then
                If ((mLogged = "1") And (mLevel = "3")) Then
                    ReturnString = "<a href=""./admin.aspx"">Administration</a>"
                End If
            ElseIf (LinkType = 3) Then
                If (mLogged = "1") Then
                    Dim PMCount As Integer = 0
                    If (Functions.IsInteger(mID)) Then
                        Dim PMReader As OdbcDataReader = Database.Read("SELECT Count(*) as TheCount FROM " & Database.DBPrefix & "_PM_TOPICS WHERE (TOPIC_TO = " & mID & " AND TOPIC_TO_READ = 0) or (TOPIC_FROM = " & mID & " AND TOPIC_FROM_READ = 0)")
                        While PMReader.Read()
                            PMCount = PMReader("TheCount")
                        End While
                        PMReader.Close()
                    End If
                    If PMCount > 0 Then
                        ReturnString = "<a href=""./pm_inbox.aspx""><b>Private Messages (" & PMCount & ")</b></a>"
                    Else
                        ReturnString = "<a href=""./pm_inbox.aspx"">Private Messages</a>"
                    End If
                End If
            End If
            Return ReturnString
        End Function

        Public Shared Function FeaturedTopicsMax(ByVal m As Match) As String
            Dim Max As Integer = CLng((m.Groups("max").Value).ToString())
            Return FeaturedTopics(0, Max, 500)
        End Function

        Public Shared Function FeaturedTopicsDays(ByVal m As Match) As String
            Dim NewsDays As Integer = CLng((m.Groups("days").Value).ToString())
            Return FeaturedTopics(NewsDays, 0, 500)
        End Function

        Public Shared Function FeaturedTopicsChars(ByVal m As Match) As String
            Dim Chars As Integer = CLng((m.Groups("chars").Value).ToString())
            Return FeaturedTopics(0, 0, Chars)
        End Function

        Public Shared Function FeaturedTopicsTitle(ByVal m As Match) As String
            Dim BoxTitle As String = (m.Groups("title").Value).ToString()
            Return FeaturedTopics(0, 0, 500, BoxTitle)
        End Function

        Public Shared Function FeaturedTopicsMaxDays(ByVal m As Match) As String
            Dim Max As Integer = CLng((m.Groups("max").Value).ToString())
            Dim NewsDays As Integer = CLng((m.Groups("days").Value).ToString())
            Return FeaturedTopics(NewsDays, Max, 500)
        End Function

        Public Shared Function FeaturedTopicsMaxChars(ByVal m As Match) As String
            Dim Max As Integer = CLng((m.Groups("max").Value).ToString())
            Dim Chars As Integer = CLng((m.Groups("chars").Value).ToString())
            Return FeaturedTopics(0, Max, Chars)
        End Function

        Public Shared Function FeaturedTopicsMaxTitle(ByVal m As Match) As String
            Dim Max As Integer = CLng((m.Groups("max").Value).ToString())
            Dim BoxTitle As String = (m.Groups("title").Value).ToString()
            Return FeaturedTopics(0, Max, 500, BoxTitle)
        End Function

        Public Shared Function FeaturedTopicsDaysChars(ByVal m As Match) As String
            Dim NewsDays As Integer = CLng((m.Groups("days").Value).ToString())
            Dim Chars As Integer = CLng((m.Groups("chars").Value).ToString())
            Return FeaturedTopics(NewsDays, 0, Chars)
        End Function

        Public Shared Function FeaturedTopicsDaysTitle(ByVal m As Match) As String
            Dim NewsDays As Integer = CLng((m.Groups("days").Value).ToString())
            Dim BoxTitle As String = (m.Groups("title").Value).ToString()
            Return FeaturedTopics(NewsDays, 0, 500, BoxTitle)
        End Function

        Public Shared Function FeaturedTopicsCharsTitle(ByVal m As Match) As String
            Dim Chars As Integer = CLng((m.Groups("chars").Value).ToString())
            Dim BoxTitle As String = (m.Groups("title").Value).ToString()
            Return FeaturedTopics(0, 0, Chars, BoxTitle)
        End Function

        Public Shared Function FeaturedTopicsMaxDaysTitle(ByVal m As Match) As String
            Dim Max As Integer = CLng((m.Groups("max").Value).ToString())
            Dim NewsDays As Integer = CLng((m.Groups("days").Value).ToString())
            Dim BoxTitle As String = (m.Groups("title").Value).ToString()
            Return FeaturedTopics(NewsDays, Max, 500, BoxTitle)
        End Function

        Public Shared Function FeaturedTopicsMaxCharsTitle(ByVal m As Match) As String
            Dim Max As Integer = CLng((m.Groups("max").Value).ToString())
            Dim Chars As Integer = CLng((m.Groups("chars").Value).ToString())
            Dim BoxTitle As String = (m.Groups("title").Value).ToString()
            Return FeaturedTopics(0, Max, Chars, BoxTitle)
        End Function

        Public Shared Function FeaturedTopicsDaysCharsTitle(ByVal m As Match) As String
            Dim NewsDays As Integer = CLng((m.Groups("days").Value).ToString())
            Dim Chars As Integer = CLng((m.Groups("chars").Value).ToString())
            Dim BoxTitle As String = (m.Groups("title").Value).ToString()
            Return FeaturedTopics(NewsDays, 0, Chars, BoxTitle)
        End Function

        Public Shared Function FeaturedTopicsMaxDaysChars(ByVal m As Match) As String
            Dim Max As Integer = CLng((m.Groups("max").Value).ToString())
            Dim NewsDays As Integer = CLng((m.Groups("days").Value).ToString())
            Dim Chars As Integer = CLng((m.Groups("chars").Value).ToString())
            Return FeaturedTopics(NewsDays, Max, Chars)
        End Function

        Public Shared Function FeaturedTopicsMaxDaysCharsTitle(ByVal m As Match) As String
            Dim Max As Integer = CLng((m.Groups("max").Value).ToString())
            Dim NewsDays As Integer = CLng((m.Groups("days").Value).ToString())
            Dim Chars As Integer = CLng((m.Groups("chars").Value).ToString())
            Dim BoxTitle As String = (m.Groups("title").Value).ToString()
            Return FeaturedTopics(NewsDays, Max, Chars, BoxTitle)
        End Function

        Public Shared Function FeaturedTopics(ByVal NewsDays As Integer, ByVal Max As Integer, ByVal Chars As Integer, Optional ByVal BoxTitle As String = "Featured Topics") As String
            Dim Show As Integer = 0
            Dim PageItems As Integer
            If Settings.ItemsPerPage <> 0 Then
                PageItems = Settings.ItemsPerPage
            Else
                PageItems = 15
            End If

            Dim NumPages, Leftover As Integer
            Dim ReturnString As String
            ReturnString = "<!-- Top News -->"

            Dim WhereClause As String
            If (NewsDays = 0) Then
                WhereClause = "(T.TOPIC_NEWS = 1) and (T.TOPIC_STATUS <> 0)"
            Else
                WhereClause = "(T.TOPIC_NEWS = 1) and (T.TOPIC_STATUS <> 0) and (" & Database.GetDateDiff("d", "T.TOPIC_DATE", Database.GetTimeStamp()) & " <= " & NewsDays & ")"
            End If

            Dim NewsReader As OdbcDataReader = Database.Read("SELECT T.TOPIC_ID, T.TOPIC_SUBJECT, T.TOPIC_MESSAGE, T.TOPIC_AUTHOR, T.TOPIC_DATE, T.TOPIC_REPLIES, T.TOPIC_FILEUPLOAD, M.MEMBER_USERNAME FROM " & Database.DBPrefix & "_TOPICS T Left Outer Join " & Database.DBPrefix & "_MEMBERS M On T.TOPIC_AUTHOR = M.MEMBER_ID WHERE " & WhereClause & " ORDER BY T.TOPIC_DATE DESC", Max)
            If NewsReader.HasRows() Then
                ReturnString &= "<table width=""100%"" class=""ContentBox"" cellpadding=""5"" cellspacing=""0"">"
                ReturnString &= "<tr class=""HeaderCell"">"
                ReturnString &= "<td align=""left"" colspan=""2"">"
                ReturnString &= "<font size=""" & Settings.HeaderSize & """ color=""" & Settings.HeaderFontColor & """><b>"
                ReturnString &= BoxTitle
                ReturnString &= "</b></font></td><td align=""right"">"
                If (Settings.RSSFeeds = 1) Then
                    ReturnString &= "<a target=""_blank"" href=""./rssfeed.aspx?ID=news""><img src=""forumimages/rss.gif"" border=""0""></a>"
                End If
                ReturnString &= "</td></tr>"
                Show = 1
            End If
            While NewsReader.Read()
                NumPages = NewsReader("TOPIC_REPLIES") \ PageItems
                Leftover = NewsReader("TOPIC_REPLIES") Mod PageItems
                If Leftover > 0 Then
                    NumPages += 1
                End If
                ReturnString &= "<tr class=""SubHeaderCell""><td align=""left"" valign=""middle"" nowrap><font size=""2"" color=""" & Settings.SubHeaderFontColor & """><b>"
                ReturnString &= "<a href=""./topics.aspx?ID=" & NewsReader("TOPIC_ID") & """>" & NewsReader("TOPIC_SUBJECT").ToString() & "</a></b>" & QuickPaging(NewsReader("TOPIC_ID"), NewsReader("TOPIC_REPLIES"), Settings.ItemsPerPage) & "</font></td>"
                ReturnString &= "<td width=""100%"" align=""center"" valign=""middle"" nowrap><font size=""1"" color=""" & Settings.SubHeaderFontColor & """>Posted By <a href=""./profile.aspx?ID=" & NewsReader("TOPIC_AUTHOR") & """>" & NewsReader("MEMBER_USERNAME").ToString() & "</a></font></td>"
                ReturnString &= "<td align=""center"" valign=""middle"" nowrap><font size=""1"" color=""" & Settings.SubHeaderFontColor & """>" & FormatDate(NewsReader("TOPIC_DATE"), 1) & "<br />" & FormatDate(NewsReader("TOPIC_DATE"), 2) & "</font></td></tr>"
                ReturnString &= "<tr class=""TableRow1""><td width=""100%"" colspan=""3""><font size=""2"" color=""" & Settings.TopicsFontColor & """>"
                Dim HasImageUpload As Boolean = "false"
                If ((Right(NewsReader("TOPIC_FILEUPLOAD").ToString(), 4) = ".jpg") Or (Right(NewsReader("TOPIC_FILEUPLOAD").ToString(), 4) = ".gif") Or (Right(NewsReader("TOPIC_FILEUPLOAD").ToString(), 4) = ".png")) Then
                    HasImageUpload = "true"
                    ReturnString &= ShowTopicFileUpload(NewsReader("TOPIC_FILEUPLOAD").ToString(), 1)
                End If
                ReturnString &= FormatString(LeftText(NewsReader("TOPIC_MESSAGE").ToString(), Chars))
                ReturnString &= "</font></td></tr>"
                ReturnString &= "<tr class=""TableRow1""><td colspan=""3"" align=""left""><font size=""2"" color=""" & Settings.TopicsFontColor & """>"
                ReturnString &= "<img src=""forumimages/latestcomments.gif"">&nbsp;<a href=""./topics.aspx?ID=" & NewsReader("TOPIC_ID") & "&PAGE=" & NumPages & """>Read The Latest Comments (" & NewsReader("TOPIC_REPLIES") & ")</a>"
                If ((Not HasImageUpload) And (NewsReader("TOPIC_FILEUPLOAD").ToString() <> "")) Then
                    ReturnString &= "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & ShowTopicFileUpload(NewsReader("TOPIC_FILEUPLOAD").ToString(), 1)
                End If
                ReturnString &= "</td></tr>"
            End While
            NewsReader.Close()

            If Show = 1 Then
                ReturnString &= "</table>"
            End If
            Return ReturnString
        End Function

        Public Shared Function MenuHTML(ByVal mID As String, ByVal mLogged As String, ByVal mLevel As String, ByVal MenuType As Integer) As String
            Dim ReturnString As String = ""
            Dim Count As Integer = 0
            Dim NewWindow As String = ""
            Dim Sep As String = ""
            Dim Separator As String = " "
            If (MenuType = 1) Then
                Separator = Settings.HorizDivide
            Else
                Separator = Settings.VertDivide
            End If

            Dim Reader As OdbcDataReader = Database.Read("SELECT * FROM " & Database.DBPrefix & "_MAIN_MENU ORDER BY LINK_ORDER")
            While Reader.Read()
                If (Count > 0) Then
                    Sep = Separator
                Else
                    Sep = ""
                End If

                If (Reader("LINK_WINDOW") = 1) Then
                    NewWindow = " target=""_blank"""
                Else
                    NewWindow = ""
                End If

                If (Reader("LINK_TYPE") = 1) Then
                    ReturnString &= Sep & "<a" & NewWindow & " href=""./default.aspx"">" & Reader("LINK_TEXT").ToString() & "</a>"
                ElseIf (Reader("LINK_TYPE") = 2) Then
                    ReturnString &= Sep & "<a" & NewWindow & " href=""./page.aspx?ID=" & Reader("LINK_PARAMETER").ToString() & """>" & Reader("LINK_TEXT").ToString() & "</a>"
                ElseIf (Reader("LINK_TYPE") = 3) Then
                    ReturnString &= Sep & "<a" & NewWindow & " href=""./forumhome.aspx"">" & Reader("LINK_TEXT").ToString() & "</a>"
                ElseIf (Reader("LINK_TYPE") = 4) Then
                    If ((mLogged = "0") And (Settings.AllowRegistration = 1)) Then
                        'ReturnString &= Sep & "<a" & NewWindow & " href=""./register.aspx"">" & Reader("LINK_TEXT").ToString() & "</a>"
                    End If
                ElseIf (Reader("LINK_TYPE") = 5) Then
                    ReturnString &= Sep & "<a" & NewWindow & " href=""./active.aspx"">" & Reader("LINK_TEXT").ToString() & "</a>"
                ElseIf (Reader("LINK_TYPE") = 6) Then
                    If ((mLogged = "1") Or (Settings.HideMembers = 0)) Then
                        ReturnString &= Sep & "<a" & NewWindow & " href=""./members.aspx"">" & Reader("LINK_TEXT").ToString() & "</a>"
                    End If
                ElseIf (Reader("LINK_TYPE") = 7) Then
                    ReturnString &= Sep & "<a" & NewWindow & " href=""./search.aspx"">" & Reader("LINK_TEXT").ToString() & "</a>"
                ElseIf (Reader("LINK_TYPE") = 8) Then
                    ReturnString &= Sep & "<a" & NewWindow & " href=""./forumhome.aspx?ID=" & Reader("LINK_PARAMETER").ToString() & """>" & Reader("LINK_TEXT").ToString() & "</a>"
                ElseIf (Reader("LINK_TYPE") = 9) Then
                    ReturnString &= Sep & "<a" & NewWindow & " href=""./forums.aspx?ID=" & Reader("LINK_PARAMETER").ToString() & """>" & Reader("LINK_TEXT").ToString() & "</a>"
                ElseIf (Reader("LINK_TYPE") = 10) Then
                    If mLogged = "1" Then
                        ReturnString &= Sep & "<a" & NewWindow & " href=""./usercp.aspx?ID=" & mID & """>" & Reader("LINK_TEXT").ToString() & "</a>"
                    End If
                ElseIf (Reader("LINK_TYPE") = 11) Then
                    If mLogged = "1" Then
                        Dim PMCount As Integer = 0
                        If (Functions.IsInteger(mID)) Then
                            Dim PMReader As OdbcDataReader = Database.Read("SELECT Count(*) as TheCount FROM " & Database.DBPrefix & "_PM_TOPICS WHERE (TOPIC_TO = " & mID & " AND TOPIC_TO_READ = 0) or (TOPIC_FROM = " & mID & " AND TOPIC_FROM_READ = 0)")
                            While PMReader.Read()
                                PMCount = PMReader("TheCount")
                            End While
                            PMReader.Close()
                        End If
                        If (PMCount > 0) Then
                            ReturnString &= Sep & "<a" & NewWindow & " href=""./pm_inbox.aspx""><b>" & Reader("LINK_TEXT").ToString() & " (" & PMCount & ")</b></a>"
                        Else
                            ReturnString &= Sep & "<a" & NewWindow & " href=""./pm_inbox.aspx"">" & Reader("LINK_TEXT").ToString() & "</a>"
                        End If
                    End If
                ElseIf (Reader("LINK_TYPE") = 12) Then
                    If mLogged = "1" Then
                        ReturnString &= Sep & "<a" & NewWindow & " href=""./editprofile.aspx?ID=" & mID & """>" & Reader("LINK_TEXT").ToString() & "</a>"
                    End If
                ElseIf (Reader("LINK_TYPE") = 13) Then

                    If mLevel = "3" Then
                        ReturnString &= Sep & "<a" & NewWindow & " href=""./admin.aspx"">" & Reader("LINK_TEXT").ToString() & "</a>"
                    End If
                ElseIf (Reader("LINK_TYPE") = 14) Then
                    ReturnString &= Sep & "<a" & NewWindow & " href=""./" & Reader("LINK_PARAMETER").ToString() & """>" & Reader("LINK_TEXT").ToString() & "</a>"
                ElseIf (Reader("LINK_TYPE") = 15) Then
                    ReturnString &= Sep & "<a" & NewWindow & " href=""./login.aspx"">" & Reader("LINK_TEXT").ToString() & "</a>"
                End If

                Count = Count + 1
            End While
            Reader.Close()
            Return ReturnString
        End Function

        Public Shared Function ForumMenu(ByVal mID As String, ByVal mLogged As String, ByVal mLevel As String, ByVal MenuType As Integer) As String
            Dim Separator As String = " "
            If (MenuType = 1) Then
                Separator = Settings.HorizDivide
            Else
                Separator = Settings.VertDivide
            End If

            Dim PMCount As Integer = 0
            If (Functions.IsInteger(mID)) Then
                Dim PMReader As OdbcDataReader = Database.Read("SELECT Count(*) as TheCount FROM " & Database.DBPrefix & "_PM_TOPICS WHERE (TOPIC_TO = " & mID & " AND TOPIC_TO_READ = 0) or (TOPIC_FROM = " & mID & " AND TOPIC_FROM_READ = 0)")
                While PMReader.Read()
                    PMCount = PMReader("TheCount")
                End While
                PMReader.Close()
            End If

            Dim ReturnString As String = ""

            If (Settings.ForumsDefault <> 1) Then
                ReturnString &= "<a href=""./default.aspx"">Main Page</a>" & Separator
            End If
            ReturnString &= "<a href=""./ForumHome.aspx"">Forums</a>"
            If mLogged = "0" Then
                'ReturnString &= Separator & "<a href=""./register.aspx"">Register</a>"
            End If
            ReturnString &= Separator & "<a href=""./active.aspx"">Active Topics</a>"
            ReturnString &= Separator & "<a href=""./members.aspx"">Members</a>"
            ReturnString &= Separator & "<a href=""./search.aspx"">Search</a>"
            If mLogged = "1" Then
                ReturnString &= Separator & "<a href=""./usercp.aspx?ID=" & mID & """>User CP</a>"
                If PMCount > 0 Then
                    ReturnString &= Separator & "<a href=""./pm_inbox.aspx""><b>Private Messages (" & PMCount & ")</b></a>"
                Else
                    ReturnString &= Separator & "<a href=""./pm_inbox.aspx"">Private Messages</a>"
                End If
            End If
            If mLevel = "3" Then
                ReturnString &= Separator & "<a href=""./admin.aspx"">Administration</a>"
            End If

            Return ReturnString
        End Function

        Public Shared Function PageMenuParentV(ByVal m As Match) As String
            Dim ParentID As Integer = CLng((m.Groups("num").Value).ToString())
            Return PageMenu(1, ParentID)
        End Function

        Public Shared Function PageMenuParentH(ByVal m As Match) As String
            Dim ParentID As Integer = CLng((m.Groups("num").Value).ToString())
            Return PageMenu(2, ParentID)
        End Function

        Public Shared Function PageMenu(ByVal MenuType As Integer, ByVal ParentID As Integer) As String
            Dim ReturnString As String = ""
            Dim Count As Integer = 0
            Dim MenuReader As OdbcDataReader = Database.Read("SELECT PAGE_ID, PAGE_NAME FROM " & Database.DBPrefix & "_PAGES WHERE PAGE_STATUS <> 0 AND PAGE_PARENT = " & ParentID & " ORDER BY PAGE_SORT ASC, PAGE_NAME")
            While MenuReader.Read()
                If (MenuType = 1) Then
                    If (Count > 0) Then
                        ReturnString &= "<br /><br />"
                    Else
                        Count += 1
                    End If
                    If MenuReader("PAGE_ID") = 1 Then
                        ReturnString &= "<a href=""./default.aspx"">" & MenuReader("PAGE_NAME") & "</a>"
                    Else
                        ReturnString &= "<a href=""./page.aspx?ID=" & MenuReader("PAGE_ID") & """>" & MenuReader("PAGE_NAME") & "</a>"
                    End If
                Else
                    If (Count > 0) Then
                        ReturnString &= "&nbsp;&nbsp;<b>|</b>&nbsp;&nbsp;"
                    Else
                        Count += 1
                    End If
                    If MenuReader("PAGE_ID") = 1 Then
                        ReturnString &= "<a href=""./default.aspx"">" & MenuReader("PAGE_NAME") & "</a>"
                    Else
                        ReturnString &= "<a href=""./page.aspx?ID=" & MenuReader("PAGE_ID") & """>" & MenuReader("PAGE_NAME") & "</a>"
                    End If
                End If
            End While
            MenuReader.Close()
            If ((System.Web.HttpContext.Current.Session("UserLevel") = "3") And (ParentID = 0)) Then
                If (MenuType = 1) Then
                    ReturnString &= "<br /><br />"
                Else
                    ReturnString &= "&nbsp;&nbsp;<b>|</b>&nbsp;&nbsp;"
                End If
                ReturnString &= "<a href=""./admin.aspx"">Admin</a>"
            End If
            Return ReturnString
        End Function

        Public Shared Sub UpdateCounts(ByVal UpdateType As Integer, ByVal Param1 As Integer, ByVal Param2 As Integer, ByVal Param3 As Integer)
            'UpdateType: 1 = normal
            'UpdateType: 2 = topic confirmed, Param1 = ForumID
            'UpdateType: 3 = reply confirmed, Param1 = ForumID
            'UpdateType: 4 = topic moved, Param1 = Old Forum, Param2 = New Forum, Param3 = TopicID
            'UpdateType: 5 = reply deleted, Param1 = ForumID

            Dim Reader As OdbcDataReader

            If (UpdateType = 2) Then
                Reader = Database.Read("SELECT TOPIC_LASTPOST_AUTHOR, TOPIC_LASTPOST_DATE FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_STATUS <> 0 and TOPIC_CONFIRMED <> 0 and FORUM_ID = " & Param1 & " ORDER BY TOPIC_LASTPOST_DATE DESC", 1)
                While Reader.Read()
                    Database.Write("UPDATE " & Database.DBPrefix & "_FORUMS SET FORUM_TOPICS = (FORUM_TOPICS+1), FORUM_POSTS = (FORUM_POSTS+1), FORUM_LASTPOST_AUTHOR = " & Reader("TOPIC_LASTPOST_AUTHOR") & ", FORUM_LASTPOST_DATE = '" & Reader("TOPIC_LASTPOST_DATE") & "' WHERE FORUM_ID = " & Param1)
                End While
                Reader.Close()
            ElseIf (UpdateType = 3) Then
                Reader = Database.Read("SELECT TOPIC_LASTPOST_AUTHOR, TOPIC_LASTPOST_DATE FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_STATUS <> 0 and TOPIC_CONFIRMED <> 0 and FORUM_ID = " & Param1 & " ORDER BY TOPIC_LASTPOST_DATE DESC", 1)
                While Reader.Read()
                    Database.Write("UPDATE " & Database.DBPrefix & "_FORUMS SET FORUM_POSTS = (FORUM_POSTS+1), FORUM_LASTPOST_AUTHOR = " & Reader("TOPIC_LASTPOST_AUTHOR") & ", FORUM_LASTPOST_DATE = '" & Reader("TOPIC_LASTPOST_DATE") & "' WHERE FORUM_ID = " & Param1)
                End While
                Reader.Close()
            ElseIf (UpdateType = 4) Then
                Dim TopicReplies As Integer = 0
                Reader = Database.Read("SELECT TOPIC_REPLIES FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_ID = " & Param3)
                While Reader.Read()
                    TopicReplies = Reader("TOPIC_REPLIES")
                End While
                Reader.Close()

                Reader = Database.Read("SELECT TOPIC_LASTPOST_AUTHOR, TOPIC_LASTPOST_DATE FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_STATUS <> 0 and TOPIC_CONFIRMED <> 0 and FORUM_ID = " & Param1 & " ORDER BY TOPIC_LASTPOST_DATE DESC", 1)
                If Reader.HasRows() Then
                    While Reader.Read()
                        Database.Write("UPDATE " & Database.DBPrefix & "_FORUMS SET FORUM_TOPICS = (FORUM_TOPICS-1), FORUM_POSTS = (FORUM_POSTS - " & TopicReplies & " - 1), FORUM_LASTPOST_AUTHOR = " & Reader("TOPIC_LASTPOST_AUTHOR") & ", FORUM_LASTPOST_DATE = '" & Reader("TOPIC_LASTPOST_DATE") & "' WHERE FORUM_ID = " & Param1)
                    End While
                Else
                    Database.Write("UPDATE " & Database.DBPrefix & "_FORUMS SET FORUM_TOPICS = 0, FORUM_POSTS = 0, FORUM_LASTPOST_AUTHOR = 0 WHERE FORUM_ID = " & Param1)
                End If
                Reader.Close()

                Reader = Database.Read("SELECT TOPIC_LASTPOST_AUTHOR, TOPIC_LASTPOST_DATE FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_STATUS <> 0 and TOPIC_CONFIRMED <> 0 and FORUM_ID = " & Param2 & " ORDER BY TOPIC_LASTPOST_DATE DESC", 1)
                While Reader.Read()
                    Database.Write("UPDATE " & Database.DBPrefix & "_FORUMS SET FORUM_TOPICS = (FORUM_TOPICS+1), FORUM_POSTS = (FORUM_POSTS + " & TopicReplies & " + 1), FORUM_LASTPOST_AUTHOR = " & Reader("TOPIC_LASTPOST_AUTHOR") & ", FORUM_LASTPOST_DATE = '" & Reader("TOPIC_LASTPOST_DATE") & "' WHERE FORUM_ID = " & Param2)
                End While
                Reader.Close()
            ElseIf (UpdateType = 5) Then
                Reader = Database.Read("SELECT TOPIC_LASTPOST_AUTHOR, TOPIC_LASTPOST_DATE FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_STATUS <> 0 and TOPIC_CONFIRMED <> 0 and FORUM_ID = " & Param1 & " ORDER BY TOPIC_LASTPOST_DATE DESC", 1)
                While Reader.Read()
                    Database.Write("UPDATE " & Database.DBPrefix & "_FORUMS SET FORUM_POSTS = (FORUM_POSTS-1), FORUM_LASTPOST_AUTHOR = " & Reader("TOPIC_LASTPOST_AUTHOR") & ", FORUM_LASTPOST_DATE = '" & Reader("TOPIC_LASTPOST_DATE") & "' WHERE FORUM_ID = " & Param1)
                End While
                Reader.Close()
            Else
                Dim TopicsCount As OdbcDataReader = Database.Read("SELECT TOPIC_ID FROM " & Database.DBPrefix & "_TOPICS")
                While TopicsCount.Read()
                    Database.Write("UPDATE " & Database.DBPrefix & "_TOPICS SET TOPIC_REPLIES = (SELECT COUNT(*) as RCounter FROM " & Database.DBPrefix & "_REPLIES WHERE TOPIC_ID = " & TopicsCount("TOPIC_ID") & " and REPLY_CONFIRMED = 1) WHERE TOPIC_ID = " & TopicsCount("TOPIC_ID"))
                End While
                TopicsCount.Close()

                Dim ForumsCount As OdbcDataReader = Database.Read("SELECT FORUM_ID FROM " & Database.DBPrefix & "_FORUMS")
                While ForumsCount.Read()
                    If (Database.DBType = "MySQL") Then
                        Database.Write("UPDATE " & Database.DBPrefix & "_FORUMS SET FORUM_LASTPOST_AUTHOR = (SELECT TOPIC_LASTPOST_AUTHOR FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_STATUS <> 0 and TOPIC_CONFIRMED <> 0 and FORUM_ID = " & ForumsCount("FORUM_ID") & " ORDER BY TOPIC_LASTPOST_DATE DESC LIMIT 1), FORUM_LASTPOST_DATE = (SELECT TOPIC_LASTPOST_DATE FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_STATUS <> 0 and TOPIC_CONFIRMED <> 0 and FORUM_ID = " & ForumsCount("FORUM_ID") & " ORDER BY TOPIC_LASTPOST_DATE DESC LIMIT 1), FORUM_TOPICS = (SELECT COUNT(*) as TCounter FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_STATUS <> 0 and TOPIC_CONFIRMED <> 0 and FORUM_ID = " & ForumsCount("FORUM_ID") & "), FORUM_POSTS = ((SELECT COUNT(*) as PCounter1 FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_STATUS <> 0 and TOPIC_CONFIRMED <> 0 and FORUM_ID = " & ForumsCount("FORUM_ID") & ") + (SELECT COUNT(*) as PCounter2 FROM " & Database.DBPrefix & "_REPLIES R Left Outer Join " & Database.DBPrefix & "_TOPICS T On R.TOPIC_ID = T.TOPIC_ID WHERE R.REPLY_CONFIRMED <> 0 and T.TOPIC_STATUS <> 0 and T.TOPIC_CONFIRMED <> 0 and T.FORUM_ID = " & ForumsCount("FORUM_ID") & ")) WHERE FORUM_ID = " & ForumsCount("FORUM_ID"))
                    Else
                        Database.Write("UPDATE " & Database.DBPrefix & "_FORUMS SET FORUM_LASTPOST_AUTHOR = (SELECT TOP 1 TOPIC_LASTPOST_AUTHOR FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_STATUS <> 0 and TOPIC_CONFIRMED <> 0 and FORUM_ID = " & ForumsCount("FORUM_ID") & " ORDER BY TOPIC_LASTPOST_DATE DESC), FORUM_LASTPOST_DATE = (SELECT TOP 1 TOPIC_LASTPOST_DATE FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_STATUS <> 0 and TOPIC_CONFIRMED <> 0 and FORUM_ID = " & ForumsCount("FORUM_ID") & " ORDER BY TOPIC_LASTPOST_DATE DESC), FORUM_TOPICS = (SELECT COUNT(*) as TCounter FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_STATUS <> 0 and TOPIC_CONFIRMED <> 0 and FORUM_ID = " & ForumsCount("FORUM_ID") & "), FORUM_POSTS = ((SELECT COUNT(*) as PCounter1 FROM " & Database.DBPrefix & "_TOPICS WHERE TOPIC_STATUS <> 0 and TOPIC_CONFIRMED <> 0 and FORUM_ID = " & ForumsCount("FORUM_ID") & ") + (SELECT COUNT(*) as PCounter2 FROM " & Database.DBPrefix & "_REPLIES R Left Outer Join " & Database.DBPrefix & "_TOPICS T On R.TOPIC_ID = T.TOPIC_ID WHERE R.REPLY_CONFIRMED <> 0 and T.TOPIC_STATUS <> 0 and T.TOPIC_CONFIRMED <> 0 and T.FORUM_ID = " & ForumsCount("FORUM_ID") & ")) WHERE FORUM_ID = " & ForumsCount("FORUM_ID"))
                    End If
                End While
                ForumsCount.Close()
            End If
        End Sub
    End Class

End Namespace
