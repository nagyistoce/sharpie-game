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
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();
            Console.SetBufferSize(Console.WindowWidth + 1, Console.WindowHeight + 1);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            for (int i = 1; i <= Console.WindowWidth; i++)
            {
                Console.Write(" ");
            }
            Console.SetCursorPosition(Console.WindowWidth - 2 - Program.version.Length, 0);
            Console.Write("v{0}", Program.version);    //Wersja programu
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            for (int i = 1; i <= Console.WindowWidth; i++)
            {
                Console.Write(" ");
            }
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
        }

        public void Glowny() //ekran główny
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(Console.WindowWidth / 2 - Locale.greeting.Length / 2, 3);
            Console.Write(Locale.greeting);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(1, Console.WindowHeight - 1);
            Console.Write("F2 - Nowa gra");
        }

        public void Score()
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(1, Console.WindowHeight - 1);
            Console.Write("Wynik: 0");
        }
    }
}
