using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sharpie
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
            Menu menu = new Menu(new string[] {"Nowa gra", "Ustawienia", "Instrukcje", "O grze", "", "Aktualizuj"}, 13, ConsoleColor.White, ConsoleColor.Green);
            window.Draw();
            window.Glowny();
            window.DialogShow(20,11,40,25,"Menu");
            int value = menu.Show();
            switch (value)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
            }
            Game gra = new Game();
            Console.ReadKey(true);
        }
    }
}
