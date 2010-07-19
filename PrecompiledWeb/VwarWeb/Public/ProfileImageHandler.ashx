<%@ WebHandler Language="C#" Class="ProfileImageHandler" %>

using System;
using System.Web;
using vwarDAL;
using System.Data;

public class ProfileImageHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string userID ="";
        string logo = "";
        
        if (context.Request.QueryString["UserID"] != null)
        {
          userID = context.Request.QueryString["UserID"];
          

        }

        if (context.Request.QueryString["Logo"] != null)
        {
            logo = context.Request.QueryString["Logo"];

        }
        
        

        if (!string.IsNullOrEmpty(userID)  && !string.IsNullOrEmpty(logo))
        {
            //TODO: create new method to get the sponsor logo or developer logo
            DataTable dt = null;

            if (logo.ToLower() == "sponsor")
            {
                dt = UserProfileDB.GetUserProfileSponsorLogoByUserID(userID);
            }
            else if (logo.ToLower() == "developer")
            {
                dt = UserProfileDB.GetUserProfileDeveloperLogoByUserID(userID);
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                if (dr["Logo"] != System.DBNull.Value && dr["LogoContentType"] != System.DBNull.Value && !string.IsNullOrEmpty(dr["LogoContentType"].ToString()))
                {
                    context.Response.ContentType = dr["LogoContentType"].ToString();                   
                    context.Response.BinaryWrite((byte[])dr["Logo"]);

                }               


            }        
        }


        context.Response.End();


    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}