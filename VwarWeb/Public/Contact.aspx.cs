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
using vwarDAL;
/// <summary>
/// 
/// </summary>
public partial class Public_Contact : System.Web.UI.Page
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindContactUs();
        }

    }
    /// <summary>
    /// 
    /// </summary>
    private void BindContactUs()
    {
        if (Context.User.Identity.IsAuthenticated)
        {
            UserProfile p = null;
            try
            {
                p = UserProfileDB.GetUserProfileByUserName(Context.User.Identity.Name.Trim());
            }

            catch
            {


            }

            if (p != null)
            {
                //bind to profile
                this.FirstNameTextBox.Text = p.FirstName.Trim();
                this.LastNameTextBox.Text = p.LastName.Trim();
                this.PhoneNumberTextBox.Text = p.Phone.Trim();
                this.EmailTextBox.Text = p.Email.Trim();
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
        Response.Redirect(Website.Pages.Types.Default);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        string fname = this.FirstNameTextBox.Text.Trim();
        string lname = this.LastNameTextBox.Text.Trim();
        string email = this.EmailTextBox.Text.Trim();
        string phone = this.PhoneNumberTextBox.Text.Trim();
        string questionRelatesTo = "";
        string question = this.QuestionsTextBox.Text.Trim();

        //question relates to
        if (this.MyQuestionRelatesToDropDownList.SelectedItem != null)
        {
            questionRelatesTo = this.MyQuestionRelatesToDropDownList.SelectedValue.Trim();
        }


        //send contact us mail to cybrarian
        //Website.Mail.SendContactUsEmail(fname, lname, email, phone, questionRelatesTo, question);
        ScriptManager.RegisterStartupScript(this, typeof(Page), "fadecontactblock", "$('#FormFields').fadeOut(400, function() { $('#Status').fadeIn('fast') });", true);
        StatusLabel.Visible = true;
        StatusLabel.Text = "Your message has been successfully sent, and someone will be contacting you shortly. Thank you for your inquiry!<br/><br/><a href='../Default.aspx'>Back to Home Page</a>";
    }
}
