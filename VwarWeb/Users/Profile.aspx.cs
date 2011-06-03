using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

public partial class Users_Profile : Website.Pages.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SearchPanel.Visible = false;
        this.MyModels1.Visible = !Website.Security.IsAdministrator();
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static Hashtable RequestKey(string Description)
    {
        System.Collections.Hashtable ht = new System.Collections.Hashtable();
        HttpContext context = HttpContext.Current;
        if (context.User.Identity.IsAuthenticated)
        {
            try
            {
                vwar.service.host.APIKeyManager keyMan = new vwar.service.host.APIKeyManager();
                vwar.service.host.APIKey key = keyMan.CreateKey(context.User.Identity.Name, context.Server.HtmlEncode(Description));
                ht["Key"] = key.Key;
                ht["Usage"] = key.Usage;
                ht["Active"] = (key.State == vwar.service.host.APIKeyState.ACTIVE) ? "Yes" : "No";
                ht["Message"] = System.Configuration.ConfigurationManager.AppSettings["ProfilePage_KeyRequestSuccess"];
            }
            catch
            {
                ht["Message"] = System.Configuration.ConfigurationManager.AppSettings["ProfilePage_KeyRequestError"];
            }
        }
        else
        {
            ht["Message"] = System.Configuration.ConfigurationManager.AppSettings["ProfilePage_KeyRequestError"];
        }
        
        return ht;
     
    }
    
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static string DeleteKey(string Key)
    {
        string msg = "";
        HttpContext context = HttpContext.Current;
        if (context.User.Identity.IsAuthenticated)
        {
            try
            {
                vwar.service.host.APIKeyManager keyMan = new vwar.service.host.APIKeyManager();
                if (keyMan.GetUserByKey(Key).Equals(context.User.Identity.Name))
                {
                    keyMan.DeleteKey(Key);
                    msg = System.Configuration.ConfigurationManager.AppSettings["ProfilePage_KeyDeleteSuccess"];
                }
                else
                {
                    msg = System.Configuration.ConfigurationManager.AppSettings["ProfilePage_KeyDeleteError"];
                }
            }
            catch
            {
                msg = System.Configuration.ConfigurationManager.AppSettings["ProfilePage_KeyDeleteError"];
            }

        }
        else
        {
            context.Response.StatusCode = 401;
        }

        return msg;
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static Hashtable UpdateKey(string Key, string Description)
    {
        System.Collections.Hashtable ht = new System.Collections.Hashtable();
        HttpContext context = HttpContext.Current;
        if (context.User.Identity.IsAuthenticated)
        {
            try
            {
                vwar.service.host.APIKeyManager keyMan = new vwar.service.host.APIKeyManager();
                if (keyMan.GetUserByKey(Key).Equals(context.User.Identity.Name))
                {
                    vwar.service.host.APIKey currentKey = keyMan.GetKeyByKey(Key);
                    currentKey.Usage = context.Server.HtmlEncode(Description);
                    if (keyMan.UpdateKey(currentKey))
                    {
                        ht["Message"] = System.Configuration.ConfigurationManager.AppSettings["ProfilePage_KeyUpdateSuccess"];
                        ht["Usage"] = currentKey.Usage;
                    }
                    else
                    {
                        ht["Message"] = System.Configuration.ConfigurationManager.AppSettings["ProfilePage_KeyUpdateError"];
                    }
                }
                else
                {
                    ht["Message"] = System.Configuration.ConfigurationManager.AppSettings["ProfilePage_KeyUpdateError"];
                }
            }
            catch
            {
                ht["Message"] = System.Configuration.ConfigurationManager.AppSettings["ProfilePage_KeyUpdateError"];
            }

        }
        else
        {
            context.Response.StatusCode = 401;
        }

        return ht;

    }
}


