using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Reflection;


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

        public int speed = 200;  // prędkość węża (poziom trudności zarazem)
        int kierunek = 0;           // kierunek na podst. liczby. Oznaczenie liczb odpowiadającym kierunkom na górze dokumentu :>
        int poprzkierunek;
        int dl_snake = 5;
        private int scorepoint = 0;
        private int meatX, meatY;
        LinkedList<Point> snake = new LinkedList<Point>();
        string body = "O";
        Point startpoint = new Point(Cursor.CenterX(), Cursor.CenterY());
        List<string> wall = new List<string> { "╔", "╗", "═", "║", "╚", "╝" };
        Thread readmove;
        int difficulty;
        string nick;
        bool pause = false;
        string mapName;

        public Game(int difficulty, string nick, string mapname) // konstruktor
        {
            switch (difficulty)
            {
                case 0:
                    speed = 250;
                    break;
                case 1:
                    speed = 150;
                    break;
                case 2:
                    speed = 95;
                    break;
            }

            this.difficulty = difficulty;
            this.nick = nick;

            Interface.Draw();
            Interface.Score(scorepoint);
            LoadMap(mapname);
            Start();
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


        private void LoadMap(string map)
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
                        if ((j == 0) && (i == Cursor.CenterY() - 2)) { board[j, i] = "╝"; }
                        else if ((j == 0) && (i == Cursor.CenterY() + 2)) { board[j, i] = "╗"; }
                        else if ((j == Console.WindowWidth - 1) && (i == Cursor.CenterY() + 2)) { board[j, i] = "╔"; }
                        else if ((j == Console.WindowWidth - 1) && (i == Cursor.CenterY() - 2)) { board[j, i] = "╚"; }
                        else if ((i == Cursor.CenterY() - 1) || (i == Cursor.CenterY()) || (i == Cursor.CenterY() + 1)) { board[j, i] = " "; }
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
            dialog.Show(Text.CenterX(Locale.ready) - 2, Cursor.CenterY() - 4, Cursor.CenterX() + Locale.ready.Length / 2 + 1, Cursor.CenterY());
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.ForegroundColor = ConsoleColor.White;
            Text.WriteXY(Text.CenterX(Locale.ready), Cursor.CenterY() - 2, Locale.ready);
            Console.ResetColor();
            ConsoleKeyInfo key = Console.ReadKey(true);
            RegenBoard(Text.CenterX(Locale.ready) - 2, Cursor.CenterY() - 4, Cursor.CenterX() + Locale.ready.Length / 2 + 1, Cursor.CenterY());
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

            readmove = new Thread(ReadMove); // startuje wątek odpowiedzialny za odczytywanie ruchów
            readmove.Start();

            GenerateMunch();
            Console.SetCursorPosition(startpoint.x, startpoint.y);
            do
            {
                if (pause)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    for (int i = 0; i < max_x; i++)
                    {
                        Text.WriteXY(i, 0, " ");
                    }
                    Text.WriteXY(Cursor.CenterX() - Locale.pause.Length / 2, 0, Locale.pause);
                    while (pause) ;
                    Console.SetCursorPosition(0, 0);
                    Console.ResetColor();
                    RegenBoard(0, 0, max_x - 1, 0);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.SetCursorPosition(snake.First.Value.x, snake.First.Value.y);
                }
                Cursor.Move(kierunek); //rusza kursorem w wybranym kierunku

                if ((wall.Contains(board[Console.CursorLeft, Console.CursorTop])) || (board[Console.CursorLeft, Console.CursorTop] == body)) //zderzenie
                {
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

                snake.AddFirst(new Point(Console.CursorLeft, Console.CursorTop));    // dodawanie czlonu do listy
                if (snake.Count > 1)
                {
                    board[snake.First.Next.Value.x, snake.First.Next.Value.y] = body; // głowa nie liczy się do planszy
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(body);
                Console.ResetColor();

                if (dl_snake == snake.Count - 1)
                {
                    Console.SetCursorPosition(snake.Last.Value.x, snake.Last.Value.y);    // idzie na koniec weza
                    snake.RemoveLast();    //usuwa czlon z listy
                    board[Console.CursorLeft, Console.CursorTop] = " ";
                    Console.Write(" "); // i usuwa na ekranie

                }

                Console.SetCursorPosition(snake.First.Value.x, snake.First.Value.y);

                if (board[Console.CursorLeft, Console.CursorTop] == "#")    //event do jedzenia
                {
                    board[Console.CursorLeft, Console.CursorTop] = " ";
                    dl_snake++;

                    if (difficulty == 2) speed -= 1;
                    else speed -= 2;
                    
                    scorepoint = scorepoint + 10;
                    Interface.Score(scorepoint); ;
                    GenerateMunch();
                    Console.SetCursorPosition(snake.First.Value.x, snake.First.Value.y);
                }

                Thread.Sleep(speed); // prędkość węża ;>


            } while (true);
        }

        private void GameOver() // ekran końca gry
        {
            Dialog dialog = new Dialog(1, ConsoleColor.White, ConsoleColor.DarkYellow);
            string wynik = Locale.score + scorepoint.ToString();
            dialog.Show(Text.CenterX(wynik) - 5, Cursor.CenterY() - 4, Cursor.CenterX() + wynik.Length / 2 + 5, Cursor.CenterY() + 2);
            Text.WriteXY(Text.CenterX(Locale.over), Cursor.CenterY() - 2, Locale.over);
            Text.WriteXY(Text.CenterX(wynik), Cursor.CenterY(), wynik);
            Interface.WritePanelLeft("Czekaj chwilkę ...");
            readmove.Abort();
            Interface.WritePanelLeft("Naciśnij dowolny klawisz, aby wrócić do menu ...");
        }

        private void ReadMove() // czyta ruchy klawiszy w osobnym wątku
        {
            ConsoleKeyInfo key;
            do
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
                    case ConsoleKey.Spacebar:
                        if (!pause) { pause = true; }
                        else { pause = false; }
                        break;

                }
            } while (true);
        }

        private void GenerateMunch() // losuje i ustawia żarcie
        {
            Random meat = new Random();
            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            do
            {
                meatX = meat.Next(0, max_x);
                meatY = meat.Next(0, max_y);
            } while (board[meatX, meatY] != " ");
            board[meatX, meatY] = "#";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Text.WriteXY(meatX, meatY, "#");
            Console.SetCursorPosition(x, y);
            Console.ResetColor();
        }

        private void RegenBoard(int x1, int y1, int x2, int y2)
        {
            Console.SetCursorPosition(x1, y1);
            Console.ResetColor();
            for (int x = x1; x <= x2; x++)
            {
                for (int y = y1; y <= y2; y++)
                {
                    Console.SetCursorPosition(x, y);
                    if (board[x, y] == null) { Console.Write(" "); }
                    else { Console.Write(board[x, y]); }
                }
            }
        }
    }
}
