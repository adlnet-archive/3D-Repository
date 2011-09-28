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
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="Controls_Edit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="MissingTextures.ascx" TagName="MissingTextures" TagPrefix="uc1" %>
<%@ Register TagPrefix="VwarWeb" TagName="Viewer3D" Src="~/Controls/Viewer3D.ascx" %>
<%@ Register TagPrefix="VwarWeb" TagName="ModelUserPermissions" Src="~/Controls/ModelUserPermissions.ascx" %>
<%@ Register TagPrefix="VwarWeb" TagName="ModelGroupPermissions" Src="~/Controls/ModelGroupPermissions.ascx" %>
<%@ Register TagPrefix="uc1" TagName="GroupAdmin" Src="~/Controls/GroupAdmin.ascx" %>
<style type="text/css" media="screen">
    body
    {
        margin: 0;
        text-align: center;
    }
    div#content
    {
        text-align: left;
    }
    object#content
    {
        display: block;
        margin: 0 auto;
    }
    
    
    
    
    /*  RadUpload change browse button to image   */
    .RadUpload .ruBrowse
    {
        background-image: url('../styles/images/Browse_BTN.png') !important;
    }
</style>
<script type="text/javascript" src="../Public/Away3D/swfobject.js"></script>
<script type="text/javascript">

    $(document).ready(function () {
        $('#away3d_Wrapper').css("margin-left", "-27px");
    });


    function ApplyChangeToModel() {
        SetUnitScale($('#<%=UnitScaleTextBox.ClientID %>').val());
        SetCurrentUpAxis($('#<%=UpAxisRadioButtonList.ClientID %> input:radio:checked').val());
    }
