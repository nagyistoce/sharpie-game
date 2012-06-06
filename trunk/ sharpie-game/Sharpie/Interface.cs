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

        public void Glowny() //ekran główny
        {
            Console.ResetColor();
            Logo();
            Text.WriteXY(Text.CenterX(Locale.desc), 9, Locale.desc);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.ResetColor();
            Dialog menudialog = new Dialog(1, ConsoleColor.White, ConsoleColor.DarkBlue);
            Menu menu = new Menu(new string[] { "Nowa gra", "Ustawienia", "Wyniki", "O grze", "", "Aktualizuj" }, 29, 15, ConsoleColor.White, ConsoleColor.DarkBlue);
            menudialog.Show(27, 13, 43, 27, "Menu", "ESC - wyjście");

            int value = menu.ShowHorizontal(true, false);
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
                    Wyniki();
                    break;
                case 3: // o grze informacja
                    Ogrze();
                    break;
                case 4:
                    break;
            }
        }

        private void NewGame()
        {
            Menu newgame = new Menu(new string[] { "Bułka z masłem", "Średni", "Hardcore" }, 43, 20, ConsoleColor.White, ConsoleColor.DarkCyan);
            Dialog ngdial = new Dialog(1, ConsoleColor.White, ConsoleColor.DarkCyan);
            ngdial.Show(41, 15, 60, 26, "Nowa gra", "ESC - powrót ");
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.White;
            ngdial.WriteOn("Wybierz poziom trudności:", 17);
            int value = newgame.ShowHorizontal(true, false);
            switch (value)
            {
                case -1:
                    break;
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
            string nickname = Properties.Settings.Default.Nick;
            Dialog settings = new Dialog(1, ConsoleColor.White, ConsoleColor.DarkYellow);
            Menu menuv = new Menu(new string[] { "OK", "Anuluj", "Zastosuj" }, 22, 22, ConsoleColor.White, ConsoleColor.DarkYellow);
            string[] poz = { "Nick (" + nickname + ")"};
            Menu menuh = new Menu(poz, Cursor.CenterX() - 1 - poz[0].Length / 2, 17, ConsoleColor.White, ConsoleColor.DarkYellow);
            settings.Show(19, 14, 51, 24, "Ustawienia", "Enter - zatwierdź  Tab - zmień menu                    ");
            bool exit = false;
            bool exit2 = false;
            do
            {
                menuv.PrintMenuV(3);
                do
                {
                    int v = menuh.ShowHorizontal(true, true);
                    switch (v)
                    {
                        case 0:
                            string nickname2 = Nick();
                            if (nickname2 != "") { nickname = nickname2; }
                            settings.Redraw();
                            poz[0] = "Nick (" + nickname + ")";
                            menuh = new Menu(poz, Cursor.CenterX() - 1 - poz[0].Length / 2, 17, ConsoleColor.White, ConsoleColor.DarkYellow);
                            menuv.PrintMenuV(3);
                            break;
                        case -2:
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
                    int value = menuv.ShowVertical(3, true, true);
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
                            Properties.Settings.Default.Nick = nickname;
                            Properties.Settings.Default.Save();
                            continue;
                        case -1:
                            exit = true;
                            exit2 = true;
                            break;
                        case -2:
                            exit = true;
                            break;
                    }
                } while (!exit);
            } while (!exit2);
        }

        private string Nick()
        {
            Dialog nick = new Dialog(1, ConsoleColor.White, ConsoleColor.DarkGreen);
            Textbox nicktb = new Textbox(Cursor.CenterX() - 7, Cursor.CenterY() - 1, 14);
            nick.Show(Cursor.CenterX() - 11, Cursor.CenterY() - 4, Cursor.CenterX() + 11, Cursor.CenterY() + 2, "Nick", "Enter - zatwierdź  Puste pole anuluje zmiany");
            string nickname;
            nickname = nicktb.Show();
            return nickname;
        }

        private void Wyniki()
        {
            Dialog listawynikow = new Dialog(1, ConsoleColor.White, ConsoleColor.DarkGray);
            listawynikow.Show(Cursor.CenterX() - 15, Cursor.CenterY() - 7, Cursor.CenterX() + 14, Cursor.CenterY() + 7, "Wyniki", "ESC - powrót  Lewo, prawo - zmiana listy");
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
                Text.WriteXY(Cursor.CenterX() - poziomy[y].Length / 2 - 3, Cursor.CenterY() - 5, "   " + poziomy[y] + "   ");
                Console.ForegroundColor = ConsoleColor.White;
                if (score.GetCount(y) > 0)
                {

                    for (int i = 0; i < score.GetCount(y); i++)
                    {
                        int pos = i + 1;
                        Text.WriteXY(Cursor.CenterX() - 13, Cursor.CenterY() - 3 + i, pos.ToString() + ". " + score.GetScore(y, i).nick + " ...");
                        Text.WriteXY(Cursor.CenterX() + 13 - score.GetScore(y, i).score.ToString().Length - 4, Cursor.CenterY() - 3 + i, "... " + score.GetScore(y, i).score.ToString());
                    }
                }
                else
                {
                    string info = "Nie masz żadnych wyników";
                    string info2 = "na tym poziomie trudności!";
                    Console.ForegroundColor = ConsoleColor.Red;
                    Text.WriteXY(Cursor.CenterX() - info.Length / 2, Cursor.CenterY() - 1, info);
                    Text.WriteXY(Cursor.CenterX() - info2.Length / 2, Cursor.CenterY(), info2);
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

        private void Ogrze()
        {
            Dialog ogrze = new Dialog(1, ConsoleColor.White, ConsoleColor.DarkCyan);
            Menu menu = new Menu(new string[] { "OK" }, Cursor.CenterX() - 2, 33, ConsoleColor.White, ConsoleColor.DarkCyan);
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
            menu.ShowVertical(0, false, false);
            Console.ResetColor();

        }

        private bool Exit()
        {
            Menu exitmenu = new Menu(new string[] { "Tak, mama mnie wzywa", "Coś ty, żartowałem/am" }, Text.CenterX(Locale.exitquestion), Cursor.CenterY(), ConsoleColor.White, ConsoleColor.Red);
            Dialog dialog = new Dialog(1, ConsoleColor.White, ConsoleColor.Red);
            dialog.Show(Text.CenterX(Locale.exitquestion) - 2, Cursor.CenterY() - 2, Cursor.CenterX() + Locale.exitquestion.Length / 2 + 3, Cursor.CenterY() + 4, "Wyjść z gry?", "ESC - powrót ");
            int value = exitmenu.ShowHorizontal(true, false);
            dialog.Clear();
            if (value == 0) { return true; }
            else { return false; }
        }

    }
}
