<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MessageCenter.aspx.cs" Inherits="Users_MessageCenter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style>

.messagetable
{
    padding:0px 0px 0px 0px;
    border:none;
   
}
.messagelist
{
    border-bottom: none;
    border-right: none;
    border-top: none;
    border-left: none;
    
}
.innertable
{
   
    border-right: none;
    border-top: none;
    border-bottom: none;
    border-radius:10px;
}

.accessbutton
{
font-family: sans-serif;
background-color: #8A2223;
color: white;
padding: 0px 3px;
border-radius: 5px;
box-shadow: 1px 1px 2px gray;
border: 1px black solid;
font-size: .6em;
vertical-align: middle;
width: 10%;
text-align: center;
margin-top: 10px;
cursor: pointer;
}
.accessbutton:hover
{
    background-color:#BA2223
}
.box
{
    border-radius:10px;
    box-shadow:lightgray 5px 5px 40px inset;
    padding:20px 20px 20px 20px;
}
.boxlabel
{
    font-family:Georgia;
    font-size: 1em;
text-shadow: 2px 2px 10px #757575;
padding: 5px;
border-radius:5px;
  cursor:pointer;
}
.boxlabel:hover
{
    color:darkblue;
}
.compose
{
    background-color: #8A2223;
border-radius: 3px;
border: 4px groove lightgray;
color: white;
text-align: center;
font-family:Georgia;
    font-size: 1em;
text-shadow: 2px 2px 10px #000000;
padding: 5px;
border-radius:5px;
  cursor:pointer;
}
.compose:hover
{
    background-color: #BA2223;
}
.messageheader
{
    font-family: Georgia;
    display:inline;
    margin-right: 1em;
    max-width:30em;
    overflow:hidden;
}
.messagetext
{
    font-family: Georgia;
}
.messagebody
{
    height: 19em;
    overflow-y: auto;
    font-family: Georgia;
    line-height:.999em;
    margin-bottom: 10px;
}
.closebutton
{
    float: right;
font-family: sans-serif;
background-color: #8A2223;
color: white;
padding: 0px 3px;
border-radius: 5px;
box-shadow: 1px 1px 2px gray;
border: 1px black solid;
font-size: .6em;
vertical-align: middle;
margin-top: 2px;
}
.closebutton:hover
{
    background-color:#BA2223
}
.MailBoxList li
{
    cursor:pointer;
    padding: 5px;
border-radius:5px;
}
.MailBoxList li:hover
{
    color:darkblue;
  
}
.inputbox
{
    border-radius: 5px;
box-shadow: inset 0px 0px 10px #AAA;
border: solid 1px #777;
margin-bottom: .5em;
}
</style>
 <script type="text/javascript" src="../Scripts/jquery-ui-1.8.7.custom.min.js"></script>
