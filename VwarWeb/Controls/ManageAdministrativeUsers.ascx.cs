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
using vwarDAL;
using System.Data;
/// <summary>
/// 
/// </summary>
public partial class Controls_ManageAdministrativeUsers : Website.Pages.ControlBase
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
            this.BindAdminGridView();

        }

    }
    /// <summary>
    /// 
    /// </summary>
    private void BindAdminGridView()
    {
        DataTable dt = UserProfileDB.GetAllAdministrativeUsers();
        this.AdminGridView.DataSource = dt;
        this.AdminGridView.DataBind();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void AdminGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "DeleteUser":

                //Delete user from the UserProfiles table by UserGuid

                try
                {

                    //remove all the user's roles
                    string[] usersRoles = Roles.GetRolesForUser(e.CommandArgument.ToString());
                    foreach (string r in usersRoles)
                    {

                        Roles.RemoveUserFromRole(e.CommandArgument.ToString(), r);


                    }

                    //delete the membership user

                    Membership.DeleteUser(e.CommandArgument.ToString(), true);

                    //bind
                    this.BindAdminGridView();
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
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CreateUserWizard1_CreatedUser(object sender, EventArgs e)
    {
        //add user to administrators roles
        string uname = this.CreateUserWizard1.UserName.Trim();

        if (Membership.GetUser(uname) != null)
        {

            try
            {
                Roles.AddUserToRole(uname, "Administrators");
            }
            catch
            {


            }

        }

        Response.Redirect(Website.Pages.Types.ManageAdministrativeUsers);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void AdminGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //disable row of current admin user
            string un = ((HyperLink)e.Row.FindControl("EmailHyperLink")).Text.Trim();
            Button btn = (Button)e.Row.FindControl("DeleteButton");


            if (un.Equals(Context.User.Identity.Name))
            {
                btn.Enabled = false;
                e.Row.ToolTip = "You cannont delete your own account";

            }

            if (!btn.Enabled)
            {
                btn.ToolTip = "";
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CreateUserWizard1_CreateUserError(object sender, CreateUserErrorEventArgs e)
    {
        MembershipCreateStatus status = e.CreateUserError;

        this.ErrorLabel.Text = status.ToString();

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CreateUserWizard1_CreatingUser(object sender, LoginCancelEventArgs e)
    {

        this.CreateUserWizard1.UserName = this.CreateUserWizard1.Email;

    }
}


