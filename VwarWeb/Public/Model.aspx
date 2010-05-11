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
    <style type="text/css">
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ToolkitScriptManager ID="sm1" runat="server">
    </ajax:ToolkitScriptManager>
    <div style="width: 100%">
        <div style="width: 90%; margin: auto;">
            <asp:FormView ID="ContentObjectFormView" runat="server" DataSourceID="LinqDataSource1"
                OnItemCommand="ContentObjectFormView_ItemCommand">
                <ItemTemplate>
                    <table>
                        <tr>
                            <td style="width: 50%">
                                <div id="tabs" style="height: 600px; width: 600px;">
                                    <ul>
                                        <li><a href="#tabs-1">Image</a></li>
                                        <li><a href="#tabs-2">3D</a></li>
                                    </ul>
                                    <div id="tabs-1" class="ui-tabs-hide" style="height: 500px; width: 550px;">
                                        <asp:Image Height="500px" Width="500px" ID="ScreenshotImage" runat="server" ToolTip='<%# Eval("Title") %>'
                                            ImageUrl='<%# Website.Common.FormatScreenshotImage(Eval("Id"), Eval("Screenshot")) %>' />
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
                                <asp:Label ID="IDLabel" runat="server" Text='<%# Bind("Id") %>' Visible="false"></asp:Label>
                                <asp:Label ID="TitleLabel" runat="server" Text='<%# Bind("Title") %>' CssClass="ModelTitle"></asp:Label>
                                <asp:HyperLink ID="editLink" Visible="false" runat="server" Text="Edit" NavigateUrl='<%# Website.Common.FormatEditUrl(Eval("Id")) %>'
                                    CssClass="Hyperlink"></asp:HyperLink>
                                <br />
                                <ajax:Rating ID="ir" runat="server" CurrentRating='<%# Website.Common.CalculateAverageRating(Eval("Id")) %>'
                                    MaxRating="5" StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar"
                                    FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar" ReadOnly="true">
                                </ajax:Rating>
                                <br />
                                <asp:Label ID="DescriptionLabel" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                <br />
                                <span runat="server" id="keywordLabel">Keywords:</span> <span id="keywords" runat="server">
                                </span>
                                <br />
                                <asp:HyperLink ID="DescriptionWebsiteURLHyperLink" runat="server" Target="_blank"
                                    CssClass="Hyperlink" Text="More Details" ToolTip='<%# Eval("DescriptionWebsiteURL") %>'
                                    NavigateUrl='<%# Eval("DescriptionWebsiteURL") %>' />
                                <br />
                                Uploaded by
                                <asp:HyperLink ID="SubmitterEmailHyperLink" runat="server" Text='<%# Website.Common.GetFullUserName( Eval("SubmitterEmail")) %>'
                                    CssClass="Hyperlink" ToolTip='<%# Eval("SubmitterEmail") %>' NavigateUrl='<%#"~/Public/Results.aspx?SubmitterEmail="+Eval("SubmitterEmail")%>' />
                                on
                                <asp:Label ID="UploadedDateLabel" runat="server" Text='<%# Eval("UploadedDate", "{0:d}") %>'></asp:Label>
                                <br />
                                <asp:Image ID="SubmitterLogoImageFilePathImage" runat="server" ImageUrl='<%# Website.Common.FormatSubmitterLogoImage(Eval("Id")) %>'
                                    ToolTip='<%# Eval("SubmitterEmail") %>' Visible='<%# Website.Common.ShowSubmitterLogoImage(Eval("Id")) %>' />
                                <br />
                                Downloads:
                                <asp:Label ID="Label12" runat="server" Text='<%# Bind("Downloads") %>'></asp:Label>
                                <br />
                                Views:
                                <asp:Label ID="Label13" runat="server" Text='<%# Bind("Views") %>'></asp:Label>
                                <br />
                                Last modified on
                                <asp:Label ID="LastModifiedLabel" runat="server" Text='<%# Bind("LastModified") %>'></asp:Label>
                                <br />
                                <asp:Button ID="DownloadButton" runat="server" Text="Download" ToolTip="Download"
                                    CommandName="DownloadZip" />
                                <asp:Label ID="LocationLabel" runat="server" Text='<%# Bind("Location") %>' Visible="true"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:FormView>
            <br />
            <br />
            <div class="ListTitle">
                Comments and Reviews</div>
            <br />
            <ajax:Rating ID="rating" runat="server" CurrentRating="3    " MaxRating="5" StarCssClass="ratingStar"
                WaitingStarCssClass="savedRatingStar" FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar">
            </ajax:Rating>
            <br />
            <asp:TextBox ID="ratingText" runat="server" TextMode="MultiLine" Columns="50"></asp:TextBox>
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
                            <asp:Label ID="Label2" Text='<%# Eval("ReviewText") %>' runat="server"></asp:Label>
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
            <asp:LinqDataSource ID="LinqDataSource1" runat="server" ContextTypeName="vwarDAL.vwarEntities"
                TableName="ContentObject" Where="Id == @Id">
                <WhereParameters>
                    <asp:QueryStringParameter DefaultValue="1" Name="Id" QueryStringField="ContentObjectID"
                        Type="Int32" />
                </WhereParameters>
            </asp:LinqDataSource>
        </div>
</asp:Content>
