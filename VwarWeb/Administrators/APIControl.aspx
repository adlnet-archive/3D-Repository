<%@ Page MasterPageFile="~/Administrator.master" Language="C#" AutoEventWireup="true" CodeFile="APIControl.aspx.cs" Inherits="Administrators_APIControl" %>
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
        }

        $(document).ready(function () {
            $("#tabs").tabs({ cookie: { expires: 30} });
            
        });
    </script>

    <div style="width:800px">
    <div>The service API is a seperate project from the 3DR web interface, but commonly the two are installed side by side. You can use these tools to configure and manage the API instance. In almost all cases, the connection data for the API should align with the connection data on the <a href="Settings.aspx">settings page</a>. </div><br />
       
        <div id="tabs">
	        <ul>
		        <li><a href="#tabs-APILocation">API Location</a></li>
                <li><a href="#tabs-APISettings">API Settings</a></li>
		        <li><a href="#tabs-Tools">Tools</a></li>
	        </ul>
	        <div id="tabs-APILocation">
                <table style="width:750px">
                     <tr class="thead"><td class="thead">API Locations</td><td class="thead"> </td> </tr>   
                     <tr><td style="width:20%">API Path</td><td><asp:TextBox style="width:95%" onkeydown="javascript: Changed( 'APIPathstatus' );" ID="APIPath" runat="server"></asp:TextBox><img runat="server" id="APIPathstatus" class="APIPathstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/><br /><div style="font-size:x-small; color:Gray">The physical path on the server of the API. By default {install directory}/3d.service.host/.</div></td></tr>
                     <tr><td style="width:20%">API URL</td><td><asp:TextBox style="width:95%" onkeydown="javascript: Changed( 'APIUrlstatus' );" ID="APIUrl" runat="server"></asp:TextBox><img runat="server" id="APIUrlstatus" class="APIUrlstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/><br /><div style="font-size:x-small; color:Gray">The URL mapped to the API. By Default, http://localhost/API/_3DRAPI.svc </div></td></tr>
                     </table>
                <asp:Button ID="TestAPILocation" Text="Test" runat="server" 
                    onclick="TestAPILocation_Click"/>
                <asp:Button ID="SaveAPILocation" Text="Save" runat="server" 
                    onclick="SaveAPILocation_Click"/>
               
                <asp:Label ID="SaveAPILocationstatus" runat="server"></asp:Label>
            </div>
            <div id="tabs-APISettings">
             <asp:Button ID="AlignWithWebFrontEnd" Text="Copy From Web Front End" runat="server" 
                    onclick="AlignWithWebFrontEnd_Click"/>
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
	        <div id="tabs-Tools">
	            <div style="width:750px">
                 
                 
                         <br />
                         Username: <asp:TextBox runat="server" ID="UserName"></asp:TextBox>
                         Password: <opp:PasswordTextBox runat="server" ID="Password"></opp:PasswordTextBox>
                         <asp:DropDownList style="width:25%" ID="LoginType" runat="server">
                         <asp:ListItem>Anonymous</asp:ListItem>
                         <asp:ListItem>Specify...</asp:ListItem>
                         </asp:DropDownList>
                 <asp:ListBox style="width:100%;height:100px;" ID="SearchResults" runat="server"></asp:ListBox>
                 <asp:Button ID="SearchAPI" Text="Search" OnClick="SearchAPI_Click" runat="server"/>&nbsp<asp:TextBox runat="server" ID="SearchString"></asp:TextBox>
                 <asp:TextBox TextMode="MultiLine" style="width:100%;height:300px;" ID="SelectedMetadata" runat="server"></asp:TextBox>
                 <asp:Button ID="Data" Text="View Data" OnClick="Data_Click" runat="server"/>
                 <asp:Button ID="Download" Text="Download" OnClick="Download_Click" runat="server"/>
                </div>
	        </div>
        </div> 
    </div>   
    
</asp:Content>