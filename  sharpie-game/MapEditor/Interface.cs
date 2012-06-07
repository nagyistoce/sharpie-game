using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sharpie;

namespace MapEditor
{
    class Interface
    {
        public void Draw()
        {
            Console.ResetColor();
            Console.Clear();
            Console.SetBufferSize(Console.WindowWidth + 1, Console.WindowHeight + 1);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            for (int i = 1; i <= Console.WindowWidth; i++)
            {
                Console.Write(" ");
            }
            Console.SetCursorPosition(0, 0);
            WritePanelRight("v" + Program.version);    //Wersja programu
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
        }

        public static void WritePanelLeft(string text)
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(1, Console.WindowHeight - 1);
            Console.Write(text);
            Console.ResetColor();
        }

        public static void WritePanelRight(string text)
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Text.WriteXY(Console.WindowWidth - 1 - text.Length, Console.WindowHeight - 1, text);
            Console.ResetColor();
        }

        private void Logo()
        {
            string a = "█████  ██   ██  █████";
            string b = "█      █ █ █ █  █    ";
            string c = "█████  █  █  █  ███  ";
            string d = "    █  █     █  █    ";
            string e = "█████  █     █  █████";

            Text.WriteXY(Cursor.CenterX() - a.Length / 2, 3, a);
            Text.WriteXY(Cursor.CenterX() - b.Length / 2, 4, b);
            Text.WriteXY(Cursor.CenterX() - c.Length / 2, 5, c);
            Text.WriteXY(Cursor.CenterX() - d.Length / 2, 6, d);
            Text.WriteXY(Cursor.CenterX() - e.Length / 2, 7, e);
        }

        public void Glowny() //ekran główny
        {
            Console.ResetColor();
            Logo();
            Text.WriteXY(Text.CenterX(Locale.desc), 9, Locale.desc);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.ResetColor();
            Dialog menudialog = new Dialog(1, ConsoleColor.White, ConsoleColor.DarkBlue);
            Menu menu = new Menu(new string[] { "Nowa mapa", "Ładuj mapę", "O programie", "", "Aktualizuj" }, 29, 15, ConsoleColor.White, ConsoleColor.DarkBlue);
            menudialog.Show(27, 13, 43, 25, "Menu", "ESC - wyjście");

            int value = menu.ShowHorizontal(true, false);
            switch (value)
            {
                case -1: // wyjście z programu
                    bool exit = Exit();
                    if (exit == true)
                    {
                        Environment.Exit(1);
                    }
                    break;
                case 0: // nowa mapa
                    Editor edit = new Editor();
                    break;
                case 1: // ładuj mapę
                    break;
                case 2: // O programie
                    Oprogramie();
                    break;
                case 4:
                    break;
            }
        }

        private void Oprogramie()
        {
            Dialog oprog = new Dialog(1, ConsoleColor.White, ConsoleColor.DarkCyan);
            Menu menu = new Menu(new string[] { "OK" }, Cursor.CenterX() - 2, 33, ConsoleColor.White, ConsoleColor.DarkCyan);
            oprog.Show(14, 11, 56, 35, "O programie", "Enter - wybierz ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            oprog.WriteOn("Sharpie Map Editor", 12);
            oprog.WriteOn("Wersja " + Program.version, 13);
            oprog.WriteOn("Licencja:", 15);
            oprog.WriteOn("This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your opinion) any later version.", 16);
            oprog.WriteOn("O autorze:", 24);
            oprog.WriteOn("Wszelkie propozycje, prośby, zapytania proszę przesyłać na adres: adirucio96@gmail.com. Błędy zgłaszać na stronie projektu.", 25);
            oprog.WriteOn("Dla A. :* (niech się moja dziewczyna cieszy :D)", 30);
            menu.ShowVertical(0, false, false);
            Console.ResetColor();

        }

        private bool Exit()
        {
            Menu exitmenu = new Menu(new string[] { "Tak", "Nie" }, Cursor.CenterX() - 6, Cursor.CenterY() + 2, ConsoleColor.White, ConsoleColor.Red);
            Dialog dialog = new Dialog(1, ConsoleColor.White, ConsoleColor.Red);
            dialog.Show(Cursor.CenterX() - 11, Cursor.CenterY() - 2, Cursor.CenterX() + 11, Cursor.CenterY() + 4, "Wyjście", "ESC - powrót ");
            dialog.WriteOn("Wyjść z programu?", Cursor.CenterY());
            int value = exitmenu.ShowVertical(2, true, false);
            dialog.Clear();
            if (value == 0) { return true; }
            else { return false; }
        }

    }
}
