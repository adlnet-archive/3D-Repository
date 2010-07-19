<%@ page title="" language="C#" masterpagefile="~/Administrator.master" autoeventwireup="true" inherits="Administrators_Default, App_Web_itrob5fo" stylesheettheme="Default" %>

<%@ Register src="../Controls/CybrarianQueue.ascx" tagname="CybrarianQueue" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContentPlaceHolder" Runat="Server">



    <div style="width: 790px; margin: auto;">
<uc1:CybrarianQueue ID="CybrarianQueue1" runat="server" />

</div>
    

</asp:Content>

