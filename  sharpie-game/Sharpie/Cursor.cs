using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeSharp
{
    class Cursor
    {
        public void Move(int direction)
        {
            if (direction == 0) { Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1); }   // góra
            if (direction == 2) { Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 1); }   // dół
            if (direction == 3) { Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop); }   // lewo
            if (direction == 1) { Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop); }   // prawo
        }

        public void SMove(int direction)
        {
            if (direction == 0) { Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop - 1); }   // góra
            if (direction == 2) { Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop + 1); }   // dół
            if (direction == 3) { Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop); }   // lewo
            if (direction == 1) { throw new Exception("Console.Write posuwa kursor w prawo :>"); }
        }
    }
}
