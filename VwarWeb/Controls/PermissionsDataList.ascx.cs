using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vwarDAL;

public partial class Controls_PermissionsDataList : System.Web.UI.UserControl
{
    public string ControlType
    {
        get;
        set;
    }



    private enum PermissionsControlType { DEFAULT_GROUP, GROUP, USER }

    private string _pid
    {
        get { 
                return (HttpContext.Current.Request.Params["pid"] == null)
                    ? HttpContext.Current.Request.Params["ContentObjectId"]
                    : HttpContext.Current.Request.Params["pid"];
            }
    }

    private PermissionsControlType _type;

    private PermissionsManager _permissionsManager;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            _permissionsManager = new PermissionsManager();
            _type = BindDataList();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Control ptable = PermissionsListView.FindControl("PermissionsTable");
            
            string title = string.Empty;
            if(_type == PermissionsControlType.DEFAULT_GROUP)
                title = "Default Groups";
            else if (_type == PermissionsControlType.GROUP)
                title = "My Groups";
            else
                title = "Special Users";

            if(ptable != null)
                ((Label)ptable.Controls[0].FindControl("PermissionsTableTitle")).Text = title;
        }
    }

    private void BindAddList(EventArgs e)
    {
        Control addColumn = PermissionsListView.FindControl("PermissionsTable")
                                                        .FindControl("AddColumn");

        var usrAdd = (TextBox)addColumn.FindControl("AddUserTextBox");
        var grpAdd = (DropDownList)addColumn.FindControl("AddGroupDropdown");


        if (_type == PermissionsControlType.GROUP)
        {
            addColumn.Controls.Remove(usrAdd);
            grpAdd.Items.Add(new ListItem("Select Available...", "-1"));
            foreach (UserGroup grp in _permissionsManager.GetUsersGroups(Context.User.Identity.Name))
                grpAdd.Items.Add(grp.GroupName);
        }
        else
            addColumn.Controls.Remove(grpAdd);
    }

 

    public void BindSelectedPermission(object sender, ListViewItemEventArgs e)
    {
        KeyValuePair<string, ModelPermissionLevel> item = (KeyValuePair<string, ModelPermissionLevel>)e.Item.DataItem;
        
        string grpName = item.Key;
        ModelPermissionLevel lvl = item.Value;

        // To the user, these are equivalent in terms of access
        if (lvl == ModelPermissionLevel.NotSet)
            lvl = _permissionsManager.CheckGroupPermissions(_permissionsManager.GetUserGroup(DefaultGroups.AllUsers), _pid);

        var row = e.Item.FindControl("DataRow");

        if (grpName == DefaultGroups.AllUsers)
            ((System.Web.UI.HtmlControls.HtmlTableCell)row.Controls[0]).InnerText = PermissionsManager.ALL_USERS_LABEL;
        else if (grpName == DefaultGroups.AnonymousUsers)
            ((System.Web.UI.HtmlControls.HtmlTableCell)row.Controls[0]).InnerText = PermissionsManager.ANONYMOUS_USERS_LABEL;

        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            DropDownList dd = (DropDownList)row.FindControl("DropdownColumn")
                                                    .FindControl("PermissionsDropdownList");

            foreach(ListItem li in dd.Items)
            {
                if (Int32.Parse(li.Value) == (int)lvl)
                {
                    li.Selected = true;
                    break;
                }
            }        
        }
    }

    private PermissionsControlType BindDataList()
    {
        PermissionsControlType type;

        ControlType = ControlType.ToLowerInvariant();
        if (ControlType.Contains("group"))
        {
            GroupList allGroups = _permissionsManager.GetUsersGroups(Context.User.Identity.Name);
            Func<GroupList, List<UserGroup>> filterFunc;
            if (ControlType == "defaultgroup")
            {
                filterFunc = GetDefaultGroups;
                type = PermissionsControlType.DEFAULT_GROUP;
            }
            else
            {
                filterFunc = GetExistingNonDefaultGroups;
                type = PermissionsControlType.GROUP;
            }

            PermissionsListView.DataSource =
                GetPermissionLevelsForGroups(filterFunc(allGroups));
                 
                
        }
        else if (ControlType.Contains("user"))
        {
            PermissionsListView.DataSource = GetPermissionLevelsForUsers();
            type = PermissionsControlType.USER;
        }
        else
            throw new Exception("Invalid ControlType specified");

        PermissionsListView.DataBind();

        return type;
    }

    private List<UserGroup> GetAddNewGroups(GroupList userGroups)
    { 
        return userGroups.Except(GetExistingNonDefaultGroups(userGroups)).Except(GetDefaultGroups(userGroups))
                         .Where(group => group.PolicyLevel >= GroupPolicyLevel.UsersAdd)
                         .ToList<UserGroup>();
    }

    private List<UserGroup> GetExistingNonDefaultGroups(GroupList userGroups)
    {
        return userGroups.Except(GetDefaultGroups(userGroups))
                         .ToList<UserGroup>();
    }

    private List<UserGroup> GetDefaultGroups(GroupList userGroups)
    {

        var ug = userGroups.Where(group => group.GroupName == DefaultGroups.AllUsers)
                           .ToList<UserGroup>();
                           
        ug.Add(_permissionsManager.GetUserGroup(DefaultGroups.AnonymousUsers));
        return ug;
    }

    private Dictionary<string, ModelPermissionLevel> GetPermissionLevelsForGroups(List<UserGroup> groups)
    {
        Dictionary<string, ModelPermissionLevel> pairs = new Dictionary<string, ModelPermissionLevel>();

        foreach (UserGroup grp in groups)
            pairs[grp.GroupName] = _permissionsManager.CheckGroupPermissions(grp, _pid);

        return pairs;
    }

    private Dictionary<string, ModelPermissionLevel> GetPermissionLevelsForUsers()
    {
        IList<PermissionDescription> description = _permissionsManager.GetUsersWithModelPermission(_pid);
        Dictionary<string, ModelPermissionLevel> pairs = new Dictionary<string, ModelPermissionLevel>();

        foreach (PermissionDescription d in description)
            pairs.Add(d.Name, d.PermissionLevel);

        return pairs;
    }

}