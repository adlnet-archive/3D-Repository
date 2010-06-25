<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Model.aspx.cs" Inherits="Public_Model" Title="Model Details" %>

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
                        <asp:Label ID="IDLabel" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="TitleLabel" runat="server" CssClass="ModelTitle"></asp:Label>
                        <asp:HyperLink ID="editLink" Visible="false" runat="server" Text="Edit" CssClass="Hyperlink"></asp:HyperLink>
                        <br />
                        <ajax:Rating ID="ir" runat="server" CurrentRating='<%# Website.Common.CalculateAverageRating(Eval("Id")) %>'
                            MaxRating="5" StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar"
                            FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar" ReadOnly="true">
                        </ajax:Rating>
                        <br />
                        <asp:Label ID="DescriptionLabel" runat="server"></asp:Label>
                        <br />
                        <span runat="server" id="keywordLabel">Keywords:</span> <span id="keywords" runat="server">
                        </span>
                        <br />
                        <asp:HyperLink ID="DescriptionWebsiteURLHyperLink" runat="server" Target="_blank"
                            CssClass="Hyperlink" Text="More Details" />
                        <br />
                        Uploaded by
                        <asp:HyperLink ID="SubmitterEmailHyperLink" runat="server" CssClass="Hyperlink">[SubmitterEmailHyperLink]</asp:HyperLink>
                        on
                        <asp:Label ID="UploadedDateLabel" runat="server"></asp:Label>
                        <br />
                        Sponsor Logo:<br />
                        <asp:Image ID="SponsorLogoImageFilePathImage" runat="server" />
                        <br />
                        Sponsor:<asp:HyperLink ID="SponsorNameHyperLink" runat="server" CssClass="Hyperlink" NavigateUrl="#">[SponsorNameHyperLink]</asp:HyperLink>
                        <br />
                        Developer Logo:<br />
                        <asp:Image ID="DeveloperLogoImageFilePathImage" runat="server" />
                        <br />
                        Developer:<asp:HyperLink ID="DeveloperNameHyperLink" runat="server" NavigateUrl="#" CssClass="Hyperlink">[DeveloperNameHyperLink]</asp:HyperLink>
                        <br />
                        Artist:<asp:HyperLink ID="ArtistNameHyperLink" runat="server" NavigateUrl="#" CssClass="Hyperlink">[ArtistNameHyperLink]</asp:HyperLink>
                        <br />
                        #Polygons:
                        <asp:Label ID="NumPolygonsLabel" runat="server">5</asp:Label>
                        <br />
                        #Textures:
                        <asp:Label ID="NumTexturesLabel" runat="server">2</asp:Label>
                        <br />
                        Creative Commons License
                        <asp:HyperLink ID="CCLHyperLink" runat="server" CssClass="Hyperlink" NavigateUrl="#">View License</asp:HyperLink>
                        <br />
                        Reviewer Tags:<asp:HyperLink ID="ReviewerTagsHyperLink" runat="server" NavigateUrl="#" CssClass="Hyperlink">[ReviewerTagsHyperLink]</asp:HyperLink>
                        <br />
                        Downloads:
                        <asp:Label ID="Label12" runat="server"></asp:Label>
                        <br />
                        Views:
                        <asp:Label ID="Label13" runat="server"></asp:Label>
                        <br />
                        Last modified on
                        <asp:Label ID="LastModifiedLabel" runat="server"></asp:Label>
                        <br />
                        <asp:Button ID="ReportViolationButton" runat="server" Text="Report Violation" OnClick="ReportViolationButton_Click"
                            OnClientClick="return confirm(Click OK to report the violation. An email will be sent to support.)" />
                        <br />
                        <asp:Label ID="LocationLabel" runat="server" Visible="true"></asp:Label><br />
                        <br />
                        <b>Download Model Type:</b>
                        <asp:DropDownList ID="ModelTypeDropDownList" runat="server">
                            <asp:ListItem>No Conversion</asp:ListItem>
                            <asp:ListItem>Collada</asp:ListItem>
                            <asp:ListItem>OBJ</asp:ListItem>
                            <asp:ListItem>3DS</asp:ListItem>
                            <asp:ListItem>O3D</asp:ListItem>
                        </asp:DropDownList>
                        <br />
                        <asp:Button ID="DownloadButton" runat="server" Text="Download" ToolTip="Download"
                            CommandName="DownloadZip" OnClick="DownloadButton_Click" />
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
                    <ajax:Rating ID="rating" runat="server" CurrentRating="3" MaxRating="5" StarCssClass="ratingStar" OnChanged="Rating_Set"
                        WaitingStarCssClass="savedRatingStar" FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar">
                    </ajax:Rating>
                    <br />
                    <asp:TextBox ID="ratingText" runat="server" TextMode="MultiLine" Columns="50">
                    </asp:TextBox>
                    <br />
                    <asp:Button ID="submitRating" Text="Add Rating" runat="server" OnClick="Rating_Click" />
                    <br />
                    <br />
                    <asp:GridView ID="CommentsGridView" runat="server" AutoGenerateColumns="false" BorderStyle="None"
                        GridLines="None" ShowHeader="false">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <ajax:Rating ID="ir" runat="server" CurrentRating='<%# Eval("Rating") %>' MaxRating="5"
                                        StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar" FilledStarCssClass="filledRatingStar"
                                        EmptyStarCssClass="emptyRatingStar" ReadOnly="true">
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
