<%@ control language="C#" autoeventwireup="true" inherits="Controls_MissingTextures, App_Web_eepm41e1" %>



    
<table cellpadding="4" cellspacing="0" border="0">
    <tr><td colspan="5">
    
        <asp:Label ID="MessageLabel" runat="server" Text=""></asp:Label>
    </td>
   
    </tr>

    <tr>
        <td>
    Current Texture:</td>
        <td>
    <asp:DropDownList ID="DropDownList1" runat="server">
        <asp:ListItem>Texture1</asp:ListItem>
        <asp:ListItem>Texture2</asp:ListItem>
    </asp:DropDownList>
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

