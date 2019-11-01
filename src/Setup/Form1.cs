using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using Microsoft.Win32;

namespace Setup
{
    public partial class Form1 : Form
    {
        private Point startPoint;
        string[] lack = {"LRCMaker", "BassNetDLL", "BassDLL", "Updater", "UpdateUnZipModule" };
        string[] lack_Name = {"主程式", "播放核心函式庫", "播放核心組件", "自動更新組件", "自動更新程式所需的函式庫", "Youtube串流網址服務" };
        public bool isalldone = false;
        public bool isrunning = false;
        public bool haswrong = false;
        WebClient wb;
        Timer t1;
        Panel progressBar_inside;
        Panel uninstallProgressBar_inside;
        int counts = 0;
        int progress;
        string downloadingItems;
        string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LRCMaker");
        //const int CS_DropSHADOW = 0x20000;
        //const int GCL_STYLE = (-26);
        ////声明Win32 API
        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //public static extern int GetClassLong(IntPtr hwnd, int nIndex);

        string ver = "N/A", ver_Date = "----";

        public Form1()
        {
            InitializeComponent();
            //SetClassLong(this.Handle, GCL_STYLE, GetClassLong(this.Handle, GCL_STYLE) | CS_DropSHADOW);

        }

        private void titleBar_MouseDown(object sender, MouseEventArgs e)
        {
            startPoint = new Point(-e.X, -e.Y);
        }

        private void titleBar_MouseMove(object sender, MouseEventArgs e)
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

        private void close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",true).GetSubKeyNames().Contains("LRCMaker"))
            {
                tabControl1.SelectedIndex = 4;
            }
            richTextBox1.SetInnerMargins(5, 5, 5, 0);
            errorInfo.SetInnerMargins(5, 5, 5, 0);
            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.ItemSize = new Size(0, 1);
            tabControl1.SizeMode = TabSizeMode.Fixed;
            if (CheckConnect())
            {
                backgroundWorker1.RunWorkerAsync();
                label10.Text = string.Format(label10.Text, "正常");
                installBtn.Enabled = true;
            }
            else
            {
                label4.Text = string.Format(label4.Text, " -");
                label5.Text = string.Format(label5.Text, " - ", " -");
                label10.Text = string.Format(label10.Text, "無法連線至伺服器取得資料，請檢查網路連線是否一切正常。");
                installBtn.Enabled = false;
            }
        }

