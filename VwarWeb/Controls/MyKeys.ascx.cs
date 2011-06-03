using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_MyKeys : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.APIKeysPanel.Visible = !Website.Security.IsAdministrator();
            if (APIKeysPanel.Visible)
            {
                APIKeysListView.DataSource = new vwar.service.host.APIKeyManager().GetKeysByUser(Context.User.Identity.Name);
                APIKeysListView.DataBind();
            }
        }
    }
}