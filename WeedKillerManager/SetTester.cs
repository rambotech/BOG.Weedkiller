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

/*
 * Copyright (c) 2009-2016, John J Schultz, all rights reserved.
 * 4/27/2012 -- JJS
 *   - Adding templates, so that event selections can be grouped into a selectable item.
 * 6/15/2010 -- JJS
 *   - Corrected number formatting which was displaying decimal points, and missing grouping.
 * 8/10/2009 -- JJS
 *   - When an event type changes its check state, hides the gridview until the visible
 *     state adjustment is completed.  This reduces looooong redraw times when items
 *     are set to hidden.
 *   - Added a counter to display the number of events are displayed.
 */

namespace WeedKillerManager
{
    /// <summary>
    /// This is the tester for a configuration.  It is intended to operate as a modal window.
    /// </summary>
    public partial class SetTester : Form
    {
        private WeedKiller worker;
        private WeedKillerConfig _c;
        private int ItemEventCount = 0;
        private int DisplayedItemEventCount = 0;
        private double DisplayedItemCumulativeSize = 0.0;
        private Dictionary<WeedKillerEventArgs.WeedKillerActionType, bool> LogThisEvent = new Dictionary<WeedKillerEventArgs.WeedKillerActionType, bool>();
        private Dictionary<WeedKillerEventArgs.WeedKillerActionType, Color> ColorBackgroundThisEvent = new Dictionary<WeedKillerEventArgs.WeedKillerActionType, Color>();
        private Dictionary<WeedKillerEventArgs.WeedKillerActionType, Color> ColorForegroundThisEvent = new Dictionary<WeedKillerEventArgs.WeedKillerActionType, Color>();
        private List<int> CustomColors = new List<int>();
        private List<string> TemplateNames = new List<string>();
        private Queue<WeedKillerEventArgs> WeedKillerEventsBacklog = new Queue<WeedKillerEventArgs>();
        private SettingsDictionary AppSettings;
        private string OriginalText;
        private WeedKillerConfig workset;
        private Dictionary<int, WeedKillerEventArgs> FileEventResults = new Dictionary<int, WeedKillerEventArgs>();

        private string TemplateNameInUse = "DEFAULT";
        private bool TemplateHasChanged = false;
        private bool InMultiSelect = false;
        private bool InTemplateAdjustment = false;
        private bool BackgroundWorkerIsProcessing = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appSettings"></param>
        public SetTester(ref SettingsDictionary appSettings)
        {
            InitializeComponent();

            this.dgvFileResults.Columns.Add("Mode", "Mode");
            this.dgvFileResults.Columns.Add("Action", "Action");
            this.dgvFileResults.Columns.Add("Result", "Result");
            this.dgvFileResults.Columns.Add("Size", "Size");
            this.dgvFileResults.Columns.Add("SizeRecovered", "Size Recovered");
            this.dgvFileResults.Columns.Add("Timestamp", "Time");
            this.dgvFileResults.Columns.Add("Path", "Path");
            this.dgvFileResults.Columns.Add("FileName", "File name");
            this.dgvFileResults.Columns.Add("Message", "Message");

            AppSettings = appSettings;
            OneTimeTemplateKeyFix();
            LoadEventViewTemplates();
            LoadEventViewCustomColors();
            LoadEventViewTemplate();
            toolStripStatusLabel1.Text = "Ready";
        }

        private void LoadVisibleFileResults()
        {
            this.toolStripStatusLabel1.Text = "Loading results...";
            this.statusStrip1.Refresh();
            this.dgvFileResults.SuspendLayout();
            this.dgvFileResults.Visible = false;
            this.dgvFileResults.Rows.Clear();
            this.Refresh();

            DisplayedItemEventCount = 0;
            DisplayedItemCumulativeSize = 0.0;
            for (int Index = 0; Index < this.FileEventResults.Count; Index++)
            {
                WeedKillerEventArgs r = FileEventResults[Index];
                if (LogThisEvent[r.Action])
                {
                    DisplayedItemEventCount++;
                    DisplayedItemCumulativeSize += r.Size;

                    DataGridViewRow rw = new DataGridViewRow();
                    rw.CreateCells(dgvFileResults,
                        new object[] {
                            (object) (r.TestMode ? "Test" : "Live"),
                            (object) (Enum.GetName(typeof(WeedKillerEventArgs.WeedKillerActionType), r.Action)),
                            (object) (r.Success ? "OK" : "FAIL"),
                            (object) r.Size.ToString("#,0"),
                            (object) r.Size_Recovered.ToString("#,0"),
                            (object) (r.TimeStamp == DateTime.MinValue ? "n/a" : r.TimeStamp.ToString("G")),
                            (object) r.Path,
                            (object) r.FileName,
                            (object) r.Message
                        });
                    rw.DefaultCellStyle.BackColor = ColorBackgroundThisEvent[r.Action];
                    rw.DefaultCellStyle.ForeColor = ColorForegroundThisEvent[r.Action];

                    this.dgvFileResults.Rows.Add(rw);
                }
            }
            this.gbxEventsOfInterest.Text = string.Format("Events ({0:#,0} items, {1:#,0} bytes)", DisplayedItemEventCount, DisplayedItemCumulativeSize);
            this.toolStripStatusLabel1.Text = "Resuming layout...";
            this.statusStrip1.Refresh();
            this.dgvFileResults.ResumeLayout();
            this.dgvFileResults.Visible = true;
            this.toolStripStatusLabel1.Text = "Ready";
        }

