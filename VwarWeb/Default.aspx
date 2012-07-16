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



<%@ Page Title="Welcome | 3D Repository" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="Default2" %>

<%@ Register TagPrefix="VwarWeb" TagName="ModelRotator" Src="Controls/ModelRotator.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta property="og:title" content="Welcome to 3DR" />
    <meta property="og:description" content="A (free!) platform for sharing 3D content across a variety of formats." />
    <meta property="og:image" content="http://3dr.adlnet.gov/styles/images/meta_image.png" />
    <script type="text/javascript" src="Scripts/jquery-ui-1.8.7.custom.min.js"></script>
    
    <link href="styles/tabs-custom.css" rel="Stylesheet" type="text/css" />
 <style type="text/css">
    .popout
    {
        top: -5px;
        left: 5px;
    }
   	.ui-widget-content
    {
       border: none;
    }
    .tagstable td
    {
        width:20%; text-align:center;
    }
     .tagstable tbody
    {
        width:100%;
    }
     .tagstable table
    {
        width:100%;
    }
     .tagstable tr
    {
        width:100%;
    }
  </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $("#HomepageTabs").tabs()
             .find("li").last().append(
                $("<span />").addClass("ui-tabs-separator")
                     .addClass("blue")
                     .addClass("last")
             );

             $(".model-teaser").registerMouseOverAnimation();
         });
     </script>

        <div id="HomepageTabs" style="width: 900px; margin: auto; position: relative; z-index: 1;">
            <ul class="tabContainer">
            <li class="first"><span class="ui-tabs-separator"></span><a href="#RandomView"><img class="tabIcon" src="styles/images/Homepage Pieces/icon_random.png" alt="Random Models" /> Random Models </a></li>
                <li><span class="ui-tabs-separator"></span><a href="#MostPopularView"><img class="tabIcon" src="styles/images/Homepage Pieces/icon_mostPopular.png" alt="Most Popular Models" /> Most Popular </a></li>
                <li><span class="ui-tabs-separator"></span><a href="#RecentlyUpdatedView"><img class="tabIcon" src="styles/images/Homepage Pieces/icon_recentlyUpdated.png"  alt="Recently Updated Models" /> Recently Updated </a></li>
                <li><span class="ui-tabs-separator"></span><a href="#HighestRatedView"><img class="tabIcon" src="styles/images/Homepage Pieces/icon_highlyRated.png" alt="Highly Rated" /> Highly Rated </a></li>
                <li><span class="ui-tabs-separator"></span><a href="#PopularTagsView"><img class="tabIcon" src="styles/images/Icons/tag.png" alt="Highly Rated" /> Popular Tags </a></li>
                <li><span class="ui-tabs-separator"></span><a href="#PopularDevelopersView"><img class="tabIcon" src="styles/images/Icons/icon_pencil.png" alt="Highly Rated" /> Contributors </a></li>
            </ul>
            <div id="RandomView">
                <VwarWeb:ModelRotator ID="RandomRotator" runat="server" />
            </div>
            <div id="MostPopularView">
                <VwarWeb:ModelRotator ID="MostPopularRotator" runat="server" />
            </div>
            <div id="RecentlyUpdatedView">
                <VwarWeb:ModelRotator ID="RecentlyUpdatedRotator" runat="server" />
            </div>
            <div id="HighestRatedView">
                <VwarWeb:ModelRotator ID="HighestRatedRotator" runat="server" />
            </div>
            <div id="PopularTagsView" class="tagstable">
               <asp:DataList RepeatDirection="Horizontal" RepeatLayout="Table" ID="PopularTagsList" RepeatColumns="5" runat="server" Height="225px"
                Width="100%">
                <ItemTemplate>
                        <asp:HyperLink ID="NumViewsLabel" Style="font-size: 13px; color: DarkBlue; font-weight:bold" runat="server" Text="<%# Container.DataItem.ToString() %>" NavigateUrl =' <%# "~/Public/results.aspx?Keywords=" + Container.DataItem.ToString().Substring(0,   Container.DataItem.ToString().IndexOf(" (")    ) + "&Method=or" %>'>
                                                
                        </asp:HyperLink>
                </ItemTemplate>
            </asp:DataList>
            </div>
             <div id="PopularDevelopersView" class="tagstable">
               <asp:DataList RepeatDirection="Horizontal" RepeatLayout="Table" ID="PopularDevelopersList" RepeatColumns="3" runat="server" Height="225px"
                Width="100%">
                <ItemTemplate>
                        <asp:HyperLink ID="NumViewsLabel" Style="font-size: 13px; color: DarkBlue; font-weight:bold" runat="server" Text="<%# Container.DataItem.ToString() %>" NavigateUrl =' <%# "~/Public/results.aspx?SponsorName=" + Container.DataItem.ToString().Substring(0,   Container.DataItem.ToString().IndexOf(" (")    ) + "&DeveloperName=" + Container.DataItem.ToString().Substring(0,   Container.DataItem.ToString().IndexOf(" (")    ) + "&ArtistName=" + Container.DataItem.ToString().Substring(0,   Container.DataItem.ToString().IndexOf(" (")    ) + "&Method=or" %>'>
                                                 
                        </asp:HyperLink>
                </ItemTemplate>
            </asp:DataList>
            </div>

        </div>
    
</asp:Content>

