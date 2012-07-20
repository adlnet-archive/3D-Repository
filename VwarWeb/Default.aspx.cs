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
using System.Drawing;
using System.Web.UI.HtmlControls;

/// <summary>
/// 
/// </summary>
public partial class Default2 : Website.Pages.PageBase
{
    /// <summary>
    /// 
    /// </summary>
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
    /// <summary>
    /// 
    /// </summary>
    private bool hrDataBound = false, rvDataBound = false, ruDataBound = false;
    /// <summary>
    /// 
    /// </summary>
    private int modelCount;
    /// <summary>
    /// 
    /// </summary>
    //private vwarDAL.IDataRepository vd;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            string[] tempPopularTags; 

            ((HyperLink)RandomRotator.FindControl("ViewMoreHyperLink")).Text = "More...";
            ((HyperLink)RandomRotator.FindControl("ViewMoreHyperLink")).NavigateUrl = "/Default.aspx?refresh=true";

            if(Session["MostPopular"] == null || Context.Request.QueryString["refresh"] != null)
            {
                BindViewData(HighestRatedRotator);
                BindViewData(MostPopularRotator);
                BindViewData(RecentlyUpdatedRotator);
                BindViewData(RandomRotator);
            

                
                ISearchProxy permissionsHonoringProxy = new DataAccessFactory().CreateSearchProxy(HttpContext.Current.User.Identity.Name);

                tempPopularTags = permissionsHonoringProxy.GetMostPopularTags();

                PopularDevelopersList.DataSource = permissionsHonoringProxy.GetMostPopularDevelopers();
                PopularDevelopersList.DataBind();

                permissionsHonoringProxy.Dispose();

                Session["HighestRated"] = ((DataList)HighestRatedRotator.FindControl("RotatorLayoutTable").FindControl("RotatorListViewRow").FindControl("RotatorListViewColumn").FindControl("RotatorListView")).DataSource;
                Session["MostPopular"] = ((DataList)MostPopularRotator.FindControl("RotatorLayoutTable").FindControl("RotatorListViewRow").FindControl("RotatorListViewColumn").FindControl("RotatorListView")).DataSource;
                Session["RecentlyUpdated"] = ((DataList)RecentlyUpdatedRotator.FindControl("RotatorLayoutTable").FindControl("RotatorListViewRow").FindControl("RotatorListViewColumn").FindControl("RotatorListView")).DataSource;
                Session["Random"] = ((DataList)RandomRotator.FindControl("RotatorLayoutTable").FindControl("RotatorListViewRow").FindControl("RotatorListViewColumn").FindControl("RotatorListView")).DataSource;
                Session["PopularTags"] = tempPopularTags;
                Session["PopularDevelopers"] = PopularDevelopersList.DataSource;
            }else
            {
                DataList list = (DataList)HighestRatedRotator.FindControl("RotatorLayoutTable")
                                        .FindControl("RotatorListViewRow")
                                            .FindControl("RotatorListViewColumn")
                                                .FindControl("RotatorListView");
                list.DataSource = (IEnumerable<ContentObject>)Session["HighestRated"];
                list.DataBind();

                list = (DataList)MostPopularRotator.FindControl("RotatorLayoutTable")
                                        .FindControl("RotatorListViewRow")
                                            .FindControl("RotatorListViewColumn")
                                                .FindControl("RotatorListView");
                list.DataSource = (IEnumerable<ContentObject>)Session["MostPopular"];
                list.DataBind();

                list = (DataList)RecentlyUpdatedRotator.FindControl("RotatorLayoutTable")
                                        .FindControl("RotatorListViewRow")
                                            .FindControl("RotatorListViewColumn")
                                                .FindControl("RotatorListView");
                list.DataSource = (IEnumerable<ContentObject>)Session["RecentlyUpdated"];
                list.DataBind();

                list = (DataList)RandomRotator.FindControl("RotatorLayoutTable")
                                        .FindControl("RotatorListViewRow")
                                            .FindControl("RotatorListViewColumn")
                                                .FindControl("RotatorListView");
                list.DataSource = (IEnumerable<ContentObject>)Session["Random"];
                list.DataBind();

                tempPopularTags = (string[])Session["PopularTags"];

                PopularDevelopersList.DataSource = (string[])Session["PopularDevelopers"];
                PopularDevelopersList.DataBind();
            }

            PopularTagsView.ClientIDMode = ClientIDMode.Static;

            StylizeTags((string [])tempPopularTags.Clone());            
        }
    }

        /// <summary>
    /// 
    /// </summary>
    /// <param name="tags"></param>
    protected void StylizeTags(string [] s)
    {
        int count = 0;

        string[] temp;
        string[] colors = { "#535050", "#2C2918", "#3E3F41", "#AAA", "#A19B88" };

        int i = 0;

        HyperLink aLink;
        Shuffle<string>(s);

        for (i = 0; i < s.Length; i++)
        {
            aLink = new HyperLink();

            temp = s[i].Split('.');
            aLink.ToolTip = temp[0] + " (" + temp[1] + ")";
            aLink.NavigateUrl = "~/Public/results.aspx?Keywords=" + Server.UrlEncode(temp[0]) + "&Method=or";

            s[i] = temp[0].Replace(" ", "&nbsp;");
            count = (Int16.Parse(temp[1]) + 5 > 30) ? 30 : Int16.Parse(temp[1]) + 5;

            aLink.Text = s[i];

            aLink.Font.Size = count;
            aLink.Style.Add("margin", "3px");
            aLink.Style.Add("display", "block");
            aLink.Style.Add("float", "left");
            aLink.Style.Add("text-decoration", "none");
            aLink.Style.Add("color", colors[count % (colors.Length)]);

            PopularTagsView.Controls.Add(aLink);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="list"></param>
    public static void Shuffle<T>(IList<T> list)
    {
        Random rng = new Random(DateTime.Now.Millisecond);
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="c"></param>
    protected void BindViewData(Control c)
    {
      //  return;
        PermissionsManager permManager = new PermissionsManager();
        string username = HttpContext.Current.User.Identity.Name;

        //You gotta love how FindControl isn't recursive...
        DataList list = (DataList)c.FindControl("RotatorLayoutTable")
                                        .FindControl("RotatorListViewRow")
                                            .FindControl("RotatorListViewColumn")
                                                .FindControl("RotatorListView");

        ISearchProxy permissionsHonoringProxy = new DataAccessFactory().CreateSearchProxy(HttpContext.Current.User.Identity.Name);

        switch (c.ID)
        {
            case "HighestRatedRotator":
                list.DataSource = permissionsHonoringProxy.GetByRating(4);
                break;

            case "MostPopularRotator":
                list.DataSource = permissionsHonoringProxy.GetByViews(4);
                break;

            case "RecentlyUpdatedRotator":
                list.DataSource = permissionsHonoringProxy.GetByLastUpdated(4);
                break;

            case "RandomRotator":
                list.DataSource = permissionsHonoringProxy.GetByRandom(4);
                break;

            default:
                throw new Exception("No control '" + c.ID + "' could be found to bind data to.");
        }
        list.DataBind();
        permissionsHonoringProxy.Dispose();
        permManager.Dispose();
    }
}
