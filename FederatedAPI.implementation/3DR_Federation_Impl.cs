using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using vwar.service.host;
using System.IO;
using System.Configuration;

namespace FederatedAPI.implementation
{
    public enum APIType { JSON, XML, SOAP };
    public class _3DR_Federation_Impl
    {
        private FederateRegister mFederateRegister;
        //Constructor, create IDataproxy
        public _3DR_Federation_Impl()
        {
                mFederateRegister = new FederateRegister();
                if (mFederateRegister.GetFederateRecord("adl") == null)
                {
                    mFederateRegister.CreateFederateRecord("adl","http://localhost/3DRAPI/_3DRAPI_Json.svc","http://localhost/3DRAPI/_3DRAPI_Xml.svc","http://localhost/3DRAPI/_3DRAPI_Soap.svc");
                }

        }
        public string GetPrefixFromPid(string PID)
        {
            string[] list = PID.Split(new char[] { ':','_' });
            return list[0];
        }
        public string GetRedirectAddress(string function, APIType api, string pid)
        {
            pid = pid.Replace('_', ':');
            string fednamespace = GetPrefixFromPid(pid);
            FederateRecord fr = mFederateRegister.GetFederateRecord(fednamespace);
            string apibase = "";

            if (api == APIType.JSON)
                apibase = fr.JSONAPI;
            if (api == APIType.XML)
                apibase = fr.XMLAPI;

            return apibase + "/"+function+"/" + pid;          
        }
        public string GetRedirectAddressModel(APIType api, string pid,string format)
        {
            string fednamespace = GetPrefixFromPid(pid);
            FederateRecord fr = mFederateRegister.GetFederateRecord(fednamespace);
            string apibase = "";

            if (api == APIType.JSON)
                apibase = fr.JSONAPI;
            if (api == APIType.XML)
                apibase = fr.XMLAPI;

            return apibase + "/" +pid+ "/" +format;
        }
        public string GetRedirectAddressModelAdvanced(APIType api, string pid, string format, string options)
        {
            string fednamespace = GetPrefixFromPid(pid);
            FederateRecord fr = mFederateRegister.GetFederateRecord(fednamespace);
            string apibase = "";

            if (api == APIType.JSON)
                apibase = fr.JSONAPI;
            if (api == APIType.XML)
                apibase = fr.XMLAPI;

            return apibase + "/Model/" + pid + "/" + format + "/" + options;
        }
        //Search the repo for a list of pids that match a search term
        //This returns the results as a list of pairs of titles and pids
        //will eventually take a pagenum and other params for more advanced searching
        public List<SearchResult> Search(string terms)
        {
            List<SearchResult> results = new List<SearchResult>();
            List<FederateRecord> federates = mFederateRegister.GetAllFederateRecords();
            System.Net.WebClient wc = new System.Net.WebClient();
            System.Runtime.Serialization.DataContractSerializer xmls = new System.Runtime.Serialization.DataContractSerializer(typeof(List<SearchResult>));
            foreach (FederateRecord fr in federates)
            {
                byte[] data = wc.DownloadData(fr.XMLAPI + "/Search/" + terms);
                List<SearchResult> fed_results = (List<SearchResult>)xmls.ReadObject(new MemoryStream(data));

                foreach (SearchResult sr in fed_results)
                    results.Add(sr);
            }
            return results;
        }
        
    }
}