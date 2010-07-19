using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Website
{

    public class Javascript
    {
        private static string ScriptFormatString = @"<script language=""javascript"" type=""text/javascript"">{0}</script>";
        public static string BackFormatString = @"<a href='javascript: history.go(-1);'>{0}</a>";

        public Javascript()
        {

        }

        public static void Alert(string message)
        {
            string alertScript = String.Format("alert('{0}');", message);
            HttpContext.Current.Response.Write(WrapInScript(alertScript));

        }

        public static void Confirm(WebControl ctl, string message)
        {
             string confirmScript  = String.Format("javascript:return confirm('{0}');", message);
             ctl.Attributes.Add("onclick", confirmScript);

        }

        public static void RefreshOpener()
         {
             HttpContext.Current.Response.Write(WrapInScript("window.opener.document.location.reload();"));
         }





        public static string WrapInScript(string scriptInnerText)
        {

            return String.Format(ScriptFormatString, scriptInnerText);
        }


    }


}
