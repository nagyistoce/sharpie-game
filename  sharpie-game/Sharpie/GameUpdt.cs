using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Utils;
using Menus;

namespace Sharpie
{
    static class GameUpdt
    {
        public static bool WasUpdateChecked = false;
        public static void UpdateProcedure()
        {
            Update updt = new Update(0, System.IO.Path.GetFileName(System.Windows.Forms.Application.ExecutablePath));
            bool IsUpdate = updt.CheckUpdate(Program.version);
            if (IsUpdate)
            {
                string dialogtext = "Dostępna jest nowa wersja gry. Zaktualizować?";
                Dialog updtdialog = new Dialog(1, ConsoleColor.White, ConsoleColor.DarkGreen);
                Menu updtmenu = new Menu(new string[] { "Tak", "Nie" }, Crs.CenterX() - 6, Crs.CenterY() + 2, ConsoleColor.White, ConsoleColor.DarkGreen);
                updtdialog.Show(Crs.CenterX() - 17, Crs.CenterY() - 3, Crs.CenterX() + 17, Crs.CenterY() + 4, "Aktualizacja", "Enter - wybierz");
                updtdialog.WriteOn(dialogtext, Crs.CenterY() - 1);
                int value = updtmenu.ShowVertical(4, true, false, 0);
                switch (value)
                {
                    case 0:
                        updtdialog.Clear();
                        string error;
                        string download = "Pobieram aktualizację ...";
                        Dialog downupdt = new Dialog(1, ConsoleColor.White, ConsoleColor.Red);
                        downupdt.Show(Crs.CenterX() - download.Length / 2 - 2, Crs.CenterY() - 2, Crs.CenterX() + download.Length / 2 + 2, Crs.CenterY() + 2);
                        downupdt.WriteOn(download, Crs.CenterY());
                        bool downstatus = updt.Download(out error);
                        if (downstatus)
                        {
                            updt.Install();
                        }
                        else
                        {
                            downupdt.Show(Crs.CenterX() - 20 - 2, Crs.CenterY() - 4, Crs.CenterX() + 20 + 2, Crs.CenterY() + 4);
                            downupdt.WriteOn(error, Crs.CenterY() - 2);
                            Menu errormenu = new Menu(new string[] { "OK" }, Crs.CenterX() - 2, Crs.CenterY() + 2, ConsoleColor.White, ConsoleColor.Red);
                            errormenu.ShowVertical(0, false, false, 0);
                            downupdt.Clear();
                        }
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
