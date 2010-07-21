<%-- Functions you can call on swfDiv:
    
    swfDiv.Load(URL) - Cause the Away3D viewer to load a url
    swfDiv.ShowControls() - Show some buttons
    swfDiv.HideControls() - hide those buttons
    swfDiv.SetScale() - set the scale (from the unit scale gui)
    swfDiv.SetUpVec() - set the up vec( either "Z" or "Y")

--%>


<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Upload.aspx.cs" Inherits="Users_Upload" Title="Upload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>



<%@ Register src="../Controls/Upload.ascx" tagname="Upload" tagprefix="uc1" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

       



        <script type="text/javascript" src="/VwarWeb/Public/Away3D/swfobject.js"></script>
        <script type="text/javascript">
            function attachSWF() {
                // <!-- For version detection, set to min. required Flash Player version, or 0 (or 0.0.0), for no version detection. --> 
                var swfVersionStr = "10.0.0";
                // <!-- To use express install, set to playerProductInstall.swf, otherwise the empty string. -->
                var xiSwfUrlStr = "playerProductInstall.swf";
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
        </script>
		<script type="text/javascript">
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
		        var parms = query.split('&');
		        for (var i = 0; i < parms.length; i++) {
		            var pos = parms[i].indexOf('=');
		            if (pos > 0) {
		                var key = parms[i].substring(0, pos);
		                var val = parms[i].substring(pos + 1);
		                qsParm[key] = val;
		            }
		        }
		        var pos = query.lastIndexOf('URL=');
		        var URL = query.substring(pos + 4, query.length);
		        alert(URL);
		        qsParm['URL'] = URL;
		    }
		    function load() {

		        getID('test3d');

		        if (swfDiv == null)
		            alert("Swfdiv is null");

		        //swfDiv.Load("http://localhost:8080/fedora/objects/adl:3/datastreams/content7/content");
		        
		        swfDiv.Load(gURL);

		    }
		    function DoLoadURL(URL) {
		        gURL = URL;
		        attachSWF();
		        setTimeout('load()', 300);
		    } 
		</script>


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
	   	        <style type="text/css" media="screen">
                        body { margin:0; text-align:center; }
                        div#content { text-align:left; }
                        object#content { display:block; margin:0 auto; }
                </style> 
       	<noscript>
            <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" width="100%" height="100%" id="test3d" >
                <param name="movie" value="test3d.swf" />
                <param name="quality" value="high" />
                <param name="bgcolor" value="#ffffff" />
                <param name="allowScriptAccess" value="sameDomain" />
                <param name="allowFullScreen" value="true" />
                <!--[if !IE]>-->



                <object type="application/x-shockwave-flash" data="\Public\Away3D\test3d.swf" width="100%" height="100%">
                    <param name="quality" value="high" />
                    <param name="bgcolor" value="#ffffff" />
                    <param name="allowScriptAccess" value="sameDomain" />
                    <param name="allowFullScreen" value="true" />
                <!--<![endif]-->
                <!--[if gte IE 6]>-->
                	<p> 
                		Either scripts and active content are not permitted to run or Adobe Flash Player version
                		10.0.0 or greater is not installed.
                	</p>
                <!--<![endif]-->
                    <a href="http://www.adobe.com/go/getflashplayer">
                        <img src="http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif" alt="Get Adobe Flash Player" />
                    </a>
                <!--[if !IE]>-->
                </object>
                <!--<![endif]-->
            </object>
	    </noscript>		

<%-- <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"  codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0" width="100%" height="500">
      <param name="movie" value="/VwarWeb/Public/Away3D/test3d.swf" />
      <param name="quality" value="high" />
      <embed src="/VwarWeb/Public/Away3D/test3d.swf" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" width="100%" height="500" id="test3d"></embed>
    </object>
  <br />
  %>
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
