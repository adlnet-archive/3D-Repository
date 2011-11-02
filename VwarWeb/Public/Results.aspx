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

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Results.aspx.cs" Inherits="Public_Results" Title="Results" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .page-number.selected 
        {
            color: #000000;
            cursor: default !important;
            text-decoration: none;
            font-weight: bold;
        }
        
        #PagingControls
        {
            width: 500px;
            margin: 30px auto 0;
            text-align: center;
            position: relative;
        }
        
        .search-list { width: 900px; margin: 0 auto; }
        
        .previous-page-button, .next-page-button { position: absolute; bottom: 0 }
        .previous-page-button { left: 0; }
        .next-page-button { right: 0; }
    </style>
    <script type="text/javascript">
        $(function () {
            $(".page-number.selected").click(function (event) {
                event.preventDefault();
                return false;
            });
            $(".model-teaser").registerMouseOverAnimation();

            $(".page-number, .previous-page-button, .next-page-button").live('click', fadeOutResults);

            $(".results-per-page-dropdown",
              ".sort-dropdown").live('change', fadeOutResults);

            UpdateSelectedPageNumber("1");
        });
        function UpdateSelectedPageNumber(newSelection) {
            $('.PreviewThumbnail').error(function () {
                $(this).attr("src", "../styles/images/nopreview_icon.png");
            });
            
            $(".page-number.selected").removeClass("selected");
            $(".page-number")
            .filter(function () {
                return $(this).text() == newSelection;
            })
            .addClass("selected");
        }
        function fadeOutResults() { $(".search-list").fadeOut(200); $(window).scrollTop(0); }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 790px; margin: auto;">
        <asp:UpdatePanel ID="SearchResultsUpdatePanel" runat="server">
            <ContentTemplate>
                
                Sort By: 
                <asp:DropDownList CssClass='sort-dropdown' ID="sort" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RefreshSearch">
                    <asp:ListItem Text="Views - High To Low" Value="views-high" Selected="True">
                    </asp:ListItem>
                    <asp:ListItem Text="Views - Low To High" Value="views-low">
                    </asp:ListItem>
                    <asp:ListItem Text="Rating - High To Low" Value="rating-high">
                    </asp:ListItem>
                    <asp:ListItem Text="Rating - Low To High" Value="rating-low">
                    </asp:ListItem>
                    <asp:ListItem Text="Last Viewed - High To Low" Value="viewed-high">
                    </asp:ListItem>
                    <asp:ListItem Text="Last Viewed - Low To High" Value="viewed-low">
                    </asp:ListItem>
                    <asp:ListItem Text="Last Updated - High To Low" Value="updated-high">
                    </asp:ListItem>
                    <asp:ListItem Text="Last Updated - Low To High" Value="updated-low">
                    </asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                Results Per Page: 
                <asp:DropDownList CssClass='results-per-page-dropdown' ID="ResultsPerPageDropdown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="NumResultsPerPageChanged">
                    <asp:ListItem Text="6" Value="6" Selected="True" />
                    <asp:ListItem Text="12" Value="12" />
                    <asp:ListItem Text="24" Value="24" />
                    <asp:ListItem Text="51" Value="51" />
                    <asp:ListItem Text="99" Value="99" />
                </asp:DropDownList>
                <br />
                <br />
                <asp:Label style='font-size: large; font-weight: bold' ID="ResultsLabel" runat="server"></asp:Label><br /><br />
                <asp:DataList class="search-list" ID="SearchList" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                    <ItemTemplate>
                        <div class="model-teaser">
                            <a id="A1" runat="server" href='<%# "~/Public/Model.aspx?ContentObjectID=" + Eval("PID") %>'>
                                <asp:Image class="PreviewThumbnail" ID="Img1" BorderWidth="0" runat="server" AlternateText='<%# Eval("Title") %>'
                                    Style="padding-top: 10px; max-width: 100px; max-height: 100px;"
                                    ImageUrl='<%# (!String.IsNullOrEmpty((string)Eval("ThumbnailId"))) ? System.IO.Path.Combine("~/thumbnails",((vwarDAL.ContentObject)Container.DataItem).ThumbnailId) : "styles/images/nopreview_icon.png" %>' /></a>
                            <br />
                            <div style="width: 70px; margin: 0 auto;">
                                <ajax:Rating ID="ir" runat="server" CurrentRating='<%# Website.Common.CalculateAverageRating(Eval("Reviews")) %>'
                                    MaxRating="5" StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar"
                                    FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar" ReadOnly="true">
                                </ajax:Rating>
                            </div>
                            <br />
                            <a id="A4" class="item-target" runat="server" href='<%# "~/Public/Model.aspx?ContentObjectID=" + Eval("PID") %>'
                                style="font-size: 12px; color: #0E4F9C; font-weight: bold">
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Title") %>' /></a>
                            <br />
                            <div style="margin-left: 5px; margin-right: 5px;">
                                <asp:Label ID="DescriptionLabel" runat="server" Text='<%#
                                                    (!String.IsNullOrEmpty((String)Eval("Description"))) ? Website.Common.FormatDescription((string)Eval("Description")) : "No description available" %>'
                                    Font-Size="Small" Style="width: 170px; word-wrap: break-word;"></asp:Label><br />
                            </div>
                            <asp:Label ID="NumViewsLabel" Style="font-size: 11px; color: Gray" runat="server">
                                                <%# ((vwarDAL.ContentObject)Container.DataItem).Views.ToString() %> Views
                            </asp:Label>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
                <asp:Label ID="NoneFoundLabel" runat="server" Visible="false" />
                <div id="PagingControls">
                    <asp:LinkButton CssClass="previous-page-button" ID="PreviousPageButton" OnClick="PageNumberChanged" runat="server">Previous</asp:LinkButton>
                    <div style="width: 300px; margin: 0 auto;">
                        <asp:Repeater ID="PageNumbersRepeater" runat="server">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="page-number" OnClick="PageNumberChanged" CommandArgument='<%# Container.DataItem.ToString() %>'>
                                    <%# Container.DataItem.ToString() %>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:Repeater> 
                    </div>
                    <asp:LinkButton CssClass="next-page-button" ID="NextPageButton" OnClick="PageNumberChanged" runat="server">Next</asp:LinkButton>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
