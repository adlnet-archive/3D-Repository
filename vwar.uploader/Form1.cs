using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
namespace vwar.uploader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            using(UploadTypeDialog d = new UploadTypeDialog())
            {
                d.ShowDialog();
                InitializeComponent();
                switch (d.UploadState)
                {
                    case UploadTypeDialog.UploadType.Single: 
                        SingleUpload uploader = new SingleUpload();
                        uploader.DefaultLicense = ConfigurationManager.AppSettings["defaultLicense"];
                        uploader.Complete += new SingleUpload.CompleteHandler(uploader_Complete);
                        uploader.Location = new Point(12, 0);
                        Controls.Add(uploader);
                        break;
                    case UploadTypeDialog.UploadType.Many:
                        BulkUpload bu = new BulkUpload();
                        bu.DefaultLicense = ConfigurationManager.AppSettings["defaultLicense"];
                        bu.Location = new Point(12, 0);
                        this.Width = 624;
                        bu.Complete += new SingleUpload.CompleteHandler(uploader_Complete);
                        Controls.Add(bu);
                        break;
                }
            }
            
        }

        void uploader_Complete(object sender, EventArgs args)
        {
            MessageBox.Show("Completed");
        }
    }
}
