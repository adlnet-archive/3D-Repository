using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Controls_MyModels : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //load my models - by User.Id.Name
        if (!Page.IsPostBack)
        {
            var factory = new vwarDAL.DataAccessFactory();
            vwarDAL.IDataRepository vd = factory.CreateDataRepositorProxy();
            MyModelsDataList.DataSource = vd.GetContentObjectsBySubmitterEmail(Context.User.Identity.Name.Trim());
            MyModelsDataList.DataBind();

        }
    }

}
