using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;

namespace Lrc_Maker
{
    public partial class UpdateManager : Form
    {
        string[] lack;
        string[] downloadLinks;
        string[] fileNames;
        bool isalldone = false;
        bool plugin;
        int counts = 0;
        WebClient wb;
        Panel progressBar_inside;
        Timer t1;
        int progress = 0;
        string downloadingItems;
        int increase = 1;

        public UpdateManager()
        {
            InitializeComponent();
        }

        public UpdateManager(string[] Item, string[] url, string[] names)
        {
            InitializeComponent();
            lack = Item;
            downloadLinks = url;
            fileNames = names;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!isalldone)
            {
                label1.Text = "準備更新...";
                progressBar_inside = new Panel();
                progressBar_inside.Parent = progressBar_outside;
                progressBar_inside.BackColor = Color.DodgerBlue;
                progressBar_inside.Height = progressBar_outside.Height;
                progressBar_inside.Location = new Point(0, 0);
                progressBar_inside.Width = 0;
                t1 = new Timer();
                t1.Interval = 10;
                t1.Tick += new EventHandler(t1_Tick);
                t1.Start();

                counts = 0;
                button3.Enabled = false;
                wb = new WebClient();
                wb.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadFileProcess);
                wb.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                dodownload();
            }
            else
            {
                if (plugin)
                {
                    this.Close();
                }
                else
                {
                    Form1 f = new Form1();
                    f.Show();
                    this.Hide();
                }
            }
        }

        private void t1_Tick(object s, EventArgs e)
        {
            if (progressBar_inside.Width < progress)
                progressBar_inside.Width += increase;
        }

        private void DownloadFileProcess(object sender, DownloadProgressChangedEventArgs e)
        {
            label1.Text = string.Format("正在下載 {0} - 進度 {1}/{2} KB", downloadingItems, Math.Round((double)(e.BytesReceived / 1024), 2).ToString(), Math.Round((double)(e.TotalBytesToReceive / 1024)).ToString());
            progress = progressBar_outside.Width * e.ProgressPercentage / 100;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            progress = progressBar_outside.Width;
            listView1.Items[counts].Text += " (Done)";
            listView1.Items[counts].ForeColor = Color.Gray;
            counts++;
            if (counts < lack.Length) dodownload();
            else
            {
                wb.Dispose();
                increase = 30;
                isalldone = true;
                button3.Enabled = true;
                label1.Text = "完成";
                button3.Text = "完成";
            }
        }

        void dodownload()
        {
            try
            {
                wb.DownloadFileAsync(new Uri(downloadLinks[counts]), fileNames[counts]);
                listView1.Items[counts].Text += "...";
                downloadingItems = lack[counts];
                //switch (lack[counts])
                //{
                //    case "DeveloperModule":
                //        wb.DownloadFileAsync(new Uri("https://sky880319.github.io/lrcmaker/DEV_UPDATER.exe"), "DEV_UPDATER.exe");
                //        wb.Dispose();
                //        break;
                //    case "BassNetDLL":
                //        wb.DownloadFileAsync(new Uri("https://sky880319.github.io/lrcmaker/Bass.Net.dll"), "Bass.Net.dll");
                //        wb.Dispose();
                //        break;
                //    case "BassDLL":
                //        wb.DownloadFileAsync(new Uri("https://sky880319.github.io/lrcmaker/bass.dll"), "bass.dll");
                //        wb.Dispose();
                //        break;
                //    case "Updater":
                //        wb.DownloadFileAsync(new Uri("https://sky880319.github.io/lrcmaker/LRCMAKER_UPDATER.exe"), "LRCMAKER_UPDATER.exe");
                //        wb.Dispose();
                //        break;
                //    case "UpdateUnZipModule":
                //        wb.DownloadFileAsync(new Uri("https://sky880319.github.io/lrcmaker/ICSharpCode.SharpZipLib.dll"), "ICSharpCode.SharpZipLib.dll");
                //        wb.Dispose();
                //        break;
                //    case "Youtube-Dl":
                //        wb.DownloadFileAsync(new Uri("https://sky880319.github.io/lrcmaker/youtube-dl.exe"), "youtube-dl.exe");
                //        break;
                //}
            }
            catch (Exception ex)
            {
                label1.Text = string.Format("發生錯誤，請聯絡開發人員。原因：{0}", ex.Message);
            }
        }

        private void UpdateManager_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < lack.Length; i++)
            {
                switch(lack[i])
                {
                    case "DeveloperModule":
                        listView1.Items.Add("開發人員測試組件").SubItems.Add("DeveloperModule");
                        break;
                    case "BassNetDLL":
                        listView1.Items.Add("播放核心程式庫").SubItems.Add("BassNetDLL");
                        break;
                    case "BassDLL":
                        listView1.Items.Add("播放核心組件").SubItems.Add("BassDLL");
                        break;
                    case "Updater":
                        listView1.Items.Add("自動更新組件").SubItems.Add("Updater");
                        break;
                    case "UpdateUnZipModule":
                        listView1.Items.Add("自動更新程式所需的函式庫").SubItems.Add("UpdateUnZipModule");
                        break;
                    case "Youtube-Dl":
                        listView1.Items.Add("Youtube串流網址服務").SubItems.Add("Youtube-Dl");
                        plugin = true;
                        break;
                }
            }
        }

        private void closebtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpdateManager_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(200, 200, 200)), 0, 0, this.Width - 1, this.Height - 1);
            progressBar_outside.BackColor = Color.FromArgb(15, 0, 0, 0);
        }
    }
}
