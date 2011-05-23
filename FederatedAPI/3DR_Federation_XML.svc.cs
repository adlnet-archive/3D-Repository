using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Web;
using System.ServiceModel.Web;
namespace FederatedAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "_3DR_Federation" in code, svc and config file together.
    public class _3DR_Federation_XML : FederatedAPI.implementation._3DR_Federation_Impl, vwar.service.host.I3DRAPI_Xml 
    {
        //A simpler url for retrieving a model
        public Stream GetModelSimple(string pid, string format)
        {
            string address = GetRedirectAddressModel(implementation.APIType.XML, pid, format);
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            WebOperationContext.Current.OutgoingResponse.Location = address;
            return null;
        }
        //Get the content for a model
        public Stream GetModel(string pid, string format, string options)
        {
            string address = GetRedirectAddressModelAdvanced(implementation.APIType.XML, pid, format, options);
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            WebOperationContext.Current.OutgoingResponse.Location = address;
            return null;
        }
        //Get the screenshot for a content object
        public Stream GetScreenshot(string pid)
        {
            string address = GetRedirectAddress("Screenshot", implementation.APIType.XML, pid);
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            WebOperationContext.Current.OutgoingResponse.Location = address;
            return null;
        }
        //Get the developer logo
        public Stream GetDeveloperLogo(string pid)
        {
            string address = GetRedirectAddress("DeveloperLogo", implementation.APIType.XML, pid);
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            WebOperationContext.Current.OutgoingResponse.Location = address;
            return null;
        }
        //Get the developer logo
        public Stream GetSponsorLogo(string pid)
        {
            string address = GetRedirectAddress("SponsorLogo", implementation.APIType.XML, pid);
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            WebOperationContext.Current.OutgoingResponse.Location = address;
            return null;
        }
        public vwar.service.host.Metadata GetMetadata(string pid)
        {
            string address = GetRedirectAddress("Metadata", implementation.APIType.XML, pid);
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            WebOperationContext.Current.OutgoingResponse.Location = address;
            return null;
        }
        //Get all the reviews for the object. Uses query permissions
        public List<vwar.service.host.Review> GetReviews(string pid)
        {
            string address = GetRedirectAddress("Reviews", implementation.APIType.XML, pid);
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            WebOperationContext.Current.OutgoingResponse.Location = address;
            return null;
        }

        //Get a supporting file from a content object
        public Stream GetSupportingFile(string pid, string filename)
        {
            string address = GetRedirectAddress("SupportingFile", implementation.APIType.XML, pid) + "/" + filename;
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            WebOperationContext.Current.OutgoingResponse.Location = address;
            return null;
        }


        public Stream GetTextureFile(string pid, string filename)
        {
            throw new NotImplementedException();
        }
    }
}
