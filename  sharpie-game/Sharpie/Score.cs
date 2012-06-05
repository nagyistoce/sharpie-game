﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace Sharpie
{
    class Score
    {
        static string path = "scores.dat";
        LinkedList<Highscore> wynik = new LinkedList<Highscore>();

        public Score() { }

        public Score(int difficulty, int score, string nick)
        {
            if (File.Exists(path)) { LoadScores(); }
            wynik.AddLast(new Highscore { difficulty = difficulty, score = score, nick = nick });
            SaveScores();
        }

        public void LoadScores()
        {
            File.Decrypt(path);
            using (StreamReader sr = File.OpenText(path))
            {
                foreach (string x in File.ReadLines(path))
                {
                    if (x.Contains(':'))
                    {
                        try
                        {
                            string[] y = x.Split(new char[] { ':' });
                            wynik.AddLast(new Highscore { difficulty = Convert.ToInt32(y[0]), score = Convert.ToInt32(y[1]), nick = y[2] });
                        }
                        catch (Exception)
                        {
                            Dialog error = new Dialog(ConsoleColor.White, ConsoleColor.Red);
                            string wiad = "Niepoprawny wynik w 'scores.dat'. Zostanie on usunięty.";
                            error.Show(Cursor.CenterX() - wiad.Length / 2 - 2, Cursor.CenterY() - 2, Cursor.CenterX() + wiad.Length / 2 + 2, Cursor.CenterY() + 2, "Błąd", "ENTER - zatwierdź                                    ");
                            error.WriteOn(wiad, Cursor.CenterY());
                            ConsoleKeyInfo key;
                            do
                            {
                                key = Console.ReadKey(true);
                            } while (key.Key != ConsoleKey.Enter);
                        }
                    }
                }
            }
        }

        public void SaveScores()
        {
            if (!File.Exists(path)) { File.Delete(path); }
            wynik.OrderBy(x => x.difficulty).ThenByDescending(y => y.score);
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine("Sharpie -- Highscores");
                sw.WriteLine("");
                foreach (Highscore score in wynik)
                {
                    sw.WriteLine(score.difficulty + ":" + score.score + ":" + score.nick);
                }
                sw.Flush();
            }

            File.Encrypt(path);
        }

        class Highscore
        {
            public int difficulty { get; set; }
            public int score { get; set; }
            public string nick { get; set; }
        }
    }
}
