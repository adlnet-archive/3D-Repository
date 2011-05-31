using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace vwar.uploader
{
    public partial class UploadTypeDialog : Form
    {
        public enum UploadType
        {
            Single,
            Many            
        }
        public UploadType UploadState { get; set; }
        public UploadTypeDialog()
        {
            InitializeComponent();
        }

        private void btnUploadOne_Click(object sender, EventArgs e)
        {
            UploadState = UploadType.Single;
            this.Close();
        }

        private void btnUploadMany_Click(object sender, EventArgs e)
        {
            UploadState = UploadType.Many;
            this.Close();
        }
    }
}
