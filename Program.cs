using System;
using System.Windows.Forms;

namespace TestCustomerWinForms.Net8
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}