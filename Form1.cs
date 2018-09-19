using CG.Web.MegaApiClient;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Omnibus
{
    public partial class Form1 : Form
    {
        private string url = "https://getcomics.info/?s=";
        private int cancelled = 0;

        private IEnumerable<HtmlNode> nodes, descNodes, ulNodes;

        private System.Net.WebClient client = new System.Net.WebClient();

        private MegaApiClient mClient = new MegaApiClient();

        private List<Group> downloadList = new List<Group>();
        private List<String> titleList = new List<String>();

        private CancellationTokenSource cts;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Create Tooltips for settings and download location button
            ToolTip toolTip1 = new ToolTip();

            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;

            toolTip1.SetToolTip(this.button1, "Settings");
            toolTip1.SetToolTip(this.button2, "Open Download Location");

            //Set the save location to the current directory if this is the first run
            if (Properties.Settings.Default.DownloadLocation == "")
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                Properties.Settings.Default.DownloadLocation = path;
                Properties.Settings.Default.Save();
            }

            //Log in to the MEGA client Anonymously
            mClient.LoginAnonymous();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lbComics.Items.Clear();

            string search = tbComicSearch.Text;
            string searchURL = url + search;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(searchURL);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                string data = readStream.ReadToEnd();

                response.Close();
                readStream.Close();

                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(data);

                nodes = doc.DocumentNode.Descendants(0).Where(n => n.HasClass("post-header-image"));

                foreach (HtmlNode n in nodes)
                {
                    string node = n.InnerHtml;
                    string[] a = node.Split('"');

                    lbComics.Items.Add(a[5]);
                }

                descNodes = doc.DocumentNode.Descendants(0).Where(n => n.HasClass("post-info"));
            }
        }

        private void lbComics_SelectedIndexChanged(object sender, EventArgs e)
        {
            HtmlNode n = nodes.ElementAt(lbComics.SelectedIndex);
            string node = n.InnerHtml;
            string[] a = node.Split('"');

            pbCover.Load(a[3]);

            HtmlNode d = descNodes.ElementAt(lbComics.SelectedIndex);
            string descNode = d.InnerHtml;
            string[] b = descNode.Split('"');
            string[] c = b[16].Split('\n');
            string[] desc = c[1].Split('<');

            tbDesc.Text = desc[0];
        }

        private void btnOpenLink_Click(object sender, EventArgs e)
        {
            if (lbComics.Items.Count > 0)
            {
                HtmlNode n = nodes.ElementAt(lbComics.SelectedIndex);
                string node = n.InnerHtml;
                string[] a = node.Split('"');

                try
                {
                    //Open the link on getcomics.info in your default browser
                    System.Diagnostics.Process.Start(a[1]);
                }
                catch (Win32Exception)
                {
                }
            }
            else
            {
                MessageBox.Show("You must search for a comic first.");
            }

            
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (lbComics.Items.Count > 0)
            {
                HtmlNode n = nodes.ElementAt(lbComics.SelectedIndex);
                string node = n.InnerHtml;
                string[] a = node.Split('"');

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(a[1]);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;

                    if (response.CharacterSet == null)
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    }

                    string data = readStream.ReadToEnd();

                    response.Close();
                    readStream.Close();

                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(data);

                    Regex regex = new Regex("href=\"https://mega.nz/.+\"", RegexOptions.IgnoreCase);
                    Match match;

                    for (match = regex.Match(data); match.Success; match = match.NextMatch())
                    {
                        Console.WriteLine("Found a href. Groups: ");
                        foreach (Group group in match.Groups)
                        {
                            string[] g1 = group.ToString().Split('"');

                            if (g1[3] == "aio-purple")
                            {
                                downloadList.Add(group);

                                HtmlNode tn = nodes.ElementAt(lbComics.SelectedIndex);
                                string tnode = n.InnerHtml;
                                string[] ta = node.Split('"');

                                string title = ta[5];
                                lbDownloads.Items.Add(title);
                                titleList.Add(title);
                            }
                            else
                            {
                                ulNodes = doc.DocumentNode.Descendants("ul");

                                foreach (HtmlNode u in ulNodes)
                                {
                                    string ulNode = u.InnerHtml;
                                    string[] b = ulNode.Split(new[] { "<li>" }, StringSplitOptions.None);

                                    for (int i = 1; i < b.Count(); i++)
                                    {
                                        String[] c = b[i].Split('"');

                                        if (c[3] == "_blank&quot;")
                                        {
                                            string[] t0 = b[i].Split('>');
                                            int index = 0;
                                            int fIndex = t0[0].LastIndexOf('(');
                                            int length = fIndex - index;

                                            string title = t0[0].Substring(index, length - 1);

                                            Regex iRegex = new Regex("href=\"https://mega.nz/.+\"", RegexOptions.IgnoreCase);
                                            Match iMatch;

                                            for (iMatch = iRegex.Match(b[i]); iMatch.Success; iMatch = iMatch.NextMatch())
                                            {
                                                foreach (Group iGroup in iMatch.Groups)
                                                {
                                                    downloadList.Add(iGroup);
                                                    lbDownloads.Items.Add(title);
                                                    titleList.Add(title);

                                                    Console.WriteLine(iGroup);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    }

                    if (downloadList.Count > 0)
                    {
                        DownloadComic();
                    }
                }
            }
            else
            {
                MessageBox.Show("You must search for a comic first.");
            }
            
        }

        async private void DownloadComic()
        {
            
            Group group = downloadList[0];
            string[] g1 = group.ToString().Split('"');

            string title = titleList[0];
            string filename = title + ".cbr";

            Uri myStringWebResource = new Uri(g1[1]);

            tbStatus.Text = "Preparing file for download";
            
            INodeInfo node = mClient.GetNodeFromLink(myStringWebResource);
            string downloadPath = Properties.Settings.Default.DownloadLocation + filename;

            IProgress<double> progressHandler = new Progress<double>(x => progressBar1.Value = (int)x);

            tbStatus.Text = "Downloading " + filename + "...";
            btnCancel.Enabled = true;

            cts = new CancellationTokenSource();

            try
            {
                await mClient.DownloadFileAsync(myStringWebResource, downloadPath, progressHandler, cts.Token);
            }
            catch (OperationCanceledException ex)
            {
                DownloadCancel(downloadPath);
            }
            catch (IOException io)
            {
                if (MessageBox.Show("Would you like to overwrite?", "File already exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (File.Exists(downloadPath))
                    {
                        File.Delete(downloadPath);
                    }
                    try
                    {
                        await mClient.DownloadFileAsync(myStringWebResource, downloadPath, progressHandler, cts.Token);
                    }
                    catch (OperationCanceledException ex)
                    {
                        DownloadCancel(downloadPath);
                    }
                }
                else
                {
                    DownloadCancel(downloadPath);
                }
            }

            DownloadComplete();                       

        }

        private void DownloadCancel(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            MessageBox.Show("Download Cancelled");
            tbStatus.Text = "Download cancelled.";
            downloadList.Clear();
            lbDownloads.Items.Clear();
            cancelled = 1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cts.Cancel();
            
            tbStatus.Text = "Download cancelled.";
            downloadList.Clear();
            lbDownloads.Items.Clear();
            cts.Dispose();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbComicSearch.Text = "";
            lbComics.Items.Clear();
            tbDesc.Text = "";
            pbCover.Image = Properties.Resources.omnibus_preview_image;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Settings form = new Settings();
            form.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String path = Properties.Settings.Default.DownloadLocation;
            Process.Start("explorer.exe", path);
        }

        
        private void DownloadComplete()
        {
            progressBar1.Value = 0;
            btnCancel.Enabled = false;

            if (cancelled == 1)
            {
                cancelled = 0;
                return;
            }
            else
            {
                downloadList.RemoveAt(0);
                lbDownloads.Items.RemoveAt(0);

                titleList.RemoveAt(0);

                tbStatus.Text = "File(s) downloaded successfully!";
                if (downloadList.Count > 0)
                {
                    DownloadComic();
                }
            }
                        
        }
    }
}