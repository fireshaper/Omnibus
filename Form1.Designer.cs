namespace Omnibus
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lbComics = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbComicSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnOpenLink = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.tbDesc = new System.Windows.Forms.RichTextBox();
            this.tbStatus = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lbDownloads = new System.Windows.Forms.ListBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pbCover = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbCover)).BeginInit();
            this.SuspendLayout();
            // 
            // lbComics
            // 
            this.lbComics.FormattingEnabled = true;
            this.lbComics.ItemHeight = 16;
            this.lbComics.Location = new System.Drawing.Point(12, 139);
            this.lbComics.Name = "lbComics";
            this.lbComics.Size = new System.Drawing.Size(534, 228);
            this.lbComics.TabIndex = 2;
            this.lbComics.SelectedIndexChanged += new System.EventHandler(this.lbComics_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(335, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Search for a comic (leave blank for recently added):";
            // 
            // tbComicSearch
            // 
            this.tbComicSearch.Location = new System.Drawing.Point(12, 42);
            this.tbComicSearch.Name = "tbComicSearch";
            this.tbComicSearch.Size = new System.Drawing.Size(286, 22);
            this.tbComicSearch.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(304, 38);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 31);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnOpenLink
            // 
            this.btnOpenLink.Location = new System.Drawing.Point(12, 105);
            this.btnOpenLink.Name = "btnOpenLink";
            this.btnOpenLink.Size = new System.Drawing.Size(93, 28);
            this.btnOpenLink.TabIndex = 4;
            this.btnOpenLink.Text = "Open Link";
            this.btnOpenLink.UseVisualStyleBackColor = true;
            this.btnOpenLink.Click += new System.EventHandler(this.btnOpenLink_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(111, 105);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(135, 28);
            this.btnDownload.TabIndex = 5;
            this.btnDownload.Text = "Download Comic";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // tbDesc
            // 
            this.tbDesc.Location = new System.Drawing.Point(12, 373);
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.ReadOnly = true;
            this.tbDesc.Size = new System.Drawing.Size(534, 126);
            this.tbDesc.TabIndex = 6;
            this.tbDesc.Text = "";
            // 
            // tbStatus
            // 
            this.tbStatus.Location = new System.Drawing.Point(12, 659);
            this.tbStatus.Name = "tbStatus";
            this.tbStatus.ReadOnly = true;
            this.tbStatus.Size = new System.Drawing.Size(535, 22);
            this.tbStatus.TabIndex = 7;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(553, 659);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(285, 22);
            this.progressBar1.TabIndex = 8;
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(252, 105);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(136, 28);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel Download";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lbDownloads
            // 
            this.lbDownloads.FormattingEnabled = true;
            this.lbDownloads.ItemHeight = 16;
            this.lbDownloads.Location = new System.Drawing.Point(12, 505);
            this.lbDownloads.Name = "lbDownloads";
            this.lbDownloads.Size = new System.Drawing.Size(826, 148);
            this.lbDownloads.TabIndex = 10;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(385, 38);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(78, 31);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // button2
            // 
            this.button2.Image = global::Omnibus.Properties.Resources.downloads_32px;
            this.button2.Location = new System.Drawing.Point(786, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(52, 52);
            this.button2.TabIndex = 13;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Image = global::Omnibus.Properties.Resources.settings_32px;
            this.button1.Location = new System.Drawing.Point(728, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(52, 52);
            this.button1.TabIndex = 12;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pbCover
            // 
            this.pbCover.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbCover.ErrorImage = global::Omnibus.Properties.Resources.omnibus_preview_image;
            this.pbCover.Image = global::Omnibus.Properties.Resources.omnibus_preview_image;
            this.pbCover.InitialImage = global::Omnibus.Properties.Resources.omnibus_preview_image;
            this.pbCover.Location = new System.Drawing.Point(553, 139);
            this.pbCover.Name = "pbCover";
            this.pbCover.Size = new System.Drawing.Size(285, 360);
            this.pbCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbCover.TabIndex = 3;
            this.pbCover.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 695);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lbDownloads);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.tbStatus);
            this.Controls.Add(this.tbDesc);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.btnOpenLink);
            this.Controls.Add(this.pbCover);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.tbComicSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbComics);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Omnibus";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbCover)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbComics;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbComicSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.PictureBox pbCover;
        private System.Windows.Forms.Button btnOpenLink;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.RichTextBox tbDesc;
        private System.Windows.Forms.TextBox tbStatus;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListBox lbDownloads;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

