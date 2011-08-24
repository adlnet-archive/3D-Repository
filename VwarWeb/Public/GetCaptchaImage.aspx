<%--
Copyright 2011 U.S. Department of Defense

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
--%>

<%@ Page Language="C#" %>

<%@ Import Namespace="System.Drawing.Imaging" %>
<%@ Import Namespace="CaptchaDLL" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">
    private void Page_Load(object sender, System.EventArgs e)
    {
        if (Session["CaptchaImageText"] != null)
        {
            // CREATE A CAPTCHA IMAGE USING THE TEXT STORED IN THE SESSION OBJECT.
            CaptchaImage ci = new CaptchaImage(Session["CaptchaImageText"].ToString(), 200, 50);

            //YOU CAN USE THE OTHER OVERLOADED METHODS ALSO
            //CaptchaImage ci = new CaptchaImage(Session["CaptchaImageText"].ToString(), 200, 50, "Courier New");
            //CaptchaImage ci = new CaptchaImage(Session["CaptchaImageText"].ToString(), 200, 50, "Courier New" ,System.Drawing.Color.White, System.Drawing.Color.Red);

            // Change the response headers to output a JPEG image.
            this.Response.Clear();
            this.Response.ContentType = "image/jpeg";

            // Write the image to the response stream in JPEG format.
            ci.Image.Save(this.Response.OutputStream, ImageFormat.Jpeg);

            // Dispose of the CAPTCHA image object.
            ci.Dispose();
        }
    }
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
</head>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
</body>
</html>
