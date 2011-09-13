using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Threading;
using System.Net.Mail;
using CaptchaDLL;

public partial class About : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        RotatorDataList.DataSource = GetRotatorSlides();
        RotatorDataList.DataBind();
        if (Context.User.Identity.IsAuthenticated)
        {
            this.EmailAddress.Enabled = false;
            this.Captcha.Visible = false;
            this.EmailValidator.Enabled = false;
        }
        else
        {
            if (!Page.IsPostBack)
            {
                Session["CaptchaImageText"] = CaptchaDLL.CaptchaImage.GenerateRandomCode(CaptchaType.AlphaNumeric, 6);
            }
        }
    }

    protected void Page_PreRender(object sender, System.EventArgs e)
    {

        if (Context.User.Identity.IsAuthenticated)
        {
            //this.QuestionCaptcha.Width = 0;
            //this.QuestionCaptcha.Visible = false;
            this.EmailLabel.Visible = false;
            this.EmailAddress.Visible = false;
        }
    }

    /* Gets the filenames that populate the RadRotator's DataSource */
    protected string[] GetRotatorSlides()
    {
        FileInfo[] info = new DirectoryInfo(Server.MapPath(@"~\Images\Slides\About")).GetFiles();
        string[] slideFiles = new string[info.Length];
        int i = 0;
        foreach (FileInfo f in info)
        {
            slideFiles[i++] = f.Name;
        }

        return slideFiles;
    }


    //Sends an email from a specified smtp server to the 
    protected void SubmitQuestion_Click(object sender, EventArgs e)
    {
        try
        {
            var capImgText = Session["CaptchaImageText"];
            if (Context.User.Identity.IsAuthenticated || (capImgText != null && txtCode.Text.ToLower() == capImgText.ToString().ToLower()))
            {

                 const string EMAILTEMPLATE = "You have received a new question from a visitor of the 3D Repository.\r\nName: {0}\r\nQuestion: {1}"; 
                 Website.Mail.SendSingleMessage(String.Format(EMAILTEMPLATE,((Context.User.Identity.IsAuthenticated) ? User.Identity.Name : EmailAddress.Text), QuestionText.Text), 
                     Website.Config.PSSupportEmail, 
                     "New question from ADL 3D Repository", 
                     Website.Config.SupportEmailFromAddress, 
                     Website.Config.SupportEmailFromAddress, 
                     String.Empty, 
                     String.Empty, 
                     true, 
                     String.Empty);
                StatusLabel.Text = "Your question has been sent. Thank you for your feedback!";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "fadeqblock", "$('#QuestionBlock').fadeOut(400, function() { $('#Status').fadeIn('fast') });", true);
            }
            else
            {
                CaptchaErrorLabel.Text = "Invalid Captcha response. Please try again.";
                CaptchaErrorLabel.Visible = true;
                ibtnRefresh_Click(sender, e);
            }
        }
        catch (Exception exc)
        {
            StatusLabel.Text = "An error was encountered while trying to ask your question. Please try again later.";
            StatusLabel.Visible = true;
        }
        
    }

    protected void ibtnRefresh_Click(object sender,EventArgs e)
    {
        Session["CaptchaImageText"] = CaptchaDLL.CaptchaImage.GenerateRandomCode(CaptchaType.AlphaNumeric, 6); //generate new string
    }
}