<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="RecoverDatabaseConnection.aspx.cs" Inherits="Public_RecoverDatabaseConnection" %>
<asp:Content ID="Content1" runat="server" 
    contentplaceholderid="ContentPlaceHolder1">
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
    <div style="width:750px;margin:0px auto;">
        
        <div>There was an error connecting to the database. This could mean that the database server is not available, or that this application is configured incorrectly. If you are the system administrator, please enter your name and password here to access the database connection recovery tool. You will find your name and password in the DefaultAdminName and DefaultAdminPassword keys in the web.config file. Please note that other users with administrative privileges will not be able to access this tool. Data about other administrative users is not available while the database is inaccessible. Likewise, if you have changed the default admin password in the database, this change will not be reflected in this recovery tool. You must use the password supplied when you installed the 3DR.</div>
        <asp:Label ID="loginStatus" Text="" runat="server"/>
        <asp:Panel ID="LoginPanel" Visible="true" runat="server">
        <table style="width:750px">
             <tr class="thead"><td class="thead">Login</td><td class="thead"> </td> </tr>   
             <tr><td style="width:20%"> 3DR Admin Name</td><td><asp:TextBox style="width:95%"  ID="AdminName" runat="server"></asp:TextBox></td></tr>
             <tr><td style="width:20%"> 3DR Admin Password</td><td><opp:PasswordTextBox style="width:95%" TextMode="Password" ID="AdminPassword" runat="server"></opp:PasswordTextBox></td></tr>
        </table>
        
        <asp:Button ID="Login" Text="Log in" runat="server" onclick="Login_Click" />
        </asp:Panel>
        <asp:Panel ID="SQLSettingPanel" Visible="false" runat="server">
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
        </asp:Panel>
    </div>   
    
</asp:Content>
