using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vwarDAL;

public partial class Default2 : Website.Pages.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            int modelCount = 4;
            var factory = new vwarDAL.DataAccessFactory();
            vwarDAL.IDataRepository vd = factory.CreateDataRepositorProxy();

            //most popular
            var mp = vd.GetMostPopular(modelCount);
            PopularListView.DataSource = mp;
            PopularListView.DataBind();

            //highly rated
            var hr = vd.GetHighestRated(modelCount);
            HighlyRatedListView.DataSource = hr;
            HighlyRatedListView.DataBind();

            //recently viewed
            var rv = vd.GetRecentlyViewed(modelCount);
            RecentlyViewedListView.DataSource = rv;
            RecentlyViewedListView.DataBind();

            //recently updated
            var ru = vd.GetRecentlyUpdated(modelCount);
            RecentlyUpdatedListView.DataSource = ru;
            RecentlyUpdatedListView.DataBind();

        }        
    }
}
