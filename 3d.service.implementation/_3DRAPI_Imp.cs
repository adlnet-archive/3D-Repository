using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Web;
using System.IO;
using System.Configuration;
namespace vwar.service.host
{
    public class _3DRAPI_Imp
    {
        //The IDataRepository that holds all the files, and stores the data
        public vwarDAL.IDataRepository FedoraProxy;
        //Constructor, create IDataproxy
        public _3DRAPI_Imp()
        {
            vwarDAL.DataAccessFactory dalf = new vwarDAL.DataAccessFactory();
            FedoraProxy = dalf.CreateDataRepositorProxy();
        }

        private string GetUserEmail()
        {

            string auth = "";
            if (WebOperationContext.Current.IncomingRequest.Headers[System.Net.HttpRequestHeader.Authorization] != null)
            {
                auth = WebOperationContext.Current.IncomingRequest.Headers[System.Net.HttpRequestHeader.Authorization].ToString();
            }
            return Security.GetProvider().GetUser(Security.GetUsernameFromHeader(auth), false).Email;
        }
        //Pull the headers from the web transaction and check security
        //currently needs to work a bit differently as it assumes a url
        public bool DoValidate(string auth, Security.TransactionType type, vwarDAL.ContentObject co)
        {
            //Get the auth header
            //if(auth == "" || auth == null)
            {

                //if there is no auth header, mock up anonymous
                if (WebOperationContext.Current.IncomingRequest.Headers[System.Net.HttpRequestHeader.Authorization] != null)
                {
                    auth = WebOperationContext.Current.IncomingRequest.Headers[System.Net.HttpRequestHeader.Authorization].ToString();
                }
                else
                {
                    auth = "anonymous:anonymous";
                }
            }
            //Do a bit of manipulation on the URL, this needs to change as the url's should remove the auth string
            string url = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.ToString();
            //  if (url.LastIndexOf('=') >= 0)
            //      url = url.Substring(0, url.LastIndexOf("Auth=") + 4);

            //Use the security class to validate this url auth and type
            if (Security.ValidateUserTransaction(url, auth, type, co))
            {
                return true;
            }

            //Set the status if they are not authourized
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Forbidden;
            return false;

        }
        //Delete a content object from the repository
        public string DeleteObject(string pid)
        {

            //Get the content object
            vwarDAL.ContentObject co = FedoraProxy.GetContentObjectById(pid, false);

            //Check permissions
            if (!DoValidate("", Security.TransactionType.Delete, co))
                return null;

            //Remove it
            co.RemoveFromRepo();

            return "";
        }
        //A simpler url for retrieving a model
        public Stream GetModelSimple(string pid, string format)
        {
            return GetModel(pid, format, "");
        }
        //Get the content for a model
        public Stream GetModel(string pid, string format, string options)
        {


            //Get the content object
            vwarDAL.ContentObject co = FedoraProxy.GetContentObjectById(pid, false);

            //Check permissions
            if (!DoValidate("", Security.TransactionType.Access, co))
                return null;

            //Check that this content object actually has a content file
            if (co.Location != "")
            {

                //If they want the dae file, they can get just the contnet file
                if (format.ToLower() == "dae" || format.ToLower() == "collada")
                {
                    Stream ms = co.GetContentFile();

                    WebOperationContext.Current.OutgoingResponse.ContentType = GetMimeType(co.Location);
                    WebOperationContext.Current.OutgoingResponse.ContentLength = ms.Length;
                    WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-disposition", "attachment; filename=" + co.Location);


                    return ms as Stream;
                }
                //note that if the options string is anything, then the cached display file is not the one the client needs
                if ((format.ToLower() == "o3d" || format.ToLower() == "o3dtgz") && options == "")
                {
                    Stream ms = FedoraProxy.GetCachedContentObjectTransform(co, "o3d");

                    WebOperationContext.Current.OutgoingResponse.ContentType = GetMimeType(co.DisplayFile);
                    WebOperationContext.Current.OutgoingResponse.ContentLength = ms.Length;
                    WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-disposition", "attachment; filename=" + co.DisplayFile);


                    return ms as Stream;
                }
                //If they want any type other than the ones above, do the conversion
                else
                {
                    //Get the base content file
                    Stream unconvertedData = co.GetContentFile();
                    //setup the conversion system
                    Utility_3D _3d = new Utility_3D();
                    _3d.Initialize(ConfigurationManager.AppSettings["DLLPATH"]);
                    Utility_3D.Model_Packager converter = new Utility_3D.Model_Packager();
                    Utility_3D.ConverterOptions opts = new Utility_3D.ConverterOptions();
                    //No need to gather metadata during this conversion, which slows down the conversion significantly
                    opts.DisableMetadataGathering();

                    //Try to convert the model to the requested format
                    Utility_3D.ConvertedModel model;
                    try
                    {
                        model = converter.Convert(unconvertedData, co.Location, format, opts);
                    }
                    catch (Utility_3D.ConversionException e)
                    {
                        throw new WebFaultException(System.Net.HttpStatusCode.ServiceUnavailable);
                    }

                    //Looks like the conversion worked.
                    WebOperationContext.Current.OutgoingResponse.ContentType = GetMimeType(co.DisplayFile + "." + format);
                    WebOperationContext.Current.OutgoingResponse.ContentLength = model.data.Length;
                    WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-disposition", "attachment; filename=" + co.Location);

                    //Return the new data
                    MemoryStream ms = new MemoryStream(model.data);
                    return ms as Stream;
                }
            }
            //There is no content for this content object
            else
            {
                return null;
            }
        }
        //Get the screenshot for a content object
        public Stream GetScreenshot(string pid)
        {
            //Get the object
            vwarDAL.ContentObject co = FedoraProxy.GetContentObjectById(pid, false);
            //Check permissions
            if (!DoValidate("", Security.TransactionType.Access, co))
                return null;
            //Set the headers and reutnr the stream
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-disposition", "attachment; filename=" + co.ScreenShot);
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-type", GetMimeType(co.ScreenShot));
            return co.GetScreenShotFile();
        }
        //Get the developer logo
        public Stream GetDeveloperLogo(string pid)
        {
            //Get the content object
            vwarDAL.ContentObject co = FedoraProxy.GetContentObjectById(pid, false);
            //Check the permissions
            if (!DoValidate("", Security.TransactionType.Access, co))
                return null;
            //SEt the status and reutnr the stream
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-disposition", "attachment; filename=" + co.DeveloperLogoImageFileName);
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-type", GetMimeType(co.DeveloperLogoImageFileName));
            return co.GetDeveloperLogoFile();
        }
        //Get the developer logo
        public Stream GetSponsorLogo(string pid)
        {
            //Get the content object
            vwarDAL.ContentObject co = FedoraProxy.GetContentObjectById(pid, false);

            //Check the permissions
            if (!DoValidate("", Security.TransactionType.Access, co))
                return null;

            //Set the status and return the stream
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-disposition", "attachment; filename=" + co.SponsorLogoImageFileName);
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-type", GetMimeType(co.SponsorLogoImageFileName));
            return co.GetSponsorLogoFile();
        }
        //Search the repo for a list of pids that match a search term
        //This returns the results as a list of pairs of titles and pids
        //will eventually take a pagenum and other params for more advanced searching
        public List<SearchResult> Search(string terms)
        {
            try
            {
                //Check permissions
                if (!DoValidate("", Security.TransactionType.Query, null))
                    return null;

                //Do the search
                List<SearchResult> results = new List<SearchResult>();
                IEnumerable<vwarDAL.ContentObject> caresults = FedoraProxy.SearchContentObjects(terms);

                //Build the search results
                foreach (vwarDAL.ContentObject co in caresults)
                {
                    SearchResult r = new SearchResult();
                    r.PID = co.PID;
                    r.Title = co.Title;
                    results.Add(r);
                }
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
        public List<Review> GetReviews(string pid)
        {
            //Get the content object
            vwarDAL.ContentObject co = FedoraProxy.GetContentObjectById(pid, false);

            //Check permissions
            if (!DoValidate("", Security.TransactionType.Query, co))
                return null;

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
            return results;
        }

        //Add a review to the contentobject. User must have create rights on the repo
        public string AddReview(Review inreview, string pid)
        {
            //Get the content object
            vwarDAL.ContentObject co = FedoraProxy.GetContentObjectById(pid, false);

            //Check permissions
            if (!DoValidate("", Security.TransactionType.Create, co))
                return "";

            //Set the user authorized for this transaction to the submitterfor the review
            inreview.Submitter = this.GetUserEmail();

            //Translate into a vwardal review
            vwarDAL.Review r = new vwarDAL.Review();
            r.Text = inreview.ReviewText;
            r.SubmittedBy = inreview.Submitter;
            r.SubmittedDate = DateTime.Parse(inreview.DateTime);
            r.Rating = inreview.Rating;

            //Add the review
            co.AddReview(r);

            //Commit the changes
            co.CommitChanges();

            return "ok";
        }

        public string UpdateMetadata(Metadata md, string pid)
        {

            //Get the content object
            vwarDAL.ContentObject co = FedoraProxy.GetContentObjectById(pid, false);

            //Check permissions
            if (!DoValidate("", Security.TransactionType.Modify, co))
                return null;

            CopyContentObjectData(md, co);

            //Make sure these changes get written back to repository
            co.CommitChanges();

            return "";
        }
        public string InsertMetadata(Metadata md)
        {
            vwarDAL.ContentObject co = new vwarDAL.ContentObject(this.FedoraProxy);
            CopyContentObjectData(md, co);
            FedoraProxy.InsertContentObject(co);
            return co.PID;
        }
        private static void CopyContentObjectData(Metadata md, vwarDAL.ContentObject co)
        {
            //Copy the data from the input object
            co.Title = md.Title;
            co.Label = md.Label;
            co.Keywords = md.Keywords;
            co.Format = md.Format;

            //Need to add logic to change values of texture references
            co.DeveloperName = md.DeveloperName;
            co.Description = md.Description;
            co.ArtistName = md.ArtistName;
            co.AssetType = md.AssetType;
            co.MoreInformationURL = md.MoreInformationURL;
            co.NumPolygons = System.Convert.ToInt32(md.NumPolygons);
            co.NumTextures = System.Convert.ToInt32(md.NumTextures);
            co.SponsorName = md.SponsorName;

            co.UnitScale = md.UnitScale;
            co.UpAxis = md.UpAxis;
        }
        //Get the metadata object for a conten object
        public Metadata GetMetadata(string pid)
        {
            //Metadata to return
            Metadata map = new Metadata();
            //Get the content object
            vwarDAL.ContentObject co = FedoraProxy.GetContentObjectById(pid, false);

            //Check the permissions
            if (!DoValidate("", Security.TransactionType.Query, co))
                return null;

            //If there is no location, dont return data
            if (co.Location != "")
            {
                map.PID = co.PID;
                map.Title = co.Title;
                map.Label = co.Label;
                map.Keywords = co.Keywords;
                map.Format = co.Format;
                map.Downloads = co.Downloads.ToString();
                map.DeveloperName = co.DeveloperName;
                map.Description = co.Description;
                map.ArtistName = co.ArtistName;
                map.AssetType = co.AssetType;
                map.MoreInformationURL = co.MoreInformationURL;
                map.NumPolygons = co.NumPolygons.ToString();
                map.NumTextures = co.NumTextures.ToString();
                map.SponsorName = co.SponsorName;
                map.SubmitterEmail = co.SubmitterEmail;
                map.UnitScale = co.UnitScale;
                map.UpAxis = co.UpAxis;
                map.UploadedDate = co.UploadedDate.ToString();
                map.Views = co.Views.ToString();
                map.Revision = co.Revision.ToString();
                map.TotalRevisions = co.NumberOfRevisions.ToString();

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
            return map;
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
        public string UploadFile(byte[] data, string pid)
        {


            //Check permissions
            if (!DoValidate("", Security.TransactionType.Create, null))
                return "Not authorized";

            vwarDAL.ContentObject co = null;
            //Create a new object
            if (pid == "")
            {
                co = FedoraProxy.GetNewContentObject();
                co.Revision = 0;
            }
            if (pid != "")
            {
                co = FedoraProxy.GetContentObjectById(pid, false);
                if (co == null)
                    return "PID does not exist";
                co.Revision = co.Revision + 1;
            }

            //Setup some default values
            co.Title = "tempupload";

            co.UploadedDate = DateTime.Now;
            co.LastModified = DateTime.Now;
            co.Views = 0;

            //The owner of this content is the person whose credentials were used to upload it
            co.SubmitterEmail = GetUserEmail();

            //Setup the conversion library
            Utility_3D _3d = new Utility_3D();
            _3d.Initialize(ConfigurationManager.AppSettings["DLLPATH"]);
            Utility_3D.Model_Packager converter = new Utility_3D.Model_Packager();
            Utility_3D.ConverterOptions opts = new Utility_3D.ConverterOptions();

            //We do want metadata gathered with this conversion
            opts.EnableMetadataGathering();

            Utility_3D.ConvertedModel model;
            //Try to convert the model package into a dae
            //Note that the system might allow you to input an skp, so this should probably take a filename
            //The conversion needs to be told that the input is skp, and currently it's hardcoded to show it as a zip
            try
            {
                model = converter.Convert(new MemoryStream(data), "content.zip", "dae", opts);
            }
            catch (Utility_3D.ConversionException e)
            {
                throw new WebFaultException(System.Net.HttpStatusCode.ServiceUnavailable);
            }

            //Copy the data gathered by the converter to the metadata
            co.NumPolygons = model._ModelData.VertexCount.Polys;
            co.NumTextures = model.textureFiles.Count;
            co.UpAxis = model._ModelData.TransformProperties.UpAxis;
            co.UnitScale = model._ModelData.TransformProperties.UnitMeters.ToString();
            co.LastModified = System.DateTime.Now;
            co.UploadedDate = System.DateTime.Now;
            co.Views = 0;


            //Place this new object in the repo
            if (pid != "")
            {
                FedoraProxy.InsertContentRevision(co);
            }
            else
            {
                FedoraProxy.InsertContentObject(co);

            }
            //Set the stream from the conversion to the content of this object
            co.SetContentFile(new MemoryStream(model.data), "content.zip");
            //Set the display file
            co.SetDisplayFile(ConvertFileToO3D(new MemoryStream(model.data)), "content.o3d");
            co.CommitChanges();

            //Add the references to textrues discovered by the converter to the database
            foreach (string i in model._ModelData.ReferencedTextures)
                co.AddTextureReference(i.ToLower(), "Diffuse", 0);

            //Add the references to missing textures to the database
            foreach (string i in model.missingTextures)
                co.AddMissingTexture(i.ToLower(), "Diffuse", 0);

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
            var application = ConfigurationManager.AppSettings["DLLPATH"] + "/o3dConverter.exe";//Path.Combine(Path.Combine(request.PhysicalApplicationPath, "bin"), "o3dConverter.exe");

            //Set the params and run
            System.Diagnostics.ProcessStartInfo processInfo = new System.Diagnostics.ProcessStartInfo(application);
            processInfo.Arguments = String.Format("\"{0}\" \"{1}\"", path, path.ToLower().Replace("zip", "o3d").Replace("skp", "o3d"));
            processInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            processInfo.RedirectStandardError = true;
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            var p = System.Diagnostics.Process.Start(processInfo);
            var error = p.StandardError.ReadToEnd();

            //Get the name of the file that should have been ouptut by o3d
            string outfile = path.ToLower().Replace("zip", "o3d").Replace("skp", "o3d");
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
        //Remove a supporting file from the repo
        public bool DeleteSupportingFile(string pid, string filename)
        {
            //Get the content object
            vwarDAL.ContentObject co = FedoraProxy.GetContentObjectById(pid, false);

            //Check the permissions
            if (!DoValidate("", Security.TransactionType.Delete, co))
                return false;

            //Remove the file
            return co.RemoveSupportingFile(filename);
        }
        //Get a supporting file from a content object
        public Stream GetSupportingFile(string pid, string filename)
        {
            //Get the content object
            vwarDAL.ContentObject co = FedoraProxy.GetContentObjectById(pid, false);

            //Check the permissions
            if (!DoValidate("", Security.TransactionType.Access, co))
                return null;

            //set the status codes and return the stream
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-disposition", "attachment; filename=" + filename);
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-type", GetMimeType(filename));
            return co.GetSupportingFile(filename);
        }
        //Add a supporting file to the content object
        public string UploadSupportingFile(byte[] indata, string pid, string filename, string description)
        {
            //Get the content object
            vwarDAL.ContentObject co = FedoraProxy.GetContentObjectById(pid, false);

            //Check the permissions
            if (!DoValidate("", Security.TransactionType.Modify, co))
                return "";

            //Add the file
            co.AddSupportingFile(new MemoryStream(indata), filename, description);
            return "Ok";
        }
        //Resolve a missing texture reference
        public string UploadMissingTexture(byte[] indata, string pid, string filename)
        {
            //Get the content object
            vwarDAL.ContentObject co = FedoraProxy.GetContentObjectById(pid, false);

            //Check the permissions
            if (!DoValidate("", Security.TransactionType.Modify, co))
                return "";

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

                return "Ok";
            }
            //Need to come up with a nice set of return codes
            return "This texture is not a missing texture";

        }
        //Upload the screenshot for the model
        public string UploadScreenShot(byte[] indata, string pid, string filename)
        {
            //Get the content obhect
            vwarDAL.ContentObject co = FedoraProxy.GetContentObjectById(pid, false);

            //Check the permissions
            if (!DoValidate("", Security.TransactionType.Modify, co))
                return "";

            //Set the screenshot file
            co.SetScreenShotFile(new MemoryStream(indata), filename);
            return "Ok";
        }
        //upload the developer logo for the content object
        public string UploadDeveloperLogo(byte[] indata, string pid, string filename)
        {
            //Get the content object
            vwarDAL.ContentObject co = FedoraProxy.GetContentObjectById(pid, false);

            //Check the permissions
            if (!DoValidate("", Security.TransactionType.Modify, co))
                return "";
            //Set the developer logo file stream
            co.SetDeveloperLogoFile(new MemoryStream(indata), filename);
            return "Ok";
        }
        //Upload the sponser logo for a content object
        public string UploadSponsorLogo(byte[] indata, string pid, string filename)
        {
            //Get the content object
            vwarDAL.ContentObject co = FedoraProxy.GetContentObjectById(pid, false);

            //Check the permissions
            if (!DoValidate("", Security.TransactionType.Modify, co))
                return "";

            //Set the sponsor logo stream
            co.SetSponsorLogoFile(new MemoryStream(indata), filename);
            return "Ok";
        }

    }
}