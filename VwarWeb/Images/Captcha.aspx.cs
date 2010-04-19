using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Drawing.Imaging;

public partial class Images_Captcha : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    // Create a CAPTCHA image using the text stored in the Session object.
    {
        CaptchaImage.CaptchaImage ci = new CaptchaImage.CaptchaImage(Session["CaptchaCode"].ToString(), 200, 50, "Century Schoolbook");

        // Change the response headers to output a JPEG image.
        Controls.Clear();
        Response.Clear();
        Response.ContentType = "image/jpeg";

        // Write the image to the response stream in JPEG format.
        ci.Image.Save(Response.OutputStream, ImageFormat.Jpeg);

        // Dispose of the CAPTCHA image object.
        ci.Dispose();
    }
}
