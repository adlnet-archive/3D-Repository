<%@ Page Language="C#" MasterPageFile="~/Administrator.master" AutoEventWireup="true" CodeFile="RebuildThumbnailCache.aspx.cs" Inherits="Users_RebuildThumbnailCache" %>
<%@ Register src="../Controls/RebuildThumbnailCache.ascx" tagname="RebuildThumbnailCache" tagprefix="uc1" %>
<%@ Register src="~/Bin/PasswordTextBox.dll" tagname="PasswordTextBox" tagprefix="opp" %>
<asp:Content ID="Content1" runat="server" 
    contentplaceholderid="AdminContentPlaceHolder">
    <uc1:RebuildThumbnailCache ID="RebuildThumbnailCache1" runat="server" />
</asp:Content>


