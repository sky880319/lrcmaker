using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections;

namespace LRCMAKER_UPDATER
{
    public partial class Form1 : Form
    {
        long filsize, dwsize;
        string fl = @"C:\LRCMAKER_UPDATE_TEMP\LRCMAKER.zip";

        private void btUnzip_Click(object sender, EventArgs e)
        {
            string path = @"d:\tmp\test1\test.zip";//壓縮檔案路徑
            UnZipFiles(path, string.Empty);
        }

        //讀取目錄下所有檔案
        private static ArrayList GetFiles(string path)
        {
            ArrayList files = new ArrayList();

            if (Directory.Exists(path))
            {
                files.AddRange(Directory.GetFiles(path));
            }

            return files;
        }

        //建立目錄
        private static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!System.IO.Directory.Exists(@"C:\LRCMAKER_UPDATE_TEMP\"))
            {
                System.IO.Directory.CreateDirectory(@"C:\LRCMAKER_UPDATE_TEMP\");
            }
            else
            {
                System.IO.Directory.Delete(@"C:\LRCMAKER_UPDATE_TEMP\", true);
                System.IO.Directory.CreateDirectory(@"C:\LRCMAKER_UPDATE_TEMP\");
            }
        }

        void XXX ()
        {
            label1.Text = "正在準備...";
            try
            {
                WebClient wb = new WebClient();
                wb.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadFileProcess);
                wb.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                wb.DownloadFileAsync(new Uri("http://stockoko.idv.tw/vsnote/lrcmaker/LRCMAKER.zip"), "LRCMAKER.zip");
            }
            catch (Exception ex)
            {
                label1.Text = ex.ToString();
            }
        }

        private void DownloadFileProcess (object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            filsize = e.TotalBytesToReceive;
            dwsize = e.BytesReceived;
            label1.Text = "正在下載資料..." + dwsize.ToString() + "/" + filsize.ToString();
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            label1.Text = "下載已完成！";
            progressBar1.Value = 0;
            Thread.Sleep(1000);
            label1.Text = "正在複製必要的檔案...";
            File.Move("LRCMAKER.zip", fl);
            timer1.Start();
            Thread.Sleep(1000);
            timer1.Stop();
            label1.Text = "等待解壓模組載入完成...";
            Thread.Sleep(1000);
            label1.Text = "解壓縮檔案...";
            UnZipFiles(fl,"");
            label1.Text = "檔案解壓縮完成！";
            Thread.Sleep(1000);
            label1.Text = "正在複製必要的檔案...";
            string pa = Path.GetFullPath("LRCMaker.exe");
            if (File.Exists("LRCMaker.exe"))
            {
                File.Delete("LRCMaker.exe");
                File.Copy(@"C:\LRCMAKER_UPDATE_TEMP\LRCMAKER\LRCMaker.exe", pa);
            }
            else File.Copy(fl + @"\LRCMaker.exe", pa);
            System.Diagnostics.Process.Start("LRCMaker.exe");
            Application.Exit();
        }
        private void UnZipFiles(string path, string password)
        {
            if (File.Exists(fl))
            {
                ZipInputStream zis = null;

                try
                {
                    string unZipPath = path.Replace(".zip", "");
                    System.IO.Directory.CreateDirectory(unZipPath);
                    zis = new ZipInputStream(File.OpenRead(path));
                    if (password != null && password != string.Empty) zis.Password = password;
                    ZipEntry entry;

                    while ((entry = zis.GetNextEntry()) != null)
                    {
                        string filePath = unZipPath + @"\" + entry.Name;

                        if (entry.Name != "")
                        {
                            FileStream fs = File.Create(filePath);
                            int size = 2048;
                            byte[] buffer = new byte[2048];
                            while (true)
                            {
                                size = zis.Read(buffer, 0, buffer.Length);
                                if (size > 0) { fs.Write(buffer, 0, size); timer1.Start(); }
                                else { timer1.Stop(); break; }
                            }

                            fs.Close();
                            fs.Dispose();
                        }
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                finally
                {
                    zis.Close();
                    zis.Dispose();
                }

                
            }
            else MessageBox.Show("更新的檔案不存在，請重新執行更新！");
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            XXX();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Value++;
        }
    }
}
