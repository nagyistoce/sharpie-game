using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Threading;

namespace Downloader
{
    public partial class Form1 : Form
    {
        string previousFolder;
        public Form1()
        {
            InitializeComponent();
        }

        FolderBrowserDialog browse;
        private void folderbt_Click(object sender, EventArgs e)
        {
            browse = new FolderBrowserDialog();
            browse.Description = "Wybierz gdzie chcesz zapisać dane:";
            browse.ShowNewFolderButton = true;
            DialogResult res = browse.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                foldertb.Text = browse.SelectedPath;
            }
        }

        int pobranych;
        int zaznaczonych;
        int dopobrania;
        bool powodzenie = true;
        private void pobierzbt_Click(object sender, EventArgs e)
        {
            pobranych = 0;
            zaznaczonych = 0;

            bool pathentered = false;
            foreach (TreeNode node in drzewo.Nodes)
            {
                if (node.Checked)
                {
                    zaznaczonych++;
                }
            }
            dopobrania = zaznaczonych;
            if (zaznaczonych == 0)
            {
                MessageBox.Show("Nie zaznaczyłeś żadnej rzeczy do pobrania!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            if (foldertb.Text != "")
            {
                pathentered = true;
            }

            if (zaznaczonych > 0 & pathentered)
            {
                WebClient klient;
                bool retry;

                pobierzbt.Enabled = false;
                drzewo.Enabled = false;
                foldertb.Enabled = false;
                folderbt.Enabled = false;
                status.Visible = true;

                for (int i = 0; i < zaznaczonych; i++)
                {
                    if (drzewo.Nodes[i].Checked)
                    {
                        string apppath = Path.Combine(foldertb.Text, drzewo.Nodes[i].Name);
                        bool value = DownloadDependencies(i, Path.GetDirectoryName(apppath));
                        if (!value)
                        {
                            powodzenie = false;
                            break;
                        }
                        do
                        {
                            if (File.Exists(apppath))
                            {
                                DialogResult result = MessageBox.Show("Plik '" + drzewo.Nodes[i].Name + "' już istnieje. Czy chcesz go zastąpić?", "Ostrzeżenie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (result == DialogResult.No)
                                {
                                    break;
                                }
                            }

                            retry = false;
                            status.Text = "Pobieranie " + (pobranych + 1) + "/" + dopobrania + ": " + drzewo.Nodes[i].Name;
                            try
                            {
                                klient = new WebClient();
                                klient.DownloadFile(new Uri("http://sharpie.cba.pl/files/" + Path.GetFileNameWithoutExtension(drzewo.Nodes[i].Name) + "/" + drzewo.Nodes[i].Name), apppath);
                                pobranych++;
                            }
                            catch (Exception ex)
                            {
                                DialogResult res = MessageBox.Show("Podczas pobierania " + drzewo.Nodes[i].Text + " wystąpił błąd i pobieranie zostało przerwane.\n" + ex.Message, "Błąd", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                                if (res == DialogResult.Retry)
                                {
                                    retry = true;
                                }
                                else
                                {
                                    powodzenie = false;
                                    if (File.Exists(apppath))
                                    {
                                        File.Delete(apppath);
                                    }
                                }
                            }
                            progressBar1.Value = (pobranych / dopobrania) * 100;
                            progressBar1.Update();
                            this.Update();
                        } while (retry);
                    }
                }
                pobierzbt.Enabled = true;
                drzewo.Enabled = true;
                foldertb.Enabled = true;
                folderbt.Enabled = true;
                if (powodzenie) { MessageBox.Show("Wybrane pliki zostały ściągnięte!", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                else { MessageBox.Show("Wybrane pliki nie zostały ściągnięte!", "Niepowodzenie", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
                status.Visible = false;
            }
        }

        string odpowiedz;
        List<string> downloadedlist = new List<string>();
        bool DownloadDependencies(int whichnode, string pathtosave)
        {
            bool retry;
            List<string> depends = new List<string>();

            do
            {
                retry = false;
                try
                {
                    WebRequest rq = WebRequest.Create("http://sharpie.cba.pl/files/" + Path.GetFileNameWithoutExtension(drzewo.Nodes[whichnode].Name) + "/ver.txt");
                    rq.Credentials = CredentialCache.DefaultCredentials;
                    HttpWebResponse rp = (HttpWebResponse)rq.GetResponse();
                    Stream st = rp.GetResponseStream();
                    StreamReader sr = new StreamReader(st);
                    odpowiedz = sr.ReadToEnd();
                }
                catch (Exception ex)
                {
                    DialogResult res = MessageBox.Show("Podczas pobierania listy wymaganych komponentów dla " + drzewo.Nodes[whichnode].Text + " wystąpił błąd.\n" + ex.Message, "Błąd", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    if (res == DialogResult.Retry)
                    {
                        retry = true;
                    }
                    else
                    {
                        return false;
                    }
                }
            } while (retry);

            string[] temp = odpowiedz.Split(':');
            if (temp[1].Length > 0)
            {
                dopobrania += temp[1].Split(',').Length;
                bool again = false;
                WebClient klient = new WebClient();
                foreach (string s in temp[1].Split(','))
                {
                    if (!downloadedlist.Exists(x => x == s))
                    {
                        do
                        {
                            again = false;
                            status.Text = "Pobieranie " + (pobranych + 1) + "/" + dopobrania + ": " + s;
                            try
                            {
                                klient.DownloadFile(new Uri("http://sharpie.cba.pl/files/DLLs/" + Path.GetFileNameWithoutExtension(s) + "/" + s), Path.Combine(pathtosave, s));
                                pobranych++;
                            }
                            catch (System.Exception ex)
                            {
                                DialogResult res = MessageBox.Show("Podczas pobierania komponentu " + s + " wymaganego dla " + drzewo.Nodes[whichnode].Text + " wystąpił błąd.\n" + ex.Message, "Błąd", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                                if (res == DialogResult.Retry)
                                {
                                    again = true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            downloadedlist.Add(s);
                            progressBar1.Value = (pobranych / dopobrania) * 100;
                            progressBar1.Update();
                            this.Update();
                        } while (again);
                    }
                    else
                    {
                        dopobrania--;
                    }
                }
            }

            return true;
        }

        private void foldertb_Leave(object sender, EventArgs e)
        {
            if (!Directory.Exists(foldertb.Text))
            {
                MessageBox.Show(this, "Podana ścieżka '" + foldertb.Text + "' nie istnieje!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                foldertb.Text = previousFolder;
            }
        }

        private void foldertb_Enter(object sender, EventArgs e)
        {
            previousFolder = foldertb.Text;
        }
    }
}
