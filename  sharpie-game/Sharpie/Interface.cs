using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sharpie
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

		public void Glowny() //ekran główny
		{
			Console.ResetColor();
			Logo();
			Text.WriteXY(Text.CenterX(Locale.desc), 9, Locale.desc);
			Console.BackgroundColor = ConsoleColor.Gray;
			Console.ForegroundColor = ConsoleColor.Black;
			Console.ResetColor();
			Dialog menudialog = new Dialog(ConsoleColor.White, ConsoleColor.DarkBlue);
			Menu menu = new Menu(new string[] { "Nowa gra", "Ustawienia", "Wyniki", "O grze", "", "Aktualizuj" }, 29 ,15, ConsoleColor.White, ConsoleColor.DarkBlue);
			menudialog.Show(27, 13, 43, 27, "Menu", "ESC - wyjście");

			int value = menu.Show();
			switch (value)
			{
				case -1: // wyjście z gry
					bool exit = Exit();
					if (exit == true)
					{
						Environment.Exit(1);
					}
					break;
				case 0: // nowa gra
                    NewGame();
					break;
				case 1: // ustawienia ( nie mam na nie pomysłu na razie)
                    Ustawienia();
					break;
				case 2: // wyniki (zrobi się)
					break;
				case 3: // o grze informacja
					Ogrze();
					ConsoleKeyInfo key;
					do
					{
						key = new ConsoleKeyInfo();
						key = Console.ReadKey(true);
					} while (key.Key != ConsoleKey.Escape);
					break;
				case 4:
					break;
			}
		}

		public void Score(int score)
		{
			Console.BackgroundColor = ConsoleColor.Gray;
			Console.ForegroundColor = ConsoleColor.Black;
			WritePanelLeft("Wynik: " + score);
			Console.ResetColor();
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
			string a = "█████  █   █   ███   ████   ████   █  █████";
			string b = "█      █   █  █   █  █   █  █   █  █  █    ";
			string c = "█████  █████  █   █  ████   ████   █  ███  ";
			string d = "    █  █   █  █████  █   █  █      █  █    ";
			string e = "█████  █   █  █   █  █   █  █      █  █████";

			Text.WriteXY(Cursor.CenterX() - a.Length / 2, 3, a);
			Text.WriteXY(Cursor.CenterX() - b.Length / 2, 4, b);
			Text.WriteXY(Cursor.CenterX() - c.Length / 2, 5, c);
			Text.WriteXY(Cursor.CenterX() - d.Length / 2, 6, d);
			Text.WriteXY(Cursor.CenterX() - e.Length / 2, 7, e);
		}

        private void NewGame()
        {
            Menu newgame = new Menu(new string[] { "Bułka z masłem", "Średni", "Hardcore" },43, 20, ConsoleColor.White, ConsoleColor.DarkCyan);
            Dialog ngdial = new Dialog(ConsoleColor.White, ConsoleColor.DarkCyan);
            ngdial.Show(41, 15, 60, 26, "Nowa gra", "ESC - powrót ");
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.White;
            ngdial.WriteOn("Wybierz poziom trudności:", 17);
            int value = newgame.Show();
            switch (value)
            {
                case 0: // łatwy
                    Game graeasy = new Game(0, Properties.Settings.Default.Nick);
                    break;
                case 1: // średni
                    Game gramed = new Game(1, Properties.Settings.Default.Nick);
                    break;
                case 2: // trudny
                    Game grahard = new Game(2, Properties.Settings.Default.Nick);
                    break;
            }
        }

        private void Ustawienia()
        {
            Dialog settings = new Dialog(ConsoleColor.White, ConsoleColor.DarkYellow);
            settings.Show(10, 12, 60, 23, "Ustawienia", "ESC - powrót");
            settings.WriteOn("Nick: ", 14);
        }

		private void Ogrze()
		{
			Dialog ogrze = new Dialog(ConsoleColor.White, ConsoleColor.DarkCyan);
			ogrze.Show(14, 11, 56, 32, "O grze", "ESC - powrót ");
			Console.ForegroundColor = ConsoleColor.White;
			Console.BackgroundColor = ConsoleColor.DarkCyan;
            ogrze.WriteOn("Sharpie", 12);
            ogrze.WriteOn("Wersja " + Program.version, 13);
            ogrze.WriteOn("Licencja:", 15);
			ogrze.WriteOn("This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your opinion) any later version.", 16);
            ogrze.WriteOn("O autorze:", 24);
            ogrze.WriteOn("Wszelkie propozycje, prośby, zapytania proszę przesyłać na adres: adirucio96@gmail.com. Błędy zgłaszać na stronie projektu.", 25);
            ogrze.WriteOn("Dla A. :* (niech się dziewczyna cieszy :D)", 30);
			Console.ResetColor();

		}

		private bool Exit()
		{
			Menu exitmenu = new Menu(new string[] { "Tak, mama mnie wzywa", "Coś ty, żartowałem/am" }, Text.CenterX(Locale.exitquestion), Cursor.CenterY(), ConsoleColor.White, ConsoleColor.Red);
			Dialog dialog = new Dialog(ConsoleColor.White, ConsoleColor.Red);
            dialog.Show(Text.CenterX(Locale.exitquestion) - 2, Cursor.CenterY() - 2, Cursor.CenterX() + Locale.exitquestion.Length / 2 + 3, Cursor.CenterY() + 4, "Wyjść z gry?", "ESC - powrót ");
			int value = exitmenu.Show();
			dialog.Clear();
			if (value == 0) { return true; }
			else { return false; }
		}

		

	}
}
