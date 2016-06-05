using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Ex05.FourInARow
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /// TODO : Check if it is allowed
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GameSettingsMenu());
        }
    }
}
