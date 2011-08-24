<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Model.aspx.cs" Inherits="Public_Model" Title="Model Details" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="VwarWeb" TagName="Viewer3D" Src="~/Controls/Viewer3D.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../styles/ModelDetails.css" rel="Stylesheet" type="text/css" />
    <link href="../styles/tabs-custom.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript" src="../scripts/o3djs/base.js"></script>
    <script type="text/javascript" src="../scripts/o3djs/simpleviewer.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.8.7.custom.min.js"></script>
    <script type="text/javascript" src="../Scripts/ViewerLoad.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/webgl-utils.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/osg.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/osgUtil.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/osgAnimation.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/osgGA.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/js/osgViewer.js"></script>
    <script type="text/javascript" src="../Scripts/OSGJS/examples/viewer/webglviewer.js"></script>
    <script type="text/javascript" src="../Scripts/_lib/jquery.cookie.js"></script>
    <script type="text/javascript" src="../Scripts/_lib/jquery.hotkeys.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.jstree.js"></script>
    <script type="text/javascript" src="../Scripts/ModelDetails.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="NotificationDialog" style="text-align: center">
        <div class="statusText">
        </div>
    </div>
    <div id="ConfirmationDialog" style="text-align: center">
        <div class="statusText">
        </div>
    </div>
    <div id="ModelDetails">
        <input type="hidden" runat="server" id="upAxis" />
        <input type="hidden" runat="server" id="unitScale" />
        <input type="hidden" runat="server" id="modelURL" />
        <table class="CenteredTable" cellpadding="4" border="0">
            <tr>
                <td height="600" width="564" class="ViewerWrapper">
                    <div id="ViewOptionsMultiPage" class="ViewerPageContainer">
                        <ul class="tabContainer">
                            <li class="first"><span class="ui-tabs-separator"></span><a href="#ImageView">Screenshot</a></li>
                            <li><span class="ui-tabs-separator"></span><a href="#SceneView">3D View</a></li>
                        </ul>
                        <div id="ImageView">
                            <asp:Image Height="500px" Width="500px" ID="ScreenshotImage" runat="server" ToolTip='<%# Eval("Title") %>' />
                            <br />
                        </div>
                        <div id="SceneView">
                            <VwarWeb:Viewer3D ID="Viewer" runat="server" />
                        </div>
                    </div>
                    <div class="addthis_toolbox addthis_default_style" style="margin-top: 3px">
                        <a href="http://www.addthis.com/bookmark.php?v=250&amp;username=xa-4cd9c9466b809f73"
                            class="addthis_button_compact">Share</a> <span class="addthis_separator">|</span>
                        <a class="addthis_button_facebook" addthis:title="Test title"></a><a class="addthis_button_twitter">
                        </a><a class="addthis_button_linkedin"></a><a class="addthis_button_digg"></a>
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
                                <asp:HyperLink ID="editLink" CssClass="Hyperlink" Visible="false" runat="server"
                                    Text="Edit"></asp:HyperLink>
                                <span id="pipehack">&nbsp;|&nbsp;</span> <a id="DeleteLink" runat="server" visible="false"
                                    class="Hyperlink">Delete</a>
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
                                        <td colspan="2" width="400">
                                            <div id="DownloadDiv">
                                                <asp:ImageButton Style="vertical-align: bottom;" ID="DownloadButton" runat="server"
                                                    Text="Download" ToolTip="Download" CommandName="DownloadZip" ImageUrl="~/styles/images/Download_BTN.png" />
                                                <asp:Label ID="LoginToDlLabel" Visible="false" runat="server">
                                                    <asp:HyperLink ID="LoginLink" NavigateUrl="~/Public/Login.aspx" runat="server">Log in</asp:HyperLink>
                                                    to download </asp:Label>
                                                <br />
                                                <br />
                                                <div id="RequiresResubmitWrapper">
                                                    <asp:CheckBox ID="RequiresResubmitCheckbox" Checked="true" runat="server" Visible="false"
                                                        Enabled="false" />
                                                    <asp:Label ID="RequiresResubmitLabel" runat="server" Style="display: inline-block;
                                                        width: 260px; vertical-align: top;" Visible="false">
                                                        I agree to re-submit any modifications back to the 3D Repository.
                                                    </asp:Label>
                                                </div>
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
                                            <asp:Image style="max-width: 400px" ID="DeveloperLogoImage" runat="server" />
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
                                            <asp:HyperLink ID="MoreDetailsHyperLink" runat="server" Target="_blank" CssClass="Hyperlink" />&nbsp;<asp:Image
                                                ID="ExternalLinkIcon" runat="server" ImageUrl="~/styles/images/externalLink.gif" Width="15px"
                                                Height="15px" ImageAlign="Bottom" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td id="SponsorInfoSection" runat="server">
                                <div class="ListTitle">
                                    <div>
                                        Sponsor Information</div>
                                </div>
                                <table border="0" style="margin-left: 5px;">
                                    <tr runat="server" id="SponsorLogoRow">
                                        <td>
                                            <asp:Image  style="max-width: 400px" ID="SponsorLogoImage" runat="server" />
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
                            <div class="ListTitle" style="width: 550px; margin-left: 6px;">
                                <div>
                                    Comments and Reviews</div>
                            </div>
                            <br />
                            <asp:Label ID="NotRatedLabel" runat="server" Style="margin-left: 10px" Font-Bold="true"
                                Text="Not yet rated.  Be the first to rate!<br /><br />" Visible="false"></asp:Label>
                            <div style="margin-left: 5px">
                                <asp:GridView ID="CommentsGridView" runat="server" AutoGenerateColumns="false" BorderStyle="None"
                                    GridLines="None" ShowHeader="false">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <ajax:Rating ID="ir" runat="server" CurrentRating='<%# Eval("Rating") %>' MaxRating="5"
                                                    StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar" FilledStarCssClass="filledRatingStar"
                                                    EmptyStarCssClass="emptyRatingStar" ReadOnly="false">
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
                                    <asp:HyperLink ID="ReveiwLoginHyperLink" NavigateUrl="~/Public/Login.aspx" Text="Log in"
                                        runat="server" />
                                    to submit a review
                                </div>
                                <div id="AuthenticatedReviewSubmission" style="display: none; margin-left: 5px" runat="server">
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
                                    <asp:RegularExpressionValidator ID="regexTextBox1" ControlToValidate="ratingText"
                                        runat="server" ValidationExpression="^[\s\S]{0,99}$" Text="99 characters max" />
                                    <br />
                                    <asp:ImageButton ID="submitRating" Text="Add Rating" runat="server" OnClick="Rating_Click"
                                        ImageUrl="~/styles/images/Add_Rating_BTN.png" />
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
