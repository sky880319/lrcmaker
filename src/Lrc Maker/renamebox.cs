using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lrc_Maker
{
    public partial class renamebox : Form
    {
        public bool cancel = false;
        public renamebox()
        {
            InitializeComponent();
        }
        public renamebox(string test)
        {
            InitializeComponent();
            textBox1.Text = test;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cancel = true;
            this.Close();
        }

        private void textBox1_MouseMove(object sender, MouseEventArgs e)
        {
            textBox1.BackColor = Color.FromArgb(255, 180, 180, 180);
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            textBox1.BackColor = Color.DarkGray;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(200,200,200)), 0, 0, this.Width - 1, this.Height - 1);
        }
    }
}
