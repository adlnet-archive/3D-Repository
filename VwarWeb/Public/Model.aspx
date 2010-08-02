<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Model.aspx.cs" Inherits="Public_Model" Title="Model Details" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../scripts/o3djs/base.js"></script>
    <script type="text/javascript" src="../scripts/o3djs/simpleviewer.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-1.3.2.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.7.2.custom.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#tabs").tabs({ selected: 0 });
            $('#tabs').bind('tabsselect', function (event, ui) {

                if (ui.index == 2) {
                    var documentFrame = $("iframe")[0];
                    documentFrame.contentWindow.location.reload(true);
                    try {
                        documentFrame.contentWindow.swfDiv.SetScale($('#<%=upAxis.ClientID %>').val());
                        documentFrame.contentWindow.swfDiv.SetUpVec($('#<%=unitScale.ClientID %>').val());
                    } catch (ex) {
                        alert(ex.Message);
                    }

                }

            });
            $("#main > table").css("margin", "auto");

        });
        var contentUrl = ""
        function LoadAway3D(url) {

            var path = window.location.href;
            var index = path.lastIndexOf('/');
            var o3dfilename = path.substring(path.lastIndexOf('='), path.length);
            url = "Away3D/test3d_back.html?URL=" + path.substring(0, index + 1) + url;
            contentUrl = url;
            $('#displayArea').attr('src', url);
        } 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadAjaxManagerProxy runat="server" ID="RadAjaxManagerProxy1">
    </telerik:RadAjaxManagerProxy>
    <div id="ModelDetails">
        <input type="hidden" runat="server" id="upAxis" />
        <input type="hidden" runat="server" id="unitScale" />
        <table class="CenteredTable" cellpadding="4">
            <tr>
                <td>
                    <div id="tabs" style="height: 600px; width: 600px;">
                        <ul id="tabHeaders" runat="server">
                            <li><a href="#tabs-1">Image</a></li>
                            <li><a href="#tabs-3">3D</a></li>
                        </ul>
                        <div id="tabs-1" class="ui-tabs-hide" style="height: 500px; width: 550px;">
                            <div id="scriptDisplay" runat="server" />
                            <asp:Image SkinID="Image" Height="500px" Width="500px" ID="ScreenshotImage" runat="server"
                                ToolTip='<%# Eval("Title") %>' />
                            <br />
                        </div>
                        <div id="tabs-3" class="ui-tabs" style="height: 500px; width: 550px;">
                            <script type="text/javascript">
                                $("#displayArea").attr("src", contentUrl);                        
                            </script>
                            <iframe id="displayArea" style="height: 500px; width: 550px;"></iframe>
                        </div>
                    </div>
                </td>
                <td rowspan="2">
                    &nbsp;
                </td>
                <td rowspan="2">
                    <br />
                    <br />
                    <asp:HyperLink ID="editLink" Visible="false" runat="server" Text="Edit" CssClass="Hyperlink"></asp:HyperLink>
                    <table border="0" cellpadding="4" cellspacing="0" width="100%">
                        <tr runat="server" id="IDRow" visible="false">
                            <td>
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
                                            <asp:Label ID="TitleLabel" runat="server" CssClass="ModelTitle"></asp:Label>&nbsp;by&nbsp;<asp:HyperLink
                                                ID="SubmitterEmailHyperLink" runat="server" CssClass="Hyperlink">[SubmitterEmailHyperLink]</asp:HyperLink>
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
                                        <td>
                                            <br />
                                            Available File Formats:
                                            <div style="float: right; margin-left: 10px;">
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
                                            </div>
                                            <%--  <asp:DropDownList ID="ModelTypeDropDownList" runat="server" SkinID="DropDownList">
                                                <asp:ListItem Value="">No Conversion</asp:ListItem>
                                                <asp:ListItem Value=".dae">Collada</asp:ListItem>
                                                <asp:ListItem Value=".obj">OBJ</asp:ListItem>
                                                <asp:ListItem Value=".3ds">3DS</asp:ListItem>
                                                <asp:ListItem Value=".o3dtgz">O3D</asp:ListItem>
                                            </asp:DropDownList>--%>
                                        </td>
                                        <td style="vertical-align: bottom; text-align: center;">
                                            <asp:ImageButton ID="DownloadButton" runat="server" Text="Download" ToolTip="Download"
                                                CommandName="DownloadZip" OnClick="DownloadButton_Click" ImageUrl="~/Images/Download_BTN.png" />
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
                                            <asp:Image ID="DeveloperLogoImage" runat="server" ImageUrl="~/Images/Logo.gif" />
                                        </td>
                                    </tr>
                                    <tr runat="server" id="SubmitterEmailRow">
                                        <td>
                                            <asp:Label ID="UploadedDateLabel" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="DeveloperRow">
                                        <td>
                                            Developer Name:
                                            <asp:HyperLink ID="DeveloperNameHyperLink" runat="server" NavigateUrl="#" CssClass="Hyperlink">[DeveloperNameHyperLink]</asp:HyperLink>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="MoreDetailsRow">
                                        <td>
                                            <br />
                                            <asp:HyperLink ID="MoreDetailsHyperLink" runat="server" Target="_blank" CssClass="Hyperlink" />
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
                                            <asp:Image ID="SponsorLogoImage" runat="server" ImageUrl="~/Images/Logo.gif" />
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
                            <div class="ListTitle">
                                <div>
                                    Comments and Reviews</div>
                            </div>
                            <br />
                            <ajax:Rating ID="rating" runat="server" CurrentRating="3" MaxRating="5" StarCssClass="ratingStar"
                                OnChanged="Rating_Set" WaitingStarCssClass="savedRatingStar" FilledStarCssClass="filledRatingStar"
                                EmptyStarCssClass="emptyRatingStar">
                            </ajax:Rating>
                            <br />
                            <asp:TextBox ID="ratingText" runat="server" TextMode="MultiLine" Columns="50" SkinID="TextBox"
                                Rows="4"></asp:TextBox>
                            <br />
                            <asp:ImageButton ID="submitRating" Text="Add Rating" runat="server" OnClick="Rating_Click"
                                ImageUrl="~/Images/Add_Rating_BTN.png" />
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
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
