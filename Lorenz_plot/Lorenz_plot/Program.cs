using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.WindowsForms;

namespace lorenz
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // This sets up the application to use visual styles and high DPI settings
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Create and run the main form
            Application.Run(new Form1());
        }
    }
}