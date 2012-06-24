using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Net;

namespace Utils
{
    public class Update
    {
        string localver;
        public string filepath, dirpath;
        string[] versionfile = new string[4];
        int mode;  // 0 - aktualizuje grę, reszta - edytor.
        string ver;
        string[] depend;

        public Update(int mode, string apppath)
        {
            this.mode = mode;
            this.filepath = apppath;
            this.dirpath = Path.GetDirectoryName(apppath);
        }

        public bool CheckUpdate()
        {
            localver = FileVersionInfo.GetVersionInfo(filepath).FileVersion.ToString();
            try
            {
                WebRequest rq = WebRequest.Create("http://sharpie.cba.pl/files/update.txt");
                rq.Credentials = CredentialCache.DefaultCredentials;
                HttpWebResponse rp = (HttpWebResponse)rq.GetResponse();
                Stream st = rp.GetResponseStream();
                StreamReader sr = new StreamReader(st);
                string odpowiedz = sr.ReadToEnd();
                versionfile = odpowiedz.Split('\n');
            }
            catch (Exception)
            {
                return false;
            }

            string[] temp;
            if (mode == 0)
            {
                temp = versionfile[1].Split(':');
            }
            else
            {
                temp = versionfile[3].Split(':');
            }

            ver = temp[0];
            depend = temp[1].Split(',');

            if (localver != ver)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Download(out string error)
        {
            WebClient klient = new WebClient();
            try
            {
                if (mode == 0)
                {
                    klient.DownloadFile(new Uri("http://sharpie.cba.pl/files/Sharpie/Sharpie.exe"), filepath + ".updt");
                }
                else
                {
                    klient.DownloadFile(new Uri("http://sharpie.cba.pl/files/MapEditor/MapEditor.exe"), filepath + ".updt");
                }

                foreach (string s in depend) // pobranie wszystkich bibliotek
                {
                    klient.DownloadFile(new Uri("http://sharpie.cba.pl/files/Sharpie/" + s), dirpath + s + ".updt");
                }

                klient.DownloadFile(new Uri("http://sharpie.cba.pl/files/Updater/installupdate.exe"), Path.GetDirectoryName(filepath));
            }
            catch (System.Exception ex)
            {
                if (File.Exists(filepath + ".updt"))  // przy błędzie usuwa pliki .updt.
                {
                    File.Delete(filepath + ".updt");
                }
                error = ex.Message;
                return false;
            }
            error = "";
            return true;
        }

        public void Install()
        {
            Process.Start("installupdate.exe", Process.GetCurrentProcess().ProcessName);
        }
    }
}
