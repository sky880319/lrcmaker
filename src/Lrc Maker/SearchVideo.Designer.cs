﻿namespace Lrc_Maker
{
    partial class SearchVideo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchVideo));
            this.button1 = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.UrlList = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.closebtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.textBox1 = new Lrc_Maker.uTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel5 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(661, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 24);
            this.button1.TabIndex = 2;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listView1
            // 
            this.listView1.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.UrlList});
            this.listView1.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.Location = new System.Drawing.Point(6, 65);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.ShowItemToolTips = true;
            this.listView1.Size = new System.Drawing.Size(688, 429);
            this.listView1.TabIndex = 4;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ItemActivate += new System.EventHandler(this.listView1_ItemActivate);
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "標題";
            this.columnHeader2.Width = 400;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "上傳對象";
            this.columnHeader3.Width = 150;
            // 
            // UrlList
            // 
            this.UrlList.Text = "網址";
            this.UrlList.Width = 120;
            // 
            // closebtn
            // 
            this.closebtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.closebtn.BackColor = System.Drawing.Color.Transparent;
            this.closebtn.DialogResult = System.Windows.Forms.DialogResult.No;
            this.closebtn.FlatAppearance.BorderSize = 0;
            this.closebtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.closebtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.closebtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closebtn.Font = new System.Drawing.Font("Marlett", 10F);
            this.closebtn.ForeColor = System.Drawing.Color.Gray;
            this.closebtn.Location = new System.Drawing.Point(669, -1);
            this.closebtn.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.closebtn.Name = "closebtn";
            this.closebtn.Size = new System.Drawing.Size(32, 28);
            this.closebtn.TabIndex = 26;
            this.closebtn.TabStop = false;
            this.closebtn.Text = "r";
            this.closebtn.UseVisualStyleBackColor = false;
            this.closebtn.Click += new System.EventHandler(this.closebtn_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Location = new System.Drawing.Point(6, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(688, 30);
            this.panel1.TabIndex = 28;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Location = new System.Drawing.Point(0, 28);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(688, 2);
            this.panel3.TabIndex = 31;
            this.panel3.Visible = false;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(124)))), ((int)(((byte)(85)))));
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(40, 5);
            this.panel4.TabIndex = 32;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.textBox1.Location = new System.Drawing.Point(6, 7);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(646, 18);
            this.textBox1.TabIndex = 31;
            this.textBox1.TipColor = System.Drawing.Color.Gray;
            this.textBox1.TipFont = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Italic);
            this.textBox1.TipText = "輸入關鍵字來尋找音樂";
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.label5.ForeColor = System.Drawing.Color.Gray;
            this.label5.Location = new System.Drawing.Point(9, 7);
            this.label5.Margin = new System.Windows.Forms.Padding(3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(185, 16);
            this.label5.TabIndex = 29;
            this.label5.Text = "尋找音樂 - Powered by Youtube";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.closebtn);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Location = new System.Drawing.Point(1, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(698, 29);
            this.panel2.TabIndex = 30;
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            // 
            // toolTip1
            // 
            this.toolTip1.BackColor = System.Drawing.Color.White;
            // 
            // timer1
            // 
            this.timer1.Interval = 40;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.White;
            this.panel5.Controls.Add(this.label1);
            this.panel5.Location = new System.Drawing.Point(6, 474);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(688, 20);
            this.panel5.TabIndex = 31;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = " ";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // SearchVideo
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.CancelButton = this.closebtn;
            this.ClientSize = new System.Drawing.Size(700, 500);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.panel5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SearchVideo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Search";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SearchVideo_Paint);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button closebtn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolTip toolTip1;
        private uTextBox textBox1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ColumnHeader UrlList;
        private System.Windows.Forms.Panel panel5;
        public System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}