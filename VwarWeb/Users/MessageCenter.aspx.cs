using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Net;
using AjaxControlToolkit;
using vwarDAL;
using System.Web.Script.Serialization;
using LR;
using System.Web.Hosting;
using vwar.service.host;

public partial class Users_MessageCenter : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public class GetMessageResponse
    {
        public bool Success;
        public int NewCount;
        public int InboxCount;
        public int SentCount;
        public int TrashCount;

        public vwarDAL.MessageList messages;
        public GetMessageResponse(bool i)
        {
            Success = i;
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static GetMessageResponse GetMessages(string Mailbox)
    {
        if (Membership.GetUser() == null || !Membership.GetUser().IsApproved)
            return new GetMessageResponse(false);

        MessageManager messageMgr = new MessageManager();
        GetMessageResponse response = new GetMessageResponse(true);
        if(Mailbox != "New")
        response.messages = messageMgr.GetMailbox(Membership.GetUser().UserName,Mailbox);
        else
        response.messages = messageMgr.GetUnreadInbox(Membership.GetUser().UserName);

        response.NewCount = messageMgr.CountMessages(Membership.GetUser().UserName, "New");
        response.InboxCount = messageMgr.CountMessages(Membership.GetUser().UserName, "Inbox");
        response.SentCount = messageMgr.CountMessages(Membership.GetUser().UserName, "Sent");
        response.TrashCount = messageMgr.CountMessages(Membership.GetUser().UserName, "Trash");

        messageMgr.Dispose();
        return response;
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static GetMessageResponse SearchMessages(string term)
    {
        if (Membership.GetUser() == null || !Membership.GetUser().IsApproved)
            return new GetMessageResponse(false);

        MessageManager messageMgr = new MessageManager();
        GetMessageResponse response = new GetMessageResponse(true);
        
        response.messages = messageMgr.Search(Membership.GetUser().UserName,term);

        response.NewCount = messageMgr.CountMessages(Membership.GetUser().UserName, "New");
        response.InboxCount = messageMgr.CountMessages(Membership.GetUser().UserName, "Inbox");
        response.SentCount = messageMgr.CountMessages(Membership.GetUser().UserName, "Sent");
        response.TrashCount = messageMgr.CountMessages(Membership.GetUser().UserName, "Trash");

        messageMgr.Dispose();
        return response;
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static bool ReadMessage(string MessageID)
    {
        if (Membership.GetUser() == null || !Membership.GetUser().IsApproved)
            return false;

        MessageManager messageMgr = new MessageManager();
        
        messageMgr.ReadMessage(Int16.Parse(MessageID),Membership.GetUser().UserName);
        messageMgr.Dispose();
        return true;
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static bool SendMessage(string toName, string Subject, string Messagetext, int thread)
    {
        if (Membership.GetUser() == null || !Membership.GetUser().IsApproved)
            return false;

        MessageManager messageMgr = new MessageManager();


        Messagetext = Westwind.Web.Utilities.HtmlSanitizer.SanitizeHtml(Messagetext,null);

        Subject = Westwind.Web.Utilities.HtmlSanitizer.SanitizeHtml(Subject, null);
        Subject = System.Text.RegularExpressions.Regex.Replace(Subject, "<.*>", "");

        messageMgr.SendMessage(Membership.GetUser().UserName, toName, Subject, Messagetext, Membership.GetUser().UserName, thread);
        messageMgr.Dispose();
        return true;
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static bool DeleteMessage(int id)
    {
        if (Membership.GetUser() == null || !Membership.GetUser().IsApproved)
            return false;

        MessageManager messageMgr = new MessageManager();

        messageMgr.MoveMessage(Membership.GetUser().UserName, id, "Trash");
        messageMgr.Dispose();
        return true;
    }
}