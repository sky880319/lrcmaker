using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32;

namespace Lrc_Maker
{
    public partial class Repair : Form
    {
        int count = 0;
        int ans;
        private Point startPoint;
        bool test = false;
        string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LRCMaker");

        public Repair()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        public Repair(string error)
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 1;
            richTextBox1.Text += "\n\n\n----------錯誤訊息請勿刪除----------\n" + error;
        }

        public string systemBits()
        {
            if (IntPtr.Size == 8)
            {
                return "x86-64";
            }
            else if(IntPtr.Size == 4)
            {
                return "x86";
            }
            else
            {
                return "unknown";
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            startPoint = new Point(-e.X + SystemInformation.FrameBorderSize.Width, -e.Y - SystemInformation.FrameBorderSize.Height);
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

        private void textBox1_MouseMove(object sender, MouseEventArgs e)
        {
            TextBox t = (TextBox)(sender);
            t.BackColor = Color.White;
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            TextBox t = (TextBox)(sender);
            t.BackColor = Color.WhiteSmoke;
        }

        private void richTextBox1_MouseLeave(object sender, EventArgs e)
        {
            RichTextBox t = (RichTextBox)(sender);
            t.BackColor = Color.WhiteSmoke;
        }

        private void richTextBox1_MouseMove(object sender, MouseEventArgs e)
        {
            RichTextBox t = (RichTextBox)(sender);
            t.BackColor = Color.White;
        }

        public static bool IsMailAddress(string mailAddress)
        {
            bool bln;
            try
            {
                System.Net.Mail.MailAddress mail = new System.Net.Mail.MailAddress(mailAddress);
                bln = false;
            }
            catch
            {
                bln = true;
            }

            return bln;
        }

        private string usedCount()
        {
            using (StreamReader str = new StreamReader(appDataFolder + "\\USED_COUNT.tmp"))
            {
                return str.ReadLine();
            }
        }

        private string sex()
        {
            if (Female.Checked) return " (女)";
            if (Male.Checked) return " (男)";
            else return "";
        }

        bool pass = true;

        private void button3_Click(object sender, EventArgs e)
        {
            if (!pass)
            {
                this.Height = 512;
                verification();
                label7.Visible = true;
                verif.Visible = true;
                panel9.Visible = true;
                button3.Enabled = false;
                if (count == 2)
                    toolTip1.Show("請先輸入驗證問題", this, panel9.Left, panel9.Top + panel9.Height);
            }
            if (pass)
            {
                if (count >= 1)
                {
                    pass = false;
                    sendmail();
                    verification();
                    if (count > 2)
                        button3.Enabled = false;
                }
                if (count <= 1)
                {
                    DialogResult dr = MessageBox.Show("按下「是」代表您同意我們蒐集您的電腦相關資訊僅作為偵錯之用途。\n例如：主機名稱、使用情形、硬體資訊、軟體資訊。\n\n若不同意請按「否」來取消這次的傳送。", "聲明", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                        sendmail();
                }
            }
        }

        void sendmail()
        {
            button3.Text = "傳送中...";
            button3.Enabled = false;
            //檢查資料
            string a = "";
            if (textBox2.TextLength == 0)
            {
                a = "請輸入您的電子信箱\n";
            }
            else if (IsMailAddress(textBox2.Text))
            {
                a = "請輸入正確的電子信箱格式\n";
            }
            if (richTextBox1.TextLength < 10)
            {
                a = a + "詳細說明中，字數不應該小於10個字\n";
            }
            if (textBox3.TextLength < 5)
            {
                a = a + "說明字數不應該小於5個字";
            }
            if (a != "")
            {
                MessageBox.Show(a, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                string osVersion = (string)reg.GetValue("ProductName") + " " + (string)reg.GetValue("ReleaseId") + " (OS 組建 " + (string)reg.GetValue("CurrentBuild") + ")";
                
                string rtb = "";
                int count = 0;
                string nowTime = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString() + " - " + DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00");
                while (true)
                {
                    try { rtb += richTextBox1.Lines[count] + "<br>"; }
                    catch { break; }
                    count++;
                }
                string body =
                    "<meta charset=\"utf-8\">" +
                    "<center>" +
                    "<tbody>" +
                    "<table style=\"width: 600px; height: 40px; color: #FFF; background-color: steelblue; margin: 0; padding-right: 20px; font-family:Arial; font-size: 12px; font-weight: bold; border-top-left-radius: 5px; border-top-right-radius: 5px;\">" +
                    "<td style=\"vertical-align:middle; text-align: right;\">LRCMaker Report Center</td>" +
                    "</table>" +
                    "<table style=\"width: 600px;background-color: #EEE;color: #555;padding: 20px;\">" +
                    "<tr>" +
                    "<td style=\"padding: 10px; border-bottom: thin #DDD inset;\" width=\"30%\">稱呼：</td>" +
                    "<td style=\"padding: 10px; border-bottom: thin #DDD inset;\">" + textBox1.Text + sex() + "</td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td style=\"padding: 10px; border-bottom: thin #DDD inset;\">電子信箱：</td>" +
                    "<td style=\"padding: 10px; border-bottom: thin #DDD inset;\">" + textBox2.Text + "</td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td style=\"padding: 10px; border-bottom: thin #DDD inset;\">主機名稱：</td>" +
                    "<td style=\"padding: 10px; border-bottom: thin #DDD inset;\">" + Environment.MachineName + "</td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td style=\"padding: 10px; border-bottom: thin #DDD inset;\">系統版本：</td>" +
                    "<td style=\"padding: 10px; border-bottom: thin #DDD inset;\">" + osVersion + "</td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td style=\"padding: 10px; border-bottom: thin #DDD inset;\">處理器架構：</td>" +
                    "<td style=\"padding: 10px; border-bottom: thin #DDD inset;\">" + systemBits() + "</td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td style=\"padding: 10px; border-bottom: thin #DDD inset;\">軟體版本：</td>" +
                    "<td style=\"padding: 10px; border-bottom: thin #DDD inset;\">" + Form1.ver + "</td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td style=\"padding: 10px; border-bottom: thin #DDD inset;\">發送時間：</td>" +
                    "<td style=\"padding: 10px; border-bottom: thin #DDD inset;\">" + nowTime + "</td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td style=\"padding: 10px; border-bottom: thin #DDD inset;\">已使用次數：</td>" +
                    "<td style=\"padding: 10px; border-bottom: thin #DDD inset;\">" + usedCount() + "</td>" +
                    "</tr>" +
                    "</table>" +
                    "<table style=\"width: 600px;background-color: #EEE;color: #555;padding: 20px; border-bottom-left-radius: 5px; border-bottom-right-radius: 5px;\">" +
                    "<tr style=\"background-color: #DFDFDF;\">" +
                    "<td style=\"padding: 10px; border-radius: 5px; font-weight: bold;\">[" + comboBox1.SelectedItem.ToString() + "] " + textBox3.Text + "</td>" +
                    "</tr>" +
                    "<tr style=\"background-color: #FFF;\">" +
                    "<td style=\"padding: 20px; border-radius: 5px; border: thin #DDD solid; word-break: break-all; word-wrap:break-word;\">" + rtb + "</td>" +
                    "</tr>" +
                    "</table>" +
                    "</tbody>" +
                    "</center>";

                MailMessage myMail = new MailMessage
                {
                    IsBodyHtml = true,
                    Priority = MailPriority.High,
                    Subject = "LRCMAKER [" + comboBox1.SelectedItem.ToString() + "] " + textBox3.Text,
                    Body = body
                    //Body = "稱呼：" + textBox1.Text + "<br><br>性別：" + sex + "<br><br>電子信箱：" + textBox2.Text + "<br><br>主機名稱：" + Environment.MachineName + "<br><br>系統版本：" + Environment.OSVersion.VersionString + " / " + sys + "<br><br>軟體版本：" + f.ver + "<br><br>問題：[" + comboBox1.SelectedItem.ToString() + "]" + textBox3.Text + "<br><br>詳細說明：" + rtb
                };
                myMail.From = new MailAddress("ivsnote@zoho.com", textBox1.Text);
                myMail.To.Add("ivsnote@zoho.com");
                //myMail.Attachments.Add(new Attachment("d:\\tse\\stock\\logo\\博谷網介紹.pdf"));

                try
                {
                    SmtpClient SMTP = new SmtpClient();
                    SMTP.Host = "smtp.zoho.com";
                    SMTP.Port = 587;
                    SMTP.DeliveryMethod = SmtpDeliveryMethod.Network;
                    SMTP.EnableSsl = true;
                    SMTP.UseDefaultCredentials = false;
                    SMTP.Credentials = new NetworkCredential(@"ivsnote@zoho.com", @"qv=142t~[;ON[*AOrM");
                    SMTP.Timeout = 10000;
                    SMTP.Send(myMail);
                    SMTP.Dispose();
                    MessageBox.Show("謝謝您，訊息已傳送，我們將盡快處理！", "提示");
                    count++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "發生錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show("訊息未送出。", "提示");
                }
            }
            button3.Text = "回報問題";
            button3.Enabled = true;
        }

        private void markform_Load(object sender, EventArgs e)
        {
            richTextBox1.SetInnerMargins(5, 5, 5, 0);
            verification();
            this.Height = 472;
            if(test)
            {
                Male.Select();
                textBox1.Text = "林子揚";
                textBox2.Text = "sky880319@gmail.com";
                comboBox1.SelectedIndex = 1;
                textBox3.Text = "問題回報功能測試";
                richTextBox1.Text = "測試問題回報功能\n\nPellentesque eu dictum odio, eget euismod ipsum. Aliquam vehicula metus ut tempor pulvinar. Maecenas dignissim nulla sit amet mauris faucibus rutrum. Aenean rhoncus, enim at efficitur tincidunt, neque ante pharetra eros, sit amet commodo eros enim sed leo. Fusce quis mauris ut dolor efficitur consectetur quis et nibh. Nunc viverra.\n\n善小電程，到你不同，著上越自行事學於問試黑經，後業再化先、低空美們成不期近；的時何活我工良快致全轉國兩受城一因得教而，收把河隨的深話是人在；及弟便師物的中臺於舉？業到爸到道去生起在，腦代環洲有年情對在孩走當那立，可車護就前歡果當政作三。";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex == 0)
            {
                richTextBox1.Text = "我希望加入OO功能，因為...";
            }
            else if(comboBox1.SelectedIndex == 1)
            {
                richTextBox1.Text = "檔案格式：（音樂檔的格式，例如：MP3）\n問題發生頻率：\n錯誤訊息：（彈出的錯誤訊息，若沒有請空白）\n詳述問題經過：（詳細描述事發經過）";
            }
            else if(comboBox1.SelectedIndex == 2)
            {
                richTextBox1.Text = "";
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if(textBox4.Text != ans.ToString())
            {
                button3.Enabled = false;
                toolTip1.Show("驗證錯誤", this, panel9.Left, panel9.Top + panel9.Height);
            }
            else
            {
                button3.Enabled = true;
                toolTip1.Hide(this);
                pass = true;
            }
        }

        void verification ()
        {
            Random r = new Random();
            int f = r.Next(0, 10), s = r.Next(0, 10);
            verif.Text = f.ToString() + "+" + s.ToString() + " =";
            panel9.Left = verif.Left + verif.Width + 2;
            ans = f + s;
        }

        private void closebtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Repair_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(200, 200, 200)), 0, 0, this.Width - 1, this.Height - 1);
        }
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
