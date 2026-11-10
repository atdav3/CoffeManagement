using System;
using System.IO;
using System.Text;

namespace CoffeeManagement.GUI.Helpers
{
    internal static class AppLogger
    {
        private static readonly object Sync = new object();
        private static readonly string LogDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CoffeeManagement", "Logs");

        public static void Error(Exception exception)
        {
            try
            {
                lock (Sync)
                {
                    Directory.CreateDirectory(LogDirectory);
                    string path = Path.Combine(LogDirectory, "app-" + DateTime.Now.ToString("yyyyMMdd") + ".log");
                    File.AppendAllText(path, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " [ERROR] " + exception + Environment.NewLine, Encoding.UTF8);
                }
            }
            catch { /* Logging must not crash the application. */ }
        }
    }
}