<script type="text/javascript">


    var ReadTimeout;
    var PollingTimeout;
    var gMessages;
    var fromLabel = 'FromName';
    var gThreadID = -1;
    var selected = -1;
    var currentbox = "Inbox";
    var replytext = "";
    function toID(myString) { return myString.replace(/\W/g, ''); }
    function ShowMessage(id,clicked) {
        
        for (var i = 0; i < gMessages.length; i++) {
            if (gMessages[i].ID == id) {
                selected = id;
                clearTimeout(ReadTimeout);
                
                $('#messagebody').html(gMessages[i].MessageText.replace(/\n/g,"<br/>"));
                var date = new Date(parseInt(gMessages[i].DateSent.substr(6)));
                $('#subject').html(gMessages[i].Subject);
                $('#DateSent').html(date.toString());

                
                if(gMessages[i].FromName == $('#ctl00_LoginView1_LoginName1').html())
                    $('#from').html('Me');
                else
                    $('#from').html(gMessages[i]['FromName']);

                if (gMessages[i].ToName == $('#ctl00_LoginView1_LoginName1').html())
                    $('#to').html('Me');
                else
                    $('#to').html(gMessages[i].ToName);
                
                if (gMessages[i].Read == false && clicked == true) {
                    ReadTimeout = window.setTimeout(function () { ReadMessage(id); }, 2000);
                    
                }


            }
        }
        
        HideCompose();
    }
    function ReadMessage(id) {


        $.ajax({
            type: "POST",
            url: "MessageCenter.aspx/ReadMessage",
            data: JSON.stringify({ MessageID: id }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (e) {

                if (e.d == true) {
                    $('#Message' + id).css('font-weight', '');
                }
                for (var i = 0; i < gMessages.length; i++) {
                    if (gMessages[i].ID == id) {
                        gMessages[i].Read = true;
                    }
                }

            },
            error: function (e, xhr) {
                window.location.href = "/public/login.aspx?ReturnUrl=%2fUsers%2fMessageCenter.aspx";
            }
        });

    }
    function GrantOrDenyRequest(inusername, inmode, inpid ) {


        $.ajax({
            type: "POST",
            url: "MessageCenter.aspx/GrantOrDenyRequest",
            data: JSON.stringify({ username: inusername, mode: inmode, pid: inpid }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (e) {

                $('#dig').html(e.d);
                $('#dig').dialog('open');

            },
            error: function (e, xhr) {
                window.location.href = "/public/login.aspx?ReturnUrl=%2fUsers%2fMessageCenter.aspx";
            }
        });

    }
    function PopulateMailboxView(e, Mailbox)
     {
                    $('#InboxLabel').html("Inbox (" + e.d.InboxCount + ")");
                    $('#NewLabel').html("New (" + e.d.NewCount + ")");
                    $('#SentLabel').html("Sent (" + e.d.SentCount + ")");
                    $('#TrashLabel').html("Trash (" + e.d.TrashCount + ")");

                    if (e.d.NewCount > 0) {
                        $('#InboxLabel').css('font-weight', 'bold');
                        $('#NewLabel').css('font-weight', 'bold');
                    } else {
                        $('#InboxLabel').css('font-weight', '');
                        $('#NewLabel').css('font-weight', '');
                    }
                    if (Mailbox != currentbox)
                        return;
                    gMessages = e.d.messages;
                    $('#MailBoxList').html('');

                    for (var i = e.d.messages.length - 1; i >= 0; i--) {

                        var BoxLabel = "";
                        fromLabel = 'FromName';
                        if (currentbox == "Search") {
                            BoxLabel = '<div style="color:#AAAAAA;width: 5%;overflow-x: hidden;display: inline-block;margin-right:1.5em">' + e.d.messages[i].Mailbox + '</div>';
                            if (e.d.messages[i].Mailbox == 'Trash') {
                                $('#deletebutton').button('option', 'disabled', true);
                                $('#replybutton').button('option', 'disabled', true);
                                $('#forwardbutton').button('option', 'disabled', false);
                            }
                            if (e.d.messages[i].Mailbox == 'Inbox') {
                                $('#deletebutton').button('option', 'disabled', false);
                                $('#replybutton').button('option', 'disabled', false);
                                $('#forwardbutton').button('option', 'disabled', false);
                            }
                            if (e.d.messages[i].Mailbox == 'Sent') {
                                $('#deletebutton').button('option', 'disabled', false);
                                $('#replybutton').button('option', 'disabled', true);
                                $('#forwardbutton').button('option', 'disabled', false);
                                fromLabel = 'ToName';
                            }
                        }
                        if (currentbox == "Inbox" || currentbox == "Trash")
                            BoxLabel = '<div style="color:#AAAAAA;width: 5%;overflow-x: hidden;display: inline-block;margin-right:1.5em">' + 'From' + '</div>';
                        if (currentbox == "Sent")
                            BoxLabel = '<div style="color:#AAAAAA;width: 5%;overflow-x: hidden;display: inline-block;margin-right:1.5em">' + 'To' + '</div>';

                        if (e.d.messages[i].Mailbox == "Sent")
                            fromLabel = 'ToName';
                        var fromtext = e.d.messages[i][fromLabel];
                        if (fromtext == $('#ctl00_LoginView1_LoginName1').html())
                            fromtext = 'Me'

                        var listitem = '<li id="Message' + e.d.messages[i].ID + '">'+BoxLabel+'<div class = "messageheader" style="width: 30%;overflow-x: hidden;display: inline-block;">' + fromtext + '</div><div class = "messageheader">' + e.d.messages[i].Subject + '</div></li>'
                        $('#MailBoxList').html($('#MailBoxList').html() + listitem);

                    }
                    for (var i = e.d.messages.length - 1; i >= 0; i--) {
                        var id_ = '' + e.d.messages[i].ID;
                        $('#Message' + id_).attr('messageid', id_ + '');
                        if (e.d.messages[i].Read != true) {
                            $('#Message' + id_).css('font-weight', 'bold');
                        }

                        $('#Message' + id_).click(function () {
                            ShowMessage($(this).attr('messageid'),true);
                            $('.MailBoxList li').css('background-color', '');
                            $(this).css('background-color', 'lightgrey');
                        });
                    }
                    if (e.d.messages.length > 0) {
                        if (selected == -1) {
                            
                            ShowMessage($('#Message' + gMessages[gMessages.length - 1].ID).attr('messageid'), false);
                            $('.MailBoxList li').css('background-color', '');
                            $('#Message' + gMessages[gMessages.length - 1].ID).css('background-color', 'lightgrey');
                        }
                        else {
                            ShowMessage($('#Message' + selected).attr('messageid'), false);
                            $('.MailBoxList li').css('background-color', '');
                            $('#Message' + selected).css('background-color', 'lightgrey');
                        }
                    } else {
                        $('#MailBoxList').html('<div style="color:lightgrey">No messages</div>');
                        $('#messagebody').html('<div style="color:lightgrey">No messages</div>');
                        $('#subject').html('<div style="color:lightgrey">No messages</div>');
                        $('#DateSent').html('<div style="color:lightgrey">No messages</div>');
                        $('#from').html('<div style="color:lightgrey">No messages</div>');
                        $('#to').html('<div style="color:lightgrey">No messages</div>');
                    }
    }
     
    function GetMessages(boxname) {

        //UpdateAssetData(string Title, string Description, string Keywords, string License)
        $.ajax({
            type: "POST",
            url: "MessageCenter.aspx/GetMessages",
            data: JSON.stringify({ Mailbox: boxname }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (e) {

                if (e.d.Success == true) {

                    PopulateMailboxView(e, this.Mailbox);
                }

            }.bind({ Mailbox: boxname }),
            error: function (e, xhr) {
                window.location.href = "/public/login.aspx?ReturnUrl=%2fUsers%2fMessageCenter.aspx";
            }
        });

    }

    function ShowCompose() {
        $('#ComposeMessageTo').show();
        $('#ComposeMessageSubject').show();
        $('#ComposeMessageBody').show();
        $('#SendButton').show();
        $('#messagedata').hide();
        $('#messagebody').hide();
        $('#replybutton').hide();
        $('#deletebutton').hide();
        $('#forwardbutton').hide();
    }
    function HideCompose() {

        $('#ComposeMessageTo').hide();
        $('#ComposeMessageSubject').hide();
        $('#ComposeMessageBody').hide();
        $('#messagedata').show();
        $('#messagebody').show();
        $('#SendButton').hide();
        $('#replybutton').show();
        $('#deletebutton').show();
        $('#forwardbutton').show();
        replytext = '';
    }
    function Reply() {
        clearTimeout(PollingTimeout);
        $('#ComposeMessageTo').val($('#from').html());
        $('#ComposeMessageSubject').val("RE: " + $('#subject').html());
        var header = "\n\n<div style='margin-left:.5em;border-left:1px solid black;padding-left:.5em'><div style='font-size:smaller;color:lightgray'>" + $('#from').html() + " sent on \n" + $('#DateSent').html() + "\nSubject: " + $('#subject').html() + "\n\n</div>";
        replytext = header + $('#messagebody').html().replace(/<br>/g, "\n") + "</div>";
        $('#ComposeMessageBody').val("");
        ShowCompose();
    }
    function Forward() {
        clearTimeout(PollingTimeout);
        $('#ComposeMessageTo').val("");
        $('#ComposeMessageSubject').val("FW: " + $('#subject').html());
        var header = "\n\n<div style='margin-left:.5em;border-left:1px solid black;padding-left:.5em'><div style='font-size:smaller;color:lightgray'>" + $('#from').html() + " sent on \n" + $('#DateSent').html() + "\nSubject: " + $('#subject').html() + "\n\n</div>";
        replytext = header + $('#messagebody').html().replace(/<br>/g, "\n") + "</div>";
        $('#ComposeMessageBody').val("");
        ShowCompose();
    }
    function SendMessage() {

        //UpdateAssetData(string Title, string Description, string Keywords, string License)
        $.ajax({
            type: "POST",
            url: "MessageCenter.aspx/SendMessage",
            data: JSON.stringify({ toName: $('#ComposeMessageTo').val(), Subject: $('#ComposeMessageSubject').val(), Messagetext: $('#ComposeMessageBody').val() + replytext, thread: gThreadID }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (e) {

                if (e.d == true) {
                    $('#SentLabel').click();
                } else {
                    alert('Error Sending Message');
                }

            },
            error: function (e, xhr) {
                window.location.href = "/public/login.aspx?ReturnUrl=%2fUsers%2fMessageCenter.aspx";
            }
        });


    }
    function DeleteMessage() {

        //UpdateAssetData(string Title, string Description, string Keywords, string License)
        $.ajax({
            type: "POST",
            url: "MessageCenter.aspx/DeleteMessage",
            data: JSON.stringify({id:selected}),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (e) {

                if (e.d == true) {
                    $('#InboxLabel').click();
                } else {
                    alert('Error Sending Message');
                }

            },
            error: function (e, xhr) {
                window.location.href = "/public/login.aspx?ReturnUrl=%2fUsers%2fMessageCenter.aspx";
            }
        });


    }
    function inboxpoll()
    {
        GetMessages('Inbox');
        PollingTimeout = window.setTimeout(function () { inboxpoll(); },5000);
        return PollingTimeout;
    }
    function newpoll() {
        GetMessages('New');
        PollingTimeout = window.setTimeout(function () { newpoll(); }, 5000);
        return PollingTimeout;
    }
    function Search(terms, inlabel) {
        
        if (terms.length <= 2) {
            
            $('#MailBoxList').html('<div style="color:lightgrey">The search term must be at least 3 characters</div>');
            return;
        }
        //UpdateAssetData(string Title, string Description, string Keywords, string License)
        $.ajax({
            type: "POST",
            url: "MessageCenter.aspx/SearchMessages",
            data: JSON.stringify({ term: terms }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (e) {

                if (e.d.Success == true) {

                    PopulateMailboxView(e, this.Mailbox);

                    var term = $(inlabel).attr('term');
                    $(inlabel).html('Search (' + e.d.messages.length + '): ' + term);
                    $(inlabel).append('<div id="closebutton' + toID(terms) + '" class="closebutton">X</div>');

                    $('#closebutton' + toID(terms)).click(function () {
                        $('#SearchBox').css('margin-top', parseInt($('#SearchBox').css('margin-top').substr(0, $('#SearchBox').css('margin-top').length - 2)) + parseInt($('#SearchLabel' + toID(terms)).outerHeight()) + 'px');
                        $(inlabel + "").remove();
                    });

                }
            } .bind({ Mailbox: 'Search', label: inlabel }),
            error: function (e, xhr) {
                window.location.href = "/public/login.aspx?ReturnUrl=%2fUsers%2fMessageCenter.aspx";
            }
        });
    }
    $(document).ready(function () {

        GetMessages('Inbox');
        HideCompose();
        $('.boxlabel').click(function () {
            $('.boxlabel').css('color', '');
            $(this).css('color', 'blue');
            $('.boxlabel').css('background-color', '');
            $(this).css('background-color', 'lightgrey');
        });
        $('#dig').dialog({ Title: 'Status', buttons: { Ok: function () { $('#dig').dialog('close'); } }, autoOpen: false, modal: true });
        $('#InboxLabel').click(function () {
            $("#replybutton").button("option", "disabled", false);
            $("#deletebutton").button("option", "disabled", false);
            $("#forward").button("option", "disabled", false);
            fromLabel = 'FromName';
            currentbox = "Inbox";
            selected = -1;
            //GetMessages('Inbox');
            clearTimeout(PollingTimeout);
            PollingTimeout = inboxpoll();
            HideCompose();

        });
        $('#NewLabel').click(function () {
            $("#replybutton").button("option", "disabled", false);
            $("#deletebutton").button("option", "disabled", false);
            $("#forward").button("option", "disabled", false);
            fromLabel = 'FromName';
            currentbox = "New";
            selected = -1;
            //GetMessages('New');
            clearTimeout(PollingTimeout);
            PollingTimeout = newpoll();
            HideCompose();

        });
        $('#SentLabel').click(function () {
            $("#replybutton").button("option", "disabled", true);
            $("#deletebutton").button("option", "disabled", false);
            $("#forward").button("option", "disabled", false);
            currentbox = "Sent";
            fromLabel = 'ToName';
            selected = -1;
            GetMessages('Sent');
            clearTimeout(PollingTimeout);

            PollingTimeout = inboxpoll();
            HideCompose();

        });
        $('#TrashLabel').click(function () {
            $("#replybutton").button("option", "disabled", true);
            $("#deletebutton").button("option", "disabled", true);
            $("#forward").button("option", "disabled", true);
            fromLabel = 'FromName';
            currentbox = "Trash";
            selected = -1;
            GetMessages('Trash');
            clearTimeout(PollingTimeout);
            PollingTimeout = inboxpoll();
            PollingNeedsGUIrefresh = false;
            HideCompose();
        });
        $('#ComposeLabel').click(function () {
            currentbox = "Compose";
            ShowCompose();
            selected = -1;
            clearTimeout(PollingTimeout);
            PollingTimeout = inboxpoll();
            PollingNeedsGUIrefresh = false;
            gThreadID = -1;

            $('#ComposeMessageTo').val("");
            $('#ComposeMessageSubject').val("");

            $('#ComposeMessageBody').val("");

        });

        $('#SendButton').button();

        $('#SendButton').click(function () {
            SendMessage();
        });

        $('#deletebutton').button();

        $('#deletebutton').click(function () {
            DeleteMessage();
        });

        $('#replybutton').button();

        $('#replybutton').click(function () {
            Reply();
        });

        $('#forwardbutton').button();

        $('#forwardbutton').click(function () {
            Forward();
        });

        $('#SearchBox').keyup(function (event) {
            if (event.keyCode == '13') {
                currentbox = "Search";
                var term = $('#SearchBox').val();
                term = toID(term);
                if ($('#SearchLabel' + term).length == 0) {
                    $('#SearchBox').before("<div id='SearchLabel" + term + "' class='boxlabel'>Searching...</div>");

                    $('#SearchLabel' + term).attr('term', $('#SearchBox').val());
                    //Search($('#SearchBox').val());
                    $('#SearchLabel' + term).html('Search: ' + $('#SearchBox').val());



                    $('#SearchBox').val('');
                    $('#SearchLabel' + term).show();
                    $('#SearchLabel' + term).click(function () {
                        $('.boxlabel').css('color', '');
                        $(this).css('color', 'blue');
                        $('.boxlabel').css('background-color', '');
                        $(this).css('background-color', 'lightgrey');
                        currentbox = "Search";
                        var term2 = $('#SearchLabel' + term).attr('term');
                        if (term2 != null)
                            Search(term2, '#SearchLabel' + toID(term));
                    });
                    $('#SearchLabel' + term).click();
                    $('#SearchBox').css('margin-top', $('#SearchBox').css('margin-top').substr(0, $('#SearchBox').css('margin-top').length - 2) - $('#SearchLabel' + term).outerHeight());
                } else {
                    $('#SearchLabel' + term).click();
                    $('#SearchBox').val('');
                }
            }
        });

        $('#aspnetForm').submit(function (e) {
            e.preventDefault();
        });

        $('#InboxLabel').click();
    });





</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table class="" style="width: 80%;margin: 0 auto;"><tr><td>
<table style='width:100%;left:10%;height:40%' class="messagetable">
    <tr>
        <td id="mailboxs" style='height: 100%;display: table;width: 100%;'><div id='mailboxlist' class="box" style="height:100%">
        <div id='ComposeLabel' class="compose">Compose</div>
        <div id='NewLabel' class="boxlabel">New</div>
        <div id='InboxLabel' class="boxlabel">Inbox</div>
        <div id='SentLabel' class="boxlabel">Sent</div>
        <div id='TrashLabel' class="boxlabel">Trash</div>
        
        <input class="inputbox" placeholder="Search..." type='text' id="SearchBox" style="margin-top:445px;width:100%;"></input>
        </div>
        </td>
        <td id="MailboxPane" style='width:75%' class="messagetable">
            <table  style='width:100%;height:100%' class="innertable">
                <tr id="messagelist" style='height:20%;' class="messagelist" ><td class="messagelist"><div class="box" style="">
                <div style="height:8em;overflow-x: hidden;overflow-y: auto;">
                    <ul id="MailBoxList" class= 'MailBoxList' style="list-style:none;margin:0px;padding:0px;">
                    <li >
                    <div id="listfrom" class = "messageheader"></div><div id="listsubject" class = "messageheader"></div>
                    </li>
                    </ul>
                </div>
                </div></td></tr>
                <tr id="messageText" style='height:80%;'><td>
                <div class="box" style="height:100%;padding:0px"><div style="height:100%;padding:20px">
                <table id="messagedata" style='margin-bottom:1em'>
                <tr><td style="font-size:1em">From</td><td><div id="from" style=""></div></td></tr>
                    <tr><td style="font-size:1em">To</td><td><div id="to" style=""></div></td></tr>
                    <tr><td style="font-size:.6em">Sent</td><td><div id="DateSent" style="font-size:.6em"></div></td></tr>
                    <tr><td style="width:4em;font-weight:bold">Subject   </td><td><div id="subject" style="font-weight:bold"></div></td></tr>
                     </table>
                    <div id="messagebody" class="messagebody"></div>
                    <div id="replybutton">Reply</div><div id="forwardbutton">Forward</div><div id="deletebutton">Delete</div>
                   <input class="inputbox" placeholder="Enter a username" type='text'  id="ComposeMessageTo" style="width:100%;"></input>
                   <input class="inputbox" placeholder="Enter a subject" type='text' id="ComposeMessageSubject" style="width:100%;"></input>
                   <textarea class="inputbox" placeholder="Enter message" rows='20' cols='80' id="ComposeMessageBody" style="line-height:.999em;width:100%;height:73%"></textarea>
                   <div id="SendButton">Send</div>
                </div></div></td></tr>
            </table>
        </td>
    </tr>
</table>
</td></tr>
</table>
<div id='dig'></div>



</asp:Content>

