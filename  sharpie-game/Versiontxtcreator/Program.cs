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
            OpenFileDialog dialogshr = new OpenFileDialog();
            dialogshr.Title = "Znajdź Sharpie.exe";
            dialogshr.Multiselect = false;
            dialogshr.Filter = "Plik Sharpie.exe|Sharpie.exe";
            DialogResult result = dialogshr.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                MessageBox.Show(Application.ProductName + " zostanie zamknięty!", "Ostrzeżenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
            }
            OpenFileDialog dialogEdit = new OpenFileDialog();
            dialogEdit.Title = "Znajdź MapEditor.exe";
            dialogEdit.Multiselect = false;
            dialogEdit.Filter = "Plik MapEditor.exe|MapEditor.exe";
            result = dialogEdit.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                MessageBox.Show(Application.ProductName + " zostanie zamknięty!", "Ostrzeżenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
            }

            FileVersionInfo vershar = FileVersionInfo.GetVersionInfo(dialogshr.FileName);
            FileVersionInfo veredit = FileVersionInfo.GetVersionInfo(dialogEdit.FileName);
            string[] linie = vershar.ToString().Split('\n');
            string liniashar = linie[3];
            linie = veredit.ToString().Split('\n');
            string liniaedit = linie[3];
            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Title = "Podaj miejsce zapisu pliku version.txt";
            savedialog.Filter = "Plik tekstowy (.txt)|*.txt";
            savedialog.FileName = "version.txt";
            DialogResult saveresult = savedialog.ShowDialog();
            if (saveresult == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(savedialog.FileName, false, Encoding.UTF8);
                sw.WriteLine("# Sharpie Version");
                sw.WriteLine(liniashar);
                sw.WriteLine("# SME Version");
                sw.Write(liniaedit);
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
    }
}