using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Drawing;
using System.IO;
using System.Reflection;
using Sharpie.Properties;
using System.Diagnostics;
using Utils;


/*
 * OZNAKOWANIE KIERUNKÓW CHODU WĘŻA :)
 * /\ - kierunek = 0
 * >  - kierunek = 1   
 * \/ - kierunek = 2
 * <  - kierunek - 3  
 */


namespace Sharpie
{

    struct Point
    {
        public int x;
        public int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class Game
    {

        static int max_x = Console.WindowWidth;
        static int max_y = Console.WindowHeight - 1;
        string[,] board = new string[max_x, max_y];  // tablica do rozpoznywania obiektów na planszy, np. ścian.

        public int speed;  // prędkość węża (poziom trudności zarazem)
        int kierunek = 0;           // kierunek na podst. liczby. Oznaczenie liczb odpowiadającym kierunkom na górze dokumentu :>
        int poprzkierunek;
        int dl_snake = 5;
        private int scorepoint = 0;
        private Point meat;
        private Point bonus = new Point(0, 0); // byle jaki punkt
        LinkedList<Point> snake = new LinkedList<Point>();
        string body = "O";
        private Point startpoint = new Point(Crs.CenterX(), Crs.CenterY());
        List<string> wall = new List<string> { "╔", "╗", "═", "║", "╚", "╝" };
        Thread readmove, odliczajbonus;
        ManualResetEvent threadpauser = new ManualResetEvent(true);
        int difficulty;
        string nick;
        Random rand = new Random();
        bool pause = false;
        string mapName;
        bool exit = false;
        bool candirectionchange = true;
        ConsoleColor kolorweza = Settings.Default.SnakeColor;
        Stopwatch bonustime = new Stopwatch();

        public Game(int difficulty, string nick, string mapname) // konstruktor
        {
            switch (difficulty)
            {
                case 0:
                    speed = 250;
                    break;
                case 1:
                    speed = 170;
                    break;
                case 2:
                    speed = 100;
                    break;
            }

            this.difficulty = difficulty;
            this.nick = nick;

            Panel.Draw(Program.version);
            Interface.Score(scorepoint);
            LoadMap(mapname);
            Start();
            if (!exit)
            {
                GameOver();
                Console.ReadKey(true);
                if (mapname == "1" || mapname == "2" || mapname == "3" || mapname == "4")
                {
                    mapname = "Mapa " + mapname;
                }
                else
                {
                    mapname = Path.GetFileNameWithoutExtension(mapname);
                }
                Score scr = new Score(difficulty, scorepoint, nick, mapname);
            }

        }


        private void LoadMap(string map) // laduje mapę
        {
            StreamReader sr;
            if (map == "1" || map == "2" || map == "3" || map == "4")
            {
                sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Sharpie.Maps.Default" + map + ".smf"));
            }
            else
            {
                sr = File.OpenText(map);
            }
            string[] lines = new string[max_y];
            mapName = sr.ReadLine();
            string[] point = sr.ReadLine().Split(',');
            startpoint = new Point(Convert.ToInt32(point[0]), Convert.ToInt32(point[1]));
            sr.ReadLine();
            for (int i = 0; i < max_y; i++)
            {
                lines[i] = sr.ReadLine();
            }

            for (int y = 0; y < max_y; y++)
            {
                lines[y] = lines[y].Replace(",", "");
                for (int x = 0; x < max_x; x++)
                {
                    Console.SetCursorPosition(x, y);
                    board[x, y] = lines[y].ElementAt(x).ToString();
                    if (board[x, y] == "!")
                    {
                        board[x, y] = null;
                        Console.Write(" ");
                    }
                    else { Console.Write(board[x, y]); }
                }
            }
        }

        private void DrawBoard()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < Console.WindowWidth; i++)   // Rysuje górę ramki
            {
                if (i == 0)
                {
                    board[i, Console.CursorTop] = "╔";
                }
                else if (i == max_x - 1)
                {
                    board[i, Console.CursorTop] = "╗";
                }
                else
                {
                    board[i, Console.CursorTop] = "═";
                }
                Console.Write(board[i, Console.CursorTop]);
            }

