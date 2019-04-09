using System;
using System.IO.Compression;
using System.IO;

namespace ModuloManager
{
	public class ModuleZipHandler : IDisposable
	{
		private readonly string ModulesRootFolderName = "Modules";
		private readonly string ModulesTempFolderName = "Temp";

		public string TempFolderName { get; private set; }
		public string GetFullTempFolderPath()
		{
			return $"{ModulesRootFolderName}\\{ModulesTempFolderName}\\{TempFolderName}";
		}
		public ModuleZipHandler(byte[] zipFile)
		{
			TempFolderName = Guid.NewGuid().ToString();
			Directory.CreateDirectory(GetFullTempFolderPath());
			using (ZipArchive archive = new ZipArchive(new MemoryStream(zipFile)))
			{
				foreach (ZipArchiveEntry entry in archive.Entries)
				{
					var destinationPath = entry.FullName;
					var path = Path.Combine(GetFullTempFolderPath(), destinationPath);
					if (entry.FullName.EndsWith("/"))
						Directory.CreateDirectory(path);
					else
						File.WriteAllBytes(path, ZipEntryToBytes(entry.Open()));
				}
			}
		}

		private byte[] ZipEntryToBytes(Stream stream)
		{
			byte[] bytes;
			using (var ms = new MemoryStream())
			{
				stream.CopyTo(ms);
				bytes = ms.ToArray();
			}
			return bytes;
		}

		internal void CutToFolder(string destFolder)
		{
			CopyToFolder(destFolder);
			Directory.Delete(GetFullTempFolderPath(), true);
		}

		internal void Clean()
		{
			try
			{
				Directory.Delete(GetFullTempFolderPath(), true);
			}
			catch { }
		}

		public void Dispose()
		{
			//Delete all objects created if exists
			Clean();
		}

		internal void CopyToFolder(string destFolder)
		{
			CopyFilesRecursively(new DirectoryInfo(GetFullTempFolderPath()), new DirectoryInfo(destFolder));
		}

		private void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
		{
			foreach (DirectoryInfo dir in source.GetDirectories())
				CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
			foreach (FileInfo file in source.GetFiles())
			{
				if (file.Name.EndsWith(".cshtml"))
				{
					var process = new CSHtmlFileInspector().ModelClassPathToDynamic(File.ReadAllText(file.FullName));
					File.WriteAllText(Path.Combine(target.FullName, file.Name), process);
				}
				else
				{
					file.CopyTo(Path.Combine(target.FullName, file.Name), true);
				}
			}
		}
	}
}
