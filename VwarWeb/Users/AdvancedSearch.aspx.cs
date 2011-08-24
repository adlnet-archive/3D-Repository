//  Copyright 2011 U.S. Department of Defense

//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at

//      http://www.apache.org/licenses/LICENSE-2.0

//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.



using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vwarDAL;
using Website;
/// <summary>
/// 
/// </summary>
public partial class Public_AdvancedSearch : Website.Pages.PageBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.MultiView1.SetActiveView(this.SearchView);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CancelButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(Website.Pages.Types.Default);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SearchButon_Click(object sender, EventArgs e)
    {
        //TODO: Use Generic Search Service for searching.
        this.BindSearchList();
    }
    /// <summary>
    /// 
    /// </summary>
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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void NewAdvanceSearchButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(Website.Pages.Types.AdvancedSearch);
    }
}
