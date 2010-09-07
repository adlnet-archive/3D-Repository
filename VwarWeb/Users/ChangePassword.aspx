<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="Users_ChangePassword" Title="Change Password" %>

<%@ Register src="../Controls/ChangePassword.ascx" tagname="ChangePassword" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <br />
    <br />
    <br />
    <br />
    <br />
    <div style="width: 400px; margin: auto;">
        <uc1:ChangePassword ID="ChangePassword1" runat="server" />
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

