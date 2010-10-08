<%@ page title="Forgot Password" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Public_ForgotPassword, App_Web_rwxwk2ol" stylesheettheme="Default" %>

<%@ Register Src="../Controls/ForgotPassword.ascx" TagName="ForgotPassword" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <br />
    <br />
    <br />
    <br />
    <div style="width: 360px; margin: auto;">
        <uc1:ForgotPassword ID="ForgotPassword1" runat="server" />
    </div>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
</asp:Content>
