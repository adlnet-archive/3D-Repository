<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" enablesessionstate="True" inherits="Users_Upload, App_Web_uhnkwe5g" title="Upload" maintainscrollpositiononpostback="true" stylesheettheme="Default" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register src="../Controls/Upload.ascx" tagname="Upload" tagprefix="uc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
    </telerik:RadAjaxManagerProxy>
       
         <uc1:Upload ID="Upload1" runat="server" />

</asp:Content>
