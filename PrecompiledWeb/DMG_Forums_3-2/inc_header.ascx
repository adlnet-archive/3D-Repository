
<%@ Control Language="VB" EnableViewState="False" ClassName="Header" Inherits="DMGForums.Global.Header" %>

<%@ Import Namespace="DMGForums.Global" %>

	<%= Functions.CustomHTMLVariables(Settings.CustomHeader, Session("UserID"), Session("UserLogged"), Session("UserLevel"))%>

