using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Net;
using System.Diagnostics;
using Un4seen.Bass;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;
using System.Net.Http;

namespace Lrc_Maker
{
    public partial class Form1 : Form
    {
        //開工前
        //1.Youtube-dl檢查新版本。
        //.......................
        private Point startPoint;
        const int CS_DropSHADOW = 0x20000;
        const int GCL_STYLE = (-26);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex);

        int oldtrackbarvalue = 0;
        public bool first = true;
        public bool timerX = false;
        string LRCFIleName = "";
        string updated = "";
        string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LRCMaker");
        int x1, x2, h1, h2;
        bool ifplaylrc = false;
        bool ifsave = true;
        bool isreprot = false;
        bool isvip = false;
        bool istipshow = false;
        bool isfirsttipshow = false;
        bool isfocustipshow = false;
        bool loadFromFile = false;
        string txtfile;

        //Player Info
        int stream = 0;
        BASSActive playstatus;
        float freq;
        float curfreq;
        bool stoped = false;

        int marqueecount = 0;

        string[] quickkey = { "S", "W", "X", "R", "A", "D" };

        Donate donate;
        ToolTip ttt;

        string[] pluginFileName = { "LRCMAKER_UPDATER.exe", "youtube-dl.exe" };
        string[] pluginMD5 = { "0B763D6ECFD17684D9B93208E9F2361C", "3A246085053C3216FA4A342DE187EBA1" };
        string[] pluginName = { "Updater", "Youtube-Dl" };

        /////////////////////////////////////////////////////////////////////////////////////
        public const string ver = "11.6.1911.17"; // 版本資訊，發佈新版之前請修改此行。
        const string revisenum = "1";
        const string dat = "2019/11/17";
        public const string updaterver = "2.0";
        /////////////////////////////////////////////////////////////////////////////////////
        bool iftestver = false;
        bool iftestwel = false;
        bool donateavailable = !false;
        //bool ifnewver = false;
        string[] note = { };
        bool dontupdate = false, getfail = false, connectfail = false;
        string httxt;

        public Form1()
        {
            InitializeComponent();
            
            //SetClassLong(this.Handle, GCL_STYLE, GetClassLong(this.Handle, GCL_STYLE) | CS_DropSHADOW); //API函数加载，实现窗体边框阴影效果
            //Bass.Net

        }

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000;
        //        return cp;
        //    }
        //}

        public Form1(string fucdef)
        {
            updated = fucdef;
            //ifnewver = ifnewx;
            //httxt = newver;
            InitializeComponent();
        }

        ///////////////////////////////////////////////////////////// T h e m e _ S e t t i n g /////////////////////////////////////////////////////////////
        void Theme_IVSnote()
        {
            title.ForeColor = Color.FromArgb(255, 75, 75);
            slider1.Color = title.ForeColor;
        }

        void Theme_Black()
        {
            title.ForeColor = Color.FromArgb(128,128,128);
            slider1.Color = title.ForeColor;
        }

        void Theme_Blues()
        {
            title.ForeColor = Color.FromArgb(12, 132, 191);
            slider1.Color = title.ForeColor;
        }

        void Theme_Brown()
        {
            title.ForeColor = Color.FromArgb(174, 91, 91);
            slider1.Color = title.ForeColor;
        }

        void Theme_Pink()
        {
            title.ForeColor = Color.FromArgb(255, 46, 46);
            slider1.Color = title.ForeColor;
        }

        void Theme_Green()
        {
            title.ForeColor = Color.FromArgb(147, 147, 72);
            slider1.Color = title.ForeColor;
        }

        void Theme_Custom(Color color)
        {
            title.ForeColor = color;
            slider1.Color = title.ForeColor;
        }

