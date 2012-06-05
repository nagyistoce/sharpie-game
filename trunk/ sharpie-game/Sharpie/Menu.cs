using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sharpie
{
	class Menu
	{
		List<string> entry;
        int menuYPos, menuXPos;
		int y;
		ConsoleColor tekst, tlo;
		ConsoleKeyInfo key;

		public Menu(string[] pozycje, int x, int y, ConsoleColor tekst, ConsoleColor tlo)
		{
			entry = new List<string>(pozycje);
			for (int i = 1; i < entry.Count; i = i + 2)
			{
				entry.Insert(i, "");
			}
			this.menuYPos = y;
            this.menuXPos = x;
			this.tekst = tekst;
			this.tlo = tlo;
		}

		public int ShowHorizontal()
		{
			for (y = 0; y < entry.Count; y++)
			{
				DrawEntryH(y, tekst, tlo);
			}
			y = 0;
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
							y = 0;
						}
						while (entry[y] == "")
						{
							y++;
						}

						continue;
					case ConsoleKey.Enter:
						return y/2;
					case ConsoleKey.Escape:
						return -1;
				}
			} while (true);

		}

        public int ShowVertical()
        {
            for (y = 0; y < entry.Count; y++)
            {
                DrawEntryH(y, tekst, tlo);
            }
            y = 0;
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
                            y = 0;
                        }
                        while (entry[y] == "")
                        {
                            y++;
                        }

                        continue;
                    case ConsoleKey.Enter:
                        return y / 2;
                }
            } while (true);

        }

		public void DrawEntryH(int whichentry, ConsoleColor textcolor, ConsoleColor backcolor)
		{
			Console.ForegroundColor = textcolor;
			Console.BackgroundColor = backcolor;
			if (entry[whichentry] != "") { Text.WriteXY(menuXPos, menuYPos + whichentry, " " + entry[whichentry] + " "); }
		}

        private void DrawEntryV(int whichentry, ConsoleColor textcolor, ConsoleColor backcolor)
        {
            Console.ForegroundColor = textcolor;
            Console.BackgroundColor = backcolor;
            if (entry[whichentry] != "") { Text.WriteXY(menuXPos, menuYPos + whichentry, " " + entry[whichentry] + " "); }
        }
	}
}
