using System;

namespace FolderDialog.Console
{    
    class Program
    {        
        [STAThread]
        static void Main(string[] args)
        {
            System.Console.WriteLine(": : : : : Folder Dialog App : : : : :");
            Bll.FolderDialog.ISelect select = new Bll.FolderDialog.Select();
            select.InitialFolder = "C:\\";
            select.ShowDialog();
            System.Console.WriteLine($"Folder Selected: {select.Folder}");
            System.Console.WriteLine("Press any key to continue...");
            System.Console.ReadLine();
        }
    }
}