        bool IsRgb(string str, ref Color color)
        {
            if (str != null)
            {
                string[] rgb = str.Split(',');
                if (rgb.Length == 3)
                {
                    try
                    {
                        int r = int.Parse(rgb[0]),
                            g = int.Parse(rgb[1]),
                            b = int.Parse(rgb[2]);
                        if (r <= 255 && r >= 0 && g <= 255 && g >= 0 && b <= 255 && b >= 0)
                        {
                            color = Color.FromArgb(r, g, b);
                            return true;
                        }
                        else return false;
                    }
                    catch { return false; }
                }
                else return false;
            }
            else return false;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        void Button_Enabled()
        {
            changetolrc.Enabled = true;
            undo.Enabled = true;
            button11.Enabled = true;
            paragraph.Enabled = true;
        }
        void Button_Enabled_False()
        {
            changetolrc.Enabled = false;
            undo.Enabled = false;
            button11.Enabled = false;
            paragraph.Enabled = false;
        }



        public static bool revise(string vers)
        {
            try
            {
                int i = Convert.ToInt32(vers.Substring(vers.Length - 1));
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            //string[] lack = new string[0];
            //int lackcount = 0;
            //for (int i = 0; i < pluginName.Length; i++)
            //{
            //    string checkFile = Path.Combine(Application.StartupPath, pluginFileName[i]);
            //    if (File.Exists(checkFile))
            //    {
            //        if (GetMD5HashFromFile(checkFile).ToUpper() != pluginMD5[i])
            //        {
            //            lackcount++;
            //            Array.Resize(ref lack, lackcount);
            //            lack[lackcount - 1] = pluginName[i];
            //        }
            //    }
            //}
            //if (lackcount > 0)
            //{
            //    UpdateManager um = new UpdateManager(lack);
            //    um.ShowInTaskbar = false;
            //    um.button1.Enabled = false;
            //    um.ShowDialog();
            //}

            CheckInfo.RunWorkerAsync();
            //滿意度調查
            if (!File.Exists(appDataFolder + "\\survey_19.tmp"))
            {
                using (StreamWriter stw = new StreamWriter(appDataFolder + "\\survey_19.tmp"))
                {
                    stw.Write(0);
                    stw.Close();
                }
                isfirsttipshow = true;
            }
            else
            {
                int usecount = 0;
                StreamReader str = new StreamReader(appDataFolder + "\\survey_19.tmp");
                usecount = int.Parse(str.ReadLine());
                str.Close();
                if (usecount >= 0)
                {
                    using (StreamWriter stw = new StreamWriter(appDataFolder + "\\survey_19.tmp"))
                    {
                        stw.Write(usecount + 1);
                        stw.Close();
                    }
                    if (usecount > 2)
                    {
                        survey s_fm = new survey();
                        s_fm.ShowDialog();
                    }
                }
            }

            //提示用戶按讚
            if (revise(ver))
            {
                //if (File.Exists(@"liked" + ver))
                //{
                //    like like = new like();
                //    like.ShowDialog();
                //    if (like.done)
                //    {
                //        File.Delete(@"liked" + ver);
                //    }
                //}
            }
            else
            {
                nowver.Text += ", 修訂版本號：" + revisenum;
            }

            if (isreprot)
            {
                Repair repair = new Repair();
                repair.ShowDialog();
            }
            //if (!isvip)
            //{
            //    if (donateavailable)
            //    {
            //        button15_Click(this, null);
            //    }
            //    else
            //    {
            //        donatebtn.Enabled = false;
            //    }
            //}
            this.Focus();
            firsttip(true);
            listBox2.Focus();
        }

        void firsttip(bool useflash)
        {
            if (isfirsttipshow)
            {
                ttt = new ToolTip() { IsBalloon = true, ToolTipIcon = ToolTipIcon.Info, ToolTipTitle = "歡迎使用本軟體", ShowAlways = true, UseFading = useflash, UseAnimation = useflash };
                ttt.Show("若要取得功能詳細說明，請點這裡。\n如果需要提示，請將滑鼠指向按鈕，片刻後，會有提示出現。", panel2, about.Left + about.Width / 2 - 17, about.Top - 80);
            }
        }

        bool ConnEx()
        {
            try
            {
                System.Net.Sockets.TcpClient clint = new System.Net.Sockets.TcpClient("sky880319.github.io", 80);
                clint.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected string GetMD5HashFromFile(string fileName)
        {
            FileStream file = new FileStream(fileName, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }

        private void CheckInfo_DoWork(object sender, DoWorkEventArgs e)
        {

            if (ConnEx())
            {
                MyWebClient wc = new MyWebClient();
                wc.Encoding = Encoding.UTF8;
                note = wc.DownloadString("https://sky880319.github.io/lrcmaker/post.txt").Split('&');
                wc.Dispose();
                
                if (isvip)
                {
                    //DWHTTP("http://stockoko.idv.tw/vsnote/lrcmaker/Developer/updateinfo.html", 1);
                }
                else
                {
                    if (!iftestver)
                    {
                        DWHTTP("https://sky880319.github.io/lrcmaker/updateinfo.html", 1);
                    }
                }
            }
            else
            {
                connectfail = true;
                CheckInfo.CancelAsync();
                CheckInfo.Dispose();
            }
        }

        private int DWHTTP(string url, int retry)
        {
            if (retry < 0)
            {
                getfail = true;
                return 1;
            }
            else
            {
                try
                {
                    MyWebClient wbc = new MyWebClient();
                    httxt = wbc.DownloadString(url);
                    if (httxt != ver)
                    {
                        if (isvip)
                        {
                            DialogResult updateinfo = MessageBox.Show("有新的測試版本可用，要前往更新嗎？\n\n" + "當前版本：" + ver + "\n最新版本：" + httxt + "\n此訊息只有開發者才可以看到", "消息", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (updateinfo == DialogResult.OK)
                            {
                                Process.Start("DEV_UPDATER.exe");
                                Application.Exit();
                            }
                            else
                            {
                                dontupdate = true;
                            }
                        }
                        else
                        {
                            DialogResult updateinfo = MessageBox.Show("有新版本可用，要前往更新嗎？\n\n" + "當前版本：" + ver + "\n最新版本：" + httxt, "消息", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (updateinfo == DialogResult.OK)
                            {
                                Process.Start("LRCMAKER_UPDATER.exe");
                                Application.Exit();
                            }
                            else
                            {
                                dontupdate = true;
                            }
                        }
                    }
                    wbc.Dispose();
                }
                catch
                {
                    return DWHTTP(url, retry--);
                }
            }
            return 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] lack = { }, dlUrl = { }, name = { };
            int lackcount = 0;

            nowver.Text = "當前版本：" + ver;
            ifnew.Text = "沒有新的版本可供使用";
            verdate.Text = "版本日期：" + dat;

            BassNet.Registration("sky880319@gmail.com", "2X28201319152222");
            Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, this.Handle);
            float vol = 1;
            Bass.BASS_ChannelGetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, ref vol);
            slider2.Value = (int)vol * 100;

            //Check Update
            if (updated == "offline")
            {
                iftestver = true;
                webBrowser1.Visible = false;
                about.Width = 154;
                pictureBox9.Visible = false;
                ifnew.Text = "使用者已定義狀態為離線使用";
            }
            if (updated == "dev")
            {
                isvip = true;
                developerbtn.Visible = true;
                if (!File.Exists(Application.StartupPath + @"\DEV_UPDATER.exe"))
                {
                    lackcount++;
                    Array.Resize(ref lack, lackcount);
                    Array.Resize(ref dlUrl, lackcount);
                    Array.Resize(ref name, lackcount);
                    lack[lackcount - 1] = "DeveloperModule";
                    dlUrl[lackcount - 1] = "https://sky880319.github.io/lrcmaker/DEV_UPDATER.exe";
                    name[lackcount - 1] = "DEV_UPDATER.exe";
                }
            }

            //檢查缺少組件
            if (revise(ver))
            {
                if (!File.Exists(appDataFolder + "\\Welcome_LRCMAKER_" + ver + ".tmp"))
                {
                    StreamWriter SW = new StreamWriter(appDataFolder + "\\Welcome_LRCMAKER_" + ver + ".tmp");
                    SW.Close();
                    WELCOME wel = new WELCOME();
                    wel.ShowDialog();
                }
                if (iftestwel)
                {
                    WELCOME wel = new WELCOME();
                    wel.ShowDialog();
                }
            }
            if (listBox1.Items.Count == 0)
            {
                Button_Enabled_False();
            }

            //初始各項設定
            if (slider2.Value == 0)
            {
                pictureBox7.BackgroundImage = ProgramImg.nvp1;
            }
            else
            {
                pictureBox7.BackgroundImage = ProgramImg.vp1;
            }
            x1 = listBox1.Top; x2 = listBox2.Top; h1 = listBox1.Height; h2 = listBox2.Height;
            comboBox1.SelectedIndex = 0;

            //檢查主題文件
            if (File.Exists(Application.StartupPath + "\\SETTING_LRCMAKER_THEME.ini"))
            {
                if (!File.Exists(appDataFolder + "\\SETTING_LRCMAKER_THEME.ini"))
                {
                    try
                    {
                        File.Copy(Application.StartupPath + "\\SETTING_LRCMAKER_THEME.ini", appDataFolder + "\\SETTING_LRCMAKER_THEME.ini");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("無法移動設定檔，原因：\n" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            if (!File.Exists(appDataFolder + "\\SETTING_LRCMAKER_THEME.ini"))
            {
                using (StreamWriter stw = new StreamWriter(appDataFolder + "\\SETTING_LRCMAKER_THEME.ini"))
                {
                    stw.Close();
                    Theme_Blues();
                }
            }
            else
            {
                string themeset = "";
                using (StreamReader str = new StreamReader(appDataFolder + "\\SETTING_LRCMAKER_THEME.ini"))
                {
                    themeset = str.ReadLine();
                    str.Close();
                }
                Color color = Color.Black;
                if (themeset == "Theme=Black") Theme_Black();
                else if (themeset == "Theme=Brown") Theme_Brown();
                else if (themeset == "Theme=Pink") Theme_Pink();
                else if (themeset == "Theme=Green") Theme_Green();
                else if (themeset == "Theme=Blues") Theme_Blues();
                else if (IsRgb(themeset, ref color)) Theme_Custom(color);
                else Theme_IVSnote();
            }

            //載入預設編碼
            if (!File.Exists(appDataFolder + "\\SETTING_LRCMAKER_ENCODING.ini"))
            {
                using (StreamWriter stw = new StreamWriter(appDataFolder + "\\SETTING_LRCMAKER_ENCODING.ini"))
                {
                    stw.Write(0);
                    stw.Close();
                }
            }
            else
            {
                int encoding = 0;
                using (StreamReader str = new StreamReader(appDataFolder + "\\SETTING_LRCMAKER_ENCODING.ini"))
                {
                    try
                    {
                        encoding = int.Parse(str.ReadLine());
                    }
                    catch
                    {
                        encoding = 0;
                    }
                    str.Close();
                }
                comboBox1.SelectedIndex = encoding;
            }

            //單組件更新
            if (lackcount > 0)
            {
                UpdateManager um = new UpdateManager(lack, dlUrl, name);
                um.ShowDialog();
            }

            //載入快速鍵訊息
            if (File.Exists(Application.StartupPath + "\\SETTING_LRCMAKER_SHORTCUT.ini"))
            {
                if (!File.Exists(appDataFolder + "\\SETTING_LRCMAKER_SHORTCUT.ini"))
                {
                    try
                    {
                        File.Copy(Application.StartupPath + "\\SETTING_LRCMAKER_SHORTCUT.ini", appDataFolder + "\\SETTING_LRCMAKER_SHORTCUT.ini");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("無法移動設定檔，原因：\n" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            if (!File.Exists(appDataFolder + "\\SETTING_LRCMAKER_SHORTCUT.ini"))
            {
                string shortcutstr = "";
                for (int i = 0; i < quickkey.Length; i++)
                {
                    shortcutstr += quickkey[i];
                    if (i < quickkey.Length - 1)
                    {
                        shortcutstr += Environment.NewLine;
                    }
                }
                using (StreamWriter stw = new StreamWriter(appDataFolder + "\\SETTING_LRCMAKER_SHORTCUT.ini"))
                {
                    stw.Write(shortcutstr);
                    stw.Close();
                }
            }
            else
            {
                StreamReader str = new StreamReader(appDataFolder + "\\SETTING_LRCMAKER_SHORTCUT.ini", Encoding.Default, true);
                int count = 0;
                string a = "";
                string[] t = new string[6];
                bool fine = false;
                try
                {
                    while (true)
                    {
                        a = str.ReadLine();
                        if (a == null)
                        {
                            fine = true;
                            break;
                        }
                        if (count < 6)
                            t[count] = a;
                        else
                        {
                            fine = false;
                            //MessageBox.Show("1");
                            break;
                        }
                        count++;
                    }
                    if (fine)
                    {
                        for (int i = 0; i < t.Length; i++)
                        {
                            if (fine)
                            {
                                for (int j = i + 1; j < t.Length; j++)
                                {
                                    if (t[i] == t[j])
                                    {
                                        fine = false;
                                        //MessageBox.Show("2");
                                        break;
                                    }
                                    else
                                    {
                                        fine = true;
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                    fine = false;
                }
                str.Close();
                if (fine)
                {
                    quickkey = t;
                }
                else
                {
                    File.Delete(appDataFolder + "\\SETTING_LRCMAKER_SHORTCUT.ini");
                }
            }

            //紀錄使用次數
            if (!File.Exists(appDataFolder + "\\USED_COUNT.tmp"))
            {
                using (StreamWriter stw = new StreamWriter(appDataFolder + "\\USED_COUNT.tmp"))
                {
                    stw.Write(1);
                    stw.Close();
                }
            }
            else
            {
                int usecount = 0;
                StreamReader str = new StreamReader(appDataFolder + "\\USED_COUNT.tmp");
                usecount = int.Parse(str.ReadLine());
                str.Close();
                using (StreamWriter stw = new StreamWriter(appDataFolder + "\\USED_COUNT.tmp"))
                {
                    stw.Write(usecount + 1);
                    stw.Close();
                }
            }

            listBox2.Focus();
        }

        public System.String currentPositionString { get; set; }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count <= 0)
                Button_Enabled_False();
            Form2 fm2 = new Form2("歌詞", "Mojim.com", ConnEx());
            fm2.ShowDialog();
            if (fm2.selected == 1)
            {
                OpenFileDialog openfile = new OpenFileDialog
                {
                    Title = "[Lrc Maker] 開啟檔案",
                    InitialDirectory = @"C:\",
                    RestoreDirectory = true,
                    Multiselect = false,
                    Filter = "文字檔|*.txt|動態歌詞檔|*.lrc"
                };
                openfile.ShowDialog();
                if (openfile.FileName == "") return;
                if (listBox1.Items.Count > 0 || listBox2.Items.Count > 0)
                {
                    DialogResult dr = MessageBox.Show("接下來要清除所有工作區資料哦，你確定嗎？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                    if (dr == DialogResult.No) return;
                }
                loadFromFile = true;
                txtfile = openfile.FileName;
                StreamReader str = new StreamReader(txtfile, encodings(), true);
                string read = "", a;
                while (true)
                {
                    a = str.ReadLine();
                    read += a;
                    if (a == null) break;
                    else read += "\n";
                }
                str.Close();
                readTextFile(read);
            }
            else if (fm2.selected == 2)
            {
                Search s = new Search();
                s.ShowDialog();
                if (!s.cancel)
                {
                    if (listBox1.Items.Count > 0 || listBox2.Items.Count > 0)
                    {
                        DialogResult dr = MessageBox.Show("接下來要清除所有工作區資料哦，你確定嗎？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                        if (dr == DialogResult.No) return;
                    }
                    listBox1.Items.Clear();
                    listBox2.Items.Clear();
                    listBox1.Items.AddRange(s.richTextBox1.Lines);
                    loadFromFile = false;
                    Button_Enabled();
                    try
                    {
                        listBox1.SelectedIndex = 0;
                    }
                    catch
                    {
                        Button_Enabled_False();
                    }
                    ifsave = false;
                }
            }
            if (fm2.selected != 0)
            {
                listBox1.Refresh();
                listBox2.Refresh();
                Button_Enabled();
            }
            listBox2.Focus();
        }

        private void readTextFile(string read)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            string pattern = @"(\[[\d+:]+\d+.\d+\][^\[]*)|(\[\w+:[^\[]*\])";
            if (Regex.IsMatch(read, pattern))
            {
                MatchCollection mark = Regex.Matches(read, @"\[(offset|ti|ar|al|by):([^\[]*)\]", RegexOptions.IgnoreCase);
                foreach (Match x in mark)
                {
                    listBox2.Items.Add(x.Value.Replace("\n", ""));
                }
                MatchCollection lrc = Regex.Matches(read, @"\[[\d+:]+\d+.\d+\][^\[]*");
                foreach (Match x in lrc)
                {
                    listBox2.Items.Add(x.Value.Replace("\n", ""));
                }
                string lrcFormat = @"\[[\d+:]+\d+.\d+\]([^\[]*)";
                foreach (string x in listBox2.Items)
                {
                    MatchCollection context = Regex.Matches(x, lrcFormat);
                    foreach (Match m in context)
                    {
                        string line = m.Groups[1].Value;
                        if (line != " " && line != "")
                            listBox1.Items.Add(line);
                    }
                }
            }
            else
            {
                string[] lrc = read.Split('\n');
                foreach (string x in lrc)
                    listBox1.Items.Add(x);
                listBox1.SelectedIndex = 0;
            }
            ifsave = false;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 fm2 = new Form2("媒體", "YouTube", ConnEx());
            fm2.ShowDialog();
            if (fm2.selected == 1)
            {
                OpenFileDialog openfile = new OpenFileDialog
                {
                    Title = "[Lrc Maker] 開啟檔案",
                    InitialDirectory = @"C:\Users\%username%\Music\",
                    RestoreDirectory = true,
                    Multiselect = false,
                    Filter = "所有支援的音訊檔|*.mp3;*.wav;*.wma;*.mpg;*.mpeg;*.m1v;*.mp2;*.mpa;*.mpe;*.m3u;*.avi;*.FLAC;*.M4A;*.AAC;*.OGG;*.3GP;*.AMR;*.APE;*.MKA;*.Opus;*.Wavpack;*.Musepack"
                };
                openfile.ShowDialog();
                string axfile = openfile.FileName;
                if (openfile.FileName == "") return;
                else
                {
                    //Bass.BASS_ChannelStop(stream);
                    Bass.BASS_StreamFree(stream);
                    ReadStream(Bass.BASS_StreamCreateFile(axfile, 0L, 0L, BASSFlag.BASS_SAMPLE_FLOAT));
                }
                LRCFIleName = Path.GetFileNameWithoutExtension(openfile.FileName);
                labelX1.Text = Path.GetFileNameWithoutExtension(openfile.FileName);
                toolTip1.SetToolTip(labelX1, labelX1.Text);
                FILEEXTSHOW(openfile.FileName);
            }
            else if (fm2.selected == 2)
            {
                string path = Application.StartupPath + @"\youtube-dl.exe";
                string cont, ver, url;

                //Download content from "yout-dl" offical webpage.
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage response = client.GetAsync("https://ytdl-org.github.io/youtube-dl/download.html").Result)
                    {
                        using (HttpContent content = response.Content)
                        {
                            cont = content.ReadAsStringAsync().Result;
                        }
                    }
                }

                //Find download link and software version.
                string pattern = @"<a\shref=""(.*)"">Windows exe<\/a>";
                url = Regex.Match(cont, pattern).Groups[1].Value;
                pattern = @"(\d+\.{0,1})+";
                ver = Regex.Match(url, pattern).Value;

                //Checking version.
                string[] lack = { "Youtube-Dl" }, dlUrl = { url }, name = { "youtube-dl.exe" };
                bool hasNewer = true;
                if (File.Exists(path))
                {
                    if (ver != FileVersionInfo.GetVersionInfo(path).FileVersion)
                    {
                        DialogResult dr = MessageBox.Show("檢查到所需組件有新版本 \"" + ver + "\" ，是否立即更新？\n建議將組件更新到最新版本以確保運作正常。", "Youtube-dl.exe", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (dr == DialogResult.Yes)
                        {
                            UpdateManager um = new UpdateManager(lack, dlUrl, name);
                            um.ShowDialog();
                        }
                        else
                        {
                            hasNewer = false;
                        }
                    }
                    else
                    {
                        hasNewer = false;
                    }
                }
                else
                {
                    UpdateManager um = new UpdateManager(lack, dlUrl, name);
                    um.ShowDialog();
                }
                if (!hasNewer)
                {
                    SearchVideo sv = new SearchVideo();
                    sv.ShowDialog();
                    if (sv.isdownloaded)
                    {
                        Bass.BASS_StreamFree(stream);
                        string audioUrl = sv.requestUrl;
                        ReadStream(Bass.BASS_StreamCreateURL(audioUrl, 0, BASSFlag.BASS_SAMPLE_FLOAT, null, this.Handle));
                        LRCFIleName = sv.title;
                        labelX1.Text = sv.title;
                        toolTip1.SetToolTip(labelX1, labelX1.Text);
                        FILEEXTSHOW(".youtube");
                    }
                }
            }
            listBox2.Focus();
        }

        private void ReadStream(int stream)
        {
            this.stream = stream;
            Bass.BASS_ChannelGetAttribute(stream, BASSAttribute.BASS_ATTRIB_FREQ, ref curfreq);
            slider3.Value = (int)curfreq;
            Bass.BASS_ChannelSlideAttribute(stream, BASSAttribute.BASS_ATTRIB_FREQ, curfreq, 0);
            pictureBox4.BackgroundImage = ProgramImg.mp1;
            long t = Bass.BASS_ChannelGetLength(stream);
            label7.Text = Format.MillisecondToSixtieth(Bass.BASS_ChannelBytes2Seconds(stream, t), false); //duration
            toolTip2.SetToolTip(label7, label7.Text);
            slider1.Maximum = Convert.ToInt32(t);
            ifsave = false;
        }

        void FILEEXTSHOW (string filedir)
        {
            musictype.Text = new FileInfo(filedir).Extension.Substring(1).ToUpper(); ;
            musictype.Visible = true;
            switch (musictype.Text)
            {
                case "MP3":
                    musictype.ForeColor = Color.FromArgb(255, 71, 163, 255);
                    break;
                case "WAV":
                    musictype.ForeColor = Color.FromArgb(255, 164, 164, 81);
                    break;
                case "WMA":
                    musictype.ForeColor = Color.FromArgb(255, 130, 192, 192);
                    break;
                case "OGG":
                    musictype.ForeColor = Color.FromArgb(255, 235, 192, 0);
                    break;
                case "FLAC":
                    musictype.ForeColor = Color.FromArgb(255, 255, 145, 36);
                    break;
                case "APE":
                    musictype.ForeColor = Color.FromArgb(255, 193, 123, 185);
                    break;
                case "YOUTUBE":
                    musictype.ForeColor = Color.Red;
                    break;
                default:
                    musictype.ForeColor = Color.FromArgb(255, 155, 155, 155);
                    break;
            }
            
        }

        private void musictype_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            GraphicsPath g = new GraphicsPath();
            Pen p = new Pen(musictype.ForeColor, 1.5f);
            g.AddRectangle(new RectangleF(0, 0, musictype.Width - 2, musictype.Height - 2));
            e.Graphics.DrawPath(p, g);
        }

        private void changetolrc_Click(object sender, EventArgs e)
        {
            string st = (string)listBox1.SelectedItem;
            try
            {
                long t = Bass.BASS_ChannelGetPosition(stream);
                string time = Format.MillisecondToSixtieth(Bass.BASS_ChannelBytes2Seconds(stream, t), true);
                listBox2.Items.Add("[" + time + "]" + st);

                if (listBox1.SelectedIndex == listBox1.Items.Count - 1 && listBox2.Items.Count > 0)
                {
                    listBox2.SelectedIndex = listBox2.Items.Count - 1;
                    MessageBox.Show("[提示]\n資料末端！", "Lrc Maker", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    try
                    {
                        listBox1.SelectedIndex++;
                        listBox2.SelectedIndex = listBox2.Items.Count - 1;
                    }
                    catch (Exception ex)
                    {
                        DialogResult dl = MessageBox.Show(ex.Message + "\n是否回報此問題？", "發生未預期的錯誤", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        if (dl == DialogResult.Yes)
                        {
                            Repair re = new Repair(ex.ToString());
                            re.ShowDialog();
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                DialogResult dl = MessageBox.Show(ex.Message + "\n是否回報此問題？", "發生未預期的錯誤", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (dl == DialogResult.Yes)
                {
                    Repair re = new Repair(ex.ToString());
                    re.ShowDialog();
                }
            }
            listBox2.Focus();
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            label6.Text = Format.MillisecondToSixtieth(Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetPosition(stream)), false);
            //label7.Text = axWindowsMediaPlayer1.currentMedia.durationString;

            //slider1.Minimum = 0;
            //slider1.Maximum = (int)System.Math.Round(axWindowsMediaPlayer1.currentMedia.duration*1000,0,MidpointRounding.AwayFromZero);
            if (hasChanged)
                this.slider1.Value = Convert.ToInt32(Bass.BASS_ChannelGetPosition(stream));
            label4.Text = slider1.Value + ", " + Convert.ToInt32(Bass.BASS_ChannelGetPosition(stream)) + " / " + slider1.Maximum;
            //double per = (double)axWindowsMediaPlayer1.Ctlcontrols.currentPosition / (double)axWindowsMediaPlayer1.currentMedia.duration * 1000;
            //int prog = (int)System.Math.Round(per, 0, MidpointRounding.AwayFromZero);
            //this.slider1.Value = prog;
            if (Bass.BASS_ChannelGetPosition(stream) == Bass.BASS_ChannelGetLength(stream))
            {
                timer1.Stop();
                pictureBox4.BackgroundImage = ProgramImg.mp1;
            }
        }

        void stateChanged()
        {

            if (playstatus == BASSActive.BASS_ACTIVE_PLAYING)
            {
                if ((pictureBox4.BackgroundImage != ProgramImg.pau1 || pictureBox4.BackgroundImage != ProgramImg.pau3) || pictureBox4.BackgroundImage != ProgramImg.pau2)
                    pictureBox4.BackgroundImage = ProgramImg.pau1;
            }
            else
            {
                if ((pictureBox4.BackgroundImage != ProgramImg.mp1 || pictureBox4.BackgroundImage != ProgramImg.mp2) || pictureBox4.BackgroundImage != ProgramImg.mp3)
                    pictureBox4.BackgroundImage = ProgramImg.mp1;
            }
        }
        

        private void TimerEventProcessor(object sender, System.EventArgs e)
        {

        }

        private void undo_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == 0)
            {
                listBox2.SelectedIndex = listBox2.Items.Count - 1;
            }
            else
            {
                int sel = listBox1.SelectedIndex;
                int cou = listBox2.Items.Count;
                string lengthX = (string)listBox1.Items[listBox1.SelectedIndex];
                int countX = lengthX.Length * 2 / 3;
                //取得L2當前選擇的上個項目的文字。
                try
                {
                    string items;
                    double Seconds;
                    if (listBox2.Items.Count > 1)
                    {
                        items = (string)listBox2.Items[listBox2.SelectedIndex - 1];
                        Seconds = int.Parse(items.Substring(1, 2)) * 60 + double.Parse((items.Substring(4, 5)));
                    }
                    else
                    {
                        Seconds = 0;
                    }

                    if (listBox2.Items.Count > 0)
                    {
                        listBox2.Items.RemoveAt(cou - 1);
                        listBox1.SelectedIndex--;
                        Bass.BASS_ChannelSetPosition(stream, Seconds);
                    }
                    else if (listBox2.Items.Count == 0)
                    {
                        MessageBox.Show("[警告]\n在\"處理後\"選單中沒有任何項目！", "Lrc Maker", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        try
                        {
                            listBox2.Items.RemoveAt(cou - 1);
                            listBox1.SelectedIndex--;
                            Bass.BASS_ChannelSetPosition(stream, Seconds);
                        }
                        catch (Exception ex)
                        {
                            DialogResult dl = MessageBox.Show(ex.Message + "\n是否回報此問題？", "發生未預期的錯誤", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                            if (dl == DialogResult.Yes)
                            {
                                Repair re = new Repair(ex.ToString());
                                re.ShowDialog();
                            }
                        }
                    }
                }
                catch
                {
                    try
                    {
                        listBox2.Items.RemoveAt(cou - 1);
                        listBox1.SelectedIndex--;
                        Bass.BASS_ChannelSetPosition(stream, Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetPosition(stream)) - countX);
                    }
                    catch
                    {
                        MessageBox.Show("[警告]\n在\"處理後\"選單中沒有任何項目！", "Lrc Maker", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            listBox2.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            listBox2.Focus();
        }

        private void ti_Click(object sender, EventArgs e)
        {
                int sel = listBox1.SelectedIndex;
                string st = (string)listBox1.SelectedItem;
                listBox2.Items.Add("[ti:" + st + "]");
                if (listBox1.SelectedIndex + 1 == listBox1.Items.Count)
                {

                }
                else
                {
                    listBox1.SelectedIndex++;
                }
                listBox2.Focus();
        }

        private void ar_Click(object sender, EventArgs e)
        {
                int sel = listBox1.SelectedIndex;
                string st = (string)listBox1.SelectedItem;
                listBox2.Items.Add("[ar:" + st + "]");
                if (listBox1.SelectedIndex + 1 == listBox1.Items.Count)
                {

                }
                else
                {
                    listBox1.SelectedIndex++;
                }
                listBox2.Focus();
        }

        private void al_Click(object sender, EventArgs e)
        {

                int sel = listBox1.SelectedIndex;
                string st = (string)listBox1.SelectedItem;
                listBox2.Items.Add("[al:" + st + "]");
                if (listBox1.SelectedIndex + 1 == listBox1.Items.Count)
                {

                }
                else
                {
                    listBox1.SelectedIndex++;
                }
                listBox2.Focus();
        }

        private void by_Click(object sender, EventArgs e)
        {
                int sel = listBox1.SelectedIndex;
                string st = (string)listBox1.SelectedItem;
                listBox2.Items.Add("[by:" + st + "]");
                if (listBox1.SelectedIndex + 1 == listBox1.Items.Count)
                {

                }
                else
                {
                    listBox1.SelectedIndex++;
                }
                listBox2.Focus();
        }

        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
        }

        private Encoding encodings()
        {
            if (comboBox1.SelectedIndex == 0) return Encoding.Default;
            else if (comboBox1.SelectedIndex == 1) return Encoding.Unicode;
            else if (comboBox1.SelectedIndex == 2) return Encoding.BigEndianUnicode;
            else if (comboBox1.SelectedIndex == 3) return Encoding.ASCII;
            else if (comboBox1.SelectedIndex == 4) return Encoding.UTF8;
            else return Encoding.Default;
        }

        private void save_Click(object sender, EventArgs e)
        {
            //Encoding encodings = Encoding.Default;
            //if (comboBox1.SelectedIndex == 0) encodings = Encoding.Default;
            //else if (comboBox1.SelectedIndex == 1) encodings = Encoding.Unicode;
            //else if (comboBox1.SelectedIndex == 2) encodings = Encoding.BigEndianUnicode;
            //else if (comboBox1.SelectedIndex == 3) encodings = Encoding.ASCII;
            //else if (comboBox1.SelectedIndex == 4) encodings = Encoding.UTF8;
            //else encodings = Encoding.Default;
            saveFile.FileName = LRCFIleName;
            if (listBox2.Items.Count == 0)
            {
                DialogResult myResult = MessageBox.Show("試圖存檔空白資料！\n確定？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (myResult == DialogResult.Yes)
                {
                    if (saveFile.ShowDialog() == DialogResult.OK)
                    {
                        System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFile.FileName,false, encodings());
                        foreach (object item in listBox2.Items)
                            sw.WriteLine(item.ToString());
                        sw.Close();
                        ifsave = true;
                    }

                }
                
            }
            else
            {
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFile.FileName, false, encodings());
                    foreach (object item in listBox2.Items)
                        sw.WriteLine(item.ToString());
                    sw.Close();
                    ifsave = true;

                    //// Save limit
                    //string fileName = appDataFolder + $"\\{Environment.MachineName}.license";
                    //if (!File.Exists(fileName))
                    //{
                    //    using (StreamWriter stw = new StreamWriter(fileName))
                    //    {
                    //        stw.Write(0);
                    //        stw.Close();
                    //    }
                    //    isfirsttipshow = true;
                    //}
                    //else
                    //{
                    //    int usecount = 0;
                    //    StreamReader str = new StreamReader(fileName);
                    //    usecount = int.Parse(str.ReadLine());
                    //    str.Close();
                    //    if (usecount >= 0)
                    //    {
                    //        using (StreamWriter stw = new StreamWriter(fileName))
                    //        {
                    //            stw.Write(usecount + 1);
                    //            stw.Close();
                    //        }
                    //        if (usecount > 2)
                    //        {
                    //            survey s_fm = new survey();
                    //            s_fm.ShowDialog();
                    //        }
                    //    }
                    //}
                }
            }
            listBox2.Focus();
        }

        struct License
        {
            string SerialNumber;

            string __MachineName;
            string __LicenseType;
            int    __SaveLimit;
            int    __SaveCount;

            public License(string sn)
            {
                SerialNumber = sn;
                __MachineName = "";
                __LicenseType = "";
                __SaveLimit = 0;
                __SaveCount = 0;
            }
            public string MachineName() { return __MachineName; }
            public string LicenseType() { return __LicenseType; }
            public int SaveLimit() { return __SaveLimit; }
            public int SaveCount() { return __SaveCount; }
        }

        private void CreateSerialNumber(string licenseType)
        {
            string machineName = Environment.MachineName;
            string mnNum = "";
            for (int i = 0; i < machineName.Length; i += 3)
            {
                mnNum += ((int)machineName[i]).ToString("000");
            }
            string ltNum = "";
            foreach (char c in licenseType)
            {
                ltNum += ((int)c).ToString("000");
            }
            long lN = long.Parse(ltNum);

            if (mnNum.Length >= ltNum.Length)
            {
                string need = mnNum.Substring(mnNum.Length - ltNum.Length);
                long mN = long.Parse(need);
                long newNeed = mN + lN;
                string newMnNum = mnNum.Substring(0, mnNum.Length - ltNum.Length) + newNeed.ToString();
                MessageBox.Show(mnNum + "\n" + ltNum + "\n" + newMnNum);
            }
            else
            {
                long mN = long.Parse(mnNum);
                long newNeed = mN + lN;
                string newMnNum = newNeed.ToString();
                MessageBox.Show(mnNum + "\n" + ltNum + "\n" + newMnNum);
            }
        }

        private void allclean_Click(object sender, EventArgs e)
        {
            Bass.BASS_StreamFree(stream);
            stream = 0;
            label6.Text = "-- : --";
            try
            {
                listBox1.Items.Clear();
                listBox2.Items.Clear();
            }
            catch { }
            loadFromFile = false;
            Button_Enabled_False();
            listBox2.Focus();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            startPoint = new Point(-e.X, -e.Y);
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (!File.Exists(appDataFolder + "\\like" + ver + ".tmp"))
            {
                StreamWriter SW = new StreamWriter(appDataFolder + "\\like" + ver + ".tmp");
                SW.Close();
                StreamWriter SW2 = new StreamWriter(appDataFolder + "\\liked" + ver + ".tmp");
                SW2.Close();
            }
            if (ifsave == false)
            {
                DialogResult myResult = MessageBox.Show("有尚未儲存的資料，關閉程式會使資料遺失？\n按[是]儲存\n按[否]關閉程式\n按[取消]返回程式", "LRC Maker", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (myResult == DialogResult.Yes)
                {

                    if (saveFile.ShowDialog() == DialogResult.OK)
                    {
                        StreamWriter sw = new StreamWriter(saveFile.FileName);
                        foreach (object item in listBox2.Items)
                            sw.WriteLine(item.ToString());
                        sw.Close();
                    }

                }
                else if (myResult == DialogResult.No)
                {
                    Application.Exit();
                }
            }
            else
            {
                Application.Exit();
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_CLOSE = 0xF060;
            if (m.Msg == WM_SYSCOMMAND && (int)m.WParam == SC_CLOSE)
            {
                if (listBox2.Items.Count > 0)
                {
                    DialogResult myResult = MessageBox.Show("有尚未儲存的資料，關閉程式會使資料遺失？\n按[是]儲存\n按[否]關閉程式\n按[取消]返回程式", "LRC Maker", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (myResult == DialogResult.Yes)
                    {

                        if (saveFile.ShowDialog() == DialogResult.OK)
                        {
                            System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFile.FileName);
                            foreach (object item in listBox2.Items)
                                sw.WriteLine(item.ToString());
                            sw.Close();
                        }

                    }
                    else if (myResult == DialogResult.No)
                    {
                        Application.Exit();
                    }
                }
                else
                {
                    Application.Exit();
                }
            }
            base.WndProc(ref m);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            startPoint = new Point(-e.X, -e.Y);
            if (istipshow)
            {
                int count = 0;
                while (true)
                {
                    if (count >= t.Length)
                    {
                        t = new ToolTip[0];
                        break;
                    }
                    t[count].Active = false;
                    count++;
                }
            }
            if (isfocustipshow)
            {
                toolTip2.Hide(pictureBox5);
            }
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

            if (!donatebtn.Visible)
            {
                if (donate.Width == 8)
                    donate.Location = new Point(this.Left + this.Width, this.Top);
                else
                    donate.Location = new Point(this.Left + this.Width, this.Top);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (istipshow) tip();
            if(isfocustipshow)
            {
                isfocustipshow = false;
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                string st = (string)listBox1.SelectedItem;
                //renamebox newrenamebox = new renamebox();
                //newrenamebox.ShowDialog();
                renamebox newrenamebox = new renamebox(st);
                newrenamebox.ShowDialog();
                if (!newrenamebox.cancel)
                {
                    try
                    {
                        listBox1.Items[listBox1.SelectedIndex] = newrenamebox.textBox1.Text;
                    }
                    catch (Exception ex)
                    {
                        DialogResult dl = MessageBox.Show(ex.Message + "\n是否回報此問題？", "發生未預期的錯誤", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        if (dl == DialogResult.Yes)
                        {
                            Repair re = new Repair(ex.ToString());
                            re.ShowDialog();
                        }
                    }
                }
            }
            listBox2.Refresh();
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            if(Bass.BASS_ChannelIsActive(stream) == BASSActive.BASS_ACTIVE_PLAYING)
            {
                listBox2.Focus();
            }
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                int posindex = listBox1.IndexFromPoint(new Point(e.X, e.Y));

                listBox1.ContextMenuStrip = null;

                if (posindex >= 0 && posindex < listBox1.Items.Count)
                {

                    listBox1.SelectedIndex = posindex;

                    contextMenuStrip1.Show(listBox1, new Point(e.X, e.Y));

                }

            }

            listBox1.Refresh();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            listBox1_DoubleClick(this, null);
            listBox2.Focus();
            //listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            //listBox1.Items.Insert(listBox1.SelectedIndex - 1, newrenamebox.textBox1.Text);

            //MessageBox.Show(newrenamebox.textBox1.Text);
            //for (int i = 0; i < listBox2.Items.Count; i++)
            //    if (listBox1.GetSelected(i))
            //    {
            //        //開啟renamebox
                    
                    
            //    }
        }

        private void slider2_Click(object sender, EventArgs e)
        {
            Bass.BASS_ChannelSlideAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, (float)slider2.Value / 100, 0);
            toolTip2.SetToolTip(slider2, slider2.Value.ToString());
            listBox2.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //double per = (double)axWindowsMediaPlayer1.Ctlcontrols.currentPosition / (double)axWindowsMediaPlayer1.currentMedia.duration * 1000;
            //int prog = (int)System.Math.Round(per, 0, MidpointRounding.AwayFromZero);
            //this.slider1.Value = prog;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            //Thread.Sleep(3000);
            playstatus = Bass.BASS_ChannelIsActive(stream);
            timer1.Enabled = true;
            if (playstatus == BASSActive.BASS_ACTIVE_PLAYING)
            {
                pictureBox4.BackgroundImage = ProgramImg.mp1;
                Bass.BASS_ChannelPause(stream);
                stoped = false;
            }
            else
            {
                Bass.BASS_ChannelPlay(stream, stoped);
                pictureBox4.BackgroundImage = ProgramImg.pau1;
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Bass.BASS_ChannelPause(stream);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Bass.BASS_ChannelStop(stream);
            timer1.Stop();
            label6.Text = "-- : --";
            if (ifplaylrc)
            {
                button11_Click(null, null);
            }
            stoped = true;
            pictureBox4.BackgroundImage = ProgramImg.mp1;
            slider1.Value = 0;
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            if (first == true)
            {
                oldtrackbarvalue = slider2.Value;
                Bass.BASS_ChannelSlideAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, 0, 0);
                slider2.Value = 0;
                first = false;
            }
            else
            {
                slider2.Value = oldtrackbarvalue;
                first = true;
            }
        }

        //private void axWindowsMediaPlayer1_PlayStateChange(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
        //{
            
        //    toolTip2.SetToolTip(label7, label7.Text);
        //    if (playstatus == BASSActive.BASS_ACTIVE_PAUSED)
        //    {
        //        pictureBox4.Image = ProgramImg.mp1;

        //    }
        //    else if (playstatus == BASSActive.BASS_ACTIVE_PLAYING)
        //    {
        //        pictureBox4.Image = ProgramImg.pau1;
        //    }
        //    else
        //    {
        //        pictureBox4.Image = ProgramImg.mp1;
        //    }
        //}

        private void pictureBox4_MouseDown(object sender, MouseEventArgs e)
        {
            playstatus = Bass.BASS_ChannelIsActive(stream);
            if (playstatus == BASSActive.BASS_ACTIVE_PAUSED)
            {
                pictureBox4.BackgroundImage = ProgramImg.mp2;

            }
            else if (playstatus == BASSActive.BASS_ACTIVE_PLAYING)
            {
                pictureBox4.BackgroundImage = ProgramImg.pau3;
            }
            else
            {
                pictureBox4.BackgroundImage = ProgramImg.mp2;
            }
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            playstatus = Bass.BASS_ChannelIsActive(stream);
            if (playstatus == BASSActive.BASS_ACTIVE_PAUSED)
            {
                pictureBox4.BackgroundImage = ProgramImg.mp1;

            }
            else if (playstatus == BASSActive.BASS_ACTIVE_PLAYING)
            {
                pictureBox4.BackgroundImage = ProgramImg.pau1;
            }
            else
            {
                pictureBox4.BackgroundImage = ProgramImg.mp1;
            }
        }

        private void pictureBox4_MouseMove(object sender, MouseEventArgs e)
        {
            playstatus = Bass.BASS_ChannelIsActive(stream);
            if (playstatus == BASSActive.BASS_ACTIVE_PAUSED)
            {
                pictureBox4.BackgroundImage = ProgramImg.mp3;

            }
            else if (playstatus == BASSActive.BASS_ACTIVE_PLAYING)
            {
                pictureBox4.BackgroundImage = ProgramImg.pau2;
            }
            else
            {
                pictureBox4.BackgroundImage = ProgramImg.mp3;
            }
        }

        private void pictureBox4_MouseUp(object sender, MouseEventArgs e)
        {
            playstatus = Bass.BASS_ChannelIsActive(stream);
            if (playstatus == BASSActive.BASS_ACTIVE_PAUSED)
            {
                pictureBox4.BackgroundImage = ProgramImg.mp1;
            }
            else if (playstatus == BASSActive.BASS_ACTIVE_PLAYING)
            {
                pictureBox4.BackgroundImage = ProgramImg.pau1;
            }
            else
            {
                pictureBox4.BackgroundImage = ProgramImg.mp1;
            }
        }

        private void label6_MouseMove(object sender, MouseEventArgs e)
        {
            //toolTip2.SetToolTip(label6, Format.MillisecondToSixtieth(Bass.BASS_ChannelBytes2Seconds(stream, slider1.Value), false));
        }

        private void slider2_ValueChanging(object sender, DevComponents.DotNetBar.CancelIntValueEventArgs e)
        {
            Bass.BASS_ChannelSlideAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, (float)slider2.Value/100, 0);
            toolTip2.SetToolTip(slider2, slider2.Value.ToString());
            listBox2.Focus();
        }

        private void slider2_ValueChanged(object sender, EventArgs e)
        {
            Bass.BASS_ChannelSlideAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, (float)slider2.Value/100, 0);
            if (slider2.Value == 0)
            {
                pictureBox7.BackgroundImage = ProgramImg.nvp1;
            }
            else
            {
                pictureBox7.BackgroundImage = ProgramImg.vp1;
            }
            listBox2.Focus();
        }

        private void pictureBox7_MouseDown(object sender, MouseEventArgs e)
        {
            if (slider2.Value == 0)
            {
                pictureBox7.BackgroundImage = ProgramImg.nvp2;
            }
            else
            {
                pictureBox7.BackgroundImage = ProgramImg.vp2;
            }
        }

        private void pictureBox7_MouseLeave(object sender, EventArgs e)
        {
            if (slider2.Value == 0)
            {
                pictureBox7.BackgroundImage = ProgramImg.nvp1;
            }
            else
            {
                pictureBox7.BackgroundImage = ProgramImg.vp1;
            }
        }

        private void pictureBox7_MouseMove(object sender, MouseEventArgs e)
        {
            if (slider2.Value == 0)
            {
                pictureBox7.BackgroundImage = ProgramImg.nvp3;
            }
            else
            {
                pictureBox7.BackgroundImage = ProgramImg.vp3;
            }
        }

        private void pictureBox7_MouseUp(object sender, MouseEventArgs e)
        {
            if (slider2.Value == 0)
            {
                pictureBox7.BackgroundImage = ProgramImg.nvp1;
            }
            else
            {
                pictureBox7.BackgroundImage = ProgramImg.vp1;
            }
        }

        private void pictureBox6_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox6.BackgroundImage = ProgramImg.st3;
        }

        private void pictureBox6_MouseLeave(object sender, EventArgs e)
        {
            pictureBox6.BackgroundImage = ProgramImg.st1;
        }

        private void pictureBox6_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox6.BackgroundImage = ProgramImg.st2;
        }

        private void pictureBox6_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox6.BackgroundImage = ProgramImg.st1;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            if (ifplaylrc)
            {
                try
                {
                    string jumptotime = listBox2.Items[listBox2.SelectedIndex].ToString();
                    double Seconds = int.Parse(jumptotime.Substring(1, 2)) * 60 + double.Parse((jumptotime.Substring(4, 5)));
                    Bass.BASS_ChannelSetPosition(stream, Seconds);
                    COUNTED = listBox2.SelectedIndex;
                }
                catch { };
            }
            else if (listBox2.Items.Count > 0)
            {
                if (ifucshow)
                {
                    this.splitContainer1.Panel2.Controls.RemoveByKey("UC" + cursel.ToString());
                    ifucshow = false;
                }
                string st = (string)listBox2.SelectedItem;
                //renamebox newrenamebox = new renamebox();
                //newrenamebox.ShowDialog();
                renamebox newrenamebox = new renamebox(st);
                newrenamebox.ShowDialog();
                if (!newrenamebox.cancel)
                {
                    try
                    {
                        listBox2.Items[listBox2.SelectedIndex] = newrenamebox.textBox1.Text;
                    }
                    catch (Exception ex)
                    {
                        DialogResult dl = MessageBox.Show(ex.Message + "\n是否回報此問題？", "發生未預期的錯誤", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        if (dl == DialogResult.Yes)
                        {
                            Repair re = new Repair(ex.ToString());
                            re.ShowDialog();
                        }
                    }
                }
            }
            listBox2.Refresh();
        }

        int cursel = -1;
        bool ifucshow = false;

        private void listBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                int posindex = listBox2.IndexFromPoint(new Point(e.X, e.Y));

                listBox2.ContextMenuStrip = null;

                if (posindex >= 0 && posindex < listBox2.Items.Count)
                {

                    listBox2.SelectedIndex = posindex;

                    contextMenuStrip2.Show(listBox2, new Point(e.X, e.Y));

                }

            }
            listBox2.Refresh();
        }

        private void about_Click(object sender, EventArgs e)
        {
            if(istipshow)
            {
                int count = 0;
                while (true)
                {
                    if (count >= t.Length)
                    {
                        t = new ToolTip[0];
                        break;
                    }
                    t[count].Active = false;
                    count++;
                }
                istipshow = false;
            }
            else
            {
                tip();
            }
            listBox2.Focus();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            listBox2_DoubleClick(this, null);
            listBox2.Focus();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            listBox2.Items.RemoveAt(listBox2.SelectedIndex);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            MessageBox.Show(Convert.ToInt32(ch).ToString());

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //if (Convert.ToInt32(e.KeyCode) == 37)
            //{
            //    MessageBox.Show("yes");
            //}
        }

        private void listBox1_KeyPress(object sender, KeyPressEventArgs e)
        {


            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            inputlryic formil = new inputlryic();
            formil.ShowDialog();
            if (!formil.cancel)
            {
                if (listBox1.Items.Count > 0 || listBox2.Items.Count > 0)
                {
                    DialogResult dr = MessageBox.Show("接下來要清除所有工作區資料哦，你確定嗎？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                    if (dr == DialogResult.No) return;
                }
                listBox1.Items.Clear();
                listBox2.Items.Clear();
                loadFromFile = false;
                string lryic = formil.richTextBox1.Text;
                char[] chs = new char[0];
                while (lryic.ToLower().IndexOf("repeat") > 0)
                {
                    string func = "";
                    int funcStart = lryic.ToLower().IndexOf("repeat");
                    int funcLength = 6;
                    char temp = char.Parse(lryic.Substring(funcStart + funcLength, 1));
                    while (!char.IsLetter(temp) && !char.IsNumber(temp) /*&& !char.IsPunctuation(temp) */ && funcStart + funcLength +1 < lryic.Length)
                    {
                        funcLength++;
                            temp = char.Parse(lryic.Substring(funcStart + funcLength, 1));
                    }
                    if (funcLength > 6)
                    {
                        func = lryic.Substring(funcStart, funcLength);
                        MessageBox.Show(func);
                        char[] repeatSymbol = func.Substring(7).ToCharArray();
                        string part = "";
                        foreach (char c in repeatSymbol)
                        {
                            if (c != ' ')
                            {
                                if (!chs.Contains(c))
                                {
                                    Array.Resize(ref chs, chs.Length);
                                    chs[chs.Length - 1] = c;
                                }
                                int start = lryic.IndexOf(c) + 1;
                                int end = lryic.IndexOf(c, start);
                                part += lryic.Substring(start, end - start) + Environment.NewLine;

                                MessageBox.Show(lryic.Substring(start, end - start),"part" + c);
                            }
                        }
                        if (part != "")
                        {
                            lryic = lryic.Remove(funcStart, funcLength);
                            MessageBox.Show(lryic, "remove");
                            lryic = lryic.Insert(funcStart, part);
                            MessageBox.Show(lryic,"insert");
                        }
                    }
                }
                foreach (char c in chs)
                    lryic.Replace(c.ToString(), "");
                RichTextBox rtb = new RichTextBox();
                rtb.Text = lryic;
                listBox1.Items.AddRange(rtb.Lines);
                Button_Enabled();
                try
                {
                    listBox1.SelectedIndex = 0;
                }
                catch
                {
                    Button_Enabled_False();
                }
                ifsave = false;
            }
            listBox1.Refresh();
            listBox2.Refresh();
            listBox2.Focus();
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            int lst = listBox1.SelectedIndex;
            if (e.KeyCode == Keys.Delete)
            {
                if (listBox1.Items.Count > 0)
                {
                    listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                    if (lst < listBox1.Items.Count)
                    {
                        listBox1.SelectedIndex = lst;
                    }
                    else if (lst == listBox1.Items.Count)
                    {
                        listBox1.SelectedIndex = lst - 1;
                    }
                    else
                        listBox1.SelectedIndex = 0;
                }
            }
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Process myProcess = new Process();
            myProcess.StartInfo.FileName = "LRCMaker.exe";
            myProcess.StartInfo.Arguments = "vip";
            myProcess.Start();
            //try
            //{
            //    int a = int.Parse("test");
            //}
            //catch (Exception ex)
            //{
            //    Repair re = new Repair(ex.ToString());
            //    re.ShowDialog();
            //}
        }

        private void ifnew_Click(object sender, EventArgs e)
        {
            Process.Start("LRCMAKER_UPDATER.exe");
            Application.Exit();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"http://ivsnote.com/");
        }

        private void 關於ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ABOUT aboutform = new ABOUT();
            aboutform.ShowDialog();
            listBox2.Focus();
        }

        private void 開啟音樂檔案ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1.Focus();
            button1_Click(this, null);
            listBox2.Focus();
        }

        private void 載入文字檔ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button4.Focus();
            button4_Click(this, null);
            listBox2.Focus();
        }

        private void 輸入歌詞ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button2.Focus();
            button2_Click_1(this, null);
            listBox2.Focus();
        }

        private void 存檔為LRC格式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save.Focus();
            save_Click(this, null);
            listBox2.Focus();
        }

        private void 初始化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            allclean.Focus();
            allclean_Click(this, null);
            listBox2.Focus();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            
        }



        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {


        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void listBox2_KeyDown(object sender, KeyEventArgs e)
        {
            bool isplaying = (Bass.BASS_ChannelIsActive(stream) == BASSActive.BASS_ACTIVE_PLAYING);
            int lst = listBox2.SelectedIndex;
            if (e.KeyCode == Keys.Delete && !ifplaylrc && !isplaying)
            {
                if (listBox2.Items.Count > 0)
                {
                    listBox2.Items.RemoveAt(listBox2.SelectedIndex);
                    if (listBox2.Items.Count > 0)
                    {
                        if (lst < listBox2.Items.Count)
                        {
                            listBox2.SelectedIndex = lst;
                        }
                        else if (lst == listBox2.Items.Count)
                        {
                            listBox2.SelectedIndex = lst - 1;
                        }
                        else
                            listBox2.SelectedIndex = 0;
                    }
                }
            }

            if (e.KeyCode == Keys.Space)
            {
                //playstatus = Bass.BASS_ChannelIsActive(stream);
                //timer2.Enabled = false;
                //timer1.Enabled = true;
                //if (playstatus == BASSActive.BASS_ACTIVE_PLAYING)
                //{
                //    pictureBox4.Image = ProgramImg.mp1;
                //    Bass.BASS_ChannelPause(stream);
                //    stoped = false;
                //}
                //else
                //{
                //    Bass.BASS_ChannelPlay(stream, stoped);
                //    pictureBox4.Image = ProgramImg.pau1;
                //}
                pictureBox4.Focus();
                pictureBox4_Click(this, null);
                listBox2.Focus();
            } 

            //快速鍵
            if (e.KeyCode == Keys.Down) //NEXT LINE
            {
                if (Bass.BASS_ChannelIsActive(stream) == BASSActive.BASS_ACTIVE_PLAYING)
                {
                    changetolrc.Focus();
                    changetolrc_Click(this, null);
                }
                listBox2.Focus();
            }
            if (e.KeyCode == Keys.Up)//CANCEL
            {
                if (Bass.BASS_ChannelIsActive(stream) == BASSActive.BASS_ACTIVE_PLAYING)
                {
                    undo.Focus();
                    undo_Click(this, null);
                }
                listBox2.Focus();
            }
            if (e.KeyCode == Keys.Left) //BACK
            {
                listBox2.Focus();
                if (Bass.BASS_ChannelIsActive(stream) == BASSActive.BASS_ACTIVE_PLAYING)
                {
                    Bass.BASS_ChannelSetPosition(stream, Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetPosition(stream)) - 1);
                    checkit();
                    if (listBox2.Items.Count>0)
                        listBox2.SetSelected(listBox2.Items.Count - 1, true);
                }
                listBox2.Focus();
            }
            if (e.KeyCode == Keys.Right) //FORWAR
            {
                listBox2.Focus();
                if (Bass.BASS_ChannelIsActive(stream) == BASSActive.BASS_ACTIVE_PLAYING)
                    Bass.BASS_ChannelSetPosition(stream, Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetPosition(stream)) + 1);
                checkit();
                listBox2.Focus();

            }
            if(e.KeyCode == Keys.Enter) //paragraph
            {
                if (Bass.BASS_ChannelIsActive(stream) == BASSActive.BASS_ACTIVE_PLAYING)
                {
                    paragraph.Focus();
                    paragraph_Click(this, null);
                }
                listBox2.Focus();
            }


            //2015.1.1新增快速鍵
            string ch = e.KeyCode.ToString();
            if (ch == quickkey[0]) //NEXT LINE
            {
                changetolrc.Focus();
                changetolrc_Click(this, null);
                listBox2.Focus();
            }
            if (ch == quickkey[1]) //CANCEL
            {
                undo.Focus();
                undo_Click(this, null);
                listBox2.Focus();
            }
            if (ch == quickkey[3]) //?
            {
                mark.Focus();
                mark_Click(this, null);
                listBox2.Focus();
            }
            if (ch == quickkey[2]) //ALL CANCEL
            {
                button3.Focus();
                button3_Click(this, null);
                listBox2.Focus();
            }
            if (ch == "H") //HELP
            {
                about.Focus();
                about_Click(this, null);
                listBox2.Focus();
            }
            //2015.1.22增加音樂倒轉、音樂快轉
            if (ch == quickkey[4]) //BACK
            {
                listBox2.Focus();
                Bass.BASS_ChannelSetPosition(stream, Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetPosition(stream)) - 1);
                checkit();
                listBox2.Focus();
            }
            if (ch == quickkey[5]) //Forward
            {
                listBox2.Focus();
                Bass.BASS_ChannelSetPosition(stream, Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetPosition(stream)) + 1);
                checkit();
                listBox2.Focus();
            }

            //2017.5.15增加速度提升：
            if(e.KeyCode == Keys.Add)
            {
                slider3.Value += 100;
            }
            if(e.KeyCode == Keys.Subtract)
            {
                slider3.Value -= 100;
            }
        }

        private void listBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void pictureBox5_Click_1(object sender, EventArgs e)
        {
            listBox2.Focus();
            pictureBox5.Image = Resource1.ok;
        }

        private void changeTheme(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            DialogResult dr = DialogResult.Abort;
            ToolStripMenuItem btn = (ToolStripMenuItem)(sender);
            switch(btn.Name)
            {
                case "Black":
                    Theme_Black();
                    break;
                case "Brown":
                    Theme_Brown();
                    break;
                case "Pink":
                    Theme_Pink();
                    break;
                case "Green":
                    Theme_Green();
                    break;
                case "Blues":
                    Theme_Blues();
                    break;
                case "customColor":
                    dr = cd.ShowDialog();
                    if(dr == DialogResult.OK)
                        Theme_Custom(cd.Color);
                    break;
                default:
                    Theme_IVSnote();
                    break;
            }
            this.Refresh();
            slider1.Refresh();

            if (dr != DialogResult.Cancel)
                using (StreamWriter stw = new StreamWriter(appDataFolder + "\\SETTING_LRCMAKER_THEME.ini"))
                {
                    if (dr == DialogResult.Abort)
                        stw.Write("Theme=" + btn.Name);
                    else if (dr == DialogResult.OK)
                        stw.Write(cd.Color.R + "," + cd.Color.G + "," + cd.Color.B);
                    stw.Close();
                }
            //if (donateavailable) donate.SETCOLOR(this.BackColor);
            listBox2.Focus();
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            listBox2.Focus();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < listBox2.Items.Count; i++)
                {

                    var listbox2items = listBox2.Items[i].ToString();
                    string contants = listbox2items.Substring(10);
                    string times = listbox2items.Substring(0, 10);

                    int cou = 0;
                    //object[] objectarray = new object[cou];
                    int[] array = new int[cou];
                    // 將與 i_contants 有相同內容的那行加入陣列之中
                    for (int j = i + 1; j < listBox2.Items.Count; j++)
                    {
                        if (listBox2.Items[j].ToString().Substring(10) == contants)
                        {
                            cou++;
                            Array.Resize(ref array, cou);
                            array[cou - 1] = j;
                        }
                    }
                    // 將陣列中的項目輸出在同一行

                    for (int k = 0; k < cou; k++)
                    {
                        times = times + listBox2.Items[array[k]].ToString().Substring(0, 10);
                        listBox2.Items.RemoveAt(array[k]);
                    }
                    listBox2.Items[i] = times + contants;
                }
            }
            catch(Exception ex)
            {
                DialogResult dl = MessageBox.Show(ex.Message + "\n是否回報此問題？", "發生未預期的錯誤", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if(dl==DialogResult.Yes)
                {
                    Repair re = new Repair(ex.ToString());
                    re.ShowDialog();
                }
            }
            listBox2.Focus();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                if (listBox1.Items[i].ToString() == "")
                {
                    listBox1.Items.RemoveAt(i);
                    i--;
                }
            }
            listBox2.Focus();
        }

        private void offset(int value)
        {
            for(int i = 0; i < listBox2.Items.Count; i++)
            {
                string temp = listBox2.Items[i].ToString();
                if (Regex.IsMatch(temp, @"\[\d+:\d+.\d+\][^\[]*"))
                {
                    double time = Format.SixtiethToMillisecond(Regex.Match(temp, @"\[\d+:\d+.\d+\]").Value) + (double)value / 1000;
                    listBox2.Items[i] = "[" + Format.MillisecondToSixtieth(time, true) + "]" + temp.Substring(temp.IndexOf(']') + 1);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            offset(-10);
            //if (listBox2.Items.Count > 0 && listBox2.Items[0].ToString().Length >= 8 && listBox2.Items[0].ToString().Substring(0, 8) == "[offset:")
            //{
            //    int a = listBox2.Items[0].ToString().IndexOf(":") + 1;
            //    int offset = int.Parse(listBox2.Items[0].ToString().Substring(a, listBox2.Items[0].ToString().IndexOf("]") - a));
            //    int newoffset = offset - 100;
            //    listBox2.Items[0] = "[offset:" + newoffset + "]";
            //    listBox2.SelectedIndex = 0;
            //}
            //else
            //{
            //    listBox2.Items.Insert(0, "[offset:-100]");
            //    listBox2.SelectedIndex = 0;
            //}
            listBox2.Focus();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            offset(10);
            //if (listBox2.Items.Count > 0 && listBox2.Items[0].ToString().Length >= 8 && listBox2.Items[0].ToString().Substring(0, 8) == "[offset:")
            //{
            //    int a = listBox2.Items[0].ToString().IndexOf(":") + 1;
            //    int offset = int.Parse(listBox2.Items[0].ToString().Substring(a, listBox2.Items[0].ToString().IndexOf("]") - a));
            //    int newoffset = offset + 100;
            //    listBox2.Items[0] = "[offset:" + newoffset + "]";
            //    listBox2.SelectedIndex = 0;
            //}
            //else
            //{
            //    listBox2.Items.Insert(0, "[offset:100]");
            //    listBox2.SelectedIndex = 0;
            //}
            listBox2.Focus();
        }
        
        private void 上方ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Insert(listBox1.SelectedIndex, "");
        }

        private void 此行下方ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Insert(listBox1.SelectedIndex + 1, "");
        }

        private void 第一行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Insert(0, "");
        }

        private void 最後一行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Insert(listBox1.Items.Count, "");
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            listBox2.Items.Insert(listBox2.SelectedIndex, "");
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            listBox2.Items.Insert(listBox2.SelectedIndex + 1, "");
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            listBox2.Items.Insert(0, "");
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            listBox2.Items.Insert(listBox2.Items.Count, "");
        }

        private void mark_Click(object sender, EventArgs e)
        {
            markform mf = new markform();
            SortedList mark = new SortedList() { { "ti", -1 }, { "ar", -1 }, { "al", -1 }, { "by", -1 } };
            if (listBox2.Items.Count > 0)
            {
                for (int i = 0; i < listBox2.Items.Count; i++)
                {
                    Match m = Regex.Match(listBox2.Items[i].ToString(), @"\[(ti|ar|al|by):([^\[]*)\]", RegexOptions.IgnoreCase);
                    if (m.Value != null)
                        switch (m.Groups[1].Value.ToLower())
                        {
                            case "ti":
                                mf.textBox1.Text = m.Groups[2].Value;
                                mark["ti"] = i;
                                break;
                            case "ar":
                                mf.textBox2.Text = m.Groups[2].Value;
                                mark["ar"] = i;
                                break;
                            case "al":
                                mf.textBox3.Text = m.Groups[2].Value;
                                mark["al"] = i;
                                break;
                            case "by":
                                mf.textBox4.Text = m.Groups[2].Value;
                                mark["by"] = i;
                                break;
                        }
                }
            }
            mf.ShowDialog();
            if (mf.result)
            {
                int cnt = 0;
                foreach (DictionaryEntry d in mark)
                    if ((int)d.Value >= 0)
                        cnt++;
                int flag = 0;
                foreach (KeyValuePair<string, string> d in mf.marks)
                {
                    int pos = (int)mark[d.Key];
                    if (pos >= 0)
                        listBox2.Items[pos] = "[" + d.Key + ":" + d.Value + "]";
                    else if (d.Value != "")
                    {
                        listBox2.Items.Insert(cnt + flag, "[" + d.Key + ":" + d.Value + "]");
                        flag++;
                    }
                }
            }
            listBox2.Focus();
        }

        private void 問題回報ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Repair re = new Repair();
            re.ShowDialog();
        }


        //2015.5.23 ADD
        private void def_Click(object sender, EventArgs e)
        {
            def.Checked = !false;
            uni.Checked = false;
            unibe.Checked = false;
            asc2.Checked = false;
            u8.Checked = false;
        }

        private void uni_Click(object sender, EventArgs e)
        {
            def.Checked = false;
            uni.Checked = !false;
            unibe.Checked = false;
            asc2.Checked = false;
            u8.Checked = false;
        }

        private void unibe_Click(object sender, EventArgs e)
        {
            def.Checked = false;
            uni.Checked = false;
            unibe.Checked = !false;
            asc2.Checked = false;
            u8.Checked = false;
        }

        private void asc2_Click(object sender, EventArgs e)
        {
            def.Checked = false;
            uni.Checked = false;
            unibe.Checked = false;
            asc2.Checked = !false;
            u8.Checked = false;
        }

        private void u8_Click(object sender, EventArgs e)
        {
            def.Checked = false;
            uni.Checked = false;
            unibe.Checked = false;
            asc2.Checked = false;
            u8.Checked = !false;
        }

        private void updatelable_Click(object sender, EventArgs e)
        {
            //updatelable.Visible = false;
        }

        bool peopleSelectCombobox = false;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loadFromFile)
            {
                DialogResult dr = MessageBox.Show("是否更改當前顯示的編碼方式？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    StreamReader str = new StreamReader(txtfile, encodings(), true);
                    string read = "", a;
                    while (true)
                    {
                        a = str.ReadLine();
                        read += a;
                        if (a == null) break;
                        else read += "\n";
                    }
                    str.Close();
                    readTextFile(read);
                }
            }
            listBox2.Focus();
            if (peopleSelectCombobox)
            {
                try
                {
                    using (StreamWriter stw = new StreamWriter(appDataFolder + "\\SETTING_LRCMAKER_ENCODING.ini"))
                    {
                        stw.Write(comboBox1.SelectedIndex);
                        stw.Close();
                    }
                }
                catch (Exception ex)
                {
                    DialogResult dl = MessageBox.Show(ex.Message + "\n是否回報此問題？", "發生未預期的錯誤", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (dl == DialogResult.Yes)
                    {
                        Repair re = new Repair(ex.ToString());
                        re.ShowDialog();
                    }
                }
                peopleSelectCombobox = false;
            }
        }

