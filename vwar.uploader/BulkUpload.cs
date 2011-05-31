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
    public partial class BulkUpload : UserControl
    {
        public BulkUpload()
        {
            InitializeComponent();
            metadataDetails.PropertyChanged += new PropertyChangedEventHandler(metadataDetails_PropertyChanged);
        }

        void metadataDetails_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (runSelectedValue)
            {
                UpdateSelectedMetada();
            }
        }
        public event vwar.uploader.SingleUpload.CompleteHandler Complete;
        private void btnCheckAll_Click(object sender, EventArgs e)
        {

            SetAllSelectedState(true);
        }

        private void SetAllSelectedState(bool state)
        {
            for (int i = 0; i < lstFiles.Items.Count; i++)
            {
                lstFiles.SetItemChecked(i, state);
            }
        }
        public string DefaultLicense
        {
            set
            {
                metadataDetails.DefaultLicense = value;
            }
        }
        private void btnUncheckAll_Click(object sender, EventArgs e)
        {
            SetAllSelectedState(false);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog diag = new FolderBrowserDialog())
            {
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    DirectoryInfo info = new DirectoryInfo(diag.SelectedPath);
                    var files = info.GetFiles("*.zip",SearchOption.AllDirectories);
                    foreach (var file in files)
                    {

                        lstFiles.Items.Add(new TempMetadata
                        {
                            Title = file.Name,
                            Description = file.Name,
                            ModelLocation = file.FullName
                        });
                    }
                }
            }            
        }
        private bool runSelectedValue = true;
        private int previouslySelected = -1;
        private void lstFiles_SelectedValueChanged(object sender, EventArgs e)
        {
            if (runSelectedValue)
            {
                UpdateSelectedMetada();
                metadataDetails.Metadata = lstFiles.SelectedItem as TempMetadata;
                previouslySelected = lstFiles.SelectedIndex;
            }
        }

        private void UpdateSelectedMetada()
        {
            if (previouslySelected >= 0)
            {
                runSelectedValue = false;
                lstFiles.Items[previouslySelected] = metadataDetails.Metadata;
                runSelectedValue = true;
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            foreach(TempMetadata tmp in lstFiles.CheckedItems)
            {

                if (tmp != null && File.Exists(tmp.ModelLocation))
                {
                    _3DRAPI_Imp api = new _3DRAPI_Imp(true);
                    Metadata md = new Metadata
                    {
                        License = tmp.License,
                        Title = tmp.Title,
                        Description = tmp.Description,
                        AssetType = "Model"
                    };
                    var pid = api.InsertMetadata(md);
                    using (FileStream modelStream = new FileStream(tmp.ModelLocation, FileMode.Open))
                    {
                        byte[] data = new byte[modelStream.Length];
                        modelStream.Read(data, 0, data.Length);
                        api.UploadFile(data, pid);
                    }
                    if (File.Exists(tmp.ScreenshotLocation))
                    {
                        using (FileStream modelStream = new FileStream(tmp.ScreenshotLocation, FileMode.Open))
                        {
                            byte[] data = new byte[modelStream.Length];
                            modelStream.Read(data, 0, data.Length);
                            api.UploadScreenShot(data, pid, Path.GetFileName(tmp.ScreenshotLocation));
                        }
                    }

                }
            }
            if (Complete != null)
            {
                Complete(this, new EventArgs());
            }
        }
    }
}
