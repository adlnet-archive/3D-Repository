<%@ page title="Register" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Public_Register, App_Web_wlg4q3ts" stylesheettheme="Default" %>

<%@ Register src="../Controls/Register.ascx" tagname="Register" tagprefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <ajax:ToolkitScriptManager ID="sm1" runat="server">
    </ajax:ToolkitScriptManager>
     <br />
    <br />
    <br />
    <br />
    <br />
    <div style="width: 460px; margin: auto; background-color: White;">
        <uc1:Register ID="Register1" runat="server" />
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

