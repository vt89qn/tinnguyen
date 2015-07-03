using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using PokerTexas.App_Controller;
using System.Data.Entity.Core.EntityClient;
using PokerTexas.App_Common;

namespace PokerTexas
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string strConnectionString = string.Empty;
            if (args != null && args.Length > 0)
            {
                strConnectionString = "Data Source=.\\db_" + args[0] + ".sqlite";
            }
            if (!string.IsNullOrEmpty(strConnectionString))
            {
                Global.DBContext = new App_Context.PokerContext(strConnectionString);
            }
            else
            {
                Global.DBContext = new App_Context.PokerContext();
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (DateTime.Today >= new DateTime(2015, 08, 01))
            {
                return;
            }
            Application.Run(new App_Present.MainForm());
        }
    }
}
