<%@ Page Language="C#" MasterPageFile="~/Administrator.master" AutoEventWireup="true" CodeFile="LRIntegration.aspx.cs" Inherits="Administrators_LRIntegration" %>
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
        }
        tr:nth-child(odd) 
        {
            background-color:#FFFFFF;
            width:100px;
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
             <tr class="thead"><td class="thead">SQL Settings</td><td class="thead"> </td> </tr>   
             <tr><td style="width:20%"> GNUPG Key ID</td><td><asp:TextBox style="width:95%" ID="GNUPG_Key_ID" runat="server"></asp:TextBox><img alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> GNUPG Key Passphrase</td><td><asp:TextBox TextMode="Password" style="width:95%" ID="GNUPG_Key_Passphrase1" runat="server"></asp:TextBox><img alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> Confirm Passphrase</td><td><asp:TextBox TextMode="Password" style="width:95%" ID="GNUPG_Key_Passphrase2" runat="server"></asp:TextBox><img alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> GNUPG Location</td><td><asp:TextBox style="width:95%" ID="GNUPG_Location" runat="server"></asp:TextBox><img alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> GNUPG Public Key URL</td><td><asp:TextBox  style="width:95%" ID="GNUPG_Public_Key_URL" runat="server"></asp:TextBox><img alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> Submitter Name</td><td><asp:TextBox  style="width:95%" ID="Submitter_Name" runat="server"></asp:TextBox><img alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> Signer Name</td><td><asp:TextBox  style="width:95%" ID="Signer_Name" runat="server"></asp:TextBox><img alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> API Base URL</td><td><asp:TextBox  style="width:95%" ID="APIBaseURL" runat="server"></asp:TextBox><img alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> LR Publish URL</td><td><asp:TextBox  style="width:95%" ID="PublishURL" runat="server"></asp:TextBox><img alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> LR Node Username</td><td><asp:TextBox  style="width:95%" ID="LRNodeUsername" runat="server"></asp:TextBox><img alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> LR Node Password</td><td><asp:TextBox  TextMode="Password" style="width:95%" ID="LRNodePassword1" runat="server"></asp:TextBox><img alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> Confirm Password</td><td><asp:TextBox  TextMode="Password" style="width:95%" ID="LRNodePassword2" runat="server"></asp:TextBox><img alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
        </table>
        <asp:Button ID="SaveLRSettings" Text="Save" runat="server"/>
        <asp:Button ID="TestSettings" Text="Test" runat="server"/>
    </div>   
    
</asp:Content>