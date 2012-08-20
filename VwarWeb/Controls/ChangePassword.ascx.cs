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

/// <summary>
/// 
/// </summary>
public partial class Controls_ChangePassword : Website.Pages.ControlBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {

        changeForm.Visible = false;
        initialEmail.Visible = false;
        errorLink.Visible = false;

        if(!handleTokenCheck())
        {
            UserName.Focus();
            initialEmail.Visible = true;
        }
    }

    protected bool handleTokenCheck(bool handleChangeForm = true)
    {
        if (Context.Request.QueryString["email"] != null && Context.Request.QueryString["t"] != null)
        {
            TokenValidator tokenMaker = new TokenValidator(Context.Request.QueryString["email"], Context.Request.QueryString["t"]);

            if (tokenMaker.ValidateUserToken())
            {
                NewPassword.Focus();
                changeForm.Visible = handleChangeForm;
                return true;
            }

            else errorLink.Visible = true;
        }

       return false;
    }

    protected void ChangePasswordPushButton_Click(object sender, EventArgs e)
    {
        errorLink.Visible = true;

        if (handleTokenCheck(false) && ConfirmNewPassword.Text == NewPassword.Text && ConfirmNewPassword.Text.Length >= 6)
        {
            
            MembershipUser mu = Membership.GetUser(Context.Request.QueryString["email"].Trim(), false);
            

            if (mu != null)
            {

                if (!mu.IsLockedOut)
                {
                    string temp = mu.ResetPassword();
                    if(mu.ChangePassword(temp, NewPassword.Text))
                        corruptedText.Text = "Your password has been changed. You may now log in.";

                    else corruptedText.Text = "ERROR!!!";
                    
                }

                else corruptedText.Text = "Your account has been locked.  Please contact the site administrator.";
                
            }

            else corruptedText.Text = "Invalid Username. Try again.";
        }

        else
        {
            corruptedText.Text = "There was an error in changing your password. Please make " +
                                 "sure they match and are both at least 6 characters long. Contact us if you continue to have difficulty.";

            NewPassword.Focus();
            changeForm.Visible = true;
        }
    }

    protected void SubmitButton_Click(object sender, EventArgs e)
    {
            MembershipUser mu = Membership.GetUser(UserName.Text.Trim(), false);
            

            if (mu != null)
            {
                if (!mu.IsLockedOut)
                {
                    string email = mu.Email;
                    TokenValidator tokenMaker = new TokenValidator(email);
                    tokenMaker.generateTokenEmail();

                    this.EmailFailure.Text =  "Instructions have been sent.";
                }
                else
                {
                    this.EmailFailure.Text = "Your account has been locked.  Please contact the site administrator.";
                }
            }
            else
            {
                this.EmailFailure.Text = "Invalid Username. Try again.";
            }
    }
}
