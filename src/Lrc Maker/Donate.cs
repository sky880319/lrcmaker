using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lrc_Maker
{
    public partial class Donate : Form
    {
        public delegate void ifclose(bool clo);
        public event ifclose ifc;
        bool a = false;

        public Donate()
        {
            InitializeComponent();
        }

        public Donate(Color clr)
        {
            InitializeComponent();
            SETCOLOR(clr);
        }

        public void SETCOLOR(Color clr)
        {
            //int R = 255, G = 255, B = 255;
            //if (clr.R <= 235) R = clr.R + 20;
            //if (clr.G <= 235) G = clr.G + 20;
            //if (clr.B <= 235) B = clr.B + 20;
            //clr = Color.FromArgb(R, G, B);
            //this.BackColor = clr;
            //richTextBox1.BackColor = clr;
            //developerbtn.ForeColor = clr;
        }

        private void Donate_Load(object sender, EventArgs e)
        {
            this.Width = 0;
            a = false;
            hidetime.Start();
            richTextBox1.SelectAll();
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox1.Select(0, 0);
            if (this.ifc != null)
            {
                this.ifc(false);
            }
            this.Focus();
        }

        public void label4_Click(object sender, EventArgs e)
        {
            //this.Width = 8;
            //this.Height = 100;
            a = true;
            //this.Location = new Point(this.Left, this.Top + 40);
            hidetime.Start();
        }

        private void developerbtn_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"https://ivsnote.com/donate/");
            //this.Width = 8;
            //this.Height = 100;
            a = true;
            //this.Location = new Point(this.Left, this.Top + 40);
            hidetime.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this.Width = 200;
            //this.Height = 300;
            a = false;
            //this.Location = new Point(this.Left, this.Top - 40);
            hidetime.Start();
        }

        private void hidetime_Tick(object sender, EventArgs e)
        {
            if (!a)
            {
                if (this.Width >= 190)
                {
                    hidetime.Stop();
                    this.Width = 200;
                    this.CreateGraphics().DrawRectangle(new Pen(Color.Goldenrod), 0, 0, this.Width - 1, this.Height - 1);
                }
                else this.Width += 10;
            }
            else
            {
                if (this.Width <= 10)
                {
                    if (this.ifc != null)
                    {
                        this.ifc(true);
                    }
                    this.Close();
                }
                else this.Width -= 10;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_Enter(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void richTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            label5.Focus();
        }

        private void richTextBox1_Click(object sender, EventArgs e)
        {
            label5.Focus();
        }

        private void richTextBox1_MouseClick(object sender, MouseEventArgs e)
        {
            label5.Focus();
        }
        private void Donate_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void Donate_Shown(object sender, EventArgs e)
        {

        }
    }
}
