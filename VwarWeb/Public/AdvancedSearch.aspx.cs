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
using System.Text;
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
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SearchButon_Click(object sender, EventArgs e)
    {
        //TODO: Use Generic Search Service for searching.
        string resultsURL = Page.ResolveClientUrl("~/Public/Results.aspx?" + BuildSearchQuery());
        Response.Redirect(resultsURL);
    }
    /// <summary>
    /// 
    /// </summary>
    private string BuildSearchQuery()
    {
        StringBuilder sb = new StringBuilder();
        string paramTemplate = "{0}={1}&";

        //Title

        if (!String.IsNullOrEmpty(this.TitleTextBox.Text))
        {
            sb.Append(String.Format(paramTemplate, "Title", TitleTextBox.Text));
        }

        //description
        if (!String.IsNullOrEmpty(this.DescriptionTextBox.Text))
        {
            sb.Append(String.Format(paramTemplate, "Description", DescriptionTextBox.Text));
        }

        //tags
        if (!String.IsNullOrEmpty(this.TagsTextBox.Text))
        {
            sb.Append(String.Format(paramTemplate, "Keywords", TagsTextBox.Text));
        }


        //developer name
        if (!String.IsNullOrEmpty(this.DeveloperNameTextBox.Text))
        {
            sb.Append(String.Format(paramTemplate, "DeveloperName", DeveloperNameTextBox.Text));
        }

        //sponsor name
        if (!String.IsNullOrEmpty(this.SponsorNameTextBox.Text))
        {
            sb.Append(String.Format(paramTemplate, "SponsorName", SponsorNameTextBox.Text));
        }

        //artist name
        if (!String.IsNullOrEmpty(this.ArtistNameTextBox.Text))
        {
            sb.Append(String.Format(paramTemplate, "ArtistName", ArtistNameTextBox.Text));
        }

        sb.Append("Method=" + MethodSelectorDropDown.SelectedValue);

        return sb.ToString();
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
