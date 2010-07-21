<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Upload.ascx.cs" Inherits="Controls_Upload" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="MissingTextures.ascx" TagName="MissingTextures" TagPrefix="uc1" %>
<ajax:ToolkitScriptManager ID="sm1" runat="server" />
<div style="width: 100%">
    <div style="margin: auto; width: 65%">
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View runat="server" ID="DefaultView">
                <div class="ListTitle">
                    Upload Asset
                </div>
                <table cellpadding="4" cellspacing="0" border="0">
                    <tr>
                        <td align="right" valign="top">
                            &nbsp;
                        </td>
                        <td align="left" valign="top">
                            <asp:Label ID="errorMessage" runat="server" CssClass="LoginFailureTextStyle"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblAssetType" CssClass="Bold" AssociatedControlID="TitleTextBox" runat="server"
                                ToolTip="Title of the asset">Asset Type:</asp:Label>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlAssetType" AutoPostBack="true" OnSelectedIndexChanged="ddlAssetType_Changed"
                                runat="server">
                                <asp:ListItem Text="Model" Value="Model"></asp:ListItem>
                                <asp:ListItem Text="Texture" Value="Texture"></asp:ListItem>
                                <asp:ListItem Text="Script" Value="Script"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            <asp:Label ID="Label2" CssClass="Bold" AssociatedControlID="TitleTextBox" runat="server"
                                ToolTip="Title of the asset">Title<span class="Red">*</span>:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="TitleTextBox" runat="server" Width="200px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TitleTextBox" ErrorMessage="Title Required" CssClass="LoginFailureTextStyle" Display="None" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            <ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="TitleTextBox" WatermarkCssClass="TextBoxWatermark" WatermarkText="ex. M16 Machine Gun" />
                            <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" HighlightCssClass="ValidatorCallOutStyle" Width="150px" TargetControlID="RequiredFieldValidator3" />
                        
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" CssClass="Hyperlink">?</asp:LinkButton>
                            <ajax:HoverMenuExtender ID="HoverMenuExtender1" runat="Server" OffsetX="6" OffsetY="0" PopDelay="50" PopupControlID="Panel1" PopupPosition="Right" TargetControlID="LinkButton1" />
                            &nbsp;<asp:Label ID="Label1" CssClass="Bold" AssociatedControlID="ContentFileUpload" runat="server" ToolTip="*.zip file path">File Upload<span class="Red">*</span>: </asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:FileUpload runat="server" ID="ContentFileUpload" Width="249px" />

                            <asp:Panel ID="Panel1" Style="display: none;" CssClass="HoverMenuStyle" Width="250px" runat="server">
                             
                            A zip file containing a *.dae file and any required textures. 
                           <br /> <br />
                            The textures may be in the zip root folder or in a subfolder.
                           </asp:Panel>	   
                            
                                                           

                            <asp:RequiredFieldValidator ID="ContentFileUploadRequiredFieldValidator" runat="server" ControlToValidate="ContentFileUpload" ErrorMessage=".Zip File Upload Required" CssClass="LoginFailureTextStyle" Display="None" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="zipValidator" runat="server" 
                                ControlToValidate="ContentFileUpload" Display="None" 
                                ErrorMessage="File must be in .zip format" Font-Bold="True" 
                                SetFocusOnError="true" 
                                ValidationExpression="(.*zip?|.*obj?|.*3ds?|.*lwo?|.*fbx?|.*dae?|.*Zip?|.*Obj?|.*Lwo?|.*Fbx?|.*Dae?|.*ZIP?|.*OBJ?|.*3DS?|.*LWO?|.*FBX?|.*DAE?)"></asp:RegularExpressionValidator>
                            <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" 
                                HighlightCssClass="ValidatorCallOutStyle" TargetControlID="zipValidator" 
                                Width="150px" />
                            <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" HighlightCssClass="ValidatorCallOutStyle" Width="150px" TargetControlID="ContentFileUploadRequiredFieldValidator" />
                       
                        </td>
                    </tr>
                    <tr id="thumbNailArea" runat="server">
                        <td align="right" valign="top">
                            <asp:Label ID="Label21" AssociatedControlID="ThumbnailFileUpload" CssClass="Bold"
                                runat="server" ToolTip="Thumbnail image of the asset">
                      Thumbnail<span class="Red">*</span>:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:FileUpload runat="server" ID="ThumbnailFileUpload" Width="200px" />
                            <asp:RequiredFieldValidator ID="ThumbnailFileUploadRequiredFieldValidator" runat="server" ControlToValidate="ThumbnailFileUpload" CssClass="LoginFailureTextStyle" Display="None" SetFocusOnError="true" ErrorMessage="Thumbnail Required" ></asp:RequiredFieldValidator>
                         <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" HighlightCssClass="ValidatorCallOutStyle" Width="150px" TargetControlID="ThumbnailFileUploadRequiredFieldValidator" />
                        
                        </td>
                    </tr>
                    <tr >
                        <td align="right" valign="top">
                            <asp:Label ID="Label3" CssClass="Bold" AssociatedControlID="DeveloperLogoFileUpload"
                                runat="server" ToolTip="Developer Logo"> Developer Logo:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:UpdatePanel ID="DeveloperLogoUpdatePanel" runat="server">
                             <ContentTemplate>
                             <asp:RadioButtonList ID="DeveloperLogoRadioButtonList" runat="server" AutoPostBack="True" onselectedindexchanged="DeveloperLogoRadioButtonList_SelectedIndexChanged">
                                <asp:ListItem Value="0" Selected="True">Use Current Logo</asp:ListItem>
                                <asp:ListItem Value="1">Upload Logo</asp:ListItem>
                                <asp:ListItem Value="2">None</asp:ListItem>
                            </asp:RadioButtonList>
                            
                            <asp:Image ID="DeveloperLogoImage" runat="server" />
                        
                            
                            <asp:Panel ID="DeveloperLogoFileUploadPanel" runat="server" Visible="false">                         
                             Upload New Developer Logo:<br />
                            <asp:FileUpload ID="DeveloperLogoFileUpload" runat="server" Width="200px" />
                            </asp:Panel>
                             
                             
                             </ContentTemplate>
                            

                            </asp:UpdatePanel>
                            
                           
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            <asp:Label ID="Label7" AssociatedControlID="DeveloperNameTextBox" CssClass="Bold"
                                runat="server" ToolTip="Developer Name"> Developer Name:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="DeveloperNameTextBox" runat="server" MaxLength="100" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            <asp:Label ID="Label8" CssClass="Bold" AssociatedControlID="SponsorLogoFileUpload"
                                runat="server" ToolTip="Sponsor Logo">Sponsor Logo:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                         <asp:UpdatePanel ID="SponsorLogoUpdatePanel" runat="server">
                           <ContentTemplate>
                           <asp:RadioButtonList ID="SponsorLogoRadioButtonList" runat="server" AutoPostBack="True" onselectedindexchanged="SponsorLogoRadioButtonList_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value="0">Use Current Logo</asp:ListItem>
                                <asp:ListItem Value="1">Upload Logo</asp:ListItem>
                                <asp:ListItem Value="2">None</asp:ListItem>
                            </asp:RadioButtonList>
                           
                            <asp:Image ID="SponsorLogoImage" runat="server" />
                           
                            
                            <asp:Panel ID="SponsorLogoFileUploadPanel" runat="server" Visible="false">
                             Upload New Sponsor Logo:<br />
                            <asp:FileUpload ID="SponsorLogoFileUpload" runat="server" Width="200px" />
                            </asp:Panel>
                           
                           
                           </ContentTemplate>
                            
                           </asp:UpdatePanel>
                        
                           
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            <asp:Label ID="Label9" CssClass="Bold" AssociatedControlID="SponsorNameTextBox" runat="server"
                                ToolTip="Sponsor Name">Sponsor Name: </asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox runat="server" ID="SponsorNameTextBox" MaxLength="100" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            <asp:Label ID="Label10" CssClass="Bold" AssociatedControlID="ArtistNameTextBox" runat="server"
                                ToolTip="Artist Name">Artist Name:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox runat="server" ID="ArtistNameTextBox" MaxLength="100" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            <asp:Label ID="Label11" CssClass="Bold" AssociatedControlID="FormatTextBox" runat="server"
                                ToolTip="Format">Format:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="FormatTextBox" runat="server" MaxLength="100" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            <asp:Label ID="Label12" CssClass="Bold" AssociatedControlID="CCLicenseDropDownList"
                                runat="server" ToolTip="Creative Commons License">Creative Commons License:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                          
                           <asp:UpdatePanel ID="CCLicenseUpdatePanel" runat="server">
                            <ContentTemplate>
                            <asp:DropDownList ID="CCLicenseDropDownList" runat="server" Font-Size="Small" AutoPostBack="True" onselectedindexchanged="CCLicenseDropDownList_SelectedIndexChanged">
                                <asp:ListItem Text="None" Value="None" Selected="True" ></asp:ListItem>
                                <asp:ListItem Text="Attribution Non-commercial No Derivatives (by-nc-nd)" Value="http://creativecommons.org/licenses/by-nc-nd/3.0/legalcode"></asp:ListItem>
                                <asp:ListItem Text="Attribution Non-commercial Share Alike (by-nc-sa)" Value="http://creativecommons.org/licenses/by-nc-sa/3.0/legalcode"></asp:ListItem>
                                <asp:ListItem Text="Attribution Non-commercial (by-nc)" Value="http://creativecommons.org/licenses/by-nc/3.0/legalcode"></asp:ListItem>
                                <asp:ListItem Text="Attribution No Derivatives (by-nd)" Value="http://creativecommons.org/licenses/by-nd/3.0/legalcode"></asp:ListItem>
                                <asp:ListItem Text="Attribution Share Alike (by-sa) " Value="http://creativecommons.org/licenses/by-sa/3.0/legalcode"></asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;<asp:HyperLink ID="CCLHyperLink" runat="server" Visible="False" Target="_blank" CssClass="Hyperlink">View</asp:HyperLink>
                        </ContentTemplate>
                           
                           </asp:UpdatePanel>
                                           </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            <asp:Label ID="Label4" CssClass="Bold" AssociatedControlID="DescriptionTextBox" runat="server"
                                ToolTip="Description of the asset"> Description:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox runat="server" ID="DescriptionTextBox" Rows="5" TextMode="MultiLine"
                                Width="400px"></asp:TextBox>
                            <ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="DescriptionTextBox"
                                WatermarkText="ex. This model contains a fully detailed M16 machine gun used for special operations."
                                WatermarkCssClass="TextBoxWatermark" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            <asp:Label ID="Label5" CssClass="Bold" AssociatedControlID="MoreInformationURLTextBox"
                                runat="server" ToolTip="URL for additional description of the asset"> More Information URL: </asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox runat="server" ID="MoreInformationURLTextBox" MaxLength="255" Width="200px"></asp:TextBox>
                            <ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="MoreInformationURLTextBox"
                                WatermarkText="ex. http://www.google.com" WatermarkCssClass="TextBoxWatermark" />
                            &nbsp;<asp:HyperLink ID="MoreInformationHyperLink" runat="server" Visible="False" CssClass="Hyperlink">View</asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="false" CssClass="Hyperlink">?</asp:LinkButton>
                            &nbsp;<asp:Label ID="Label6" runat="server" AssociatedControlID="KeywordsTextBox" CssClass="Bold" ToolTip="Tags/keywords to describe the asset">Tags:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="KeywordsTextBox" runat="server" Rows="5" TextMode="MultiLine" Width="400px"></asp:TextBox>
                            <ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" TargetControlID="KeywordsTextBox" WatermarkCssClass="TextBoxWatermark" WatermarkText="ex. guns,machine guns" />
                         
                            <asp:Panel ID="Panel2" Style="display: none;" CssClass="HoverMenuStyle" Width="250px" runat="server">
                              Keywords or phrases that describe your item (separated with commas)
                              </asp:Panel>	         

                         
                                    
                            <ajax:HoverMenuExtender ID="HoverMenuExtender2" runat="Server" TargetControlID="LinkButton2" PopupControlID="Panel2" PopupPosition="Right" OffsetX="6" OffsetY="0" PopDelay="50" />

                        
                        
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left" valign="top">
                            <asp:Button ID="Step1NextButton" runat="server" OnClick="Step1NextButton_Click" Text="Next &gt;" />
                            &nbsp;<asp:Button ID="Step1CancelButton" runat="server" CausesValidation="False"
                                OnClick="CancelButton_Click" Text="Cancel" ToolTip="Cancel" />
                        </td>
                    </tr>
                   
                </table>
            </asp:View>
            <asp:View runat="server" ID="ValidationView">
                <div class="ListTitle">
                    Validate Model
                </div>
                <table cellpadding="4" cellspacing="0" border="0">
                    <tr>
                        <td colspan="2">
                            <asp:Image Height="100px" Width="100px" ID="ModelImage" runat="server" ToolTip='<%# Eval("Title") %>'
                                ImageUrl="~/Content/25/SU27/SU27.jpg" Visible="False" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <asp:Label ID="Label14" runat="server" AssociatedControlID="UnitScaleTextBox" Text="Unit Scale:"
                                CssClass="Bold" ToolTip="Unit Scale"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="UnitScaleTextBox" runat="server" Width="200px" 
                                ontextchanged="UnitScaleTextBox_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <asp:Label ID="Label15" runat="server" Text="Up Axis:" AssociatedControlID="UpAxisRadioButtonList"
                                CssClass="Bold" ToolTip="Up Axis"></asp:Label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="UpAxisRadioButtonList" runat="server">
                                <asp:ListItem Selected="True">X</asp:ListItem>
                                <asp:ListItem>Y</asp:ListItem>
                                <asp:ListItem>Z</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <asp:Label ID="Label16" runat="server" Text="Number of Polygons:" AssociatedControlID="NumPolygonsTextBox"
                                CssClass="Bold" ToolTip="Number of Polygons"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="NumPolygonsTextBox" runat="server" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <asp:Label ID="Label17" runat="server" Text="Number of Textures:" AssociatedControlID="NumTexturesTextBox"
                                CssClass="Bold" ToolTip="Number of Textures"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="NumTexturesTextBox" runat="server" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <asp:Label ID="Label18" runat="server" Text="Textures UV Coordinate Channel:" AssociatedControlID="UVCoordinateChannelTextBox"
                                CssClass="Bold" ToolTip="Textures UV Coordinate Channel"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="UVCoordinateChannelTextBox" runat="server" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <asp:Label ID="Label13" runat="server" Text="Intention of Texture:" AssociatedControlID="IntentionofTextureTextBox"
                                CssClass="Bold" ToolTip="Intention of Texture"></asp:Label>
                        </td>
                        <td>
                           <asp:TextBox ID="IntentionofTextureTextBox" runat="server" Rows="5" TextMode="MultiLine" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left" valign="top">
                            <asp:Button ID="ValidationViewSubmitButton" runat="server" Text="Submit" CausesValidation="False"
                                OnClick="ValidationViewSubmitButton_Click" />
                            &nbsp;<asp:Button ID="SkipStepButton" runat="server" Text="Skip" OnClick="SkipStepButton_Click" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View runat="server" ID="MissingTextureView">
                <asp:Label ID="StepTitleLabel" runat="server" Text="Upload Missing Texture" CssClass="ListTitle"></asp:Label><br />
                <br />
                <uc1:MissingTextures ID="MissingTextures1" runat="server" />
                <uc1:MissingTextures ID="MissingTextures2" runat="server" />
                <uc1:MissingTextures ID="MissingTextures3" runat="server" />
                <uc1:MissingTextures ID="MissingTextures4" runat="server" />
                <uc1:MissingTextures ID="MissingTextures5" runat="server" />
                <uc1:MissingTextures ID="MissingTextures6" runat="server" />
                <uc1:MissingTextures ID="MissingTextures7" runat="server" />
                <uc1:MissingTextures ID="MissingTextures8" runat="server" />
                <br />
                <br />
                <asp:Button ID="MissingTextureViewBackButton" runat="server" OnClick="MissingTextureViewBackButton_Click"
                    Text="&lt; Back" />
                &nbsp;<asp:Button ID="MissingTextureViewNextButton" runat="server" OnClick="MissingTextureViewNextButton_Click"
                    Text="Next &gt;" />
                &nbsp;<asp:Button ID="MissingTextureViewCancelButton" runat="server" OnClick="MissingTextureViewCancelButton_Click"
                    Text="Cancel" />
            </asp:View>
        </asp:MultiView>
    </div>
</div>
