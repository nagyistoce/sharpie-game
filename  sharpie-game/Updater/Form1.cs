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

		public void ChangePicture(Image img)
		{
			pictureBox1.Image = img;
		}
        bool sharpieup, editorup;
        Upgrade updte = new Upgrade();
		private void UpdateProcess()
		{
            updte.CheckUpdate(out sharpieup, out editorup);
            if (editorup && sharpieup)
            {
                label1.Text = "Dostępna jest nowa wersja gry oraz edytora map";
                progressBar1.Value = 100;
                pictureBox1.Image = Properties.Resources.Warning;
                FormSizeDown();
                FormSizeRight();
            }
			else if (sharpieup)
			{
				label1.Text = "Dostępna jest nowa wersja gry";
				progressBar1.Value = 100;
				pictureBox1.Image = Properties.Resources.Warning;
				FormSizeDown();
			}
            else if (editorup)
            {
                label1.Text = "Dostępna jest nowa wersja edytora map";
                progressBar1.Value = 100;
                pictureBox1.Image = Properties.Resources.Warning;
                FormSizeDown();
            }
            else
            {
                label1.Text = "Aktualne!";
                progressBar1.Value = 100;
                pictureBox1.Image = Properties.Resources.OK;
            }
		}

		private void Form1_Shown(object sender, EventArgs e)
		{
			UpdateProcess();
		}

		private void FormSizeDown()
		{
			int speed = 30;
			this.button1.Visible = true;
			for (int i = 100; i <= 123; i++)
			{
				if (i <= 105)
				{
					speed = speed - 4;
				}
				else if (i >= 118)
				{
					speed = speed + 3;
				}

				this.Height = i;
				Thread.Sleep(speed);
				this.Update();
			}
		}

		private void FormSizeUp()
		{
			int speed = 30;
			this.button1.Visible = false;
			for (int i = 123; i >= 100; i--)
			{
				if (i <= 105)
				{
					speed = speed + 4;
				}
				else if (i >= 118)
				{
					speed = speed - 3;
				}

				this.Height = i;
				Thread.Sleep(speed);
				this.Update();
			}
		}

        private void FormSizeRight()
        {
            int speed = 25;
            for (int i = 380; i <= 430; i++)
            {
                if (i <= 385)
                {
                    speed = speed - 4;
                }
                else if (i >= 425)
                {
                    speed = speed + 3;
                }

                this.Width = i;
                Thread.Sleep(speed);
                this.Update();
            }
        }

        private void FormSizeLeft()
        {
            int speed = 20;
            for (int i = 430; i >= 380; i--)
            {
                if (i <= 385)
                {
                    speed = speed + 4;
                }
                else if (i >= 425)
                {
                    speed = speed - 3;
                }

                this.Width = i;
                Thread.Sleep(speed);
                this.Update();
            }
        }

		private void button1_Click(object sender, EventArgs e)
		{
			button1.Enabled = false;
			progressBar1.Style = ProgressBarStyle.Marquee;
			pictureBox1.Image = Properties.Resources.Download;
			if (updte.DoUpgrade(sharpieup, editorup) == true)
			{
				label1.Text = "Zaktualizowano!";
				pictureBox1.Image = Properties.Resources.OK;
			}
			else
			{
				label1.Text = "Aktualizacja nie powiodła się!";
				pictureBox1.Image = Properties.Resources.Error;
			}

            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.Value = 100;
            FormSizeUp();
            if (this.Width == 430)
            {
                FormSizeLeft();
            }
		}
	}
}
