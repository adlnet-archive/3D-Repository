﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using vwarDAL;

/// <summary>
/// Summary description for Permissions
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class Permissions : System.Web.Services.WebService {


    private Dictionary<string, ModelPermissionLevel> _tempUserPermissions
    {
        get
        {
            if (HttpContext.Current.Session["UserPermissions"] == null)
                HttpContext.Current.Session["UserPermissions"] = new Dictionary<string, ModelPermissionLevel>();

            return (Dictionary<string, ModelPermissionLevel>)HttpContext.Current.Session["UserPermissions"];
        }
    }

    private Dictionary<string, ModelPermissionLevel> _tempGroupPermissions
    {
        get
        {
            if (HttpContext.Current.Session["GroupPermissions"] == null)
                HttpContext.Current.Session["GroupPermissions"] = new Dictionary<string, ModelPermissionLevel>();

            return (Dictionary<string, ModelPermissionLevel>)HttpContext.Current.Session["GroupPermissions"];
        }
    }

    public Dictionary<string, ModelPermissionLevel> TempUserPermissions
    {
        get { return _tempUserPermissions; }
    }

    public Dictionary<string, ModelPermissionLevel> TempGroupPermissions
    {
        get { return _tempGroupPermissions; }
    }

    public Permissions () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }


    [WebMethod(EnableSession=true)]
    [ScriptMethod]
    public bool RemoveUserPermission(string pid, string username, bool temp)
    {
        PermissionsManager permissionsManager = new PermissionsManager();
        pid = HttpContext.Current.Server.UrlDecode(pid);

        if (Context.User.Identity.IsAuthenticated)
        {
            if (Website.Common.IsValidUser(username))
            {

                PermissionErrorCode error = PermissionErrorCode.Ok;

                if (temp)
                    _tempUserPermissions.Remove(username);
                else
                    error = permissionsManager.RemoveUserPermission(HttpContext.Current.User.Identity.Name, pid, username);

                switch (error)
                {
                    case PermissionErrorCode.Ok:
                        HttpContext.Current.Response.StatusCode = 200;
                        break;
                    case PermissionErrorCode.NotAuthorized:
                        HttpContext.Current.Response.StatusCode = 401;
                        break;
                    default:
                        HttpContext.Current.Response.StatusCode = 400;
                        break;
                }

                return true;
            }
            else
            {
                HttpContext.Current.Response.StatusCode = 400;
                return false;
            }
        }
        else
        {
            HttpContext.Current.Response.StatusCode = 401;
            return false;
        }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public string SavePermissions(string type, string pid, List<string> targets, List<string> permissions, bool temp)
    {
        PermissionsManager permissionsManager = new PermissionsManager();

        HttpContext context = HttpContext.Current;
        string identity = context.User.Identity.Name;
        pid = context.Server.UrlDecode(pid);

        if (targets.Count == permissions.Count
            && (type == "user" || type == "group"))
        {
            for (int i = 0; i < targets.Count; i++)
            {
                ModelPermissionLevel perm = (ModelPermissionLevel)(Int32.Parse(permissions[i]));
                if (type == "group")
                {
                    //Transform any end-user-view groupnames into codebehind groupnames
                    if (targets[i] ==  PermissionsManager.ALL_USERS_LABEL)
                        targets[i] = DefaultGroups.AllUsers;
                    else if (targets[i] == PermissionsManager.ANONYMOUS_USERS_LABEL)
                        targets[i] = DefaultGroups.AnonymousUsers;
                }

                PermissionErrorCode errorCode = PermissionErrorCode.Ok;

                if (type == "user" && !Website.Common.IsValidUser(targets[i]))
                    errorCode = PermissionErrorCode.DoesNotExist;
                else
                {
                    if (temp)
                    {
                        if (type == "group")
                            _tempGroupPermissions[targets[i]] = perm;
                        else
                            _tempUserPermissions[targets[i]] = perm;
                    }
                    else
                    {
                        errorCode = (type == "group")
                                ? permissionsManager.SetModelToGroupLevel(identity, pid, targets[i], perm)
                                : permissionsManager.SetModelToUserLevel(identity, pid, targets[i], perm);
                    }
                }

                switch (errorCode)
                {
                    case PermissionErrorCode.Ok:
                        context.Response.StatusCode = 200;
                        break;
                    case PermissionErrorCode.NotAuthorized:
                        context.Response.StatusCode = 401;
                        break;
                    default:
                        context.Response.StatusCode = 400;
                        break;
                }

                if (errorCode != PermissionErrorCode.Ok)
                    break;
            }
        }
        else
            context.Response.StatusCode = 400;

        //TODO: Add more specific error messages
        return (context.Response.StatusCode == 200) ? "success" : "failure";
    }

    [WebMethod(EnableSession=true)]
    [ScriptMethod]
    public bool CheckUser(string username)
    {
        return Website.Common.IsValidUser(username);
    }

    public void SaveTempPermissions(string pid)
    {
        // You would call this from SumbitUpload in Upload.aspx.cs and it 
        // would save the temp permissions now that the PID exists
    }

}
