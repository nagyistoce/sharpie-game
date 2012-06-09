using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Sharpie;
using System.Windows.Forms;

namespace MapEditor
{
    class Map
    {
        static string name = "";
		public static string path = @"Maps\";
        string[] lines = new string[Console.WindowHeight - 1];

		public bool LoadMap(out string[] map)
		{
			OpenFileDialog open = new OpenFileDialog();
			open.Title = "Otwórz mapę ...";
			open.Filter = "Sharpie Map Format (*.smf)|*.smf";
			open.DefaultExt = "smf";
			open.AutoUpgradeEnabled = true;
			open.AddExtension = true;

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			open.InitialDirectory = Path.GetFullPath(path);

			DialogResult result = open.ShowDialog();
			switch (result)
			{
				case DialogResult.Abort:
					map = lines;
					return false;
			}

			using (StreamReader sr = File.OpenText(open.FileName))
			{
				name = sr.ReadLine();
				sr.ReadLine();
				for (int i = 0; i < Console.WindowHeight - 1; i++)
				{
					lines[i] = sr.ReadLine();
				}
			}
			map = lines;
			return true;
		}

        public bool SaveMap(string[,] maptable)
        {
			SaveFileDialog save = new SaveFileDialog();
			save.Title = "Zapisz mapę ...";
			save.Filter = "Sharpie Map Format (*.smf)|*.smf";
			save.DefaultExt = "smf";
			save.OverwritePrompt = true;
			save.AutoUpgradeEnabled = true;
			save.AddExtension = true;

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			save.InitialDirectory = Path.GetFullPath(path);

			DialogResult result = save.ShowDialog();
			switch (result)
			{
				case DialogResult.Cancel:
					return false;
			}

            for (int y = 0; y < Console.WindowHeight - 1; y++)
            {
                for (int x = 0; x < Console.WindowWidth; x++)
                {
					if (maptable[x, y] == null)
					{
						lines[y] += "!" + ",";
					}
					else
					{
						lines[y] += maptable[x, y] + ",";
					}
                }
            }

            using (StreamWriter sw = File.CreateText(save.FileName))
            {
                sw.WriteLine(Path.GetFileName(save.FileName));
                sw.WriteLine();
                foreach (string x in lines)
                {
                    sw.WriteLine(x);
                }
                sw.Flush();
            }

            return true;
        }

    }
}
