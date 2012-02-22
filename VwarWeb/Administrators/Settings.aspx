<%@ Page Language="C#" MasterPageFile="~/Administrator.master" AutoEventWireup="true" CodeFile="Settings.aspx.cs" Inherits="Administrators_Settings" %>
<%@ Register src="../Controls/RebuildThumbnailCache.ascx" tagname="RebuildThumbnailCache" tagprefix="uc1" %>

<asp:Content ID="Content1" runat="server" 
    contentplaceholderid="AdminContentPlaceHolder">
    <script type="text/javascript" src="../Scripts/jquery-ui-1.8.7.custom.min.js"></script>
        <script type="text/javascript" src="../Scripts/jquery.cookie.js"></script>
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
           
            $("#ctl00_ctl00_ContentPlaceHolder1_AdminContentPlaceHolder_SaveMySQLSettings").attr("disabled", true);
            $("#ctl00_ctl00_ContentPlaceHolder1_AdminContentPlaceHolder_SaveFedoraSettings").attr("disabled", true);

            $("#ctl00_ctl00_ContentPlaceHolder1_AdminContentPlaceHolder_testMySQLStatus").html("");
            $("#ctl00_ctl00_ContentPlaceHolder1_AdminContentPlaceHolder_TestFedoraStatus").html("");
            $("#ctl00_ctl00_ContentPlaceHolder1_AdminContentPlaceHolder_SaveAdvancedSettingsstatus").html("");
            $("#ctl00_ctl00_ContentPlaceHolder1_AdminContentPlaceHolder_TestEmailSettingsStatus").html("");
            $("#ctl00_ctl00_ContentPlaceHolder1_AdminContentPlaceHolder_testSiteInfoStatus").html("");
        }

        $(document).ready(function () {
            $("#tabs").tabs({ cookie: { expires: 30} });
            

        });
    </script>

    <div style="width:800px">
    <div>Here, you can configure the behavior and appearance of this 3DR installation. Settings will show an exclamation mark when they have been modified but not saved. In some cases, the save buttons will be disabled until a test is performed. In this case, the save option will only activate after a successful test. Some settings have their own pages. Visit <a href="Email Text.aspx">"Email Text"</a> to modify the text of emails sent by the site. Visit <a href="LRIntegration.aspx">"Learning Registry"</a> to configure the connection to the Federal Learning Registry.</div><br />
        <div id="tabs">
	        <ul>
		        <li><a href="#tabs-BasicSettings">Basic Settings</a></li>
                <li><a href="#tabs-Connections">Connections</a></li>
		        <li><a href="#tabs-AdvancedSettings">Advanced Settings</a></li>
	        </ul>
	        <div id="tabs-Connections">
	


                <table style="width:750px">
                     <tr class="thead"><td class="thead">SQL Settings</td><td class="thead"> </td> </tr>   
                     <tr><td style="width:20%"> MySQL IP Address</td><td><asp:TextBox style="width:95%" onkeydown="javascript: Changed( 'MySQLIPstatus' );" ID="MySQLIP" runat="server"></asp:TextBox><img runat="server" id="MySQLIPstatus" class="MySQLIPstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/><br /><div style="font-size:x-small; color:Gray">The IP address of the MySQL server the 3DR will use to store data.</div></td></tr>
                     <tr><td style="width:20%"> MySQL Port Number</td><td><asp:TextBox style="width:95%" onkeydown="javascript: Changed( 'MySQLPortstatus' );" ID="MySQLPort" runat="server"></asp:TextBox><img runat="server" id="MySQLPortstatus" class="MySQLPortstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
                     <tr><td style="width:20%"> MySQL UserName</td><td><asp:TextBox style="width:95%" onkeydown="javascript: Changed( 'MySQLUserNamestatus' );" ID="MySQLUserName" runat="server"></asp:TextBox><img runat="server" id="MySQLUserNamestatus"  class="MySQLUserNamestatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
                     <tr><td style="width:20%"> MySQL Password</td><td><opp:PasswordTextBox TextMode="Password" style="width:95%" onkeydown="javascript: Changed( 'MySQLPassword1status' );" ID="MySQLPassword1" runat="server"></opp:PasswordTextBox><img runat="server" class="MySQLPassword1status" id="MySQLPassword1status" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
                     <tr><td style="width:20%"> Confirm Password</td><td><opp:PasswordTextBox TextMode="Password" style="width:95%" onkeydown="javascript: Changed( 'MySQLPassword2status' );" ID="MySQLPassword2" runat="server"></opp:PasswordTextBox><img runat="server" class="MySQLPassword2status" id="MySQLPassword2status" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
                </table>
                <asp:Button ID="SaveMySQLSettings" Text="Save" runat="server" 
                    onclick="SaveMySQLSettings_Click"/>
                <asp:Button ID="TestMySQLSettings" Text="Test" runat="server" 
                    onclick="TestMySQLSettings_Click"/>
                <asp:Label ID="testMySQLStatus" Text="" runat="server"></asp:Label>

                <table style="width:750px">
                     <tr class="thead"><td class="thead">Fedora Settings</td><td class="thead"> </td> </tr>   
                     <tr><td style="width:20%"> Fedora URL</td><td><asp:TextBox style="width:95%" ID="FedoraURL" onkeydown="javascript: Changed( 'FedoraURLstatus' );" runat="server"></asp:TextBox><img runat="server" class="FedoraURLstatus" id="FedoraURLstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/><br /><div style="font-size:x-small; color:Gray">The URL for the fedora service. ie. "http://localhost:8080/fedora". </div></td></tr> 
                     <tr><td style="width:20%"> Fedora UserName</td><td><asp:TextBox style="width:95%" ID="FedoraUserName" onkeydown="javascript: Changed( 'FedoraUserNamestatus' );" runat="server"></asp:TextBox><img runat="server" class="FedoraUserNamestatus" id="FedoraUserNamestatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
                     <tr><td style="width:20%"> Fedora Namespace</td><td><asp:TextBox style="width:95%" ID="FedoraNameSpace" onkeydown="javascript: Changed( 'FedoraNameSpacestatus' );"  runat="server"></asp:TextBox><img runat="server" class="FedoraNameSpacestatus" id="FedoraNameSpacestatus" alt="check" src="../styles/images/Icons/checkmark.gif"/><br /><div style="font-size:x-small; color:Gray">The namespace for this server. ***WARNING*** Look for the "Change Namespace" tool in the Admin tools. </div></td></tr>
                     <tr><td style="width:20%"> Fedora Password</td><td><opp:PasswordTextBox TextMode="Password" style="width:95%" ID="FedoraPassword1" onkeydown="javascript: Changed( 'FedoraPassword1status' );" runat="server"></opp:PasswordTextBox><img runat="server" class="FedoraPassword1status" id="FedoraPassword1status" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
                     <tr><td style="width:20%"> Confirm Password</td><td><opp:PasswordTextBox TextMode="Password" style="width:95%" ID="FedoraPassword2" onkeydown="javascript: Changed( 'FedoraPassword2status' );" runat="server"></opp:PasswordTextBox><img runat="server" class="FedoraPassword2status" id="FedoraPassword2status" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
                </table>
                <asp:Button ID="SaveFedoraSettings" Text="Save" runat="server" 
                    onclick="SaveFedoraSettings_Click"/>
                <asp:Button ID="TestFedoraSettings" Text="Test" runat="server" 
                    onclick="TestFedoraSettings_Click"/>
                 <asp:Label ID="TestFedoraStatus" Text="" runat="server"></asp:Label>
            </div>
            <div id="tabs-BasicSettings">
                <table style="width:750px">
                     <tr class="thead"><td class="thead">Outgoing Email Settings</td><td class="thead"> </td> </tr>   
                     <tr><td style="width:20%"> Email Enabled</td><td><asp:DropDownList style="width:95%" ID="EmailEnabled" onchange="javascript: Changed( 'EmailEnabledstatus' );" runat="server">
                         <asp:ListItem>true</asp:ListItem>
                         <asp:ListItem>false</asp:ListItem>
                         </asp:DropDownList><img runat="server" class="EmailEnabledstatus" id="EmailEnabledstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/><br /><div style="font-size:x-small; color:Gray">Enable or disable email globally.</div></td></tr>
                     <tr><td style="width:20%"> SMTP Server</td><td><asp:TextBox style="width:95%" ID="SMTPServer" onkeydown="javascript: Changed( 'SMTPServerstatus' );" runat="server"></asp:TextBox><img runat="server" class="SMTPServerstatus" id="SMTPServerstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/><br /><div style="font-size:x-small; color:Gray">The URL of the outgoing mail server.</div></td></tr>
                     <tr><td style="width:20%"> SMTP Username</td><td><asp:TextBox style="width:95%" ID="SMTPUserName" onkeydown="javascript: Changed( 'SMTPUserNamestatus' );" runat="server"></asp:TextBox><img runat="server" class="SMTPUserNamestatus" id="SMTPUserNamestatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
                     <tr><td style="width:20%"> SMTP Password</td><td><opp:PasswordTextBox TextMode="Password" style="width:95%" onkeydown="javascript: Changed( 'SMTPPasswordstatus' );" ID="SMTPPassword" runat="server"></opp:PasswordTextBox><img runat="server" class="SMTPPasswordstatus" id="SMTPPasswordstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
                     <tr><td style="width:20%"> Confirm Password</td><td><opp:PasswordTextBox TextMode="Password" style="width:95%" onkeydown="javascript: Changed( 'SMTPPassword1status' );" ID="SMTPPassword1" runat="server"></opp:PasswordTextBox><img runat="server" class="SMTPPassword1status" id="SMTPPassword1status" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
                </table>
                <asp:Button ID="SaveEmailSettings" Text="Save" runat="server" 
                    onclick="SaveEmailSettings_Click"/>
                <asp:Button ID="TestEmailSettings" Text="Test" runat="server" 
                    onclick="TestEmailSettings_Click"/>
                 <asp:Label ID="TestEmailSettingsStatus" Text="" runat="server"></asp:Label>
                <table style="width:750px">
                     <tr class="thead"><td class="thead">Site Information</td><td class="thead"> </td> </tr>   
                     <tr><td style="width:20%"> Site Name</td><td><asp:TextBox style="width:95%" ID="SiteName" onkeydown="javascript: Changed( 'SiteNamestatus' );" runat="server"></asp:TextBox><img runat="server" class="SiteNamestatus" id="SiteNamestatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
                     <tr><td style="width:20%"> Company Name</td><td><asp:TextBox style="width:95%" ID="CompanyName" onkeydown="javascript: Changed( 'CompanyNamestatus' );" runat="server"></asp:TextBox><img runat="server" class="CompanyNamestatus" id="CompanyNamestatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
                     <tr><td style="width:20%"> Company Email</td><td><asp:TextBox style="width:95%" ID="CompanyEmail" onkeydown="javascript: Changed( 'CompanyEmailstatus' );" runat="server"></asp:TextBox><img runat="server" class="CompanyEmailstatus" id="CompanyEmailstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
                     <tr><td style="width:20%"> Support Email</td><td><asp:TextBox  style="width:95%" ID="SupportEmail" onkeydown="javascript: Changed( 'SupportEmailstatus' );" runat="server"></asp:TextBox><img runat="server" class="SupportEmailstatus" id="SupportEmailstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
                     <tr><td style="width:20%"> Company Address</td><td><asp:TextBox  style="width:95%" ID="CompanyAddress" onkeydown="javascript: Changed( 'CompanyAddressstatus' );" runat="server"></asp:TextBox><img runat="server" class="CompanyAddressstatus" id="CompanyAddressstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
                     <tr><td style="width:20%"> Company Phone</td><td><asp:TextBox  style="width:95%" ID="CompanyPhone" onkeydown="javascript: Changed( 'CompanyPhonestatus' );" runat="server"></asp:TextBox><img runat="server" class="CompanyPhonestatus" id="CompanyPhonestatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
                     <tr><td style="width:20%"> Company Fax</td><td><asp:TextBox  style="width:95%" ID="CompanyFax" onkeydown="javascript: Changed( 'CompanyFaxstatus' );" runat="server"></asp:TextBox><img runat="server" class="CompanyFaxstatus" id="CompanyFaxstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
                     <tr><td style="width:20%"> Map URL</td><td><asp:TextBox  style="width:95%" ID="ContactUsViewMapUrl" onkeydown="javascript: Changed( 'ContactUsViewMapUrlstatus' );" runat="server"></asp:TextBox><img runat="server" class="ContactUsViewMapUrlstatus" id="ContactUsViewMapUrlstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
                </table>
                <asp:Button ID="SaveSiteInformation" Text="Save" runat="server" 
                    onclick="SaveSiteInformation_Click"/>
                    <asp:Label ID="testSiteInfoStatus" Text="" runat="server"></asp:Label>
            	        </div>
	        <div id="tabs-AdvancedSettings">
	
                 <table style="width:750px">
                     <tr class="thead"><td class="thead">Site Behavior</td><td class="thead"> </td> </tr>   
                     <tr><td style="width:20%">Approve Users by Default</td><td><asp:DropDownList style="width:95%" ID="ApproveUsersDefault" onchange="javascript: Changed( 'ApproveUsersDefaultstatus' );" runat="server">
                         <asp:ListItem>true</asp:ListItem>
                         <asp:ListItem>false</asp:ListItem>
                         </asp:DropDownList><img runat="server" id="ApproveUsersDefaultstatus" class="ApproveUsersDefaultstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/><br /><div style="font-size:x-small; color:Gray">When set to true, the site will not require an administrator to approve new user registrations.</div></td></tr> 
                         <tr><td style="width:20%">Google Analytics Account ID</td><td><asp:TextBox style="width:95%" onkeydown="javascript: Changed( 'GoogleAnalyticsAccountIDstatus' );" ID="GoogleAnalyticsAccountID" runat="server"></asp:TextBox><img runat="server" id="GoogleAnalyticsAccountIDstatus" class="GoogleAnalyticsAccountIDstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/><br /><div style="font-size:x-small; color:Gray">Use this value to link the 3DR to your Google Analytics account.</div></td></tr>
                         <tr><td style="width:20%">Conversion Library Location</td><td><asp:TextBox style="width:95%" onkeydown="javascript: Changed( 'ConversionLibraryLocationstatus' );" ID="ConversionLibraryLocation" runat="server"></asp:TextBox><img runat="server" id="ConversionLibraryLocationstatus" class="ConversionLibraryLocationstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/><br /><div style="font-size:x-small; color:Gray">The path to the conversion library DLL's on the server.</div></td></tr>
                 </table>
                 <table style="width:750px">
                     <tr class="thead"><td class="thead">Site Appearance</td><td class="thead"> </td> </tr>   
                     <tr><td style="width:20%">Header Graphic Path</td><td><asp:TextBox style="width:95%" onkeydown="javascript: Changed( 'HeaderGraphicPathstatus' );" ID="HeaderGraphicPath" runat="server"></asp:TextBox><img runat="server" id="HeaderGraphicPathstatus" class="HeaderGraphicPathstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/><br /><div style="font-size:x-small; color:Gray">The path on the server of the image to display in the site header.</div></td></tr>
                     <tr><td style="width:20%">Footer Control Path</td><td><asp:TextBox style="width:95%" onkeydown="javascript: Changed( 'FooterControlPathstatus' );" ID="FooterControlPath" runat="server"></asp:TextBox><img runat="server" id="FooterControlPathstatus" class="FooterControlPathstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/><br /><div style="font-size:x-small; color:Gray">The path on the server of the ASP.net control to display as the site footer. This should be a .ascx file.</div></td></tr>
                     <tr><td style="width:20%">About Page URL</td><td><asp:TextBox style="width:95%" onkeydown="javascript: Changed( 'AboutPageURLstatus' );" ID="AboutPageURL" runat="server"></asp:TextBox><img runat="server" id="AboutPageURLstatus" class="AboutPageURLstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/><br /><div style="font-size:x-small; color:Gray">The path on the server of the ASP.net page to display when the user clicks the "About" tab. This should be a .aspx file.</div></td></tr>
                 </table>
                <asp:Button ID="SaveAdvancedSettings" Text="Save" runat="server" 
                     onclick="SaveAdvancedSettings_Click" />
                <asp:Label ID="SaveAdvancedSettingsstatus" Text="" runat="server"></asp:Label>
	        </div>
        </div> 
    </div>   
    
</asp:Content>