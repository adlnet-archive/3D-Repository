
<%@ Page Language="C#" MasterPageFile="~/Administrator.master" AutoEventWireup="true" CodeFile="Email Text.aspx.cs" Inherits="Administrators_Email_Text" %>
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
            margin-top:10px;
        }
        tr:nth-child(odd) 
        {
            background-color:#FFFFFF;
            width:100px;
            margin-top:10px;
        }
        .Outlined
        {
           
            width:750px;
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
    <div>The 3DR can be configured to send emails when certain events occur. You can enter text for those emails below. All emails will be from the Support Address, and will be sent either to the user, or to the administrator of the site. In the case that the email is sent to the administrator, it will be sent to the Support Address. You must have a working connection to a SMPT server for mail to be sent. Visit the <a href="settings.aspx">settings</a> page to modify this connection.</div><br />
     <div id="tabs">
	        <ul>
		        <li><a href="#tabs-NewModel">Model Uploaded</a></li>
                <li><a href="#tabs-NewUser">User Registered</a></li>
		        <li><a href="#tabs-Approved">Registration Approved</a></li>
                <li><a href="#tabs-Requested">Registration Requested</a></li>
	        </ul>
	        <div id="tabs-NewModel">
                <table  class="Outlined">
                     <tr class="thead"><td class="thead">Sent On:</td><td class="thead">New Model Uploaded to the repository</td> </tr>   
                     <tr><td style="width:20%">Subject</td><td><asp:TextBox runat="server" onkeydown="javascript: Changed( 'UploadedSubjectstatus' );" style="width:95%"  id="UploadedSubject" ></asp:TextBox><img class="UploadedSubjectstatus" runat="server" ID="UploadedSubjectstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
                     <tr><td style="width:20%">Message</td><td><asp:TextBox TextMode="MultiLine" onkeydown="javascript: Changed( 'UploadedBodystatus' );" style="width:95%;height:300px;" ID="UploadedBody" runat="server"></asp:TextBox><img class="UploadedBodystatus" runat="server" ID="UploadedBodystatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
                     <tr><td style="width:20%">Enabled</td><td style="width:80%"><asp:DropDownList style="width:95%" ID="UploadedEnabled" onchange="javascript: Changed( 'UploadedEnabledstatus' );" runat="server">
                         <asp:ListItem>true</asp:ListItem>
                         <asp:ListItem>false</asp:ListItem>
                         </asp:DropDownList><img class="UploadedEnabledstatus" runat="server" ID="UploadedEnabledstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/><div style="font-size:x-small; color:Gray">This message will be sent to the site support address when a user uploads a model. You can use {pid}, {username}, and {title}, which will be filled with the appropriate details.</div></td></tr>
                <tr><td></td><td><asp:Button  id="SaveMySQLSettings" Text="Save" runat="server"  onclick="SaveNewModelSettings_Click"/> </tr></td>
                </table>
            </div>
            <div id="tabs-NewUser">
                <table class="Outlined">
                     <tr class="thead"><td class="thead">Sent On:</td><td class="thead">A new user has registered for the site.</td> </tr>   
                     <tr><td style="width:20%">Subject</td><td><asp:TextBox onkeydown="javascript: Changed( 'RegisteredSubjectstatus' );" style="width:95%" ID="RegisteredSubject" runat="server"></asp:TextBox><img class="RegisteredSubjectstatus" runat="server" ID="RegisteredSubjectstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
                     <tr><td style="width:20%">Message</td><td><asp:TextBox TextMode="MultiLine" onkeydown="javascript: Changed( 'RegisteredBodystatus' );" style="width:95%;height:300px;" ID="RegisteredBody" runat="server"></asp:TextBox><img class="RegisteredBodystatus" runat="server" ID="RegisteredBodystatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
                     <tr><td style="width:20%">Enabled</td><td style="width:80%"><asp:DropDownList style="width:95%" ID="RegisteredEnabled" onchange="javascript: Changed( 'RegisteredEnabledstatus' );" runat="server">
                         <asp:ListItem>true</asp:ListItem>
                         <asp:ListItem>false</asp:ListItem>
                         </asp:DropDownList><img class="RegisteredEnabledstatus" runat="server" ID="RegisteredEnabledstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/><div style="font-size:x-small; color:Gray">This will be sent to the site support address when someone enrolls a new user account. You can use {username} and {email} which will be filled with the user details.</div></td></tr>
        
                <tr><td></td><td><asp:Button  ID="Button1" Text="Save" runat="server" onclick="SaveNewUserRegisteredSettings_Click"/> </tr></td>
                </table>
            </div>
            <div id="tabs-Approved">
                 <table class="Outlined">
                     <tr class="thead"><td class="thead">Sent On:</td><td class="thead">An account request has been approved.</td> </tr>   
                     <tr><td style="width:20%">Subject</td><td><asp:TextBox onkeydown="javascript: Changed( 'ApprovedSubjectstatus' );" style="width:95%" ID="ApprovedSubject" runat="server"></asp:TextBox><img class="ApprovedSubjectstatus" runat="server"  ID="ApprovedSubjectstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
                     <tr><td style="width:20%">Message</td><td><asp:TextBox TextMode="MultiLine" onkeydown="javascript: Changed( 'ApprovedBodystatus' );" style="width:95%;height:300px;" ID="ApprovedBody" runat="server"></asp:TextBox><img class="ApprovedBodystatus" runat="server" ID="ApprovedBodystatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
                     <tr><td style="width:20%">Enabled</td><td style="width:80%"><asp:DropDownList style="width:95%" ID="ApprovedEnabled" onchange="javascript: Changed( 'ApprovedEnabledstatus' );" runat="server">
                         <asp:ListItem>true</asp:ListItem>
                         <asp:ListItem>false</asp:ListItem>
                         </asp:DropDownList><img class="ApprovedEnabledstatus" runat="server" ID="ApprovedEnabledstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/><div style="font-size:x-small; color:Gray">This will be sent to a user when an account they have requested is approved by the site administrator. You can include {username} and {passwordhint} which will be filled with the proper values. </div></td></tr>
        
        
                <tr><td></td><td><asp:Button  ID="Button2" Text="Save" runat="server" onclick="SaveRegistrationApprovedSettings_Click"/> </tr></td>
                </table>
            </div>
            <div id="tabs-Requested">
                         <table class="Outlined">
                     <tr class="thead"><td class="thead">Sent On:</td><td class="thead">A user requested an account.</td> </tr>   
                     <tr><td style="width:20%">Subject</td><td><asp:TextBox onkeydown="javascript: Changed( 'RequestedSubjectstatus' );" style="width:95%" ID="RequestedSubject" runat="server"></asp:TextBox><img class="RequestedSubjectstatus" runat="server"  ID="RequestedSubjectstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
                     <tr><td style="width:20%">Message</td><td><asp:TextBox TextMode="MultiLine" onkeydown="javascript: Changed( 'RequestedBodystatus' );" style="width:95%;height:300px;" ID="RequestedBody" runat="server"></asp:TextBox><img class="RequestedBodystatus" runat="server" ID="RequestedBodystatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td></tr>
                     <tr><td style="width:20%">Enabled</td><td style="width:80%"><asp:DropDownList style="width:95%" ID="RequestedEnabled" onchange="javascript: Changed( 'RequestedEnabledstatus' );" runat="server">
                         <asp:ListItem>true</asp:ListItem>
                         <asp:ListItem>false</asp:ListItem>
                         </asp:DropDownList><img class="RequestedEnabledstatus" runat="server" ID="RequestedEnabledstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/><div style="font-size:x-small; color:Gray">This will be sent to a user to confirm that they have requested an account on the site.</div></td></tr>
        
                <tr><td></td><td><asp:Button  ID="Button3" Text="Save" runat="server" onclick="SaveRegistrationRequestedSettings_Click"/> </tr></td>
                </table>
            </div>
        </div>   
    </div>
</asp:Content>