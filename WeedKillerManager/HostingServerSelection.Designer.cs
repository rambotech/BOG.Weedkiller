namespace WeedKillerManager
{
    /// <summary>
    /// 
    /// </summary>
    partial class HostingServerSelection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HostingServerSelection));
            this.rbScope = new System.Windows.Forms.RadioButton();
            this.rbScope2 = new System.Windows.Forms.RadioButton();
            this.gbxMachineList = new System.Windows.Forms.GroupBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtMachineName = new System.Windows.Forms.TextBox();
            this.lbxMachines = new System.Windows.Forms.ListBox();
            this.btnAccept = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbxMachineList.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbScope
            // 
            this.rbScope.AutoSize = true;
            this.rbScope.Checked = true;
            this.rbScope.Location = new System.Drawing.Point(13, 13);
            this.rbScope.Name = "rbScope";
            this.rbScope.Size = new System.Drawing.Size(241, 17);
            this.rbScope.TabIndex = 0;
            this.rbScope.TabStop = true;
            this.rbScope.Text = "These configurations can run on any machine";
            this.rbScope.UseVisualStyleBackColor = true;
            this.rbScope.Click += new System.EventHandler(this.rbScope_Click);
            // 
            // rbScope2
            // 
            this.rbScope2.AutoSize = true;
            this.rbScope2.Location = new System.Drawing.Point(13, 37);
            this.rbScope2.Name = "rbScope2";
            this.rbScope2.Size = new System.Drawing.Size(325, 17);
            this.rbScope2.TabIndex = 1;
            this.rbScope2.TabStop = true;
            this.rbScope2.Text = "These configurations are only allowed to run on these machines";
            this.rbScope2.UseVisualStyleBackColor = true;
            this.rbScope2.Click += new System.EventHandler(this.rbScope2_Click);
            // 
            // gbxMachineList
            // 
            this.gbxMachineList.Controls.Add(this.btnDelete);
            this.gbxMachineList.Controls.Add(this.btnAdd);
            this.gbxMachineList.Controls.Add(this.txtMachineName);
            this.gbxMachineList.Controls.Add(this.lbxMachines);
            this.gbxMachineList.Location = new System.Drawing.Point(13, 61);
            this.gbxMachineList.Name = "gbxMachineList";
            this.gbxMachineList.Size = new System.Drawing.Size(319, 180);
            this.gbxMachineList.TabIndex = 2;
            this.gbxMachineList.TabStop = false;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(250, 146);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(63, 25);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Dele&te";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(183, 146);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(63, 25);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "A&dd";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtMachineName
            // 
            this.txtMachineName.Location = new System.Drawing.Point(7, 149);
            this.txtMachineName.Name = "txtMachineName";
            this.txtMachineName.Size = new System.Drawing.Size(170, 20);
            this.txtMachineName.TabIndex = 1;
            // 
            // lbxMachines
            // 
            this.lbxMachines.FormattingEnabled = true;
            this.lbxMachines.Location = new System.Drawing.Point(6, 9);
            this.lbxMachines.Name = "lbxMachines";
            this.lbxMachines.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbxMachines.Size = new System.Drawing.Size(307, 134);
            this.lbxMachines.TabIndex = 0;
            this.lbxMachines.SelectedIndexChanged += new System.EventHandler(this.lbxMachines_SelectedIndexChanged);
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(77, 247);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(75, 23);
            this.btnAccept.TabIndex = 3;
            this.btnAccept.Text = "&Accept";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(212, 247);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // HostingServerSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 275);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.gbxMachineList);
            this.Controls.Add(this.rbScope2);
            this.Controls.Add(this.rbScope);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HostingServerSelection";
            this.Text = "Hosting Server(s)";
            this.gbxMachineList.ResumeLayout(false);
            this.gbxMachineList.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbScope;
        private System.Windows.Forms.RadioButton rbScope2;
        private System.Windows.Forms.GroupBox gbxMachineList;
        private System.Windows.Forms.ListBox lbxMachines;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtMachineName;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDelete;
    }
}