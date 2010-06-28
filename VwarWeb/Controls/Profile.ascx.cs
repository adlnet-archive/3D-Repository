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
using vwarDAL;

public partial class Controls_Profile : System.Web.UI.UserControl
{




    protected void Page_Load(object sender, EventArgs e)
    {
        Trace.IsEnabled = true;

        if (!Page.IsPostBack)
        {
            MultiView1.ActiveViewIndex = 0;
        }
    }
    protected void ChangePasswordLinkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Users/ChangePassword.aspx");
    }
    protected void EditProfileLinkButton_Click(object sender, EventArgs e)
    {
        //show edit view
        MultiView1.SetActiveView(EditProfileView);

        this.BindProfile();

        
    }
    protected void CancelButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }
    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        //update profile
        MembershipUser mu = Membership.GetUser(Context.User.Identity.Name.Trim());

        //user guid
        string memGuid = mu.ProviderUserKey.ToString();
       
        UserProfile p = UserProfileDB.GetUserProfileByMembershipUserGUID(memGuid);
        bool isNew = (p == null);



        string fName = this.FirstNameTextBox.Text.Trim();
        string lName = this.LastNameTextBox.Text.Trim();
        string email = this.EmailTextBox.Text.Trim();
        string website = this.WebsiteURLTextBox.Text.Trim();
        string sponsName = this.SponsorNameTextBox.Text.Trim();
        string devName = this.DeveloperNameTextBox.Text.Trim();
        string artName = this.ArtistNameTextBox.Text.Trim();
        string phone = this.PhoneTextBox.Text.Trim();

        byte[] sponsorLogoByteArray = null;
        string sponsorLogoContentType = "";
        string sponsorLogoFileName = "";

        //sponsor logo
        if (this.SponsorLogoFileUpload.PostedFile != null && !String.IsNullOrEmpty(this.SponsorLogoFileUpload.PostedFile.FileName))
        {

            sponsorLogoContentType = this.SponsorLogoFileUpload.PostedFile.ContentType.Trim();
            sponsorLogoFileName = this.SponsorLogoFileUpload.FileName;

            //check valid content type
            if (Website.Common.IsValidLogoImageContentType(sponsorLogoContentType))
            {
                sponsorLogoByteArray = Website.Common.GetByteArrayFromFileUpload(this.SponsorLogoFileUpload);


            }
            else
            {
                //show error message
                this.EditProfileViewErrorMessageLabel.Text = "Invalid Sponsor Logo file type. Please upload a valid image file";
                MultiView1.SetActiveView(EditProfileView);
                return; 

            }


        }
        else
        {
            //set to current
            if (!isNew)
            {
                if (p.SponsorLogo != null && !string.IsNullOrEmpty(p.SponsorLogoContentType))
                {
                    if (this.RemoveSponsorLogoCheckBox.Checked)
                    {
                        sponsorLogoByteArray = null;
                        sponsorLogoContentType = string.Empty;
                        sponsorLogoFileName = string.Empty;

                    }
                    else
                    {
                        //set to current
                        sponsorLogoByteArray = p.SponsorLogo;
                        sponsorLogoContentType = p.SponsorLogoContentType;
                        sponsorLogoFileName = p.SponsorLogoFileName;

                    }
                    
                }


            }


        }

        byte[] developerLogoByteArray = null;
        string developerLogoContentType = null;
        string developerLogoFileName = "";

        //developer logo
        if (this.DeveloperLogoFileUpload.PostedFile != null && !string.IsNullOrEmpty(this.DeveloperLogoFileUpload.PostedFile.FileName))
        {
            
            developerLogoContentType = this.DeveloperLogoFileUpload.PostedFile.ContentType.Trim();
            developerLogoFileName = this.DeveloperLogoFileUpload.FileName;

            //check valid content type
            if (Website.Common.IsValidLogoImageContentType(developerLogoContentType))
            {
                developerLogoByteArray = Website.Common.GetByteArrayFromFileUpload(this.DeveloperLogoFileUpload);

            }
            else
            {
                this.EditProfileViewErrorMessageLabel.Text = "Invalid Developer Logo file type. Please upload a valid image file";
                MultiView1.SetActiveView(EditProfileView);
                return; 

            }

        }
        else
        {
            if (!isNew)
            {
                //set to current
                if (p.DeveloperLogo != null && !string.IsNullOrEmpty(p.DeveloperLogoContentType))
                {
                    if (this.RemoveDeveloperLogoCheckBox.Checked)
                    {
                        //set to null
                        developerLogoByteArray = null;
                        developerLogoContentType = string.Empty;
                        developerLogoFileName = string.Empty;
                    }
                    else
                    {
                        //set to current
                        developerLogoByteArray = p.DeveloperLogo;
                        developerLogoContentType = p.DeveloperLogoContentType;
                        developerLogoFileName = p.DeveloperLogoFileName;

                    }
                    
                }

            }


        }
        
        //update
        if (p != null)
        {

            //update
            p.FirstName = fName;
            p.LastName = lName;
            p.Email = email;
            p.WebsiteURL = website;
            p.SponsorName = sponsName;
            p.DeveloperName = devName;
            p.ArtistName = artName;
            p.Phone = phone;
            p.SponsorLogo = sponsorLogoByteArray;
            p.SponsorLogoContentType = sponsorLogoContentType;
            p.SponsorLogoFileName = sponsorLogoFileName;
            p.DeveloperLogo = developerLogoByteArray;
            p.DeveloperLogoContentType = developerLogoContentType;
            p.DeveloperLogoFileName = developerLogoFileName;

            try
            {
                
                UserProfileDB.UpdateUserProfile(p, Context.User.Identity.Name);
                ConfirmationLabel.Text = "Your profile has been successfully updated.";

            }
            catch
            {
                this.EditProfileViewErrorMessageLabel.Text = "Unable to update your profile. Please try again or <a href = Contact.aspx class='Hyperlink'>Contact Us</a> if you have any questions.";
                MultiView1.SetActiveView(EditProfileView);
                return; 

            }


        }
        else
        {
            //insert
            try
            {
                p = UserProfileDB.InsertUserProfile(memGuid, fName, lName, email, Context.User.Identity.Name, Context.User.Identity.Name, website, sponsName, devName, artName, phone, sponsorLogoByteArray, sponsorLogoContentType, sponsorLogoFileName, developerLogoByteArray, developerLogoContentType, developerLogoFileName);
                ConfirmationLabel.Text = "Your profile has been successfully created.";
            
            }
            catch 
            {
                this.EditProfileViewErrorMessageLabel.Text = "Unable to create your profile. Please try again or <a href = Contact.aspx class='Hyperlink'>Contact Us</a> if you have any questions.";
                MultiView1.SetActiveView(EditProfileView);
                return; 
               
            }      

            

        }
                
        MultiView1.SetActiveView(ConfirmationView);
       
        
      
    }
    protected void ContinueButton_Click(object sender, EventArgs e)
    {
        //send back to edit profile view
        MultiView1.SetActiveView(EditProfileView);
    }
    protected void ConfirmationButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }


    private void BindProfile()
    {

      
        UserProfile _userProfile = null;

        try
        {
            _userProfile = UserProfileDB.GetUserProfileByMembershipUserGUID(Membership.GetUser().ProviderUserKey.ToString());
        }
        catch
        {


        }


        //bind textboxes

        if (_userProfile != null)
        {

            //FirstName
            if (!string.IsNullOrEmpty(_userProfile.FirstName))
            {
                this.FirstNameTextBox.Text = _userProfile.FirstName;

            }

            //Lastname
            if (!string.IsNullOrEmpty(_userProfile.LastName))
            {
                this.LastNameTextBox.Text = _userProfile.LastName;
            }

            //email
            if (!string.IsNullOrEmpty(_userProfile.Email))
            {
                this.EmailTextBox.Text = _userProfile.Email;
            }

            //websiteURL
            if (!string.IsNullOrEmpty(_userProfile.WebsiteURL))
            {
                this.WebsiteURLTextBox.Text = _userProfile.WebsiteURL;
            }

            //sponsor name
            if (!string.IsNullOrEmpty(_userProfile.SponsorName))
            {
                this.SponsorNameTextBox.Text = _userProfile.SponsorName;
            }

            //Sponsor logo
            if (_userProfile.SponsorLogo != null && !string.IsNullOrEmpty(_userProfile.SponsorLogoContentType.Trim()))
            {
              string sponsorImageUrl =  Website.Pages.Types.FormatProfileImageHandler(_userProfile.UserID.ToString(),"Sponsor");

              if (!string.IsNullOrEmpty(sponsorImageUrl))
              {
                  this.SponsorImageThumbnail.ImageUrl = sponsorImageUrl;
                  this.SponsorImageThumbnail.Visible = true;
                  this.RemoveSponsorLogoCheckBox.Visible = true;

              }
             
            }
                        

            //developer name
            if (!string.IsNullOrEmpty(_userProfile.DeveloperName))
            {
                this.DeveloperNameTextBox.Text = _userProfile.DeveloperName;
            }

            //Developer logo
            if (_userProfile.DeveloperLogo != null && !string.IsNullOrEmpty(_userProfile.DeveloperLogoContentType.Trim()))
            {
                string developerLogoURL = Website.Pages.Types.FormatProfileImageHandler(_userProfile.UserID.ToString(), "Developer");

                if (!string.IsNullOrEmpty(developerLogoURL))
                {
                    this.DeveloperLogoImageThumbnail.ImageUrl = developerLogoURL;
                    this.DeveloperLogoImageThumbnail.Visible = true;
                    this.RemoveDeveloperLogoCheckBox.Visible = true;

                }
               

            }


            //artist name
            if (!string.IsNullOrEmpty(_userProfile.ArtistName))
            {
                this.ArtistNameTextBox.Text = _userProfile.ArtistName;
            }

            //phone
            if (!string.IsNullOrEmpty(_userProfile.Phone))
            {
                this.PhoneTextBox.Text = _userProfile.Phone;

            }



        }

    }

}
