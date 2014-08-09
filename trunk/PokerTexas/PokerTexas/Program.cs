using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace PokerTexas
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                if (IntPtr.Size == 4)
                {
                    if (File.Exists("SQLite.Interop_x86.dll"))
                    {
                        if (File.Exists("SQLite.Interop.dll"))
                        {
                            File.Delete("SQLite.Interop.dll");
                        }
                        File.Copy("SQLite.Interop_x86.dll", "SQLite.Interop.dll");
                    }
                }
                else if (IntPtr.Size == 8)
                {
                    if (File.Exists("SQLite.Interop_x64.dll"))
                    {
                        if (File.Exists("SQLite.Interop.dll"))
                        {
                            File.Delete("SQLite.Interop.dll");
                        }
                        File.Copy("SQLite.Interop_x64.dll", "SQLite.Interop.dll");
                    }
                }
            }
            catch { }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (DateTime.Today >= new DateTime(2014, 09, 01))
            {
                return;
            }
            Application.Run(new FormMain());
        }
    }
}
