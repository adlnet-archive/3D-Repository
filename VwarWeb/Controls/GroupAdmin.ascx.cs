using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vwarDAL;
public partial class Controls_GroupAdmin : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Context.User.Identity.IsAuthenticated)
        {
            throw new Exception("User not logged in");
        }
        BindGroups();
        foreach (var policy in Enum.GetValues(typeof(GroupPolicyLevel)))
        {
            ddlPermissionLevel.Items.Add(policy.ToString());
        }
    }
    public IList<String> CurrentUsers
    {
        get
        {
            IList<String> data = Session["CurrentUsers"] as IList<String>;
            if (data != null)
                return data;
            else
                return new List<String>();
        }
        set { Session["CurrentUsers"] = value; }
    }
    public void UsersPerGroup_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        vwarDAL.PermissionsManager prmManager = new vwarDAL.PermissionsManager();
        var selectedUser = CurrentUsers[e.RowIndex];
        prmManager.RemoveUserFromGroup(Context.User.Identity.Name, SelectedGroupName, selectedUser);
        prmManager.Dispose();
    }
    public void UsersPerGroup_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }

    private void BindGroups()
    {
        vwarDAL.PermissionsManager prmManager = new vwarDAL.PermissionsManager();
        CurrentUserGroups.DataSource = prmManager.GetGroupsByOwner(Context.User.Identity.Name);
        CurrentUserGroups.DataBind();
        UsersPerGroup.DataSource = new List<String>();
        UsersPerGroup.DataBind();
        prmManager.Dispose();
    }
    public void Refresh_Click(object sender, EventArgs e)
    {
        BindGroups();
    }
    public void btnSubmit_Click(object sender, EventArgs e)
    {
        PermissionsManager mgr = new PermissionsManager();
        GroupPolicyLevel lvl;
        Enum.TryParse<GroupPolicyLevel>(ddlPermissionLevel.SelectedValue, out lvl);
        var result = mgr.CreateGroup(txtGroupName.Text, Context.User.Identity.Name, txtGroupDescription.Text, lvl);
        switch (result)
        {
            case PermissionErrorCode.AlreadyExists:
                lblErrorMessage.Text = "Group Already Exists";
                break;
            case PermissionErrorCode.NotAuthorized:
                break;
            case PermissionErrorCode.DoesNotExist:
                break;
            case PermissionErrorCode.OutOfRange:
                break;
            case PermissionErrorCode.AlreadyIncluded:
                break;
            case PermissionErrorCode.NotIncluded:
                break;
            case PermissionErrorCode.Ok:
                BindGroups();
                break;
            default:
                break;
        }
        mgr.Dispose();
    }
    private String SelectedGroupName
    {
        get
        {
            return Session["SelectedGroupName"] as string;
        }
        set
        {
            Session["SelectedGroupName"] = value;
        }
    }
    public void btnAddUser_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(SelectedGroupName))
        {
            vwarDAL.PermissionsManager prmManager = new vwarDAL.PermissionsManager();
            var result = prmManager.AddUserToGroup(Context.User.Identity.Name, SelectedGroupName, txtAddUser.Text);
            switch (result)
            {
                case vwarDAL.PermissionErrorCode.AlreadyIncluded:
                case vwarDAL.PermissionErrorCode.AlreadyExists:
                    lblErrorMessage.Text = "User already exists in group";
                    break;
                case vwarDAL.PermissionErrorCode.NotAuthorized:
                    lblErrorMessage.Text = "You are not authorized to add users to this group";
                    break;
                case vwarDAL.PermissionErrorCode.DoesNotExist:
                    break;
                case vwarDAL.PermissionErrorCode.OutOfRange:
                    break;
                case vwarDAL.PermissionErrorCode.NotIncluded:
                    break;
                case vwarDAL.PermissionErrorCode.Ok:
                    SetSelectedGroupsUsers(SelectedGroupName);
                    break;
                default:
                    break;
            }
            prmManager.Dispose();
        }
        
    }

    public void CurrentUserGroups_SelectedIndexChanged(object sender, EventArgs e)
    {
        vwarDAL.PermissionsManager prmManager = new vwarDAL.PermissionsManager();
        var selected = CurrentUserGroups.SelectedIndex;
        var grpLst = CurrentUserGroups.DataSource as vwarDAL.GroupList;
        var selectedGroup = grpLst[selected];
        SelectedGroupName = selectedGroup.GroupName;
        SetSelectedGroupsUsers(SelectedGroupName);
        addUser.Enabled = true;
        prmManager.Dispose();
    }

    public void bthDeleteGroup_Click(object sender, EventArgs e)
    {
        vwarDAL.PermissionsManager prmManager = new vwarDAL.PermissionsManager();
        prmManager.DeleteGroup(Context.User.Identity.Name, bthDeleteGroup.CommandArgument);
        BindGroups();
        var script = "window.setTimeout(function(){$('#deleteGroupDialog').dialog('close');},500);";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "closedeletegroup", script, true);
        prmManager.Dispose();
    }

    public void CurrentUserGroups_rowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteGroup")
        {
            
            int index = System.Convert.ToInt16(e.CommandArgument);
            UserGroup selectedGroup = ((GroupList)CurrentUserGroups.DataSource)[index];
            vwarDAL.PermissionsManager prmManager = new vwarDAL.PermissionsManager();
           
            var script = "window.setTimeout(function(){document.ShowDeleteGroup('" + selectedGroup.GroupName + "');},500);";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "deletegroup", script,true);
            bthDeleteGroup.CommandArgument = selectedGroup.GroupName;
            bthDeleteGroup.Text = "Delete " + selectedGroup.GroupName;
            prmManager.Dispose();
        }
    }

    
    private void SetSelectedGroupsUsers(string selectedGroup)
    {
        vwarDAL.PermissionsManager prmManager = new vwarDAL.PermissionsManager();
        var data = prmManager.GetGroupsUsers(selectedGroup);
        UsersPerGroup.DataSource = data;
        CurrentUsers = data;
        UsersPerGroup.DataBind();
        prmManager.Dispose();
    }
}