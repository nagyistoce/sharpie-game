using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using Menus;

namespace MapEditor
{
	static class Interface
	{
		private static void Logo()
		{
			string a = "█████  ██   ██  █████";
			string b = "█      █ █ █ █  █    ";
			string c = "█████  █  █  █  ███  ";
			string d = "    █  █     █  █    ";
			string e = "█████  █     █  █████";

			Text.WriteXY(Crs.CenterX() - a.Length / 2, 3, a);
			Text.WriteXY(Crs.CenterX() - b.Length / 2, 4, b);
			Text.WriteXY(Crs.CenterX() - c.Length / 2, 5, c);
			Text.WriteXY(Crs.CenterX() - d.Length / 2, 6, d);
			Text.WriteXY(Crs.CenterX() - e.Length / 2, 7, e);
		}

		public static void Glowny() //ekran główny
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
			int value = menu.ShowHorizontal(true, false, menu.prevPos);
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
			Menu menu = new Menu(new string[] { "OK" }, Crs.CenterX() - 2, Crs.CenterY() + 8, ConsoleColor.Black, ConsoleColor.Gray);
			inst.Show(Crs.CenterX() - 22, Crs.CenterY() - 8, Crs.CenterX() + 22, Crs.CenterY() + 10, "Instrukcja", "Enter - wybór");
			inst.WriteOn("Sterowanie:", Crs.CenterY() - 7);
			inst.WriteOn("Kl. kierunkowe - poruszanie kursorem", Crs.CenterY() - 6);
			inst.WriteOn("Kl. 1-6 - stawia ściankę", Crs.CenterY() - 5);
			inst.WriteOn("Spacja - stawia powietrze", Crs.CenterY() - 4);
			inst.WriteOn("Backspace lub Delete - stawia próżnię", Crs.CenterY() - 3);
			inst.WriteOn("S - punkt startowy węża", Crs.CenterY() - 2);
            inst.WriteOn("CapsLock - blokuje element", Crs.CenterY() - 1);
			inst.WriteOn("ESC - menu", Crs.CenterY());
			inst.WriteOn("W próżni nie jest generowane jedzenie, najlepiej ją stawiać w polach otoczonych ścianą. Na nowej mapie wszędzie jest powietrze.", Crs.CenterY() + 2);
			menu.ShowVertical(0, true, false, 0);
		}

		private static void Oprogramie()
		{
			Dialog oprog = new Dialog(1, ConsoleColor.White, ConsoleColor.DarkCyan);
			Menu menu = new Menu(new string[] { "OK" }, Crs.CenterX() - 2, 33, ConsoleColor.White, ConsoleColor.DarkCyan);
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
			menu.ShowVertical(0, true, false, 0);
			Console.ResetColor();

		}

		private static bool Exit()
		{
			Menu exitmenu = new Menu(new string[] { "Tak", "Nie" }, Crs.CenterX() - 6, Crs.CenterY() + 2, ConsoleColor.White, ConsoleColor.Red);
			Dialog dialog = new Dialog(1, ConsoleColor.White, ConsoleColor.Red);
			dialog.Show(Crs.CenterX() - 11, Crs.CenterY() - 2, Crs.CenterX() + 11, Crs.CenterY() + 4, "Wyjście", "ESC - powrót ");
			dialog.WriteOn("Wyjść z programu?", Crs.CenterY());
			int value = exitmenu.ShowVertical(2, true, false, 0);
			dialog.Clear();
			if (value == 0) { return true; }
			else { return false; }
		}

	}
}
