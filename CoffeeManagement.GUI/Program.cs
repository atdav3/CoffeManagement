using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoffeeManagement.GUI.Forms;
using CoffeeManagement.GUI.Helpers;

namespace CoffeeManagement.GUI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (sender, args) => UiError.Show(args.Exception);
            AppDomain.CurrentDomain.UnhandledException += (sender, args) => AppLogger.Error(args.ExceptionObject as Exception ?? new Exception("Unknown unhandled exception."));
            using (LoginForm login = new LoginForm())
            {
                if (login.ShowDialog() == DialogResult.OK)
                    Application.Run(new MainForm(login.Session));
            }
        }
    }
}
