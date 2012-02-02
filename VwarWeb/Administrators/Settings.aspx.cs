using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Configuration;
using System.Reflection;
public partial class Administrators_Settings : System.Web.UI.Page
{
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

            SaveMySQLSettings.Enabled = false;
            SaveFedoraSettings.Enabled = false;
        }
    }
    protected void SaveMySQLSettings_Click(object sender, EventArgs e)
    {


        if (MySQLPassword2.Text != MySQLPassword1.Text)
        {
            testMySQLStatus.Text = "Passwords Must Match";
            return;
        }
        
        string ConnectionString = "Driver={MySQL ODBC 5.1 Driver};Server=" + MySQLIP.Text + ";Port=" + MySQLPort.Text + ";Database=3dr;User=" + MySQLUserName.Text + ";Password=" + MySQLPassword2.Text + ";Option=3";

        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        ConnectionStringsSection section = config.GetSection("connectionStrings") as ConnectionStringsSection;
        if (section != null)
        {
            section.ConnectionStrings["postgreSQLConnectionString"].ConnectionString = ConnectionString;
            config.Save();
        }
        testMySQLStatus.Text = "Saved";
    }
    protected void TestMySQLSettings_Click(object sender, EventArgs e)
    {
        if (MySQLPassword2.Text != MySQLPassword1.Text)
        {
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
                    SaveMySQLSettings.Enabled = true;
                    testMySQLStatus.Text = "Test Successful";
                }
            }
            catch (Exception ex)
            {
                SaveMySQLSettings.Enabled = false;
                testMySQLStatus.Text = "Test Failed: " + ex.Message;
            }
        }
    }
    protected void TestFedoraSettings_Click(object sender, EventArgs e)
    {
        if (FedoraPassword1.Text != FedoraPassword2.Text)
        {
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
            TestFedoraStatus.Text = "Test Successful";
            SaveFedoraSettings.Enabled = true;
        }
        catch (Exception ex)
        {
            SaveFedoraSettings.Enabled = false;
            TestFedoraStatus.Text = "Test Failed: " + ex.Message;
        }

    }
    protected void SaveFedoraSettings_Click(object sender, EventArgs e)
    {
        if (FedoraPassword1.Text != FedoraPassword2.Text)
        {
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
            config.Save();
        }
        testMySQLStatus.Text = "Saved";

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

        testSiteInfoStatus.Text = "Saved";
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
            section.Settings["ProductionEmailSmtpServer"].Value = SMTPServer.Text;
            section.Settings["ProductionEmailUsername"].Value = SMTPUserName.Text;
            section.Settings["ProductionEmailPassword"].Value = SMTPPassword.Text;         
            config.Save();
        }

        TestEmailSettingsStatus.Text = "saved";
    }
}