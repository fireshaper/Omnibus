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
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebClient = System.Net.WebClient;

namespace Omnibus
{
    public partial class Form1 : Form
    {

        private String version = "1.4.7";
        private String url = "https://getcomics.info/?s=";
        private int cancelled = 0;
        private bool isDownloading = false;
        private int complete;
        private bool status;
        private String pbID;
        private int idCount = 0;
        public int LVCount = 0;
        private int page = 1;

        private object comicIndex;

        private IEnumerable<HtmlNode> nodes, descNodes, ulNodes, newpageNodes, oldpageNodes;

        private System.Net.WebClient client = new System.Net.WebClient();

        private MegaApiClient mClient = new MegaApiClient();

        private List<String> downloadList = new List<String>();
        private List<String> titleList = new List<String>();

        private CancellationTokenSource cts;

        string downloadPath = "";

        string megaURL = "";


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
            if (Properties.Settings.Default.LogLocation == "")
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                Properties.Settings.Default.LogLocation = path;
                Properties.Settings.Default.Save();
            }


            //Log in to the MEGA client Anonymously
            mClient.LoginAnonymous();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            searchComics("");
            LogWriter("Searched for new comics.");
        }

        private void lbComics_SelectedIndexChanged(object sender, EventArgs e)
        {
            HtmlNode n = nodes.ElementAt(lbComics.SelectedIndex);
            string node = n.InnerHtml;
            string[] a = node.Split('"');

            try
            {
                pbCover.Load(a[3]);
            }
            catch (WebException we)
            {
                LogWriter("Error loading cover image.");
                pbCover.Image = Omnibus.Properties.Resources.omnibus_preview_image;
            }
            

            HtmlNode d = descNodes.ElementAt(lbComics.SelectedIndex);
            string descNode = d.InnerHtml;
            string[] b = descNode.Split('"');
            string[] c = b[16].Split('\n');
            string[] preDesc = c[0].Split('>');
            string[] desc = preDesc[6].Split('<');

            string[] c1 = c[0].Split('|');
            string[] c2 = c1[0].Split('>');
            string date = c2[3];


            tbDesc.Text = "Date: " + date + "\r\n\r\n" + desc[0];
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
                    LogWriter("Caught an exception when trying to open the link in your browser: " + a[1]);
                }
            }
            else
            {
                MessageBox.Show("You must search for a comic first.");
            }
        }

        private async void btnDownload_ClickAsync(object sender, EventArgs e)
        {
            //if no comics are downloading, clear the list
            if (isDownloading == false)
            {
                for (int i = 0; i < lvDownloads.Items.Count; i++)
                {
                    lvDownloads.Items[i].Remove(); 
                }

                idCount = 0;
                downloadList.Clear();
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

                        string comicDLLink = "";

                        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                        doc.LoadHtml(data);

                        var htmlNodes = doc.DocumentNode.SelectSingleNode("//a[@title='Mega Link']");

                        if (htmlNodes == null)
                        {
                            foreach(HtmlNode node1 in doc.DocumentNode.SelectNodes("//a"))
                            {
                                if (node1.SelectNodes(".//span") != null)
                                {
                                    foreach (HtmlNode node2 in node1.SelectNodes(".//span"))
                                    {
                                        string value = node2.InnerText;
                                        if (value == "Mega")
                                        {
                                            comicDLLink = node1.Attributes["href"].Value;
                                        }

                                    }
                                }
                            }
                        }
                        else
                        {
                            comicDLLink = htmlNodes.Attributes["href"].Value;
                        }                       

                        if (comicDLLink != "")
                        {
                            //string lastEV = "";
                            string lastURL = "";
                            List<string> EVs = new List<string>();

                            //Get the MEGA link by getting response of download link
                            /*
                            using (var client = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip }))
                            {
                                var hrequest = new HttpRequestMessage()
                                {
                                    RequestUri = new Uri(comicDLLink),
                                    Method = HttpMethod.Get
                                };

                                HttpResponseMessage hresponse = client.SendAsync(hrequest).Result;
                                var statusCode = (int)hresponse.StatusCode;

                                megaURL = hrequest.RequestUri.ToString();
                            }
                            */

                            //Get MEGA link by decrypting Base64 string in the address
                            string[] comicLinkArray = comicDLLink.Split('/');
                            string comicLinkEnc = comicLinkArray[4];
                            byte[] comicLinkConverted = System.Convert.FromBase64String(comicLinkEnc);
                            megaURL = System.Text.ASCIIEncoding.ASCII.GetString(comicLinkConverted);


                            if (lastURL != megaURL)
                            {
                                downloadList.Add(megaURL);

                                HtmlNode tn = nodes.ElementAt(lbComics.SelectedIndex);
                                string tnode = n.InnerHtml;
                                string[] ta = node.Split('"');

                                string title = replaceASCII(ta[5]);

                                //add item to listview
                                AddLVItem(idCount.ToString(), title);
                                //idCount++;

                                titleList.Add(title);

                                lastURL = megaURL;
                            }
                                        
                        }
                        else
                            MessageBox.Show("No download link available. Go to comic's page and download manually.");


                        //if (downloadList.Count > 0 && isDownloading == false)
                        //{
                        //    DownloadComic(idCount);
                        //}

                        DownloadComic(idCount);
                        idCount++;
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
            ListViewItem comicItem = new ListViewItem();
            ProgressBar pb = new ProgressBar();

            comicItem.SubItems[0].Text = title;
            comicItem.SubItems.Add("Pending");
            comicItem.SubItems.Add("");
            comicItem.SubItems.Add(id);
            lvDownloads.Items.Add(comicItem);
            //lvDownloads.Items.Add(new ListViewItem(new[] { title, "Pending", "", id }));
            Debug.WriteLine("Adding " + title + " to lvDownloads with ID: " + id);

            Rectangle r = comicItem.SubItems[2].Bounds;
            pb.SetBounds(r.X, r.Y, r.Width, r.Height);
            pb.Minimum = 0;
            pb.Maximum = 100;
            pb.Value = 0;
            pb.Name = id;
            lvDownloads.Controls.Add(pb);
        }

        public async void DownloadComic(int idCount)
        {
            complete = 1;
            status = true;
            String id = idCount.ToString();
            //String id = lvDownloads.Items[0].SubItems[3].Text;
            //String url = downloadList[0];
            String url = downloadList[idCount];

            Console.WriteLine("id = " + id);

            Uri myStringWebResource = new Uri(url);

            try
            {
                INodeInfo node = mClient.GetNodeFromLink(myStringWebResource);
            }
            catch (Exception e)
            {
                LogWriter("Exception from mega client: " + e);
            }

            //string filename = titleList[0];
            string filename = titleList[idCount];
            downloadPath = Properties.Settings.Default.DownloadLocation + "\\" + filename + ".cbr";

            IProgress<double> progressHandler = new Progress<double>(x => UpdateItemValue(id, (int)x));  //update progressbar with progress
            //IProgress<double> progressHandler = new Progress<double>(x => Console.WriteLine("{0}%", x)); //write progress to Console

            Console.WriteLine("Downloading: " + downloadPath);
            LogWriter("Downloading new comic: " + downloadPath);
            isDownloading = true;

            cts = new CancellationTokenSource();

            try
            {
                await mClient.DownloadFileAsync(myStringWebResource, downloadPath, progressHandler, cts.Token);
            }
            catch (OperationCanceledException ex)
            {
                complete = 0;
                CancelDownload(downloadPath);
                isDownloading = false;
                LogWriter("Download Canceled: " + downloadPath);
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
                        isDownloading = false;
                        CancelDownload(downloadPath);
                    }
                }
                else
                {
                    complete = 0;
                    isDownloading = false;
                    CancelDownload(downloadPath);
                }
            }
            catch (ArgumentException aex)
            {
                complete = 0;
                isDownloading = false;
                CancelDownload(downloadPath);
                LogWriter(aex.ToString());

                MessageBox.Show("There was a problem downloading the comic. Try again later or download manually by clicking the Open Link button.\n\nIf this continues to happen try the following:\n1: Close Omnibus\n2: Go to https://getcomics.info in your browser\n3: Search for any comic and click the Mega button\n4: Open Omnibus and attempt the download again.");
            }

            //return "complete";
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
                    isDownloading = true;
                }
                else if (value >= 100)
                {
                    lvi.SubItems[1].Text = "Complete";
                    lvDownloads.Controls.Remove(pb);
                    complete = 1;
                    isDownloading = false;
                    DownloadComplete();
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
            isDownloading = false;
            //idCount++;
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
                idCount = 0;
            }
        }

        private void cmsComics_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "search")
            {
                searchComics("listBox");

            }
            if (e.ClickedItem.Name == "validate")
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

                    string comicDLLink = "";

                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(data);

                    var htmlNodes = doc.DocumentNode.SelectSingleNode("//a[@title='Mega Link']");

                    if (htmlNodes == null)
                    {
                        foreach (HtmlNode node1 in doc.DocumentNode.SelectNodes("//a"))
                        {
                            if (node1.SelectNodes(".//span") != null)
                            {
                                foreach (HtmlNode node2 in node1.SelectNodes(".//span"))
                                {
                                    string value = node2.InnerText;
                                    if (value == "Mega")
                                    {
                                        comicDLLink = node1.Attributes["href"].Value;
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        comicDLLink = htmlNodes.Attributes["href"].Value;
                    }

                    if (comicDLLink != "")
                    {
                        //string lastEV = "";
                        string lastURL = "";
                        List<string> EVs = new List<string>();

                        //Get MEGA link by decrypting Base64 string in the address
                        string[] comicLinkArray = comicDLLink.Split('/');
                        string comicLinkEnc = comicLinkArray[4];
                        byte[] comicLinkConverted = System.Convert.FromBase64String(comicLinkEnc);
                        megaURL = System.Text.ASCIIEncoding.ASCII.GetString(comicLinkConverted);

                        MessageBox.Show("MEGA URL: " + megaURL);

                    }
                    else
                        MessageBox.Show("No download link available. Go to comic's page and download manually.");
                }
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
            //idCount++;

            //if (downloadList.Count > 0)
            //{
            //    DownloadComic(idCount);
            //}
        }

        private void tbComicSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(this, new EventArgs());
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void DownloadComplete()
        {
            
            downloadList.RemoveAt(0);
            titleList.RemoveAt(0);
                
            //if (downloadList.Count > 0)
            //{
            //    idCount++;
            //    DownloadComic(idCount);
            //}

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
                    MessageBox.Show("There was an error. Try again in a couple of minutes.");
                }
            }
        }

        private void LogWriter(string line)
        {
            string datetime = DateTime.Now.ToString("MM-dd-yy HH:mm:ss");
            string logPath = Properties.Settings.Default.LogLocation;

            File.AppendAllText(logPath + "\\log.txt", "(" + datetime + ") - " + line + Environment.NewLine);
        }

    }
}