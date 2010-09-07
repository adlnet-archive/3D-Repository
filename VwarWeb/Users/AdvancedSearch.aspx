<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AdvancedSearch.aspx.cs" Inherits="Public_AdvancedSearch" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  
    <telerik:RadAjaxManagerProxy runat="server" ID="RadAjaxManagerProxy1"></telerik:RadAjaxManagerProxy>

    <div style="width: 790px; margin: auto;">
     
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="SearchView" runat="server">

        <table cellpadding="4" cellspacing="0" border="0">
       <tr>
           <td align="right" valign="top">
               <asp:Label ID="TitleLabel" runat="server" Text="Title:"></asp:Label>
           </td>
           <td>
               <asp:TextBox ID="TitleTextBox" runat="server" Width="200px"></asp:TextBox>
           </td>
       </tr>
       <tr>
           <td align="right" valign="top">
               <asp:Label ID="Label1" runat="server" Text="Description:"></asp:Label>
           </td>
           <td>
               <asp:TextBox ID="DescriptionTextBox" runat="server" Width="200px" Rows="5" TextMode="MultiLine"></asp:TextBox>
           </td>
       </tr>
       <tr>
           <td align="right" valign="top">
               <asp:Label ID="Label2" runat="server" Text="Tags/Keywords:"></asp:Label>
           </td>
           <td>
               <asp:TextBox ID="TagsTextBox" runat="server" Width="200px" Rows="5" TextMode="MultiLine"></asp:TextBox>
           </td>
       </tr>
       <tr>
           <td align="right" valign="top">
               <asp:Label ID="Label3" runat="server" Text="Developer Name:"></asp:Label>
           </td>
           <td>
               <asp:TextBox ID="DeveloperNameTextBox" runat="server" Width="200px"></asp:TextBox>
           </td>
       </tr>
       <tr>
           <td align="right" valign="top">
               <asp:Label ID="Label4" runat="server" Text="Sponsor Name:"></asp:Label>
           </td>
           <td>
               <asp:TextBox ID="SponsorNameTextBox" runat="server" Width="200px"></asp:TextBox>
           </td>
       </tr>
       <tr>
           <td align="right" valign="top">
               <asp:Label ID="Label5" runat="server" Text="Artist Name:"></asp:Label>
           </td>
           <td>
               <asp:TextBox ID="ArtistNameTextBox" runat="server" Width="200px"></asp:TextBox>
           </td>
       </tr>
       <tr>
           <td align="right" valign="top">
               &nbsp;</td>
           <td>
               <asp:Button ID="SearchButon" runat="server" Text="Search" onclick="SearchButon_Click" />
               &nbsp;<asp:Button ID="CancelButton" runat="server" onclick="CancelButton_Click" Text="Cancel" />
           </td>
       </tr>
</table>

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
                                             <%-- <img id="Img1" style="border: 0" src='<%# Website.Common.FormatScreenshotImage(Eval("PID"), Eval("Screenshot")) %>'
                                                    alt='<%# Eval("Title") %>' runat="server" class="DisplayImage" />--%>
                                                <telerik:RadBinaryImage ID="Img1" BorderWidth="0" runat="server" AlternateText='<%# Eval("Title") %>' Width="100px" Height="100px" ResizeMode="Fit" ImageUrl='<%# String.Format("~/Public/Model.ashx?pid={0}&file={1}",Eval("PID"),Eval("Screenshot")) %>' />
                                            </a>
                                            <br />
                                             <div style="clear: both; margin: auto;">
                                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                    <tr>
                                                        <td style="width: 32%">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <ajax:Rating ID="ir" runat="server" CurrentRating='<%# Website.Common.CalculateAverageRating(Eval("PID")) %>'
                                                                MaxRating="5" StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar"
                                                                FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar" ReadOnly="false" Visible='<%# Website.Common.CalculateAverageRating(Eval("PID")) > 0 %>'>
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

