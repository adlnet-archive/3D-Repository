using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vwarDAL;

public partial class Controls_ModelGroupPermissions : System.Web.UI.UserControl
{
    public string PID { get { return Session["PID"] as String; } set { Session["PID"] = value; } }
    protected void Page_Load(object sender, EventArgs e)
    {
        PermissionsManager mgr = new PermissionsManager();
        var pid = Request.QueryString["ContentObjectID"];
        this.PID = pid;
        ddlPermission.Items.Clear();
        foreach (var item in Enum.GetNames(typeof(vwarDAL.ModelPermissionLevel)))
        {
            ddlPermission.Items.Add(item);
        }
        BindGroups(mgr);
    }

    private void BindGroups(PermissionsManager mgr)
    {
        usersWithPermissionToModel.DataSource = mgr.GetGroupsByPid(PID);
        usersWithPermissionToModel.DataBind();
    }

    public void usersWithPermissionToModel_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        PermissionsManager mgr = new PermissionsManager();
        mgr.RemoveGroupPermission(Context.User.Identity.Name, PID, e.Values[0] as String);
        BindGroups(mgr);
    }

    public void btnAddUser_Click(object sender, EventArgs args)
    {
        PermissionsManager mgr = new PermissionsManager();
        ModelPermissionLevel lvl;
        Enum.TryParse<ModelPermissionLevel>(ddlPermission.SelectedValue, out lvl);
        var result = mgr.SetModelToGroupLevel(Context.User.Identity.Name,  PID,txtGroupName.Text, lvl);
        BindGroups(mgr);
    }
}