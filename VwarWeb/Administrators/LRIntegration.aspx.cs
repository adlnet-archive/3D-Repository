using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
public partial class Administrators_LRIntegration : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
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
    }
}