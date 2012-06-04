using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sharpie
{

    // nick|1273217418
    public class Scores
    {
        string path = @".\scores.dat";
        List<Highscore>[] wyniki;
        List<string> file;
        int[] start;
        int[] end;

        public Scores()
        {
            file = new List<string>();
            wyniki = new List<Highscore>[3];
            for (int i = 0; i < wyniki.Length; i++)
            {
                wyniki[i] = new List<Highscore>();
            }

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

        ~Scores()
        {
            if (wyniki.Length > 0)
            {
                ReadIndexOf();
                for (int i = 0; i < wyniki.Length; i++)
                {
                    int x = 0;
                    for (int j = start[i] + 1; j < end[i]; j++)
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

        private void LoadScores()
        {
            ReadIndexOf();
            for (int i = 0; i < wyniki.Length; i++)
            {
                for (int j = start[i] + 1; j < end[i]; j++)
                {

                    string[] readfile = { file[j].Substring(0, file[j].IndexOf('|')), file[j].Substring(file[j].IndexOf('|') + 1, file[j].Length - file[j].IndexOf('|') - 1)};
                    wyniki[i].Add(new Highscore { Nick = readfile[0], Score = readfile[1] });
                }
            }
            for (int i = 0; i < wyniki.Length; i++)
            {
                wyniki[i].Sort();
            }
        }

        public void AddScore(int difficulty, string score, string nick)
        {
            wyniki[difficulty].Add(new Highscore { Score = score, Nick = nick });
        }

        private void ReadIndexOf()
        {
            start = new int[] { file.IndexOf("Easy:"), file.IndexOf("Medium:"), file.IndexOf("Hard:") };
            end = new int[] { file.IndexOf(":Easy"), file.IndexOf(":Medium"), file.IndexOf(":Hard") };
        }
    }

    class Highscore
    {
        public string Score { get; set; }
        public string Nick { get; set; }
    }
}
