using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lrc_Maker
{
    public partial class inputlryic : Form
    {
        public bool cancel = false;

        public inputlryic()
        {
            InitializeComponent();
        }

        public inputlryic(string st)
        {
            InitializeComponent();
            richTextBox1.Text = st;
        }

        private void ti_Click(object sender, EventArgs e)
        {
            cancel = true;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void richTextBox1_MouseLeave(object sender, EventArgs e)
        {
            richTextBox1.BackColor = Color.WhiteSmoke;
        }

        private void richTextBox1_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void inputlryic_Load(object sender, EventArgs e)
        {
            richTextBox1.SetInnerMargins(5, 5, 5, 0);
        }

        private void inputlryic_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(200, 200, 200)), 0, 0, this.Width - 1, this.Height - 1);
        }

        private void richTextBox1_MouseEnter(object sender, EventArgs e)
        {
            richTextBox1.BackColor = Color.FromArgb(240, 240, 240);
        }
    }
}
