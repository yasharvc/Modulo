using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloContracts.Module.Service
{
	public class ServiceFunction : Attribute
	{
		public List<Type> ParametersTypes { get; private set; } = new List<Type>();
		public string ReturnType { get; private set; } = "";

		public ServiceFunction(params Type[] ParametersTypes)
		{
			foreach (var type in ParametersTypes)
			{
				this.ParametersTypes.Add(type);
			}
		}
		public ServiceFunction(string ReturnType,params Type[] ParametersTypes)
		{
			foreach (var type in ParametersTypes)
			{
				this.ParametersTypes.Add(type);
			}
			this.ReturnType = ReturnType;
		}
	}
	public class ServiceModel : Attribute
	{

	}
}