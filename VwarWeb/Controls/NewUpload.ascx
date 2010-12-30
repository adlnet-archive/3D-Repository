<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewUpload.ascx.cs" Inherits="Controls_NewUpload" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="VwarWeb" TagName="Viewer3D" Src="~/Controls/Viewer3D.ascx" %>


<div style="width: 900px; margin: 0 auto">


        <h1>
            Upload a 3D Model</h1>
        <telerik:RadPanelBar ID="UploadControl" ExpandMode="SingleExpandedItem" Width="900"
            runat="server" Style="position: relative; z-index: 1">
            <Items>
                <telerik:RadPanelItem ID="UploadPanel" Text="1. Upload" Expanded="true" runat="server">
                    <ContentTemplate>
                        <div class="UploadControlItem">
                            <div class="PanelLayoutContainer" style="height: 200px">
                                <div class="LRPanelLayout" style="width: 30%;">
                                    <span style="font-size: large; font-weight: bold; text-align: left; margin: 0 0">Choose
                                        Your Format:</span>
                                    <div id="ChooseModelContainer" style="display: inline; width: 50%; height: 45%; position: relative;
                                        top: 20px; z-index: 2">
                                        <button id="modelUploadPlaceholderButton" value="Upload" />
                                    </div>
                                    <div style="margin: 56px 30px 0 30px; text-align: left; display: none;">
                                        <a href="#" onclick="return false;">Learn more</a> about best practices for uploading
                                        a model to 3DR.
                                    </div>
                                </div>
                                <div class="LRPanelLayout" style="text-align: center; padding-top: 5px; left: 30%;
                                    vertical-align: bottom; width: 30%">
                                    <asp:Image ImageUrl="~/Images/thumb-zip-folder.jpg" runat="server" />
                                    <ul style="text-align: left;">
                                        <li>A .zip file containing the following:</li>
                                        <ul>
                                            <li>A <a href="#" onclick="return false;">recognized model format</a></li>
                                            <li>Referenced texture files</li>
                                        </ul>
                                    </ul>
                                </div>
                                <div class="LRPanelLayout" style="text-align: center; padding-top: 100px; font-weight: bold;
                                    font-size: large; left: 60%; width: 10%;">
                                    OR</div>
                                <div class="LRPanelLayout" style="padding-top: 2px; left: 70%; width: 25%;">
                                    <img src="http://upload.wikimedia.org/wikipedia/en/b/bf/Sketchuplogo.png" />
                                    <ul style="text-align: left;">
                                        <li>An .skp (Google SketchUp) file</li>
                                    </ul>
                                </div>
                            </div>
                            <div id="DetailsAndStatusPanel" class="PanelLayoutContainer" style="height: 300px;">
                                <div class="LRPanelLayout" style="width: 30%">
                                    <div id="UploadStatusArea">
                                        <div style="position: absolute; top: 50px; right: 5px">
                                            <a href='#' id="CancelButton" onclick='cancelModelUpload(); return false'>Cancel</a>
                                        </div>
                                        <table style="position: relative; right: 40px; top: 64px;">
                                            <tr id="modelUpload">
                                                <td width="16" valign="top">
                                                    1.
                                                </td>
                                                <td width="150">
                                                    <div id="modelUploadProgress" class="progress">
                                                    </div>
                                                    <div id="modelUploadStatus">
                                                    </div>
                                                </td>
                                                <td width="16" valign="top">
                                                    <img id="modelUploadIcon" />
                                                </td>
                                            </tr>
                                            <tr class="resettable upload">
                                                <td />
                                                <td>
                                                    <div id="modelUploadMessage">
                                                    </div>
                                                </td>
                                                <td />
                                            </tr>
                                            <tr id="formatDetect" style="display: none;" class="resettable upload">
                                                <td width="16">
                                                    2.
                                                </td>
                                                <td width="150">
                                                    <div id="formatDetectStatus">
                                                    </div>
                                                </td>
                                                <td valign="top">
                                                    <img id="formatDetectIcon" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td />
                                                <td>
                                                    <div id="formatDetectMessage" class="resettable upload">
                                                    </div>
                                                </td>
                                                <td />
                                            </tr>
                                            <tr id="conversionStep" style="display: none;" class="resettable upload">
                                                <td width="16" valign="top">
                                                    3.
                                                </td>
                                                <td width="150">
                                                    <div id="conversionStatus">
                                                    </div>
                                                </td>
                                                <td width="16" valign="top">
                                                    <img id="conversionIcon" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td />
                                                <td>
                                                    <div id="conversionMessage" class="resettable upload">
                                                    </div>
                                                </td>
                                                <td />
                                            </tr>
                                            <tr>
                                                <td />
                                                <td>
                                                    <br />
                                                    <br />
                                                    <br />
                                                </td>
                                                <td />
                                            </tr>
                                        </table>
                                    </div>
                                    <a class="NextButton" style="position: relative; top: 30px; left: 85px;" href="#"
                                        onclick="step1_next(); return false;"></a>
                                </div>
                                <div class="LRPanelLayout" style="width: 70%; left: 30%;">
                                    <div style="margin-left: 78px">
                                        <h2 style="margin-bottom: 3px">
                                            While You're Waiting...</h2>
                                        Please fill out the following info:
                                        <br />
                                    </div>
                                    <table style="margin: 0 auto;">
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="TitleLabel" runat="server" Text="Title (Required)" />
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TitleInput" runat="server" Columns="55"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator Text="*" ControlToValidate="TitleInput" Style="color: Red;
                                                    font-weight: bold; font-size: large" runat="server"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" valign="top">
                                                <asp:Label ID="DescriptionLabel" runat="server" Text="Description" />
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="DescriptionInput" runat="server" Style="width: 424px" TextMode="MultiLine"
                                                    Rows="4"></asp:TextBox>
                                            </td>
                                            <td />
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="TagsLabel" runat="server" Text="Tags" />
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TagsInput" runat="server" Columns="55"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <%--This is the privacy settings section. Since we haven't implemented privacy settings, it is purposefully commmented out. %>
<%--                                    <tr>
                                        <td align="right" valign="top">
                                            <asp:Label ID="PrivacySettingsLabel" runat="server" Text="Privacy Settings" />
                                        </td>
                                        <td align="left">
                                            <asp:RadioButtonList ID="PrivacyOptions" runat="server">
                                                <asp:ListItem Text="Public (anyone can search for and view your model)" Value="public"
                                                    Selected="True" />
                                                <asp:ListItem>Private (only users you specify can find your content) <a href="#" onclick="return false;">Add users</a><sup>+</sup></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>--%>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </telerik:RadPanelItem>
                <telerik:RadPanelItem ID="AxisScalePanel" Text="2. Axis, Scale, Thumbnail" Enabled="true"
                    runat="server">
                    <ContentTemplate>
                        <div class="PanelLayoutContainer" id="ViewableView" style="height: 635px">
                            <div class="LRPanelLayout" style="width: 550px; z-index: 1">
                                <VwarWeb:Viewer3D ID="ModelViewer" runat="server" />
                                <a class="NextButton" href="#" onclick="step2_next(); return false;"></a>
                            </div>
                            <div class="LRPanelLayout" style="width: 320px; left: 528px; text-align: left">
                                <div id="ViewerAdjustmentAccordion" style="padding-top: 10px;">
                                    <h3>
                                        <a href='#'>Set Scale</a></h3>
                                    <div>
                                        <div id="ScaleAdjustmentArea" style="position: relative; top: 10px">
                                            <p style="text-align: left; padding-right: 25px;">
                                                Next, we need a little bit of context for your model. Using the slider, adjust the
                                                scale of the model. If necessary, change the unit type.
                                            </p>
                                            <h3>
                                                Unit Scale:
                                            </h3>
                                            <span id="scaleText" style="position: relative; bottom: 5px;"></span>
                                            <div id="scaleSlider" style="width: 200px;">
                                            </div>
                                            <select id="unitType" style="left: 210px; position: relative; top: -17px;">
                                                <option value="0.01">cm</option>
                                                <option value="1" selected="selected">m</option>
                                                <option value="0.0254">in</option>
                                                <option value="0.3048">ft</option>
                                            </select>
                                        </div>
                                    </div>
                                    <h3>
                                        <a href='#'>Set Axis</a></h3>
                                    <div>
                                        <p style="text-align: left; padding-right: 25px;">
                                            Now, please specify the up axis so that your model displays correctly in our viewer:
                                        </p>
                                        <input type="radio" name="UpAxis" value="Y" />Y<br />
                                        <input type="radio" name="UpAxis" value="Z" />Z<br />
                                        <br />
                                    </div>
                                    <h3>
                                        <a href='#'>Set Thumbnail</a></h3>
                                    <div>
                                        <p style="text-align: left; padding-right: 25px; margin-top: -17px">
                                            Now we need a thumbnail of your model so everyone can preview your work. To zoom
                                            in and out, use the scroll wheel on your mouse. When you are ready to take your
                                            snapshot, click the button below:
                                        </p>
                                        <center>
                                            <input type="submit" onclick="TakeUploadSnapshot(); return false;" value="Take Snapshot" /><br />
                                            <h3>
                                                OR</h3>
                                        </center>
                                        <div id="ThumbnailViewableWidget" style="text-align: center;">
                                            <div class="flashContainer" style="margin: 0 auto; width: inherit;">
                                            </div>
                                            <button id="screenshot_viewable_Placeholder">
                                            </button>
                                            <br />
                                            <br />
                                            <div class="progressbarContainer">
                                            </div>
                                            <br />
                                            <span class="statusText"></span>
                                            <img class="statusIcon" /><a href='#' class='cancel' style="display: none;">Cancel</a><br />
                                            <span class="errorMessage"></span>
                                            <h3 style="margin-right: 130px; margin-top: 0px;">
                                                Preview:</h3>
                                            <div id="ThumbnailPreviewContainer" style="width: 200px; min-height: 200px; border: 1px solid black;
                                                margin: 0 auto;">
                                                <img id="ThumbnailPreview_Viewable" class="previewImage" src="../Images/nopreview_icon.png" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="PanelLayoutContainer" id="RecognizedView" style="display: none; height: 440px">
                            <div class="LRPanelLayout" style="width: 50%; left: 25%">
                                <p style="text-align: center;">
                                    Now we need a thumbnail of your model so everyone can see a preview of your work.
                                    Choose a JPEG, PNG, or GIF file:
                                </p>
                                <div id="ThumbnailRecognizedWidget" style="text-align: center">
                                    <div class="flashContainer" style="margin: 0 auto; width: inherit;">
                                    </div>
                                    <button id="screenshot_recognized_Placeholder">
                                    </button>
                                    <br />
                                    <div class="progressbarContainer">
                                    </div>
                                    <span class="statusText"></span>
                                    <img class="statusIcon" /><a href='#' class='cancel' style="display: none;">Cancel</a><br />
                                    <span class="errorMessage"></span>
                                    <h3 style="margin-right: 130px">
                                        Preview:</h3>
                                    <div id="ThumbnailRecognizedPreviewContainer" style="width: 200px; min-height: 200px;
                                        border: 1px solid black; margin: 0 auto;">
                                        <img id="ThumbnailPreview_Recognized" class="previewImage" src="../Images/nopreview_icon.png" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </telerik:RadPanelItem>
                <telerik:RadPanelItem ID="DetailsPanel" Text="3. Add Details" Enabled="true" runat="server">
                    <ContentTemplate>
                        <div class="PanelLayoutContainer" id="Details" style="min-height: 400px; border: none;width: 898px; margin: 0 auto; overflow:hidden;">
                            
                                <div id="DetailsTabs" class="tabs-bottom">
                                    <ul>
                                        <li><a href="#tabs-1">Developer Info</a></li>
                                        <li><a href="#tabs-2">Sponsor Info</a></li>
                                        <li><a href="#tabs-3">License Type</a></li>
                                    </ul>
                                    <div id="tabs-1">
                                        <p style="margin: 0 auto; width: 80%; text-align: center">
                                            You're almost done! Please take the time to fill out additional information about
                                            your model. Let people know where your model came from and who created it. Make
                                            sure to give credit where it's deserved! You can also set the license type so that
                                            no one uses your assets in an illicit way.
                                        </p>
                                        <div class="formLayout">
                                            <label for="DevName">
                                                Developer Name</label>
                                            <input id="DeveloperName" name="DevName" /><br />
                                            <label for="ArtName">
                                                Artist Name</label>
                                            <input id="ArtistName" name="ArtName" /><br />
                                            <label for="DevUrl">
                                                URL</label>
                                            <input id="DeveloperUrl" name="DevUrl" /><br />
                                            <label for="DevLogo">
                                                Developer Logo</label>
                                            <div id="DevLogoUploadWidget" >
                                                <div class="flashContainer" style="margin: 0 auto; width: inherit;">
                                                </div>
                                                <button id="devlogo_Placeholder">
                                                </button>
                                                <div style="display:inline-block; width: 200px">
                                                <div class="progressbarContainer" style="display:inline-block"><a href='#' class='cancel' style="display: none;">Cancel</a><br />
                                                </div><br />
                                                <span class="statusText"></span><img class="statusIcon" />
                                                
                                                </div>
                                                <span class="errorMessage"></span>
                                                <div style="position: absolute; left: 543px; top: 78px">
                                                    <h3 style="margin-right: 130px">
                                                        Preview:</h3>
                                                    <div id="DevLogoPreviewContainer" style="width: 200px; min-height: 200px; border: 1px solid black; margin: 0 auto;">
                                                        <img id="DevLogoImage" class="previewImage" src="../Images/nopreview_icon.png" />
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <div id="tabs-2">
                                         <p style="margin: 0 auto; width: 80%;  text-align: center">
                                            You're almost done! Please take the time to fill out additional information about
                                            your model. Let people know where your model came from and who created it. Make
                                            sure to give credit where it's deserved! You can also set the license type so that
                                            no one uses your assets in an illicit way.
                                        </p>
                                        <div class="formLayout">
                                            <label for="SponsName">Sponsor Name</label>
                                            <input id="SponsorName" name="SponsName" /><br />
                                            <label for="SponsUrl">URL</label>
                                            <input id="SponsorUrl" name="SponsUrl" /><br />
                                            <label>Sponsor Logo</label>
                                             <div id="SponsorLogoUploadWidget" >
                                                <div class="flashContainer" style="margin: 0 auto; width: inherit;">
                                                </div>
                                                <button id="sponsorlogo_Placeholder">
                                                </button>
                                                <div style="display:inline-block; width: 200px">
                                                <div class="progressbarContainer" style="display:inline-block"><a href='#' class='cancel' style="display: none;">Cancel</a><br />
                                                </div><br />
                                                <span class="statusText"></span><img class="statusIcon" />
                                                
                                                </div>
                                                <span class="errorMessage"></span>
                                                <div style="position: absolute; left: 543px; top: 78px">
                                                    <h3 style="margin-right: 130px">
                                                        Preview:</h3>
                                                    <div id="SponsorLogoPreviewContainer" style="width: 200px; min-height: 200px; border: 1px solid black; margin: 0 auto;">
                                                        <img id="SponsorLogoImage" class="previewImage" src="../Images/nopreview_icon.png" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="tabs-3">
                                        <p style="margin: 0 auto; width: 80%;  text-align: center">
                                            You're almost done! Please take the time to fill out additional information about
                                            your model. Let people know where your model came from and who created it. Make
                                            sure to give credit where it's deserved! You can also set the license type so that
                                            no one uses your assets in an illicit way.
                                        </p>
                                        <div class="formLayout" style="width: 666px; margin: 0 auto; height: 240px">
                                            <label for="LicType">License Type</label>
                                            <select id="LicenseType" name="LicType">
                                                <option value=".by" selected="selected">Attribution</option>
                                                <option value=".by-sa">Attribution-ShareAlike</option>
                                                <option value=".by-nd">Attribution-NoDerivatives</option>
                                                <option value=".by-nc">Attribution-NonCommercial</option>
                                                <option value=".by-nc-sa">Attribution-NonCommercial-ShareAlike</option>
                                                <option value=".by-nc-nd">Attribution-NonCommercial-NoDerivatives</option>
                                            </select><br />
                                            <label for="LicDesc">Description</label>
                                            <div id="LicenseDescriptionContainer" name="LicDesc" style="width:347px; display: inline-block; margin-top: -13px">
                                                <p class="cc by license-selected">
                                                    This license lets others distribute, remix, tweak, and build upon your work, even
                                                    commercially, as long as they credit you for the original creation. This is the
                                                    most accommodating of licenses offered. Recommended for maximum dissemination and
                                                    use licensed materials.
                                                </p>
                                                <p class="cc by-sa" style="display: none;">
                                                    This license lets others remix, tweak, and build upon your work even for commercial
                                                    reasons, as long as they credit you and license their new creations under the identical
                                                    terms. This license is often compared to “copyleft” free and open source software
                                                    licenses. All new works based on yours will carry the same license, so any derivatives
                                                    will also allow commercial use. 
                                                </p>
                                                <p class="cc by-nd" style="display: none;">
                                                    This license allows for redistribution, commercial and non-commercial, as long as
                                                    it is passed along unchanged and in whole, with credit to you.
                                                </p>
                                                <p class="cc by-nc" style="display: none;">
                                                    This license lets others remix, tweak, and build upon your work non-commercially,
                                                    and although their new works must also acknowledge you and be non-commercial, they
                                                    don’t have to license their derivative works on the same terms.
                                                </p>
                                                <p class="cc by-nc-sa" style="display: none;">
                                                    This license lets others remix, tweak, and build upon your work non-commercially,
                                                    as long as they credit you and license their new creations under the identical terms.
                                                </p>
                                                <p class="cc by-nc-nd" style="display: none;">
                                                    This license is the most restrictive of our six main licenses, only allowing others
                                                    to download your works and share them with others as long as they credit you, but
                                                    they can’t change them in any way or use them commercially.
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                               
                            </div>
                        </div>
                    </ContentTemplate>
                </telerik:RadPanelItem>
            </Items>
        </telerik:RadPanelBar>
        <a class="NextButton" href="#" onclick="submitUpload(); return false;"></a>
</div>
