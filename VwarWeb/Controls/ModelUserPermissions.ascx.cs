using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vwarDAL;
public partial class Controls_ModelUserPermissions : System.Web.UI.UserControl
{
    public string PID { get { return Session["PID"] as String; } set { Session["PID"] = value; } }
    protected void Page_Load(object sender, EventArgs e)
    {
        PermissionsManager mgr = new PermissionsManager();
        var pid = Request.QueryString["ContentObjectID"];
        this.PID = pid;
        if (!IsPostBack)
        {
            foreach (var item in Enum.GetNames(typeof(vwarDAL.ModelPermissionLevel)))
            {
                if (item.ToString() != "Invisible" && item.ToString() != "NotSet") 
                ddlUserPermission.Items.Add(item);
            }
        }

        BindUsers(mgr);
        usersWithPermissionToModel.RowDeleting += new GridViewDeleteEventHandler(usersWithPermissionToModel_RowDeleting);
    }

    private void BindUsers(PermissionsManager mgr)
    {
        usersWithPermissionToModel.DataSource = mgr.GetUsersWithModelPermission(PID);
        usersWithPermissionToModel.DataBind();
    }

    public void usersWithPermissionToModel_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        PermissionsManager mgr = new PermissionsManager();
        mgr.RemoveUserPermission(Context.User.Identity.Name, PID, e.Values[0] as String);
        BindUsers(mgr);
    }
    public void btnAddUser_Click(object sender, EventArgs args)
    {
        PermissionsManager mgr = new PermissionsManager();
        ModelPermissionLevel lvl;
        Enum.TryParse<ModelPermissionLevel>(ddlUserPermission.SelectedValue, out lvl);
        var result = mgr.SetModelToUserLevel(Context.User.Identity.Name, txtAddUser.Text, PID, lvl);
        BindUsers(mgr);
    }
}