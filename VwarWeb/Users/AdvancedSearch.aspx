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
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="SearchView" runat="server">
        <div id="SearchFormWrapper">
        
        <p style="text-align: left;"> Please fill out at least one of the following fields:</p>
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
               &nbsp;<asp:Button ID="CancelButton" runat="server" onclick="CancelButton_Click" Text="Cancel" />
           </td>
       </tr>
</table>
        </div>
        </asp:View>
        <asp:View ID="ResultsView" runat="server">
       <div style="width: 790px; margin: auto;">
         <%--<asp:DropDownList ID="sort" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ChangeSort">
            <asp:ListItem Text="Views - High To Low" Value="views-high" Selected="True"></asp:ListItem>
            <asp:ListItem Text="Views - Low To High" Value="views-low"></asp:ListItem>
            <asp:ListItem Text="Rating - High To Low" Value="rating-high"></asp:ListItem>
            <asp:ListItem Text="Rating - Low To High" Value="rating-low"></asp:ListItem>
            <asp:ListItem Text="Last Viewed - High To Low" Value="viewed-high"></asp:ListItem>
            <asp:ListItem Text="Last Viewed - Low To High" Value="viewed-low"></asp:ListItem>
            <asp:ListItem Text="Last Updated - High To Low" Value="updated-high"></asp:ListItem>
            <asp:ListItem Text="Last Updated - Low To High" Value="updated-low"></asp:ListItem>            
        </asp:DropDownList>
        <br />--%>
        <asp:DataList ID="SearchList" runat="server" RepeatColumns="4" RepeatLayout="Table"
            RepeatDirection="Horizontal" EditItemStyle-Width="100%" ItemStyle-VerticalAlign="Top">
            <ItemTemplate>
               <div style="text-align:center; margin:auto;">
                                            <a id="A1" runat="server" href='<%# "~/Public/Model.aspx?ContentObjectID=" + Eval("PID") %>'>
                                                <asp:Image ID="Img1" BorderWidth="0" runat="server" AlternateText='<%# Eval("Title") %>' Width="100px" Height="100px" ResizeMode="Fit" ImageUrl='<%# String.Format("~/Public/Model.ashx?pid={0}&file={1}",Eval("PID"),Eval("Screenshot")) %>' />
                                            </a>
                                            <br />
                                             <div style="clear: both; margin: auto;">
                                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                    <tr>
                                                        <td style="width: 32%">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <ajax:Rating ID="ir" runat="server" CurrentRating='<%# Website.Common.CalculateAverageRating(Eval("Reviews")) %>'
                                                                MaxRating="5" StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar"
                                                                FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar" ReadOnly="false" Visible='<%# Website.Common.CalculateAverageRating(Eval("Reviews")) > 0 %>'>
                                                            </ajax:Rating>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <br />
                                            <a id="A4" runat="server" href='<%# "~/Public/Model.aspx?ContentObjectID=" + Eval("PID") %>'
                                                style="font-size: 12px; color: #0E4F9C; font-weight: bold">
                                                <%# Eval("Title") %></a>

                                            <br />
                                            <br />
                                            <asp:Label ID="DescriptionLabel" runat="server" Text='<%#Eval("Description") %>'
                                                Font-Size="Small"></asp:Label><br />
                                            
                                        </div>
            </ItemTemplate>
            <ItemStyle Width="200px" HorizontalAlign="Left" />
        </asp:DataList>


        <asp:Label ID="NoneFoundLabel" runat="server" Visible="false" />
           <br />
           <asp:Button ID="NewAdvancedSearchButton" runat="server" onclick="NewAdvanceSearchButton_Click" Text="New Search" />
    </div>
        </asp:View>
    </asp:MultiView>
        
    
    
    </div>
   
   


   
</asp:Content>

