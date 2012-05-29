using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Updater;

namespace Updater
{
    public partial class Form1 : Form
    {
        public FileVersionInfo ver;
        public Thread updt;

        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            updt = new Thread(UpdateProcess);
            updt.SetApartmentState(ApartmentState.STA);
            updt.Start();
        }

        private void UpdateProcess()
        {
            Upgrade updte = new Upgrade();
            try
            {
                if (updte.CheckUpdate() == true)
                {
                    if (updte.DoUpgrade() == true)
                    {
                        MessageBox.Show("Gra została pomyślnie zaktualizowana!", "Aktualizator", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Nie udało się zaktualizować gry.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Posiadasz najnowszą wersję gry, nie ma potrzeby aktualizacji.", "Aktualizator", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
               
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Application.Exit();
            }
            Application.Exit();
        }
    }
}
