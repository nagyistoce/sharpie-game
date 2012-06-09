﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sharpie
{
	public class Text
	{
		// metody pozycjonowania tekstu
        public static int CenterX(string tekst)
        {
            return Console.WindowWidth / 2 - tekst.Length / 2;
        }

        public static int CenterY(string tekst)
        {
            return Console.WindowHeight / 2 - tekst.Length / 2;
        }

        public static int Right(string tekst)
        {
            return Console.WindowWidth - tekst.Length;
        }

        public static int Right(string tekst, int margin)
        {
            return margin - tekst.Length;
        }

        public static void WriteXY(int x, int y, string text)
        {
            //int left = Console.CursorLeft;
            //int top = Console.CursorTop;
            Console.SetCursorPosition(x, y);
            Console.Write(text);
            //Console.SetCursorPosition(left, top);
        }
	}
}
