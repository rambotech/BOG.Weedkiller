using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using BOG.Framework;
using BOG.WeedKiller;

//  Copyright (c) 2009-2016, John J Schultz, all rights reserved.

namespace WeedKillerManager
{
    public partial class SetEditor : Form
    {
        private const string UnsavedMessage = " (content needs to be persisted) ";

        private WeedKillerConfigSet cs = new WeedKillerConfigSet();
        private WeedKillerConfig ConfigSandbox = new WeedKillerConfig();
        private SettingsDictionary AppSettings;
        private bool EditMode = false;
        private int SelectedConfiguration = -1;

        private string _StorageFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        private string _ConfigurationFilePath = string.Empty;
        private string _ConfigurationFile = string.Empty;
        private bool _Saved = true;
        private bool _NonOverridable_Shutdown = false;

        public string StorageFolder
        {
            get { return _StorageFolder; }
            set { _StorageFolder = value; }
        }

        public string ConfigurationFilePath
        {
            get { return _ConfigurationFilePath; }
            set { _ConfigurationFilePath = value; }
        }

        public bool Saved
        {
            get { return _Saved; }
            set { _Saved = value; }
        }

        public bool NonOverridable_Shutdown
        {
            get { return _NonOverridable_Shutdown; }
            set { _NonOverridable_Shutdown = value; }
        }

        public SetEditor(ref SettingsDictionary appSettings)
        {
            AppSettings = appSettings;
            InitializeComponent();
        }

        private void AdjustControls()
        {
            this.ParentForm.MainMenuStrip.Enabled = !EditMode;
            this.lvwConfigSet.Enabled = ! EditMode;
            this.btnAdd.Enabled = ! EditMode;
            this.btnDelete.Enabled = (!EditMode && this.lvwConfigSet.SelectedIndices.Count >= 1);
            this.btnAccept.Enabled = EditMode;
            this.btnCancel.Enabled = EditMode;
            this.lvwConfigSet.Focus();
        }

        private void AdjustListViewItemColor(int index)
        {
            this.lvwConfigSet.Items[index].BackColor = ((WeedKillerConfig)this.lvwConfigSet.Items[index].Tag).TestOnly ? Color.FromArgb(0xCCFFCC) : Color.White;
            this.lvwConfigSet.Items[index].ForeColor = ((WeedKillerConfig)this.lvwConfigSet.Items[index].Tag).Enabled ? Color.Black : (((WeedKillerConfig)this.lvwConfigSet.Items[index].Tag).TestOnly ? Color.DarkGray : Color.FromArgb(0x4444FF));
        }

        public void AdjustServerExectionFilter()
        {
            HostingServerSelection HostForm = new HostingServerSelection(cs.ExecutionServer);
            HostForm.ShowDialog();
            if (!HostForm.Accepted)
            {
                return;
            }
            cs.ExecutionServer = HostForm.HostNames;
            _Saved = false;
        }

        public void AdjustNTAccountPermissions()
        {
            Permissions PermissionForm = new Permissions(ref AppSettings, ref cs);
            PermissionForm.ShowDialog();
        }

