using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using vwar.service.host;

namespace vwar.uploader
{
    public partial class MetadataDetails : UserControl
    {
        class LicenseData
        {
            public String Name { get; set; }
            public String Key { get; set; }
            public override string ToString()
            {
                return Name;
            }
        }
        public string DefaultLicense
        {
            set
            {
                foreach (var item in cmbLicense.Items)
                {
                    if ((item as LicenseData).Name == value)
                    {
                        cmbLicense.SelectedItem = item;
                        break;
                    }
                }
            }
        }
        public TempMetadata Metadata
        {
            get
            {
                return new TempMetadata
                {
                    Title = txtTitle.Text,
                    Description = txtDescription.Text,
                    ModelLocation = txtModel.Text,
                    ScreenshotLocation = txtScreenshot.Text,
                    License = LicenseUrl
                };
            }
            set
            {
                if (!File.Exists(value.ModelLocation))
                    throw new ArgumentException("Model must be a valid path of the filesystem");
                if (!String.IsNullOrEmpty(value.ScreenshotLocation) && !File.Exists(value.ScreenshotLocation))
                    throw new ArgumentException("Screenshot must be a valid path of the filesystem");
                txtTitle.Text = value.Title;
                txtDescription.Text = value.Description;
                txtModel.Text = value.ModelLocation;
                txtScreenshot.Text = value.ScreenshotLocation;
                picScreenshot.ImageLocation = value.ScreenshotLocation;
                LicenseUrl = value.License;
            }
        }
        public string LicenseUrl
        {
            private set
            {
                foreach (var item in cmbLicense.Items)
                {
                    if (value == FormatLicenseUrl((item as LicenseData).Key))
                    {
                        cmbLicense.SelectedItem = item;
                        break;
                    }
                }
            }
            get
            {
                LicenseData license = cmbLicense.SelectedItem as LicenseData;
                return FormatLicenseUrl(license.Key);
            }
        }
        private string FormatLicenseUrl(string key)
        {

            return String.Format("http://creativecommons.org/licenses/{0}/3.0/legalcode", key);
        }
        public MetadataDetails()
        {
            InitializeComponent();
            cmbLicense.Items.Add(new LicenseData
            {
                Name = "Public Domain",
                Key = "publicdomain"
            });
            cmbLicense.Items.Add(new LicenseData
            {
                Name = "Attribution",
                Key = "by"
            });
            cmbLicense.Items.Add(new LicenseData
            {
                Name = "Attribution-ShareAlike",
                Key = "by-sa"
            });
            cmbLicense.Items.Add(new LicenseData
            {
                Name = "Attribution-NoDerivatives",
                Key = "by-nd"
            });
            cmbLicense.Items.Add(new LicenseData
            {
                Name = "Attribution-NonCommercial",
                Key = "by-nc"
            });
            cmbLicense.Items.Add(new LicenseData
            {
                Name = "Attribution-NonCommercial-ShareAlike",
                Key = "by-nc-sa"
            });
            cmbLicense.Items.Add(new LicenseData
            {
                Name = "Attribution-NonCommercial-NoDerivatives",
                Key = "by-nc-nd"
            });


        }
        private void btnModel_Click(object sender, EventArgs e)
        {
            FileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Zip Files (*.zip)|*.zip";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtModel.Text = dialog.FileName;
            }
        }

        private void btnBrowseScreenshot_Click(object sender, EventArgs e)
        {
            FileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtScreenshot.Text = dialog.FileName;
                picScreenshot.ImageLocation = txtScreenshot.Text;
            }
        }


        private void txt_Validating(object sender, CancelEventArgs e)
        {
            string error = null;
            TextBox control = sender as TextBox;
            if (control != null)
            {
                if (String.IsNullOrEmpty(control.Text))
                {
                    e.Cancel = true;
                    error = "Required";
                }
                epErrors.SetError(control, error);
            }
        }
    }
}
