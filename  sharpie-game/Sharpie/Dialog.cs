using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Sharpie
{
    class Dialog
    {
        ConsoleColor tekst = ConsoleColor.Black;
        ConsoleColor tlo = ConsoleColor.Gray;
        int x1, x2, y1, y2;

        public Dialog(ConsoleColor tekst, ConsoleColor tlo)
        {
            this.tekst = tekst;
            this.tlo = tlo;
        }

        public void Show(int x1, int y1, int x2, int y2)
        {
            DrawDialog(x1,y1,x2,y2);
        }

        public void Show(int x1, int y1, int x2, int y2, string title)
        {
            DrawDialog(x1, y1, x2, y2);
            Console.SetCursorPosition((x1 + x2) / 2 - title.Length / 2, y1);
            Console.Write(" {0} ", title);
        }

        public void Show(int x1, int y1, int x2, int y2, string title, string panelhelp)
        {
            DrawDialog(x1, y1, x2, y2);
            Console.SetCursorPosition((x1 + x2) / 2 - title.Length / 2, y1);
            Console.Write(" {0} ", title);
            Interface.WritePanelLeft(panelhelp);
        }

        public void Clear()
        {
            for (int x = x1; x <= x2; x++)
            {
                for (int y = y1; y <= y2; y++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(" ");
                }
            }
        }

        private void DrawDialog(int x1, int y1, int x2, int y2)
        {
            this.x1 = x1;
            this.x2 = x2;
            this.y1 = y1;
            this.y2 = y2;
            Console.BackgroundColor = tlo;
            Console.ForegroundColor = tekst;
            Console.SetCursorPosition(x1, y1);

            for (int x = x1; x <= x2; x++)
            {
                for (int y = y1; y <= y2; y++)
                {
                    Console.SetCursorPosition(x, y);
                    if ((y == y1) && (x == x1)) { Console.Write("╔"); } // góra-lewo
                    else if ((y == y1) && (x == x2)) { Console.Write("╗"); } // góra-prawo
                    else if ((y == y2) && (x == x1)) { Console.Write("╚"); } // dół-lewo
                    else if ((y == y2) && (x == x2)) { Console.Write("╝"); } // dół-prawo
                    else if (((y != y1) || (y != y2)) && ((x == x1) || (x == x2))) { Console.Write("║"); } // bok lewo/prawo
                    else if (((y == y1) || (y == y2)) && ((x != x1) || (x != x2))) { Console.Write("═"); } // bok góra/dół
                    else { Console.Write(" "); }
                }
            }
        }

		public void WriteOn(string text, int YPos)
		{
			int length = x2 - x1 - 4;
            ArrayList linie = new ArrayList();
            int index;

            Console.ForegroundColor = tekst;
            Console.BackgroundColor = tlo;
			if (text.Length <= length)
			{
				Text.WriteXY(x1 + 2, YPos, text);
			}
			else
			{
				char[] delimiters = new char[] { '.', ',', ':', ';', ' ' };
				int i = 0;
				do
				{
					if (text.Length >= length)
					{
						index = text.LastIndexOfAny(delimiters, length);
						linie.Add(text.Substring(0, index));
						text = text.Remove(0, index + 1);
					}
					else
					{
						linie.Add(text.Substring(0, text.Length));
						text = text.Remove(0);
					}
					Text.WriteXY(x1 + 2, YPos + i, linie[i].ToString());
					i++;
				} while (text != "");
			}

            Console.ResetColor();
		}
	}
}
