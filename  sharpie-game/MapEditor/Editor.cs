using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Sharpie;

namespace MapEditor
{
    class Editor
    {
        static int max_x = Console.WindowWidth;
        static int max_y = Console.WindowHeight - 1;
        string[,] board = new string[max_x, max_y];
        List<string> wall = new List<string> { "╔", "╗", "═", "║", "╚", "╝" };
        ConsoleKeyInfo key;

        public Editor()
        {
            Interface editor = new Interface();
            editor.Draw();
            Start();
            Console.ReadKey(true);
        }

        private void Start()
        {
            Dialog pause = new Dialog(0, ConsoleColor.White, ConsoleColor.DarkMagenta);
            Console.CursorVisible = true;
            Console.SetCursorPosition(Cursor.CenterX(), Cursor.CenterY());
            do
            {
                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        Cursor.Move(0);
                        break;
                    case ConsoleKey.RightArrow:
                        Cursor.Move(1);
                        break;
                    case ConsoleKey.DownArrow:
                        Cursor.Move(2);
                        break;
                    case ConsoleKey.LeftArrow:
                        Cursor.Move(3);
                        break;
                    case ConsoleKey.Spacebar:
                        break;
                }
            } while (true);
        }
    }
}
