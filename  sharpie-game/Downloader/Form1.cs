﻿using System;
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

        private void pobierzbt_Click(object sender, EventArgs e)
        {
            pobierzbt.Enabled = false;
            drzewo.Enabled = false;
            foldertb.Enabled = false;
            folderbt.Enabled = false;
            progressBar1.Enabled = true;

            bool existchecked = false;
            bool pathentered = false;
            foreach (TreeNode node in drzewo.Nodes)
            {
                if (node.Checked)
                {
                    existchecked = true;
                    break;
                }
            }

            if (!existchecked)
            {
                MessageBox.Show("Nie zaznaczyłeś żadnej rzeczy do pobrania!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            if (foldertb.Text != "")
            {
                pathentered = true;
            }

            if (existchecked & pathentered)
            {
                WebClient klient;
                bool retry;

                if (drzewo.Nodes[0].Checked)
                {
                    do
                    {
                        string path = Path.Combine(foldertb.Text, "Sharpie.exe");
                        if (File.Exists(path))
                        {
                            DialogResult result = MessageBox.Show("Plik '" + Path.GetFileName(path) + "' już istnieje. Czy chcesz go zastąpić?", "Ostrzeżenie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.No)
                            {
                                break;
                            }
                        }

                        retry = false;
                        try
                        {
                            klient = new WebClient();
                            klient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(klient_DownloadProgressChanged);
                            klient.DownloadFileCompleted += new AsyncCompletedEventHandler(klient_DownloadFileCompleted);
                            this.UseWaitCursor = true;
                            klient.DownloadFileAsync(new Uri("http://sharpie.cba.pl/files/Sharpie/Sharpie.exe"), path);
                            while (klient.IsBusy) ;
                        }
                        catch (Exception ex)
                        {
                            DialogResult res = MessageBox.Show("Nie udało się pobrać '" + drzewo.Nodes[0].Text + "'.\n" + ex.Message, "Błąd", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                            if (res == DialogResult.Retry)
                            {
                                retry = true;
                            }
                        }
                    } while (retry);

                }

                if (drzewo.Nodes[1].Checked)
                {
                    do
                    {
                        string path = Path.Combine(foldertb.Text, "Updater.exe");
                        if (File.Exists(path))
                        {
                            DialogResult result = MessageBox.Show("Plik '" + Path.GetFileName(path) + "' już istnieje. Czy chcesz go zastąpić?", "Ostrzeżenie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.No)
                            {
                                break;
                            }
                        }

                        retry = false;
                        try
                        {
                            klient = new WebClient();
                            klient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(klient_DownloadProgressChanged);
                            klient.DownloadFileCompleted += new AsyncCompletedEventHandler(klient_DownloadFileCompleted);
                            this.UseWaitCursor = true;
                            klient.DownloadFileAsync(new Uri("http://sharpie.cba.pl/files/Updater/Updater.exe"), path);
                        }
                        catch (Exception ex)
                        {
                            DialogResult res = MessageBox.Show("Nie udało się pobrać '" + drzewo.Nodes[0].Text + "'.\n" + ex.Message, "Błąd", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                            if (res == DialogResult.Retry)
                            {
                                retry = true;
                            }
                        }
                    } while (retry);

                }

                if (drzewo.Nodes[2].Checked)
                {
                    do
                    {
                        string path = Path.Combine(foldertb.Text, "MapEditor.exe");
                        if (File.Exists(path))
                        {
                            DialogResult result = MessageBox.Show("Plik '" + Path.GetFileName(path) + "' już istnieje. Czy chcesz go zastąpić?", "Ostrzeżenie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.No)
                            {
                                break;
                            }
                        }

                        retry = false;
                        try
                        {
                            klient = new WebClient();
                            klient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(klient_DownloadProgressChanged);
                            klient.DownloadFileCompleted += new AsyncCompletedEventHandler(klient_DownloadFileCompleted);
                            this.UseWaitCursor = true;
                            klient.DownloadFileAsync(new Uri("http://sharpie.cba.pl/files/MapEditor/MapEditor.exe"), path);
                        }
                        catch (Exception ex)
                        {
                            DialogResult res = MessageBox.Show("Nie udało się pobrać '" + drzewo.Nodes[0].Text + "'.\n" + ex.Message, "Błąd", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                            if (res == DialogResult.Retry)
                            {
                                retry = true;
                            }
                        }
                    } while (retry);

                }

                MessageBox.Show("Wszystkie pliki zostały pobrane!", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            pobierzbt.Enabled = true;
            drzewo.Enabled = true;
            foldertb.Enabled = true;
            folderbt.Enabled = true;
        }

        void klient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.UseWaitCursor = false;
        }

        void klient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
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
