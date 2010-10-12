<%@ Page language="VB" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Header" Src="inc_header.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Footer" Src="inc_footer.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings runat="server" />
	</HEAD>
	<BODY>
		<form runat="server">

	<DMG:Header id="DMGHeader" runat="server" />

		<br /><br />
		<table border="0" align="center"><tr><td>
			<DMG:Login id="DMGLogin" ShowLogin="1" runat="server" />
		</td></tr></table>
		<br /><br />

	<DMG:Footer id="DMGFooter" runat="server" />

		</form>
	</BODY>
</HTML>