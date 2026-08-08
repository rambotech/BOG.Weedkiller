using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using BOG.SwissArmyKnife;

namespace BOG.WeedKillerManager.App
{
    public partial class HelpBox : Form
    {
		AssemblyVersion av;

		public HelpBox()
        {
            InitializeComponent();
			av = new AssemblyVersion(SwissArmyKnife.AssemblyVersion.AssemblySource.Executing);
		}

		private void HelpBox_Load(object sender, EventArgs e)
        {
            System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
            string x = Path.Combine(Path.GetDirectoryName(av.Filename), "WeedKillerHelp.htm");
            this.webBrowser1.Navigate(x);
        }
    }
}
