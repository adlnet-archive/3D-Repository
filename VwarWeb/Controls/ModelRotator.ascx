<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ModelRotator.ascx.cs" Inherits="Controls_ModelRotator" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<table border="0" width="100%" style="padding: 15px">
                        <tr>
                           <%-- <td style="width: 50px;" align="center">
                                <asp:Image ID="LeftArrow" runat="server" ImageUrl="~/Images/Arrow_Left_ON.png" />
                            </td>--%>
                            <td align="center">
                                <telerik:RadRotator ID="RotatorListView" runat="server" Height="250px" ScrollDuration="500"
                                    FrameDuration="2000" Width="700px" RotatorType="AutomaticAdvance" WrapFrames="false">
                                    <ItemTemplate>
                                        <div class="radRotatoritemTemplate">
                                            <a id="A1" runat="server" href='<%# "~/Public/Model.aspx?ContentObjectID=" + Eval("PID") %>'>
                                                <%-- <img id="Img1" style="border: 0" src='<%# Website.Common.FormatScreenshotImage(Eval("PID"), Eval("Screenshot")) %>'
                                                    alt='<%# Eval("Title") %>' runat="server" class="DisplayImage" />--%>
                                                <telerik:RadBinaryImage ID="Img1" BorderWidth="0" runat="server" AlternateText='<%# Eval("Title") %>'
                                                    Width="100px" Height="100px" ResizeMode="Fit" ImageUrl='<%# String.Format("~/Public/Model.ashx?pid={0}&file={1}",Eval("PID"),Eval("Screenshot")) %>' />
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
                                                                FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar" ReadOnly="false"
                                                                Visible='<%# Website.Common.CalculateAverageRating(Eval("PID")) > 0 %>'>
                                                            </ajax:Rating>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <a id="A4" runat="server" href='<%# "~/Public/Model.aspx?ContentObjectID=" + Eval("PID") %>'
                                                style="font-size: 12px; color: #0E4F9C; font-weight: bold">
                                                <%# Eval("Title") %></a>
                                            <br />
                                            <br />
                                            <div style="margin-left: 5px; margin-right: 5px;">
                                                <asp:Label ID="DescriptionLabel" runat="server" Text='<%#
                                                    (Eval("Description") != null) ? FormatDescription((string)Eval("Description")) : "No description available" %>'
                                                    Font-Size="Small"></asp:Label><br />
                                            </div>
                                        </div>
                                        <div style="float: left; margin-top: 35px;">
                                            <asp:Image ID="VerticalLineSeparator" runat="server" ImageUrl="~/Images/Grey_Line.png" />
                                        </div>
                                    </ItemTemplate>
                                    <%-- <ControlButtons LeftButtonID="LeftArrow" RightButtonID="RightArrow" />--%>
                                </telerik:RadRotator>
                            </td>
                            <%-- <td style="width: 50px;" align="center">
                                <asp:Image ID="RightArrow" runat="server" ImageUrl="~/Images/Arrow_Right_ON.png" />
                            </td>--%>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <asp:HyperLink ID="ViewMoreHyperLink" runat="server" Style="font-size: 12px;
                                    color: #0E4F9C;" NavigateUrl="~/Public/Results.aspx?Group=rating-high">View All >></asp:HyperLink>
                            </td>
                        </tr>
                    </table>
