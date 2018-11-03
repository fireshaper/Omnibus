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
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.DownloadLocation = tbDLocation.Text;
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
    }
}
