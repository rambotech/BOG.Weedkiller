using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.IO;

//  Copyright (c) 2009-2016, John J Schultz, all rights reserved.

namespace WeedKillerManager
{
    partial class AboutBox : Form
    {
        WinAppInfo x = new WinAppInfo();
        int SplitterPosition = 0;

        public AboutBox()
        {
            InitializeComponent();
            BOG.Framework.AssemblyVersion av = new BOG.Framework.AssemblyVersion(System.Reflection.Assembly.GetExecutingAssembly());

            x.AssemblyVersion = av.Version;
            x.BuildDate = av.BuildDate;
            x.Copyright = AssemblyCopyright;
            x.Description = AssemblyDescription;
            x.FullPath = av.FullPath;
            x.Name = av.Name;
            x.Processor = av.Processor;
            x.ProductName = AssemblyProduct;
            x.Title = AssemblyTitle;
            x.Version = av.Version;

            this.pgInfo.SelectedObject = x;
            SplitterPosition = this.splitContainer1.SplitterDistance;
            this.Text = string.Format(this.Text, x.ProductName);

            string ReadMeFile = Path.Combine(Path.GetDirectoryName(x.FullPath), "Read_Me.txt");
            if (File.Exists(ReadMeFile))
            {
                try
                {
                    using (StreamReader r = new StreamReader(ReadMeFile))
                    {
                        this.txtReadMe.Text = r.ReadToEnd();
                        r.Close();
                    }
                }
                catch
                {
                    this.txtReadMe.Text = "** Error loading Read_Me.txt contents **";
                }
            }
            else
            {
                this.tabControl1.TabPages.RemoveAt(2);
            }
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }
        #endregion

        private void linkHomePage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkHomePage.LinkVisited = true;
            System.Diagnostics.Process.Start(this.linkHomePage.Tag.ToString());
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.linkHomePage.LinkVisited = true;
            System.Diagnostics.Process.Start(this.linkHomePage.Tag.ToString());
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void splitContainer1_Resize(object sender, EventArgs e)
        {
            this.splitContainer1.SplitterDistance = SplitterPosition;
        }
    }

    [DefaultPropertyAttribute("Description")]
    class WinAppInfo
    {
        private string _Title;
        private string _ProductName;
        private string _AssemblyVersion;
        private string _Copyright;
        private string _Description;

        private string _BuildDate;
        private string _FullPath;
        private string _Name;
        private string _Processor;
        private string _Version;

        [CategoryAttribute("Admin"), DisplayNameAttribute("Title"), DescriptionAttribute("The name assigned to the project"), ReadOnly(true)]
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        [CategoryAttribute("Admin"), DisplayNameAttribute("Product Name"), DescriptionAttribute("The name assigned to the project"), ReadOnly(true)]
        public string ProductName
        {
            get { return _ProductName; }
            set { _ProductName = value; }
        }

        [CategoryAttribute("Admin"), DisplayNameAttribute("Assembly Version"), DescriptionAttribute("The specific build number.. (1.0.0.X is Alpha, 1.0.1.X is Beta)"), ReadOnly(true)]
        public string AssemblyVersion
        {
            get { return _AssemblyVersion; }
            set { _AssemblyVersion = value; }
        }

        [CategoryAttribute("Admin"), DisplayNameAttribute("Copyright"), DescriptionAttribute(""), ReadOnly(true)]
        public string Copyright
        {
            get { return _Copyright; }
            set { _Copyright = value; }
        }

        [CategoryAttribute("Admin"), DisplayNameAttribute("Description"), DescriptionAttribute(""), ReadOnly(true)]
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        [CategoryAttribute("Technical"), DisplayNameAttribute("Build Date"), DescriptionAttribute("Date this version was constructed."), ReadOnly(true)]
        public string BuildDate
        {
            get { return _BuildDate; }
            set { _BuildDate = value; }
        }

        [CategoryAttribute("Technical"), DisplayNameAttribute("Application Location"), DescriptionAttribute("The location of the specific executable running."), ReadOnly(true)]
        public string FullPath
        {
            get { return _FullPath; }
            set { _FullPath = value; }
        }

        [CategoryAttribute("Technical"), DisplayNameAttribute("Application Location"), DescriptionAttribute("The name stored within the executable itself."), ReadOnly(true)]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        [CategoryAttribute("Technical"), DisplayNameAttribute("Processor"), DescriptionAttribute("Targeted operating platform"), ReadOnly(true)]
        public string Processor
        {
            get { return _Processor; }
            set { _Processor = value; }
        }

        [CategoryAttribute("Technical"), DisplayNameAttribute("Version"), DescriptionAttribute("The file version of the executable."), ReadOnly(true)]
        public string Version
        {
            get { return _Version; }
            set { _Version = value; }
        }
    }
}
