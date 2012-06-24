using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public static class Panel
    {
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
            Text.WriteXY(Console.WindowWidth - 1 - text.Length, Console.WindowHeight - 1, text);
            Console.ResetColor();
        }

        public static void Draw(string version)
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
            WritePanelRight("v" + version);    //Wersja programu
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
        }
    }
}
