<%@ Page Title="Login" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Public_Login" %>

<%@ Register src="../Controls/Login.ascx" tagname="Login" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<br />
<br />
<br />
<br />
<br />
<div style="width: 410px; margin: auto; text-align:center">
    <uc1:Login ID="Login1" runat="server"/>
</div>
<br />
<br />
<br />
<br />
</asp:Content>

