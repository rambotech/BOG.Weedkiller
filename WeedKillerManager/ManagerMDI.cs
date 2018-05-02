using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Reflection;
using System.Windows.Forms;
using BOG.WeedKiller;
using BOG.Framework;

//  Copyright (c) 2009-2016, John J Schultz, all rights reserved.

namespace WeedKillerManager
{
    public partial class ManagerMDI : Form
    {
        private static string _HelpURL = string.Empty;
        private static string _ExecutablePath = string.Empty;
        private static string _ConfigurationPath = string.Empty;
        private static string _ConfigurationFile = string.Empty;
        private static string _DefaultConfigurationFile = string.Empty;
        public static SettingsDictionary _AppSettings;
        private static string _StorageFolder = string.Empty;
        private int _MRUsize = 10;
        private bool InFileClick = false;
        private DataTable LocalFileList = new DataTable();

        // The File menu.
        private ToolStripMenuItem MyMenu;

        // The menu items we use to display files.
        private ToolStripMenuItem[] MenuItems;

        public ManagerMDI(string[] args)
        {
            InitializeComponent();

            LocalFileList.Columns.Add("Timestamp", Type.GetType("System.DateTime"));
            LocalFileList.Columns.Add("FileName", Type.GetType("System.String"));
            BOG.Framework.AssemblyVersion a = new BOG.Framework.AssemblyVersion(System.Reflection.Assembly.GetEntryAssembly());
            _ExecutablePath = a.FullPath.Substring(0, a.FullPath.Length - a.Name.Length);
            _HelpURL = Path.Combine(_ExecutablePath, "WeedKillerHelp.htm");
            _ConfigurationPath = Path.Combine(Path.Combine(System.Environment.GetEnvironmentVariable("APPDATA"), "Bits of Genius"), "Weed Killer Manager");
            _ConfigurationFile = Path.Combine(_ConfigurationPath, "usersettings.xml");
            _DefaultConfigurationFile = Path.Combine(_ExecutablePath, "default_usersettings.xml");
            _StorageFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            try
            {
                if (!File.Exists(_ConfigurationFile) && File.Exists(_DefaultConfigurationFile))
                {
                    File.Copy(_DefaultConfigurationFile, _ConfigurationFile);
                }
            }
            catch
            {
            }

            _AppSettings = new SettingsDictionary(_ConfigurationFile);
            _AppSettings.LoadSettings();

            LoadOptions();

            string FileName = string.Empty;
            for (int i = 0; i < 100 && (FileName = (string)_AppSettings.GetSetting(string.Format("Recovery{0:0#}", i), string.Empty)) != string.Empty; i++)
            {
                if (File.Exists(FileName))
                {
                    string NewFileName = string.Empty;
                    // a file which was previously saved, but was not current at the time of closing.
                    if (FileName.Length >= 13 && FileName.Substring(FileName.Length - 13, 13).ToLower() == ".recovery.wkconf")
                    {
                        NewFileName = FileName.Substring(0, FileName.Length - 13);
                    }

                    OpenFile(FileName);
                    Application.DoEvents();
                    SetEditor ChildForm = (SetEditor)this.MdiChildren[this.MdiChildren.GetUpperBound(0)];
                    ChildForm.ConfigurationFilePath = NewFileName;
                    ChildForm.Saved = false;
                    ChildForm.Text = ChildForm.ConfigurationFilePath == string.Empty ? "Untitled" : Path.GetFileNameWithoutExtension(NewFileName);
                    File.Delete(FileName);
                }
                _AppSettings.DeleteSetting(string.Format("Recovery{0:0#}", i));
            }
            _AppSettings.SaveSettings();

            // If the command line is not empty, treat each argument as a configuration file,
            // and open the file.  This allows files to be opened from the explorer extension
            // "Open"
            for (int Index = 0; args != null && Index < args.Length; Index++)
            {
                if (args[Index].IndexOfAny(new char[] { '*', '?' }) < 0 && !File.Exists(args[Index]))
                {
                    MessageBox.Show(string.Format("Non-existent file specific on command line: {0}", args[Index]), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    foreach (string ThisFileName in Directory.GetFiles(Path.GetDirectoryName(args[Index]), Path.GetFileName(args[Index]), SearchOption.TopDirectoryOnly))
                    {
                        OpenFile(ThisFileName);
                        if (this.MdiChildren.Length > 0)
                        {
                            Application.DoEvents();
                            SetEditor ChildForm = (SetEditor)this.MdiChildren[this.MdiChildren.GetUpperBound(0)];
                            if (ChildForm.ConfigurationFilePath == ThisFileName)
                            {
                                ChildForm.Saved = true;
                                ChildForm.Text = Path.GetFileNameWithoutExtension(ThisFileName);
                            }
                        }
                    }
                }
            }
            LoadMRU();
            AdjustMenu();
            this.toolStripStatusLabel.Text = "Ready";
        }

        private void LoadOptions()
        {
            _StorageFolder = (string)_AppSettings.GetSetting("LastFolderLocation", _StorageFolder);
            _MRUsize = int.Parse((string)_AppSettings.GetSetting("Options.MRUsize", _MRUsize.ToString()));
        }

        private string MakeMruIdentifier(int index, string suffix)
        {
            return string.Format("ManagerMDI.FileMenu.MRU.{0}.{1}", index, suffix);
        }

        private void SaveMRU()
        {
            int Index = 0;
            foreach (DataRow r in LocalFileList.Select(string.Empty, "Timestamp DESC"))
            {
                string FileSetting = MakeMruIdentifier(Index, "FileName");
                string TimestampSetting = MakeMruIdentifier(Index, "DateTime");
                _AppSettings.SetSetting(FileSetting, r.ItemArray[LocalFileList.Columns["FileName"].Ordinal]);
                _AppSettings.SetSetting(TimestampSetting, r.ItemArray[LocalFileList.Columns["Timestamp"].Ordinal]);
                Index++;
                if (Index == _MRUsize)
                {
                    break;
                }
            }
            while (Index < _MRUsize)
            {
                string FileSetting = MakeMruIdentifier(Index, "FileName");
                string TimestampSetting = MakeMruIdentifier(Index, "DateTime");
                _AppSettings.DeleteSetting(FileSetting);
                _AppSettings.DeleteSetting(TimestampSetting);
                Index++;
            }
            _AppSettings.SaveSettings();
        }

        private void RemoveMRU(string fileName)
        {
            int LocationIndex = 0;
            for (LocationIndex = 0; LocationIndex < LocalFileList.Rows.Count; LocationIndex++)
            {
                if (string.Compare((string)LocalFileList.Rows[LocationIndex].ItemArray[LocalFileList.Columns["FileName"].Ordinal], fileName, true) == 0)
                {
                    break;
                }
            }
            if (LocationIndex < LocalFileList.Rows.Count)
            {
                LocalFileList.Rows.RemoveAt(LocationIndex);
            }
            SaveMRU();
            LoadMRU();
        }

        public void EraseMRU()
        {
            while (LocalFileList.Rows.Count > 0)
            {
                LocalFileList.Rows.RemoveAt(0);
            }
            SaveMRU();
            LoadMRU();
        }

        public void AddMRU(string fileName)
        {
            int LocationIndex = 0;
            for (LocationIndex = 0; LocationIndex < LocalFileList.Rows.Count; LocationIndex++)
            {
                if (string.Compare((string)LocalFileList.Rows[LocationIndex].ItemArray[LocalFileList.Columns["FileName"].Ordinal], fileName, true) == 0)
                {
                    break;
                }
            }
            if (LocationIndex < LocalFileList.Rows.Count)
            {
                return;
            }
            LocalFileList.Rows.Add(new object[] { DateTime.Now, fileName });
            SaveMRU();
            LoadMRU();
        }

        private void LoadMRU()
        {
            if (recentToolStripMenuItem.DropDownItems.Count > 0)
            {
                for (int Index = 0; Index < recentToolStripMenuItem.DropDownItems.Count; Index++)
                {
                    recentToolStripMenuItem.DropDownItems[Index].Click -= File_Click;
                }
            }
            recentToolStripMenuItem.DropDownItems.Clear();
            recentToolStripMenuItem.Enabled = false;
            LocalFileList.Rows.Clear();
            for (int Index = 0; Index < _MRUsize; Index++)
            {
                string FileSetting = MakeMruIdentifier(Index, "FileName");
                string TimestampSetting = MakeMruIdentifier(Index, "DateTime");
                if (_AppSettings.HasSetting(FileSetting) && _AppSettings.HasSetting(TimestampSetting))
                {
                    LocalFileList.Rows.Add(new object[] { 
                        _AppSettings.GetSetting(TimestampSetting, DateTime.Now), 
                        _AppSettings.GetSetting(FileSetting, string.Empty)
                    });
                }
            }
            recentToolStripMenuItem.Enabled = LocalFileList.Rows.Count > 0;
            int ItemCount = 0;
            if (recentToolStripMenuItem.Enabled)
            {
                MyMenu = recentToolStripMenuItem;
                MenuItems = new ToolStripMenuItem[LocalFileList.Rows.Count];
                foreach (DataRow r in LocalFileList.Select(string.Empty, "Timestamp DESC"))
                {
                    MenuItems[ItemCount] = new ToolStripMenuItem();
                    MenuItems[ItemCount].Visible = true;
                    MenuItems[ItemCount].Text = string.Format("&{0} {1}", ItemCount + 1, r.ItemArray[LocalFileList.Columns["FileName"].Ordinal]);
                    MenuItems[ItemCount].Tag = r.ItemArray[LocalFileList.Columns["FileName"].Ordinal];
                    MenuItems[ItemCount].Click += File_Click;
                    ItemCount++;
                    if (ItemCount > _MRUsize)
                    {
                        break;
                    }
                }
                MyMenu.DropDownItems.AddRange(MenuItems);
            }
        }

        private void AdjustMenu()
        {
            this.saveToolStripMenuItem.Enabled = (this.ActiveMdiChild != null);
            this.saveAsToolStripMenuItem.Enabled = (this.ActiveMdiChild != null);
            this.publishAsToolStripMenuItem.Enabled = (this.ActiveMdiChild != null);
            this.copyToolStripMenuItem.Enabled = (this.ActiveMdiChild != null);
            this.pasteToolStripMenuItem.Enabled = (this.ActiveMdiChild != null);
            this.testSelectedProcessToolStripMenuItem.Enabled = (this.ActiveMdiChild != null);
            this.adjustWorkerServersToolStripMenuItem.Enabled = (this.ActiveMdiChild != null);
            this.adjustNTPermissionsToolStripMenuItem.Enabled = (this.ActiveMdiChild != null);
            this.windowsMenu.Enabled = (this.ActiveMdiChild != null);
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            SetEditor childForm = new SetEditor(ref _AppSettings);
            childForm.MdiParent = this;
            childForm.Text = "Untitled";
            childForm.Tag = null;
            childForm.Show();
            AdjustMenu();
        }

        public void OpenFile(string filename)
        {
            string corePath = Path.GetDirectoryName(filename);
            bool InUse = false;
            foreach (SetEditor childForm in MdiChildren)
            {
                if (filename == (string)childForm.ConfigurationFilePath)
                {
                    childForm.Activate();
                    InUse = true;
                    break;
                }
            }
            if (!InUse)
            {
                bool FileIsOk = true;
                SetEditor childForm = new SetEditor(ref _AppSettings);
                childForm.ConfigurationFilePath = filename;
                try
                {
                    childForm.LoadFromFile();
                    AddMRU(filename);
                }
                catch (Exception e)
                {
                    FileIsOk = false;
                    childForm.Dispose();
                    MessageBox.Show(string.Format("Error loading configuration file: {0}\r\n{1}\r\nThe file may be corrupt.", filename, e.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                if (FileIsOk)
                {
                    childForm.StorageFolder = corePath;
                    childForm.MdiParent = this;
                    childForm.Show();
                }
                AdjustMenu();
            }
            if (corePath != _StorageFolder)
            {
                _AppSettings.LoadSettings();
                _AppSettings.SetSetting("LastFolderLocation", corePath);
                _AppSettings.SaveSettings();
            }
            _StorageFolder = corePath;
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = _StorageFolder;
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Select configuration set(s) to load";
            openFileDialog.Filter = "Weed Killer Config Files (*.wkconf)|*.wkconf";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                foreach (string FileName in openFileDialog.FileNames)
                {
                    OpenFile(FileName);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SetEditor o = (SetEditor)this.ActiveMdiChild;
            }
            catch
            {
                MessageBox.Show("Only a configuration set can be saved to a file--not a tester.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            SetEditor f = (SetEditor)this.ActiveMdiChild;
            f.SaveToFile(f.ConfigurationFilePath);
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SetEditor o = (SetEditor)this.ActiveMdiChild;
            }
            catch
            {
                MessageBox.Show("Only a configuration set can be saved to a file--not a tester.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            SetEditor f = (SetEditor)this.ActiveMdiChild;
            f.SaveToFile(string.Empty);
        }

        private void publishAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SetEditor o = (SetEditor)this.ActiveMdiChild;
            }
            catch
            {
                MessageBox.Show("Only a configuration set can be saved to a file--not a tester.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            SetEditor f = (SetEditor)this.ActiveMdiChild;
            f.SaveToFile(string.Empty, true);
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SetEditor o = (SetEditor)this.ActiveMdiChild;
            }
            catch
            {
                MessageBox.Show("Only a selected configuration item can be copied from the editor--not from the tester.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            SetEditor f = (SetEditor)this.ActiveMdiChild;
            if (f.CopyConfigToClipboard())
            {
                this.toolStripStatusLabel.Text = "Selected configuration item(s) copied to clipboard.";
            }
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SetEditor o = (SetEditor)this.ActiveMdiChild;
            }
            catch
            {
                MessageBox.Show("Only a configuration set can be saved to a file--not a tester.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            SetEditor f = (SetEditor)this.ActiveMdiChild;
            if (f.PasteConfigFromClipboard())
            {
                this.toolStripStatusLabel.Text = "New configuration item(s) creted from clipboard.";
            }
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
            this.toolStripStatusLabel.Text = string.Empty;
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
            this.toolStripStatusLabel.Text = string.Empty;
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
            this.toolStripStatusLabel.Text = string.Empty;
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
            this.toolStripStatusLabel.Text = string.Empty;
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (SetEditor childForm in MdiChildren)
            {
                childForm.Close();
            }
            AdjustMenu();
            this.toolStripStatusLabel.Text = string.Empty;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox f = new AboutBox();
            f.ShowDialog();
            this.toolStripStatusLabel.Text = string.Empty;
        }

        private void ManagerMDI_FormClosing(object sender, FormClosingEventArgs e)
        {
            _AppSettings.SaveSettings();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpBox h = new HelpBox();
            h.ShowDialog();
        }

        private void ManagerMDI_MdiChildActivate(object sender, EventArgs e)
        {
            AdjustMenu();
            this.toolStripStatusLabel.Text = this.ActiveMdiChild == null ? "Use New or Open from the File menu to start." : ((SetEditor)this.ActiveMdiChild).ConfigurationFilePath;
        }

        private void testSelectedProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripStatusLabel.Text = "Tester active.. close it to return to editor.";
            try
            {
                SetEditor o = (SetEditor)this.ActiveMdiChild;
            }
            catch
            {
                MessageBox.Show("You must have an configuration item selected in an editor window to launch a tester", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            SetEditor f = (SetEditor)this.ActiveMdiChild;
            f.LaunchTester();
            this.toolStripStatusLabel.Text = string.Empty;
        }

        private void adjustWorkerServersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripStatusLabel.Text = "Adjust the list of servers which can execute the configurations in this set.";
            try
            {
                SetEditor o = (SetEditor)this.ActiveMdiChild;
            }
            catch
            {
                MessageBox.Show("You must have an configuration item selected in an editor window to adjust the server filter", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            SetEditor f = (SetEditor)this.ActiveMdiChild;
            f.AdjustServerExectionFilter();
            this.toolStripStatusLabel.Text = string.Empty;
        }

        private void adjustNTPermissionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripStatusLabel.Text = "Apply or Remove an NT Account with permissions to delete items in the target folder(s)";
            try
            {
                SetEditor o = (SetEditor)this.ActiveMdiChild;
            }
            catch
            {
                MessageBox.Show("You must have an configuration item selected in an editor window to adjust the server filter", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            SetEditor f = (SetEditor)this.ActiveMdiChild;
            f.AdjustNTAccountPermissions();
            this.toolStripStatusLabel.Text = string.Empty;
        }

        private void recentToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options f = new Options(_AppSettings);
            f.ShowDialog();
            _AppSettings.LoadSettings();
            LoadOptions();
            LoadMRU();
        }

        // The user selected a file from the menu.
        private void File_Click(object sender, EventArgs e)
        {
            if (InFileClick) 
            {
                return;
            }
            InFileClick = true;
            // Get the corresponding FileInfo object.
            ToolStripMenuItem menu_item = (ToolStripMenuItem) sender;
            string FileSelected = (string) menu_item.Tag;

            // Check to see if the file exists.
            if (File.Exists(FileSelected) == false)
            {
                if (DialogResult.Yes == MessageBox.Show("Do you want to remove this file from the MRU list.", "File not found", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
                {
                    RemoveMRU(FileSelected);
                }
            }
            else
            {
                OpenFile(FileSelected);
            }
            InFileClick = false;
        }

        private void readMeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process notePad = new System.Diagnostics.Process();

            notePad.StartInfo.FileName = "notepad.exe";
            notePad.StartInfo.Arguments = "ReadMe.txt";

            notePad.Start();
        }
    }
}
