using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Sharpie;

namespace MapEditor
{
    class Editor
    {
        static int max_x = Console.WindowWidth;
        static int max_y = Console.WindowHeight - 1;
        string[,] board = new string[max_x, max_y];
        List<string> wall = new List<string> { "║", "═", "╔", "╗", "╚", "╝" };
        ConsoleKeyInfo key;

        public Editor()
        {
            Interface editor = new Interface();
            editor.Draw();
            Start();
        }

        private void SetWall(int nrsciany)
        {
            board[Console.CursorLeft, Console.CursorTop] = wall[nrsciany];
            Konsola.Write(board[Console.CursorLeft, Console.CursorTop]);
        }

        private void SetAir()
        {
            board[Console.CursorLeft, Console.CursorTop] = " ";
            Konsola.Write(board[Console.CursorLeft, Console.CursorTop]);
        }

        private void SetVoid()
        {
            board[Console.CursorLeft, Console.CursorTop] = null;
            Konsola.Write(board[Console.CursorLeft, Console.CursorTop]);
        }

        private void Start()
        {
            Console.CursorVisible = true;
            Console.SetCursorPosition(Cursor.CenterX(), Cursor.CenterY());
            bool exit = false;
            do
            {
                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        Cursor.Move(0);
                        break;
                    case ConsoleKey.RightArrow:
                        Cursor.Move(1);
                        break;
                    case ConsoleKey.DownArrow:
                        Cursor.Move(2);
                        break;
                    case ConsoleKey.LeftArrow:
                        Cursor.Move(3);
                        break;
                    case ConsoleKey.Spacebar:
                        SetAir();
                        break;
                    case ConsoleKey.D1:
                        SetWall(0);
                        break;
                    case ConsoleKey.D2:
                        SetWall(1);
                        break;
                    case ConsoleKey.D3:
                        SetWall(2);
                        break;
                    case ConsoleKey.D4:
                        SetWall(3);
                        break;
                    case ConsoleKey.D5:
                        SetWall(4);
                        break;
                    case ConsoleKey.D6:
                        SetWall(5);
                        break;
                    case ConsoleKey.Backspace | ConsoleKey.Delete:
                        SetVoid();
                        break;
                    case ConsoleKey.Escape:
                        exit = PauseMenu();
                        break;
                }
            } while (!exit);
        }

        private bool PauseMenu()
        {
            Console.CursorVisible = false;
            Point curpos = new Point(Console.CursorLeft, Console.CursorTop);
            Dialog pause = new Dialog(0, ConsoleColor.White, ConsoleColor.DarkMagenta);
            bool exit = false;
            do
            {
                Menu menu = new Menu(new string[] { "Zapisz mapę", "", "Powrót do menu" }, Console.WindowWidth - 19, Console.WindowHeight - 8, ConsoleColor.White, ConsoleColor.DarkMagenta);
                pause.Show(Console.WindowWidth - 21, Console.WindowHeight - 10, Console.WindowWidth - 2, Console.WindowHeight - 3, "Menu", "ESC - powrót");
                int value = menu.ShowHorizontal(true, false);
                switch (value)
                {
                    case -1:
                        exit = true;
                        break;
                    case 0:
                        break;
                    case 2:
                        Menu exitmenu = new Menu(new string[] { "Tak", "Nie" }, Cursor.CenterX() - 6, Cursor.CenterY() + 2, ConsoleColor.White, ConsoleColor.Red);
                        Dialog dialog = new Dialog(1, ConsoleColor.White, ConsoleColor.Red);
                        dialog.Show(Cursor.CenterX() - 11, Cursor.CenterY() - 2, Cursor.CenterX() + 11, Cursor.CenterY() + 4, "Wyjście", "ESC - powrót ");
                        dialog.WriteOn("Wyjść do menu?", Cursor.CenterY());
                        int v = exitmenu.ShowVertical(2, true, false);
                        Console.ResetColor();
                        dialog.Clear();
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
            } while (!exit);

            Console.SetCursorPosition(curpos.x, curpos.y);
            Console.CursorVisible = true;
            return false;
        }
    }

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

    class Konsola
    {
        public static void Write(string text)
        {
            if (text == null) { text = "▒"; }
            Text.WriteXY(Console.CursorLeft, Console.CursorTop, text);
        }
    }
}
