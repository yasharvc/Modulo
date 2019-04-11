using ModuloContracts.Module.Interfaces;
using ModuloContracts.Module.Meta;
using ModuloContracts.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ModuloContracts.Module
{
	public abstract class Manifesto : IManifest
	{
		public abstract string ModuleName { get; }

		public abstract string Title { get; }

		public abstract string Description { get; }

		public Version Version { get => GetType().Assembly.GetName().Version; }

		public virtual IEnumerable<ITest> Tests => new List<ITest>();

		public virtual IEnumerable<Dependency> Dependencies => new List<Dependency>();

		public virtual IEnumerable<IMenu> Menus => new List<IMenu>();

		public virtual bool IsSystemModule => false;

		public virtual Dictionary<string, BaseViewComponent> HomePageViewComponents => new Dictionary<string, BaseViewComponent>();
		
		public IEnumerable<Type> AreaControllers
		{
			get
			{
				Type t = GetType();
				Assembly assembly = t.Assembly;
				var types = assembly.GetTypes();
				var res = new List<Type>();
				foreach (var type in types)
					if (type.BaseType == typeof(AreaController))
						res.Add(type);
				return res;
			}
		}

		public ServiceMeta ServiceMeta { get; set; }

		public virtual void OnConfigure() { }
	}
}
