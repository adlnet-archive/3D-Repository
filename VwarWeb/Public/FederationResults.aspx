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
    CodeFile="FederationResults.aspx.cs" Inherits="Public_Results" Title="Results" %>

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

<asp:Panel ID="SearchPanel2" class="SearchContainer" runat="server" DefaultButton="SearchFederatonButton"
            Style="margin-top: 0px">
            <div align="center">
                <table cellpadding="6" cellspacing="0" border="0">
                    <tr>
                        <td style="vertical-align: bottom;">
                            <asp:TextBox CssClass="SearchTextBox" ID="SearchFederationTextBox" Width="290px" runat="server"
                                ToolTip="Enter search terms here"></asp:TextBox>
                        </td>
                        <td style="vertical-align: bottom;">
                            <asp:Button ID="SearchFederatonButton" runat="server" Text="Search the Network" CausesValidation="false"
                                ToolTip="Search for models" OnClick="SearchFederatonButton_Click"  />
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            Results Per Page: 
                            <asp:DropDownList CssClass='results-per-page-dropdown' ID="ResultsPerPageDropdown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="NumResultsPerPageChanged">
                                <asp:ListItem Text="6" Value="6" Selected="True" />
                                <asp:ListItem Text="12" Value="12" />
                                <asp:ListItem Text="24" Value="24" />
                                <asp:ListItem Text="51" Value="51" />
                                <asp:ListItem Text="99" Value="99" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
            
        </asp:Panel>

    <div style="width: 790px; margin: auto;">
        <asp:UpdatePanel ID="SearchResultsUpdatePanel" runat="server" Visible=false>
            <ContentTemplate>
                <asp:Label style='font-size: large; font-weight: bold' ID="ResultsLabel" runat="server"></asp:Label><br /><br />
                <asp:DataList class="search-list" ID="SearchList" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" EnableViewState=false>
                    <ItemTemplate>
                        <div class="model-teaser">
                            <a id="A1" runat="server" href='<%# "~/Public/Model.aspx?ContentObjectID=" + Eval("pid") %>'>
                                <asp:Image class="PreviewThumbnail" ID="Img1" BorderWidth="0" runat="server" AlternateText='<%# Eval("Title") %>'
                                    Style="padding-top: 10px; max-width: 100px; max-height: 100px;"
                                    ImageUrl='<%# "http://3dr.adlnet.gov/Federation/3DR_Federation.svc/" + Eval("pid") + "/Thumbnail?ID=00-00-00" %>' /></a>
                            <br />
                            <a id="A4" class="item-target" runat="server" href='<%# "~/Public/FederationView.aspx?ContentObjectID=" + Eval("PID") %>'
                                style="font-size: 12px; color: #0E4F9C; font-weight: bold">
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Title") %>' /></a>
                                <br />
                            <a id="A2" runat="server" href='<%# Eval("OrganizationURL") %>'
                                style=" font-size: 10px; color:Gray; font-weight:normal">
                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("OrganizationName") %>' /></a>
                            
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
    <br />
        <div id="FederationText" runat="server" style="max-width:90%;margin:0px auto auto auto">
        The 3DR federation is a partnership of organizations each hosting separate 3D Repository web services. The "Search" button above makes it possible to distribute your search across all the repositories in this network. You can download and view models from any participating organization, but you cannot upload directly to the federation. To submit your content, please upload directly to one of the partnership websites. If you upload to this web page, your content will be stored by the organization that hosts it, and may not be reflected in the federated search if the owner of this repository has not listed this site. 
        </div>
</asp:Content>
