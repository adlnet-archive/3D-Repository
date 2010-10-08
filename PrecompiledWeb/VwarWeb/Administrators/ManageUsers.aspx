<%@ page title="" language="C#" masterpagefile="~/Administrator.master" autoeventwireup="true" inherits="Administrators_ManageUsers, App_Web_v2eyi2xn" stylesheettheme="Default" %>

<%@ Register src="../Controls/ManageUsers.ascx" tagname="ManageUsers" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContentPlaceHolder" Runat="Server">
<div style="width: 790px; margin: auto;">

  <uc1:ManageUsers ID="ManageUsers1" runat="server" />
</div>
  

</asp:Content>

