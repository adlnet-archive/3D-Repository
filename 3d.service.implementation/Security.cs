using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Security.Cryptography;

namespace vwar.service.implementation
{
    public class Security
    {
        //Get the membership provider. This should be set up in the cofig
        static private Simple.Providers.MySQL.MysqlMembershipProvider mProvider;
        static public Simple.Providers.MySQL.MysqlMembershipProvider GetProvider()
        {
            //Make sure the provider is setup before accessing
            initialize();
            return mProvider;
        }
        //Flag to show if the system is initialized
        static private bool mInitialized = false;
        //Initialize the system, get the provider and setup default user
        static private void initialize()
        {
            if (!mInitialized)
            {
                //Get the provider. Should be setup in the config
                mProvider = (Simple.Providers.MySQL.MysqlMembershipProvider)(Membership.Providers["3DRAPIMembership"]);

                //Setup the default user
                if (mProvider.GetUser("API_USER", false) == null)
                {
                    MembershipCreateStatus mcs = new MembershipCreateStatus();

                    mProvider.CreateUser("API_USER",
                                            EncodeBytesAsHex(Hash("password")),
                                            "test@test.com",
                                            "",
                                            "",
                                            true,
                                            null,
                                            out mcs);
                }
                else
                {
                    //Make sure the password for the default user is password
                    mProvider.GetUser("API_USER", false).ChangePassword(mProvider.GetUser("API_USER", false).GetPassword(), EncodeBytesAsHex(Hash("password")));
                }
                mInitialized = true;
            }
        }
        //Take a username, a hash and the url. Decide if the supplied hash is correct
        static bool ValidateURL(string url, string username, byte[] signature)
        {

            initialize();

            //Get the password
            byte[] result;
            SHA256 shaM = new SHA256Managed();

            string password = mProvider.GetPassword(username, "");


            //Concat the username and password with the url 
            string fullurl = url + username + password;

            //Get the hash of the url plus name plus password
            result = Hash(fullurl);

            //Must be 32 bytes
            if (signature.Length != 32)
                throw (new SystemException("Message Signature must be 32 bytes"));

            //Must be 32 bytes
            if (result.Length != 32)
                throw (new SystemException("Something is seriously wrong. Hash must be 32 bytes"));

            //Check to see if they match
            bool match = true;
            for (int i = 0; i < 32; i++)
                match = match && (signature[i] == result[i]);

            return match;
        }

        //Get the hash for a string
        private static byte[] Hash(string url)
        {
            byte[] result;
            SHA256 shaM = new SHA256Managed();

            byte[] ms = new byte[url.Length];

            for (int i = 0; i < url.Length; i++)
            {
                byte b = Convert.ToByte(url[i]);
                ms[i] = (b);
            }

            shaM.Initialize();

            result = shaM.ComputeHash(ms, 0, ms.Length);

            return result;

        }
        //This just does the base64 encoding, used to encode as ascii encoding of hex
        public static string EncodeBytesAsHex(byte[] bytes)
        {

            //dont know what the hell I was thinking. just going to swithc out for base64
            return System.Convert.ToBase64String(bytes);


            //int[] hashchars = new int[bytes.Length * 2];
            //for (int i = 0; i < bytes.Length; i++)
            //{
            //    hashchars[i * 2 + 1] = (bytes[i] & 15);
            //    hashchars[i * 2 + 0] = ((bytes[i] >> 4) & 15);

            //}

            //string hash = "";
            //for (int i = 0; i < hashchars.Length; i++)
            //{
            //    if (hashchars[i] >= 0 && hashchars[i] <= 9)
            //        hash += Convert.ToString(hashchars[i]);
            //    else if (hashchars[i] == 10)
            //        hash += 'a';
            //    else if (hashchars[i] == 11)
            //        hash += 'b';
            //    else if (hashchars[i] == 12)
            //        hash += 'c';
            //    else if (hashchars[i] == 13)
            //        hash += 'd';
            //    else if (hashchars[i] == 14)
            //        hash += 'e';
            //    else if (hashchars[i] == 15)
            //        hash += 'f';
            //}
            //return hash;
        }
        //Pull the username out of a string formated username:hash
        public static string GetUsernameFromHeader(string header)
        {
            int colon = header.IndexOf(':');
            return header.Substring(0, colon);
        }
        //Used in the hex encoding, get a byte for a character
        static byte CharToByte(char c)
        {
            if (c >= '0' && c <= '9')
                return (byte)Convert.ToInt32(c.ToString());
            if (c == 'A' || c == 'a')
                return 10;
            if (c == 'B' || c == 'b')
                return 11;
            if (c == 'C' || c == 'c')
                return 12;
            if (c == 'D' || c == 'd')
                return 13;
            if (c == 'E' || c == 'e')
                return 14;
            if (c == 'F' || c == 'f')
                return 15;

            throw (new SystemException("HashString should only contain ascii [0-9][a-f][A-F]"));


        }
        //This used to use an ascii encoded hex string, not just does base 64
        static byte[] HexStringtoBytes(string hex)
        {
            // dont konw what I was thinking, but this should just be base 64

            return System.Convert.FromBase64String(hex);

            //byte[] result = new byte[hex.Length / 2];

            //for (int i = 0; i < hex.Length / 2; i++)
            //{
            //    byte b1 = CharToByte(hex[i * 2 + 1]);
            //    byte b2 = CharToByte(hex[i * 2 + 0]);
            //    b2 = (byte)(b2 << 4);
            //    result[i] = (byte)(b1 + b2);
            //}
            //return result;
        }
        //Pulls the hash out of a string formated username:hash
        static byte[] GetHashFromHeader(string header)
        {
            int colon = header.IndexOf(':');
            string asciiHexHash = header.Substring(colon + 1);
            return HexStringtoBytes(asciiHexHash);
        }

        //This enumerates the different types of operations on a content object
        public enum TransactionType { Create, Delete, Modify, Access, Query };

        //Do the business logic related to checking a url against the auth string, and checking if the user is allowed
        //to do the operation on the content object
        static public bool ValidateUserTransaction(string url, string auth, TransactionType type, vwarDAL.ContentObject co)
        {
            //For now, anyone can query
            if (type == TransactionType.Query)
            {
                return true;
            }
            if (type == TransactionType.Access)
            {

                //No auth control for Access transactions 
                return true;
            }
            if (type == TransactionType.Create)
            {
                //The user exists in the provider and gave the correct url+password hash
                //Here we assume that any user can create content
                return ValidateURL(url, GetUsernameFromHeader(auth), GetHashFromHeader(auth));
            }
            if (type == TransactionType.Modify)
            {
                //The user exists in the provider and gave the correct url+password hash
                if (ValidateURL(url, GetUsernameFromHeader(auth), GetHashFromHeader(auth)))
                {
                    //the user must be the owner of the content object
                    if (Security.GetProvider().GetUser(GetUsernameFromHeader(auth), false).Email == co.SubmitterEmail || GetUsernameFromHeader(auth) == "API_USER")
                    {
                        return true;
                    }
                }
                return false;
            }
            if (type == TransactionType.Delete)
            {
                //The user exists in the provider and gave the correct url+password hash
                if (ValidateURL(url, GetUsernameFromHeader(auth), GetHashFromHeader(auth)))
                {
                    //the user must be the owner of the content object
                    if (Security.GetProvider().GetUser(GetUsernameFromHeader(auth), false).Email == co.SubmitterEmail || GetUsernameFromHeader(auth) == "API_USER")
                    {
                        return true;
                    }
                }
                return false;
            }

            return false;
        }
    };
}