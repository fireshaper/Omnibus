using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omnibus
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            tbDLocation.Text = Properties.Settings.Default.DownloadLocation;
            tbLLocation.Text = Properties.Settings.Default.LogLocation;

            if (Properties.Settings.Default.LogEnabled == true)
            {
                cbLogs.Checked = true;
            }
            else
            {
                cbLogs.Checked = false;
            }

            tbUserAgent.Text = Properties.Settings.Default.UserAgent;
            tbCfduid.Text = Properties.Settings.Default.cfduid;
            tbCF_clearance.Text = Properties.Settings.Default.cf_clearance;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.DownloadLocation = tbDLocation.Text;
            Properties.Settings.Default.Save();

            Properties.Settings.Default.LogLocation = tbLLocation.Text;
            Properties.Settings.Default.Save();

            Properties.Settings.Default.UserAgent = tbUserAgent.Text;
            Properties.Settings.Default.Save();

            Properties.Settings.Default.cfduid = tbCfduid.Text;
            Properties.Settings.Default.Save();

            Properties.Settings.Default.cf_clearance = tbCF_clearance.Text;
            Properties.Settings.Default.Save();

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                tbDLocation.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnBrowseLog_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                tbLLocation.Text = folderBrowserDialog1.SelectedPath;
            }
        }


        private void cbLogs_CheckedChanged(object sender, EventArgs e)
        {
            if (cbLogs.Checked == true)
            {
                Properties.Settings.Default.LogEnabled = true;
            }
            else
            {
                Properties.Settings.Default.LogEnabled = false;
            }
        }
    }
}
