using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Watch_Face_Editor
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
#if NETCOREAPP3_1 || NET6_0 || NET7_0 || NET8_0 || NET9_0 || NET10_0
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
#endif
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(args));
        }
    }
}
