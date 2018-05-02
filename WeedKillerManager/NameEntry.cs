using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeedKillerManager
{
    public partial class NameEntry : Form
    {
        public NameEntry(string title, string defaultName)
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
            this.Text = title;
            this.txtName.Text = defaultName;
            this.txtName.SelectionStart = 0;
            this.txtName.SelectionLength = this.txtName.Text.Length;
            this.txtName.Focus();
        }

        public string GetName { get { return this.txtName.Text; } }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
