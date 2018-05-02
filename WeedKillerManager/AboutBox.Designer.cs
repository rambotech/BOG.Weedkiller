namespace WeedKillerManager
{
    partial class AboutBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.linkHomePage = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabLicense = new System.Windows.Forms.TabPage();
            this.txtLicense = new System.Windows.Forms.TextBox();
            this.tabTech = new System.Windows.Forms.TabPage();
            this.pgInfo = new System.Windows.Forms.PropertyGrid();
            this.tabReadMe = new System.Windows.Forms.TabPage();
            this.txtReadMe = new System.Windows.Forms.TextBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabLicense.SuspendLayout();
            this.tabTech.SuspendLayout();
            this.tabReadMe.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(-3, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.AutoScrollMinSize = new System.Drawing.Size(199, 235);
            this.splitContainer1.Panel1.Controls.Add(this.linkHomePage);
            this.splitContainer1.Panel1.Controls.Add(this.pictureBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnOK);
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(634, 274);
            this.splitContainer1.SplitterDistance = 204;
            this.splitContainer1.TabIndex = 33;
            this.splitContainer1.Resize += new System.EventHandler(this.splitContainer1_Resize);
            // 
            // linkHomePage
            // 
            this.linkHomePage.AutoSize = true;
            this.linkHomePage.Location = new System.Drawing.Point(34, 248);
            this.linkHomePage.Name = "linkHomePage";
            this.linkHomePage.Size = new System.Drawing.Size(144, 13);
            this.linkHomePage.TabIndex = 49;
            this.linkHomePage.TabStop = true;
            this.linkHomePage.Tag = "http://www.bitsofgenius.com";
            this.linkHomePage.Text = "http://www.bitsofgenius.com";
            this.linkHomePage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkHomePage_LinkClicked);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::WeedKillerManager.Properties.Resources.bogwf;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(199, 235);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(263, 242);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(156, 24);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabLicense);
            this.tabControl1.Controls.Add(this.tabTech);
            this.tabControl1.Controls.Add(this.tabReadMe);
            this.tabControl1.Location = new System.Drawing.Point(4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(419, 232);
            this.tabControl1.TabIndex = 0;
            // 
            // tabLicense
            // 
            this.tabLicense.Controls.Add(this.txtLicense);
            this.tabLicense.Location = new System.Drawing.Point(4, 22);
            this.tabLicense.Name = "tabLicense";
            this.tabLicense.Padding = new System.Windows.Forms.Padding(3);
            this.tabLicense.Size = new System.Drawing.Size(411, 206);
            this.tabLicense.TabIndex = 1;
            this.tabLicense.Text = "License";
            this.tabLicense.UseVisualStyleBackColor = true;
            // 
            // txtLicense
            // 
            this.txtLicense.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLicense.Location = new System.Drawing.Point(3, 3);
            this.txtLicense.Multiline = true;
            this.txtLicense.Name = "txtLicense";
            this.txtLicense.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLicense.Size = new System.Drawing.Size(405, 204);
            this.txtLicense.TabIndex = 0;
            this.txtLicense.Text = resources.GetString("txtLicense.Text");
            // 
            // tabTech
            // 
            this.tabTech.Controls.Add(this.pgInfo);
            this.tabTech.Location = new System.Drawing.Point(4, 22);
            this.tabTech.Name = "tabTech";
            this.tabTech.Padding = new System.Windows.Forms.Padding(3);
            this.tabTech.Size = new System.Drawing.Size(411, 206);
            this.tabTech.TabIndex = 0;
            this.tabTech.Text = "Technical";
            this.tabTech.UseVisualStyleBackColor = true;
            // 
            // pgInfo
            // 
            this.pgInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgInfo.Location = new System.Drawing.Point(0, 0);
            this.pgInfo.Name = "pgInfo";
            this.pgInfo.Size = new System.Drawing.Size(411, 208);
            this.pgInfo.TabIndex = 47;
            this.pgInfo.ToolbarVisible = false;
            // 
            // tabReadMe
            // 
            this.tabReadMe.Controls.Add(this.txtReadMe);
            this.tabReadMe.Location = new System.Drawing.Point(4, 22);
            this.tabReadMe.Name = "tabReadMe";
            this.tabReadMe.Size = new System.Drawing.Size(411, 206);
            this.tabReadMe.TabIndex = 2;
            this.tabReadMe.Text = "ReadMe.txt";
            this.tabReadMe.UseVisualStyleBackColor = true;
            // 
            // txtReadMe
            // 
            this.txtReadMe.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReadMe.Location = new System.Drawing.Point(4, 4);
            this.txtReadMe.Multiline = true;
            this.txtReadMe.Name = "txtReadMe";
            this.txtReadMe.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtReadMe.Size = new System.Drawing.Size(403, 201);
            this.txtReadMe.TabIndex = 0;
            this.txtReadMe.WordWrap = false;
            // 
            // AboutBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 273);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimumSize = new System.Drawing.Size(579, 275);
            this.Name = "AboutBox";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About {0}";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabLicense.ResumeLayout(false);
            this.tabLicense.PerformLayout();
            this.tabTech.ResumeLayout(false);
            this.tabReadMe.ResumeLayout(false);
            this.tabReadMe.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel linkHomePage;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabLicense;
        private System.Windows.Forms.TabPage tabTech;
        private System.Windows.Forms.PropertyGrid pgInfo;
        private System.Windows.Forms.TextBox txtLicense;
        private System.Windows.Forms.TabPage tabReadMe;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtReadMe;

    }
}