</script>
<div id="UploadControl">
    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View runat="server" ID="DefaultView">
            <div class="ListTitle">
                &nbsp;&nbsp;Upload Asset
            </div>
            <table cellpadding="4" cellspacing="0" border="0" runat="server" id="MainTable">
                <tr>
                    <td align="right" valign="top">
                        &nbsp;
                    </td>
                    <td align="left" valign="top">
                        <asp:Label ID="errorMessage" runat="server" CssClass="LoginFailureTextStyle"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="style1">
                    </td>
                    <td align="left">
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top" class="style1">
                        <asp:Label ID="Label2" CssClass="Bold" AssociatedControlID="TitleTextBox" runat="server"
                            ToolTip="Title of the asset">Title<span class="Red">*</span>:</asp:Label>
                    </td>
                    <td align="left" valign="top">
                        <asp:TextBox ID="TitleTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TitleTextBox"
                            ErrorMessage="Title Required" CssClass="LoginFailureTextStyle" Display="None"
                            SetFocusOnError="true"></asp:RequiredFieldValidator>
                        <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" HighlightCssClass="ValidatorCallOutStyle"
                            Width="150px" TargetControlID="RequiredFieldValidator3" />
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top" class="style1">
                        <asp:Label ID="Label1" CssClass="Bold" AssociatedControlID="ContentFileUpload" runat="server"
                            ToolTip="*.zip fi
                                        le path">File Upload<span class="Red">*</span>: </asp:Label>
                    </td>
                    <td align="left" valign="top">
                        <asp:FileUpload ID="ContentFileUpload" runat="server" Width="430px" />
                        <asp:Panel ID="Panel1" Style="display: none;" CssClass="HoverMenuStyle" Width="250px"
                            runat="server">
                            A zip file containing a *.dae file and any required textures.
                            <br />
                            <br />
                            The textures may be in the zip root folder or in a subfolder.
                        </asp:Panel>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" CssClass="Hyperlink"
                            Style="position: relative; right: 95px;">?</asp:LinkButton>
                        <ajax:HoverMenuExtender ID="HoverMenuExtender1" runat="Server" OffsetX="6" OffsetY="0"
                            PopDelay="50" PopupControlID="Panel1" PopupPosition="Right" TargetControlID="LinkButton1" />
                        <asp:RequiredFieldValidator ID="ContentFileUploadRequiredFieldValidator" runat="server"
                            ControlToValidate="ContentFileUpload" ErrorMessage=".Zip File Upload Required"
                            CssClass="LoginFailureTextStyle" Display="None" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="zipValidator" runat="server" ControlToValidate="ContentFileUpload"
                            Display="None" ErrorMessage="File must be in .zip format" Font-Bold="True" SetFocusOnError="true"
                            ValidationExpression="^.*\.(zip|skp)$"></asp:RegularExpressionValidator>
                        <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" HighlightCssClass="ValidatorCallOutStyle"
                            TargetControlID="zipValidator" Width="150px" />
                        <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" HighlightCssClass="ValidatorCallOutStyle"
                            Width="150px" TargetControlID="ContentFileUploadRequiredFieldValidator" />
                    </td>
                </tr>
                <tr id="thumbNailArea" runat="server">
                    <td align="right" valign="top" class="style1">
                        <asp:Label ID="Label21" AssociatedControlID="ThumbnailFileUpload" CssClass="Bold"
                            runat="server" ToolTip="Thumbnail image of the asset">
                      Thumbnail<span class="Red">*</span>:</asp:Label>
                    </td>
                    <td align="left" valign="top">
                        <asp:FileUpload runat="server" ID="ThumbnailFileUpload" Width="430px" /><br />
                        <asp:Image ID="ThumbnailFileImage" runat="server" AutoAdjustImageControlSize="true"
                            ResizeMode="Fit" Visible="false" Width="100px" Height="100px" />

                            <asp:RegularExpressionValidator ID="ScreenshotValidator" runat="server" ControlToValidate="ThumbnailFileUpload"
                            ErrorMessage="Only GIF, JPEG, and PNG are supported" Font-Bold="true" SetFocusOnError="true"
                            ValidationExpression="^.*\.((jpg|JPG)|(png|PNG)|(gif|GIF))" />
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top" class="style1">
                        <asp:Label ID="Label12" CssClass="Bold" AssociatedControlID="CCLicenseDropDownList"
                            runat="server" ToolTip="Content License">Creative Commons License:</asp:Label>
                    </td>
                    <td align="left" valign="bottom">
                        <div style="float: left; margin-right: 10px;">
                            <asp:DropDownList ID="CCLicenseDropDownList" runat="server" EnableEmbeddedSkins="False"
                                Font-Size="Small" AutoPostBack="True" OnSelectedIndexChanged="CCLicenseDropDownList_SelectedIndexChanged"
                                NoWrap="True" Width="330px" CausesValidation="False">
                                <Items>
                                    <asp:ListItem runat="server" Text="Public domain" Value="http://creativecommons.org/publicdomain/mark/1.0/"
                                        Selected="True" />
                                    <asp:ListItem runat="server" Text="Attribution" Value="http://creativecommons.org/licenses/by/3.0/legalcode" />
                                    <asp:ListItem runat="server" Text="Attribution Share Alike (by-sa)" Value="http://creativecommons.org/licenses/by-sa/3.0/legalcode" />
                                    <asp:ListItem runat="server" Text="Attribution No Derivatives (by-nd)" Value="http://creativecommons.org/licenses/by-nd/3.0/legalcode" />
                                    <asp:ListItem runat="server" Text="Attribution Non-commercial (by-nc)" Value="http://creativecommons.org/licenses/by-nc/3.0/legalcode" />
                                    <asp:ListItem runat="server" Text="Attribution Non-commercial Share Alike (by-nc-sa)"
                                        Value="http://creativecommons.org/licenses/by-nc-sa/3.0/legalcode" />
                                    <asp:ListItem runat="server" Text="Attribution Non-commercial No Derivatives (by-nc-nd)"
                                        Value="http://creativecommons.org/licenses/by-nc-nd/3.0/legalcode" />
                                </Items>
                            </asp:DropDownList>
                        </div>
                        <asp:HyperLink ID="CCLHyperLink" runat="server" Visible="False" Target="_blank" CssClass="Hyperlink">View</asp:HyperLink>
                        <br />
                        <asp:CheckBox ID="RequireResubmitCheckbox" Style="position: relative; top: 20px"
                            runat="server" />
                        <div style="width: 300px; display: inline-block; vertical-align: text-top">
                            Require that any modifications be re-submitted back to the 3D Repository
                        </div>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Panel ID="HeaderPanel" runat="server">
                <div class="ListTitle">
                    &nbsp;&nbsp;Optional Information&nbsp;<asp:Image ID="ExpandCollapseImage" runat="server" /></div>
            </asp:Panel>
            <asp:Panel ID="CollapsiblePanel" runat="server">
                <table cellpadding="4" cellspacing="0" border="0">
                    <tr>
                        <td align="right" valign="top" width="224px">
                            <asp:Label ID="Label3" CssClass="Bold" AssociatedControlID="DeveloperLogoFileUpload"
                                runat="server" ToolTip="Developer Logo">Developer Logo:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:RadioButtonList ID="DeveloperLogoRadioButtonList" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="DeveloperLogoRadioButtonList_SelectedIndexChanged">
                                <asp:ListItem Value="0" Selected="True">Use Current Logo</asp:ListItem>
                                <asp:ListItem Value="1">Upload Logo</asp:ListItem>
                                <asp:ListItem Value="2">None</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:Image ID="DeveloperLogoImage" runat="server" />
                            <asp:Panel ID="DeveloperLogoFileUploadPanel" runat="server" Visible="false">
                                <asp:FileUpload ID="DeveloperLogoFileUpload" runat="server" Width="430px" />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" width="224px">
                            <asp:Label ID="Label7" AssociatedControlID="DeveloperNameTextBox" CssClass="Bold"
                                runat="server" ToolTip="Developer Name"> Developer Name:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="DeveloperNameTextBox" runat="server" MaxLength="100" CssClass="TextBox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" width="224px">
                            <asp:Label ID="Label10" CssClass="Bold" AssociatedControlID="ArtistNameTextBox" runat="server"
                                ToolTip="Artist Name">Artist:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox runat="server" ID="ArtistNameTextBox" MaxLength="100" CssClass="TextBox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Image ID="LineSeparator1" runat="server" ImageUrl="~/styles/images/grey_line_separator.png" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" width="224px">
                            <asp:Label ID="Label8" CssClass="Bold" AssociatedControlID="SponsorLogoFileUpload"
                                runat="server" ToolTip="Sponsor Logo">Sponsor Logo:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:RadioButtonList ID="SponsorLogoRadioButtonList" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="SponsorLogoRadioButtonList_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value="0">Use Current Logo</asp:ListItem>
                                <asp:ListItem Value="1">Upload Logo</asp:ListItem>
                                <asp:ListItem Value="2">None</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:Image ID="SponsorLogoImage" runat="server" />
                            <asp:Panel ID="SponsorLogoFileUploadPanel" runat="server" Visible="false">
                                <asp:FileUpload ID="SponsorLogoFileUpload" runat="server" Width="430px" />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" width="224px">
                            <asp:Label ID="Label9" CssClass="Bold" AssociatedControlID="SponsorNameTextBox" runat="server"
                                ToolTip="Sponsor Name">Sponsor Name: </asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox runat="server" ID="SponsorNameTextBox" MaxLength="100" CssClass="TextBox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/styles/images/grey_line_separator.png" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" width="224px">
                            <asp:Label ID="Label11" CssClass="Bold" AssociatedControlID="FormatTextBox" runat="server"
                                ToolTip="Format">Format:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="FormatTextBox" runat="server" MaxLength="100" CssClass="TextBox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" width="224px">
                            <asp:Label ID="Label4" CssClass="Bold" AssociatedControlID="DescriptionTextBox" runat="server"
                                ToolTip="Description of the asset"> Description:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox runat="server" ID="DescriptionTextBox" Rows="5" TextMode="MultiLine"
                                CssClass="TextBox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" width="224px">
                            <asp:Label ID="Label5" CssClass="Bold" AssociatedControlID="MoreInformationURLTextBox"
                                runat="server" ToolTip="URL for additional description of the asset">More Information URL:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox runat="server" ID="MoreInformationURLTextBox" MaxLength="255" CssClass="TextBox"></asp:TextBox>
                            &nbsp;<asp:HyperLink ID="MoreInformationHyperLink" runat="server" Visible="False"
                                CssClass="Hyperlink">View</asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" width="224px">
                            <asp:Label ID="Label6" runat="server" AssociatedControlID="KeywordsTextBox" CssClass="Bold"
                                ToolTip="Tags/keywords to describe the asset">Tags:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <div style="float: left;">
                                <asp:TextBox ID="KeywordsTextBox" CssClass="TextBox" runat="server" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <VwarWeb:ModelUserPermissions runat="server" ID="userPermissions" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <VwarWeb:ModelGroupPermissions runat="server" ID="groupPermissions" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <ajax:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="CollapsiblePanel"
                CollapsedSize="0" Collapsed="True" AutoCollapse="False" AutoExpand="False" CollapsedImage="~/styles/images/Collapse_Down_BTN.png"
                ExpandedImage="~/styles/images/Collapse_Up_BTN.png" ExpandDirection="Vertical"
                CollapseControlID="HeaderPanel" ExpandControlID="HeaderPanel" ImageControlID="ExpandCollapseImage"
                CollapsedText="Click to expand." ExpandedText="Click to collapse." />
            <asp:Button ID="Step1NextButton" runat="server" OnClick="Step1NextButton_Click" Text="Next &gt;" />
            &nbsp;<asp:Button ID="Step1CancelButton" runat="server" CausesValidation="False"
                OnClick="CancelButton_Click" Text="Cancel" ToolTip="Cancel" />
        </asp:View>
        <asp:View runat="server" ID="ValidationView">
            <div class="ListTitle">
                &nbsp;&nbsp;Validate Model
            </div>
            <table>
                <tr>
                    <td colspan="2">
                        <VwarWeb:Viewer3D ID="ModelViewer" runat="server" />
                    </td>
                    <td valign="top">
                        <div id="ViewerStatus" style="display: none; width: 300px; text-align: left; margin-left: -46px">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellpadding="4" cellspacing="0" border="0">
                            <tr>
                                <td align="right" valign="top">
                                    <asp:Label ID="Label14" runat="server" AssociatedControlID="UnitScaleTextBox" Text="Unit Scale:"
                                        CssClass="Bold" ToolTip="Unit Scale"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="UnitScaleTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    <asp:Label ID="Label15" runat="server" Text="Up Axis:" AssociatedControlID="UpAxisRadioButtonList"
                                        CssClass="Bold" ToolTip="Up Axis"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:RadioButtonList ID="UpAxisRadioButtonList" runat="server">
                                        <asp:ListItem Selected="True">Y</asp:ListItem>
                                        <asp:ListItem>Z</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    <asp:Label ID="Label16" runat="server" Text="Number of Polygons:" AssociatedControlID="NumPolygonsTextBox"
                                        CssClass="Bold" ToolTip="Number of Polygons"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="NumPolygonsTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    <asp:Label ID="Label17" runat="server" Text="Number of Textures:" AssociatedControlID="NumTexturesTextBox"
                                        CssClass="Bold" ToolTip="Number of Textures"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="NumTexturesTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top" class="style1">
                                    <asp:Label ID="Label18" runat="server" Text="Textures UV Coordinate Channel:" AssociatedControlID="UVCoordinateChannelTextBox"
                                        CssClass="Bold" ToolTip="Textures UV Coordinate Channel"></asp:Label>
                                </td>
                                <td align="left" class="style1">
                                    <asp:TextBox ID="UVCoordinateChannelTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="style1" valign="top">
                                    <strong>Thumbnail:</strong>
                                </td>
                                <td align="left" class="style1">
                                    <asp:Image ID="ThumbnailImage" runat="server" Height="200px" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    &nbsp;
                                </td>
                                <td align="left" valign="top">
                                    <asp:Button ID="ValidationViewSubmitButton" runat="server" Text="Submit" OnClick="ValidationViewSubmitButton_Click" />
                                    &nbsp;<asp:Button ID="SkipStepButton" runat="server" Text="Skip" OnClick="SkipStepButton_Click" />
                                    &nbsp;<input type="button" value="Apply" onclick="ApplyChangeToModel();" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View runat="server" ID="MissingTextureView">
            <table>
                <tr>
                    <td>
                        <div id="Div1">
                        </div>
                        <noscript>
                            <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" width="100%" height="100%"
                                id="Object1">
                                <param name="movie" value="ViewerApplication.swf" />
                                <param name="quality" value="high" />
                                <param name="bgcolor" value="#ffffff" />
                                <param name="allowScriptAccess" value="sameDomain" />
                                <param name="allowFullScreen" value="true" />
                                <!--[if !IE]>-->
                                <object type="application/x-shockwave-flash" data="\Public\Away3D\ViewerApplication.swf"
                                    width="100%" height="100%">
                                    <param name="quality" value="high" />
                                    <param name="bgcolor" value="#ffffff" />
                                    <param name="allowScriptAccess" value="sameDomain" />
                                    <param name="allowFullScreen" value="true" />
                                    <!--<![endif]-->
                                    <!--[if gte IE 6]>-->
                                    <p>
                                        Either scripts and active content are not permitted to run or Adobe Flash Player
                                        version 10.0.0 or greater is not installed.
                                    </p>
                                    <!--<![endif]-->
                                    <a href="http://www.adobe.com/go/getflashplayer">
                                        <img src="http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif"
                                            alt="Get Adobe Flash Player" />
                                    </a>
                                    <!--[if !IE]>-->
                                </object>
                                <!--<![endif]-->
                            </object>
                        </noscript>
                    </td>
                    <td>
                        <asp:Label ID="StepTitleLabel" runat="server" Text="Upload Missing Texture" CssClass="ListTitle"></asp:Label><br />
                        <br />
                        <asp:Panel ID="MissingTextureArea" runat="server">
                        </asp:Panel>
                        <br />
                        <br />
                        <asp:Button ID="MissingTextureViewBackButton" runat="server" OnClick="MissingTextureViewBackButton_Click"
                            Text="&lt; Back" />
                        &nbsp;<asp:Button ID="MissingTextureViewNextButton" runat="server" OnClick="MissingTextureViewNextButton_Click"
                            Text="Next &gt;" />
                        &nbsp;<asp:Button ID="MissingTextureViewCancelButton" runat="server" OnClick="MissingTextureViewCancelButton_Click"
                            Text="Cancel" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</div>
