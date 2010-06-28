using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using vwarDAL;

public partial class Administrators_ManageUsers : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
      
        if (!Page.IsPostBack)
        {
            BindNotApprovedUsersGridView();
            BindLockedOutUsersGridView();

        }


    }

    private void BindNotApprovedUsersGridView()
    {
        DataTable dt = UserProfileDB.GetAllAspnetUsersNotApprovedDataTable();
        this.NotApprovedUsersGridView.DataSource = dt;
        this.NotApprovedUsersGridView.DataBind();
    }

    private void BindLockedOutUsersGridView()
    {

        DataTable dt = UserProfileDB.GetAllAspnetUsersLockedOutDataTable();
        this.LockedOutUsersGridView.DataSource = dt;
        this.LockedOutUsersGridView.DataBind();

    }





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
                            // Website.Mail.SendRegistrationApprovalEmail(notApprovedUser.Email);

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
                    bool success = UserProfileDB.DeleteUserProfileByUserName(e.CommandArgument.ToString());

                    if (success)
                    {
                        //remove all the user's roles
                        string[] usersRoles = Roles.GetRolesForUser(e.CommandArgument.ToString());
                        foreach (string r in usersRoles)
                        {
                            Roles.RemoveUserFromRole(e.CommandArgument.ToString(), r);

                        }

                        //delete the membership user
                        Membership.DeleteUser(e.CommandArgument.ToString(), true);

                    }

                    //bind
                    this.BindNotApprovedUsersGridView();

                }
                catch
                {
                    
                   
                }
              
                break;



        }
        
    }
}