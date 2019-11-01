using System.Windows.Forms;

namespace Lrc_Maker
{
    public partial class like : Form
    {
        public bool done = false;

        public like()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            done = true;
            this.Close();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            done = false;
            this.Close();
        }
    }
}
