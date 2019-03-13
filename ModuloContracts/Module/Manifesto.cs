﻿using System;
using System.Collections.Generic;
using System.Text;
using ModuloContracts.Enums;

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

		public virtual void OnConfigure() { }
	}
}