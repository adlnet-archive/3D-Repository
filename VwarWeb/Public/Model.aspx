<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Model.aspx.cs" Inherits="Public_Model" Title="Model Details" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="VwarWeb" TagName="Viewer3D" Src="~/Controls/Viewer3D.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../Stylesheets/ModelDetails.css" rel="Stylesheet" type="text/css" />     

    <script type="text/javascript" src="../scripts/o3djs/base.js"></script>
    <script type="text/javascript" src="../scripts/o3djs/simpleviewer.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.8.7.custom.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.mousewheel.min.js"></script>
    <script type="text/javascript" src="../Scripts/ViewerLoad.js"></script>
    <script type="text/javascript" src="../Scripts/ModelDetails.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/webgl-utils.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/osg.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/osgUtil.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/osgAnimation.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/osgGA.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/osgViewer.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/examples/viewer/webglviewer.js"></script>
    <script type="text/javascript">

        function querySt(ji) {
            hu = window.location.search.substring(1);
            gy = hu.split("&");
            for (i = 0; i < gy.length; i++) {
                ft = gy[i].split("=");
                if (ft[0] == ji) {
                    return ft[1];
                }
            }
        }

        function DownloadModel(informat) {

          //  alert(querySt('ContentObjectID'));
          //  $.ajax({
          //      type: "POST",
          //      url: "Model.aspx/DownloadButton_Click_Impl",
          //      data: JSON.stringify({ format: informat, ContentObjectID: querySt('ContentObjectID') }),
          //      contentType: "application/json; charset=utf-8",
          //      dataType: "json",
          //      success: function (object, responseStatus, request) {
          //          $dialog.hide();
          //      }
          //  });
            window.location.href = "../DownloadModel.ashx?PID=" + querySt('ContentObjectID') + "&Format=" + informat;
        }

        $(document).ready(function () {




            var top = $('#ctl00_ContentPlaceHolder1_DownloadButton').offset().top + $('#ctl00_ContentPlaceHolder1_DownloadButton').height();
            var left = $('#DownloadDiv').offset().left;
            var width = $('#3DAssetbar').width();
            var $dialog = $('<div></div>')
		            .load('downloadoptions.html')
		            .dialog({
		                autoOpen: false,
		                title: 'Download Model',
		                show: "fold",
		                hide: "fold",
		                //modal: true,
		                resizable: false,
		                draggable: false,
		                position: [left, top],
		                width: 'auto',
		                height: 'auto'
		            });

            $('#ctl00_ContentPlaceHolder1_DownloadButton').click(function () {

                var top = $('#ctl00_ContentPlaceHolder1_DownloadButton').offset().top + $('#ctl00_ContentPlaceHolder1_DownloadButton').height() - $(document).scrollTop();
                var left = $('#DownloadDiv').offset().left;
                var width = $('#3DAssetbar').width();
                $dialog.dialog("option", "width", 'auto');
                $dialog.dialog("option", "position", [left, top]);
                $dialog.dialog('open');

                // prevent the default action, e.g., following a link
                return false;
            });
        });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadAjaxManagerProxy runat="server" ID="RadAjaxManagerProxy1">
    </telerik:RadAjaxManagerProxy>
    <div id="NotificationDialog" style="text-align: center">
        <div class="statusText"></div>
    </div>
    <div id="ConfirmationDialog" style="text-align: center">
        <div class="statusText"></div>
    </div>
    <div id="ModelDetails">
        <input type="hidden" runat="server" id="upAxis" />
        <input type="hidden" runat="server" id="unitScale" />
        <input type="hidden" runat="server" id="modelURL" />

         

        <table class="CenteredTable" cellpadding="4" border="0" align=center>
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
                           <%-- <asp:HyperLink ID="editLink" Visible="false" runat="server" Text="Edit" ImageUrl="~/Images/Edit_BTN.png"></asp:HyperLink> --%>
                            <asp:HyperLink ID="editLink" CssClass="Hyperlink" Visible="false" runat="server" Text="Edit"></asp:HyperLink>
                            <span id="pipehack">&nbsp;|&nbsp;</span>
                            <a id="DeleteLink" runat="server" class="Hyperlink" Visible="false">Delete</a>
                                <asp:Label ID="IDLabel" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                 
                                <div class="ListTitle">
                                    <div id="3DAssetbar">
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
                                            <asp:HyperLink ID="CCLHyperLink" runat="server" Target="_blank" CssClass="Hyperlink">
                                            </asp:HyperLink>
                                            
                                        </td>
                                    </tr>
                                    <tr runat="server" id="DescriptionRow">
                                        <td>
                                            <asp:Label ID="DescriptionLabel" runat="server" />
                                        </td>
                                        <td style="text-align: center;">
                                                <a id="ReportViolationButton" class="Hyperlink">Report a Violation</a>
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
                                                        <ajax:Rating ID="ir" runat="server" CurrentRating='<%# Website.Common.CalculateAverageRating(Eval("Reviews")) %>'
                                                            MaxRating="5" StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar"
                                                            FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar" ReadOnly="true">
                                                        </ajax:Rating>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" width='400px'>
                                            <br />
                                           <div id="DownloadDiv" >
                                           
                                                <asp:ImageButton style="vertical-align:bottom;" ID="DownloadButton" runat="server" Text="Download" ToolTip="Download"
                                                CommandName="DownloadZip" OnClientClick="return ValidateResubmitChecked();" OnClick="DownloadButton_Click" ImageUrl="~/Images/Download_BTN.png" />
                                                <br /><br />
                                                <asp:CheckBox ID="RequiresResubmitCheckbox" Checked="true" Text="I agree to re-submit any modifications back to the 3D Repository"
                                                    runat="server" Visible="false" Enabled="false" /> 
                                            </div>
                                        </td>
                                       
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td id="DeveloperInfoSection" runat="server">
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
                            <td id="SponsorInfoSection"  runat="server">
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
