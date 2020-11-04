namespace Omnibus
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.tbDLocation = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnBrowseLog = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbLLocation = new System.Windows.Forms.TextBox();
            this.cbLogs = new System.Windows.Forms.CheckBox();
            this.lblWebAgent = new System.Windows.Forms.Label();
            this.lblCfduid = new System.Windows.Forms.Label();
            this.tbUserAgent = new System.Windows.Forms.TextBox();
            this.tbCfduid = new System.Windows.Forms.TextBox();
            this.tbCfClearance = new System.Windows.Forms.TextBox();
            this.lblCf_Clearance = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbDLocation
            // 
            this.tbDLocation.Location = new System.Drawing.Point(31, 35);
            this.tbDLocation.Margin = new System.Windows.Forms.Padding(2);
            this.tbDLocation.Name = "tbDLocation";
            this.tbDLocation.Size = new System.Drawing.Size(417, 20);
            this.tbDLocation.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Download Location:";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(443, 423);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(56, 19);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(504, 423);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(56, 19);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(452, 34);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(2);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(56, 19);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnBrowseLog
            // 
            this.btnBrowseLog.Location = new System.Drawing.Point(452, 146);
            this.btnBrowseLog.Margin = new System.Windows.Forms.Padding(2);
            this.btnBrowseLog.Name = "btnBrowseLog";
            this.btnBrowseLog.Size = new System.Drawing.Size(56, 19);
            this.btnBrowseLog.TabIndex = 7;
            this.btnBrowseLog.Text = "Browse";
            this.btnBrowseLog.UseVisualStyleBackColor = true;
            this.btnBrowseLog.Click += new System.EventHandler(this.btnBrowseLog_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 131);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Log Location:";
            // 
            // tbLLocation
            // 
            this.tbLLocation.Location = new System.Drawing.Point(31, 147);
            this.tbLLocation.Margin = new System.Windows.Forms.Padding(2);
            this.tbLLocation.Name = "tbLLocation";
            this.tbLLocation.Size = new System.Drawing.Size(417, 20);
            this.tbLLocation.TabIndex = 5;
            // 
            // cbLogs
            // 
            this.cbLogs.AutoSize = true;
            this.cbLogs.Location = new System.Drawing.Point(31, 101);
            this.cbLogs.Name = "cbLogs";
            this.cbLogs.Size = new System.Drawing.Size(85, 17);
            this.cbLogs.TabIndex = 8;
            this.cbLogs.Text = "Enable Logs";
            this.cbLogs.UseVisualStyleBackColor = true;
            this.cbLogs.CheckedChanged += new System.EventHandler(this.cbLogs_CheckedChanged);
            // 
            // lblWebAgent
            // 
            this.lblWebAgent.AutoSize = true;
            this.lblWebAgent.Location = new System.Drawing.Point(28, 210);
            this.lblWebAgent.Name = "lblWebAgent";
            this.lblWebAgent.Size = new System.Drawing.Size(57, 13);
            this.lblWebAgent.TabIndex = 9;
            this.lblWebAgent.Text = "UserAgent";
            // 
            // lblCfduid
            // 
            this.lblCfduid.AutoSize = true;
            this.lblCfduid.Location = new System.Drawing.Point(27, 270);
            this.lblCfduid.Name = "lblCfduid";
            this.lblCfduid.Size = new System.Drawing.Size(48, 13);
            this.lblCfduid.TabIndex = 10;
            this.lblCfduid.Text = "__cfduid";
            // 
            // tbUserAgent
            // 
            this.tbUserAgent.Location = new System.Drawing.Point(31, 226);
            this.tbUserAgent.Name = "tbUserAgent";
            this.tbUserAgent.Size = new System.Drawing.Size(417, 20);
            this.tbUserAgent.TabIndex = 12;
            // 
            // tbCfduid
            // 
            this.tbCfduid.Location = new System.Drawing.Point(30, 286);
            this.tbCfduid.Name = "tbCfduid";
            this.tbCfduid.Size = new System.Drawing.Size(418, 20);
            this.tbCfduid.TabIndex = 13;
            // 
            // tbCfClearance
            // 
            this.tbCfClearance.Location = new System.Drawing.Point(30, 339);
            this.tbCfClearance.Name = "tbCfClearance";
            this.tbCfClearance.Size = new System.Drawing.Size(418, 20);
            this.tbCfClearance.TabIndex = 15;
            // 
            // lblCf_Clearance
            // 
            this.lblCf_Clearance.AutoSize = true;
            this.lblCf_Clearance.Location = new System.Drawing.Point(28, 323);
            this.lblCf_Clearance.Name = "lblCf_Clearance";
            this.lblCf_Clearance.Size = new System.Drawing.Size(69, 13);
            this.lblCf_Clearance.TabIndex = 14;
            this.lblCf_Clearance.Text = "cf_clearance";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 375);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(419, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "NOTE: If one, or more, of these cookies are not showing in your browser, leave it" +
    " blank.";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 453);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbCfClearance);
            this.Controls.Add(this.lblCf_Clearance);
            this.Controls.Add(this.tbCfduid);
            this.Controls.Add(this.tbUserAgent);
            this.Controls.Add(this.lblCfduid);
            this.Controls.Add(this.lblWebAgent);
            this.Controls.Add(this.cbLogs);
            this.Controls.Add(this.btnBrowseLog);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbLLocation);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbDLocation);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Settings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbDLocation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnBrowseLog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbLLocation;
        private System.Windows.Forms.CheckBox cbLogs;
        private System.Windows.Forms.Label lblWebAgent;
        private System.Windows.Forms.Label lblCfduid;
        private System.Windows.Forms.TextBox tbUserAgent;
        private System.Windows.Forms.TextBox tbCfduid;
        private System.Windows.Forms.TextBox tbCfClearance;
        private System.Windows.Forms.Label lblCf_Clearance;
        private System.Windows.Forms.Label label3;
    }
}