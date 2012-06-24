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
            foreach (string s in files)
            {
                Process proc = GetaProcess(Path.GetFileName(s));
                proc.CloseMainWindow();
                proc.WaitForExit();
                proc.Close();
                File.Move(s, Path.GetFileNameWithoutExtension(s));
                proc.Start();
            }
        }
    }
}
