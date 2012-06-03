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
			Cursor.WriteXY(Console.WindowWidth / 2 - Locale.desc.Length / 2, 9, Locale.desc);
			Console.BackgroundColor = ConsoleColor.Gray;
			Console.ForegroundColor = ConsoleColor.Black;
			Console.ResetColor();
			Dialog menudialog = new Dialog(ConsoleColor.White, ConsoleColor.DarkBlue);
			Menu menu = new Menu(new string[] { "Nowa gra", "Ustawienia", "Wyniki", "O grze", "", "Aktualizuj" }, 15, ConsoleColor.White, ConsoleColor.DarkBlue);
			menudialog.Show(20, 13, 50, 27, "Menu", "ESC - wyjście");
			int value = menu.Show();
			switch (value)
			{
				case -2:
					bool exit = Exit();
					if (exit == true)
					{
						Environment.Exit(1);
					}
					break;
				case 0:
					Game gra = new Game();
					break;
				case 2:
					break;
				case 4:
					break;
				case 6:
					Ogrze();
					ConsoleKeyInfo key;
					do
					{
						key = new ConsoleKeyInfo();
						key = Console.ReadKey(true);
					} while (key.Key != ConsoleKey.Escape);
					break;
				case 8:
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
			Cursor.WriteXY(Console.WindowWidth - 1 - text.Length, Console.WindowHeight - 1, text);
			Console.ResetColor();
		}

		private void Logo()
		{
			string a = "█████  █   █   ███   ████   ████   █  █████";
			string b = "█      █   █  █   █  █   █  █   █  █  █    ";
			string c = "█████  █████  █   █  ████   ████   █  ███  ";
			string d = "    █  █   █  █████  █   █  █      █  █    ";
			string e = "█████  █   █  █   █  █   █  █      █  █████";

			Cursor.WriteXY(Console.WindowWidth / 2 - a.Length / 2, 3, a);
			Cursor.WriteXY(Console.WindowWidth / 2 - b.Length / 2, 4, b);
			Cursor.WriteXY(Console.WindowWidth / 2 - c.Length / 2, 5, c);
			Cursor.WriteXY(Console.WindowWidth / 2 - d.Length / 2, 6, d);
			Cursor.WriteXY(Console.WindowWidth / 2 - e.Length / 2, 7, e);
		}

		private void Ogrze()
		{
			Dialog ogrze = new Dialog(ConsoleColor.White, ConsoleColor.DarkCyan);
			ogrze.Show(10, 12, 60, 27, "O grze", "ESC - powrót ");
			Console.ForegroundColor = ConsoleColor.White;
			Console.BackgroundColor = ConsoleColor.DarkCyan;
			Cursor.WriteXY(12,14, "Sharpie, wersja "+ Program.version);
			Cursor.WriteXY(12,16, "This program is free software:");
			Cursor.WriteXY(12,17, "you can redistribute it and/or modify");
			Console.ResetColor();

		}

		private bool Exit()
		{
			WritePanelLeft("                ");
			Menu exitmenu = new Menu(new string[] { "Tak, mama mnie wzywa", "Coś ty, żartowałem" }, Console.WindowHeight / 2, ConsoleColor.White, ConsoleColor.Red);
			Dialog dialog = new Dialog(ConsoleColor.White, ConsoleColor.Red);
			dialog.Show(Console.WindowWidth / 2 - Locale.exitquestion.Length / 2 - 3, Console.WindowHeight / 2 - 2, Console.WindowWidth / 2 + Locale.exitquestion.Length / 2 + 3, Console.WindowHeight / 2 + 4, "Wyjść z gry?");
			int value = exitmenu.Show();
			dialog.Clear();
			if (value == 0) { return true; }
			else { return false; }
		}

		

	}
}
