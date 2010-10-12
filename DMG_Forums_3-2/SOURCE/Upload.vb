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
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Data.Odbc
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Math
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports Microsoft.VisualBasic
Imports DMGForums.Global

Namespace DMGForums.Upload

	'---------------------------------------------------------------------------------------------------
	' UploadPage - Codebehind For upload.aspx
	'---------------------------------------------------------------------------------------------------
	Public Class UploadPage
		Inherits System.Web.UI.Page

		Public file As System.Web.UI.HtmlControls.HtmlInputFile
		Public UploadType As System.Web.UI.WebControls.TextBox
		Public RankingID As System.Web.UI.WebControls.TextBox
		Public RotatorID As System.Web.UI.WebControls.TextBox
		Public GalleryID As System.Web.UI.WebControls.TextBox
		Public AvatarName As System.Web.UI.WebControls.TextBox
		Public PhotoDesc As System.Web.UI.WebControls.TextBox
		Public ImageURL As System.Web.UI.WebControls.TextBox
		Public ImageDescription As System.Web.UI.WebControls.TextBox
		Public ImageBorder As System.Web.UI.WebControls.TextBox
		Public ImageWindow As System.Web.UI.WebControls.DropDownList
		Public InsertToPage As System.Web.UI.WebControls.Button
		Public UploadForm As System.Web.UI.WebControls.PlaceHolder
		Public Message As System.Web.UI.WebControls.Label
		Dim ImageName as String = "0"

		Public AllowTheCustomAvatar, AllowMemberPhoto as Boolean

		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
			if (Session("UserLevel") = "3") then
				AllowTheCustomAvatar = true
				AllowMemberPhoto = true
			else
				Dim ProfileReader as OdbcDataReader = Database.Read("SELECT MEMBER_RANKING, MEMBER_POSTS, MEMBER_AVATAR_ALLOWCUSTOM FROM " & Database.DBPrefix & "_MEMBERS WHERE MEMBER_ID = " & Session("UserID"))
					While(ProfileReader.Read())
						AllowTheCustomAvatar = Functions.AllowCustom(ProfileReader("MEMBER_RANKING"), ProfileReader("MEMBER_POSTS"), ProfileReader("MEMBER_AVATAR_ALLOWCUSTOM"), "CustomAvatar")
					End While
				ProfileReader.Close()

				if ((Settings.MemberPhotoSize > 0) and (Session("UserID") = Request.Querystring("ID"))) then
					AllowMemberPhoto = true
				else
					AllowMemberPhoto = false
				end if
			end if

			if Page.IsPostBack then
				Select Case UploadType.text
					Case "avatar"
						UploadAvatar(file.PostedFile)
					Case "customavatar"
						UploadCustomAvatar(file.PostedFile)
					Case "rankimage"
						UploadRankImage(file.PostedFile)
					Case "image"
						UploadImage(file.PostedFile)
					Case "pageimage"
						UploadImage(file.PostedFile, 6)
					Case "document"
						UploadDocument(file.PostedFile)
					Case "imagerotator"
						UploadRotator(file.PostedFile)
					Case "memberphoto"
						UploadMemberPhoto(file.PostedFile)
					Case "photogallery"
						UploadPhotoGallery(file.PostedFile)
					Case Else
						UploadImage(file.PostedFile)
				End Select
			else
				if Request.Querystring("TYPE") = "rankimage" then
					Message.text = "Upload An Image To Attach To This Ranking"
				else
					Message.text = "&nbsp;"
				end if
				UploadType.text = Request.Querystring("TYPE")
				RankingID.text = Request.Querystring("RANK")
				RotatorID.text = Request.Querystring("ROTATOR")
				GalleryID.text = Request.Querystring("GALLERY")
			end if
		End Sub

		Sub UploadAvatar(file as HttpPostedFile)
			if (Session("UserLevel") = "3") then
				if Len(AvatarName.text) <> 0 then
					if (Functions.IsImage(file)) then
						Upload(file, "avatars", 3)
					else
						Message.Text = "Error: Must be .jpg or .gif file."
					end if
				else
					Message.Text = "Error: You Must Enter An Avatar Name"
				end if
			else
				Message.Text = "Error: Access Denied"
			end if
		End Sub

		Sub UploadCustomAvatar(file as HttpPostedFile)
			if (Session("UserLogged") = "1") and (AllowTheCustomAvatar) then
				if (Functions.IsImage(file)) then
					Upload(file, "customavatars", 1)
					Dim FileType as String = Right(file.FileName, 3)
					Database.Write("UPDATE " & Database.DBPrefix & "_MEMBERS SET MEMBER_AVATAR_CUSTOMLOADED = 1, MEMBER_AVATAR_CUSTOMTYPE = '" & FileType & "' WHERE MEMBER_ID = " & Request.Querystring("ID"))
				else
					Message.Text = "Error: Must be .jpg or .gif file."
				end if
			else
				Message.Text = "Error: Access Denied"
			end if
		End Sub

		Sub UploadMemberPhoto(file as HttpPostedFile)
			if (Session("UserLogged") = "1") and (AllowMemberPhoto) then
				if (Functions.IsImage(file)) then
					Dim TotalMemberPhotoSize as Integer = 0
					Dim Reader as OdbcDataReader = Database.Read("SELECT SUM(PHOTO_SIZE) as TotalSize FROM " & Database.DBPrefix & "_MEMBER_PHOTOS WHERE MEMBER_ID = " & Request.Querystring("ID"))
					While Reader.Read()
						if (Not Functions.IsDBNull(Reader("TotalSize"))) then
							TotalMemberPhotoSize = Reader("TotalSize")
						end if
					End While
					Reader.Close()

					if ((file.ContentLength+TotalMemberPhotoSize) <= (Settings.MemberPhotoSize*1024)) then
						Dim FileType as String = Right(file.FileName, 3)
						Database.Write("INSERT INTO " & Database.DBPrefix & "_MEMBER_PHOTOS (MEMBER_ID, PHOTO_EXTENSION, PHOTO_SIZE, PHOTO_DESCRIPTION) VALUES (" & Request.Querystring("ID") & ", '" & FileType & "', " & file.ContentLength & ", '" & Functions.RepairString(PhotoDesc.text) & "')")
						Dim Reader2 as OdbcDataReader = Database.Read("SELECT PHOTO_ID FROM " & Database.DBPrefix & "_MEMBER_PHOTOS WHERE MEMBER_ID = " & Request.Querystring("ID") & " ORDER BY PHOTO_ID DESC", 1)
						While Reader2.Read()
							ImageName = Reader2("PHOTO_ID").ToString()
						End While
						Reader2.Close()

						Upload(file, "memberphotos", 5)
					else
						Message.Text = "Error: Uploading this file will put you over the size limit.<br />You must delete existing files before uploading."
					end if
				else
					Message.Text = "Error: Must be .jpg or .gif file."
				end if
			else
				Message.Text = "Error: Access Denied"
			end if
		End Sub

		Sub UploadRankImage(file as HttpPostedFile)
			if (Session("UserLevel") = "3") and (RankingID.text <> "") then
				if (Functions.IsImage(file)) then
					Dim RankingReader as OdbcDataReader = Database.Read("SELECT RANK_ID FROM " & Database.DBPrefix & "_RANKINGS WHERE RANK_ID = " & RankingID.text)
						If RankingReader.HasRows then
							Upload(file, "rankimages", 2)
						End If
					RankingReader.Close()
				else
					Message.Text = "Error: Must be .jpg or .gif file."
				end if
			else
				Message.Text = "Error: Access Denied"
			end if
		End Sub

		Sub UploadImage(file as HttpPostedFile, Optional FileType as Integer = 0)
			if Session("UserLevel") = "3" then
				if (Functions.IsImage(file)) then
					Upload(file, "images", FileType)
				else
					Message.Text = "Error: Must be .jpg or .gif file."
				end if
			else
				Message.Text = "Error: Access Denied"
			end if
		End Sub

		Sub UploadDocument(file as HttpPostedFile)
			if Session("UserLevel") = "3" then
				Upload(file, "documents", 0)
			else
				Message.Text = "Error: Access Denied"
			end if
		End Sub

		Sub UploadRotator(file as HttpPostedFile)
			if (Session("UserLevel") = "3") then
				if (Functions.IsImage(file)) then
					Dim FileType as String = Right(file.FileName, 3)
					Dim Border as String = "0"
					if (Functions.IsInteger(ImageBorder.text)) then
						Border = ImageBorder.text
					end if
					Database.Write("INSERT INTO " & Database.DBPrefix & "_ROTATOR_IMAGES (ROTATOR_ID, IMAGE_EXTENSION, IMAGE_URL, IMAGE_DESCRIPTION, IMAGE_WINDOW, IMAGE_BORDER) VALUES (" & RotatorID.text & ", '" & FileType & "', '" & ImageURL.text & "', '" & Functions.RepairString(ImageDescription.text) & "', " & ImageWindow.SelectedValue & ", " & Border & ")")
					Dim Reader as OdbcDataReader = Database.Read("SELECT IMAGE_ID FROM " & Database.DBPrefix & "_ROTATOR_IMAGES WHERE IMAGE_URL = '" & ImageURL.text & "' ORDER BY IMAGE_ID DESC", 1)
					While Reader.Read()
						ImageName = Reader("IMAGE_ID").ToString()
					End While
					Reader.Close()
					Upload(file, "rotatorimages", 4)
				else
					Message.Text = "Error: Must be .jpg or .gif file."
				end if
			else
				Message.Text = "Error: Access Denied"
			end if
		End Sub

		Sub UploadPhotoGallery(file as HttpPostedFile)
			if (Session("UserLevel") = "3") then
				if (Functions.IsImage(file)) then
					Dim FileType as String = Right(file.FileName, 3)
					Database.Write("INSERT INTO " & Database.DBPrefix & "_GALLERY_PHOTOS (GALLERY_ID, PHOTO_EXTENSION, PHOTO_DESCRIPTION) VALUES (" & GalleryID.text & ", '" & FileType & "', '" & Functions.RepairString(PhotoDesc.text) & "')")
					Dim Reader as OdbcDataReader = Database.Read("SELECT PHOTO_ID FROM " & Database.DBPrefix & "_GALLERY_PHOTOS WHERE PHOTO_DESCRIPTION = '" & Functions.RepairString(PhotoDesc.text) & "' and GALLERY_ID = " & GalleryID.text & " ORDER BY PHOTO_ID DESC", 1)
					While Reader.Read()
						ImageName = Reader("PHOTO_ID").ToString()
					End While
					Reader.Close()
					Upload(file, "photogalleries", 5)
				else
					Message.Text = "Error: Must be .jpg or .gif file."
				end if
			else
				Message.Text = "Error: Access Denied"
			end if
		End Sub

		Sub Upload(file as HttpPostedFile, folder as String, Type as Integer)
			Dim FileName as String = System.IO.Path.GetFileName(file.FileName)
		
			'-------------------------------------------------------------------------------------------------------------------------------------------------
			'TYPES: 1 = custom avatar, 2 = rank image, 3 = avatar, 4 = rotator image, 5 = member photo & gallery, 6 = Page Photo, all others are normal images
			'-------------------------------------------------------------------------------------------------------------------------------------------------

			if (Type = 1) then
				FileName = Request.Querystring("ID") & Right(FileName, 4)
			elseif (Type = 4) or (Type = 5) then
				FileName = ImageName & Right(FileName, 4)
			end if

			Dim FilePath as String = MapPath(folder & "/" & FileName)

			if ((Type = 1) or (Type = 3)) then
				ResizeImageAndSave(file, FilePath, Settings.AvatarSize)
			elseif (Type = 5) then
				file.SaveAs(FilePath)
				Dim ThumbnailFilePath as String = MapPath(folder & "/" & ImageName & "_s" & Right(FileName, 4))
				ResizeImageAndSave(file, ThumbnailFilePath, Settings.ThumbnailSize)
			else
				file.SaveAs(FilePath)
			end if

			Dim FolderID as Integer = 0
			Dim Reader as OdbcDataReader = Database.Read("SELECT FOLDER_ID FROM " & Database.DBPrefix & "_FOLDERS WHERE FOLDER_NAME = '" & folder & "' AND FOLDER_PARENT = 0")
			While Reader.Read()
				FolderID = Reader("FOLDER_ID")
			End While
			Reader.Close()

			Reader = Database.Read("SELECT FILE_ID FROM " & Database.DBPrefix & "_FILES WHERE FILE_NAME = '" & FileName & "' AND FILE_FOLDER = " & FolderID, 1)
				if (Not Reader.HasRows) then
					Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_FOLDER, FILE_NAME, FILE_CORE) VALUES (" & FolderID & ", '" & FileName & "', 0)")
					if (Type = 3) then
						Database.Write("INSERT INTO " & Database.DBPrefix & "_AVATARS (AVATAR_NAME, AVATAR_IMAGE) VALUES ('" & AvatarName.text & "', '" & FileName & "')")
						AvatarName.text = ""
					end if
					if (Type = 5) then
						Database.Write("INSERT INTO " & Database.DBPrefix & "_FILES (FILE_FOLDER, FILE_NAME, FILE_CORE) VALUES (" & FolderID & ", '" & ImageName & "_s" & Right(FileName, 4) & "', 0)")
					end if
				end if
			Reader.Close()

			if (Type = 2) then
				Database.Write("UPDATE " & Database.DBPrefix & "_RANKINGS SET RANK_IMAGE = '" & FileName & "' WHERE RANK_ID = " & RankingID.text)
			end if

			if (Type = 1) then
				Message.Text = "Avatar Uploaded Successfully.<br />You must close this window and save your profile to apply the changes.<br />Be sure to select ""yes"" where the form asks about using your custom avatar."
				AvatarName.text = ""
			elseif (Type = 4) then
				Message.Text = "Image Has Been Added To The Rotation."
				ImageURL.text = ""
				ImageDescription.text = ""
				ImageBorder.text = "0"
				ImageWindow.SelectedValue = 1
			elseif (Type = 5) then
				if ((Request.QueryString("TYPE") = "memberphoto") and (Request.QueryString("ISTOPIC") = "1")) then
					Message.Text = "Photo Uploaded Successfully.<br /><br />It will be attached to the thread after you submit<br />and will also appear in your member profile."
				else
					Message.Text = "Photo Uploaded Successfully."
				end if
				PhotoDesc.text = ""
			elseif (Type = 6) then
				UploadForm.visible = "false"
				Message.Text = """" & FileName & """ - Uploaded Successfully<br /><br />"
				InsertToPage.visible = "true"
				InsertToPage.OnClientClick = "javascript:PassBackToParent('" & folder & "/" & FileName & "');window.close();"
			else
				Message.Text = """" & FileName & """ - Uploaded Successfully"
			end if
		End Sub

		Sub ResizeImageAndSave(file as HttpPostedFile, FilePath as String, MaxSize as Integer)
			Dim NewBitmap As new Bitmap(file.InputStream, false)
			if ((NewBitmap.Width > MaxSize) or (NewBitmap.Height > MaxSize)) then
				Dim NewRatio as Decimal
				Dim NewWidth as Integer = 0
				Dim NewHeight as Integer = 0

				if (NewBitmap.Width > NewBitmap.Height) then
					NewRatio = MaxSize / NewBitmap.Width
					NewWidth = MaxSize
					Dim TempHeight as Decimal = NewBitmap.Height * NewRatio
					NewHeight = TempHeight
				else
					NewRatio = MaxSize / NewBitmap.Height
					NewHeight = MaxSize
					Dim TempWidth as Decimal = NewBitmap.Width * NewRatio
					NewWidth = TempWidth
				end if

				Dim Thumbnail as Bitmap = new Bitmap(NewWidth, NewHeight)
				Dim g as Graphics = Graphics.FromImage(Thumbnail)
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
				g.FillRectangle(Brushes.White,0,0,NewWidth,NewHeight)
				g.DrawImage(NewBitmap,0,0,NewWidth,NewHeight)
				NewBitmap.Dispose()

				Thumbnail.Save(FilePath)
				Thumbnail.Dispose()
			else
				file.SaveAs(FilePath)
			end if
		End Sub
	End Class

End Namespace