        private bool CheckConnect()
        {
            try
            {
                System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient("sky880319.github.io", 80);
                client.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            WebClient wc = new WebClient();
            ver = wc.DownloadString("https://sky880319.github.io/lrcmaker/updateinfo.html");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://ivsnote.com/");
        }

        private void readmeBtn_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void installBtn_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        bool ischecked = false;
        private void tabPage3_Paint(object sender, PaintEventArgs e)
        {
            if (!ischecked)
            {
                if (CheckConnect())
                {
                    label15.Text = string.Format("狀態：{0}", "等待使用者指令...");
                }
                else
                {
                    label15.ForeColor = Color.FromArgb(255, 46, 46);
                    label15.Text = string.Format("狀態：{0}", "無法連線至伺服器，請檢查網路連線是否一切正常。");
                }
                ischecked = true;
            }
            textBox1_TextChanged(this, null);
            checkBox1_CheckedChanged(this, null);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.IndexOfAny(Path.GetInvalidPathChars()) != -1)
            {
                textBox1.ForeColor = Color.FromArgb(255, 46, 46);
                label16.ForeColor = Color.FromArgb(255, 46, 46);
                label16.Text = "- 路徑格式錯誤";
                startBtn.Enabled = false;
            }
            else
            {
                if (Path.IsPathRooted(textBox1.Text))
                {
                    try
                    {
                        DriveInfo di = new DriveInfo(Path.GetFullPath(textBox1.Text).Substring(0, 3));
                        if (di.IsReady)
                        {
                            textBox1.Text = Path.GetFullPath(textBox1.Text);
                            textBox1.ForeColor = Color.DimGray;
                            label16.ForeColor = Color.DimGray;
                            label16.Text = string.Format("- 可用空間：{0}", Math.Round(((double)di.AvailableFreeSpace / 1024 / 1024 / 1024), 2).ToString() + " GB");
                            if (!isrunning)
                            {
                                startBtn.Enabled = true;
                            }
                        }
                        else
                        {
                            textBox1.ForeColor = Color.FromArgb(255, 46, 46);
                            label16.ForeColor = Color.FromArgb(255, 46, 46);
                            label16.Text = "- 無法使用該磁碟機";
                            startBtn.Enabled = false;
                        }
                    }
                    catch
                    {
                        textBox1.ForeColor = Color.FromArgb(255, 46, 46);
                        label16.ForeColor = Color.FromArgb(255, 46, 46);
                        label16.Text = "- 路徑格式錯誤";
                        startBtn.Enabled = false;
                    }
                }
                else
                {
                    textBox1.ForeColor = Color.FromArgb(255, 46, 46);
                    label16.ForeColor = Color.FromArgb(255, 46, 46);
                    label16.Text = "- 路徑格式錯誤";
                    startBtn.Enabled = false;
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                label17.Text = string.Format("所需空間：{0} MB", "23");
            }
            else
            {
                label17.Text = string.Format("所需空間：{0} MB", "15");
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            button1.Click -= button1_Click;
            button1.Click += (s, e1) => { tabControl1.SelectedIndex = 2; };
            tabControl1.SelectedIndex = 1;

        }

        int temp;
        private void startBtn_Click(object sender, EventArgs e)
        {
            if(checkBox2.Checked)
            {
                if (!isalldone)
                {
                    if (checkBox1.Checked)
                    {
                        Array.Resize(ref lack, lack.Length + 1);
                        lack[lack.Length - 1] = "Youtube-Dl";
                    }
                    isrunning = true;
                    startBtn.Enabled = false;
                    panel3.Enabled = false;
                    checkBox1.Enabled = false;
                    checkBox2.Enabled = false;
                    button4.Visible = false;
                    close.Enabled = false;
                    linkLabel2.Enabled = false;
                    if (!Directory.Exists(textBox1.Text))
                        Directory.CreateDirectory(textBox1.Text);
                    progressBar_inside = new Panel();
                    progressBar_inside.Parent = progressBar_outside;
                    progressBar_inside.BackColor = Color.DodgerBlue;
                    progressBar_inside.Height = progressBar_outside.Height;
                    progressBar_inside.Location = new Point(0, 0);
                    progressBar_inside.Width = 0;
                    temp = 0;
                    counts = 0;
                    wb = new WebClient();
                    wb.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadFileProcess);
                    wb.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                    dodownload();
                }
                else
                {
                    tabControl1.SelectedIndex = 3;
                }
            }
            else
            {
                MessageBox.Show("請同意服務條款內容後再安裝。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DownloadFileProcess(object sender, DownloadProgressChangedEventArgs e)
        {
            label15.Text = string.Format("狀態：正在下載{0} {1}% - 進度 {2}/{3} KB", downloadingItems, e.ProgressPercentage, Math.Round((double)(e.BytesReceived / 1024), 2).ToString(), Math.Round((double)(e.TotalBytesToReceive / 1024)).ToString());
            progressBar_inside.Width = temp + progressBar_outside.Width / lack.Length * e.ProgressPercentage / 100;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                close.Enabled = true;
                haswrong = true;
                isrunning = false;
                errorInfo.Text = e.Error.ToString();
                label1.ForeColor = Color.FromArgb(255, 46, 46);
                BackColor = Color.FromArgb(255, 46, 46);
                tabControl1.SelectedIndex = 7;
            }
            else if(!haswrong)
            {
                temp = progressBar_inside.Width;
                counts++;
                if (counts < lack.Length) dodownload();
                else
                {
                    counts = lack.Length + 1;
                    progressBar_inside.Width = progressBar_outside.Width;
                    label15.Text = "狀態：正在複製必要的檔案...";
                    if (System.IO.File.Exists(Path.Combine(Path.GetFullPath(textBox1.Text), "Uninstaller.exe")))
                        System.IO.File.Delete(Path.Combine(Path.GetFullPath(textBox1.Text), "Uninstaller.exe"));
                    System.IO.File.Copy(Process.GetCurrentProcess().MainModule.FileName, Path.Combine(Path.GetFullPath(textBox1.Text), "Uninstaller.exe"));
                    label15.Text = "狀態：正在登陸機碼...";
                    WriteRegedit("DisplayIcon", Path.Combine(Path.GetFullPath(textBox1.Text), "LRCMaker.exe"), RegistryValueKind.String);
                    WriteRegedit("DisplayName", "LRC歌詞檔案製作工具", RegistryValueKind.String);
                    WriteRegedit("DisplayVersion", ver, RegistryValueKind.String);
                    WriteRegedit("Publisher", "Visual Studio 學習筆記", RegistryValueKind.String);
                    WriteRegedit("InstallLocation", Path.GetFullPath(textBox1.Text), RegistryValueKind.String);
                    WriteRegedit("UninstallString", Path.Combine(Path.GetFullPath(textBox1.Text), "Uninstaller.exe"), RegistryValueKind.String);
                    WriteRegedit("RepairPath", Path.Combine(Path.GetFullPath(textBox1.Text), "Uninstaller.exe"), RegistryValueKind.String);
                    WriteRegedit("ModifyPath", Path.Combine(Path.GetFullPath(textBox1.Text), "Uninstaller.exe"), RegistryValueKind.String);
                    WriteRegedit("URLInfoAbout", "http://ivsnote.com/", RegistryValueKind.String);
                    WriteRegedit("EstimatedSize", "23000", RegistryValueKind.DWord);
                    try
                    {
                        label15.Text = "狀態：正在建立捷徑...";
                        CreateShortcut("LRC歌詞檔案製作工具", Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), Path.Combine(Path.GetFullPath(textBox1.Text), "LRCMaker.exe"));
                        CreateShortcut("LRC歌詞檔案製作工具", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs"), Path.Combine(Path.GetFullPath(textBox1.Text), "LRCMaker.exe"));
                        label15.Text = "狀態：安裝完成！";
                    }
                    catch (Exception ex)
                    {
                        label15.ForeColor = Color.FromArgb(255, 46, 46);
                        label15.Text = string.Format("狀態：安裝完成但無法建立捷徑，原因：{0}", ex.Message);
                    }
                    isalldone = true;
                    startBtn.Enabled = true;
                    close.Enabled = true;
                    startBtn.Text = "完成";
                    isrunning = false;
                }
            }
        }

        void dodownload()
        {
            try
            {
                switch (lack[counts])
                {
                    case "LRCMaker":
                        wb.DownloadFileAsync(new Uri("https://sky880319.github.io/lrcmaker/LRCMaker.exe"), Path.Combine(Path.GetFullPath(textBox1.Text),"LRCMaker.exe"));
                        wb.Dispose();
                        break;
                    case "BassNetDLL":
                        wb.DownloadFileAsync(new Uri("https://sky880319.github.io/lrcmaker/Bass.Net.dll"), Path.Combine(Path.GetFullPath(textBox1.Text), "Bass.Net.dll"));
                        wb.Dispose();
                        break;
                    case "BassDLL":
                        wb.DownloadFileAsync(new Uri("https://sky880319.github.io/lrcmaker/bass.dll"), Path.Combine(Path.GetFullPath(textBox1.Text), "bass.dll"));
                        wb.Dispose();
                        break;
                    case "Updater":
                        wb.DownloadFileAsync(new Uri("https://sky880319.github.io/lrcmaker/LRCMAKER_UPDATER.exe"), Path.Combine(Path.GetFullPath(textBox1.Text), "LRCMAKER_UPDATER.exe"));
                        wb.Dispose();
                        break;
                    case "UpdateUnZipModule":
                        wb.DownloadFileAsync(new Uri("https://sky880319.github.io/lrcmaker/ICSharpCode.SharpZipLib.dll"), Path.Combine(Path.GetFullPath(textBox1.Text), "ICSharpCode.SharpZipLib.dll"));
                        wb.Dispose();
                        break;
                    case "Youtube-Dl":
                        wb.DownloadFileAsync(new Uri("https://sky880319.github.io/lrcmaker/youtube-dl.exe"), Path.Combine(Path.GetFullPath(textBox1.Text), "youtube-dl.exe"));
                        break;
                }
                downloadingItems = lack_Name[counts];
            }
            catch (WebException ex)
            {
                close.Enabled = true;
                haswrong = true;
                isrunning = false;
                errorInfo.Text = ex.ToString();
                label1.ForeColor = Color.FromArgb(255, 46, 46);
                BackColor = Color.FromArgb(255, 46, 46);
                tabControl1.SelectedIndex = 7;
            }
        }

        public void CreateShortcut(string Name, string FolderPath, string FilePath)

        {
            WshShellClass wshShell = new WshShellClass();
            IWshRuntimeLibrary.IWshShortcut shortcut;
            // Create the shortcut
            shortcut = (IWshRuntimeLibrary.IWshShortcut)wshShell.CreateShortcut(FolderPath + "\\" + Name + ".lnk");
            shortcut.TargetPath = FilePath;
            shortcut.WorkingDirectory = Path.GetDirectoryName(FilePath);
            shortcut.Description = "啟動" + Name;
            shortcut.Save();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Process proc = new Process();
            proc.StartInfo.WorkingDirectory = Path.GetFullPath(textBox1.Text);
            proc.StartInfo.FileName = Path.Combine(Path.GetFullPath(textBox1.Text), "LRCMaker.exe");
            proc.Start();
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            textBox1.Text = fbd.SelectedPath;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ver_Date = ver.Substring(ver.LastIndexOf('.') - 4, 4);
            label4.Text = string.Format(label4.Text, ver);
            label5.Text = string.Format(label5.Text, "20" + ver_Date.Substring(0, 2), ver_Date.Substring(2, 2));
        }

        private void uninstallBtn_Click(object sender, EventArgs e)
        {
            textBox2.Text = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\LRCMaker", "InstallLocation", "null").ToString();
            tabControl1.SelectedIndex = 5;
            isalldone = false;
        }

        private void repairBtn_Click(object sender, EventArgs e)
        {
            startBtn.Text = "修復";
            panel3.Enabled = false;
            checkBox2.Checked = true;
            checkBox2.Enabled = false;
            button4.Click -= button4_Click;
            button4.Click += (s, e1) => { tabControl1.SelectedIndex = 4; };
            linkLabel2.Enabled = false;
            label12.Text = "正在修復...";
            textBox1.Text = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\LRCMaker", "InstallLocation", "null").ToString();
            tabControl1.SelectedIndex = 2;
        }

        private void startUninstallBtn_Click(object sender, EventArgs e)
        {
            if (!isalldone)
            {
                startUninstallBtn.Enabled = false;
                checkBox4.Enabled = false;
                close.Enabled = false;

                uninstallProgressBar_inside = new Panel();
                uninstallProgressBar_inside.Parent = uninstallProgressBar_outside;
                uninstallProgressBar_inside.BackColor = Color.FromArgb(255, 46, 46);
                uninstallProgressBar_inside.Height = uninstallProgressBar_outside.Height;
                uninstallProgressBar_inside.Location = new Point(0, 0);
                uninstallProgressBar_inside.Width = 0;

                t1 = new Timer();
                t1.Interval = 10;
                t1.Tick += new EventHandler(t1_Tick);
                t1.Start();

                try
                {
                    label27.Text = string.Format("狀態：{0}", "正在關閉應用程式...");
                    stopApp("LRCMaker");
                    stopApp("youtube-dl");
                    progress = (int)(uninstallProgressBar_outside.Width * 0.1);
                    System.Threading.Thread t1 = new System.Threading.Thread(sleep);
                    t1.Start();
                    t1.Join();
                    label27.Text = string.Format("狀態：{0}", "正在移除程式...");
                    string[] filelist = { "LRCMaker.exe", "Bass.Net.dll", "bass.dll", "LRCMAKER_UPDATER.exe", "ICSharpCode.SharpZipLib.dll", "youtube-dl.exe" };
                    foreach (string filename in filelist)
                    {
                        string path = Path.Combine(Path.GetFullPath(textBox2.Text), filename);
                        if (System.IO.File.Exists(path))
                            System.IO.File.Delete(path);
                    }
                    progress = (int)(uninstallProgressBar_outside.Width * 0.5);
                    string folder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    if (System.IO.File.Exists(Path.Combine(folder, "LRC歌詞檔案製作工具.lnk")))
                        System.IO.File.Delete(Path.Combine(folder, "LRC歌詞檔案製作工具.lnk"));
                    folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs");
                    if (System.IO.File.Exists(Path.Combine(folder, "LRC歌詞檔案製作工具.lnk")))
                        System.IO.File.Delete(Path.Combine(folder, "LRC歌詞檔案製作工具.lnk"));
                    progress = (int)(uninstallProgressBar_outside.Width * 0.7);
                    if (!checkBox4.Checked)
                    {
                        label27.Text = string.Format("狀態：{0}", "正在移除設定...");
                        if (Directory.Exists(appDataFolder))
                        {
                            foreach (string filename in Directory.GetFiles(appDataFolder, "*"))
                            {
                                string path = Path.Combine(appDataFolder, filename);
                                if (System.IO.File.Exists(path))
                                    System.IO.File.Delete(path);
                            }
                            Directory.Delete(appDataFolder);
                        }
                        foreach (string filename in Directory.GetFiles(Path.GetFullPath(textBox2.Text), "*.tmp"))
                        {
                            string path = Path.Combine(Path.GetFullPath(textBox2.Text), filename);
                            if (System.IO.File.Exists(path))
                                System.IO.File.Delete(path);
                        }
                        foreach (string filename in Directory.GetFiles(Path.GetFullPath(textBox2.Text), "like*"))
                        {
                            string path = Path.Combine(Path.GetFullPath(textBox2.Text), filename);
                            if (System.IO.File.Exists(path))
                                System.IO.File.Delete(path);
                        }
                        foreach (string filename in Directory.GetFiles(Path.GetFullPath(textBox2.Text), "*.ini"))
                        {
                            string path = Path.Combine(Path.GetFullPath(textBox2.Text), filename);
                            if (System.IO.File.Exists(path))
                                System.IO.File.Delete(path);
                        }
                    }
                    progress = (int)(uninstallProgressBar_outside.Width * 0.9);
                    label27.Text = string.Format("狀態：{0}", "正在移除機碼...");
                    Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", true).DeleteSubKey("LRCMaker");
                    progress = uninstallProgressBar_outside.Width;
                    label27.Text = string.Format("狀態：{0}", "完成...");
                    isalldone = true;
                }
                catch (Exception ex)
                {
                    label27.Text = string.Format("狀態：{0}", "未能完整移除程式，原因：" + ex.Message);
                    startUninstallBtn.Text = "重試";
                }
                startUninstallBtn.Enabled = true;
                close.Enabled = true;
                startUninstallBtn.Text = "完成";
                isrunning = false;
            }
            else
            {
                tabControl1.SelectedIndex = 6;
            }
        }

        private void sleep()
        {
            System.Threading.Thread.Sleep(2000);
            Application.DoEvents();
        }

        private void t1_Tick(object s, EventArgs e)
        {
            if (uninstallProgressBar_inside.Width < progress)
            {
                if ((progress - uninstallProgressBar_inside.Width) < 50)
                    uninstallProgressBar_inside.Width += 5;
                else if ((progress - uninstallProgressBar_inside.Width) < 100)
                    uninstallProgressBar_inside.Width += 10;
                else if ((progress - uninstallProgressBar_inside.Width) < 300)
                    uninstallProgressBar_inside.Width += 30;
                else if ((progress - uninstallProgressBar_inside.Width) >= 300)
                    uninstallProgressBar_inside.Width += 50;
                else
                    uninstallProgressBar_inside.Width += 1;
            }
        }

        private void stopApp(string appName)
        {
            foreach (Process p in Process.GetProcessesByName(appName))
            {
                p.Kill();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isrunning)
            {
                MessageBox.Show("安裝程式正在進行中，強制關閉可能造成檔案毀損。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
            else
            {
                Application.Exit();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 4;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/c @sleep 5 & @del/f \"" + Path.Combine(Path.GetFullPath(textBox1.Text), "Uninstaller.exe") + "\"";
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            Application.Exit();
        }

        private void repairBtn_MouseEnter(object sender, EventArgs e)
        {
            label26.Text = string.Format("說明：\n{0}", "當您的 LRC歌詞檔案製作工具 發生問題時，您可以透過這項功能重新安裝。");
        }

        private void uninstallBtn_MouseEnter(object sender, EventArgs e)
        {
            label26.Text = string.Format("說明：\n{0}", "如果您再也用不到 LRC歌詞檔案檔案製作工具 了，可以透過這項功能解除安裝。（但很危險不要按！）");
        }

        private void button7_MouseEnter(object sender, EventArgs e)
        {
            label26.Text = string.Format("說明：\n{0}", "不想解除安裝了就按這裡。:)");
        }

        private void leave(object sender, EventArgs e)
        {
            label26.Text = string.Format("說明：\n{0}", "");
        }

        private void label40_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void WriteRegedit(string name, string value, RegistryValueKind kind)
        {
            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\LRCMaker", true).SetValue(name, value, kind);
        }

        //protected override void WndProc(ref Message m)
        //{
        //    const int WM_SYSCOMMAND = 0x0112;
        //    const int SC_CLOSE = 0xF060;
        //    if (m.Msg == WM_SYSCOMMAND && (int)m.WParam == SC_CLOSE)
        //    {
                
        //    }
        //    base.WndProc(ref m);
        //}
    }

    public static class RichTextBoxExtensions
    {
        public static void SetInnerMargins(this TextBoxBase textBox, int left, int top, int right, int bottom)
        {
            var rect = textBox.GetFormattingRect();

            var newRect = new Rectangle(left, top, rect.Width - left - right, rect.Height - top - bottom);
            textBox.SetFormattingRect(newRect);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public readonly int Left;
            public readonly int Top;
            public readonly int Right;
            public readonly int Bottom;

            private RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom)
            {
            }
        }

        [DllImport(@"User32.dll", EntryPoint = @"SendMessage", CharSet = CharSet.Auto)]
        private static extern int SendMessageRefRect(IntPtr hWnd, uint msg, int wParam, ref RECT rect);

        [DllImport(@"user32.dll", EntryPoint = @"SendMessage", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, ref Rectangle lParam);

        private const int EmGetrect = 0xB2;
        private const int EmSetrect = 0xB3;

        private static void SetFormattingRect(this TextBoxBase textbox, Rectangle rect)
        {
            var rc = new RECT(rect);
            SendMessageRefRect(textbox.Handle, EmSetrect, 0, ref rc);
        }

        private static Rectangle GetFormattingRect(this TextBoxBase textbox)
        {
            var rect = new Rectangle();
            SendMessage(textbox.Handle, EmGetrect, (IntPtr)0, ref rect);
            return rect;
        }
    }
}
