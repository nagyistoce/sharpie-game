using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Net;
using System.Diagnostics;
using Sharpie.Properties;
using Utils;
namespace Sharpie
{
	class Interface
	{
		static string filename;
        static int menupos = 0;

		public static void Score(int score)
		{
			Console.BackgroundColor = ConsoleColor.Gray;
			Console.ForegroundColor = ConsoleColor.Black;
			Utils.Panel.WritePanelLeft("Wynik: " + score + "           ");
			Console.ResetColor();
		}

		private static void Logo()
		{
			string a = "█████  █   █   ███   ████   ████   █  █████";
			string b = "█      █   █  █   █  █   █  █   █  █  █    ";
			string c = "█████  █████  █   █  ████   ████   █  ███  ";
			string d = "    █  █   █  █████  █   █  █      █  █    ";
			string e = "█████  █   █  █   █  █   █  █      █  █████";

			Text.WriteXY(Crs.CenterX() - a.Length / 2, 3, a);
			Text.WriteXY(Crs.CenterX() - b.Length / 2, 4, b);
			Text.WriteXY(Crs.CenterX() - c.Length / 2, 5, c);
			Text.WriteXY(Crs.CenterX() - d.Length / 2, 6, d);
			Text.WriteXY(Crs.CenterX() - e.Length / 2, 7, e);
		}

        static Dialog menudialog;
		public static void Glowny() //ekran główny
		{
			Console.ResetColor();
			Logo();
			Text.WriteXY(Text.CenterX(Locale.desc), 9, Locale.desc);
			Console.ResetColor();

            if (!Update.Update.WasUpdateChecked)
            {
                if (Update.Update.IsInternet())
                {
                    Update.Update updt = new Update.Update(0, System.Windows.Forms.Application.ExecutablePath, Program.version);
                    updt.UpdateProcedure();
                }
                Update.Update.WasUpdateChecked = true;
            }
			menudialog = new Dialog(1, ConsoleColor.White, ConsoleColor.DarkBlue);
			Menu menu = new Menu(new string[] { "Nowa gra", "Ustawienia", "Wyniki", "Instrukcja", "O grze" }, 29, 15, ConsoleColor.White, ConsoleColor.DarkBlue);

			menudialog.Show(27, 13, 43, 25, "Menu", "ESC - wyjście   ");
			int value = menu.ShowHorizontal(true, false, menupos);
			switch (value)
			{
				case -1: // wyjście z gry
                    menupos = menu.prevPos;
					bool exit = Exit();
					if (exit == true)
					{
						Environment.Exit(1);
					}
					break;
				case 0: // nowa gra
                    menupos = value;
					string map = SelectMap();
					if (map == "") { break; }
					NewGame(map);
					break;
				case 1: // ustawienia
                    menupos = value;
					Ustawienia();
					break;
				case 2: // wyniki
                    menupos = value;
					Wyniki();
					break;
                case 3:
                    menupos = value;
                    Instrukcja();
                    break;
				case 4: // o grze informacja
                    menupos = value;
					Ogrze();
					break;
			}
		}

		private static string SelectMap()
		{
			Menu map = new Menu(new string[] { "Mapa 1", "Mapa 2", "Mapa 3", "Mapa 4", "", "Własna mapa ..."}, 43, 16, ConsoleColor.White, ConsoleColor.DarkCyan);
			Dialog mapdial = new Dialog(1, ConsoleColor.White, ConsoleColor.DarkCyan);
			mapdial.Show(41, 11, 61, 28, "Nowa gra", "ESC - powrót do menu");
			Console.BackgroundColor = ConsoleColor.DarkCyan;
			Console.ForegroundColor = ConsoleColor.White;
			mapdial.WriteOn("Wybierz mapę:", 13);
			int value = map.ShowHorizontal(true, false, 0);
			switch (value)
			{
				case -1:
					return "";
				case 0:
					return "1";
				case 1:
					return "2";
				case 2:
					return "3";
				case 3:
					return "4";
				case 5:
					return LoadMap();
			}
			return "";
		}

