using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeSharp
{
    class Program
    {
        public static string name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        public static string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Remove(5);
        static void Main(string[] args)
        {
            Console.SetWindowSize(60,50);
            Console.Title = name;
            Console.CursorVisible = false;
            Interface window = new Interface();
            window.Draw();
            window.Glowny();
            ConsoleKeyInfo a;
            do 
            {
                a = Console.ReadKey(true);
            } while (a.Key != ConsoleKey.F2);
            Game gra = new Game();
            Console.ReadKey(true);
        }
    }
}
