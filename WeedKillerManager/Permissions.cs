using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BOG.Framework;
using BOG.WeedKiller;

namespace WeedKillerManager
{
    public partial class Permissions : Form
    {
        private NTGroup g = new NTGroup();
        private SettingsDictionary AppSettings;
        private WeedKillerConfigSet cs = new WeedKillerConfigSet();
        private string ServerName = string.Empty;

        public Permissions(ref SettingsDictionary appSettings, ref WeedKillerConfigSet c1)
        {
            InitializeComponent();
            AppSettings = appSettings;
            this.txtNTAccount.Text = (string)AppSettings.GetSetting("Permissions.NTAccount", string.Empty);
            foreach (WeedKillerConfig c in c1.ConfigSet) cs.ConfigSet.Add(c.CloneTyped());
        }

        private void WeedKillerEventProcessor(object sender, WeedKillerEventArgs e)
        {
            switch (e.Action)
            {
                case WeedKillerEventArgs.WeedKillerActionType.BeginServer:
                    AppendToScrollingTextbox(ref this.txtResults, string.Format("\r\nServer: \\{0} ...", e.Message));
                    break;
                case WeedKillerEventArgs.WeedKillerActionType.EndServer:
                    ServerName = string.Empty;
                    break;
                case WeedKillerEventArgs.WeedKillerActionType.ResolvedRootDirectory:
                    AppendToScrollingTextbox(ref this.txtResults, string.Format("\r\n{0} ACL: {1}",
                        this.rbAddAccess.Checked ? "Add to" : "Remove from",
                        e.Path));
                    string result = "OK";
                    try
                    {
                        if (this.rbAddAccess.Checked)
                        {
                            g.AddDirectorySecurity(e.Path, this.txtNTAccount.Text,
                                    System.Security.AccessControl.FileSystemRights.ListDirectory |
                                    System.Security.AccessControl.FileSystemRights.Traverse |
                                    System.Security.AccessControl.FileSystemRights.Delete |
                                    System.Security.AccessControl.FileSystemRights.DeleteSubdirectoriesAndFiles,
                                    System.Security.AccessControl.InheritanceFlags.ContainerInherit |
                                    System.Security.AccessControl.InheritanceFlags.ObjectInherit,
                                    System.Security.AccessControl.PropagationFlags.None,
                                    System.Security.AccessControl.AccessControlType.Allow);
                        }
                        else
                        {
                            g.RemoveDirectorySecurity(e.Path, this.txtNTAccount.Text,
                                    System.Security.AccessControl.FileSystemRights.ListDirectory |
                                    System.Security.AccessControl.FileSystemRights.Traverse |
                                    System.Security.AccessControl.FileSystemRights.Delete |
                                    System.Security.AccessControl.FileSystemRights.DeleteSubdirectoriesAndFiles,
                                    System.Security.AccessControl.InheritanceFlags.ContainerInherit |
                                    System.Security.AccessControl.InheritanceFlags.ObjectInherit,
                                    System.Security.AccessControl.PropagationFlags.None,
                                    System.Security.AccessControl.AccessControlType.Allow);
                        }
                    }
                    catch (Exception err)
                    {
                        result = string.Format("ERR: {0}", err.Message);
                    }
                    AppendToScrollingTextbox(ref this.txtResults, "\r\n" + result);
                    
                    break;
                default:
                    // not one we deal with.
                    break;
            }
        }

        private void AppendToScrollingTextbox(ref TextBox t, string appendText)
        {
            t.Text += appendText;
            t.SelectionStart = t.Text.Length;
            t.SelectionLength = 0;
            t.ScrollToCaret();
            t.Refresh();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (this.txtNTAccount.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter the name of the account", "No account specified", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.btnApply.Enabled = false;
            this.txtResults.Text = "Applying permission changes...";
            this.Refresh();

            WeedKiller worker = new WeedKiller();
            try
            {
                worker.WeedKillerEvent += new WeedKillerEventHandler(WeedKillerEventProcessor);
                foreach (WeedKillerConfig w in cs.ConfigSet)
                {
                    AppendToScrollingTextbox(ref this.txtResults, "\r\n--> " + w.Description);
                    worker.KillWeeds(w);
                }
            }
            catch (Exception ex)
            {
                AppendToScrollingTextbox(ref this.txtResults, "\r\n" + DetailedException.WithMachineContent(ref ex));
            }
            finally
            {
                try
                {
                    worker.WeedKillerEvent -= new WeedKillerEventHandler(WeedKillerEventProcessor);
                }
                finally
                {
                }
            }
            this.btnApply.Enabled = true;
        }

        private void Permissions_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.txtNTAccount.Text.Trim().Length == 0) return;
            AppSettings.SetSetting("Permissions.NTAccount", this.txtNTAccount.Text);
            AppSettings.SaveSettings();
        }
    }
}
