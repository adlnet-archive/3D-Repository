using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
namespace vwarDAL
{
    public class DataAccessFactory
    {
        private string ConnectionString { get { return ConfigurationManager.ConnectionStrings["vwarEntities"].ConnectionString; } }
        
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
        public IDataRepository CreateDataRepositorProxy()
        {

            return new FedoraCommonsRepo(FedoraUrl, FedoraUserName, FedoraPasswrod);
        }
    }
}
