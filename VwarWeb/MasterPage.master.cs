using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class MasterPage : System.Web.UI.MasterPage
{

    const int NUM_TAG_BUCKETS = 8;
    const int NUM_TAG_KEYWORDS = 20;
    const int TAG_FONT_SIZE = 10;
   
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Context.User.Identity.IsAuthenticated)
        {
            //set logout tooltip
            this.LoginStatus1.ToolTip = "Logout";
            GetUserNameForWelcomeMessage();

            this.AdminPanel.Visible = Website.Security.IsAdministrator();
            this.AdvancedSearchHyperLink.ToolTip = "Advanced Search";

        }
        else
        {
            this.LoginStatus1.ToolTip = "Login";
            
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

    protected void LoadTagCloudData()
    {
        vwarDAL.IDataRepository vd = new vwarDAL.DataAccessFactory().CreateDataRepositorProxy();
        //List<Tuple<string, int>> tagFrequencies = (List<Tuple<string, int>>)vd.GetMostPopularKeywords(NUM_TAG_KEYWORDS);
        /*List<Tuple<string, int>> weightedOutput = new List<Tuple<string, int>>();
       


        List<List<Tuple<string, int>>> buckets = new List<List<Tuple<string, int>>>();

        for (int i = 0; i < NUM_TAG_BUCKETS; i++)
        {
            buckets.Add(new List<Tuple<string, int>>());
        }

        int tagFreqIndex = 0;
        for (int i = 0; i < NUM_TAG_BUCKETS; i++)
        {
            for (int j = 0; j < tagFrequencies.Count / NUM_TAG_BUCKETS; j++)
            {
                buckets[i].Add(tagFrequencies[tagFreqIndex++]);
            }
        }

        for (int i = tagFreqIndex; i < tagFrequencies.Count; i++)
        {
            buckets[NUM_TAG_BUCKETS - 1].Add(tagFrequencies[i]);
        }

        int bucketIndex = NUM_TAG_BUCKETS;
        foreach (List<Tuple<string, int>> bucket in buckets)
        {
            foreach (Tuple<string, int> tag in bucket)
            {
                weightedOutput.Add(Tuple.Create(tag.Item1, bucketIndex * TAG_FONT_SIZE));
            }
            bucketIndex--;
        }
        */
      //  TagsRepeater.DataSource = tagFrequencies;
      //  TagsRepeater.DataBind();
    }
}
