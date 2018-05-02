using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WeedKillerManager
{
    public partial class HostingServerSelection : Form
    {
        private bool _Accepted = false;

        public bool Accepted
        {
            get { return _Accepted; }
        }

        public string HostNames
        {
            get
            {
                List<string> _HostNames = new List<string>();
                foreach (object o in this.lbxMachines.Items)
                {
                    _HostNames.Add((string)o);
                }
                return string.Join(",", _HostNames.ToArray()).Trim();
            }
        }

        public HostingServerSelection(string hostServerList)
        {
            InitializeComponent();
            this.lbxMachines.Items.AddRange(hostServerList.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries));
            AdjustControls();
        }

        private void AdjustControls()
        {
            this.rbScope.Checked = this.lbxMachines.Items.Count == 0;
            this.rbScope2.Checked = this.lbxMachines.Items.Count > 0;
            this.btnDelete.Enabled = this.lbxMachines.SelectedIndices.Count > 0;
            this.btnAdd.Enabled = this.lbxMachines.SelectedIndices.Count == 0;
            this.txtMachineName.Enabled = btnAdd.Enabled;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.txtMachineName.Text.Trim().Length == 0)
            {
                MessageBox.Show("There is no server name to add", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (this.txtMachineName.Text.IndexOfAny(new char[] { ',', '\\'}) >= 0)
            {
                MessageBox.Show("The machine name can not contain any commas or backslashes", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach (object o in this.lbxMachines.Items)
            {
                if (((string) o).Trim().ToLower() == this.txtMachineName.Text.Trim().ToLower())
                {
                    MessageBox.Show("The name already exists in the server list", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            this.lbxMachines.Items.Add(this.txtMachineName.Text.Trim());
            AdjustControls();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            _Accepted = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbScope_Click(object sender, EventArgs e)
        {
            if (this.lbxMachines.Items.Count > 0)
            {
                if (MessageBox.Show("This will clear the server list and allow any server to execute these configurations\r\nAre you sure you wish to continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    this.rbScope2.Checked = false;
                    this.rbScope.Checked = true;
                    this.lbxMachines.Items.Clear();
                }
            }
            AdjustControls();
        }

        private void rbScope2_Click(object sender, EventArgs e)
        {
            AdjustControls();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will clear the selected server names from the list.\r\nAre you sure you wish to continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                while (this.lbxMachines.SelectedIndices.Count > 0)
                {
                    this.lbxMachines.Items.RemoveAt(this.lbxMachines.SelectedIndices[0]);
                }
            }
            this.rbScope.Checked = this.lbxMachines.Items.Count == 0;
            this.rbScope2.Checked = this.lbxMachines.Items.Count > 0;
        }

        private void lbxMachines_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdjustControls();
        }
    }
}
