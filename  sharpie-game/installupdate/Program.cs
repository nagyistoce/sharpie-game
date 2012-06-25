using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Threading;

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
            Application.EnableVisualStyles();

            if (args.Length == 0)
            {
                Console.WriteLine("Brak argumentów! Co ja mam instalować?\nNaciśnij dowolny klawisz, aby kontynuować ...");
                Console.ReadKey();
                Environment.Exit(0);
            }

            FindAndKillProcess(Path.GetFileNameWithoutExtension(args[0]));
            MessageBox.Show("Upewnij się, że wszystkie okna gry/edytora są zamknięte!", "Ostrzeżenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            bool retry = false;
            foreach (string s in files)
            {
                do
                {
                    try
                    {
                        if (Path.GetExtension(s) == ".updt")
                        {
                            if (File.Exists(Path.GetFileNameWithoutExtension(s)))
                            {
                                File.Delete(Path.GetFileNameWithoutExtension(s));
                            }
                            File.Move(s, Path.GetFileNameWithoutExtension(s));
                        }
                    }
                    catch (Exception ex)
                    {
                        DialogResult res = MessageBox.Show("Nie można uzyskać dostępu do pliku " + Path.GetFileNameWithoutExtension(s) + ".\nUpewnij się, że plik jest zamknięty i spróbuj jeszcze raz!\n\nPełna treść błędu:\n" + ex.Message, "Błąd", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                        if (res == DialogResult.Retry)
                        {
                            retry = true;
                        }
                        else
                        {
                            DialogResult res2 = MessageBox.Show("Na pewno chcesz przerwać instalację aktualizacji?\nByć może nie będziesz w stanie uruchomić gry/edytora!", "Ostrzeżenie", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                            if (res2 == DialogResult.Yes)
                            {
                                Environment.Exit(0);
                            }
                            else
                            {
                                retry = true;
                            }
                        }
                    }
                } while (retry);
            }

            Process.Start(Path.GetFullPath(args[0]));
        }

        public static void FindAndKillProcess(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.StartsWith(name))
                {
                    clsProcess.Kill();
                }
            }
        }
    }
}
