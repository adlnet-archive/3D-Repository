<%@ Page language="VB" Inherits="DMGForums.Members.Avatars" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="Avatars" ShowBackground="false" runat="server" />

		<script type="text/javascript">
		<!--
			function PassBackToParent(AvatarID, AvatarName)
			{ 
				if (document.all)
				{
					window.opener.document.getElementById('txtAvatar').innerText = AvatarID;
					window.opener.document.getElementById('txtAvatarName').innerText = AvatarName;
				}
				else
				{
					window.opener.document.getElementById('txtAvatar').value = AvatarID;
					window.opener.document.getElementById('txtAvatarName').value = AvatarName;
				}

				if (window.opener.document.getElementById('txtShowAvatar'))
					window.opener.document.getElementById('txtShowAvatar').value = 1;

				if (window.opener.document.getElementById('txtUseCustomAvatar'))
					window.opener.document.getElementById('txtUseCustomAvatar').value = 0;
			}
		//-->
		</script>
	</HEAD>
	<BODY>
		<form runat="server">

			<table border="0" width="100%" height="100%">
			<tr>
			<td width="100%" height="100%" align="center" valign="middle">

				<font size="3" color="<%=Settings.FontColor%>"><b>Click The Avatar You Wish To Change To</b></font>

				<asp:DataList id="Avatars" runat="server" AutoGenerateColumns="False" border="0" CellPadding="10" RepeatColumns="5" RepeatDirection="Horizontal">
					<ItemTemplate>
						<font size="2">
						<center>
						<a href="JavaScript:onClick=PassBackToParent('<%# DataBinder.Eval(Container.DataItem, "AVATAR_ID") %>', '<%# DataBinder.Eval(Container.DataItem, "AVATAR_NAME") %>');window.close()" style="cursor:hand; color: <%=Settings.LinkColor%>;"><img border="0" src="avatars/<%# DataBinder.Eval(Container.DataItem, "AVATAR_IMAGE") %>"><br /><%# DataBinder.Eval(Container.DataItem, "AVATAR_NAME") %></a>
						</center>
						</font>
					</ItemTemplate>
				</asp:DataList>

				<div align="center" id="NoItemsDiv" class="MessageBox" runat="server" />

				<font size="2">
				<br /><br /><a href="JavaScript:onClick=window.close();">Close Window</a>
				</font>

			</td>
			</tr>
			</table>

		</form>
	</BODY>
</HTML>