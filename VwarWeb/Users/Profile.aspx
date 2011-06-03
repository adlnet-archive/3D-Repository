<%@ Page Title="Profile" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Profile.aspx.cs" Inherits="Users_Profile" %>
<%@ Register src="../Controls/Profile.ascx" tagname="Profile" tagprefix="uc2" %>
<%@ Register src="../Controls/MyModels.ascx" tagname="MyModels" tagprefix="uc1" %>
<%@ Register Src="../Controls/MyKeys.ascx" TagName="MyKeys" TagPrefix="vwarweb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript" src="../Scripts/jquery-ui-1.8.7.custom.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />
    <br />
    <div style="width: 600px; margin: auto;">
        <uc2:Profile ID="Profile1" runat="server" />
        <br />
        <br />
        <br />
        <uc1:MyModels ID="MyModels1" runat="server" />
        <br />
        <br />
        <br />
        <vwarweb:MyKeys ID="KeysControl" runat="server" />
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

