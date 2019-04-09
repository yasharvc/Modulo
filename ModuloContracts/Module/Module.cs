using ModuloContracts.Enums;
using ModuloContracts.Module.Interfaces;
using System;
using System.Reflection;

namespace ModuloContracts.Module
{
	public class Module
	{
		public string PathToDll { get; private set; }
		public IManifest Manifest { get; private set; }
		public ModuleStatus Status { get; private set; } = ModuleStatus.Disable;
		Assembly Assembly { get; set; }
		public Module(Assembly assembly)
		{
			Assembly = assembly;
		}
		public Module(string pathToDll,IManifest manifest,ModuleStatus status)
		{
			PathToDll = pathToDll;
			Manifest = manifest;
			Status = status;
		}
		public virtual void OnConfigure() { Manifest.OnConfigure(); }
		public static implicit operator ModuleInformation(Module mdl)
		{
			var m = mdl.Manifest;
			return new ModuleInformation { Description = m.Description, IsSystemModule = m.IsSystemModule, ModuleName = m.ModuleName, Status = mdl.Status, Title = m.Title };
		}

		public void SetStatus(ModuleStatus Status) => this.Status = Status;

		public void SetPathToDll(string path) => PathToDll = path;

		public void SetManifest(IManifest manifest)
		{
			Manifest = manifest;
		}
	}
}
