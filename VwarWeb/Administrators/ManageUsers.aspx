<%@ Page Title="" Language="C#" MasterPageFile="~/Administrator.master" AutoEventWireup="true" CodeFile="ManageUsers.aspx.cs" Inherits="Administrators_ManageUsers" %>

<%@ Register src="../Controls/ManageUsers.ascx" tagname="ManageUsers" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContentPlaceHolder" Runat="Server">

    <uc1:ManageUsers ID="ManageUsers1" runat="server" />

</asp:Content>

