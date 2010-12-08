using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vwarDAL;
using Utils;

public partial class Controls_NewUpload : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void ModelUpload_NextStep(object sender, EventArgs e)
    {
        FileStatus currentStatus = (FileStatus)HttpContext.Current.Session["fileStatus"];
        currentStatus.filename = this.TitleInput.Text.Trim().Replace(' ', '_') + ".zip";

        ContentObject tempFedoraCO = (ContentObject)Session["contentObject"];
        tempFedoraCO.Title = this.TitleInput.Text.Trim();
        tempFedoraCO.Description = this.DescriptionInput.Text.Trim();
        tempFedoraCO.Location = currentStatus.filename;
        

        //Add the keywords
        if (this.TagsInput.Text.LastIndexOf(',') == -1) //They used whitespace as delimiter
        {
            tempFedoraCO.Keywords = String.Join(",", this.TagsInput.Text.Split(' '));
        }
        else
        {
            tempFedoraCO.Keywords = this.TagsInput.Text;
        }

        
        /* If viewable, we go to the set axis and scale step
         * If it's recognized, then we skip to the thumbnail step.
         */
        if (currentStatus.type == FormatType.VIEWABLE)
        {
            
            tempFedoraCO.DisplayFile = currentStatus.filename.Replace("zip", "o3d");
            string script = string.Format("var vLoader = new ViewerLoader('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}'); vLoader.LoadViewer();", Page.ResolveClientUrl("~/Public/"),
                                                                                                                              "Model.ashx?temp=true&file=",
                                                                                                                              tempFedoraCO.Location,
                                                                                                                              tempFedoraCO.DisplayFile,
                                                                                                                              "",
                                                                                                                              "",
                                                                                                                              "false");

        }
        else if (currentStatus.type == FormatType.RECOGNIZED)
        {
            tempFedoraCO.DisplayFile = "N/A";
        }

        Session["contentObject"] = tempFedoraCO;
    }
}