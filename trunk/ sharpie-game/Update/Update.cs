using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Collections;
using Utils;

namespace Update
{
    public class Update
    {
        public string filepath, dirpath, appname;
        string odpowiedz;
        string appver;
        string localver;
        int mode;  // 0 - aktualizuje grę, reszta - edytor.
        List<CompVer> lista = new List<CompVer>();
        public static bool WasUpdateChecked = false;

        public Update(int mode, string appname, string version)
        {
            this.appname = appname;
            localver = version;
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

        public bool CheckUpdate()
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

        public static bool IsInternet()
        {
            try
            {
                IPHostEntry checkconnect = Dns.GetHostEntry("www.google.com"); // ponieważ serwery google'a mają duży uptime ;>
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public void UpdateProcedure()
        {
            bool IsUpdate = CheckUpdate();
            if (IsUpdate)
            {
                string text;
                if (mode == 0)
                {
                    text = "gry";
                }
                else
                {
                    text = "edytora";
                }
                string dialogtext = "Dostępna jest nowa wersja " + text + ". Zaktualizować?";
                Dialog updtdialog = new Dialog(1, ConsoleColor.White, ConsoleColor.DarkGreen);
                Menu updtmenu = new Menu(new string[] { "Tak", "Nie" }, Crs.CenterX() - 6, Crs.CenterY() + 2, ConsoleColor.White, ConsoleColor.DarkGreen);
                updtdialog.Show(Crs.CenterX() - 17, Crs.CenterY() - 3, Crs.CenterX() + 17, Crs.CenterY() + 4, "Aktualizacja", "Enter - wybierz");
                updtdialog.WriteOn(dialogtext, Crs.CenterY() - 1);
                int value = updtmenu.ShowVertical(4, true, false, 0);
                switch (value)
                {
                    case 0:
                        updtdialog.Clear();
                        string error;
                        string download = "Pobieram aktualizację ...";
                        Dialog downupdt = new Dialog(1, ConsoleColor.White, ConsoleColor.Red);
                        downupdt.Show(Crs.CenterX() - download.Length / 2 - 2, Crs.CenterY() - 2, Crs.CenterX() + download.Length / 2 + 2, Crs.CenterY() + 2);
                        downupdt.WriteOn(download, Crs.CenterY());
                        bool downstatus = Download(out error);
                        if (downstatus)
                        {
                            Install();
                        }
                        else
                        {
                            downupdt.Show(Crs.CenterX() - 20 - 2, Crs.CenterY() - 4, Crs.CenterX() + 20 + 2, Crs.CenterY() + 4);
                            downupdt.WriteOn(error, Crs.CenterY() - 2);
                            Menu errormenu = new Menu(new string[] { "OK" }, Crs.CenterX() - 2, Crs.CenterY() + 2, ConsoleColor.White, ConsoleColor.Red);
                            errormenu.ShowVertical(0, false, false, 0);
                            downupdt.Clear();
                        }
                        break;
                    case 1:
                        updtdialog.Clear();
                        break;
                    case -1:
                        updtdialog.Clear();
                        break;
                }
            }
        }

    }
}
