﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RebuildThumbnailCache.aspx.cs" Inherits="Users_RebuildThumbnailCache" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
            Text="Rebuild" />
    
    </div>
    <asp:ListBox ID="ListBox1" runat="server" Height="560px" Width="557px">
    </asp:ListBox>
    </form>
</body>
</html>
