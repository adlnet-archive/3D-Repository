
<%@ Control Language="VB" EnableViewState="False" ClassName="Footer" Inherits="DMGForums.Global.Footer" %>

<%@ Import Namespace="DMGForums.Global" %>

	<table width="97%" align="center" cellpadding="5" cellspacing="0" class="ContentBox">
	<tr class="FooterCell"><td width="100%">

		<table border="0" width="100%" cellpadding="0" cellspacing="0">
		<tr>
		<td width="150" nowrap>
		<% if Session("UserLevel") = "3" then %>
			<font size="<%=Settings.FooterSize%>" color="<%=Settings.FooterFontColor%>">
				<a href="admin.aspx" style="color: <%=Settings.FooterFontColor%>">Administration</a>
			</font>
		<% else %>
			&nbsp;
		<% end if %>
		</td>
		<td width="100%" align="center">
			<font size="<%=Settings.FooterSize%>" color="<%=Settings.FooterFontColor%>">
				<%=Settings.Copyright%>
			</font>
		</td>
		<td width="150" align="center" nowrap>
			<font size="<%=Settings.FooterSize%>" color="<%=Settings.FooterFontColor%>">
				<%=Settings.DMGVersionText%>
			</font>
		</td>
		</tr>
		</table>

	</td></tr>
	</table>

	<br />

	<%=Functions.CustomHTMLVariables(Settings.CustomFooter, Session("UserID"), Session("UserLogged"), Session("UserLevel"))%>
