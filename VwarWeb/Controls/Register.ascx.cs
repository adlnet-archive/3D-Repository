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
using System.IO;
using DMGForums.Global;
/// <summary>
/// 
/// </summary>
public partial class Controls_Register : Website.Pages.ControlBase
{
    /// <summary>
    /// 
    /// </summary>
    private Random random = new Random();
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// 
    /// </summary>
    private string CaptchaCode
    {
        //stores captcha code
        get { return Session["CaptchaCode"].ToString().Trim(); }
        set { Session["CaptchaCode"] = value; }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SwitchToOpenId(object sender, EventArgs e)
    {
        MultiView1.SetActiveView(OpenIdCreationView);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CreateUserWizardStep1_CreatingUser(object sender, EventArgs e)
    {
        //set username to email address
        CreateUserWizard1.UserName = this.CreateUserWizard1.Email.Trim();

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CreateUserWizardStep1_CreatedUser(object sender, EventArgs e)
    {
        // Create an empty Profile for the newly created user       
        //ProfileCommon p = (ProfileCommon)ProfileCommon.Create(CreateUserWizard1.UserName, true);

        // Populate some Profile properties off of the create user wizard
        //p.FirstName = ((TextBox)CreateUserWizard1.CreateUserStep.Controls[0].FindControl("FirstName")).Text;
        //p.LastName = ((TextBox)CreateUserWizard1.CreateUserStep.Controls[0].FindControl("LastName")).Text;


        //load new membership user and set 'Comment' property to {FirstName}|{LastName}

        MembershipUser mu = String.IsNullOrEmpty(CreateUserWizard1.UserName) ? Membership.GetUser(CreateOpenIDWizard.UserName) : Membership.GetUser(CreateUserWizard1.UserName.Trim());

        //approved = false by default

        if (mu != null)
        {

            //set approved 
            mu.IsApproved = Website.Config.MembershipUserApprovedByDefault;
            string targetRole = "Users";
            if (!Roles.IsUserInRole(mu.UserName, targetRole))
            {
                Roles.AddUserToRole(mu.UserName, targetRole);
            }

            //create user profile

            UserProfile p = null;

            string memGUID = mu.ProviderUserKey.ToString().Trim();
            string fName = ((TextBox)CreateUserWizard1.CreateUserStep.Controls[0].FindControl("FirstName")).Text;
            string lName = ((TextBox)CreateUserWizard1.CreateUserStep.Controls[0].FindControl("LastName")).Text;
            string email = ((TextBox)CreateUserWizard1.CreateUserStep.Controls[0].FindControl("Email")).Text;

            try
            {
                p = UserProfileDB.InsertUserProfile(memGUID, fName, lName, email, email, email);
                mu.Comment = p.FirstName.Trim() + "|" + p.LastName.Trim();

                Membership.UpdateUser(mu);
                Database.ConnString = Website.Config.PostgreSQLConnectionString;
                Database.DBType = "MySQL";
                Database.DBPrefix = "DMG";
                email = Functions.RepairString(email);
                string password = Functions.Encrypt(Functions.RepairString(CreateUserWizard1.Password));
                PermissionsManager mgr = new PermissionsManager();
                mgr.AddUserToGroup(System.Configuration.ConfigurationManager.AppSettings["DefaultAdminName"], vwarDAL.DefaultGroups.AllUsers, mu.UserName);
                if (Website.Config.SendEmailForNewRegistrations)
                {
                    Website.Mail.SendNewRegistrationNotificationEmail(mu);

                }
                Database.Write("INSERT INTO " + Database.DBPrefix + "_MEMBERS (MEMBER_USERNAME, MEMBER_PASSWORD, MEMBER_LEVEL, MEMBER_EMAIL, MEMBER_LOCATION, MEMBER_HOMEPAGE, MEMBER_SIGNATURE, MEMBER_SIGNATURE_SHOW, MEMBER_IM_AOL, MEMBER_IM_ICQ, MEMBER_IM_MSN, MEMBER_IM_YAHOO, MEMBER_POSTS, MEMBER_DATE_JOINED, MEMBER_DATE_LASTVISIT, MEMBER_TITLE, MEMBER_TITLE_ALLOWCUSTOM, MEMBER_TITLE_USECUSTOM, MEMBER_EMAIL_SHOW, MEMBER_IP_LAST, MEMBER_IP_ORIGINAL, MEMBER_REALNAME, MEMBER_OCCUPATION, MEMBER_SEX, MEMBER_AGE, MEMBER_BIRTHDAY, MEMBER_NOTES, MEMBER_FAVORITESITE, MEMBER_PHOTO, MEMBER_AVATAR, MEMBER_AVATAR_SHOW, MEMBER_AVATAR_ALLOWCUSTOM, MEMBER_AVATAR_USECUSTOM, MEMBER_AVATAR_CUSTOMLOADED, MEMBER_AVATAR_CUSTOMTYPE, MEMBER_VALIDATED, MEMBER_VALIDATION_STRING, MEMBER_RANKING) VALUES ('" + email + "','" + password + "', " + 1 + ", '" + email + "', ' ', ' ', ' ', 0, '', '', '', '', 0, " + Database.GetTimeStamp() + ", " + Database.GetTimeStamp() + ", '', 0, 0, 0, '', '', '', '', '', '', '', '', '', '', 1, 0 , 0, 0, 0, 'jpg', 1, '', 0)");
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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CreateOpenIdUserWizardStep1_CreateUserError(object sender, EventArgs e)
    {
        //show RegisterView to see error
        MultiView1.SetActiveView(OpenIdCreationView);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    protected void ValidateAgreementsCheckbox(object sender, ServerValidateEventArgs args)
    {
        CustomValidator validator = (CustomValidator)sender;
        args.IsValid = ((CheckBox)validator.Parent.FindControl("TermsOfServiceCheckbox")).Checked;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CreateUserWizardStep1_CreateUserError(object sender, EventArgs e)
    {
        //show RegisterView to see error
        MultiView1.SetActiveView(RegisterView);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ErrorContinueButton_Click(object sender, EventArgs e)
    {
        //redirect to create a new captcha
        Response.Redirect(Request.RawUrl);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ConfirmationViewContinueButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(Website.Pages.Types.Default);
    }
}
