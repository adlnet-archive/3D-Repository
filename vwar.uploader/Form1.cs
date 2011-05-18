using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace vwar.uploader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            var mainControl = new MetadataDetails
            {
                Location = new Point(10, 0)
            };
            mainControl.Complete += new MetadataDetails.CompleteHandler(mainControl_Complete);
            this.Controls.Add(mainControl);
            InitializeComponent();

        }

        void mainControl_Complete(object sender, EventArgs args)
        {
            MessageBox.Show("Completed");
        }

    }
}
