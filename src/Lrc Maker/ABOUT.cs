using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lrc_Maker
{
    public partial class ABOUT : Form
    {

        public ABOUT()
        {
            InitializeComponent();
        }

        private Point startPoint;
        bool pFocus = false;

        private void ABOUT_DoubleClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            startPoint = new Point(-e.X, -e.Y);
            pFocus = true;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            //如果使用者使用的是左鍵按下，意旨使用右鍵拖曳無效
            if (e.Button == MouseButtons.Left && pFocus)
            {
                Point mousePos = Control.MousePosition;
                //新視窗的位置
                mousePos.Offset(startPoint.X, startPoint.Y);
                //改變視窗位置
                Location = mousePos;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("提示：\n單行轉換　　按向下鍵\n單行取消　　按向上鍵\n音樂倒轉　　按向左鍵\n音樂快轉　　按向右鍵\n標記為歌名　按下鍵盤「Ｔ」\n標記為歌手　按下鍵盤「Ｒ」\n標記為專輯　按下鍵盤「Ｌ」\n標記為編者　按下鍵盤「Ｂ」\n全部取消　　按下鍵盤「Ｘ」\n按空白鍵可以播放／暫停音樂\n在選項上按DELETE可以刪除各項", "使用快速鍵");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("載入歌詞文件時產生亂碼。\n原因：編碼格式不合法。\n解決：將文字檔(txt)轉成系統預設的ANSI格式。");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"http://ivsnote.com/");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Repair re = new Repair();
            re.ShowDialog();
        }

        private void ABOUT_Shown(object sender, EventArgs e)
        {
            Graphics g = panel2.CreateGraphics();
            Pen p = new Pen(richTextBox1.ForeColor, 1.5F);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            g.DrawRectangle(p, new Rectangle(richTextBox1.Location.X - 5, richTextBox1.Location.Y - 5, richTextBox1.Width + 10, richTextBox1.Height + 10));
            label1.Focus();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(panel1.BackColor), 0, 0, this.Width - 1, this.Height - 1);
            label1.Focus();
        }

        private void richTextBox2_Click(object sender, EventArgs e)
        {
            label1.Focus();
        }

        private void richTextBox2_MouseDown(object sender, MouseEventArgs e)
        {
            label1.Focus();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void closebtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            pFocus = false;
        }
    }
}
