<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Public_Results, App_Web_rwxwk2ol" title="Results" stylesheettheme="Default" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 
    <telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
    </telerik:RadAjaxManagerProxy>
    <div style="width: 790px; margin: auto;">
        <asp:DropDownList ID="sort" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ChangeSort">
            <asp:ListItem Text="Views - High To Low" Value="views-high" Selected="True"></asp:ListItem>
            <asp:ListItem Text="Views - Low To High" Value="views-low"></asp:ListItem>
            <asp:ListItem Text="Rating - High To Low" Value="rating-high"></asp:ListItem>
            <asp:ListItem Text="Rating - Low To High" Value="rating-low"></asp:ListItem>
            <asp:ListItem Text="Last Viewed - High To Low" Value="viewed-high"></asp:ListItem>
            <asp:ListItem Text="Last Viewed - Low To High" Value="viewed-low"></asp:ListItem>
            <asp:ListItem Text="Last Updated - High To Low" Value="updated-high"></asp:ListItem>
            <asp:ListItem Text="Last Updated - Low To High" Value="updated-low"></asp:ListItem>            
        </asp:DropDownList>
        <br />
        <br />
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
<EditItemStyle Width="100%"></EditItemStyle>

            <ItemStyle Width="200px" HorizontalAlign="Left" />
        </asp:DataList>
        <asp:Label ID="NoneFoundLabel" runat="server" Visible="false" /><br />
        <asp:Button ID="BackButton" runat="server" Text="< Back" onclick="BackButton_Click" />
    </div>
</asp:Content>
