using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_ModelRotator : System.Web.UI.UserControl
{


    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected string FormatDescription(string desc)
    {
        return (desc.Length > 50) ? desc.Substring(0, 50) + "..." : desc;
    }
}