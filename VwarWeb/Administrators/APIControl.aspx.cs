using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Configuration;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Web.Script.Serialization;
using System.Net;
using System.Net.NetworkInformation;
public partial class Administrators_APIControl : System.Web.UI.Page
{
    private Dictionary<string, string> GetOrignialValues()
    {
        if (ViewState["originialvalues"] == null)
            ViewState["originialvalues"] = new Dictionary<string, string>();

        return ViewState["originialvalues"] as Dictionary<string, string>;
    }
    protected string GetAppSetting(XmlNode appSettings, string name)
    {
        foreach (XmlNode c in appSettings.ChildNodes)
        {
            if (c.Attributes["key"].Value == name)
                return c.Attributes["value"].Value;
        }
        return "";
    }
    protected void SetConnectionString(XmlNode connectionStrings, string name, string value)
    {
        foreach (XmlNode c in connectionStrings.ChildNodes)
        {
            if (c.Attributes["name"].Value == name)
                c.Attributes["connectionString"].Value = value;
        }
        
    }
    protected void  SetAppSetting(XmlNode appSettings, string name, string value)
    {
        foreach (XmlNode c in appSettings.ChildNodes)
        {
            if (c.Attributes["key"].Value == name)
                c.Attributes["value"].Value = value;
        }
       
    }
    protected string GetConnectionString(XmlNode connectionStrings, string name)
    {
        foreach (XmlNode c in connectionStrings.ChildNodes)
        {
            if (c.Attributes["name"].Value == name)
                return c.Attributes["connectionString"].Value;
        }
        return "";
    }
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            APIPath.Text = ConfigurationManager.AppSettings["APIPATH"];
            APIUrl.Text = ConfigurationManager.AppSettings["LR_Integration_APIBaseURL"];
            SaveAPILocation.Enabled = false;
            SaveMySQLSettings.Enabled = false;
            SaveFedoraSettings.Enabled = false;
            LoadSettings();

