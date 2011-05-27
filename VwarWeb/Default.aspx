<%@ Page Title="Welcome | 3D Repository" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="Default2" %>

<%@ Register TagPrefix="VwarWeb" TagName="ModelRotator" Src="Controls/ModelRotator.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta property="og:title" content="Welcome to 3DR" />
    <meta property="og:description" content="A (free!) platform for sharing 3D content across a variety of formats." />
    <meta property="og:image" content="http://3dr.adlnet.gov/Images/meta_image.png" />
    <script type="text/javascript" src="Scripts/jquery-ui-1.8.7.custom.min.js"></script>
    
    <link href="Stylesheets/tabs-custom.css" rel="Stylesheet" type="text/css" />
 <style type="text/css">
  
    .radRotatoritemTemplate
    {
       width: 200px;  
       height: 200px;   
       float:left;
       margin: 20px 5px;
       text-align: center;
       background: none;
       position: relative;
       cursor: pointer;
    }
    
    .with-background
    {
        background: url("Images/outer-box.png")
    }
    
    .popout
    {
        top: -5px;
        left: 5px;
    }
    
   	.ui-widget-content
   {
       border: none;
   }

  </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $("#tabs").tabs()
             .find("li").last().append(
                $("<span />").addClass("ui-tabs-separator")
                     .addClass("blue")
                     .addClass("last")
             );

             $(".radRotatoritemTemplate").bind('mouseenter', function (event) {
                 $(this).addClass('with-background')
                        .animate({
                           left: '+=5',
                           top: '-=5'
                        }, 100);
             }).bind('mouseleave', function (event) {
                 $(this).removeClass('with-background')
                        .animate({
                           left: '-=5',
                           top: '+=5'
                        }, 100);
             }).click(function (event) {
                 event.preventDefault();
                 window.location.href = $(this).find(".item-target").attr("href");
             });
         });
     </script>

        <div id="tabs" style="width: 900px; margin: auto; position: relative; z-index: 1;">
            <ul class="tabContainer">
                <li class="first"><span class="ui-tabs-separator"></span><a href="#MostPopularView"><img class="tabIcon" src="Images/Homepage Pieces/icon_mostPopular.png" /> Most Popular </a></li>
                <li><span class="ui-tabs-separator"></span><a href="#RecentlyUpdatedView"><img class="tabIcon" src="Images/Homepage Pieces/icon_recentlyUpdated.png" /> Recently Updated </a></li>
                <li><span class="ui-tabs-separator"></span><a href="#HighestRatedView"><img class="tabIcon" src="Images/Homepage Pieces/icon_highlyRated.png" /> Highly Rated </a></li>
            </ul>
            <div id="MostPopularView">
                <VwarWeb:ModelRotator ID="MostPopularRotator" runat="server" />
            </div>
            <div id="RecentlyUpdatedView">
                <VwarWeb:ModelRotator ID="RecentlyUpdatedRotator" runat="server" />
            </div>
            <div id="HighestRatedView">
                <VwarWeb:ModelRotator ID="HighestRatedRotator" runat="server" />
            </div>

        </div>
    
</asp:Content>

