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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lbComics = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbComicSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnOpenLink = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.tbDesc = new System.Windows.Forms.RichTextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pbCover = new System.Windows.Forms.PictureBox();
            this.lvDownloads = new System.Windows.Forms.ListView();
            this.Title = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Progress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Cancel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.clearDownloads = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLastPage = new System.Windows.Forms.Button();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.cmsComics = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.search = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pbCover)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.cmsComics.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbComics
            // 
            this.lbComics.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbComics.FormattingEnabled = true;
            this.lbComics.Location = new System.Drawing.Point(9, 113);
            this.lbComics.Margin = new System.Windows.Forms.Padding(2);
            this.lbComics.Name = "lbComics";
            this.lbComics.Size = new System.Drawing.Size(402, 186);
            this.lbComics.TabIndex = 2;
            this.lbComics.SelectedIndexChanged += new System.EventHandler(this.lbComics_SelectedIndexChanged);
            this.lbComics.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbComics_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(251, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Search for a comic (leave blank for recently added):";
            // 
            // tbComicSearch
            // 
            this.tbComicSearch.Location = new System.Drawing.Point(9, 34);
            this.tbComicSearch.Margin = new System.Windows.Forms.Padding(2);
            this.tbComicSearch.Name = "tbComicSearch";
            this.tbComicSearch.Size = new System.Drawing.Size(216, 20);
            this.tbComicSearch.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(228, 31);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(56, 25);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnOpenLink
            // 
            this.btnOpenLink.Location = new System.Drawing.Point(9, 85);
            this.btnOpenLink.Margin = new System.Windows.Forms.Padding(2);
            this.btnOpenLink.Name = "btnOpenLink";
            this.btnOpenLink.Size = new System.Drawing.Size(70, 23);
            this.btnOpenLink.TabIndex = 4;
            this.btnOpenLink.Text = "Open Link";
            this.btnOpenLink.UseVisualStyleBackColor = true;
            this.btnOpenLink.Click += new System.EventHandler(this.btnOpenLink_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(83, 85);
            this.btnDownload.Margin = new System.Windows.Forms.Padding(2);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(101, 23);
            this.btnDownload.TabIndex = 5;
            this.btnDownload.Text = "Download Comic";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // tbDesc
            // 
            this.tbDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDesc.Location = new System.Drawing.Point(9, 303);
            this.tbDesc.Margin = new System.Windows.Forms.Padding(2);
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.ReadOnly = true;
            this.tbDesc.Size = new System.Drawing.Size(402, 103);
            this.tbDesc.TabIndex = 6;
            this.tbDesc.Text = "";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(289, 31);
            this.btnClear.Margin = new System.Windows.Forms.Padding(2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(58, 25);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // button2
            // 
            this.button2.Image = global::Omnibus.Properties.Resources.downloads_32px;
            this.button2.Location = new System.Drawing.Point(590, 10);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(39, 42);
            this.button2.TabIndex = 13;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Image = global::Omnibus.Properties.Resources.settings_32px;
            this.button1.Location = new System.Drawing.Point(546, 10);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(39, 42);
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
            this.pbCover.Location = new System.Drawing.Point(415, 113);
            this.pbCover.Margin = new System.Windows.Forms.Padding(2);
            this.pbCover.Name = "pbCover";
            this.pbCover.Size = new System.Drawing.Size(214, 292);
            this.pbCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbCover.TabIndex = 3;
            this.pbCover.TabStop = false;
            // 
            // lvDownloads
            // 
            this.lvDownloads.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvDownloads.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Title,
            this.Status,
            this.Progress});
            this.lvDownloads.Location = new System.Drawing.Point(9, 410);
            this.lvDownloads.Margin = new System.Windows.Forms.Padding(2);
            this.lvDownloads.Name = "lvDownloads";
            this.lvDownloads.Scrollable = false;
            this.lvDownloads.Size = new System.Drawing.Size(620, 165);
            this.lvDownloads.TabIndex = 14;
            this.lvDownloads.UseCompatibleStateImageBehavior = false;
            this.lvDownloads.View = System.Windows.Forms.View.Details;
            this.lvDownloads.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvDownloads_MouseClick);
            // 
            // Title
            // 
            this.Title.Text = "Title";
            this.Title.Width = 306;
            // 
            // Status
            // 
            this.Status.Text = "Status";
            this.Status.Width = 131;
            // 
            // Progress
            // 
            this.Progress.Text = "Progress";
            this.Progress.Width = 177;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Cancel,
            this.toolStripSeparator1,
            this.clearDownloads});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(164, 54);
            this.contextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip1_ItemClicked);
            // 
            // Cancel
            // 
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(163, 22);
            this.Cancel.Text = "Cancel";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(160, 6);
            // 
            // clearDownloads
            // 
            this.clearDownloads.Name = "clearDownloads";
            this.clearDownloads.Size = new System.Drawing.Size(163, 22);
            this.clearDownloads.Text = "Clear Downloads";
            // 
            // btnLastPage
            // 
            this.btnLastPage.Enabled = false;
            this.btnLastPage.Location = new System.Drawing.Point(358, 85);
            this.btnLastPage.Margin = new System.Windows.Forms.Padding(2);
            this.btnLastPage.Name = "btnLastPage";
            this.btnLastPage.Size = new System.Drawing.Size(23, 23);
            this.btnLastPage.TabIndex = 15;
            this.btnLastPage.Text = "<";
            this.btnLastPage.UseVisualStyleBackColor = true;
            this.btnLastPage.Click += new System.EventHandler(this.btnLastPage_Click);
            // 
            // btnNextPage
            // 
            this.btnNextPage.Enabled = false;
            this.btnNextPage.Location = new System.Drawing.Point(386, 85);
            this.btnNextPage.Margin = new System.Windows.Forms.Padding(2);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(23, 23);
            this.btnNextPage.TabIndex = 16;
            this.btnNextPage.Text = ">";
            this.btnNextPage.UseVisualStyleBackColor = true;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // cmsComics
            // 
            this.cmsComics.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.search});
            this.cmsComics.Name = "cmsComics";
            this.cmsComics.Size = new System.Drawing.Size(186, 26);
            this.cmsComics.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsComics_ItemClicked);
            // 
            // search
            // 
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(185, 22);
            this.search.Text = "Search for this comic";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 582);
            this.Controls.Add(this.btnNextPage);
            this.Controls.Add(this.btnLastPage);
            this.Controls.Add(this.lvDownloads);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.tbDesc);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.btnOpenLink);
            this.Controls.Add(this.pbCover);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.tbComicSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbComics);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbCover)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.cmsComics.ResumeLayout(false);
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
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListView lvDownloads;
        private System.Windows.Forms.ColumnHeader Title;
        private System.Windows.Forms.ColumnHeader Status;
        private System.Windows.Forms.ColumnHeader Progress;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem Cancel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem clearDownloads;
        private System.Windows.Forms.Button btnLastPage;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.ContextMenuStrip cmsComics;
        private System.Windows.Forms.ToolStripMenuItem search;
    }
}

