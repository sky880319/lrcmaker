using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;

using Google.Apis.Services;
using Google.Apis.YouTube.v3;

namespace Lrc_Maker
{
    public partial class SearchVideo : Form
    {
        string[] Lryic = { };
        private Point startPoint;
        public bool cancel = true;
        public RichTextBox richTextBox1 = new RichTextBox();
        public string title;
        public string tempID;
        public string requestUrl;
        public bool isdownloaded = false;

        public SearchVideo()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                //Process COLOR
                Random rnd = new Random(Guid.NewGuid().GetHashCode());
                int colornum = rnd.Next(0, 5);
                switch (colornum)
                {
                    case 1:
                        panel4.BackColor = Color.Orange;
                        break;
                    case 2:
                        panel4.BackColor = Color.DarkGray;
                        break;
                    case 3:
                        panel4.BackColor = Color.DarkSeaGreen;
                        break;
                    case 4:
                        panel4.BackColor = Color.FromArgb(255, 215, 170, 101);
                        break;
                    case 5:
                        panel4.BackColor = Color.YellowGreen;
                        break;
                    default:
                        panel4.BackColor = Color.DeepSkyBlue;
                        break;
                }

                listView1.Items.Clear();
                try
                {
                    panel3.Visible = true;
                    panel4.Width = 30;
                    timer1.Start();
                    Run();
                }
                catch (AggregateException ex)
                {
                    foreach (var exc in ex.InnerExceptions)
                    {
                        MessageBox.Show("Error: " + exc.Message);
                    }
                    timer1.Stop();
                    panel3.Visible = false;
                }
            }
        }

        private void Run()
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyCT-LrPTbXx8utRMfczV3Qilzy0SvpD20M",
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            Console.Write("Search:");
            string search = textBox1.Text;
            searchListRequest.Q = search; // Replace with your search term.
            searchListRequest.MaxResults = 50;

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = searchListRequest.Execute();
            panel4.Width = 688;
            //List<string> videos = new List<string>();
            //List<string> channels = new List<string>();
            //List<string> playlists = new List<string>();

            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.
            int count = 0;
            foreach (var searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        //videos.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.VideoId));
                        listView1.Items.Add(searchResult.Snippet.Title);
                        listView1.Items[count].SubItems.Add(searchResult.Snippet.ChannelTitle);
                        listView1.Items[count].SubItems.Add(searchResult.Id.VideoId);
                        count++;
                        break;
                }
            }
            timer1.Stop();
            panel3.Visible = false;
        }

        Youtube_dl youtube_dl;
        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
                backgroundWorker1.Dispose();
            }
            int count = 0;
            int currentid = Process.GetCurrentProcess().Id;
            foreach (Process p in Process.GetProcessesByName("youtube-dl"))
            {
                count++;
            }
            if (count >= 1)
            {
                foreach (Process p in Process.GetProcessesByName("youtube-dl"))
                {
                    if (p.Id != currentid)
                        p.Kill();
                }
            }
            Random rdn = new Random();
            title = listView1.Items[listView1.SelectedIndices[0]].Text;
            tempID = "" + rdn.Next(11111, 99999);
            youtube_dl = new Youtube_dl();
            //youtube_dl.Download(listView1.Items[listView1.SelectedIndices[0]].SubItems[2].Text,"mp3", tempID + ".%(ext)s");
            //youtube_dl.proc.OutputDataReceived += new DataReceivedEventHandler((s, e1) =>
            //{
            //    label1.Text = e1.Data;
            //});
            //youtube_dl.proc.BeginOutputReadLine();

            youtube_dl.getVideoUrl(listView1.Items[listView1.SelectedIndices[0]].SubItems[2].Text, "mp4");
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            listView1.Enabled = false;
            youtube_dl.proc.WaitForExit();
            //youtube_dl.proc.Close();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            requestUrl = youtube_dl.proc.StandardOutput.ReadToEnd();
            listView1.Enabled = true;
            youtube_dl.proc.Close();
            isdownloaded = true;
            this.Close();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            startPoint = new Point(-e.X, -e.Y);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            //如果使用者使用的是左鍵按下，意旨使用右鍵拖曳無效
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                //新視窗的位置
                mousePos.Offset(startPoint.X, startPoint.Y);
                //改變視窗位置
                Location = mousePos;
            }
        }

        private void closebtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) //paragraph
            {
                button1_Click(this, null);
            }
        }

        private void mini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


        void wb_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            panel4.Width += (688 - panel4.Width) * e.ProgressPercentage;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (panel4.Width < 300)
                panel4.Width += 5;
        }

        private void SearchVideo_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(200, 200, 200)), 0, 0, this.Width - 1, this.Height - 1);
        }
    }

    public class Youtube_dl
    {
        public ProcessStartInfo procinfo;
        public Process proc;
        public void Download(string videoID, string format = "", string fileName = "")
        {
            if (proc != null) proc.Close();
            string arguments = "-x";
            if (format != "") arguments += " --audio-format " + format;
            if (fileName != "") arguments += " -o temp/" + fileName;
            arguments += " https://www.youtube.com/watch?v=" + videoID;
            procinfo = new ProcessStartInfo
            {
                FileName = "youtube-dl",
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            proc = new Process
            {
                StartInfo = procinfo
            };
            proc.Start();
        }

        public void getVideoUrl(string videoID, string format = "")
        {
            if (proc != null) proc.Close();
            string arguments = "-g";
            if (format != "") arguments += " -f " + format;
            arguments += " https://www.youtube.com/watch?v=" + videoID;
            procinfo = new ProcessStartInfo
            {
                FileName = "youtube-dl",
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            proc = new Process
            {
                StartInfo = procinfo
            };
            proc.Start();

        }

        public bool HasExit() { return proc.HasExited; }
    }
}
