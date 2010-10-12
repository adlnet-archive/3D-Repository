<%@ Page language="VB" Inherits="DMGForums.NewPage" maintainScrollPositionOnPostBack="true" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="New Page" runat="server" />
	</HEAD>
	<BODY>
		<form runat="server">

	<DMG:Header id="DMGHeader" runat="server" />
	<DMG:Login id="DMGLogin" ShowLogin="1" runat="server" />

	<asp:Panel id="PagePanel" runat="server">

	<table width="97%" align="center" class="ContentBox" cellpadding="5" cellspacing="0">
	<tr class="HeaderCell">
	<td align="left">
		<font size="<%=Settings.HeaderSize%>" color="<%=Settings.HeaderFontColor%>"><b><a href="admin.aspx" style="color:<%=Settings.HeaderFontColor%>;">DMG Forums Admin Console</a></b></font>
	</td>
	</tr>
	<tr class="TableRow1">
	<td style="border-top:1px solid <%=Settings.TableBorderColor%>;">

		<table border="2" cellpadding="10" align="center" width="700">
		<tr class="TableRow2">
		<td align="center" valign="middle" height="40" colspan="2">
			<font size="4" color="<%=Settings.TopicsFontColor%>"><b>Create New Page</b></font>
		</td>
		</tr>
		<tr>
		<td colspan="2">
			<table border="0" cellpadding="6" align="center">
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Page Name:</b><br /><i>(For Links)</i></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtPageName" size="30" maxlength="100" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Parent Page:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:DropDownList id="txtPageParent" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Page Title:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtPageTitle" size="60" maxlength="100" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Page Content:</b></font>
				</td>
				<td align="left" valign="middle"><asp:Button id="DMGCode" OnClientClick="javascript:openHelp('DMGAdminCode.html')" text="DMG Codes" runat="server" />&nbsp;&nbsp;&nbsp;<input type="button" id="EditImages" onclick="javascript:openUploader('pageimages.aspx?ID=0')" value="Thumbnail/Photo" />&nbsp;&nbsp;&nbsp;<asp:Button id="AddGallery" OnClientClick="javascript:openUploader('pagenewgallery.aspx')" text="Insert Gallery" runat="server" />&nbsp;&nbsp;&nbsp;<asp:Button id="AddImage" OnClientClick="javascript:openUploader('upload.aspx?TYPE=pageimage')" text="Upload Image" runat="server" /></td>
			</tr>
			<tr>
				<td align="left" valign="middle" colspan="2">
					<asp:Textbox id="txtPageContent" Textmode="multiline" runat="server" Columns="85" Rows="25" />
				</td>
			</tr>
			</table>
		</td>
		</tr>
		<tr>
		<td width="50%" align="center" valign="top">
			<font size="4" color="<%=Settings.TopicsFontColor%>"><b>Page Options</b></font><br /><br />
			<table border="0" cellpadding="6" align="center">
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Security:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:DropDownList id="txtSecurity" AutoPostBack="true" OnSelectedIndexChanged="SecurityChange" runat="server">
						<asp:ListItem Selected="True" Value="0" Text="All Visitors" runat="server" />
						<asp:ListItem Value="1" Text="Allowed Members List" runat="server" />
						<asp:ListItem Value="2" Text="Password Protected" runat="server" />
						<asp:ListItem Value="3" Text="Moderators Only" runat="server" />
						<asp:ListItem Value="4" Text="Members Only" runat="server" />
					</asp:DropDownList>
				</td>
			</tr>
			<asp:PlaceHolder id="PasswordPanel" visible="false" runat="server">
				<tr>
					<td align="right" valign="middle">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Password:</b></font>
					</td>
					<td align="left" valign="middle">
						<asp:Textbox id="txtPassword" TextMode="password" size="30" maxlength="50" runat="server" />
					</td>
				</tr>
			</asp:PlaceHolder>
			<asp:PlaceHolder id="AllowedUsersPanel" visible="false" runat="server">
				<tr>
					<td align="right" valign="top">
						<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Member List:</b></font>
					</td>
					<td align="left" valign="top">
						<table border="0" cellpadding="6">
						<tr>
						<td align="left" valign="top">
							<font size="2" color="<%=Settings.TopicsFontColor%>">Available</font><br />
							<asp:ListBox DataValueField="MEMBER_ID" DataTextField="MEMBER_USERNAME" id="txtMembers" runat="server" />
						</td>
						<td align="center" valign="middle">
							<asp:button id="FArrow" onclick="AddMember" CssClass="dmgbuttons" runat="server" Text=">>" /><br /><br />
							<asp:button id="BArrow" onclick="RemoveMember" CssClass="dmgbuttons" runat="server" Text="<<" />
						</td>
						<td align="left" valign="top">
							<font size="2" color="<%=Settings.TopicsFontColor%>">Selected</font><br />
							<asp:ListBox id="txtAllowedMembers" runat="server" />
						</td>
						</tr>
						</table>
					</td>
				</tr>
			</asp:PlaceHolder>
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Add To The Main Menu?</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:DropDownList id="txtAddToMainMenu" runat="server">
						<asp:ListItem Value="1" Text="Yes" runat="server" />
						<asp:ListItem Value="0" Text="No" Selected="True" runat="server" />
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Show Title?</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:DropDownList id="txtShowTitle" runat="server">
						<asp:ListItem Selected="True" Value="1" Text="Yes" runat="server" />
						<asp:ListItem Value="0" Text="No" runat="server" />
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Auto-Format HTML?</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:DropDownList id="txtAutoFormat" runat="server">
						<asp:ListItem Value="1" Text="Yes" runat="server" />
						<asp:ListItem Selected="True" Value="0" Text="No" runat="server" />
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Show HTML Header/Footer?</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:DropDownList id="txtShowHeaders" runat="server">
						<asp:ListItem Selected="True" Value="1" Text="Yes" runat="server" />
						<asp:ListItem Value="0" Text="No" runat="server" />
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Show Login Box?</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:DropDownList id="txtShowLogin" runat="server">
						<asp:ListItem Value="1" Text="Yes" runat="server" />
						<asp:ListItem Value="0" Text="No" runat="server" />
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Status:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:DropDownList id="txtStatus" runat="server">
						<asp:ListItem Selected="True" Value="1" Text="On" runat="server" />
						<asp:ListItem Value="0" Text="Off" runat="server" />
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Sort By:</b></font>
				</td>
				<td align="left" valign="top">
					<asp:Textbox id="txtSortBy" size="10" maxlength="10" value="1" runat="server" />
					<font size="2" color="<%=Settings.TopicsFontColor%>"><br />(Default Is 1)</font>
				</td>
			</tr>
			</table>
		</td>
		<td width="50%" align="center" valign="top">
			<font size="4" color="<%=Settings.TopicsFontColor%>"><b>Sub-Category Options</b></font><br /><br />
			<table border="0" cellpadding="6" align="center">
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Sub-Category Title:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtSubTitle" Value="Sub-Categories" size="20" maxlength="100" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Show Title?</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:DropDownList id="txtSubShowTitle" runat="server">
						<asp:ListItem Value="1" Text="Yes" runat="server" />
						<asp:ListItem Value="0" Text="No" Selected="True" runat="server" />
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td align="right" valign="middle">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Columns:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:Textbox id="txtSubColumns" size="5" Value="1" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Alignment:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:DropDownList id="txtSubAlign" runat="server">
						<asp:ListItem Value="1" Text="Left" Selected="True" runat="server" />
						<asp:ListItem Value="2" Text="Center" runat="server" />
						<asp:ListItem Value="3" Text="Right" runat="server" />
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td align="right" valign="top">
					<font size="2" color="<%=Settings.TopicsFontColor%>"><b>Sub-Category Status:</b></font>
				</td>
				<td align="left" valign="middle">
					<asp:DropDownList id="txtSubStatus" runat="server">
						<asp:ListItem Value="1" Text="On" Selected="True" runat="server" />
						<asp:ListItem Value="0" Text="Off" runat="server" />
					</asp:DropDownList>
				</td>
			</tr>
			</table>
		</td>
		</tr>
		<tr>
		<td colspan="2" align="center" height="40" valign="middle">
			<asp:Button type="submit" id="SubmitButton" onclick="SubmitPage" text="Submit Page" CssClass="AdminButtons" runat="server" />
		</td>
		</tr>
		</table>

	</td>
	</tr>
	</table>
	<br />

	</asp:Panel>

	<div align="center" id="NoItemsDiv" class="MessageBox" runat="server" />

	<DMG:Footer id="DMGFooter" runat="server" />

		</form>
	</BODY>
</HTML>