using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Users_Profile : Website.Pages.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SearchPanel.Visible = false;
        this.MyModels1.Visible = !Website.Security.IsAdministrator();
    }
}