using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Scut.Client.Runtime;

namespace ScutClientSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            LuaRuntime.Start(Application.StartupPath);
            Application.Run(new Main());

        }
    }
}
