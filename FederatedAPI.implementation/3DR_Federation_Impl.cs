using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using vwar.service.host;
using System.IO;
using System.Configuration;

namespace FederatedAPI.implementation
{
   
    public enum APIType { REST, SOAP };
    public class RequestFederationResponse
    {
        public int status;
        public string assignedPrefix;
        public string message;
    }
    public class ModifyFederationResponse
    {
        public int status;
        public string assignedPrefix;
        public string message;
    }
    public class ModifyFederationRequest
    {
        public string OrganizationPOCEmail;
        public string OrganizationPOCPassword;
        public string NamespacePrefix;
    }

    public class ApproveFederateResponse
    {
        public string message;
    }
    public class ApproveFederateRequest
    {
        public string OrganizationPOCEmail;
        public string OrganizationPOCPassword;
        public string NamespacePrefix;
    }

    [System.Runtime.Serialization.DataContractAttribute]
    public class GetAllFederatesResponse
    {
        [System.Runtime.Serialization.DataMember]
        public FederateRecord[] Federates;
        public GetAllFederatesResponse(List<FederateRecord> inlist)
        {

            Federates = new FederateRecord[inlist.Count];
            int count = 0;
            foreach (FederateRecord f in inlist)
            {
                Federates[count] = f;
                Federates[count].OrganizationPOCPassword = "********";
                count++;
            }
        }
    }
    public class _3DR_Federation_Impl
    {
        private static string AdminPassword = "$c0rmR0ck$";
        private static string AdminUsername = "FederationAdmin";
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
        public GetAllFederatesResponse GetAllFederates()
        {
            return new GetAllFederatesResponse(mFederateRegister.GetAllFederateRecords()); 
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

                    if (api == APIType.REST)
                        apibase = fr.RESTAPI;


                    return apibase + "/" + pid + "/" + function  ;
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

                    if (api == APIType.REST)
                        apibase = fr.RESTAPI;
                 
                    return apibase + "/" + pid + "/Format/" + format;
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

                if (api == APIType.REST)
                    apibase = fr.RESTAPI;

                return apibase + "/"+ pid + "/Model/" + format + "/" + options;
            }
            //"should never reach here"
            return fr.OrganizationURL;
        }
        public class SearchStart
        {
            public List<SearchResult> results;
            public FederateRecord fed;
            public string terms;
        }
        public void Search1Delegate(object frpair)
        {
            FederateRecord fr = ((SearchStart)frpair).fed;
            List<SearchResult> results = ((SearchStart)frpair).results;
            string terms = ((SearchStart)frpair).terms;

            System.Net.WebClient wc = new System.Net.WebClient();
            System.Runtime.Serialization.DataContractSerializer xmls = new System.Runtime.Serialization.DataContractSerializer(typeof(List<SearchResult>));


            if (fr.ActivationState == FederateState.Active && fr.AllowFederatedSearch == true)
            {
                try
                {
                    byte[] data = wc.DownloadData(fr.RESTAPI + "/Search/" + terms + "/xml?ID=00-00-00");
                    List<SearchResult> fed_results = (List<SearchResult>)xmls.ReadObject(new MemoryStream(data));

                    lock (((System.Collections.IList)results).SyncRoot)
                    {
                        foreach (SearchResult sr in fed_results)
                            results.Add(sr);
                    }
                    
                }
                catch (System.Net.WebException e)
                {
                    throw e;
                   // fr.ActivationState = FederateState.Offline;
                   // mFederateRegister.UpdateFederateRecord(fr);
                }
            }
        }
        //Search the repo for a list of pids that match a search term
        //This returns the results as a list of pairs of titles and pids
        //will eventually take a pagenum and other params for more advanced searching
        public List<SearchResult> Search(string terms, string key)
        {
            List<SearchResult> results = new List<SearchResult>();
            List<FederateRecord> federates = mFederateRegister.GetAllFederateRecords();
            List<System.Threading.Thread> threads = new List<System.Threading.Thread>();
            foreach (FederateRecord fr in federates)
            {
                System.Threading.ParameterizedThreadStart ts = new System.Threading.ParameterizedThreadStart(Search1Delegate);
                System.Threading.Thread t = new System.Threading.Thread(ts);

                SearchStart ss = new SearchStart();
                ss.terms = terms;
                ss.results = results;
                ss.fed = fr;
                //Search1Delegate(ss);
                t.Start(ss);
                threads.Add(t);
            }
            bool done = false;
            while (!done)
            {
                done = true;
                foreach (System.Threading.Thread t in threads)
                {
                    if (t.IsAlive)
                        done = false;
                }
                System.Threading.Thread.Sleep(300);
            }
            return results;
        }
        public enum RequestStatus { RequestAccepted, AlreadyRegistered, PrefixCollision, BadURL };
        public ModifyFederationResponse SetFederateState(ModifyFederationRequest request, FederateState state)
        {
            ModifyFederationResponse response = new ModifyFederationResponse();
            response.status = -1;
            response.message = "Not implemented";


           
            //check for collisions in the names of the services
            FederateRecord fr = mFederateRegister.GetFederateRecord(request.NamespacePrefix);
            
                if (fr.namespacePrefix == request.NamespacePrefix || request.NamespacePrefix == AdminUsername )
                {
                    if ((fr.OrganizationPOCEmail == request.OrganizationPOCEmail && fr.OrganizationPOCPassword == request.OrganizationPOCPassword) ||
                        request.OrganizationPOCEmail == AdminUsername && request.OrganizationPOCPassword == AdminPassword)
                    {
                        
                        
                            if (state == FederateState.Delisted)
                            {
                                if (fr.ActivationState == FederateState.Delisted)
                                {
                                    response.message = "Federate at " + fr.RESTAPI + " has already been delisted. Please contact the administrator if you would like to re-enable the account.";
                                    return response;
                                }
                                else
                                {
                                    fr.ActivationState = FederateState.Delisted;
                                    mFederateRegister.UpdateFederateRecord(fr);
                                    response.message = "Federate at " + fr.RESTAPI + " has been delisted. The namespace " + fr.namespacePrefix + " will be reserved for this account in the future.";
                                    return response;
                                }
                            }
                            if (state == FederateState.Active)
                            {
                                if (fr.ActivationState == FederateState.Active)
                                {
                                    response.message = "Federate at " + fr.RESTAPI + " is already active.";
                                    return response;
                                }
                                else if (fr.ActivationState == FederateState.Offline)
                                {
                                    fr.ActivationState = FederateState.Active;
                                    mFederateRegister.UpdateFederateRecord(fr);
                                    response.message = "Federate at " + fr.RESTAPI + " has been set as online.";
                                    return response;
                                }
                                else
                                {
                                    response.message = "Federate at " + fr.RESTAPI + " is awaiting admin approval, and cannot be set as online";
                                    return response;
                                }
                            }
                            if (state == FederateState.Offline)
                            {
                                if (fr.ActivationState == FederateState.Offline)
                                {
                                    response.message = "Federate at " + fr.RESTAPI + " is already offline.";
                                    return response;
                                }
                                else if (fr.ActivationState == FederateState.Active)
                                {
                                    fr.ActivationState = FederateState.Offline;
                                    mFederateRegister.UpdateFederateRecord(fr);
                                    response.message = "Federate at " + fr.RESTAPI + " has been set as offline.";
                                    return response;
                                }
                                else
                                {
                                    response.message = "Federate at " + fr.RESTAPI + " is awaiting admin approval, and cannot be set as offline";
                                    return response;
                                }
                            }
                        }
                        else
                        {
                            response.message = "Incorrect Password";
                            return response;
                        }
                    }
                else
                {
                    response.message = "Wrong user for this namespace";
                    return response;
                }
            
        

        response.message = "Namespace not found";
        return response;
           
        }
        public FederatedAPI.implementation.ApproveFederateResponse ApproveFederate(FederatedAPI.implementation.ApproveFederateRequest request)
        {
            ApproveFederateResponse response = new ApproveFederateResponse();
            if (request.OrganizationPOCEmail != AdminUsername)
            {
                response.message = "Only the Federation Admin may approve this federate.";
                return response;
            }
            if(request.OrganizationPOCPassword != AdminPassword)
            {
                response.message = "Incorrect Federation Admin password.";
                return response;
            }
                FederateRecord f = mFederateRegister.GetFederateRecord(request.NamespacePrefix);
                f.ActivationState = FederateState.Active;
                mFederateRegister.UpdateFederateRecord(f);
            response.message = "The federate has been activated";
            return response;
        }
        public ModifyFederationResponse ModifyFederate(ModifyFederationRequest request, string state)
        {
            return SetFederateState(request, (FederateState)System.Convert.ToInt16(state));       
        }
        
