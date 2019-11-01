using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;



namespace Lrc_Maker
{
    public partial class markform : Form
    {
        public markform()
        {
            InitializeComponent();
        }
        private Point startPoint;
        public bool result = false;
        public int cnt = 0;
        public string ti = "", al = "", ar = "", by = "";
        public Dictionary<string, string> marks = new Dictionary<string, string>() { { "ti", "" }, { "ar", "" }, { "al", "" }, { "by", "" } };

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        //{
        //    pictureBox2.Image = Resource1.close4;
        //}

        //private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        //{
        //    pictureBox2.Image = Resource1.close3;
        //}

        //private void pictureBox2_MouseLeave(object sender, EventArgs e)
        //{
        //    pictureBox2.Image = Resource1.close2;
        //}

        //private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        //{
        //    pictureBox2.Image = Resource1.close3;
        //}

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

        private void textBox1_MouseMove(object sender, MouseEventArgs e)
        {
            textBox1.BackColor = Color.FromArgb(255, 180, 180, 180);
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            textBox1.BackColor = Color.DarkGray;
        }

        private void textBox2_MouseLeave(object sender, EventArgs e)
        {
            textBox2.BackColor = Color.DarkGray;
        }

        private void textBox2_MouseMove(object sender, MouseEventArgs e)
        {
            textBox2.BackColor = Color.FromArgb(255, 180, 180, 180);
        }

        private void textBox3_MouseLeave(object sender, EventArgs e)
        {
            textBox3.BackColor = Color.DarkGray;
        }

        private void textBox3_MouseMove(object sender, MouseEventArgs e)
        {
            textBox3.BackColor = Color.FromArgb(255, 180, 180, 180);
        }

        private void textBox4_MouseLeave(object sender, EventArgs e)
        {
            textBox3.BackColor = Color.DarkGray;
        }

        private void textBox4_MouseMove(object sender, MouseEventArgs e)
        {
            textBox3.BackColor = Color.FromArgb(255, 180, 180, 180);
        }

        private void markform_Load(object sender, EventArgs e)
        {

        }

        private void ok_Click(object sender, EventArgs e)
        {

            if (!(textBox1.Text == ""))
            {
                marks["ti"] = textBox1.Text;
            }
            if (!(textBox2.Text == ""))
            {
                marks["ar"] = textBox2.Text;
            }
            if (!(textBox3.Text == ""))
            {
                marks["al"] = textBox3.Text;
            }
            if (!(textBox4.Text == ""))
            {
                marks["by"] = textBox4.Text;
            }
            result = true;
            this.Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void markform_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(200, 200, 200)), 0, 0, this.Width - 1, this.Height - 1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox2.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox3.Focus();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox4.Focus();
        }
    }
}
