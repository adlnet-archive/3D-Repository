
<%@ Page Language="C#" MasterPageFile="~/Administrator.master" AutoEventWireup="true" CodeFile="Email Text.aspx.cs" Inherits="Administrators_Email_Text" %>
<%@ Register src="../Controls/RebuildThumbnailCache.ascx" tagname="RebuildThumbnailCache" tagprefix="uc1" %>

<asp:Content ID="Content1" runat="server" 
    contentplaceholderid="AdminContentPlaceHolder">
    <style type="text/css">
        .thead
        {
            color:White;
            background-color:#507CD1;
            font-weight:bold;
        }
        tr
        {
            width:100px;
        }
        tr:nth-child(even) 
        {
            background-color:#EFF3FB;
            width:100px;
            margin-top:10px;
        }
        tr:nth-child(odd) 
        {
            background-color:#FFFFFF;
            width:100px;
            margin-top:10px;
        }
        .Thumbtable
        {
            border-right: outset 2px blue;
            border-left: outset 2px blue;
            border-top: outset 2px blue;
            border-bottom: outset 2px blue;
            width:100%;
        }
        .pid
        {
            border-right: outset 2px blue;
            border-left: outset 2px blue;
            border-top: outset 2px blue;
            border-bottom: outset 2px blue;
            width:10%;
            text-align:center;
        }
        .Scrollpane
        {
            max-height : 500px;
            overflow : scroll;
        }
    </style>
    
    <div style="width:750px">
        <table style="width:750px">
             <tr class="thead"><td class="thead">Sent On:</td><td class="thead">New Model Uploaded </td> </tr>   
             <tr><td style="width:20%">Subject</td><td><asp:TextBox style="width:95%" ID="MySQLIP" runat="server"></asp:TextBox><img alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%">Message</td><td><asp:TextBox style="width:95%;height:300px;" ID="MySQLPort" runat="server"></asp:TextBox><img alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
        </table>
        <asp:Button ID="SaveMySQLSettings" Text="Save" runat="server"/>
        <table style="width:750px">
             <tr class="thead"><td class="thead">Sent On:</td><td class="thead">New User Registered </td> </tr>   
             <tr><td style="width:20%">Subject</td><td><asp:TextBox style="width:95%" ID="TextBox1" runat="server"></asp:TextBox><img alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%">Message</td><td><asp:TextBox style="width:95%;height:300px;" ID="TextBox2" runat="server"></asp:TextBox><img alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
        </table>
        <asp:Button ID="Button1" Text="Save" runat="server"/>
    </div>   
    
</asp:Content>