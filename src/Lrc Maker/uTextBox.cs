using System.Drawing;
using System.Windows.Forms;

namespace Lrc_Maker
{
    public partial class uTextBox : TextBox
    {
        private string _tipText = string.Empty; //提示訊息
        public string TipText
        {
            get { return _tipText; }
            set { _tipText = value; Invalidate(); }
        }

        private Color _tipColor = SystemColors.HighlightText; //訊息顏色
        public Color TipColor
        {
            get { return _tipColor; }
            set { _tipColor = value; Invalidate(); }
        }

        private Font _tipFont = DefaultFont; //訊息字型
        public Font TipFont
        {
            get { return _tipFont; }
            set { _tipFont = value; Invalidate(); }
        }

        const int WM_PAINT = 0xF; //繪製的訊息

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_PAINT && !string.IsNullOrEmpty(_tipText) && Text.Length == 0 && Enabled && !ReadOnly && !Focused) //判斷TextBox的狀態決定要不要顯示提示訊息
            {
                TextFormatFlags formatFlags = TextFormatFlags.Default; //使用原始設定的對齊方式來顯示提示訊息

                switch (TextAlign)
                {
                    case HorizontalAlignment.Center:
                        formatFlags = TextFormatFlags.HorizontalCenter;
                        break;
                    case HorizontalAlignment.Left:
                        formatFlags = TextFormatFlags.Left;
                        break;
                    case HorizontalAlignment.Right:
                        formatFlags = TextFormatFlags.Right;
                        break;
                }

                TextRenderer.DrawText(Graphics.FromHwnd(Handle), _tipText, _tipFont, ClientRectangle, _tipColor, BackColor, formatFlags); //畫出提示訊息
            }
        }

        public uTextBox()
        {
            InitializeComponent();
        }
    }
}
