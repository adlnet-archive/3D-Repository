<%@ Page Language="C#" MasterPageFile="~/Administrator.master" AutoEventWireup="true" CodeFile="Settings.aspx.cs" Inherits="Administrators_Settings" %>
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
    </style>
    <script type="text/javascript">
        function Changed(id) {
           
            document.getElementById(id).src = "../styles/images/Icons/warning.gif";
        }
    
    </script>
    <div style="width:750px">
        <table style="width:750px">
             <tr class="thead"><td class="thead">SQL Settings</td><td class="thead"> </td> </tr>   
             <tr><td style="width:20%"> MySQL IP Address</td><td><asp:TextBox style="width:95%" onkeydown="javascript: Changed( 'MySQLIPstatus' );" ID="MySQLIP" runat="server"></asp:TextBox><img id="MySQLIPstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> MySQL Port Number</td><td><asp:TextBox style="width:95%" onkeydown="javascript: Changed( 'MySQLPortstatus' );" ID="MySQLPort" runat="server"></asp:TextBox><img id="MySQLPortstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> MySQL UserName</td><td><asp:TextBox style="width:95%" onkeydown="javascript: Changed( 'MySQLUserNamestatus' );" ID="MySQLUserName" runat="server"></asp:TextBox><img id="MySQLUserNamestatus"alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> MySQL Password</td><td><opp:PasswordTextBox TextMode="Password" style="width:95%" onkeydown="javascript: Changed( 'MySQLPassword1status' );" ID="MySQLPassword1" runat="server"></opp:PasswordTextBox><img  id="MySQLPassword1status" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> Confirm Password</td><td><opp:PasswordTextBox TextMode="Password" style="width:95%" onkeydown="javascript: Changed( 'MySQLPassword2status' );" ID="MySQLPassword2" runat="server"></opp:PasswordTextBox><img id="MySQLPassword2status" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
        </table>
        <asp:Button ID="SaveMySQLSettings" Text="Save" runat="server" 
            onclick="SaveMySQLSettings_Click"/>
        <asp:Button ID="TestMySQLSettings" Text="Test" runat="server" 
            onclick="TestMySQLSettings_Click"/>
        <asp:Label ID="testMySQLStatus" Text="" runat="server"></asp:Label>

        <table style="width:750px">
             <tr class="thead"><td class="thead">Fedora Settings</td><td class="thead"> </td> </tr>   
             <tr><td style="width:20%"> Fedora URL</td><td><asp:TextBox style="width:95%" ID="FedoraURL" onkeydown="javascript: Changed( 'FedoraURLstatus' );" runat="server"></asp:TextBox><img ID="FedoraURLstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr> 
             <tr><td style="width:20%"> Fedora UserName</td><td><asp:TextBox style="width:95%" ID="FedoraUserName" onkeydown="javascript: Changed( 'FedoraUserNamestatus' );" runat="server"></asp:TextBox><img ID="FedoraUserNamestatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> Fedora Namespace</td><td><asp:TextBox style="width:95%" ID="FedoraNameSpace" onkeydown="javascript: Changed( 'FedoraNameSpacestatus' );"  runat="server"></asp:TextBox><img ID="FedoraNameSpacestatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> Fedora Password</td><td><opp:PasswordTextBox TextMode="Password" style="width:95%" ID="FedoraPassword1" onkeydown="javascript: Changed( 'FedoraPassword1status' );" runat="server"></opp:PasswordTextBox><img ID="FedoraPassword1status" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> Confirm Password</td><td><opp:PasswordTextBox TextMode="Password" style="width:95%" ID="FedoraPassword2" onkeydown="javascript: Changed( 'FedoraPassword2status' );" runat="server"></opp:PasswordTextBox><img ID="FedoraPassword2status" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
        </table>
        <asp:Button ID="SaveFedoraSettings" Text="Save" runat="server" 
            onclick="SaveFedoraSettings_Click"/>
        <asp:Button ID="TestFedoraSettings" Text="Test" runat="server" 
            onclick="TestFedoraSettings_Click"/>
         <asp:Label ID="TestFedoraStatus" Text="" runat="server"></asp:Label>

        <table style="width:750px">
             <tr class="thead"><td class="thead">Outgoing Email Settings</td><td class="thead"> </td> </tr>   
             <tr><td style="width:20%"> SMTP Server</td><td><asp:TextBox style="width:95%" ID="SMTPServer" onkeydown="javascript: Changed( 'SMTPServerstatus' );" runat="server"></asp:TextBox><img ID="SMTPServerstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> SMTP Username</td><td><asp:TextBox style="width:95%" ID="SMTPUserName" onkeydown="javascript: Changed( 'SMTPUserNamestatus' );" runat="server"></asp:TextBox><img ID="SMTPUserNamestatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> SMTP Password</td><td><opp:PasswordTextBox TextMode="Password" style="width:95%" onkeydown="javascript: Changed( 'SMTPPasswordstatus' );" ID="SMTPPassword" runat="server"></opp:PasswordTextBox><img ID="SMTPPasswordstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> Confirm Password</td><td><opp:PasswordTextBox TextMode="Password" style="width:95%" onkeydown="javascript: Changed( 'SMTPPassword1status' );" ID="SMTPPassword1" runat="server"></opp:PasswordTextBox><img ID="SMTPPassword1status" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
        </table>
        <asp:Button ID="SaveEmailSettings" Text="Save" runat="server" 
            onclick="SaveEmailSettings_Click"/>
        <asp:Button ID="TestEmailSettings" Text="Test" runat="server"/>
         <asp:Label ID="TestEmailSettingsStatus" Text="" runat="server"></asp:Label>
        <table style="width:750px">
             <tr class="thead"><td class="thead">Site Information</td><td class="thead"> </td> </tr>   
             <tr><td style="width:20%"> Site Name</td><td><asp:TextBox style="width:95%" ID="SiteName" onkeydown="javascript: Changed( 'SiteNamestatus' );" runat="server"></asp:TextBox><img ID="SiteNamestatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> Company Name</td><td><asp:TextBox style="width:95%" ID="CompanyName" onkeydown="javascript: Changed( 'CompanyNamestatus' );" runat="server"></asp:TextBox><img ID="CompanyNamestatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> Company Email</td><td><asp:TextBox style="width:95%" ID="CompanyEmail" onkeydown="javascript: Changed( 'CompanyEmailstatus' );" runat="server"></asp:TextBox><img ID="CompanyEmailstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> Support Email</td><td><asp:TextBox  style="width:95%" ID="SupportEmail" onkeydown="javascript: Changed( 'SupportEmailstatus' );" runat="server"></asp:TextBox><img ID="SupportEmailstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> Company Address</td><td><asp:TextBox  style="width:95%" ID="CompanyAddress" onkeydown="javascript: Changed( 'CompanyAddressstatus' );" runat="server"></asp:TextBox><img ID="CompanyAddressstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> Company Phone</td><td><asp:TextBox  style="width:95%" ID="CompanyPhone" onkeydown="javascript: Changed( 'CompanyPhonestatus' );" runat="server"></asp:TextBox><img ID="CompanyPhonestatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> Company Fax</td><td><asp:TextBox  style="width:95%" ID="CompanyFax" onkeydown="javascript: Changed( 'CompanyFaxstatus' );" runat="server"></asp:TextBox><img ID="CompanyFaxstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
             <tr><td style="width:20%"> Map URL</td><td><asp:TextBox  style="width:95%" ID="ContactUsViewMapUrl" onkeydown="javascript: Changed( 'ContactUsViewMapUrlstatus' );" runat="server"></asp:TextBox><img ID="ContactUsViewMapUrlstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
        </table>
        <asp:Button ID="SaveSiteInformation" Text="Save" runat="server" 
            onclick="SaveSiteInformation_Click"/>
            <asp:Label ID="testSiteInfoStatus" Text="" runat="server"></asp:Label>
    </div>   
    
</asp:Content>