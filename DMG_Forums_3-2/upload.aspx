<%@ Page language="VB" Inherits="DMGForums.Upload.UploadPage" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="Upload" ShowBackground="false" runat="server" />

		<script type="text/javascript">
		<!--
			function PassBackToParent(FilePath)
			{
				if (window.opener.document.getElementById('txtContent'))
				{
					if (document.all)
					{
						window.opener.document.getElementById('txtContent').innerText += '<img src=\"' + FilePath + '\">';
					}
					else
					{
						window.opener.document.getElementById('txtContent').value += '<img src=\"' + FilePath + '\">';
					}
				}
				if (window.opener.document.getElementById('txtPageContent'))
				{
					if (document.all)
					{
						window.opener.document.getElementById('txtPageContent').innerText += '<img src=\"' + FilePath + '\">';
					}
					else
					{
						window.opener.document.getElementById('txtPageContent').value += '<img src=\"' + FilePath + '\">';
					}
				}
			}
		//-->
		</script>
	</HEAD>
	<BODY>
		<form enctype="multipart/form-data" runat="server">

			<table border="0" width="100%" height="100%">
			<tr>
			<td width="100%" height="100%" align="center" valign="middle">

				<asp:Textbox id="UploadType" visible="false" runat="server" />
				<asp:Textbox id="RankingID" visible="false" runat="server" />
				<asp:Textbox id="RotatorID" visible="false" runat="server" />
				<asp:Textbox id="GalleryID" visible="false" runat="server" />

				<asp:PlaceHolder ID="UploadForm" runat="server">
					<table border="0">
					<% if UploadType.text = "avatar" then %>
					<tr>
						<td align="right" valign="top">
							<font size="2" color="<%=Settings.FontColor%>">Avatar Name:</font>
						</td>
						<td>
							<asp:Textbox id="AvatarName" size="30" maxlength="30" runat="server" /> <font size="2" color="<%=Settings.FontColor%>">(enter a name)</font>
						</td>
					</tr>
					<% end if %>
					<% if ((UploadType.text = "memberphoto") or (UploadType.text = "photogallery")) then %>
					<tr>
						<td align="right" valign="top">
							<font size="2" color="<%=Settings.FontColor%>">Photo Description:</font>
						</td>
						<td>
							<asp:Textbox id="PhotoDesc" size="30" maxlength="100" runat="server" /> <font size="2" color="<%=Settings.FontColor%>">(leave blank for none)</font>
						</td>
					</tr>
					<% end if %>
					<tr>
						<td align="right" valign="top">
							<font size="2" color="<%=Settings.FontColor%>">File:</font>
						</td>
						<td>
							<input type="file" id="file" size="30" runat="server" />
						</td>
					</tr>
					<% if UploadType.text = "imagerotator" then %>
					<tr>
						<td align="right" valign="top">
							<font size="2" color="<%=Settings.FontColor%>">Image URL:</font>
						</td>
						<td>
							<asp:Textbox id="ImageURL" size="30" maxlength="100" runat="server" /> <font size="2" color="<%=Settings.FontColor%>">(leave blank for none)</font>
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<font size="2" color="<%=Settings.FontColor%>">Image Text:</font>
						</td>
						<td>
							<asp:Textbox id="ImageDescription" size="30" maxlength="100" runat="server" /> <font size="2" color="<%=Settings.FontColor%>">(leave blank for none)</font>
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<font size="2" color="<%=Settings.FontColor%>">Image Border Size:</font>
						</td>
						<td>
							<asp:Textbox id="ImageBorder" size="3" maxlength="2" value="0" runat="server" />
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<font size="2" color="<%=Settings.FontColor%>">Open Link In New Window?</font>
						</td>
						<td>
							<asp:DropDownList id="ImageWindow" runat="server">
								<asp:ListItem Selected="True" Value="1" Text="Yes" />
								<asp:ListItem Value="0" Text="No" />
							</asp:DropDownList>
						</td>
					</tr>
					<% end if %>
					</table>

					<br />
					<center><asp:Button type="submit" id="submit" text="Submit" runat="server" /></center>
					<br />
				</asp:PlaceHolder>

				<font size="2" color="<%=Settings.FontColor%>">
				<center><b>
					<asp:Label id="Message" runat="server" />
					<asp:Button id="InsertToPage" text="Click Here To Insert Into The Page" visible="false" runat="server" />
					<br /><br />
					<a href="JavaScript:onClick=window.close()">Close Window</a>
				</b></center>
				</font>

			</td>
			</tr>
			</table>

		</form>
	</BODY>
</HTML>