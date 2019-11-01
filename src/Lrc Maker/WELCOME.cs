using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Lrc_Maker
{
    public partial class WELCOME : Form
    {
        private Point startPoint;
        const int CS_DropSHADOW = 0x20000;
        const int GCL_STYLE = (-26);
        //声明Win32 API
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex);

        public WELCOME()
        {
            InitializeComponent();
            //SetClassLong(this.Handle, GCL_STYLE, GetClassLong(this.Handle, GCL_STYLE) | CS_DropSHADOW); //API函数加载，实现窗体边框阴影效果
        }

        private void WELCOME_Load(object sender, EventArgs e)
        {
            tabControl1.SelectedTabIndex = 0;
            labelX1.Text = "　第一次使用本軟體？\n　稍後將帶您熟悉本軟體的操作方式：\n\n　　　" + Form1.ver + " 版後的異動／更新歷史\n　　　軟體的操作方式";
        }

        private void WELCOME_MouseDown(object sender, MouseEventArgs e)
        {
            startPoint = new Point(-e.X, -e.Y);
        }

        private void WELCOME_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void WELCOME_MouseMove(object sender, MouseEventArgs e)
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void NEXT_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTabIndex = tabControl1.SelectedTabIndex + 1;
        }

        private void BACK_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTabIndex = tabControl1.SelectedTabIndex - 1;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tabControlPanel1_Click(object sender, EventArgs e)
        {

        }

        private void closebtn_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("您即將略過重要教學！\n除非您已經熟悉新功能，否則：\n按「取消」後繼續", "貼心提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dr == DialogResult.OK)
                this.Close();
        }
    }
}
