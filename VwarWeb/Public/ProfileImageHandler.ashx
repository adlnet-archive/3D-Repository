<%--
Copyright 2011 U.S. Department of Defense

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
--%>



<%@ WebHandler Language="C#" Class="ProfileImageHandler" %>

using System;
using System.Web;
using vwarDAL;
using System.Data;
/// <summary>
/// 
/// </summary>
public class ProfileImageHandler : IHttpHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void ProcessRequest(HttpContext context)
    {
        string userID = "";
        string logo = "";

        if (context.Request.QueryString["UserID"] != null)
        {
            userID = context.Request.QueryString["UserID"];


        }

        if (context.Request.QueryString["Logo"] != null)
        {
            logo = context.Request.QueryString["Logo"];

        }



        if (!string.IsNullOrEmpty(userID) && !string.IsNullOrEmpty(logo))
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
    /// <summary>
    /// 
    /// </summary>
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
