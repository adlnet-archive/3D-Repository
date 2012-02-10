using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Configuration;
public partial class Public_RecoverDatabaseConnection : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //If you're loading the error handler page, and the database connection works, just redirect home.
        if (!IsPostBack)
        {
            bool test = TestConnectionString(ConfigurationManager.ConnectionStrings["postgreSQLConnectionString"].ConnectionString);
            if (test)
            {
                HttpContext.Current.Response.Redirect("~/default.aspx");
                return;
            }
               
        }

        if (AdminName.Text == ConfigurationManager.AppSettings["DefaultAdminName"] &&
           AdminPassword.Text == ConfigurationManager.AppSettings["DefaultAdminPassword"])
        {
            if (MySQLIP.Text == "")
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
            }
        }
    }

    protected void SaveMySQLSettings_Click(object sender, EventArgs e)
    {

        if (!(AdminName.Text == ConfigurationManager.AppSettings["DefaultAdminName"] &&
           AdminPassword.Text == ConfigurationManager.AppSettings["DefaultAdminPassword"]))
        {
            return;
        }

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
        HttpContext.Current.Response.Redirect("~/default.aspx");
        testMySQLStatus.Text = "Saved";
    }
    protected void TestMySQLSettings_Click(object sender, EventArgs e)
    {
        if (!(AdminName.Text == ConfigurationManager.AppSettings["DefaultAdminName"] &&
           AdminPassword.Text == ConfigurationManager.AppSettings["DefaultAdminPassword"]))
        {
            return;
        }
        if (MySQLPassword2.Text != MySQLPassword1.Text)
        {
            testMySQLStatus.Text = "Passwords Must Match";
            return;
        }
        string ConnectionString = "Driver={MySQL ODBC 5.1 Driver};Server=" + MySQLIP.Text + ";Port=" + MySQLPort.Text + ";Database=3dr;User=" + MySQLUserName.Text + ";Password=" + MySQLPassword2.Text + ";Option=3";

        bool test = TestConnectionString(ConnectionString);

                if(test)
                {
                    SaveMySQLSettings.Enabled = true;
                    testMySQLStatus.Text = "Test Successful";
                }
                else
                {
                    SaveMySQLSettings.Enabled = false;
                    testMySQLStatus.Text = "Test Failed";
                }
        
    }
    protected bool TestConnectionString(string connectionString)
    {
        using (System.Data.Odbc.OdbcConnection conn = new System.Data.Odbc.OdbcConnection(connectionString))
        {
            try
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        return true;
    }
    protected void Login_Click(object sender, EventArgs e)
    {
        if (AdminName.Text == ConfigurationManager.AppSettings["DefaultAdminName"] &&
            AdminPassword.Text == ConfigurationManager.AppSettings["DefaultAdminPassword"])
        {
            SQLSettingPanel.Visible = true;
           // LoginPanel.Visible = false;
        }
        else
        {
            LoginPanel.Visible = false;
            loginStatus.Text = "Login Failed";
        }

    }
}