using MetMah.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EscapeFromMetMah
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new MainForm());
            //Application.Run(new Form1(new System.IO.DirectoryInfo(@"C:\Users\Asus\Desktop\Images")));
        }
    }
}
