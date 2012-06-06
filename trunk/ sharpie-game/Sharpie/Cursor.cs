using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sharpie
{
    class Cursor
    {
        public static void Move(int direction)
        {
            if (direction == 0) // góra
            {
                if (Console.CursorTop == 0) { Console.SetCursorPosition(Console.CursorLeft, Console.WindowHeight - 2); }
                else { Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1); }
            }

            if (direction == 1) // prawo
            {
                if (Console.CursorLeft == Console.WindowWidth - 1) { Console.SetCursorPosition(0, Console.CursorTop); }
                else { Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop); }
            }

            if (direction == 2) // dół
            {
                if (Console.CursorTop == Console.WindowHeight - 2) { Console.SetCursorPosition(Console.CursorLeft, 0); }
                else { Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 1); }
            }

            if (direction == 3) // lewo
            {
                if (Console.CursorLeft == 0) { Console.SetCursorPosition(Console.WindowWidth - 1, Console.CursorTop); }
                else { Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop); }
            }

        }

        public static int CenterX()
        {
            return Console.WindowWidth / 2;
        }

        public static int CenterY()
        {
            return Console.WindowHeight / 2;
        }
    }
}
