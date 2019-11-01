namespace Lrc_Maker
{
    partial class metroProgBar
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.progress = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // progress
            // 
            this.progress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.progress.Dock = System.Windows.Forms.DockStyle.Left;
            this.progress.Location = new System.Drawing.Point(0, 0);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(50, 50);
            this.progress.TabIndex = 0;
            this.progress.MouseDown += new System.Windows.Forms.MouseEventHandler(this.progress_MouseDown);
            this.progress.MouseEnter += new System.EventHandler(this.metroProgBar_MouseEnter);
            this.progress.MouseLeave += new System.EventHandler(this.metroProgBar_MouseLeave);
            this.progress.MouseMove += new System.Windows.Forms.MouseEventHandler(this.progress_MouseMove);
            this.progress.MouseUp += new System.Windows.Forms.MouseEventHandler(this.progress_MouseUp);
            this.progress.Resize += new System.EventHandler(this.progress_Resize);
            // 
            // metroProgBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.progress);
            this.MinimumSize = new System.Drawing.Size(50, 0);
            this.Name = "metroProgBar";
            this.Size = new System.Drawing.Size(200, 50);
            this.Load += new System.EventHandler(this.metroProgBar_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.metroProgBar_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.metroProgBar_MouseDown);
            this.MouseEnter += new System.EventHandler(this.metroProgBar_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.metroProgBar_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.metroProgBar_MouseMove);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel progress;
    }
}
