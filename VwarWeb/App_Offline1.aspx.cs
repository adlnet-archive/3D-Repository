using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class App_Offline1 : Website.Pages.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.SearchPanel.Visible = false;
    }
}