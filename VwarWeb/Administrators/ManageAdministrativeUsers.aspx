<%@ Page Title="" Language="C#" MasterPageFile="~/Administrator.master" AutoEventWireup="true" CodeFile="ManageAdministrativeUsers.aspx.cs" Inherits="Administrators_ManageAdministrativeUsers" %>


<%@ Register src="../Controls/ManageAdministrativeUsers.ascx" tagname="ManageAdministrativeUsers" tagprefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="AdminContentPlaceHolder" Runat="Server">

  

    <uc1:ManageAdministrativeUsers ID="ManageAdministrativeUsers1" runat="server" />

  

</asp:Content>

