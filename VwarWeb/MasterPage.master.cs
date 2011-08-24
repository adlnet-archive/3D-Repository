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
using System.Web.Security;

public partial class MasterPage : System.Web.UI.MasterPage
{

    const int NUM_TAG_BUCKETS = 8;
    const int NUM_TAG_KEYWORDS = 20;
    const int TAG_FONT_SIZE = 10;
   
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Context.User.Identity.IsAuthenticated)
        {
            //set logout tooltip
            this.LoginStatus1.ToolTip = "Logout";

            this.AdminPanel.Visible = Website.Security.IsAdministrator();
            this.AdvancedSearchHyperLink.ToolTip = "Advanced Search";

        }
        else
        {
            this.LoginStatus1.ToolTip = "Login";
            
        }

        
    }
    protected void Page_Init(object sender, EventArgs e)
    {
       // ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "styles-home", String.Format("var STYLES_HOME = '{0}';", Page.ResolveClientUrl("~/styles")), true);
        ScriptManager.RegisterClientScriptInclude(this, Page.GetType(), "site-utils", Page.ResolveUrl("~/Scripts/site-utils.js"));
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
       
        
    }

    protected void SearchButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Public/Results.aspx?Search=" + Server.UrlEncode(SearchTextBox.Text.Trim()));
    }
}
