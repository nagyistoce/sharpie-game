﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sharpie
{
	class Program
	{
		public static string name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
		public static string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Remove(5);
		static void Main(string[] args)
		{
			Console.SetWindowSize(70, 40);
			Console.Title = name;
			Console.CursorVisible = false;
			Interface window = new Interface();
			do
			{
				window.Draw();
				window.Glowny();
			} while (true);
		}
	}
}
