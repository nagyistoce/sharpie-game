﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace Sharpie
{
	class Program
	{
		public static string name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        public static string version = FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion;
		static void Main(string[] args)
		{
			Console.SetWindowSize(70, 41);
			Console.Title = name;
			Console.CursorVisible = false;

			do
			{
				Interface.Draw();
				Interface.Glowny();
			} while (true);
		}
	}
}
