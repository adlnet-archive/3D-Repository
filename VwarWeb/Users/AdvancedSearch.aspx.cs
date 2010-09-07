using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vwarDAL;
using Website;

public partial class Public_AdvancedSearch : Website.Pages.PageBase
{

    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.MultiView1.SetActiveView(this.SearchView);
        }
    }

    protected void CancelButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(Website.Pages.Types.Default);
    }
    protected void SearchButon_Click(object sender, EventArgs e)
    {
        //TODO: Use Generic Search Service for searching.
        this.BindSearchList();
    }


    private void BindSearchList()
    {
        vwarDAL.IDataRepository vd = DAL;
        IEnumerable<ContentObject> co = null;
       
        //Title
      
        if (!String.IsNullOrEmpty(this.TitleTextBox.Text))
        {        
            co = vd.GetContentObjectsByTitle(this.TitleTextBox.Text.Trim());

        }

        //description
        if (!String.IsNullOrEmpty(this.DescriptionTextBox.Text))
        {
            co = vd.GetContentObjectsByDescription(this.DescriptionTextBox.Text.Trim());

        }


        //tags
        if (!String.IsNullOrEmpty(this.TagsTextBox.Text))
        {
            co = vd.GetContentObjectsByKeyWords(this.TagsTextBox.Text.Trim());
            
        }


        //developer name
        if (!String.IsNullOrEmpty(this.DeveloperNameTextBox.Text))
        {
            co = vd.GetContentObjectsByDeveloperName(this.DeveloperNameTextBox.Text.Trim());

        }

        //sponsor name
        if (!String.IsNullOrEmpty(this.SponsorNameTextBox.Text))
        {
            co = vd.GetContentObjectsBySponsorName(this.SponsorNameTextBox.Text.Trim());

        }



        //artist name
        if (!String.IsNullOrEmpty(this.ArtistNameTextBox.Text))
        {
            co = vd.GetContentObjectsByArtistName(this.ArtistNameTextBox.Text.Trim());

        }

      

        //show none found label?
        if (co != null && co.Count() > 0)
        {
            NoneFoundLabel.Visible = false;
            SearchList.Visible = true;    

            SearchList.DataSource = co;
            SearchList.DataBind();
        }
        else
        {
                this.NoneFoundLabel.Text = "No models were found. <br />";
                NoneFoundLabel.Visible = true;
                SearchList.Visible = false;    

        }


        this.MultiView1.SetActiveView(this.ResultsView);

       
    }







    protected void NewAdvanceSearchButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(Website.Pages.Types.AdvancedSearch);
    }
}