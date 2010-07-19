using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.Odbc;

/// <summary>
/// Summary description for AutoComplete
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class AutoComplete : System.Web.Services.WebService
{

    public AutoComplete()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    private void AddStringToList(List<string> stringList, string prefixText, string stringToAdd)
    {
        //add word to list if it starts with the prefix

        if (stringToAdd.ToLower().Trim().IndexOf(prefixText.ToLower().Trim()) == 0)
        {
            //prevent duplicates
            if (!stringList.Contains(stringToAdd))
            {
                stringList.Add(stringToAdd);
            }
        }
    }

    //executes sql and return string list for autocomplete prefixText
    private List<string> GetCompletionListFromSql(string prefixText, string sql, string columnName)
    {
        List<string> rv = new List<string>();

        string connstring = Website.Config.PostgreSQLConnectionString;

        DataTable dt = new DataTable();

        using (OdbcDataAdapter da = new OdbcDataAdapter(sql, connstring))
        {
           
            try
            {

                da.Fill(dt);


            }
            catch
            {
            }

        }


        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                //add string if column exists and value is not null
                if (dt.Columns[columnName] != null && !object.ReferenceEquals(dr[columnName], System.DBNull.Value))
                {
                    this.AddStringToList(rv, prefixText, dr[columnName].ToString().Trim());
                }
            }
        }

        return rv;
    }


    [WebMethod()]
    public string[] GetKeywordsCompletionList(string prefixText, int count)
    {
       
        //string sql = "SELECT DISTINCT TOP 20 OrganizationCode FROM Organization WHERE OrganizationCode LIKE '" + prefixText + "' + '%' AND Inactive = 0 ORDER BY OrganizationCode";

        string sql = @"SELECT DISTINCT `Keyword` from `keywords` WHERE `Keyword` LIKE '" + prefixText  + "%' ORDER BY `keyword` Limit 20";

       List<string> sa = this.GetCompletionListFromSql(prefixText, sql, "Keyword");

        return sa.ToArray();
    }






}
