<%@ Page Language="C#"MasterPageFile="~/Administrator.master" AutoEventWireup="true" CodeFile="ManageFederation.aspx.cs" Inherits="Administrators_ManageFederation" %>
<%@ Register src="../Controls/RebuildThumbnailCache.ascx" tagname="RebuildThumbnailCache" tagprefix="uc1" %>


<asp:Content ID="Content1" runat="server"  contentplaceholderid="AdminContentPlaceHolder" >
    <style type="text/css">
.outlineDiv
        {
            border-right: outset 2px blue;
            border-left: outset 2px blue;
            border-top: outset 2px blue;
            border-bottom: outset 2px blue;
            width:750px;
        }
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
         input[type=text]
        {
            height:20px;   
        }
       input[type=password]
        {
            height:20px;   
        }
        input[type=checkbox]
        {
            height:15px;   
        }
</style>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.8.7.custom.min.js"></script>
        <script type="text/javascript" src="../Scripts/jquery.cookie.js"></script>
 <script type="text/javascript">
     function Changed(id) {

         $("." + id).attr("src", "../styles/images/Icons/warning.gif");
     }

     $(document).ready(function () {
         $("#tabs").tabs({ cookie: { expires: 30} });


     });
    </script>
<div style="width:550px;margin:0px auto">
    <div style="display:inline-block;margin-right:auto">
        <asp:Image ID="APIStatusIcon" runat="server" style="margin-left:auto;height:100px;width:100px" ImageURL="../styles/images/big_ok.png" />
        <div style="font-size:x-small; color:Gray;margin-left:auto;width:100px">API Online</div>
    </div>
    <div style="display:inline-block">
        <asp:Image ID="EnrollmentStatusIcon" runat="server" style="margin-left:auto;height:100px;width:100px" ImageUrl="../styles/images/big_ok.png" />
        <div style="font-size:x-small; color:Gray;margin-left:auto;width:100px">Namespace Enrolled</div>
    </div>
    <div style="display:inline-block">
        <asp:Image ID="FederationStateIcon" runat="server" style="margin-left:auto;height:100px;width:100px" ImageUrl="../styles/images/big_ok.png" />
        <div style="font-size:x-small; color:Gray;margin-left:auto;width:100px">Federation Active</div>
    </div>
     <div style="display:inline-block">
        <asp:Image ID="DownloadAllowedIcon" runat="server" style="margin-left:auto;height:100px;width:100px" ImageUrl="../styles/images/big_ok.png" />
        <div style="font-size:x-small; color:Gray;margin-left:auto;width:100px">Download Allowed</div>
    </div>
     <div style="display:inline-block">
        <asp:Image ID="SearchAllowedIcon" runat="server" style="margin-left:auto;height:100px;width:100px" ImageUrl="../styles/images/big_ok.png" />
        <div style="font-size:x-small; color:Gray;margin-left:auto;width:100px">Search Allowed</div>
    </div>
