using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sharpie
{
	class Menu
	{
		List<string> entry;
		int menuYPos;
		int y;
		ConsoleColor tekst, tlo;
		ConsoleKeyInfo key;

		public Menu(string[] pozycje, int wysokosc, ConsoleColor tekst, ConsoleColor tlo)
		{
			entry = new List<string>(pozycje);
			for (int i = 1; i < entry.Count; i = i + 2)
			{
				entry.Insert(i, "");
			}
			this.menuYPos = wysokosc;
			this.tekst = tekst;
			this.tlo = tlo;
		}

		public int Show()
		{
			for (y = 0; y < entry.Count; y++)
			{
				DrawEntry(y, tekst, tlo);
			}
			y = 0;
			do
			{
				DrawEntry(y, tlo, tekst);
				key = Console.ReadKey(true);
				DrawEntry(y, tekst, tlo);
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
						return y;
					case ConsoleKey.Escape:
						return -2;
				}
			} while (true);

		}

		public void DrawEntry(int whichentry, ConsoleColor textcolor, ConsoleColor backcolor)
		{
			Console.ForegroundColor = textcolor;
			Console.BackgroundColor = backcolor;
			if (entry[whichentry] != "") { Cursor.WriteXY(Console.WindowWidth / 2 - entry[whichentry].Length / 2, menuYPos + whichentry, " " + entry[whichentry] + " "); }
		}
	}
}
