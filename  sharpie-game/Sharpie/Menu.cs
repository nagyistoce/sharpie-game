using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sharpie
{
    class Menu
    {
        string[] entry;
        short menuYPos;
        short y;
        ConsoleKeyInfo key;

        public Menu(string[] pozycje, short wysokosc)
        {
            this.entry = pozycje;
            this.menuYPos = wysokosc;
        }

        public short Show()
        {
            for (y = 0; y < entry.Length; y++)
            {
                DrawEntry(y, ConsoleColor.Gray, ConsoleColor.Black);
            }
            y = 0;
            do 
            {
                DrawEntry(y, ConsoleColor.Black, ConsoleColor.Gray);
                key = Console.ReadKey(true);
                DrawEntry(y, ConsoleColor.Gray, ConsoleColor.Black);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        y--;
                        if (y == -1)
                        {
                            y = (short)entry.Length;
                            y--;
                        }
                        continue;
                    case ConsoleKey.DownArrow:
                        y++;
                        if (y == (short)entry.Length)
                        {
                            y = 0;
                        }
                        continue;
                }
            } while (true);
            //} while (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Escape);
            return y;
        }

        public void DrawEntry(int whichentry, ConsoleColor textcolor, ConsoleColor backcolor)
        {
            Console.ForegroundColor = textcolor;
            Console.BackgroundColor = backcolor;
            Cursor.WriteXY(Console.WindowWidth / 2 - entry[whichentry].Length / 2, menuYPos + whichentry,entry[whichentry]);
        }
    }
}
