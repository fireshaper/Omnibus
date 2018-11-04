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
        private String url = "https://getcomics.info/?s=";
        private int cancelled = 0;
        private int complete;
        private bool status;
        private String pbID;
        private int idCount = 0;

        private IEnumerable<HtmlNode> nodes, descNodes, ulNodes, liNodes;

        private System.Net.WebClient client = new System.Net.WebClient();

        private MegaApiClient mClient = new MegaApiClient();

        //private List<Group> downloadList = new List<Group>();
        private List<String> downloadList = new List<String>();
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

                    string title = replaceASCII(a[5]);
                                     
                    lbComics.Items.Add(title);
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
            string[] preDesc = c[0].Split('>');
            string[] desc = preDesc[6].Split('<');

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

                    //Regex regex = new Regex("href=\"https://mega.nz/.+\"", RegexOptions.IgnoreCase);
                    Regex regex = new Regex("(?<=go.php-url=)(.*)Mega", RegexOptions.IgnoreCase);
                    Match match;

                    if (regex.Match(data).Success != false)
                    {
                        for (match = regex.Match(data); match.Success; match = match.NextMatch())
                        {
                            string lastEV = "";
                            List<string> EVs = new List<string>();

                            foreach (Group group in match.Groups)
                            {
                                string[] g1 = group.ToString().Split(new string[] { "<a" }, StringSplitOptions.None);
                                int lastURL = g1.Length - 1;
                                string[] g2 = g1[lastURL].Split('"');
                                                                
                                if (g2[7] == "aio-purple")
                                {
                                    string gcURL = g2[5];
                                    string[] gcURLArray = gcURL.Split('/');

                                    string encodedValue = gcURLArray[4];

                                    if (lastEV != encodedValue)
                                    {
                                        byte[] urlData = Convert.FromBase64String(encodedValue);
                                        string decodedURL = Encoding.UTF8.GetString(urlData);

                                        downloadList.Add(decodedURL);

                                        HtmlNode tn = nodes.ElementAt(lbComics.SelectedIndex);
                                        string tnode = n.InnerHtml;
                                        string[] ta = node.Split('"');

                                        string title = replaceASCII(ta[5]);

                                        //add item to listview
                                        AddLVItem("0", title);

                                        titleList.Add(title);

                                        lastEV = encodedValue;
                                    }

                                }
                                else
                                {
                                    ulNodes = doc.DocumentNode.Descendants("ul");
                                    //liNodes = doc.DocumentNode.Descendants("li");
                                    int id = 0;
                                    

                                    foreach (HtmlNode u in ulNodes)
                                    {
                                        string ulNode = u.InnerHtml;
                                        string[] b = ulNode.Split(new[] { "<li>" }, StringSplitOptions.None);

                                        for (int i = 1; i < b.Count(); i++)
                                        {
                                            String[] c = b[i].Split('"');

                                            if (c.Length > 7 && c[7] == "_blank&quot;")
                                            {
                                                string[] t0 = b[i].Split('>');
                                                int index = 0;
                                                int fIndex = t0[0].LastIndexOf('(');
                                                int length = fIndex - index;

                                                string titleSub = t0[0].Substring(index, length - 1);

                                                string title = replaceASCII(titleSub);

                                                Regex iRegex = new Regex("(?<=go.php-url=)(.*)Mega", RegexOptions.IgnoreCase);
                                                Match iMatch;

                                                if (regex.Match(data).Success != false)
                                                {
                                                    for (iMatch = iRegex.Match(b[i]); iMatch.Success; iMatch = iMatch.NextMatch())
                                                    {
                                                        foreach (Group iGroup in iMatch.Groups)
                                                        {
                                                            string[] g01 = iGroup.ToString().Split(new string[] { "<a" }, StringSplitOptions.None);
                                                            int lastURLa = g01.Length - 1;
                                                            string[] g02 = g01[lastURLa].Split('"');

                                                            string gcURL = g02[5];
                                                            string[] gcURLArray = gcURL.Split('/');

                                                            string encodedValue = gcURLArray[4];

                                                            if (!EVs.Contains(encodedValue))
                                                            {
                                                                byte[] urlData = Convert.FromBase64String(encodedValue);
                                                                string decodedURL = Encoding.UTF8.GetString(urlData);

                                                                downloadList.Add(decodedURL);

                                                                //add item to listview
                                                                AddLVItem(id.ToString(), title);
                                                                id++;

                                                                titleList.Add(title);

                                                                Console.WriteLine(title);

                                                                EVs.Add(encodedValue);
                                                            }

                                                            
                                                        }
                                                    }
                                                }
                                                else
                                                    MessageBox.Show("No downloads available, go to the comic's page to download.");

                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }
                    else
                        MessageBox.Show("No download link available. Go to comic's page and download manually.");
                    

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
            status = true;
            String id = idCount.ToString();
            String url = downloadList[0];
            
            Uri myStringWebResource = new Uri(url);
         
            INodeInfo node = mClient.GetNodeFromLink(myStringWebResource);
            string filename = node.Name;
            downloadPath = Properties.Settings.Default.DownloadLocation + "\\" + filename;
         
            IProgress<double> progressHandler = new Progress<double>(x => UpdateItemValue(id, (int)x));

            Console.WriteLine("Downloading: " + downloadPath);
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

                if (value == 1 && status == true)
                {
                    lvi.SubItems[1].Text = "Downloading";
                    status = false;
                }
                else if (value >= 100)
                {
                    lvi.SubItems[1].Text = "Complete";
                    lvDownloads.Controls.Remove(pb);
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
                
            if (downloadList.Count > 0)
            {
                idCount++;
                DownloadComic(idCount);
            }
            else
            {
                btnCancel.Enabled = false;
            }
        }

        private string replaceASCII(string title)
        {
            if (title.Contains("&#8211;"))
                title = title.Replace("&#8211;", "-");

            if (title.Contains("&#8217;"))
                title = title.Replace("&#8217;", "'");

            if (title.Contains("&#038;"))
                title = title.Replace("&#038;", "&");

            return title;
        }
    }
}