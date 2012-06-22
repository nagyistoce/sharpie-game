using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Updater
{
	static class Program
	{
		/// <summary>
		/// Command line arguments: /gameonly (aktualizuje tylko grę), /editoronly (aktualizuje tylko edytor), /startgame (uruchamia grę po aktualizacji).
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}
	}
}
