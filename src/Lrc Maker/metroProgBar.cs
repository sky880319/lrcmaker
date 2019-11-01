using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lrc_Maker
{
    public partial class metroProgBar : UserControl
    {
        [Browsable(true), Category("外觀")]
        public int Maximum { get; set; } = 100;
        private int dispWidth() => this.Width - Padding.Left - Padding.Right;
        [Browsable(true), Category("外觀")]
        public int Value
        {
            get => val;
            set
            {
                val = value;
                progress.Width = (int)(dispWidth() * (double)val / Maximum);
            }
        }
        private int val;

        [Browsable(true), Category("外觀")]
        public Color Color { get { return progress.BackColor; } set { progress.BackColor = value; } }




        public metroProgBar()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        private void metroProgBar_Load(object sender, EventArgs e)
        {

        }

        private void metroProgBar_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color), progress.Width + Padding.Left, progress.Top + progress.Height / 2, this.Width - Padding.Right, progress.Top + progress.Height / 2);
        }

        int mousePositionX = -1;
        bool progressBarHandle = false;

        private void ValueChange(int mousePositionX)
        {
            if (mousePositionX <= dispWidth())
            {
                Value = (int)(Maximum * (double)mousePositionX / dispWidth());
            }
        }

        private void metroProgBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (progressBarHandle)
                    mousePositionX = e.X;
                else
                    mousePositionX = e.X - Padding.Left;
                ValueChange(mousePositionX);
            }
            progressBarHandle = false;
        }

        private void progress_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                progressBarHandle = true;
                OnMouseDown(e);
            }
        }

        private void metroProgBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                OnMouseDown(e);
        }

        private void progress_MouseDown(object sender, MouseEventArgs e)
        {
            progressBarHandle = true;
            OnMouseDown(e);
        }

        private void progress_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }

        Color ori;

        private void metroProgBar_MouseEnter(object sender, EventArgs e)
        {
            ori = progress.BackColor;
            progress.BackColor = Color.FromArgb(ori.A - 55, ori);
            this.Refresh();
        }

        private void metroProgBar_MouseLeave(object sender, EventArgs e)
        {
            progress.BackColor = ori;
            this.Refresh();
        }

        private void progress_MouseUp(object sender, MouseEventArgs e)
        {
            OnMouseUp(e);
        }
    }
}
