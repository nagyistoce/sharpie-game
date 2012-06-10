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
			Menu menu = new Menu(new string[] { "Nowa mapa", "Ładuj mapę", "Instrukcja", "O programie", "", "Aktualizuj" }, 29, 15, ConsoleColor.White, ConsoleColor.DarkBlue);
			menudialog.Show(27, 13, 43, 27, "Menu", "ESC - wyjście");
			Editor edit;
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
					edit = new Editor(false);
					break;
				case 1:
					edit = new Editor(true);
					break;
				case 2: // ładuj mapę
					Instrukcja();
					break;
				case 3: // O programie
					Oprogramie();
					break;
				case 5:
					break;
			}
		}

		public static void Instrukcja()
		{
			Dialog inst = new Dialog(1, ConsoleColor.Black, ConsoleColor.Gray);
			Menu menu = new Menu(new string[] { "OK" }, Cursor.CenterX() - 2, Cursor.CenterY() + 8, ConsoleColor.Black, ConsoleColor.Gray);
			inst.Show(Cursor.CenterX() - 22, Cursor.CenterY() - 8, Cursor.CenterX() + 22, Cursor.CenterY() + 10, "Instrukcja", "Enter - wybór");
			inst.WriteOn("Sterowanie:", Cursor.CenterY() - 7);
			inst.WriteOn("Kl. kierunkowe - poruszanie kursorem", Cursor.CenterY() - 6);
			inst.WriteOn("Kl. 1-6 - stawia ściankę", Cursor.CenterY() - 5);
			inst.WriteOn("Spacja - stawia powietrze", Cursor.CenterY() - 4);
			inst.WriteOn("Backspace lub Delete - stawia próżnię", Cursor.CenterY() - 3);
			inst.WriteOn("S - punkt startowy węża", Cursor.CenterY() - 2);
            inst.WriteOn("CapsLock - blokuje element", Cursor.CenterY() - 1);
			inst.WriteOn("ESC - menu", Cursor.CenterY());
			inst.WriteOn("W próżni nie jest generowane jedzenie, najlepiej ją stawiać w polach otoczonych ścianą. Na nowej mapie wszędzie jest powietrze.", Cursor.CenterY() + 2);
			menu.ShowVertical(0, true, false);
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
			menu.ShowVertical(0, true, false);
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
