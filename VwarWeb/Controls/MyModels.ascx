<%--
Copyright 2011 U.S. Department of Defense

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
--%>



<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MyModels.ascx.cs" Inherits="Controls_MyModels" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<div class="ListTitle">My Models</div>
<asp:DataList ID="MyModelsDataList" runat="server" CellPadding="25" RepeatColumns="2" RepeatDirection="Horizontal">
    <ItemTemplate>
      <div style="height:200px; width:170px;">
             <a id="A1" runat="server" href='<%# "~/Public/Model.aspx?ContentObjectID=" + Eval("PID") %>'>
                                             
                                                <asp:Image ID="Img1" BorderWidth="0" runat="server" AlternateText='<%# Eval("Title") %>' Width="100px" Height="100px" ResizeMode="Fit" ImageUrl='<%# String.Format("~/Public/Model.ashx?pid={0}&file={1}",Eval("PID"),Eval("Screenshot")) %>' />
                                            </a>
                                            <br />
                                              <div style="clear: both; margin: auto;">
                                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                    <tr>
                                                        <td style="width: 32%">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <ajax:Rating ID="ir" runat="server" CurrentRating='<%# Website.Common.CalculateAverageRating(Eval("Reviews")) %>'
                                                                MaxRating="5" StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar"
                                                                FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar" ReadOnly="false" Visible='<%# Website.Common.CalculateAverageRating(Eval("Reviews")) > 0 %>'>
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
                                                Font-Size="Small"></asp:Label><br />
      </div>                                    
    </ItemTemplate>
    
</asp:DataList>
