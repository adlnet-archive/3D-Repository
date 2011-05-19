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
        public delegate void CompleteHandler(object sender, EventArgs args);
        public event CompleteHandler Complete;
        class LicenseData
        {
            public String Name { get; set; }
            public String Key { get; set; }
            public override string ToString()
            {
                return Name;
            }
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
            cmbLicense.SelectedIndex = 0;

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

        private void btnUpload_Click(object sender, EventArgs e)
        {
            _3DRAPI_Imp api = new _3DRAPI_Imp(true);
            LicenseData license = cmbLicense.SelectedItem as LicenseData;
            Metadata md = new Metadata
            {
                Title = txtTitle.Text,
                Description = txtDescription.Text,
                License = String.Format("http://creativecommons.org/licenses/{0}/3.0/legalcode", license.Key)
                
            };
            var pid = api.InsertMetadata(md);
            using (FileStream modelStream = new FileStream(txtModel.Text, FileMode.Open))
            {
                byte[] data = new byte[modelStream.Length];
                modelStream.Read(data, 0, data.Length);
                api.UploadFile(data, pid);
            }
            using (FileStream modelStream = new FileStream(txtScreenshot.Text, FileMode.Open))
            {
                byte[] data = new byte[modelStream.Length];
                modelStream.Read(data, 0, data.Length);
                api.UploadScreenShot(data, pid, Path.GetFileName(txtScreenshot.Text));
            }
            if (Complete != null)
            {
                Complete(this, new EventArgs());
            }
        }
    }
}
