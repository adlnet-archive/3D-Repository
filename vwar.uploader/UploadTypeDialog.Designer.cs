namespace vwar.uploader
{
    partial class UploadTypeDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnUploadOne = new System.Windows.Forms.Button();
            this.btnUploadMany = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnUploadOne
            // 
            this.btnUploadOne.Location = new System.Drawing.Point(13, 13);
            this.btnUploadOne.Name = "btnUploadOne";
            this.btnUploadOne.Size = new System.Drawing.Size(141, 23);
            this.btnUploadOne.TabIndex = 0;
            this.btnUploadOne.Text = "Upload One Package";
            this.btnUploadOne.UseVisualStyleBackColor = true;
            this.btnUploadOne.Click += new System.EventHandler(this.btnUploadOne_Click);
            // 
            // btnUploadMany
            // 
            this.btnUploadMany.Location = new System.Drawing.Point(160, 13);
            this.btnUploadMany.Name = "btnUploadMany";
            this.btnUploadMany.Size = new System.Drawing.Size(141, 23);
            this.btnUploadMany.TabIndex = 1;
            this.btnUploadMany.Text = "Upload Multiple Packages";
            this.btnUploadMany.UseVisualStyleBackColor = true;
            this.btnUploadMany.Click += new System.EventHandler(this.btnUploadMany_Click);
            // 
            // UploadTypeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 56);
            this.Controls.Add(this.btnUploadMany);
            this.Controls.Add(this.btnUploadOne);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "UploadTypeDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Upload One Or Multiple";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnUploadOne;
        private System.Windows.Forms.Button btnUploadMany;
    }
}