using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Controls_Profile : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            MultiView1.ActiveViewIndex = 0;
        }
    }
    protected void ChangePasswordLinkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Users/ChangePassword.aspx");
    }
    protected void EditProfileLinkButton_Click(object sender, EventArgs e)
    {
        //show edit view
        MultiView1.SetActiveView(EditProfileView);

        if (Profile != null && !String.IsNullOrEmpty(Profile.FirstName))
        {
            FirstNameTextBox.Text = Profile.FirstName.Trim();
        }
        if (Profile != null && !String.IsNullOrEmpty(Profile.LastName))
        {
            LastNameTextBox.Text = Profile.LastName.Trim();
        }

        ////bind values to tb's (if exists)
        //ProfileCommon p = null;
        //try
        //{
        //    p = Profile.GetProfile(Membership.GetUser().Email);
        //}
        //catch (Exception ex)
        //{ 
        
        //}

        //if (p != null)
        //{ 
        //    //first name
        //    if (!String.IsNullOrEmpty(Profile.FirstName.Trim()))
        //    {
        //        FirstNameTextBox.Text = Profile.FirstName.Trim();
        //    }

        //    //last name
        //    if (!String.IsNullOrEmpty(Profile.LastName.Trim()))
        //    {
        //        LastNameTextBox.Text = Profile.LastName.Trim();
        //    }

        //}

    }
    protected void CancelButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }
    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        //update profile
        if (!String.IsNullOrEmpty(FirstNameTextBox.Text.Trim()) && !String.IsNullOrEmpty(LastNameTextBox.Text.Trim()))
        {
            //update profile
            Profile.FirstName = FirstNameTextBox.Text.Trim();
            Profile.LastName = LastNameTextBox.Text.Trim();

            //save to MembershipUser.Comment with pipe delimiter
            MembershipUser mu = Membership.GetUser();
            mu.Comment = Profile.FirstName + "|" + Profile.LastName;
            Membership.UpdateUser(mu);

            //show confirmation
            MultiView1.SetActiveView(ConfirmationView);
            ConfirmationLabel.Text = "Your profile has been successfully updated.";
        }
        else
        {
            //show error msg
            MultiView1.SetActiveView(ErrorView);
            ErrorLabel.Text = "Please enter a valid 'First Name' and 'Last Name'.";
        }
    }
    protected void ContinueButton_Click(object sender, EventArgs e)
    {
        //send back to edit profile view
        MultiView1.SetActiveView(EditProfileView);
    }
    protected void ConfirmationButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }
}
