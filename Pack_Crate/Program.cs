using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;


namespace Pack_Crate
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Para.pstrDBIP = ConfigurationManager.AppSettings["QMS_Server"];   //AppSettings["QMS_Server"];
            Para.pstrDBName = ConfigurationManager.AppSettings["QMS_DBA"];
            Para.pstrDBUID = ConfigurationManager.AppSettings["QMS_UIDA"];
            Para.pstrDBPWD = ConfigurationManager.AppSettings["QMS_PWDA"];
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 formPrint = new Form1();
            formPrint.ShowDialog();
        }

    }
}