        private void LoadEventViewTemplate()
        {
            InMultiSelect = true;
            this.LogThisEvent.Clear();
            this.cbxlEventsOfInterest.Items.Clear();
            this.ColorForegroundThisEvent.Clear();
            this.ColorBackgroundThisEvent.Clear();

            foreach (WeedKillerEventArgs.WeedKillerActionType x in Enum.GetValues(typeof(WeedKillerEventArgs.WeedKillerActionType)))
            {
                string KeyName = Enum.GetName(typeof(WeedKillerEventArgs.WeedKillerActionType), x);

                bool IsChecked = bool.Parse((string)AppSettings.GetSetting("Tester.EventSelect." + TemplateNameInUse + "." + KeyName, "True"));
                Color DisplayColorBackground = Color.FromArgb(int.Parse((string)AppSettings.GetSetting("Tester.EventColor." + TemplateNameInUse + ".Background." + KeyName, Color.White.ToArgb().ToString())));
                Color DisplayColorForeground = Color.FromArgb(int.Parse((string)AppSettings.GetSetting("Tester.EventColor." + TemplateNameInUse + ".Foreground." + KeyName, Color.Black.ToArgb().ToString())));
                this.LogThisEvent.Add(x, IsChecked);
                this.ColorForegroundThisEvent.Add(x, DisplayColorForeground);
                this.ColorBackgroundThisEvent.Add(x, DisplayColorBackground);
                this.cbxlEventsOfInterest.Items.Add(KeyName, IsChecked);
            }
            InMultiSelect = false;
            LoadVisibleFileResults();
            TemplateHasChanged = false;
        }

        private void LoadEventViewCustomColors()
        {
            this.ColorForegroundThisEvent.Clear();
            this.ColorBackgroundThisEvent.Clear();

            for (int Index = 0; Index < 32; Index++)
            {
                string KeyName = string.Format("Tester.EventColor.DEFAULT.CustomColors.{0:0#}", Index);
                if (AppSettings.HasSetting(KeyName))
                {
                    CustomColors.Add(int.Parse((string)AppSettings.GetSetting(KeyName, "0")));
                }
            }
        }

