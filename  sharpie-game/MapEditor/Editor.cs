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
        string[] lines = new string[max_y];
        Point startpoint = new Point(Cursor.CenterX(), Cursor.CenterY());
        List<string> wall = new List<string> { "║", "═", "╔", "╗", "╚", "╝" };
        ConsoleKeyInfo key, key2;

        public Editor(bool load)
        {
            Interface editor = new Interface();
            editor.Draw();
            if (load)
            {
                Map map = new Map();
                bool IsLoaded = map.LoadMap(out lines, out startpoint);
                if (IsLoaded) { PrepareMap(); }
                else { return; }
            }
            else
            {
                for (int x = 0; x < Console.WindowWidth; x++)
                {
                    for (int y = 0; y < Console.WindowHeight - 1; y++)
                    {
                        board[x, y] = " ";
                    }
                }
            }
            Start();
        }

        private void PrepareMap()
        {
            for (int y = 0; y < max_y; y++)
            {
                lines[y] = lines[y].Replace(",", "");
                for (int x = 0; x < max_x; x++)
                {
                    Console.SetCursorPosition(x, y);
                    board[x, y] = lines[y].ElementAt(x).ToString();
                    if (board[x, y] == "!") { board[x, y] = null; }
                    Block.PutObject(board[x, y]);
                }
            }
        }

        private void SetWall(int nrsciany)
        {
            board[Console.CursorLeft, Console.CursorTop] = wall[nrsciany];
            Block.PutObject(board[Console.CursorLeft, Console.CursorTop]);
        }

        private void SetAir()
        {
            if (board[Console.CursorLeft, Console.CursorTop] != "S")
            {
                board[Console.CursorLeft, Console.CursorTop] = " ";
                Block.PutObject(board[Console.CursorLeft, Console.CursorTop]);
            }
        }

        private void SetVoid()
        {
            board[Console.CursorLeft, Console.CursorTop] = null;
            Block.PutObject(board[Console.CursorLeft, Console.CursorTop]);
        }

        private void SetStartPoint(Point point)
        {
            startpoint = point;
            board[startpoint.x, startpoint.y] = "S";
            Text.WriteXY(startpoint.x, startpoint.y, "S");
        }

        private void ChangeStartPoint()
        {
            Point curpos = new Point(Console.CursorLeft, Console.CursorTop);
            Text.WriteXY(startpoint.x, startpoint.y, " ");
            board[startpoint.x, startpoint.y] = " ";
            startpoint = new Point(curpos.x, curpos.y);
            Console.SetCursorPosition(curpos.x, curpos.y);
            board[startpoint.x, startpoint.y] = "S";
            Block.PutObject("S");
        }

        private void Start()
        {
            Console.CursorVisible = true;
            SetStartPoint(startpoint);
            Console.SetCursorPosition(Cursor.CenterX(), Cursor.CenterY());
            bool exit = false;
            
            do
            {
                CursorCordPanel();
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
                    case ConsoleKey.Escape:
                        exit = PauseMenu();
                        break;
                    case ConsoleKey.S:
                        ChangeStartPoint();
                        break;
                }

                if (Console.CapsLock) { key = key2; }
                switch (key.Key)
                {
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
                    case ConsoleKey.Delete:
                        SetVoid();
                        break;
                    case ConsoleKey.Backspace:
                        SetVoid();
                        break;
                }
                if ((!Console.CapsLock) && (key.Key == ConsoleKey.Spacebar || key.Key == ConsoleKey.D1 || key.Key == ConsoleKey.D2 || key.Key == ConsoleKey.D3 || key.Key == ConsoleKey.D4 || key.Key == ConsoleKey.D5 || key.Key == ConsoleKey.D6 || key.Key == ConsoleKey.Delete || key.Key == ConsoleKey.Backspace))
                {
                    key2 = key;
                }

                if (board[startpoint.x, startpoint.y] != "S")
                {
                    Point curpos = new Point(Console.CursorLeft, Console.CursorTop);
                    Random rand = new Random();
                    while (board[Console.CursorLeft, Console.CursorTop] != " ")
                    {
                        Console.SetCursorPosition(rand.Next(max_x), rand.Next(max_y));
                    }
                    SetStartPoint(new Point(Console.CursorLeft, Console.CursorTop));
                    Console.SetCursorPosition(curpos.x, curpos.y);
                }
            } while (!exit);
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
                    Block.PutObject(board[x, y]);
                    if (startpoint.x == x & startpoint.y == y)
                    {
                        Block.PutObject("S");
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
            do
            {
                Menu menu = new Menu(new string[] { "Zapisz mapę", "Instrukcja", "", "Powrót do menu" }, Console.WindowWidth - 19, Console.WindowHeight - 10, ConsoleColor.White, ConsoleColor.DarkMagenta);
                pause.Show(Console.WindowWidth - 21, Console.WindowHeight - 12, Console.WindowWidth - 2, Console.WindowHeight - 3, "Menu", "ESC - powrót    ");
                int value = menu.ShowHorizontal(true, false);
                switch (value)
                {
                    case -1:
                        exit = true;
                        break;
                    case 0:
                        Map map = new Map();
                        map.SaveMap(board, startpoint);
                        break;
                    case 1:
                        Interface.Instrukcja();
                        RegenBoard(Cursor.CenterX() - 22, Cursor.CenterY() - 8, Cursor.CenterX() + 22, Cursor.CenterY() + 9);
                        break;
                    case 3:
                        Menu exitmenu = new Menu(new string[] { "Tak", "Nie" }, Cursor.CenterX() - 6, Cursor.CenterY() + 2, ConsoleColor.White, ConsoleColor.Red);
                        Dialog dialog = new Dialog(1, ConsoleColor.White, ConsoleColor.Red);
                        dialog.Show(Cursor.CenterX() - 11, Cursor.CenterY() - 2, Cursor.CenterX() + 11, Cursor.CenterY() + 4, "Wyjście", "ESC - powrót     ");
                        dialog.WriteOn("Wyjść do menu?", Cursor.CenterY());
                        int v = exitmenu.ShowVertical(2, true, false);
                        Console.ResetColor();
                        RegenBoard(Cursor.CenterX() - 11, Cursor.CenterY() - 2, Cursor.CenterX() + 11, Cursor.CenterY() + 4);
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

            RegenBoard(Console.WindowWidth - 21, Console.WindowHeight - 12, Console.WindowWidth - 2, Console.WindowHeight - 3);
            Console.SetCursorPosition(curpos.x, curpos.y);
            Console.CursorVisible = true;
            return false;
        }

        private void CursorCordPanel()
        {
            Point curpos = new Point(Console.CursorLeft, Console.CursorTop);
            Interface.WritePanelRight("  X: " + Console.CursorLeft + " Y: " + Console.CursorTop);
            Console.SetCursorPosition(curpos.x, curpos.y);
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

    class Block
    {
        public static void PutObject(string text)
        {
            if (text == null) { text = "▒"; }
            Point curpos = new Point(Console.CursorLeft, Console.CursorTop);
            Text.WriteXY(Console.CursorLeft, Console.CursorTop, text);
            Console.SetCursorPosition(curpos.x, curpos.y);
        }
    }
}
