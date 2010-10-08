using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vwarDAL;
using Telerik.Web.UI;

public partial class Default2 : Website.Pages.PageBase
{

    protected class TabDataSource
    {
        //The text to be displayed on the tab
        public string TabText;

        //The name of the icon displayed next to the text
        public string IconName;

        public TabDataSource(string text, string icon)
        {
            TabText = text;
            IconName = icon;
        }
    }

    private bool hrDataBound = false, rvDataBound = false, ruDataBound = false;
    private int modelCount;
    private vwarDAL.IDataRepository vd;


    protected void Page_Load(object sender, EventArgs e)
    {        
        if (!Page.IsPostBack)
        {
            AddTab("Highly Rated", "icon_highlyRated");
            AddPageView(TabStrip.FindTabByText("Highly Rated"));
            AddTab("Recently Viewed", "icon_recentlyViewed");
            AddPageView(TabStrip.FindTabByText("Recently Viewed"));
            AddTab("Recently Updated", "icon_recentlyUpdated");
            AddPageView(TabStrip.FindTabByText("Recently Updated"));
        }        
    }

    protected void AddTab(string tabName, string imageName)
    {
        RadTab t = new RadTab();
        t.Text = tabName;
        t.ImageUrl = "Images/Homepage Pieces/" + imageName + ".png";
        TabStrip.Tabs.Add(t);
    }

    protected void AddPageView(RadTab t)
    {
        RadPageView pv = new RadPageView();
        pv.ID = t.Text.Replace(" ", "")+"View";
        ModelBrowseMultiPage.PageViews.Add(pv);
        pv.CssClass = "rotatorView";
        t.PageViewID = pv.ID;
    }

    protected void ModelBrowseMultiPage_PageViewCreated(object sender, RadMultiPageEventArgs e)
    {
        Control rotatorControl = Page.LoadControl("~/Controls/ModelRotator.ascx");
        rotatorControl.ID = e.PageView.ID.Replace("View", "Rotator");
        RadRotator r = (RadRotator)rotatorControl.FindControl("RotatorListView");
        string groupname;
        switch(e.PageView.ID)
        {
            case "HighlyRatedView":
                groupname = "rating-high";
                r.DataSource = DAL.GetHighestRated(4);
                break;

            case "RecentlyViewedView":
                groupname = "viewed-high";
                r.DataSource = DAL.GetRecentlyViewed(4);
                break;

            case "RecentlyUpdatedView":
                groupname = "updated-high";
              //  r.DataSource = DAL.GetRecentlyUpdated(4);
                break;
            
            default:
                throw new Exception("The PageView ID requested cannot be found");
        }

        r.DataBind();
        ((HyperLink) rotatorControl.FindControl("ViewMoreHyperLink")).NavigateUrl = "~/Public/Results.aspx?Group="+groupname;
        e.PageView.Controls.Add(rotatorControl);

    }

    protected void TabStrip_TabClick(object sender, RadTabStripEventArgs e)
    {
        //AddPageView(e.Tab);
       // e.Tab.PageView.Selected = true;
    }



}
