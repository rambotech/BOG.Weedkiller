using System;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.ApplicationServices;

namespace WeedKillerManager
{
    class SingleInstanceApplication : WindowsFormsApplicationBase
    {
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        public SingleInstanceApplication()
        {
            this.IsSingleInstance = true;
            this.EnableVisualStyles = true;
            this.Startup += new StartupEventHandler(SingleInstanceApplication_Startup);
            this.StartupNextInstance += new StartupNextInstanceEventHandler(SingleInstanceApplication_StartupNextInstance);
        }

        ~SingleInstanceApplication()
        {
            this.Startup -= new StartupEventHandler(SingleInstanceApplication_Startup);
            this.StartupNextInstance -= new StartupNextInstanceEventHandler(SingleInstanceApplication_StartupNextInstance);
        }

        void SingleInstanceApplication_StartupNextInstance(object sender, StartupNextInstanceEventArgs eventArgs)
        {
            foreach (string s in eventArgs.CommandLine)
            {
                foreach (string ThisFileName in Directory.GetFiles(Path.GetDirectoryName(s), Path.GetFileName(s), SearchOption.TopDirectoryOnly))
                {
                    ((ManagerMDI)this.MainForm).OpenFile(ThisFileName);
                }
            }
            SetForegroundWindow(this.MainForm.Handle);
        }

        void SingleInstanceApplication_Startup(object sender, StartupEventArgs eventArgs)
        {
            this.MainForm = new ManagerMDI(null);
            foreach (string s in eventArgs.CommandLine)
            {
                foreach (string ThisFileName in Directory.GetFiles(Path.GetDirectoryName(s), Path.GetFileName(s), SearchOption.TopDirectoryOnly))
                {
                    ((ManagerMDI)this.MainForm).OpenFile(ThisFileName);
                }
            }
            SetForegroundWindow(this.MainForm.Handle);
        }
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// This application is a single-instance application.
        /// </summary>
        /// 
        [STAThread]
        static void Main(string[] args)
        {
            Application.SetCompatibleTextRenderingDefault(false);
            SingleInstanceApplication app = new SingleInstanceApplication();
            app.Run(args);
        }
    }
}
