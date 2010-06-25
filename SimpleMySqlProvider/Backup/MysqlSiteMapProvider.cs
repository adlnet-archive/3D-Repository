using System;
using System.Web;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Configuration;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Configuration.Provider;
using System.Security.Permissions;
using System.Data.Common;
using System.Data.Odbc;
using System.Data;

namespace Simple.Providers.MySQL
{

    public class MySqlSiteMapProvider : StaticSiteMapProvider
    {
        private const string _errmsg1 = "Missing node ID";
        private const string _errmsg2 = "Duplicate node ID";
        private const string _errmsg3 = "Missing parent ID";
        private const string _errmsg4 = "Invalid parent ID";
        private const string _errmsg5 =
            "Empty or missing connectionStringName";
        private const string _errmsg6 = "Missing connection string";
        private const string _errmsg7 = "Empty connection string";

        private string _applicationName;
        private string _connect;
        private int _indexID, _indexTitle, _indexUrl,
            _indexDesc, _indexRoles, _indexParent;
        private Dictionary<int, SiteMapNode> _nodes =
            new Dictionary<int, SiteMapNode>(16);
        private SiteMapNode _root;

        public override void Initialize(string name,
            NameValueCollection config)
        {
            // Verify that config isn't null
            if (config == null)
                throw new ArgumentNullException("config");

            // Assign the provider a default name if it doesn't have one
            if (String.IsNullOrEmpty(name))
                name = "SimpleMySqlSiteMapProvider";

            // Add a default "description" attribute to config if the
            // attribute doesn't exist or is empty
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Simple MySQL site map provider");
            }

            // Call the base class's Initialize method
            base.Initialize(name, config);

            // Initialize _connect
            string connect = config["connectionStringName"];

            if (String.IsNullOrEmpty(connect))
                throw new ProviderException(_errmsg5);

            config.Remove("connectionStringName");

            if (WebConfigurationManager.ConnectionStrings[connect] == null)
                throw new ProviderException(_errmsg6);

            _connect = WebConfigurationManager.ConnectionStrings
                [connect].ConnectionString;

            if (String.IsNullOrEmpty(_connect))
                throw new ProviderException(_errmsg7);

            // In beta 2, SiteMapProvider processes the
            // securityTrimmingEnabled attribute but fails to remove it.
            // Remove it now so we can check for unrecognized
            // configuration attributes.

            //if (config["securityTrimmingEnabled"] != null)
            //    config.Remove("securityTrimmingEnabled");

            if (config["applicationName"] != null)
            {
                _applicationName = config["applicationName"];
                config.Remove("applicationName");
            }
            // Throw an exception if unrecognized attributes remain
            if (config.Count > 0)
            {
                string attr = config.GetKey(0);
                if (!String.IsNullOrEmpty(attr))
                    throw new ProviderException
                        ("Unrecognized attribute: " + attr);
            }
        }

        public override SiteMapNode BuildSiteMap()
        {
            lock (this)
            {
                // Return immediately if this method has been called before
                if (_root != null)
                    return _root;

                // Query the database for site map nodes

                string sSQL = null;
                DataSet reader = null;
                try
                {
                    sSQL = "SELECT * FROM `SiteMap` WHERE `ApplicationName` = '"+_applicationName+"' ORDER BY `Parent`;";

                    reader = RawDBQuery.ExecuteDatasetQueryOnDB(sSQL, _connect);
                    _indexID = 0;
                    _indexTitle = 2;
                    _indexDesc = 3;
                    _indexUrl = 4;
                    _indexRoles = 5;
                    _indexParent = 6;

                    int iCount = 0;
                    foreach (DataRow dr in reader.Tables[0].Rows)
                    {
                        if (iCount == 0)
                        {
                            _root = CreateSiteMapNodeFromDataRow(dr);
                            AddNode(_root, null);
                        }
                        else
                        {
                            // Create another site map node and
                            // add it to the site map
                            SiteMapNode node =
                                CreateSiteMapNodeFromDataRow(dr);
                            AddNode(node,
                                GetParentNodeFromDataRow(dr));
                        }
                        iCount++;
                    }
                }
                finally
                {
                    if (reader != null) reader.Dispose();
                    reader = null;
                    sSQL = null;
                }

                // Return the root SiteMapNode
                return _root;
            }
        }