            for (int i = 1; i < Console.WindowHeight - 1; i++)  // Rysuje środek ramki
            {
                for (int j = 0; j < Console.WindowWidth; j++)
                {
                    if (j == 0 | j == Console.WindowWidth - 1)
                    {
                        if ((j == 0) && (i == Crs.CenterY() - 2)) { board[j, i] = "╝"; }
                        else if ((j == 0) && (i == Crs.CenterY() + 2)) { board[j, i] = "╗"; }
                        else if ((j == Console.WindowWidth - 1) && (i == Crs.CenterY() + 2)) { board[j, i] = "╔"; }
                        else if ((j == Console.WindowWidth - 1) && (i == Crs.CenterY() - 2)) { board[j, i] = "╚"; }
                        else if ((i == Crs.CenterY() - 1) || (i == Crs.CenterY()) || (i == Crs.CenterY() + 1)) { board[j, i] = " "; }
                        else if (board[j, i] == null) { board[j, i] = "║"; }
                        else { board[j, i] = " "; }
                    }
                    else
                    {
                        board[j, i] = " ";
                    }
                    Console.Write(board[j, i]);
                }
            }

            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            for (int i = 0; i < Console.WindowWidth; i++)   // Rysuje dół ramki
            {
                if (i == 0)
                {
                    board[i, max_y - 1] = "╚";
                }
                else if (i == Console.WindowWidth - 1)
                {
                    board[i, max_y - 1] = "╝";
                }
                else
                {
                    board[i, max_y - 1] = "═";
                }
                Console.Write(board[i, max_y - 1]);
            }
        }

        private void Start()
        {
            Dialog dialog = new Dialog(1, ConsoleColor.White, ConsoleColor.DarkMagenta);
            Dialog pauza = new Dialog(0, ConsoleColor.White, ConsoleColor.Red);
            dialog.Show(Text.CenterX(Locale.ready) - 2, Crs.CenterY() - 4, Crs.CenterX() + Locale.ready.Length / 2 + 1, Crs.CenterY());
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.ForegroundColor = ConsoleColor.White;
            Text.WriteXY(Text.CenterX(Locale.ready), Crs.CenterY() - 2, Locale.ready);
            Console.ResetColor();
            ConsoleKeyInfo key = Console.ReadKey(true);
            Regen(Text.CenterX(Locale.ready) - 2, Crs.CenterY() - 4, Crs.CenterX() + Locale.ready.Length / 2 + 1, Crs.CenterY());
            switch (key.Key) // pierwszy ruch
            {
                case ConsoleKey.UpArrow:
                    kierunek = 0;
                    break;
                case ConsoleKey.RightArrow:
                    kierunek = 1;
                    break;
                case ConsoleKey.DownArrow:
                    kierunek = 2;
                    break;
                case ConsoleKey.LeftArrow:
                    kierunek = 3;
                    break;
            }

            odliczajbonus = new Thread(BonusTimer);
            odliczajbonus.Start();
            readmove = new Thread(ReadMove); // startuje wątek odpowiedzialny za odczytywanie ruchów
            readmove.Start();

            GenerateMunch();
            Console.SetCursorPosition(startpoint.x, startpoint.y);
            do
            {
                if (pause)
                {
                    bonustime.Stop();
                    threadpauser.Reset();
                    exit = PauseMenu();
                    if (exit)
                    {
                        Panel.WritePanelLeft("Czekaj chwilkę ...");
                        readmove.Abort();
                        break;
                    }
                    else
                    {
                        pause = false;
                        Console.SetCursorPosition(snake.First.Value.x, snake.First.Value.y);
                        threadpauser.Set();
                        bonustime.Start();
                    }
                }
                Crs.Move(kierunek); //rusza kursorem w wybranym kierunku
                if ((wall.Contains(board[Console.CursorLeft, Console.CursorTop])) || (board[Console.CursorLeft, Console.CursorTop] == body)) //zderzenie
                {
                    bonustime.Stop();
                    Console.ForegroundColor = ConsoleColor.Red;
                    while (snake.Count > 0) // animacja znikania
                    {
                        Console.SetCursorPosition(snake.First.Value.x, snake.First.Value.y);
                        Console.Write("*");
                        snake.RemoveFirst();
                        Thread.Sleep(50);
                    }
                    Console.ResetColor();
                    break; // wychodzi z pętli
                }

                if (board[Console.CursorLeft, Console.CursorTop] == "#")    //event do jedzenia
                {
                    board[Console.CursorLeft, Console.CursorTop] = " ";
                    Point curpos = new Point(Console.CursorLeft, Console.CursorTop);
                    dl_snake++;

                    if (speed < 90 & dl_snake % 2 == 0) { speed -= 1; }
                    else { speed -= 1; }

                    scorepoint = scorepoint + 10;
                    Interface.Score(scorepoint);
                    GenerateMunch();
                    Console.SetCursorPosition(curpos.x, curpos.y);
                }

                if (board[Console.CursorLeft, Console.CursorTop] == "♦")    //event do bonusu
                {
                    board[Console.CursorLeft, Console.CursorTop] = " ";
                    Point curpos = new Point(Console.CursorLeft, Console.CursorTop);
                    dl_snake -= 3;

                    speed += 3;

                    scorepoint = scorepoint + 30;
                    Interface.Score(scorepoint);
                    Console.SetCursorPosition(curpos.x, curpos.y);
                }

                snake.AddFirst(new Point(Console.CursorLeft, Console.CursorTop));    // dodawanie czlonu do listy
                board[snake.First.Value.x, snake.First.Value.y] = "H";
                if (snake.Count > 1)
                {
                    board[snake.First.Next.Value.x, snake.First.Next.Value.y] = body; // głowa nie liczy się do planszy
                }
                Console.ForegroundColor = kolorweza;
                Console.Write(body);
                Console.ResetColor();

                if (dl_snake <= snake.Count - 1)
                {
                    int ileusun;
                    if (dl_snake < snake.Count - 1) { ileusun = 2; }
                    else { ileusun = 1; }
                    for (int i = 0; i < ileusun; i++)
                    {
                        if (snake.Count > 2)
                        {
                            Console.SetCursorPosition(snake.Last.Value.x, snake.Last.Value.y);    // idzie na koniec weza
                            snake.RemoveLast();    //usuwa czlon z listy
                            board[Console.CursorLeft, Console.CursorTop] = " ";
                            Console.Write(" "); // i usuwa na ekranie
                        }
                    }
                }

                Console.SetCursorPosition(snake.First.Value.x, snake.First.Value.y);
                candirectionchange = true;
                Thread.Sleep(speed); // prędkość węża ;>


            } while (true);
        }

