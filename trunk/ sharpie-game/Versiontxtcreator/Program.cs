using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace Versiontxtcreator
{
    class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            Program versioner = new Program();
            versioner.GetVersiontoTxt();
        }

        void GetVersiontoTxt()
        {
            Application.EnableVisualStyles();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Znajdź Sharpie.exe";
            dialog.Multiselect = false;
            dialog.Filter = "Plik Sharpie.exe|Sharpie.exe";
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string filepath = dialog.FileName;
                FileVersionInfo ver = FileVersionInfo.GetVersionInfo(filepath);
                string[] linie = ver.ToString().Split('\n');
                string linia = linie[3];
                SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.Title = "Podaj miejsce zapisu pliku version.txt";
                savedialog.Filter = "Plik tekstowy (.txt)|*.txt";
                savedialog.FileName = "version.txt";
                DialogResult saveresult = savedialog.ShowDialog();
                if (saveresult == DialogResult.OK)
                {
                    StreamWriter sw = new StreamWriter(savedialog.FileName, false, Encoding.UTF8);
                    sw.Write(linia);
                    sw.Flush();
                    sw.Close();
                    MessageBox.Show("Plik version.txt został pomyślnie utworzony w " + Path.GetDirectoryName(savedialog.FileName), "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(Application.ProductName + " zostanie zamknięty!", "Ostrzeżenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Application.Exit();
                }

            }
            else
            {
                MessageBox.Show(Application.ProductName + " zostanie zamknięty!", "Ostrzeżenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
            }
        }
    }
}