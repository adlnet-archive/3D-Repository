<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Model.aspx.cs" Inherits="Public_Model" Title="Model Details" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="VwarWeb" TagName="Viewer3D" Src="~/Controls/Viewer3D.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    
    <script type="text/javascript" src="../scripts/o3djs/base.js"></script>
    <script type="text/javascript" src="../scripts/o3djs/simpleviewer.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-1.3.2.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.7.2.custom.min.js"></script>
    <script type="text/javascript" src="../Scripts/ViewerLoad.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.viewerTab').click(function () {
                vLoader.LoadViewer();
            });

            $('.imageTab').click(function () {
                vLoader.ResetViewer();
            });
        });
    </script>
    <style type="text/css">
        .ViewerPageContainer
        {
            background-color: white;
            border: 1px solid gray;
            height: 550px;
            position: relative;
            top: -1px;
            width: 550px;
            z-index: 0;
        }
        
        .ViewerWrapper
        {
            padding-left: 10px;
        }
        
        .ViewerItem
        {
            width: 500px;
            height: 500px;
            margin: 25px;
            overflow: hidden;
            border:none;
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadAjaxManagerProxy runat="server" ID="RadAjaxManagerProxy1">
    </telerik:RadAjaxManagerProxy>
    <div id="ModelDetails">
        <input type="hidden" runat="server" id="upAxis" />
        <input type="hidden" runat="server" id="unitScale" />
        <input type="hidden" runat="server" id="modelURL" />

         

        <table class="CenteredTable" cellpadding="4" border="0">
            <tr>
                <td height="600" width="564" class="ViewerWrapper">
               <telerik:RadTabStrip ID="ViewOptionsTab" Skin="WebBlue" runat="server" SelectedIndex="0" MultiPageID="ViewOptionsMultiPage" CssClass="front">
                <Tabs>
                <telerik:RadTab Text="Image" ImageUrl="~/Images/2D-Tab-Icon.png" CssClass="imageTab"/>
                <telerik:RadTab Text="3D Viewer" ImageUrl="~/Images/3D-Tab-Icon.png" CssClass="viewerTab"/>
                </Tabs>
                </telerik:RadTabStrip>
                <telerik:RadMultiPage ID="ViewOptionsMultiPage" SelectedIndex="0" runat="server" CssClass="ViewerPageContainer">
                    <telerik:RadPageView ID="ImageView" runat="server" CssClass="ViewerItem">
                       <div id="scriptDisplay" runat="server" />
                        <asp:Image Height="500px" Width="500px" ID="ScreenshotImage" runat="server"
                            ToolTip='<%# Eval("Title") %>' />
                        <br />
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="O3DView" runat="server">
                        <VwarWeb:Viewer3D ID="viewer" runat="server" />
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
                <!-- AddThis Button BEGIN -->
<div class="addthis_toolbox addthis_default_style" style="margin-top: 3px">
<a href="http://www.addthis.com/bookmark.php?v=250&amp;username=xa-4cd9c9466b809f73" class="addthis_button_compact">Share</a>
<span class="addthis_separator">|</span>
<a class="addthis_button_facebook" addthis:title="Test title"></a>
<a class="addthis_button_twitter"></a>
<a class="addthis_button_linkedin"></a>
<a class="addthis_button_digg"></a>
</div>
<script type="text/javascript" src="http://s7.addthis.com/js/250/addthis_widget.js#username=xa-4cd9c9466b809f73"></script>
<!-- AddThis Button END -->

                <div id='ViewerStatus' style='display: none;'>
                
                </div>
                </td>
                <td rowspan="2">
                    &nbsp;
                </td>
                <td rowspan="2">              
                    <table border="0" cellpadding="4" cellspacing="0" width="100%">
                        <tr runat="server" id="IDRow" visible="true">
                            <td>
                                
                            <asp:HyperLink ID="editLink" Visible="false" runat="server" Text="Edit" ImageUrl="~/Images/Edit_BTN.png"></asp:HyperLink>
                                
                                <asp:Label ID="IDLabel" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                 
                                <div class="ListTitle">
                                    <div>
                                        3D Asset</div>
                                </div>
                                <br />
                                <table border="0" style="margin-left: 5px;">
                                
                                    <tr>
                                        <td>
                                            <asp:Label ID="TitleLabel" runat="server" CssClass="ModelTitle"></asp:Label>
                                            <asp:HyperLink ID="SubmitterEmailHyperLink" runat="server" CssClass="Hyperlink" Visible="false">[SubmitterEmailHyperLink]</asp:HyperLink>
                                                
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:HyperLink ID="CCLHyperLink" runat="server" Target="_blank" CssClass="Hyperlink" />
                                        </td>
                                    </tr>
                                    <tr runat="server" id="DescriptionRow">
                                        <td>
                                            <asp:Label ID="DescriptionLabel" runat="server" />
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:LinkButton ID="ReportViolationButton" CssClass="Hyperlink" runat="server" Text="Report a Violation"
                                                OnClick="ReportViolationButton_Click" />
                                        </td>
                                    </tr>
                                    <tr runat="server" id="KeywordsRow">
                                        <td>
                                            <br />
                                            <span runat="server" id="keywordLabel">Keywords:</span> <span id="keywords" runat="server">
                                            </span>
                                        </td>
                                        <td>
                                            <table border="0" class="CenteredTable">
                                                <tr>
                                                    <td>
                                                        <ajax:Rating ID="ir" runat="server" CurrentRating='<%# Website.Common.CalculateAverageRating(Eval("Id")) %>'
                                                            MaxRating="5" StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar"
                                                            FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar" ReadOnly="true">
                                                        </ajax:Rating>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" >
                                            <br />
                                           <div>
                                            Available File Formats
                                                <telerik:RadComboBox ID="ModelTypeDropDownList" runat="server" CausesValidation="False"
                                                    EnableEmbeddedSkins="false">
                                                    <Items>
                                                        <telerik:RadComboBoxItem runat="server" Text="No Conversion" Value="" />
                                                        <telerik:RadComboBoxItem runat="server" Text="Collada" Value=".dae" />
                                                        <telerik:RadComboBoxItem runat="server" Text="OBJ" Value=".obj" />
                                                        <telerik:RadComboBoxItem runat="server" Text="3DS" Value=".3DS" />
                                                        <telerik:RadComboBoxItem runat="server" Text="O3D" Value=".O3Dtgz" />
                                                    </Items>
                                                </telerik:RadComboBox>
                                                <asp:ImageButton style="vertical-align:bottom;" ID="DownloadButton" runat="server" Text="Download" ToolTip="Download"
                                                CommandName="DownloadZip" OnClick="DownloadButton_Click" ImageUrl="~/Images/Download_BTN.png" />

                                            </div>
                                        </td>
                                       
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="ListTitle">
                                    <div>
                                        Developer Information</div>
                                </div>
                                <table border="0" style="margin-left: 5px;">
                                    <tr runat="server" id="DeveloperLogoRow">
                                        <td>
                                            <%--<asp:Image ID="DeveloperLogoImage" runat="server" ImageUrl= />--%>
                                            <telerik:RadBinaryImage ID="DeveloperLogoImage" runat="server"  />
                            
                                        </td>
                                    </tr>
                                    <tr runat="server" id="SubmitterEmailRow">
                                        <td>
                                            <asp:Label ID="UploadedDateLabel" runat="server" Enabled="false" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="DeveloperRow">
                                        <td>
                                            Developer Name:
                                            <asp:HyperLink ID="DeveloperNameHyperLink" runat="server" NavigateUrl="#" CssClass="Hyperlink">[DeveloperNameHyperLink]</asp:HyperLink>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="ArtistRow">
                                        <td>
                                            Artist Name:
                                            <asp:HyperLink ID="ArtistNameHyperLink" runat="server" NavigateUrl="#" CssClass="Hyperlink">[ArtistNameHyperLink]</asp:HyperLink>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="MoreDetailsRow">
                                        <td>
                                            <br />
                                            <asp:HyperLink ID="MoreDetailsHyperLink" runat="server" Target="_blank" CssClass="Hyperlink" />&nbsp;<asp:Image ID="ExternalLinkIcon" runat="server" ImageUrl="~/Images/externalLink.gif" Width="15px" Height="15px" ImageAlign="Bottom" />
                                        </td>
                                    </tr>
                                </table>
                            </td>   
                        </tr>
                        <tr>
                            <td>
                                <div class="ListTitle">
                                    <div>
                                        Sponsor Information</div>
                                </div>
                                <table border="0" style="margin-left: 5px;">
                                    <tr runat="server" id="SponsorLogoRow">
                                        <td>
                                             <telerik:RadBinaryImage ID="SponsorLogoImage" runat="server"  />
                          
                                            <%--<asp:Image ID="SponsorLogoImage" runat="server" />--%>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="SponsorNameRow">
                                        <td>
                                            Sponsor Name:
                                            <asp:Label ID="SponsorNameLabel" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="ListTitle">
                                    <div>
                                        Asset Details</div>
                                </div>
                                <table border="0" style="margin-left: 5px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="FormatLabel" runat="server" />
                                        </td>
                                    </tr>
                                    <tr runat="server" id="NumPolygonsRow">
                                        <td>
                                            <br />
                                            Number of Polygons:
                                            <asp:Label ID="NumPolygonsLabel" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="NumTexturesRow">
                                        <td>
                                            Number of Textures:
                                            <asp:Label ID="NumTexturesLabel" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="DownloadsRow">
                                        <td>
                                            <br />
                                            Downloads:
                                            <asp:Label ID="DownloadsLabel" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="ViewsRow">
                                        <td>
                                            Views:
                                            <asp:Label ID="ViewsLabel" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="updatePanel" runat="server" EnableViewState="true" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            
                            <div class="ListTitle" style="width:550px; margin-left:6px;">
                                <div>
                                    Comments and Reviews</div>
                            </div>

                            
                            <br />
                            <asp:Label ID="NotRatedLabel" runat="server" Font-Bold="true" Text="Not yet rated.  Be the first to rate!<br /><br />" Visible="false"></asp:Label>
                            <div style="margin-left: 5px">
                            
                            <asp:GridView ID="CommentsGridView" runat="server" AutoGenerateColumns="false" BorderStyle="None"
                                GridLines="None" ShowHeader="false">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                           <ajax:Rating ID="ir" runat="server" CurrentRating='<%# Eval("Rating") %>'
                                                                MaxRating="5" StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar"
                                                                FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar" ReadOnly="false">
                                                            </ajax:Rating>
                                            <br />
                                            <asp:Label ID="Label2" Text='<%# Eval("Text") %>' runat="server"></asp:Label>
                                            <br />
                                            Submitted By:
                                            <asp:Label Text='<%#Website.Common.GetFullUserName( Eval("SubmittedBy")) %>' runat="server"></asp:Label>
                                            On
                                            <asp:Label ID="Label1" Text='<%# Eval("SubmittedDate","{0:d}") %>' runat="server"></asp:Label>
                                            <hr />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div id="UnauthenticatedReviewSubmission" style="display: none;" runat="server">
                                <asp:HyperLink ID="ReveiwLoginHyperLink" NavigateUrl="~/Public/Login.aspx" Text="Log in" runat="server" /> to submit a review
                            </div>
                            <div id="AuthenticatedReviewSubmission" style="display:none; margin-left: 5px" runat="server">
                                <asp:Label Text="Write your own review:" runat="server" />
                                <br />
                                <ajax:Rating ID="rating" runat="server" CurrentRating="3" MaxRating="5" StarCssClass="ratingStar"
                                    OnChanged="Rating_Set" WaitingStarCssClass="savedRatingStar" FilledStarCssClass="filledRatingStar"
                                    EmptyStarCssClass="emptyRatingStar">
                                </ajax:Rating>
                                <br />
                                <asp:TextBox ID="ratingText" runat="server" TextMode="MultiLine" Columns="44" SkinID="TextBox"
                                    Rows="4"></asp:TextBox>
                                <br />
                                <asp:ImageButton ID="submitRating" Text="Add Rating" runat="server" OnClick="Rating_Click"
                                    ImageUrl="~/Images/Add_Rating_BTN.png" />
                                <br />
                                <br />
                            </div>
                            </div>



                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
