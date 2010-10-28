using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_Viewer3D : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Load up the javascript that makes this all happen
        //Page.ClientScript.RegisterClientScriptInclude("SimpleViewer", Server.MapPath("~/Scripts/o3djs/simpleviewer.js"));
        //Page.ClientScript.RegisterClientScriptInclude("O3DBase", Server.MapPath("~/Scripts/o3djs/base.js"));
    }
}