        private void comboBox1_MouseCaptureChanged(object sender, EventArgs e)
        {
            listBox2.Focus();
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            peopleSelectCombobox = true;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        string fileName = string.Empty, txtfileName = string.Empty;

        private void button1_DragEnter(object sender, DragEventArgs e)
        {
            button1.Image = null;
            button1.Text = "音訊檔\n拖曳至此";
            button1.BackColor = Color.FromArgb(30, 0, 0, 0);
            button4.Image = null;
            button4.Text = "歌詞\n拖曳至此";
            button4.BackColor = Color.FromArgb(30, 0, 0, 0);
            try
            {
                if (e.Data.GetDataPresent("FileNameW"))
                {
                    string[] data = (e.Data.GetData("FileNameW") as string[]);
                    fileName = data[0];
                    e.Effect = DragDropEffects.All;
                }
            }
            catch (Exception ex)
            {
                DialogResult dl = MessageBox.Show(ex.Message + "\n是否回報此問題？", "發生未預期的錯誤", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (dl == DialogResult.Yes)
                {
                    Repair re = new Repair(ex.ToString());
                    re.ShowDialog();
                }
            }
        }

        private void button1_DragDrop(object sender, DragEventArgs e)
        {
            button1.Image = Resource1.file_a;
            button1.Text = "";
            button1.BackColor = Color.FromArgb(180, 255, 255, 255);
            button4.Image = Resource1.file_t;
            button4.Text = "";
            button4.BackColor = Color.FromArgb(180, 255, 255, 255);
            try
            {
                if (File.Exists(fileName))
                {
                    string axfile = fileName;
                    LRCFIleName = Path.GetFileNameWithoutExtension(fileName);
                    //LoadFile
                    Bass.BASS_StreamFree(stream);
                    stream = Bass.BASS_StreamCreateFile(axfile, 0L, 0L, BASSFlag.BASS_SAMPLE_FLOAT);
                    Bass.BASS_ChannelGetAttribute(stream, BASSAttribute.BASS_ATTRIB_FREQ, ref curfreq);
                    slider3.Value = (int)curfreq;
                    Bass.BASS_ChannelSlideAttribute(stream, BASSAttribute.BASS_ATTRIB_FREQ, curfreq, 0);
                    long t = Bass.BASS_ChannelGetLength(stream);
                    label7.Text = Format.MillisecondToSixtieth(Bass.BASS_ChannelBytes2Seconds(stream, t), false); //duration
                    toolTip2.SetToolTip(label7, label7.Text);
                    labelX1.Text = Path.GetFileName(fileName);
                    toolTip1.SetToolTip(labelX1, Path.GetFileName(fileName));
                    pictureBox4.BackgroundImage = ProgramImg.mp1;
                    labelX1.Text = Path.GetFileNameWithoutExtension(fileName);
                    toolTip1.SetToolTip(labelX1, labelX1.Text);
                    slider1.Maximum = Convert.ToInt32(t);
                    FILEEXTSHOW(fileName);
                }
            }
            catch (Exception ex)
            {
                DialogResult dl = MessageBox.Show(ex.Message + "\n是否回報此問題？", "發生未預期的錯誤", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (dl == DialogResult.Yes)
                {
                    Repair re = new Repair(ex.ToString());
                    re.ShowDialog();
                }
            }

            listBox2.Focus();
        }

        private void button4_DragDrop(object sender, DragEventArgs e)
        {
            button1.Image = Resource1.file_a;
            button1.Text = "";
            button1.BackColor = Color.FromArgb(180, 255, 255, 255);
            button4.Image = Resource1.file_t;
            button4.Text = "";
            button4.BackColor = Color.FromArgb(180, 255, 255, 255);
            try
            {
                if (File.Exists(txtfileName))
                {
                    Button_Enabled_False();
                    txtfile = txtfileName;
                    if (listBox1.Items.Count > 0 || listBox2.Items.Count > 0)
                    {
                        DialogResult dr = MessageBox.Show("接下來要清除所有工作區資料哦，你確定嗎？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                        if (dr == DialogResult.No) return;
                    }
                    loadFromFile = true;
                    StreamReader str = new StreamReader(txtfile, encodings(), true);
                    string read = "", a;
                    while (true)
                    {
                        a = str.ReadLine();
                        read += a;
                        if (a == null) break;
                        else read += "\n";
                    }
                    str.Close();
                    readTextFile(read);
                    Button_Enabled();
                }
            }
            catch (Exception ex)
            {
                DialogResult dl = MessageBox.Show(ex.Message + "\n是否回報此問題？", "發生未預期的錯誤", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (dl == DialogResult.Yes)
                {
                    Repair re = new Repair(ex.ToString());
                    re.ShowDialog();
                }
            }
            listBox2.Focus();
        }

        double[] timelist;

        private void button11_Click(object sender, EventArgs e)
        {
            //退出編輯模式
            this.splitContainer1.Panel2.Controls.RemoveByKey("UC" + cursel.ToString());
            ifucshow = false;
            editmode.Text = "進入編輯模式";
            editmodeenable = false;
            //預覽，測試腳本
            try
            {
                if (listBox2.Items.Count > 0)
                {
                    button1.Enabled = !button1.Enabled;
                    button2.Enabled = !button2.Enabled;
                    button4.Enabled = !button4.Enabled;
                    allclean.Enabled = !allclean.Enabled;
                    save.Enabled = !save.Enabled;
                    button5.Enabled = !button5.Enabled;
                    button6.Enabled = !button6.Enabled;
                    button7.Enabled = !button7.Enabled;
                    button8.Enabled = !button8.Enabled;
                    changetolrc.Enabled = !changetolrc.Enabled;
                    undo.Enabled = !undo.Enabled;
                    paragraph.Enabled = !paragraph.Enabled;
                    button3.Enabled = !button3.Enabled;
                    mark.Enabled = !mark.Enabled;

                    if (ifplaylrc)
                    {
                        playlrc.Stop();
                        timer1.Stop();
                        Bass.BASS_ChannelStop(stream);
                        ifplaylrc = !true;
                        contextMenuStrip2.Enabled = !false;
                        listBox2.SelectedIndex = 0;
                        button11.Image = Resource1.eye;
                        COUNTED = 0;
                        stoped = true;
                        pictureBox4.BackgroundImage = ProgramImg.mp1;
                        //listBox2.Items.RemoveAt(listBox2.Items.Count - 1);
                    }
                    else
                    {
                        //listBox2.Items.Insert(listBox2.Items.Count, "[" + axWindowsMediaPlayer1.currentMedia.durationString + ".00]結束");
                        ifplaylrc = true;
                        contextMenuStrip2.Enabled = false;
                        Bass.BASS_ChannelStop(stream);
                        listBox2.SelectedIndex = -1;
                        button11.Image = Resource1.eyenon;
                        //時間軸加入陣列
                        timelist = new double[listBox2.Items.Count];
                        for (int i = 0; i < listBox2.Items.Count; i++)
                        {
                            try
                            {

                                int minutes = int.Parse(listBox2.Items[i].ToString().Substring(1, 2));
                                double seconds = double.Parse(listBox2.Items[i].ToString().Substring(4, 5));
                                double totalsec = minutes * 60 + seconds;
                                timelist[i] = totalsec;
                            }
                            catch
                            {
                                timelist[i] = -1/*999999999999*/;
                            }
                        }
                        Bass.BASS_ChannelPlay(stream, true);
                        playlrc.Start();
                        timer1.Start();
                        pictureBox4.BackgroundImage = ProgramImg.pau1;
                    }
                }
                else
                {
                    MessageBox.Show("請將歌詞轉換後再進行預覽", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                DialogResult dl = MessageBox.Show(ex.Message + "\n是否回報此問題？", "發生未預期的錯誤", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (dl == DialogResult.Yes)
                {
                    Repair re = new Repair(ex.ToString());
                    re.ShowDialog();
                }
            }
            listBox2.Focus();
        }

        int COUNTED = 0;

        private void playlrc_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Bass.BASS_ChannelIsActive(stream) == BASSActive.BASS_ACTIVE_STOPPED)
                {
                    button11_Click(null, null);
                }
                else
                {
                    if (COUNTED < listBox2.Items.Count)
                    {
                        if (timelist[COUNTED] == -1)
                        {
                            COUNTED++;
                        }
                        else if (Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetPosition(stream)) > timelist[COUNTED])
                        {
                            listBox2.SelectedIndex = COUNTED;
                            COUNTED++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                button11_Click(null, null);
                DialogResult dl = MessageBox.Show(ex.Message + "\n是否回報此問題？", "發生未預期的錯誤", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (dl == DialogResult.Yes)
                {
                    Repair re = new Repair(ex.ToString());
                    re.ShowDialog();
                }
            }
        }

        void checkit()
        {
            if (ifplaylrc)
            {
                for (int i = timelist.Length - 1; i >= 0; i--)
                {
                    if (timelist[i] == -1)
                    {
                        continue;
                    }
                    else if (timelist[i] <= Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetPosition(stream)))
                    {
                        listBox2.SelectedIndex = i;
                        COUNTED = i;
                        break;
                    }
                    else
                    {
                        listBox2.SelectedIndex = -1;
                        COUNTED = 0;
                    }
                }
            }
        }

        private void slider3_ValueChanged(object sender, EventArgs e)
        {
            listBox2.Focus();
        }

        private void slider3_Click(object sender, EventArgs e)
        {
            Bass.BASS_ChannelSlideAttribute(stream, BASSAttribute.BASS_ATTRIB_FREQ, slider3.Value, 0);
            Bass.BASS_ChannelGetAttribute(stream, BASSAttribute.BASS_ATTRIB_FREQ, ref freq);
            toolTip2.SetToolTip(slider3, "播放速度：" + (freq / curfreq).ToString() + "X\n按右鍵復原至預設。");
            toolTip2.Show("播放速度：" + (freq / curfreq).ToString() + "X\n按右鍵復原至預設。", slider3);
            listBox2.Focus();
        }

        private void paragraph_Click(object sender, EventArgs e)
        {
            try
            {
                long t = Bass.BASS_ChannelGetPosition(stream);
                string time = Format.MillisecondToSixtieth(Bass.BASS_ChannelBytes2Seconds(stream, t), true);
                listBox2.Items.Add("[" + time + "] ");
                listBox2.SelectedIndex = listBox2.Items.Count - 1;
            }
            catch (Exception ex)
            {
                DialogResult dl = MessageBox.Show(ex.Message + "\n是否回報此問題？", "發生未預期的錯誤", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (dl == DialogResult.Yes)
                {
                    Repair re = new Repair(ex.ToString());
                    re.ShowDialog();
                }
            }
            listBox2.Focus();
        }


        bool timtic = false;
        int tic = 0;
        private void timer3_Tick_1(object sender, EventArgs e)
        {
            if (timtic)
            {
                tic = 0;
                marquee.Start();
            }
            else
                marquee.Stop();
            timtic = !timtic;
        }

        private void marquee_Tick(object sender, EventArgs e)
        {
                if (label3.Top + label3.Height > 0)
                {
                    tic++;
                    if (tic < 33)
                    {
                        label3.Top -= 1;
                    }
                }
                else
                {
                    label3.Top = -16;
                    marqueecount++;
                }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            timer3.Start();
            timtic = false;
        }

        private void pictureBox8_MouseMove(object sender, MouseEventArgs e)
        {
            closepanel.BackColor = Color.IndianRed;
        }

        private void pictureBox8_MouseLeave(object sender, EventArgs e)
        {
            closepanel.BackColor = Color.Sienna;
        }

        private void pictureBox8_MouseDown(object sender, MouseEventArgs e)
        {
            closepanel.BackColor = Color.Firebrick;
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            timer3.Stop();
        }

        private void pictureBox8_MouseUp(object sender, MouseEventArgs e)
        {
            closepanel.BackColor = Color.Sienna;
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            MyWebClient wc = new MyWebClient();
            wc.Encoding = Encoding.UTF8;
            note = wc.DownloadString("https://sky880319.github.io/lrcmaker/post.txt").Split('&');
            wc.Dispose();
            label3.Top = 4;
            label3.Text = "";
            for(int i = 0; i < note.Length; i++)
            {
                label3.Text += Environment.NewLine + Environment.NewLine;
                label3.Text += note[i];
            }
            panel3.Width = label3.Width + 20;
            panel3.Left = (this.Width - panel3.Width) / 2;
            panel3.Visible = true;
            marqueecount = 0;
            timer3.Start();
            timtic = false;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("此功能目前為測試階段，服務由：Mojim 提供，網址：https://mojim.com/ ，若使用有上任何問題歡迎使用問題回報功能！", "LRC歌詞檔案製作工具", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Search s = new Search();
            s.ShowDialog();
            if (!s.cancel)
            {
                if (listBox1.Items.Count > 0)
                {
                    DialogResult dr = MessageBox.Show("「轉換前」區域有東西，要清除嗎？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        listBox1.Items.Clear();
                    }
                }
                listBox1.Items.AddRange(s.richTextBox1.Lines);
                Button_Enabled();
                try
                {
                    if (listBox2.Items.Count > 0)
                    {
                        DialogResult dr = MessageBox.Show("「轉換後」區域有東西，要清除嗎？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dr == DialogResult.Yes)
                        {
                            listBox2.Items.Clear();
                        }
                    }
                    listBox1.SelectedIndex = 0;
                }
                catch
                {
                    Button_Enabled_False();
                }
                listBox2.Focus();
            }
            listBox1.Refresh();
            listBox2.Refresh();
            ifsave = false;
            listBox2.Focus();
        }

        private void splitContainer1_MouseUp(object sender, MouseEventArgs e)
        {
            g.Clear(this.BackColor);
            listBox2.Focus();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            splitContainer1.SplitterDistance = 500;
            listBox2.Focus();
        }

        private void 線上尋找歌詞ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button12_Click(this, null);
        }

        bool hasChanged = true;

        private void slider1_MouseDown(object sender, MouseEventArgs e)
        {
            if (stream < 0 && e.Button == MouseButtons.Left)
                toolTip1.Show(Format.MillisecondToSixtieth(Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetPosition(stream)), false), slider1, e.X, -25);
            timer1.Stop();
            hasChanged = false;
            Bass.BASS_ChannelSetPosition(stream, slider1.Value);
            timer1.Start();
            hasChanged = true;
            checkit();
            listBox2.Focus();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(20, 0, 0, 0)), comboBox1.Location.X - 1, comboBox1.Location.Y - 1, comboBox1.Width + 1, comboBox1.Height + 1);
        }

        private void panel3_MouseEnter(object sender, EventArgs e)
        {
            timer3.Stop();
        }

        private void panel3_MouseLeave(object sender, EventArgs e)
        {
            timer3.Start();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (donateavailable)
            {
                donate = new Donate(this.BackColor);
                donate.ifc += new Donate.ifclose(ifdonatecls);
                donate.Show(this);
                donate.Location = new Point(this.Left + this.Width, this.Top);
            }
        }

        private void ifdonatecls(bool ifc)
        {
            donatebtn.Visible = ifc;
        }

        bool ifdbtnuse = true;
        int btnshowcount = 0;

        private void donatebtn_MouseMove(object sender, MouseEventArgs e)
        {
            ifdbtnuse = true;
            wowbtn.Start();
        }

        private void wowbtn_Tick(object sender, EventArgs e)
        {
            if (ifdbtnuse)
            {
                donatebtn.ImageAlign = ContentAlignment.MiddleLeft;
                if (btnshowcount < 13)
                {
                    donatebtn.Width += 5;
                    donatebtn.Left -= 5;
                    btnshowcount++;
                }
                else
                {
                    wowbtn.Stop();
                    donatebtn.Text = "贊助我們";
                    donatebtn.TextAlign = ContentAlignment.MiddleRight;
                }
            }
            else
            {
                donatebtn.Text = "";
                donatebtn.TextAlign = ContentAlignment.MiddleCenter;
                if (btnshowcount > 0)
                {
                    donatebtn.Width -= 5;
                    donatebtn.Left += 5;
                    btnshowcount--;
                }
                else
                {
                    wowbtn.Stop();
                    donatebtn.ImageAlign = ContentAlignment.MiddleCenter;
                }
            }
        }

        private void donatebtn_MouseLeave(object sender, EventArgs e)
        {
            ifdbtnuse = false;
            wowbtn.Start();
        }

        private void kbop_Click(object sender, EventArgs e)
        {
            listBox2.Focus();
            kbosf f = new kbosf(quickkey);
            this.Enabled = false;
            f.ShowDialog(this);
            quickkey = f.str;
            this.Enabled = true;
            listBox2.Focus();
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            button1.Image = null;
            button1.Text = "音訊檔\n拖曳至此";
            button1.BackColor = Color.FromArgb(20, 0, 0, 0);
            button4.Image = null;
            button4.Text = "歌詞\n拖曳至此";
            button4.BackColor = Color.FromArgb(20, 0, 0, 0);
        }

        private void Form1_DragLeave(object sender, EventArgs e)
        {
            button1.Image = Resource1.file_a;
            button1.Text = "";
            button1.BackColor = Color.FromArgb(180, 255, 255, 255);
            button4.Image = Resource1.file_t;
            button4.Text = "";
            button4.BackColor = Color.FromArgb(180, 255, 255, 255);
        }

        private void button1_DragLeave(object sender, EventArgs e)
        {
            button1.Image = Resource1.file_a;
            button4.Image = Resource1.file_t;
        }

        Graphics g; Pen p;

        private void splitContainer1_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.Show("拖曳改變大小", splitContainer1);
            g = splitContainer1.CreateGraphics();
            p = new Pen(Color.FromArgb(128,128,128), 1.5F);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            g.DrawLine(p, new Point(splitContainer1.SplitterDistance + 2, 0), new Point(splitContainer1.SplitterDistance + 2, splitContainer1.Location.Y + splitContainer1.Height));
        }

        private void splitContainer1_MouseLeave(object sender, EventArgs e)
        {
            g.Clear(this.BackColor);
        }

        private void counttime_Tick(object sender, EventArgs e)
        {
            //movehint.Visible = false;
            //g.Clear(this.BackColor);
            //counttime.Stop();
        }

        private void button4_DragEnter(object sender, DragEventArgs e)
        {
            button4.Image = null;
            button4.Text = "歌詞\n拖曳至此";
            button4.BackColor = Color.FromArgb(30, 0, 0, 0);
            button1.Image = null;
            button1.Text = "音訊檔\n拖曳至此";
            button1.BackColor = Color.FromArgb(30, 0, 0, 0);
            try
            {
                if (e.Data.GetDataPresent("FileNameW"))
                {
                    string[] data = (e.Data.GetData("FileNameW") as string[]);
                    txtfileName = data[0];
                    e.Effect = DragDropEffects.All;
                }
            }
            catch (Exception ex)
            {
                DialogResult dl = MessageBox.Show(ex.Message + "\n是否回報此問題？", "發生未預期的錯誤", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (dl == DialogResult.Yes)
                {
                    Repair re = new Repair(ex.ToString());
                    re.ShowDialog();
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            SETMENU.Show(toolsbtn, new Point(0, 28));
        }

        bool max = false;

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Bass.BASS_ChannelStop(stream);
            Bass.BASS_StreamFree(stream);
            Bass.BASS_Stop();
            Bass.BASS_Free();
        }

        private void slider3_ValueChanging(object sender, DevComponents.DotNetBar.CancelIntValueEventArgs e)
        {
            toolTip2.Hide(slider3);
            Bass.BASS_ChannelSlideAttribute(stream, BASSAttribute.BASS_ATTRIB_FREQ, slider3.Value, 0);
            Bass.BASS_ChannelGetAttribute(stream, BASSAttribute.BASS_ATTRIB_FREQ, ref freq);
            toolTip2.SetToolTip(slider3, "播放速度：" + (freq / curfreq).ToString() + "X\n按右鍵復原至預設。");
            toolTip2.Show("播放速度：" + (freq / curfreq).ToString() + "X\n按右鍵復原至預設。", slider3);
        }

        private void slider3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Bass.BASS_ChannelSlideAttribute(stream, BASSAttribute.BASS_ATTRIB_FREQ, slider3.Value, 0);
                Bass.BASS_ChannelGetAttribute(stream, BASSAttribute.BASS_ATTRIB_FREQ, ref freq);
                toolTip2.SetToolTip(slider3, "播放速度：" + (freq / curfreq).ToString() + "X\n按右鍵復原至預設。");
                toolTip2.Show("播放速度：" + (freq / curfreq).ToString() + "X\n按右鍵復原至預設。", slider3);
            }
            else if (e.Button == MouseButtons.Right)
            {
                slider3.Value = (int)curfreq;
                Bass.BASS_ChannelSlideAttribute(stream, BASSAttribute.BASS_ATTRIB_FREQ, curfreq,0);
            }
        }

        private void slider3_DoubleClick(object sender, EventArgs e)
        {

        }

        private void slider2_MouseDown(object sender, MouseEventArgs e)
        {
            Bass.BASS_ChannelSlideAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, (float)slider2.Value / 100, 0);
            toolTip2.SetToolTip(slider2, slider2.Value.ToString());
            listBox2.Focus();
        }

        private void listBox2_Leave(object sender, EventArgs e)
        {
            toolTip4.Show("快速鍵已停用。點一下圖示恢復。", pictureBox5, -5, -25);
            pictureBox5.Image = Resource1.warning;
            label2.Font = new Font(label1.Font, label1.Font.Style & ~FontStyle.Bold);
        }

        bool editmodeenable = false;

        private void editmode_Click(object sender, EventArgs e)
        {
            if (editmodeenable)
            {
                this.splitContainer1.Panel2.Controls.RemoveByKey("UC" + cursel.ToString());
                ifucshow = false;
                editmode.Text = "進入編輯模式";
                editmodeenable = false;
            }
            else
            {
                editmodeenable = true;
                editmode.Text = "退出編輯模式";
            }
        }

        private void listBox2_MouseUp(object sender, MouseEventArgs e)
        {
            if (listBox2.SelectedIndex >= 0 && editmodeenable && listBox2.SelectedIndex != cursel)
            {
                if (ifucshow)
                {
                    this.splitContainer1.Panel2.Controls.RemoveByKey("UC" + cursel.ToString());
                }
                if (Format.SixtiethToMillisecond(listBox2.SelectedItem.ToString()) != -1)
                {
                    cursel = listBox2.SelectedIndex;
                    int itemloc = e.Y / listBox2.ItemHeight;
                    updownControl uc = new updownControl()
                    {
                        Location = new Point(1, listBox2.ItemHeight * (itemloc)),
                        Name = "UC" + listBox2.SelectedIndex.ToString(),
                        BackColor = Color.FromArgb(120, 255, 255, 255)
                    };
                    uc.pictureBox1.MouseDown += new MouseEventHandler(valueup);
                    uc.pictureBox2.MouseDown += new MouseEventHandler(valuedown);
                    this.splitContainer1.Panel2.Controls.Add(uc);
                    uc.BringToFront();
                    ifucshow = true;
                }
            }
        }

        private void valueup(object sender, EventArgs e)
        {
            string TimeLine = listBox2.SelectedItem.ToString();
            double time = Format.SixtiethToMillisecond(TimeLine) + 0.01;
            listBox2.Items[listBox2.SelectedIndex] = "[" + Format.MillisecondToSixtieth(time, true) + "]" + TimeLine.Substring(TimeLine.IndexOf(']') + 1);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Repair re = new Repair();
            re.ShowDialog();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(title.ForeColor), 0, 0, this.Width - 1, this.Height - 1);
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(20, 0, 0, 0)), 0, 0, toolBar.Width - 1, toolBar.Height - 1);
        }

        private void slider1_Click(object sender, EventArgs e)
        {
            
            label4.Text = slider1.Value + ", " + Convert.ToInt32(Bass.BASS_ChannelGetPosition(stream)) + " / " + slider1.Maximum;
        }

        private void slider1_MouseMove(object sender, MouseEventArgs e)
        {
            if (stream < 0 && e.Button == MouseButtons.Left)
                toolTip1.Show(Format.MillisecondToSixtieth(Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetPosition(stream)), false), slider1, e.X, -25);
        }

