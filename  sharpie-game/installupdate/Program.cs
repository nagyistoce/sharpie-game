using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace installupdate
{
    class Program
    {
        private static Process GetaProcess(string processname)
        {
            Process[] aProc = Process.GetProcessesByName(processname);

            if (aProc.Length > 0)
                return aProc[0];

            else return null;
        }

        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.updt");

            if (args.Length <= 1)
            {
                throw new Exception("Brak argumentów! Co ja mam zamknąć?");
            }
            Process proc = GetaProcess(Path.GetFileName(args[0]));
            proc.CloseMainWindow();
            proc.WaitForExit();
            proc.Close();

            foreach (string s in files)
            {
                if (File.Exists(Path.GetFileNameWithoutExtension(s)))
                {
                    File.Delete(Path.GetFileNameWithoutExtension(s));
                }

                File.Move(s, Path.GetFileNameWithoutExtension(s));
            }

            proc.Start();
        }
    }
}
