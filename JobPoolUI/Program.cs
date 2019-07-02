using System;
using System.Windows.Forms;

namespace JobPoolUI
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
			CommonCode.DataLayer.Const.Initialize(@"JobPoolUI\bin\Debug\");
			Application.Run(new FMain());
        }
    }
}
