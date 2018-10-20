using System;
using System.Windows.Forms;
using Akka.Actor;
using Akka.Persistence.Sqlite;

namespace Voltcode.AkkaNetPersistenceSamples.Accounting.CashMachine
{
    static class Program
    {       
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {                        
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CashMachineFrontend());
        }
    }
}
