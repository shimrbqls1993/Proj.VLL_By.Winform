using Proj.VVL.Services.Kiwoom.Managers;

namespace Proj.VVL
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary> fd
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}