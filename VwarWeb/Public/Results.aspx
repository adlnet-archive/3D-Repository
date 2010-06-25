<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Results.aspx.cs" Inherits="Public_Results" Title="Results" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ToolkitScriptManager ID="sm1" runat="server">
    </ajax:ToolkitScriptManager>
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
        <asp:DataList ID="SearchList" runat="server" RepeatColumns="4" RepeatLayout="Table"
            RepeatDirection="Horizontal" EditItemStyle-Width="100%" ItemStyle-VerticalAlign="Top">
            <ItemTemplate>
                <div style="vertical-align: top;">
                    <ajax:Rating ID="ir" runat="server" CurrentRating='<%# Website.Common.CalculateAverageRating(Eval("PID")) %>'
                        MaxRating="5" StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar"
                        FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar" ReadOnly="true">
                    </ajax:Rating>
                    <a id="A2" runat="server" style="vertical-align: top;" href='<%# "~/Public/Model.aspx?ContentObjectID=" + Eval("PID") %>'
                        class="Hyperlink">
                        <img id="Img2" src='<%# Website.Common.FormatScreenshotImage(Eval("PID"), Eval("Screenshot")) %>'
                            alt='<%# Eval("Title") %>' runat="server" class="DisplayImage" />
                        <br />
                        <%# Eval("Title") %>
                    </a>
                    <br />
                    <asp:Label ID="DescriptionLabel" runat="server" Text='<%#Eval("Description") %>'
                        Font-Size="Small"></asp:Label>
                    <br />
                    <br />
                    <span style="font-size: x-small">Uploaded by:
                        <br />
                        <asp:HyperLink ID="SubmitterEmailHyperLink" runat="server" Text='<%# Website.Common.GetFullUserName( Eval("SubmitterEmail")) %>'
                            CssClass="Hyperlink" ToolTip='<%# Eval("SubmitterEmail") %>' NavigateUrl='<%# "~/Public/Results.aspx?SubmitterEmail=" + Eval("SubmitterEmail") %>' />
                    </span>
                </div>
                <br />
            </ItemTemplate>
            <ItemStyle Width="200px" HorizontalAlign="Center" />
        </asp:DataList>
        <asp:Label ID="NoneFoundLabel" runat="server" Visible="false">No models were found.</asp:Label>
    </div>
</asp:Content>
