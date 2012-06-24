using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;

namespace Sharpie
{
    static class GameUpdt
    {
        public static bool WasUpdateChecked = false;
        public static void UpdateProcedure()
        {
            Update.Update updt = new Update.Update(0, Application.ExecutablePath);
            bool IsUpdate = updt.CheckUpdate();
            if (IsUpdate)
            {
                string dialogtext = "Dostępna jest nowa wersja gry. Zaktualizować?";
                Dialog updtdialog = new Dialog(1, ConsoleColor.White, ConsoleColor.Green);
                Menu updtmenu = new Menu(new string[] { "Tak", "Nie" }, Cursor.CenterX() - 2, Cursor.CenterY() + 1, ConsoleColor.White, ConsoleColor.Green);
                updtdialog.Show(Cursor.CenterX() - dialogtext.Length / 2 - 2, Cursor.CenterY() - 3, Cursor.CenterX() + dialogtext.Length / 2 + 2, Cursor.CenterY() + 3, "Aktualizacja", "Enter - wybierz        ");
                updtdialog.WriteOn(dialogtext, Cursor.CenterY() - 1);
                int value = updtmenu.ShowVertical(3, true, false, 0);
                switch (value)
                {
                    case 0:
                        updtdialog.Clear();
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
    }
}
