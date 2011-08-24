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
using System.Text;
using System.Configuration;
namespace vwarDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class DataAccessFactory
    {
        /// <summary>
        /// 
        /// </summary>
        private string FedoraManagementUrl
        {
            get
            {
                return (ConfigurationManager.AppSettings["vwarDAL_FedoraAPIM_Fedora_API_M_Service"]);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private string FedoraAccessUrl
        {
            get
            {
                return (ConfigurationManager.AppSettings["vwarDAL_FedoraAPIM_Fedora_API_A_Service"]);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private string FedoraUrl
        {
            get
            {
                return (ConfigurationManager.AppSettings["fedoraUrl"]);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private string FedoraUserName
        {
            get
            {
                return (ConfigurationManager.AppSettings["fedoraUserName"]);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private string FedoraPassword
        {
            get
            {
                return (ConfigurationManager.AppSettings["fedoraPassword"]);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private string FedoraNamespace
        {
            get
            {
                return (ConfigurationManager.AppSettings["fedoraNamespace"]);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private string ConnectionString
        {
            get
            {
                return (ConfigurationManager.ConnectionStrings["postgreSQLConnectionString"].ConnectionString);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDataRepository CreateDataRepositorProxy()
        {

            return new FedoraCommonsRepo(FedoraUrl, FedoraUserName, FedoraPassword, FedoraAccessUrl, FedoraManagementUrl, ConnectionString, FedoraNamespace);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ITempContentManager CreateTempContentManager()
        {
            return new TempWebContentManager(ConnectionString);
        }
    }
}
