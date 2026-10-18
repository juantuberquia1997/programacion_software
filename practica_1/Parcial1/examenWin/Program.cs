using System;
using System.Windows.Forms;

namespace frmParcial1
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmParcial1());
        }
    }
}
