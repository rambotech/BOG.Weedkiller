namespace WeedKillerManager
{
    partial class SetTester
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetTester));
            this.gbxResults = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvFileResults = new System.Windows.Forms.DataGridView();
            this.lblWaitMessage = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtResults = new System.Windows.Forms.TextBox();
            this.chkForceTestMode = new System.Windows.Forms.CheckBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescriptionLabel = new System.Windows.Forms.Label();
            this.btnLaunch = new System.Windows.Forms.Button();
            this.chkStopAfter = new System.Windows.Forms.CheckBox();
            this.tbarStopAfter = new System.Windows.Forms.TrackBar();
            this.lblStopAfter = new System.Windows.Forms.Label();
            this.gbxEventsOfInterest = new System.Windows.Forms.GroupBox();
            this.btnDropTemplate = new System.Windows.Forms.Button();
            this.btnSaveTemplate = new System.Windows.Forms.Button();
            this.cbxTemplateName = new System.Windows.Forms.ComboBox();
            this.btnAdjustColorBG = new System.Windows.Forms.Button();
            this.btnAdjustColorFG = new System.Windows.Forms.Button();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.btnInvertAll = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.cbxlEventsOfInterest = new System.Windows.Forms.CheckedListBox();
            this.bgwRunWeedKillAction = new System.ComponentModel.BackgroundWorker();
            this.tmrProgressUpdate = new System.Windows.Forms.Timer(this.components);
            this.gbxResults.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFileResults)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbarStopAfter)).BeginInit();
            this.gbxEventsOfInterest.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxResults
            // 
            this.gbxResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxResults.Controls.Add(this.splitContainer1);
            this.gbxResults.Location = new System.Drawing.Point(5, 215);
            this.gbxResults.Name = "gbxResults";
            this.gbxResults.Size = new System.Drawing.Size(774, 286);
            this.gbxResults.TabIndex = 0;
            this.gbxResults.TabStop = false;
            this.gbxResults.Text = "Results";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(6, 19);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvFileResults);
            this.splitContainer1.Panel1.Controls.Add(this.lblWaitMessage);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.statusStrip1);
            this.splitContainer1.Panel2.Controls.Add(this.txtResults);
            this.splitContainer1.Size = new System.Drawing.Size(762, 261);
            this.splitContainer1.SplitterDistance = 161;
            this.splitContainer1.TabIndex = 2;
            // 
            // dgvFileResults
            // 
            this.dgvFileResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvFileResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvFileResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFileResults.Location = new System.Drawing.Point(3, 3);
            this.dgvFileResults.Name = "dgvFileResults";
            this.dgvFileResults.Size = new System.Drawing.Size(756, 155);
            this.dgvFileResults.TabIndex = 2;
            // 
            // lblWaitMessage
            // 
            this.lblWaitMessage.AutoSize = true;
            this.lblWaitMessage.Location = new System.Drawing.Point(371, 57);
            this.lblWaitMessage.Name = "lblWaitMessage";
            this.lblWaitMessage.Size = new System.Drawing.Size(61, 13);
            this.lblWaitMessage.TabIndex = 3;
            this.lblWaitMessage.Text = "Please wait";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 74);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(762, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(70, 17);
            this.toolStripStatusLabel1.Text = "Initializing...";
            // 
            // txtResults
            // 
            this.txtResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResults.Location = new System.Drawing.Point(3, 3);
            this.txtResults.Multiline = true;
            this.txtResults.Name = "txtResults";
            this.txtResults.ReadOnly = true;
            this.txtResults.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResults.Size = new System.Drawing.Size(756, 68);
            this.txtResults.TabIndex = 1;
            // 
            // chkForceTestMode
            // 
            this.chkForceTestMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkForceTestMode.AutoSize = true;
            this.chkForceTestMode.Location = new System.Drawing.Point(633, 110);
            this.chkForceTestMode.Name = "chkForceTestMode";
            this.chkForceTestMode.Size = new System.Drawing.Size(107, 17);
            this.chkForceTestMode.TabIndex = 5;
            this.chkForceTestMode.Text = "Force Test Mode";
            this.chkForceTestMode.UseVisualStyleBackColor = true;
            this.chkForceTestMode.CheckedChanged += new System.EventHandler(this.chkForceTestMode_CheckedChanged);
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Location = new System.Drawing.Point(68, 3);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.Size = new System.Drawing.Size(705, 20);
            this.txtDescription.TabIndex = 4;
            // 
            // lblDescriptionLabel
            // 
            this.lblDescriptionLabel.AutoSize = true;
            this.lblDescriptionLabel.Location = new System.Drawing.Point(2, 6);
            this.lblDescriptionLabel.Name = "lblDescriptionLabel";
            this.lblDescriptionLabel.Size = new System.Drawing.Size(60, 13);
            this.lblDescriptionLabel.TabIndex = 3;
            this.lblDescriptionLabel.Text = "Description";
            // 
            // btnLaunch
            // 
            this.btnLaunch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLaunch.Location = new System.Drawing.Point(632, 147);
            this.btnLaunch.Name = "btnLaunch";
            this.btnLaunch.Size = new System.Drawing.Size(138, 53);
            this.btnLaunch.TabIndex = 6;
            this.btnLaunch.Text = "&Launch";
            this.btnLaunch.UseVisualStyleBackColor = true;
            this.btnLaunch.Click += new System.EventHandler(this.btnLaunch_Click);
            // 
            // chkStopAfter
            // 
            this.chkStopAfter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkStopAfter.AutoSize = true;
            this.chkStopAfter.Location = new System.Drawing.Point(633, 36);
            this.chkStopAfter.Name = "chkStopAfter";
            this.chkStopAfter.Size = new System.Drawing.Size(75, 17);
            this.chkStopAfter.TabIndex = 7;
            this.chkStopAfter.Text = "Stop after:";
            this.chkStopAfter.UseVisualStyleBackColor = true;
            this.chkStopAfter.CheckedChanged += new System.EventHandler(this.chkStopAfter_CheckedChanged);
            // 
            // tbarStopAfter
            // 
            this.tbarStopAfter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbarStopAfter.Enabled = false;
            this.tbarStopAfter.LargeChange = 100;
            this.tbarStopAfter.Location = new System.Drawing.Point(632, 59);
            this.tbarStopAfter.Maximum = 1000;
            this.tbarStopAfter.Minimum = 10;
            this.tbarStopAfter.Name = "tbarStopAfter";
            this.tbarStopAfter.Size = new System.Drawing.Size(138, 45);
            this.tbarStopAfter.SmallChange = 10;
            this.tbarStopAfter.TabIndex = 8;
            this.tbarStopAfter.TickFrequency = 100;
            this.tbarStopAfter.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.tbarStopAfter.Value = 10;
            this.tbarStopAfter.Scroll += new System.EventHandler(this.tbarStopAfter_Scroll);
            // 
            // lblStopAfter
            // 
            this.lblStopAfter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStopAfter.AutoSize = true;
            this.lblStopAfter.Location = new System.Drawing.Point(714, 37);
            this.lblStopAfter.Name = "lblStopAfter";
            this.lblStopAfter.Size = new System.Drawing.Size(46, 13);
            this.lblStopAfter.TabIndex = 9;
            this.lblStopAfter.Text = "(full test)";
            // 
            // gbxEventsOfInterest
            // 
            this.gbxEventsOfInterest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxEventsOfInterest.Controls.Add(this.btnDropTemplate);
            this.gbxEventsOfInterest.Controls.Add(this.btnSaveTemplate);
            this.gbxEventsOfInterest.Controls.Add(this.cbxTemplateName);
            this.gbxEventsOfInterest.Controls.Add(this.btnAdjustColorBG);
            this.gbxEventsOfInterest.Controls.Add(this.btnAdjustColorFG);
            this.gbxEventsOfInterest.Controls.Add(this.btnClearAll);
            this.gbxEventsOfInterest.Controls.Add(this.btnInvertAll);
            this.gbxEventsOfInterest.Controls.Add(this.btnSelectAll);
            this.gbxEventsOfInterest.Controls.Add(this.cbxlEventsOfInterest);
            this.gbxEventsOfInterest.Location = new System.Drawing.Point(5, 29);
            this.gbxEventsOfInterest.Name = "gbxEventsOfInterest";
            this.gbxEventsOfInterest.Size = new System.Drawing.Size(621, 180);
            this.gbxEventsOfInterest.TabIndex = 11;
            this.gbxEventsOfInterest.TabStop = false;
            this.gbxEventsOfInterest.Text = "Events Displayed";
            // 
            // btnDropTemplate
            // 
            this.btnDropTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDropTemplate.Location = new System.Drawing.Point(504, 149);
            this.btnDropTemplate.Name = "btnDropTemplate";
            this.btnDropTemplate.Size = new System.Drawing.Size(111, 21);
            this.btnDropTemplate.TabIndex = 21;
            this.btnDropTemplate.Text = "Drop Template";
            this.btnDropTemplate.UseVisualStyleBackColor = true;
            this.btnDropTemplate.Click += new System.EventHandler(this.btnDropTemplate_Click);
            // 
            // btnSaveTemplate
            // 
            this.btnSaveTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveTemplate.Location = new System.Drawing.Point(387, 149);
            this.btnSaveTemplate.Name = "btnSaveTemplate";
            this.btnSaveTemplate.Size = new System.Drawing.Size(111, 21);
            this.btnSaveTemplate.TabIndex = 20;
            this.btnSaveTemplate.Text = "Save Template";
            this.btnSaveTemplate.UseVisualStyleBackColor = true;
            this.btnSaveTemplate.Click += new System.EventHandler(this.btnSaveTemplate_Click);
            // 
            // cbxTemplateName
            // 
            this.cbxTemplateName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxTemplateName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxTemplateName.FormattingEnabled = true;
            this.cbxTemplateName.Location = new System.Drawing.Point(12, 149);
            this.cbxTemplateName.Name = "cbxTemplateName";
            this.cbxTemplateName.Size = new System.Drawing.Size(344, 21);
            this.cbxTemplateName.Sorted = true;
            this.cbxTemplateName.TabIndex = 19;
            this.cbxTemplateName.SelectedIndexChanged += new System.EventHandler(this.cbxTemplateName_SelectedIndexChanged);
            // 
            // btnAdjustColorBG
            // 
            this.btnAdjustColorBG.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdjustColorBG.Location = new System.Drawing.Point(504, 117);
            this.btnAdjustColorBG.Name = "btnAdjustColorBG";
            this.btnAdjustColorBG.Size = new System.Drawing.Size(111, 21);
            this.btnAdjustColorBG.TabIndex = 17;
            this.btnAdjustColorBG.Text = "BG Color";
            this.btnAdjustColorBG.UseVisualStyleBackColor = true;
            this.btnAdjustColorBG.Click += new System.EventHandler(this.btnAdjustColorBG_Click);
            // 
            // btnAdjustColorFG
            // 
            this.btnAdjustColorFG.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdjustColorFG.Location = new System.Drawing.Point(387, 117);
            this.btnAdjustColorFG.Name = "btnAdjustColorFG";
            this.btnAdjustColorFG.Size = new System.Drawing.Size(111, 21);
            this.btnAdjustColorFG.TabIndex = 16;
            this.btnAdjustColorFG.Text = "FG Color";
            this.btnAdjustColorFG.UseVisualStyleBackColor = true;
            this.btnAdjustColorFG.Click += new System.EventHandler(this.btnAdjustColorFG_Click);
            // 
            // btnClearAll
            // 
            this.btnClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearAll.Location = new System.Drawing.Point(245, 117);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(111, 21);
            this.btnClearAll.TabIndex = 14;
            this.btnClearAll.Text = "Clear All";
            this.btnClearAll.UseVisualStyleBackColor = true;
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // btnInvertAll
            // 
            this.btnInvertAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInvertAll.Location = new System.Drawing.Point(128, 117);
            this.btnInvertAll.Name = "btnInvertAll";
            this.btnInvertAll.Size = new System.Drawing.Size(111, 21);
            this.btnInvertAll.TabIndex = 13;
            this.btnInvertAll.Text = "Invert All";
            this.btnInvertAll.UseVisualStyleBackColor = true;
            this.btnInvertAll.Click += new System.EventHandler(this.btnInvertAll_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectAll.Location = new System.Drawing.Point(11, 117);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(111, 21);
            this.btnSelectAll.TabIndex = 12;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // cbxlEventsOfInterest
            // 
            this.cbxlEventsOfInterest.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxlEventsOfInterest.FormattingEnabled = true;
            this.cbxlEventsOfInterest.Location = new System.Drawing.Point(9, 18);
            this.cbxlEventsOfInterest.Name = "cbxlEventsOfInterest";
            this.cbxlEventsOfInterest.Size = new System.Drawing.Size(606, 94);
            this.cbxlEventsOfInterest.TabIndex = 11;
            this.cbxlEventsOfInterest.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.cbxlEventsOfInterest_ItemCheck);
            // 
            // bgwRunWeedKillAction
            // 
            this.bgwRunWeedKillAction.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwRunWeedKillAction_DoWork);
            this.bgwRunWeedKillAction.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwRunWeedKillAction_RunWorkerCompleted);
            // 
            // tmrProgressUpdate
            // 
            this.tmrProgressUpdate.Tick += new System.EventHandler(this.tmrProgressUpdate_Tick);
            // 
            // SetTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 504);
            this.Controls.Add(this.gbxEventsOfInterest);
            this.Controls.Add(this.lblStopAfter);
            this.Controls.Add(this.tbarStopAfter);
            this.Controls.Add(this.chkStopAfter);
            this.Controls.Add(this.btnLaunch);
            this.Controls.Add(this.chkForceTestMode);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDescriptionLabel);
            this.Controls.Add(this.gbxResults);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(790, 538);
            this.Name = "SetTester";
            this.Text = "Tester";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetTester_FormClosing);
            this.gbxResults.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFileResults)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbarStopAfter)).EndInit();
            this.gbxEventsOfInterest.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxResults;
        private System.Windows.Forms.CheckBox chkForceTestMode;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDescriptionLabel;
        private System.Windows.Forms.Button btnLaunch;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvFileResults;
        private System.Windows.Forms.TextBox txtResults;
        private System.Windows.Forms.CheckBox chkStopAfter;
        private System.Windows.Forms.TrackBar tbarStopAfter;
        private System.Windows.Forms.Label lblStopAfter;
        private System.Windows.Forms.GroupBox gbxEventsOfInterest;
        private System.Windows.Forms.CheckedListBox cbxlEventsOfInterest;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.Button btnInvertAll;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnAdjustColorFG;
        private System.Windows.Forms.Button btnAdjustColorBG;
        private System.Windows.Forms.Label lblWaitMessage;
        private System.Windows.Forms.Button btnDropTemplate;
        private System.Windows.Forms.Button btnSaveTemplate;
        private System.Windows.Forms.ComboBox cbxTemplateName;
        private System.ComponentModel.BackgroundWorker bgwRunWeedKillAction;
        private System.Windows.Forms.Timer tmrProgressUpdate;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}