		private static string LoadMap()
		{
			string path = @"Maps\";
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			Thread thr = new Thread(new ThreadStart(OpenMapDialog));
			thr.SetApartmentState(ApartmentState.STA);
			thr.IsBackground = false;
			thr.Start();
			thr.Join();
			return filename;
		}

		private static void OpenMapDialog()
		{
			string path = @"Maps\";
			System.Windows.Forms.OpenFileDialog open = new System.Windows.Forms.OpenFileDialog();
			open.Title = "Otwórz mapę ...";
			open.Filter = "Sharpie Map Format (*.smf)|*.smf";
			open.DefaultExt = "smf";
			open.AutoUpgradeEnabled = true;
			open.AddExtension = true;
			open.InitialDirectory = Path.GetFullPath(path);
			System.Windows.Forms.DialogResult result = open.ShowDialog();
			if (result == System.Windows.Forms.DialogResult.Cancel)
			{
				filename = "";
			}
			else
			{
				filename = open.FileName;
			}
		}

		private static bool NewGame(string mapname)
		{
			Menu newgame = new Menu(new string[] { "Bułka z masłem", "Średni", "Hardcore" }, 49, 20, ConsoleColor.White, ConsoleColor.DarkGreen);
			Dialog ngdial = new Dialog(1, ConsoleColor.White, ConsoleColor.DarkGreen);
			ngdial.Show(47, 15, 66, 26, "Nowa gra");
			Console.BackgroundColor = ConsoleColor.DarkCyan;
			Console.ForegroundColor = ConsoleColor.White;
			ngdial.WriteOn("Wybierz poziom trudności:", 17);
			int value = newgame.ShowHorizontal(true, false, 1);
			switch (value)
			{
				case -1:
					return false;
				case 0: // łatwy
					Game graeasy = new Game(0, Properties.Settings.Default.Nick, mapname);
					break;
				case 1: // średni
					Game gramed = new Game(1, Properties.Settings.Default.Nick, mapname);
					break;
				case 2: // trudny
					Game grahard = new Game(2, Properties.Settings.Default.Nick, mapname);
					break;
			}
			return true;
		}

		private static void Ustawienia()
		{
			string nickname = Properties.Settings.Default.Nick;
			Dialog settings = new Dialog(1, ConsoleColor.White, ConsoleColor.DarkYellow);
			Menu menuv = new Menu(new string[] { "OK", "Anuluj", "Zastosuj" }, 22, 22, ConsoleColor.White, ConsoleColor.DarkYellow);
			string[] poz = { "Nick (" + nickname + ")", "Kolor węża" };
			Menu menuh = new Menu(poz, Crs.CenterX() - 1 - poz[1].Length / 2, 16, ConsoleColor.White, ConsoleColor.DarkYellow);
			settings.Show(19, 14, 51, 24, "Ustawienia", "Enter - zatwierdź  Tab - zmień menu                    ");
			bool exit = false;
			bool exit2 = false;
            int menuposh = 0;
            int menuposv = 0;
			do
			{
				menuv.PrintMenuV(3);
				do
				{
					int v = menuh.ShowHorizontal(true, true, menuposh);
					switch (v)
					{
						case 0:
                            menuposh = v;
							string nickname2 = Nick();
							if (nickname2 != "") { nickname = nickname2; }
							settings.Redraw();
							menuh.entry[0] = "Nick (" + nickname + ")";
							menuv.PrintMenuV(3);
							continue;
                        case 1:
                            menuposh = v;
                            SnakeColor();
                            menudialog.Redraw();
                            settings.Redraw();
                            menuv.PrintMenuV(3);
                            continue;
						case -2:
                            menuposh = menuh.prevPos;
							exit = true;
							break;
						case -1:
							exit = true;
							exit2 = true;
							break;
					}
				} while (!exit);

				if (exit2) { break; }

				menuh.PrintMenuH();
				do
				{
					int value = menuv.ShowVertical(3, true, true, menuposv);
					switch (value)
					{
						case 0:
							Properties.Settings.Default.Nick = nickname;
							Properties.Settings.Default.Save();
							exit = true;
							exit2 = true;
							continue;
						case 1:
							exit = true;
							exit2 = true;
							continue;
						case 2:
                            menuposv = value;
                            exit = true;
							Properties.Settings.Default.Nick = nickname;
							Properties.Settings.Default.Save();
                            break;
						case -1:
							exit = true;
							exit2 = true;
							break;
						case -2:
                            menuposv = menuv.prevPos;
							exit = true;
							break;
					}
				} while (!exit);
			} while (!exit2);
		}

