<%@ Page Language="C#" MasterPageFile="~/Administrator.master" AutoEventWireup="true" CodeFile="LRIntegration.aspx.cs" Inherits="Administrators_LRIntegration" %>
<%@ Register src="../Controls/RebuildThumbnailCache.ascx" tagname="RebuildThumbnailCache" tagprefix="uc1" %>

<asp:Content ID="Content1" runat="server" 
    contentplaceholderid="AdminContentPlaceHolder">
    <style type="text/css">
        .thead
        {
            color:White;
            background-color:#507CD1;
            font-weight:bold;
        }
        tr
        {
            width:100px;
        }
        tr:nth-child(even) 
        {
            background-color:#EFF3FB;
            width:100px;
        }
        tr:nth-child(odd) 
        {
            background-color:#FFFFFF;
            width:100px;
        }
        .Thumbtable
        {
            border-right: outset 2px blue;
            border-left: outset 2px blue;
            border-top: outset 2px blue;
            border-bottom: outset 2px blue;
            width:100%;
        }
        .pid
        {
            border-right: outset 2px blue;
            border-left: outset 2px blue;
            border-top: outset 2px blue;
            border-bottom: outset 2px blue;
            width:10%;
            text-align:center;
        }
        .Scrollpane
        {
            max-height : 500px;
            overflow : scroll;
        }
        .savebutton
        {
            
        }
        .EnabledDropdown
        {
            
        }
        input[type=text]
        {
            height:20px;   
        }
        input[type=password]
        {
            height:20px;   
        }
    </style>
    <script type="text/javascript">
        function Changed(id) {

            $("." + id).attr("src", "../styles/images/Icons/warning.gif");
        }
        function ChangedEnabled() {

           

            if($(".EnabledDropdown").attr("value") == "false")
                $(".savebutton").removeAttr("disabled");

            if($(".EnabledDropdown").attr("value") == "true")
                $(".savebutton").attr("disabled", "disabled");
        }
    </script>
    <div style="width:750px">
     <div>These settings configure the connection between this instance of the 3DR and the <a href="http://www.learningregistry.org/">Federal Learning Registry.</a> The Learing Registry is a distributed database of learning metadata and paradata. The 3DR will submit to the Registry metadata about learning assets (in this case, models) available through this site. It will also submit usage data, such as model view counts, download counts, and reviews. The save button below will be disabled until a successful test is performed. Note that the Registry submittions will contain the URL of your API service interface. Click here to manage your <a href="APIControl.aspx"> API service</a>.</div><br />
        <table style="width:750px">
             <tr class="thead"><td class="thead">SQL Settings</td><td class="thead"> </td> </tr>   
             <tr><td style="width:20%"> LR Integration Enabled</td><td>
             <asp:DropDownList style="width:95%" ID="LRIntegrationEnabled" class="EnabledDropdown" onchange="javascript: ChangedEnabled();" runat="server">
                 <asp:ListItem>true</asp:ListItem>
                 <asp:ListItem>false</asp:ListItem>
                 </asp:DropDownList><img class="LRIntegrationEnabledstatus" id="LRIntegrationEnabledstatus" runat="server" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> GNUPG Key ID</td><td><asp:TextBox onkeydown="javascript: Changed( 'GNUPG_Key_IDstatus' );" style="width:95%" ID="GNUPG_Key_ID" runat="server"></asp:TextBox><img class="GNUPG_Key_IDstatus" id="GNUPG_Key_IDstatus" runat="server" alt="check" src="../styles/images/Icons/checkmark.gif"/><br /><div style="font-size:x-small; color:Gray">This is the key ID on the server's local keyring that will be used to sign the Learning Registry documents.</div></td></tr>
             <tr><td style="width:20%"> GNUPG Key Passphrase</td><td><opp:PasswordTextBox onkeydown="javascript: Changed( 'GNUPG_Key_Passphrase1status' );" TextMode="Password" style="width:95%" ID="GNUPG_Key_Passphrase1" runat="server"></opp:PasswordTextBox><img class="GNUPG_Key_Passphrase1status" id="GNUPG_Key_Passphrase1status" runat="server" alt="check" src="../styles/images/Icons/checkmark.gif"/><div style="font-size:x-small; color:Gray">The passphrase for the above key.</div></td></tr>
             <tr><td style="width:20%"> Confirm Passphrase</td><td><opp:PasswordTextBox onkeydown="javascript: Changed( 'GNUPG_Key_Passphrase2status' );" TextMode="Password" style="width:95%" ID="GNUPG_Key_Passphrase2" runat="server"></opp:PasswordTextBox><img class="GNUPG_Key_Passphrase2status" id="GNUPG_Key_Passphrase2status" runat="server" alt="check" src="../styles/images/Icons/checkmark.gif"/><div style="font-size:x-small; color:Gray"></div></td></tr>
             <tr><td style="width:20%"> GNUPG Location</td><td><asp:TextBox onkeydown="javascript: Changed( 'GNUPG_Locationstatus' );" style="width:95%" ID="GNUPG_Location" runat="server"></asp:TextBox><img class="GNUPG_Locationstatus" id="GNUPG_Locationstatus" runat="server" alt="check" src="../styles/images/Icons/checkmark.gif"/><div style="font-size:x-small; color:Gray">The location on the server of the GNUPG executable.</div></td></tr>
             <tr><td style="width:20%"> GNUPG Public Key URL</td><td><asp:TextBox onkeydown="javascript: Changed( 'GNUPG_Public_Key_URLstatus' );"  style="width:95%" ID="GNUPG_Public_Key_URL" runat="server"></asp:TextBox><img class="GNUPG_Public_Key_URLstatus" id="GNUPG_Public_Key_URLstatus" runat="server" alt="check" src="../styles/images/Icons/checkmark.gif"/><div style="font-size:x-small; color:Gray">The url of the public key for the above Key ID.</div></td></tr>
             <tr><td style="width:20%"> Submitter Name</td><td><asp:TextBox onkeydown="javascript: Changed( 'Submitter_Namestatus' );"  style="width:95%" ID="Submitter_Name" runat="server"></asp:TextBox><img class="Submitter_Namestatus" id="Submitter_Namestatus" runat="server" alt="check" src="../styles/images/Icons/checkmark.gif"/><div style="font-size:x-small; color:Gray">The name to use for the submitter field of the Resource Data Description Document.</div></td></tr>
             <tr><td style="width:20%"> Signer Name</td><td><asp:TextBox onkeydown="javascript: Changed( 'Signer_Namestatus' );"  style="width:95%" ID="Signer_Name" runat="server"></asp:TextBox><img class="Signer_Namestatus" id="Signer_Namestatus" runat="server" alt="check" src="../styles/images/Icons/checkmark.gif"/><div style="font-size:x-small; color:Gray">The name to use for the signer of the document.</div></td></tr>
             <tr><td style="width:20%"> API Base URL</td><td><asp:TextBox onkeydown="javascript: Changed( 'APIBaseURLstatus' );"  style="width:95%" ID="APIBaseURL" runat="server"></asp:TextBox><img class="APIBaseURLstatus" id="APIBaseURLstatus" runat="server" alt="check" src="../styles/images/Icons/checkmark.gif"/><div style="font-size:x-small; color:Gray">The URL for this servers 3DR API.</div></td></tr>
             <tr><td style="width:20%"> LR Publish URL</td><td><asp:TextBox onkeydown="javascript: Changed( 'PublishURLstatus' );"  style="width:95%" ID="PublishURL" runat="server"></asp:TextBox><img class="PublishURLstatus" id="PublishURLstatus" runat="server" alt="check" src="../styles/images/Icons/checkmark.gif"/><div style="font-size:x-small; color:Gray">The URL of the LR node to publish to.</div></td></tr>
             <tr><td style="width:20%"> LR Node Username</td><td><asp:TextBox onkeydown="javascript: Changed( 'LRNodeUsernamestatus' );" style="width:95%" ID="LRNodeUsername" runat="server"></asp:TextBox><img class="LRNodeUsernamestatus" id="LRNodeUsernamestatus" runat="server" alt="check" src="../styles/images/Icons/checkmark.gif"/><div style="font-size:x-small; color:Gray">The username for the LR node.</div></td></tr>
             <tr><td style="width:20%"> LR Node Password</td><td><opp:PasswordTextBox onkeydown="javascript: Changed( 'LRNodePassword1status' );"  TextMode="Password" style="width:95%" ID="LRNodePassword1" runat="server"></opp:PasswordTextBox><img class="LRNodePassword1status" id="LRNodePassword1status" runat="server" alt="check" src="../styles/images/Icons/checkmark.gif"/><div style="font-size:x-small; color:Gray">The password for the LR node.</div></td></tr>
             <tr><td style="width:20%"> Confirm Password</td><td><opp:PasswordTextBox onkeydown="javascript: Changed( 'LRNodePassword2status' );" TextMode="Password" style="width:95%" ID="LRNodePassword2" runat="server"></opp:PasswordTextBox><img class="LRNodePassword2status" id="LRNodePassword2status" runat="server" alt="check" src="../styles/images/Icons/checkmark.gif"/><div style="font-size:x-small; color:Gray"></div></td></tr>
        </table>
        <asp:Button class="savebutton" ID="SaveLRSettings" Text="Save" runat="server" 
            onclick="SaveLRSettings_Click"/>
        <asp:Button ID="TestSettings" Text="Test" runat="server" 
            onclick="TestSettings_Click"/>
        <asp:Label ID="LRStatus" Text="" runat="server"></asp:Label>
    </div>   
    
</asp:Content>