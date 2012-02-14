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
using System.Web.Script.Services;
using System.Web.Script.Serialization;
public partial class Public_Results : Website.Pages.PageBase
{
    const int DEFAULT_RESULTS_PER_PAGE = 5;

    private int _ResultsPerPage;
    private int _PageNumber = 1;

    class SearchResult
    {
        public string PID { get; set; }
        public string Title { get; set; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        SearchPanel.Visible = false;
        _ResultsPerPage = 6;
        if(!IsPostBack)
        if (Request.QueryString["SearchTerms"] != "" && Request.QueryString["SearchTerms"] != null)
        {
            ApplySearchResults(GetSearchResults(Request.QueryString["SearchTerms"]));
            SearchFederationTextBox.Text = Request.QueryString["SearchTerms"];
            SearchResultsUpdatePanel.Visible = true;
        }

       
    }


    private IEnumerable<SearchResult> GetSearchResults(string terms)
    {
        List<SearchResult> SearchResults = new List<SearchResult>();
       
            System.Net.WebClient wc = new System.Net.WebClient();
            wc.Credentials = new System.Net.NetworkCredential("AnonymousUser", "");
            string data = wc.DownloadString("http://3dr.adlnet.gov/Federation/3DR_Federation.svc/Search/"+terms+"/json?ID=00-00-00");
            SearchResults.Clear();
            SearchResult[] results = (new JavaScriptSerializer()).Deserialize<SearchResult[]>(data);
            foreach (SearchResult s in results)
            {
                SearchResults.Add(s);
            }
        return SearchResults;
    }
    protected void RefreshSearch(object sender, EventArgs args)
    {
        if (Request.QueryString["SearchTerms"] != "")
        {
             ApplySearchResults(GetSearchResults(Request.QueryString["SearchTerms"]));
        }
        else
        {
            ApplySearchResults(GetSearchResults(SearchFederationTextBox.Text));
        }
    }
    protected void SearchFederatonButton_Click(object sender, EventArgs args)
    {

        Response.Redirect("~/Public/FederationResults.aspx?SearchTerms=" + SearchFederationTextBox.Text);
           // ApplySearchResults(GetSearchResults(SearchFederationTextBox.Text));
        
    }
    private void ApplySearchResults(IEnumerable<SearchResult> co)
    {
        List<SearchResult> results = new List<SearchResult>();
        for (int i = (_PageNumber - 1) * System.Convert.ToInt16(ResultsPerPageDropdown.Text); i<co.Count() &&  i < (_PageNumber - 1) * System.Convert.ToInt16(ResultsPerPageDropdown.Text) + System.Convert.ToInt16(ResultsPerPageDropdown.Text); i++)
        {
            results.Add(co.ElementAt(i));
        }

        SearchList.DataSource = results;
        SearchList.DataBind();
        BindPageNumbers(co.Count());
        Client_UpdateSelectedPageNumber();
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
        //Get the page number from the value displayed to the user
        LinkButton btn = (LinkButton)sender;
        if (btn.CommandArgument == "Next")
            _PageNumber += 1;
        _PageNumber = System.Convert.ToInt32(btn.CommandArgument);

        IEnumerable<SearchResult> co = null;
        if (Request.QueryString["SearchTerms"] != "")
        {
            co = GetSearchResults(Request.QueryString["SearchTerms"]);
        }
        else
        {
            co = GetSearchResults(SearchFederationTextBox.Text);
        }

       
        ApplySearchResults(co);

        BindPageNumbers(co.Count());
                        
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
        _ResultsPerPage = System.Convert.ToInt32(ResultsPerPageDropdown.SelectedValue);

        IEnumerable<SearchResult> co = null;
        if (Request.QueryString["SearchTerms"] != "")
        {
            co = GetSearchResults(Request.QueryString["SearchTerms"]);
        }
        else
        {
            co = GetSearchResults(SearchFederationTextBox.Text);
        }
        if (co != null)
            BindPageNumbers(co.Count());

        ApplySearchResults(co);
    }
}