		private static string Nick()
		{
			Dialog nick = new Dialog(1, ConsoleColor.White, ConsoleColor.DarkGreen);
			Textbox nicktb = new Textbox(Crs.CenterX() - 7, Crs.CenterY() - 1, 14);
			nick.Show(Crs.CenterX() - 11, Crs.CenterY() - 4, Crs.CenterX() + 11, Crs.CenterY() + 2, "Nick", "Enter - zatwierdź  Puste pole anuluje zmiany");
			string nickname;
			nickname = nicktb.Show();
			return nickname;
		}

        private static void SnakeColor()
        {
            Dialog snake = new Dialog(1, ConsoleColor.White, ConsoleColor.DarkGray);
            string[] poz = new string[8];
            for (int i = 0; i < 8; i++)
            {
                poz[i] = "OOOOOO";
            }
            ConsoleColor[] kolory = new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.Cyan, ConsoleColor.Gray, ConsoleColor.Green, ConsoleColor.Magenta, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Yellow };
            ColorMenu menu = new ColorMenu(poz, kolory, Crs.CenterX() - poz[0].Length / 2, 14, ConsoleColor.Black, ConsoleColor.DarkGray);
            snake.Show(Crs.CenterX() - 13, 12, Crs.CenterX() + 13, 30, "Kolor węża", "Enter - wybór  ESC - anuluj           ");
            int colormenupos = 0;
            for (int i = 0; i < kolory.Length; i++)
            {
                if (kolory[i] == Settings.Default.SnakeColor)
                {
                    colormenupos = i;
                }
            }
            int value = menu.ShowHorizontal(true, false, colormenupos);
            if (value != -1)
            {
                Settings.Default.SnakeColor = kolory[value];
            }
            snake.Clear();
        }

		private static void Wyniki()
		{
			Dialog listawynikow = new Dialog(1, ConsoleColor.White, ConsoleColor.DarkGray);
			listawynikow.Show(Crs.CenterX() - 15, Crs.CenterY() - 7, Crs.CenterX() + 14, Crs.CenterY() + 7, "Wyniki", "ESC - powrót  Lewo, prawo - zmiana listy");
			string[] poziomy = { "Bułka z masłem ►", "◄ Średni ►", "◄ Hardcore" };
			int y;
			Score score = new Score();
			ConsoleKeyInfo key;
			y = 0;
			bool exit = false;
			do
			{
				Console.BackgroundColor = ConsoleColor.DarkGray;
				Console.ForegroundColor = ConsoleColor.Gray;
				Text.WriteXY(Crs.CenterX() - poziomy[y].Length / 2 - 3, Crs.CenterY() - 5, "   " + poziomy[y] + "   ");
				Console.ForegroundColor = ConsoleColor.White;
				if (score.GetCount(y) > 0)
				{

					for (int i = 0; i < score.GetCount(y); i++)
					{
						int pos = i + 1;
						Text.WriteXY(Crs.CenterX() - 13, Crs.CenterY() - 3 + i, pos.ToString() + ". " + score.GetScore(y, i).nick + " (" + score.GetScore(y, i).map + ")");
						Text.WriteXY(Crs.CenterX() + 13 - score.GetScore(y, i).score.ToString().Length - 4, Crs.CenterY() - 3 + i, "... " + score.GetScore(y, i).score.ToString());
					}
				}
				else
				{
					string info = "Nie masz żadnych wyników";
					string info2 = "na tym poziomie trudności!";
					Console.ForegroundColor = ConsoleColor.Red;
					Text.WriteXY(Crs.CenterX() - info.Length / 2, Crs.CenterY() - 1, info);
					Text.WriteXY(Crs.CenterX() - info2.Length / 2, Crs.CenterY(), info2);
					Console.ForegroundColor = ConsoleColor.White;
				}
				key = Console.ReadKey(true);
				switch (key.Key)
				{
					case ConsoleKey.LeftArrow:
						if (y != 0)
						{
							y--;
							listawynikow.Redraw();
						}
						continue;
					case ConsoleKey.RightArrow:
						if (y != 2)
						{
							y++;
							listawynikow.Redraw();
						}
						continue;
					case ConsoleKey.Escape:
						exit = true;
						break;
				}
			} while (!exit);
		}

