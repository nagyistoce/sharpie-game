using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
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
        static int max_y = Console.WindowHeight - 2;
        string[,] board = new string[max_x, max_y];  // tablica do rozpoznywania obiektów na planszy, np. ścian.

        public int speed = 250;     // prędkość węża (poziom trudności zarazem)
        int kierunek = 0;           // kierunek na podst. liczby. Oznaczenie liczb odpowiadającym kierunkom na górze dokumentu:>
        int dl_snake = 5;
        private int scorepoint = 0;


        private void UstawDlugosc()
        {
            for (int i = 1; i < dl_snake; i++)
            {

            }
        }

        private void DrawBoard()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(0, 1);
            for (int i = 0; i < Console.WindowWidth; i++)   // Rysuje górę ramki
            {
                if (i == 0)
                {
                    board[i, 1] = "/";
                }
                else if (i == Console.WindowWidth - 1)
                {
                    board[i, 1] = @"\";
                }
                else
                {
                    board[i, 1] = "-";
                }
                Console.Write(board[i, 1]);
            }

            for (int i = 2; i < Console.WindowHeight - 2; i++)  // Rysuje środek ramki
            {
                for (int j = 0; j < Console.WindowWidth; j++)
                {
                    if (j == 0 | j == Console.WindowWidth - 1)
                    {
                        board[j, i] = "|";
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
                    board[i, 1] = @"\";
                }
                else if (i == Console.WindowWidth - 1)
                {
                    board[i, 1] = "/";
                }
                else
                {
                    board[i, 1] = "-";
                }
                Console.Write(board[i, 1]);
            }
        }

        private void Setsegment()
        {

        }

        private void Start()
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 - Locale.ready.Length / 2, Console.WindowHeight / 2 - 2);
            Console.Write(Locale.ready);
            
            LinkedList<int> snakeX = new LinkedList<int>();
            LinkedList<int> snakeY = new LinkedList<int>();
            Cursor crs = new Cursor();
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            Random meat = new Random();
            
            key = Console.ReadKey(true);
            Console.SetCursorPosition(Console.WindowWidth / 2 - Locale.ready.Length / 2, Console.WindowHeight / 2 - 2);
            for (int i = 0; i < Locale.ready.Length; i++)
            {
                Console.Write(" ");
            }
            Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);
            snakeX.AddFirst(Console.CursorLeft);
            snakeY.AddFirst(Console.CursorTop);
            Thread.Sleep(speed);
            Console.Write("O");
            Thread.Sleep(speed);
            switch (key.Key)        // pierwsze wyjście węża
            {
                case ConsoleKey.UpArrow:
                    kierunek = 0;
                    for (int i = 1; i < dl_snake; i++)
                    {
                        crs.SMove(0);
                        snakeX.AddFirst(Console.CursorLeft);
                        snakeY.AddFirst(Console.CursorTop);
                        Console.Write("O");
                        Thread.Sleep(speed);
                    }
                    break;
                case ConsoleKey.DownArrow:
                    kierunek = 2;
                    for (int i = 1; i < dl_snake; i++)
                    {
                        crs.SMove(2);
                        board[Console.CursorLeft, Console.CursorTop] = "O";
                        snakeX.AddFirst(Console.CursorLeft);
                        snakeY.AddFirst(Console.CursorTop);
                        Console.Write("O");
                        Thread.Sleep(speed);
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    kierunek = 3;
                    for (int i = 1; i < dl_snake; i++)
                    {
                        crs.SMove(3);
                        board[Console.CursorLeft, Console.CursorTop] = "O";
                        snakeX.AddFirst(Console.CursorLeft);
                        snakeY.AddFirst(Console.CursorTop);
                        Console.Write("O");
                        Thread.Sleep(speed);
                    }
                    break;
                case ConsoleKey.RightArrow:
                    kierunek = 1;
                    for (int i = 1; i < dl_snake; i++)
                    {
                        board[Console.CursorLeft, Console.CursorTop] = "O";
                        snakeX.AddFirst(Console.CursorLeft);
                        snakeY.AddFirst(Console.CursorTop);
                        Console.Write("O");
                        Thread.Sleep(speed);
                    }
                    break;
            }
            
            new Thread(ReadMove).Start();
            do
            {
                if (kierunek != 1) { crs.SMove(kierunek); }
                if (board[Console.CursorLeft, Console.CursorTop] != " ")
                {
                    if (board[Console.CursorLeft, Console.CursorTop] == "+")
                    {
                        board[Console.CursorLeft, Console.CursorTop] = " ";
                        dl_snake++;
                        scorepoint = scorepoint + 10;

                    }
                    else
                    {
                        while (snakeX.Count > 0)
                        {
                            Console.SetCursorPosition(snakeX.First.Value, snakeY.First.Value);
                            Console.Write("*");
                            snakeX.RemoveFirst();
                            Thread.Sleep(50);
                        }
                        break;
                    }
                }
                board[Console.CursorLeft, Console.CursorTop] = "O";
                snakeX.AddFirst(Console.CursorLeft);
                snakeY.AddFirst(Console.CursorTop);
                if (dl_snake < snakeX.Count)
                {
                    Console.SetCursorPosition(snakeX.Last.Value, snakeY.Last.Value);
                    board[Console.CursorLeft, Console.CursorTop] = " ";
                    Console.Write(" ");
                    snakeX.RemoveLast();
                    snakeY.RemoveLast();
                    Console.SetCursorPosition(snakeX.First.Value, snakeY.First.Value);
                }
                Console.Write("O");
                Thread.Sleep(speed);
            } while (true);
        }

        private void GameOver()
        { 
            Console.SetCursorPosition(Console.WindowWidth / 2 - Locale.over.Length / 2, Console.WindowHeight / 2 - 1);
            Console.Write(Locale.over);
            string wynik = Locale.score + scorepoint.ToString();
            Console.SetCursorPosition(Console.WindowWidth / 2 - wynik.Length , Console.WindowHeight / 2 + 1);
        }

        private void ReadMove()
        {
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
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
            } while (true);
        }


        public Game()
        {
            Interface gra = new Interface();
            gra.Draw();
            gra.Score();
            DrawBoard();
            Start();
            GameOver();
        }
    }
}
