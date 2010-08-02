<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MissingTextures.ascx.cs" Inherits="Controls_MissingTextures" %>



    
<table cellpadding="4" cellspacing="0" border="0">
    <tr><td colspan="5">
    
        <asp:Label ID="MessageLabel" runat="server" Text=""></asp:Label>
    </td>
   
    </tr>

    <tr>
        <td>
    Current Texture:</td>
        <td>
            <asp:Label runat="server" ID="oldFileName"></asp:Label>
        </td>
        <td>
            &nbsp;<b>OR</b>&nbsp;</td>
        <td>
            Upload:
        </td>
        <td>
            <asp:FileUpload ID="FileUpload1" runat="server" Width="200px" />
        </td>
        
    </tr>
    
</table>