</div>
<br />
<asp:Panel runat="server">
<div>
The 3DR Federation is a service hosted by ADL that can aggregate content from seperate 3DR servers. When a user searches the federation, the search query is forwared to each of the participating 3DR servers, and results are agregated and returned to the user. When someone requests a model from the federation, the namespace of the model's PID is used to determine the proper server, and the request is either forwarded or proxied to that server. Currently, the federation uses the AnonymousUser account to access the individual server, so if you want data to be available to federation users, it must be marked for download and search by anonymous users. You can see a list of all the participating 3DR servers <a href="http://3dr.adlnet.gov/Federation/FederationManager.htm">here.</a> Currently, the federation is only exposed to queries through an API. This API mirrors the interface to the individual 3DR API's, so that applications may be written against both services simultaniously. ADL currently host's a simple HTML search tool for searching the federation at <a href="http://http://3dr.adlnet.gov/Federation/FederationTest.htm">http://http://3dr.adlnet.gov/Federation/FederationTest.htm.</a> In the future, individual 3DR servers that participte in the federation will be able to display search results from the federation on their own search pages.
</div></asp:Panel><br />

 <div id="tabs">
	        <ul>
		        <li><a href="#tabs-Enrollment">Enroll</a></li>
                <li><a href="#tabs-Status">Set Status</a></li>
	        </ul>
	        <div id="tabs-Enrollment" style="width:750px">

                <div>These settings have been gathered from the website configuration. Settings marked with an X require changes before federation is possible.</div><br />
                <div class="outlineDiv">
 
                 <table style="width:750px">
                 
                 <tr>
                    <td><img runat="server" id="APIURLstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td>
                    <td style="width:20%">API Url</td>
                    <td><asp:label style="width:95%" ID="APIURL" text="the value of the api" runat="server"></asp:label><br />
                    <asp:label runat="server" ID="APIURLHelp" text="The URL of this server's API. Visit the <a href='APIControl.aspx'>API page</a> to modify this value" style="font-size:x-small; color:Gray"></asp:label></td>
                 </tr>
                 <tr>
                    <td><img runat="server" id="Namespacestatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td>
                    <td style="width:20%">Namespace</td>
                    <td><asp:label style="width:95%" ID="Namespace" text="the value of the namespace" runat="server"></asp:label><br />
                    <asp:label runat="server" ID="NamespaceHelp" text="The namespace reserved in the Federation for this server's API. Visit the <a href='APIControl.aspx'>API page</a> to modify this value" style="font-size:x-small; color:Gray"></asp:label></td>
                 </tr>
                  <tr>
                    <td><img runat="server" id="OrganizationNameStatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td>
                    <td style="width:20%">Organization Name</td>
                    <td><asp:label style="width:95%" ID="OrganizationName" text="the value of the OrganizationName" runat="server"></asp:label><br />
                    <asp:label runat="server" ID="OrganizationNamehelp" text="Change the 'Company Name' on the <a href='Settings.aspx'>settings page</a> to modify this value" style="font-size:x-small; color:Gray"></asp:label></td>
                 </tr>
                 <tr>
                    <td><img runat="server" id="OrganizationURLstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td>
                    <td style="width:20%">Organization URL</td>
                    <td><asp:label style="width:95%" ID="OrganizationURL" text="the value of the OrganizationURL" runat="server"></asp:label><br />
                    <asp:label runat="server" ID="OrganizationURLhelp" text="The address of this 3DR web site." style="font-size:x-small; color:Gray"></asp:label></td>
                 </tr>
                 <tr>
                    <td><img runat="server" id="OrganizationEmailstatus" alt="check" src="../styles/images/Icons/checkmark.gif"/></td>
                    <td style="width:20%">Organization Email</td>
                    <td><asp:label style="width:95%" ID="OrganizationEmail" text="the value of the OrganizationURL" runat="server"></asp:label><br />
                    <asp:label runat="server" ID="OrganizationEmailHelp" text="Change the 'Support Email' on the <a href='Settings.aspx'>settings page</a> to modify this value" style="font-size:x-small; color:Gray"></asp:label></td>
                 </tr>    
                 </table>

                </div>    
                <br />
                <div>Please specify these settings</div>
                <div class="outlineDiv">
 
                 <table style="width:750px">
                 
                 <tr>
                    <td style="width:20%">Allow Download</td>
                    <td><asp:CheckBox style="width:95%;height:20px;vertical-align:middle" ID="AllowDownload" text="Allow the federation to download from this server" runat="server"></asp:CheckBox></td>
                 </tr>
                 <tr>
                    <td style="width:20%">Allow Search</td>
                    <td><asp:CheckBox style="width:95%;height:20px;vertical-align:middle" ID="AllowSearch" text="Allow the federation to search this server" runat="server"></asp:CheckBox></td>
                 </tr>
                 <tr>
                    <td style="width:20%">UserName</td>
                    <td><asp:TextBox style="width:95%;height:20px;vertical-align:middle" ID="POCName" runat="server"></asp:TextBox></td>
                 </tr>
                  <tr>
                    <td style="width:20%">Password</td>
                    <td><opp:PasswordTextBox style="width:95%;height:20px;vertical-align:middle" ID="FederationPassword1" runat="server"></opp:PasswordTextBox></td>
                 </tr>
                   <tr>
                    <td style="width:20%">Confirm</td>
                    <td><opp:PasswordTextBox style="width:95%;height:20px;vertical-align:middle" ID="FederationPassword2" runat="server"></opp:PasswordTextBox></td>
                 </tr>
                 </table>
 
                </div>
<br />
<asp:Button ID="EnrollServer" runat="server" Text="Join the Federation!" 
                    onclick="EnrollServer_Click"/><asp:Label ID="RequestFederationStatus" runat="server"></asp:Label>
                </div>
                <div id="tabs-Status" style="width:750px">     
                
                    <table style="width:750px">
                     
                     
                        <tr>
                        <td style="width:20%">Allow Download</td>
                        <td><asp:DropDownList style="width:95%" ID="FederateStateRequest" runat="server">
                         <asp:ListItem>Online</asp:ListItem>
                         <asp:ListItem>Offline</asp:ListItem>
                         </asp:DropDownList></td>
                     </tr>
                     <tr>
                        <td style="width:20%">Allow Download</td>
                        <td><asp:CheckBox style="width:95%;height:20px;vertical-align:middle" ID="AllowDownloadUpdate" text="Allow the federation to download from this server" runat="server"></asp:CheckBox></td>
                     </tr>
                     <tr>
                        <td style="width:20%">Allow Search</td>
                        <td><asp:CheckBox style="width:95%;height:20px;vertical-align:middle" ID="AllowSearchUpdate" text="Allow the federation to search this server" runat="server"></asp:CheckBox></td>
                     </tr>
                      <tr>
                        <td style="width:20%">UserName</td>
                        <td><opp:PasswordTextBox style="width:95%;height:20px;vertical-align:middle" ID="UpdatePOCName" runat="server"></opp:PasswordTextBox></td>
                     </tr>
                      <tr>
                        <td style="width:20%">Password</td>
                        <td><opp:PasswordTextBox style="width:95%;height:20px;vertical-align:middle" ID="UpdateFederationPassword1" runat="server"></opp:PasswordTextBox></td>
                     </tr>
                       <tr>
                        <td style="width:20%">Confirm</td>
                        <td><opp:PasswordTextBox style="width:95%;height:20px;vertical-align:middle" ID="UpdateFederationPassword2" runat="server"></opp:PasswordTextBox></td>
                     </tr>
                     </table>
                     <br />
<asp:Button ID="RequestStatusChange" runat="server" Text="Change Status" OnClick="RequestStatusChange_Click"/>
                </div><asp:Label ID="UpdateFederationStatus" runat="server"></asp:Label>
</div>

</asp:Content>