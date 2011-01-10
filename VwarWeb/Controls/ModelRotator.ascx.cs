using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_ModelRotator : System.Web.UI.UserControl
{
    const int MAX_CHARS_PER_LINE = 27;
    const int MAX_TOTAL_CHARS = 50;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

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