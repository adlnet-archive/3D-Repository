//  Copyright 2011 U.S. Department of Defense

//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at

//      http://www.apache.org/licenses/LICENSE-2.0

//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.



using System.Web.Security;
using System.Configuration.Provider;
using System.Collections.Specialized;
using System;
using System.Data;
using System.Data.Odbc;
using System.Configuration;
using System.Diagnostics;
using System.Web;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
/// <summary>
/// 
/// </summary>
public partial class Controls_ViewUserControl : System.Web.UI.Page
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        //UserProfileDB db = new UserProfileDB();

        //insert 
        //UserProfile p = db.InsertUserProfile("00eeac44-c78c-459d-90b9-2cae9ba90e09", "John", "Doe", "jdoe@email.com", "http://www.google.com", "Jane Doe", "Dev Name", "Art Name", "111-222-3333", "John Fabian");


        //get the user profile by id
        //UserProfile p2 = db.GetUserProfileByUserID(p.UserID);

        //get the user profile by membershipguid
        //UserProfile p3 = db.GetUserProfileByMembershipUserGUID(p.MembershipUserGuid);


        //get all user profiles data table
        //DataTable dt = db.GetAllUserProfilesDataTable();

        //int count = dt.Rows.Count;


        //get all user profiles list
        // List<UserProfile> allProfiles = db.GetAllUserProfilesList();

        //int count2 = allProfiles.Count;


        //UserProfile p4 = db.GetUserProfileByUserID(14);

        ////update 
        //p4.FirstName = "Tom";
        //p4.LastName = "Sawyer";
        //db.UpdateUserProfile(p4, "John Fabian");

        //string fname = p4.FirstName;
        //string lname = p4.LastName;
        
        //delete         
    }
}
