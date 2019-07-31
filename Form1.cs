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

        private String version = "1.4.2.1";
        private String url = "https://getcomics.info/?s=";
        private int cancelled = 0;
        private int complete;
        private bool status;
        private String pbID;
        private int idCount = 0;
        private int page = 1;

        private object comicIndex;

        private IEnumerable<HtmlNode> nodes, descNodes, ulNodes, newpageNodes, oldpageNodes;

        private System.Net.WebClient client = new System.Net.WebClient();

        private MegaApiClient mClient = new MegaApiClient();

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
            this.Text = "Omnibus - v" + version;
            
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
            searchComics("");

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
                if (lbComics.SelectedItems.Count == 0)
                {
                    MessageBox.Show("You must select a comic before downloading.");
                }
                else
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

                        //Regex regex = new Regex("(?<=go.php-url=)(.*)Mega", RegexOptions.IgnoreCase); //old regex string
                        Regex regex = new Regex("(?<=go.php-urls/)(.*)Mega", RegexOptions.IgnoreCase); //updated regex string to match new paths
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

                                    for (int i = 0; i < g1.Count(); i++)
                                    {
                                        if (g1[i].Contains("aio-purple"))
                                        {
                                            //MessageBox.Show("Found in:" + i);
                                            string[] g3 = g1[i].Split('"');

                                            string gcURL = g3[5];
                                            string[] gcURLArray = gcURL.Split('/');

                                            string encodedValue = "";

                                            if (gcURLArray.Length >= 4)
                                            {
                                                encodedValue = gcURLArray[4];      //first hash location
                                            }
                                            else
                                            {
                                                gcURL = g3[0];
                                                encodedValue = gcURL;               //second hash location
                                            }

                                            if (IsBase64(encodedValue) != true)
                                            {
                                                MessageBox.Show("No downloads available, go to the comic's page to download.");
                                            }

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
                                            break;
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

        
        private void btnClear_Click(object sender, EventArgs e)
        {
            tbComicSearch.Text = "";
            lbComics.Items.Clear();
            tbDesc.Text = "";
            pbCover.Image = Properties.Resources.omnibus_preview_image;
            btnLastPage.Enabled = false;
            btnNextPage.Enabled = false;
            page = 1;
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

        private void lbComics_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = lbComics.IndexFromPoint(e.Location);
                if (index != ListBox.NoMatches)
                {
                    comicIndex = lbComics.Items[index];
                    lbComics.SelectedIndex = index;
                    cmsComics.Show(Cursor.Position);
                }
                lbComics.SelectedItem = lbComics.IndexFromPoint(e.X, e.Y);
                
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

        private void cmsComics_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "search")
            {
                searchComics("listBox");

            }
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

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            searchComics("nextPage");

        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            searchComics("lastPage");

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

        private void searchComics(string function)
        {
            string search = "";

            if (function == "nextPage")
            {
                page++;

                search = tbComicSearch.Text;
            }
            else if (function == "lastPage")
            {
                page--;

                search = tbComicSearch.Text;
            }
            else if (function == "listBox")
            {
                search = lbComics.GetItemText(comicIndex);
                string[] sArray = search.Split('(');
                string[] sArray2 = sArray[0].Split('#');
                search = sArray2[0];

                page = 1;

                tbComicSearch.Text = search;
            }
            else
            {
                page = 1;
                search = tbComicSearch.Text;
            }

            lbComics.Items.Clear();
            pbCover.Image = Properties.Resources.omnibus_preview_image;

            string searchURL = "https://getcomics.info/page/" + page + "/?s=" + search;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(searchURL);
            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                LogWriter("Searching for " + searchURL + " failed.");
                LogWriter(ex.ToString());
            }
            
            if (response == null)
            {
                MessageBox.Show("Comic does not exist. Try again.");

                tbComicSearch.Text = "";
                lbComics.Items.Clear();
                tbDesc.Text = "";
                pbCover.Image = Properties.Resources.omnibus_preview_image;
                btnLastPage.Enabled = false;
                btnNextPage.Enabled = false;
            }
            else
            {
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

                    var npNodes = doc.DocumentNode.Descendants(0).Where(n => n.HasClass("pagination-newer"));
                    var opNodes = doc.DocumentNode.Descendants(0).Where(n => n.HasClass("pagination-older"));

                    if (opNodes.Count() > 0)
                    {
                        btnNextPage.Enabled = true;
                    }
                    else
                    {
                        btnNextPage.Enabled = false;
                    }

                    if (npNodes.Count() > 0)
                    {
                        btnLastPage.Enabled = true;
                    }
                    else
                    {
                        btnLastPage.Enabled = false;
                    }

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
                else
                {
                    MessageBox.Show("There was an error. Try again in a couple of minnutes.");
                }
            }
        }

        private void LogWriter(string line)
        {
            string datetime = DateTime.Now.ToString("MM-dd-yy HH:mm:ss");
            File.AppendAllText(@"log.txt", "(" + datetime + ") - " + line + Environment.NewLine);
        }

        public static bool IsBase64(string base64String)
        {
            if (string.IsNullOrEmpty(base64String) || base64String.Length % 4 != 0
               || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
                return false;

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught: {0}", e);
            }
            return false;
        }
    }
}