using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapEditor
{
    class Program
    {
		public static string name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
		public static string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Remove(5);

		[STAThread]
        static void Main(string[] args)
        {
            Console.SetWindowSize(70, 41);
            Console.Title = name;
            Console.CursorSize = 100;
            Console.CursorVisible = false;
            Interface window = new Interface();
            do
            {
                window.Draw();
                window.Glowny();
            } while (true);
        }
    }
}
