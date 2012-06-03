using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sharpie
{

    // nick|1273217418
    class Scores
    {
        string path = @".\scores.dat";
        public LinkedList<Highscore> wyniki;
        List<string> file;

        public void Initialize()
        {
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("Sharpie -- Highscores");
                    sw.WriteLine();
                    sw.Flush();
                }
            }
            else
            {
                file = new List<string>();
                using (StreamReader sr = File.OpenText(path))
                {
                    foreach (string rd in File.ReadLines(path))
                    {
                        file.Add(rd);
                    }
                }
            }

            LoadScores();
        }

        private void LoadScores()
        {
            wyniki = new LinkedList<Highscore>();
            for (int i = 0; i < file.Count; i++)
            {
                wyniki.AddLast(new Highscore { Difficulty = Convert.ToInt32(file[i].Substring(0, file.IndexOf("|"))), Nick = file[i].Substring(file.IndexOf("|"), file.LastIndexOf("|")), Score = file[i].Substring(file.LastIndexOf("|")) });
            }
        }

        public void AddScore(int difficulty, int score, string nick)
        {
            wyniki.AddLast(new Highscore { Difficulty = difficulty, Score = score.ToString(), Nick = nick });
        }

        public void SaveScores()
        {

        }
    }

    class Highscore
    {
        public int Difficulty { get; set; }
        public string Score { get; set; }
        public string Nick { get; set; }
    }
}
