using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using vwarDAL;
using System.IO;

public partial class Controls_Register : Website.Pages.ControlBase
{
    private Random random = new Random();

    private string GenerateRandomCode()
    {
        //create a new random number to be used as a captcha
        string s = "";
        for (int i = 0; i <= 5; i++)
        {
            s = String.Concat(s, this.random.Next(10).ToString());
        }
        return s;
    }

    private string CaptchaCode
    {
        //stores captcha code
        get { return Session["CaptchaCode"].ToString().Trim(); }
        set { Session["CaptchaCode"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Context.User.Identity.IsAuthenticated)
        {
            //redirect auth users to home
            Response.Redirect("~/Default.aspx");
        }

        if (!Page.IsPostBack)
        {
            //show captcha view and initialize captcha code
            MultiView1.ActiveViewIndex = 0;
            CaptchaCode = GenerateRandomCode();
            CaptchaCodeTextBox.Focus();

            //set reload link to page url
            ReloadHyperLink.NavigateUrl = Request.RawUrl;
        }
    }

    protected void CreateUserWizardStep1_CreatingUser(object sender, EventArgs e)
    {
        //set username to email address
        CreateUserWizard1.UserName = this.CreateUserWizard1.Email.Trim();


      
     


    }

    protected void CreateUserWizardStep1_CreatedUser(object sender, EventArgs e)
    {
        // Create an empty Profile for the newly created user       
        //ProfileCommon p = (ProfileCommon)ProfileCommon.Create(CreateUserWizard1.UserName, true);

        // Populate some Profile properties off of the create user wizard
        //p.FirstName = ((TextBox)CreateUserWizard1.CreateUserStep.Controls[0].FindControl("FirstName")).Text;
        //p.LastName = ((TextBox)CreateUserWizard1.CreateUserStep.Controls[0].FindControl("LastName")).Text;


        //load new membership user and set 'Comment' property to {FirstName}|{LastName}
               
        MembershipUser mu = Membership.GetUser(CreateUserWizard1.UserName.Trim());
        
        //approved = false by default



        if (mu != null)
        {

            //set approved 
            mu.IsApproved = Website.Config.MembershipUserApprovedByDefault;

            Roles.AddUserToRole(mu.UserName, "Users");

            //create user profile
            
            UserProfile p = null;

            string memGUID = mu.ProviderUserKey.ToString().Trim();
            string fName = ((TextBox)CreateUserWizard1.CreateUserStep.Controls[0].FindControl("FirstName")).Text;
            string lName = ((TextBox)CreateUserWizard1.CreateUserStep.Controls[0].FindControl("LastName")).Text;            
            string email = ((TextBox)CreateUserWizard1.CreateUserStep.Controls[0].FindControl("Email")).Text;

            try
            {
                p = UserProfileDB.InsertUserProfile(memGUID, fName, lName, email, email,email);
                mu.Comment = p.FirstName.Trim() + "|" + p.LastName.Trim();
                Membership.UpdateUser(mu);

            }
            catch
            {
                
               
            }
          
           
        }

        // Save profile - must be done since we explicitly created it 
        // p.Save();

        if (Website.Config.MembershipUserApprovedByDefault == false)
        {
            //not approved
            FormsAuthentication.SignOut();
            MultiView1.SetActiveView(ConfirmationView);

        }
        else
        {
            //approved
            FormsAuthentication.RedirectFromLoginPage(CreateUserWizard1.UserName, false);
            Website.Mail.SendRegistrationApprovalEmail(mu.Email);
        }
        
        


       
    }

    protected void CreateUserWizardStep1_CreateUserError(object sender, EventArgs e)
    {
        //show RegisterView to see error
        MultiView1.SetActiveView(RegisterView);
    }
    protected void CaptchaSubmitButton_Click(object sender, EventArgs e)
    {
        //verify correct captch is entered
        if (CaptchaCodeTextBox.Text.Trim().Equals(CaptchaCode))
        {
            MultiView1.SetActiveView(RegisterView);
            CreateUserWizard1.CreateUserStep.Controls[0].FindControl("FirstName").Focus();
        }
        else
        {
            MultiView1.SetActiveView(ErrorView);
            ErrorContinueButton.Focus();
        }
    }

    protected void ErrorContinueButton_Click(object sender, EventArgs e)
    {
        //redirect to create a new captcha
        Response.Redirect(Request.RawUrl);
    }









    protected void ConfirmationViewContinueButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(Website.Pages.Types.Default);
    }
}
