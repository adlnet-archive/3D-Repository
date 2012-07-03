<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Model.aspx.cs" Inherits="Public_Model" Title="Model Details" SmartNavigation="True"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="VwarWeb" TagName="Viewer3D" Src="~/Controls/Viewer3D.ascx" %>
<%@ Register TagPrefix="VwarWeb" TagName="PermissionsManagementWidget" Src="~/Controls/PermissionsManagementWidget.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../styles/ModelDetails.css" rel="Stylesheet" type="text/css" />
    <link href="../styles/tabs-custom.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript" src="../scripts/o3djs/base.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.8.7.custom.min.js"></script>
    <script type="text/javascript" src="../scripts/o3djs/simpleviewer.js"></script>
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
    <script type="text/javascript" src="../Scripts/ViewerLoad.js"></script>
    <script type="text/javascript" src="../Scripts/ImageUploadWidget.js"></script>
    <script type="text/javascript" src="../Scripts/fileuploader.js"></script>
    <script type="text/javascript" >

        var DeveloperLogoUploadWidget;
        $(document).ready(function () {
            DeveloperLogoUploadWidget = new ImageUploadWidget("screenshot_viewable", $("#DeveloperLogoUploadWidgetDiv"));
        });
    </script>
});
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
                            <asp:Image Height="500px" Width="500px" ID="ScreenshotImage" runat="server" ToolTip='<%# Eval("Title") %>' AlternateText="Screenshot" />
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
                            <td id="EditorButtons" runat="server">
                                <asp:LinkButton ID="editLink" CssClass="Hyperlink" Visible="false" runat="server"
                                    Text="Edit" OnClick="BeginEditing"></asp:LinkButton>
                                <span id="pipehack">&nbsp;|&nbsp;</span> 
                                <a id="PermissionsLink" runat="server" visible="false" class="Hyperlink">Permissions</a>
                                <span id="Span1">&nbsp;|&nbsp;</span>
                                <a id="DeleteLink" runat="server" visible="false"
                                    class="Hyperlink">Delete</a>
                                <asp:Label ID="IDLabel" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td id="_3DAssetSection" runat="server">
                                <div class="ListTitle">
                                    <div>
                                        3D Asset <asp:LinkButton runat="server" ID="EditAssetInfo" style="cursor:pointer;float:right" Text="Edit" onclick="EditAssetInfo_Click" visible="false"></asp:LinkButton>
                                                 <asp:LinkButton runat="server" ID="EditAssetInfoCancel" style="cursor:pointer;float:right;margin-right:10px" Text="Cancel " visible="false" onclick="EditAssetInfoCancel_Click" ></asp:LinkButton>
                                        </div>
                                </div>
                                <br />
                                <table border="0" style="margin-left: 5px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="TitleLabel" runat="server" CssClass="ModelTitle"></asp:Label><asp:TextBox CssClass="ModelTitle" ID="EditTitle"  style="width:100%;border-radius:5px" runat="server"></asp:TextBox>
                                            <asp:HyperLink ID="SubmitterEmailHyperLink" runat="server" CssClass="Hyperlink" Visible="false">[SubmitterEmailHyperLink]</asp:HyperLink>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:HyperLink ID="CCLHyperLink" runat="server" Target="_blank" CssClass="Hyperlink">
                                            </asp:HyperLink>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="DescriptionRow">
                                        <td>
                                            <asp:Label ID="DescriptionLabel" style='width: 350px; display: block' runat="server" /><asp:TextBox TextMode="MultiLine" ID="EditDescription" runat="server" style="width:100%;border-radius:5px;height:110px"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center;">
                                            <a id="ReportViolationButton" runat="server" class="Hyperlink">Report a Violation</a>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="KeywordsRow">
                                        <td>
                                            <br />
                                            <span runat="server" id="keywordLabel">Keywords:</span> <span id="keywords" runat="server">
                                            </span><asp:TextBox style="color:darkblue;font-size:small;border-radius:5px;width:100%;" ID="EditKeywords" runat="server"></asp:TextBox>
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
                                                    Text="Download" ToolTip="Download" CommandName="DownloadZip" ImageUrl="~/styles/images/Download_BTN.png" AlternateText="Download Buton" />
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
                                    <tr id="SelectLicenseArea" runat="server" visible="false">
                                    <td colspan="2" width="400">
                                        <asp:Label runat="server">
                                        License Type</asp:Label>
                                        <select id="LicenseType"  runat="server" style="width:50%;float:right;border-radius:5px">
                                            <option value="publicdomain">Public Domain</option>
                                            <option value="by" >Attribution</option>
                                            <option value="by-sa" selected="selected">Attribution-ShareAlike</option>
                                            <option value="by-nd">Attribution-NoDerivatives</option>
                                            <option value="by-nc">Attribution-NonCommercial</option>
                                            <option value="by-nc-sa">Attribution-NonCommercial-ShareAlike</option>
                                            <option value="by-nc-nd">Attribution-NonCommercial-NoDerivatives</option>
                                        </select>
                                        </td>
                                    </tr> 
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td id="DeveloperInfoSection" runat="server">
                                <div class="ListTitle">
                                    <div>
                                        Developer Information <asp:LinkButton runat="server" ID="EditDeveloperInfo" OnClick="EditDeveloperInfo_Click" style="float:right" text="Edit" visible="false"></asp:LinkButton>
                                                              <asp:LinkButton runat="server" ID="EditDeveloperInfoCancel" OnClick="EditDeveloperInfoCancel_Click" style="float:right;margin-right:10px" text="Cancel" visible="false"></asp:LinkButton>
                                    </div>
                                </div>
                                <table border="0" style="margin-left: 5px;width:100%">
                                    <tr runat="server" id="DeveloperLogoRow">
                                        <td>
                                            <asp:Image style="max-width: 400px" ID="DeveloperLogoImage" runat="server" AlternateText="Developer Logo" />
                                        </td>
                                    </tr>
                                    <tr id="UploadDeveloperLogoRow" visible="false" runat="server">
                                        <td>
                                            <div id="DeveloperLogoUploadWidgetDiv" style="font-size: smaller;border: solid 1px lightGrey;border-radius: 5px;vertical-align: middle;"><asp:Button style="font-size:smaller;height:100%" runat="server" ID="DeleteDeveloperLogo" text="Delete Logo" OnClick="DeleteDeveloperLogo_Click" /><asp:FileUpload style="font-size:smaller;"  runat="server" ID="UploadDeveloperLogo" /></div>
                                            
                                        </td>
                                    </tr>
                                    <tr runat="server" id="SubmitterEmailRow">
                                        <td>
                                            <asp:Label ID="UploadedDateLabel" runat="server" Enabled="false" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="DeveloperRow">
                                        <td style="vertical-align:middle">
                                            Developer Name:
                                            <asp:HyperLink ID="DeveloperNameHyperLink" runat="server" NavigateUrl="#" CssClass="Hyperlink">[DeveloperNameHyperLink]</asp:HyperLink><asp:TextBox ID="EditDeveloperNameHyperLink" runat="server" style="border-radius:5px;width:50%;float:right" visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="ArtistRow">
                                        <td style="vertical-align:middle">
                                            Artist Name:
                                            <asp:HyperLink ID="ArtistNameHyperLink" runat="server" NavigateUrl="#" CssClass="Hyperlink">[ArtistNameHyperLink]</asp:HyperLink><asp:TextBox ID="EditArtistNameHyperLink" runat="server" style="border-radius:5px;width:50%;float:right" visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="MoreDetailsRow">
                                        <td>
                                            <br />
                                            <asp:HyperLink ID="MoreDetailsHyperLink" runat="server" Target="_blank" CssClass="Hyperlink" />&nbsp;<asp:Image
                                                ID="ExternalLinkIcon" runat="server" ImageUrl="~/styles/images/externalLink.gif" Width="15px"
                                                Height="15px" ImageAlign="Bottom" AlternateText="External Link" /><asp:TextBox ID="EditMoreInformationURL" runat="server" style="border-radius:5px;width:50%;float:right" visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td id="SponsorInfoSection" runat="server">
                                <div class="ListTitle">
                                    <div>
                                        Sponsor Information <asp:LinkButton runat="server" ID="EditSponsorInfo" OnClick="EditSponsorInfo_Click" style="float:right" Text="Edit" visible="false"></asp:LinkButton>
                                                            <asp:LinkButton runat="server" ID="EditSponsorInfoCancel" OnClick="EditSponsorInfoCancel_Click" style="float:right;margin-right:10px" Text="Cancel" visible="false"></asp:LinkButton>
                                        
                                        </div>
                                </div>
                                <table border="0" style="margin-left: 5px;width:100%">
                                    <tr runat="server" id="SponsorLogoRow">
                                        <td>
                                            <asp:Image  style="max-width: 400px" ID="SponsorLogoImage" runat="server" AlternateText="Sponsor Logo" />
                                        </td>
                                    </tr>
                                    <tr id="UploadSponsorLogoRow" visible="false" runat="server">
                                        <td>
                                            <div id="Div1" style="font-size: smaller;border: solid 1px lightGrey;border-radius: 5px;vertical-align: middle;"><asp:Button style="font-size:smaller;height:100%" runat="server" ID="DeleteSponsorLogo" text="Delete Logo" OnClick="DeleteSponsorLogo_Click" /><asp:FileUpload style="font-size:smaller;"  runat="server" ID="UploadSponsorLogo" /></div>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="SponsorNameRow">
                                        <td style="vertical-align:middle">
                                            Sponsor Name:
                                            <asp:Label ID="SponsorNameLabel" runat="server" /><asp:TextBox ID="EditSponsorNameLabel" runat="server" style="border-radius:5px;width:50%;float:right" visible="false"/>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td id="AssetDetailsSection" runat="server">
                                <div class="ListTitle">
                                    <div>
                                        Asset Details <asp:LinkButton runat="server" ID="EditDetails" style="float:right" text="Edit" OnClick="EditDetails_click" visible="false"></asp:LinkButton>
                                                      <asp:LinkButton runat="server" ID="EditDetailsCancel" style="float:right;margin-right:10px" text="Cancel" visible="false" OnClick="EditDetailsCancel_click"></asp:LinkButton>
                                    </div>
                                </div>
                                <table border="0" style="margin-left: 5px;">
                                    <tr>
                                        <td style="vertical-align:middle">
                                            <asp:Label ID="FormatLabelHead" text="Native format: " runat="server" /> <asp:Label ID="FormatLabel" runat="server" /><asp:TextBox ID="EditFormatLabel" style="border-radius:5px" runat="server" visible="false"/>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="NumPolygonsRow">
                                        <td style="vertical-align:middle">
                                            <br />
                                            Number of Polygons:
                                            <asp:Label ID="NumPolygonsLabel" runat="server"></asp:Label><asp:TextBox ID="EditNumPolygonsLabel" style="border-radius:5px" runat="server" visible="false"/>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="NumTexturesRow">
                                        <td style="vertical-align:middle">
                                            Number of Textures:
                                            <asp:Label ID="NumTexturesLabel" runat="server"></asp:Label><asp:TextBox ID="EditNumTexturesLabel" style="border-radius:5px" runat="server" visible="false"/>
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
                        <tr>
                            <td  id="SupportingFilesSection" runat="server">
                                <div class="ListTitle">
                                    <div>
                                        Supporting Documents <asp:LinkButton runat="server" OnClick="UploadSupportingFile_Click" ID="UploadSupportingFile" style="float:right" text="Add" visible="false"></asp:LinkButton>
                                                             <asp:LinkButton runat="server" ID="UploadSupportingFileCancel" style="float:right;margin-right:10px" text="Cancel" visible="false" OnClick="UploadSupportingFileCancel_Click"></asp:LinkButton>
                                        </div>
                                </div>
                               
                                <asp:GridView runat="server" ID="SupportingFileGrid" CssClass="SupportingFileTable"  OnRowCommand="SupportingFileGrid_RowCommand"
                                    AutoGenerateColumns="False" ShowHeader="False">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="Label3" Text="No Supporting Files" runat="server"></asp:Label>
                                    </EmptyDataTemplate>

                                    <Columns> 
                                        <asp:BoundField DataField="Filename"/>
                                        <asp:BoundField DataField="Description" ControlStyle-BorderStyle="None" />
                                        <asp:ButtonField ButtonType="Image" ControlStyle-CssClass="DownloadSupportingFile" ImageUrl="../styles/images/icons/expand.jpg"  CommandName="Download"/>
                                    </Columns>
                                </asp:GridView>
                                 <div id="UploadSupportingFileSection" visible="false" runat="server">
                                       
                                            <div id="Div2" style="font-size: smaller;border: solid 1px lightGrey;border-radius: 5px;vertical-align: middle;"><asp:FileUpload style="font-size:smaller;vertical-align:top"  runat="server" ID="SupportingFileUpload" /><asp:TextBox runat="server" TextMode="MultiLine" id="SupportingFileUploadDescription" style="border-radius:5px"></asp:TextBox></div>
                                                
                                </div>
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
                                        ImageUrl="~/styles/images/Add_Rating_BTN.png" AlternateText="Submit Rating" />
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
    <VwarWeb:PermissionsManagementWidget ID="PermissionsManagementControl" runat="server" />
</asp:Content>
