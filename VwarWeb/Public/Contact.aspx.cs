using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vwarDAL;

public partial class Public_Contact : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindContactUs();
        }

    }

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
    protected void CancelButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(Website.Pages.Types.Default);
    }
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