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

            if (args.Length == 0)
            {
                throw new Exception("Brak argumentów! Co ja mam zamknąć?");
            }
            Process proc = GetaProcess(args[0]);
            if (proc != null)
            {
                proc.CloseMainWindow();
                proc.WaitForExit();
                proc.Close();
            }

            foreach (string s in files)
            {
                Console.WriteLine(s);
                Console.WriteLine(Path.GetFileNameWithoutExtension(s));
                if (File.Exists(Path.GetFileNameWithoutExtension(s)))
                {
                    File.Delete(Path.GetFileNameWithoutExtension(s));
                }
                File.Move(s, Path.GetFileNameWithoutExtension(s));
            }

            if (proc != null) { Process.Start(Path.GetFullPath(args[0])); }
        }
    }
}
