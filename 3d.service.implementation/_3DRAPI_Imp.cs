using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Web;
using System.IO;
using System.Configuration;
using System.Security.Cryptography;
using System.Drawing;
namespace vwar.service.host
{
    public class _3DRAPI_Imp
    {
        //The IDataRepository that holds all the files, and stores the data
        public vwarDAL.IDataRepository FedoraProxy1;
        private bool _IgnoreAuth = false;
        
         ~_3DRAPI_Imp()
        {
            if (FedoraProxy1 != null)
                FedoraProxy1.Dispose();
        }
        public void Dispose()
        {
            if (FedoraProxy1 != null)
                FedoraProxy1.Dispose();
        }
        //Constructor, create IDataproxy
        public _3DRAPI_Imp(bool ignoreAuth = false)
        {
            _IgnoreAuth = ignoreAuth;
           
            
          //  PermManager = new PermissionsManager();
        }
        public vwarDAL.IDataRepository GetRepo()
        {
            if (FedoraProxy1 != null)
                return FedoraProxy1;

            vwarDAL.DataAccessFactory dalf = new vwarDAL.DataAccessFactory();
            FedoraProxy1 = dalf.CreateDataRepositorProxy();
            return FedoraProxy1;
        }
        public void ReleaseRepo()
        {
            if (FedoraProxy1 != null)
                FedoraProxy1.Dispose();
            FedoraProxy1 = null;
        }
        public virtual void SetResponseHeaders(string type, int length, string disposition)
        {

            WebOperationContext.Current.OutgoingResponse.ContentType = type;
            WebOperationContext.Current.OutgoingResponse.ContentLength = length;
            WebOperationContext.Current.OutgoingResponse.Headers["Content-disposition"]= disposition;         

        }
        public virtual bool CheckKey(string key)
        {
            APIKeyManager KeyManager = new APIKeyManager();
            if (key == null)
            {
                WebOperationContext.Current.OutgoingResponse.Headers[System.Net.HttpResponseHeader.WwwAuthenticate] = "BASIC realm=\"3DR API\"";
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                // throw new WebFaultException(System.Net.HttpStatusCode.Unauthorized);
                return false;
            }
            if (KeyManager.GetUserByKey(key) == null)
            {
                WebOperationContext.Current.OutgoingResponse.Headers[System.Net.HttpResponseHeader.WwwAuthenticate] = "BASIC realm=\"3DR API\"";
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Unauthorized;
               // throw new WebFaultException(System.Net.HttpStatusCode.Unauthorized);
                return false;
            }
            KeyManager.Dispose();
            return true;
        }
        private string GetUserEmail()
        {
             //Return note about the authorization scheme used
            WebOperationContext.Current.OutgoingResponse.Headers[System.Net.HttpResponseHeader.WwwAuthenticate] = "BASIC realm=\"3DR API\"";

            //Start by assuming anonymous
            string username = vwarDAL.DefaultUsers.Anonymous[0];
            string password = "";

            //if there is an auth header, check it
            if (WebOperationContext.Current.IncomingRequest.Headers[System.Net.HttpRequestHeader.Authorization] != null)
            {
                //string should start with "BASIC ", remove this
                string auth = WebOperationContext.Current.IncomingRequest.Headers[System.Net.HttpRequestHeader.Authorization].Substring(6);
                System.Text.Encoding enc = System.Text.Encoding.ASCII;
                //Decode from base64
                auth = enc.GetString(System.Convert.FromBase64String(auth));
                username = auth.Split(new char[] { ':' })[0];
                password = auth.Split(new char[] { ':' })[1];

                //Dont bother checking password for anonymous
                if (username != vwarDAL.DefaultUsers.Anonymous[0])
                {
                    //Get the membership provider
                    Simple.Providers.MySQL.MysqlMembershipProvider provider = (Simple.Providers.MySQL.MysqlMembershipProvider)System.Web.Security.Membership.Providers["MysqlMembershipProvider"];

                    return provider.GetUser(username,false).Email;
                }
                else
                {
                    return username;
                }
            }

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["AssumeAnonymousUserWhenMissingAuthHeader"]))
                return vwarDAL.DefaultUsers.Anonymous[0];

            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Unauthorized;
            return "";
        }

       
        private static string Base64EncodeHash(string url)
        {
            byte[] result;
            HMACSHA1 shaM = new HMACSHA1();

            byte[] ms = new byte[url.Length];

            for (int i = 0; i < url.Length; i++)
            {
                byte b = Convert.ToByte(url[i]);
                ms[i] = (b);
            }

            shaM.Initialize();

            result = shaM.ComputeHash(ms, 0, ms.Length);

            
            
            return System.Convert.ToBase64String(result); ;

        }
        public virtual string GetUsername()
        {
            //Return note about the authorization scheme used
            WebOperationContext.Current.OutgoingResponse.Headers[System.Net.HttpResponseHeader.WwwAuthenticate] = "BASIC realm=\"3DR API\"";

            //Start by assuming anonymous
            string username = vwarDAL.DefaultUsers.Anonymous[0];
            string password = "";

            //if there is an auth header, check it
            if (WebOperationContext.Current.IncomingRequest.Headers[System.Net.HttpRequestHeader.Authorization] != null)
            {
                //string should start with "BASIC ", remove this
                string auth = WebOperationContext.Current.IncomingRequest.Headers[System.Net.HttpRequestHeader.Authorization].Substring(6);
                System.Text.Encoding enc = System.Text.Encoding.ASCII;
                //Decode from base64
                auth = enc.GetString(System.Convert.FromBase64String(auth));
                username = auth.Split(new char[] { ':' })[0];
                password = auth.Split(new char[] { ':' })[1];

                //Dont bother checking password for anonymous
                if (username != vwarDAL.DefaultUsers.Anonymous[0])
                {
                    //Get the membership provider
                    Simple.Providers.MySQL.MysqlMembershipProvider provider = (Simple.Providers.MySQL.MysqlMembershipProvider)System.Web.Security.Membership.Providers["MysqlMembershipProvider"];

                    //Check if the suer is logged in correctly
                    bool validate = provider.ValidateUser(username, password);
                    //if they did not validate, then return false and send 401
                    if (!validate)
                    {
                        WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                        return "";
                    }
                    else
                    {
                        return username;
                    }
                }
                else
                {
                    return username;
                }
            }

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["AssumeAnonymousUserWhenMissingAuthHeader"]))
                return vwarDAL.DefaultUsers.Anonymous[0];

            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Unauthorized;
            return "";
        }

        /// <summary>
        /// User basic HTTP authorization, reads the header and does the auth
        /// </summary>
        /// <param name="type">The transaction type to validate</param>
        /// <param name="co">the content object to validate the operation on</param>
        /// <returns>True if the user may perform this operation on the contentobject</returns>
        public virtual bool DoValidate(Security.TransactionType type, vwarDAL.ContentObject co)
        {
            //Return note about the authorization scheme used
            WebOperationContext.Current.OutgoingResponse.Headers[System.Net.HttpResponseHeader.WwwAuthenticate] = "BASIC realm=\"3DR API\"";

            //Start by assuming anonymous
            string username = vwarDAL.DefaultUsers.Anonymous[0];
            string password = "";

            //if there is an auth header, check it
            if (WebOperationContext.Current.IncomingRequest.Headers[System.Net.HttpRequestHeader.Authorization] != null)
            {
                //string should start with "BASIC ", remove this
                string auth = WebOperationContext.Current.IncomingRequest.Headers[System.Net.HttpRequestHeader.Authorization].Substring(6);
                System.Text.Encoding enc = System.Text.Encoding.ASCII;
                //Decode from base64
                auth = enc.GetString( System.Convert.FromBase64String(auth));
                username = auth.Split(new char[] { ':' })[0];
                password = auth.Split(new char[] { ':' })[1];

                //Dont bother checking password for anonymous
                if (username != vwarDAL.DefaultUsers.Anonymous[0])
                {
                    //Get the membership provider
                    Simple.Providers.MySQL.MysqlMembershipProvider provider =  (Simple.Providers.MySQL.MysqlMembershipProvider)System.Web.Security.Membership.Providers["MysqlMembershipProvider"];
                    
                    //Check if the suer is logged in correctly
                    bool validate = provider.ValidateUser(username, password);
                    //if they did not validate, then return false and send 401
                    if (!validate)
                    {
                        WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                        return false;

                    }
                    
                }
            }

            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["AssumeAnonymousUserWhenMissingAuthHeader"]))
            {
                //This will force uses to enter the username AnonymousUser! if you want to just assume it when there is no
                //header, just remove this block,
                if (WebOperationContext.Current.IncomingRequest.Headers[System.Net.HttpRequestHeader.Authorization] == null)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    return false;
                }
            }

            //Do the actual check of permissions
            vwarDAL.PermissionsManager prm = new vwarDAL.PermissionsManager();
            if (type != Security.TransactionType.Create)
            {
                vwarDAL.ModelPermissionLevel Permission = prm.GetPermissionLevel(username, co.PID);
                prm.Dispose();
                if (type == Security.TransactionType.Query && Permission >= vwarDAL.ModelPermissionLevel.Searchable)
                {
                    return true;
                }
                if (type == Security.TransactionType.Access && Permission >= vwarDAL.ModelPermissionLevel.Fetchable)
                {
                    return true;
                }
                if (type == Security.TransactionType.Modify && Permission >= vwarDAL.ModelPermissionLevel.Editable)
                {
                    return true;
                }
                if (type == Security.TransactionType.Delete && Permission >= vwarDAL.ModelPermissionLevel.Admin)
                {
                    return true;
                }
                if (type == Security.TransactionType.Create && Permission >= vwarDAL.ModelPermissionLevel.Admin)
                {
                    return true;
                }
            }
            prm.Dispose();
            //If asking for create permission, and got here,then it must be a valid user. But, can't be anon.
            if (type == Security.TransactionType.Create)
            {
                if (username == vwarDAL.DefaultUsers.Anonymous[0])
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    return false;
                }
                else return true;
            }

            //Set the status if they are not authourized
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Unauthorized;
            return false;

        }
        //Delete a content object from the repository
        public string DeleteObject(string pid)
        {

            //Get the content object
            vwarDAL.ContentObject co = GetRepo().GetContentObjectById(pid, false);

            //Check permissions
            if (!DoValidate( Security.TransactionType.Delete, co))
                return null;

            //Remove it
            co.RemoveFromRepo();
            ReleaseRepo();
            return "ok";
        }
        //A simpler url for retrieving a model
        public Stream GetModelSimple(string pid, string format, string key)
        {
            return GetModel(pid, format, "",key);
        }
        //Get the content for a model
        public Stream GetTextureFile(string pid, string filename, string key)
        {
            if (!CheckKey(key))
                return null;

            pid = pid.Replace('_', ':');

            //Get the content object
            vwarDAL.ContentObject co = GetRepo().GetContentObjectById(pid, false);

            //Check permissions
            if (!DoValidate(Security.TransactionType.Access, co))
                return null;

            //Check that this content object actually has a content file
            if (co.Location != "")
            {
                Stream ms = co.GetContentFile();

                Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(ms);
                foreach (Ionic.Zip.ZipEntry ze in zip)
                {
                    if (ze.FileName.ToLower() == filename.ToLower())
                    {
                        MemoryStream texture = new MemoryStream();
                        ze.Extract(texture);
                        
                        SetResponseHeaders(GetMimeType(ze.FileName.ToLower()),(int)texture.Length,"attachment; filename=" + ze.FileName.ToLower());
                        
                        texture.Seek(0, SeekOrigin.Begin);
                        ReleaseRepo();
                        return texture as Stream;
                    }
                }
            }
            ReleaseRepo();
            return null;
        }
        public bool Is3DFile(string extension)
        {
            if (extension.ToLower() == ".dae") return true;
            if (extension.ToLower() == ".obj") return true;
            if (extension.ToLower() == ".3ds") return true;
            if (extension.ToLower() == ".json") return true;
            if (extension.ToLower() == ".fbx") return true;
            if (extension.ToLower() == ".flt") return true;

            return false;
        }
        //Get the content for a model
        public Stream GetModel(string pid, string format, string options, string key)
        {
            if (!CheckKey(key))
                return null;
            pid = pid.Replace('_', ':');

            //Get the content object
            vwarDAL.ContentObject co = GetRepo().GetContentObjectById(pid, false);

            //Check permissions
            if (!DoValidate(Security.TransactionType.Access, co))
            {
                ReleaseRepo();
                return null;
            }

            MemoryStream ms = null;
            //Check that this content object actually has a content file
            if (co.Location != "")
            {

                //If they want the dae file, they can get just the contnet file
                if (format.ToLower() == "dae" || format.ToLower() == "collada")
                {
                    ms = (MemoryStream)co.GetContentFile();
                    
                    SetResponseHeaders(GetMimeType(co.Location), (int)ms.Length, "attachment; filename=" + co.Location);
                    //return ms as Stream;
                }
                //note that if the options string is anything, then the cached display file is not the one the client needs
                else if ((format.ToLower() == "o3d" || format.ToLower() == "o3dtgz") && options == "")
                {
                    ms = (MemoryStream)GetRepo().GetCachedContentObjectTransform(co, "o3d");
                   
                    SetResponseHeaders(GetMimeType(co.DisplayFile), (int)ms.Length, "attachment; filename=" + co.DisplayFile);
                    //no point following on to try to uncompress this - it's already uncompressed
                    ReleaseRepo();
                    return ms as Stream;
                }
                //If they want any type other than the ones above, do the conversion
                else
                {
                    //Get the base content file
                    Stream unconvertedData = co.GetContentFile();
                    //setup the conversion system
                    Utility_3D _3d = new Utility_3D();
                    _3d.Initialize(ConfigurationManager.AppSettings["LibraryLocation"]);
                    Utility_3D.Model_Packager converter = new Utility_3D.Model_Packager();
                    Utility_3D.ConverterOptions opts = new Utility_3D.ConverterOptions();
                    //No need to gather metadata during this conversion, which slows down the conversion significantly
                    //opts.DisableMetadataGathering();

                    //Try to convert the model to the requested format
                    Utility_3D.ConvertedModel model;
                    try
                    {
                        model = converter.Convert(unconvertedData, co.Location, format, opts);
                    }
                    catch (Utility_3D.ConversionException e)
                    {
                        throw new System.Net.WebException(e.what());
                    }

                    //Looks like the conversion worked.
                    SetResponseHeaders(GetMimeType(co.DisplayFile + "." + format), (int)model.data.Length, "attachment; filename=" + co.Location);
                    //Return the new data
                    ms = new MemoryStream(model.data);
                    //return ms as Stream;
                    GetRepo().IncrementDownloads(co.PID);
                    ReleaseRepo();
                }
            }

            //
            if (options == "uncompressed")
            {
                ms.Seek(0, SeekOrigin.Begin);
                Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(ms);
                foreach (Ionic.Zip.ZipEntry ze in zip)
                {
                    if (Is3DFile(Path.GetExtension(ze.FileName.ToLower())))
                    {
                        MemoryStream model = new MemoryStream();
                        ze.Extract(model);
                       
                        SetResponseHeaders(GetMimeType(ze.FileName.ToLower()), (int)model.Length, "attachment; filename=" + ze.FileName.ToLower());
                        model.Seek(0, SeekOrigin.Begin);
                        ReleaseRepo();
                        return model as Stream;
                    }
                }
            }
            //There is no content for this content object
           
            return ms;
            
        }
        //Get the screenshot for a content object
        public Stream GetOriginalUploadFile(string pid, string key)
        {

            if (!CheckKey(key))
                return null;
            pid = pid.Replace('_', ':');
            //Get the object
            vwarDAL.ContentObject co = GetRepo().GetContentObjectById(pid, false);
            //Check permissions
            if (!DoValidate(Security.TransactionType.Access, co))
            {
                ReleaseRepo();
                return null;
            }
            //Set the headers and reutnr the stream
          
            Stream data =  co.GetOriginalUploadFile();
            if (data == null)
                data = GetRepo().GetContentFile(pid, co.OriginalFileName);

            SetResponseHeaders(GetMimeType(co.OriginalFileName), (int)data.Length, "attachment; filename=" + co.OriginalFileName);
            GetRepo().IncrementDownloads(co.PID);
            ReleaseRepo();
            return data;
        }
        //Get the screenshot for a content object
        public Stream GetScreenshot(string pid, string key)
        {
            if (!CheckKey(key))
                return null;
            pid = pid.Replace('_', ':');
            //Get the object
            vwarDAL.ContentObject co = GetRepo().GetContentObjectById(pid, false);
            //Check permissions
            if (!DoValidate(Security.TransactionType.Query, co))
            {
                ReleaseRepo();
                return null;
            }
           

            Stream thumb = co.GetScreenShotFile();
            if (thumb == null || thumb.Length == 0)
                thumb = (new vwarDAL.DataAccessFactory()).CreateDataRepositorProxy().GetContentFile(co.PID, co.ScreenShotId);
            //Set the headers and reutnr the stream
           
            SetResponseHeaders(GetMimeType(co.ScreenShot), (int)thumb.Length, "attachment; filename=" + co.ScreenShot);
            if (thumb == null || thumb.Length == 0)
            {
                if (WebOperationContext.Current != null)
                {
                    WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-disposition", "attachment; filename=" + "nopreview.png");
                    WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-type", GetMimeType("nopreview.png"));
                }
                thumb = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\nopreview_icon.png"), FileMode.Open, FileAccess.Read);
                SetResponseHeaders(GetMimeType("nopreview_icon.png"), (int)thumb.Length, "attachment; filename=" + "nopreview_icon.png");
            }
            ReleaseRepo();
            return thumb;
        }
        //Get the thumbnail for a content object
        public Stream GetThumbnail(string pid, string key)
        {
            
            if (!CheckKey(key))
                return null;
            
            pid = pid.Replace('_', ':');
            //Get the object
            vwarDAL.ContentObject co = GetRepo().GetContentObjectById(pid, false);
            
            //Check permissions
            if (!DoValidate(Security.TransactionType.Query, co))
            {
                ReleaseRepo();
                return null;
            }

          

            Stream thumb = GetRepo().GetContentFile(pid, co.ThumbnailId);
            if (thumb == null || thumb.Length == 0)
            {
                thumb = GetRepo().GetContentFile(pid, co.Thumbnail);
            }
            if (thumb == null || thumb.Length == 0)
            {
                thumb = GetRepo().GetContentFile(pid, co.ScreenShotId);
            }
            if (thumb == null || thumb.Length == 0)
            {
                thumb = GetRepo().GetContentFile(pid, co.Thumbnail);
            }
            if (thumb == null || thumb.Length == 0)
            {
                thumb = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\nopreview_icon.png"), FileMode.Open, FileAccess.Read);
            }
            ReleaseRepo();
            SetResponseHeaders(GetMimeType(co.ScreenShot), (int)thumb.Length, "attachment; filename=" + co.ScreenShot);
            return thumb;
        }
        //Get the developer logo
        public Stream GetDeveloperLogo(string pid, string key)
        {
            if (!CheckKey(key))
                return null;
            pid = pid.Replace('_', ':');
            //Get the content object
            vwarDAL.ContentObject co = GetRepo().GetContentObjectById(pid, false);
            //Check the permissions
            if (!DoValidate(Security.TransactionType.Query, co))
            {
                ReleaseRepo();
                return null;
            }
           
           
            Stream data = co.GetDeveloperLogoFile();
            SetResponseHeaders(GetMimeType(co.DeveloperLogoImageFileName), (int)data.Length, "attachment; filename=" + co.DeveloperLogoImageFileName);
            ReleaseRepo();
            return data;
        }
        //Get the developer logo
        public Stream GetSponsorLogo(string pid, string key)
        {
            if (!CheckKey(key))
                return null;
            pid = pid.Replace('_', ':');
            //Get the content object
            vwarDAL.ContentObject co = GetRepo().GetContentObjectById(pid, false);

            //Check the permissions
            if (!DoValidate(Security.TransactionType.Query, co))
            {
                ReleaseRepo();
                return null;
            }

            Stream data = co.GetSponsorLogoFile();
            SetResponseHeaders(GetMimeType(co.SponsorLogoImageFileName), (int)data.Length, "attachment; filename=" + co.SponsorLogoImageFileName);
            ReleaseRepo();
            return data;
        }
        //Search the repo for a list of pids that match a search term
        //This returns the results as a list of pairs of titles and pids
        //will eventually take a pagenum and other params for more advanced searching

        public List<SearchResult> Search(string terms, string key)
        {
            if (!CheckKey(key))
                return null;
            try
            {
                terms = HttpUtility.UrlDecode(terms);
                String[] termlist = terms.Split(new char[]{' ',',','&'},StringSplitOptions.RemoveEmptyEntries);

                string username = GetUsername();
                if (username == "")
                    return null;
                //Do the search
            
                List<SearchResult> results = new List<SearchResult>();
                vwarDAL.PermissionsManager prm = new vwarDAL.PermissionsManager();
                vwarDAL.DataAccessFactory factory = new vwarDAL.DataAccessFactory();
                vwarDAL.ISearchProxy search = factory.CreateSearchProxy(username);
                foreach (string searchterm in termlist)
                {
                    
                    
                    IEnumerable<vwarDAL.ContentObject> caresults = search.QuickSearch(searchterm);

                    //Build the search results
                    if(caresults != null)
                    foreach (vwarDAL.ContentObject co in caresults)
                    {
                        SearchResult r = new SearchResult();
                        r.PID = co.PID;
                        r.Title = co.Title;
                        results.Add(r);              
                    }    
                }
                search.Dispose();
                prm.Dispose();

                return results;
            }
            catch (Exception ex)
            {
                List<SearchResult> results = new List<SearchResult>();
                results.Add(new SearchResult
                {
                    Title = ex.Message + ex.StackTrace
                });
                
                return results;
            }
            //return them
            
        }
        public List<SearchResult> AdvancedSearch(string searchmethod ,string searchstring, string key)
        {
            if (!CheckKey(key))
                return null;
            try
            {
                searchstring = HttpUtility.UrlDecode(searchstring);
                String[] termpairlist = searchstring.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                System.Collections.Specialized.NameValueCollection searchFieldsAndTerms = new System.Collections.Specialized.NameValueCollection();
                foreach (string s in termpairlist)
                {
                    string[] t = s.Split(new char[]{'='});
                    searchFieldsAndTerms[t[0]] = t[1];
                }
                string username = GetUsername();
                if (username == "")
                    return null;
                //Do the search

                List<SearchResult> results = new List<SearchResult>();
                vwarDAL.PermissionsManager prm = new vwarDAL.PermissionsManager();
               
                    vwarDAL.DataAccessFactory factory = new vwarDAL.DataAccessFactory();
                    vwarDAL.ISearchProxy search = factory.CreateSearchProxy(username);

                    vwarDAL.SearchMethod method = vwarDAL.SearchMethod.OR;
                    if(searchmethod.Equals("AND",StringComparison.CurrentCultureIgnoreCase))
                        method = vwarDAL.SearchMethod.AND;

                    IEnumerable<vwarDAL.ContentObject> caresults = search.SearchByFields(searchFieldsAndTerms, method);

                    //Build the search results
                    foreach (vwarDAL.ContentObject co in caresults)
                    {
                        SearchResult r = new SearchResult();
                        r.PID = co.PID;
                        r.Title = co.Title;
                        results.Add(r);
                    }
                    search.Dispose();
                return results;
            }
            catch (Exception ex)
            {
                List<SearchResult> results = new List<SearchResult>();
                results.Add(new SearchResult
                {
                    Title = ex.Message
                });
                return results;
            }
            //return them

        }
        //Get all the reviews for the object. Uses query permissions
        public List<Review> GetReviews(string pid, string key)
        {
            if (!CheckKey(key))
                return null;
            pid = pid.Replace('_', ':');
            //Get the content object
            vwarDAL.ContentObject co = GetRepo().GetContentObjectById(pid, false);

            //Check permissions
            if (!DoValidate(Security.TransactionType.Query, co))
            {
                ReleaseRepo();
                return null;
            }

            //Setup the return structure
            List<Review> results = new List<Review>();

            //Loop over reviews and add to rreturn structure
            foreach (vwarDAL.Review dr in co.Reviews)
            {
                Review r = new Review();
                r.DateTime = dr.SubmittedDate.ToString();
                r.Rating = dr.Rating;
                r.Submitter = dr.SubmittedBy;
                r.ReviewText = dr.Text;
                results.Add(r);

            }

            //return the reviews
            ReleaseRepo();
            return results;
        }

        //Add a review to the contentobject. User must have create rights on the repo
        public string AddReview(Review inreview, string pid, string key)
        {
            if (!CheckKey(key))
                return null;

            pid = pid.Replace('_', ':');

            //Get the content object
            vwarDAL.ContentObject co = GetRepo().GetContentObjectById(pid, false);

            //Check permissions
            if (!DoValidate(Security.TransactionType.Access, co))
            {
                ReleaseRepo();
                return "";
            }

            //Set the user authorized for this transaction to the submitterfor the review
            inreview.Submitter = this.GetUserEmail();

            //Translate into a vwardal review
            vwarDAL.Review r = new vwarDAL.Review();
            r.Text = inreview.ReviewText;
            r.SubmittedBy = inreview.Submitter;
            r.SubmittedDate = DateTime.Now;
            r.Rating = inreview.Rating;
            if (r.Rating < 0) r.Rating = 0;
            if (r.Rating > 5) r.Rating = 5;
            if (r.Text == null) r.Text = "";
            //Add the review
            co.AddReview(r);

            //Commit the changes
            co.CommitChanges();
            ReleaseRepo();
            return "ok";
        }






        public string UpdateMetadata(Metadata md, string pid, string key)
        {
            if (!CheckKey(key))
                return null;

            pid = pid.Replace('_', ':');
            //Get the content object
            vwarDAL.ContentObject co = GetRepo().GetContentObjectById(pid, false);

            //Check permissions
            if (!DoValidate(Security.TransactionType.Modify, co))
            {
                ReleaseRepo();
                return null;
            }

            CopyContentObjectData(md, co);

            //Make sure these changes get written back to repository
            co.CommitChanges();
            ReleaseRepo();
            return "";
        }
        public string InsertMetadata(Metadata md)
        {
            vwarDAL.ContentObject co = new vwarDAL.ContentObject(GetRepo());
            CopyContentObjectData(md, co);
            GetRepo().InsertContentObject(co);
            ReleaseRepo();
            return co.PID;
        }
        private static void CopyContentObjectData(Metadata md, vwarDAL.ContentObject co)
        {
            //Copy the data from the input object
            co.Title = md.Title;
            co.Keywords = md.Keywords;
            co.Format = md.Format;
            //co.CreativeCommonsLicenseURL = md.License;
            //Need to add logic to change values of texture references
            co.DeveloperName = md.DeveloperName;
            co.Description = md.Description;
            co.ArtistName = md.ArtistName;
            co.AssetType = md.AssetType;
            co.NumPolygons = System.Convert.ToInt32(md.NumPolygons);
            co.NumTextures = System.Convert.ToInt32(md.NumTextures);
            co.SponsorName = md.SponsorName;

            co.UnitScale = md.UnitScale;
            co.UpAxis = md.UpAxis;
            co.MoreInformationURL = md.MoreInformationURL;
            
        }
        //Get the metadata object for a conten object
        public Metadata GetMetadata(string pid, string key)
        {
            if (!CheckKey(key))
                return null;
            try
            {
              
                //Metadata to return
                Metadata map = new Metadata();
                pid = pid.Replace('_', ':');
                //Get the content object
                vwarDAL.ContentObject co = GetRepo().GetContentObjectById(pid, false);

                //Check the permissions
                if (!DoValidate(Security.TransactionType.Query, co))
                {
                    ReleaseRepo();
                    return null;
                }

                //If there is no location, dont return data
                if (co.Location != "")
                {
                    map.PID = co.PID;
                    map.Title = co.Title;
                    map.Keywords = co.Keywords;
                    map.Format = co.Format;
                    map.Downloads = co.Downloads.ToString();
                    map.DeveloperName = co.DeveloperName;
                    map.Description = co.Description;
                    map.ArtistName = co.ArtistName;
                    map.AssetType = co.AssetType;
                    map.NumPolygons = co.NumPolygons.ToString();
                    map.NumTextures = co.NumTextures.ToString();
                    map.SponsorName = co.SponsorName;
                    map.UnitScale = co.UnitScale;
                    map.UpAxis = co.UpAxis;
                    map.UploadedDate = co.UploadedDate.ToString();
                    map.Views = co.Views.ToString();
                    map.Revision = co.Revision.ToString();
                    map.TotalRevisions = co.NumberOfRevisions.ToString();
                   // map.License = co.CreativeCommonsLicenseURL;
                    //Get the supporting files, and copy to a serializable class
                    map.SupportingFiles = new List<SupportingFile>();
                    foreach (vwarDAL.SupportingFile i in co.SupportingFiles)
                    {
                        SupportingFile f2 = new SupportingFile();
                        f2.Filename = i.Filename;
                        f2.Description = i.Description;

                        map.SupportingFiles.Add(f2);
                    }

                    //Get the texture references and copy to a serializable class
                    map.TextureReferences = new List<Texture>();
                    foreach (vwarDAL.Texture i in co.TextureReferences)
                    {
                        Texture f2 = new Texture();
                        f2.mFilename = i.mFilename;
                        f2.mType = i.mType;
                        f2.mUVSet = i.mUVSet;

                        map.TextureReferences.Add(f2);
                    }

                    //Get the missing textures, and copy to a serializable class
                    map.MissingTextures = new List<Texture>();
                    foreach (vwarDAL.Texture i in co.MissingTextures)
                    {
                        Texture f2 = new Texture();
                        f2.mFilename = i.mFilename;
                        f2.mType = i.mType;
                        f2.mUVSet = i.mUVSet;

                        map.MissingTextures.Add(f2);
                    }


                }
                //Return the data
                ReleaseRepo();
                return map;
            }
            catch (Exception ex)
            {
                return new Metadata { Title = ex.Message };
            }
            ReleaseRepo();
            return new Metadata { Title = "got here" };
        }
        //Get the mimetype string for a given filename
        private string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }
       
        //Upload a new content object. Returns the pid of the uploaded file
        public string UploadFile(byte[] data, string pid, string key)
        {

            if (!CheckKey(key))
                return null;

            //Check permissions
            if (!DoValidate( Security.TransactionType.Create, null))
                return "Not authorized";

            vwarDAL.ContentObject co = null;
            //Create a new object
            if (pid == "")
            {
                co = GetRepo().GetNewContentObject();
                co.Revision = 0;
                //Setup some default values
                co.Title = "tempupload";
                co.Views = 0;
            }
            if (pid != "")
            {
                co = GetRepo().GetContentObjectById(pid, false);
                if (co == null)
                {
                    ReleaseRepo();
                    return "PID does not exist";
                }
                co.Revision = co.Revision + 1;
            }

            co.UploadedDate = DateTime.Now;
            co.LastModified = DateTime.Now;
            

            //The owner of this content is the person whose credentials were used to upload it
            co.SubmitterEmail = GetUserEmail();

            Utility_3D.ConvertedModel model;
            try
            {
            //Setup the conversion library
            Utility_3D _3d = new Utility_3D();
            _3d.Initialize(ConfigurationManager.AppSettings["LibraryLocation"]);
            Utility_3D.Model_Packager converter = new Utility_3D.Model_Packager();
            Utility_3D.ConverterOptions opts = new Utility_3D.ConverterOptions();

            //We do want metadata gathered with this conversion
            opts.EnableMetadataGathering();
            opts.EnableScaleTextures(512);
            opts.EnableTextureConversion("png");
            
            //Try to convert the model package into a dae
            //Note that the system might allow you to input an skp, so this should probably take a filename
            //The conversion needs to be told that the input is skp, and currently it's hardcoded to show it as a zip
           
                model = converter.Convert(new MemoryStream(data), "content.zip", "dae", opts);
            }
            catch (Utility_3D.ConversionException e)
            {
                model = null;
            }
            catch (System.Exception e)
            {
                model = null;
            }
            
            if (model != null)
            {
                //Copy the data gathered by the converter to the metadata
                co.NumPolygons = model._ModelData.VertexCount.Polys;
                co.NumTextures = model.textureFiles.Count;
                co.UpAxis = model._ModelData.TransformProperties.UpAxis;
                co.UnitScale = model._ModelData.TransformProperties.UnitMeters.ToString();
                co.LastModified = System.DateTime.Now;
                co.UploadedDate = System.DateTime.Now;
                co.Views = 0;
            }


            //Place this new object in the repo
            if (pid != "")
            {
                GetRepo().InsertContentRevision(co);
            }
            else
            {
                GetRepo().InsertContentObject(co);
            }

            if (model != null)
            {
                //Set the stream from the conversion to the content of this object
                co.SetContentFile(new MemoryStream(model.data), "content.zip");
                //Set the display file
                Stream displayfile = ConvertFileToO3D(new MemoryStream(model.data));
                if(displayfile!= null)
                    co.SetDisplayFile(displayfile, "content.o3d");
           
                //Add the references to textrues discovered by the converter to the database
                foreach (string i in model._ModelData.ReferencedTextures)
                    co.AddTextureReference(i.ToLower(), "Diffuse", 0);

                //Add the references to missing textures to the database
                foreach (string i in model.missingTextures)
                    co.AddMissingTexture(i.ToLower(), "Diffuse", 0);
            }

            //set the original file data
            co.OriginalFileName = "OriginalUpload.zip";
            co.OriginalFileId = GetRepo().SetContentFile(new MemoryStream(data), co.PID, co.OriginalFileName);
            co.CommitChanges();

            //setup the default permissions
            vwarDAL.PermissionsManager perm = new vwarDAL.PermissionsManager();
            perm.SetModelToGroupLevel(GetUserEmail(), co.PID, vwarDAL.DefaultGroups.AllUsers, vwarDAL.ModelPermissionLevel.Fetchable);
            perm.SetModelToGroupLevel(GetUserEmail(), co.PID, vwarDAL.DefaultGroups.AnonymousUsers, vwarDAL.ModelPermissionLevel.Searchable);


            perm.Dispose();
            ReleaseRepo();
            //return the pid of this new object
            return co.PID;
        }
        //Run the o3d commandline tool, copy the ouput back as a stream
        private static Stream ConvertFileToO3D(Stream data)
        {
            //Create a temp file for the object
            string path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName() + ".zip");
            System.IO.FileStream f = new FileStream(path, System.IO.FileMode.CreateNew);

            data.Seek(0, SeekOrigin.Begin);
            //write the stream to the file
            int b = data.ReadByte();
            while (b >= 0)
            {
                f.WriteByte((byte)b);
                b = data.ReadByte();
            }
            f.Close();
            //Get the path to the o3d tool
            var application = ConfigurationManager.AppSettings["LibraryLocation"] + "/o3dConverter.exe";//Path.Combine(Path.Combine(request.PhysicalApplicationPath, "bin"), "o3dConverter.exe");

            //Set the params and run
            System.Diagnostics.ProcessStartInfo processInfo = new System.Diagnostics.ProcessStartInfo(application);
            processInfo.Arguments = String.Format("\"{0}\" \"{1}\"", path, path.ToLower().Replace("zip", "o3d").Replace("skp", "o3d"));
            processInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            processInfo.RedirectStandardError = true;
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            var p = System.Diagnostics.Process.Start(processInfo);
            //var error = p.StandardError.ReadToEnd();

            p.WaitForExit(5000);
            if (!p.HasExited)
            {

                p.Kill();
                while (!p.HasExited)
                {
                    System.Threading.Thread.Sleep(10);
                }
            }

            //Get the name of the file that should have been ouptut by o3d
            string outfile = path.ToLower().Replace("zip", "o3d").Replace("skp", "o3d");
            if (File.Exists(outfile))
            {
                f = new FileStream(outfile, System.IO.FileMode.Open);
                MemoryStream m = new MemoryStream();

                //Read that file into a stream
                b = f.ReadByte();
                while (b >= 0)
                {
                    m.WriteByte((byte)b);
                    b = f.ReadByte();
                }
                f.Close();
                m.Seek(0, SeekOrigin.Begin);
                //Return the stream
                return m;
            }
            return null;
        }
        //Remove a supporting file from the repo
        public bool DeleteSupportingFile(string pid, string filename)
        {
            //Get the content object
            vwarDAL.ContentObject co = GetRepo().GetContentObjectById(pid, false);

            //Check the permissions
            if (!DoValidate(Security.TransactionType.Delete, co))
            {
                ReleaseRepo();
                return false;
            }

            ReleaseRepo();
            //Remove the file
            return co.RemoveSupportingFile(filename);
        }
        //Get a supporting file from a content object
        public Stream GetSupportingFile(string pid, string filename, string key)
        {
            if (!CheckKey(key))
                return null;
            pid = pid.Replace('_', ':');
            //Get the content object
            
            vwarDAL.ContentObject co = GetRepo().GetContentObjectById(pid, false);
    

            //Check the permissions
            if (!DoValidate(Security.TransactionType.Access, co))
            {
                ReleaseRepo();
                return null;
            }

            //set the status codes and return the stream
            Stream data = co.GetSupportingFile(filename);
            SetResponseHeaders(GetMimeType(filename), (int)data.Length, "attachment; filename=" + filename);
            ReleaseRepo();
            return data;
        }
        //Add a supporting file to the content object
        public string UploadSupportingFile(byte[] indata, string pid, string filename, string description, string key)
        {

            if (!CheckKey(key))
                return null;
            pid = pid.Replace('_', ':');

            //Get the content object
            vwarDAL.ContentObject co = GetRepo().GetContentObjectById(pid, false);

            //Check the permissions
            if (!DoValidate(Security.TransactionType.Modify, co))
            {
                ReleaseRepo();
                return "";
            }

            //Add the file
            co.AddSupportingFile(new MemoryStream(indata), filename, description);
            ReleaseRepo();
            return "Ok";
        }
        //Resolve a missing texture reference
        public string UploadMissingTexture(byte[] indata, string pid, string filename, string key)
        {

            if (!CheckKey(key))
                return null;
            pid = pid.Replace('_', ':');

            //Get the content object
            vwarDAL.ContentObject co = GetRepo().GetContentObjectById(pid, false);

            //Check the permissions
            if (!DoValidate(Security.TransactionType.Modify, co))
            {
                ReleaseRepo();
                return "";
            }

            //Check that the file they are attempting to replace is actually missing
            vwarDAL.Texture found = null; ;
            foreach (vwarDAL.Texture i in co.MissingTextures)
            {
                if (i.mFilename.ToLower() == filename.ToLower())
                {
                    found = i;
                }
            }
            //If they are updating a texture that is missing

            if (found != null)
            {
                //Remove it from the list of missing textures
                co.RemoveMissingTexture(filename.ToLower());

                //The model converter class can be used to insert the file into the zip
                Utility_3D.Model_Packager _pack = new Utility_3D.Model_Packager();
                Utility_3D.ConvertedModel model = new Utility_3D.ConvertedModel();
                Stream sdata = co.GetContentFile();
                byte[] data = new byte[sdata.Length];
                sdata.Read(data, 0, (int)sdata.Length);

                //Insert the content file into a Model, and call the Add function
                model.data = data;
                _pack.AddTextureToModel(ref model, new MemoryStream(indata), filename.ToLower());

                //Overwrite the existing contentfile with the new one that contains the new texture
                co.SetContentFile(new MemoryStream(model.data), "content.zip");
                ReleaseRepo();
                return "Ok";
            }
            ReleaseRepo();
            //Need to come up with a nice set of return codes
            return "This texture is not a missing texture";

        }
        //Upload the screenshot for the model
        public string UploadScreenShot(byte[] indata, string pid, string filename, string key)
        {

            if (!CheckKey(key))
                return null;
            pid = pid.Replace('_', ':');

            //Get the content obhect
            vwarDAL.ContentObject co = GetRepo().GetContentObjectById(pid, false);

            //Check the permissions
            if (!DoValidate(Security.TransactionType.Modify, co))
            {
                ReleaseRepo();
                return "";
            }

            //Set the screenshot file
            co.SetScreenShotFile(new MemoryStream(indata), filename);

            //create the thumbnail
            MemoryStream thumb = new MemoryStream();
            Bitmap map = new Bitmap(new MemoryStream(indata));
            map.GetThumbnailImage(100,100,null,System.IntPtr.Zero).Save(thumb,System.Drawing.Imaging.ImageFormat.Png);
            thumb.Seek(0,SeekOrigin.Begin);
            co.SetThumbnailFile(thumb,"thumbnail.png");
            ReleaseRepo();
            return "Ok";
        }
        //upload the developer logo for the content object
        public string UploadDeveloperLogo(byte[] indata, string pid, string filename, string key)
        {
            if (!CheckKey(key))
                return null;
            pid = pid.Replace('_', ':');
            //Get the content object
            vwarDAL.ContentObject co = GetRepo().GetContentObjectById(pid, false);

            //Check the permissions
            if (!DoValidate(Security.TransactionType.Modify, co))
            {
                ReleaseRepo();
                return "";
            }
            //Set the developer logo file stream
            co.SetDeveloperLogoFile(new MemoryStream(indata), filename);
            ReleaseRepo();
            return "Ok";
        }
        //Upload the sponser logo for a content object
        public string UploadSponsorLogo(byte[] indata, string pid, string filename, string key)
        {

            if (!CheckKey(key))
                return null;
            pid = pid.Replace('_', ':');

            //Get the content object
            vwarDAL.ContentObject co = GetRepo().GetContentObjectById(pid, false);

            //Check the permissions
            if (!DoValidate(Security.TransactionType.Modify, co))
            {
                ReleaseRepo();
                return "";
            }

            //Set the sponsor logo stream
            co.SetSponsorLogoFile(new MemoryStream(indata), filename);
            ReleaseRepo();
            return "Ok";
        }
        //Upload the screenshot for the model
        public string SetGroupPermission(string pid, string groupname, string level, string key)
        {

            if (!CheckKey(key))
                return null;
            pid = pid.Replace('_', ':');

            //Get the content obhect
            vwarDAL.ContentObject co = GetRepo().GetContentObjectById(pid, false);

            //Check the permissions
            if (!DoValidate(Security.TransactionType.Modify, co))
            {
                ReleaseRepo();
                return "";
            }

            vwarDAL.PermissionsManager perm = new vwarDAL.PermissionsManager();
            vwarDAL.PermissionErrorCode code = perm.SetModelToGroupLevel(GetUsername(), pid, groupname, (vwarDAL.ModelPermissionLevel)Enum.Parse(typeof(vwarDAL.ModelPermissionLevel), level));
            ReleaseRepo();
            return System.Enum.GetName(typeof(vwarDAL.PermissionErrorCode), code);
        }
        //Upload the screenshot for the model
        public string SetUserPermission(string pid, string username, string level, string key)
        {

            if (!CheckKey(key))
                return null;
            pid = pid.Replace('_', ':');

            //Get the content obhect
            vwarDAL.ContentObject co = GetRepo().GetContentObjectById(pid, false);

            //Check the permissions
            if (!DoValidate(Security.TransactionType.Modify, co))
            {
                ReleaseRepo();
                return "";
            }

            vwarDAL.PermissionsManager perm = new vwarDAL.PermissionsManager();
            vwarDAL.PermissionErrorCode code = perm.SetModelToUserLevel(GetUsername(), pid, username, (vwarDAL.ModelPermissionLevel)Enum.Parse(typeof(vwarDAL.ModelPermissionLevel), level));
            perm.Dispose();
            ReleaseRepo();
            return System.Enum.GetName(typeof(vwarDAL.PermissionErrorCode), code);
        }
        //Upload the screenshot for the model
        public string GetUserPermission(string pid, string username, string key)
        {

            if (!CheckKey(key))
                return null;
            pid = pid.Replace('_', ':');

            //Get the content obhect
            vwarDAL.ContentObject co = GetRepo().GetContentObjectById(pid, false);

            //Check the permissions
            if (!DoValidate(Security.TransactionType.Query, co))
            {
                ReleaseRepo();
                return "";
            }
            //tells you what hte level is for this user, taking into account group membership
            vwarDAL.ModelPermissionLevel level = vwarDAL.ModelPermissionLevel.NotSet;
            vwarDAL.PermissionsManager perm = new vwarDAL.PermissionsManager();
            foreach(vwarDAL.UserGroup g in perm.GetUsersGroups(username))
            {
                if(perm.CheckGroupPermissions(g, pid) > level)
                    level = perm.CheckGroupPermissions(g, pid);
            }
            perm.Dispose();
            ReleaseRepo();
            return System.Enum.GetName(typeof(vwarDAL.ModelPermissionLevel), level);
        }
        //Upload the screenshot for the model
        public string GetGroupPermission(string pid, string groupname, string key)
        {

            if (!CheckKey(key))
                return null;
            pid = pid.Replace('_', ':');

            //Get the content obhect
            vwarDAL.ContentObject co = GetRepo().GetContentObjectById(pid, false);

            //Check the permissions
            if (!DoValidate(Security.TransactionType.Query, co))
            {
                ReleaseRepo();
                return "";
            }

            vwarDAL.PermissionsManager perm = new vwarDAL.PermissionsManager();
            vwarDAL.ModelPermissionLevel level = perm.CheckGroupPermissions(perm.GetUserGroup(groupname), pid);
            perm.Dispose();
            ReleaseRepo();
            return System.Enum.GetName(typeof(vwarDAL.ModelPermissionLevel), level);
        }
    }
}