using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BOG.Framework;

namespace WeedKillerManager
{
    public partial class Options : Form
    {
        SettingsDictionary _ChangedSettings = new SettingsDictionary();

        public Options(SettingsDictionary original)
        {
            InitializeComponent();
            _ChangedSettings = original;

            this.nudMRUsize.Value = int.Parse((string)original.GetSetting("Options.MRUsize", "10"));
        }

        private void ApplyNewSettings()
        {
            _ChangedSettings.SetSetting("Options.MRUsize", this.nudMRUsize.Value.ToString());
            _ChangedSettings.SaveSettings();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("Discard and lose changes?", "Cancel", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
            {
                this.Close();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ApplyNewSettings();
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ApplyNewSettings();
            btnApply.Enabled = false;
        }

        private void nudMRUsize_ValueChanged(object sender, EventArgs e)
        {
            btnApply.Enabled = true;
        }

        private void btnClearMruList_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Do you want to remove all most recently used files from the list in File / Recent?", "Clear MRU List", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                Form z = Application.OpenForms["ManagerMDI"];
                ((ManagerMDI)z).EraseMRU();
            }
        }
    }
}