        public RequestFederationResponse RequestFederation(FederatedAPI.implementation.FederateRecord request)
        {
            RequestFederationResponse response = new RequestFederationResponse();
            List<FederateRecord> federates = mFederateRegister.GetAllFederateRecords();
            
            //check for collisions in the names of the services
            foreach (FederateRecord fr in federates)
            {
                if (fr.RESTAPI == request.RESTAPI && fr.ActivationState != FederateState.Delisted)
                {
                    response.message = "This URL is already registered to " + fr.OrginizationName + ". Please contact " + fr.OrganizationPOC + " at " + fr.OrganizationPOCEmail;
                    response.status = (int)RequestStatus.AlreadyRegistered; ;
                    return response;
                }
            }

            //check for collisions in the namespace prefix
            foreach (FederateRecord fr in federates)
            {
                if (fr.namespacePrefix == request.namespacePrefix && fr.ActivationState != FederateState.Delisted)
                {
                    response.message = "This prefix is already registered to " + fr.OrginizationName + ". Please choose a different prefix and try again";
                    response.status = (int)RequestStatus.PrefixCollision; ;
                    return response;
                }
            }

            //Check that the json api works
            try
            {
                System.Net.WebRequest req = System.Net.WebRequest.Create(request.RESTAPI + "/Search/test/json?ID=00-00-00");
                System.Net.HttpWebResponse res = (System.Net.HttpWebResponse)req.GetResponse();
                if (res.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    response.status = (int)RequestStatus.BadURL;
                    response.message = "Could not contact the API. Your API must be online and visible to register with the federation.";
                    return response;
                }
            }
            catch(SystemException e)
            {
                response.status = (int)RequestStatus.BadURL;
                response.message = "Could not contact the API. Your API must be online and visible to register with the federation.";
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