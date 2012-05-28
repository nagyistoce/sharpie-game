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
            }
            catch (Exception)
            {
                DialogResult result = MessageBox.Show(updateCheckVersionFile + "Po kliknięciu OK wybierz ścieżkę gry.", "Błąd", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                if (result == DialogResult.OK)
                {
                    OpenFileDialog dialog = new OpenFileDialog();
                    dialog.Title = "Znajdź Sharpie.exe";
                    dialog.Multiselect = false;
                    dialog.Filter = "Plik Sharpie.exe|Sharpie.exe";
                    result = dialog.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        string filepath = dialog.FileName;
                        FileVersionInfo ver = FileVersionInfo.GetVersionInfo(filepath);
                    }
                    else
                    {
                        MessageBox.Show("Aktualizator zostanie zamknięty!", "Ostrzeżenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        throw new Exception();
                        Application.Exit();
                    }
                }
                else
                {
                    MessageBox.Show("Aktualizator zostanie zamknięty!", "Ostrzeżenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    throw new Exception();
                    
                }

            }

            try
            {
                WebRequest rq = WebRequest.Create("http://serwer.pl/aktualizacja/wersja.txt");
                rq.Credentials = CredentialCache.DefaultCredentials;
                HttpWebResponse rp = (HttpWebResponse)rq.GetResponse();
                Stream st = rp.GetResponseStream();
                StreamReader sr = new StreamReader(st);
                string odpowiedz = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                MessageBox.Show(updateInfoError + " " + ex.Message + "/nAplikacja zostanie zamknięta.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
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
    }
}
