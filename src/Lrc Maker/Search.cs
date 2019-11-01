using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;

namespace Lrc_Maker
{
    public partial class Search : Form
    {
        string[] Lryic = { };
        private Point startPoint;
        public bool cancel = true;
        public RichTextBox richTextBox1 = new RichTextBox();
        

        public Search()
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
                countlink = 0;
                try
                {
                    WebClient wc = new WebClient();
                    wc.Encoding = Encoding.UTF8;
                    wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wb_DownloadProgressChanged);
                    wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wb_DownloadStringCompleted);
                    wc.DownloadStringAsync(new Uri("https://mojim.com/" + textBox1.Text + ".html?t3"));
                    panel3.Visible = true;
                    panel4.Width = 10;
                    timer1.Start();
                }
                catch
                {
                    MessageBox.Show("針對您搜尋的項目：「" + textBox1.Text + "」我們無法找到您要的結果！\n請再次確認：\n\n　1.拼字是否正確\n　2.是否輸入了特殊字元\n　3.網際網路連線是否流暢\n\n如果問題持續發生，請聯絡我們。", "疑？", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    timer1.Stop();
                }
            }
        }

        int countlink = 0;

        public static string[] GetLink(string htmlsource)
        {
            List<string> res = new List<string>();
            //Regex regex = new Regex (@"(?:href\s*=)(?:[\s""']*)(?!#|mailto|location.|javascript)(?<PARAM1>.*?)(?:[\s>""'])",RegexOptions.IgnoreCase);
            Regex regex = new Regex(@"(?:<a)(?!#|mailto|location.|javascript)(?<PARAM1>.*?)(?:</a>)", RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(htmlsource);

            foreach(Match match in matches)
            {
                res.Add(match.Groups["PARAM1"].Value);
            }

            return res.ToArray();
        }

        public static string GetRealLink(string htmlsource)
        {
            List<string> res = new List<string>();
            Regex regex = new Regex(@"(?:href\s*=)(?:[\s""']*)(?!#|mailto|location.|javascript)(?<PARAM1>.*?)(?:[\s>""'])", RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(htmlsource);

            foreach (Match match in matches)
            {
                res.Add(match.Groups["PARAM1"].Value);
            }

            return res[0];
        }

        public static string RemoveHTMLTag(string htmlSource, bool ifname)
        {
            //移除  javascript code.
            htmlSource = Regex.Replace(htmlSource, @"<script[\d\D]*?>[\d\D]*?</script>", String.Empty);

            //移除html tag.
            htmlSource = Regex.Replace(htmlSource, @"<[^>]*>", String.Empty);

            //移除編號
            int delect0 = htmlSource.IndexOf('.');
            if (delect0 > 0 && ifname)
            {
                htmlSource = htmlSource.Substring(delect0 + 1);
            }
            return htmlSource;
        
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            WebClient wc = new WebClient();
            wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wb_DownloadProgressChanged);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
            try
            {
                
                wc.DownloadStringAsync(new Uri(Lryic[listView1.SelectedIndices[0] * 4 + 1]));
                panel3.Visible = true;
                panel4.Width = 10;
                timer1.Start();
            }
            catch
            {
                MessageBox.Show("暫時無法讀取歌詞，請再試一次。\n如果問題持續發生，請聯絡我們。", "糟糕...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                timer1.Stop();
            }
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

        private void report_Click(object sender, EventArgs e)
        {

        }

        private void Search_Load(object sender, EventArgs e)
        {
            
        }

        void wb_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            timer1.Stop();
            panel3.Visible = false;
            try
            {
                string htmltext = e.Result;
                string s1 = htmltext.Substring(htmltext.IndexOf("<table class=\"iB\""));
                string s2 = s1.Substring(0, s1.IndexOf("</table>") + 8).Replace("<a href=\"", "<a href=\"https://mojim.com");
                string[] Links = new string[1];
                foreach (var str in GetLink(s2))
                {
                    Array.Resize(ref Links, countlink + 1);
                    Links[countlink] = "<a " + str + "</a>";
                    countlink++;
                }
                Lryic = new string[countlink + countlink / 3];
                for (int i = 0; i < countlink / 3; i++)
                {
                    Lryic[i * 4] = RemoveHTMLTag(Links[i * 3 + 2], true);
                    Lryic[i * 4 + 1] = GetRealLink(Links[i * 3 + 2]);
                    Lryic[i * 4 + 2] = RemoveHTMLTag(Links[i * 3], false);
                    Lryic[i * 4 + 3] = RemoveHTMLTag(Links[i * 3 + 1], false);
                }
                for (int i = 0; i < Lryic.Count() / 4; i++)
                {
                    listView1.Items.Add(Lryic[i * 4]);
                    listView1.Items[i].SubItems.Add(Lryic[i * 4 + 2]);
                    listView1.Items[i].SubItems.Add(Lryic[i * 4 + 3]);
                }
                if (listView1.Items.Count == 0)
                {
                    MessageBox.Show("您搜尋的項目：「" + textBox1.Text + "」沒有結果。", "抱歉", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                //string str00 = "";
                //for(int i =0; i < Lryic.Count(); i++)
                //{
                //    str00 += Lryic[i] + "\n";
                //}
                //MessageBox.Show(str00);
            }
            catch
            {
                MessageBox.Show("針對您搜尋的項目：「" + textBox1.Text + "」我們無法找到您要的結果！\n請再次確認：\n\n　1.拼字是否正確\n　2.是否輸入了特殊字元\n　3.網際網路連線是否流暢\n\n如果問題持續發生，請聯絡我們。", "疑？", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            timer1.Stop();
            panel3.Visible = false;
            try
            {
                string ds = e.Result;
                string s1 = ds.Substring(ds.IndexOf("<dl id='fsZx1' class='fsZx1' >"));
                string s2 = s1.Substring(0, s1.IndexOf("</dl>")).Replace("<br />", "\n").Replace("更多更詳盡歌詞 在 <a href=\"http://mojim.com\" >※ Mojim.com　魔鏡歌詞網 </a>", string.Empty);
                try
                {
                    s2 = s2.Substring(0, s2.IndexOf("["));
                }
                catch { };
                inputlryic f = new inputlryic(RemoveHTMLTag(s2, false));
                f.ShowDialog();

                if (!f.cancel)
                {
                    this.cancel = f.cancel;
                    this.richTextBox1.Text = f.richTextBox1.Text;
                    this.Close();
                }
                else
                {
                    this.cancel = f.cancel;
                }
            }
            catch
            {
                MessageBox.Show("暫時無法讀取歌詞，請再試一次。\n如果問題持續發生，請聯絡我們。", "糟糕...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        void wb_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            panel4.Width += (550 - panel4.Width) * e.ProgressPercentage;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (panel4.Width < 300)
                panel4.Width += 1;
        }

        private void Search_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(200, 200, 200)), 0, 0, this.Width - 1, this.Height - 1);
        }
    }
}
