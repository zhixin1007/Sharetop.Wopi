using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tester
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static string GetCaption(this string filename)
        {
            if (filename.Contains("."))
            {
                return filename.Substring(0, filename.LastIndexOf("."));
            }
            else
            {
                return filename;
            }
        }

        public static string GetExtension(this string filename)
        {
            if (filename.Contains("."))
            {
                return filename.Substring(filename.LastIndexOf('.') + 1);
            }
            else
            {
                return "";
            }
        }
    }
}
