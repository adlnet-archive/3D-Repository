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
public partial class Controls_ForgotPassword : Website.Pages.ControlBase
{
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
        else
        {
            //set focus to username
            UserName.Focus();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CancelButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Public/Login.aspx");
    }
}
