using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Controls_Login : System.Web.UI.UserControl
{

    protected string _lockoutText = "Account locked! Please <a href = Contact.aspx class='Hyperlink'>Contact Us</a> to have your account unlocked.";
    protected string _notApprovedText = "Account not approved!  Please <a href = Contact.aspx class='Hyperlink'>Contact Us</a> if you have any questions.";


    protected void Page_Load(object sender, System.EventArgs e)
    {
        this.Visible = !Context.User.Identity.IsAuthenticated;


        if (!Context.User.Identity.IsAuthenticated)
        {

            Page.SetFocus(this.Login1.FindControl("UserName"));

        }

    }

    protected void Login1_LoggedIn(object sender, System.EventArgs e)
    {
        Response.Redirect(this.Login1.DestinationPageUrl);
    }

   
    public void Login_LoggingIn(object sender, LoginCancelEventArgs e)
    {

        string uName = this.Login1.UserName.Trim();

        //lookup
        MembershipUser mu = null;
        try
        {

            mu = Membership.GetUser(uName);
        }
        catch
        {

        }  

        //catch locked out users and cancel
        if ((mu != null))
        {
            if (!mu.IsApproved)
            {
                ((Literal)this.Login1.FindControl("FailureText")).Text = this._notApprovedText;
                e.Cancel = true;
            }

            if (mu.IsLockedOut)
            {

                ((Literal)this.Login1.FindControl("FailureText")).Text = this._lockoutText;
                e.Cancel = true;


            }




        }

        //determine landing page in page_load - just redirect here
        bool hasReturnUrl = Request.QueryString["ReturnUrl"] != null;
        
        bool isAdmin = Roles.IsUserInRole(uName, "Administrators");

        if (isAdmin)
        {

              //admin page
            this.Login1.DestinationPageUrl = Website.Pages.Types.AdministratorsDefault;
            

        }
        else if (hasReturnUrl)
        {

            this.Login1.DestinationPageUrl = Request.QueryString["ReturnUrl"];


        }
        else
        {
            //default page
            this.Login1.DestinationPageUrl = Website.Pages.Types.Default;

        }
        

       

    }

    protected void Page_PreRender(object sender, System.EventArgs e)
    {

        ((Image)this.Login1.FindControl("ErrorIconImage")).Visible = (((Literal)this.Login1.FindControl("FailureText")).Text.Length > 0);
    }
    
}
