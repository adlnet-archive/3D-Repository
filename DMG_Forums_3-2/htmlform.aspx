<%@ Page language="VB" Inherits="DMGForums.HtmlForm" %>

<%@ Import Namespace="DMGForums.Global" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<STYLE TYPE="text/css">
		<!--
			BODY
			{
				margin: 0px;
				background-color: transparent;
				font-family: <%=Settings.FontFace%>;
			}
			A:link {text-decoration: <%=Settings.LinkDecoration%>; color: <%=Settings.LinkColor%>}
			A:visited {text-decoration: <%=Settings.VLinkDecoration%>; color: <%=Settings.VLinkColor%>}
			A:active {text-decoration: <%=Settings.ALinkDecoration%>; color: <%=Settings.ALinkColor%>}
			A:hover {text-decoration: <%=Settings.HLinkDecoration%>; color: <%=Settings.HLinkColor%>}
		-->
		</STYLE>

		<script type="text/javascript">
		<!--
			function resizeIframe(iframeID)
			{ 
				if (document.getElementById)
				{
					var FramePageHeight = framePage.scrollHeight + 5;
					var FramePageWidth = framePage.scrollWidth + 5;
					parent.document.getElementById(iframeID).style.height=FramePageHeight;
					parent.document.getElementById(iframeID).style.width=FramePageWidth;
				}
			}
		//-->
		</script>
	</HEAD>
	<BODY id="framePage" onload="resizeIframe('DMGForm')">
		<form runat="server"></form>

		<font size="2" color="<%=Settings.FontColor%>">
			<%=FormText%>
		</font>
	</BODY>
</HTML>