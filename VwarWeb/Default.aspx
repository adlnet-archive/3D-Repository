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
        }
        
        
        
   .rrClipRegion { border:0; vertical-align:middle;} 
   
   .rrItem
    {
       
       
        text-align:center;
    }

   
   
  </style>
   

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
    </telerik:RadAjaxManagerProxy>


        <script type="text/javascript">var ie = 0;</script>
        <!--[if IE]>
        <script type="text/javascript">ie = 1;</script>
        <![endif]--> 
    <script type="text/javascript">
        $(function () {
            $('#tabs').tabs();
            if (!ie) {
                $("li").css("margin-left", "1px");
            }
        });     
    </script>



   

    <div style="width: 100%;">
        <div id="tabs" style="width: 900px; margin: auto;">
            <ul>
                <%--<li><a href="#tabs-1">
                    <asp:Image ID="Image0" ImageUrl="~/Images/P_tab_on.jpg" ToolTip="Popular" runat="server"
                        Height="55px" /></a></li>--%>
                <li><a href="#tabs-2"><span class="HyperLink2" style="vertical-align:middle;">Highly Rated</span>&nbsp;
                    <asp:Image ID="Image1" ImageUrl="~/Images/Highly_Rated_ICON.png" ToolTip="Highly Rated" runat="server" style="vertical-align:middle;"/> </a></li>
                        
                <li><a href="#tabs-3"><span class="HyperLink2" style="vertical-align:middle;">Recently Viewed</span>&nbsp;
                    <asp:Image ID="Image2" ImageUrl="~/Images/Recently_Viewed_ICON.png" ToolTip="Recently Viewed" runat="server" style="vertical-align:middle;" />  </a></li>
                     
                <li><a href="#tabs-4"><span class="HyperLink2" style="vertical-align:middle;">Recently Updated</span>&nbsp;
                    <asp:Image ID="Image3" ImageUrl="~/Images/Recently_Updated_ICON.png" ToolTip="Recently Updated"
                        runat="server" style="vertical-align:middle;" /></a></li>
            </ul>
         
            <div id="tabs-2">
                <div style="background-color: White; border: 1px solid #7f7f7f;">
                    <table border="0" width="100%">
                        <tr>
                            <td style="width: 50px;" align="center">
                                <asp:Image ID="LeftArrow" runat="server" ImageUrl="~/Images/Arrow_Left_ON.png" />
                            </td>
                            <td align="center">
                                <telerik:RadRotator ID="HighlyRatedListView" runat="server" Height="250px" ScrollDuration="500"
                                    FrameDuration="2000" Width="700px"  RotatorType="Buttons" WrapFrames="false">
                                    <ItemTemplate>   
                                                                   
                                        <div class="radRotatoritemTemplate">
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
                                                                FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar" ReadOnly="false">
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
                                                Font-Size="Small"></asp:Label>
                                            <span style="font-size: x-small">Uploaded by:
                                                <br />
                                                <asp:HyperLink ID="SubmitterEmailHyperLink" Style="color: #0E4F9C" runat="server"
                                                    Text='<%# Website.Common.GetFullUserName( Eval("SubmitterEmail")) %>' ToolTip='<%# Eval("SubmitterEmail") %>'
                                                    NavigateUrl='<%# "~/Public/Results.aspx?SubmitterEmail=" + Eval("SubmitterEmail") %>' />
                                            </span>
                                        </div>
                                        <div style="float: left; margin-top: 35px;">
                                            <asp:Image ID="VerticalLineSeparator" runat="server" ImageUrl="~/Images/Grey_Line.png" />
                                        </div>
                                    </ItemTemplate>
                                    <ControlButtons LeftButtonID="LeftArrow" RightButtonID="RightArrow" />
                                </telerik:RadRotator>
                            </td>
                            <td style="width: 50px;" align="center">
                                <asp:Image ID="RightArrow" runat="server" ImageUrl="~/Images/Arrow_Right_ON.png" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <asp:HyperLink ID="ViewMoreHighlyRatedHyperLink" runat="server" Style="font-size: 12px;
                                    color: #0E4F9C;" NavigateUrl="~/Public/Results.aspx?Group=rating-high">View All >></asp:HyperLink>
                            </td>
                        </tr>
                    </table>                   
                </div>
            </div>

            <div id="tabs-3">
            <div style="background-color:White;border: 1px solid #7f7f7f;">
             
                <table border="0" width="100%">
                        <tr>
                            <td style="width: 50px;" align="center">
                                <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/Arrow_Left_ON.png" />
                            </td>
                            <td align="center">
                                <telerik:RadRotator ID="RecentlyViewedListView" runat="server" Height="250px" ScrollDuration="500"
                                    FrameDuration="2000" Width="700px"  RotatorType="Buttons" WrapFrames="false" >
                                    <ItemTemplate>                                    
                                        <div class="radRotatoritemTemplate">
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
                                                                FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar" ReadOnly="false">
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
                                                Font-Size="Small"></asp:Label>
                                            <span style="font-size: x-small">Uploaded by:
                                                <br />
                                                <asp:HyperLink ID="SubmitterEmailHyperLink" Style="color: #0E4F9C" runat="server"
                                                    Text='<%# Website.Common.GetFullUserName( Eval("SubmitterEmail")) %>' ToolTip='<%# Eval("SubmitterEmail") %>'
                                                    NavigateUrl='<%# "~/Public/Results.aspx?SubmitterEmail=" + Eval("SubmitterEmail") %>' />
                                            </span>
                                        </div>
                                        <div style="float: left; margin-top: 35px;">
                                            <asp:Image ID="VerticalLineSeparator" runat="server" ImageUrl="~/Images/Grey_Line.png" />
                                        </div>
                                    </ItemTemplate>
                                    <ControlButtons LeftButtonID="LeftArrow" RightButtonID="RightArrow" />
                                </telerik:RadRotator>
                            </td>
                            <td style="width: 50px;" align="center">
                                <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/Arrow_Right_ON.png" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <asp:HyperLink ID="HyperLink1" runat="server" Style="font-size: 12px;
                                    color: #0E4F9C;" NavigateUrl="~/Public/Results.aspx?Group=viewed-high">View All >></asp:HyperLink>
                            </td>
                        </tr>
                    </table>                   
          </div>
            </div>
             
            <div id="tabs-4">
            <div style="background-color:White;border: 1px solid #7f7f7f;">
                <table border="0" width="100%">
                        <tr>
                            <td style="width: 50px;" align="center">
                                <asp:Image ID="Image6" runat="server" ImageUrl="~/Images/Arrow_Left_ON.png" />
                            </td>
                            <td align="center">
                                <telerik:RadRotator ID="RecentlyUpdatedListView" runat="server" Height="250px" ScrollDuration="500"
                                    FrameDuration="2000" Width="700px"  RotatorType="Buttons" WrapFrames="false" >
                                    <ItemTemplate>                                    
                                        <div class="radRotatoritemTemplate">
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
                                                                FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar" ReadOnly="false">
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
                                                Font-Size="Small"></asp:Label>
                                            <span style="font-size: x-small">Uploaded by:
                                                <br />
                                                <asp:HyperLink ID="SubmitterEmailHyperLink" Style="color: #0E4F9C" runat="server"
                                                    Text='<%# Website.Common.GetFullUserName( Eval("SubmitterEmail")) %>' ToolTip='<%# Eval("SubmitterEmail") %>'
                                                    NavigateUrl='<%# "~/Public/Results.aspx?SubmitterEmail=" + Eval("SubmitterEmail") %>' />
                                            </span>
                                        </div>
                                        <div style="float: left; margin-top: 35px;">
                                            <asp:Image ID="VerticalLineSeparator" runat="server" ImageUrl="~/Images/Grey_Line.png" />
                                        </div>
                                    </ItemTemplate>
                                    <ControlButtons LeftButtonID="LeftArrow" RightButtonID="RightArrow" />
                                </telerik:RadRotator>
                            </td>
                            <td style="width: 50px;" align="center">
                                <asp:Image ID="Image7" runat="server" ImageUrl="~/Images/Arrow_Right_ON.png" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <asp:HyperLink ID="HyperLink2" runat="server" Style="font-size: 12px;
                                    color: #0E4F9C;" NavigateUrl="~/Public/Results.aspx?Group=updated-high">View All >></asp:HyperLink>
                            </td>
                        </tr>
                    </table>                   
                </div>
            </div>
           
        </div>
    </div>
</asp:Content>
