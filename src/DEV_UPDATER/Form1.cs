using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Drawing;

namespace DEV_UPDATER
{
    public partial class Form1 : Form
    {
        long filsize, dwsize;


        public Form1()
        {
            InitializeComponent();
        }

        private void closebtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //檢查程式是否關閉？
            foreach (Process p in Process.GetProcessesByName("LRCMaker"))
            {
                p.Kill();
            }
            //Process myProcess = new Process();
            //myProcess.StartInfo.FileName = "Lucky_Gambler_GUI";
            //if (!myProcess.HasExited)  //如果沒有就關閉
            //{
            //    myProcess.Kill();
            //    MessageBox.Show("偵測到主程式未正確關閉，已為您關閉！");
            //}
            Thread.Sleep(2000);
            label1.Text = "Preparing...";
            try
            {
                WebClient wb = new WebClient();
                wb.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadFileProcess);
                wb.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                wb.DownloadFileAsync(new Uri("http://stockoko.idv.tw/vsnote/lrcmaker/Developer/LRCMaker.exe"), "LRCMaker.exe");
            }
            catch (Exception ex)
            {
                label1.Text = ex.ToString();
            }
        }

        private void DownloadFileProcess(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            filsize = e.TotalBytesToReceive;
            dwsize = e.BytesReceived;
            label1.Text = "Downloading Data..." + dwsize.ToString() + "/" + filsize.ToString();
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            label1.Text = "Done!";
            Refresh();
            Thread.Sleep(1000);
            //label1.Text = "正在複製必要的檔案...";
            //File.Move("LRCMAKER.zip", fl);
            //timer1.Start();
            //Thread.Sleep(1000);
            //timer1.Stop();
            //label1.Text = "等待解壓模組載入完成...";
            //Thread.Sleep(1000);
            //label1.Text = "解壓縮檔案...";
            //UnZipFiles(fl, "");
            //label1.Text = "檔案解壓縮完成！";
            //Thread.Sleep(1000);
            //label1.Text = "正在複製必要的檔案...";
            //string pa = Path.GetFullPath("Lucky_Gambler_GUI.exe");
            //if (File.Exists(Application.StartupPath+"\\Lucky_Gambler_GUI.exe"))
            //{
            //    File.Delete(Application.StartupPath+"\\Lucky_Gambler_GUI.exe");
            //    File.Move("C:\\Lucky_Gambler_GUI_new.exe", Application.StartupPath+"\\Lucky_Gambler_GUI.exe");
            //    File.Delete("C:\\Lucky_Gambler_GUI_new.exe");
            //    //File.Replace("Lucky_Gambler_GUI_new.exe", "Lucky_Gambler_GUI.exe", "Lucky_Gambler_GUI.bak");

            //}
            //else
            //{
            //    File.Delete(Application.StartupPath+"\\Lucky_Gambler_GUI.exe");
            //    File.Move("C:\\Lucky_Gambler_GUI_new.exe", Application.StartupPath+"\\Lucky_Gambler_GUI.exe");
            //    File.Delete("C:\\Lucky_Gambler_GUI_new.exe");
            //}
            //    //else File.Copy(fl + @"\LRCMaker.exe", pa);
            label1.Text = "Update sucess! Press\" Done\" to start program.";
            panel1.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process myProcess = new Process();
            myProcess.StartInfo.FileName = "LRCMaker.exe";
            myProcess.StartInfo.Arguments = "vip";
            myProcess.Start();
            Application.Exit();
        }

        private Point startPoint;

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            startPoint = new Point(-e.X + SystemInformation.FrameBorderSize.Width, -e.Y - SystemInformation.FrameBorderSize.Height);
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
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
    }
}
