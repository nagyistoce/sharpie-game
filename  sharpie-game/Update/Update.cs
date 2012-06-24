using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Net;

namespace Update
{
    public class Update
    {
        string ver;
        public string filepath;
        string[] versionfile = new string[4];
        int mode;  // 0 - aktualizuje grę, reszta - edytor.

        public Update(int mode, string apppath)
        {
            this.mode = mode;
            this.filepath = apppath;
        }

        public bool CheckUpdate()
        {
            ver = FileVersionInfo.GetVersionInfo(filepath).FileVersion.ToString();
            try
            {
                WebRequest rq = WebRequest.Create("http://sharpie.cba.pl/files/Updater/upversion.txt");
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

            if (mode == 0)
            {
                if (ver != versionfile[1])
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (ver != versionfile[3])
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
                klient.DownloadFile(new Uri("http://sharpie.cba.pl/files/Updater/installupdate.exe"), Path.GetDirectoryName(filepath));
            }
            catch (System.Exception ex)
            {
                if (File.Exists(filepath + ".updt"))
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
            Process.Start("installupdate.exe");
        }
    }
}
