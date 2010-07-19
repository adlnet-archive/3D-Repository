<%@ page title="Login" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Public_Login, App_Web_wlg4q3ts" stylesheettheme="Default" %>

<%@ Register src="../Controls/Login.ascx" tagname="Login" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<br />
<br />
<br />
<br />
<br />
<div style="width: 260px; margin: auto;">
    <uc1:Login ID="Login1" runat="server" />
</div>
<br />
<br />
<br />
<br />
</asp:Content>