        private void slider1_MouseUp(object sender, MouseEventArgs e)
        {
            toolTip1.Hide(slider1);
        }

        private void listBox2_Enter(object sender, EventArgs e)
        {
            pictureBox5.Image = Resource1.ok;
            toolTip4.Hide(pictureBox5);
            label2.Font = new Font(label1.Font, label1.Font.Style | FontStyle.Bold);
        }

        private void listBox1_Enter(object sender, EventArgs e)
        {
            label1.Font = new Font(label1.Font, label1.Font.Style | FontStyle.Bold);
        }

        private void listBox1_Leave(object sender, EventArgs e)
        {
            label1.Font = new Font(label1.Font, label1.Font.Style & ~FontStyle.Bold);
        }

        private void Button9_Click_1(object sender, EventArgs e)
        {
            CreateSerialNumber("HNK500");
        }

        private void valuedown(object sender, EventArgs e)
        {
            string TimeLine = listBox2.SelectedItem.ToString();
            double time = Format.SixtiethToMillisecond(TimeLine) - 0.01;
            listBox2.Items[listBox2.SelectedIndex] = "[" + Format.MillisecondToSixtieth(time, true) + "]" + TimeLine.Substring(TimeLine.IndexOf(']') + 1);
        }

        private void CheckInfo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (connectfail)
            {
                webBrowser1.Visible = false;
                pictureBox9.Visible = false;
                about.Width = 154;
                ifnew.Text = "離線使用中";
                panel3.Visible = false;
            }
            else
            {
                label3.Text = "";
                for (int i = 0; i < note.Length; i++)
                {
                    label3.Text += Environment.NewLine + Environment.NewLine;
                    label3.Text += note[i];
                }
                panel3.Width = label3.Width + 20;
                panel3.Left = (this.Width - panel3.Width) / 2;
                panel3.Visible = true;
                timer3.Start();
                timtic = false;
            }
            if(getfail) updatelable.Text = "download failed.";
            if (dontupdate)
            {
                updatelable.Visible = true;
                updatelable.Text = "建議更新至最新版" /*+ httxt*/;
                ifnew.Text = "最新版本：" + httxt;
                ifnew.Enabled = true;
            }
            CheckInfo.Dispose();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            
            if (max)
            {
                //this.Size = new System.Drawing.Size(950, 550);
                //listBox2.Width = listBox2.Width - 400;
                //listBox1.Width = listBox1.Width - 570;
                //listBox2.Left = listBox2.Left + 400;
                //listBox2.Height = h2;
                //listBox1.Height = h1;
                //listBox2.Top = x2;
                //listBox1.Top = x1;
                //mark.Left = mark.Left + 400;
                //changetolrc.Left = changetolrc.Left + 400;
                //undo.Left = undo.Left + 400;
                //button3.Left = button3.Left + 400;
                //button11.Left = button11.Left + 400;
                //paragraph.Left = paragraph.Left + 400;
                //panel3.Width = panel3.Width - 970;
                max = false;
            }
            else
            {
                this.Width += 100;
                //listBox2.Width = listBox2.Width + 400;
                //listBox1.Width = listBox1.Width + 570;
                //listBox2.Height = listBox2.Height + 530;
                //listBox1.Height = listBox1.Height + 530;
                //listBox2.Top = listBox2.Top - 530;
                //listBox1.Top = listBox1.Top - 530;
                //listBox2.Left = listBox2.Left - 400;
                //mark.Left = mark.Left - 400;
                //changetolrc.Left = changetolrc.Left - 400;
                //undo.Left = undo.Left + 400;
                //button3.Left = button3.Left - 400;
                //button11.Left = button11.Left - 400;
                //paragraph.Left = paragraph.Left - 400;
                //panel3.Width = panel3.Width + 970;
                //max = true;
            }
            listBox2.Focus();
        }

        ToolTip[] t = new ToolTip[0];

        void tip()
        {
            tipshow("標記歌曲資訊到歌詞裡", "標記歌名、專輯、演唱歌手以及編者，快速鍵：「" + quickkey[3] + "」。", splitContainer1.Panel1, toolBar.Location, 20, -160);
            tipshow("轉換靜態歌詞到動態", "當歌曲準備演唱到「處理前」中的其中一句歌詞時，點擊\n此處或按下快速鍵：「" + quickkey[0] + "」、「↓」，以新增時間標記到歌詞。", splitContainer1.Panel1, toolBar.Location, 20, -125);
            tipshow("返回到上一個步驟", "重新標記上一句歌詞，快速鍵：「" + quickkey[1] + "」、「↑」。", splitContainer1.Panel1, toolBar.Location, 20, -75);
            tipshow("標記一個空白段落", "間奏準備開始時，點擊此處或按下快速鍵：「Enter」，以新增一個空白標記到歌詞。", splitContainer1.Panel1, toolBar.Location, 20, -40);
            tipshow("初始化已製做的歌詞", "清除「處理後」的全部資料，快速鍵：「" + quickkey[2] + "」。", splitContainer1.Panel1, toolBar.Location, 20, -5);
            tipshow("預覽製作好的歌詞", "歌詞製作完成後，點擊此處預覽歌詞在播放器上播放的結果。", splitContainer1.Panel1, toolBar.Location, 20, 30);
            tipshow("重設版面配置", "恢復「處理前」與「處理後」區塊成預設的大小。", splitContainer1.Panel1, toolBar.Location, 20, 65);
            tipshow("開啟/尋找音訊資源", "點此選擇從本機或網路\n載入資源，或拖曳檔案到此處。", panel2, button1.Location, -130, -25);
            tipshow("初始化所有資料", "點擊後，所有項目將被重設。", panel2, allclean.Location, -116,-25);
            tipshow("開啟/尋找歌詞資源", "點此選擇從本機或網路載入資源，或拖曳檔案到此處。", panel2, button4.Location, 0,-25);
            tipshow("輸入歌詞", "手動輸入歌詞。", panel2, button2.Location, 0,-25);
            tipshow("存檔", "歌詞製作完成後，您可以儲存結果輸出成LRC或其他格式。", panel2, save.Location, save.Width -500, -0);
            tipshow("讀取與存檔的編碼方式", "每種播放器支援的編碼方式不同，這裡可以提供其他編碼選項。\n載入文件為亂碼時亦可透過此處調整編碼方式。", panel2, comboBox1.Location, comboBox1.Width - 525, -27);
            tipshow("歌詞調整控制區塊", "刪除空行：載入的歌詞串若有空白行可點此全部清除。\n精簡歌詞：歌詞製作完成後，將擁有相同歌詞的時間標記合併。\n+0.01/-0.01：歌詞製作完成後若時間延遲，點此調整整個時間\n　　　  　　　軸的偏差值。", panel2, button7.Location, button7.Width - 525, 0);
            tipshow("歌詞編輯區域1", "靜態歌詞存放區，點兩下或點按滑鼠右鍵編輯。", listBox1, new Point(0, 0), listBox1.Width / 33 * 5 + 25, listBox1.Height - 30);
            tipshow("歌詞編輯區域2", "動態歌詞存放區，點兩下或點按滑鼠右鍵編輯。", listBox2, new Point(0, 0), listBox2.Width / 33 * 5 + 10, listBox2.Height - 30);
            tipshow("焦點檢查", "當此圖示變成驚嘆號時，表示您無法使用快速鍵功能，您必須點一下驚嘆號，方可以繼續使用快速鍵。", this, toolBar.Location, 0, -35);
            tipshow("速度調整", "調整歌曲播放速度。", panel1, slider3.Location, slider3.Width -135, slider3.Height - 55);
            tipshow("工具箱", "提供編輯、設定主題、快速鍵、問題回報，以及顯示版本詳情、檢查更新。", panel1, toolsbtn.Location, 0, toolsbtn.Height);
            istipshow = true;
        }

        void tipshow(string title, string tip, IWin32Window window, Point p, int offsetX, int offsetY)
        {
            Array.Resize(ref t, t.Length + 1);
            t[t.Length - 1] = new ToolTip() { IsBalloon = false, ToolTipIcon = ToolTipIcon.Info, ToolTipTitle = title, ShowAlways = true };
            t[t.Length - 1].Show(tip, window, p.X + offsetX, p.Y + offsetY);
        }
    }

    public class MyWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest WR = base.GetWebRequest(uri);
            WR.Timeout = 5000;
            return WR;
        }
    }

    public class Format
    {
        public static string MillisecondToSixtieth(double time, bool Millisecond)
        {
            string mm = ((int)time / 60).ToString("00");
            double ss = ((int)time % 60);
            double ms = (time - (int)time);
            string t;
            if (Millisecond)
            {
                t = mm + ":" + (ss + ms).ToString("00.00");
            }
            else
            {
                t = mm + ":" + ss.ToString("00");
            }
            return t;
        }

        public static double SixtiethToMillisecond(string TimeLine)
        {
            try
            {
                TimeLine = TimeLine.Substring(TimeLine.IndexOf('[') + 1, TimeLine.IndexOf(']') - 1);
                string[] t = TimeLine.Split(':');
                double minutes = int.Parse(t[0]);
                double second = double.Parse(t[1]);
                return minutes * 60 + second;
            }
            catch
            {
                return -1;
            }
        }
    }
}
