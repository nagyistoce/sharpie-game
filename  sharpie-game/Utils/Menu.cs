﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace Utils
{
    public class Menu
    {
        public List<string> entry;
        int menuYPos, menuXPos, y;
        ConsoleColor tekst, tlo;
        ConsoleKeyInfo key;
        public int prevPos;

        public Menu(string[] pozycje, int x, int y, ConsoleColor tekst, ConsoleColor tlo)
        {
            entry = new List<string>(pozycje);
            this.menuYPos = y;
            this.menuXPos = x;
            this.tekst = tekst;
            this.tlo = tlo;
        }

        public int ShowHorizontal(bool CanEscape, bool CanFocus, int setPos)
        {
            if (entry[1] != "")
            {
                for (int i = 1; i < entry.Count; i += 2)
                {
                    entry.Insert(i, "");
                }
            }

            for (y = 0; y < entry.Count; y++)
            {
                DrawEntryH(y, tekst, tlo);
            }
            y = setPos*2;
            do
            {
                DrawEntryH(y, tlo, tekst);
                key = Console.ReadKey(true);
                DrawEntryH(y, tekst, tlo);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        y--;
                        if (y < 0)
                        {
                            y = entry.Count - 1;
                        }
                        while (entry[y] == "")
                        {
                            y--;
                        }
                        continue;
                    case ConsoleKey.DownArrow:
                        y++;
                        if (y >= entry.Count)
                        {
                            if (CanFocus)
                            {
                                prevPos = y / 2;
                                return -2;
                            }
                            else { y = 0; }
                        }
                        while (entry[y] == "")
                        {
                            y++;
                        }

                        continue;
                    case ConsoleKey.Enter:
                        return y / 2;
                }
                if (CanEscape)
                {
                    switch (key.Key)
                    {
                        case ConsoleKey.Escape:
                            prevPos = y / 2;
                            return -1;
                    }
                }
                if (CanFocus)
                {
                    switch (key.Key)
                    {
                        case ConsoleKey.Tab:
                            prevPos = y / 2;
                            return -2;
                    }
                }
            } while (true);

        }

        public int ShowVertical(int separatepx, bool CanEscape, bool CanFocus, int setPos)
        {
            PrintMenuV(separatepx);
            y = setPos;
            do
            {
                DrawEntryV(y, separatepx, tlo, tekst);
                key = Console.ReadKey(true);
                DrawEntryV(y, separatepx, tekst, tlo);
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        y--;
                        if (y < 0)
                        {
                            y = entry.Count - 1;
                        }
                        while (entry[y] == "")
                        {
                            y--;
                        }
                        continue;
                    case ConsoleKey.RightArrow:
                        y++;
                        if (y >= entry.Count)
                        {
                            y = 0;
                        }
                        while (entry[y] == "")
                        {
                            y++;
                        }
                        continue;
                    case ConsoleKey.Enter:
                        return y;
                }

                if (CanEscape)
                {
                    switch (key.Key)
                    {
                        case ConsoleKey.Escape:
                            prevPos = y;
                            return -1;
                    }
                }
                if (CanFocus)
                {
                    switch (key.Key)
                    {
                        case ConsoleKey.Tab:
                            prevPos = y;
                            return -2;
                        case ConsoleKey.UpArrow:
                            prevPos = y;
                            return -2;
                    }
                }

            } while (true);

        }

        public void PrintMenuV(int odstep)
        {
            for (y = 0; y < entry.Count; y++)
            {
                DrawEntryV(y, odstep, tekst, tlo);
            }
        }

        public void PrintMenuH()
        {
            for (y = 0; y < entry.Count; y++)
            {
                DrawEntryH(y, tekst, tlo);
            }
        }

        public void DrawEntryH(int whichentry, ConsoleColor textcolor, ConsoleColor backcolor)
        {
            Console.ForegroundColor = textcolor;
            Console.BackgroundColor = backcolor;
            if (entry[whichentry] != "")
            {
                Text.WriteXY(menuXPos, menuYPos + whichentry, " " + entry[whichentry] + " ");
            }
        }

        private void DrawEntryV(int whichentry, int separatepx, ConsoleColor textcolor, ConsoleColor backcolor)
        {
            Console.ForegroundColor = textcolor;
            Console.BackgroundColor = backcolor;
            if (entry[whichentry] != "")
            {
                if (whichentry == 0) { Text.WriteXY(menuXPos, menuYPos, " " + entry[whichentry] + " "); }
                else
                {
                    int length = 0;
                    for (int i = whichentry; i > 0; i--)
                    {
                        if (i == 1) { length = length + entry[whichentry - 1].Length + separatepx + 2; }
                        else { length = length + entry[whichentry - 1].Length; }
                    }

                    Text.WriteXY(menuXPos + length, menuYPos, " " + entry[whichentry] + " ");
                }
            }
        }
    }
}