        public static void Instrukcja()
        {
            Dialog inst = new Dialog(1, ConsoleColor.Black, ConsoleColor.Gray);
            Menu menu = new Menu(new string[] { "OK" }, Crs.CenterX() - 2, Crs.CenterY() + 8, ConsoleColor.Black, ConsoleColor.Gray);
            inst.Show(Crs.CenterX() - 19, Crs.CenterY() - 8, Crs.CenterX() + 19, Crs.CenterY() + 10, "Instrukcja", "Enter - wybór");
            inst.WriteOn("Sterowanie:", Crs.CenterY() - 6);
            inst.WriteOn("Kl. kierunkowe - kierunek węża", Crs.CenterY() - 4);
            inst.WriteOn("ESC - menu pauzy", Crs.CenterY() - 3);
            inst.WriteOn("Twoim zadaniem jest jeść jedzenie, aby rosnąć i zbierać punkty uważając przy tym, żeby nie zderzyć się ze ścianą lub samym sobą.", Crs.CenterY());
            menu.ShowVertical(0, true, false, 0);
        }

		private static void Ogrze()
		{
			Dialog ogrze = new Dialog(1, ConsoleColor.White, ConsoleColor.DarkCyan);
			Menu menu = new Menu(new string[] { "OK" }, Crs.CenterX() - 2, 33, ConsoleColor.White, ConsoleColor.DarkCyan);
			ogrze.Show(14, 11, 56, 35, "O grze", "Enter - wybierz ");
			Console.ForegroundColor = ConsoleColor.White;
			Console.BackgroundColor = ConsoleColor.DarkCyan;
			ogrze.WriteOn("Sharpie", 12);
			ogrze.WriteOn("Wersja " + Program.version, 13);
			ogrze.WriteOn("Licencja:", 15);
			ogrze.WriteOn("This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your opinion) any later version.", 16);
			ogrze.WriteOn("O autorze:", 24);
			ogrze.WriteOn("Wszelkie propozycje, prośby, zapytania proszę przesyłać na adres: adirucio96@gmail.com. Błędy zgłaszać na stronie projektu.", 25);
			ogrze.WriteOn("Dla A. :* (niech się moja dziewczyna cieszy :D)", 30);
			menu.ShowVertical(0, false, false, 0);
			Console.ResetColor();

		}

		private static bool Exit()
		{
			Menu exitmenu = new Menu(new string[] { "Tak, mama mnie wzywa", "Coś ty, żartowałem/am" }, Text.CenterX(Locale.exitquestion), Crs.CenterY(), ConsoleColor.White, ConsoleColor.Red);
			Dialog dialog = new Dialog(1, ConsoleColor.White, ConsoleColor.Red);
			dialog.Show(Text.CenterX(Locale.exitquestion) - 2, Crs.CenterY() - 2, Crs.CenterX() + Locale.exitquestion.Length / 2 + 3, Crs.CenterY() + 4, "Wyjść z gry?", "ESC - powrót ");
			int value = exitmenu.ShowHorizontal(true, false, 0);
			dialog.Clear();
			if (value == 0) { return true; }
			else { return false; }
		}

	}
}
