using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Sharpie;

namespace MapEditor
{
    class Map
    {
        public static string path = "";
        static string name = "";
        string[] lines = new string[Console.WindowHeight - 1];

        public Map(string mapname)
        {
            path = mapname + ".smp";
            name = mapname;
            
        }

        public bool SaveMap(string[,] maptable)
        {
            if (File.Exists(path))
            {
                if (FileExistsDialog())
                {
                    File.Delete(path);
                }
                else
                {
                    return false;
                }
            }

            for (int y = 0; y < Console.WindowHeight; y++)
            {
                for (int x = 0; x < Console.WindowWidth; x++)
                {
                    lines[y] = lines[y] + maptable[x, y] + ",";
                }
            }

            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(name);
                sw.WriteLine();
                foreach (string x in lines)
                {
                    sw.WriteLine(x);
                }
                sw.Flush();
            }
            return true;
        }

        private bool FileExistsDialog()
        {
            Menu exitmenu = new Menu(new string[] { "Tak", "Nie" }, Cursor.CenterX() - 6, Cursor.CenterY() + 2, ConsoleColor.White, ConsoleColor.Red);
            Dialog dialog = new Dialog(1, ConsoleColor.White, ConsoleColor.Red);
            dialog.Show(Cursor.CenterX() - Locale.fileexist.Length / 2 - 2, Cursor.CenterY() - 2, Cursor.CenterY() + Locale.fileexist.Length / 2 + 2, Cursor.CenterY() + 4, "Wyjście", "ESC - powrót ");
            dialog.WriteOn(Locale.fileexist, Cursor.CenterY());
            int value = exitmenu.ShowVertical(2, false, false);
            dialog.Clear();
            if (value == 0) { return true; }
            else { return false; }
        }

    }
}
