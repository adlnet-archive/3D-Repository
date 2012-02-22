using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;

public partial class Administrators_LRIntegration : System.Web.UI.Page
{
    private Dictionary<string, string> GetOrignialValues()
    {
        if (ViewState["originialvalues"] == null)
            ViewState["originialvalues"] = new Dictionary<string, string>();

        return ViewState["originialvalues"] as Dictionary<string, string>;
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

            GNUPG_Key_ID.Text = ConfigurationManager.AppSettings["LR_Integration_KeyID"];
            GNUPG_Key_Passphrase1.Text = ConfigurationManager.AppSettings["LR_Integration_KeyPassPhrase"];
            GNUPG_Key_Passphrase2.Text = ConfigurationManager.AppSettings["LR_Integration_KeyPassPhrase"];
            GNUPG_Location.Text = ConfigurationManager.AppSettings["LR_Integration_GPGLocation"];
            GNUPG_Public_Key_URL.Text = ConfigurationManager.AppSettings["LR_Integration_PublicKeyURL"];
            Submitter_Name.Text = ConfigurationManager.AppSettings["LR_Integration_SubmitterName"];
            Signer_Name.Text = ConfigurationManager.AppSettings["LR_Integration_SignerName"];
            APIBaseURL.Text = ConfigurationManager.AppSettings["LR_Integration_APIBaseURL"];
            PublishURL.Text = ConfigurationManager.AppSettings["LR_Integration_PublishURL"];
            LRNodeUsername.Text = ConfigurationManager.AppSettings["LR_Integration_NodeUsername"];
            LRNodePassword1.Text = ConfigurationManager.AppSettings["LR_Integration_NodePassword"];
            LRNodePassword2.Text = ConfigurationManager.AppSettings["LR_Integration_NodePassword"];
            LRIntegrationEnabled.Text = ConfigurationManager.AppSettings["LR_Integration_Enabled"];

            ResetDefaultText(GNUPG_Key_ID);
            ResetDefaultText(GNUPG_Key_Passphrase1);
            ResetDefaultText(GNUPG_Key_Passphrase2);
            ResetDefaultText(GNUPG_Location);
            ResetDefaultText(GNUPG_Public_Key_URL);
            ResetDefaultText(Submitter_Name);
            ResetDefaultText(Signer_Name);
            ResetDefaultText(APIBaseURL);
            ResetDefaultText(PublishURL);
            ResetDefaultText(LRNodeUsername);
            ResetDefaultText(LRNodePassword1);
            ResetDefaultText(LRNodePassword2);
            ResetDefaultText(LRIntegrationEnabled);

            SaveLRSettings.Enabled = false;
        }
    }
    protected void SaveLRSettings_Click(object sender, EventArgs e)
    {
        if (GNUPG_Key_Passphrase1.Text != GNUPG_Key_Passphrase2.Text)
        {
            LRStatus.Text = "GNUPG Passpharse must match!";
            return;
        }
        if (LRNodePassword1.Text != LRNodePassword2.Text)
        {
            LRStatus.Text = "LR Node Password must match!";
            return;
        }
        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        AppSettingsSection section = config.GetSection("appSettings") as AppSettingsSection;
        if (section != null)
        {
            SetSetting(section, "LR_Integration_KeyID", GNUPG_Key_ID.Text);
            SetSetting(section, "LR_Integration_KeyPassPhrase", GNUPG_Key_Passphrase1.Text);
            SetSetting(section, "LR_Integration_GPGLocation", GNUPG_Location.Text);
            SetSetting(section, "LR_Integration_PublicKeyURL", GNUPG_Public_Key_URL.Text);
            SetSetting(section, "LR_Integration_SubmitterName", Submitter_Name.Text);
            SetSetting(section, "LR_Integration_SignerName", Signer_Name.Text);
            SetSetting(section, "LR_Integration_APIBaseURL", APIBaseURL.Text);
            SetSetting(section, "LR_Integration_PublishURL", PublishURL.Text);
            SetSetting(section, "LR_Integration_NodeUsername", LRNodeUsername.Text);
            SetSetting(section, "LR_Integration_NodePassword", LRNodePassword1.Text);
            SetSetting(section, "LR_Integration_Enabled", LRIntegrationEnabled.Text);
            
           
            config.Save();
        }
        ResetDefaultText(GNUPG_Key_ID);
        ResetDefaultText(GNUPG_Key_Passphrase1);
        ResetDefaultText(GNUPG_Key_Passphrase2);
        ResetDefaultText(GNUPG_Location);
        ResetDefaultText(GNUPG_Public_Key_URL);
        ResetDefaultText(Submitter_Name);
        ResetDefaultText(Signer_Name);
        ResetDefaultText(APIBaseURL);
        ResetDefaultText(PublishURL);
        ResetDefaultText(LRNodeUsername);
        ResetDefaultText(LRNodePassword1);
        ResetDefaultText(LRNodePassword2);
        ResetDefaultText(LRIntegrationEnabled);
        LRStatus.Text = "Saved";
        Updatecheckmarks();
    }
    protected void TestSettings_Click(object sender, EventArgs e)
    {
        if (GNUPG_Key_Passphrase1.Text != GNUPG_Key_Passphrase2.Text)
        {
            LRStatus.Text = "GNUPG Passpharse must match!";
            return;
        }
        if (LRNodePassword1.Text != LRNodePassword2.Text)
        {
            LRStatus.Text = "LR Node Password must match!";
            return;
        }
        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        AppSettingsSection section = config.GetSection("appSettings") as AppSettingsSection;
        if (section != null)
        {
            SetSetting(section, "LR_Integration_KeyID", GNUPG_Key_ID.Text);
            SetSetting(section, "LR_Integration_KeyPassPhrase", GNUPG_Key_Passphrase1.Text);
            SetSetting(section, "LR_Integration_GPGLocation", GNUPG_Location.Text);
            SetSetting(section, "LR_Integration_PublicKeyURL", GNUPG_Public_Key_URL.Text);
            SetSetting(section, "LR_Integration_SubmitterName", Submitter_Name.Text);
            SetSetting(section, "LR_Integration_SignerName", Signer_Name.Text);
            SetSetting(section, "LR_Integration_APIBaseURL", APIBaseURL.Text);
            SetSetting(section, "LR_Integration_PublishURL", PublishURL.Text);
            SetSetting(section, "LR_Integration_NodeUsername", LRNodeUsername.Text);
            SetSetting(section, "LR_Integration_NodePassword", LRNodePassword1.Text);
            SetSetting(section, "LR_Integration_Enabled", LRIntegrationEnabled.Text);
            config.Save();
        }

        try
        {
            vwarDAL.ContentObject co = new vwarDAL.ContentObject();
            
            co.Keywords = "test";
            co.Label = "test label";
            co.PID = "testPID";
            co.Location = "asdf";
            co.LastViewed = DateTime.Now;
            co.LastModified = DateTime.Now;
            co.ArtistName = "test";
            co.AssetType = "model";
            co.CreativeCommonsLicenseURL = "";
            co.Description = "test description";
            co.DeveloperName = "";
            LRStatus.Text = LR.LR_3DR_Bridge.ModelViewedInternal(co);
        }
        catch (Exception ex)
        {
            LRStatus.Text = ex.Message;
        }

        SetSetting(section, "LR_Integration_KeyID", GetOrignialValues()[GNUPG_Key_ID.ID]);
        SetSetting(section, "LR_Integration_KeyPassPhrase",GetOrignialValues()[ GNUPG_Key_Passphrase1.ID]);
        SetSetting(section, "LR_Integration_GPGLocation",GetOrignialValues()[ GNUPG_Location.ID]);
        SetSetting(section, "LR_Integration_PublicKeyURL",GetOrignialValues()[ GNUPG_Public_Key_URL.ID]);
        SetSetting(section, "LR_Integration_SubmitterName",GetOrignialValues()[ Submitter_Name.ID]);
        SetSetting(section, "LR_Integration_SignerName",GetOrignialValues()[ Signer_Name.ID]);
        SetSetting(section, "LR_Integration_APIBaseURL",GetOrignialValues()[ APIBaseURL.ID]);
        SetSetting(section, "LR_Integration_PublishURL",GetOrignialValues()[ PublishURL.ID]);
        SetSetting(section, "LR_Integration_NodeUsername",GetOrignialValues()[ LRNodeUsername.ID]);
        SetSetting(section, "LR_Integration_NodePassword",GetOrignialValues()[LRNodePassword1.ID]);
        SetSetting(section, "LR_Integration_Enabled",GetOrignialValues()[ LRIntegrationEnabled.ID]);
        if(LRStatus.Text.Contains("\"OK\": true"))
            SaveLRSettings.Enabled = true;

        Updatecheckmarks();
    }
}