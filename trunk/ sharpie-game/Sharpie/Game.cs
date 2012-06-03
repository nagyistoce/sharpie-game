using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;


/*
 * OZNAKOWANIE KIERUNKÓW CHODU WĘŻA :)
 * /\ - kierunek = 0
 * >  - kierunek = 1   
 * \/ - kierunek = 2
 * <  - kierunek - 3  
 */


namespace Sharpie
{
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
		string[] wall = { "╔", "╗", "═", "║", "╚", "╝" };
		LinkedList<int> snakeX = new LinkedList<int>();
		LinkedList<int> snakeY = new LinkedList<int>();
		string body = "O";
		Thread readmove;


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
			Dialog dialog = new Dialog(ConsoleColor.White, ConsoleColor.DarkMagenta);
			dialog.Show(Text.CenterX(Locale.ready) - 2, Cursor.CenterY() - 4, Cursor.CenterX() + Locale.ready.Length / 2 + 1, Cursor.CenterY());
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.ForegroundColor = ConsoleColor.White;
			Text.WriteXY(Text.CenterX(Locale.ready), Cursor.CenterY() - 2, Locale.ready);
			Console.ResetColor();
			ConsoleKeyInfo key = Console.ReadKey(true);
			dialog.Clear();
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

			Console.SetCursorPosition(Text.CenterX(Locale.ready), Cursor.CenterY() - 2);
			for (int i = 0; i < Locale.ready.Length; i++)
			{
				Console.Write(" ");
			}
			GenerateMunch();
			Console.SetCursorPosition(Cursor.CenterX(), Cursor.CenterY());
			do
			{
				Cursor.Move(kierunek); //rusza kursorem w wybranym kierunku

				if (Console.CursorTop != 1)	// jebane obejście błędu na y = 1, bo się nie wiem czemu sypie kurwa
				{
					if ((board[Console.CursorLeft, Console.CursorTop] != " " && board[Console.CursorLeft, Console.CursorTop] != "#") || (board[Console.CursorLeft, Console.CursorTop] == body)) //zderzenie
					{
						Console.ForegroundColor = ConsoleColor.Red;
						while (snakeX.Count > 0) // animacja znikania
						{
							Console.SetCursorPosition(snakeX.First.Value, snakeY.First.Value);
							Console.Write("*");
							snakeX.RemoveFirst();
							snakeY.RemoveFirst();
							Thread.Sleep(50);
						}
						Console.ResetColor();
						break; // wychodzi z pętli
					}
				}

				snakeX.AddFirst(Console.CursorLeft);    // dodawanie czlonu do listy
				snakeY.AddFirst(Console.CursorTop);
				if (snakeX.Count > 1)
				{
					board[snakeX.First.Next.Value, snakeY.First.Next.Value] = body; // głowa nie liczy się do planszy
				}
				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write(body);
				Console.ResetColor();

				if (dl_snake == snakeX.Count - 1)
				{
					Console.SetCursorPosition(snakeX.Last.Value, snakeY.Last.Value);    // idzie na koniec weza
					snakeX.RemoveLast();    //usuwa czlon z listy
					snakeY.RemoveLast(); //czysci pole w tablicy
					board[Console.CursorLeft, Console.CursorTop] = " ";
					Console.Write(" "); // i usuwa na ekranie

				}

				Console.SetCursorPosition(snakeX.First.Value, snakeY.First.Value);

				if (board[Console.CursorLeft, Console.CursorTop] == "#")    //event do jedzenia
				{
					board[Console.CursorLeft, Console.CursorTop] = " ";
					dl_snake++;
					speed--;
					scorepoint = scorepoint + 10;
					UpdateScore();
					GenerateMunch();
					Console.SetCursorPosition(snakeX.First.Value, snakeY.First.Value);
				}

				Thread.Sleep(speed); // prędkość węża ;>


			} while (true);
		}

		private void GameOver() // ekran końca gry
		{
			Dialog dialog = new Dialog(ConsoleColor.Gray, ConsoleColor.Black);
			string wynik = Locale.score + scorepoint.ToString();
            dialog.Show(Text.CenterX(wynik) - 5, Cursor.CenterY() - 4, Cursor.CenterX() + wynik.Length / 2 + 5, Cursor.CenterY() + 2);
            Text.WriteXY(Text.CenterX(Locale.over), Cursor.CenterY() - 2, Locale.over);
			Text.WriteXY(Text.CenterX(wynik), Cursor.CenterY(), wynik);
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
			readmove.Abort();
			GameOver();
			Console.ReadKey(true);
			GC.Collect(); // zbiera śmieci, bo troche tego tu jest
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
			Text.WriteXY(meatX, meatY,"#");
			Console.SetCursorPosition(x, y);
			Console.ResetColor();
		}

		private void UpdateScore() // aktualizuje wynik na dole
		{
			Interface score = new Interface();
			score.Score(scorepoint);
		}
	}
}
