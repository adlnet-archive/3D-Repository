using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class Controls_MissingTextures : Website.Pages.ControlBase
{

    protected void Page_Render(object sender, EventArgs e)
    {
        oldFileName.Text = OldFile;
    }
    public Stream FileContent { get { return FileUpload1.FileContent; } }
    public String FileName { get { return FileUpload1.FileName; } }
    public String OldFile { get; set; }
}