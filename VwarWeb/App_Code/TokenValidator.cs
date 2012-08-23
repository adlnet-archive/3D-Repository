using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using Simple.Providers.MySQL;
using Website;
using System.Web.Security;

/// <summary>
/// Summary description for TokenValidator
/// </summary>
public class TokenValidator
{
    string token;
    string email;
    string connectionString;

    DateTime now = DateTime.Now;

	public TokenValidator(string address)
	{
        email = address;
        connectionString = getConnectionString();
	}

    public TokenValidator(string address, string pToken)
    {
        email = address;
        token = pToken;
        connectionString = getConnectionString();
    }

    public bool generateTokenEmail()
    {
        Random r = new Random(DateTime.Now.Millisecond);
        token = getSHA256Hash(r.Next(10000).ToString() + DateTime.Now.Millisecond.ToString() + email + r.Next(10000).ToString());

        if (Mail.sendTokenEmail(email, generateBody()))
        {
            RawDBQuery.StoreNewToken(email, token, now.AddHours(12), connectionString);
            return true;
        }

        return false;
    }

    public string generateBody()
    {
        var request = HttpContext.Current.Request;
        string uri = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, request.ApplicationPath.TrimEnd('/')) + 
                                   "/Public/ChangePassword.aspx?email=" + email + "&t=" + token;

        string body = "<html><body>In order to change your password, you must <a href=\""+uri+"\">click here</a>. You will then be asked to enter a new password. <br/><br/>If you are unable" + 
                      " to click the link, please copy and paste this address into your address bar: <br/>" + uri + "<br/><br/>Please do not reply to this email.</html></body>";

        return body;
    }

    public bool ValidateUserToken()
    {
        return Mail.IsValidEmail(email) && RawDBQuery.checkUserToken(email, token, now, connectionString);
    }

    public string getConnectionString()
    {

        string pConnectionString =
            System.Configuration.ConfigurationManager.ConnectionStrings["postgreSQLConnectionString"].ConnectionString;

        if (pConnectionString == null || pConnectionString.Trim() == "")
        {
            throw new System.Configuration.Provider.ProviderException("Connection string cannot be blank.");
        }

        return pConnectionString;
    }

    public string getSHA256Hash(string tInput)
    {
        byte[] input = Encoding.UTF8.GetBytes(tInput);
        SHA256 hash = SHA256CryptoServiceProvider.Create();

        //Makes letters lowercase and removes hyphens to standardize the look of the hash, and make it URI friendly
        return (BitConverter.ToString(hash.ComputeHash(input)).ToLower().Replace("-", ""));
    }

    public void deleteUserTokens()
    {

        RawDBQuery.deleteUserTokens(email, connectionString);
    }
}