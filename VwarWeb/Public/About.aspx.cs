using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Threading;
using Telerik.Web.UI;
using System.Net.Mail;

public partial class About : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        SlidesRotator.DataSource = GetRotatorSlides();
        SlidesRotator.DataBind();
        if (Context.User.Identity.IsAuthenticated)
        {
            this.EmailAddress.Enabled = false;
            this.QuestionCaptcha.Enabled = false;
            this.EmailValidator.Enabled = false;
        }
    }

    protected void Page_PreRender(object sender, System.EventArgs e)
    {
        
        if (Context.User.Identity.IsAuthenticated)
        {
            this.QuestionCaptcha.Width = 0;
            this.QuestionCaptcha.Visible = false;
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
            if (Context.User.Identity.IsAuthenticated ||
                (!Context.User.Identity.IsAuthenticated && QuestionCaptcha.IsValid))
            {
                string toAddr = "austin.montoya.ctr@adlnet.gov";


                MailMessage message = new MailMessage("austin.r.montoya@gmail.com", toAddr);

                message.Subject = "New question from ADL 3D Repository";
                message.Body = "You have received a new question from a visitor of the 3D Repository.\n" +
                    "Name: " + ((Context.User.Identity.IsAuthenticated) ? User.Identity.Name : EmailAddress.Text) + "\n" +
                    "Question: " + QuestionText.Text;

                System.Collections.Specialized.NameValueCollection settings = System.Web.Configuration.WebConfigurationManager.AppSettings;
                SmtpClient mailClient = new SmtpClient(settings["TestEmailSmtpServer"], System.Convert.ToInt16(settings["TestEmailPortNumber"]));
                mailClient.EnableSsl = true;
                mailClient.UseDefaultCredentials = false;
                mailClient.Credentials = new System.Net.NetworkCredential(settings["TestEmailUsername"], settings["TestEmailPassword"]);
                mailClient.Timeout = 30000;

                mailClient.Send(message);
                StatusLabel.Text = "Your question has been sent. Thank you for your feedback!";
                QuestionPanel.ResponseScripts.Add("$('#QuestionBlock').fadeOut(400, function() { $('#Status').fadeIn('fast') });");         
            }
        }
        catch (Exception exc)
        {
            StatusLabel.Text = "An error was encountered while trying to ask your question. Please try again later.";
        }
    }
}