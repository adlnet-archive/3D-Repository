using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Controls_ForgotPassword : Website.Pages.ControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Context.User.Identity.IsAuthenticated)
        {
            //redirect auth users to home
            Response.Redirect("~/Default.aspx");
        }
        else
        {
            //set focus to username
            UserName.Focus();
        }
    }
    protected void SubmitButton_Click(object sender, EventArgs e)
    {
            if (this.UserName.Text.Length > 0)
            {
                MembershipUser mu = Membership.GetUser(this.UserName.Text.Trim());

                if (mu != null)
                {
                    if (!mu.IsLockedOut)
                    {
                        string email = mu.Email;
                        Website.Mail.SendForgotPassword(email);
                        this.FailureText.Text = "Your password has been sent.";
                    }
                    else 
                    {
                        this.FailureText.Text = "Your account has been locked.  Please contact the site administrator.";                    
                    }
                }
                else
                {
                    this.FailureText.Text = "Invalid Username. Try again.";
                }
            }
        
    }
    protected void CancelButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Public/Login.aspx");
    }
}
