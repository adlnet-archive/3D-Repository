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
using System.Reflection;
public partial class Public_Results : Website.Pages.PageBase
{
    const int DEFAULT_RESULTS_PER_PAGE = 5;

    private int _ResultsPerPage;
    private int _PageNumber = 1;

    private bool _Presorted = false;

    

    private string SortInfo
    {
        get { return sort.SelectedValue; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        vwarDAL.ISearchProxy _SearchProxy = new DataAccessFactory().CreateSearchProxy(Context.User.Identity.Name);
        _ResultsPerPage = System.Convert.ToInt32(ResultsPerPageDropdown.SelectedValue);

        string tempUrl = "";
        string[] acceptableArray = {"Search", "Keywords", "DeveloperName", "ArtistName", "SponsorName"};

        for (int i = 0; i < acceptableArray.Length; i++)
        {
            if (Context.Request.QueryString[acceptableArray[i]] != null)
            {
                tempUrl = (acceptableArray[i] == "Keywords") ? "Keywords;" + Context.Request.QueryString[acceptableArray[i]] : Context.Request.QueryString[acceptableArray[i]];
                break;
            }
        }

        APILink.NavigateUrl = "https://" + ConfigurationManager.AppSettings["LR_Integration_APIBaseURL"] + "/Search/" + Server.UrlEncode(tempUrl) + "/json?id=00-00-00";

        //Search
        if (!IsPostBack)
        {    
            SetInitialSortValue();
            
            IEnumerable<ContentObject> co = GetSearchResults();

            if (co != null && co.Count() > 0)
            {
                //If it's presorted, we cannot rely on the number of items in the returned collection
                //to get the true number of objects matching the specified search query
                int totalResults = (_Presorted) ? _SearchProxy.GetContentObjectCount() : co.Count();
                BindPageNumbers(totalResults);

                ApplySearchResults(co);
            }
            else
            {
                NoneFoundLabel.Visible = true;
                if(Membership.GetUser() == null)
                    NoneFoundLabel.Text = "No models found. It's possible that some models are hidden by their owners. Try logging in for more results. <br />";
                else
                    NoneFoundLabel.Text = "No models found. It's possible that some models are hidden by their owners. <br />";
                SearchList.Visible = false;
            }
        }
        _SearchProxy.Dispose();
    }


    private IEnumerable<ContentObject> GetSearchResults()
    {
        IEnumerable<ContentObject> co = null;
        vwarDAL.ISearchProxy _SearchProxy = new DataAccessFactory().CreateSearchProxy(Context.User.Identity.Name);
        SearchMethod method;
        string methodParam = Request.QueryString["Method"];
        if (!String.IsNullOrEmpty(methodParam) && methodParam.ToLowerInvariant() == "and")
            method = SearchMethod.AND;
        else
            method = SearchMethod.OR;

        if (!String.IsNullOrEmpty(Request.QueryString["Search"]))
        {
            string searchTerm = Request.QueryString["Search"];
            if (!String.IsNullOrEmpty(searchTerm))
            {
                SearchTextBox.Text = Server.UrlDecode(searchTerm);
                if (SearchTextBox.Text.Contains(','))
                {
                    var terms = SearchTextBox.Text.Split(',');
                    co = _SearchProxy.QuickSearch(terms, method);
                }
                else
                {
                    co = _SearchProxy.QuickSearch(SearchTextBox.Text);
                }
            }
        }
        else if (!String.IsNullOrEmpty(Request.QueryString["Group"]))
        {
            /* Here, we are getting "everything" (limit NumResultsPerPage) by a grouping.
            
               By doing a little processing ahead of time, we can
               avoid having to use ApplySort when unnecessary, as well
               as reduce the size of the data returned from MySQL. 
            */

            string[] sortParams = SortInfo.Split('-');
            if (sortParams.Length == 2) //Make sure it's in the right format!
            {
                SortOrder order = (sortParams[1].ToLowerInvariant() == "low")
                  ? SortOrder.Ascending
                  : SortOrder.Descending;

                int start = (_PageNumber - 1) * _ResultsPerPage;

                switch (sortParams[0].ToLowerInvariant())
                {
                    case "views":
                        co = _SearchProxy.GetByViews(_ResultsPerPage, start, order);
                        break;

                    case "updated":
                        co = _SearchProxy.GetByLastUpdated(_ResultsPerPage, start, order);
                        break;

                    case "viewed":
                        co = _SearchProxy.GetByLastViewed(_ResultsPerPage, start, order);
                        break;

                    case "rating":
                    default:
                        co = _SearchProxy.GetByRating(_ResultsPerPage, start, order);
                        break;
                }
                _Presorted = true;
            }
        }
        else //We're searching by field
        {
            System.Collections.Specialized.NameValueCollection fieldsToSearch = new System.Collections.Specialized.NameValueCollection();

            string[] searchableFields = { "Title", "Description", "ArtistName", "SponsorName", "DeveloperName", "Keywords" };

            foreach (string field in searchableFields)
                if (!String.IsNullOrEmpty(Request.QueryString[field]))
                    fieldsToSearch[field] = Server.UrlDecode(Request.QueryString[field]);

            co = _SearchProxy.SearchByFields(fieldsToSearch, method);
        }

        _SearchProxy.Dispose();
        return co;
    }
    protected void SetInitialSortValue()
    {
        if (!String.IsNullOrEmpty(Request.QueryString["Group"]))
        {
            sort.SelectedValue = Request.QueryString["Group"];
        }
    }
    protected void RefreshSearch(object sender, EventArgs args)
    {
        ApplySearchResults(GetSearchResults());
    }
    private void ApplySearchResults(IEnumerable<ContentObject> co)
    {
        if (!_Presorted)
            SearchList.DataSource = ApplySort(co).Skip((_PageNumber - 1) * _ResultsPerPage).Take(_ResultsPerPage);
        else
            SearchList.DataSource = co;

        SearchList.DataBind();
        Client_UpdateSelectedPageNumber();
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

        if (Request.QueryString["ContentObjectID"] != null && !string.IsNullOrEmpty(Request.QueryString["ContentObjectID"].ToString()))
        {
            string coid = Server.UrlDecode(Request.QueryString["ContentObjectID"].ToString().Trim());

            url = Website.Pages.Types.FormatModel(coid);

        }

        Response.Redirect(url);
    }
    protected void BindPageNumbers(int numResults)
    {
        int numPages = Math.Max((int)Math.Ceiling(numResults / (float)_ResultsPerPage), 1);
        int range = Math.Min(10, numPages);
        int start = 0;

        //Determine the page numbers to add, based on the range
        if (_PageNumber <= System.Convert.ToInt32(range * 0.5f))
            start = 1;
        else if (_PageNumber >= System.Convert.ToInt32(numPages - range * 0.5f))
            start = numPages - range + 1;
        else
            start = _PageNumber - System.Convert.ToInt32(0.5f * range);

        int[] datasource = new int[range];

        int i = 0;
        for (int j = start; j <= start + range - 1; j++)
            datasource[i++] = j;

        PageNumbersRepeater.DataSource = datasource;
        PageNumbersRepeater.DataBind();

        UpdateResultsLabel((_PageNumber-1) * _ResultsPerPage + 1, numResults);
        UpdatePreviousNextButtons(numPages);
    }
    protected void PageNumberChanged(object sender, EventArgs e)
    {
        vwarDAL.ISearchProxy _SearchProxy = new DataAccessFactory().CreateSearchProxy(Context.User.Identity.Name);
        //Get the page number from the value displayed to the user
        LinkButton btn = (LinkButton)sender;
        _PageNumber = System.Convert.ToInt32(btn.CommandArgument);

        IEnumerable<ContentObject> co = GetSearchResults();
        ApplySearchResults(co);

        BindPageNumbers(_Presorted
                        ? _SearchProxy.GetContentObjectCount()
                        : co.Count());
        _SearchProxy.Dispose();
    }
    protected void UpdatePreviousNextButtons(int numPages)
    {
        PreviousPageButton.Visible = _PageNumber > 1;
        NextPageButton.Visible = _PageNumber < numPages;

        if (PreviousPageButton.Visible)
            PreviousPageButton.CommandArgument = (_PageNumber - 1).ToString();

        if (NextPageButton.Visible)
            NextPageButton.CommandArgument = (_PageNumber + 1).ToString();
    }
    protected void UpdateResultsLabel(int start, int numResults)
    {
        string resultsTemplate = "Showing results {0}-{1} of {2}";
        int end = start + _ResultsPerPage - 1;
        if (end > numResults)
            end = numResults;
        ResultsLabel.Text = String.Format(resultsTemplate, start, end, numResults);
    }
    private void Client_UpdateSelectedPageNumber()
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "updatepgnum", "UpdateSelectedPageNumber('" + _PageNumber.ToString() + "');", true);
    }
    protected void NumResultsPerPageChanged(object sender, EventArgs e)
    {
        vwarDAL.ISearchProxy _SearchProxy = new DataAccessFactory().CreateSearchProxy(Context.User.Identity.Name);
        _ResultsPerPage = System.Convert.ToInt32(ResultsPerPageDropdown.SelectedValue);

        IEnumerable<ContentObject> co = GetSearchResults();

        if (co != null)
            BindPageNumbers(_Presorted
                            ? _SearchProxy.GetContentObjectCount()
                            : co.Count());

        ApplySearchResults(co);
        _SearchProxy.Dispose();
    }
}
