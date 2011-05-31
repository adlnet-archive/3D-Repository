using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using vwar.service.host;
using System.IO;

namespace vwar.uploader
{
    public partial class SingleUpload : UserControl
    {
        public delegate void CompleteHandler(object sender, EventArgs args);
        public event CompleteHandler Complete;
        public SingleUpload()
        {
            InitializeComponent();
        }
        public string DefaultLicense
        {
            set
            {
                metadataDetails.DefaultLicense = value;
            }
        }
        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                _3DRAPI_Imp api = new _3DRAPI_Imp(true);
                
                Metadata md = new Metadata
                {
                    Title = metadataDetails.Metadata.Title,
                    Description = metadataDetails.Metadata.Description,
                    License = metadataDetails.LicenseUrl,
                    AssetType = "Model"

                };
                var pid = api.InsertMetadata(md);
                using (FileStream modelStream = new FileStream(metadataDetails.Metadata.ModelLocation, FileMode.Open))
                {
                    byte[] data = new byte[modelStream.Length];
                    modelStream.Read(data, 0, data.Length);
                    api.UploadFile(data, pid);
                }
                using (FileStream modelStream = new FileStream(metadataDetails.Metadata.ScreenshotLocation, FileMode.Open))
                {
                    byte[] data = new byte[modelStream.Length];
                    modelStream.Read(data, 0, data.Length);
                    api.UploadScreenShot(data, pid, Path.GetFileName(metadataDetails.Metadata.ScreenshotLocation));
                }
                if (Complete != null)
                {
                    Complete(this, new EventArgs());
                }
            }
        }
    }
}
