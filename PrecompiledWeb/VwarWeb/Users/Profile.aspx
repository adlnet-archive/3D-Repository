<%@ page title="Profile" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Users_Profile, App_Web_uhnkwe5g" stylesheettheme="Default" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register src="../Controls/Profile.ascx" tagname="Profile" tagprefix="uc2" %>
<%@ Register src="../Controls/MyModels.ascx" tagname="MyModels" tagprefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />
    <br />
     
    <telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
    </telerik:RadAjaxManagerProxy>
    <div style="width: 600px; margin: auto;">
        <uc2:Profile ID="Profile1" runat="server" />
        <br />
        <br />
        <br />
        <uc1:MyModels ID="MyModels1" runat="server" />
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

