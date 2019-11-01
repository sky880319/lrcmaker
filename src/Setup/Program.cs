using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Setup
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        static Form1 f;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new ThreadExceptionEventHandler(OnThreadException);
            f = new Form1();
            Application.Run(f);
        }

        private static void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            f.close.Enabled = true;
            f.haswrong = true;
            f.isrunning = false;
            f.errorInfo.Text = e.Exception.ToString();
            f.label1.ForeColor = Color.FromArgb(255, 46, 46);
            f.BackColor = Color.FromArgb(255, 46, 46);
            f.tabControl1.SelectedIndex = 7;
            //throw new NotImplementedException();
        }
    }
}
