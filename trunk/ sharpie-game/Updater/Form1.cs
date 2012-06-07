﻿using System;
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

		private void UpdateProcess()
		{
			Upgrade updte = new Upgrade();
			if (updte.CheckUpdate() == true)
			{
				label1.Text = "Dostępna jest nowa wersja";
				progressBar1.Value = 100;
				pictureBox1.Image = Properties.Resources.Warning;
				FormSizeOut();
			}
			else
			{
				label1.Text = "Masz najnowszą wersję gry!";
				progressBar1.Value = 100;
				pictureBox1.Image = Properties.Resources.OK;
			}
		}

		private void Form1_Shown(object sender, EventArgs e)
		{
			UpdateProcess();
		}

		private void FormSizeOut()
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

		private void FormSizeIn()
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

		private void button1_Click(object sender, EventArgs e)
		{
			button1.Enabled = false;
			progressBar1.Style = ProgressBarStyle.Marquee;
			pictureBox1.Image = Properties.Resources.Download;
			Upgrade updte = new Upgrade();
			if (updte.DoUpgrade() == true)
			{
				label1.Text = "Gra została zaktualizowana!";
				progressBar1.Style = ProgressBarStyle.Continuous;
				progressBar1.Value = 100;
				pictureBox1.Image = Properties.Resources.OK;
				FormSizeIn();
			}
			else
			{
				label1.Text = "Gra nie została zaktualizowana!";
				progressBar1.Style = ProgressBarStyle.Continuous;
				progressBar1.Value = 100;
				pictureBox1.Image = Properties.Resources.Error;
				FormSizeIn();
			}
		}
	}
}