        void BonusTimer()
        {
            int kiedy;
            while (true)
            {
                kiedy = rand.Next(15, 35) * 1000;
                bonustime.Start();

                while (bonustime.ElapsedMilliseconds <= kiedy) ;
                bonustime.Reset();
                GenerateRandomBonus();

                switch (difficulty)
                {
                    case 0:
                        kiedy = rand.Next(10, 15) * 1000;
                        break;
                    case 1:
                        kiedy = rand.Next(8, 12) * 1000;
                        break;
                    case 2:
                        kiedy = rand.Next(5, 10) * 1000;
                        break;
                }

                bonustime.Start();

                while (bonustime.ElapsedMilliseconds <= kiedy) ;
                RemoveRandomBonus();
                bonustime.Reset();
            }
        }


        private void Regen(int x1, int y1, int x2, int y2)
        {
            Console.SetCursorPosition(x1, y1);
            Console.ResetColor();
            for (int x = x1; x <= x2; x++)
            {
                for (int y = y1; y <= y2; y++)
                {
                    if (board[x, y] == "#")
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Text.WriteXY(x, y, board[x, y]);
                    }
                    else if (board[x, y] == "♦")
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Text.WriteXY(x, y, board[x, y]);
                    }
                    else if (board[x, y] == body | board[x, y] == "H")
                    {
                        Console.ForegroundColor = kolorweza;
                        Text.WriteXY(x, y, body);
                    }
                    else if (board[x, y] == null)
                    {
                        Text.WriteXY(x, y, " ");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Text.WriteXY(x, y, board[x, y]);
                    }
                }
            }
        }

        private bool PauseMenu()
        {
            Console.CursorVisible = false;
            Point curpos = new Point(Console.CursorLeft, Console.CursorTop);
            Dialog pause = new Dialog(0, ConsoleColor.White, ConsoleColor.DarkMagenta);
            bool exit = false;
            int pos = 0;
            Menu menu = new Menu(new string[] { "O mapie (n.d.)", "Instrukcja", "", "Powrót do menu" }, Console.WindowWidth - 19, Console.WindowHeight - 10, ConsoleColor.White, ConsoleColor.DarkMagenta);
            pause.Show(Console.WindowWidth - 21, Console.WindowHeight - 12, Console.WindowWidth - 2, Console.WindowHeight - 3, "Menu (alpha)", "ESC - powrót do gry    ");
            do
            {
                int value = menu.ShowHorizontal(true, false, pos);
                switch (value)
                {
                    case -1:
                        exit = true;
                        break;
                    case 0:
                        pos = value;
                        break;
                    case 1:
                        pos = value;
                        Interface.Instrukcja();
                        Regen(Crs.CenterX() - 19, Crs.CenterY() - 8, Crs.CenterX() + 19, Crs.CenterY() + 10);
                        break;
                    case 3:
                        pos = value;
                        Menu exitmenu = new Menu(new string[] { "Tak", "Nie" }, Crs.CenterX() - 6, Crs.CenterY() + 2, ConsoleColor.White, ConsoleColor.Red);
                        Dialog dialog = new Dialog(1, ConsoleColor.White, ConsoleColor.Red);
                        dialog.Show(Crs.CenterX() - 11, Crs.CenterY() - 2, Crs.CenterX() + 11, Crs.CenterY() + 4, "Wyjście", "ESC - powrót         ");
                        dialog.WriteOn("Wyjść z gry?", Crs.CenterY());
                        int v = exitmenu.ShowVertical(2, true, false, 0);
                        Console.ResetColor();
                        Regen(Crs.CenterX() - 11, Crs.CenterY() - 2, Crs.CenterX() + 11, Crs.CenterY() + 4);
                        switch (v)
                        {
                            case 0:
                                exit = true;
                                return true;
                            case 2:
                                break;
                        }
                        break;
                }
                pause.Redraw();
            } while (!exit);
            Regen(Console.WindowWidth - 21, Console.WindowHeight - 12, Console.WindowWidth - 2, Console.WindowHeight - 3);
            Interface.Score(scorepoint);
            Console.SetCursorPosition(curpos.x, curpos.y);
            return false;
        }

        private void GameOver() // ekran końca gry
        {
            Dialog dialog = new Dialog(1, ConsoleColor.White, ConsoleColor.DarkYellow);
            string wynik = Locale.score + scorepoint.ToString();
            dialog.Show(Text.CenterX(wynik) - 5, Crs.CenterY() - 4, Crs.CenterX() + wynik.Length / 2 + 5, Crs.CenterY() + 2);
            Text.WriteXY(Text.CenterX(Locale.over), Crs.CenterY() - 2, Locale.over);
            Text.WriteXY(Text.CenterX(wynik), Crs.CenterY(), wynik);
            Panel.WritePanelLeft("Czekaj chwilkę ...");
            readmove.Abort();
            Panel.WritePanelLeft("Naciśnij dowolny klawisz, aby wrócić do menu ...");
        }

        private void ReadMove() // czyta ruchy klawiszy w osobnym wątku
        {
            ConsoleKeyInfo key;
            do
            {
                threadpauser.WaitOne();
                if (candirectionchange)
                {
                    key = Console.ReadKey(true);
                    poprzkierunek = kierunek;
                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (poprzkierunek != 2) kierunek = 0;
                            break;
                        case ConsoleKey.RightArrow:
                            if (poprzkierunek != 3) kierunek = 1;
                            break;
                        case ConsoleKey.DownArrow:
                            if (poprzkierunek != 0) kierunek = 2;
                            break;
                        case ConsoleKey.LeftArrow:
                            if (poprzkierunek != 1) kierunek = 3;
                            break;
                        case ConsoleKey.Escape:
                            pause = true;
                            break;

                    }
                }
                Thread.Sleep(speed / 5);
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }
            } while (true);
        }

        private void GenerateMunch() // losuje i ustawia żarcie
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            do
            {
                meat = new Point(rand.Next(0, max_x), rand.Next(0, max_y));
            } while (board[meat.x, meat.y] != " ");
            board[meat.x, meat.y] = "#";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Text.WriteXY(meat.x, meat.y, "#");
            Console.SetCursorPosition(x, y);
            Console.ResetColor();
        }

        private void GenerateRandomBonus()
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            do
            {
                bonus = new Point(rand.Next(0, max_x), rand.Next(0, max_y));
            } while (board[bonus.x, bonus.y] != " ");
            board[bonus.x, bonus.y] = "♦";
            Console.ForegroundColor = ConsoleColor.Yellow;
            Text.WriteXY(bonus.x, bonus.y, "♦");
            Console.SetCursorPosition(x, y);
            Console.ResetColor();
        }

        private void RemoveRandomBonus()
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            board[bonus.x, bonus.y] = " ";
            Text.WriteXY(bonus.x, bonus.y, " ");
            Console.SetCursorPosition(x, y);
        }
    }
}
