/*********************************************************************
 * Rob Chadwick
 * 5/3/2010
 * 
 * This page will convert and return a file from the content directory into the 
 * vastpark format.
 * 
 * Prereq's: requires the vastpark provided model converter in the /bin directory
 * 
 * Input: the querystring should be ContentID=[contentID]&UserName=[username]&Password=[password]
 *        the password can be encrypted or plain text
 *        the encryption is 8 bit DES, with a key of "!#$a54?3"
 *        and an initializaion vector of 01234567
 *      
 * Output: The page response either contains the converted file or an error message
 * 
 * **********************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Diagnostics;
using System.Text;
using System.Security.Cryptography;

//Utility to encrypt strings
namespace ExtractAndSerialize
{
    public class Encryption64
    {
        //The key
        private byte[] key;
        //The initialization vector
        private byte[] IV = { 0, 1, 2, 3, 4, 5, 6, 7 }; //= new System.Array(0xH12, 0xH34, 0xH56, 0xH78, 0xH90, 0xHAB, 0xHCD, 0xHEF);

        //decrypt a string with the given key 
        public string Decrypt(string stringToDecrypt, string sEncryptionKey)
        {
            //move the string into a byte array
            byte[] inputByteArray;
            try
            {
                //get the first 8 bytes of the key
                key = System.Text.Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
                //create the service
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                //convert from base64 encoding (which represents bytes as ascii)
                inputByteArray = Convert.FromBase64String(stringToDecrypt);
                //stream to hold decrypted data
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                //attach cryptography to the ms memory stream. Decrypt with key and vector
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                //decrypt
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                //read the decrypted memory stream to a string
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                //return the string
                return encoding.GetString(ms.ToArray());
            }
            //return the error message
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        //encrypt a string into a base64 encoded encrypted string
        public string Encrypt(string stringToEncrypt, string SEncryptionKey)
        {
            try
            {
                //get the first 8 bytes of the key
                key = System.Text.Encoding.UTF8.GetBytes(SEncryptionKey.Substring(0, 8));
                //create the crypto service
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                //byte array to hold the string as bytes
                Byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                //memorystream to hold the encoded data
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                //attach a cryptostream to the memory stream, encrypt with key and vector
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                //write the input bytes
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                //convert the encrypted memory stream to base64, so can be sent in ascii
                return Convert.ToBase64String(ms.ToArray());
            }
            //return the error message
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    };
}



public partial class Public_Default : Website.Pages.PageBase
{
    int TIME_TO_WAIT_FOR_CONVERSION = 120;                         //the number of seconds to wait for vastpark conversion
    string VASTPARK_CONVERTER_FILENAME = "ModelPackager.exe";      //The model packager executable name
    string ENCRYPTION_KEY = "!#$a54?3";                            //the encryption key

    //Encrypt a string
    public string encryptQueryString(string strQueryString)
    {
        ExtractAndSerialize.Encryption64 oES =
            new ExtractAndSerialize.Encryption64();
        return oES.Encrypt(strQueryString, ENCRYPTION_KEY);
    }

    //Decrypt a string
    public string decryptQueryString(string strQueryString)
    {
        ExtractAndSerialize.Encryption64 oES =
            new ExtractAndSerialize.Encryption64();
        return oES.Decrypt(strQueryString, ENCRYPTION_KEY);
    }

    //Print the unknown user message
    protected void UnknownUser()
    {
        Response.Write("Error: Unknown User name.");
    }
    //print the invalid Query message
    protected void InvalidQuery()
    {
        Response.Write("Error: The query format is incorrect.");
    }
    //print the invalid password message
    protected void InvalidPassword()
    {
        Response.Write("Error: Incorrect Password.");
    }
    //print the invalid content ID message
    protected void InvalidContentID()
    {
        Response.Write("Error: The content ID is invalid.");
    }
    //on page load
    protected void Page_Load(object sender, EventArgs e)
    {

        int ContentID;      //The requested content ID
        string UserName;    //The username
        string Password;    //the password
        string decryptedPassword; //the password after decryption
        string ContentPath; // the local file system path to the content directory
        string ContentFile; // the local file system path and name of the .dae file
        string ConverterPath; // the local file system path to the vastpark converter

        //if the contentID param is not in the request, show the error message
        if (Request.Params["ContentID"] != null)
            ContentID = System.Convert.ToInt32(Request.Params["ContentID"]);
        else
        {
            InvalidQuery();
            return;
        }

        //If the username param is not in the request, show the error message
        if (Request.Params["UserName"] != null)
            UserName = Request.Params["UserName"];
        else
        {
            InvalidQuery();
            return;
        }

        //if the password is not in the ID, show the error message
        if (Request.Params["Password"] != null)
            Password = Request.Params["Password"];
        else
        {
            InvalidQuery();
            return;
        }

        //get the link to the data layer, and connect to the database
        try
        {
            vwarDAL.IDataRepository vd = DAL;
        }
        //if the datalayer fails, print error and end
        catch (Exception ex2)
        {
            Response.Write("Error: Cannot access content database.");
            return;
        }

        //Get the member from the membership data store
        MembershipUser mu = null;
        try
        {
            mu = Membership.GetUser(UserName);
        }
        //The user could not be loaded
        catch (Exception ex)
        {
            UnknownUser();
            return;
        }

        //Decrypt the password
        decryptedPassword = decryptQueryString(Password.Replace(" ", "+"));

        //check the users password. Either the encrypted or unencrypted password will work
        if (mu.GetPassword() != Password && decryptedPassword != mu.GetPassword())
        {
            InvalidPassword();
            return;
        }

        //Combine the path of the webserver and "Content"
        ContentPath = Path.Combine(Request.PhysicalApplicationPath, "Content");
        //Add the content ID as a string to the path
        ContentPath = Path.Combine(ContentPath,ContentID.ToString());

        //If the path does not exist, then show the error message
        if (!Directory.Exists(ContentPath))
        {
            InvalidContentID();
            return;
        }

        //Find the first sub directory in the content path
        string[] SubDirs;
        SubDirs = Directory.GetDirectories(ContentPath);

        //there should be a single subdirectory
        if (SubDirs.Count() == 0)
        {
            Response.Write("Error: Directory invalid");
            return;
        }

        //Combine the content path and its first sub directory
        ContentPath = Path.Combine(ContentPath, SubDirs[0]);

        //Get the filename of the first dae file
        string[] Files = Directory.GetFiles(ContentPath, "*.dae",SearchOption.AllDirectories);
        if (Files.Count() == 0)
        {
            Response.Write("Error: There is no .dae file in the directory");
            return;
        }

        //combine the filename. Now, ContentPath contains the absolute path to the 
        //dae file of the requested ContentObjectID
        ContentFile = Path.Combine(ContentPath, Files[0]);

        //Get the path to the converter
        ConverterPath = Path.Combine(Path.Combine(Request.PhysicalApplicationPath, "bin"), VASTPARK_CONVERTER_FILENAME);

        //Check that the converter is in the bin directory
        if (!File.Exists(ConverterPath))
        {
            Response.Write("Error: Cannot find the VastPark converter");
        }

        //Build the process info to execute the converter on the content
        System.Diagnostics.ProcessStartInfo Converter = new System.Diagnostics.ProcessStartInfo(ConverterPath);
        Converter.Arguments = String.Format("\"{0}\"", ContentFile);
        Converter.WindowStyle = ProcessWindowStyle.Hidden;
        Converter.RedirectStandardError = true;
        Converter.CreateNoWindow = true;
        Converter.UseShellExecute = false;
        
        //start the process
        Process p = Process.Start(Converter);
        
        //wait for the converter to exit
        //or for 120 seconds
        int SecondCount = TIME_TO_WAIT_FOR_CONVERSION;
        while (!p.HasExited && SecondCount > 0)
        {
            SecondCount--;
            System.Threading.Thread.Sleep(1000);
        }
        var error = p.StandardError.ReadToEnd();

        //Get the directory contents matching *.model
        string[] NewFiles = Directory.GetFiles(ContentPath, "*.model",SearchOption.AllDirectories);

        //There should now be a .model file
        if (NewFiles.Count() == 0)
        {
            Response.Write("Error: The Vastpark conversion was unsuccessful.");
            return;
        }

        //Get the name of the new .model file
        string ConvertedFileName = Path.Combine(ContentPath , NewFiles[0]);

        
        //Send this file as the response
        Response.ContentType = "application/octet-stream";
        Response.WriteFile(ConvertedFileName);
        
    }
}
