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



using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Website
{
    /// <summary>
    /// Summary description for Security
    /// </summary>
    /// 
    public class Security
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsAdministrator()
        {
            return HttpContext.Current.User.IsInRole("Administrators");
        }
        /// <summary>
        /// 
        /// </summary>
        public static void CreateRolesAndAdministrativeUser()
        {
            if (Website.Config.GenerateDefaultAdministratorOnApplicationStartup)
            {
                try
                {
                    var userName = System.Configuration.ConfigurationManager.AppSettings["DefaultAdminName"];
                    var password = System.Configuration.ConfigurationManager.AppSettings["DefaultAdminPassword"];
                    if (!Roles.RoleExists("Administrators"))
                    {
                        Roles.CreateRole("Administrators");
                    }
                    if (!Roles.RoleExists("Users"))
                    {
                        Roles.CreateRole("Users");
                    }
                    if (Membership.FindUsersByName(userName).Count == 0)
                    {
                        Membership.CreateUser(userName, password, userName);
                    }

                    if (!Roles.IsUserInRole(userName, "Administrators"))
                    {
                        Roles.AddUserToRole(userName, "Administrators");
                    }
                    vwarDAL.PermissionsManager pmgr = new vwarDAL.PermissionsManager();
                    pmgr.CreateGroup(vwarDAL.DefaultGroups.AllUsers, userName, vwarDAL.DefaultGroups.AllUsers, vwarDAL.GroupPolicyLevel.UsersAddRemove);
                    pmgr.CreateGroup(vwarDAL.DefaultGroups.AnonymousUsers, userName, vwarDAL.DefaultGroups.AnonymousUsers, vwarDAL.GroupPolicyLevel.UsersAddRemove);
                }
                catch (Exception ex)
                {
                    //throw new ApplicationException("Unable to connect to the membership database. Please contact support");

                }
            }
        }
    }
}