        private SiteMapNode GetParentNodeFromDataRow(DataRow dr)
        {
            // Make sure the parent ID is present
            if (dr.IsNull(_indexParent))
                throw new ProviderException(_errmsg3);

            // Get the parent ID from the DataReader
            int pid = int.Parse(dr[_indexParent].ToString());

            // Make sure the parent ID is valid
            if (!_nodes.ContainsKey(pid))
                throw new ProviderException(_errmsg4);

            // Return the parent SiteMapNode
            return _nodes[pid];
        }

        private SiteMapNode CreateSiteMapNodeFromDataRow(DataRow dr)
        {
            // Make sure the node ID is present
            if (dr.IsNull(_indexID))
                throw new ProviderException(_errmsg1);

            // Get the node ID from the DataReader
            int id = int.Parse(dr[_indexID].ToString());

            // Make sure the node ID is unique
            if (_nodes.ContainsKey(id))
                throw new ProviderException(_errmsg2);

            // Get title, URL, description, and roles from the DataReader
            string title = dr.IsNull(_indexTitle) ?
                null : dr[_indexTitle].ToString().Trim();
            string url = dr.IsNull(_indexUrl) ?
                null : dr[_indexUrl].ToString().Trim();
            string description = dr.IsNull(_indexDesc) ?
                null : dr[_indexDesc].ToString().Trim();
            string roles = dr.IsNull(_indexRoles) ?
                null : dr[_indexRoles].ToString().Trim();

            // If roles were specified, turn the list into a string array
            string[] rolelist = null;
            if (!String.IsNullOrEmpty(roles))
                rolelist = roles.Split(new char[] { ',', ';' }, 512);

            // Create a SiteMapNode
            SiteMapNode node = new SiteMapNode(this, id.ToString(), url,
                title, description, rolelist, null, null, null);

            // Record the node in the _nodes dictionary
            _nodes.Add(id, node);

            // Return the node        
            return node;
        }

        protected override SiteMapNode GetRootNodeCore()
        {
            BuildSiteMap();
            return _root;
        }

        // Helper methods
        private SiteMapNode
            CreateSiteMapNodeFromDataReader(DbDataReader reader)
        {
            // Make sure the node ID is present
            if (reader.IsDBNull(_indexID))
                throw new ProviderException(_errmsg1);

            // Get the node ID from the DataReader
            int id = reader.GetInt32(_indexID);

            // Make sure the node ID is unique
            if (_nodes.ContainsKey(id))
                throw new ProviderException(_errmsg2);

            // Get title, URL, description, and roles from the DataReader
            string title = reader.IsDBNull(_indexTitle) ?
                null : reader.GetString(_indexTitle).Trim();
            string url = reader.IsDBNull(_indexUrl) ?
                null : reader.GetString(_indexUrl).Trim();
            string description = reader.IsDBNull(_indexDesc) ?
                null : reader.GetString(_indexDesc).Trim();
            string roles = reader.IsDBNull(_indexRoles) ?
                null : reader.GetString(_indexRoles).Trim();

            // If roles were specified, turn the list into a string array
            string[] rolelist = null;
            if (!String.IsNullOrEmpty(roles))
                rolelist = roles.Split(new char[] { ',', ';' }, 512);

            // Create a SiteMapNode
            SiteMapNode node = new SiteMapNode(this, id.ToString(), url,
                title, description, rolelist, null, null, null);

            // Record the node in the _nodes dictionary
            _nodes.Add(id, node);

            // Return the node        
            return node;
        }

        private SiteMapNode
            GetParentNodeFromDataReader(DbDataReader reader)
        {
            // Make sure the parent ID is present
            if (reader.IsDBNull(_indexParent))
                throw new ProviderException(_errmsg3);

            // Get the parent ID from the DataReader
            int pid = reader.GetInt32(_indexParent);

            // Make sure the parent ID is valid
            if (!_nodes.ContainsKey(pid))
                throw new ProviderException(_errmsg4);

            // Return the parent SiteMapNode
            return _nodes[pid];
        }
    }
}