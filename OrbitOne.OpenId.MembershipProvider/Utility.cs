using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;

namespace OrbitOne.OpenId.MembershipProvider
{
   internal class Utility
    {

        private static readonly string EVENT_SOURCE = "OpenIDMembershipProvider";
        private static readonly string EVENT_LOG = "Application";



       public static string GetConfigValue(string configValue, string defaultValue)
       {
           if (String.IsNullOrEmpty(configValue))
               return defaultValue;

           return configValue;
       }

       public static string GenerateNewSalt()
       {
           byte[] saltInBytes = new byte[16];
           System.Security.Cryptography.RNGCryptoServiceProvider saltGenerator = new System.Security.Cryptography.RNGCryptoServiceProvider();
           saltGenerator.GetBytes(saltInBytes);
           return Convert.ToBase64String(saltInBytes);
       }

       public static string GenerateNewGUID()
       {
           return Guid.NewGuid().ToString();
       }


       public static Uri NormalizeIdentityUrl(String identityUrl)
       {
           Uri retVal = null;
           // To get an iname to fit onto a Uri object, we prefix
           // with "xri:". This is because Uri object will not allow "xri://".
           if (identityUrl.StartsWith("xri://"))
           {
               identityUrl = identityUrl.Substring("xri://".Length);
               retVal = new Uri("xri:" + identityUrl);
           }
           else if (identityUrl.StartsWith("=") || identityUrl.StartsWith("@"))
           {
               retVal = new Uri("xri:" + identityUrl);
           }
           else if (!identityUrl.StartsWith("http://"))
           {
               retVal = Janrain.OpenId.UriUtil.NormalizeUri(string.Format("http://{0}/", identityUrl.Trim("/".ToCharArray())));

           }
           else
           {
               retVal = Janrain.OpenId.UriUtil.NormalizeUri(identityUrl);
           }
           return retVal;
       }

       public static String IdentityUrlToDisplayString(Uri identityUrl)
       {
           String absoluteUri = identityUrl.AbsoluteUri;
           if (absoluteUri.StartsWith("xri:"))
           {
               absoluteUri = absoluteUri.Substring("xri:".Length);
           }
           return absoluteUri;
       }
       public static byte[] HexToByte(string hexString)
       {
           byte[] returnBytes = new byte[hexString.Length / 2];
           for (int i = 0; i < returnBytes.Length; i++)
               returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
           return returnBytes;
       }


       public static void WriteToEventLog(Exception e, string action)
       {
           EventLog log = new EventLog();
           log.Source = EVENT_SOURCE;
           log.Log = EVENT_LOG;

           string message = "An exception occurred communicating with the data source.\n\n";
           message += "Action: " + action + "\n\n";
           message += "Exception: " + e.ToString();

           log.WriteEntry(message);
       }


    }
}
