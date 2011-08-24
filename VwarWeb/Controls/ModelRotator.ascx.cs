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

namespace VwarWeb
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Controls_ModelRotator : System.Web.UI.UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        const int MAX_CHARS_PER_LINE = 27;
        /// <summary>
        /// 
        /// </summary>
        const int MAX_TOTAL_CHARS = 50;
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
        /// <param name="desc"></param>
        /// <returns></returns>
        protected string FormatDescription(string desc)
        {
            string newval = (desc.Length > 50) ? desc.Substring(0, 50) + "..." : desc;
            for (int i = 0; i < newval.Length; i++)
            {
                if (i % MAX_CHARS_PER_LINE == 0)
                {
                    newval.Insert(i, "\n");
                }
            }

            return newval;
        }
    }
}
