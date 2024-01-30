namespace VideoDownloadForm
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tbUrl = new System.Windows.Forms.TextBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.tvStatus = new System.Windows.Forms.TreeView();
            this.button1 = new System.Windows.Forms.Button();
            this.btnUpdateTreeview = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // tbUrl
            // 
            this.tbUrl.Location = new System.Drawing.Point(12, 12);
            this.tbUrl.Name = "tbUrl";
            this.tbUrl.Size = new System.Drawing.Size(1313, 31);
            this.tbUrl.TabIndex = 0;
            this.tbUrl.Text = "https://ak84.com/playr/150660-1-1.html";
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(1331, 12);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(112, 34);
            this.btnDownload.TabIndex = 1;
            this.btnDownload.Text = "download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // tvStatus
            // 
            this.tvStatus.FullRowSelect = true;
            this.tvStatus.Location = new System.Drawing.Point(12, 61);
            this.tvStatus.Name = "tvStatus";
            this.tvStatus.Size = new System.Drawing.Size(1495, 823);
            this.tvStatus.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1449, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 34);
            this.button1.TabIndex = 3;
            this.button1.Text = "download";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnUpdateTreeview
            // 
            this.btnUpdateTreeview.Location = new System.Drawing.Point(1567, 12);
            this.btnUpdateTreeview.Name = "btnUpdateTreeview";
            this.btnUpdateTreeview.Size = new System.Drawing.Size(157, 34);
            this.btnUpdateTreeview.TabIndex = 4;
            this.btnUpdateTreeview.Text = "update treenode";
            this.btnUpdateTreeview.UseVisualStyleBackColor = true;
            this.btnUpdateTreeview.Click += new System.EventHandler(this.btnUpdateTreeview_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1780, 994);
            this.Controls.Add(this.btnUpdateTreeview);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tvStatus);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.tbUrl);
            this.Name = "Form1";
            this.Text = "YR";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox tbUrl;
        private Button btnDownload;
        private TreeView tvStatus;
        private Button button1;
        private Button btnUpdateTreeview;
        private System.Windows.Forms.Timer timer1;
    }
}