
<%@ Control Language="VB" EnableViewState="False" ClassName="Settings" Inherits="DMGForums.Global.Settings" %>

<%@ Import Namespace="DMGForums.Global" %>

		<TITLE><%=HtmlTitle%><%= IIF(CustomTitle().Length > 0, " | " & CustomTitle(), "") %></TITLE>

		<STYLE TYPE="text/css">
		<!--
			BODY
			{
				<%= IIF(((BGImage <> "") and (BGImage <> " ") and (ShowBackground())), "background-image: url(" & BGImage & ");", "") %>
				margin-top:<%=TopMargin%>px;
				margin-left:<%=SideMargin%>px;
				margin-right:<%=SideMargin%>px;
				background-color: <%=BGColor%>;
				font-family:<%=FontFace%>;
				<%= IIF(((ScrollbarColor = "") or (ScrollbarColor = " ")), "/*", "") %>
					scrollbar-face-color: <%=ScrollbarColor%>;
					scrollbar-shadow-color: #534741;
					scrollbar-highlight-color: white;
					scrollbar-3dlight-color: white;
					scrollbar-darkshadow-color: #534741;
					scrollbar-track-color: black;
					scrollbar-arrow-color: white
				<%= IIF(((ScrollbarColor = "") or (ScrollbarColor = " ")), "*/", "") %>
			}
		-->
		</STYLE>

		<%=Functions.CustomHTMLVariables(CustomCSS, Session("UserID"), Session("UserLogged"), Session("UserLevel"))%>

		<meta name="Description" content="<%=PageTitle%><%= IIF(CustomTitle().Length > 0, " - " & CustomTitle(), "") %>">
		<meta name="Keywords" content="dmgforums, dmg forums, dmgdevelopment, dmg development, <%=MetaKeywords%>">
		<meta name="Copyright" content="All Code Is (C) DMG Development">

		<script type="text/javascript">
		<!--
		function openUploader(url)
		{
			popupWin = window.open(url,'new_page','width=590,height=375,scrollbars=no,toolbar=no,menubar=no,resizable=no')
		}
		function openAvatars(url)
		{
			popupWin = window.open(url,'new_page','width=640,height=480,scrollbars=yes,toolbar=no,menubar=no,resizable=no')
		}
		function openHelp(url)
		{
			popupWin = window.open(url,'new_page','width=800,height=600,scrollbars=yes,toolbar=no,menubar=no,resizable=no')
		}
		function openPhoto(url)
		{
			popupWin = window.open(url,'new_page','HEIGHT=200,WIDTH=200,left=50,top=50,scrollbars=auto,toolbar=no,menubar=no,resizable=1')
		}
		//-->
		</script>
