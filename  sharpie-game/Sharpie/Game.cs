using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


/*
 * OZNAKOWANIE KIERUNKÓW CHODU WĘŻA :)
 * /\ - kierunek = 0
 * >  - kierunek = 1   
 * \/ - kierunek = 2
 * <  - kierunek - 3  
 */


namespace SnakeSharp
{    
    class Game
    {

        static int max_x = Console.WindowWidth;
        static int max_y = Console.WindowHeight - 1;
        string[,] board = new string[max_x, max_y];  // tablica do rozpoznywania obiektów na planszy, np. ścian.

        public int speed = 250;  // prędkość węża (poziom trudności zarazem)
        int kierunek = 0;           // kierunek na podst. liczby. Oznaczenie liczb odpowiadającym kierunkom na górze dokumentu :>
        int poprzkierunek;
        int dl_snake = 5;
        private int scorepoint = 0;
        private int meatX, meatY;
        string[] wall = { "╔", "╗", "═", "║", "╚", "╝" };

        private void DrawBoard()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < Console.WindowWidth; i++)   // Rysuje górę ramki
            {
                if (i == 0)
                {
                    board[i, 1] = "╔";
                }
                else if (i == Console.WindowWidth - 1)
                {
                    board[i, 1] = "╗";
                }
                else
                {
                    board[i, 1] = "═";
                }
                Console.Write(board[i, 1]);
            }

            for (int i = 2; i < Console.WindowHeight - 1; i++)  // Rysuje środek ramki
            {
                for (int j = 0; j < Console.WindowWidth; j++)
                {
                    if (j == 0 | j == Console.WindowWidth - 1)
                    {
                        board[j, i] = "║";
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
                    board[i, 1] = "╚";
                }
                else if (i == Console.WindowWidth - 1)
                {
                    board[i, 1] = "╝";
                }
                else
                {
                    board[i, 1] = "═";
                }
                Console.Write(board[i, 1]);
            }
        }

        private void Start()
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 - Locale.ready.Length / 2, Console.WindowHeight / 2 - 2);
            Console.Write(Locale.ready);

            LinkedList<int> snakeX = new LinkedList<int>();
            LinkedList<int> snakeY = new LinkedList<int>();
            Cursor crs = new Cursor();

            Random meat = new Random();
            do
            {
                meatX = meat.Next(max_x);
                meatY = meat.Next(max_y);
            } while (board[meatX, meatY] != " ");
            board[meatX, meatY] = "#";
            Cursor.WriteXY(meatX, meatY, "#");

            ConsoleKeyInfo key = Console.ReadKey(true);

            switch (key.Key)
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

            new Thread(ReadMove).Start();
            Console.SetCursorPosition(Console.WindowWidth / 2 - Locale.ready.Length / 2, Console.WindowHeight / 2 - 2);
            for (int i = 0; i < Locale.ready.Length; i++)
            {
                Console.Write(" ");
            }
            Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);
            do
            {
                Cursor.Move(kierunek); //rusza kursorem w wybranym kierunku

                if ((board[Console.CursorLeft, Console.CursorTop] != " " && board[Console.CursorLeft, Console.CursorTop] != "#") || (snakeX.Find(Console.CursorLeft).Value == Console.CursorLeft && snakeY.Find(Console.CursorTop).Value == Console.CursorTop)) //zderzenie
                {
                    while (snakeX.Count > 0)
                    {
                        Console.SetCursorPosition(snakeX.First.Value, snakeY.First.Value);
                        Console.Write("*");
                        snakeX.RemoveFirst();
                        snakeY.RemoveFirst();
                        Thread.Sleep(50);
                    }
                    break;
                }

                snakeX.AddFirst(Console.CursorLeft);    // i dodawany czlon do listy
                snakeY.AddFirst(Console.CursorTop);
                Console.Write("O");

                if (dl_snake == snakeX.Count-1)
                {
                    Console.SetCursorPosition(snakeX.Last.Value, snakeY.Last.Value);    //potem idzie na koniec
                    snakeX.RemoveLast();    //usuwa czlon z listy
                    snakeY.RemoveLast(); //czysci pole w tablicy
                    Console.Write(" "); // i usuwa na ekranie

                }

                Console.SetCursorPosition(snakeX.First.Value, snakeY.First.Value);

                if (board[Console.CursorLeft, Console.CursorTop] == "#")    //event do jedzenia
                {
                    board[Console.CursorLeft, Console.CursorTop] = " ";
                    dl_snake++;
                    speed++;
                    scorepoint = scorepoint + 10;
                    UpdateScore();
                    do
                    {
                        meatX = meat.Next(0, max_x);
                        meatY = meat.Next(0, max_y);
                    } while (board[meatX, meatY] != " ");
                    board[meatX, meatY] = "#";
                    Console.SetCursorPosition(meatX, meatY);
                    Console.Write("#");
                    Console.SetCursorPosition(snakeX.First.Value, snakeY.First.Value);
                }

                Thread.Sleep(speed);

            } while (true);
        }

        private void GameOver()
        {
            Cursor.WriteXY(Console.WindowWidth / 2 - Locale.over.Length / 2, Console.WindowHeight / 2 - 2, Locale.over);
            string wynik = Locale.score + scorepoint.ToString();
            Interface.WritePanelLeft("                                           ");
            Cursor.WriteXY(Console.WindowWidth / 2 - wynik.Length / 2, Console.WindowHeight / 2, wynik);
        }

        private void ReadMove()
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
                }
            } while (true);
        }


        public Game()
        {
            Interface gra = new Interface();
            gra.Draw();
            gra.Score(scorepoint);
            DrawBoard();
            Start();
            GameOver();
        }

        private void GenerateMunch()
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
            Console.SetCursorPosition(meatX, meatY);
            Console.Write("#");
            Console.SetCursorPosition(x, y);
        }

        private void UpdateScore()
        {
            Interface score = new Interface();
            score.Score(scorepoint);
        }
    }
}
