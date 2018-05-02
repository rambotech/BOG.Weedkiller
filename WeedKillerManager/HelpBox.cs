using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace WeedKillerManager
{
    public partial class HelpBox : Form
    {
        public HelpBox()
        {
            InitializeComponent();
        }

        private void HelpBox_Load(object sender, EventArgs e)
        {
            System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
            string x = Path.Combine(Path.GetDirectoryName(a.CodeBase.Replace("file:///", string.Empty)), "WeedKillerHelp.htm");
            this.webBrowser1.Navigate(x);
        }
    }
}
