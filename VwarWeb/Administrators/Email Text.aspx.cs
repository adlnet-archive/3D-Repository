using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Web.Configuration;
public partial class Administrators_Email_Text : System.Web.UI.Page
{
    private Dictionary<string, string> GetOrignialValues()
    {
        if (ViewState["originialvalues"] == null)
            ViewState["originialvalues"] = new Dictionary<string, string>();

        return ViewState["originialvalues"] as Dictionary<string, string>;
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
    private void BlankStatus()
    {
      
    }
    protected void ResetDefaultText(Control c)
    {
        if (c is TextBox)
            GetOrignialValues()[c.ID] = (c as TextBox).Text;
        if (c is DropDownList)
            GetOrignialValues()[c.ID] = (c as DropDownList).Text;
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
    void SetSetting(AppSettingsSection section, string setting, string value)
    {
        if (section.Settings[setting] != null)
            section.Settings[setting].Value = value;
        else
        {
            section.Settings.Add(new KeyValueConfigurationElement(setting, value));
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            UploadedBody.Text = ConfigurationManager.AppSettings["EMAIL_UploadedBody"];
            UploadedSubject.Text = ConfigurationManager.AppSettings["EMAIL_UploadedSubject"];
            RequestedBody.Text = ConfigurationManager.AppSettings["EMAIL_RequestedBody"];
            RequestedSubject.Text = ConfigurationManager.AppSettings["EMAIL_RequestedSubject"];
            ApprovedBody.Text = ConfigurationManager.AppSettings["EMAIL_ApprovedBody"];
            ApprovedSubject.Text = ConfigurationManager.AppSettings["EMAIL_ApprovedSubject"];
            RegisteredBody.Text = ConfigurationManager.AppSettings["EMAIL_RegisteredBody"];
            RegisteredSubject.Text = ConfigurationManager.AppSettings["EMAIL_RegisteredSubject"];

            RegisteredEnabled.Text = ConfigurationManager.AppSettings["EMAIL_RegisteredEnabled"];
            UploadedEnabled.Text = ConfigurationManager.AppSettings["EMAIL_UploadedEnabled"];
            ApprovedEnabled.Text = ConfigurationManager.AppSettings["EMAIL_ApprovedEnabled"];
            RequestedEnabled.Text = ConfigurationManager.AppSettings["EMAIL_RequestedEnabled"];

            ResetDefaultText(UploadedBody);
            ResetDefaultText(UploadedSubject);
            ResetDefaultText(RequestedBody);
            ResetDefaultText(RequestedSubject);
            ResetDefaultText(ApprovedBody);
            ResetDefaultText(ApprovedSubject);
            ResetDefaultText(RegisteredBody);
            ResetDefaultText(RegisteredSubject);

            ResetDefaultText(RegisteredEnabled);
            ResetDefaultText(UploadedEnabled);
            ResetDefaultText(ApprovedEnabled);
            ResetDefaultText(RequestedEnabled);
        }
    }

    protected void SaveNewModelSettings_Click(object sender, EventArgs e)
    {
        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        AppSettingsSection section = config.GetSection("appSettings") as AppSettingsSection;
        if (section != null)
        {
            SetSetting(section, "EMAIL_UploadedBody", UploadedBody.Text);
            SetSetting(section, "EMAIL_UploadedSubject", UploadedSubject.Text);
            SetSetting(section, "EMAIL_UploadedEnabled", UploadedEnabled.Text);
            config.Save();
        }
        ResetDefaultText(UploadedBody);
        ResetDefaultText(UploadedSubject);
        ResetDefaultText(UploadedEnabled);
        Updatecheckmarks();
    }
    protected void SaveNewUserRegisteredSettings_Click(object sender, EventArgs e)
    {
        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        AppSettingsSection section = config.GetSection("appSettings") as AppSettingsSection;
        if (section != null)
        {
            SetSetting(section, "EMAIL_RegisteredBody", RegisteredBody.Text);
            SetSetting(section, "EMAIL_RegisteredSubject", RegisteredSubject.Text);
            SetSetting(section, "EMAIL_RegisteredEnabled", RegisteredEnabled.Text);

            
            config.Save();
        }
        ResetDefaultText(RegisteredBody);
        ResetDefaultText(RegisteredSubject);
        ResetDefaultText(RegisteredEnabled);
        Updatecheckmarks();
    }
    protected void SaveRegistrationApprovedSettings_Click(object sender, EventArgs e)
    {
        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        AppSettingsSection section = config.GetSection("appSettings") as AppSettingsSection;
        if (section != null)
        {
            SetSetting(section, "EMAIL_ApprovedBody", ApprovedBody.Text);
            SetSetting(section, "EMAIL_ApprovedSubject", ApprovedSubject.Text);
            SetSetting(section, "EMAIL_ApprovedEnabled", ApprovedEnabled.Text);

           
            config.Save();
        }
        ResetDefaultText(ApprovedBody);
        ResetDefaultText(ApprovedSubject);
        ResetDefaultText(ApprovedEnabled);
        Updatecheckmarks();
    }
    protected void SaveRegistrationRequestedSettings_Click(object sender, EventArgs e)
    {
        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        AppSettingsSection section = config.GetSection("appSettings") as AppSettingsSection;
        if (section != null)
        {
            SetSetting(section, "EMAIL_RequestedBody", RequestedBody.Text);
            SetSetting(section, "EMAIL_RequestedSubject", RequestedSubject.Text);
            SetSetting(section, "EMAIL_RequestedEnabled", RequestedEnabled.Text);

            config.Save();
        }
        ResetDefaultText(RequestedBody);
        ResetDefaultText(RequestedSubject);
        ResetDefaultText(RequestedEnabled);
        Updatecheckmarks();
    }
}