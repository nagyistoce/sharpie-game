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
        public List<Highscore>[] wyniki;
        List<string> file;
        int[] start;
        int[] end;

        public void Initialize()
        {
            file = new List<string>();
            if (File.Exists(path))
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    foreach (string rd in File.ReadLines(path))
                    {
                        file.Add(rd);
                    }
                }
                LoadScores();
            }
            else
            {
                file.Add("Sharpie -- Highscores");
                file.Add("");
                file.Add("Easy:");
                file.Add("");
                file.Add(":Easy");
                file.Add("");
                file.Add("Medium:");
                file.Add("");
                file.Add(":Hard");
                file.Add("");
                file.Add("Hard:");
                file.Add("");
                file.Add(":Hard");
                file.Add("");
            }

            
        }

        private void LoadScores()
        {
            ReadIndexOf();
            wyniki = new List<Highscore>[2];
            for (int i = 0; i < file.Count; i++)
            {
                for (int j = start[i] + 1; j < end[i]; i++)
                {
                    string[] readfile = new string[2];
                    readfile = file[j].Split('|');
                    wyniki[i].Add(new Highscore { Difficulty = Convert.ToInt32(readfile[0]), Nick = readfile[1], Score = readfile[2] });
                }
            }
            for (int i = 0; i < wyniki.Length; i++)
            {
                wyniki[i].Sort();
            }
        }

        public void AddScore(int difficulty, int score, string nick)
        {
            wyniki[difficulty].Add(new Highscore { Difficulty = difficulty, Score = score.ToString(), Nick = nick });
        }

        public void SaveScores()
        {
            if (wyniki.Length > 0)
            {
                ReadIndexOf();
            for (int i = 0; i < file.Count; i++)
            {
                int x = 0;
                for (int j = start[i] + 1; j < end[i]; i++)
                {
                    if (!file.Contains(wyniki[i][x].Nick + "|" + wyniki[i][x].Score))
                    {
                        file.Insert(j, wyniki[i][x].Nick + "|" + wyniki[i][x].Score);
                    }
                    x++;
                }
            }
            }


            using (StreamWriter sw = File.CreateText(path))
            {
                foreach (string x in file)
                {
                    sw.WriteLine(x);
                }
                sw.Flush();
            }
        }

        private void ReadIndexOf()
        {
            start = new int[] { file.IndexOf("Easy:"), file.IndexOf("Medium:"), file.IndexOf("Hard:") };
            end = new int[] { file.IndexOf(":Easy"), file.IndexOf(":Medium"), file.IndexOf(":Hard") };
        }
    }

    class Highscore
    {
        public int Difficulty { get; set; }
        public string Score { get; set; }
        public string Nick { get; set; }
    }
}
