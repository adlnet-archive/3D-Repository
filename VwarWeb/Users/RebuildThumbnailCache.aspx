<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RebuildThumbnailCache.aspx.cs" Inherits="Users_RebuildThumbnailCache" %>
<%@ Register src="../Controls/RebuildThumbnailCache.ascx" tagname="RebuildThumbnailCache" tagprefix="uc1" %>
<asp:Content ID="RebuildThumbnailCacheContent" ContentPlaceHolderID="head" Runat="Server">
  
</asp:Content>
<asp:Content ID="Content1" runat="server" 
    contentplaceholderid="ContentPlaceHolder1">
    <uc1:RebuildThumbnailCache ID="RebuildThumbnailCache1" runat="server" />
</asp:Content>
