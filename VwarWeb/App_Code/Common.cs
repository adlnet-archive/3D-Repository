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
            var user = Membership.GetUser(userName.ToString());
            if (user!=null && !String.IsNullOrEmpty(user.Comment))
            {
                return user.Comment.Replace('|', ' ');
            }
            return userName.ToString();
        }
        //formats URL of screenshot image stored at ~/content/{id}/{screenshot}
        public static string FormatScreenshotImage(object contentObjectID, object screenshot)
        {
            string rv = "";
            if (contentObjectID != null && screenshot != null)
            {
                rv = "~/Content/" + contentObjectID + "/" + screenshot;            
            }
            return rv;
        }
        public static string FormatEditUrl(object contentObjectID)
        {
            string rv = "";
            if (contentObjectID != null )
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
            int id = 0;
            if (int.TryParse(contentObjectId.ToString(), out id))
            {
                vwarDAL.vwarDAL dal = new vwarDAL.vwarDAL(Website.Config.EntityConnectionString);
                var co = dal.GetContentObjectById(id, false);
                int rating = 0;
                foreach (var review in co.Reviews)
                {
                    rating += review.Rating;
                }
                if (co.Reviews.Count() > 0)
                {
                    return rating / co.Reviews.Count();
                }
            }
            return 0;
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

    }

}
