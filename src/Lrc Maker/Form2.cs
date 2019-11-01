using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lrc_Maker
{
    public partial class Form2 : Form
    {
        public int selected;

        public Form2(string type, string searchEngine, bool connState)
        {
            InitializeComponent();
            if (!connState) button2.Enabled = false;
            label2.Text = string.Format(label2.Text, type);
            label3.Text = string.Format(label3.Text, searchEngine, type);
            switch (searchEngine)
            {
                case "YouTube":
                    button2.Image = Properties.Resources.YouTube11;
                    button2.BackColor = Color.FromArgb(255, 46, 46);
                    break;
                default:
                    button2.Image = Properties.Resources.View_Earth;
                    button2.BackColor = Color.FromArgb(255, 145, 36);
                    break;
            }
        }

        private void closebtn_Click(object sender, EventArgs e)
        {
            selected = 0;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            selected = 1;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            selected = 2;
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(200, 200, 200)), 0, 0, this.Width - 1, this.Height - 1);
        }
    }
}
