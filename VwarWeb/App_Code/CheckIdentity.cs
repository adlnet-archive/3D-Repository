using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using vwarDAL;

/// <summary>
/// Summary description for CheckIdentity
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class CheckIdentity : System.Web.Services.WebService {

    public CheckIdentity () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
}
