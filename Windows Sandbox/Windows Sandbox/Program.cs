using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Windows_Sandbox
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Run the application with provided arguments
            if (args.Length != 0 && args[0] != null)
            {
                var appInterface = new Form1();
                appInterface.ShowDialog();
               appInterface.interfaceMan(args);
                Environment.Exit(0);

            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
