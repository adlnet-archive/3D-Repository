using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
namespace vwarDAL
{
    public class DataAccessFactory
    {
        
        private string FedoraManagementUrl
        {
            get
            {
                return (ConfigurationManager.AppSettings["vwarDAL_FedoraAPIM_Fedora_API_M_Service"]);
            }
        }
        private string FedoraAccessUrl
        {
            get
            {
                return (ConfigurationManager.AppSettings["vwarDAL_FedoraAPIM_Fedora_API_A_Service"]);
            }
        }
        private string FedoraUrl
        {
            get
            {
                return (ConfigurationManager.AppSettings["fedoraUrl"]);
            }
        }
        private string FedoraUserName
        {
            get
            {
                return (ConfigurationManager.AppSettings["fedoraUserName"]);
            }
        }
        private string FedoraPasswrod
        {
            get
            {
                return (ConfigurationManager.AppSettings["fedoraPassword"]);
            }
        }
        private string FedoraNamespace
        {
            get
            {
                return (ConfigurationManager.AppSettings["fedoraNamespace"]);
            }
        }
        private string ConnectionString
        {
            get
            {
                return (ConfigurationManager.ConnectionStrings["postgreSQLConnectionString"].ConnectionString);
            }
        }
        public IDataRepository CreateDataRepositorProxy()
        {

            return new FedoraCommonsRepo(FedoraUrl, FedoraUserName, FedoraPasswrod, FedoraAccessUrl,FedoraManagementUrl,ConnectionString,FedoraNamespace);
        }
        public ITempContentManager CreateTempContentManager()
        {
            return new TempWebContentManager(ConnectionString);
        }
    }
}
