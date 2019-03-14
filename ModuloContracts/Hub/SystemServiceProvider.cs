using ModuloContracts.Hub.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloContracts.Hub
{
	public class SystemServiceProvider
	{
		public static ISystemServiceProvider SystemServicesProvider
		{
			get
			{
				return OnSystemServiceProvider();
			}
		}

		public static event SystemServiceProviderEventArgs OnSystemServiceProvider;
		public static bool IsServiceProviderPresent => OnSystemServiceProvider != null;
	}
}
