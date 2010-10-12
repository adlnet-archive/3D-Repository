<%@ Page language="VB" Inherits="DMGForums.PageNewGallery" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="Create New Gallery" ShowBackground="false" runat="server" />

		<script type="text/javascript">
		<!--
			function PassBackToParent(GalleryID, NumColumns)
			{
				if (window.opener.document.getElementById('txtContent'))
				{
					if (document.all)
					{
						window.opener.document.getElementById('txtContent').innerText += '[PhotoGallery ID=' + GalleryID + ' Columns=' + NumColumns + ']';
					}
					else
					{
						window.opener.document.getElementById('txtContent').value += '[PhotoGallery ID=' + GalleryID + ' Columns=' + NumColumns + ']';
					}
				}
				if (window.opener.document.getElementById('txtPageContent'))
				{
					if (document.all)
					{
						window.opener.document.getElementById('txtPageContent').innerText += '[PhotoGallery ID=' + GalleryID + ' Columns=' + NumColumns + ']';
					}
					else
					{
						window.opener.document.getElementById('txtPageContent').value += '[PhotoGallery ID=' + GalleryID + ' Columns=' + NumColumns + ']';
					}
				}
			}
		//-->
		</script>
	</HEAD>
	<BODY>
		<form runat="server">

		<table border="0" width="100%" height="100%">
		<tr>
		<td width="100%" height="100%" align="center" valign="middle">

			<asp:PlaceHolder ID="PagePanel" runat="server">

				<asp:PlaceHolder ID="InsertGalleryForm" visible="false" runat="server">
					<asp:Button id="SelectNew" onclick="CreateNewGallery" Text="Create New Gallery" runat="server" />
					<br /><br />
					<asp:Button id="SelectExisting" onclick="SelectExistingGallery" Text="Select Existing Gallery" runat="server" />
				</asp:PlaceHolder>

				<asp:PlaceHolder ID="ExistingGalleryForm" visible="false" runat="server">
					<table border="0" cellpadding="6" align="center">
					<tr>
						<td align="right" valign="middle">
							<font size="2" color="<%=Settings.FontColor%>"><b>Gallery:</b></font>
						</td>
						<td align="left" valign="middle">
							<asp:DropDownList id="ExistingDropDown" runat="server" />
						</td>
					</tr>
					<tr>
						<td align="right" valign="middle">
							<font size="2" color="<%=Settings.FontColor%>"><b>Number of Columns:</b></font>
						</td>
						<td align="left" valign="middle">
							<asp:Textbox id="ExistingGalleryColumns" size="5" maxlength="10" Text="5" runat="server" />
						</td>
					</tr>
					</table>

					<br />
					<center><asp:Button id="ExistingSubmit" onclick="SubmitExistingGallery" text="Submit" runat="server" /></center>
				</asp:PlaceHolder>

				<asp:PlaceHolder ID="ExistingGalleryConfirm" visible="false" runat="server">
					<center>
					<font size="3" color="<%=Settings.FontColor%>"><b><asp:Label id="ExistingGallerySuccess" runat="server" /></b></font>					
					<br /><br />
					<asp:Button id="InsertExisting" text="Click Here To Insert Into The Page" runat="server" />
					</center>
				</asp:PlaceHolder>

				<asp:PlaceHolder ID="NewGalleryForm" visible="false" runat="server">
					<table border="0" cellpadding="6" align="center">
					<tr>
						<td align="right" valign="middle">
							<font size="2" color="<%=Settings.FontColor%>"><b>Gallery Name:</b></font>
						</td>
						<td align="left" valign="middle">
							<asp:Textbox id="NewGalleryName" size="30" maxlength="30" runat="server" />
						</td>
					</tr>
					<tr>
						<td align="right" valign="middle">
							<font size="2" color="<%=Settings.FontColor%>"><b>Number of Columns:</b></font>
						</td>
						<td align="left" valign="middle">
							<asp:Textbox id="NewGalleryColumns" size="5" maxlength="10" Text="5" runat="server" />
						</td>
					</tr>
					</table>

					<br />
					<center><asp:Button id="GallerySubmit" onclick="SubmitNewGallery" text="Submit" runat="server" /></center>
				</asp:PlaceHolder>

				<asp:PlaceHolder ID="NewGalleryConfirm" visible="false" runat="server">
					<center>
					<font size="3" color="<%=Settings.FontColor%>"><b><asp:Label id="CreateGallerySuccess" runat="server" /></b></font>					
					<br /><br />
					<asp:Button id="GalleryAddPhotos" onclick="AddPhotos" text="Click Here To Upload Photos" runat="server" />
					</center>
				</asp:PlaceHolder>

			</asp:PlaceHolder>

			<div align="center" id="NoItemsDiv" class="MessageBox" runat="server" />

		</td>
		</tr>
		</table>

		</form>
	</BODY>
</HTML>