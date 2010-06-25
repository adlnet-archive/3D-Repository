using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Public_AdvancedSearch : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            MultiView1.ActiveViewIndex = 0;
        }
    }

    protected void ChangeSort(object sender, EventArgs args)
    {
        
    }


    protected void CancelButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(Website.Pages.Types.Default);
    }
    protected void SearchButon_Click(object sender, EventArgs e)
    {
        this.MultiView1.SetActiveView(this.ResultsView);
    }
}