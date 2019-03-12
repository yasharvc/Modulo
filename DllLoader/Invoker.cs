using ModuloContracts.Exceptions.SystemExceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Utility;

namespace DllLoader
{
	public class Invoker
	{
		Loader Loader { get; set; }
		public Invoker(Loader loader)
		{
			Loader = loader;
		}

		public T CreateInstance<T>(string ClassFullName) where T : class => Loader.GetMainAssembly().CreateInstance(ClassFullName, true) as T;

		public T InvokeMethod<T>(object obj, string MethodName, IEnumerable<Type> CustomAttributes, params object[] Parameters) where T:class
		{
			try
			{
				var method = GetMethod(obj, MethodName,CustomAttributes,Parameters.Length);
				var convertedParameters = ConvertParameteres(method, Parameters);
				return method.Invoke(obj, convertedParameters) as T;
			}
			catch (MethodNotFoundException e)
			{
				throw e;
			}
			catch(Exception e)
			{
				throw e;
			}
		}

		private object[] ConvertParameteres(MethodInfo method, object[] parameters)
		{
			var methodParameteres = method.GetParameters();
			Dictionary<string,object> res = new Dictionary<string, object>();
			int i = 0;
			foreach(var methodParameter in methodParameteres)
			{
				if (methodParameter.ParameterType.IsPrimitiveType())
				{
					res[methodParameter.Name] = Convert.ChangeType(parameters[i++], methodParameter.ParameterType);
				}
				else
				{
					res[methodParameter.Name] = JsonConvert.DeserializeObject(parameters[i++] as string, methodParameter.ParameterType);
				}
			}
			return res.Select(m => m.Value).ToArray();
		}

		private MethodInfo GetMethod(object obj, string methodName, IEnumerable<Type> customAttributes, int parametersCount)
		{
			var methods = obj.GetType().GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
			foreach (var method in methods)
				if (IsMethodEligable(methodName, customAttributes, parametersCount, method))
					return method;
			throw new MethodNotFoundException(obj.GetType().FullName, methodName);
		}

		private bool IsMethodEligable(string MethodName, IEnumerable<Type> CustomAttributes, int ParametersCount, MethodInfo method)
		{
			return method.GetParameters().Length == ParametersCount &&
								method.Name.Equals(MethodName, StringComparison.OrdinalIgnoreCase)
								&& HasAllAttributes(CustomAttributes, method);
		}

		private bool HasAllAttributes(IEnumerable<Type> CustomAttributes, MethodInfo method)
		{
			foreach (var attr in CustomAttributes)
				if (method.GetCustomAttribute(attr) == null)
					return false;
			return true;
		}
	}
}
