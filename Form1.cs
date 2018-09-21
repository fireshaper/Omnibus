using CG.Web.MegaApiClient;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
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
        private int complete;
        private int idCount = 0;

        private IEnumerable<HtmlNode> nodes, descNodes, ulNodes;

        private System.Net.WebClient client = new System.Net.WebClient();

        private MegaApiClient mClient = new MegaApiClient();

        private List<Group> downloadList = new List<Group>();
        private List<String> titleList = new List<String>();

        private CancellationTokenSource cts;

        string downloadPath = "";


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
            for (int i = 0; i < lvDownloads.Items.Count; i++)
            {
                lvDownloads.Items[i].Remove();
            }

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

                                //add item to listview
                                AddLVItem("0", title);

                                titleList.Add(title);
                            }
                            else
                            {
                                ulNodes = doc.DocumentNode.Descendants("ul");
                                int id = 0;

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

                                                    //add item to listview
                                                    AddLVItem(id.ToString(),title);
                                                    id++;

                                                    titleList.Add(title);

                                                    Console.WriteLine(title);
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
                        DownloadComic(idCount);
                    }
                }
            }
            else
            {
                MessageBox.Show("You must search for a comic first.");
            }
            
        }

        private void AddLVItem(string id, string title)
        {
            ListViewItem lvi = new ListViewItem();
            ProgressBar pb = new ProgressBar();

            lvi.SubItems[0].Text = title;
            lvi.SubItems.Add("Pending");
            lvi.SubItems.Add("");
            lvi.SubItems.Add(id);
            lvDownloads.Items.Add(lvi);

            Rectangle r = lvi.SubItems[2].Bounds;
            pb.SetBounds(r.X, r.Y, r.Width, r.Height);
            pb.Minimum = 0;
            pb.Maximum = 100;
            pb.Value = 0;
            pb.Name = id;
            lvDownloads.Controls.Add(pb);
        }

        async private void DownloadComic(int idCount)
        {
            complete = 1;
            String id = idCount.ToString();
            Group group = downloadList[0];
            string[] g1 = group.ToString().Split('"');

            string title = titleList[0];
            string filename = title + ".cbr";

            Uri myStringWebResource = new Uri(g1[1]);
         
            INodeInfo node = mClient.GetNodeFromLink(myStringWebResource);
            downloadPath = Properties.Settings.Default.DownloadLocation + filename;

            IProgress<double> progressHandler = new Progress<double>(x => UpdateItemValue(id, (int)x));

            Console.WriteLine("Downloading " + filename);
            btnCancel.Enabled = true;

            cts = new CancellationTokenSource();

            try
            {
                await mClient.DownloadFileAsync(myStringWebResource, downloadPath, progressHandler, cts.Token);
            }
            catch (OperationCanceledException ex)
            {
                complete = 0;
                CancelDownload(downloadPath);
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
                        complete = 0;
                        CancelDownload(downloadPath);
                    }
                }
                else
                {
                    complete = 0;
                    CancelDownload(downloadPath);
                }
            }

            if (complete == 1)
            {
                DownloadComplete();
            }
                                  

        }

        private void UpdateItemValue(string id, int value)
        {
            ListViewItem lvi = GetLVItemById(id);
            ProgressBar pb = GetPBById(id);

            if (lvi != null && pb != null)
            {
                pb.Value = value;

                if (value == 1)
                {
                    lvi.SubItems[1].Text = "Downloading";
                }
                else if (value >= 100)
                {
                    lvi.SubItems[1].Text = "Complete";
                    complete = 1;
                }
                
            }
            
        }

        private void CancelDownload(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            cancelled = 1;
            idCount++;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cts.Cancel();
            
            downloadList.Clear();
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

        private void lvDownloads_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (lvDownloads.FocusedItem.Bounds.Contains(e.Location))
                {
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            
            if (e.ClickedItem.Name == "Cancel")
            {
                cts.Cancel();

                for (int i = 0; i < lvDownloads.Items.Count; i++)
                {
                    if (lvDownloads.Items[i].Selected)
                    {
                        String lvId = lvDownloads.Items[i].SubItems[3].ToString();
                        String[] lvIdArray = lvId.Split('{');
                        String id = lvIdArray[1].Substring(0,1);

                        ListViewItem lvi = GetLVItemById(id);
                        ProgressBar pb = GetPBById(id);

                        lvi.SubItems[1].Text = "Cancelled";
                        lvDownloads.Controls.Remove(pb);

                    }

                }

                DownloadCancelled();

            }
            else if (e.ClickedItem.Name == "clearDownloads")
            {
                for (int i = 0; i < lvDownloads.Items.Count; i++)
                {
                    lvDownloads.Items[i].Remove();
                }
            }
        }

        private void lvDownloads_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            
        }


        private void button2_Click(object sender, EventArgs e)
        {
            String path = Properties.Settings.Default.DownloadLocation;
            Process.Start("explorer.exe", path);
        }

        private ListViewItem GetLVItemById(string id)
        {
            ListViewItem lvi = lvDownloads.Items.Cast<ListViewItem>().FirstOrDefault(q => q.SubItems[3].Text == id);

            return lvi;
        }

        private ProgressBar GetPBById(string id)
        {
            ProgressBar pb = lvDownloads.Controls.OfType<ProgressBar>().FirstOrDefault(q => q.Name == id);

            return pb;
        }

        private void DownloadCancelled()
        {
            downloadList.RemoveAt(0);
            titleList.RemoveAt(0);
            idCount++;

            if (downloadList.Count > 0)
            {
                DownloadComic(idCount);
            }

        }
        
        private void DownloadComplete()
        {
            
            downloadList.RemoveAt(0);
            titleList.RemoveAt(0);
            idCount++;
                
            if (downloadList.Count > 0)
            {
                DownloadComic(idCount);
            }
            else
            {
                btnCancel.Enabled = false;
            }
        }
    }
}