        private void OneTimeTemplateKeyFix()
        {
            bool Changed = false;

            if (AppSettings.HasSetting("Tester.EventSelect.ZeroLengthFileSpared") == true)
            {
                Changed = true;
                AppSettings.SetSetting("Tester.EventSelect.DEFAULT.ZeroLengthFileSpared", (string)AppSettings.GetSetting("Tester.EventSelect.ZeroLengthFileSpared", string.Empty)); AppSettings.DeleteSetting("Tester.EventSelect.ZeroLengthFileSpared");
            }
            if (AppSettings.HasSetting("Tester.EventSelect.ReadOnlyFileSpared") == true)
            {
                Changed = true;
                AppSettings.SetSetting("Tester.EventSelect.DEFAULT.ReadOnlyFileSpared", (string)AppSettings.GetSetting("Tester.EventSelect.ReadOnlyFileSpared", string.Empty)); AppSettings.DeleteSetting("Tester.EventSelect.ReadOnlyFileSpared");
            }
            if (AppSettings.HasSetting("Tester.EventSelect.FreshFileSpared") == true)
            {
                Changed = true;
                AppSettings.SetSetting("Tester.EventSelect.DEFAULT.FreshFileSpared", (string)AppSettings.GetSetting("Tester.EventSelect.FreshFileSpared", string.Empty)); AppSettings.DeleteSetting("Tester.EventSelect.FreshFileSpared");
            }
            if (AppSettings.HasSetting("Tester.EventSelect.AgedFileRemoved") == true)
            {
                Changed = true;
                AppSettings.SetSetting("Tester.EventSelect.DEFAULT.AgedFileRemoved", (string)AppSettings.GetSetting("Tester.EventSelect.AgedFileRemoved", string.Empty)); AppSettings.DeleteSetting("Tester.EventSelect.AgedFileRemoved");
            }
            if (AppSettings.HasSetting("Tester.EventSelect.MaxCountFileRemoved") == true)
            {
                Changed = true;
                AppSettings.SetSetting("Tester.EventSelect.DEFAULT.MaxCountFileRemoved", (string)AppSettings.GetSetting("Tester.EventSelect.MaxCountFileRemoved", string.Empty)); AppSettings.DeleteSetting("Tester.EventSelect.MaxCountFileRemoved");
            }
            if (AppSettings.HasSetting("Tester.EventSelect.MinCountFileSpared") == true)
            {
                Changed = true;
                AppSettings.SetSetting("Tester.EventSelect.DEFAULT.MinCountFileSpared", (string)AppSettings.GetSetting("Tester.EventSelect.MinCountFileSpared", string.Empty)); AppSettings.DeleteSetting("Tester.EventSelect.MinCountFileSpared");
            }
            if (AppSettings.HasSetting("Tester.EventSelect.FileNoMatch") == true)
            {
                Changed = true;
                AppSettings.SetSetting("Tester.EventSelect.DEFAULT.FileNoMatch", (string)AppSettings.GetSetting("Tester.EventSelect.FileNoMatch", string.Empty)); AppSettings.DeleteSetting("Tester.EventSelect.FileNoMatch");
            }
            if (AppSettings.HasSetting("Tester.EventSelect.DirectoryNoMatch") == true)
            {
                Changed = true;
                AppSettings.SetSetting("Tester.EventSelect.DEFAULT.DirectoryNoMatch", (string)AppSettings.GetSetting("Tester.EventSelect.DirectoryNoMatch", string.Empty)); AppSettings.DeleteSetting("Tester.EventSelect.DirectoryNoMatch");
            }
            if (AppSettings.HasSetting("Tester.EventSelect.ResolvedRootDirectory") == true)
            {
                Changed = true;
                AppSettings.SetSetting("Tester.EventSelect.DEFAULT.ResolvedRootDirectory", (string)AppSettings.GetSetting("Tester.EventSelect.ResolvedRootDirectory", string.Empty)); AppSettings.DeleteSetting("Tester.EventSelect.ResolvedRootDirectory");
            }
            if (AppSettings.HasSetting("Tester.EventSelect.RootDirectoryExcluded") == true)
            {
                Changed = true;
                AppSettings.SetSetting("Tester.EventSelect.DEFAULT.RootDirectoryExcluded", (string)AppSettings.GetSetting("Tester.EventSelect.RootDirectoryExcluded", string.Empty)); AppSettings.DeleteSetting("Tester.EventSelect.RootDirectoryExcluded");
            }
            if (AppSettings.HasSetting("Tester.EventSelect.ReadOnlyDirectorySpared") == true)
            {
                Changed = true;
                AppSettings.SetSetting("Tester.EventSelect.DEFAULT.ReadOnlyDirectorySpared", (string)AppSettings.GetSetting("Tester.EventSelect.ReadOnlyDirectorySpared", string.Empty)); AppSettings.DeleteSetting("Tester.EventSelect.ReadOnlyDirectorySpared");
            }
            if (AppSettings.HasSetting("Tester.EventSelect.EmptyDirectoryRemoved") == true)
            {
                Changed = true;
                AppSettings.SetSetting("Tester.EventSelect.DEFAULT.EmptyDirectoryRemoved", (string)AppSettings.GetSetting("Tester.EventSelect.EmptyDirectoryRemoved", string.Empty)); AppSettings.DeleteSetting("Tester.EventSelect.EmptyDirectoryRemoved");
            }
            if (AppSettings.HasSetting("Tester.EventSelect.AccessDenied") == true)
            {
                Changed = true;
                AppSettings.SetSetting("Tester.EventSelect.DEFAULT.AccessDenied", (string)AppSettings.GetSetting("Tester.EventSelect.AccessDenied", string.Empty)); AppSettings.DeleteSetting("Tester.EventSelect.AccessDenied");
            }
            if (AppSettings.HasSetting("Tester.EventSelect.UnhandledError") == true)
            {
                Changed = true;
                AppSettings.SetSetting("Tester.EventSelect.DEFAULT.UnhandledError", (string)AppSettings.GetSetting("Tester.EventSelect.UnhandledError", string.Empty)); AppSettings.DeleteSetting("Tester.EventSelect.UnhandledError");
            }
            if (AppSettings.HasSetting("Tester.EventSelect.Begin") == true)
            {
                Changed = true;
                AppSettings.SetSetting("Tester.EventSelect.DEFAULT.Begin", (string)AppSettings.GetSetting("Tester.EventSelect.Begin", string.Empty)); AppSettings.DeleteSetting("Tester.EventSelect.Begin");
            }
            if (AppSettings.HasSetting("Tester.EventSelect.BeginServer") == true)
            {
                Changed = true;
                AppSettings.SetSetting("Tester.EventSelect.DEFAULT.BeginServer", (string)AppSettings.GetSetting("Tester.EventSelect.BeginServer", string.Empty)); AppSettings.DeleteSetting("Tester.EventSelect.BeginServer");
            }
            if (AppSettings.HasSetting("Tester.EventSelect.BeginFolder") == true)
            {
                Changed = true;
                AppSettings.SetSetting("Tester.EventSelect.DEFAULT.BeginFolder", (string)AppSettings.GetSetting("Tester.EventSelect.BeginFolder", string.Empty)); AppSettings.DeleteSetting("Tester.EventSelect.BeginFolder");
            }
            if (AppSettings.HasSetting("Tester.EventSelect.EndFolder") == true)
            {
                Changed = true;
                AppSettings.SetSetting("Tester.EventSelect.DEFAULT.EndFolder", (string)AppSettings.GetSetting("Tester.EventSelect.EndFolder", string.Empty)); AppSettings.DeleteSetting("Tester.EventSelect.EndFolder");
            }
            if (AppSettings.HasSetting("Tester.EventSelect.EndServer") == true)
            {
                Changed = true;
                AppSettings.SetSetting("Tester.EventSelect.DEFAULT.EndServer", (string)AppSettings.GetSetting("Tester.EventSelect.EndServer", string.Empty)); AppSettings.DeleteSetting("Tester.EventSelect.EndServer");
            }
            if (AppSettings.HasSetting("Tester.EventSelect.End") == true)
            {
                Changed = true;
                AppSettings.SetSetting("Tester.EventSelect.DEFAULT.End", (string)AppSettings.GetSetting("Tester.EventSelect.End", string.Empty)); AppSettings.DeleteSetting("Tester.EventSelect.End");
            }

            if (Changed)
            {
                AppSettings.SaveSettings();
            }
        }