        public void LoadFromFile()
        {
            try
            {
                cs = ObjectXMLSerializer<WeedKillerConfigSet>.LoadDocumentFormat(_ConfigurationFilePath);
                this.lvwConfigSet.Items.Clear();
                foreach (WeedKillerConfig c in cs.ConfigSet)
                {
                    ListViewItem i = new ListViewItem(c.Description);
                    i.Tag = c;
                    i.Group = new ListViewGroup("Configuration Set");
                    this.lvwConfigSet.Items.Add(i);
                    AdjustListViewItemColor(this.lvwConfigSet.Items.Count - 1);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("Error loading configuration from file: {0}", e.Message), "Can't load file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Text = Path.GetFileNameWithoutExtension(_ConfigurationFilePath);
            this.lvwConfigSet.Sort();
        }

        public bool SaveToFile(string TargetFile, bool PublishingMode)
        {
            string SaveTargetFile = TargetFile;

            if (!_Saved && PublishingMode && _ConfigurationFilePath != string.Empty)
            {
                _Saved = SaveToFile(_ConfigurationFilePath, false);
            }
            if (!_Saved && PublishingMode)
            {
                MessageBox.Show("You are attempting to publish a file you have not yet saved.  You must first save the file, before publishing it.", "Configuration not saved", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return _Saved;
            }

            if (SaveTargetFile == string.Empty)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = _StorageFolder;
                saveFileDialog.Title = PublishingMode ? "Location to publish the configuration file (i.e. visible to worker)" : "Location to save configuration file";
                saveFileDialog.Filter = "Weed Killer Config Files (*.wkconf)|*.wkconf";
                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    SaveTargetFile = saveFileDialog.FileName;
                    _Saved = true;
                }
            }

            if (SaveTargetFile != string.Empty)
            {
                try
                {
                    cs.ConfigSet.Clear();
                    cs.Updated = DateTime.Now;
                    foreach (ListViewItem i in this.lvwConfigSet.Items)
                    {
                        cs.ConfigSet.Add((WeedKillerConfig)i.Tag);
                    }
                    ObjectXMLSerializer<WeedKillerConfigSet>.SaveDocumentFormat(cs, SaveTargetFile);
                    if (!PublishingMode)
                    {
                        _Saved = true;
                        _ConfigurationFilePath = SaveTargetFile;
                        this.Text = Path.GetFileNameWithoutExtension(_ConfigurationFilePath);

                        Form z = Application.OpenForms["ManagerMDI"]; 
                        ((ManagerMDI) z).AddMRU(SaveTargetFile);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error saving configuration to file", string.Format("Can't save file: {0}", e.Message), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return _Saved;
        }

        public bool SaveToFile(string TargetFile)
        {
            return SaveToFile(TargetFile, false);
        }

        public bool CopyConfigToClipboard()
        {
            bool result = false;
            if (this.lvwConfigSet.SelectedIndices.Count > 0)
            {
                WeedKillerConfigSet cs = new WeedKillerConfigSet();
                for (int i=0; i < this.lvwConfigSet.SelectedIndices.Count; i++)
                {
                    cs.ConfigSet.Add((WeedKillerConfig)this.lvwConfigSet.Items[this.lvwConfigSet.SelectedIndices[i]].Tag);
                }
                Clipboard.SetText(ObjectXMLSerializer<WeedKillerConfigSet>.CreateDocumentFormat(cs));
                result = true;
            }
            else
            {
                MessageBox.Show("You must select one or more configuration items to copy", "Nothing to do", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return result;
        }

        public bool PasteConfigFromClipboard()
        {
            bool result = false;
            try
            {
                WeedKillerConfigSet cs = ObjectXMLSerializer<WeedKillerConfigSet>.CreateObjectFormat(Clipboard.GetText());
                foreach (WeedKillerConfig c in cs.ConfigSet)
                {
                    c.Description = "Copy of " + c.Description;
                    ListViewItem i = new ListViewItem(c.Description);
                    i.Tag = c;
                    i.Group = new ListViewGroup("Configuration Set");
                    this.lvwConfigSet.Items.Add(i);
                    AdjustListViewItemColor(this.lvwConfigSet.Items.Count - 1);
                }
                _Saved = false;
                AdjustControls();
                this.lvwConfigSet.Sort();
                result = true;
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "Can't paste from clipboard", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            NameEntry dialog = new NameEntry("Enter the name of the configuration","New Configuration");
            if (dialog.ShowDialog(this) == DialogResult.Cancel)
                return;
            WeedKillerConfig c = new WeedKillerConfig();
            c.Description = dialog.GetName;
            ListViewItem i = new ListViewItem(c.Description);
            i.Tag = (object) "new";
            i.Group = new ListViewGroup("Configuration Set");
            this.lvwConfigSet.Items.Add(i);
            int ActiveIndex;
            for (ActiveIndex = 0; ActiveIndex < this.lvwConfigSet.Items.Count; ActiveIndex++)
            {
                try
                {
                    if ((string)this.lvwConfigSet.Items[ActiveIndex].Tag == "new")
                        break;
                }
                catch { }
            }
            this.lvwConfigSet.Items[ActiveIndex].Tag = c;
            this.lvwConfigSet.SelectedItems.Clear();
            this.lvwConfigSet.Items[ActiveIndex].Selected = true;
            AdjustListViewItemColor(ActiveIndex);
            _Saved = false;
            AdjustControls();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Remove selected configurations?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                int iMaxIndex;
                while (this.lvwConfigSet.SelectedIndices.Count > 0)
                {
                    // the items are deleted from the highest index value to the lowest, to prevent
                    // index rearrangement at the lower indexes.
                    iMaxIndex = 0;
                    for (int i = 0; i < this.lvwConfigSet.SelectedIndices.Count; i++)
                    {
                        if (iMaxIndex < this.lvwConfigSet.SelectedIndices[i])
                        {
                            iMaxIndex = this.lvwConfigSet.SelectedIndices[i];
                        }
                    }
                    this.lvwConfigSet.Items.RemoveAt(iMaxIndex);
                }
                _Saved = false;
                AdjustControls();
            }
            else
            {
                MessageBox.Show("Selected Configuration not removed", "Abort", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            this.lvwConfigSet.Items[SelectedConfiguration].Tag = ConfigSandbox.Clone();
            this.lvwConfigSet.Items[SelectedConfiguration].Text = ((WeedKillerConfig)this.lvwConfigSet.Items[SelectedConfiguration].Tag).Description;
            AdjustListViewItemColor(SelectedConfiguration);
            this.lvwConfigSet.Sort();
            EditMode = false;
            _Saved = false;
            AdjustControls();
            this.lvwConfigSet.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Undo Changes", "Restore original values for this setting", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                this.pgSettings.SelectedObject = null;
                ConfigSandbox = ((WeedKillerConfig)this.lvwConfigSet.Items[SelectedConfiguration].Tag).CloneTyped();
                this.pgSettings.SelectedObject = ConfigSandbox;
                EditMode = false;
                AdjustControls();
                this.lvwConfigSet.Focus();
            }
        }

        private void lvwConfigSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lvwConfigSet.SelectedIndices.Count == 1)
            {
                SelectedConfiguration = this.lvwConfigSet.SelectedIndices[0];
                ConfigSandbox = ((WeedKillerConfig)this.lvwConfigSet.Items[SelectedConfiguration].Tag).CloneTyped();
                this.pgSettings.SelectedObject = ConfigSandbox;
            }
            else
            {
                SelectedConfiguration = -1;
                this.pgSettings.SelectedObject = null;
            }
            AdjustControls();
        }

        private void pgSettings_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (! EditMode && this.pgSettings.SelectedObject != null)
            {
                EditMode = true;
                AdjustControls();
            }
        }

        public void LaunchTester()
        {
            if (this.lvwConfigSet.SelectedIndices.Count == 1)
            {
                SetTester childForm = new SetTester(ref AppSettings);
                childForm.config = ((WeedKillerConfig)this.lvwConfigSet.Items[this.lvwConfigSet.SelectedIndices[0]].Tag).CloneTyped();
                childForm.Tag = null;
                childForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("You must have an configuration item selected in an editor window to launch a tester", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void lvwConfigSet_DoubleClick(object sender, EventArgs e)
        {
            LaunchTester();
        }

        private void lvwConfigSet_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch ((byte) e.KeyChar)
            {
                case 1:   // Ctrl+A
                    for (int i=0; i < this.lvwConfigSet.Items.Count; i++)
                        this.lvwConfigSet.Items[i].Selected = true;
                    break;
                case 4:   // Ctrl+D
                    for (int i = 0; i < this.lvwConfigSet.Items.Count; i++)
                        this.lvwConfigSet.Items[i].Selected = false;
                    break;
                case 9:   // Ctrl+I
                    for (int i = 0; i < this.lvwConfigSet.Items.Count; i++)
                        this.lvwConfigSet.Items[i].Selected = ! this.lvwConfigSet.Items[i].Selected;
                    break;
            }
        }

        private void SetEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (EditMode && e.CloseReason == CloseReason.UserClosing)
            {
                MessageBox.Show("You must Accept or Cancel your changes to the current configuration entry", "Changes in progress", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
                return;
            }
            if (!Saved)
            {
                bool isOverriddable = true;
                switch (e.CloseReason)
                {
                    case CloseReason.TaskManagerClosing:
                    case CloseReason.WindowsShutDown:
                    case CloseReason.ApplicationExitCall:
#if DEBUG
                    case CloseReason.MdiFormClosing:  // enabled for IDE Testing only!!
#endif
                    case CloseReason.None:
                        isOverriddable = false;
                        break;
                }

                if (isOverriddable)
                {
                    DialogResult d = MessageBox.Show("Save changes?", "Changes not saved", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                    if (d == DialogResult.Yes)
                    {
                        SaveToFile(_ConfigurationFilePath);
                    }
                    else if (d == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    // If not overridable, content is saved to temporary files for recovery on next startup.
                    int i = 0;
                    string RecoveryFile = _ConfigurationFilePath == string.Empty ? string.Format("{0}\\Unsaved_WeedKillerTemp_{1}.wkconf", _StorageFolder, System.Guid.NewGuid().ToString("N")) : _ConfigurationFilePath + ".recovery.wkconf";
                    SaveToFile(RecoveryFile);

                    for (i = 0; i < 100 && (string)AppSettings.GetSetting(string.Format("Recovery{0:0#}", i), string.Empty) != string.Empty; i++) ;

                    AppSettings.SetSetting(string.Format("Recovery{0:0#}", i), (object) RecoveryFile);
                    AppSettings.SaveSettings();
                }
            }
        }

        private void SetEditor_Leave(object sender, EventArgs e)
        {
            if (EditMode)
            {
                this.Focus();
                this.Refresh();
                MessageBox.Show("You must Accept or Cancel your changes to the current configuration entry", "Changes in progress", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
