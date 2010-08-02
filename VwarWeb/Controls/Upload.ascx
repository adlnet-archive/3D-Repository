<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Upload.ascx.cs" Inherits="Controls_Upload" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="MissingTextures.ascx" TagName="MissingTextures" TagPrefix="uc1" %>

<telerik:RadAjaxManagerProxy runat="server" ID="RadAjaxManagerProxy1">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="ddlAssetType">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="MainTable" UpdatePanelHeight="" />
            </UpdatedControls>
        </telerik:AjaxSetting>
       <%-- <telerik:AjaxSetting AjaxControlID="Step1NextButton">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="ThumbnailFileUpload" 
                    UpdatePanelHeight="" />
            </UpdatedControls>
        </telerik:AjaxSetting>--%>
        <telerik:AjaxSetting AjaxControlID="CCLicenseDropDownList">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="CCLHyperLink" UpdatePanelHeight="" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="AddKeywordButton">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="KeywordsListBox" UpdatePanelHeight="" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="RemoveKeywordsButton">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="KeywordsListBox" UpdatePanelHeight="" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>


</telerik:RadAjaxManagerProxy>

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
        background-image: url('../Images/Browse_BTN.png') !important;  
    } 

    
    </style>
    <script type="text/javascript" src="/VwarWeb/Scripts/jquery-1.3.2.min.js"></script>
    <script type="text/javascript" src="/VwarWeb/Public/Away3D/swfobject.js"></script>
   <script type="text/javascript">
        function attachSWF() {
            // <!-- For version detection, set to min. required Flash Player version, or 0 (or 0.0.0), for no version detection. --> 
            var swfVersionStr = "10.0.0";
            // <!-- To use express install, set to playerProductInstall.swf, otherwise the empty string. -->
            var xiSwfUrlStr = "/VwarWeb/Public/Away3D/playerProductInstall.swf";
            var flashvars = {};
            var params = {};
            params.quality = "high";
            params.bgcolor = "#ffffff";
            params.allowscriptaccess = "sameDomain";
            params.allowfullscreen = "true";
            var attributes = {};
            attributes.id = "test3d";
            attributes.name = "test3d";
            attributes.align = "middle";

            swfobject.embedSWF(
                "/VwarWeb/Public/Away3D/test3d.swf", "flashContent",
                "500", "500",
                swfVersionStr, xiSwfUrlStr,
                flashvars, params, attributes);
            //	<!-- JavaScript enabled so display the flashContent div in case it is not replaced with a swf object. -->
            //swfobject.createCSS("#flashContent", "display:block;text-align:left;");
        }
        var swfDiv;
        var gURL;
        function getID(swfID) {
            if (navigator.appName.indexOf("Microsoft") != -1) {
                swfDiv = window[swfID];
            } else {
                swfDiv = document[swfID];
            }
        }

        var qsParm = new Array();
        function qs() {
            var query = window.location.search.substring(1);
            qsParm['URL'] = query.replace("URL=", "");

        }
        function load() {
            getID('test3d');
            try {
                swfDiv.Load(gURL);
            } catch (ex) {
                alert(ex.Message);
            }

        }
        function DoLoadURL(URL) {

            gURL = URL;
            attachSWF();
            setTimeout('load()', 500);
        }
        function ApplyChangeToModel() {
            swfDiv.SetScale($('#<%=UnitScaleTextBox.ClientID %>').val());
            swfDiv.SetUpVec($('#<%=UpAxisRadioButtonList.ClientID %> input:radio:checked').val());
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
                            <asp:Label ID="lblAssetType" CssClass="Bold" AssociatedControlID="TitleTextBox" runat="server"
                                ToolTip="Title of the asset">Asset Type:</asp:Label>
                        </td>
                        <td align="left">
                        <div style="float:left; margin-right:10px;">
                        <telerik:RadComboBox ID="ddlAssetType" AutoPostBack="True" 
                                onselectedindexchanged="ddlAssetType_Changed" runat="server" Width="330px" 
                                CausesValidation="False" EnableEmbeddedSkins="False">
                            <Items>
                                  <telerik:RadComboBoxItem runat="server" Text="Model"  Value="Model"  Selected="True" />
                                     
                                  <telerik:RadComboBoxItem runat="server" Text="Texture"  Value="Texture" />
                                  <telerik:RadComboBoxItem runat="server" Text="Script"  Value="Script" />
                                  <telerik:RadComboBoxItem runat="server" Text="Other"  Value="Other" />
                            </Items>
                        
                        </telerik:RadComboBox></div><asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="false" CssClass="Hyperlink">?</asp:LinkButton>
                                


                            <%--<asp:DropDownList ID="ddlAssetType" AutoPostBack="true" OnSelectedIndexChanged="ddlAssetType_Changed"
                                runat="server" Width="330px" SkinID="DropDownList">
                                <asp:ListItem Text="Model" Value="Model"></asp:ListItem>
                                <asp:ListItem Text="Texture" Value="Texture"></asp:ListItem>
                                <asp:ListItem Text="Script" Value="Script"></asp:ListItem>
                                 <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                            </asp:DropDownList>--%>
                            
                            
                             <asp:Panel ID="Panel3" Style="display: none;" CssClass="HoverMenuStyle" Width="250px"
                                runat="server">
                                If NURBS select "Other".                               
                            </asp:Panel>
                            
                            <ajax:HoverMenuExtender ID="HoverMenuExtender3" runat="Server" OffsetX="6" OffsetY="0"
                                PopDelay="50" PopupControlID="Panel3" PopupPosition="Right" TargetControlID="LinkButton3" />
                       
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" class="style1">
                            <asp:Label ID="Label2" CssClass="Bold" AssociatedControlID="TitleTextBox" runat="server"
                                ToolTip="Title of the asset">Title<span class="Red">*</span>:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="TitleTextBox" runat="server" CssClass="TextBox" 
                                SkinID="TextBox"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TitleTextBox"
                                ErrorMessage="Title Required" CssClass="LoginFailureTextStyle" Display="None"
                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                            <%--<ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="TitleTextBox"
                                WatermarkCssClass="TextBoxWatermark" WatermarkText="ex. M16 Machine Gun" />--%>
                            <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" HighlightCssClass="ValidatorCallOutStyle"
                                Width="150px" TargetControlID="RequiredFieldValidator3" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" class="style1">
                            <asp:Label ID="Label1" CssClass="Bold" AssociatedControlID="ContentFileUpload"
                                runat="server" ToolTip="*.zip file path">File Upload<span class="Red">*</span>: </asp:Label>
                            
                           
                            
                        </td>

                        <td align="left" valign="top">
                            <asp:FileUpload runat="server" ID="ContentFileUpload" Width="430px" />
                            <asp:Panel ID="Panel1" Style="display: none;" CssClass="HoverMenuStyle" Width="250px"
                                runat="server">
                                A zip file containing a *.dae file and any required textures.
                                <br />
                                <br />
                                The textures may be in the zip root folder or in a subfolder.
                            </asp:Panel>

                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" CssClass="Hyperlink">?</asp:LinkButton>
                            <ajax:HoverMenuExtender ID="HoverMenuExtender1" runat="Server" OffsetX="6" OffsetY="0"
                                PopDelay="50" PopupControlID="Panel1" PopupPosition="Right" TargetControlID="LinkButton1" />


                            <%--<asp:RequiredFieldValidator ID="ContentFileUploadRequiredFieldValidator" runat="server"
                                ControlToValidate="ContentFileUpload" ErrorMessage=".Zip File Upload Required"
                                CssClass="LoginFailureTextStyle" Display="None" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="zipValidator" runat="server" ControlToValidate="ContentFileUpload"
                                Display="None" ErrorMessage="File must be in .zip format" Font-Bold="True" SetFocusOnError="true"
                                ValidationExpression="(.*zip?|.*obj?|.*3ds?|.*lwo?|.*fbx?|.*dae?|.*Zip?|.*Obj?|.*Lwo?|.*Fbx?|.*Dae?|.*ZIP?|.*OBJ?|.*3DS?|.*LWO?|.*FBX?|.*DAE?)"></asp:RegularExpressionValidator>
                            <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" HighlightCssClass="ValidatorCallOutStyle"
                                TargetControlID="zipValidator" Width="150px" />
                            <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" HighlightCssClass="ValidatorCallOutStyle"
                                Width="150px" TargetControlID="ContentFileUploadRequiredFieldValidator" />--%>
                        
                        
                        
                        </td>
                    </tr>
                    <tr id="thumbNailArea" runat="server">
                        
                           <td align="right" valign="top" class="style1">
                            <asp:Label ID="Label21" AssociatedControlID="ThumbnailFileUpload" CssClass="Bold"
                                runat="server" ToolTip="Thumbnail image of the asset">
                      Thumbnail<span class="Red">*</span>:</asp:Label>
                        </td>
                           <td align="left" valign="top">
                       
                       <%--<telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                          <script type="text/javascript">

                              function clientFileUploaded(sender, args) {
                                  var ajaxManager = $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>");
                                  ajaxManager.ajaxRequest("BindImage");

                              }


                              function clientDeleting(sender, args) {
                                  var ajaxManager = $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>");
                                  ajaxManager.ajaxRequest("RemoveImage");

                              }


                              


                            </script>

                        </telerik:RadScriptBlock> --%>

                            <asp:FileUpload runat="server" ID="ThumbnailFileUpload" Width="430px" />

                        <%--  <telerik:RadAsyncUpload runat="server" ID="ThumbnailFileUpload"  InputSize="57"   maxfileinputscount="1"  OnFileUploaded="ThumbnailFileUpload_FileUploaded" OnClientFileUploaded="clientFileUploaded" OnClientDeleting="clientDeleting" >
                            <Localization Select="" />
                          
                          </telerik:RadAsyncUpload>
                                                  
                           <telerik:RadBinaryImage ID="ThumbnailFileImage" runat="server" AutoAdjustImageControlSize="true" ResizeMode="Fit" Visible="false" />--%>
                           
                           <%--<asp:RequiredFieldValidator ID="ThumbnailFileUploadRequiredFieldValidator" runat="server"
                                ControlToValidate="ThumbnailFileUpload" CssClass="LoginFailureTextStyle" Display="None"
                                SetFocusOnError="true" ErrorMessage="Thumbnail Required"></asp:RequiredFieldValidator>


                            <ajax:ValidatorCalloutExtender ID="ThumbnailFileUploadValidatorCalloutExtender4" runat="server" HighlightCssClass="ValidatorCallOutStyle"
                                Width="150px" TargetControlID="ThumbnailFileUploadRequiredFieldValidator"  />
                            --%>
                          
                        </td>
                        
                      
                        
                     
                    </tr>

                    <tr>
                        <td align="right" valign="top" class="style1">
                            <asp:Label ID="Label12" CssClass="Bold" AssociatedControlID="CCLicenseDropDownList"
                                runat="server" ToolTip="Creative Commons License">Creative Commons License:</asp:Label>
                        </td>
                        <td align="left" valign="bottom" >
                            <div style="float:left; margin-right:10px;">
                            
                            <telerik:RadComboBox ID="CCLicenseDropDownList" runat="server" 
                                EnableEmbeddedSkins="False" Font-Size="Small" AutoPostBack="True" 
                                onselectedindexchanged="CCLicenseDropDownList_SelectedIndexChanged" 
                                NoWrap="True" Width="330px" CausesValidation="False">
                                        
                                <Items>
                                    <telerik:RadComboBoxItem runat="server" 
                                        Text="Attribution Non-commercial Share Alike (by-nc-sa)"  
                                        Value="http://creativecommons.org/licenses/by-nc-sa/3.0/legalcode" 
                                        Selected="True" />
                                       
                                    <telerik:RadComboBoxItem runat="server" Text="Attribution Non-commercial No Derivatives (by-nc-nd)"  Value="http://creativecommons.org/licenses/by-nc-nd/3.0/legalcode" />
                                       
                                    <telerik:RadComboBoxItem runat="server" Text="Attribution Non-commercial (by-nc)"  Value="http://creativecommons.org/licenses/by-nc/3.0/legalcode" />
                                      
                                    <telerik:RadComboBoxItem runat="server" Text="Attribution No Derivatives (by-nd)" Value="http://creativecommons.org/licenses/by-nd/3.0/legalcode" />
                                       
                                    <telerik:RadComboBoxItem runat="server" Text="Attribution Share Alike (by-sa)" Value="http://creativecommons.org/licenses/by-sa/3.0/legalcode" />
                                     <telerik:RadComboBoxItem runat="server" Text="None" Value="" />
                                        
                                </Items>
                                        
                            </telerik:RadComboBox>
                            </div>
                            
                          
                             <asp:HyperLink ID="CCLHyperLink" runat="server" Visible="False" Target="_blank" CssClass="Hyperlink">View</asp:HyperLink>
                   
                           
                          
                                
                            
                          
                          
                         

                            <%--<asp:UpdatePanel ID="CCLicenseUpdatePanel" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="CCLicenseDropDownList" runat="server" Font-Size="Small" AutoPostBack="True"
                                        OnSelectedIndexChanged="CCLicenseDropDownList_SelectedIndexChanged" 
                                        SkinID="DropDownList">                                        
                                        <asp:ListItem Text="Attribution Non-commercial Share Alike (by-nc-sa)" Value="http://creativecommons.org/licenses/by-nc-sa/3.0/legalcode"  Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Attribution Non-commercial No Derivatives (by-nc-nd)" Value="http://creativecommons.org/licenses/by-nc-nd/3.0/legalcode"></asp:ListItem>                                       
                                        <asp:ListItem Text="Attribution Non-commercial (by-nc)" Value="http://creativecommons.org/licenses/by-nc/3.0/legalcode"></asp:ListItem>
                                        <asp:ListItem Text="Attribution No Derivatives (by-nd)" Value="http://creativecommons.org/licenses/by-nd/3.0/legalcode"></asp:ListItem>
                                        <asp:ListItem Text="Attribution Share Alike (by-sa)" Value="http://creativecommons.org/licenses/by-sa/3.0/legalcode"></asp:ListItem>
                                        <asp:ListItem Text="None" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                   
                                </ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </td>
                    </tr>
                    </table>

                    <br />
                <asp:Panel ID="HeaderPanel" runat="server">
              
                <div class="ListTitle">&nbsp;&nbsp;Optional Information&nbsp;<asp:Image ID="ExpandCollapseImage" runat="server" /></div>
                
                </asp:Panel>

                 <asp:Panel ID="CollapsiblePanel" runat="server">                    
                    <table cellpadding="4" cellspacing="0" border="0">


                    <tr>
                        <td align="right" valign="top" width="224px">
                            <asp:Label ID="Label3" CssClass="Bold" AssociatedControlID="DeveloperLogoFileUpload"
                                runat="server" ToolTip="Developer Logo">Developer Logo:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:UpdatePanel ID="DeveloperLogoUpdatePanel" runat="server">
                                <ContentTemplate>
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
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" width="224px">
                            <asp:Label ID="Label7" AssociatedControlID="DeveloperNameTextBox" CssClass="Bold"
                                runat="server" ToolTip="Developer Name"> Developer Name:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="DeveloperNameTextBox" runat="server" MaxLength="100" 
                                CssClass="TextBox" SkinID="TextBox"></asp:TextBox>
                        </td>
                    </tr>

                     <tr>
                        <td align="right" valign="top" width="224px">
                            <asp:Label ID="Label10" CssClass="Bold" AssociatedControlID="ArtistNameTextBox" runat="server"
                                ToolTip="Artist Name">Artist:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox runat="server" ID="ArtistNameTextBox" MaxLength="100" 
                                CssClass="TextBox" SkinID="TextBox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Image ID="LineSeparator1" runat="server" ImageUrl="~/Images/grey_line_separator.png" />
                        </td> 
                    </tr>

                    <tr>
                        <td align="right" valign="top" width="224px">
                            <asp:Label ID="Label8" CssClass="Bold" AssociatedControlID="SponsorLogoFileUpload"
                                runat="server" ToolTip="Sponsor Logo">Sponsor Logo:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:UpdatePanel ID="SponsorLogoUpdatePanel" runat="server">
                                <ContentTemplate>
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
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" width="224px">
                            <asp:Label ID="Label9" CssClass="Bold" AssociatedControlID="SponsorNameTextBox" runat="server"
                                ToolTip="Sponsor Name">Sponsor Name: </asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox runat="server" ID="SponsorNameTextBox" MaxLength="100" 
                                CssClass="TextBox" SkinID="TextBox"></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td colspan="2" align="center">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/grey_line_separator.png" />
                        </td> 
                    </tr>
                   
                    <tr>
                        <td align="right" valign="top" width="224px">
                            <asp:Label ID="Label11" CssClass="Bold" AssociatedControlID="FormatTextBox" runat="server"
                                ToolTip="Format">Format:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="FormatTextBox" runat="server" MaxLength="100" 
                                CssClass="TextBox" SkinID="TextBox"></asp:TextBox>
                        </td>
                    </tr>
                    
                    <tr>
                        <td align="right" valign="top" width="224px">
                            <asp:Label ID="Label4" CssClass="Bold" AssociatedControlID="DescriptionTextBox" runat="server"
                                ToolTip="Description of the asset"> Description:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox runat="server" ID="DescriptionTextBox" Rows="5" 
                                TextMode="MultiLine" CssClass="TextBox" SkinID="TextBox"></asp:TextBox>
                           <%-- <ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="DescriptionTextBox"
                                WatermarkText="ex. This model contains a fully detailed M16 machine gun used for special operations."
                                WatermarkCssClass="TextBoxWatermark" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" width="224px">
                            <asp:Label ID="Label5" CssClass="Bold" AssociatedControlID="MoreInformationURLTextBox"
                                runat="server" ToolTip="URL for additional description of the asset">More Information URL:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox runat="server" ID="MoreInformationURLTextBox" MaxLength="255" 
                                CssClass="TextBox" SkinID="TextBox"></asp:TextBox>
                           <%-- <ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="MoreInformationURLTextBox"
                                WatermarkText="ex. http://www.google.com" WatermarkCssClass="TextBoxWatermark" />--%>
                            &nbsp;<asp:HyperLink ID="MoreInformationHyperLink" runat="server" Visible="False"
                                CssClass="Hyperlink">View</asp:HyperLink>
                        </td>
                    </tr>
                      <tr>
                            <td align="right" valign="top" width="224px">
                                <asp:Label ID="Label6" runat="server" AssociatedControlID="KeywordsTextBox"
                                    CssClass="Bold" ToolTip="Tags/keywords to describe the asset">Tags:</asp:Label>
                            </td>
                            <td align="left" valign="top">
                                
                               <%--  <asp:TextBox ID="KeywordsTextBox" runat="server" Width="300px" 
                                    AutoCompleteType="Disabled"></asp:TextBox> --%>
                                     <telerik:RadScriptBlock runat="server" ID="KeywordsScriptBlock">                              
                                        <script type="text/javascript">

                                            function OnClientItemsRequesting(sender, eventArgs) {
                                                var context = eventArgs.get_context();
                                                context["prefixText"] = eventArgs.get_text();

                                                //Don't send the request until the user types in 2 or more characters   
                                                if (eventArgs.get_text().length <= 1) {
                                                    eventArgs.set_cancel(true);
                                                }
                                            }   
                                         </script>  
                                    </telerik:RadScriptBlock>
                                
                            
                              <div style="float:left;">
                               <telerik:RadComboBox ID="KeywordsTextBox" Runat="server"   
                                    EnableLoadOnDemand="true" MarkFirstMatch="true" AllowCustomText="true" 
                                    OnClientItemsRequesting="OnClientItemsRequesting" Width="327px" EnableEmbeddedSkins="false" >
                                    <WebServiceSettings Path="../Services/AutoComplete.asmx" Method="GetRadKeywordsCompletionList"/> </telerik:RadComboBox>
                                </div>

                                <div>
                                <asp:ImageButton ID="AddKeywordButton" runat="server" CausesValidation="false" ImageUrl="~/Images/add_btn.png"  OnClick="AddKeywordButton_Click" />
                                </div>
                              <br />
                               <div style="float:left;">
                               <asp:ListBox ID="KeywordsListBox" runat="server" 
                                        SelectionMode="Multiple" CssClass="TextBox" SkinID="ListBox"></asp:ListBox> 
                               </div>
                                
                              <div>
                                  <asp:ImageButton ID="RemoveKeywordsButton" runat="server" CausesValidation="false" ImageUrl="~/Images/remove_btn.png" OnClick="RemoveKeywordsButton_Click" /> 
                             </div>
                                  
                                  
                                  
                                      
                               
                            </td>
                        </tr>

                 </table>
                </asp:Panel>
                        <br />     
              <ajax:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="CollapsiblePanel"
                    CollapsedSize="0"  Collapsed="True" AutoCollapse="False" AutoExpand="False"
                    CollapsedImage="~/Images/Collapse_Down_BTN.png" ExpandedImage="~/Images/Collapse_Up_BTN.png" ExpandDirection="Vertical"
                    CollapseControlID="HeaderPanel" ExpandControlID="HeaderPanel" ImageControlID="ExpandCollapseImage" CollapsedText="Click to expand." ExpandedText="Click to collapse." />

                     <asp:Button ID="Step1NextButton" runat="server" OnClick="Step1NextButton_Click" Text="Next &gt;" />
                            &nbsp;<asp:Button ID="Step1CancelButton" runat="server" CausesValidation="False"
                                OnClick="CancelButton_Click" Text="Cancel" ToolTip="Cancel" />

                 
            </asp:View>

            <asp:View runat="server" ID="ValidationView">
                <div class="ListTitle">
                    Validate Model
                </div>
                <table>
                    <tr>
                        <td colspan="2">
                            <div id="flashContent">
                                <%--<p>
	        	To view this page ensure that Adobe Flash Player version 
				10.0.0 or greater is installed. 
			</p>
			 <script type="text/javascript">
			    var pageHost = ((document.location.protocol == "https:") ? "https://" : "http://");
			    document.write("<a href='http://www.adobe.com/go/getflashplayer'><img src='"
								+ pageHost + "www.adobe.com/images/shared/download_buttons/get_flash_player.gif' alt='Get Adobe Flash player' /></a>"); 
			</script>
                                --%>
                            </div>
                            <noscript>
                                <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" width="100%" height="100%"
                                    id="test3d">
                                    <param name="movie" value="test3d.swf" />
                                    <param name="quality" value="high" />
                                    <param name="bgcolor" value="#ffffff" />
                                    <param name="allowScriptAccess" value="sameDomain" />
                                    <param name="allowFullScreen" value="true" />
                                    <!--[if !IE]>-->
                                    <object type="application/x-shockwave-flash" data="\Public\Away3D\test3d.swf" width="100%"
                                        height="100%">
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
                    </tr>
                    <tr>
                        <td>
                            <table cellpadding="4" cellspacing="0" border="0">
                                <tr>
                                    <td align="left" valign="top">
                                        <asp:Label ID="Label14" runat="server" AssociatedControlID="UnitScaleTextBox" Text="Unit Scale:"
                                            CssClass="Bold" ToolTip="Unit Scale"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="UnitScaleTextBox" runat="server" 
                                            OnTextChanged="UnitScaleTextBox_TextChanged" CssClass="TextBox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
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
                                    <td align="left" valign="top">
                                        <asp:Label ID="Label16" runat="server" Text="Number of Polygons:" AssociatedControlID="NumPolygonsTextBox"
                                            CssClass="Bold" ToolTip="Number of Polygons"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="NumPolygonsTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <asp:Label ID="Label17" runat="server" Text="Number of Textures:" AssociatedControlID="NumTexturesTextBox"
                                            CssClass="Bold" ToolTip="Number of Textures"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="NumTexturesTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <asp:Label ID="Label18" runat="server" Text="Textures UV Coordinate Channel:" AssociatedControlID="UVCoordinateChannelTextBox"
                                            CssClass="Bold" ToolTip="Textures UV Coordinate Channel"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="UVCoordinateChannelTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <asp:Label ID="Label13" runat="server" Text="Intention of Texture:" AssociatedControlID="IntentionofTextureTextBox"
                                            CssClass="Bold" ToolTip="Intention of Texture"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="IntentionofTextureTextBox" runat="server" Rows="5" 
                                            TextMode="MultiLine" CssClass="TextBox"></asp:TextBox>
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
                                <%--<p>
	        	To view this page ensure that Adobe Flash Player version 
				10.0.0 or greater is installed. 
			</p>
			 <script type="text/javascript">
			    var pageHost = ((document.location.protocol == "https:") ? "https://" : "http://");
			    document.write("<a href='http://www.adobe.com/go/getflashplayer'><img src='"
								+ pageHost + "www.adobe.com/images/shared/download_buttons/get_flash_player.gif' alt='Get Adobe Flash player' /></a>"); 
			</script>
                                --%>
                            </div>
                            <noscript>
                                <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" width="100%" height="100%"
                                    id="Object1">
                                    <param name="movie" value="test3d.swf" />
                                    <param name="quality" value="high" />
                                    <param name="bgcolor" value="#ffffff" />
                                    <param name="allowScriptAccess" value="sameDomain" />
                                    <param name="allowFullScreen" value="true" />
                                    <!--[if !IE]>-->
                                    <object type="application/x-shockwave-flash" data="\Public\Away3D\test3d.swf" width="100%"
                                        height="100%">
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
