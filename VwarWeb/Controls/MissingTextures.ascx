<%--
Copyright 2011 U.S. Department of Defense

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
--%>



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

