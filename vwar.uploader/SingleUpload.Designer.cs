namespace vwar.uploader
{
    partial class SingleUpload
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnUpload = new System.Windows.Forms.Button();
            this.metadataDetails = new vwar.uploader.MetadataDetails();
            this.SuspendLayout();
            // 
            // btnUpload
            // 
            this.btnUpload.CausesValidation = false;
            this.btnUpload.Location = new System.Drawing.Point(3, 317);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(394, 23);
            this.btnUpload.TabIndex = 1;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // metadataDetails
            // 
            this.metadataDetails.CausesValidation = false;
            this.metadataDetails.Location = new System.Drawing.Point(10, 10);
            this.metadataDetails.Name = "metadataDetails";
            this.metadataDetails.Size = new System.Drawing.Size(380, 300);
            this.metadataDetails.TabIndex = 0;
            // 
            // SingleUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CausesValidation = false;
            this.Controls.Add(this.metadataDetails);
            this.Controls.Add(this.btnUpload);
            this.Name = "SingleUpload";
            this.Size = new System.Drawing.Size(400, 340);
            this.ResumeLayout(false);

        }

        #endregion

        private MetadataDetails metadataDetails;
        private System.Windows.Forms.Button btnUpload;


    }
}
