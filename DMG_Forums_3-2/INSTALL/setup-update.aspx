<%@ Page language="VB" Inherits="DMGForums.Setup.Update" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<title>DMG Forums Installer</title>
	</HEAD>
	<BODY>
		<form enctype="multipart/form-data" runat="server">

			<asp:Panel id="SetupForm" visible="false" runat="server">
				<center><font size="4" face="arial,helvetica"><b>DMG Forums Upgrade Utility</b></font><br /><br />
				<font face="arial,helvetica" size="2">
				Press the update button below to upgrade your database to the latest version of the software.<br /><br />
				<asp:Button type="submit" id="Submit" onclick="UpdateForums" text="Update Forums" runat="server" />
				</font>
				</center>
			</asp:Panel>

			<asp:Panel id="MessagePanel" visible="false" runat="server">
				<table border="0" width="100%" height="100%">
				<tr>
				<td width="100%" height="100%" align="center" valign="middle">
					<font face="arial,helvetica" size="3">
						<asp:Label id="Message" runat="server" />
					</font>
				</td>
				</tr>
				</table>
			</asp:Panel>

		</form>
	</BODY>
</HTML>
