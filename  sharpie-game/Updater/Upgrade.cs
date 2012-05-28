using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Net;
using System.IO;

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

        public bool CheckUpdate()
        {
            try
            {
                FileVersionInfo ver = FileVersionInfo.GetVersionInfo(@".\Sharpie.exe");
                WebRequest rq = WebRequest.Create("http://serwer.pl/aktualizacja/wersja.txt");
                rq.Credentials = CredentialCache.DefaultCredentials;
                HttpWebResponse rp = (HttpWebResponse)rq.GetResponse();
                Stream st = rp.GetResponseStream();
                StreamReader sr = new StreamReader(st);
                string odpowiedz = sr.ReadToEnd();
            }
            catch (FileNotFoundException)
            {
                DialogResult result = MessageBox.Show(updateCheckVersionFile + "Po kliknięciu OK znajdź i wybierz plik Sharpie.exe", "Błąd", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                if (result == DialogResult.OK)
                {
                    FindSharpie();
                }
                else
                {
                    MessageBox.Show("Aktualizator zostanie zamknięty!", "Ostrzeżenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    throw new Exception();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(updateInfoError + " " + ex.Message + "/nAktualizator zostanie zamknięty!.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception();
            }

            if (this.ver.FileVersion != this.odpowiedz)
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
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Znajdź Sharpie.exe";
            dialog.Multiselect = false;
            dialog.Filter = "Plik Sharpie.exe|Sharpie.exe";
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string filepath = dialog.FileName;
                FileVersionInfo ver = FileVersionInfo.GetVersionInfo(filepath);
                MessageBox.Show(ver.ToString(), "Tytuł", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Aktualizator zostanie zamknięty!", "Ostrzeżenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw new Exception();
            }
        }
    }
}
