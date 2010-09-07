using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;


namespace Website
{
    /// <summary>
    /// Summary description for Common
    /// </summary>
    public class Common
    {
        public Common()
        {
        }
        public static string GetFullUserName(object userName)
        {
            if (userName != null)
            {
                var user = Membership.GetUser(userName.ToString());
                if (user != null && !String.IsNullOrEmpty(user.Comment))
                {
                    return user.Comment.Replace('|', ' ');
                }
                return userName.ToString();
            }
            return "";
        }

       
        //formats URL of screenshot image stored at ~/content/{id}/{screenshot}
        public static string FormatScreenshotImage(object contentObjectID, object screenshot)
        {
            return String.Format("~/Public/Model.ashx?pid={0}&file={1}",contentObjectID,screenshot);
        }
        public static string FormatEditUrl(object contentObjectID)
        {
            string rv = "";
            if (contentObjectID != null)
            {
                rv = "~/Users/Upload.aspx?ContentObjectID=" + contentObjectID.ToString();
            }
            return rv;
        }
        //returns full path of zip file located at ~/content/{id}/{location}
        public static string FormatZipFilePath(object contentObjectID, object location)
        {
            string rv = "";
            if (contentObjectID != null && location != null)
            {
                string virtualPath = "~/Content/" + contentObjectID + "/" + location;
                rv = HttpContext.Current.Server.MapPath(virtualPath);
            }
            return rv;
        }

        //bind to ImageUrl of screenshot image - stored at ~/Images/SubmitterLogos/{id}.{gif/jpg/jpeg/png}
        public static string FormatSubmitterLogoImage(object contentObjectID)
        {
            string rv = "";
            if (contentObjectID != null)
            {
                //try to create path to logo image - return "" if not found
                rv = "~/Images/SubmitterLogos/" + contentObjectID + ".jpg";
                if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(rv)))
                {
                    rv = "~/Images/SubmitterLogos/" + contentObjectID + ".jpeg";
                    if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(rv)))
                    {
                        rv = "~/Images/SubmitterLogos/" + contentObjectID + ".png";
                        if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(rv)))
                        {
                            rv = "~/Images/SubmitterLogos/" + contentObjectID + ".gif";
                            if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(rv)))
                            {
                                rv = "";
                            }
                        }
                    }
                }
            }
            return rv;
        }
        public static int CalculateAverageRating(object contentObjectId)
        {


            var factory = new vwarDAL.DataAccessFactory();
            vwarDAL.IDataRepository dal = factory.CreateDataRepositorProxy();
            var co = dal.GetContentObjectById(contentObjectId.ToString(), false,true);
            int rating = 0;
            foreach (var review in co.Reviews)
            {
                rating += review.Rating;
            }
            if (co.Reviews.Count() > 0)
            {
                return rating / co.Reviews.Count();
            }

            return rating;
        }
        //bind to visible property of logo image
        public static Boolean ShowSubmitterLogoImage(object contentObjectID)
        {
            Boolean rv = false;
            if (FormatSubmitterLogoImage(contentObjectID) != "")
            {
                rv = true;
            }
            return rv;
        }

        public static byte[] GetByteArrayFromFileUpload(FileUpload fu)
        {
            byte[] rv = null;

            if (fu.PostedFile != null && !string.IsNullOrEmpty(fu.PostedFile.FileName))
            {
                int imageLength = fu.PostedFile.ContentLength;
                rv = new byte[imageLength];
                fu.PostedFile.InputStream.Read(rv, 0, imageLength);
            }

            return rv;
        }

        public static bool IsValidLogoImageContentType(string contentType)
        {
            bool rv = false;

            if (!string.IsNullOrEmpty(contentType))
            {

                switch (contentType.ToLower().Trim())
                {
                    case "image/gif":
                    case "image/jpeg":
                    case "image/bmp":
                    case "image/png":
                        rv = true;
                        break;
                }

            }

            return rv;
        }


    }

}
