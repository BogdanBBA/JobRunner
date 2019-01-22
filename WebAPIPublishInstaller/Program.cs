using CommonCode.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace WebAPIPublishInstaller
{
    class Program
    {
        private const bool SIMULATE_IIS_RESET = true;

        private static void DoIISReset()
        {
            if (SIMULATE_IIS_RESET)
                return;
            Process iisReset = new Process();
            iisReset.StartInfo.FileName = "iisreset.exe";
            //iisReset.StartInfo.RedirectStandardOutput = true;
            //iisReset.StartInfo.UseShellExecute = false;
            iisReset.Start();
            iisReset.WaitForExit();
        }

        private static void DeleteFolderAndContents(DirectoryInfo directory)
        {
            Console.ForegroundColor = ConsoleColor.White;
            foreach (FileInfo file in directory.EnumerateFiles())
            {
                Console.WriteLine($" - deleting file '{Path.GetFileName(file.FullName)}'...");
                file.Delete();
            }

            foreach (DirectoryInfo folder in directory.EnumerateDirectories())
            {
                DeleteFolderAndContents(folder);
                folder.Delete();
            }
        }

        private static void CopyFiles(DirectoryInfo source, DirectoryInfo destination)
        {
            Console.ForegroundColor = ConsoleColor.White;
            List<FileInfo> files = source.EnumerateFiles().ToList();
            for (int index = 0; index < files.Count; index++)
            {
                string filename = Path.GetFileName(files[index].FullName);
                Console.WriteLine($" - copying file {index + 1}/{files.Count} '{filename}'...");
                File.Copy(files[index].FullName, Path.Combine(destination.FullName, filename));
            }
        }

        static void Main(string[] args)
        {
            try
            {
                if (!Utils.IsAdministrator())
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("This application must be run as administrator!");
                    Console.WriteLine("Exiting unsuccessfully...");
                    Thread.Sleep(5000);
                    return;
                }

                string sourcePath = Path.GetFullPath(Environment.CurrentDirectory + @"\..\..\..\WebAPI\bin\Release\netcoreapp2.2\publish\");
                string destinationPath = @"C:\inetpub\wwwroot\";

                if (!Directory.Exists(sourcePath))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"The source directory '{sourcePath}' does not exist! Exiting unsuccessfully...");
                    Thread.Sleep(10000);
                    return;
                }

                if (!Directory.Exists(destinationPath))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"The destination directory '{destinationPath}' does not exist! Exiting unsuccessfully...");
                    Thread.Sleep(10000);
                    return;
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Resetting IIS...");
                DoIISReset();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{Environment.NewLine}Deleting existing data...");
                DeleteFolderAndContents(new DirectoryInfo(destinationPath));

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{Environment.NewLine}Copying new data...");
                CopyFiles(new DirectoryInfo(sourcePath), new DirectoryInfo(destinationPath));

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{Environment.NewLine}Done!");
                Thread.Sleep(5000);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{Environment.NewLine}An ERROR occured:{Environment.NewLine}{Environment.NewLine}{e.ToString()}");
                Thread.Sleep(10000);
            }
        }
    }
}
