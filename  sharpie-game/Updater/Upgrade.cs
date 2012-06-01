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
        private string filepath = Path.GetFullPath("Sharpie.exe");
        private string path = Path.GetDirectoryName("Sharpie.exe");
        private string linia;
        private string netlinia;

        public string ReturnVer()
        {
            return netlinia;
        }

        public bool CheckUpdate()
        {
            Form1 form = new Form1();
        Retry:
            try
            {
                ver = FileVersionInfo.GetVersionInfo(Path.GetFullPath(filepath));
                string[] linie = ver.ToString().Split('\n');
                linia = linie[3];
                WebRequest rq = WebRequest.Create("http://sharpie-game.googlecode.com/files/version.txt");
                rq.Credentials = CredentialCache.DefaultCredentials;
                HttpWebResponse rp = (HttpWebResponse)rq.GetResponse();
                Stream st = rp.GetResponseStream();
                StreamReader sr = new StreamReader(st);
                odpowiedz = sr.ReadToEnd();
                string[] linie2 = odpowiedz.Split('\n');
                netlinia = linie2[0];
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
                    MessageBox.Show(form.Parent, "Aktualizator zostanie zamknięty!", "Ostrzeżenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Application.Exit();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(form.Parent, updateInfoError + " " + ex.Message + "\nAktualizator zostanie zamknięty!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            if (linia != netlinia)
            {
                return true;
            }
            else
            {
                return false;
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
                filepath = dialog.FileName;
                path = Path.GetDirectoryName(filepath);
                ver = FileVersionInfo.GetVersionInfo(filepath);
            }
            else
            {
                MessageBox.Show(form.Parent, "Aktualizator zostanie zamknięty!", "Ostrzeżenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
            }
        }

        public bool DoUpgrade()
        {
            Form1 form = new Form1();
            WebClient klient = new WebClient();
            try
            {
                File.Delete(filepath);
                klient.DownloadFile(new Uri("http://sharpie-game.googlecode.com/files/Sharpie.exe"), @".\Sharpie.exe");
            }
            catch (IOException ex)
            {
                DialogResult result = MessageBox.Show(form.Parent, ex + " Upewnij się, że gra jest wyłączona i kliknij Retry, aby ponowić próbę.", "Błąd", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Cancel)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
