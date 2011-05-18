using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using vwar.service.implementation;
using System.IO;

namespace vwar.uploader
{
    public partial class MetadataDetails : UserControl
    {
        public delegate void CompleteHandler(object sender, EventArgs args);
        public event CompleteHandler Complete;
        public MetadataDetails()
        {
            InitializeComponent();
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
            Metadata md = new Metadata
            {
                Title = txtTitle.Text,
                Description = txtDescription.Text
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
