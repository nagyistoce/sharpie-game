using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sharpie
{

    // Score|nick
    public class Scores
    {
        
        string path = @"scores.dat";
        List<Highscore>[] wyniki = { new List<Highscore>(), new List<Highscore>(), new List<Highscore>() };
        List<string> file = new List<string>();
        int[] index;
        string end = ":::";

        public Scores()
        {
            if (File.Exists(path))
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    foreach (string rd in File.ReadLines(path))
                    {
                        file.Add(rd);
                    }
                }
            }
            else
            {
                file.Add("Sharpie -- Highscores");
                file.Add("");
                file.Add("Easy:");
                file.Add("");
                file.Add("Medium:");
                file.Add("");
                file.Add("Hard:");
                file.Add("");
                file.Add(":::");
            }

            LoadFile();
        }

        ~Scores()
        {
            ReadIndexOf();
            for (int i = 0; i < wyniki.Length; i++)
            {
                while (wyniki[i].Count > 10)
                {
                    wyniki[i].Sort();
                    wyniki[i].RemoveAt(wyniki[i].Count - 1);
                }

                for (int x = 0; x < wyniki[i].Count; x++)
                {
                    file.Insert(index[x] + 1, wyniki[i][x].Score + "|" + wyniki[i][x].Nick);

                }

                wyniki[i].Clear();
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

        private void LoadFile()
        {
            ReadIndexOf();
            for (int i = 0; i < wyniki.Length; i++)
            {
                for (int j = index[i] + 1; j < index[i + 1]; j++)
                {
                    try
                    {
                        wyniki[i].Add(new Highscore { Score = file[j].Substring(file[j].IndexOf("|") + 1), Nick = file[j].Substring(0, file[j].IndexOf("|")) });
                    }
                    catch (System.Exception) { }
                }
                wyniki[i].Sort();
            }
        }

        public void AddScore(int difficulty, string score, string nick)
        {
            wyniki[difficulty].Add(new Highscore { Score = score, Nick = nick });
        }

        private void ReadIndexOf()
        {
            index = new int[] { file.IndexOf("Easy:"), file.IndexOf("Medium:"), file.IndexOf("Hard:"), file.IndexOf(end)};
        }
    }

    class Highscore
    {
        public string Score { get; set; }
        public string Nick { get; set; }
    }
}
