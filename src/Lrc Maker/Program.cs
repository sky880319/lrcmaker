using System;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Lrc_Maker
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        /// 

        [STAThread]
        static void Main(string[] str)
        {
            string[] lack = { }, dlUrl = { }, name = { };
            int lackcount = 0;
            //string ver = "5.6.1506.0b";
            //bool ifnew = false;
            //string httxt;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new ThreadExceptionEventHandler(OnThreadException);
            //檢查使用者設定文件夾
            string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LRCMaker");
            if (!Directory.Exists(appDataFolder))
            {
                foreach (string filename in Directory.GetFiles(Application.StartupPath, "*.tmp"))
                {
                    string path = Path.Combine(Application.StartupPath, filename);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }
                foreach (string filename in Directory.GetFiles(Application.StartupPath, "like*"))
                {
                    string path = Path.Combine(Path.GetFullPath(Application.StartupPath), filename);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }
                Directory.CreateDirectory(appDataFolder);
            }
            if (!File.Exists(Application.StartupPath + @"\Bass.Net.dll"))
            {
                lackcount++;
                Array.Resize(ref lack, lackcount);
                Array.Resize(ref dlUrl, lackcount);
                Array.Resize(ref name, lackcount);
                lack[lackcount - 1] = "BassNetDLL";
                dlUrl[lackcount - 1] = "https://sky880319.github.io/lrcmaker/Bass.Net.dll";
                name[lackcount - 1] = "Bass.Net.dll";
            }
            if (!File.Exists(Application.StartupPath + @"\bass.dll"))
            {
                lackcount++;
                Array.Resize(ref lack, lackcount);
                Array.Resize(ref dlUrl, lackcount);
                Array.Resize(ref name, lackcount);
                lack[lackcount - 1] = "BassDLL";
                dlUrl[lackcount - 1] = "https://sky880319.github.io/lrcmaker/bass.dll";
                name[lackcount - 1] = "bass.dll";
            }
            if (!File.Exists(Application.StartupPath + @"\LRCMAKER_UPDATER.exe"))
            {
                lackcount++;
                Array.Resize(ref lack, lackcount);
                Array.Resize(ref dlUrl, lackcount);
                Array.Resize(ref name, lackcount);
                lack[lackcount - 1] = "Updater";
                dlUrl[lackcount - 1] = "https://sky880319.github.io/lrcmaker/LRCMAKER_UPDATER.exe";
                name[lackcount - 1] = "LRCMAKER_UPDATER.exe";
            }
            if (lackcount > 0)
            { 
                Application.Run(new UpdateManager(lack, dlUrl, name));
            }
            else
            {
                if (str.Length > 0)
                {
                    Application.Run(new Form1(str[0]));
                    //MyWebClient wbc = new MyWebClient();
                    //httxt = wbc.DownloadString("http://stockoko.idv.tw/vsnote/lrcmaker/updateinfo.html");
                    //if (httxt != ver)
                    //{
                    //    DialogResult updateinfo = MessageBox.Show("有新版本可用，要前往更新嗎？\n\n" + "當前版本：" + ver + "\n最新版本：" + httxt, "消息", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    //    if (updateinfo == DialogResult.OK)
                    //    {
                    //        Process.Start("LRCMAKER_UPDATER.exe");
                    //        Application.Exit();
                    //    }
                    //    else
                    //    {
                    //        ifnew = true;
                    //        if (str.Length > 0)
                    //        {
                    //            Application.Run(new Form1(str[0], ifnew, httxt));
                    //        }
                    //        else
                    //        {
                    //            Application.Run(new Form1("", ifnew, httxt));
                    //        }
                    //    }

                    //    //updatelable.Visible = true;
                    //    //updatelable.Text = "有新版本可用：" + httxt;
                    //    //updatelable.ForeColor = Color.DarkOrange;
                    //    //ifnew.Text = "有新版本可用：" + httxt;
                    //    //ifnew.Enabled = true;
                    //}
                    //wbc.Dispose();
                }
                else
                {
                    Application.Run(new Form1());
                }
            }
        }

        private static void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            DialogResult dl = MessageBox.Show(e.Exception.Message + "\n是否回報此問題？", "發生未預期的錯誤", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (dl == DialogResult.Yes)
            {
                Repair re = new Repair(e.Exception.ToString());
                re.ShowDialog();
            }
        }
    }
}
