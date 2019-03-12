using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ModuloResolver
{
	public class ManifestResolver
	{
		Assembly Assembly { get; set; }
		public ManifestResolver(string pathToDll)
		{
			AppDomain domain = AppDomain.CurrentDomain;
			domain.Load()
		}
	}
}
