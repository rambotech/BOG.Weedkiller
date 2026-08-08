namespace BOG.WeedKillerManager.App
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
			splitContainer1 = new SplitContainer();
			linkHomePage = new LinkLabel();
			pictureBox1 = new PictureBox();
			btnOK = new Button();
			tabControl1 = new TabControl();
			tabLicense = new TabPage();
			txtLicense = new TextBox();
			tabTech = new TabPage();
			pgInfo = new PropertyGrid();
			tabReadMe = new TabPage();
			txtReadMe = new TextBox();
			((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
			splitContainer1.Panel1.SuspendLayout();
			splitContainer1.Panel2.SuspendLayout();
			splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			tabControl1.SuspendLayout();
			tabLicense.SuspendLayout();
			tabTech.SuspendLayout();
			tabReadMe.SuspendLayout();
			SuspendLayout();
			// 
			// splitContainer1
			// 
			splitContainer1.Dock = DockStyle.Fill;
			splitContainer1.IsSplitterFixed = true;
			splitContainer1.Location = new Point(10, 10);
			splitContainer1.Margin = new Padding(4, 3, 4, 3);
			splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			splitContainer1.Panel1.AutoScroll = true;
			splitContainer1.Panel1.AutoScrollMinSize = new Size(199, 235);
			splitContainer1.Panel1.Controls.Add(linkHomePage);
			splitContainer1.Panel1.Controls.Add(pictureBox1);
			// 
			// splitContainer1.Panel2
			// 
			splitContainer1.Panel2.Controls.Add(btnOK);
			splitContainer1.Panel2.Controls.Add(tabControl1);
			splitContainer1.Size = new Size(717, 294);
			splitContainer1.SplitterDistance = 194;
			splitContainer1.SplitterWidth = 5;
			splitContainer1.TabIndex = 33;
			splitContainer1.Resize += splitContainer1_Resize;
			// 
			// linkHomePage
			// 
			linkHomePage.AutoSize = true;
			linkHomePage.Location = new Point(18, 241);
			linkHomePage.Margin = new Padding(4, 0, 4, 0);
			linkHomePage.Name = "linkHomePage";
			linkHomePage.Size = new Size(164, 15);
			linkHomePage.TabIndex = 49;
			linkHomePage.TabStop = true;
			linkHomePage.Tag = "http://www.bitsofgenius.com";
			linkHomePage.Text = "http://www.bitsofgenius.com";
			linkHomePage.LinkClicked += linkHomePage_LinkClicked;
			// 
			// pictureBox1
			// 
			pictureBox1.Dock = DockStyle.Top;
			pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
			pictureBox1.Location = new Point(0, 0);
			pictureBox1.Margin = new Padding(4, 3, 4, 3);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new Size(199, 271);
			pictureBox1.TabIndex = 1;
			pictureBox1.TabStop = false;
			pictureBox1.Click += pictureBox1_Click;
			// 
			// btnOK
			// 
			btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			btnOK.Location = new Point(328, 257);
			btnOK.Margin = new Padding(4, 3, 4, 3);
			btnOK.Name = "btnOK";
			btnOK.Size = new Size(182, 28);
			btnOK.TabIndex = 1;
			btnOK.Text = "OK";
			btnOK.UseVisualStyleBackColor = true;
			btnOK.Click += btnOK_Click;
			// 
			// tabControl1
			// 
			tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			tabControl1.Controls.Add(tabLicense);
			tabControl1.Controls.Add(tabTech);
			tabControl1.Controls.Add(tabReadMe);
			tabControl1.Location = new Point(5, 5);
			tabControl1.Margin = new Padding(4, 3, 4, 3);
			tabControl1.Name = "tabControl1";
			tabControl1.SelectedIndex = 0;
			tabControl1.Size = new Size(510, 246);
			tabControl1.TabIndex = 0;
			// 
			// tabLicense
			// 
			tabLicense.Controls.Add(txtLicense);
			tabLicense.Location = new Point(4, 24);
			tabLicense.Margin = new Padding(4, 3, 4, 3);
			tabLicense.Name = "tabLicense";
			tabLicense.Padding = new Padding(4, 3, 4, 3);
			tabLicense.Size = new Size(502, 218);
			tabLicense.TabIndex = 1;
			tabLicense.Text = "License";
			tabLicense.UseVisualStyleBackColor = true;
			// 
			// txtLicense
			// 
			txtLicense.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			txtLicense.Location = new Point(4, 3);
			txtLicense.Margin = new Padding(4, 3, 4, 3);
			txtLicense.Multiline = true;
			txtLicense.Name = "txtLicense";
			txtLicense.ScrollBars = ScrollBars.Vertical;
			txtLicense.Size = new Size(493, 213);
			txtLicense.TabIndex = 0;
			txtLicense.Text = resources.GetString("txtLicense.Text");
			// 
			// tabTech
			// 
			tabTech.Controls.Add(pgInfo);
			tabTech.Location = new Point(4, 24);
			tabTech.Margin = new Padding(4, 3, 4, 3);
			tabTech.Name = "tabTech";
			tabTech.Padding = new Padding(4, 3, 4, 3);
			tabTech.Size = new Size(481, 240);
			tabTech.TabIndex = 0;
			tabTech.Text = "Technical";
			tabTech.UseVisualStyleBackColor = true;
			// 
			// pgInfo
			// 
			pgInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			pgInfo.BackColor = SystemColors.Control;
			pgInfo.Location = new Point(0, 0);
			pgInfo.Margin = new Padding(4, 3, 4, 3);
			pgInfo.Name = "pgInfo";
			pgInfo.Size = new Size(479, 240);
			pgInfo.TabIndex = 47;
			pgInfo.ToolbarVisible = false;
			// 
			// tabReadMe
			// 
			tabReadMe.Controls.Add(txtReadMe);
			tabReadMe.Location = new Point(4, 24);
			tabReadMe.Margin = new Padding(4, 3, 4, 3);
			tabReadMe.Name = "tabReadMe";
			tabReadMe.Size = new Size(481, 240);
			tabReadMe.TabIndex = 2;
			tabReadMe.Text = "ReadMe.txt";
			tabReadMe.UseVisualStyleBackColor = true;
			// 
			// txtReadMe
			// 
			txtReadMe.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			txtReadMe.Location = new Point(5, 5);
			txtReadMe.Margin = new Padding(4, 3, 4, 3);
			txtReadMe.Multiline = true;
			txtReadMe.Name = "txtReadMe";
			txtReadMe.ScrollBars = ScrollBars.Both;
			txtReadMe.Size = new Size(469, 231);
			txtReadMe.TabIndex = 0;
			txtReadMe.WordWrap = false;
			// 
			// AboutBox
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(737, 314);
			Controls.Add(splitContainer1);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			Margin = new Padding(4, 3, 4, 3);
			MinimumSize = new Size(753, 353);
			Name = "AboutBox";
			Padding = new Padding(10, 10, 10, 10);
			ShowIcon = false;
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "About {0}";
			splitContainer1.Panel1.ResumeLayout(false);
			splitContainer1.Panel1.PerformLayout();
			splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
			splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			tabControl1.ResumeLayout(false);
			tabLicense.ResumeLayout(false);
			tabLicense.PerformLayout();
			tabTech.ResumeLayout(false);
			tabReadMe.ResumeLayout(false);
			tabReadMe.PerformLayout();
			ResumeLayout(false);

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
