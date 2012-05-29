﻿using System;
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

        public bool CheckUpdate()
        {
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
                goto Retry;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(updateInfoError + " " + ex.Message + "\nAktualizator zostanie zamknięty!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception();
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
                MessageBox.Show("Aktualizator zostanie zamknięty!", "Ostrzeżenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw new Exception();
            }
        }

        public bool DoUpgrade()
        {
            WebClient klient = new WebClient();
            Form1 form = new Form1();
            do 
            {
                try
                {
                    File.Delete(filepath);
                    klient.DownloadFile("http://sharpie-game.googlecode.com/files/Sharpie.exe", @".\Sharpie.exe");
                    return true;
                }
                catch (IOException ex)
                {
                    DialogResult result = MessageBox.Show(ex + " Upewnij się, że gra jest wyłączona i kliknij Retry, aby ponowić próbę.", "Błąd", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    if (result == DialogResult.Cancel)
                    {
                        MessageBox.Show("Aktualizator zostanie zamknięty! Nie można zaktualizować gry.", "Ostrzeżenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        throw new Exception();
                    }
                }
            } while (true);
        }
    }
}