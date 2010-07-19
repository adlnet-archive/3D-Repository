<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Upload.aspx.cs" Inherits="Users_Upload" Title="Upload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>



<%@ Register src="../Controls/Upload.ascx" tagname="Upload" tagprefix="uc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

  <br />

<%--
  
    <div style="width: 100%">
        <div style="margin: auto; width: 65%">
            <div class="ListTitle">
                Upload Asset</div>
            <br />
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View runat="server" ID="DefaultView">
                    <div>
                        <asp:Label ID="errorMessage" runat="server" CssClass="LoginFailureTextStyle"></asp:Label>
                    </div>
                    <div style="margin-bottom: 5px;">
                        <asp:Label ID="Label2" CssClass="FormLabel" AssociatedControlID="title" runat="server"
                            ToolTip="Title of the asset">
                Title<span class="Red">*</span>:
                        </asp:Label>
                        <asp:TextBox runat="server" ID="title" Width="400px" MaxLength="100"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="title"
                            ErrorMessage="* Required" CssClass="LoginFailureTextStyle" Style="display: inline;"
                            Display="Dynamic"></asp:RequiredFieldValidator>
                        <ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="title"
                            WatermarkText="ex. M16 Machine Gun" WatermarkCssClass="TextBoxWatermark" />
                    </div>
                    <div style="margin-bottom: 5px; display: none;">
                        <asp:Label ID="Label6" CssClass="FormLabel" AssociatedControlID="collection" runat="server"
                            ToolTip="Collection name">
                Collection<span class="Red">*</span>:        
                        </asp:Label>
                        <asp:DropDownList ID="collection" runat="server">
                            <asp:ListItem Selected="True">Aircraft</asp:ListItem>
                            <asp:ListItem>Armored Vehicles</asp:ListItem>
                            <asp:ListItem>Artillery</asp:ListItem>
                            <asp:ListItem>Guns</asp:ListItem>
                            <asp:ListItem>Unmanned Systems</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div style="margin-bottom: 5px;">
                        <asp:Label CssClass="FormLabel" AssociatedControlID="contentFile" runat="server"
                            ToolTip="*.zip file path">
                *.zip File Upload<sup>1</sup><span class="Red">*</span>:
                        </asp:Label>
                        <asp:FileUpload runat="server" ID="contentFile" Width="50%" />
                        <asp:RegularExpressionValidator ID="zipValidator" ControlToValidate="contentFile"
                            ErrorMessage="File must be in .zip format" runat="server" ValidationExpression=".*zip?"
                            Font-Bold="True" Display="Dynamic"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="ContentFileRequiredFieldValidator" runat="server" ControlToValidate="contentFile"
                            ErrorMessage="* Required" CssClass="LoginFailureTextStyle" Style="display: inline;"
                            Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    <div style="margin-bottom: 5px;">
                        <asp:Label ID="Label1" CssClass="FormLabel" AssociatedControlID="thumbnailFile" runat="server"
                            ToolTip="Thumbnail image of the asset">
                Thumbnail<span class="Red">*</span>:
                        </asp:Label>
                        <asp:FileUpload runat="server" ID="thumbnailFile" Width="50%" />
                        <asp:RequiredFieldValidator ID="ThumbnailFileRequiredFieldValidator" runat="server" ControlToValidate="thumbnailFile"
                            ErrorMessage="* Required" CssClass="LoginFailureTextStyle" Style="display: inline;"
                            Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    <div style="margin-bottom: 5px;">
                        <asp:Label AssociatedControlID="SubmitterLogoImageFilePath" CssClass="FormLabel"
                            runat="server" ToolTip="Logo image to display with asset">
                Submitter Logo:
                        </asp:Label>
                        <asp:FileUpload runat="server" ID="SubmitterLogoImageFilePath" Width="50%" />
                    </div>
                    <div style="margin-bottom: 5px;">
                        <asp:Label ID="Label3" CssClass="FormLabel" AssociatedControlID="description" runat="server"
                            ToolTip="Description of the asset">
                Description<span class="Red">*</span>:
                        </asp:Label>
                        <asp:TextBox runat="server" ID="description" TextMode="MultiLine" Width="400px" Rows="5"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="description"
                            ErrorMessage="* Required" CssClass="LoginFailureTextStyle" Style="display: inline;"
                            Display="Dynamic"></asp:RequiredFieldValidator>
                        <ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="description"
                            WatermarkText="ex. This model contains a fully detailed M16 machine gun used for special operations."
                            WatermarkCssClass="TextBoxWatermark" />
                    </div>
                    <div style="margin-bottom: 5px;">
                        <asp:Label ID="Label4" CssClass="FormLabel" AssociatedControlID="infoLink" runat="server"
                            ToolTip="URL for additional description of the asset">
                More Information URL:
                        </asp:Label>
                        <asp:TextBox runat="server" ID="infoLink" Width="400px" MaxLength="255"></asp:TextBox>
                        <ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="infoLink"
                            WatermarkText="ex. http://www.google.com" WatermarkCssClass="TextBoxWatermark" />
                    </div>
                    <div style="margin-bottom: 5px;">
                        <asp:Label ID="Label5" CssClass="FormLabel" AssociatedControlID="infoLink" runat="server"
                            ToolTip="Tags/keywords to describe the asset">
                Tags<sup>2</sup>:    
                        </asp:Label>
                        <asp:TextBox runat="server" ID="keywords" TextMode="MultiLine" Rows="5" Width="400px"></asp:TextBox>
                        <ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" TargetControlID="keywords"
                            WatermarkText="ex. guns,machine guns" WatermarkCssClass="TextBoxWatermark" />
                    </div>
                    <br />
                    <div style="margin-bottom: 5px;">
                        <asp:Button Text="Submit" runat="server" CssClass="FormLabelButton" ID="btnSubmit"
                            OnClick="btnSubmit_Click" ToolTip="Submit" />
                        <asp:Button Text="Cancel" runat="server" ID="btnCancel" OnClick="btnCancel_Click"
                            CausesValidation="False" ToolTip="Cancel" />
                    </div>
                    <br />
                    <i><sup>1</sup>*.zip File Upload</i> - A zip file containing a *.dae file and any
                    required textures.
                    <br />
                    The textures may be in the zip root folder or in a subfolder.
                    <br />
                    <br />
                    <i><sup>2</sup>Tags</i> - Keywords or phrases that describe your item (separated
                    with commas)
                    <br />
                    <br />
                </asp:View>
                <asp:View runat="server" ID="ConfirmationView">
                    <asp:Label ID="ConfirmationLabel" runat="server"></asp:Label>
                    <br />
                    <br />
                    <asp:Button ID="ContinueButton" runat="server" Text="Continue" ToolTip="Continue"
                        OnClick="ContinueButton_Click" />
                </asp:View>
            </asp:MultiView>
        </div>
    </div>
    --%>

    
    <uc1:Upload ID="Upload1" runat="server" />
    
    
</asp:Content>
