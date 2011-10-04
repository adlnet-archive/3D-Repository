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



<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AdvancedSearch.aspx.cs" Inherits="Public_AdvancedSearch" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
    #SearchFormWrapper
    {
        margin-left: auto;
        margin-right: auto;
        width: 790px
    }
    .style1
    {
        width: 130px;
    }
    .style2
    {
        width: 212px;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  

    <div style="width: 790px; margin-left: auto; margin-right: auto">
    <div class="ListTitle">Advanced Search</div>
        <div id="SearchFormWrapper">
        
       <p style="text-align: left;"> I want to find models that match 
        <asp:DropDownList ID="MethodSelectorDropDown" runat="server" style="vertical-align: middle;">
            <asp:ListItem Selected="True" Text="any" Value="or" />
            <asp:ListItem Text="all" Value="and" />
        </asp:DropDownList>&nbsp;
         of the following criteria:
       </p>
       <table cellpadding="4" cellspacing="0" border="0">
       <tr>
           <td align="right" valign="top" class="style2">
               <asp:Label ID="TitleLabel" runat="server" Text="Title:" CssClass="Bold"></asp:Label>
           </td>
           <td>
               <asp:TextBox ID="TitleTextBox" runat="server" Width="350px"></asp:TextBox>
           </td>
       </tr>
       <tr>
           <td align="right" valign="top" class="style2">
               <asp:Label ID="Label1" runat="server" Text="Description:" CssClass="Bold"></asp:Label>
           </td>
           <td>
               <asp:TextBox ID="DescriptionTextBox" runat="server" Width="350px" Rows="5" TextMode="MultiLine"></asp:TextBox>
           </td>
       </tr>
       <tr>
           <td align="right" valign="top" class="style2">
               <asp:Label ID="Label2" runat="server" Text="Tags/Keywords:" CssClass="Bold"></asp:Label>
           </td>
           <td>
               <asp:TextBox ID="TagsTextBox" runat="server" Width="350px" Rows="5" TextMode="MultiLine"></asp:TextBox>
           </td>
       </tr>
       <tr>
           <td align="right" valign="top" class="style2">
               <asp:Label ID="Label3" runat="server" Text="Developer Name:" CssClass="Bold"></asp:Label>
           </td>
           <td>
               <asp:TextBox ID="DeveloperNameTextBox" runat="server" Width="350px"></asp:TextBox>
           </td>
       </tr>
       <tr>
           <td align="right" valign="top" class="style2">
               <asp:Label ID="Label4" runat="server" Text="Sponsor Name:" CssClass="Bold"></asp:Label>
           </td>
           <td>
               <asp:TextBox ID="SponsorNameTextBox" runat="server" Width="350px"></asp:TextBox>
           </td>
       </tr>
       <tr>
           <td align="right" valign="top" class="style2">
               <asp:Label ID="Label5" runat="server" Text="Artist Name:" CssClass="Bold"></asp:Label>
           </td>
           <td>
               <asp:TextBox ID="ArtistNameTextBox" runat="server" Width="350px"></asp:TextBox>
           </td>
       </tr>
       <tr>
           <td align="right" valign="top" class="style2">
               &nbsp;</td>
           <td>
               <asp:Button ID="SearchButon" runat="server" Text="Find Models" onclick="SearchButon_Click" />
           </td>
       </tr>
</table>
        </div>
    </div>
</asp:Content>

