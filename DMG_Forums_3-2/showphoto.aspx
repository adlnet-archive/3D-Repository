<%@ Page language="VB" %>

<%@ Import Namespace="DMGForums.Global" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<TITLE>Photo Gallery</TITLE>

		<STYLE TYPE="text/css">
		<!--
			BODY
			{
				margin: 0px;
				background-color: <%=Settings.BGColor%>;
				font-family: <%=Settings.FontFace%>;
			}
		-->
		</STYLE>

		<script type="text/javascript">
		<!--
			function resizeWindow()
			{
				if (window.innerWidth)
				{
					iWidth = window.innerWidth;
					iHeight = window.innerHeight;
				}
				else
				{
					iWidth = document.body.clientWidth;
					iHeight = document.body.clientHeight;
				}
				iWidth = document.images[0].width - iWidth;
				iHeight = document.images[0].height - iHeight;
				window.resizeBy(iWidth, iHeight);
				self.focus();
			}
		//-->
		</script>
	</HEAD>
	<BODY onload="resizeWindow()">
		<form runat="server">

			<center>
			<a href="javascript: self.close()"><img border="0" src="<%=Request.Querystring("PHOTO")%>"></a>
			</center>

		</form>
	</BODY>
</HTML>