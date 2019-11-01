using System;
using System.IO;
using System.Windows.Forms;

namespace Lrc_Maker
{
    public partial class survey : Form
    {
        string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LRCMaker");

        public survey()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"https://goo.gl/forms/V3DkUzNKUot1QZfm1");
            using (StreamWriter stw = new StreamWriter(appDataFolder + "\\survey_19.tmp"))
            {
                stw.Write(-1);
            }
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            using (StreamWriter stw = new StreamWriter(appDataFolder + "\\survey_19.tmp"))
            {
                stw.Write(-1);
            }
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
