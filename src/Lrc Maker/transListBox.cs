using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lrc_Maker
{
    public partial class transListBox : ListBox
    {
        public transListBox()
        {
            InitializeComponent();
            //如果为 true，控件将自行绘制，而不是通过操作系统来绘制。  
            //如果为 false，将不会引发 Paint 事件。此样式仅适用于派生自 Control 的类。  
            //this.SetStyle(ControlStyles.UserPaint, true);

            //如果为 true，控件接受 alpha 组件小于 255 的 BackColor 以模拟透明。  
            //仅在 UserPaint 位设置为 true 并且父控件派生自 Control 时才模拟透明。  
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            this.Invalidate();
            base.OnSelectedIndexChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.Focused && this.SelectedItem != null)
            {
                //设置选中行背景颜色  
                Rectangle itemRect = this.GetItemRectangle(this.SelectedIndex);
                //e.Graphics.FillRectangle(Brushes.Red, itemRect);  
                e.Graphics.FillRectangle(Brushes.LightBlue, itemRect);
            }

            base.OnPaint(e);
        }
    }
}
