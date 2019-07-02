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
		private const bool SIMULATE_ALL = true;
		private const bool SIMULATE_IIS_RESET = false;
		private const bool SIMULATE_FILE_DELETE_COPY = false;
		private const bool SIMULATE_OPEN_JOB_RUNNER_UI = false;

		private static void DoIISReset()
		{
			if (SIMULATE_IIS_RESET || SIMULATE_ALL)
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
			if (SIMULATE_FILE_DELETE_COPY || SIMULATE_ALL)
				return;

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
			if (SIMULATE_FILE_DELETE_COPY || SIMULATE_ALL)
				return;

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

				if (SIMULATE_IIS_RESET || SIMULATE_FILE_DELETE_COPY || SIMULATE_OPEN_JOB_RUNNER_UI || SIMULATE_ALL)
				{
					Console.ForegroundColor = ConsoleColor.DarkYellow;
					Console.WriteLine($"Warning: IIS reset, file delete/copy or UI app open is only simulated!{Environment.NewLine}Have you forgotten to change some debug values?");
					Thread.Sleep(3000);
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

				string jobRunnerUIFolder = Path.GetFullPath(Environment.CurrentDirectory + @"\..\..\..\JobPoolUI\bin\Debug\");
				string jobRunnerUIExe = Path.Combine(jobRunnerUIFolder, @"JobPoolUI.exe");

				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine($"{Environment.NewLine}Starting the job runner...");
				if (!(SIMULATE_OPEN_JOB_RUNNER_UI || SIMULATE_ALL))
					Process.Start(new ProcessStartInfo(jobRunnerUIExe) { WorkingDirectory = jobRunnerUIFolder });

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
