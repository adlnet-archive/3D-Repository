namespace vwar.uploader
{
    partial class MetadataDetails
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
            this.components = new System.ComponentModel.Container();
            this.btnModel = new System.Windows.Forms.Button();
            this.txtModel = new System.Windows.Forms.TextBox();
            this.btnBrowseScreenshot = new System.Windows.Forms.Button();
            this.txtScreenshot = new System.Windows.Forms.TextBox();
            this.picScreenshot = new System.Windows.Forms.PictureBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnUpload = new System.Windows.Forms.Button();
            this.cmbLicense = new System.Windows.Forms.ComboBox();
            this.lblLicense = new System.Windows.Forms.Label();
            this.epErrors = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picScreenshot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.epErrors)).BeginInit();
            this.SuspendLayout();
            // 
            // btnModel
            // 
            this.btnModel.Location = new System.Drawing.Point(0, 53);
            this.btnModel.Name = "btnModel";
            this.btnModel.Size = new System.Drawing.Size(75, 23);
            this.btnModel.TabIndex = 22;
            this.btnModel.Text = "Model";
            this.btnModel.UseVisualStyleBackColor = true;
            this.btnModel.Click += new System.EventHandler(this.btnModel_Click);
            // 
            // txtModel
            // 
            this.txtModel.Location = new System.Drawing.Point(81, 55);
            this.txtModel.Name = "txtModel";
            this.txtModel.Size = new System.Drawing.Size(278, 20);
            this.txtModel.TabIndex = 21;
            this.txtModel.Validating += new System.ComponentModel.CancelEventHandler(this.txt_Validating);
            // 
            // btnBrowseScreenshot
            // 
            this.btnBrowseScreenshot.Location = new System.Drawing.Point(0, 79);
            this.btnBrowseScreenshot.Name = "btnBrowseScreenshot";
            this.btnBrowseScreenshot.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseScreenshot.TabIndex = 20;
            this.btnBrowseScreenshot.Text = "Screenshot";
            this.btnBrowseScreenshot.UseVisualStyleBackColor = true;
            this.btnBrowseScreenshot.Click += new System.EventHandler(this.btnBrowseScreenshot_Click);
            // 
            // txtScreenshot
            // 
            this.txtScreenshot.Location = new System.Drawing.Point(81, 81);
            this.txtScreenshot.Name = "txtScreenshot";
            this.txtScreenshot.Size = new System.Drawing.Size(278, 20);
            this.txtScreenshot.TabIndex = 19;
            this.txtScreenshot.Validating += new System.ComponentModel.CancelEventHandler(this.txt_Validating);
            // 
            // picScreenshot
            // 
            this.picScreenshot.Location = new System.Drawing.Point(3, 135);
            this.picScreenshot.Name = "picScreenshot";
            this.picScreenshot.Size = new System.Drawing.Size(367, 158);
            this.picScreenshot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picScreenshot.TabIndex = 18;
            this.picScreenshot.TabStop = false;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(81, 29);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(278, 20);
            this.txtDescription.TabIndex = 17;
            this.txtDescription.Validating += new System.ComponentModel.CancelEventHandler(this.txt_Validating);
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(15, 32);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(60, 13);
            this.lblDescription.TabIndex = 16;
            this.lblDescription.Text = "Description";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(81, 3);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(278, 20);
            this.txtTitle.TabIndex = 15;
            this.txtTitle.Validating += new System.ComponentModel.CancelEventHandler(this.txt_Validating);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(48, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(27, 13);
            this.lblTitle.TabIndex = 14;
            this.lblTitle.Text = "Title";
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(3, 299);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(367, 23);
            this.btnUpload.TabIndex = 13;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // cmbLicense
            // 
            this.cmbLicense.FormattingEnabled = true;
            this.cmbLicense.Location = new System.Drawing.Point(81, 108);
            this.cmbLicense.Name = "cmbLicense";
            this.cmbLicense.Size = new System.Drawing.Size(278, 21);
            this.cmbLicense.TabIndex = 23;
            // 
            // lblLicense
            // 
            this.lblLicense.AutoSize = true;
            this.lblLicense.Location = new System.Drawing.Point(15, 111);
            this.lblLicense.Name = "lblLicense";
            this.lblLicense.Size = new System.Drawing.Size(44, 13);
            this.lblLicense.TabIndex = 24;
            this.lblLicense.Text = "License";
            // 
            // epErrors
            // 
            this.epErrors.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.epErrors.ContainerControl = this;
            // 
            // MetadataDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblLicense);
            this.Controls.Add(this.cmbLicense);
            this.Controls.Add(this.btnModel);
            this.Controls.Add(this.txtModel);
            this.Controls.Add(this.btnBrowseScreenshot);
            this.Controls.Add(this.txtScreenshot);
            this.Controls.Add(this.picScreenshot);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnUpload);
            this.Name = "MetadataDetails";
            this.Size = new System.Drawing.Size(380, 335);
            ((System.ComponentModel.ISupportInitialize)(this.picScreenshot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.epErrors)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnModel;
        private System.Windows.Forms.TextBox txtModel;
        private System.Windows.Forms.Button btnBrowseScreenshot;
        private System.Windows.Forms.TextBox txtScreenshot;
        private System.Windows.Forms.PictureBox picScreenshot;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.ComboBox cmbLicense;
        private System.Windows.Forms.Label lblLicense;
        private System.Windows.Forms.ErrorProvider epErrors;
    }
}
