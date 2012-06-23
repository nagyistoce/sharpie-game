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
            dialogshr.InitialDirectory = @"C:\Users\Adam\Documents\Visual Studio 2010\Projects\Sharpie\Sharpie\bin\Debug";
            dialogshr.Filter = "Plik Sharpie.exe|Sharpie.exe";
            DialogResult result = dialogshr.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                MessageBox.Show(Application.ProductName + " zostanie zamknięty!", "Ostrzeżenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Environment.Exit(0);
            }
            OpenFileDialog dialogEdit = new OpenFileDialog();
            dialogEdit.Title = "Znajdź MapEditor.exe";
            dialogEdit.Multiselect = false;
            dialogEdit.InitialDirectory = @"C:\Users\Adam\Documents\Visual Studio 2010\Projects\Sharpie\MapEditor\bin\Debug";
            dialogEdit.Filter = "Plik MapEditor.exe|MapEditor.exe";
            result = dialogEdit.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                MessageBox.Show(Application.ProductName + " zostanie zamknięty!", "Ostrzeżenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Environment.Exit(0);
            }

            string vershar = FileVersionInfo.GetVersionInfo(dialogshr.FileName).FileVersion.ToString();
            string veredit = FileVersionInfo.GetVersionInfo(dialogEdit.FileName).FileVersion.ToString();
            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Title = "Podaj miejsce zapisu pliku version.txt";
            savedialog.Filter = "Plik tekstowy (.txt)|*.txt";
            savedialog.FileName = "upversion.txt";
            savedialog.InitialDirectory = @"C:\Users\Adam\Desktop";
            DialogResult saveresult = savedialog.ShowDialog();
            if (saveresult == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(savedialog.FileName, false, Encoding.UTF8);
                sw.WriteLine("# Sharpie Version");
                sw.WriteLine(vershar);
                sw.WriteLine("# SME Version");
                sw.Write(veredit);
                sw.Flush();
                sw.Close();
                MessageBox.Show("Plik upversion.txt został pomyślnie utworzony w " + Path.GetDirectoryName(savedialog.FileName), "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(Application.ProductName + " zostanie zamknięty!", "Ostrzeżenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Environment.Exit(0);
            }
        }
    }
}