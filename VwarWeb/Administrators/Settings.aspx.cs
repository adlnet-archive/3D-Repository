using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Configuration;
using System.Reflection;
using System.Net.Mail;
using System.Web.UI.HtmlControls;
public partial class Administrators_Settings : System.Web.UI.Page
{
    

    private Dictionary<string, string> GetOrignialValues()
    {
        if (ViewState["originialvalues"] == null)
            ViewState["originialvalues"] = new Dictionary<string, string>();

        return ViewState["originialvalues"] as Dictionary<string, string>;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        
       
        if (!IsPostBack)
        {
            
           
            //Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=3dr;User=root;Password=;Option=3
            string connection = ConfigurationManager.ConnectionStrings["postgreSQLConnectionString"].ConnectionString;
            try
            {
                MySQLIP.Text = connection.Substring(connection.IndexOf("Server=") + 7, connection.IndexOf(";", connection.IndexOf("Server=")) - connection.IndexOf("Server=") - 7);
            }
            catch (Exception e1)
            {
                MySQLIP.Text = "localhost";
            }
            try
            {
                MySQLPort.Text = connection.Substring(connection.IndexOf("Port=") + 5, connection.IndexOf(";", connection.IndexOf("Port=")) - connection.IndexOf("Port=") - 5);
            }
            catch (Exception e2)
            {
                MySQLPort.Text = "3306";
            }
            MySQLUserName.Text = connection.Substring(connection.IndexOf("User=") + 5, connection.IndexOf(";", connection.IndexOf("User=")) - connection.IndexOf("User=") - 5);
            MySQLPassword1.Text = connection.Substring(connection.IndexOf("Password=") + 9, connection.IndexOf(";", connection.IndexOf("Password=")) - connection.IndexOf("Password=") - 9);
            MySQLPassword2.Text = connection.Substring(connection.IndexOf("Password=") + 9, connection.IndexOf(";", connection.IndexOf("Password=")) - connection.IndexOf("Password=") - 9);

            FedoraURL.Text = ConfigurationManager.AppSettings["FedoraURL"];
            FedoraUserName.Text = ConfigurationManager.AppSettings["fedoraUserName"];
            FedoraPassword1.Text = ConfigurationManager.AppSettings["fedoraPassword"];
            FedoraPassword2.Text = ConfigurationManager.AppSettings["fedoraPassword"];
            FedoraNameSpace.Text = ConfigurationManager.AppSettings["fedoraNamespace"];

            SMTPServer.Text = ConfigurationManager.AppSettings["ProductionEmailSmtpServer"];
            SMTPUserName.Text = ConfigurationManager.AppSettings["ProductionEmailUsername"];
            SMTPPassword.Text = ConfigurationManager.AppSettings["ProductionEmailPassword"];
            SMTPPassword1.Text = ConfigurationManager.AppSettings["ProductionEmailPassword"];


            SiteName.Text = ConfigurationManager.AppSettings["SiteName"];
            CompanyName.Text = ConfigurationManager.AppSettings["CompanyName"];
            CompanyEmail.Text = ConfigurationManager.AppSettings["CompanyEmail"];
            SupportEmail.Text = ConfigurationManager.AppSettings["SupportEmail"];
            CompanyAddress.Text = ConfigurationManager.AppSettings["CompanyAddress"];
            CompanyPhone.Text = ConfigurationManager.AppSettings["CompanyPhone"];
            CompanyFax.Text = ConfigurationManager.AppSettings["CompanyFax"];
            ContactUsViewMapUrl.Text = ConfigurationManager.AppSettings["ContactUsViewMapUrl"];

            EmailEnabled.Text = ConfigurationManager.AppSettings["EmailingActive"];


            ApproveUsersDefault.Text = ConfigurationManager.AppSettings["MembershipUserApprovedByDefault"];
            GoogleAnalyticsAccountID.Text = ConfigurationManager.AppSettings["GoogleAnalyticsAccountID"];
            ConversionLibraryLocation.Text = ConfigurationManager.AppSettings["LibraryLocation"];
            HeaderGraphicPath.Text = ConfigurationManager.AppSettings["HeaderGraphicPath"];
            FooterControlPath.Text = ConfigurationManager.AppSettings["FooterControlPath"];
            AboutPageURL.Text = ConfigurationManager.AppSettings["AboutPageUrl"];


            SaveMySQLSettings.Enabled = false;
            SaveFedoraSettings.Enabled = false;

            ResetDefaultText(MySQLIP);
            ResetDefaultText(MySQLPort);
            ResetDefaultText(MySQLUserName);
            ResetDefaultText(MySQLPassword1);
            ResetDefaultText(MySQLPassword2);
           
            ResetDefaultText(FedoraURL);
            ResetDefaultText(FedoraUserName);
            ResetDefaultText(FedoraPassword1);
            ResetDefaultText(FedoraPassword2);
            ResetDefaultText(FedoraNameSpace);
           
            ResetDefaultText(SMTPServer);
            ResetDefaultText(SMTPUserName);
            ResetDefaultText(SMTPPassword);
            ResetDefaultText(SMTPPassword1);
           
            ResetDefaultText(SiteName);
            ResetDefaultText(CompanyName);
            ResetDefaultText(CompanyEmail);
            ResetDefaultText(SupportEmail);
            ResetDefaultText(CompanyAddress);
            ResetDefaultText(CompanyPhone);
            ResetDefaultText(CompanyFax);
            ResetDefaultText(ContactUsViewMapUrl);
            ResetDefaultText(EmailEnabled);

            ResetDefaultText(ApproveUsersDefault);
            ResetDefaultText(GoogleAnalyticsAccountID);
            ResetDefaultText(ConversionLibraryLocation);
            ResetDefaultText(HeaderGraphicPath);
            ResetDefaultText(FooterControlPath);
            ResetDefaultText(AboutPageURL);
         
           
             
        }
    }
    private Control FindControlRecursive(Control root, string id)
    {
        if (root.ID == id)
        {
            return root;
        }

        foreach (Control c in root.Controls)
        {
            Control t = FindControlRecursive(c, id);
            if (t != null)
            {
                return t;
            }
        }

        return null;
    }
    protected void Updatecheckmarks()
    {

        foreach (KeyValuePair<string, string> c in GetOrignialValues())
        {
            Control b = FindControlRecursive(this,c.Key);
            if (b is TextBox )
            {
                if (GetOrignialValues()[(c.Key)] != (b as TextBox).Text)
                    (FindControlRecursive(this, c.Key + "status") as HtmlImage).Src = "../styles/images/Icons/warning.gif";
                else
                    (FindControlRecursive(this, c.Key + "status") as HtmlImage).Src = "../styles/images/Icons/checkmark.gif";
            }
            if (b is DropDownList)
            {
                if (GetOrignialValues()[(c.Key)] != (b as DropDownList).Text)
                    (FindControlRecursive(this, c.Key + "status") as HtmlImage).Src = "../styles/images/Icons/warning.gif";
                else
                    (FindControlRecursive(this, c.Key + "status") as HtmlImage).Src = "../styles/images/Icons/checkmark.gif";
            }

            
        } 
    }
    protected void ResetDefaultText(Control c)
    {
        if(c is TextBox)
            GetOrignialValues()[c.ID] = (c as TextBox).Text;
        if(c is DropDownList)
            GetOrignialValues()[c.ID] = (c as DropDownList).Text;
    }
    private void BlankStatus()
    {
        testMySQLStatus.Text = "";
        TestFedoraStatus.Text = "";
        testSiteInfoStatus.Text = "";
        TestEmailSettingsStatus.Text = "";
        SaveAdvancedSettingsstatus.Text = "";
    }
    protected void SaveMySQLSettings_Click(object sender, EventArgs e)
    {


        if (MySQLPassword2.Text != MySQLPassword1.Text)
        {
            BlankStatus();
            testMySQLStatus.Text = "Passwords Must Match";
            return;
        }
        
        string ConnectionString = "Driver={MySQL ODBC 5.1 Driver};Server=" + MySQLIP.Text + ";Port=" + MySQLPort.Text + ";Database=3dr;User=" + MySQLUserName.Text + ";Password=" + MySQLPassword2.Text + ";Option=3";
        string APIKeyDatabaseConnection = "Driver={MySQL ODBC 5.1 Driver};Server=" + MySQLIP.Text + ";Port=" + MySQLPort.Text + ";Database=apikeys;User=" + MySQLUserName.Text + ";Password=" + MySQLPassword2.Text + ";Option=3";

        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        ConnectionStringsSection section = config.GetSection("connectionStrings") as ConnectionStringsSection;
        if (section != null)
        {
            section.ConnectionStrings["postgreSQLConnectionString"].ConnectionString = ConnectionString;
            section.ConnectionStrings["APIKeyDatabaseConnection"].ConnectionString = APIKeyDatabaseConnection;
            config.Save();
        }
        BlankStatus();
        testMySQLStatus.Text = "Saved";
        ResetDefaultText(MySQLIP);
        ResetDefaultText(MySQLPort);
        ResetDefaultText(MySQLUserName);
        ResetDefaultText(MySQLPassword1);
        ResetDefaultText(MySQLPassword2);
        Updatecheckmarks();
    }
    protected void TestMySQLSettings_Click(object sender, EventArgs e)
    {
        if (MySQLPassword2.Text != MySQLPassword1.Text)
        {
            BlankStatus();
            testMySQLStatus.Text = "Passwords Must Match";
            return;
        }
        string ConnectionString = "Driver={MySQL ODBC 5.1 Driver};Server=" + MySQLIP.Text + ";Port=" + MySQLPort.Text + ";Database=3dr;User=" + MySQLUserName.Text + ";Password=" + MySQLPassword2.Text + ";Option=3";

        using (System.Data.Odbc.OdbcConnection conn = new System.Data.Odbc.OdbcConnection(ConnectionString))
        {
            try
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    BlankStatus();
                    SaveMySQLSettings.Enabled = true;
                    testMySQLStatus.Text = "Test Successful";
                }
            }
            catch (Exception ex)
            {
                BlankStatus();
                SaveMySQLSettings.Enabled = false;
                testMySQLStatus.Text = "Test Failed: " + ex.Message;
            }
        }
        Updatecheckmarks();
    }
    protected void TestFedoraSettings_Click(object sender, EventArgs e)
    {
        if (FedoraPassword1.Text != FedoraPassword2.Text)
        {
            BlankStatus();
            TestFedoraStatus.Text = "Passwords Must Match";
            return;
        }

        try
        {
            FedoraAPIA.FedoraAPIAService svc = new FedoraAPIA.FedoraAPIAService();
            svc.Url = FedoraURL.Text + "services/access";
            System.Net.NetworkCredential _Credantials = new System.Net.NetworkCredential(FedoraUserName.Text, FedoraPassword1.Text);
            svc.Credentials = _Credantials;
            svc.findObjects(new string[] { "pid" }, "1", new FedoraAPIA.FieldSearchQuery());
            BlankStatus();
            TestFedoraStatus.Text = "Test Successful";
            SaveFedoraSettings.Enabled = true;
        }
        catch (Exception ex)
        {
            BlankStatus();
            SaveFedoraSettings.Enabled = false;
            TestFedoraStatus.Text = "Test Failed: " + ex.Message;
        }
        Updatecheckmarks();
    }
    protected void SaveFedoraSettings_Click(object sender, EventArgs e)
    {
        if (FedoraPassword1.Text != FedoraPassword2.Text)
        {
            BlankStatus();
            TestFedoraStatus.Text = "Passwords Must Match";
            return;
        }

        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        AppSettingsSection section = config.GetSection("appSettings") as AppSettingsSection;
        if (section != null)
        {
                
            section.Settings["fedoraURL"].Value = FedoraURL.Text;
            section.Settings["vwarDAL_FedoraAPIM_Fedora_API_M_Service"].Value = FedoraURL.Text +"services/management";
            section.Settings["vwarDAL_FedoraAPIM_Fedora_API_A_Service"].Value = FedoraURL.Text + "services/access";
            section.Settings["fedoraUserName"].Value = FedoraUserName.Text;
            section.Settings["fedoraPassword"].Value = FedoraPassword1.Text;
            section.Settings["fedoraNamespace"].Value = FedoraNameSpace.Text;
            config.Save();
        }
        BlankStatus();
        testMySQLStatus.Text = "Saved";
        ResetDefaultText(FedoraURL);
        ResetDefaultText(FedoraUserName);
        ResetDefaultText(FedoraPassword1);
        ResetDefaultText(FedoraPassword2);
        ResetDefaultText(FedoraNameSpace);
        Updatecheckmarks();
    }
    protected void SaveSiteInformation_Click(object sender, EventArgs e)
    {
        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        AppSettingsSection section = config.GetSection("appSettings") as AppSettingsSection;
        if (section != null)
        {
            section.Settings["SiteName"].Value = SiteName.Text;
            section.Settings["CompanyName"].Value = CompanyName.Text;
            section.Settings["CompanyEmail"].Value = CompanyEmail.Text;
            section.Settings["SupportEmail"].Value = SupportEmail.Text;
            section.Settings["CompanyAddress"].Value = CompanyAddress.Text;
            section.Settings["CompanyPhone"].Value = CompanyPhone.Text;
            section.Settings["CompanyFax"].Value = CompanyFax.Text;
            section.Settings["ContactUsViewMapUrl"].Value = ContactUsViewMapUrl.Text;
            config.Save();
        }

        ResetDefaultText(SiteName);
        ResetDefaultText(CompanyName);
        ResetDefaultText(CompanyEmail);
        ResetDefaultText(SupportEmail);
        ResetDefaultText(CompanyAddress);
        ResetDefaultText(CompanyPhone);
        ResetDefaultText(CompanyFax);
        ResetDefaultText(ContactUsViewMapUrl);

        BlankStatus();
        testSiteInfoStatus.Text = "Saved";
        Updatecheckmarks();
    }
    protected void SaveEmailSettings_Click(object sender, EventArgs e)
    {
        if (SMTPPassword.Text != SMTPPassword1.Text)
        {
            TestEmailSettingsStatus.Text = "Passwords Must Match";
            return;
        }

        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        AppSettingsSection section = config.GetSection("appSettings") as AppSettingsSection;
        if (section != null)
        {
            
            section.Settings["EmailingActive"].Value = EmailEnabled.Text;
            section.Settings["ProductionEmailSmtpServer"].Value = SMTPServer.Text;
            section.Settings["ProductionEmailUsername"].Value = SMTPUserName.Text;
            section.Settings["ProductionEmailPassword"].Value = SMTPPassword.Text;         
            config.Save();
        }

        BlankStatus();
        TestEmailSettingsStatus.Text = "Saved";
        ResetDefaultText(SMTPServer);
        ResetDefaultText(SMTPUserName);
        ResetDefaultText(SMTPPassword);
        ResetDefaultText(SMTPPassword1);
        ResetDefaultText(EmailEnabled);

        
        Updatecheckmarks();
    }
    protected void TestEmailSettings_Click(object sender, EventArgs e)
    {
        if (SMTPPassword.Text != SMTPPassword1.Text)
        {
            TestEmailSettingsStatus.Text = "Passwords Must Match";
            return;
        }

        try
        {
            SmtpClient smtp = new SmtpClient(SMTPServer.Text, 587);
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential(SMTPUserName.Text, SMTPPassword.Text);

            MailMessage mail = new MailMessage(ConfigurationManager.AppSettings["SupportEmail"], ConfigurationManager.AppSettings["SupportEmail"]);
            mail.Subject = "Testing the SMTP settings for the 3DR";
            smtp.Send(mail);
            BlankStatus();
            TestEmailSettingsStatus.Text = "Test Message sent to " + ConfigurationManager.AppSettings["SupportEmail"];
        }
        catch (Exception ex)
        {
            BlankStatus();
            TestEmailSettingsStatus.Text = ex.Message;
        }
        Updatecheckmarks();
    }
    void SetSetting(AppSettingsSection section, string setting, string value)
    {
        if (section.Settings[setting] != null)
            section.Settings[setting].Value = value;
        else
        {
            section.Settings.Add(new KeyValueConfigurationElement(setting, value));
        }
    }
    protected void SaveAdvancedSettings_Click(object sender, EventArgs e)
    {
        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        AppSettingsSection section = config.GetSection("appSettings") as AppSettingsSection;
        if (section != null)
        {
            SetSetting(section, "MembershipUserApprovedByDefault", ApproveUsersDefault.Text);
            SetSetting(section, "GoogleAnalyticsAccountID", GoogleAnalyticsAccountID.Text);
            SetSetting(section, "LibraryLocation", ConversionLibraryLocation.Text);
            SetSetting(section, "HeaderGraphicPath", HeaderGraphicPath.Text);
            SetSetting(section, "FooterControlPath", FooterControlPath.Text);
            SetSetting(section, "AboutPageUrl", AboutPageURL.Text);
            config.Save();
        }

         ResetDefaultText(ApproveUsersDefault);
            ResetDefaultText(GoogleAnalyticsAccountID);
            ResetDefaultText(ConversionLibraryLocation);
            ResetDefaultText(HeaderGraphicPath);
            ResetDefaultText(FooterControlPath);
            ResetDefaultText(AboutPageURL);
        Updatecheckmarks();
        SaveAdvancedSettingsstatus.Text = "Saved";
    }
}