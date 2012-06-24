using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace MapEditor
{
    class Program
    {
		public static string name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        public static string version = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Windows.Forms.Application.ExecutablePath).FileVersion;

		[STAThread]
        static void Main(string[] args)
        {
            Console.SetWindowSize(70, 41);
            Console.Title = name;
            Console.CursorSize = 100;
            Console.CursorVisible = false;
            do
            {
                Panel.Draw(version);
                Interface.Glowny();
            } while (true);
        }
    }
}
