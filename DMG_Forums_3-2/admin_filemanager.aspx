<%@ Page language="VB" Inherits="DMGForums.Admin.FileManager" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>
<%@ Import Namespace="DMGForums.Global.Functions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="Admin | File Manager" runat="server" />
	</HEAD>
	<BODY>
		<form runat="server">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" ShowLogin="1" runat="server" />

	<asp:PlaceHolder id="PagePanel" runat="server">

		<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
		<tr class="HeaderCell">
		<td align="left">
			<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b><a href="admin.aspx" style="color:<%=Settings.HeaderFontColor%>;">DMG Forums Admin Console</a> >> <a href="admin_filemanager.aspx" style="color:<%=Settings.HeaderFontColor%>;">File Manager</a></b></font>
		</td>
		</tr>
		<tr class="TableRow1">
		<td style="border-top:1px solid <%=Settings.TableBorderColor%>;">
			<br />

			<asp:PlaceHolder id="FileManagerPanel" runat="server">
				<table border="0">
				<tr>
				<td width="25" align="left"><a href="admin_filemanager.aspx?ID=0"><img border="0" src="forumimages/folder_home.gif"></a></td>
				<td width="25" align="left"><asp:PlaceHolder id="UpIcon" runat="server"><a href="admin_filemanager.aspx?ID=<%=FolderParent%>"><img border="0" src="forumimages/folder_up.gif"></a></asp:PlaceHolder></td>
				<td colspan="3" align="left">
					<font size="3" color="<%=Settings.TopicsFontColor%>"><b>&nbsp;&nbsp;&nbsp;/<asp:Label id="FolderPathLabel" runat="server" /></b></font>
				</td>
				</tr>
					<asp:Repeater id="FolderList" runat="server">
					<ItemTemplate>
						<tr>
						<td width="25">&nbsp;</td><td width="25">&nbsp;</td><td width="25" align="left"><a href="admin_filemanager.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "FOLDER_ID") %>"><img border="0" src="forumimages/folder.gif"></a></td><td align="left"><a href="admin_filemanager.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "FOLDER_ID") %>"><%# DataBinder.Eval(Container.DataItem, "FOLDER_NAME") %></a></td><td align="left">&nbsp;&nbsp;&nbsp;<asp:button id="DeleteFolderButton" onclick="DeleteFolder" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "FOLDER_ID") %>' CssClass="dmgbuttons" runat="server" Text="X" visible='<%# IIF(DataBinder.Eval(Container.DataItem, "FOLDER_CORE") <> 1, "True", "False") %>' /></td>
						</tr>
					</ItemTemplate>
					</asp:Repeater>

					<asp:Repeater id="FileList" runat="server">
					<ItemTemplate>
						<tr>
						<td width="25">&nbsp;</td><td width="25">&nbsp;</td><td align="right" width="25" height="25" align="center" valign="middle"><%# GetFileIcon(DataBinder.Eval(Container.DataItem, "FILE_NAME")) %></td><td align="left"><a target="_blank" href="<%=FolderPath%><%# DataBinder.Eval(Container.DataItem, "FILE_NAME") %>"><%# DataBinder.Eval(Container.DataItem, "FILE_NAME") %></a></td><td align="left">&nbsp;&nbsp;&nbsp;<asp:button id="DeleteFileButton" onclick="DeleteFileConfirmation" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "FILE_ID") %>' CssClass="dmgbuttons" runat="server" Text="X" visible='<%# IIF(DataBinder.Eval(Container.DataItem, "FILE_CORE") <> 1, "True", "False") %>' /></td>
						</tr>
					</ItemTemplate>
					</asp:Repeater>
				</table>
				<br />
				<table border="0">
				<tr>
				<td width="25">&nbsp;</td><td width="25">&nbsp;</td><td align="center">
					<input type="file" id="file" size="25" runat="server" /><br /><asp:Button type="submit" id="NewFileButton" onclick="UploadFile" text="Upload File" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				</table>
				<br />
				<table border="0">
				<tr>
				<td width="25">&nbsp;</td><td width="25">&nbsp;</td><td align="center">
					<asp:Textbox id="NewFolder" Size="40" MaxLength="100" runat="server" /><br /><asp:Button type="submit" id="NewFolderButton" onclick="UploadFolder" text="New Folder" CssClass="AdminButtons" runat="server" />
				</td>
				</tr>
				</table>
			</asp:PlaceHolder>

			<asp:PlaceHolder id="ConfirmFileDelete" visible="false" runat="server">
				<table border="0" width="97%" align="center" cellpadding="5" cellspacing="0">
				<tr>
				<td width="100%" align="center" valign="bottom">
					<br /><br />
					<font size="2" color="<%=Settings.TopicsFontColor%>">Are you sure you want to delete this file?</font><br /><br />
					<asp:Button id="FileDeleteButton" onclick="DeleteFile" runat="server" Text="YES" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button id="NoFileDeleteButton" onclick="CancelDeleteFile" runat="server" Text="NO" />
				</td>
				</tr>
				</table>
			</asp:PlaceHolder>

			<br />
		</td>
		</tr>
		</table>
		<br />

	</asp:PlaceHolder>

	<div align="center" id="NoItemsDiv" class="MessageBox" runat="server" />

	<DMG:Footer id="DMGFooter" runat="server" />

		</form>
	</BODY>
</HTML>