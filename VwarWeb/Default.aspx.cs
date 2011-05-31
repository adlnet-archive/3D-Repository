using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vwarDAL;


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
            BindViewData(HighestRatedRotator);
            BindViewData(MostPopularRotator);
            BindViewData(RecentlyUpdatedRotator);
        }
    }

   

    protected void BindViewData(Control c)
    {
        //You gotta love how FindControl isn't recursive...
        DataList list = (DataList)c.FindControl("RotatorLayoutTable")
                                        .FindControl("RotatorListViewRow")
                                            .FindControl("RotatorListViewColumn")
                                                .FindControl("RotatorListView");
        switch (c.ID)
        {
            case "HighestRatedRotator":
                list.DataSource = DAL.GetHighestRated(4);
                break;

            case "MostPopularRotator":
                list.DataSource = DAL.GetMostPopular(4);
                break;

            case "RecentlyUpdatedRotator":
                list.DataSource = DAL.GetRecentlyUpdated(4);
                break;

            default:
                throw new Exception("No control '"+ c.ID + "' could be found to bind data to.");
        }
        list.DataBind();
    }

}
