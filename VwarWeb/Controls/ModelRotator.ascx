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
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ModelRotator.ascx.cs" Inherits="VwarWeb.Controls_ModelRotator" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<script type="text/javascript">
    $('.PreviewThumbnail').error(function () {
        $(this).attr("src", "styles/images/nopreview_icon.png");
    });
</script>
<table id="RotatorLayoutTable" runat="server" border="0" width="100%">
    <tr id="RotatorListViewRow" runat="server">
        <td align="center" id="RotatorListViewColumn">
            <asp:DataList RepeatDirection="Horizontal" ID="RotatorListView" runat="server" Height="225px"
                Width="705" onselectedindexchanged="RotatorListView_SelectedIndexChanged">
                <ItemTemplate>
                    <div class="model-teaser">
                        <a id="A1" runat="server" href='<%# "~/Public/Model.aspx?ContentObjectID=" + Eval("PID") %>'>
                            <asp:Image class="PreviewThumbnail" ID="Img1" BorderWidth="0" runat="server" AlternateText='<%# Eval("Title") %>'
                                Width="100" Height="100" Style="padding-top: 10px"
                                ImageUrl='<%#"~/Public/Serve.ashx?mode=GetThumbnail&pid=" + Eval("PID") %>' />
                        </a>
                        <br />
                        <div style="width: 70px; margin: 0 auto;">
                            <ajax:rating id="ir" runat="server" currentrating='<%# Website.Common.CalculateAverageRating(Eval("Reviews")) %>'
                                maxrating="5" starcssclass="ratingStar" waitingstarcssclass="savedRatingStar"
                                filledstarcssclass="filledRatingStar" emptystarcssclass="emptyRatingStar" readonly="true">
                                                </ajax:rating>
                        </div>
                        <br />
                        <a id="A4" class="item-target" runat="server" href='<%# "~/Public/Model.aspx?ContentObjectID=" + Eval("PID") %>'
                            style="font-size: 12px; color: #0E4F9C; font-weight: bold">
                            <asp:Label runat="server" Text='<%# Eval("Title") %>' /></a>
                        <br />
                        <div style="margin-left: 5px; margin-right: 5px;">
                            <asp:Label ID="DescriptionLabel" runat="server" Text='<%#
                                                    (!String.IsNullOrEmpty((String)Eval("Description"))) ? FormatDescription((string)Eval("Description")) : "No description available" %>'
                                Font-Size="Small" Style="width: 170px; word-wrap: break-word;"></asp:Label><br />
                        </div>
                        <asp:Label ID="NumViewsLabel" Style="font-size: 11px; color: Gray" runat="server">
                                                <%# ((vwarDAL.ContentObject)Container.DataItem).Views.ToString() %> Views
                        </asp:Label>
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </td>
    </tr>
    <tr>
        <td colspan="3" align="center">
            <asp:HyperLink ID="ViewMoreHyperLink" runat="server" Style="font-size: 12px; color: #0E4F9C;"
                NavigateUrl="~/Public/Results.aspx?Group=rating-high">View All >></asp:HyperLink>
        </td>
    </tr>
</table>
