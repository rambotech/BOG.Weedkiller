namespace WeedKillerManager
{
    partial class Permissions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Permissions));
            this.rbAddAccess = new System.Windows.Forms.RadioButton();
            this.rbRemoveAccess = new System.Windows.Forms.RadioButton();
            this.gbxNTAccount = new System.Windows.Forms.GroupBox();
            this.txtNTAccount = new System.Windows.Forms.TextBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.gbxResults = new System.Windows.Forms.GroupBox();
            this.txtResults = new System.Windows.Forms.TextBox();
            this.gbxNTAccount.SuspendLayout();
            this.gbxResults.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbAddAccess
            // 
            this.rbAddAccess.AutoSize = true;
            this.rbAddAccess.Checked = true;
            this.rbAddAccess.Location = new System.Drawing.Point(12, 12);
            this.rbAddAccess.Name = "rbAddAccess";
            this.rbAddAccess.Size = new System.Drawing.Size(82, 17);
            this.rbAddAccess.TabIndex = 0;
            this.rbAddAccess.TabStop = true;
            this.rbAddAccess.Text = "Add Access";
            this.rbAddAccess.UseVisualStyleBackColor = true;
            // 
            // rbRemoveAccess
            // 
            this.rbRemoveAccess.AutoSize = true;
            this.rbRemoveAccess.Location = new System.Drawing.Point(12, 31);
            this.rbRemoveAccess.Name = "rbRemoveAccess";
            this.rbRemoveAccess.Size = new System.Drawing.Size(103, 17);
            this.rbRemoveAccess.TabIndex = 1;
            this.rbRemoveAccess.Text = "Remove Access";
            this.rbRemoveAccess.UseVisualStyleBackColor = true;
            // 
            // gbxNTAccount
            // 
            this.gbxNTAccount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxNTAccount.Controls.Add(this.txtNTAccount);
            this.gbxNTAccount.Location = new System.Drawing.Point(238, 12);
            this.gbxNTAccount.Name = "gbxNTAccount";
            this.gbxNTAccount.Size = new System.Drawing.Size(253, 66);
            this.gbxNTAccount.TabIndex = 2;
            this.gbxNTAccount.TabStop = false;
            this.gbxNTAccount.Text = "NT Account";
            // 
            // txtNTAccount
            // 
            this.txtNTAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNTAccount.Location = new System.Drawing.Point(16, 28);
            this.txtNTAccount.Name = "txtNTAccount";
            this.txtNTAccount.Size = new System.Drawing.Size(222, 20);
            this.txtNTAccount.TabIndex = 0;
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(13, 55);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(219, 23);
            this.btnApply.TabIndex = 3;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // gbxResults
            // 
            this.gbxResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxResults.Controls.Add(this.txtResults);
            this.gbxResults.Location = new System.Drawing.Point(12, 85);
            this.gbxResults.Name = "gbxResults";
            this.gbxResults.Size = new System.Drawing.Size(477, 186);
            this.gbxResults.TabIndex = 4;
            this.gbxResults.TabStop = false;
            this.gbxResults.Text = "Results";
            // 
            // txtResults
            // 
            this.txtResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResults.Location = new System.Drawing.Point(7, 20);
            this.txtResults.Multiline = true;
            this.txtResults.Name = "txtResults";
            this.txtResults.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResults.Size = new System.Drawing.Size(464, 160);
            this.txtResults.TabIndex = 0;
            // 
            // Permissions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 283);
            this.Controls.Add(this.gbxResults);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.gbxNTAccount);
            this.Controls.Add(this.rbRemoveAccess);
            this.Controls.Add(this.rbAddAccess);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(517, 321);
            this.Name = "Permissions";
            this.Text = "Permissions";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Permissions_FormClosing);
            this.gbxNTAccount.ResumeLayout(false);
            this.gbxNTAccount.PerformLayout();
            this.gbxResults.ResumeLayout(false);
            this.gbxResults.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbAddAccess;
        private System.Windows.Forms.RadioButton rbRemoveAccess;
        private System.Windows.Forms.GroupBox gbxNTAccount;
        private System.Windows.Forms.TextBox txtNTAccount;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.GroupBox gbxResults;
        private System.Windows.Forms.TextBox txtResults;
    }
}