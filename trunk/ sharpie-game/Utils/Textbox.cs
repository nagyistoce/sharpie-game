using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public class Textbox
    {
        int x, y, length;
        string text = "";

        public Textbox(int x, int y, int length)
        {
            this.x = x;
            this.y = y;
            this.length = length;
        }

        public void PrintTextbox()
        {
            Dialog ramka = new Dialog(0, Console.ForegroundColor, Console.BackgroundColor);
            ramka.Show(x - 1, y - 1, x + length + 1, y + 1);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            
            for (int i = 0; i <= length; i++)
            {
                Text.WriteXY(x + i, y, " ");
            }
        }

        public string Show()
        {
            Console.CursorVisible = true;
            PrintTextbox();
            Console.SetCursorPosition(x, y);
            text = Console.ReadLine();
            Console.CursorVisible = false;
            return text;
        }
    }
}
