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
/// <summary>
/// 
/// </summary>
public partial class Controls_Login : Website.Pages.ControlBase
{
    /// <summary>
    /// 
    /// </summary>
    protected string _lockoutText = "Account locked! Please <a href = Contact.aspx class='Hyperlink'>Contact Us</a> to have your account unlocked.";
    /// <summary>
    /// 
    /// </summary>
    protected string _notApprovedText = "Account not approved!  Please <a href = Contact.aspx class='Hyperlink'>Contact Us</a> if you have any questions.";
    protected string errorText = "";
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, System.EventArgs e)
    {
        this.Visible = !Context.User.Identity.IsAuthenticated;
       
        if (!Context.User.Identity.IsAuthenticated)
        {

            Page.SetFocus(this.Login1.FindControl("UserName"));

        }

        if (Page.Request.UrlReferrer != null)
        {
            this.Login1.DestinationPageUrl = Page.Request.UrlReferrer.AbsoluteUri;
        }

        errorLink.Visible = false;

        if(Session["change"] != null)
            errorLink.Visible = true;

        Session.Clear();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Login1_LoggedIn(object sender, System.EventArgs e)
    {
        Response.Redirect(this.Login1.DestinationPageUrl);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

        if (hasReturnUrl)
        {

            this.Login1.DestinationPageUrl = Request.QueryString["ReturnUrl"];


        }
        else if (isAdmin)
        {

            //admin page
            this.Login1.DestinationPageUrl = Website.Pages.Types.AdministratorsDefault;


        }
        else
        {
            //default page
            this.Login1.DestinationPageUrl = Website.Pages.Types.Default;

        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRender(object sender, System.EventArgs e)
    {

        ((Image)this.Login1.FindControl("ErrorIconImage")).Visible = (((Literal)this.Login1.FindControl("FailureText")).Text.Length > 0);
    }
}
