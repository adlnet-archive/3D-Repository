//  Copyright 2011 U.S. Department of Defense

//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at

//      http://www.apache.org/licenses/LICENSE-2.0

//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.



using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using vwarDAL;
/// <summary>
/// 
/// </summary>
public partial class Administrators_ManageUsers : Website.Pages.ControlBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            this.BindUserProfiles();
            this.BindNotApprovedUsersGridView();
            this.BindLockedOutUsersGridView();

        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void BindAllUsers()
    {
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageIndex"></param>
    private void BindUserProfiles(int pageIndex = 0)
    {
        DataTable dt = UserProfileDB.GetAllUserProfilesDataTable();
        this.UserProfilesGridView.DataSource = dt;
        UserProfilesGridView.PageIndex = pageIndex;
        this.UserProfilesGridView.DataBind();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageIndex"></param>
    private void BindNotApprovedUsersGridView(int pageIndex = 0)
    {
        DataTable dt = UserProfileDB.GetAllAspnetUsersNotApprovedDataTable();
        this.NotApprovedUsersGridView.DataSource = dt;
        NotApprovedUsersGridView.PageIndex = pageIndex;
        this.NotApprovedUsersGridView.DataBind();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageIndex"></param>
    private void BindLockedOutUsersGridView(int pageIndex = 0)
    {

        DataTable dt = UserProfileDB.GetAllAspnetUsersLockedOutDataTable();
        this.LockedOutUsersGridView.DataSource = dt;
        LockedOutUsersGridView.PageIndex = pageIndex;
        this.LockedOutUsersGridView.DataBind();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void UserProfilesGridView_PageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        BindUserProfiles(e.NewPageIndex);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void NotApprovedUsersGridView_PageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        BindNotApprovedUsersGridView(e.NewPageIndex);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void LockedOutUsersGridView_PageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        BindLockedOutUsersGridView(e.NewPageIndex);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    protected string FormatName(object name)
    {
        string rv = "";

        if (name != null && name != DBNull.Value)
        {
            if (name.ToString().Contains("|"))
            {
                rv = name.ToString().Replace("|", " ");

            }
        }

        return rv;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LockedUsersGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        var lockedUser = Membership.GetUser(e.CommandArgument.ToString());
        if (lockedUser != null && lockedUser.IsLockedOut)
        {
            lockedUser.UnlockUser();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void UserProfilesGridView_RowDeleting(object sender, EventArgs e)
    {
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void UsersGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if ("delete".Equals(e.CommandName, StringComparison.InvariantCultureIgnoreCase))
        {
            DelteUser(e.CommandArgument.ToString());

        }
        else if ("ban".Equals(e.CommandName, StringComparison.InvariantCultureIgnoreCase))
        {

            BanUser(e.CommandArgument.ToString());
        }
        BindUserProfiles();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    private static void BanUser(string userName)
    {
        var user = Membership.GetUser(userName);
        user.IsApproved = false;
        Membership.UpdateUser(user);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void NotApprovedUsersGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {

            case "ApproveUser":
                //get the membership user
                try
                {
                    MembershipUser notApprovedUser = Membership.GetUser(e.CommandArgument.ToString());

                    if (notApprovedUser != null)
                    {
                        //approve
                        notApprovedUser.IsApproved = true;

                        //TODO: uncomment
                        Website.Mail.SendRegistrationApprovalEmail(notApprovedUser.Email);

                        //update
                        Membership.UpdateUser(notApprovedUser);

                        //bind
                        this.BindNotApprovedUsersGridView();

                    }
                }
                catch
                {

                }

                break;

            case "UnlockUser":

                try
                {
                    //get user
                    MembershipUser lockedUser = Membership.GetUser(e.CommandArgument.ToString());

                    //unlock
                    bool isLocked = lockedUser.UnlockUser();

                    //TODO: uncomment
                    //Website.Mail.SendAccountUnlockedEmail(lockedUser.Email);

                    //update
                    Membership.UpdateUser(lockedUser);

                    //bind
                    this.BindLockedOutUsersGridView();
                }
                catch
                {

                }

                break;

            case "DeleteUser":

                //Delete user from the UserProfiles table by UserGuid

                try
                {
                    var userName = e.CommandArgument.ToString();

                    DelteUser(userName);

                    //bind
                    this.BindNotApprovedUsersGridView();

                }
                catch
                {

                }

                break;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    private static void DelteUser(string userName)
    {
        bool success = UserProfileDB.DeleteUserProfileByUserName(userName);
        if (success)
        {
            //remove all the user's roles
            string[] usersRoles = Roles.GetRolesForUser(userName);
            foreach (string r in usersRoles)
            {
                Roles.RemoveUserFromRole(userName, r);

            }

            //delete the membership user
            Membership.DeleteUser(userName, true);
        }
    }
}
