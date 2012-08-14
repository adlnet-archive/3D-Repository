using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TokenValidator
/// </summary>
public class TokenValidator
{
    string token;
    string email;

	public TokenValidator(string address)
	{
        email = address;
		//
		// TODO: Check whether or not this email address has an outstanding token
		//

	}

    public bool generateTokenEmail()
    {

        return Website.Mail.sendTokenEmail(email, "Hello bob");
    }

    public bool ValidateUserToken()
    {


        return true;
    }
}