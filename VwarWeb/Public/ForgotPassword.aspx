<%@ Page Title="Forgot Password" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ForgotPassword.aspx.cs" Inherits="Public_ForgotPassword" %>

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
