using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Collections;

namespace Utils
{
    public class Update
    {
        public string filepath, dirpath, appname;
        string odpowiedz;
        string appver;
        int mode;  // 0 - aktualizuje grę, reszta - edytor.
        List<CompVer> lista = new List<CompVer>();

        public Update(int mode, string appname)
        {
            this.appname = appname;
            this.filepath = Path.GetFullPath(appname);
            this.dirpath = Path.GetDirectoryName(filepath);
            this.mode = mode;
        }

        struct CompVer
        {
            public string component;
            public string localver;
            public string newver;

            public CompVer(string component, string localver) : this(component, localver, "") { }

            public CompVer(string component, string localver, string newver)
            {
                this.component = component;
                this.localver = localver;
                this.newver = newver;
            }
        }

        public bool CheckUpdate(string localver)
        {
            try
            {
                WebRequest rq = WebRequest.Create("http://sharpie.cba.pl/files/" + Path.GetFileNameWithoutExtension(appname) + "/ver.txt");
                rq.Credentials = CredentialCache.DefaultCredentials;
                HttpWebResponse rp = (HttpWebResponse)rq.GetResponse();
                Stream st = rp.GetResponseStream();
                StreamReader sr = new StreamReader(st);
                odpowiedz = sr.ReadToEnd();
            }
            catch (Exception)
            {
                return false;
            }

            string[] temp = odpowiedz.Split(':');

            appver = temp[0];
            if (temp[1].Length > 0)
            {
                foreach (string s in temp[1].Split(','))
                {
                    if (File.Exists(s))
                    {
                        lista.Add(new CompVer(s, FileVersionInfo.GetVersionInfo(s).FileVersion.ToString()));
                    }
                    else
                    {
                        lista.Add(new CompVer(s, ""));
                    }
                }

                for (int i = 0; i < lista.Count; i++)
                {

                    WebRequest rq = WebRequest.Create("http://sharpie.cba.pl/files/DLLs/" + Path.GetFileNameWithoutExtension(lista[i].component) + "/ver.txt");
                    rq.Credentials = CredentialCache.DefaultCredentials;
                    HttpWebResponse rp = (HttpWebResponse)rq.GetResponse();
                    Stream st = rp.GetResponseStream();
                    StreamReader sr = new StreamReader(st);
                    lista[i] = new CompVer(lista[i].component, lista[i].localver, sr.ReadToEnd());
                }
            }

            if (localver != appver)
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

                foreach (CompVer cv in lista) // pobranie wszystkich bibliotek
                {
                    if (cv.localver != cv.newver)
                    {
                        klient.DownloadFile(new Uri("http://sharpie.cba.pl/files/DLLs/" + Path.GetFileNameWithoutExtension(cv.component) + "/" + cv.component), Path.Combine(dirpath, cv.component + ".updt"));
                    }
                }

                klient.DownloadFile(new Uri("http://sharpie.cba.pl/files/Updater/installupdate.exe"), Path.Combine(dirpath, "installupdate.exe"));
            }
            catch (System.Exception ex)
            {
                string[] filenames = Directory.GetFiles(dirpath, "*.updt");
                foreach (string file in filenames)
                {
                    File.Delete(file);
                }

                error = ex.Message;
                return false;
            }
            error = "";
            return true;
        }

        public void Install()
        {
            Process.Start("installupdate.exe", "\"" + Process.GetCurrentProcess().ProcessName + "\"");
        }
    }
}
