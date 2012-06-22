using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Security.Permissions;

namespace Updater
{
    class Upgrade
    {
        public const string updateSuccess = "Sharpie został zaktualizowany.";
        public const string updateCurrent = "Masz najnowszą wersję gry!";
        public const string updateInfoError = "Błąd w uzyskiwaniu informacji o aktualizacji!";
        public const string updateCheckVersionFile = "Nie można pobrać wersji gry z pliku! ";
        private FileVersionInfo ver;
        private string odpowiedz;
        public string sharpiefilepath = Path.GetFullPath("Sharpie.exe");
        private string sharpiepath = Path.GetDirectoryName("Sharpie.exe");
        private string editorfilepath = Path.GetFullPath("MapEditor.exe");
        private string editorpath = Path.GetDirectoryName("MapEditor.exe");
        string[] wersja = new string[2];
        string[] linie2 = new string[6];

        public void CheckUpdate(out bool updtSharpie, out bool updtEditor)
        {
            updtSharpie = false;
            updtEditor = false;
            Form1 form = new Form1();
        Retry:
            try
            {
                updtSharpie = true;
                ver = FileVersionInfo.GetVersionInfo(Path.GetFullPath(sharpiefilepath));
                string[] linie = ver.ToString().Split('\n');
                wersja[0] = linie[3];
            }
            catch (FileNotFoundException)
            {
                DialogResult result = MessageBox.Show(form.Parent, updateCheckVersionFile + "Po kliknięciu OK znajdź i wybierz plik Sharpie.exe", "Błąd", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                if (result == DialogResult.OK)
                {
                    FindSharpie();
                    goto Retry;
                }
                else
                {
                    updtSharpie = false;
                }
            }
        Retry2:
            try
            {
                updtEditor = true;
                ver = FileVersionInfo.GetVersionInfo(Path.GetFullPath(editorfilepath));
                string[] linie = ver.ToString().Split('\n');
                wersja[1] = linie[3];
            }
            catch (FileNotFoundException)
            {
                DialogResult result = MessageBox.Show(form.Parent, updateCheckVersionFile + "Po kliknięciu OK znajdź i wybierz plik MapEditor.exe", "Błąd", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                if (result == DialogResult.OK)
                {
                    FindSME();
                    updtEditor = true;
                    goto Retry2;
                }
                else
                {
                    updtEditor = false;
                }
            }

            if (!updtEditor & !updtSharpie)
            {
                MessageBox.Show(form.Parent, "Nie ma czego zaktualizować, aktualizator zostanie zamknięty!", "Ostrzeżenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
            }

            try
            {
                WebRequest rq = WebRequest.Create("http://sharpie-game.googlecode.com/files/upversion.txt");
                rq.Credentials = CredentialCache.DefaultCredentials;
                HttpWebResponse rp = (HttpWebResponse)rq.GetResponse();
                Stream st = rp.GetResponseStream();
                StreamReader sr = new StreamReader(st);
                odpowiedz = sr.ReadToEnd();
                linie2 = odpowiedz.Split('\r');
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(form.Parent, updateInfoError + " " + ex.Message + "\nAktualizator zostanie zamknięty!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            if (updtSharpie && wersja[0] != linie2[1])
            {
                updtSharpie = false;
            }
            if (updtEditor && wersja[1] != linie2[4])
            {
                updtEditor = false;
            }


        }

        private void FindSharpie()
        {
            Form1 form = new Form1();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Znajdź Sharpie.exe";
            dialog.Multiselect = false;
            dialog.Filter = "Plik Sharpie.exe|Sharpie.exe";
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                sharpiefilepath = dialog.FileName;
                sharpiepath = Path.GetDirectoryName(sharpiefilepath);
                ver = FileVersionInfo.GetVersionInfo(sharpiefilepath);
            }
            else
            {
                MessageBox.Show(form.Parent, "Aktualizator zostanie zamknięty!", "Ostrzeżenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
            }
        }

        private void FindSME()
        {
            Form1 form = new Form1();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Znajdź MapEditor.exe";
            dialog.Multiselect = false;
            dialog.Filter = "Plik MapEditor.exe|MapEditor.exe";
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                editorfilepath = dialog.FileName;
                editorpath = Path.GetDirectoryName(editorfilepath);
                ver = FileVersionInfo.GetVersionInfo(editorfilepath);
            }
            else
            {
                MessageBox.Show(form.Parent, "Aktualizator zostanie zamknięty!", "Ostrzeżenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
            }
        }

        public bool DoUpgrade(bool UpdateSharpie, bool UpdateEditor)
        {
            Form1 form = new Form1();
            WebClient klient = new WebClient();
            try
            {
                if (UpdateSharpie)
                {
                    File.Delete(sharpiefilepath);
                    klient.DownloadFile(new Uri("http://sharpie-game.googlecode.com/files/Sharpie.exe"), sharpiefilepath);
                }
                if (UpdateEditor)
                {
                    File.Delete(editorfilepath);
                    klient.DownloadFile(new Uri("http://sharpie-game.googlecode.com/files/MapEditor.exe"), editorfilepath);
                }
            }
            catch (IOException ex)
            {
                DialogResult result = MessageBox.Show(form.Parent, ex + " Upewnij się, że aplikacja jest wyłączona i kliknij Retry, aby ponowić próbę.", "Błąd", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Cancel)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
