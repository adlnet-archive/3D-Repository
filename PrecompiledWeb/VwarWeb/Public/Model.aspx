<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Public_Model, App_Web_wlg4q3ts" title="Model Details" stylesheettheme="Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../scripts/o3djs/base.js"></script>
    <script type="text/javascript" src="../scripts/o3djs/simpleviewer.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-1.3.2.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.7.2.custom.min.js"></script>
    <script type="text/javascript">
        var firstView = true;
        $(document).ready(function () {
            $("#tabs").tabs({ select: function (event, ui) {
                if (ui.index == 1 && !firstView) {
                }
                firstView = false;
            }
            });
            $("#main > table").css("margin", "auto");
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="mng" runat="server">
    </asp:ScriptManager>
    <div style="width: 100%">
        <div style="width: 90%; margin: auto;">
            <table>
                <tr>
                    <td style="width: 50%">
                        <div id="tabs" style="height: 600px; width: 600px;">
                            <ul id="tabHeaders" runat="server">
                                <li><a href="#tabs-1">Image</a></li>
                                <li><a href="#tabs-2">3D</a></li>
                            </ul>
                            <div id="tabs-1" class="ui-tabs" style="height: 500px; width: 550px;">
                                <div id="scriptDisplay" runat="server" />
                                <asp:Image Height="500px" Width="500px" ID="ScreenshotImage" runat="server" ToolTip='<%# Eval("Title") %>' />
                                <br />
                            </div>
                            <div id="tabs-2" class="ui-tabs-hide" style="height: 500px; width: 550px;">
                                <table width="100%" style="height: 500px;">
                                    <tr>
                                        <td valign="middle" align="center" height="100%">
                                            <table width="100%" style="height: 100%">
                                                <tr>
                                                    <td height="100%">
                                                        <table id="container" width="90%" style="height: 60%;" border="2">
                                                            <tr>
                                                                <td height="20%">
                                                                    <div id="o3d" style="width: 100%; height: 100%;">
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <p>
                                                        </p>
                                                        Drag The Mouse To Rotate<br />
                                                        Scrollwheel To Zoom<br />
                                                        Resize The Window To Resize The View
                                                        <div style="color: red;" id="loading">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                    <td style="vertical-align: top;">
                        <table border="0" cellpadding="4" cellspacing="0" width="100%">
                            <tr runat="server" id="IDRow" visible="false">
                                <td>
                                    <asp:Label ID="IDLabel" runat="server" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="TitleLabel" runat="server" CssClass="ModelTitle"></asp:Label>
                                    <asp:HyperLink ID="editLink" Visible="false" runat="server" Text="Edit" CssClass="Hyperlink"></asp:HyperLink>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <ajax:Rating ID="ir" runat="server" CurrentRating='<%# Website.Common.CalculateAverageRating(Eval("Id")) %>' MaxRating="5" StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar" FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar" ReadOnly="true">
                                    </ajax:Rating>
                                </td>
                            </tr>
                            <tr runat="server" id="DescriptionRow">
                                <td>
                                    <asp:Label ID="DescriptionLabel" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr  runat="server" id="KeywordsRow">
                                <td>
                                    <span runat="server" id="keywordLabel">Keywords:</span> <span id="keywords" runat="server"></span>
                                </td>
                            </tr>
                            <tr  runat="server" id="MoreDetailsRow">
                                <td>
                                    <asp:HyperLink ID="MoreDetailsHyperLink" runat="server" Target="_blank" CssClass="Hyperlink" Text="More Details" />
                                </td>
                            </tr>
                            <tr runat="server" id="SubmitterEmailRow">
                                <td>
                                    Uploaded by
                                    <asp:HyperLink ID="SubmitterEmailHyperLink" runat="server" CssClass="Hyperlink">[SubmitterEmailHyperLink]</asp:HyperLink>
                                    &nbsp;on
                                    <asp:Label ID="UploadedDateLabel" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="SponsorLogoRow">
                                <td>
                                    Sponsor Logo:<br />
                                    <asp:Image ID="SponsorLogoImage" runat="server" />
                                </td>
                            </tr>
                            <tr runat="server" id="SponsorNameRow">
                                <td>
                                    Sponsor:
                                    <asp:HyperLink ID="SponsorNameHyperLink" runat="server" CssClass="Hyperlink" NavigateUrl="#">[SponsorNameHyperLink]</asp:HyperLink>
                                </td>
                            </tr>
                            <tr runat="server" id="DeveloperLogoRow">
                                <td>
                                    Developer Logo:<br />
                                    <asp:Image ID="DeveloperLogoImage" runat="server" />
                                </td>
                            </tr>
                            <tr runat="server" id="DeveloperRow">
                                <td>
                                    Developer:
                                    <asp:HyperLink ID="DeveloperNameHyperLink" runat="server" NavigateUrl="#" CssClass="Hyperlink">[DeveloperNameHyperLink]</asp:HyperLink>
                                </td>
                            </tr>
                            <tr runat="server" id="ArtistRow">
                                <td>
                                    Artist:
                                    <asp:HyperLink ID="ArtistNameHyperLink" runat="server" NavigateUrl="#" CssClass="Hyperlink">[ArtistNameHyperLink]</asp:HyperLink>
                                </td>
                            </tr>
                            <tr runat="server" id="NumPolygonsRow">
                                <td>
                                    #Polygons:
                                    <asp:Label ID="NumPolygonsLabel" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="NumTexturesRow">
                                <td>
                                    #Textures:
                                    <asp:Label ID="NumTexturesLabel" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="CCLRow">
                                <td>
                                    Creative Commons License:
                                    <asp:HyperLink ID="CCLHyperLink" runat="server" CssClass="Hyperlink" Target="_blank">View License</asp:HyperLink>
                                </td>
                            </tr>
                            
                            <tr runat="server" id="DownloadsRow">
                                <td>
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
                            <tr runat="server" id="LastModifiedRow">
                                <td>
                                    Last modified on
                                    <asp:Label ID="LastModifiedLabel" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="LocationRow">
                                <td>
                                    <asp:Label ID="LocationLabel" runat="server" Visible="true"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <b>
                        <br />
                        Download Model Type:</b>
                        <asp:DropDownList ID="ModelTypeDropDownList" runat="server">
                            <asp:ListItem>No Conversion</asp:ListItem>
                            <asp:ListItem>Collada</asp:ListItem>
                            <asp:ListItem>OBJ</asp:ListItem>
                            <asp:ListItem>3DS</asp:ListItem>
                            <asp:ListItem>O3D</asp:ListItem>
                        </asp:DropDownList>
                        <br /><br />
                        <asp:Button ID="DownloadButton" runat="server" Text="Download" ToolTip="Download" CommandName="DownloadZip" OnClick="DownloadButton_Click" />
                    &nbsp;<asp:Button ID="ReportViolationButton" runat="server" Text="Report Violation" OnClick="ReportViolationButton_Click"  />
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <asp:UpdatePanel ID="updatePanel" runat="server" EnableViewState="true" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <div class="ListTitle">
                        Comments and Reviews</div>
                    <br />
                    <ajax:Rating ID="rating" runat="server" CurrentRating="3" MaxRating="5" StarCssClass="ratingStar" OnChanged="Rating_Set" WaitingStarCssClass="savedRatingStar" FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar">
                    </ajax:Rating>
                    <br />
                    <asp:TextBox ID="ratingText" runat="server" TextMode="MultiLine" Columns="50">
                    </asp:TextBox>
                    <br />
                    <asp:Button ID="submitRating" Text="Add Rating" runat="server" OnClick="Rating_Click" />
                    <br />
                    <br />
                    <asp:GridView ID="CommentsGridView" runat="server" AutoGenerateColumns="false" BorderStyle="None" GridLines="None" ShowHeader="false">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <ajax:Rating ID="ir" runat="server" CurrentRating='<%# Eval("Rating") %>' MaxRating="5" StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar" FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar" ReadOnly="true">
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
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
