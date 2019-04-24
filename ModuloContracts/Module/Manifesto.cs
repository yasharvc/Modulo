using ModuloContracts.Module.Interfaces;
using ModuloContracts.Module.Meta;
using ModuloContracts.MVC;
using System;
using System.Collections.Generic;
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

		public virtual Dictionary<string, BaseViewComponent> ViewComponents => new Dictionary<string, BaseViewComponent>();

		public virtual BaseViewComponent GetCustomViewComponent(string name) => null;

		public virtual void OnConfigure() { }
	}

	public class DummyManifest : Manifesto
	{
		string name = "DummyName";
		string title = "Dummy Title";
		string desc = "Dummy Description";

		public override string ModuleName => name;

		public override string Title => title;

		public override string Description => desc;

		public DummyManifest(string name) : this(name, "", "") { }
		public DummyManifest(string name, string title) : this(name, title, "") { }
		public DummyManifest(string name, string title, string description)
		{
			this.name = name;
			this.title = title;
			this.desc = description;
		}
	}
}
