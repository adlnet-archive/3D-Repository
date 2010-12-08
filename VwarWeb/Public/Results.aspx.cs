using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using vwarDAL;
using Website;
public partial class Public_Results : Website.Pages.PageBase
{
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        
        //Search
        if (!IsPostBack)
        {
            SetInitialSortValue();
        }
        IEnumerable<ContentObject> co = GetSearchResults();

        ApplySearchResults(co);

        this.BackButton.Visible = !string.IsNullOrEmpty(SearchTextBox.Text);


    }
    private IEnumerable<ContentObject> GetSearchResults()
    {        
        vwarDAL.IDataRepository vd = DAL;
        IEnumerable<ContentObject> co = null;

        if (!String.IsNullOrEmpty(Request.QueryString["Search"]))
        {
            //place search term in search box
            {
                SearchTextBox.Text = Server.UrlDecode(Request.QueryString["Search"].Trim());
            }

            co = vd.SearchContentObjects(Request.QueryString["Search"].Trim());
        }

        if (!String.IsNullOrEmpty(Request.QueryString["Keywords"]))
        {
            //place search term in search box
            {
                SearchTextBox.Text = Server.UrlDecode(Request.QueryString["Keywords"].Trim());
            }

            co = vd.GetContentObjectsByKeyWords(Request.QueryString["Keywords"].Trim());
        }

        //SubmitterEmail
        if (!String.IsNullOrEmpty(Request.QueryString["SubmitterEmail"]))
        {
            {
                SearchTextBox.Text = Server.UrlDecode(Request.QueryString["SubmitterEmail"].Trim());
            }

            co = vd.GetContentObjectsBySubmitterEmail(Request.QueryString["SubmitterEmail"].Trim());
        }


        //sponsorName
        if (!String.IsNullOrEmpty(Request.QueryString["SponsorName"]))
        {

            {
                SearchTextBox.Text = Server.UrlDecode(Request.QueryString["SponsorName"].Trim());
            }

            co = vd.GetContentObjectsBySponsorName(Request.QueryString["SponsorName"].Trim());
        }

        //DeveloperName
        if (!String.IsNullOrEmpty(Request.QueryString["DeveloperName"]))
        {
            {
                SearchTextBox.Text = Server.UrlDecode(Request.QueryString["DeveloperName"].Trim());
            }

            co = vd.GetContentObjectsByDeveloperName(Request.QueryString["DeveloperName"].Trim());
        }

        //Artist
        if (!String.IsNullOrEmpty(Request.QueryString["Artist"]))
        {
            {
                SearchTextBox.Text = Server.UrlDecode(Request.QueryString["Artist"].Trim());
            }

            co = vd.GetContentObjectsByArtistName(Request.QueryString["Artist"].Trim());
        }


        //CollectionName
        //if (!String.IsNullOrEmpty(Request.QueryString["CollectionName"]))
        //{
        //    co = vd.GetContentObjectsByCollectionName(Request.QueryString["CollectionName"].Trim());
        //}

        //show none found label?
        if (co == null || co.Count() == 0)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["Group"]))
            {
                co = vd.GetAllContentObjects();
            }
            else
            {
                NoneFoundLabel.Visible = true;
                NoneFoundLabel.Text = "No models found. <br />";
                SearchList.Visible = false;
            }
        }
        return co;
    }
    protected void SetInitialSortValue()
    {
        if (!String.IsNullOrEmpty(Request.QueryString["Group"]))
        {
            sort.SelectedValue = Request.QueryString["Group"];
        }
    }
    protected void ChangeSort(object sender, EventArgs args)
    {
        ApplySearchResults(GetSearchResults());
    }
    private void ApplySearchResults(IEnumerable<ContentObject> co)
    {
        SearchList.DataSource = ApplySort(co); ;
        SearchList.DataBind();
    }
    private string SortInfo
    {
        get
        {
            return sort.SelectedValue;
        }
    }
    private IEnumerable<ContentObject> ApplySort(IEnumerable<ContentObject> co)
    {
        if (co == null)
            return new List<ContentObject>();
        const string VIEWS = "views";
        const string VIEWED = "viewed";
        const string UPDATED = "updated";
        const string HIGH = "high";
        const string LOW = "low";
        const string RATING = "rating";
        Func<ContentObject, int> VIEWFUNC = (x) => x.Views;
        Func<ContentObject, int> VIEWEDFUNC = (x) => (int)x.LastViewed.ToBinary();
        Func<ContentObject, int> UPDATEDFUNC = (x) => (int)x.LastModified.ToBinary();
        Func<ContentObject, int> RATINGFUNC = (x) => Common.CalculateAverageRating(x.Reviews);
        var sortInfo = SortInfo.Split('-');
        if (sortInfo[0].Equals(VIEWS, StringComparison.InvariantCultureIgnoreCase) &&
            sortInfo[1].Equals(HIGH, StringComparison.InvariantCultureIgnoreCase))
        {
            return co.OrderByDescending(VIEWFUNC);
        }
        else if (sortInfo[0].Equals(VIEWS, StringComparison.InvariantCultureIgnoreCase) &&
            sortInfo[1].Equals(LOW, StringComparison.InvariantCultureIgnoreCase))
        {
            return co.OrderBy(VIEWFUNC);
        }
        else if (sortInfo[0].Equals(RATING, StringComparison.InvariantCultureIgnoreCase) &&
            sortInfo[1].Equals(HIGH, StringComparison.InvariantCultureIgnoreCase))
        {
            return co.OrderByDescending(RATINGFUNC);
        }
        else if (sortInfo[0].Equals(RATING, StringComparison.InvariantCultureIgnoreCase) &&
            sortInfo[1].Equals(LOW, StringComparison.InvariantCultureIgnoreCase))
        {
            return co.OrderBy(RATINGFUNC);
        }
        else if (sortInfo[0].Equals(VIEWED, StringComparison.InvariantCultureIgnoreCase) &&
            sortInfo[1].Equals(HIGH, StringComparison.InvariantCultureIgnoreCase))
        {
            return co.OrderByDescending(VIEWEDFUNC);
        }
        else if (sortInfo[0].Equals(VIEWED, StringComparison.InvariantCultureIgnoreCase) &&
            sortInfo[1].Equals(LOW, StringComparison.InvariantCultureIgnoreCase))
        {
            return co.OrderBy(VIEWEDFUNC);
        }
        else if (sortInfo[0].Equals(UPDATED, StringComparison.InvariantCultureIgnoreCase) &&
            sortInfo[1].Equals(HIGH, StringComparison.InvariantCultureIgnoreCase))
        {
            return co.OrderByDescending(UPDATEDFUNC);
        }
        else if (sortInfo[0].Equals(UPDATED, StringComparison.InvariantCultureIgnoreCase) &&
            sortInfo[1].Equals(LOW, StringComparison.InvariantCultureIgnoreCase))
        {
            return co.OrderBy(UPDATEDFUNC);
        }
        return new List<ContentObject>();
    }
    protected void BackButton_Click(object sender, EventArgs e)
    {
        string url = Request.ServerVariables["HTTP_REFERER"].ToString();

        if (Request.QueryString["ContentObjectID"]!= null && !string.IsNullOrEmpty(Request.QueryString["ContentObjectID"].ToString()))
        {
            string coid = Server.UrlDecode(Request.QueryString["ContentObjectID"].ToString().Trim());

            url = Website.Pages.Types.FormatModel(coid);

        }
        

        Response.Redirect(url);
    }
}
