using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using vwarDAL;
using System.Data;

public partial class Controls_ManageAdministrativeUsers : Website.Pages.ControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.BindAdminGridView();

        }

    }

    private void BindAdminGridView()
    {
        DataTable dt = UserProfileDB.GetAllAdministrativeUsers();
        this.AdminGridView.DataSource = dt;
        this.AdminGridView.DataBind();
    }

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

    protected void AdminGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //disable row of current admin user
            string un = ((HyperLink)e.Row.FindControl("EmailHyperLink")).Text.Trim();
            Button btn = (Button)e.Row.FindControl("DeleteButton");


            if(un.Equals(Context.User.Identity.Name))
            {
                btn.Enabled = false;
                e.Row.ToolTip  = "You cannont delete your own account";

            }

            if(!btn.Enabled)
            {
                btn.ToolTip = "";
            }


        }

      
    }
    protected void CreateUserWizard1_CreateUserError(object sender, CreateUserErrorEventArgs e)
    {
        MembershipCreateStatus status = e.CreateUserError;

        this.ErrorLabel.Text = status.ToString();

    }
    protected void CreateUserWizard1_CreatingUser(object sender, LoginCancelEventArgs e)
    {
       
        this.CreateUserWizard1.UserName = this.CreateUserWizard1.Email;


    }
}


