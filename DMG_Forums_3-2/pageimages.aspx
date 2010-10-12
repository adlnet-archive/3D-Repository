<%@ Page language="VB" Inherits="DMGForums.PageImages" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="Page Images" ShowBackground="false" runat="server" />
	</HEAD>
	<BODY>
		<form runat="server">

		<table border="0" width="100%" height="100%">
		<tr>
		<td width="100%" height="100%" align="center" valign="middle">

			<asp:PlaceHolder ID="PagePanel" runat="server">

				<asp:PlaceHolder ID="ProductImagesMain" visible="false" runat="server">
					<asp:Button id="UploadThumbnailButton" onclick="OpenThumbnailForm" Text="Upload Thumbnail Image" runat="server" />
					<br /><br />
					<asp:Button id="UploadPhotoButton" onclick="OpenPhotoForm" Text="Upload Full Photo" runat="server" />
					<br /><br />
					<asp:Button id="DeleteThumbnailButton" onclick="DeleteThumbnail" Text="Delete Thumbnail" visible="false" runat="server" />
					<br /><br />
					<asp:Button id="DeletePhotoButton" onclick="DeletePhoto" Text="Delete Full Photo" visible="false" runat="server" />
				</asp:PlaceHolder>

				<asp:PlaceHolder ID="ThumbnailForm" visible="false" runat="server">
					<table border="0" cellpadding="6" align="center">
					<tr>
						<td align="right" valign="middle">
							<font size="2" color="<%=Settings.FontColor%>"><b>File:</b></font>
						</td>
						<td align="left" valign="middle">
							<input type="file" id="ThumbnailFile" size="30" runat="server" />
						</td>
					</tr>
					</table>

					<br />
					<center><asp:Button id="ThumbnailSubmit" onclick="UploadThumbnail" text="Submit" runat="server" /></center>
				</asp:PlaceHolder>

				<asp:PlaceHolder ID="PhotoForm" visible="false" runat="server">
					<table border="0" cellpadding="6" align="center">
					<tr>
						<td align="right" valign="middle">
							<font size="2" color="<%=Settings.FontColor%>"><b>File:</b></font>
						</td>
						<td align="left" valign="middle">
							<input type="file" id="PhotoFile" size="30" runat="server" />
						</td>
					</tr>
					</table>

					<br />
					<center><asp:Button id="PhotoSubmit" onclick="UploadPhoto" text="Submit" runat="server" /></center>
				</asp:PlaceHolder>

				<font color="<%=Settings.FontColor%>">
					<asp:Label id="Message" runat="server" />
				</font>

			</asp:PlaceHolder>

			<div align="center" id="NoItemsDiv" class="MessageBox" runat="server" />

		</td>
		</tr>
		</table>

		</form>
	</BODY>
</HTML>