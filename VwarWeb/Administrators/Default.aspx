<%@ Page Title="" Language="C#" MasterPageFile="~/Administrator.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Administrators_Default" %>

<%@ Register src="../Controls/CybrarianQueue.ascx" tagname="CybrarianQueue" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContentPlaceHolder" Runat="Server">

    <uc1:CybrarianQueue ID="CybrarianQueue1" runat="server" />

</asp:Content>

