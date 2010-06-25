<%@ Page Title="Home" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="Default2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="Scripts/jquery-1.3.2.min.js"></script>

    <script type="text/javascript" src="Scripts/jquery-ui-1.7.2.custom.min.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ToolkitScriptManager ID="sm1" runat="server">
    </ajax:ToolkitScriptManager>

    <script type="text/javascript">
        $(document).ready(function() {
            $("#tabs").tabs({ select: function(event, ui) {
                try {
                    var src = "src";
                    $("#MasterPage_ContentPlaceHolder1_Image0").attr(src, "Images/P_tab.jpg");
                    $("#MasterPage_ContentPlaceHolder1_Image1").attr(src, "Images/HR_tab.jpg")
                    $("#MasterPage_ContentPlaceHolder1_Image2").attr(src, "Images/RV_tab.jpg");
                    $("#MasterPage_ContentPlaceHolder1_Image3").attr(src, "Images/RU_tab.jpg");
                    var target = $("#MasterPage_ContentPlaceHolder1_Image" + ui.index);
                    var currentSrc = target.attr(src);
                    var i = currentSrc.lastIndexOf('.');
                    target.attr(src, currentSrc.substr(0, i) + "_on" + currentSrc.substr(i));
                } catch (ex) {
                    alert(ex.message);
                }
            }
            })
        });
    </script>

    <div style="width: 100%; text-align: center;">
        <div id="tabs" style="width: 900px; margin: auto;">
            <ul>
                <li><a href="#tabs-1">
                    <asp:Image ID="Image0" ImageUrl="~/Images/P_tab_on.jpg" ToolTip="Popular" runat="server"
                        Height="55px" /></a></li>
                <li><a href="#tabs-2">
                    <asp:Image ID="Image1" ImageUrl="~/Images/HR_tab.jpg" ToolTip="Highly Rated" runat="server"
                        Height="55px" /></a></li>
                <li><a href="#tabs-3">
                    <asp:Image ID="Image2" ImageUrl="~/Images/RV_tab.jpg" ToolTip="Recently Viewed" runat="server"
                        Height="55px" /></a></li>
                <li><a href="#tabs-4">
                    <asp:Image ID="Image3" ImageUrl="~/Images/RU_tab.jpg" ToolTip="Recently Updated"
                        runat="server" Height="55px" /></a></li>
            </ul>
            <div id="tabs-1" class="ui-tabs-hide">
                <asp:ListView ID="PopularListView" runat="server">
                    <ItemTemplate>
                        <td id="Td4" runat="server" style="vertical-align: top;">
                            <ajax:Rating ID="ir" runat="server" CurrentRating='<%# Website.Common.CalculateAverageRating(Eval("PID")) %>'
                                MaxRating="5" StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar"
                                FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar" ReadOnly="true">
                            </ajax:Rating>
                            <br />
                            <a id="A4" runat="server" href='<%# "~/Public/Model.aspx?ContentObjectID=" + Eval("PID") %>'
                                class="Hyperlink" title="View Details">
                                <img id="Img4" src='<%# Website.Common.FormatScreenshotImage(Eval("PID"), Eval("ScreenShot")) %>'
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
                        </td>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        No models found.
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="Table4" runat="server" border="0" class="ListViewTable" cellpadding="3">
                            <tr id="itemPlaceholderContainer" runat="server">
                                <td id="itemPlaceholder" runat="server" style="text-align: center;">
                                </td>
                            </tr>
                        </table>
                        <table width="100%" cellpadding="10" cellspacing="0">
                            <tr>
                                <td align="right">
                                    <asp:HyperLink ID="ViewMorePopularHyperLink" runat="server" CssClass="Hyperlink"
                                        NavigateUrl="~/Public/Results.aspx?Group=views-high">View More >></asp:HyperLink>
                                </td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                </asp:ListView>
            </div>
            <div id="tabs-2" class="ui-tabs-hide">
                <asp:ListView ID="HighlyRatedListView" runat="server">
                    <ItemTemplate>
                        <td id="Td1" runat="server" style="vertical-align: top;">
                            <ajax:Rating ID="ir" runat="server" CurrentRating='<%# Website.Common.CalculateAverageRating(Eval("PID")) %>'
                                MaxRating="5" StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar"
                                FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar" ReadOnly="true">
                            </ajax:Rating>
                            <br />
                            <a id="A1" runat="server" href='<%# "~/Public/Model.aspx?ContentObjectID=" + Eval("PID") %>'
                                class="Hyperlink">
                                <img id="Img1" src='<%# Website.Common.FormatScreenshotImage(Eval("PID"), Eval("Screenshot")) %>'
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
                        </td>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        No models found.
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="Table1" runat="server" border="0" style="" class="ListViewTable" cellpadding="3">
                            <tr id="itemPlaceholderContainer" runat="server">
                                <td id="itemPlaceholder" runat="server">
                                </td>
                            </tr>
                        </table>
                        <table width="100%" cellpadding="10" cellspacing="0">
                            <tr>
                                <td align="right">
                                    <asp:HyperLink ID="ViewMoreHighlyRatedHyperLink" runat="server" CssClass="Hyperlink"
                                        NavigateUrl="~/Public/Results.aspx?Group=rating-high">View More >></asp:HyperLink>
                                </td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                </asp:ListView>
            </div>
            <div id="tabs-3" class="ui-tabs-hide">
                <asp:ListView ID="RecentlyViewedListView" runat="server">
                    <ItemTemplate>
                        <td id="Td2" runat="server" style="vertical-align: top;">
                            <ajax:Rating ID="ir" runat="server" CurrentRating='<%# Website.Common.CalculateAverageRating(Eval("PID")) %>'
                                MaxRating="5" StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar"
                                CssClass="review" FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar"
                                ReadOnly="true">
                            </ajax:Rating>
                            <br />
                            <a id="A2" runat="server" href='<%# "~/Public/Model.aspx?ContentObjectID=" + Eval("PID") %>'
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
                        </td>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        No models found.
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="Table2" runat="server" border="0" style="" class="ListViewTable" cellpadding="3">
                            <tr id="itemPlaceholderContainer" runat="server">
                                <td id="itemPlaceholder" runat="server">
                                </td>
                            </tr>
                        </table>
                        <table width="100%" cellpadding="10" cellspacing="0">
                            <tr>
                                <td align="right">
                                    <asp:HyperLink ID="ViewMoreRecentlyViewedHyperLink" runat="server" CssClass="Hyperlink"
                                        NavigateUrl="~/Public/Results.aspx?Group=viewed-high">View More >></asp:HyperLink>
                                </td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                </asp:ListView>
            </div>
            <div id="tabs-4" class="ui-tabs-hide">
                <asp:ListView ID="RecentlyUpdatedListView" runat="server">
                    <ItemTemplate>
                        <td id="Td3" runat="server" style="vertical-align: top;">
                            <ajax:Rating ID="ir" runat="server" CurrentRating='<%# Website.Common.CalculateAverageRating(Eval("PID")) %>'
                                MaxRating="5" StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar"
                                CssClass="review" FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar"
                                ReadOnly="true">
                            </ajax:Rating>
                            <br />
                            <a id="A3" runat="server" href='<%# "~/Public/Model.aspx?ContentObjectID=" + Eval("PID") %>'
                                class="Hyperlink">
                                <img id="Img3" src='<%# Website.Common.FormatScreenshotImage(Eval("PID"), Eval("Screenshot")) %>'
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
                        </td>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        No models found.
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="Table3" runat="server" border="0" style="" class="ListViewTable" cellpadding="3">
                            <tr id="itemPlaceholderContainer" runat="server">
                                <td id="itemPlaceholder" runat="server">
                                </td>
                            </tr>
                        </table>
                        <table width="100%" cellpadding="10" cellspacing="0">
                            <tr>
                                <td align="right">
                                    <asp:HyperLink ID="ViewMoreRecentlyUpdatedHyperLink" runat="server" CssClass="Hyperlink"
                                        NavigateUrl="~/Public/Results.aspx?Group=updated-high">View More >></asp:HyperLink>
                                </td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                </asp:ListView>
            </div>
        </div>
    </div>
</asp:Content>