        private void SetActiveTemplate(string templateName)
        {
            TemplateNameInUse = templateName;
            AppSettings.SetSetting("Tester.EventTemplateSelected", templateName);
            AppSettings.SaveSettings();
            this.toolStripStatusLabel1.Text = "The active template has been set to " + templateName;
        }

        private void SaveEventViewTemplate(string templateName)
        {
            if (templateName.Length == 0 || templateName.IndexOfAny(new char[] { '<', '>', '&', '.', '-', ',' }) >= 0)
            {
                MessageBox.Show("The template name cannot be blank or have any of these characters: < > & . - space", "Illegal name for template", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (WeedKillerEventArgs.WeedKillerActionType x in Enum.GetValues(typeof(WeedKillerEventArgs.WeedKillerActionType)))
            {
                string KeyName = Enum.GetName(typeof(WeedKillerEventArgs.WeedKillerActionType), x);
                AppSettings.SetSetting("Tester.EventSelect." + templateName + "." + KeyName, LogThisEvent[x].ToString());
                AppSettings.SetSetting("Tester.EventColor." + templateName + ".Background." + KeyName, ColorBackgroundThisEvent[x].ToArgb().ToString());
                AppSettings.SetSetting("Tester.EventColor." + templateName + ".Foreground." + KeyName, ColorForegroundThisEvent[x].ToArgb().ToString());
            }
            AppSettings.SaveSettings();
            MessageBox.Show(
                string.Format(
                    "Settings saved for {0}.", templateName),
                    "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            TemplateHasChanged = false;
            this.toolStripStatusLabel1.Text = "Settings saved for template " + templateName;
        }

        private void SaveEventViewCustomColors()
        {
            for (int Index = 0; Index < 32; Index++)
            {
                string KeyName = string.Format("Tester.EventColor.DEFAULT.CustomColors.{0:0#}", Index);
                if (Index < CustomColors.Count)
                {
                    AppSettings.SetSetting(KeyName, CustomColors[Index].ToString());
                }
                else if (AppSettings.HasSetting(KeyName))
                {
                    AppSettings.DeleteSetting(KeyName);
                }
            }
            AppSettings.SaveSettings();
            this.toolStripStatusLabel1.Text = "Event custom colors have been saved.";
        }

        private void DropEventViewTemplate()
        {
            InMultiSelect = true;
            List<string> RemoveThis = new List<string>();

            foreach (string key in AppSettings.GetKeys())
            {
                string[] elements = key.Split(new char[] { '.' });
                if (elements.Length < 4 || elements[0] != "Tester" || elements[2] != this.cbxTemplateName.Text)
                {
                    continue;
                }
                RemoveThis.Add(key);
            }
            while (RemoveThis.Count > 0)
            {
                AppSettings.DeleteSetting(RemoveThis[0]);
                RemoveThis.RemoveAt(0);
            }
            this.TemplateNameInUse = "DEFAULT";

            AppSettings.GetSetting("Tester.EventTemplateSelected", this.TemplateNameInUse);
            AppSettings.SaveSettings();
            TemplateHasChanged = false;
            this.toolStripStatusLabel1.Text = "Template was removed or, if DEFAULT was selected, was reset.";
        }

        private void LoadEventViewTemplates()
        {
            this.TemplateNames.Clear();
            this.cbxTemplateName.Items.Clear();
            InMultiSelect = true;

            this.TemplateNameInUse = (string)AppSettings.GetSetting("Tester.EventTemplateSelected", "DEFAULT");
            bool FoundTemplateInUse = false;
            foreach (string key in AppSettings.GetKeys())
            {
                int StartsAt = key.IndexOf("Tester.EventSelect.", StringComparison.InvariantCultureIgnoreCase);
                if (StartsAt != 0 || key.Length < 21)
                {
                    continue;
                }
                string TemplateName = key.Split(new char[] { '.' })[2];
                if (this.TemplateNames.Contains(TemplateName) == false)
                {
                    this.TemplateNames.Add(TemplateName);
                    this.cbxTemplateName.Items.Add(TemplateName);
                    if (TemplateName == TemplateNameInUse)
                    {
                        this.cbxTemplateName.SelectedItem = TemplateName;
                        FoundTemplateInUse = true;
                    }
                }
            }
            if (FoundTemplateInUse == false)
            {
                TemplateNameInUse = "DEFAULT";
                this.cbxTemplateName.SelectedItem = TemplateNameInUse;
            }
            this.cbxTemplateName.Sorted = true;
            InMultiSelect = false;
            LoadVisibleFileResults();
            TemplateHasChanged = false;
        }

        public WeedKillerConfig config
        {
            set
            {
                _c = value.CloneTyped();
                this.txtDescription.Text = _c.Description;
                this.chkForceTestMode.Checked = _c.TestOnly || !_c.Enabled;
                this.chkForceTestMode.Enabled = _c.Enabled;
            }
        }

        public string ConfigurationFilePath
        {
            set
            {
                this.Text = string.Format("Tester -- {0}", value == null ? "{not saved}" : Path.GetFileName(value));
            }
        }

        private void AppendToScrollingTextbox(ref TextBox t, string appendText)
        {
            t.SelectionStart = t.Text.Length - 1;
            t.Text += appendText;
            t.SelectionLength = t.Text.Length - t.SelectionStart;
            t.ScrollToCaret();
            t.Refresh();
        }

        private void WeedKillerEventReceiver(object sender, WeedKillerEventArgs e)
        {
            WeedKillerEventsBacklog.Enqueue(e);
        }

        private void WeedKillerEventProcessor()
        {
            WeedKillerEventArgs e = WeedKillerEventsBacklog.Dequeue();

            FileEventResults.Add(FileEventResults.Count, e);

            ItemEventCount++;
            if (ItemEventCount % 50 == 0)
            {
                AppendToScrollingTextbox(ref this.txtResults, string.Format("{0:#,0} Items processed.\r\n", ItemEventCount));
            }
            if (this.chkStopAfter.Checked && ItemEventCount >= this.tbarStopAfter.Value)
            {
                worker.AbortRequested = true;
            }
        }

        private void BackgroundWorkerStarts()
        {
            BackgroundWorkerIsProcessing = true;
            OriginalText = this.btnLaunch.Text;
            workset = _c;
            this.lblWaitMessage.Refresh();

            workset.TestOnly = this.chkForceTestMode.Checked ? true : workset.TestOnly;

            this.btnLaunch.Enabled = false;
            this.btnLaunch.Text = "Processing...";
            this.txtResults.Text = string.Format("Start Time: {0:F}\r\n\r\n", DateTime.Now);
            FileEventResults.Clear();
            this.dgvFileResults.Rows.Clear();
            this.chkForceTestMode.Enabled = false;
            this.chkStopAfter.Enabled = false;
            this.tbarStopAfter.Enabled = false;
            this.cbxlEventsOfInterest.Enabled = false;
            this.btnAdjustColorBG.Enabled = false;
            this.btnAdjustColorFG.Enabled = false;
            this.btnClearAll.Enabled = false;
            this.btnInvertAll.Enabled = false;
            this.btnSelectAll.Enabled = false;
            this.cbxTemplateName.Enabled = false;
            this.btnSaveTemplate.Enabled = false;
            this.btnDropTemplate.Enabled = false;
            this.Refresh();

            string OriginalRoot = workset.RootFolder;

            if (!workset.Enabled)
            {
                AppendToScrollingTextbox(ref this.txtResults, "*** WARNING: The process is set to disabled... forcing test mode to on.\r\n");
                workset.TestOnly = true;
            }

            AppendToScrollingTextbox(ref this.txtResults, string.Format("\r\n-- Process: {0}\r\n", workset.Description));

            worker = new WeedKiller();
            ItemEventCount = 0;
        }

        private void BackgroundWorkerEnds()
        {
            LoadVisibleFileResults();
            this.btnLaunch.Enabled = true;
            this.btnLaunch.Text = OriginalText;
            this.cbxlEventsOfInterest.Enabled = true;
            this.chkForceTestMode.Enabled = _c.Enabled;

            this.chkStopAfter.Enabled = true;
            this.tbarStopAfter.Enabled = true;
            this.btnAdjustColorBG.Enabled = true;
            this.btnAdjustColorFG.Enabled = true;
            this.btnClearAll.Enabled = true;
            this.btnInvertAll.Enabled = true;
            this.btnSelectAll.Enabled = true;
            this.cbxTemplateName.Enabled = true;
            this.btnSaveTemplate.Enabled = true;
            this.btnDropTemplate.Enabled = true;
            this.gbxEventsOfInterest.Text = string.Format("Events ({0:#,0} items, {1:#,0} bytes)", DisplayedItemEventCount, DisplayedItemCumulativeSize);
            this.toolStripStatusLabel1.Text = "Weed Killer has completed processing in " + (_c.TestOnly ? "test" : "LIVE") + "mode.";
        }

        private void btnLaunch_Click(object sender, EventArgs e)
        {
            if (!this.chkForceTestMode.Checked && !_c.TestOnly)
            {
                if (MessageBox.Show("The configuration is not set to Test Only, nor have you selected Force Test Mode.  So continuing will actually delete any aged files which qualify?\r\n\r\nIs this what you want?", "CAUTION", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                {
                    return;
                }
            }
            BackgroundWorkerStarts();
            bgwRunWeedKillAction.RunWorkerAsync();
            tmrProgressUpdate.Interval = 250;
            tmrProgressUpdate.Enabled = true;
            this.toolStripStatusLabel1.Text = "The weed killer configuration is now running in " + (_c.TestOnly ? "TEST" : "LIVE") + " mode.";
        }

        private void chkForceTestMode_CheckedChanged(object sender, EventArgs e)
        {
            _c.TestOnly = this.chkForceTestMode.Checked;
            this.toolStripStatusLabel1.Text = "The next lauch will be a " + (_c.TestOnly ? "TEST" : "LIVE") + "run.";
        }

        private void chkStopAfter_CheckedChanged(object sender, EventArgs e)
        {
            this.tbarStopAfter.Enabled = chkStopAfter.Checked;
            this.lblStopAfter.Text = chkStopAfter.Checked ? this.tbarStopAfter.Value.ToString() : "(full test)";
        }

        private void tbarStopAfter_Scroll(object sender, EventArgs e)
        {
            this.lblStopAfter.Text = string.Format("{0:#,0}", this.tbarStopAfter.Value);
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            InMultiSelect = true;
            for (int i = 0; i < this.cbxlEventsOfInterest.Items.Count; i++)
            {
                this.cbxlEventsOfInterest.SetItemChecked(i, true);
            }
            InMultiSelect = false;
            LoadVisibleFileResults();
            TemplateHasChanged = true;
            this.toolStripStatusLabel1.Text = "All events selected for display.";
        }

        private void btnInvertAll_Click(object sender, EventArgs e)
        {
            InMultiSelect = true;
            for (int i = 0; i < this.cbxlEventsOfInterest.Items.Count; i++)
            {
                this.cbxlEventsOfInterest.SetItemChecked(i, !this.cbxlEventsOfInterest.CheckedIndices.Contains(i));
            }
            InMultiSelect = false;
            LoadVisibleFileResults();
            TemplateHasChanged = true;
            this.toolStripStatusLabel1.Text = "Events selected for display are now inverted.";
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            InMultiSelect = true;
            for (int i = 0; i < this.cbxlEventsOfInterest.Items.Count; i++)
            {
                this.cbxlEventsOfInterest.SetItemChecked(i, false);
            }
            InMultiSelect = false;
            LoadVisibleFileResults();
            this.toolStripStatusLabel1.Text = "No events selected for display.";
            TemplateHasChanged = true;
        }

        private void cbxlEventsOfInterest_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (LogThisEvent.ContainsKey(
                (WeedKillerEventArgs.WeedKillerActionType) Enum.Parse(
                    typeof(WeedKillerEventArgs.WeedKillerActionType), 
                    this.cbxlEventsOfInterest.Items[e.Index].ToString(),
                    true)))
            {
                LogThisEvent.Remove(
                    (WeedKillerEventArgs.WeedKillerActionType)Enum.Parse(
                        typeof(WeedKillerEventArgs.WeedKillerActionType),
                        this.cbxlEventsOfInterest.Items[e.Index].ToString(),
                        true));
            }
            LogThisEvent.Add(
                (WeedKillerEventArgs.WeedKillerActionType)Enum.Parse(
                        typeof(WeedKillerEventArgs.WeedKillerActionType),
                        this.cbxlEventsOfInterest.Items[e.Index].ToString(),
                        true),
                e.NewValue == CheckState.Checked);
            if (!InMultiSelect)
            {
                LoadVisibleFileResults();
            }
            TemplateHasChanged = true;
            this.toolStripStatusLabel1.Text = "Event " + this.cbxlEventsOfInterest.Items[e.Index].ToString() + " is now " + 
                (e.NewValue == CheckState.Checked ? "displayed." : "hidden");
        }

        private void btnSaveTemplate_Click(object sender, EventArgs e)
        {
            NameEntry dialog = new NameEntry("Template Name", this.cbxTemplateName.Text);
            if (dialog.ShowDialog(this) == DialogResult.Cancel)
                return;
            var TemplateNameToSave = dialog.GetName;
            if (TemplateNameToSave.Length == 0 || TemplateNameToSave.IndexOfAny(new char[] { '<', '>', '&', '.', '-' }) >= 0)
            {
                MessageBox.Show("The template name cannot be blank or have any of these characters: < > & . - space", "Illegal name for template", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SaveEventViewTemplate(TemplateNameToSave);
            this.toolStripStatusLabel1.Text = "The template named " + TemplateNameInUse + " has been saved, and is now the active template.";
            SetActiveTemplate(TemplateNameToSave);
            LoadEventViewTemplates();
            LoadEventViewTemplate();
        }

        private void btnDropTemplate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("Do you wish to drop the template named {0} ?  This cannot be recovered if you proceed.", TemplateNameInUse), "Drop Template", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.toolStripStatusLabel1.Text = "The template named " + TemplateNameInUse + " has been removed.";
                DropEventViewTemplate();
                LoadEventViewTemplates();
                LoadEventViewTemplate();
            }
            TemplateHasChanged = false;
        }

        private void btnAdjustColorFG_Click(object sender, EventArgs e)
        {
            if (cbxlEventsOfInterest.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Select an event type to adjust", "No event selected", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            ColorDialog cd = new ColorDialog();
            cd.AllowFullOpen = true;
            cd.AnyColor = true;
            cd.FullOpen = true;
            cd.SolidColorOnly = false;
            cd.CustomColors = CustomColors.ToArray();
            cd.Color = (Color)ColorForegroundThisEvent[
                (WeedKillerEventArgs.WeedKillerActionType)Enum.Parse(
                    typeof(WeedKillerEventArgs.WeedKillerActionType), 
                    cbxlEventsOfInterest.Items[cbxlEventsOfInterest.SelectedIndices[0]].ToString(),
                    true)];
            if (cd.ShowDialog() == DialogResult.OK)
            {
                ColorForegroundThisEvent[
                    (WeedKillerEventArgs.WeedKillerActionType) Enum.Parse(
                        typeof(WeedKillerEventArgs.WeedKillerActionType), 
                        cbxlEventsOfInterest.Items[cbxlEventsOfInterest.SelectedIndices[0]].ToString(),
                        true)] = cd.Color;
                CustomColors.Clear();
                CustomColors.AddRange(cd.CustomColors);
                LoadVisibleFileResults();
                SaveEventViewCustomColors();
                this.toolStripStatusLabel1.Text = "Foreground color changed for event " + cbxlEventsOfInterest.Items[cbxlEventsOfInterest.SelectedIndices[0]].ToString();
            }
        }

        private void btnAdjustColorBG_Click(object sender, EventArgs e)
        {
            if (cbxlEventsOfInterest.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Select an event type to adjust", "No event selected", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            ColorDialog cd = new ColorDialog();
            cd.AllowFullOpen = true;
            cd.AnyColor = true;
            cd.FullOpen = true;
            cd.SolidColorOnly = false;
            cd.CustomColors = CustomColors.ToArray();
            cd.Color = (Color)ColorBackgroundThisEvent[
                    (WeedKillerEventArgs.WeedKillerActionType)Enum.Parse(
                        typeof(WeedKillerEventArgs.WeedKillerActionType), 
                        cbxlEventsOfInterest.Items[cbxlEventsOfInterest.SelectedIndices[0]].ToString()
                        ,true)];
            if (cd.ShowDialog() == DialogResult.OK)
            {
                ColorBackgroundThisEvent[
                    (WeedKillerEventArgs.WeedKillerActionType)Enum.Parse(
                        typeof(WeedKillerEventArgs.WeedKillerActionType), 
                        cbxlEventsOfInterest.Items[cbxlEventsOfInterest.SelectedIndices[0]].ToString(),
                        true)] = cd.Color;
                CustomColors.Clear();
                CustomColors.AddRange(cd.CustomColors);
                LoadVisibleFileResults();
                SaveEventViewCustomColors();
                this.toolStripStatusLabel1.Text = "Background color changed for event " + cbxlEventsOfInterest.Items[cbxlEventsOfInterest.SelectedIndices[0]].ToString();
            }
        }

        private void SetTester_FormClosing(object sender, FormClosingEventArgs e)
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

            if (isOverriddable && TemplateHasChanged)
            {
                switch (MessageBox.Show(
                    string.Format(
                        "You have not saved changes to the template {0}.  Do you wish to update them before you leave?",
                        TemplateNameInUse),
                    "Template Changed",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Stop))
                {
                    case DialogResult.Yes:
                        SaveEventViewTemplate(TemplateNameInUse);
                        SetActiveTemplate(TemplateNameInUse);
                        break;

                    case DialogResult.No:
                        break;

                    case DialogResult.Cancel:
                        e.Cancel = true;
                        this.toolStripStatusLabel1.Text = "Exit aborted... it's ok to save your work now";
                        break;
                }
            }
        }

        private void cbxTemplateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((string)this.cbxTemplateName.SelectedItem == TemplateNameInUse || InTemplateAdjustment)
            {
                return;
            }
            InTemplateAdjustment = true;
            string ChangeToTemplate = (string)this.cbxTemplateName.SelectedItem;
            bool SkipActions = false;
            if (TemplateHasChanged)
            {
                switch (MessageBox.Show("Do you wish to update these changes?", string.Format("The settings for template {0} have changed", TemplateNameInUse), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning))
                {
                    case DialogResult.Cancel:
                        this.cbxTemplateName.SelectedItem = TemplateNameInUse;
                        SkipActions = true;
                        break;

                    case DialogResult.Yes:
                        SaveEventViewTemplate(TemplateNameInUse);
                        break;

                    case DialogResult.No:
                        break;
                }
            }
            if (SkipActions == false)
            {
                TemplateNameInUse = ChangeToTemplate;
                this.toolStripStatusLabel1.Text = "Template selection changed to " + TemplateNameInUse;
                SetActiveTemplate(TemplateNameInUse);
                LoadEventViewTemplates();
                LoadEventViewTemplate();
            }
            InTemplateAdjustment = false;
        }

        delegate void updateTxtResultsDelegate(string newText);

        private void updateTxtResults(string newText)
        {
            if (txtResults.InvokeRequired)
            {
                // this is worker thread
                updateTxtResultsDelegate del = new updateTxtResultsDelegate(updateTxtResults);
                txtResults.Invoke(del, new object[] { newText });
            }
            else
            {
                // this is UI thread
                this.txtResults.SelectionStart = this.txtResults.Text.Length - 1;
                this.txtResults.Text += newText;
                this.txtResults.SelectionLength = this.txtResults.Text.Length - this.txtResults.SelectionStart;
                this.txtResults.ScrollToCaret();
                this.txtResults.Refresh();
            }
        }

        //delegate void QueueWeedKillerEventDelegate(WeedKillerEventArgs newEvent);

        //private void QueueWeedKillerEvent(WeedKillerEventArgs newEvent)
        //{
        //    if (txtResults.InvokeRequired)
        //    {
        //        // this is worker thread
        //        updateTxtResultsDelegate del = new updateTxtResultsDelegate(updateTxtResults);
        //        txtResults.Invoke(del, new object[] { newText });
        //    }
        //    else
        //    {
        //        // this is UI thread
        //        this.txtResults.SelectionStart = this.txtResults.Text.Length - 1;
        //        this.txtResults.Text += newText;
        //        this.txtResults.SelectionLength = this.txtResults.Text.Length - this.txtResults.SelectionStart;
        //        this.txtResults.ScrollToCaret();
        //        this.txtResults.Refresh();
        //    }
        //}

        private void bgwRunWeedKillAction_DoWork(object sender, DoWorkEventArgs e)
        {
            string WriteThis = string.Empty;
            try
            {
                worker.WeedKillerEvent += new WeedKillerEventHandler(WeedKillerEventReceiver);
                worker.KillWeeds(workset);
            }
            catch (Exception ex)
            {
                updateTxtResults("\r\n" + DetailedException.WithMachineContent(ref ex));
            }
            finally
            {
                try
                {
                    worker.WeedKillerEvent -= new WeedKillerEventHandler(WeedKillerEventReceiver);
                }
                finally
                {
                }
            }

            updateTxtResults(string.Format("\r\nEnd Time: {0:F}, {1:#,0} events captured\r\n", DateTime.Now, ItemEventCount));
        }

        private void bgwRunWeedKillAction_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void bgwRunWeedKillAction_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorkerIsProcessing = false;
        }

        private void tmrProgressUpdate_Tick(object sender, EventArgs e)
        {
            tmrProgressUpdate.Enabled = false;
            while (WeedKillerEventsBacklog.Count > 0)
            {
                WeedKillerEventProcessor();
                tmrProgressUpdate.Enabled = true;
            }
            if (tmrProgressUpdate.Enabled) return;
            if (BackgroundWorkerIsProcessing)
            {
                tmrProgressUpdate.Enabled = true;
            }
            else
            {
                BackgroundWorkerEnds();
            }
        }
    }
}