            ResetDefaultText(APIPath);
            ResetDefaultText(APIUrl);
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


            

        }
    }
    class SearchResult
    {
        public string PID;
        public string Title;
    }
    public void SearchAPI_Click(object sender, EventArgs e)
    {
        try
        {
            System.Net.WebClient wc = new System.Net.WebClient();
            if (LoginType.Text == "Anonymous")
                wc.Credentials = new System.Net.NetworkCredential("AnonymousUser", "");
            string data = wc.DownloadString(APIUrl.Text + "/Search/" + SearchString.Text + "/json?ID=00-00-00");
            SearchResults.Items.Clear();
            SearchResult[] results = (new JavaScriptSerializer()).Deserialize<SearchResult[]>(data);
            foreach (SearchResult s in results)
            {
                SearchResults.Items.Add("Pid: " + s.PID + " " + "Title: " + s.Title);       
            }
           
        }
        catch (Exception ex)
        {
            SearchResults.Items.Clear();
            SearchResults.Items.Add(ex.Message);
        }
    }
    public void Data_Click(object sender, EventArgs e)
    {
        try
        {
            System.Net.WebClient wc = new System.Net.WebClient();
            if (LoginType.Text == "Anonymous")
                wc.Credentials = new System.Net.NetworkCredential("AnonymousUser", "");
            string data = wc.DownloadString(APIUrl.Text + "/" + SearchResults.Text.Substring(5,SearchResults.Text.IndexOf(" ",5)-5) + "/Metadata/json?ID=00-00-00");
            data = data.Replace("{", "");
            data = data.Replace("}", "");
            data = data.Replace("\"", "");
            data = data.Replace(":", "=");
            data = data.Replace(",", "\n");
            SelectedMetadata.Text = data;
            

        }
        catch (Exception ex)
        {
            SearchResults.Items.Clear();
            SearchResults.Items.Add(ex.Message);
        }
    }
    public void Download_Click(object sender, EventArgs e)
    {
    }
    private void LoadSettings()
    {
        try
        {
                    XmlDocument doc = null;
                
                    doc = new XmlDocument();
                    doc.Load(APIPath.Text+"web.config");
                    XmlNode appSettings = doc.SelectSingleNode("//appSettings");
                    FedoraUserName.Text = GetAppSetting(appSettings, "fedoraUserName");

                    FedoraURL.Text = GetAppSetting(appSettings, "fedoraUrl");
                    FedoraUserName.Text = GetAppSetting(appSettings, "fedoraUserName");
                    FedoraPassword1.Text = GetAppSetting(appSettings, "fedoraPassword");
                    FedoraPassword2.Text = GetAppSetting(appSettings, "fedoraPassword");
                    FedoraNameSpace.Text = GetAppSetting(appSettings, "fedoraNamespace");

                    string connection = GetConnectionString(doc.SelectSingleNode("//connectionStrings"),"postgreSQLConnectionString");
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
            catch (Exception ex)
            {
                MySQLIP.Text = ex.Message;
            }
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
            Control b = FindControlRecursive(this, c.Key);
            if (b is TextBox)
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
        if (c is TextBox)
            GetOrignialValues()[c.ID] = (c as TextBox).Text;
        if (c is DropDownList)
            GetOrignialValues()[c.ID] = (c as DropDownList).Text;
    }
    protected string GetIP()
    {
       return Dns.GetHostEntry(Dns.GetHostName()).AddressList
         .Where(a => !a.IsIPv6LinkLocal && !a.IsIPv6Multicast && !a.IsIPv6SiteLocal)
         .First().ToString();

    }
    protected void SaveAPILocation_Click(object sender, EventArgs e)
    {

        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        AppSettingsSection section = config.GetSection("appSettings") as AppSettingsSection;
        if (section != null)
        {
            string path = APIPath.Text;
            
            path = path.Replace("~/", Server.MapPath("~"));
            path = path.Replace("~\\", Server.MapPath("~"));
            path = path.Replace("/","\\");
            path = System.IO.Path.GetFullPath(path);
            APIPath.Text = path;
            SetSetting(section, "LR_Integration_APIBaseURL", APIUrl.Text);
            SetSetting(section, "APIPATH", APIPath.Text.Replace("localhost",GetIP()));
            config.Save();
            SaveAPILocationstatus.Text = "Saved";
            LoadSettings();
        }

        ResetDefaultText(APIPath);
        ResetDefaultText(APIUrl);

        Updatecheckmarks();
        
    }
    protected void TestAPILocation_Click(object sender, EventArgs e)
    {

        string path = APIPath.Text;

        path = path.Replace("~/", Server.MapPath("~"));
        path = path.Replace("~\\", Server.MapPath("~"));
        path = path.Replace("/", "\\");
        path = System.IO.Path.GetFullPath(path);
        APIPath.Text = path;

        try
        {
            int len = System.IO.Directory.GetFiles(path, "_3DRAPI.svc").Length;
            if (len == 0)
            {
                SaveAPILocationstatus.Text = "Could not find _3DRAPI.svc in the path.";
                Updatecheckmarks();
                return;
            }
        }
        catch (Exception ex)
        {
            SaveAPILocationstatus.Text = ex.Message;
            Updatecheckmarks();
            return;
        }

        APIUrl.Text = APIUrl.Text.Replace("localhost", GetIP());

        System.Net.WebClient wc = new System.Net.WebClient();
        try
        {
            wc.Credentials = new System.Net.NetworkCredential("AnonymousUser", "");
            SaveAPILocationstatus.Text = wc.DownloadString(APIUrl.Text + "/Search/test/json?ID=00-00-00");
            SaveAPILocationstatus.Text = "Test Successful";
            SaveAPILocation.Enabled = true;
        }
        catch (Exception ex)
        {
            SaveAPILocationstatus.Text = ex.Message;
        }
        
        Updatecheckmarks();
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
        
        XmlDocument doc = null;
        doc = new XmlDocument();
        doc.Load(APIPath.Text + "web.config");
        SetConnectionString(doc.SelectSingleNode("//connectionStrings"), "postgreSQLConnectionString", ConnectionString);
        SetConnectionString(doc.SelectSingleNode("//connectionStrings"), "APIKeyDatabaseConnection", APIKeyDatabaseConnection);
        doc.Save(APIPath.Text + "web.config");
        
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
    protected void BlankStatus() { }
    protected void SaveFedoraSettings_Click(object sender, EventArgs e)
    {
        if (FedoraPassword1.Text != FedoraPassword2.Text)
        {
            BlankStatus();
            TestFedoraStatus.Text = "Passwords Must Match";
            return;
        }


        XmlDocument doc = null;
        doc = new XmlDocument();
        doc.Load(APIPath.Text + "web.config");
        SetAppSetting(doc.SelectSingleNode("//appSettings"), "fedoraUrl", FedoraURL.Text);
        SetAppSetting(doc.SelectSingleNode("//appSettings"), "fedoraUserName", FedoraUserName.Text);
        SetAppSetting(doc.SelectSingleNode("//appSettings"), "fedoraPassword", FedoraPassword1.Text);
        SetAppSetting(doc.SelectSingleNode("//appSettings"), "fedoraNamespace", FedoraNameSpace.Text);
        SetAppSetting(doc.SelectSingleNode("//appSettings"), "vwarDAL_FedoraAPIM_Fedora_API_M_Service", FedoraURL.Text + "services/management");
        SetAppSetting(doc.SelectSingleNode("//appSettings"), "vwarDAL_FedoraAPIM_Fedora_API_A_Service", FedoraURL.Text + "services/access");
        doc.Save(APIPath.Text + "web.config");

        BlankStatus();
        testMySQLStatus.Text = "Saved";
        ResetDefaultText(FedoraURL);
        ResetDefaultText(FedoraUserName);
        ResetDefaultText(FedoraPassword1);
        ResetDefaultText(FedoraPassword2);
        ResetDefaultText(FedoraNameSpace);
        Updatecheckmarks();
    }

    protected void AlignWithWebFrontEnd_Click(object sender, EventArgs e)
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

        SaveMySQLSettings.Enabled = false;
        SaveFedoraSettings.Enabled = false;
       
        Updatecheckmarks();
    }
}