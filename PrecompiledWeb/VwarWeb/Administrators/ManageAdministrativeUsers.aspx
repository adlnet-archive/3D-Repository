<%@ page title="" language="C#" masterpagefile="~/Administrator.master" autoeventwireup="true" inherits="Administrators_ManageAdministrativeUsers, App_Web_itrob5fo" stylesheettheme="Default" %>


<%@ Register src="../Controls/ManageAdministrativeUsers.ascx" tagname="ManageAdministrativeUsers" tagprefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="AdminContentPlaceHolder" Runat="Server">

  
  <div style="width: 790px; margin: auto;">
  
    <uc1:ManageAdministrativeUsers ID="ManageAdministrativeUsers1" runat="server" />
  
  
  </div>
  

  

</asp:Content>

