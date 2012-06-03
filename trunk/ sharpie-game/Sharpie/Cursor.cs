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
			if (direction == 0) { Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1); }   // góra
			if (direction == 2) { Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 1); }   // dół
			if (direction == 3) { Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop); }   // lewo
			if (direction == 1) { Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop); }   // prawo
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
