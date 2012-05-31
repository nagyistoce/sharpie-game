using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeSharp
{
    class Interface
    {
        public void Draw()
        {
            Console.ResetColor();
            Console.Clear();
            Console.SetBufferSize(Console.WindowWidth + 1, Console.WindowHeight + 1);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            for (int i = 1; i <= Console.WindowWidth; i++)
            {
                Console.Write(" ");
            }
            Console.SetCursorPosition(0, 0);
            WritePanelRight("v" + Program.version);    //Wersja programu
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
        }

        public void Glowny() //ekran główny
        {
            Console.ResetColor();
            Logo();
            Cursor.WriteXY(Console.WindowWidth / 2 - Locale.desc.Length / 2, 9, Locale.desc);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            WritePanelLeft("F2 - Nowa gra");
            Console.ResetColor();
        }

        public void Score(int score)
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            WritePanelLeft("Wynik: " + score);
            Console.ResetColor();
        }

        public static void WritePanelLeft(string text)
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(1, Console.WindowHeight - 1);
            Console.Write(text);
            Console.ResetColor();
        }

        public static void WritePanelRight(string text)
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Cursor.WriteXY(Console.WindowWidth - 1 - text.Length, Console.WindowHeight - 1, text);
            Console.ResetColor();
        }

        private void Logo()
        {
            string a = "█████  █   █   ███   ████   ████   █  █████";
            string b = "█      █   █  █   █  █   █  █   █  █  █    ";
            string c = "█████  █████  █   █  ████   ████   █  ███  ";
            string d = "    █  █   █  █████  █   █  █      █  █    ";
            string e = "█████  █   █  █   █  █   █  █      █  █████";

            Cursor.WriteXY(Console.WindowWidth / 2 - a.Length / 2, 3, a);
            Cursor.WriteXY(Console.WindowWidth / 2 - b.Length / 2, 4, b);
            Cursor.WriteXY(Console.WindowWidth / 2 - c.Length / 2, 5, c);
            Cursor.WriteXY(Console.WindowWidth / 2 - d.Length / 2, 6, d);
            Cursor.WriteXY(Console.WindowWidth / 2 - e.Length / 2, 7, e);
        }
    }
}
