using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace Sharpie
{
    class Score
    {
        static string path = "Sharpie.scores";
        public LinkedList<Highscore> wynik = new LinkedList<Highscore>();

        public Score()
        {
            if (File.Exists(path)) { LoadScores(); }
        }

        public Score(int difficulty, int score, string nick)
        {
            if (File.Exists(path)) { LoadScores(); }
            wynik.AddLast(new Highscore { difficulty = difficulty, score = score, nick = nick });
            SaveScores();
        }

        public int GetCount(int difficulty)
        {
            return wynik.Count(x => x.difficulty == difficulty);
        }

        public Highscore GetScore(int difficulty, int index)
        {
            List<Highscore> lista = new List<Highscore>(wynik.Where(x => x.difficulty == difficulty));
            return lista[index];
        }

        public void LoadScores()
        {
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
                            Dialog error = new Dialog(1, ConsoleColor.White, ConsoleColor.Red);
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
            if (File.Exists(path)) { File.Delete(path); }
            IEnumerable<Highscore> sortowanie = wynik.OrderByDescending(y => y.score);
            wynik = new LinkedList<Highscore>(sortowanie.ToList());
            for (int i = 0; i <= 2; i++)
            {
                while (wynik.Count(predicate => predicate.difficulty == i) > 10)
                {
                    wynik.Remove(wynik.Last(predicate => predicate.difficulty == i));
                }
            }

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
            File.SetAttributes(path, FileAttributes.Hidden);
        }

        public class Highscore
        {
            public int difficulty { get; set; }
            public int score { get; set; }
            public string nick { get; set; }
        }
    }
}
