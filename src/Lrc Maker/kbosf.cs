using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Lrc_Maker
{
    public partial class kbosf : Form
    {
        private Point startPoint;
        public string[] str;
        string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LRCMaker");

        public kbosf()
        {
            InitializeComponent();
        }

        public kbosf(string[] sc)
        {
            InitializeComponent();
            str = sc;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            startPoint = new Point(-e.X, -e.Y);
            toolTip1.Hide(this);
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
            toolTip1.Hide(this);
            TextBox txt = (TextBox)(sender);
            string keydown = e.KeyCode.ToString();
            string[] key = { w.Text, s.Text, a.Text, d.Text, x.Text, r.Text };
            int count = 0;
            foreach (string k in key)
            {
                if (keydown == k)
                {
                    count++;
                }
            }

            Keys[] used = { Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Space, Keys.Enter, Keys.Delete, Keys.Back, Keys.Subtract, Keys.Add };
            foreach (Keys k in used)
            {
                if(e.KeyCode == k)
                {
                    txt.Text = "";
                    toolTip1.Show("快速鍵已被使用。", this, txt.Left, txt.Top + txt.Height);
                    break;
                }
                else
                {
                    if (count < 1)
                    {
                        txt = (TextBox)(sender);
                        txt.Text = keydown;
                    }
                    else if (keydown != txt.Text)
                    {
                        txt.Text = "";
                        toolTip1.Show("不能使用相同的按鍵設定快速鍵。", this, txt.Left, txt.Top + txt.Height);
                    }
                }
            }
            
        }

        private void defaultkey_Click(object sender, EventArgs e)
        {
            w.Text = "W";
            s.Text = "S";
            a.Text = "A";
            d.Text = "D";
            x.Text = "X";
            r.Text = "R";
        }

        private void d_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            str = new string[] { s.Text, w.Text, x.Text, r.Text, a.Text, d.Text };
            string st = "";
            for (int i = 0; i < str.Length; i++)
            {
                st += str[i];
                if (i < str.Length - 1)
                {
                    st += Environment.NewLine;
                }
            }
            using (StreamWriter stw = new StreamWriter(appDataFolder + "\\SETTING_LRCMAKER_SHORTCUT.ini"))
            {
                stw.Write(st);
                stw.Close();
            }
            this.Close();
        }

        private void kbosf_Load(object sender, EventArgs e)
        {
            w.Text = str[1];
            s.Text = str[0];
            a.Text = str[4];
            d.Text = str[5];
            x.Text = str[2];
            r.Text = str[3];
        }

        private void kbosf_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(200, 200, 200)), 0, 0, this.Width - 1, this.Height - 1);
        }
    }
}
