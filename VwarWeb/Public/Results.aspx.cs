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

    private int _ResultsPerPage
    {
        get { return (int)Session["ResultsPerPage"]; }
        set { Session["ResultsPerPage"] = value; }
    }

    private string SortInfo
    {
        get { return sort.SelectedValue; }
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        //Search
        if (!IsPostBack)
        {
            SetInitialSortValue();
            _ResultsPerPage = DEFAULT_RESULTS_PER_PAGE;

            IEnumerable<ContentObject> co = GetSearchResults();

            if (co != null)
                BindPageNumbers(co.Count());
            ApplySearchResults(co, 1);
        }

    }


    private IEnumerable<ContentObject> GetSearchResults()
    {
        vwarDAL.IDataRepository vd = DAL;
        vwarDAL.ISearchProxy searchProxy = new DataAccessFactory().CreateSearchProxy(Context.User.Identity.Name);
        IEnumerable<ContentObject> co = null;

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
                    co = searchProxy.QuickSearch(terms, method);
                }
                else
                {
                    co = searchProxy.QuickSearch(SearchTextBox.Text);
                }
            }
        }
        else //We're searching by field
        {
            System.Collections.Specialized.NameValueCollection fieldsToSearch = new System.Collections.Specialized.NameValueCollection();

            string[] searchableFields = { "Title", "Description", "ArtistName", "SponsorName", "DeveloperName", "Keywords" };

            foreach (string field in searchableFields)
                if (!String.IsNullOrEmpty(Request.QueryString[field]))
                    fieldsToSearch[field] = Server.UrlDecode(Request.QueryString[field]);

            co = searchProxy.SearchByFields(fieldsToSearch, method);
        }

        //show none found label?
        if (co == null || co.Count() == 0)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["Group"]))
            {
                //If the logged in user is the site admin, get all models, even if the permission are broken or something
                //This is necessary for hte admin to be able to fix permission on models that for some reason were removed 
                //from the allusers group.
                if (Context.User.Identity.Name.Equals(System.Configuration.ConfigurationManager.AppSettings["DefaultAdminName"],StringComparison.CurrentCultureIgnoreCase))
                    co = vd.GetAllContentObjects();
                else
                    co = vd.GetAllContentObjects(Context.User.Identity.Name);
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
    protected void RefreshSearch(object sender, EventArgs args)
    {
        ApplySearchResults(GetSearchResults(), 1);
    }
    private void ApplySearchResults(IEnumerable<ContentObject> co, int pageNum)
    {
        SearchList.DataSource = ApplySort(co).Skip((pageNum - 1) * _ResultsPerPage).Take(_ResultsPerPage);
        SearchList.DataBind();
        UpdatePreviousNextButtons(pageNum);
        Client_UpdateSelectedPageNumber(pageNum);
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
        int numPages = Math.Max(numResults / _ResultsPerPage, 1);
        if (numResults > _ResultsPerPage && numResults % _ResultsPerPage > 0)
            numPages++;

        int[] datasource = new int[numPages];

        for (int i = 0; i < datasource.Length; i++)
            datasource[i] = i + 1;
        PageNumbersRepeater.DataSource = datasource;
        PageNumbersRepeater.DataBind();
    }
    protected void PageNumberChanged(object sender, EventArgs e)
    {
        //Get the page number from the value displayed to the user
        LinkButton btn = (LinkButton)sender;
        int pageNum = System.Convert.ToInt32(btn.CommandArgument);
        ApplySearchResults(GetSearchResults(), pageNum);

    }
    protected void UpdatePreviousNextButtons(int pagenum)
    {
        PreviousPageButton.Visible = pagenum > 1;
        NextPageButton.Visible = pagenum < PageNumbersRepeater.Controls.Count;

        if (PreviousPageButton.Visible)
            PreviousPageButton.CommandArgument = (pagenum - 1).ToString();

        if (NextPageButton.Visible)
            NextPageButton.CommandArgument = (pagenum + 1).ToString();
    }
    private void Client_UpdateSelectedPageNumber(int pageNum)
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "updatepgnum", "UpdateSelectedPageNumber('" + pageNum.ToString() + "');", true);
    }
    protected void NumResultsPerPageChanged(object sender, EventArgs e)
    {
        _ResultsPerPage = System.Convert.ToInt32(ResultsPerPageDropdown.SelectedValue);

        IEnumerable<ContentObject> co = GetSearchResults();

        if (co != null)
            BindPageNumbers(co.Count());

        ApplySearchResults(co, 1);
    }
   /* private object GetContentObjectProperty(ContentObject co)
    {
        //Reflection is your friend
        Type t = co.GetType();
        PropertyInformation propInfo = t.GetProperties(BindingFlags.Public | 
    }*/

}
