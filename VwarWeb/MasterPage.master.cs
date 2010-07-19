using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.ID = "MasterPage";
        
        if (Context.User.Identity.IsAuthenticated)
        {
            //set logout tooltip
            this.LoginStatus1.ToolTip = "Logout";
            GetUserNameForWelcomeMessage();

            this.AdminPanel.Visible = Website.Security.IsAdministrator();
            this.AdvancedSearchHyperLink.Enabled = true;
            this.AdvancedSearchHyperLink.ToolTip = "Advanced Search";

        }
        else 
        {
            this.LoginStatus1.ToolTip = "Login";
            this.AdvancedSearchHyperLink.Enabled = false;
            this.AdvancedSearchHyperLink.ToolTip = "You must be a member to use the advance search feature.";
        }
    }
    protected void SearchButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Public/Results.aspx?Search=" + Server.UrlEncode(SearchTextBox.Text.Trim()));
    }

    private string GetUserNameForWelcomeMessage()
    { 
        string rv = "";

        //make sure user is auth
        if (Context.User.Identity.IsAuthenticated)
        {
            //find label in loginview
            //Label l = (Label)LoginView1.Controls[0].FindControl("UserNameLabel");
            ////&& !String.IsNullOrEmpty(l.Text)
            //if (l != null)
            //{ 
            //    //load profile
            //    if (Profile.GetProfile(rv) != null)
            //    {
            //        //set to profile fname/lname and hide normal loginName
            //        if (!String.IsNullOrEmpty(Profile.FirstName) && !String.IsNullOrEmpty(Profile.LastName))
            //        {
            //            rv = Profile.FirstName.Trim() + " " + Profile.LastName.Trim();
            //            l.Text = String.Format("Welcome {0}!", rv);
            //            LoginView1.Controls[0].FindControl("LoginName1").Visible = false;
            //        }
            //    }            
            //}

        }

        return rv;
    }
}
