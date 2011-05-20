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
    public class RequestFederationResponse
    {
        public int status;
        public string assignedPrefix;
        public string message;
    }
    public class _3DR_Federation_Impl
    {
        private FederateRegister mFederateRegister;
        //Constructor, create IDataproxy
        public _3DR_Federation_Impl()
        {
                mFederateRegister = new FederateRegister();
                //if (mFederateRegister.GetFederateRecord("adl") == null)
                //{
                //    FederateRecord adl = new FederateRecord();
                //    adl.JSONAPI = "http://localhost/3DRAPI/_3DRAPI_Json.svc";
                //    adl.XMLAPI = "http://localhost/3DRAPI/_3DRAPI_Xml.svc";
                //    adl.SOAPAPI = "http://localhost/3DRAPI/_3DRAPI_Soap.svc";
                //    adl.namespacePrefix = "adl";
                //    adl.OrganizationPOC = "Rob Chadwick";
                //    adl.OrganizationPOCEmail = "Rob.Chadwick.Ctr@adlnet.gov";
                //    adl.OrganizationPOCPassword = "none";
                //    adl.OrganizationURL = "http://www.adl.gov";
                //    adl.OrginizationName = "ADL";
                //    adl.AllowFederatedDownload = true;
                //    adl.AllowFederatedSearch = true;
                //    adl.ActivationState = FederateState.Active;
                //    mFederateRegister.CreateFederateRecord(adl);
                //}

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
                if (fr == null)
                    throw new System.Net.WebException("The prefix for this model is not recognized by the federation");

                if (fr.ActivationState == FederateState.Unapproved)
                {
                    throw new System.Net.WebException("The account for this namespace is waiting to be approved. Please visit <a href=\"" + fr.OrganizationURL + "\">" + fr.OrganizationURL + "</a> for more information.");
                }
                if (fr.AllowFederatedSearch != true)
                {
                    throw new System.Net.WebException("The account for this namespace is does not allow searching or downloading metadata through the federation. Please visit <a href=\"" + fr.OrganizationURL + "\">" + fr.OrganizationURL + "</a> for more information.");
                }

                if (fr.ActivationState == FederateState.Active && fr.AllowFederatedSearch == true)
                {
                    string apibase = "";

                    if (api == APIType.JSON)
                        apibase = fr.JSONAPI;
                    if (api == APIType.XML)
                        apibase = fr.XMLAPI;

                    return apibase + "/" + function + "/" + pid;
                }
                else
                {
                    return fr.OrganizationURL;
                }
        }
        public string GetRedirectAddressModel(APIType api, string pid,string format)
        {
          
                string fednamespace = GetPrefixFromPid(pid);
                FederateRecord fr = mFederateRegister.GetFederateRecord(fednamespace);
                if (fr == null)
                    throw new System.Net.WebException("The prefix for this model is not recognized by the federation");


                if (fr.ActivationState == FederateState.Unapproved)
                {
                    throw new System.Net.WebException("The account for this namespace is waiting to be approved. Please visit <a href=\"" + fr.OrganizationURL + "\">" + fr.OrganizationURL + "</a> for more information.");
                }
                if (fr.AllowFederatedDownload != true)
                {
                    throw new System.Net.WebException("The account for this namespace is does not allow downloading through the federation. Please visit <a href=\"" + fr.OrganizationURL + "\">" + fr.OrganizationURL + "</a> for more information.");
                }
                if (fr.ActivationState == FederateState.Active && fr.AllowFederatedDownload == true)
                {
                    string apibase = "";

                    if (api == APIType.JSON)
                        apibase = fr.JSONAPI;
                    if (api == APIType.XML)
                        apibase = fr.XMLAPI;

                    return apibase + "/" + pid + "/" + format;
                }
                //"should never reach here"
                return fr.OrganizationURL;
                
        }
        public string GetRedirectAddressModelAdvanced(APIType api, string pid, string format, string options)
        {
            string fednamespace = GetPrefixFromPid(pid);
            FederateRecord fr = mFederateRegister.GetFederateRecord(fednamespace);
            if(fr == null)
                throw new System.Net.WebException("The prefix for this model is not recognized by the federation");

            if (fr.ActivationState == FederateState.Unapproved)
            {
                throw new System.Net.WebException("The account for this namespace is waiting to be approved. Please visit <a href=\""+fr.OrganizationURL+"\">" +fr.OrganizationURL+"</a> for more information.");
            }
            if (fr.AllowFederatedDownload != true)
            {
                throw new System.Net.WebException("The account for this namespace is does not allow downloading through the federation. Please visit <a href=\"" + fr.OrganizationURL + "\">" + fr.OrganizationURL + "</a> for more information.");
            }
            if (fr.ActivationState == FederateState.Active && fr.AllowFederatedDownload == true)
            {
                string apibase = "";

                if (api == APIType.JSON)
                    apibase = fr.JSONAPI;
                if (api == APIType.XML)
                    apibase = fr.XMLAPI;

                return apibase + "/Model/" + pid + "/" + format + "/" + options;
            }
            //"should never reach here"
            return fr.OrganizationURL;
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
                if (fr.ActivationState == FederateState.Active && fr.AllowFederatedSearch == true)
                {
                    byte[] data = wc.DownloadData(fr.XMLAPI + "/Search/" + terms);
                    List<SearchResult> fed_results = (List<SearchResult>)xmls.ReadObject(new MemoryStream(data));

                    foreach (SearchResult sr in fed_results)
                        results.Add(sr);
                }
            }
            return results;
        }
        public enum RequestStatus { RequestAccepted, AlreadyRegistered, PrefixCollision, BadURL };
        public RequestFederationResponse RequestFederation(FederatedAPI.implementation.FederateRecord request)
        {
            RequestFederationResponse response = new RequestFederationResponse();
            List<FederateRecord> federates = mFederateRegister.GetAllFederateRecords();
            
            //check for collisions in the names of the services
            foreach (FederateRecord fr in federates)
            {
                if (fr.JSONAPI == request.JSONAPI || fr.XMLAPI == request.XMLAPI)
                {
                    response.message = "This URL is already registered to " + fr.OrginizationName + ". Please contact " + fr.OrganizationPOC + " at " + fr.OrganizationPOCEmail;
                    response.status = (int)RequestStatus.AlreadyRegistered; ;
                    return response;
                }
            }

            //check for collisions in the namespace prefix
            foreach (FederateRecord fr in federates)
            {
                if (fr.namespacePrefix == request.namespacePrefix)
                {
                    response.message = "This prefix is already registered to " + fr.OrginizationName + ". Please choose a different prefix and try again";
                    response.status = (int)RequestStatus.PrefixCollision; ;
                    return response;
                }
            }

            //Check that the json api works
            try
            {
                System.Net.WebRequest req = System.Net.WebRequest.Create(request.JSONAPI);
                System.Net.HttpWebResponse res = (System.Net.HttpWebResponse)req.GetResponse();
                if (res.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    response.status = (int)RequestStatus.BadURL;
                    response.message = "Could not contact the JSON API. Your API must be online and visible to register with the federation.";
                    return response;
                }
            }
            catch(SystemException e)
            {
                response.status = (int)RequestStatus.BadURL;
                response.message = "Could not contact the JSON API. Your API must be online and visible to register with the federation.";
                return response;
            }

            //check that the xml api works
            try
            {
                System.Net.WebRequest req1 = System.Net.WebRequest.Create(request.XMLAPI);
                System.Net.HttpWebResponse res1 = (System.Net.HttpWebResponse)req1.GetResponse();
                if (res1.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    response.status = (int)RequestStatus.BadURL;
                    response.message = "Could not contact the XML API. Your API must be online and visible to register with the federation.";
                    return response;
                }
            }
            catch (SystemException e)
            {
                response.status = (int)RequestStatus.BadURL;
                response.message = "Could not contact the XML API. Your API must be online and visible to register with the federation.";
                return response;
            }

            //Create the record
            request.ActivationState = FederateState.Unapproved;
            mFederateRegister.CreateFederateRecord(request);
            
            response.assignedPrefix = request.namespacePrefix;
            response.status = (int)RequestStatus.RequestAccepted;
            response.message = "You have been assigned the prefix: " + response.assignedPrefix + "<br/>You will receive an email when the administrator enables this account<br/>status: " + response.status.ToString();
            return response;
        }
    }
}