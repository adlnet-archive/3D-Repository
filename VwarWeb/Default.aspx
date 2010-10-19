<%@ Page Title="Home" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="Default2" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<script type="text/javascript" src="Scripts/jquery-1.3.2.min.js"></script>

    <script type="text/javascript" src="Scripts/jquery-ui-1.7.2.custom.min.js"></script>--%>

     <style type="text/css">
  
    .radRotatoritemTemplate
        {
            height: 200px;
            width: 170px;     
            float:left;
            margin: 40px 0;
        }
        
        
    
   .rrClipRegion { border:0; vertical-align:middle;} 
   
   .rrItem
    {
       text-align:center;
    }

   .rtsSelected
   {
      border-top: 1px solid;
   }

   
   .rotatorView
   {
       background-color: White; 
       border: 1px solid #7f7f7f;
       width: 864px;
       margin: 16px auto;
   }
   
   .rrItemsList
   {
       height: 250px;
   }
   
   #outerBoxWrapper
   {
       width: 900px;
       z-index: -1;
       margin-left: auto;
       margin-right: auto;
       position: relative;
       border: 1px solid;
       top: -1px; 
       background:none repeat scroll 0 0 #FFFFFF;
   }
   
   #outerBoxBody
   {
       margin-left: auto;
       margin-right: auto;
       width: 866px;


   } 


   
  </style>
   

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadAjaxLoadingPanel ID="MultiPageLoadingPanel" runat="server" Skin="WebBlue"></telerik:RadAjaxLoadingPanel>
   <%-- <telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ModelBrowseMultiPage">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ModelBrowseMultiPage" LoadingPanelID="MultiPageLoadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>--%> 


        <script type="text/javascript">var ie = 0;</script>
        <!--[if IE]>
        <script type="text/javascript">ie = 1;</script>
        <![endif]--> 


     <script type="text/javascript">
      /*  $(function () {
            $('#tabs').tabs();
            if (!ie) {
                $("li").css("margin-left", "1px");
            }
        });*/

        function onTabSelecting(sender, args)
        {
            if (args.get_tab().get_pageViewID())
            {
                args.get_tab().set_postBack(false);
            }
        }

    </script>



   

        <div id="tabs" style="width: 900px; margin: auto; position: relative; z-index: 1;">

           <%--  <div id="tab_buttons" align="left" style="margin-left: 0px">
            <asp:ImageButton ID="highlyRated" ImageUrl="~/Images/Homepage Pieces/button_highlyrated_off.png" ToolTip="Highly Rated Models in the 3D Repository" runat="server" CssClass="tabs" OnClientClick="return showTab('highlyRated')"/>
            <asp:ImageButton ID="recentlyViewed" ImageUrl="~/Images/Homepage Pieces/button_recentlyViewed_on.png" ToolTip="Popular models with lots of recent views" 
                             runat="server" CssClass="Tabs" OnClientClick="return showTab('recentlyViewed')"  style="margin-left:11px" />
            <asp:ImageButton ID="recentlyUpdated" ImageUrl="~/Images/Homepage Pieces/button_recentlyUpdated_off.png" ToolTip="Models that have been recently updated" runat="server" CssClass="Tabs" OnClientClick="return showTab('recentlyUpdated')" style="margin-left: 8px" />

            </div>--%>
            <span class="homeTabStrip">
            <telerik:RadTabStrip runat="server" ID="TabStrip" Orientation="HorizontalTop"
                                 Skin="WebBlue" SelectedIndex="0" MultiPageID="ModelBrowseMultiPage" OnTabClick="TabStrip_TabClick"
                                 OnClientTabSelecting="onTabSelecting">
            </telerik:RadTabStrip>
            </span>
            
                <div id="outerBoxWrapper">
                    <div id="outerBoxBody" >
                        
                    <telerik:RadAjaxPanel ID="RadAjaxPanel1" LoadingPanelID="MultiPageLoadingPanel"  runat="server" >
                        <telerik:RadMultiPage runat="server" ID="ModelBrowseMultiPage" SelectedIndex="0" 
                            OnPageViewCreated="ModelBrowseMultiPage_PageViewCreated" >
                        </telerik:RadMultiPage>
                         <asp:Timer ID="LoadDelayTimer" Interval="1" OnTick="BindMultiPageData" runat="server"/>
            </telerik:RadAjaxPanel>
                    </div>
                </div>
               
                
        </div>
    
</asp:Content>
