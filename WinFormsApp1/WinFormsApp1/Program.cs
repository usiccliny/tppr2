using Newtonsoft.Json;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace WinFormsApp1
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SQLlite sQLlite = new SQLlite();

            //string exeDirectory = Application.StartupPath;
            //string jsonPath = Path.Combine(exeDirectory, "data.json");

            sQLlite.CreateTable();
            //sQLlite.InsertData(jsonPath);
            //sQLlite.TransferData();


            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}