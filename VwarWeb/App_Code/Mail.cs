//  Copyright 2011 U.S. Department of Defense

//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at

//      http://www.apache.org/licenses/LICENSE-2.0

//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.



using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Text;
using System;
using System.Web.UI;
using System.Collections;

namespace Website
{
    /// <summary>
    /// 
    /// </summary>
    public static class Mail
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="fromAddress"></param>
        /// <param name="fromName"></param>
        /// <param name="bccAddress"></param>
        /// <param name="ccAddress"></param>
        /// <param name="isHtmlFormat"></param>
        /// <param name="attachmentFileName"></param>
        public static void SendSingleMessage(string body, string toAddress, string subject, string fromAddress, string fromName, string bccAddress, string ccAddress, bool isHtmlFormat, string attachmentFileName)
        {
            fromAddress = "";
            fromName = "";
            bccAddress = "";
            ccAddress = "";
            isHtmlFormat = false;
            attachmentFileName = "";

            //emailing functionality active?
            if (Website.Config.EmailingActive)
            {

                //from address 
                MailAddress f = default(MailAddress);
                if (string.IsNullOrEmpty(fromAddress))
                {
                    //defaultfrom address
                    f = new MailAddress(Website.Config.DefaultEmailFromAddress, Website.Config.CompanyName);
                }
                else
                {
                    //optional from address
                    if (fromName.Equals(string.Empty))
                    {
                        fromName = Website.Config.CompanyName;
                    }
                    f = new MailAddress(fromAddress, fromName);
                }


                //msg subjet and body
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(f, new MailAddress(toAddress));
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = false;

                //bcc address(es)?
                if (!bccAddress.Trim().Equals(string.Empty))
                {
                    //multiple bccs - split if bcc has ';'
                    if (bccAddress.IndexOf(";") > -1)
                    {
                        string[] bccs = bccAddress.Split(';');
                        for (int i = 0; i <= bccs.Length - 1; i++)
                        {
                            message.Bcc.Add(bccs[i].ToString().Trim());
                        }
                    }
                    else
                    {
                        message.Bcc.Add(bccAddress);
                    }
                }

                //cc address(es)
                if (!ccAddress.Trim().Equals(string.Empty))
                {
                    //multiple cc addresses - split if cc has ';'
                    if (ccAddress.IndexOf(";") > -1)
                    {
                        string[] ccs = ccAddress.Split(';');
                        for (int i = 0; i <= ccs.Length - 1; i++)
                        {
                            message.CC.Add(ccs[i].ToString().Trim());
                        }
                    }
                    else
                    {
                        message.CC.Add(ccAddress);
                    }
                }

                //attachment? (does not work for bulk emails)
                if (!attachmentFileName.Equals(string.Empty) && System.IO.File.Exists(attachmentFileName))
                {
                    message.Attachments.Add(new Attachment(attachmentFileName));
                }

                //set SMTP server/credentials differently for development and production environments
                SmtpClient smtp = default(SmtpClient);
               // if (Website.Config.IsProductionEnvironment)
                {
                    //production environment
                    if (Website.Config.UseWebServersBuiltInSmtpMailServer)
                    {
                        //send email from web server's builtin smtp server
                        smtp = new SmtpClient("127.0.0.1");
                    }
                    else
                    {
                        //send from remote SMTP server
                        smtp = new SmtpClient(Website.Config.ProductionEmailSmtpServer, 587);
                        smtp.UseDefaultCredentials = false;
                        smtp.EnableSsl = true;
                        smtp.Credentials = new System.Net.NetworkCredential(Website.Config.ProductionEmailUsername, Website.Config.ProductionEmailPassword);
                    }
                }

                //send msg
                try
                {
                    smtp.Send(message);
                }
                catch (Exception ex)
                {
                    //trace error so user cannot see - catch at development time
                    HttpContext.Current.Trace.Warn("Mail Error at " + smtp.Host, ex.ToString());
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        public static void SendForgotPassword(string email)
        {
            if ((Membership.GetUser(email) != null))
            {
                MembershipUser mu = Membership.GetUser(email);
                if (!mu.IsLockedOut)
                {
                    string pwd = mu.GetPassword();
                    string subject = Website.Config.CompanyName + " Password Reminder";
                    StringBuilder body = new StringBuilder();
                    body.Append(Website.Config.CompanyName);
                    body.Append(" Password Reminder");
                    body.Append(Environment.NewLine);
                    body.Append(Environment.NewLine);
                    body.Append("Below is the password you requested to access your account. Please keep this information in a secure place.");
                    body.Append(Environment.NewLine);
                    body.Append(Environment.NewLine);
                    body.Append("Your password is: ");
                    body.Append(pwd);

                    //signature
                    body.Append(SiteSignature());

                    try
                    {
                        Website.Mail.SendSingleMessage(body.ToString(), email, subject, "", "", "", "", false, "");
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="toAddress"></param>
        /// <param name="ccEmailAddress"></param>
        /// <param name="subject"></param>
        /// <param name="domain"></param>
        /// <param name="email"></param>
        /// <param name="update"></param>
        /// <param name="category"></param>
        /// <param name="priority"></param>
        /// <param name="shortDescription"></param>
        /// <param name="status"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static string SendSupportRequest(string toAddress, string ccEmailAddress, string subject, string domain, string email, string update, string category, string priority, string shortDescription, string status,
        string description)
        {
            string rv = "";
            StringBuilder body = new StringBuilder();

            //header
            body.Append(Website.Config.SupportRequestEmailHeader).Append(Environment.NewLine).Append(Environment.NewLine);

            //body
            if (!update.Equals(string.Empty))
            {
                body.Append("Update:").Append(Environment.NewLine);
                body.Append(update);
                body.Append(Environment.NewLine).Append(Environment.NewLine);
                body.Append("------------------------------------------------------------------");
                body.Append(Environment.NewLine).Append(Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(domain))
            {
                body.Append("Domain: ").Append(domain).Append(Environment.NewLine);
            }
            if (!string.IsNullOrEmpty(email))
            {
                body.Append("Email: ").Append(email).Append(Environment.NewLine);
            }

            body.Append("Category: ").Append(category).Append(Environment.NewLine);
            body.Append("Priority: ").Append(priority).Append(Environment.NewLine);

            if (!string.IsNullOrEmpty(shortDescription))
            {
                body.Append("Short Description: ").Append(shortDescription).Append(Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(shortDescription))
            {
                body.Append("Description: ").Append(description).Append(Environment.NewLine).Append(Environment.NewLine);
            }

            //footer
            body.Append(Website.Config.SupportRequestEmailFooter);

            //signature
            body.Append(SiteSignature());

            string msgBody = body.ToString();
            Website.Mail.SendSingleMessage(msgBody, toAddress, subject, "", "", "", ccEmailAddress, false, "");

            return msgBody;
        }

        /*public static void SendNewRegistrationNotificationEmail(MembershipUser newUser)
        {
            if (newUser != null) {
                string subject = Website.Config.SiteName + " - New User Registration ";
                StringBuilder body = new StringBuilder();
                string url = Website.Pages.Types.AdministratorsManageUsers;
                try {
                    Page p = (Page)HttpContext.Current.Handler;
                    url = p.ResolveUrl(Website.Pages.Types.FormatManageUsersUrl(newUser.Email));
                }
                catch (Exception ex) {
                    
                }
                
                //body
                body.Append("A new user has just registered for ");
                body.Append(Website.Config.SiteName).Append(".");
                body.Append(System.Environment.NewLine).Append(System.Environment.NewLine);
                
                body.Append("Visit the link below to view the new user's information.");
                body.Append(System.Environment.NewLine).Append(System.Environment.NewLine);
                body.Append(Website.Config.DomainName);
                
                //link to user info
                body.Append(url);
                
                //signature
                body.Append(SiteSignature());
                try {
                    Website.Mail.SendSingleMessage(body.ToString(), Website.Config.NewRegistrationNotificationEmailToAddress, subject, Website.Config.DefaultEmailFromAddress, Website.Config.CompanyName, "", "", false, "");
                }
                catch (Exception ex) {
                    
                    
                }
                
                
            }
        }*/
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static string SiteSignature()
        {
            string rv = "";
            StringBuilder body = new StringBuilder();
            body.Append(System.Environment.NewLine);
            body.Append(System.Environment.NewLine);
            body.Append(System.Environment.NewLine);
            body.Append(Website.Config.CompanyName);
            body.Append(System.Environment.NewLine);
            body.Append(Website.Config.DomainName);
            rv = body.ToString();
            return rv;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        public static void SendRegistrationConfirmation(string email)
        {
            if ((Membership.GetUser(email) != null))
            {
                MembershipUser user = Membership.GetUser(email);
                string subject = Website.Config.CompanyName + " Account Confirmation";
                StringBuilder body = new StringBuilder();
                body.Append("Dear ");
                body.Append(user.Email.Trim());
                body.Append(",");
                body.Append(Environment.NewLine);
                body.Append(Environment.NewLine);
                body.Append("Your account profile has been successfully created.");
                body.Append(Environment.NewLine);
                body.Append(Environment.NewLine);
                //body.Append("You may now access additional information by signing-in to ");
                body.Append("You will receive an additional email once your account is approved");
                //body.Append(Website.Config.DomainName);
                //body.Append(" with your email address and password.");

                //signature
                body.Append(SiteSignature());
                try
                {
                    Website.Mail.SendSingleMessage(body.ToString(), email, subject, "", "", "", "", false, "");
                }
                catch
                {


                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        public static void SendRegistrationApprovalEmail(string email)
        {
            if ((Membership.GetUser(email) != null))
            {
                MembershipUser user = Membership.GetUser(email);
                string subject = "Welcome to the ADL 3D Repository";
                StringBuilder body = new StringBuilder();
                body.Append(String.Format(@"Hello,

Thanks for signing up for the ADL 3D Repository!  Your username is:

{0}


Please head on over to {1} Here are some things you can do to get started:

   1.  Upload a model
   2.  Download a model
   3.  Send us feedback: cybrarian@somecompany.com


If you’re not already a member of the 3D Repositories Google Group, please consider joining:

http://groups.google.com/group/3d-repositories


- The ADL 3D Repository Team", email, Website.Config.DomainName));



                //signature               
                try
                {
                    Website.Mail.SendSingleMessage(body.ToString(), email, subject, "", "", "", "", false, "");
                }
                catch
                {


                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        public static void SendAccountUnlockedEmail(string email)
        {
            if ((Membership.GetUser(email) != null))
            {
                MembershipUser user = Membership.GetUser(email);
                string subject = Website.Config.CompanyName + " Account Unlock";
                StringBuilder body = new StringBuilder();
                body.Append("Dear ");
                body.Append(user.Email.Trim());
                body.Append(",");
                body.Append(Environment.NewLine);
                body.Append(Environment.NewLine);
                body.Append("Your account has been unlocked.");
                body.Append(Environment.NewLine);
                body.Append(Environment.NewLine);
                body.Append("You may now access additional information by signing-in to ");

                body.Append(Website.Config.DomainName);
                body.Append(" with your email address and password.");

                //signature
                body.Append(SiteSignature());
                try
                {
                    Website.Mail.SendSingleMessage(body.ToString(), email, subject, "", "", "", "", false, "");
                }
                catch
                {


                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentObjID"></param>
        /// <param name="contentObjectName"></param>
        public static void SendReportViolationEmail(string contentObjID, string contentObjectName, string description, string userEmail)
        {

            if (!string.IsNullOrEmpty(contentObjID))
            {
                string url = Website.Config.DomainName + "/Public/Model.aspx?ContentObjectID=" + contentObjID;


                string subject = "3DR Violation Report";
                StringBuilder body = new StringBuilder();


                body.Append("A violation has been reported for a model identified by " + contentObjectName +
                            ". Click on the following link to view the content object.");
                body.Append(System.Environment.NewLine).Append(System.Environment.NewLine);
                body.Append(url + System.Environment.NewLine);
                body.AppendFormat("Reporter's Email: {0}\nViolation Description: {1}", userEmail, description);

                //signature
                body.Append(SiteSignature());
                try
                {
                    Website.Mail.SendSingleMessage(body.ToString(), Website.Config.CybrarianEmail, subject, "", "", "", "", true, "");
                }
                catch
                {


                }

            }


        }
        /// <summary>
        /// email checking routine 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string email)
        {
            bool isValid = false;

            string pattern = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            System.Text.RegularExpressions.Regex regularExpression = new System.Text.RegularExpressions.Regex(pattern);
            isValid = regularExpression.IsMatch(email.Trim());

            return isValid;
        }
        /// <summary>
        /// sends bulk emails to items in dt 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="emailColumnName"></param>
        /// <param name="senderCopyAddress"></param>
        /// <param name="bccRecipientsPerEmail"></param>
        public static void SendBulkEmail(DataTable dt, string subject, string body, string emailColumnName, string senderCopyAddress, int bccRecipientsPerEmail)
        {
            emailColumnName = "Email";
            senderCopyAddress = "";
            bccRecipientsPerEmail = 5;

            //config control
            if (Config.EmailingActive)
            {
                ArrayList addresses = new ArrayList();

                //loop through dt
                foreach (DataRow dr in dt.Rows)
                {

                    //check for null
                    if ((!object.ReferenceEquals(dr[emailColumnName], System.DBNull.Value)))
                    {
                        string currentEmail = dr[emailColumnName].ToString().ToLower().Trim();

                        //validate
                        if (IsValidEmail(currentEmail))
                        {
                            addresses.Add(currentEmail);

                            //check limit
                            if (addresses.Count == bccRecipientsPerEmail)
                            {

                                //to (bcc)
                                string bcc = string.Join(";", (string[])addresses.ToArray(Type.GetType("System.String")));

                                //send
                                SendSingleMessage(body, "", subject, "", "", bcc, "", false, "");

                                //clear addresses
                                addresses.Clear();

                            }

                        }
                    }
                }

                //leftovers
                if (addresses.Count > 0)
                {
                    string bcc = string.Join(";", (string[])addresses.ToArray(Type.GetType("System.String")));
                    SendSingleMessage(body, "", subject, "", "", bcc, "", false, "");
                }

                //copy to admin
                if (!senderCopyAddress.Equals(string.Empty))
                {
                    SendSingleMessage(body, senderCopyAddress, subject, "", "", "", "", false, "");
                }

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <param name="questionRelatesTo"></param>
        /// <param name="question"></param>
        /// <returns></returns>
        public static string SendContactUsEmail(string firstName, string lastName, string email, string phone, string questionRelatesTo, string question)
        {

            string subject = "Contact Requested";

            StringBuilder body = new StringBuilder();
            body.Append("A contact request has been sent from ").Append(firstName).Append(" ").Append(lastName).Append(".");
            body.Append(System.Environment.NewLine);

            //qeustion relates to
            if (!string.IsNullOrEmpty(questionRelatesTo))
            {
                body.Append("The users questions relates to: ").Append(questionRelatesTo).Append(".");
                body.Append(System.Environment.NewLine);
            }

            //question            
            body.Append("Question: ").Append(question);


            body.Append(System.Environment.NewLine).Append(System.Environment.NewLine);
            body.Append("Please contact the user at the following email: ").Append(email);
            if (!string.IsNullOrEmpty(phone))
            {
                body.Append(" or the following phone number ").Append(phone).Append(".");
            }
            else
            {
                body.Append(".");
            }



            //signature
            body.Append(SiteSignature());

            //send
            try
            {
                Website.Mail.SendSingleMessage(body.ToString(), Website.Config.CybrarianEmail, subject, "", "", "", "", false, "");
            }
            catch
            {


            }


            return body.ToString();

        }
    }
}
