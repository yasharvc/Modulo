using System.IO;
using System.Linq;

namespace ModuloManager
{
	public class ModuleFolderInspector
	{
		public string ReleaseNote { get; private set; }
		private string folder = "";
		public ModuleFolderInspector(string folder)
		{
			this.folder = folder;
			ReleaseNote = getReleaseNotes();
		}
		private string getReleaseNotes()
		{
			try
			{
				return File.ReadAllText(Path.Combine(folder, "ReleaseNotes.txt"));
			}
			catch
			{
				return "";
			}
		}

		internal byte[] GetDll()
		{
			try
			{
				return File.ReadAllBytes(GetDllPath());
			}
			catch
			{
				throw new FileNotFoundException("File not found", folder);
			}
		}

		public string GetDllPath()
		{
			return Directory.GetFiles(folder, "*.dll").Single();
		}
	}
}
